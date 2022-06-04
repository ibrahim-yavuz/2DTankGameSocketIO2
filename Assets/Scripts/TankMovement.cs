using System;
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using GameDevWare.Serialization;
using Newtonsoft.Json;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float speed = 2f;
    public float rotationSpeed = 1f;
    private string lastSentData = "";

    private void Update()
    {
        ControlTank();
    }

    private void ControlTank()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.up * speed * Time.deltaTime;
            SendPositionData();
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += transform.up * -1 * speed * Time.deltaTime;
            SendPositionData();
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            SendPositionData();
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
            SendPositionData();
        }
    }

    void SendPositionData()
    {
        PlayerInfo playerInfo = new PlayerInfo();
        playerInfo.user_id = "Player0";
        playerInfo.pos_x = transform.position.x;
        playerInfo.pos_y = transform.position.y;
        playerInfo.rot_z = transform.eulerAngles.z;
        
        UIController.socket.Emit("tankPosition", JsonConvert.SerializeObject(playerInfo));
    }
}
