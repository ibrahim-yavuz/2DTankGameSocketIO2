using UnityEngine;

public class Tank
{
    public GameObject tank { get; set; }
    public string user_id { get; set; }

    public Tank(GameObject tank, string user_id)
    {
        this.tank = tank;
        this.user_id = user_id;
    }
}