using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {


    public Camera cam;
    public static GameMaster gm;

    public GameObject timer;
    public GameObject countDown;
    public GameObject player;
    public GameObject gameOverUI;
    private MonstersSpawnerControl spawnerControl;
    private JokerSpawnerControl jokerSpawnerControl;

    void Awake()
    {
        cam.orthographicSize = Screen.height / 2;
        Debug.Log("GM AWAKE !!!");
        spawnerControl = FindObjectOfType<MonstersSpawnerControl>();
        jokerSpawnerControl = FindObjectOfType<JokerSpawnerControl>();
        countDown.gameObject.SetActive(true);
    }


    void Start () {
        if (gm == null)
        {
            Debug.Log("GM START !!!");
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
            gm.StartCoroutine(gm.SpawnPlayer());
        }
    }

    public int spawnDelay = 3;
    public int spawnCounter = 3;
	
    public IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(spawnDelay);
        Debug.Log("SPAWNING PLAYER !!");
        Vector3 randomPoint = new Vector3(Random.Range(0, Screen.width-(Screen.width/20)), Random.Range(0, Screen.height-(Screen.height/12)), 0);
        player = Instantiate(player, randomPoint, Quaternion.identity);
        timer.SetActive(true);
        InvokeRepeating("SpawnAMonster", 0f, 1f);
        InvokeRepeating("SpawnAJoker", 0f, 1f);
        player.name = "Player";
    }

    public void KillPlayer(GameObject player)
    {
        Destroy(player);
        gameOverUI.SetActive(true);
        //gm.StartCoroutine(gm.SpawnPlayer());
    }

    public void SpawnAMonster()
    {
        if (spawnerControl.num_of_monsters < spawnerControl.monsters_limit)
        {
            spawnerControl.randomSpawnPoint = Random.Range(0, spawnerControl.spawnPoints.Length);
            spawnerControl.randomMonster = Random.Range(0, spawnerControl.monsters.Length);
            spawnerControl.monster = Instantiate(spawnerControl.monsters[spawnerControl.randomMonster], spawnerControl.spawnPoints[spawnerControl.randomSpawnPoint].position,
                Quaternion.identity);
            spawnerControl.monster.name = "Enemy";
            spawnerControl.num_of_monsters++;
            //Debug.Log("ACTIVE MONSTERS: " + num_of_monsters);
        }
    }

    public void SpawnAJoker()
    {
        if (jokerSpawnerControl.num_of_jokers < jokerSpawnerControl.joker_limit)
        {
            jokerSpawnerControl.randomSpawnPoint = Random.Range(0, jokerSpawnerControl.spawnPoints.Length);
            jokerSpawnerControl.randomJoker = Random.Range(0, jokerSpawnerControl.jokers.Length);
            jokerSpawnerControl.joker = Instantiate(jokerSpawnerControl.jokers[jokerSpawnerControl.randomJoker], jokerSpawnerControl.spawnPoints[jokerSpawnerControl.randomSpawnPoint].position,
                Quaternion.identity);
            jokerSpawnerControl.joker.name = "Joker";
            jokerSpawnerControl.num_of_jokers++;
        }
    }

}
