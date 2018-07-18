using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class BubbleController : MonoBehaviour {

    Rigidbody2D myBody;
    public GameObject player;

    SpriteRenderer bubbleRend;
    bool isBubbleEffectActive = false;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        bubbleRend = GetComponent<SpriteRenderer> ();
        gameObject.transform.localScale = new Vector3(player.transform.localScale.x, player.transform.localScale.y , player.transform.localScale.z);
        transform.name = transform.name.Replace("(Clone)", "").Trim();
        myBody = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z);
        Debug.Log("isBubbleEffectActive: " + isBubbleEffectActive);
        if (isBubbleEffectActive)
        {
            Invoke("StartBubbleEffect", 5f);
        }
    }

    void SetIsBubbleEffectActive(bool val)
    {
        if (val)
        {
            isBubbleEffectActive = val;
        }
        else
        {
            isBubbleEffectActive = val;
            Color oldColor = bubbleRend.material.color;
            oldColor.a = 1f;
            bubbleRend.material.color = oldColor;
        }
      
    }

    public void StartBubbleEffect()
    {
        float alpha = 0f;
        float lerp = Mathf.PingPong(Time.time, 0.5f) / 0.5f;

        alpha = Mathf.Lerp(0f, 1f, lerp);
        Color oldColor = bubbleRend.material.color;
        oldColor.a = alpha;
        bubbleRend.material.color = oldColor;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.name)
        {
            case "Enemy":
                //Rigidbody2D enemyRB = col.gameObject.GetComponent<Rigidbody2D>();
                //Vector3 force = -enemyRB.velocity.normalized * 50000;
                //enemyRB.AddForce(force);

                //Vector3 point = col.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(transform.position);
                //Vector3 force = -point.normalized * 5000;
                //enemyRB.AddForce(force);
                break;

            case "TopSpike":

                player.GetComponent<Rigidbody2D>().AddForce(-col.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(transform.position).normalized * 5000);
                break;




        }
    }
}
