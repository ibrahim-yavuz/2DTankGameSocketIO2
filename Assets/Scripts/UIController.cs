using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    private List<GameObject> playerTextObjects = new List<GameObject>();

    public Transform chatPanelHolder;
    public GameObject chatTextPrefab;
    public InputField chatMessageInputField;

    private string lastResponse = "";
    
    void Start()
    {
        var uri = new Uri("http://207.154.197.220:3000");
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
        socket.OnUnityThread("playerConnected", response =>
        {
            if (!response.ToString().Equals(lastResponse))
            {
                players = JsonConvert.DeserializeObject<List<string>>(response.GetValue().ToString());
                
                foreach (var obj in playerTextObjects)
                {
                    Destroy(obj);
                }
                
                foreach (var player in players)
                {
                    if (player != null)
                    {
                        GameObject playerText = Instantiate(playerTextPrefab, playerTextsHolder);
                        playerText.GetComponent<Text>().text = player;
                        playerTextObjects.Add(playerText);
                    }
                    
                }
                lastResponse = response.ToString();
            }
        });
        
        socket.OnUnityThread("message", response =>
        {
            
            GameObject chatText = Instantiate(chatTextPrefab, chatPanelHolder);
            chatText.GetComponent<Text>().text = response.GetValue().GetString();
        });
    }

    public void OnClickStartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    private void OnApplicationQuit()
    {
        socket.Emit("disconnected", ".");
    }

    public void OnClickSendMessageButton()
    {
        socket.Emit("message", chatMessageInputField.text);
        chatMessageInputField.text = "";
    }
}
