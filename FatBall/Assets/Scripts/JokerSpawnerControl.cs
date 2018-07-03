using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokerSpawnerControl : MonoBehaviour {

    public Transform[] spawnPoints;
    public GameObject[] jokers;
    public int randomSpawnPoint, randomJoker;


    public GameObject joker;
    public int num_of_jokers = 0;
    public int joker_limit = 50;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("SpawnAMonster", 0f, 1f);
    }


    public void SpawnAJoker()
    {
        if (num_of_jokers < joker_limit)
        {
            randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            randomJoker = Random.Range(0, jokers.Length);
            joker = Instantiate(jokers[randomJoker], spawnPoints[randomSpawnPoint].position,
                Quaternion.identity);
            joker.name = "Joker";
            num_of_jokers++;
            //Debug.Log("ACTIVE MONSTERS: " + num_of_monsters);
        }
    }
}
