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


    public string me = "Tilda";
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
        PlayerModel[] players = NetworkController.Instance.leaderboard.players;

        for (int i=0; i < players.Length; i++)
        {
            playerBtn = Instantiate(prefabBtn, leaderBoardContent.transform);
            playerBtn.GetComponentInChildren<Text>().text =  players[i].nickname + "                    " + players[i].highscore;
            playerBtn.name = players[i].nickname;

            if (players[i].device_id == NetworkController.Instance.playerModel.device_id)
            {
                float ratio = (1f / players.ToArray().Length);
                scrollTo.y = 1 - ratio * i;
                Button me = GameObject.Find(players[i].nickname).GetComponent<Button>();
                me.GetComponent<Image>().color = Color.grey;
            }
        }

    }

    public void loadScene(string sceneName)
    {
        //SoundManager.Instance.MusicSource.Pause();
        MenuCtrl.Instance.loadScene(sceneName);
    }

    private void Update()
    {
    }


}
