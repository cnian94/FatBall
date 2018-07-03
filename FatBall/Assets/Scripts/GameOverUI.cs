﻿using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    private void Awake()
    {
    }

    public void Quit()
    {
        Debug.Log("APPLICATION QUIT !!");
        Application.Quit();
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
