using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChartboostSDK;
using System.Linq;
using UnityEngine.Events;

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

    public GameObject ResultPanel;

    public TimerScript timerScript;

    public Text IngameResult;

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

    public float moveSpeedDivider = 0.65f;



    public int MonsterSpawnLimit;

    public int numOfStrawberry;

    int pointFromJokers;
    int pointFromEnemy;
    //string[] time_score;
    float time_score;
    int pointFromTime;
    public int finalScore;

    public Color charColor;

    public UnityEvent FinishEvent;

    public GameObject ChartBoost;



    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        numOfStrawberry = 0;
        timerScript = timer.GetComponent<TimerScript>();
        //soundManager = FindObjectOfType<SoundManagerScript>();
        spawnerControl = FindObjectOfType<MonstersSpawnerControl>();
        jokerSpawnerControl = FindObjectOfType<JokerSpawnerControl>();
        spikeSpawner = FindObjectOfType<SpikeSpawnerControl>();
        Application.targetFrameRate = 60;
        jokerTimeLeft = jokerSpawnTime + spawnDelay;
        countDown.gameObject.SetActive(true);
        extendTime = Random.Range(6f, 10f);
        ChartBoost = GameObject.FindGameObjectWithTag("ChartBoost");
    }

    IEnumerator TakeSs()
    {
        int no = 1;
        string name = "gameIpadthree" + no + ".png";

        while (true)
        {
            yield return new WaitForSeconds(5);
            ScreenCapture.CaptureScreenshot(name);
            no++;
            name = "gameIpadthree" + no + ".png";
        }
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
        pointFromJokers = eatedJoker * 5;
        pointFromEnemy = -eatedEnemy;
        //time_score = timerScript.time_text.text.Split(':');
        //time_score = float.Parse(timerScript.minutes) * 60 + float.Parse(timerScript.seconds);
        //pointFromTime = int.Parse(time_score[0]) * 60 + int.Parse(time_score[1].Split('.')[0]);
        finalScore = (int)timerScript.t + pointFromJokers + pointFromEnemy;
        IngameResult.text = finalScore.ToString();


        if (jokerTimeLeft <= 0f)
        {
            SpawnAJoker();

            jokerTimeLeft += jokerSpawnTime;
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


    public IEnumerator IncreaseMonsterLimit() //Monster sayısının artış hızı
    {
        while (player && spawnerControl.num_of_monsters < MonsterSpawnLimit)
        {
            //Debug.Log("INCREASING MONSTER LIMIT !!");
            yield return new WaitForSeconds(5);
            spawnerControl.monsters_limit = spawnerControl.monsters_limit + Mathf.Log(spawnerControl.monsters_limit) / 1.5f;
            SpawnAMonster();
        }
    }

    int FindDominantChannel(float r, float g, float b)
    {
        float max3 = Mathf.Max(r, Mathf.Max(g, b));
        if (max3 == r)
        {
            return 0;
        }

        if (max3 == g)
        {
            return 1;
        }

        if (max3 == b)
        {
            return 2;
        }

        return -1;
    }

    public static Color GetMostUsedColor(Texture2D bitMap)
    {
        var colorIncidence = new Dictionary<Color, int>();
        for (var x = 0; x < bitMap.width; x++)
        {
            for (var y = 0; y < bitMap.height; y++)
            {
                var pixelColor = bitMap.GetPixel(x, y);
                if (colorIncidence.Keys.Contains<Color>(pixelColor))
                {
                    colorIncidence[pixelColor]++;
                }
                if (!colorIncidence.Keys.Contains<Color>(pixelColor) && pixelColor != new Color(0, 0, 0, 0) && pixelColor != new Color(1, 1, 1, 1))
                {

                    if (pixelColor.r >= 0.5f || pixelColor.g >= 0.5f || pixelColor.b >= 0.5f)
                    {
                        if (pixelColor.r >= 0.7f && pixelColor.g >= 0.7f && pixelColor.b >= 0.7f)
                        {
                            //colorIncidence.Add(pixelColor, 1);
                        }
                        else
                        {
                            colorIncidence.Add(pixelColor, 1);
                        }
                    }
                }

            }
        }
        //Debug.Log("VALUE: " + colorIncidence.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value).First().Value);
        return colorIncidence.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value).First().Key;

    }

    public Sprite GetSelectedCharSprite(int index)
    {

        byte[] dataImage = System.Convert.FromBase64String(NetworkManager.instance.inventoryList.inventory[index].character.img);
        Texture2D mytexture = new Texture2D(1, 1);
        mytexture.LoadImage(dataImage);

        Color[] MyPixel = mytexture.GetPixels();
        //Debug.Log("AVARAGE COLOR: " + GetAverageColor(mytexture));
        //charColor = GetColorFromArray(MyPixel, 100, 200);
        //charColor = GetAverageColor(mytexture);
        //Debug.Log("AVARAGE COLOR: " + GetMostUsedColor(mytexture));
        charColor = GetMostUsedColor(mytexture);

        Sprite charSprite = Sprite.Create(mytexture, new Rect(0.0f, 0.0f, mytexture.width, mytexture.height), new Vector2(0.5f, 0.5f));
        return charSprite;
    }

    public IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(spawnDelay);
        NetworkManager.instance.PlayCounter++;
        CancelInvoke("PlayStartSound");
        Vector3 randomPoint = new Vector3(Random.Range(Screen.width / 6, Screen.width - (Screen.width / 6)), Random.Range(Screen.height / 3, Screen.height - (Screen.height / 3)), 1);

        Debug.Log("SELECTED CHAR: " + PlayerPrefs.GetInt("selectedChar"));
        player.GetComponent<SpriteRenderer>().sprite = GetSelectedCharSprite(PlayerPrefs.GetInt("selectedChar"));
        DestroyImmediate(player.GetComponent<PolygonCollider2D>(), true);
        DestroyImmediate(player.GetComponent<PolygonCollider2D>(), true);
        player.AddComponent<PolygonCollider2D>();
        player.GetComponent<PolygonCollider2D>().isTrigger = true;
        player.AddComponent<PolygonCollider2D>();
        player = Instantiate(player, randomPoint, Quaternion.identity);
        moveSpeedDivider = moveSpeedDivider - ((int.Parse(NetworkManager.instance.inventoryList.inventory[PlayerPrefs.GetInt("selectedChar")].character.attr.Split(',')[0]) - 4) * 0.05f);
        float moveSpeed = Screen.width / (moveSpeedDivider);
        //Debug.Log("moveSpeed: " + moveSpeed);
        player.GetComponent<PlayerController>().moveSpeed = moveSpeed;
        player.name = "Player";
        MonsterSpawnLimit = Random.Range(15, 25);


        timer.SetActive(true);
        //PauseButton.SetActive(true);
        SpawnAMonster();
        StartCoroutine(IncreaseMonsterLimit());
        StartCoroutine(ExtendSpike());
        StartCoroutine(ReduceSpikeExtendTime());
        /*if (NetworkManager.instance.PlayCounter == NetworkManager.instance.RandomAdLimit)
        {
            Debug.Log("CACHING INTERSTITIAL !!");
            ChartBoost.GetComponent<CharBoostManager>().CacheInterstitial("Game Over");
        }*/
        //StartCoroutine(TakeSs());
    }

    public void KillPlayer(GameObject player)
    {
        Destroy(player);
        CalculateScore();
        //Debug.Log("Enemyeated" + eatedEnemy);
        //Debug.Log("Jokereated" + eatedJoker);
    }

    public void CalculateScore()
    {
        pointFromJokers = eatedJoker * 5;
        pointFromEnemy = -eatedEnemy;
        //time_score = timerScript.time_text.text.Split(':');
        //pointFromTime = int.Parse(time_score[0]) * 60 + int.Parse(time_score[1].Split('.')[0]);
        //finalScore = pointFromTime + pointFromJokers + pointFromEnemy;
        //time_score = int.Parse(timerScript.minutes) * 60 + int.Parse(timerScript.seconds);
        finalScore = (int)timerScript.t + pointFromJokers + pointFromEnemy;
        //Debug.Log("poi1ntFromJokers:" + pointFromJokers);
        //Debug.Log("pointFromEnemy:" + pointFromEnemy);
        //Debug.Log("pointFromTime:" + pointFromTime);
        //Debug.Log("FINAL SCORE:" + finalScore);
        if (finalScore > NetworkManager.instance.playerModel.highscore)
        {
            //Debug.Log("New high score !!");
            NetworkManager.instance.playerModel.highscore = finalScore;
            NetworkManager.instance.playerModel.coins = NetworkManager.instance.playerModel.coins + finalScore;
            NetworkManager.instance.StartCoroutine(NetworkManager.instance.SetHighScore());
            timerScript.result = "Healthy Food X " + eatedJoker + " = " + pointFromJokers + System.Environment.NewLine +
                                         "Junk Food X " + eatedEnemy + " = " + pointFromEnemy + System.Environment.NewLine +
                                         "Life Span = " + pointFromTime + System.Environment.NewLine +
                                         "---------------------------------------" + System.Environment.NewLine +
                                         "Total: " + finalScore;
            FinishEvent.Invoke();

        }

        else
        {
            NetworkManager.instance.playerModel.coins = NetworkManager.instance.playerModel.coins + finalScore;
            NetworkManager.instance.StartCoroutine(NetworkManager.instance.SetHighScore());
            timerScript.result = "Healthy Food X " + eatedJoker + " = " + pointFromJokers + System.Environment.NewLine +
                             "Junk Food X " + eatedEnemy + " = " + pointFromEnemy + System.Environment.NewLine +
                             "Life Span = " + pointFromTime + System.Environment.NewLine +
                             "--------------------------------" + System.Environment.NewLine +
                             "Total: " + finalScore;
            FinishEvent.Invoke();
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
            if (numOfStrawberry >= 2 && jokerSpawnerControl.jokers[jokerSpawnerControl.randomJoker].CompareTag("GrapeFruitJoker"))
            {
                //Debug.Log("COND 1");
                jokerSpawnerControl.joker = Instantiate(jokerSpawnerControl.jokers[6], randomPoint, Quaternion.identity);
                jokerSpawnerControl.joker.name = "Joker";
                jokerSpawnerControl.num_of_jokers++;
            }

            if (numOfStrawberry < 2 && jokerSpawnerControl.jokers[jokerSpawnerControl.randomJoker].CompareTag("GrapeFruitJoker"))
            {
                //Debug.Log("COND 2");
                numOfStrawberry++;
                jokerSpawnerControl.joker = Instantiate(jokerSpawnerControl.jokers[jokerSpawnerControl.randomJoker], randomPoint, Quaternion.identity);
                jokerSpawnerControl.joker.name = "Joker";
                jokerSpawnerControl.num_of_jokers++;
            }

            if (!jokerSpawnerControl.jokers[jokerSpawnerControl.randomJoker].CompareTag("GrapeFruitJoker"))
            {
                //Debug.Log("COND 3");
                jokerSpawnerControl.joker = Instantiate(jokerSpawnerControl.jokers[jokerSpawnerControl.randomJoker], randomPoint, Quaternion.identity);
                jokerSpawnerControl.joker.name = "Joker";
                jokerSpawnerControl.num_of_jokers++;

            }

        }
    }


    IEnumerator GetRandomSpike()
    {
        while (true)
        {
            randomSpike = spikeSpawner.spikes[Random.Range(0, spikeSpawner.spikes.Length)];
            SpikeControl spikeControl = randomSpike.GetComponent<SpikeControl>();
            if (spikeControl.isWaiting)
            {
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
        while (true)
        {
            yield return new WaitForSeconds(extendTime);
            StartCoroutine(GetRandomSpike());
        }
    }

    IEnumerator ReduceSpikeExtendTime()
    {
        float reducedBy = Random.Range(0.6f, 1.5f);

        while (true)
        {
            yield return new WaitForSeconds(10f);
            if (extendTime >= Random.Range(2f, 3f))
            {
                extendTime -= reducedBy;
            }
        }
    }

}
