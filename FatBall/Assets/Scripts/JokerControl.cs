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

    public static float max_distance_from_view = 100f;
    public JokerSpawnerControl spawnerControl;
    public PlayerController playerControl;
    public MonstersSpawnerControl monstersSpawnerControl; // bu abi neden burda ?


    Vector3 temp;
    private bool isJokerMovementAllowed = true;


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
        accelerationTime = Random.Range(Screen.width / 1500f, Screen.width / 500f);
        //maxSpeed = Random.Range(1f, 4f);
        spawnerControl = FindObjectOfType<JokerSpawnerControl>();
        playerControl = FindObjectOfType<PlayerController>();
        monstersSpawnerControl = FindObjectOfType<MonstersSpawnerControl>();
        transform.name = transform.name.Replace("(Clone)", "").Trim();
        target = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();

        float randomScale = Random.Range(Screen.width / 25f, Screen.width / 18f);
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
            maxSpeed = Random.Range(1f, Screen.width / 300f);
            yield return new WaitForSeconds(accelerationTime);

        }
    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Random.Range(10f, 15f) / 50f);
        Vector3 position = gameObject.transform.position;
   
        if (position.x <= -max_distance_from_view || position.x >= Screen.width + max_distance_from_view ||
            position.y <= -max_distance_from_view || position.y >= Screen.height + max_distance_from_view)
        {
            spawnerControl.randomSpawnPoint = Random.Range(0, spawnerControl.spawnPoints.Length);
            gameObject.transform.position = new Vector3(spawnerControl.spawnPoints[spawnerControl.randomSpawnPoint].position.x, spawnerControl.spawnPoints[spawnerControl.randomSpawnPoint].position.y, transform.position.z);
        }
    }


    void FixedUpdate()
    {
        rb.AddForce(movement * maxSpeed);
    }

    void RevertJokerEffect()
    {

        if (gameObject.CompareTag("GrapeFruitJoker"))
        {
            //playerControl.moveForce = 500;
            playerControl.moveSpeed = Screen.width / 1.1f;
            Destroy(gameObject);
        }

        if (gameObject.CompareTag("BeerJoker"))
        {
            //playerControl.moveForce = 500;
            playerControl.moveSpeed = Screen.width / 1.1f;
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
                if (gameObject.CompareTag("GrapeFruitJoker")) //tavşanı yerse
                {
                    gameObject.SetActive(false);
                    SoundManager.Instance.Play("GrapeFruitJoker");
                    spawnerControl.num_of_jokers--; //yediği için joker sayısı 1 azalır ki yenisi çıkabilsin
                                                    //playerControl.moveForce = playerControl.moveForce * 2;
                    target.SendMessage("StartWaneEffect", gameObject.tag); 
                    playerControl.moveSpeed = playerControl.moveSpeed * 2; //movespeed 2 katına çıkar
                    Invoke("RevertJokerEffect", 5.0f); //5sn sonra efekt gider. Yukarda revert var. Revert aşağıda olsa daha doğru olmaz mı ?

                }

                if (gameObject.CompareTag("BeerJoker")) //tavşanla aynı mantık
                {
                    gameObject.SetActive(false);
                    SoundManager.Instance.Play("BeerJoker");
                    spawnerControl.num_of_jokers--;
                    target.SendMessage("StartWaneEffect", gameObject.tag);
                    playerControl.moveSpeed = playerControl.moveSpeed / 2;
                    Invoke("RevertJokerEffect", 5.0f);
                }

                if (gameObject.CompareTag("RadishJoker") && !GameMaster.gm.isBubbleCatched) //beni biraz aştı :D her yerde var
                {
                    GameMaster.gm.isBubbleCatched = true;
                    gameObject.SetActive(false);
                    SoundManager.Instance.PlayMusic("RadishJoker");
                    spawnerControl.num_of_jokers--;
                    bubble = Instantiate(bubble, target.transform.localPosition, Quaternion.identity);
                    bubble.SendMessage("SetIsBubbleEffectActive", true);
                    Invoke("RevertJokerEffect", 8.0f); //8 saniye sürüyor

                }

                if (gameObject.CompareTag("BroccoliJoker"))
                {
                    gameObject.SetActive(false);
                    SoundManager.Instance.Play("BroccoliJoker");
                    spawnerControl.num_of_jokers--;
                    target.SendMessage("StartWaneEffect", gameObject.tag); 
                }

                if (gameObject.CompareTag("Reset"))
                {
                    gameObject.SetActive(false);
                    GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy"); //Bütün enemyleri tek yerde topladık.Tag'i spawn.
                    SoundManager.Instance.Play("Reset");
                    for (var i = 0; i < Enemies.Length; i++) //bütün enemyler olana kadar.
                    {
                        Destroy(Enemies[i]); //kaç tane enemy varsa i sayısına eşit işte
                    }
                    monstersSpawnerControl.monsters_limit = 3; //en başa döner monster limit ve monster sayısı. Bunu değiştirirsen MonsterSpawnerControl'ü de değiştir. Hatta hieracy de de değiştir garanti olsun.
                    monstersSpawnerControl.num_of_monsters = 0; //üsttekinin aynısı geçerli.

                    spawnerControl.num_of_jokers--;
                }

                break;
        }
    }

}
