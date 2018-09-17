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
    public float monsters_limit;
    //Jokercontrolde Reset jokerinin altında da değişiklik yap.

     void Awake()
    {
        monsters_limit = Random.Range(2f, 7f);


        for(int i=0; i < spawnPoints.Length; i++)
        {

           if(i == 0)
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

}
