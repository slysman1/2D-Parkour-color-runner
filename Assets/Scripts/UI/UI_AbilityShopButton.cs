using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_AbilityShopButton : MonoBehaviour
{
    private Player player;
    public GameObject donateButton;
    public SpriteRenderer spriteToChange;
    public Image myDisplayImage;
    private Text priceText;
    private UI_AbilityShop AbilitiyShopUI;

    public Text descriptionText;


    public int myId;
    private bool buttonSelected;

    public string skillName;
    public string skillDescription;
    public string colorCodeToSell;
    public Color colorToSell;
    public int priceForColor;

    // Start is called before the first frame update
    void Start()
    {

        AbilitiyShopUI = GameObject.Find("PassiveAbilityShopUI").GetComponent<UI_AbilityShop>();
        player = GameObject.Find("Player").GetComponent<Player>();

        myDisplayImage = transform.GetChild(0).GetComponent<Image>();
        priceText = transform.GetChild(1).GetComponent<Text>();

        myDisplayImage.color = colorToSell;
        priceText.text = priceForColor.ToString("#,#");

        AssignButtonComponents();
    }

    private void OnDisable()
    {
        player.CheckWhatIsPurchased();
    }



    private void AssignButtonComponents()
    {
        this.gameObject.AddComponent<Button>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);
    }

    private void CheckIfCanBuy()
    {
        int myMoney = GameManager.instance.coinsInBank;
        bool purchasedMade = PlayerPrefs.GetInt(skillName) == 1;

        if (!purchasedMade)
        {
            if (myMoney < priceForColor)
            {
                AbilitiyShopUI.PlayNotification("Not enough money!");
            }
            else
            {
                MakePuchase();
                AbilitiyShopUI.PlayNotification("Successful!");
            }
        }
        else
        {
            if (ColorUtility.TryParseHtmlString(colorCodeToSell, out colorToSell))
            {
                spriteToChange.color = colorToSell;
                PlayerPrefs.SetString("myColorCode", colorCodeToSell);
                AbilitiyShopUI.PlayNotification("Successful!");
            }
        }
        
    }

    private void MakePuchase()
    {
        if (ColorUtility.TryParseHtmlString(colorCodeToSell, out colorToSell))
        {
            spriteToChange.color = colorToSell;
        }

        PlayerPrefs.SetString("myColorCode", colorCodeToSell);
        GameManager.instance.SpendMoney(priceForColor);
        PlayerPrefs.SetInt(skillName, 1);
    }


    private void DisplayColors()
    {
        if (spriteToChange != null)
        {
            AbilitiyShopUI.heroDisplayImage.color = colorToSell;
        }

    }

    private void ButtonClick()
    {
        if (!buttonSelected)
        {
            SwitchOffOtherButtons();
            buttonSelected = true;
            DisplayColors();
            descriptionText.text = skillDescription;

            bool purchasedMade = PlayerPrefs.GetInt(skillName) == 1;
            if (!purchasedMade)
            {
                AbilitiyShopUI.PlayNotification("Click again to purchase");
            }
            else
            {
                AbilitiyShopUI.PlayNotification("Click again to select color");
            }

            #region donate buttons switcher // don't look here , it's ugly


            if (myId < 3)
            {
                donateButton.SetActive(false);
            }
            else 
            {
                donateButton.SetActive(true);
            }

            #endregion
        }
        else
        {
            CheckIfCanBuy();
        }
    }

    private void SwitchOffOtherButtons()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).GetComponent<UI_AbilityShopButton>().buttonSelected = false;
            AbilitiyShopUI.SwitchOffDonateButtons();
        }
    }
}
