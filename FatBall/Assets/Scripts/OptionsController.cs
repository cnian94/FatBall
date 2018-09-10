using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OptionsController : MonoBehaviour
{

    public GameObject Player;

    public GameObject charOptionsPanel;
    public GameObject charOptionsContent;

    public GameObject themeOptionsPanel;
    public GameObject themeOptionsContent;

    public GameObject unlockPanel;
    public Image coinImage;
    public Text priceText;

    private bool charPanelCreated = false;
    private bool themePanelCreated = false;

    public Button charBtn;
    public Button themeBtn;

    //public Image charDeskPrefab;
    //private Image charDesk;

    public Button charDeskButtonPrefab;
    private Button charDeskButton;

    public Image charImage;
    public Image lockImage;
    public Image selectedImage;
    public Text monsterName;

    public InventoryItem[] inventory;
    public List<Sprite> PlayerSprites = new List<Sprite>();
    public List<Sprite> ThemeSprites = new List<Sprite>();

    public Dictionary<string, KeyValuePair<int[], Sprite>> dictionary = new Dictionary<string, KeyValuePair<int[], Sprite>>();

    Color normalColor;
    Color selectedColor;

    public int selectedSegment = 0;


    byte[] dataImage;
    Texture2D mytexture;
    Sprite charSprite;

    void CreateDictionary()
    {

        for (int i = 0; i < PlayerSprites.ToArray().Length; i++)
        {
            int[] key = { Random.Range(1000, 10000), Random.Range(0, 2), 0};
            KeyValuePair<int[], Sprite> pair = new KeyValuePair<int[], Sprite>(key, PlayerSprites[i]);
            dictionary.Add(PlayerSprites[i].name, pair);
        }
    }

    // Use this for initialization
    void Start()
    {
        inventory = NetworkController.Instance.inventoryList.inventory;
        normalColor = new Color(1, 1, 1, 1);
        selectedColor = new Color(0.86f, 0.78f, 0.49f, 1);
        charBtn.GetComponent<Image>().color = selectedColor;

        CreateDictionary();

        SetPanel(0);

    }

    public void SelectChar(string name, Sprite charSprite, bool purchased, int price)
    {
        Debug.Log("SELECTED CHAR: " + name);
        GameObject selected = GameObject.Find(name);

        if (purchased) 
        {
            if (!GameObject.Find("CharSelectedImage"))
            {
                selectedImage = Instantiate(selectedImage, selected.transform);
                selectedImage.name = "CharSelectedImage";
                Player.GetComponent<SpriteRenderer>().sprite = charSprite;
                DestroyImmediate(Player.GetComponent<PolygonCollider2D>(), true);
                Player.AddComponent<PolygonCollider2D>();
                Player.GetComponent<PolygonCollider2D>().isTrigger = true;
            }

            if (GameObject.Find("CharSelectedImage"))
            {
                selectedImage.transform.SetParent(selected.transform, false);
                Player.GetComponent<SpriteRenderer>().sprite = charSprite;
                DestroyImmediate(Player.GetComponent<PolygonCollider2D>(), true);
                Player.AddComponent<PolygonCollider2D>();
                Player.GetComponent<PolygonCollider2D>().isTrigger = true;
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
            monsterName = Instantiate(monsterName, unlockPanel.transform);
            monsterName.text = name;
            monsterName.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.7f);
            monsterName.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.7f);
            monsterName.transform.localScale = monsterName.transform.localScale * 5;

            coinImage = Instantiate(coinImage, unlockPanel.transform);
            priceText = Instantiate(priceText, unlockPanel.transform);
            priceText.text = price.ToString();


        }


    }


    private void SetPanel(int index)
    {

        if (index == 0 && !charPanelCreated)
        {
            Debug.Log("index:" + index);
            Debug.Log("charPanelCreated:" + charPanelCreated);
            Debug.Log("inventory.Length:" + inventory.Length);


            for (int i = 0; i < inventory.Length ; i++)
            {
                try
                {
                    //Debug.Log(inventory[i].character.char_name + ": " + inventory[i].character.img.Length);
                    this.dataImage = System.Convert.FromBase64String(inventory[i].character.img);
                    this.mytexture = new Texture2D(1, 1);
                    this.mytexture.LoadImage(this.dataImage);

                    this.charSprite = Sprite.Create(this.mytexture, new Rect(0.0f, 0.0f, this.mytexture.width, this.mytexture.height), new Vector2(0.5f, 0.5f));

                }
                catch (System.FormatException e)
                {
                    Debug.Log(inventory[i].character.char_name + ": " + inventory[i].character.img.Length);
                    //inventory[i].character.img += "=";
                    inventory[i].character.img = inventory[i].character.img.Substring(0, inventory[i].character.img.Length - 3);
                    Debug.Log(inventory[i].character.img.Length);
                    this.dataImage = System.Convert.FromBase64String(inventory[i].character.img);
                    this.mytexture = new Texture2D(1, 1);
                    this.mytexture.LoadImage(this.dataImage);

                    this.charSprite = Sprite.Create(this.mytexture, new Rect(0.0f, 0.0f, this.mytexture.width, this.mytexture.height), new Vector2(0.5f, 0.5f));
                }

                charDeskButton = Instantiate(charDeskButtonPrefab, charOptionsContent.transform);
                charDeskButton.name = inventory[i].character.char_name;

                //byte[] dataImage = System.Convert.FromBase64String(inventory[i].character.img);
                Texture2D mytexture = new Texture2D(1, 1);
                mytexture.LoadImage(dataImage);

                Sprite charSprite = Sprite.Create(mytexture, new Rect(0.0f, 0.0f, mytexture.width, mytexture.height), new Vector2(0.5f, 0.5f));

                charDeskButton.onClick.AddListener(delegate { SelectChar(inventory[i].character.char_name, charSprite, inventory[i].purchased, inventory[i].character.price); });
                charImage = Instantiate(charImage, charDeskButton.transform);


                charImage.GetComponent<Image>().sprite = charSprite;
                //charImage.GetComponent<Image>().sprite = entry.Value.Value;


                monsterName = Instantiate(monsterName, charDeskButton.transform);
                monsterName.text = inventory[i].character.char_name;

                if (!inventory[i].purchased)
                {
                    lockImage = Instantiate(lockImage, charDeskButton.transform);
                    charDeskButton.GetComponent<Image>().color = new Color(0.407f, 0.407f, 0.407f, 0.439f);

                }


            }

            /*foreach (KeyValuePair<string, KeyValuePair<int[], Sprite>> entry in dictionary)
            {
                //Debug.Log("Name: " + entry.Key);
                //Debug.Log("Price: " + entry.Value.Key[0]);
                //Debug.Log("Purchased: " + entry.Value.Key[1]);
                //Debug.Log("-----------------------------------");

                charDeskButton = Instantiate(charDeskButtonPrefab, charOptionsContent.transform);
                charDeskButton.name = entry.Key;
                charDeskButton.onClick.AddListener(delegate { SelectChar(entry.Key, entry.Value.Value, entry.Value.Key[1], entry.Value.Key[0]); });
                charImage = Instantiate(charImage, charDeskButton.transform);
                charImage.GetComponent<Image>().sprite = entry.Value.Value;

                monsterName = Instantiate(monsterName, charDeskButton.transform);
                monsterName.text = entry.Key;

                if(entry.Value.Key[1] == 0)
                {
                    lockImage = Instantiate(lockImage, charDeskButton.transform);
                    charDeskButton.GetComponent<Image>().color = new Color(0.407f, 0.407f, 0.407f, 0.439f);

                }
            }*/

            charPanelCreated = true;

        }

        if (index == 1 && !themePanelCreated)
        {
            for (int i = 0; i < ThemeSprites.ToArray().Length; i++)
            {

                charDeskButton = Instantiate(charDeskButtonPrefab, charOptionsContent.transform);
                //charImage.GetComponent<Image>().sprite = PlayerSprites[i];
                //charImage = Instantiate(charImage, charDesk.transform);
            }

            themePanelCreated = true;
        }
    }


    public void ChangeSegment(int index)
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
    }

    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        SoundManager.Instance.Play("Button");
    }



}
