using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TankController : MonoBehaviour
{
    public GameObject tank;
    List<Tank> tanks = new List<Tank>();

    private void Start()
    {
        foreach (var player in UIController.players)
        {
            var createdTank = Instantiate(tank, Vector3.zero, Quaternion.identity);
            tanks.Add(new Tank(createdTank, player));
            createdTank.name = player;
        }
    }
    
    void Update()
    {
        GetPlayerInfo();
    }

    void GetPlayerInfo()
    {
        PlayerInfo playerInfo = null;
        UIController.socket.On("tankPosition", response =>
        {
            playerInfo = JsonConvert.DeserializeObject<PlayerInfo>(response.GetValue().ToString());
            
            var positionX = playerInfo.pos_x;
            var positionY = playerInfo.pos_y;
            var rotationZ = playerInfo.rot_z;
        
            int tankIndex = FindTankIndexById("Player0");
            
            tanks.ElementAt(tankIndex).tank.transform.position = new Vector2(positionX, positionY);
            tanks.ElementAt(tankIndex).tank.transform.rotation = Quaternion.Euler(0,0,rotationZ);
        });
    }

    int FindTankIndexById(string id)
    {
        for (int i = 0; i < tanks.Count; i++)
        {
            if (tanks.ElementAt(i).user_id.Equals(id))
            {
                return i;
            }
        }
        
        return -1;
    }
}
