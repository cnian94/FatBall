using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

public class NetworkManager : MonoBehaviour
{

    /// <summary>
    /// _instance of the MyClass class.
    /// </summary>
    /// <remarks>
    /// Please use this for all calls to this class.
    /// </remarks>
    public static NetworkManager instance
    {
        get { return _instance ?? (_instance = new GameObject("NetworkManager").AddComponent<NetworkManager>()); }
    }

    private static NetworkManager _instance;

    //public static NetworkManager _instance = null;

    //public GameObject ProgressBar;

    private AsyncOperation asyncLoad;

    public string device_id;
    public LeaderBoardList leaderboard;

    public PlayerModel playerModel;
    public InventoryList inventoryList;

    public GameObject ProgressBar;


    private string REGISTER_URL = "https://fatball.herokuapp.com/api/register";
    private string CHECK_URL = "https://fatball.herokuapp.com/api/check";
    private string PLAYER_URL = "https://fatball.herokuapp.com/api/player";
    private string LEADERBOARD_URL = "https://fatball.herokuapp.com/api/leaderboard";
    private string COINBOARD_URL = "https://fatball.herokuapp.com/api/coinboard";
    private string INVENTORY_URL = "https://fatball.herokuapp.com/api/inventory?id=";

    public int RandomAdLimit;
    public int PlayCounter;

    [System.Serializable]
    public class RegisterEvent : UnityEngine.Events.UnityEvent<bool> { }
    public RegisterEvent registerEvent;

    public UnityEvent notMemberEvent;

    public UnityEvent inventoryFetchedEvent;

    public UnityEvent notificationEvent;
    //public bool isNotification = false;

    public UnityEvent coinBoardFetched;

    [System.Serializable]
    public class WinnerCredsEvent : UnityEngine.Events.UnityEvent<int> { }
    public WinnerCredsEvent winnerCredsEvent;

    public bool inventoryNeeded = true;

