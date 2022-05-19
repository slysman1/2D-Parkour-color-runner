using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Components
    private AdMobSimple adMobSimple;
    private Player player;
    private UI UI;
    public ComboControllerUI comboController;
    #endregion
    #region Coin info

    public int coinsCollected;
    public int coinsInBank;
    public int amountOfRewinds;
    

    #endregion
    #region Score info
    public int currentMetersRan;
    public int lastScoreCalculated;
    public int highScore;

    private int calculatedScore;
    private bool canIncreaseScore;
    #endregion

    public bool noAdPurchased;
    public bool connectedToCloud;

    [Header("Skybox material")]
    [SerializeField] private Material[] skyBox;

    [SerializeField] private string mainScene;



    private void Awake()
    {
        instance = this;


        Time.timeScale = 1;
        noAdPurchased = PlayerPrefs.GetInt("NoAds") == 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        adMobSimple = GameObject.Find("AdMobManager").GetComponent<AdMobSimple>();
        player = GameObject.Find("Player").GetComponent<Player>();
        UI = GameObject.Find("Canvas").GetComponent<UI>();

        ShowSimpleAd();

        int weatherId = Random.Range(0, skyBox.Length);
        Debug.Log("Weather ID:" + weatherId + "Weather name:" + skyBox[weatherId]);
        RenderSettings.skybox = skyBox[weatherId] ;

        LoadValues(); // loads values of player prfs
        AudioManager.instance.PlayBGM(Random.Range(0, 2));

        if (weatherId == 2)
        {
            AchievementManager.instance.SendAchiveUpdate(15, 1);
            AchievementManager.instance.DoIncrementalAchievemnt(GPGSIDs.achievement_oh_imagine_a_landits_a_faraway_place);
        }

        

    }

    // Update is called once per frame
    void Update()
    {
        ScoreIncreaseCheck();

        if (player.unlockedSkillNumber[5])
        {
            amountOfRewinds = 99;
        }
    }

    private void ShowSimpleAd()
    {
        int adCounter = PlayerPrefs.GetInt("ShowSimpleAd");
        adCounter++;
        PlayerPrefs.SetInt("ShowSimpleAd", adCounter);

        if (adCounter > 3)
        {
            adMobSimple.ShowAd();
            adCounter = 0;
            PlayerPrefs.SetInt("ShowSimpleAd", adCounter);
        }
    }

    public void GameStart()
    {
        player.canRun = true;
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(mainScene);
    }


    public void AddCoin()
    {
        if (comboController.comboCounter > 1)
        {
            coinsCollected += 1 * comboController.comboCounter;
        }else
        {
            coinsCollected++;
        }
    }

    public void ResurrectPlayer()
    {
        player.ResurectPlayer();
        UI.TapToStartBTN();
    }

    public void GameEnds()
    {
        bool isRewinding = player.GetComponent<RewindController>().isRewinding;

        if (!isRewinding)
        {
            calculatedScore = currentMetersRan * coinsCollected; // multiplies meters ran and coins collected to check final score
            UI.SwitchToEndGameUI(); // final score passsing to UI
            Time.timeScale = 1;




            #region play extra advertisment
            int extraAdCounter = PlayerPrefs.GetInt("ExtraAdCounter");
            extraAdCounter++;
            PlayerPrefs.SetInt("ExtraAdCounter", extraAdCounter);
            if (PlayerPrefs.GetInt("ExtraAdCounter") == 7)
            {
                AdMobRewarded.instance.ShowRewaredAdNoReward();
                PlayerPrefs.SetInt("ExtraAdCounter", 0);
            }

            #endregion
        }
        else
        {
            
        }



    }

    public void MultiplyCoins(int multiplierValue)
    {
        coinsCollected += coinsCollected * multiplierValue;
    }

    public void SaveValues()
    {    

        PlayerPrefs.SetInt("coinsInBank", coinsInBank + coinsCollected); // sums coins in bank and coins collected and saves it
        PlayerPrefs.SetInt("lastScore", calculatedScore); // saving last score
        PlayerPrefs.SetInt("rewinds", amountOfRewinds);

        if (calculatedScore > highScore)
             PlayerPrefs.SetInt("highScore", calculatedScore); // changing high score to last score if it's bigger


        AchievementManager.instance.SendAchiveUpdate(5, coinsCollected);

        if (connectedToCloud)
        {
            GameObject.Find("GPGS_LoginManager").GetComponent<GPGSManager>().OpenSave(true);
        }else
        {
            GameRestart();
        }

    }

    public void ExtraSaveValuse()
    {
        PlayerPrefs.SetInt("extraSave", 1);
        PlayerPrefs.SetInt("extraSaveCoinsInBank", coinsInBank + coinsCollected); // sums coins in bank and coins collected and saves it
        PlayerPrefs.SetInt("extraSaveLastScore", calculatedScore); // saving last score
        PlayerPrefs.SetInt("extraSaveRewinds", amountOfRewinds);

        if (calculatedScore > highScore)
            PlayerPrefs.SetInt("extraHighScore", calculatedScore); // changing high score to last score if it's bigger
    }

    public void changeAmountOfRewinds(int addAmount)
    {
         amountOfRewinds = PlayerPrefs.GetInt("rewinds");
         PlayerPrefs.SetInt("rewinds", amountOfRewinds += addAmount);
         amountOfRewinds = PlayerPrefs.GetInt("rewinds");
         Debug.Log(amountOfRewinds);
    }

    public void LoadValues()
    {
        bool haveExtraSave = PlayerPrefs.GetInt("extraSave") == 1;

        if (haveExtraSave)
        {
            //get values from extra save instead of ussuals
            coinsInBank = PlayerPrefs.GetInt("extraSaveCoinsInBank"); // sums coins in bank and coins collected and saves it
            lastScoreCalculated = PlayerPrefs.GetInt("extraSaveLastScore"); // saving last score
            highScore = PlayerPrefs.GetInt("extraSaveHighScore"); // changing high score to last score if it's bigger
            amountOfRewinds = PlayerPrefs.GetInt("extraSaveRewinds");


            ///set extra save to zero
            PlayerPrefs.SetInt("extraSave", 0);
            PlayerPrefs.SetInt("extraSaveCoinsInBank",0);
            PlayerPrefs.SetInt("extraSaveLastScore", 0);
            PlayerPrefs.SetInt("extraSaveRewinds", 0);
            PlayerPrefs.SetInt("extraHighScore", 0);

        }
        else
        {
            coinsInBank = PlayerPrefs.GetInt("coinsInBank");
            lastScoreCalculated = PlayerPrefs.GetInt("lastScore");
            highScore = PlayerPrefs.GetInt("highScore");
            amountOfRewinds = PlayerPrefs.GetInt("rewinds");
        }
        

    }

    private void ScoreIncreaseCheck()
    {
        if (canIncreaseScore)
            currentMetersRan = Mathf.RoundToInt(player.rb.transform.position.x);
        

        if (player.rb.velocity.x > 0)
            canIncreaseScore = true;
        else
            canIncreaseScore = false;
    }

    public void SpendMoney(int price)
    {
        coinsInBank -= price;

        PlayerPrefs.SetInt("coinsInBank", coinsInBank);
        AchievementManager.instance.SendAchiveUpdate(6, price);

        //int moneySpent = UI_Achievemnts.instance.achiveCounter[6];

        //Debug.Log(moneySpent);
    }

    private void OnApplicationQuit()
    {
        SaveValues();
    }
}
