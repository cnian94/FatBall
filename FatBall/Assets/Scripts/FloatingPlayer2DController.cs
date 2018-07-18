using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class FloatingPlayer2DController : MonoBehaviour {

    Rigidbody2D myBody;

    public float moveForce = 500, boostMultiplier = 2;
    public GameMaster gameMaster;
    public GameObject Explosion;

    bool isHalfSizeJokerCatched = false;

    // Use this for initialization
    void Start () {
        Debug.Log("PLAYER START CALLED !!");
        gameMaster = FindObjectOfType<GameMaster>();
        myBody = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isHalfSizeJokerCatched)
        {
            StartCoroutine(LerpScale(2f));
        }

    }

    // Update is called once per frame 
    void FixedUpdate () {
        Vector2 moveVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical")) * moveForce;
        bool isBoosting = CrossPlatformInputManager.GetButton("Boost");
        myBody.AddForce(moveVec * (isBoosting ? boostMultiplier : 1));
	}



    void SetIsHalfSizeJokerCatched(bool val)
    {
        isHalfSizeJokerCatched = val;
    }


    /*void ScaleOverTime(Vector3 destinationScale)
    {
        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, destinationScale, Time.deltaTime * 10);
    }*/


    IEnumerator LerpScale(float time)
    {
        SetIsHalfSizeJokerCatched(false);   
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(gameObject.transform.localScale.x / 2, gameObject.transform.localScale.y / 2, gameObject.transform.localScale.z); ;
        float originalTime = time;


        while (time > 0.0f)
        {
            time -= Time.deltaTime;

            transform.localScale = Vector3.Lerp(targetScale, originalScale, time / originalTime);
            yield return null;

        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (!GameMaster.gm.isBubbleCatched)
        {
            switch (col.gameObject.name)
            {

                case "TopSpike":
                    GameObject.Find("Timer").SendMessage("Finish");
                    Explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
                    Explosion.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                    //gameOverUI.SetActive(true);
                    gameMaster.KillPlayer(gameObject);
                    Destroy(Explosion, 3);
                    break;

                case "LeftSpike":
                    GameObject.Find("Timer").SendMessage("Finish");
                    Explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
                    Explosion.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                    //gameOverUI.SetActive(true);
                    gameMaster.KillPlayer(gameObject);
                    Destroy(Explosion, 3);
                    break;

                case "BottomSpike":
                    GameObject.Find("Timer").SendMessage("Finish");
                    Explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
                    Explosion.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                    //gameOverUI.SetActive(true);
                    gameMaster.KillPlayer(gameObject);
                    Destroy(Explosion, 3);
                    break;

                case "RightSpike":
                    GameObject.Find("Timer").SendMessage("Finish");
                    Explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
                    Explosion.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                    //gameOverUI.SetActive(true);
                    gameMaster.KillPlayer(gameObject);
                    Destroy(Explosion, 3);
                    break;
            }
        }

    }
}