    public bool isCharSelected = false;



    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            //Debug.Log("NOT NULL");
            Destroy(gameObject);
        }
        else
        {
            //PlayerPrefs.DeleteAll();
            //Debug.Log("NULL");
            _instance = this;
            RandomAdLimit = UnityEngine.Random.Range(2, 5);
            DontDestroyOnLoad(this);
        }

    }


    public IEnumerator Register()
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
            //Debug.Log("Erro: " + request.error);
        }
        else
        {

            TakenModel taken = JsonUtility.FromJson<TakenModel>(request.downloadHandler.text);
            //Debug.Log("TAKEN MODEL:" + taken.taken);

            if (taken.taken)
            {
                //Debug.Log("NICKNAME ALREADY TAKEN !!");
                registerEvent.Invoke(true);
            }
            else
            {
                registerEvent.Invoke(false);
                PlayerPrefs.SetString("player", request.downloadHandler.text);
                PlayerPrefs.SetInt("selectedChar", 0);
                _instance.playerModel = JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text);
                OSPermissionSubscriptionState one_signal_state = OneSignal.GetPermissionSubscriptionState();
                _instance.playerModel.one_signal_id = one_signal_state.subscriptionStatus.userId;
                //Debug.Log("NEW ONE SIGNAL APP ID: " + _instance.playerModel.one_signal_id);
                StartCoroutine(SetOneSignalId());
                _instance.StartCoroutine(_instance.GetInventory());
                //Debug.Log("NICKNAME NOT TAKEN !!");
                //Debug.Log("RESPONSE:"+ request.downloadHandler.text);
                //Debug.Log("PLAYER RESPONSE:" + JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text).ToString());

            }
        }
    }

    public IEnumerator SetOneSignalId()
    {
        string json = JsonUtility.ToJson(_instance.playerModel);
        //Debug.Log("JSON:" + json);
        var request = new UnityWebRequest(PLAYER_URL, "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            //Debug.Log("Erro: " + request.error);
        }
        else
        {
            PlayerPrefs.SetString("player", request.downloadHandler.text);
            _instance.playerModel = JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text);
        }
    }



    public IEnumerator CheckDeviceIsRegistered()
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
            //Debug.Log("Erro: " + request.error);
        }
        else
        {
            //Debug.Log("RESPONSEEEE: " + request.downloadHandler.text);

            if (request.downloadHandler.text.Length == 1)
            {
                if (Convert.ToInt32(request.downloadHandler.text) == 0)
                {
                    PlayerPrefs.DeleteKey("player");
                    _instance.notMemberEvent.Invoke();
                }
            }
            else
            {
                PlayerPrefs.SetString("player", request.downloadHandler.text);

                _instance.playerModel = JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text);
                //Debug.Log("PLAYER MODEL: " + _instance.playerModel.ToString());
                if (_instance.playerModel.one_signal_id == null || _instance.playerModel.one_signal_id == "")
                {
                    //Debug.Log("ONE SIGNAL APP ID IS NULL !!");
                    OSPermissionSubscriptionState one_signal_state = OneSignal.GetPermissionSubscriptionState();
                    _instance.playerModel.one_signal_id = one_signal_state.subscriptionStatus.userId;
                    //Debug.Log("NEW ONE SIGNAL APP ID: " + _instance.playerModel.one_signal_id);
                    StartCoroutine(SetOneSignalId());
                }

                _instance.StartCoroutine(_instance.GetInventory());
            }
        }
    }

    public IEnumerator GetLeaderBoard()
    {
        var request = new UnityWebRequest(LEADERBOARD_URL, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        yield return _instance.StartCoroutine(_instance.WaitForLeaderboard(request));
    }


    private IEnumerator WaitForLeaderboard(UnityWebRequest request)
    {
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            //Debug.Log("Erro: " + request.error);
        }
        else
        {
            _instance.leaderboard = LeaderBoardList.CreateFromJSON(request.downloadHandler.text);
            //Debug.Log("LEADERBOARD:" + leaderboard.players.Length);
            SceneManager.LoadScene("LeaderBoardScene");
        }
    }

    public IEnumerator GetCoinBoard()
    {
        var request = new UnityWebRequest(COINBOARD_URL, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        yield return _instance.StartCoroutine(_instance.WaitForCoinboard(request));
    }

    private IEnumerator WaitForCoinboard(UnityWebRequest request)
    {
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            //Debug.Log("Erro: " + request.error);
        }
        else
        {
            _instance.leaderboard = LeaderBoardList.CreateFromJSON(request.downloadHandler.text);
            coinBoardFetched.Invoke();
            //SceneManager.LoadScene("LeaderBoardScene");
        }
    }




    public IEnumerator GetInventory()
    {
        if (inventoryNeeded)
        {
            //Debug.Log("INVENTORY NEEDED !!");
            var request = new UnityWebRequest(INVENTORY_URL + playerModel.device_id, "GET");
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            yield return _instance.StartCoroutine(WaitForInventory(request));
        }

        else
        {
            //Debug.Log("INVENTORY NOT NEEDED !!");
            //_instance.ProgressBar.SetValue(Mathf.Clamp01(100f));
            GameObject progressBar = GameObject.FindGameObjectWithTag("ProgressBar");
            progressBar.GetComponent<LoadingProgress>().SetValue(Mathf.Clamp01(100f));
            progressBar.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            _instance.inventoryFetchedEvent.Invoke();
            yield return null;
        }

    }


    private IEnumerator WaitForInventory(UnityWebRequest request)
    {

        //Debug.Log("Trying the bundle download");
        _instance.asyncLoad = request.SendWebRequest();
        //yield return null;

        while (!asyncLoad.isDone)
        {
            //Debug.Log("D PROGRESS: " + request.downloadProgress);
            //_instance.ProgressBar.SetValue(Mathf.Clamp01(request.downloadProgress / 0.9f));
            GameObject progressBar = GameObject.FindGameObjectWithTag("ProgressBar");
            progressBar.GetComponent<LoadingProgress>().SetValue(Mathf.Clamp01(request.downloadProgress / 0.9f));
            yield return null;
        }

        if (request.error != null)
        {
            //Debug.Log("Erro: " + request.error);
        }
        else
        {
            _instance.inventoryList = InventoryList.CreateFromJSON(request.downloadHandler.text);
            if (PlayerPrefs.GetInt("selectedChar") == 0 || !_instance.inventoryList.inventory[PlayerPrefs.GetInt("selectedChar")].purchased)
            {
                PlayerPrefs.SetInt("selectedChar", 0);
            }
            _instance.inventoryFetchedEvent.Invoke();



            if (SceneManager.GetActiveScene().name == "OptionsScene")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            /*_instance.inventoryList = InventoryList.CreateFromJSON(request.downloadHandler.text);
            //Debug.Log("D PROGRESS: " + request.downloadProgress);

            if (SceneManager.GetActiveScene().name == "MenuScene")
            {
                _instance.progressBar.gameObject.SetActive(false);
                _instance.notMemberPanel.gameObject.SetActive(false);
                _instance.memberPanel.gameObject.SetActive(true);
                //Debug.Log("COINS: " + playerModel.coins.ToString());
                _instance.PlayerCoinText.text = _instance.playerModel.coins.ToString();
            }*/

        }
    }


    public IEnumerator UnlockMonster(int char_id)
    {
        UnlockModel unlockModel = new UnlockModel(_instance.playerModel.device_id, char_id);
        string json = JsonUtility.ToJson(unlockModel);
        //Debug.Log("JSON:" + json);
        var request = new UnityWebRequest(INVENTORY_URL, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            //Debug.Log("Erro: " + request.error);
        }
        else
        {
            PlayerPrefs.SetString("player", request.downloadHandler.text);
            _instance.playerModel = JsonUtility.FromJson<PlayerModel>(PlayerPrefs.GetString("player"));
            _instance.StartCoroutine(GetInventory());
        }
    }

    public IEnumerator SetHighScore(bool doubled = false)
    {
        string json = JsonUtility.ToJson(_instance.playerModel);
        //Debug.Log("HighScore JSON:" + json);
        var request = new UnityWebRequest(PLAYER_URL, "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            //Debug.Log("Erro: " + request.error);
        }
        else
        {
            if (!doubled)
            {
                //Debug.Log("RESPONSE: " + request.downloadHandler.text);
                PlayerPrefs.SetString("player", request.downloadHandler.text);
                _instance.playerModel = JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text);
                //Debug.Log("CHAR COLOR: " + GameMaster.gm.charColor);
                SoundManager.Instance.Play("Explosion");
            }

            else
            {
                PlayerPrefs.SetString("player", request.downloadHandler.text);
                _instance.playerModel = JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text);
                SceneManager.LoadScene(0);
            }
        }
    }

    public IEnumerator SetWinnerCreds()
    {
        string json = JsonUtility.ToJson(_instance.playerModel);
        //Debug.Log("Winner Creds JSON:" + json);
        var request = new UnityWebRequest(PLAYER_URL, "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            //Debug.Log("Erro: " + request.error);
        }
        else
        {
            //Debug.Log("RESPONSE: " + request.downloadHandler.text);
            PlayerPrefs.SetString("player", request.downloadHandler.text);
            _instance.playerModel = JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text);
            _instance.winnerCredsEvent.Invoke(1);
        }
    }


}
