﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterControl : MonoBehaviour
{

    Rigidbody2D rb;

    public float accelerationTime;
    public float maxSpeed;

    private Vector3 movement;

    public static float max_distance_from_view;
    private MonstersSpawnerControl spawnerControl;

    Vector3 temp;

    private bool isMonsterMovementAllowed = true;

    public bool isColliding = false;
    //private int hitCount = 0;

    public float monsterDivider = 5;



    public void SetIsMonsterMovementAllowed(bool val)
    {
        if (!val)
        {
            movement = Vector3.zero;
            maxSpeed = 0;
            isMonsterMovementAllowed = val;
        }

        else
        {
            isMonsterMovementAllowed = val;
            StartCoroutine(MoveMonster());
        }
    }

    void Awake()
    {

        //accelerationTime = Random.Range(Screen.width / 1500f, Screen.width / 375f);
        accelerationTime = Random.Range(0.5f, 2f);
        max_distance_from_view = 200f;
    }

    // Use this for initialization
    void Start()
    {
        spawnerControl = FindObjectOfType<MonstersSpawnerControl>();
        transform.name = transform.name.Replace("(Clone)", "").Trim();
        rb = GetComponent<Rigidbody2D>();
        float randomScale = Random.Range(Screen.width / 11f, Screen.width / 18f);
        StartCoroutine(MoveMonster());

        temp = transform.localScale;
        temp.x = randomScale;
        temp.y = randomScale;

        transform.localScale = temp;

        monsterDivider = monsterDivider + ((int.Parse(NetworkManager.instance.inventoryList.inventory[PlayerPrefs.GetInt("selectedChar")].character.attr.Split(',')[2]) - 1));
        //Debug.Log("monsterDivider" + monsterDivider);
    }

    IEnumerator MoveMonster()
    {
        while (isMonsterMovementAllowed)
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
        //Vector3 position = gameObject.transform.position;
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


    void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {

            case "Player":
                {
                    if (!GameMaster.gm.isBubbleCatched)
                    {
                        if (!isColliding)
                        {
                            isColliding = true;
                            //Debug.Log("boyut10" + gameObject.transform.localScale.x / 10);
                            //Debug.Log("boyut8" + gameObject.transform.localScale.x / 8);
                            Vector3 targetScale = new Vector3(col.gameObject.transform.localScale.x + gameObject.transform.localScale.x / monsterDivider, col.gameObject.transform.localScale.y + gameObject.transform.localScale.y / monsterDivider , gameObject.transform.localScale.z);
                            SoundManager.Instance.Play("Enemy");
                            spawnerControl.num_of_monsters--;
                            GameMaster.gm.SpawnAMonster();
                            Destroy(gameObject);
                            Vector3[] scales = { col.gameObject.transform.localScale, targetScale };
                            col.gameObject.SendMessage("StartGetFatEffect", scales);
                            GameMaster.gm.eatedEnemy++;
                            GameMaster.gm.comboCounter = 0;
                        }
                    }
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
