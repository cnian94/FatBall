using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JokerControl : MonoBehaviour {

 

    Rigidbody2D rb;
    GameObject target;
    private SoundManagerScript soundManager;

    public GameObject bubble;

    public float accelerationTime = 2f;
    public float maxSpeed;

    private Vector3 movement;
    private float timeLeft;

    public static float max_distance_from_view = 200f;
    public JokerSpawnerControl spawnerControl;
    public FloatingPlayer2DController playerControl;

    //private bool isCatched = false;
    

    Vector3 temp;


    public float GetRandomScale()
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

        return -1;
    }

    // Use this for initialization
    void Start()
    {
        spawnerControl = FindObjectOfType<JokerSpawnerControl>();
        playerControl = FindObjectOfType<FloatingPlayer2DController>();
        transform.name = transform.name.Replace("(Clone)", "").Trim();
        //gameObject.tag = "spawn";
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
            playerControl.moveForce = 500;
            Destroy(gameObject);
        }

        if (gameObject.tag == "turtle")
        {
            playerControl.moveForce = 500;
            Destroy(gameObject);
        }

        if (gameObject.tag == "shield")
        {
            Destroy(gameObject);
            Destroy(bubble);
            GameMaster.gm.isBubbleCatched = false;
            SoundManagerScript.audioSrc.Stop();
            SoundManagerScript.audioSrc.clip = null;
        }


    }

    void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.name)
        {

            case "Player":
                Debug.Log("Player catched a " + gameObject.tag + "joker");
                if(gameObject.tag == "rabbit")
                {
                    gameObject.SetActive(false);
                    soundManager.PlaySound("Fastjoker");
                    spawnerControl.num_of_jokers--;
                    playerControl.moveForce = playerControl.moveForce * 2;
                    Invoke("RevertJokerEffect", 5.0f);

                }

                if (gameObject.tag == "turtle")
                {
                    gameObject.SetActive(false);
                    soundManager.PlaySound("joker");
                    spawnerControl.num_of_jokers--;
                    playerControl.moveForce = playerControl.moveForce / 2;
                    Invoke("RevertJokerEffect", 5.0f);
                }

                if (gameObject.tag == "shield" && !GameMaster.gm.isBubbleCatched)
                {
                    GameMaster.gm.isBubbleCatched = true;
                    gameObject.SetActive(false);
                    soundManager.PlaySound("Shieldjoker");
                    Debug.Log("Shield catched !!");
                    bubble = Instantiate(bubble, target.transform.localPosition, Quaternion.identity);
                    Invoke("RevertJokerEffect", 8.0f);
                }

                if (gameObject.tag == "HalfSize")
                {
                    gameObject.SetActive(false);
                    soundManager.PlaySound("HalfSize");
                    spawnerControl.num_of_jokers--;
                    col.gameObject.transform.localScale = new Vector3(col.gameObject.transform.localScale.x/2 , col.gameObject.transform.localScale.y/2 , col.gameObject.transform.localScale.z);
                    
                }

                    break;
        }
    }

}
