using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCtrl : MonoBehaviour
{

    private SoundManagerScript soundManager;
    public static MenuCtrl mc;


    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void muteSound()
    {
        if (soundManager.GetComponent<SoundManagerScript>().isMuted)
        {
            soundManager.GetComponent<SoundManagerScript>().isMuted = !soundManager.GetComponent<SoundManagerScript>().isMuted;
            soundManager.GetComponent<SoundManagerScript>().audioSrc.Stop();
        }
        else
        {
            soundManager.GetComponent<SoundManagerScript>().isMuted = !soundManager.GetComponent<SoundManagerScript>().isMuted;
            soundManager.GetComponent<SoundManagerScript>().audioSrc.Play();
        }
    }


    void Awake()
    {
        soundManager = FindObjectOfType<SoundManagerScript>();
    }


    void Start()
    {
      soundManager.PlaySound("GameSound");

    }
}