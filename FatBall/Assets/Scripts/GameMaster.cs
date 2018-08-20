using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{


    public Camera cam;
    public static GameMaster gm;

    private int spawnDelay = 3;

    public GameObject timer;
    public GameObject countDown;
    public GameObject player;
    public GameObject gameOverUI;

    private SoundManagerScript soundManager;
    private MonstersSpawnerControl spawnerControl;
    private SpikeSpawnerControl spikeSpawner;
    private JokerSpawnerControl jokerSpawnerControl;
    public int[] jokerWeights = { 50, 50, 20, 20, 2 }; //Jokerlerin çıkma ağırlıkları, Reset'in ağırlığını player controllerdan değiştir.
    //Public olduğu için Unity'de de ağırlıklarını değiştir. Sırayla Rabbit,Turtle,Shield,Half,Reset.
    private float jokerTimeLeft;
    public float jokerSpawnTime = 5f;
    public float monsterSpawnTime = 5f;

    public bool isBubbleCatched = false;

    public float extendTime;

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManagerScript>();
        spawnerControl = FindObjectOfType<MonstersSpawnerControl>();
        jokerSpawnerControl = FindObjectOfType<JokerSpawnerControl>();
        spikeSpawner = FindObjectOfType<SpikeSpawnerControl>();
        Application.targetFrameRate = 60;
        jokerTimeLeft = jokerSpawnTime + spawnDelay;
        cam.orthographicSize = Screen.height / 2;
        countDown.gameObject.SetActive(true);
        extendTime = Random.Range(6f, 10f);
    }


    void Start()
    {
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


    public IEnumerator IncreaseMonsterLimit() //Monster sayısının artış hızı
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            //spawnerControl.monsters_limit = spawnerControl.monsters_limit * 2;
            //spawnerControl.monsters_limit++;
            spawnerControl.monsters_limit = spawnerControl.monsters_limit + Mathf.Log(spawnerControl.monsters_limit);
            SpawnAMonster();
        }
    }

    public IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(spawnDelay);
        CancelInvoke("PlayStartSound");
        Vector3 randomPoint = new Vector3(Random.Range(Screen.width / 10, Screen.width - (Screen.width / 10)), Random.Range(Screen.height / 5, Screen.height - (Screen.height / 5)), 1);
        player = Instantiate(player, randomPoint, Quaternion.identity);
        player.name = "Player";
        timer.SetActive(true);
        SpawnAMonster();
        StartCoroutine(IncreaseMonsterLimit());
        StartCoroutine(ExtendSpike());
        StartCoroutine(ReduceSpikeExtendTime());
    }

    public void KillPlayer(GameObject player)
    {
        Destroy(player);
        soundManager.PlaySound("Explosion");
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
        if (jokerSpawnerControl.num_of_jokers < jokerSpawnerControl.jokerLimit)
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

    GameObject GetRandomSpike()
    {

        GameObject randomSpike = spikeSpawner.spikes[Random.Range(0, spikeSpawner.spikes.Length)];
        SpikeControl spikeControl = randomSpike.GetComponent<SpikeControl>();

        if (spikeControl.isMovingToView)
        {
            return randomSpike;
        }

        else
        {
            return GetRandomSpike();
        }

    }


    Vector3 CalcEndPos(string tempName, Vector3 startPos)
    {
        Vector3 endPos = startPos;


        if (tempName == "TopLeftCornerSpike")
        {
            endPos.x = Screen.height / 10.32f;
            endPos.y = Screen.height - Screen.height / 10.32f;
        }

        if (tempName == "TopRightCornerSpike")
        {
            endPos.x = Screen.width - Screen.height / 10.32f;
            endPos.y = Screen.height - Screen.height / 10.32f;
        }

        if (tempName == "BottomLeftCornerSpike")
        {
            endPos.x = Screen.height / 10.32f;
            endPos.y = Screen.height / 10.32f;
        }

        if (tempName == "BottomRightCornerSpike")
        {
            endPos.x = Screen.width - Screen.height / 10.32f;
            endPos.y = Screen.height / 10.32f;
        }

        if (tempName == "TopSpike")
        {

            endPos.y = Screen.height - Screen.height / 7.5f;
        }

        if (tempName == "BottomSpike")
        {
            endPos.y = Screen.height / 7.5f;
        }

        if (tempName == "RightSpike")
        {
            endPos.x = Screen.width - Screen.height / 7.5f;
        }

        if (tempName == "LeftSpike")
        {

            endPos.x = Screen.height / 7.5f;
        }

        return endPos;
    }

    IEnumerator NormalizeSpike(GameObject spike, SpikeControl spikeControl)
    {
        yield return new WaitUntil(() => spike.transform.position == spikeControl.startPos);
        spikeControl.endPos = spikeControl.tempEndPos;
    }


    IEnumerator ExtendSpike()
    {
        while (true)
        {
            yield return new WaitForSeconds(extendTime);
            GameObject randomSpike = GetRandomSpike();
            SpikeControl spikeControl = randomSpike.GetComponent<SpikeControl>();
            spikeControl.endPos = CalcEndPos(randomSpike.name, spikeControl.startPos);
            StartCoroutine(NormalizeSpike(randomSpike, spikeControl));

            //yield return new WaitUntil(() => randomSpike.transform.position == spikeControl.startPos);
            //spikeControl.endPos = spikeControl.tempEndPos;
        }
    }

    IEnumerator ReduceSpikeExtendTime()
    {
        float reducedBy = Random.Range(0.3f, 1f);

        while (true)
        {
            yield return new WaitForSeconds(10f);
            if(extendTime >= 1f)
            {
                extendTime -= reducedBy;
            }
        }
    }



}
