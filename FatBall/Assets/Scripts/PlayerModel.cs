﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerModel
{

    public string device_id;
    public string nickname;
    public int highscore;
    public int coins;

    public PlayerModel(string device_id, string nickname)
    {
        this.device_id = device_id;
        this.nickname = nickname;
    }

    public PlayerModel(string device_id, string nickname, int highscore, int coins) : this(device_id, nickname)
    {
        this.highscore = highscore;
        this.coins = coins;
    }

    public override string ToString()
    {
        return "device_id:" + this.device_id + System.Environment.NewLine
             + "nickname:" + this.nickname + System.Environment.NewLine
             + "highscore:" + this.highscore + System.Environment.NewLine
             + "coins:" + this.coins + System.Environment.NewLine;
    }
}


[Serializable]
public class LeaderBoardList
{

    public PlayerModel[] players;


    public static LeaderBoardList CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<LeaderBoardList>(jsonString);
    }

}
