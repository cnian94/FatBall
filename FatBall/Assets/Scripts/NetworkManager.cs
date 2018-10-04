﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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

    public LoadingProgress progressBarPre;
    public LoadingProgress progressBar;

    private AsyncOperation asyncLoad;

    public string device_id;
    public LeaderBoardList leaderboard;

    public PlayerModel playerModel;
    public InventoryList inventoryList;


    private string REGISTER_URL = "https://fatball.herokuapp.com/api/register";
    private string CHECK_URL = "https://fatball.herokuapp.com/api/check";
    private string PLAYER_URL = "https://fatball.herokuapp.com/api/player";
    private string LEADERBOARD_URL = "https://fatball.herokuapp.com/api/leaderboard";
    private string INVENTORY_URL = "https://fatball.herokuapp.com/api/inventory?id=";

    public int RandomAdLimit;
    public int PlayCounter;

    [System.Serializable]
    public class RegisterEvent : UnityEngine.Events.UnityEvent<bool> { }
    public RegisterEvent registerEvent;

    public UnityEvent inventoryFetchedEvent;

    public bool inventoryNeeded = true;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.Log("NOT NULL");
            Destroy(gameObject);
        }
        else
        {
            //PlayerPrefs.DeleteAll();
            Debug.Log("NULL");
            _instance = this;
            RandomAdLimit = Random.Range(2, 5);
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
            Debug.Log("TAKEN MODEL:" + taken.taken);

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
                _instance.StartCoroutine(_instance.GetInventory());
                //Debug.Log("NICKNAME NOT TAKEN !!");
                //Debug.Log("RESPONSE:"+ request.downloadHandler.text);
                //Debug.Log("PLAYER RESPONSE:" + JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text).ToString());

            }
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
            //Debug.Log("DEVICE IS ALREADY REGISTERED: " + request.responseCode);
            PlayerPrefs.SetString("player", request.downloadHandler.text);

            if (PlayerPrefs.GetInt("selectedChar") == 0)
            {
                PlayerPrefs.SetInt("selectedChar", 0);
            }

            _instance.playerModel = JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text);

            _instance.StartCoroutine(_instance.GetInventory());
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




    public IEnumerator GetInventory()
    {
        if (inventoryNeeded)
        {
            Debug.Log("INVENTORY NEEDED !!"); 
            var request = new UnityWebRequest(INVENTORY_URL + playerModel.device_id, "GET");
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            yield return _instance.StartCoroutine(WaitForInventory(request));
        }

        else
        {
            Debug.Log("INVENTORY NOT NEEDED !!");
            //_instance.progressBar.gameObject.SetActive(false);
            _instance.progressBar.SetValue(Mathf.Clamp01(100f));
            yield return new WaitForSeconds(0.1f);
            _instance.inventoryFetchedEvent.Invoke();
            yield return null;
        }

    }


    private IEnumerator WaitForInventory(UnityWebRequest request)
    {

        Debug.Log("Trying the bundle download");
        _instance.asyncLoad = request.SendWebRequest();
        //yield return null;

        while (!asyncLoad.isDone)
        {
            //Debug.Log("D PROGRESS: " + request.downloadProgress);
            _instance.progressBar.SetValue(Mathf.Clamp01(request.downloadProgress / 0.9f));
            yield return null;
        }

        if (request.error != null)
        {
            //Debug.Log("Erro: " + request.error);
        }
        else
        {
            _instance.inventoryList = InventoryList.CreateFromJSON(request.downloadHandler.text);
            _instance.inventoryFetchedEvent.Invoke();


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
            //Debug.Log("Erro: " + request.error);
        }
        else
        {
            if (!doubled)
            {
                //Debug.Log("RESPONSE: " + request.downloadHandler.text);
                PlayerPrefs.SetString("player", request.downloadHandler.text);
                _instance.playerModel = JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text);
                GameMaster.gm.gameOverUI.gameObject.SetActive(true);
            }

            else
            {
                PlayerPrefs.SetString("player", request.downloadHandler.text);
                _instance.playerModel = JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text);
                SceneManager.LoadScene(0);
            }
        }
    }
}