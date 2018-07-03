using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class TimerScript : MonoBehaviour {

    public GameObject time_btn_obj;
    public Text time_text;
    public Sprite time_end_sprite;
    private float startTime;
    private bool finished = false;


    void Start () {
        startTime = Time.timeSinceLevelLoad;
    }

    void Update()
    {
        if (finished)
        {
            startTime = 0f;
        }

        if (!finished && Time.timeSinceLevelLoad >= 3)
        {
            float t = Time.timeSinceLevelLoad - startTime;

            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2");

            time_text.text = minutes + ":" + seconds;
        }
    }

    public void Finish()
    {
        finished = true;
        time_btn_obj.GetComponent<Image>().sprite = time_end_sprite;
    }

}
