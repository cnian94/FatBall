using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokerSpawnerControl : MonoBehaviour {

    public Transform[] spawnPoints;
    public GameObject[] jokers;  //normalde public int[] jokers yazmak gerekmez mi ? Jokerweights ile nasıl bağladık ?
    public int randomSpawnPoint, randomJoker;

    public GameObject joker;
    public int num_of_jokers = 0;
    public int jokerLimit = 4;

    void Awake()
    {
        //spawnPoints[0].transform.position = new Vector3(Screen.width + 50, Random.Range(0f, Screen.height), 0);
        //spawnPoints[1].transform.position = new Vector3(-50, Random.Range(0f, Screen.height), 0);

        for (int i = 0; i < spawnPoints.Length; i++)
        {

            if (i == 0)
            {
                spawnPoints[i].transform.position = new Vector3(Random.Range(0f, Screen.width), -50, 1);
            }

            if (i == 1 || i == 2)
            {
                spawnPoints[i].transform.position = new Vector3(Screen.width + 50, Random.Range(0f, Screen.height), 1);
            }

            if (i == 3)
            {
                spawnPoints[i].transform.position = new Vector3(Random.Range(0f, Screen.width), Screen.height + 50, 1);
            }

            if (i == 4 || i == 5)
            {
                spawnPoints[i].transform.position = new Vector3(-50, Random.Range(0f, Screen.height), 1);
            }

        }

    }

    // Use this for initialization
    void Start()
    {


    }
}
