using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCtrl : MonoBehaviour
{

    // Singleton instance.
    public static MenuCtrl Instance = null;


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
            NetworkController.Instance.StartCoroutine(NetworkController.Instance.GetLeaderBoard());
        }

        if (sceneName.Equals("OptionsScene"))
        {
            SceneManager.LoadScene(sceneName);
        }

        if (sceneName.Equals("MenuScene"))
        {
            //SoundManager.Instance.MusicSource.Play();
            SceneManager.LoadScene(sceneName);

        }
    }


    public void muteSound()
    {
        if (!SoundManager.Instance.isMuted)
        {
            SoundManager.Instance.isMuted = !SoundManager.Instance.isMuted;
            soundButton.GetComponent<Image>().sprite = soundOffSprite;
            //SoundManager.Instance.EffectsSource.mute = true;
            SoundManager.Instance.MusicSource.mute = true;
        }
        else
        {
            //soundManager.SetActive(true);
            //SoundManager.Instance.EffectsSource.mute = false;
            SoundManager.Instance.MusicSource.mute = false;
            SoundManager.Instance.isMuted = !SoundManager.Instance.isMuted;
            soundButton.GetComponent<Image>().sprite = soundOnSprite;
            //SoundManager.Instance.PlayMusic("GameSound");
        }
    }


    void Awake()
    {
        // If there is not already an instance of MenuCtrl, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
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
    }
}