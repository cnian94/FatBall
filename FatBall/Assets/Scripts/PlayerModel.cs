using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel{

    public string device_id;
    public string nickname;
    public int highscore;
    public int coins;

    public PlayerModel(string device_id, string nickname)
    {
        this.device_id = device_id;
        this.nickname = nickname;
    }
}
