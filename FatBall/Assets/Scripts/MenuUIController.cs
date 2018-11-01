using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ChartboostSDK;

public class MenuUIController : MonoBehaviour
{

    public GameObject notMemberPanel;
    public GameObject memberPanel;
    public GameObject winnerPanel;
    public GameObject connectionPanel;
    public GameObject ProgressBar;
    public Text TipText;

    public Text PlayerCoinText;

    public Text nickname;
    public Text takenText;

    public Text WinnerText;

    private string[] Tips = { "Beer is your friend", "Don't touch spikes", "Mushroom is the shield", "Conquer that heart", "Eat healthy", "No, not the cake", "Tilt to play" };

    private void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        NetworkManager.instance.notificationEvent.AddListener(SetWinnerPanel);
        NetworkManager.instance.inventoryFetchedEvent.AddListener(SetMemberPanelActive);
        NetworkManager.instance.registerEvent.AddListener(SetNotMemberPanelActive);
        NetworkManager.instance.winnerCredsEvent.AddListener(WinnerCredsSet);
        NetworkManager.instance.notMemberEvent.AddListener(NotMember);
        Check();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void NotMember()
    {
        notMemberPanel.gameObject.SetActive(true);
    }

    void WinnerCredsSet(int val)
    {
        winnerPanel.gameObject.SetActive(false);
        if (val == 1)
        {
            WinnerText.gameObject.SetActive(true);
        }
        StartCoroutine(CloseWinnerPanel(val));
    }

    IEnumerator CloseWinnerPanel(int val)
    {
        if (val == 1)
        {
            yield return new WaitForSeconds(4f);
            WinnerText.gameObject.SetActive(false);
            Check();
        }
        else
        {
            Check();
        }

        //TipText.text += "PS: " + Tips[Random.Range(0, 5)];

    }

    void SetWinnerPanel()
    {
        ProgressBar.gameObject.SetActive(false);
        //Debug.Log("Openning Winner Panel !!");
        memberPanel.gameObject.SetActive(false);
        winnerPanel.gameObject.SetActive(true);
    }

    void SetNotMemberPanelActive(bool taken)
    {
            if (taken)
            {
                takenText.text = "already taken !!";
            }
            else
            {
                //do nothing
            }
    }

    void SetMemberPanelActive()
    {
        //TipText.text += "PS: " + NetworkManager.instance.isNotification;

        //NetworkManager.instance.inventoryList = InventoryList.CreateFromJSON(text);
        //Debug.Log("D PROGRESS: " + request.downloadProgress);
        //Debug.Log("isInitialized: " + Chartboost.isInitialized());

            if (SceneManager.GetActiveScene().name == "MenuScene")
            {
                //NetworkManager.instance.progressBar.gameObject.SetActive(false);??
                ProgressBar.gameObject.SetActive(false);
                notMemberPanel.gameObject.SetActive(false);
                memberPanel.gameObject.SetActive(true);
                //Debug.Log("COINS: " + playerModel.coins.ToString());
                PlayerCoinText.text = NetworkManager.instance.playerModel.coins.ToString();
            }


            if (SceneManager.GetActiveScene().name == "OptionsScene" && NetworkManager.instance.inventoryNeeded)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            int index = Random.Range(0, Tips.Length);
            TipText.text += Tips[index];
    }


    private void Check()
    {
        //Debug.Log("CHECKINGGG !!");
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //this.playerModel = new PlayerModel("3323e9048b337f17b71d49e4ac5925e951ada236", "cano", 197, 3747);
        //PlayerPrefs.SetString("player",JsonUtility.ToJson(playerModel));
        //Debug.Log("PLAYER PREFS: " + PlayerPrefs.GetString("player"));

        if (PlayerPrefs.GetString("player") == null || PlayerPrefs.GetString("player").Equals(""))
        {
            //Debug.Log("PLAYER NULL !!");
            notMemberPanel.gameObject.SetActive(true); //burayı kapat
            //memberPanel.gameObject.SetActive(false);
            //memberPanel.SetActive(true); //bunu yaz
        }

        else
        {
            //Debug.Log("PLAYER NOT NULL !!");
            //Debug.Log("PLAYERRR: " + PlayerPrefs.GetString("player"));
            //PlayerModel player = new PlayerModel(SystemInfo.deviceUniqueIdentifier, "TEST");
            NetworkManager.instance.playerModel = JsonUtility.FromJson<PlayerModel>(PlayerPrefs.GetString("player"));
            ProgressBar.gameObject.SetActive(true);
            NetworkManager.instance.StartCoroutine(NetworkManager.instance.CheckDeviceIsRegistered());
        }
    }

    public void GetIn()
    {
        NetworkManager.instance.device_id = SystemInfo.deviceUniqueIdentifier;
        NetworkManager.instance.playerModel = new PlayerModel(NetworkManager.instance.device_id, nickname.text);
        //NetworkManager.instance.ProgressBar = Instantiate(NetworkManager.instance.ProgressBar, gameObject.transform.GetChild(0).transform);
        ProgressBar.gameObject.SetActive(true);
        NetworkManager.instance.StartCoroutine(NetworkManager.instance.Register());
    }
}
