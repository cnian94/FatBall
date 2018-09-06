using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    Vector2 movement;
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

    public Vector3 dirInit = Vector3.zero;

    Matrix4x4 calibrationMatrix;


    void Awake()
    {
        moveSpeed = Screen.width / 0.5f; //Joker Control 106 dan sonrasını da değiştir
        tempScale = transform.localScale;
        tempScale.x = GetPlayerScaleX();
        tempScale.y = GetPlayerScaleX();
        transform.localScale = tempScale;
    }

    // Use this for initialization
    float GetPlayerScaleX()
    {
        float x = Screen.width / 25f;  // yeni playerlar için      Screen.width / 25f  olucak
        return x;
    }
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        gameMaster = FindObjectOfType<GameMaster>();

        CalibrateAccelerometer();
    }


    //Method for calibration 
    void CalibrateAccelerometer()
    {
        dirInit = Input.acceleration;

        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0f, 0f, -1f), dirInit);
        //create identity matrix ... rotate our matrix to match up with down vec
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, new Vector3(1f, 1f, 1f));

        //get the inverse of the matrix
        calibrationMatrix = matrix.inverse;

    }

    //Method to get the calibrated input 
    Vector3 FixAcceleration(Vector3 accelerator)
    {
        Vector3 accel = this.calibrationMatrix.MultiplyVector(accelerator);
        return accel;
    }

    private void FixedUpdate()
    {
        //rb.velocity = new Vector2(dirX, dirY);
        rb.AddForce(movement); //tilt control açar
    }


    Vector2 _InputDir;
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


        _InputDir = FixAcceleration(Input.acceleration); //tilt control açar
        movement = new Vector2(_InputDir.x, _InputDir.y) * moveSpeed;  //tilt control açar




        /*if (!timer.GetComponent<TimerScript>().GetIsPaused())  //dokunmatik oynamak için 
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
        }*/  //buraya kadar dokunmatik oynamak için


    }


    void StartWaneEffect(string tag)  //Joker yediği zaman küçülme süresi
    {
        StartCoroutine(LerpScale(1f, tag));
    }

    void StartGetFatEffect(Vector3[] scales)  // Monster yediği zaman küçülme süresi
    {
        StartCoroutine(LerpScaleMonster(1f, scales));
    }



    IEnumerator LerpScaleMonster(float time, Vector3[] scales)
    {

        float originalTime = time;
        Debug.Log("scales: " + scales[0] + " - " + scales[1]);

        while (time > 0f)
        {
            time -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(scales[1], scales[0], time / originalTime);
            Debug.Log("Timer: " + time);
            yield return null;
        }
    }

    IEnumerator LerpScale(float time, string jokerTag)
    {

        Vector3 originalScale = transform.localScale;
        float originalTime = time;
        Vector3 bubbleScale = new Vector3();
        Vector3 bubbleTargetScale = new Vector3();
        GameObject bubble = GameObject.Find("Bubble");

        if (jokerTag == "BroccoliJoker")
        {

            Vector3 targetScale = new Vector3(gameObject.transform.localScale.x / 2, gameObject.transform.localScale.y / 2, gameObject.transform.localScale.z);

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

        else
        {
            Vector3 targetScale = new Vector3(gameObject.transform.localScale.x - gameObject.transform.localScale.x / 8, gameObject.transform.localScale.y - gameObject.transform.localScale.y / 8, gameObject.transform.localScale.z);
            if (bubble)
            {
                bubbleScale = bubble.transform.localScale;
                bubbleTargetScale = new Vector3(bubbleScale.x  - bubbleScale.x/8, bubbleScale.y  - bubbleScale.y/8, bubbleScale.z);
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

            }
        }

    }
}
