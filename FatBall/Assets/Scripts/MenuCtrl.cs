using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCtrl : MonoBehaviour
{

    public GameObject soundManager;

    public Button soundButton;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    //private SoundManagerScript soundManager;
    public static MenuCtrl mc;



    public void loadScene(string sceneName)
    {
        if (sceneName.Equals("MainScene"))
        {
            SoundManager.Instance.MusicSource.Stop();
        }
        SceneManager.LoadScene(sceneName);
    }


    public void muteSound()
    {
        if (!SoundManager.Instance.isMuted)
        {
            SoundManager.Instance.isMuted = !SoundManager.Instance.isMuted;
            soundButton.GetComponent<Image>().sprite = soundOffSprite;
            soundManager.SetActive(false);
        }
        else
        {
            soundManager.SetActive(true);
            SoundManager.Instance.isMuted = !SoundManager.Instance.isMuted;
            soundButton.GetComponent<Image>().sprite = soundOnSprite;
            SoundManager.Instance.PlayMusic("GameSound");


        }
    }


    void Awake()
    {
        //soundManager = FindObjectOfType<SoundManagerScript>();
    }


    void Start()
    {
        SoundManager.Instance.PlayMusic("GameSound");
    }
}