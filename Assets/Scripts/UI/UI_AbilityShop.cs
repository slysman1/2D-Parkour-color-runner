using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AbilityShop : MonoBehaviour
{
    private Player player;

    [SerializeField] private GameObject buttonToGenerate;
    [SerializeField] private GameObject[] donateButtons;

    [Header("Notification info")]
    public GameObject notificationButton;
    [SerializeField] private Transform notificationPosition;

    [Header("Display info")]
    public Image heroDisplayImage;
    [SerializeField] Text coinsOwned;
    [SerializeField] Text skilldescriptionText;


    [Header("Hero shop conent")]
    [SerializeField] private Color[] colorsForHero;
    [SerializeField] private string[] colorCodeToSell;
    [SerializeField] private int[] priceForHeroCollor;
    public string[] skillName;
    [TextArea]
    public string[] skillDescription;


    [Header("Shop parents")]
    [SerializeField] private Transform heroShopParent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        GenerateButtons();
        SwitchOffDonateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        coinsOwned.text = GameManager.instance.coinsInBank.ToString("#,#");
    }

    private void GenerateButtons()
    {
        // hero color shop generation
        for (int i = 0; i < colorsForHero.Length; i++)
        {
            GameObject createdButton = Instantiate(buttonToGenerate, heroShopParent);
            UI_AbilityShopButton createdButtonScript = createdButton.GetComponent<UI_AbilityShopButton>();
            createdButtonScript.priceForColor = priceForHeroCollor[i];
            createdButtonScript.spriteToChange = player.GetComponent<SpriteRenderer>();
            createdButtonScript.skillName = skillName[i];
            createdButtonScript.colorCodeToSell = colorCodeToSell[i];
            createdButtonScript.descriptionText = skilldescriptionText;
            createdButtonScript.skillDescription = skillDescription[i];
            createdButtonScript.colorToSell = colorsForHero[i];
            createdButtonScript.donateButton = donateButtons[i];
            createdButtonScript.myId = i;

            




            //createdButtonScript.colorToSell = Random.ColorHSV(); // used for random color
            //createdButtonScript.priceForColor = Random.Range(5, 150); // used for random price

        }
    }

    public void PlayNotification(string notifiactionText)
    {
        GameObject notification = Instantiate(notificationButton, notificationPosition.transform.position, transform.rotation, notificationPosition);
        notification.GetComponent<Text>().text = notifiactionText;
    }

    public void SwitchOffDonateButtons()
    {
        for (int i = 0; i < donateButtons.Length; i++)
        {
            donateButtons[i].SetActive(false);
        }
    }
}


