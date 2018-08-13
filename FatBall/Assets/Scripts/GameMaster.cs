using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {


    public Camera cam;
    public static GameMaster gm;

   private int spawnDelay = 3;
   private int spawnCounter = 3;

    public GameObject timer;
    public GameObject countDown;
    public GameObject player;
    public GameObject gameOverUI;
    private SpikeSpawnerControl spikeSpawnerControl;
    private MonstersSpawnerControl spawnerControl;
    private SoundManagerScript soundManager;
    private JokerSpawnerControl jokerSpawnerControl;
    public int[] jokerWeights = { 10, 10, 2, 2, 1}; //Jokerlerin çıkma ağırlıkları, Reset'in ağırlığını player controllerdan değiştir.
    //Public olduğu için Unity'de de ağırlıklarını değiştir. Sırayla Rabbit,Turtle,Shield,Half,Reset.
    private float jokerTimeLeft;
    private float monsterTimer;
    public float jokerSpawnTime = 5f;
    public float monsterSpawnTime = 5f;

    public bool isBubbleCatched = false;

    void Awake()
    {
        Application.targetFrameRate = 60;
        jokerTimeLeft = jokerSpawnTime + spawnDelay;
        monsterTimer = monsterSpawnTime + spawnDelay;
        cam.orthographicSize = Screen.height / 2;
        spikeSpawnerControl = FindObjectOfType<SpikeSpawnerControl>();
        spawnerControl = FindObjectOfType<MonstersSpawnerControl>();
        jokerSpawnerControl = FindObjectOfType<JokerSpawnerControl>();
        soundManager = FindObjectOfType<SoundManagerScript>();
        countDown.gameObject.SetActive(true);
    }


    void Start () {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
            soundManager.PlaySound("Start");
            gm.StartCoroutine(gm.SpawnPlayer());

        }
       
    }

    void Update()
    {
        jokerTimeLeft -= Time.deltaTime;

        if (jokerTimeLeft <= 0f)
        {
            SpawnAJoker();

            jokerTimeLeft += jokerSpawnTime;
        }
    }

    /*void PlayStartSound()
    {
        soundManager.PlaySound("Start");
    }*/

    public void IncreaseMonsterLimit() //Monster sayısının artış hızı
    {
        Debug.Log("INCREASING MONSTER LIMIT !!");
        //spawnerControl.monsters_limit = spawnerControl.monsters_limit * 2;
        spawnerControl.monsters_limit = spawnerControl.monsters_limit + Mathf.Log(spawnerControl.monsters_limit);
        //spawnerControl.monsters_limit++;
        SpawnAMonster();
    }
    
    public IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(spawnDelay);
        CancelInvoke("PlayStartSound");
        Vector3 randomPoint = new Vector3(Random.Range(Screen.width / 10, Screen.width-(Screen.width/10)), Random.Range(Screen.height / 5, Screen.height - (Screen.height / 5)), 1);
        player = Instantiate(player, randomPoint, Quaternion.identity);
        player.name = "Player";
        timer.SetActive(true);
        SpawnAMonster();
        InvokeRepeating("IncreaseMonsterLimit", 5 , 5);
    }

    public void KillPlayer(GameObject player)
    {
        Destroy(player);
        soundManager.PlaySound("Explosion");
        CancelInvoke("IncreaseMonsterLimit");
        gameOverUI.SetActive(true);
    }

    public void SpawnAMonster()
    {
        while (spawnerControl.num_of_monsters < spawnerControl.monsters_limit)
        {
            spawnerControl.randomSpawnPoint = Random.Range(0, spawnerControl.spawnPoints.Length);
            spawnerControl.randomMonster = Random.Range(0, spawnerControl.monsters.Length);
            spawnerControl.monster = Instantiate(spawnerControl.monsters[spawnerControl.randomMonster], spawnerControl.spawnPoints[spawnerControl.randomSpawnPoint].position,
                Quaternion.identity);
            spawnerControl.monster.name = "Enemy";
            spawnerControl.num_of_monsters++;
        }
    }

    public int GetSumOfWeights(int[] weights)
    {
        var total = 0;
        for (var i = 0; i < weights.Length; i++)
        {
            total += weights[i];
        }
        return total;
    }

    public int GetRandomWeightedIndex(int[] weights)
    {
        if (weights == null || weights.Length == 0) return -1;
        var total = GetSumOfWeights(weights);
        int i;

        float r = Random.value;
        float s = 0f;

        for (i = 0; i < weights.Length; i++)
        {
            if (weights[i] <= 0f) continue;

            s += (float)weights[i] / total;
            if (s >= r) return i;
        }

        return -1;
    }


    public void SpawnAJoker()
    {
            if(jokerSpawnerControl.num_of_jokers < jokerSpawnerControl.jokerLimit)
        {
            jokerSpawnerControl.randomSpawnPoint = Random.Range(0, jokerSpawnerControl.spawnPoints.Length);
            jokerSpawnerControl.randomJoker = GetRandomWeightedIndex(jokerWeights);
            Vector3 randomPoint = jokerSpawnerControl.spawnPoints[jokerSpawnerControl.randomSpawnPoint].position;
            randomPoint.z = 1;
            jokerSpawnerControl.joker = Instantiate(jokerSpawnerControl.jokers[jokerSpawnerControl.randomJoker], randomPoint,
                Quaternion.identity);
            jokerSpawnerControl.joker.name = "Joker";
            jokerSpawnerControl.num_of_jokers++;
        }

    }



}
