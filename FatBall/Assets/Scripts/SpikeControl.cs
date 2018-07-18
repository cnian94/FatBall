using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeControl : MonoBehaviour {
    Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();

        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        float height = GetComponent<SpriteRenderer>().bounds.size.y;


        float force = Random.Range(10f, 15f);
        if (rb.name == "TopSpike")
        {
            Debug.Log("WIDTH OF SPIKE: " + width);
            Debug.Log("HEIGHT OF SPIKE: " + height);
            //rb.AddForceAtPosition(new Vector2(0, force), rb.transform.position);
            rb.velocity = new Vector2(0, force);
        }

        if (rb.name == "LeftSpike")
        {
   
            //rb.AddForceAtPosition(new Vector2(-force, 0), rb.transform.position);
            rb.velocity = new Vector2(-force, 0);
        }

        if (rb.name == "BottomSpike")
        {
            //rb.AddForceAtPosition(new Vector2(0, -force), rb.transform.position);
            rb.velocity = new Vector2(0, -force);
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


        if (gameObject.name == "TopSpike" && gameObject.transform.position.y >= Screen.height + Screen.height / 40)
        {
            //rb.AddForceAtPosition(new Vector2(0, -force), rb.transform.position);
            rb.velocity = new Vector2(0, -force);

        }

        if (gameObject.name == "TopSpike" && gameObject.transform.position.y <= Screen.height - Screen.height / 40)
        {
            //rb.AddForceAtPosition(new Vector2(0, force), rb.transform.position);
            rb.velocity = new Vector2(0, force);
        }

        if (gameObject.name == "LeftSpike" && gameObject.transform.position.x <= -(Screen.width / 24))
        {
            //rb.AddForceAtPosition(new Vector2(force, 0), rb.transform.position);
            rb.velocity = new Vector2(force, 0);
        }

        if (gameObject.name == "LeftSpike" && gameObject.transform.position.x >= Screen.width / 24)
        {
            //rb.AddForceAtPosition(new Vector2(-force, 0), rb.transform.position);
            rb.velocity = new Vector2(-force, 0);
        }

        if (gameObject.name == "BottomSpike" && gameObject.transform.position.y <= -(Screen.height / 40))
        {
            //rb.AddForceAtPosition(new Vector2(0, force), rb.transform.position);
            rb.velocity = new Vector2(0, force);
        }

        if (gameObject.name == "BottomSpike" && gameObject.transform.position.y >= Screen.height / 40)
        {
            //rb.AddForceAtPosition(new Vector2(0, -force), rb.transform.position);
            rb.velocity = new Vector2(0, -force);
        }

        if (gameObject.name == "RightSpike" && gameObject.transform.position.x >= Screen.width + Screen.width / 24)
        {
            //rb.AddForceAtPosition(new Vector2(-5f, 0), rb.transform.position);
            rb.velocity = new Vector2(-force, 0);
        }

        if (gameObject.name == "RightSpike" && gameObject.transform.position.x <= Screen.width - Screen.width / 24)
        {
            //rb.AddForceAtPosition(new Vector2(5f, 0), rb.transform.position);
            rb.velocity = new Vector2(force, 0);
        }
    }

}
