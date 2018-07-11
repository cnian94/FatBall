using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonstersSpawnerControl : MonoBehaviour {

	public Transform[] spawnPoints;
	public GameObject[] monsters;
	public int randomSpawnPoint, randomMonster;


    public GameObject monster;
    public int num_of_monsters = 0;
    public int monsters_limit = 50;

    // Use this for initialization
    void Start () {
        InvokeRepeating("SpawnAMonster", 0f, 1f);
    }


    public void SpawnAMonster()
	{
		if (num_of_monsters < monsters_limit) {
			randomSpawnPoint = Random.Range (0, spawnPoints.Length);
			randomMonster = Random.Range (0, monsters.Length);
			monster = Instantiate (monsters [randomMonster], spawnPoints [randomSpawnPoint].position,
				Quaternion.identity);
            monster.name = "Enemy";
            num_of_monsters++;
            //Debug.Log("ACTIVE MONSTERS: " + num_of_monsters);
		}
	}

}
