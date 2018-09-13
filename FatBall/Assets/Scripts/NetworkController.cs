using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkController : MonoBehaviour
{

    // Singleton instance.
    public static NetworkController Instance = null;

    public GameObject notMemberPanel;
    public GameObject memberPanel;

    public Text nickname;
    private string device_id;

    public PlayerModel playerModel;
    public InventoryList inventoryList;

    private string REGISTER_URL = "http://192.168.1.104:5000/api/register";
    private string CHECK_URL = "http://192.168.1.104:5000/api/check";
    private string PLAYER_URL = "http://192.168.1.104:5000/api/player";
    private string INVENTORY_URL = "http://192.168.1.104:5000/api/inventory?id=";



    // Initialize the singleton instance.
    private void Awake()
    {
        // If there is not already an instance of NetworkController, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Set NetworkController to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);

        playerModel = JsonUtility.FromJson<PlayerModel>(PlayerPrefs.GetString("player"));
        //playerModel = new PlayerModel("3323e9048b337f17b71d49e4ac5925e951ada236", "cano", 3,  241);
        //PlayerPrefs.SetString("player",JsonUtility.ToJson(playerModel));

        if (playerModel == null)
        {
            memberPanel.SetActive(false);
            notMemberPanel.SetActive(true); //burayı kapat
            //memberPanel.SetActive(true); //bunu yaz

        }

        else
        {
            //memberPanel.SetActive(true);
            StartCoroutine(CheckDeviceIsRegistered());
        }

        //PlayerPrefs.SetString("player", null);
    }


    IEnumerator Register()
    {
        string json = JsonUtility.ToJson(playerModel);
        var request = new UnityWebRequest(REGISTER_URL, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send();

        if (request.error != null)
        {
            Debug.Log("Erro: " + request.error);
        }
        else
        {
            Debug.Log("Status Code: " + request.responseCode);


            //Debug.Log("RESPONSE:"+ request.downloadHandler.text);
            //Debug.Log("PLAYER RESPONSE:" + JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text).ToString());
            PlayerPrefs.SetString("player", request.downloadHandler.text);

            StartCoroutine(GetInventory());
            notMemberPanel.SetActive(false);
            memberPanel.SetActive(true);
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
        yield return request.Send();

        if (request.error != null)
        {
            Debug.Log("Erro: " + request.error);
        }
        else
        {
            Debug.Log("DEVICE IS ALREADY REGISTERED: " + request.responseCode);
            PlayerPrefs.SetString("player", request.downloadHandler.text);
            playerModel = JsonUtility.FromJson<PlayerModel>(PlayerPrefs.GetString("player"));

            notMemberPanel.SetActive(false);
            memberPanel.SetActive(true);
            StartCoroutine(GetInventory());
        }
    }

    public IEnumerator GetInventory()
    {
        string json = JsonUtility.ToJson(playerModel);
        var request = new UnityWebRequest(INVENTORY_URL + playerModel.device_id, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        yield return request.Send();

        if (request.error != null)
        {
            Debug.Log("Erro: " + request.error);
        }
        else
        {
            Debug.Log("Response:"+ request.downloadHandler.text);
            inventoryList = InventoryList.CreateFromJSON(request.downloadHandler.text);
            Debug.Log("INVENTORY:" + inventoryList.inventory.Length);
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
        yield return request.Send();

        if (request.error != null)
        {
            Debug.Log("Erro: " + request.error);
        }
        else
        {
            StartCoroutine(GetInventory());
        }
    }

    public IEnumerator SetHighScore()
    {
        string json = JsonUtility.ToJson(playerModel);
        Debug.Log("JSON:" + json);
        var request = new UnityWebRequest(PLAYER_URL, "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send();

        if (request.error != null)
        {
            Debug.Log("Erro: " + request.error);
        }
        else
        {
            Debug.Log("RESPONSE: " + request.downloadHandler.text);
            PlayerPrefs.SetString("player", request.downloadHandler.text);
            playerModel = JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text);
        }
    }




    // Use this for initialization
    void Start()
    {

    }

}

