using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    public float moveSpeed;

    Rigidbody2D rb;

    Touch touch;
    Vector3 touchPosition, whereToMove;
    bool isMoving = false;

    float previousDistanceToTouchPos, currentDistanceToTouchPos;

    public GameMaster gameMaster;
    public GameObject Explosion;

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
        moveSpeed = 500f;
        //old_z = 0;
        //playerLastPos = gameObject.transform.position;
    }

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        gameMaster = FindObjectOfType<GameMaster>();
    }
	
	// Update is called once per frame
	void Update () {

        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //playerCurrentPos = gameObject.transform.position;

        if (gameObject.transform.localScale.x <= 39)
        {
            gameMaster.jokerWeights[3] = 0;
        }
        if (gameObject.transform.localScale.x > 39)
        {
            gameMaster.jokerWeights[3] = 39;

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
                previousDistanceToTouchPos = 0;
                currentDistanceToTouchPos = 0;
                isMoving = true;
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                touchPosition.z = 0;
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


        /*for (int i=0; i < enemies.Length - 1; i++)
        {
           for(int j=i+1; j<enemies.Length; j++)
            {
                ba = enemies[i].transform.position - enemies[j].transform.position;
                bc = gameObject.transform.position - enemies[j].transform.position;
                ab = enemies[j].transform.position - enemies[i].transform.position;
                ac = gameObject.transform.position - enemies[i].transform.position;

                if (Vector3.Dot(ba, bc) > 0 && Vector3.Dot(ab, ac) > 0)
                {
                    //Debug.Log(Vector3.Cross(bc, ba));
                    new_z = Vector3.Cross(bc, ba).z;

                    if (Mathf.Sign(old_z) != Mathf.Sign(new_z) && old_z != 0)
                    {
                        //Debug.Log("OLD Z: " + old_z);
                        if (enemies[i].transform.position.x > 0 && enemies[i].transform.position.x < Screen.width && enemies[i].transform.position.y > 0
                            && enemies[i].transform.position.y < Screen.height && enemies[j].transform.position.x > 0 &&
                            enemies[j].transform.position.x < Screen.width && enemies[j].transform.position.y > 0
                            && enemies[j].transform.position.y < Screen.height)
                        {
                            //Debug.Log("Player passed between enemies !!");
                            Destroy(enemies[i]);
                            Destroy(enemies[j]);
                        }

                    }
                }
            }
        }
        old_z = Vector3.Cross(bc, ba).z;*/
        //playerLastPos = playerCurrentPos;
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
          bubbleScale  = bubble.transform.localScale;
          bubbleTargetScale   = new Vector3(bubbleScale.x / 2, bubbleScale.y / 2, bubbleScale.z);
        }

        while (time > 0f)
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
