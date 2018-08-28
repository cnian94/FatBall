using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class TimerScript : MonoBehaviour
{

    public GameObject time_btn_obj;
    public Text time_text;
    public Text pausedTimeText;
    public Sprite time_end_sprite;
    private float startTime;
    private bool finished = false;
    private bool isPaused = false;

    private float t;


    void Start()
    {
        startTime = Time.timeSinceLevelLoad;
    }

    void Update()
    {
        if (finished)
        {
            startTime = 0f;
        }

        if (!finished && !isPaused && Time.timeSinceLevelLoad >= 3)
        {
            t = Time.timeSinceLevelLoad - startTime;

            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2");

            time_text.text = minutes + ":" + seconds;
            pausedTimeText.text = time_text.text;
            //Debug.Log(minutes + ":" + seconds);
        }
    }

    public void Finish()
    {
        finished = true;
    }

    public void SetIsPaused(bool val)
    {
        isPaused = val;
    }


    public bool GetIsPaused()
    {
        return isPaused;
    }

}
