using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeControl : MonoBehaviour {
    Rigidbody2D rb;
    float width;
    float height;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
        //Debug.Log("CENTER OF MASS: " + rb.position);

        //Debug.Log("SPIKE SIZE 0: " + width + " - " + height);


        float force = Random.Range(10f, 15f);

        if(rb.name == "TopLeftCornerSpike")
        {
            rb.velocity = new Vector2(force, -force);
        }

        if (rb.name == "TopSpike")
        {
            rb.velocity = new Vector2(0, force);
        }

        if (rb.name == "TopRightCornerSpike")
        {
            rb.velocity = new Vector2(-force, -force);
        }


        if (rb.name == "LeftSpike")
        {
   
            //rb.AddForceAtPosition(new Vector2(-force, 0), rb.transform.position);
            rb.velocity = new Vector2(-force, 0);
        }

        if (rb.name == "BottomRightCornerSpike")
        {
            rb.velocity = new Vector2(-force, force);
        }
        

        if (rb.name == "BottomSpike")
        {
            //rb.AddForceAtPosition(new Vector2(0, -force), rb.transform.position);
            rb.velocity = new Vector2(0, -force);
        }

        if (rb.name == "BottomLeftCornerSpike")
        {
            rb.velocity = new Vector2(force, force);
        }

        if (rb.name == "RightSpike")
        {
  
            //rb.AddForceAtPosition(new Vector2(force, 0), rb.transform.position);
            rb.velocity = new Vector2(force, 0);
        }
    }

    void FixedUpdate()
    {
        float force = Random.Range(10f, 15f);


        if (gameObject.name == "TopLeftCornerSpike" && gameObject.transform.position.y >= Screen.height + height / 3.5)
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


        if (gameObject.name == "TopSpike" && gameObject.transform.position.y >= Screen.height + height / 2)
        {
            rb.velocity = new Vector2(0, -force);

        }

        if (gameObject.name == "TopSpike" && gameObject.transform.position.y <= Screen.height - height / 4)
        {
            rb.velocity = new Vector2(0, force);
        }

        if (gameObject.name == "LeftSpike" && gameObject.transform.position.x <= -height)
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

        if (gameObject.name == "BottomSpike" && gameObject.transform.position.y >= height / 4)
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
        }
    }

}
