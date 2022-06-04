using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SocketIOClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static SocketIOUnity socket;
    public Transform playerTextsHolder;
    public GameObject playerTextPrefab;
    public static List<string> players = new List<string>();

    private string lastResponse = "";
    
    void Start()
    {
        var uri = new Uri("http://localhost:3000");
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
            {
                {"token", "UNITY"}
            }, 
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        socket.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        socket.On("playerConnected", response =>
        {
            if (!response.ToString().Equals(lastResponse))
            {
                players = JsonConvert.DeserializeObject<List<string>>(response.GetValue().ToString());
                
                foreach (var player in players)
                {
                    GameObject playerText = Instantiate(playerTextPrefab, playerTextsHolder);
                    playerText.GetComponent<Text>().text = player;
                }
                lastResponse = response.ToString();
            }
        });
    }

    public void OnClickStartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    private void OnApplicationQuit()
    {
        //socket.Emit("disconnected", );
    }
}
