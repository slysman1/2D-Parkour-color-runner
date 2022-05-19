using UnityEngine;
using UnityEngine.UI;

public class UI_ShopController : MonoBehaviour
{
    private Player player;

    [SerializeField] private GameObject buttonToGenerate;

    [Header("Notification info")]
    public GameObject notificationButton;
    [SerializeField] private Transform notificationPosition;

    [Header("Display info")]
    public Image heroDisplayImage;
    public GameObject platformDisplay;
    [SerializeField] Text coinsOwned;


    [Header("Hero shop conent")]
    [SerializeField] private Color[] colorsForHero;
    [SerializeField] private int[] priceForHeroCollor;

    [Header("Platform shop conent")]
    [SerializeField] private Color[] colorsForPlatform;
    [SerializeField] private int[] priceForPlatformCollor;

    [Header("Shop parents")]
    [SerializeField] private Transform heroShopParent;
    [SerializeField] private Transform platformShopParent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        GenerateButtons();
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
            UI_ColorShopButton createdButtonScript = createdButton.GetComponent<UI_ColorShopButton>();



            createdButtonScript.colorToSell = Random.ColorHSV(); // used for random color
            createdButtonScript.priceForColor = Random.Range(25, 150); // used for random price

            // uncomment these if you want to set price and color by yourself
            //createdButtonScript.colorToSell = colorsForHero[i];
            //createdButtonScript.priceForColor = priceForHeroCollor[i];
            createdButtonScript.spriteToChange = player.GetComponent<SpriteRenderer>();
            //Debug.Log(colorsForHero[i].ToString());

            
        }

        //platform color shop generation
        for (int i = 0; i < colorsForPlatform.Length; i++)
        {
            GameObject createdButton = Instantiate(buttonToGenerate, platformShopParent);

            UI_ColorShopButton createdButtonScript = createdButton.GetComponent<UI_ColorShopButton>();

            createdButtonScript.colorToSell = colorsForPlatform[i];
            createdButtonScript.priceForColor = Random.Range(25, 150);

            //createdButtonScript.priceForColor = priceForPlatformCollor[i];

            createdButtonScript.transform.GetChild(0).GetComponent<Image>().sprite = null;
            createdButtonScript.transform.GetChild(0).localPosition = new Vector2(-280, 0);
            createdButtonScript.transform.GetChild(0).localScale = new Vector2(0.25f, 0.25f);
        }
    }

    public void PlayNotification(string notifiactionText)
    {
        GameObject notification = Instantiate(notificationButton, notificationPosition.transform.position, transform.rotation, notificationPosition);
        notification.GetComponent<Text>().text = notifiactionText;
    }
}
