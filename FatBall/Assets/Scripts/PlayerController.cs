using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;

    Rigidbody2D rb;

    Touch touch;
    Vector3 touchPosition, whereToMove;
    bool isMoving = false;

    float previousDistanceToTouchPos, currentDistanceToTouchPos;

    public GameMaster gameMaster;
    public GameObject Explosion;
    public GameObject timer;
    Vector3 tempScale;


    // For Acceloremeter
    float dirX;
    float dirY;

    private Vector2 dirInit = Vector2.zero;


    /*private GameObject[] enemies;
    private float old_z;
    private float new_z;
    private Vector3 ba;
    private Vector3 bc;
    private Vector3 ab;
    private Vector3 ac;
    private Vector3 playerLastPos;
    private Vector3 playerCurrentPos;*/


    void Awake()
    {
        moveSpeed = Screen.width / 1.1f; //Joker Control 106 dan sonrasını da değiştir
        //old_z = 0;
        //playerLastPos = gameObject.transform.position;
        tempScale = transform.localScale;
        tempScale.x = GetPlayerScaleX();
        tempScale.y = GetPlayerScaleX();
        transform.localScale = tempScale;
        Input.gyro.enabled = true;
    }

    // Use this for initialization
    float GetPlayerScaleX()
    {
        float x = Screen.width / 12.5f;
        return x;
    }
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        gameMaster = FindObjectOfType<GameMaster>();


        //dirInit.x = Input.acceleration.x;
        //dirInit.y = Input.acceleration.y;

    }

    private void FixedUpdate()
    {
        //rb.velocity = new Vector2(dirX, dirY);
    }

    // Update is called once per frame
    void Update()
    {
        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //playerCurrentPos = gameObject.transform.position;

        if (gameObject.transform.localScale.x <= Screen.width / 10f)
        {
            gameMaster.jokerWeights[3] = 0;
        }
        if (gameObject.transform.localScale.x > Screen.width / 10f)
        {
            gameMaster.jokerWeights[3] = 20;
        }

        
        /*
        //new accelerometer
        Vector2 dir = Vector2.zero;

        // you need to send the difference of your current accelerometer position to the initial state.
        dir.x = Input.acceleration.x - dirInit.x;
        dir.y = Input.acceleration.y - dirInit.y;
        //dir.z = transform.position.z;
        if (dir.sqrMagnitude > 1)
        {
            dir.Normalize();
        }

        dir *= Time.deltaTime;
        transform.Translate(dir * moveSpeed);
        */




        if (!timer.GetComponent<TimerScript>().GetIsPaused())
        {
            if (isMoving)
            {
                currentDistanceToTouchPos = (touchPosition - transform.position).magnitude;
            }

            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    previousDistanceToTouchPos = 0;
                    currentDistanceToTouchPos = 0;
                    isMoving = true;
                    touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    touchPosition.z = 0;
                    whereToMove = (touchPosition - transform.position).normalized;
                    rb.velocity = new Vector2(whereToMove.x * moveSpeed, whereToMove.y * moveSpeed);
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    isMoving = false;
                    rb.velocity = Vector2.zero;
                }

            }

            if (Input.GetMouseButtonDown(0))
            {
                previousDistanceToTouchPos = 0;
                currentDistanceToTouchPos = 0;
                isMoving = true;
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                touchPosition.z = 0;
                whereToMove = (touchPosition - transform.position).normalized;
                rb.velocity = new Vector2(whereToMove.x * moveSpeed, whereToMove.y * moveSpeed);
            }

            if (currentDistanceToTouchPos > previousDistanceToTouchPos)
            {
                isMoving = false;
                rb.velocity = Vector2.zero;
            }

            if (Input.GetMouseButtonUp(0)) //yeni hareket için
            {
                isMoving = false;
                rb.velocity = Vector2.zero;
            }

            if (isMoving)
            {
                previousDistanceToTouchPos = (touchPosition - transform.position).magnitude;
            }
        }


    }

    void SetIsHalfSizeJokerCatched()
    {
        StartCoroutine(LerpScale(2f));
    }

    IEnumerator LerpScale(float time)
    {
        //SetIsHalfSizeJokerCatched(false);
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(gameObject.transform.localScale.x / 2, gameObject.transform.localScale.y / 2, gameObject.transform.localScale.z);
        float originalTime = time;
        Vector3 bubbleScale = new Vector3();
        Vector3 bubbleTargetScale = new Vector3();
        GameObject bubble = GameObject.Find("Bubble");
        if (bubble)
        {
            bubbleScale = bubble.transform.localScale;
            bubbleTargetScale = new Vector3(bubbleScale.x / 2, bubbleScale.y / 2, bubbleScale.z);
        }

        while (time > 0f)
        {
            time -= Time.deltaTime;

            transform.localScale = Vector3.Lerp(targetScale, originalScale, time / originalTime);

            if (bubble != null)
            {
                bubble.transform.localScale = Vector3.Lerp(bubbleTargetScale, bubbleScale, time / originalTime);
            }
            yield return null;

        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!GameMaster.gm.isBubbleCatched)
        {
            switch (col.gameObject.tag)
            {

                case "Spike":
                    GameObject.Find("Timer").SendMessage("Finish");
                    Explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
                    Explosion.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                    //gameOverUI.SetActive(true);
                    gameMaster.KillPlayer(gameObject);
                    Destroy(Explosion, 3);
                    break;

                    /*case "LeftSpike":
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
                        break; */
            }
        }

    }
}
