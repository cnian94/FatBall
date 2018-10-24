using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;



public class TimerScript : MonoBehaviour
{

    public GameObject time_btn_obj;
    public Text time_text;
    public Sprite time_end_sprite;
    private float startTime;
    private bool finished = false;

    public string result;

    public float t;

    public string minutes;
    public string seconds;
    private Color textcolor;


    void Start()
    {
        startTime = Time.timeSinceLevelLoad;
        GameMaster.gm.FinishEvent.AddListener(Finish);
    }

    void Update()
    {

        if (!finished  && Time.timeSinceLevelLoad >= 3)
        {
            t = Time.timeSinceLevelLoad - startTime;

            //minutes = ((int)t / 60).ToString();
            //seconds = (t % 60).ToString("f2");

            //time_text.text = minutes + ":" + seconds;
        }
    }

    public void Finish()
    {
        finished = true;
        Color charcolor = GameMaster.gm.charColor;
        charcolor.a = 0;
        textcolor = new Color (1, 1, 1, 1) - charcolor;
        GameMaster.gm.IngameResult.gameObject.SetActive(false);
        time_text.text = result;
        time_text.color = textcolor;
        GameMaster.gm.ResultPanel.GetComponent<Image>().color = GameMaster.gm.charColor;
        GameMaster.gm.gameOverUI.gameObject.SetActive(true);
        //Debug.Log("Finish" + result);
        startTime = 0f;
        if (NetworkManager.instance.PlayCounter == NetworkManager.instance.RandomAdLimit)
        {
            //AdsManager.instance.ShowRandomdAd();
            GameMaster.gm.ChartBoost.GetComponent<CharBoostManager>().ShowVideo("Game Over");
            NetworkManager.instance.PlayCounter = 0;
            NetworkManager.instance.RandomAdLimit = Random.Range(2, 5);
        }

    }

}
