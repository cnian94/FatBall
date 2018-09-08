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



    IEnumerator CheckDeviceIsRegistered(string device_id)
    {

        string json = JsonUtility.ToJson(playerModel);
        var request = new UnityWebRequest("http://127.0.0.1:5000/api/register", "POST");
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
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            PlayerPrefs.SetString("device_id", device_id);
            notMemberPanel.SetActive(false);
            memberPanel.SetActive(true);
        }

    }


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

        device_id = SystemInfo.deviceUniqueIdentifier;

        if (PlayerPrefs.GetString("device_id") == "")
        {
            notMemberPanel.SetActive(true);

        }

        else
        {
            Debug.Log("Nickname: " + PlayerPrefs.GetString("device_id"));
            memberPanel.SetActive(true);
        }
    }

    public void GetIn()
    {
        playerModel = new PlayerModel(device_id, nickname.text);
        StartCoroutine(CheckDeviceIsRegistered(device_id));
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
