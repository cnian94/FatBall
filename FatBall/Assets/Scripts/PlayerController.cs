using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    public float moveSpeed = 50f;

    Rigidbody2D rb;

    Touch touch;
    Vector3 touchPosition, whereToMove;
    bool isMoving = false;

    float previousDistanceToTouchPos, currentDistanceToTouchPos;

    public GameMaster gameMaster;
    public GameObject Explosion;

    bool isHalfSizeJokerCatched = false;

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        gameMaster = FindObjectOfType<GameMaster>();
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("PlayerScale:" + gameObject.transform.localScale);
        if (gameObject.transform.localScale.x <= 20)
        {
            gameMaster.jokerWeights[3] = 0;
        }
        if (gameObject.transform.localScale.x > 20)
        {
            gameMaster.jokerWeights[3] = 10;

        }
        if (isMoving)
        {
            currentDistanceToTouchPos = (touchPosition - transform.position).magnitude;
        }

        if(Input.touchCount > 0)
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

        }

        if (Input.GetMouseButtonDown(0))
        {
                //Debug.Log("MOUSE POS: " + Input.mousePosition);
                previousDistanceToTouchPos = 0;
                currentDistanceToTouchPos = 0;
                isMoving = true;
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                touchPosition.z = 0;
                //Debug.Log("TOUCH POS: " + touchPosition);
                whereToMove = (touchPosition - transform.position).normalized;
                rb.velocity = new Vector2(whereToMove.x * moveSpeed, whereToMove.y * moveSpeed);
        }
		
        if(currentDistanceToTouchPos > previousDistanceToTouchPos)
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
        }

        if (isMoving)
        {
            previousDistanceToTouchPos = (touchPosition - transform.position).magnitude;
        }

        if (isHalfSizeJokerCatched)
        {
            StartCoroutine(LerpScale(2f));
        }
    }

    void SetIsHalfSizeJokerCatched(bool val)
    {
        isHalfSizeJokerCatched = val;
    }

    IEnumerator LerpScale(float time)
    {
        SetIsHalfSizeJokerCatched(false);
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(gameObject.transform.localScale.x / 2, gameObject.transform.localScale.y / 2, gameObject.transform.localScale.z);
        float originalTime = time;
        GameObject bubble = GameObject.Find("Bubble");
        Vector3 bubbleScale = bubble.transform.localScale;
        Vector3 bubbleTargetScale = new Vector3(bubbleScale.x / 2, bubbleScale.y / 2, bubbleScale.z); ;


        while (time > 0.0f)
        {
            time -= Time.deltaTime;

            transform.localScale = Vector3.Lerp(targetScale, originalScale, time / originalTime);
            
            if(bubble != null)
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
