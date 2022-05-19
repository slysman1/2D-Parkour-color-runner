using UnityEngine;
using UnityEngine.UI;

public class UI_ColorShopButton : MonoBehaviour
{
    private UI_ShopController shopController;

    public Image myDisplayImage;
    private Text priceText;
    public SpriteRenderer spriteToChange;

    private bool buttonSelected;

    public Color colorToSell;
    public int priceForColor;

    public int myMoney = 1000;

    // Start is called before the first frame update
    void Start()
    {
        shopController = GameObject.Find("ColorShopUI").GetComponent<UI_ShopController>();

        myDisplayImage = transform.GetChild(0).GetComponent<Image>();
        priceText = transform.GetChild(1).GetComponent<Text>();

        myDisplayImage.color = colorToSell;
        priceText.text = priceForColor.ToString();

        

        AssignButtonComponents();
    }

    private void AssignButtonComponents()
    {
        this.gameObject.AddComponent<Button>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);
    }

    private void CheckIfCanBuy()
    {
        myMoney = GameManager.instance.coinsInBank;

        if (myMoney < priceForColor)
        {
            shopController.PlayNotification("Not enough money!");
        }
        else
        {
            if (spriteToChange == null)
            {
                Player player = GameObject.Find("Player").GetComponent<Player>();
                player.purchasedCollor = colorToSell;
                player.canColorPlatform = true;
            }
            else
            {
                spriteToChange.color = colorToSell;
            }

            GameManager.instance.SpendMoney(priceForColor);

            shopController.PlayNotification("Successful!");
        }
    }

   

    private void DisplayColors()
    {
        if (spriteToChange != null)
        {
            shopController.heroDisplayImage.color = colorToSell;
        }
        else
        {
            shopController.platformDisplay.GetComponent<SpriteRenderer>().color = colorToSell;
        }
    }

    private void ButtonClick()
    {
        if (!buttonSelected)
        {
            SwitchOffOtherButtons();
            buttonSelected = true;
            DisplayColors();
            shopController.PlayNotification("Click again to purchase");
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
            transform.parent.GetChild(i).GetComponent<UI_ColorShopButton>().buttonSelected = false;
        }
    }
}
