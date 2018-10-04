using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JokerControl : MonoBehaviour
{


    Rigidbody2D rb;
    GameObject target;

    public GameObject soundManager;//SoundManagerScript'in kodlarını buraya bağlarken kullanmak için isim verdik.
    //private SoundManagerScript soundManagerScript;

    public GameObject bubble; //bubble için farklı bir kod olduğu için bu abiyi ayrı tuttuk diye düşünüyorum.

    public float accelerationTime;
    public float maxSpeed;

    private Vector3 movement;
    private float timeLeft;


    public JokerSpawnerControl JokerSpawnerControl;
    public static float max_distance_from_view = 200f;
    public JokerSpawnerControl spawnerControl;
    public PlayerController playerControl;
    public MonstersSpawnerControl monstersSpawnerControl; // bu abi neden burda ?


    Vector3 temp;
    private bool isJokerMovementAllowed = true;

    private bool isColliding = false;


    public void SetIsJokerMovementAllowed(bool val)
    {
        if (!val)
        {
            movement = Vector3.zero;
            maxSpeed = 0;
            isJokerMovementAllowed = val;
        }

        else
        {
            isJokerMovementAllowed = val;
            StartCoroutine(MoveJoker());
        }

    }

    // Use this for initialization
    void Start()
    {
        accelerationTime = Random.Range(0.5f, 2f);
        //maxSpeed = Random.Range(1f, 4f);
        spawnerControl = FindObjectOfType<JokerSpawnerControl>();
        playerControl = FindObjectOfType<PlayerController>();
        monstersSpawnerControl = FindObjectOfType<MonstersSpawnerControl>();
        JokerSpawnerControl = FindObjectOfType<JokerSpawnerControl>();
        transform.name = transform.name.Replace("(Clone)", "").Trim();
        target = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();

        float randomScale = Random.Range(Screen.width / 11f, Screen.width / 18f);
        temp = transform.localScale;
        temp.x = randomScale;
        temp.y = randomScale;
        transform.localScale = temp;

        StartCoroutine(MoveJoker());
    }

    IEnumerator MoveJoker()
    {
        while (isJokerMovementAllowed)
        {
            movement = new Vector3(-movement.x + Random.Range(-20f, 20f), -movement.y + Random.Range(-20f, 20f), 0);
            maxSpeed = Random.Range(Screen.width / 750f, Screen.width / 300f);
            yield return new WaitForSeconds(accelerationTime);

        }
    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Random.Range(Screen.width / 750f, Screen.width / 500f));
        Vector3 position = gameObject.transform.position;

        if (position.x <= -max_distance_from_view || position.x >= Screen.width + max_distance_from_view ||
            position.y <= -max_distance_from_view || position.y >= Screen.height + max_distance_from_view)
        {
            spawnerControl.randomSpawnPoint = Random.Range(0, spawnerControl.spawnPoints.Length);
            movement = new Vector3(-movement.x + Random.Range(-20f, 20f), -movement.y + Random.Range(-20f, 20f), 0);
            maxSpeed = Random.Range(Screen.width / 750f, Screen.width / 300f);
            gameObject.transform.position = new Vector3(spawnerControl.spawnPoints[spawnerControl.randomSpawnPoint].position.x, spawnerControl.spawnPoints[spawnerControl.randomSpawnPoint].position.y, transform.position.z);
        }
    }


    void FixedUpdate()
    {
        rb.AddForce(movement * maxSpeed);

        if (rb.velocity.x >= Screen.width / 2.5f || rb.velocity.y >= Screen.width / 2.5f || rb.velocity.x <= -Screen.width / 2.5f || rb.velocity.y <= -Screen.width / 2.5f)
        {
            Invoke("ReduceVelocity", 1f);
        }
    }

    void ReduceVelocity()
    {
        rb.velocity = new Vector3(20, 20, 20);
    }

    void RevertJokerEffect()
    {

        if (gameObject.CompareTag("GrapeFruitJoker"))
        {
            //playerControl.moveForce = 500;
            playerControl.moveSpeed = Screen.width / 0.5f;
            Destroy(gameObject);
        }

        if (gameObject.CompareTag("BeerJoker"))
        {
            //playerControl.moveForce = 500;
            playerControl.moveSpeed = Screen.width / 0.5f;
            Destroy(gameObject);
        }

        if (gameObject.CompareTag("RadishJoker"))
        {
            Destroy(gameObject);
            Destroy(bubble);
            GameMaster.gm.isBubbleCatched = false;
            SoundManager.Instance.MusicSource.Stop();
            bubble.SendMessage("SetIsBubbleEffectActive", false);
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {

        switch (col.gameObject.name)
        {

            case "Player":
                if (gameObject.CompareTag("GrapeFruitJoker") && !isColliding) //tavşanı yerse
                {
                    isColliding = true;
                    gameObject.SetActive(false);
                    SoundManager.Instance.Play("GrapeFruitJoker");
                    spawnerControl.num_of_jokers--; //yediği için joker sayısı 1 azalır ki yenisi çıkabilsin
                                                    //playerControl.moveForce = playerControl.moveForce * 2;
                    target.SendMessage("StartWaneEffect", gameObject.tag);
                    playerControl.moveSpeed = playerControl.moveSpeed * 1.2f; //movespeed 1.2 katına çıkar
                    Invoke("RevertJokerEffect", 5.0f); //5sn sonra efekt gider. Yukarda revert var. Revert aşağıda olsa daha doğru olmaz mı ?
                    GameMaster.gm.eatedJoker++;
                    GameMaster.gm.numOfStrawberry--;


                }

                if (gameObject.CompareTag("BeerJoker") && !isColliding) //tavşanla aynı mantık
                {
                    isColliding = true;
                    gameObject.SetActive(false);
                    SoundManager.Instance.Play("BeerJoker");
                    spawnerControl.num_of_jokers--;
                    target.SendMessage("StartWaneEffect", gameObject.tag);
                    playerControl.moveSpeed = playerControl.moveSpeed / 1.2f;
                    Invoke("RevertJokerEffect", 5.0f);
                    GameMaster.gm.eatedJoker++;
                }

                if (gameObject.CompareTag("RadishJoker") && !GameMaster.gm.isBubbleCatched && !isColliding) //beni biraz aştı :D her yerde var
                {
                    isColliding = true;
                    GameMaster.gm.isBubbleCatched = true;
                    gameObject.SetActive(false);
                    SoundManager.Instance.PlayMusic("RadishJoker");
                    spawnerControl.num_of_jokers--;
                    bubble = Instantiate(bubble, target.transform.localPosition, Quaternion.identity);
                    bubble.SendMessage("SetIsBubbleEffectActive", true);
                    Invoke("RevertJokerEffect", 8.0f); //8 saniye sürüyor
                    GameMaster.gm.eatedJoker++;

                }

                if (gameObject.CompareTag("BroccoliJoker") && !isColliding)
                {
                    isColliding = true;
                    gameObject.SetActive(false);
                    SoundManager.Instance.Play("BroccoliJoker");
                    spawnerControl.num_of_jokers--;
                    target.SendMessage("StartWaneEffect", gameObject.tag);
                    GameMaster.gm.eatedJoker++;
                }

                if (gameObject.CompareTag("Reset") && !isColliding)
                {
                    isColliding = true;
                    gameObject.SetActive(false);
                    GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy"); //Bütün enemyleri tek yerde topladık.Tag'i spawn.
                    SoundManager.Instance.Play("Reset");
                    for (var i = 0; i < Enemies.Length; i++) //bütün enemyler olana kadar.
                    {
                        Destroy(Enemies[i]); //kaç tane enemy varsa i sayısına eşit işte
                    }

                    int temp = monstersSpawnerControl.num_of_monsters;

                    monstersSpawnerControl.monsters_limit = 3; //en başa döner monster limit ve monster sayısı. Bunu değiştirirsen MonsterSpawnerControl'ü de değiştir. Hatta hieracy de de değiştir garanti olsun.
                    monstersSpawnerControl.num_of_monsters = 0; //üsttekinin aynısı geçerli.
                    spawnerControl.num_of_jokers--;
                    GameMaster.gm.eatedJoker++;

                    if (temp >= GameMaster.gm.MonsterSpawnLimit)
                    {
                        GameMaster.gm.StartCoroutine(GameMaster.gm.IncreaseMonsterLimit());
                    }

                }

                if (gameObject.CompareTag("GrapeJoker") && !isColliding)
                {
                    isColliding = true;
                    gameObject.SetActive(false);
                    SoundManager.Instance.Play("Enemy");
                    spawnerControl.num_of_jokers--;
                    target.SendMessage("StartWaneEffect", gameObject.tag);
                    GameMaster.gm.eatedJoker++;
                }

                if (gameObject.CompareTag("CherryJoker") && !isColliding)
                {
                    isColliding = true;
                    gameObject.SetActive(false);
                    SoundManager.Instance.Play("Enemy");
                    spawnerControl.num_of_jokers--;
                    target.SendMessage("StartWaneEffect", gameObject.tag);
                    GameMaster.gm.eatedJoker++;
                }
                break;
        }
    }

    void OnTriggerExit()
    {
        if (isColliding)
        {
            isColliding = false; //Allows for another object to be struck by this one
        }
    }

}
