using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeControl : MonoBehaviour
{
    //Rigidbody2D rb;

    //float height;
    float sWidth;
    float sHeight;

    public bool isExtending;

    public float travelDuration;

    public Vector3 tempEndPos;
    public Vector3 endPos;

    public Vector3 startPos;

    public bool isMovingToView = false;

    public GameObject Player;

    Vector3 tempScale;


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
        isExtending = false;
        tempEndPos = transform.position;
        endPos = transform.position;
        travelDuration = Random.Range(1f, 3f);
        Debug.Log(gameObject.GetComponent<SpriteRenderer>().bounds.size.x + "--" + gameObject.GetComponent<SpriteRenderer>().bounds.size.y);
        
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
        
        //StartCoroutine(MoveSpike(startPos, travelDuration));

        


        /*float force = Random.Range(30f, 50f);

        if (rb.name == "TopLeftCornerSpike") //Sol üste köşe spike için uygulanan force. Hem x hem de y de hareket edeceği için 2 yönede force var. Hareketin çapraz olmasını aşağıda move kısmında tanımladık diye düşünüyorum.
        {
            rb.velocity = new Vector2(force, -force);
        }

        if (rb.name == "TopSpike")  //Top spikelar sadece yukarı aşağıya hareket ettiği için sadece y de force var. Böyle devam...
        {
            rb.velocity = new Vector2(0, force);
        }

        if (rb.name == "TopRightCornerSpike")
        {
            rb.velocity = new Vector2(-force, -force);
        }


        if (rb.name == "LeftSpike")
        {

            rb.velocity = new Vector2(-force, 0);
        }

        if (rb.name == "BottomRightCornerSpike")
        {
            rb.velocity = new Vector2(-force, force);
        }


        if (rb.name == "BottomSpike")
        {
            rb.velocity = new Vector2(0, -force);
        }

        if (rb.name == "BottomLeftCornerSpike")
        {
            rb.velocity = new Vector2(force, force);
        }

        if (rb.name == "RightSpike")
        {
            rb.velocity = new Vector2(force, 0);
        }*/


    }

    void FixedUpdate()
    {
        //float force = Random.Range(30f, 50f);


        /*if (gameObject.name == "TopLeftCornerSpike" && gameObject.transform.position.y >= Screen.height + height / 3.5)
        {
            rb.velocity = new Vector2(force, -force);
        }

        if (gameObject.name == "TopLeftCornerSpike" && gameObject.transform.position.y <= Screen.height - height / 6.5)
        {
            rb.velocity = new Vector2(-force, force);
        }

        if (gameObject.name == "TopRightCornerSpike" && gameObject.transform.position.y >= Screen.height + height / 3.5)
        {
            rb.velocity = new Vector2(-force, -force);
        }

        if (gameObject.name == "TopRightCornerSpike" && gameObject.transform.position.y <= Screen.height - height / 6.5)
        {
            rb.velocity = new Vector2(force, force);
        }


        if (gameObject.name == "TopSpike" && gameObject.transform.position.y >= Screen.height + height / 2) //screen height + kendi heightının yarısından yukarı veya eşit olunca aşağıya doğru force başlar.
        {
            rb.velocity = new Vector2(0, -force);

        }

        if (gameObject.name == "TopSpike" && gameObject.transform.position.y <= Screen.height - height / 3.5) //screen height - kendi heightının çeyreğinden yukarı veya eşit olunca yukarıya doğru force başlar.
        {
            rb.velocity = new Vector2(0, force);
        }

        if (gameObject.name == "LeftSpike" && gameObject.transform.position.x <= -height) // heee zaten solda sıfır noktasında o yüzden direk height. Ama height acaba burda spike ın eni mi oluyor. Öyle olması gerek çünkü az çok spike widht height böyle oluyor. Ağırlık merkezinden spawn ediyor.
        {
            rb.velocity = new Vector2(force, 0);
        }

        if (gameObject.name == "LeftSpike" && gameObject.transform.position.x >=  height / 3)
        {
            rb.velocity = new Vector2(-force, 0);
        }

        if (gameObject.name == "BottomRightCornerSpike" && gameObject.transform.position.y <= -height / 3.5)
        {
            rb.velocity = new Vector2(-force, force);
        }

        if (gameObject.name == "BottomRightCornerSpike" && gameObject.transform.position.y >= height / 6.5)
        {
            rb.velocity = new Vector2(force, -force);
        }


        if (gameObject.name == "BottomLeftCornerSpike" && gameObject.transform.position.y <= -height / 3.5)
        {
            rb.velocity = new Vector2(force, force);
        }

        if (gameObject.name == "BottomLeftCornerSpike" && gameObject.transform.position.y >= height / 6.5)
        {
            rb.velocity = new Vector2(-force, -force);
        }

        if (gameObject.name == "BottomSpike" && gameObject.transform.position.y <=  -height / 2)
        {
            rb.velocity = new Vector2(0, force);
        }

        if (gameObject.name == "BottomSpike" && gameObject.transform.position.y >= height / 3.5)
        {
            rb.velocity = new Vector2(0, -force);
        }

        if (gameObject.name == "RightSpike" && gameObject.transform.position.x >= Screen.width + height)
        {
            rb.velocity = new Vector2(-force, 0);
        }

        if (gameObject.name == "RightSpike" && gameObject.transform.position.x <= Screen.width - height / 3)
        {
            rb.velocity = new Vector2(force, 0);
        }*/



        /*if (gameObject.name == "TopLeftCornerSpike" && gameObject.transform.position.y >= sHeight + sHeight / 10.5)
        {
            rb.velocity = new Vector2(force, -force);
        }

        if (gameObject.name == "TopLeftCornerSpike" && gameObject.transform.position.y <= sHeight + sHeight / 18)
        {
            rb.velocity = new Vector2(-force, force);
        }

        if (gameObject.name == "TopRightCornerSpike" && gameObject.transform.position.y >= sHeight + sHeight / 10.5)
        {
            rb.velocity = new Vector2(-force, -force);
        }

        if (gameObject.name == "TopRightCornerSpike" && gameObject.transform.position.y <= sHeight + sHeight / 18)
        {
            rb.velocity = new Vector2(force, force);
        }


        if (gameObject.name == "TopSpike" && gameObject.transform.position.y >= sHeight + sHeight / 7.5) //screen height + kendi heightının yarısından yukarı veya eşit olunca aşağıya doğru force başlar.
        {
            rb.velocity = new Vector2(0, -force);

        }

        if (gameObject.name == "TopSpike" && gameObject.transform.position.y <= sHeight + sHeight / 12) //screen height - kendi heightının çeyreğinden yukarı veya eşit olunca yukarıya doğru force başlar.
        {
            rb.velocity = new Vector2(0, force);
        }

        if (gameObject.name == "LeftSpike" && gameObject.transform.position.x <= -sHeight / 7.325) // heee zaten solda sıfır noktasında o yüzden direk height. Ama height acaba burda spike ın eni mi oluyor. Öyle olması gerek çünkü az çok spike widht height böyle oluyor. Ağırlık merkezinden spawn ediyor.
        {
            rb.velocity = new Vector2(force, 0);
        }

        if (gameObject.name == "LeftSpike" && gameObject.transform.position.x >= -sHeight / 12)
        {
            rb.velocity = new Vector2(-force, 0);
        }

        if (gameObject.name == "BottomRightCornerSpike" && gameObject.transform.position.y <= -sHeight / 10.5)
        {
            rb.velocity = new Vector2(-force, force);
        }

        if (gameObject.name == "BottomRightCornerSpike" && gameObject.transform.position.y >= -sHeight / 18)
        {
            rb.velocity = new Vector2(force, -force);
        }


        if (gameObject.name == "BottomLeftCornerSpike" && gameObject.transform.position.y <= -sHeight / 10.5)
        {
            rb.velocity = new Vector2(force, force);
        }

        if (gameObject.name == "BottomLeftCornerSpike" && gameObject.transform.position.y >= -sHeight / 18)
        {
            rb.velocity = new Vector2(-force, -force);
        }

        if (gameObject.name == "BottomSpike" && gameObject.transform.position.y <= -sHeight / 7.5)
        {
            rb.velocity = new Vector2(0, force);
        }

        if (gameObject.name == "BottomSpike" && gameObject.transform.position.y >= -sHeight / 12)
        {
            rb.velocity = new Vector2(0, -force);
        }

        if (gameObject.name == "RightSpike" && gameObject.transform.position.x >= sWidth + sHeight / 7.325)
        {
            rb.velocity = new Vector2(-force, 0);
        }

        if (gameObject.name == "RightSpike" && gameObject.transform.position.x <= sWidth + sHeight / 12)
        {
            rb.velocity = new Vector2(force, 0);
        }*/

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

        while (!isExtending)
        {
            // First step, travel from A to B
            float counter = 0f;
            while (counter < travelDuration)
            {
                isMovingToView = true;
                transform.position = Vector3.Lerp(startPos, endPos, counter / travelDuration);
                counter += Time.deltaTime;
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
                yield return null;
            }

            transform.position = startPos;
        }
    }
    /*void OnTriggerEnter2D(Collider2D col)
    {
        
        switch (col.gameObject.tag)
        {

            case "Player":
                Player = GameObject.FindGameObjectWithTag("Player");
                Player.GetComponent<Rigidbody2D>().AddForce(new Vector2(2000, 1000));


                break;




        }
    }*/

}
