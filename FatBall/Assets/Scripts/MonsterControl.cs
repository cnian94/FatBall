﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterControl : MonoBehaviour {

    //float moveSpeed;
    //Vector3 directionToTarget;

    Rigidbody2D rb;
	GameObject target;

    public float accelerationTime = 2f;
    public float maxSpeed;

    private Vector3 movement;
    private float timeLeft;
    public bool isEnterTheView = false;

    public static float max_distance_from_view = 200f;
    public MonstersSpawnerControl spawnerControl;

    Vector3 temp;

    // Use this for initialization
    void Start () {
        spawnerControl = FindObjectOfType<MonstersSpawnerControl>();
        transform.name = transform.name.Replace("(Clone)", "").Trim();
        gameObject.tag = "spawn";
        target = GameObject.Find ("Player");
		rb = GetComponent<Rigidbody2D> ();
        float randomScale = Random.Range(10f, 20f);
        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
           randomScale = Random.Range(0f, 1f);
        }

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
            movement = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
            maxSpeed = Random.Range(2f, 12f);
            if (SceneManager.GetActiveScene().name == "MenuScene")
            {
                maxSpeed = Random.Range(0f, 1f);
            }
            timeLeft += accelerationTime;

            Vector3 position = this.rb.gameObject.transform.position;
            //Debug.Log(position.x + " vs " + Screen.width);
            //Debug.Log(position.y + " vs " + Screen.height);


            if(position.x <= -max_distance_from_view || position.x >= Screen.width + max_distance_from_view ||
                position.y <= -max_distance_from_view || position.y >= Screen.height + max_distance_from_view)
            {
                //Debug.Log("ENEMY's GONE SO FAR!! COME HERE BACK !!");
                this.rb.gameObject.transform.position = new Vector3(Random.Range(-50f, 0), Random.Range(-50f, 0), transform.position.z);
            }



            if (position.x > 0 && position.x < Screen.width && position.y > 0 && position.y < Screen.height)
            {
                this.isEnterTheView = true;
                //Debug.Log("ENEMY ENTERED THE VIEW !! " + this.rb.gameObject.name);
            }

            if ((position.x < 0 || position.x > Screen.width || position.y < 0 || position.y > Screen.height) && this.isEnterTheView)
            {
                //Debug.Log("ENEMY LEAVED THE VIEW !! " + this.rb.gameObject.name);
                this.isEnterTheView = false;
                //rb.gameObject.SetActive(false);
                //Destroy(gameObject);

                this.rb.gameObject.transform.position = new Vector3(-position.x + Screen.width, -position.y + Screen.height, transform.position.z);
            }
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(movement * maxSpeed);
    }

    void OnTriggerEnter2D (Collider2D col)
	{
		switch (col.gameObject.name) {

		case "Player":
			//MonstersSpawnerControl.spawnAllowed = false;
			//Instantiate (explosion, col.gameObject.transform.position, Quaternion.identity);
			Destroy (gameObject);
            spawnerControl.num_of_monsters--;
            //Debug.Log("ACTIVE MONSTERS: " + MonstersSpawnerControl.num_of_monsters);
            spawnerControl.SpawnAMonster();
            col.gameObject.transform.localScale = new Vector3(col.gameObject.transform.localScale.x + gameObject.transform.localScale.x, col.gameObject.transform.localScale.y + gameObject.transform.localScale.y, gameObject.transform.localScale.z);
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