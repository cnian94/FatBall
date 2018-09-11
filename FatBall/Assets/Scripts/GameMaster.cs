using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{


    public static GameMaster gm;

    private int spawnDelay = 3;

    public GameObject timer;
    public GameObject countDown;
    public GameObject player;
    public GameObject gameOverUI;
    public GameObject gamePausedUI;
    public GameObject PauseButton;

    public TimerScript timerScript;

    //private SoundManagerScript soundManager;
    private MonstersSpawnerControl spawnerControl;
    private SpikeSpawnerControl spikeSpawner;
    private JokerSpawnerControl jokerSpawnerControl;
    public int[] jokerWeights = { 80, 70, 45, 20, 10, 80, 80 }; //Jokerlerin çıkma ağırlıkları, Reset'in ağırlığını player controllerdan değiştir.
    //Public olduğu için Unity'de de ağırlıklarını değiştir. Sırayla Rabbit,Turtle,Shield,Half,Reset,Cherry,Grape. Half size ağırlğını Player controller 81 den değiştir.
    private float jokerTimeLeft;
    public float jokerSpawnTime = 5f;
    public float monsterSpawnTime = 5f;

    public bool isBubbleCatched = false;

    GameObject randomSpike;
    public float extendTime;

    void Awake()
    {
        timerScript = timer.GetComponent<TimerScript>();
        //soundManager = FindObjectOfType<SoundManagerScript>();
        spawnerControl = FindObjectOfType<MonstersSpawnerControl>();
        jokerSpawnerControl = FindObjectOfType<JokerSpawnerControl>();
        spikeSpawner = FindObjectOfType<SpikeSpawnerControl>();
        Application.targetFrameRate = 60;
        jokerTimeLeft = jokerSpawnTime + spawnDelay;
        countDown.gameObject.SetActive(true);
        extendTime = Random.Range(6f, 10f);
    }


    void Start()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
            SoundManager.Instance.Play("Start");
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
        while (!timerScript.GetIsPaused() && spawnerControl.monsters_limit<25)
        {
            yield return new WaitForSeconds(5);
            //spawnerControl.monsters_limit = spawnerControl.monsters_limit * 2;
            //spawnerControl.monsters_limit++;
            spawnerControl.monsters_limit = spawnerControl.monsters_limit + Mathf.Log(spawnerControl.monsters_limit) / 1.5f;
            SpawnAMonster();
        }
    }

    public IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(spawnDelay);
        CancelInvoke("PlayStartSound");
        Vector3 randomPoint = new Vector3(Random.Range(Screen.width / 6, Screen.width - (Screen.width / 6)), Random.Range(Screen.height / 3, Screen.height - (Screen.height / 3)), 1);
        player = Instantiate(player, randomPoint, Quaternion.identity);
        player.name = "Player";
        timer.SetActive(true);
        //PauseButton.SetActive(true);
        SpawnAMonster();
        StartCoroutine(IncreaseMonsterLimit());
        StartCoroutine(ExtendSpike());
        StartCoroutine(ReduceSpikeExtendTime());
    }

    public void KillPlayer(GameObject player)
    {
        Destroy(player);
        //soundManager.PlaySound("Explosion");
        SoundManager.Instance.Play("Explosion");
        gameOverUI.SetActive(true);
        PauseButton.SetActive(false);
        Debug.Log("Enemyeated" + spawnerControl.eatedEnemy);
        Debug.Log("Jokereated" + jokerSpawnerControl.eatedJoker);
    }

    public void PauseGame()
    {
        timer.GetComponent<TimerScript>().SetIsPaused(true);
        gamePausedUI.SetActive(true);
        PauseButton.SetActive(false);

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].GetComponent<MonsterControl>().SetIsMonsterMovementAllowed(false);
            monsters[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        /*GameObject[] jokers = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].GetComponent<MonsterControl>().SetIsMonsterMovementAllowed(false);
        }*/

        GameObject[] spikes = GameObject.FindGameObjectsWithTag("Spike");
        for (int i = 0; i < spikes.Length; i++)
        {
            spikes[i].GetComponent<SpikeControl>().SetCanMove(false);
        }

    }

    public void ResumeGame()
    {
        timer.GetComponent<TimerScript>().SetIsPaused(false);
        gamePausedUI.SetActive(false);
        PauseButton.SetActive(true);

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].GetComponent<MonsterControl>().SetIsMonsterMovementAllowed(true);
        }

        /*GameObject[] jokers = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].GetComponent<MonsterControl>().SetIsMonsterMovementAllowed(false);
        }*/

        GameObject[] spikes = GameObject.FindGameObjectsWithTag("Spike");
        for (int i = 0; i < spikes.Length; i++)
        {
            spikes[i].GetComponent<SpikeControl>().SetCanMove(true);
        }

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

    IEnumerator GetRandomSpike()
    {
        while (true)
        {
            randomSpike = spikeSpawner.spikes[Random.Range(0, spikeSpawner.spikes.Length)];
            SpikeControl spikeControl = randomSpike.GetComponent<SpikeControl>();
            //spikeControl.isMovingToView
            if (spikeControl.isWaiting)
            {

                //SpikeControl spikeControl = randomSpike.GetComponent<SpikeControl>();
                spikeControl.endPos = CalcEndPos(randomSpike.name, spikeControl.startPos);
                StartCoroutine(NormalizeSpike(randomSpike, spikeControl));
                break;
            }
            yield return null;
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
        yield return new WaitForSeconds(spikeControl.travelDuration + 5);
        spikeControl.endPos = spikeControl.tempEndPos;
    }


    IEnumerator ExtendSpike()
    {
        while (!timerScript.GetIsPaused())
        {
            yield return new WaitForSeconds(extendTime);
            StartCoroutine(GetRandomSpike());
        }
    }

    IEnumerator ReduceSpikeExtendTime()
    {
        float reducedBy = Random.Range(0.6f, 1.5f);

        while (!timerScript.GetIsPaused())
        {
            yield return new WaitForSeconds(10f);
            if (extendTime >= Random.Range(2f, 3f))
            {
                extendTime -= reducedBy;
            }
        }
    }



}
