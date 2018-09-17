using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OptionsController : MonoBehaviour
{

    public GameObject Player;

    public Text PlayerCoinText;
    public Image PlayerCoin;

    public GameObject charOptionsPanel;
    public GameObject charOptionsContent;

    //public GameObject themeOptionsPanel;
    //public GameObject themeOptionsContent;

    public GameObject unlockPanel;
    public Button unlockButton;
    public Text priceText;

    private bool charPanelCreated = false;
    //private bool themePanelCreated = false;

    public Button charBtn;
    public Button themeBtn;

    public Button charDeskButtonPrefab;
    private Button charDesk;

    public Image charImage;
    public Image lockImage;
    public Image selectedImage;
    public Text monsterName;

    public InventoryItem[] inventory;
    public List<Sprite> PlayerSprites = new List<Sprite>();
    public List<Sprite> ThemeSprites = new List<Sprite>();

    Color normalColor;
    Color selectedColor;




    // Use this for initialization
    void Start()
    {
        PlayerCoinText.text = NetworkController.Instance.playerModel.coins.ToString();
        inventory = NetworkController.Instance.inventoryList.inventory;
        normalColor = new Color(1, 1, 1, 1);
        selectedColor = new Color(0.86f, 0.78f, 0.49f, 1);
        charBtn.GetComponent<Image>().color = selectedColor;

        SetPanel();

    }

    public void SelectChar(Sprite charSprite, int charIndex)
    {
        string name = inventory[charIndex].character.char_name;
        bool purchased = inventory[charIndex].purchased;
        int price = inventory[charIndex].character.price;
        int char_id = int.Parse(inventory[charIndex].character.char_id);

        Debug.Log("SELECTED CHAR: " + name);
        GameObject selected = GameObject.Find(name);

        if (purchased)
        {
            if (!GameObject.Find("CharSelectedImage"))
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
            }

        }

        else
        {
            Debug.Log("NOT PURCHASED !!");
            unlockPanel.SetActive(true);


            GameObject charToUnlock = new GameObject();
            Image charToUnlockImage = charToUnlock.AddComponent<Image>();


            charToUnlockImage.sprite = charSprite;
            charToUnlock.transform.localScale = charToUnlock.transform.localScale * 5;
            charToUnlock = Instantiate(charToUnlock, unlockPanel.transform);
            charToUnlock.name = "CharToUnlock";


            monsterName = Instantiate(monsterName, unlockPanel.transform);
            monsterName.text = name;
            monsterName.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.7f);
            monsterName.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.7f);
            monsterName.transform.localScale = monsterName.transform.localScale * 5;

            priceText = Instantiate(priceText, unlockPanel.transform);
            priceText.text = price.ToString();

            unlockButton = Instantiate(unlockButton, unlockPanel.transform);
            unlockButton.onClick.AddListener(delegate { UnlockMonster(char_id, price); });
        }
    }


    private void SetPanel() // old parameter int index
    {

        if (!charPanelCreated)
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
                    Debug.Log(inventory[i].character.img.Length);
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


                monsterName = Instantiate(monsterName, charDesk.transform);
                monsterName.text = inventory[i].character.char_name;

                if (!inventory[i].purchased)
                {
                    lockImage = Instantiate(lockImage, charDesk.transform);
                    charDesk.GetComponent<Image>().color = new Color(0.407f, 0.407f, 0.407f, 0.439f);
                }

                if (i == PlayerPrefs.GetInt("selectedChar"))
                {
                    selectedImage = Instantiate(selectedImage, charDesk.transform);
                    selectedImage.name = "CharSelectedImage";
                }

            }
            charPanelCreated = true;

        }

        /*if (index == 1 && !themePanelCreated)
        {
            for (int i = 0; i < ThemeSprites.ToArray().Length; i++)
            {

                Button charDeskButton = Instantiate(charDeskButtonPrefab, charOptionsContent.transform);
            }

            themePanelCreated = true;
        }*/
    }

    /*public void ChangeSegment(int index)
    {
        if (index == selectedSegment)
        {
            //do nothing
        }

        else
        {
            if (index == 0)
            {
                selectedSegment = 0;
                themeBtn.GetComponent<Image>().color = normalColor;
                charBtn.GetComponent<Image>().color = selectedColor;
                themeOptionsPanel.SetActive(false);
                charOptionsPanel.SetActive(true);
                SetPanel(0);
            }

            if (index == 1)
            {
                selectedSegment = 1;
                charBtn.GetComponent<Image>().color = normalColor;
                themeBtn.GetComponent<Image>().color = selectedColor;
                charOptionsPanel.SetActive(false);
                themeOptionsPanel.SetActive(true);
                SetPanel(1);
            }
        }
    }*/

    public void loadScene(string sceneName)
    {
        //SoundManager.Instance.MusicSource.Pause();
        MenuCtrl.Instance.loadScene(sceneName);
    }

    IEnumerator StopCoinAnim(float time, int char_id)
    {
        yield return new WaitForSeconds(time);
        SoundManager.Instance.MusicSource.Stop();
        SoundManager.Instance.PlayMusic("GameSound");
        PlayerCoin.GetComponent<Animator>().SetTrigger("fixed");
        NetworkController.Instance.StartCoroutine(NetworkController.Instance.UnlockMonster(char_id));

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
            currCountdownValue-=0.2f;
            coins -= (price / (countdownValue * 5));
            PlayerCoinText.text = ((int)coins).ToString();
        }
    }


    public void UnlockMonster(int char_id, int price)
    {
        if (price > NetworkController.Instance.playerModel.coins)
        {
            Debug.Log("Yo, you don't have enough money for this shit !!");
        }

        else
        {
            StartCoroutine(DecreasePlayerCoin(price, 3, char_id));
        }
    }



}
