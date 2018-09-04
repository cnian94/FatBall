using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;


public class LeaderboardController : MonoBehaviour
{

    //public GameObject SoundManager;
    public GameObject leaderBoardPanel;
    public GameObject leaderBoardContent;

    public Button prefabBtn;
    private Button playerBtn;


    public string me = "Tilda";
    private Vector2 scrollTo;

    private string[] playerz =
{
        "Shanda",
         "Mae",
         "Eneida",
        "Felecia",
        "Zachariah",
              "Filomena",
         "Leopoldo",
        "Monnie",
           "Venice",
      "Sheila",
       "Beau",
        "Sean",
         "Paul",
      "Reiko",
        "Arlene",
"Sang",
"Wilton",
"Somer",
"Lavina",
"Tilda",
"Donte",
"Lashaunda",
"Oneida",
"Nada",
"Micki",
"Belkis",
"Sunni",
"Royce",
"Janey",
"Adell",
"Kelli",
"Kary",
"Lauralee",
"Tawnya",
"Odilia",
"Jacquelynn",
"Bo",
"Melonie",
"Fabian",
"Sheri",
"Boyce",
"Merideth",
"Breanne",
"Rosalva",
"Marita",
"Edra",
"Scottie",
"Shemeka",
"Mitsue",
"Marilee"
};


    private Dictionary<string, double> players2 = new Dictionary<string, double>();


    // Use this for initialization
    void Start()
    {
        CreateRandomLeaderboard();
        //Debug.Log(leaderBoardPanel.GetComponent<ScrollRect>().normalizedPosition);
        //SoundManager.GetComponent<SoundManagerScript>().PlaySound("GameSound");

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
        for (int i = 0; i < playerz.Length; i++)
        {
            int randomScore = Random.Range(0, 460);
            players2.Add(playerz[i], randomScore);
        }

        SortLeaderboard();
        StartCoroutine(ScrollToMyScore(0.5f));
    }

    void SortLeaderboard()
    {

        var myList = players2.ToList();
        
        myList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));


        foreach (KeyValuePair<string, double> player in myList)
        {

            playerBtn = Instantiate(prefabBtn, leaderBoardContent.transform);
            playerBtn.GetComponentInChildren<Text>().text = player.Key + "                    " + player.Value;
            playerBtn.name = player.Key;
        }

        FindMe(myList);

    }

    void FindMe(List<KeyValuePair<string, double>> players) 
    {
        var index = 0;
        foreach (KeyValuePair<string, double> player in players)
        {
            if(player.Key.Equals(me))
            {
                index = players.IndexOf(player);
                float ratio = (1f / players.ToArray().Length);
                scrollTo.y = 1 - ratio * index;
                Button me = GameObject.Find(player.Key).GetComponent<Button>();
                me.GetComponent<Image>().color = Color.grey;
            }
        }

    }

    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        SoundManager.Instance.Play("Button");
    }

    private void Update()
    {
        //Debug.Log(leaderBoardPanel.GetComponent<ScrollRect>().normalizedPosition);
    }


}
