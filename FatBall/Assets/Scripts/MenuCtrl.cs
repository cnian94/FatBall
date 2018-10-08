using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCtrl : MonoBehaviour
{

    // Singleton instance.
    public static MenuCtrl instance = null;


    public Button soundButton;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;



    public void loadScene(string sceneName)
    {
        SoundManager.Instance.Play("Button");
        if (sceneName.Equals("MainScene"))
        {
            SoundManager.Instance.MusicSource.Stop();
            SceneManager.LoadScene(sceneName);
        }

        if (sceneName.Equals("LeaderBoardScene"))
        {
            NetworkManager.instance.StartCoroutine(NetworkManager.instance.GetLeaderBoard());
        }

        if (sceneName.Equals("OptionsScene"))
        {
            SceneManager.LoadScene(sceneName);
        }

        if (sceneName.Equals("MenuScene"))
        {
            //SoundManager.instance.MusicSource.Play();
            SceneManager.LoadScene(sceneName);

        }
    }


    public void muteSound()
    {
        if (!SoundManager.Instance.isMuted)
        {
            SoundManager.Instance.isMuted = !SoundManager.Instance.isMuted;
            soundButton.GetComponent<Image>().sprite = soundOffSprite;
            //SoundManager.instance.EffectsSource.mute = true;
            SoundManager.Instance.MusicSource.mute = true;
        }
        else
        {
            //soundManager.SetActive(true);
            //SoundManager.instance.EffectsSource.mute = false;
            SoundManager.Instance.MusicSource.mute = false;
            SoundManager.Instance.isMuted = !SoundManager.Instance.isMuted;
            soundButton.GetComponent<Image>().sprite = soundOnSprite;
            //SoundManager.instance.PlayMusic("GameSound");
        }
    }


    void Awake()
    {
        // If there is not already an instance of MenuCtrl, set it to this.
        if (instance == null)
        {
            instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //Set MenuCtrl to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        //DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        if (!SoundManager.Instance.MusicSource.isPlaying)
        {
            SoundManager.Instance.PlayMusic("GameSound");
        }
        if (SoundManager.Instance.MusicSource.mute == true)
        {
            soundButton.GetComponent<Image>().sprite = soundOffSprite;
        }
    }

}