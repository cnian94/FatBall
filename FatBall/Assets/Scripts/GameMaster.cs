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
    public GameObject danger;

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


    public int eatedEnemy = 0;
    public int eatedJoker = 0;

    public AnimationClip Dangerclip;


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
        while (!timerScript.GetIsPaused() && spawnerControl.monsters_limit < 25)
        {
            yield return new WaitForSeconds(5);
            //spawnerControl.monsters_limit = spawnerControl.monsters_limit * 2;
            //spawnerControl.monsters_limit++;
            spawnerControl.monsters_limit = spawnerControl.monsters_limit + Mathf.Log(spawnerControl.monsters_limit) / 1.5f;
            SpawnAMonster();
        }
    }

    public Sprite GetSelectedCharSprite(int index)
    {

        byte[] dataImage = System.Convert.FromBase64String(NetworkController.Instance.inventoryList.inventory[index].character.img);
        Texture2D mytexture = new Texture2D(1, 1);
        mytexture.LoadImage(dataImage);

        Sprite charSprite = Sprite.Create(mytexture, new Rect(0.0f, 0.0f, mytexture.width, mytexture.height), new Vector2(0.5f, 0.5f));
        return charSprite;
    }

    public IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(spawnDelay);
        CancelInvoke("PlayStartSound");
        Vector3 randomPoint = new Vector3(Random.Range(Screen.width / 6, Screen.width - (Screen.width / 6)), Random.Range(Screen.height / 3, Screen.height - (Screen.height / 3)), 1);

        player.GetComponent<SpriteRenderer>().sprite = GetSelectedCharSprite(PlayerPrefs.GetInt("selectedChar"));
        DestroyImmediate(player.GetComponent<PolygonCollider2D>(), true);
        player.AddComponent<PolygonCollider2D>();
        player.GetComponent<PolygonCollider2D>().isTrigger = true;
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
        Debug.Log("Enemyeated" + eatedEnemy);
        Debug.Log("Jokereated" + eatedJoker);
        CalculateScore();
    }

    public void CalculateScore()
    {
        int pointFromJokers = eatedJoker * 2;
        int pointFromEnemy = -eatedEnemy;
        string[] time_score = timerScript.time_text.text.Split(':');
        int pointFromTime = int.Parse(time_score[0]) * 60 + int.Parse(time_score[1].Split('.')[0]);
        int finalScore = pointFromTime + pointFromJokers + pointFromEnemy;
        Debug.Log("pointFromJokers:" + pointFromJokers);
        Debug.Log("pointFromEnemy:" + pointFromEnemy);
        Debug.Log("pointFromTime:" + pointFromTime);
        Debug.Log("FINAL SCORE:" + finalScore);
        if (finalScore > NetworkController.Instance.playerModel.highscore)
        {
            Debug.Log("New high score !!");
            NetworkController.Instance.playerModel.highscore = finalScore;
            NetworkController.Instance.playerModel.coins = NetworkController.Instance.playerModel.coins + finalScore;
            NetworkController.Instance.StartCoroutine(NetworkController.Instance.SetHighScore());
        }
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

    Vector3 GetDangerPos()
    {
        Vector3 pos = randomSpike.GetComponent<SpikeControl>().endPos;
        pos.z = 1;

        if (randomSpike.name.Equals("TopSpike"))
        {
            pos.y -= 300;
        }

        if (randomSpike.name.Equals("RightSpike"))
        {
            pos.x -= 300;

        }

        if (randomSpike.name.Equals("BottomSpike"))
        {
            pos.y += 300;

        }

        if (randomSpike.name.Equals("LeftSpike"))
        {

            pos.x += 300;
        }

        if (randomSpike.name.Equals("TopLeftCornerSpike"))
        {
            pos.x += 300;
            pos.y -= 300;
        }

        if (randomSpike.name.Equals("TopRightCornerSpike"))
        {
            pos.x -= 300;
            pos.y -= 300;
        }


        if (randomSpike.name.Equals("BottomRightCornerSpike"))
        {
            pos.x -= 300;
            pos.y += 300;

        }

        if (randomSpike.name.Equals("BottomLeftCornerSpike"))
        {
            pos.x += 300;
            pos.y += 300;
        }

        return pos;
    }


    void SetDangerClip()
    {

        float startTime = 0;
        float endTime = 0.50f;

        Vector3 startValue = danger.transform.localScale;
        Vector3 endValue = new Vector3(danger.transform.localScale.x + 10, danger.transform.localScale.y + 10, danger.transform.localScale.z);

        AnimationCurve curve_x = AnimationCurve.Linear(startTime, startValue.x, endTime, endValue.x);
        AnimationCurve curve_y = AnimationCurve.Linear(startTime, startValue.y, endTime, endValue.y);

        startTime += 0.50f;
        endTime += 0.50f;
        startValue.x += 10;
        startValue.y += 10;

        endValue.x += 30;
        endValue.y += 30;

        AnimationCurve curve_x2 = AnimationCurve.Linear(startTime, startValue.x, endTime, endValue.x);
        AnimationCurve curve_y2 = AnimationCurve.Linear(startTime, startValue.x, endTime, endValue.x);

        string relativeObjectName = string.Empty; // Means the object holding the animator component

        Dangerclip.SetCurve(relativeObjectName, typeof(Transform), "localScale.x", curve_x);
        Dangerclip.SetCurve(relativeObjectName, typeof(Transform), "localScale.y", curve_y);
        Dangerclip.SetCurve(relativeObjectName, typeof(Transform), "localScale.x", curve_x2);
        Dangerclip.SetCurve(relativeObjectName, typeof(Transform), "localScale.y", curve_y2);
    }


    IEnumerator GetRandomSpike()
    {
        while (true)
        {
            randomSpike = spikeSpawner.spikes[Random.Range(0, spikeSpawner.spikes.Length)];
            SetDangerClip();
            SpikeControl spikeControl = randomSpike.GetComponent<SpikeControl>();
            //spikeControl.isMovingToView
            if (spikeControl.isWaiting)
            {
                GameObject dangerObject = GameObject.FindGameObjectWithTag("Danger");
                SoundManager.Instance.Play("Alert");
                if (dangerObject == null)
                {
                    Debug.Log("DANGER OBJECT NOT FOUND !!");
                    Instantiate(danger, GetDangerPos(), Quaternion.identity);
                    dangerObject = GameObject.FindGameObjectWithTag("Danger");
                    yield return new WaitForSeconds(1.5f);
                    Destroy(dangerObject);
                }

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
