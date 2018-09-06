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


    public List<Sprite> PlayerSprites = new List<Sprite>();
    public List<Sprite> ThemeSprites = new List<Sprite>();

    public Dictionary<string, KeyValuePair<int[], Sprite>> dictionary = new Dictionary<string, KeyValuePair<int[], Sprite>>();


    Color normalColor;
    Color selectedColor;

    public int selectedSegment = 0;


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
        normalColor = new Color(1, 1, 1, 1);
        selectedColor = new Color(0.86f, 0.78f, 0.49f, 1);
        charBtn.GetComponent<Image>().color = selectedColor;

        CreateDictionary();

        SetPanel(0);

    }

    public void SelectChar(string name, Button charDesk)
    {
        Debug.Log(name);
        selectedImage = Instantiate(selectedImage, charDeskButton.transform);
    }


    private void SetPanel(int index)
    {

        if (index == 0 && !charPanelCreated)
        {

            foreach (KeyValuePair<string, KeyValuePair<int[], Sprite>> entry in dictionary)
            {
                //Debug.Log("Name: " + entry.Key);
                //Debug.Log("Price: " + entry.Value.Key[0]);
                //Debug.Log("Purchased: " + entry.Value.Key[1]);
                //Debug.Log("-----------------------------------");

                //charDesk = Instantiate(charDeskPrefab, charOptionsContent.transform);
                charDeskButton = Instantiate(charDeskButtonPrefab, charOptionsContent.transform);
                charDeskButton.name = entry.Key;
                charDeskButton.onClick.AddListener(delegate { SelectChar(entry.Key, charDeskButton); });
                charImage.GetComponent<Image>().sprite = entry.Value.Value;
                charImage = Instantiate(charImage, charDeskButton.transform);

                monsterName = Instantiate(monsterName, charDeskButton.transform);
                monsterName.GetComponent<Text>().text = entry.Key;

                if(entry.Value.Key[1] == 0)
                {
                    lockImage = Instantiate(lockImage, charDeskButton.transform);
                    charDeskButton.GetComponent<Image>().color = new Color(0.407f, 0.407f, 0.407f, 0.439f);

                }
            }

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
