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

    void Awake()
    {
        spawnPoints[0].transform.position = new Vector3(Screen.width + 50, Random.Range(0f, Screen.height), 0);
        spawnPoints[1].transform.position = new Vector3(-50, Random.Range(0f, Screen.height), 0);

    }

    // Use this for initialization
    void Start()
    {


    }
}
