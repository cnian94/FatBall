using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{

    public GameObject player;

    SpriteRenderer bubbleRend;
    bool isBubbleEffectActive = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        bubbleRend = GetComponent<SpriteRenderer>();
        gameObject.transform.localScale = new Vector3(player.transform.localScale.x*1.90f, player.transform.localScale.y*1.95f, player.transform.localScale.z);
        transform.name = transform.name.Replace("(Clone)", "").Trim();
    }

    void Update()
    {
        transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z);
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
    /*void OnTriggerEnter2D(Collider2D col)
    {
        
        switch (col.gameObject.tag)
        {

            case "Spike":
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(2000, 1000));


                break;




        }
    }*/


}
