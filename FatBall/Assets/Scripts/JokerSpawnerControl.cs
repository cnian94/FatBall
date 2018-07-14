using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokerSpawnerControl : MonoBehaviour {

    public Transform[] spawnPoints;
    public GameObject[] jokers;
    public int randomSpawnPoint, randomJoker;


    public GameObject joker;
    public int num_of_jokers = 0;
    public int jokerLimit = 3;

    // Use this for initialization
    void Start()
    {


    }
}
