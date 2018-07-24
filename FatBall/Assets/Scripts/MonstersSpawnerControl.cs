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
    public float monsters_limit = 3;
    //Jokercontrolde Reset jokerinin altında da değişiklik yap.

    // Use this for initialization
    void Start () {
    }

    void Update()
    {
        
    }

}
