using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;


public class LeaderboardController : MonoBehaviour
{

    public GameObject leaderBoardPanel;
    public GameObject leaderBoardContent;

    public Button prefabBtn;
    private Button playerBtn;

    public GameObject PlayerPanelPrefab;
    private GameObject PlayerPanel;

    private Vector2 scrollTo;


    // Use this for initialization
    void Start()
    {
        CreateRandomLeaderboard();
        StartCoroutine(ScrollToMyScore(0.5f));
    }

    IEnumerator ScrollToMyScore(float time)
    {
        Vector2 originalPos = new Vector2(0.0f, 0.1f);
        Vector2 targetPos = scrollTo;
        float originalTime = time;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            leaderBoardPanel.GetComponent<ScrollRect>().normalizedPosition = Vector2.Lerp(targetPos, originalPos, time / originalTime);
            yield return null;

        }
    }

    void CreateRandomLeaderboard()
    {
        PlayerModel[] players = NetworkManager.instance.leaderboard.players;

        for (int i=0; i < players.Length; i++)
        {
            PlayerPanel = Instantiate(PlayerPanelPrefab, leaderBoardContent.transform);
            PlayerPanel.transform.GetChild(0).GetComponent<Text>().text = (i+1) + ". " + players[i].nickname;
            PlayerPanel.transform.GetChild(1).GetComponent<Text>().text = players[i].highscore.ToString();
            PlayerPanel.name = players[i].nickname;

            if (players[i].device_id == NetworkManager.instance.playerModel.device_id)
            {
                float ratio = (1f / players.ToArray().Length);
                scrollTo.y = 1 - ratio * i;
                PlayerPanel.transform.GetChild(0).GetComponent<Text>().color = Color.red;
                PlayerPanel.transform.GetChild(1).GetComponent<Text>().color = Color.red;

                //Text me = GameObject.Find(players[i].nickname).GetComponent<Text>();
                //me.GetComponent<Text>().color = Color.red;
            }
        }

    }

    public void loadScene(string sceneName)
    {
        //SoundManager.instance.MusicSource.Pause();
        MenuCtrl.instance.loadScene(sceneName);
    }

    private void Update()
    {
    }


}
