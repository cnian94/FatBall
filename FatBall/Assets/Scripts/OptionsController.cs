using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour
{

    public GameObject charOptionsPanel;
    public GameObject charOptionsContent;

    public GameObject themeOptionsPanel;
    public GameObject themeOptionsContent;

    public Image charDeskPrefab;
    private Image charDesk;

    public Image charImage;

    public Button charBtn;
    public Button themeBtn;

    public List<Sprite> PlayerSprites = new List<Sprite>();
    public List<Sprite> ThemeSprites = new List<Sprite>();

    Color normalColor;
    Color selectedColor;

    public int selectedSegment = 0;

    // Use this for initialization
    void Start()
    {
        normalColor = new Color(1, 1, 1, 1);
        selectedColor = new Color(0.86f, 0.78f, 0.49f, 1);
        charBtn.GetComponent<Image>().color = selectedColor;

        SetPanel(0);

    }

    private void SetPanel(int index)
    {

        if (index == 0)
        {
            for (int i = 0; i < PlayerSprites.ToArray().Length; i++)
            {

                charDesk = Instantiate(charDeskPrefab, charOptionsContent.transform);
                charImage.GetComponent<Image>().sprite = PlayerSprites[i];
                charImage = Instantiate(charImage, charDesk.transform);
            }
        }

        if (index == 1)
        {
            for (int i = 0; i < ThemeSprites.ToArray().Length; i++)
            {

                charDesk = Instantiate(charDeskPrefab, themeOptionsContent.transform);
                //charImage.GetComponent<Image>().sprite = PlayerSprites[i];
                //charImage = Instantiate(charImage, charDesk.transform);
            }
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
