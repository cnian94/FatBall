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
    }

    public void OpenMenu()
    {
        SoundManager.Instance.MusicSource.Stop();
        SceneManager.LoadScene(0);
    }

    public void OpenLeadersBoard()
    {
        SceneManager.LoadScene(2);
    }
}
