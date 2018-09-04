using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeControl : MonoBehaviour
{
    //Rigidbody2D rb;

    //float height;
    float sWidth;
    float sHeight;

    public bool canMove;

    public float travelDuration;

    public Vector3 tempEndPos;
    public Vector3 endPos;

    public Vector3 startPos;

    public bool isMovingToView = false;

    public GameObject Player;

    Vector3 tempScale;

    public float counter;
    public bool isWaiting = false;



    private void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
        //height = GetComponent<SpriteRenderer>().bounds.size.y;  //spike'ın heightını bulur sayı olarak yazar
        tempScale = transform.localScale;
        tempScale.x = GetSpikeScaleX();
        tempScale.y = GetSpikeScaleY();

        transform.localScale = tempScale;
        sWidth = Screen.width;
        sHeight = Screen.height;
        canMove = true;
        tempEndPos = transform.position;
        endPos = transform.position;
        travelDuration = Random.Range(3f, 4f);

    }

    public void SetCanMove(bool val)
    {
        if (val)
        {
            canMove = val;
            StartCoroutine(MoveSpike(startPos, travelDuration));
        }
        else
        {
            canMove = val;
        }
    }


    float GetSpikeScaleX()
    {
        float x = Screen.width / 15;
        return x;
    }

    float GetSpikeScaleY()
    {
        float y = Screen.height / 8.89f;
        return y;
    }

    // Use this for initialization
    void Start()
    {
        string tempName = gameObject.name;
        startPos = gameObject.transform.position;
        CalcEndPos(tempName, startPos);

        StartCoroutine(MoveSpike(startPos, travelDuration));
    }

    void CalcEndPos(string tempName, Vector3 startPos)
    {
        //Vector3 endPos = startPos;


        if (tempName == "TopLeftCornerSpike")
        {

            tempEndPos.x = -sHeight / 18;
            tempEndPos.y = sHeight + sHeight / 18;

            endPos.x = -sHeight / 18;
            endPos.y = sHeight + sHeight / 18;
        }

        if (tempName == "TopRightCornerSpike")
        {

            tempEndPos.x = Screen.width + sHeight / 18;
            tempEndPos.y = sHeight + sHeight / 18;

            endPos.x = Screen.width + sHeight / 18;
            endPos.y = sHeight + sHeight / 18;
        }

        if (tempName == "BottomLeftCornerSpike")
        {
            tempEndPos.x = -sHeight / 18;
            tempEndPos.y = -sHeight / 18;

            endPos.x = -sHeight / 18;
            endPos.y = -sHeight / 18;
        }

        if (tempName == "BottomRightCornerSpike")
        {

            tempEndPos.x = Screen.width + sHeight / 18;
            tempEndPos.y = -sHeight / 18;

            endPos.x = Screen.width + sHeight / 18;
            endPos.y = -sHeight / 18;
        }

        if (tempName == "TopSpike")
        {

            tempEndPos.y = sHeight + sHeight / 12;

            endPos.y = sHeight + sHeight / 12;
        }

        if (tempName == "BottomSpike")
        {

            tempEndPos.y = -sHeight / 12;

            endPos.y = -sHeight / 12;
        }

        if (tempName == "RightSpike")
        {
            tempEndPos.x = sWidth + sHeight / 12;

            endPos.x = sWidth + sHeight / 12;
        }

        if (tempName == "LeftSpike")
        {
            tempEndPos.x = -sHeight / 12;

            endPos.x = -sHeight / 12;
        }
    }


    private IEnumerator MoveSpike(Vector3 startPos, float travelDuration)
    {

        while (canMove)
        {
            // First step, travel from A to B
            counter = 0f;

            while (counter < travelDuration)
            {

                isMovingToView = true;
                transform.position = Vector3.Lerp(startPos, endPos, counter / travelDuration);
                counter += Time.deltaTime;
                //Debug.Log("Counter 1: " + counter);
                yield return null;
            }

            // Make sure you're exactly at B, in case the counter 
            // wasn't precisely equal to travelDuration at the end
            transform.position = endPos;

            // Third step, travel back from B to A
            counter = 0f;
            while (counter < travelDuration)
            {

                isMovingToView = false;
                transform.position = Vector3.Lerp(endPos, startPos, counter / travelDuration);
                counter += Time.deltaTime;
                //Debug.Log("Counter 2: " + counter);
                yield return null;
            }

            transform.position = startPos;

            isWaiting = true;
            yield return new WaitForSeconds(1f);
            isWaiting = false;
        }
    }
}
