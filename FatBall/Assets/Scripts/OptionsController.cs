using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OptionsController : MonoBehaviour
{

    public GameObject ProgressBar;

    public GameObject Player;

    public Text PlayerCoinText;
    public Image PlayerCoin;
    public Text NoMoneyText;
    public Button backButton;

    public GameObject charOptionsPanel;
    public GameObject charOptionsContent;

    public GameObject unlockPanel;
    public Button unlockButton;
    public Text priceText;

    public GameObject SelectPanel;
    public Button SelectButton;

    public Image CharToSelectImage;
    public Text CharNameText;

    public Button charBtn;
    public Button themeBtn;

    public Button charDeskButtonPrefab;
    private Button charDesk;

    public Text monsterNamePre;

    public Image charImage;
    public Image lockImage;
    public Image selectedImage;
    public Text monsterName;
    public Image charToUnlockImage;
    public Button UnlockPanelCloseBtn;

    public InventoryItem[] inventory;
    public List<Sprite> PlayerSprites = new List<Sprite>();
    public List<Sprite> ThemeSprites = new List<Sprite>();

    Color selectedColor;

    private bool isUnlockPanelActive;
    private bool isUnlockCreated;

    Image attr1;
    Image attr2;
    Image attr3;



    private void Awake()
    {
        isUnlockPanelActive = false;
        isUnlockCreated = false;
        //Debug.Log("isUnlockPanelActive:" + isUnlockPanelActive);
    }


    // Use this for initialization
    void Start()
    {
        PlayerCoinText.text = NetworkManager.instance.playerModel.coins.ToString();
        inventory = NetworkManager.instance.inventoryList.inventory;
        selectedColor = new Color(0.86f, 0.78f, 0.49f, 1);
        charBtn.GetComponent<Image>().color = selectedColor;

        SetPanel();
        //Invoke("TakeSS", 3f);
    }

    void TakeSS()
    {
        ScreenCapture.CaptureScreenshot("inventoryIpad_char.png");
    }

    public void SelectChar(Sprite charSprite, int charIndex)
    {
        if (!isUnlockPanelActive)
        {
            string name = inventory[charIndex].character.char_name;
            bool purchased = inventory[charIndex].purchased;
            int price = inventory[charIndex].character.price;
            int char_id = int.Parse(inventory[charIndex].character.char_id);

            //Debug.Log("SELECTED CHAR: " + name);
            //Debug.Log("CHAR INDEX:" + charIndex);

            if (purchased)
            {
                backButton.gameObject.SetActive(false);
                SelectPanel.gameObject.SetActive(true);

                CharToSelectImage.sprite = charSprite;
                CharToSelectImage.name = "CharToSelect";
                SelectButton.onClick.AddListener(delegate { EquipChar(charIndex, name); });

                CharNameText.text = name;

                StartCoroutine(SetAttributesAnimation(0.5f, charIndex));

                /*if (!GameObject.Find("CharSelectedImage"))
                {
                    selectedImage = Instantiate(selectedImage, selected.transform);
                    selectedImage.name = "CharSelectedImage";
                    PlayerPrefs.SetInt("selectedChar", charIndex);

                    //Player.GetComponent<SpriteRenderer>().sprite = charSprite;
                    //DestroyImmediate(Player.GetComponent<PolygonCollider2D>(), true);
                    //Player.AddComponent<PolygonCollider2D>();
                    //Player.GetComponent<PolygonCollider2D>().isTrigger = true;
                }

                if (GameObject.Find("CharSelectedImage"))
                {
                    selectedImage.transform.SetParent(selected.transform, false);
                    PlayerPrefs.SetInt("selectedChar", charIndex);

                    //Player.GetComponent<SpriteRenderer>().sprite = charSprite;
                    //DestroyImmediate(Player.GetComponent<PolygonCollider2D>(), true);
                    //Player.AddComponent<PolygonCollider2D>();
                    //Player.GetComponent<PolygonCollider2D>().isTrigger = true;
                }*/

            }

            else
            {
                //Debug.Log("NOT PURCHASED !!");
                unlockPanel.gameObject.SetActive(true);
                isUnlockPanelActive = true;
                backButton.gameObject.SetActive(false);

                if (!isUnlockCreated)
                {
                    isUnlockCreated = true;
                    charToUnlockImage.sprite = charSprite;
                    //charToUnlockImage = Instantiate(charToUnlockImage, unlockPanel.transform);
                    charToUnlockImage.name = "CharToUnlock";


                    //monsterName = Instantiate(monsterName, unlockPanel.transform);
                    //monsterName.GetComponent<RectTransform>().offsetMin = new Vector2(monsterName.GetComponent<RectTransform>().offsetMin.x, 80);
                    //monsterName.GetComponent<RectTransform>().offsetMax = new Vector2(monsterName.GetComponent<RectTransform>().offsetMax.x, 30);
                    //monsterName.GetComponent<RectTransform>().sizeDelta = new Vector2(370, monsterName.GetComponent<RectTransform>().sizeDelta.y);
                    //monsterName.fontSize = 70;
                    monsterName.text = name;

                    StartCoroutine(SetAttributesAnimation(0.5f, charIndex));
                    //SetAttributes(charIndex);


                    //priceText = Instantiate(priceText, unlockPanel.transform);
                    priceText.text = price.ToString();

                    //unlockButton = Instantiate(unlockButton, unlockPanel.transform);
                    unlockButton.onClick.AddListener(delegate { UnlockMonster(char_id, price, NetworkManager.instance.playerModel.coins); });
                }

                else
                {
                    charToUnlockImage.sprite = charSprite;

                    //monsterName.GetComponent<RectTransform>().offsetMin = new Vector2(monsterName.GetComponent<RectTransform>().offsetMin.x, 80);
                    //monsterName.GetComponent<RectTransform>().offsetMax = new Vector2(monsterName.GetComponent<RectTransform>().offsetMax.x, 30);
                    //monsterName.GetComponent<RectTransform>().sizeDelta = new Vector2(370, monsterName.GetComponent<RectTransform>().sizeDelta.y);
                    //monsterName.fontSize = 70;
                    monsterName.text = name;

                    StartCoroutine(SetAttributesAnimation(0.5f, charIndex));
                    //SetAttributes(charIndex);

                    priceText.text = price.ToString();

                    unlockButton.onClick.RemoveAllListeners();
                    unlockButton.onClick.AddListener(delegate { UnlockMonster(char_id, price, NetworkManager.instance.playerModel.coins); });
                }
                //Invoke("TakeSS", 3f);
            }
        }
    }

    IEnumerator SetAttributesAnimation(float time, int charIndex)
    {
        float animationTime = 0f;
        string[] attributes = inventory[charIndex].character.attr.Split(',');
        float x = (int.Parse(attributes[0]) / 10f / time);
        float y = (int.Parse(attributes[1]) / 10f / time);
        float z = (int.Parse(attributes[2]) / 10f / time);

        attr1 = GameObject.FindGameObjectWithTag("Attr1").GetComponent<Image>();
        attr2 = GameObject.FindGameObjectWithTag("Attr2").GetComponent<Image>();
        attr3 = GameObject.FindGameObjectWithTag("Attr3").GetComponent<Image>();


        while (animationTime < time)
        {
            animationTime += Time.deltaTime;
            Debug.Log("ANIMATION TIME: " + animationTime);
            //countdownSprite.fillAmount = animationTime / time;
            attr1.fillAmount = animationTime * time * x / time;
            attr2.fillAmount = animationTime * time * y / time;
            attr3.fillAmount = animationTime * time * z / time;
            yield return null;
        }
    }


    public void CloseUnlockPanel(int panelIndex) // Unlock panel --> 0 ,  SelectPanel --> 1
    {
        if (panelIndex == 0)
        {
            unlockPanel.gameObject.SetActive(false);
            isUnlockPanelActive = false;
            NoMoneyText.gameObject.SetActive(false);
            backButton.gameObject.SetActive(true);
            attr1.fillAmount = 0;
            attr2.fillAmount = 0;
            attr3.fillAmount = 0;
        }

        if (panelIndex == 1)
        {
            SelectPanel.gameObject.SetActive(false);
            backButton.gameObject.SetActive(true);
            attr1.fillAmount = 0;
            attr2.fillAmount = 0;
            attr3.fillAmount = 0;
        }

    }

    public void EquipChar(int charIndex, string name)
    {
        SelectPanel.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        attr1.fillAmount = 0;
        attr2.fillAmount = 0;
        attr3.fillAmount = 0;

        GameObject selected = GameObject.Find(name);

        if (!NetworkManager.instance.isCharSelected)
        {
            selectedImage = Instantiate(selectedImage, selected.transform);
            selectedImage.name = "CharSelectedImage";
            PlayerPrefs.SetInt("selectedChar", charIndex);
            NetworkManager.instance.isCharSelected = true;

            //Player.GetComponent<SpriteRenderer>().sprite = charSprite;
            //DestroyImmediate(Player.GetComponent<PolygonCollider2D>(), true);
            //Player.AddComponent<PolygonCollider2D>();
            //Player.GetComponent<PolygonCollider2D>().isTrigger = true;
        }

        if (NetworkManager.instance.isCharSelected)
        {
            selectedImage.transform.SetParent(selected.transform, false);
            PlayerPrefs.SetInt("selectedChar", charIndex);
            NetworkManager.instance.isCharSelected = true;

            //Player.GetComponent<SpriteRenderer>().sprite = charSprite;
            //DestroyImmediate(Player.GetComponent<PolygonCollider2D>(), true);
            //Player.AddComponent<PolygonCollider2D>();
            //Player.GetComponent<PolygonCollider2D>().isTrigger = true;
        }
    }


    public void UnlockMonster(int char_id, int price, int playerCoins)
    {
        //int playerTempPrice = NetworkController.instance.playerModel.coins;
        //Debug.Log("PLAYER COINS:" + playerCoins);
        unlockButton.interactable = false;
        if (price > playerCoins)
        {
            //Debug.Log("Yo, you don't have enough money for this shit !!");
            NoMoneyText.gameObject.SetActive(true);
            NoMoneyText.text = "Yo, you don't have enough";

        }

        else
        {
            UnlockPanelCloseBtn.gameObject.SetActive(false);
            StartCoroutine(DecreasePlayerCoin(price, 3, char_id));
        }
    }

    float currCountdownValue;
    public IEnumerator DecreasePlayerCoin(float price, float countdownValue, int char_id)
    {
        PlayerCoin.GetComponent<Animator>().SetTrigger("purchased");
        SoundManager.Instance.PlayMusic("CoinSound");
        currCountdownValue = countdownValue;
        float coins = int.Parse(PlayerCoinText.text);
        StartCoroutine(StopCoinAnim(countdownValue, char_id));
        while (currCountdownValue > 0)
        {
            yield return new WaitForSeconds(0.2f);
            currCountdownValue -= 0.2f;
            coins -= (price / (countdownValue * 5));
            PlayerCoinText.text = ((int)coins).ToString();
        }
    }

    IEnumerator StopCoinAnim(float time, int char_id)
    {
        yield return new WaitForSeconds(time);
        SoundManager.Instance.MusicSource.Stop();
        SoundManager.Instance.PlayMusic("GameSound");
        PlayerCoin.GetComponent<Animator>().SetTrigger("fixed");
        ProgressBar.gameObject.SetActive(true);
        NetworkManager.instance.inventoryNeeded = true;
        NetworkManager.instance.StartCoroutine(NetworkManager.instance.UnlockMonster(char_id));
        unlockPanel.gameObject.SetActive(false);
        isUnlockPanelActive = false;

    }


    private void SetPanel() // old parameter int index
    {


        for (int i = 0; i < inventory.Length; i++)
        {
            byte[] dataImage;
            Texture2D mytexture = new Texture2D(1, 1);
            Sprite charSprite;

            try
            {
                //Debug.Log(inventory[i].character.char_name + ": " + inventory[i].character.img.Length);
                dataImage = System.Convert.FromBase64String(inventory[i].character.img);
                mytexture = new Texture2D(1, 1);
                mytexture.LoadImage(dataImage);

            }
            catch (System.FormatException e)
            {
                //Debug.Log(inventory[i].character.img.Length);
                dataImage = System.Convert.FromBase64String(inventory[i].character.img);
                mytexture = new Texture2D(1, 1);
                mytexture.LoadImage(dataImage);
            }

            charSprite = Sprite.Create(mytexture, new Rect(0.0f, 0.0f, mytexture.width, mytexture.height), new Vector2(0.5f, 0.5f));

            charDesk = Instantiate(charDeskButtonPrefab, charOptionsContent.transform);
            int charIndex = i;
            charDesk.onClick.AddListener(delegate { SelectChar(charSprite, charIndex); });
            charDesk.name = inventory[i].character.char_name;

            charImage = Instantiate(charImage, charDesk.transform);
            charImage.GetComponent<Image>().sprite = charSprite;


            monsterNamePre = Instantiate(monsterNamePre, charDesk.transform);
            monsterNamePre.name = "MonsterNamePreText";
            //monsterNamePre.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.6f);
            monsterNamePre.text = inventory[i].character.char_name;
            bool selected = false;
            if (!inventory[i].purchased)
            {
                lockImage = Instantiate(lockImage, charDesk.transform);
                charDesk.GetComponent<Image>().color = new Color(0.407f, 0.407f, 0.407f, 0.439f);
                selected = true;
            }

            if (i == PlayerPrefs.GetInt("selectedChar") && inventory[i].purchased)
            {
                Debug.Log("selectedCharIndex: " + PlayerPrefs.GetInt("selectedChar") + "   " + inventory[PlayerPrefs.GetInt("selectedChar")].character.char_name);
                selectedImage = Instantiate(selectedImage, charDesk.transform);
                selectedImage.name = "CharSelectedImage";
                NetworkManager.instance.isCharSelected = true;
            }

            if (i == inventory.Length - 1 && !selected)
            {
                Debug.Log("CANNOT FOUND SELECTED !!");
                lockImage = Instantiate(lockImage, charDesk.transform);
                charDesk.GetComponent<Image>().color = new Color(0.407f, 0.407f, 0.407f, 0.439f);
                selected = true;
                PlayerPrefs.SetInt("selectedChar", 0);
                NetworkManager.instance.isCharSelected = true;
            }
        }
    }


    public void loadScene(string sceneName)
    {
        NetworkManager.instance.inventoryNeeded = false;
        MenuCtrl.instance.loadScene(sceneName);
    }


}
