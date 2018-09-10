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

    private string REGISTER_URL = "http://127.0.0.1:5000/api/register";
    private string CHECK_URL = "http://127.0.0.1:5000/api/check";
    private string INVENTORY_URL = "http://127.0.0.1:5000/api/inventory?id=";



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

        if (playerModel == null)
        {
            notMemberPanel.SetActive(true);

        }

        else
        {
            //memberPanel.SetActive(true);
            StartCoroutine(CheckDeviceIsRegistered());
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


            notMemberPanel.SetActive(false);
            memberPanel.SetActive(true);
        }
    }

    public void GetIn()
    {
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

            //Debug.Log("RESPONSE:"+ request.downloadHandler.text);
            //Debug.Log("PLAYER RESPONSE:" + JsonUtility.FromJson<PlayerModel>(request.downloadHandler.text).ToString());


            notMemberPanel.SetActive(false);
            memberPanel.SetActive(true);
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




    // Use this for initialization
    void Start()
    {
        StartCoroutine(GetInventory());
    }

}

