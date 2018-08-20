﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JokerControl : MonoBehaviour {


    Rigidbody2D rb;
    GameObject target;
    private SoundManagerScript soundManager;//SoundManagerScript'in kodlarını buraya bağlarken kullanmak için isim verdik.

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


    // Use this for initialization
    void Start()
    {
        accelerationTime = Random.Range(0.5f, 1.5f);
        //maxSpeed = Random.Range(1f, 4f);
        spawnerControl = FindObjectOfType<JokerSpawnerControl>();
        playerControl = FindObjectOfType<PlayerController>();
        monstersSpawnerControl = FindObjectOfType<MonstersSpawnerControl>();
        transform.name = transform.name.Replace("(Clone)", "").Trim();
        target = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        soundManager = FindObjectOfType<SoundManagerScript>();

        float randomScale = Random.Range(25f, 35f);
        temp = transform.localScale;
        temp.x = randomScale;
        temp.y = randomScale;
        transform.localScale = temp;

        StartCoroutine(MoveJoker());
    }

    IEnumerator MoveJoker()
    {
        while (true)
        {
            movement = new Vector3(-movement.x + Random.Range(-20f, 20f), -movement.y + Random.Range(-20f, 20f), 0);
            maxSpeed = Random.Range(1f, 2.5f);
            yield return new WaitForSeconds(accelerationTime);

        }
    }


    // Update is called once per frame
    void Update()
    {
        //timeLeft -= Time.deltaTime;
        transform.Rotate(0, 0, Random.Range(10f, 15f) / 50);
        Vector3 position = gameObject.transform.position;

        /*if (timeLeft <= 0)
        {
            movement = new Vector3(-movement.x + Random.Range(-20f, 20f), -movement.y + Random.Range(-20f, 20f), 0);
            maxSpeed = Random.Range(1f, 4f);
            timeLeft += accelerationTime;
        }*/


   
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

        if(gameObject.CompareTag("GrapeFruitJoker")) 
        {
            //playerControl.moveForce = 500;
            playerControl.moveSpeed = 500;
            Destroy(gameObject);
        }

        if (gameObject.CompareTag("ColaJoker"))
        {
            //playerControl.moveForce = 500;
            playerControl.moveSpeed = 500;
            Destroy(gameObject);
        }

        if (gameObject.CompareTag("RadishJoker"))
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
                if(gameObject.CompareTag("GrapeFruitJoker")) //tavşanı yerse
                {
                    gameObject.SetActive(false);
                    soundManager.PlaySound("RabbitJoker"); //SoundManagerScrippten çeker
                    spawnerControl.num_of_jokers--; //yediği için joker sayısı 1 azalır ki yenisi çıkabilsin
                    //playerControl.moveForce = playerControl.moveForce * 2;
                    playerControl.moveSpeed = playerControl.moveSpeed * 2; //movespeed 2 katına çıkar
                    Invoke("RevertJokerEffect", 5.0f); //5sn sonra efekt gider. Yukarda revert var. Revert aşağıda olsa daha doğru olmaz mı ?

                }

                if (gameObject.CompareTag("ColaJoker")) //tavşanla aynı mantık
                {
                    gameObject.SetActive(false);
                    soundManager.PlaySound("Joker");
                    spawnerControl.num_of_jokers--;
                    //playerControl.moveForce = playerControl.moveForce / 2;
                    playerControl.moveSpeed = playerControl.moveSpeed / 2;
                    Invoke("RevertJokerEffect", 5.0f);
                }

                if (gameObject.CompareTag("RadishJoker") && !GameMaster.gm.isBubbleCatched) //beni biraz aştı :D her yerde var
                {
                    GameMaster.gm.isBubbleCatched = true;
                    gameObject.SetActive(false);
                    soundManager.PlaySound("ShieldJoker"); //SoundManagerScrippten sesi çekiyor.
                    spawnerControl.num_of_jokers--;
                    bubble = Instantiate(bubble, target.transform.localPosition, Quaternion.identity);
                    bubble.SendMessage("SetIsBubbleEffectActive", true);
                    Invoke("RevertJokerEffect", 8.0f); //8 saniye sürüyor
                    
                }

                if (gameObject.CompareTag("BroccoliJoker"))
                {
                    gameObject.SetActive(false);
                    soundManager.PlaySound("HalfSizeJoker");
                    spawnerControl.num_of_jokers--;
                    target.SendMessage("SetIsHalfSizeJokerCatched"); //Revert değil. Player controller 98. satırda açıklanıyor bu satır.
                }

                if (gameObject.CompareTag("Reset"))
                {
                    gameObject.SetActive(false);
                    GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy"); //Bütün enemyleri tek yerde topladık.Tag'i spawn.
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
