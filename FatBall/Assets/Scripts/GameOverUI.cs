using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    private void Awake()
    {
        SoundManager.Instance.PlayMusic("GameSound");
    }

    public void Quit()
    {
        Debug.Log("APPLICATION QUIT !!");
        Application.Quit();
    }

    public void retry()
    {
        SoundManager.Instance.MusicSource.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SoundManager.Instance.Play("Button");
    }

    public void OpenMenu()
    {
        SoundManager.Instance.MusicSource.Stop();
        SceneManager.LoadScene(0);
        SoundManager.Instance.Play("Button");
    }

    public void OpenLeadersBoard()
    {
        SoundManager.Instance.Play("Button");
        MenuCtrl.Instance.loadScene("LeaderBoardScene");
    }
}
