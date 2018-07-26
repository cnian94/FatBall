using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JokerControl : MonoBehaviour {


    Rigidbody2D rb;
    GameObject target;
    private SoundManagerScript soundManager;//SoundManagerScript'in kodlarını buraya bağlarken kullanmak için isim verdik.

    public GameObject bubble; //bubble için farklı bir kod olduğu için bu abiyi ayrı tuttuk diye düşünüyorum.

    public float accelerationTime = 2f; 
    public float maxSpeed;

    private Vector3 movement;
    private float timeLeft;

    public static float max_distance_from_view = 200f;
    public JokerSpawnerControl spawnerControl;
    public PlayerController playerControl;
    public MonstersSpawnerControl monstersSpawnerControl; // bu abi neden burda ?


    Vector3 temp;


    public float GetRandomScale() // Random scale vermişiz. Yeni png formatlarında hepsi aynı boy olursa random boylar da aynı aralık olur.
    {
        if(gameObject.tag == "rabbit")
        {
            return Random.Range(2f, 4f);
        }

        if (gameObject.tag == "turtle")
        {
            return Random.Range(5f, 7f);
        }

        if (gameObject.tag == "shield")
        {
            return Random.Range(7f, 12f);
        }

        if (gameObject.tag == "HalfSize")
        {
            return Random.Range(7f, 12f);
        }

        if (gameObject.tag == "Reset")
        {
            return Random.Range(7f, 12f);
        }
        return -1;
    }

    // Use this for initialization
    void Start()
    {
        spawnerControl = FindObjectOfType<JokerSpawnerControl>();
        //playerControl = FindObjectOfType<FloatingPlayer2DController>();
        playerControl = FindObjectOfType<PlayerController>();
        monstersSpawnerControl = FindObjectOfType<MonstersSpawnerControl>();
        transform.name = transform.name.Replace("(Clone)", "").Trim();
        target = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        soundManager = FindObjectOfType<SoundManagerScript>();

        float randomScale = GetRandomScale();

        temp = transform.localScale;
        temp.x = randomScale;
        temp.y = randomScale;

        transform.localScale = temp;
    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Random.Range(10f, 15f) / 50);
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            movement = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
            maxSpeed = Random.Range(2f, 8f);

            timeLeft += accelerationTime;

            Vector3 position = this.rb.gameObject.transform.position;


            if (position.x <= -max_distance_from_view || position.x >= Screen.width + max_distance_from_view ||
                position.y <= -max_distance_from_view || position.y >= Screen.height + max_distance_from_view)
            {
                spawnerControl.randomSpawnPoint = Random.Range(0, spawnerControl.spawnPoints.Length);
                this.rb.gameObject.transform.position = new Vector3(spawnerControl.spawnPoints[spawnerControl.randomSpawnPoint].position.x, spawnerControl.spawnPoints[spawnerControl.randomSpawnPoint].position.y, transform.position.z);
            }

            /*if (isBubbleEffectActive)
            {
                Debug.Log("STARTING BUBBLE EFFECT !!");
                StartBubbleEffect();
            }*/

        }
   

    }

    void FixedUpdate()
    {
        rb.AddForce(movement * maxSpeed);
    }

    void RevertJokerEffect()
    {
        Debug.Log("BUBBLE: " + bubble);

        if(gameObject.tag == "rabbit") 
        {
            //playerControl.moveForce = 500;
            playerControl.moveSpeed = 500;
            Destroy(gameObject);
        }

        if (gameObject.tag == "turtle")
        {
            //playerControl.moveForce = 500;
            playerControl.moveSpeed = 500;
            Destroy(gameObject);
        }

        if (gameObject.tag == "shield")
        {
            Destroy(gameObject);
            Destroy(bubble);
            GameMaster.gm.isBubbleCatched = false;
            SoundManagerScript.audioSrc.Stop();
            SoundManagerScript.audioSrc.clip = null;
            bubble.SendMessage("SetIsBubbleEffectActive", false);
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.name)
        {

            case "Player":
                Debug.Log("Player catched a " + gameObject.tag + "joker");
                if(gameObject.tag == "rabbit") //tavşanı yerse
                {
                    gameObject.SetActive(false);
                    soundManager.PlaySound("Fastjoker"); //SoundManagerScrippten çeker
                    spawnerControl.num_of_jokers--; //yediği için joker sayısı 1 azalır ki yenisi çıkabilsin
                    //playerControl.moveForce = playerControl.moveForce * 2;
                    playerControl.moveSpeed = playerControl.moveSpeed * 2; //movespeed 2 katına çıkar
                    Invoke("RevertJokerEffect", 5.0f); //5sn sonra efekt gider. Yukarda revert var. Revert aşağıda olsa daha doğru olmaz mı ?

                }

                if (gameObject.tag == "turtle") //tavşanla aynı mantık
                {
                    gameObject.SetActive(false);
                    soundManager.PlaySound("joker");
                    spawnerControl.num_of_jokers--;
                    //playerControl.moveForce = playerControl.moveForce / 2;
                    playerControl.moveSpeed = playerControl.moveSpeed / 2;
                    Invoke("RevertJokerEffect", 5.0f);
                }

                if (gameObject.tag == "shield" && !GameMaster.gm.isBubbleCatched) //beni biraz aştı :D her yerde var
                {
                    GameMaster.gm.isBubbleCatched = true;
                    gameObject.SetActive(false);
                    soundManager.PlaySound("Shieldjoker"); //SoundManagerScrippten sesi çekiyor.
                    //Debug.Log("Shield catched !!");
                    bubble = Instantiate(bubble, target.transform.localPosition, Quaternion.identity);
                    bubble.SendMessage("SetIsBubbleEffectActive", true);
                    Invoke("RevertJokerEffect", 8.0f); //8 saniye sürüyor
                    
                }

                if (gameObject.tag == "HalfSize")
                {
                    gameObject.SetActive(false);
                    soundManager.PlaySound("HalfSize");
                    spawnerControl.num_of_jokers--;
                    target.SendMessage("SetIsHalfSizeJokerCatched", true); //Revert değil. Player controller 98. satırda açıklanıyor bu satır.
                }

                if (gameObject.tag == "Reset")
                {
                    gameObject.SetActive(false);
                    GameObject[] Enemies = GameObject.FindGameObjectsWithTag("spawn"); //Bütün enemyleri tek yerde topladık.Tag'i spawn.
                    for (var i = 0; i < Enemies.Length; i++) //bütün enemyler olana kadar.
                    {
                        Destroy(Enemies[i]); //kaç tane enemy varsa i sayısına eşit işte
                    }
                    monstersSpawnerControl.monsters_limit = 3; //en başa döner monster limit ve monster sayısı. Bunu değiştirirsen MonsterSpawnerControl'ü de değiştir. Hatta hieracy de de değiştir garanti olsun.
                    monstersSpawnerControl.num_of_monsters = 0; //üsttekinin aynısı geçerli.
                        
                    //soundManager.PlaySound("HalfSize");
                    spawnerControl.num_of_jokers--;
                    //target.SendMessage("SetIsHalfSizeJokerCatched", true);
                }

                break;
        }
    }

}
