using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkController : MonoBehaviour
{

    // Singleton instance.
    public static NetworkController Instance = null;

    public GameObject notMemberPanel;
    public GameObject memberPanel;
    public GameObject connectionPanel;

    public Text PlayerCoinText;

    public Text nickname;
    private string device_id;
    public Text takenText;

    public LeaderBoardList leaderboard;

    public PlayerModel playerModel;
    public InventoryList inventoryList;


    private string REGISTER_URL = "https://fatball.herokuapp.com/api/register";
    private string CHECK_URL = "https://fatball.herokuapp.com/api/check";
    private string PLAYER_URL = "https://fatball.herokuapp.com/api/player";
    private string LEADERBOARD_URL = "https://fatball.herokuapp.com/api/leaderboard";
    private string INVENTORY_URL = "https://fatball.herokuapp.com/api/inventory?id=";

    public int RandomAdLimit ;
    public int PlayCounter;



    // Initialize the singleton instance.
    private void Awake()
    {
        //Debug.Log("NETWORK AWAKE !!");

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //Debug.Log("Error. Check internet connection!");
            connectionPanel.SetActive(true);
        }

        else
        {
            PlayCounter = 0 ;
            RandomAdLimit = Random.Range(2, 5);
            //PlayerPrefs.DeleteAll();
            //Check if instance already exists
            if (Instance == null)
            {
                //if not, set instance to this
                Instance = this;
                Check();
            }

            //If instance already exists and it's not this:
            else if (Instance != this)
            {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
                Check();
            }

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Check()
    {
        //this.playerModel = new PlayerModel("3323e9048b337f17b71d49e4ac5925e951ada236", "cano", 197, 3747);
        //PlayerPrefs.SetString("player",JsonUtility.ToJson(playerModel));
        Debug.Log("PLAYER PREFS: " + PlayerPrefs.GetString("player"));

        if (PlayerPrefs.GetString("player") == null || PlayerPrefs.GetString("player").Equals(""))
        {
            memberPanel.SetActive(false);
            notMemberPanel.SetActive(true); //burayı kapat
            //memberPanel.SetActive(true); //bunu yaz
        }

        else
        {
            playerModel = JsonUtility.FromJson<PlayerModel>(PlayerPrefs.GetString("player"));
            Debug.Log("PLAYER MODEL NOT NULL !!");
            Instance.StartCoroutine(CheckDeviceIsRegistered());
        }
    }


    IEnumerator Register()
    {
        string json = JsonUtility.ToJson(playerModel);
        var request = new UnityWebRequest(REGISTER_URL, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Erro: " + request.error);
        }
        else
        {
           
            TakenModel taken = JsonUtility.FromJson<TakenModel>(request.downloadHandler.text);
            Debug.Log("TAKEN MODEL:" + taken.taken);

            if (taken.taken)
            {
                Debug.Log("NICKNAME ALREADY TAKEN !!");
                takenText.text = "c'mon, be creative !!";
            }
            else
            {
                Debug.Log("NICKNAME NOT TAKEN !!");
                //Debug.Log("RESPONSE:"+ request.downloadHandler.text);
                //Debug.Log("PLAYER RESPONSE:" + JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text).ToString());
                PlayerPrefs.SetString("player", request.downloadHandler.text);
                PlayerPrefs.SetInt("selectedChar", 0);
                playerModel = JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text);
                StartCoroutine(GetInventory());
                notMemberPanel.SetActive(false);
                memberPanel.SetActive(true);
                PlayerCoinText.text = playerModel.coins.ToString();
            }
        }
    }

    public void GetIn()
    {
        device_id = SystemInfo.deviceUniqueIdentifier;
        playerModel = new PlayerModel(device_id, nickname.text);
        StartCoroutine(Register());
    }



    IEnumerator CheckDeviceIsRegistered()
    {

        string json = JsonUtility.ToJson(playerModel);
        var request = new UnityWebRequest(CHECK_URL, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Erro: " + request.error);
        }
        else
        {
            //Debug.Log("DEVICE IS ALREADY REGISTERED: " + request.responseCode);
            PlayerPrefs.SetString("player", request.downloadHandler.text);

            if (PlayerPrefs.GetInt("selectedChar") == 0)
            {
                PlayerPrefs.SetInt("selectedChar", 0);
            }

            playerModel = JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text);

            notMemberPanel.SetActive(false);
            memberPanel.SetActive(true);
            //Debug.Log("COINS: " + playerModel.coins.ToString());
            PlayerCoinText.text = playerModel.coins.ToString();
            Instance.StartCoroutine(GetInventory());
        }
    }

    public IEnumerator GetLeaderBoard()
    {
        var request = new UnityWebRequest(LEADERBOARD_URL, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        yield return StartCoroutine(WaitForLeaderboard(request));
    }


    private IEnumerator WaitForLeaderboard(UnityWebRequest request)
    {
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Erro: " + request.error);
        }
        else
        {
            leaderboard = LeaderBoardList.CreateFromJSON(request.downloadHandler.text);
            Debug.Log("LEADERBOARD:" + leaderboard.players.Length);
            SceneManager.LoadScene("LeaderBoardScene");
        }
    }




    public IEnumerator GetInventory()
    {
        //string json = JsonUtility.ToJson(playerModel);
        var request = new UnityWebRequest(INVENTORY_URL + playerModel.device_id, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        yield return Instance.StartCoroutine(WaitForInventory(request));
    }


    private IEnumerator WaitForInventory(UnityWebRequest request)
    {
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Erro: " + request.error);
        }
        else
        {
            inventoryList = InventoryList.CreateFromJSON(request.downloadHandler.text);
            if (SceneManager.GetActiveScene().name == "OptionsScene")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }


    public IEnumerator UnlockMonster(int char_id)
    {
        UnlockModel unlockModel = new UnlockModel(playerModel.device_id, char_id);
        string json = JsonUtility.ToJson(unlockModel);
        Debug.Log("JSON:" + json);
        var request = new UnityWebRequest(INVENTORY_URL, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Erro: " + request.error);
        }
        else
        {
            PlayerPrefs.SetString("player", request.downloadHandler.text);
            playerModel = JsonUtility.FromJson<PlayerModel>(PlayerPrefs.GetString("player"));
            StartCoroutine(GetInventory());
        }
    }

    public IEnumerator SetHighScore(bool doubled = false)
    {
        string json = JsonUtility.ToJson(playerModel);
        //Debug.Log("JSON:" + json);
        var request = new UnityWebRequest(PLAYER_URL, "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Erro: " + request.error);
        }
        else
        {
            if (!doubled)
            {
                Debug.Log("RESPONSE: " + request.downloadHandler.text);
                PlayerPrefs.SetString("player", request.downloadHandler.text);
                playerModel = JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text);
                GameMaster.gm.gameOverUI.SetActive(true);
            }

            else
            {
                PlayerPrefs.SetString("player", request.downloadHandler.text);
                playerModel = JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text);
                SceneManager.LoadScene(0);
            }
        }
    }




    // Use this for initialization
    void Start()
    {
        //Debug.Log("NETWORK START !!");
    }

}

