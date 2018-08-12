using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterControl : MonoBehaviour {

    //float moveSpeed;
    //Vector3 directionToTarget;

    Rigidbody2D rb;
	GameObject target;
    private SoundManagerScript soundManager;

    public float accelerationTime;
    public float maxSpeed;

    private Vector3 movement;
    private float timeLeft;
    //public bool isEnterTheView = false;

    public static float max_distance_from_view;
    private MonstersSpawnerControl spawnerControl;
    private GameMaster gameMaster;

    Vector3 temp;

    void Awake()
    {
        accelerationTime = Random.Range(1f, 2f);
        max_distance_from_view = 100f;
        movement = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
    }

    // Use this for initialization
    void Start () {
        spawnerControl = FindObjectOfType<MonstersSpawnerControl>();
        gameMaster = FindObjectOfType<GameMaster>();
        transform.name = transform.name.Replace("(Clone)", "").Trim();
        //gameObject.tag = "spawn";
        target = GameObject.Find ("Player");
		rb = GetComponent<Rigidbody2D>();
        float randomScale = Random.Range(10f, 20f);
        soundManager = FindObjectOfType<SoundManagerScript>();
        /*if (SceneManager.GetActiveScene().name == "MenuScene")
        {
           randomScale = Random.Range(0f, 1f);
        }*/

        temp = transform.localScale;
        temp.x = randomScale;
        temp.y = randomScale;

        transform.localScale = temp;
    }


    // Update is called once per frame
    void Update () {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            //movement = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
            movement = new Vector3(-movement.x + Random.Range(-5f, 5f), -movement.y + Random.Range(-5f, 5f), 0);
            maxSpeed = Random.Range(1f, 6f);
            //Debug.Log("Movement: " + movement);
            //Debug.Log("Max Speed: " + maxSpeed);
            //Debug.Log("----------------------");
            timeLeft += accelerationTime;
        }

        Vector3 position = gameObject.transform.position;


        if (position.x <= -max_distance_from_view || position.x >= Screen.width + max_distance_from_view ||
            position.y <= -max_distance_from_view || position.y >= Screen.height + max_distance_from_view)
        {
            //movement = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
            spawnerControl.randomSpawnPoint = Random.Range(0, spawnerControl.spawnPoints.Length);
            gameObject.transform.position = new Vector3(spawnerControl.spawnPoints[spawnerControl.randomSpawnPoint].position.x, spawnerControl.spawnPoints[spawnerControl.randomSpawnPoint].position.y, transform.position.z);
        }



        /*if (position.x > 0 && position.x < Screen.width && position.y > 0 && position.y < Screen.height)
        {
            this.isEnterTheView = true;
        }

        if ((position.x < 0 || position.x > Screen.width || position.y < 0 || position.y > Screen.height))
        {
            //Debug.Log("ENEMY LEAVED THE VIEW !! " + this.rb.gameObject.name);
            //this.isEnterTheView = false;
            //rb.gameObject.SetActive(false);
            //Destroy(gameObject);
            this.rb.gameObject.transform.position = new Vector3(-position.x + Screen.width, -position.y + Screen.height, transform.position.z);
        }*/
    }

    void FixedUpdate()
    {
        rb.AddForce(movement * maxSpeed);
    }

    void OnTriggerEnter2D (Collider2D col)
	{
		switch (col.gameObject.name) {

		case "Player":
                if (!gameMaster.isBubbleCatched)
                {
                    Destroy(gameObject);
                    soundManager.PlaySound("Enemy");
                    spawnerControl.num_of_monsters--;
                    gameMaster.SpawnAMonster();
                    col.gameObject.transform.localScale = new Vector3(col.gameObject.transform.localScale.x + gameObject.transform.localScale.x / 2, col.gameObject.transform.localScale.y + gameObject.transform.localScale.y / 2, gameObject.transform.localScale.z);
                }
			break;
		}
	}


    void MoveMonster ()
	{
		if (target != null) {
            //Debug.Log("MONSTER IS MOVING!!!");
            
            //directionToTarget = (target.transform.position - transform.position).normalized;
			//rb.velocity = new Vector2(directionToTarget.x * moveSpeed, directionToTarget.y * moveSpeed);
        }
		else
			rb.velocity = Vector3.zero;
	}
}
