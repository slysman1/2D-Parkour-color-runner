
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    RewindController playerRewinder;

    #region Menu Items
    public GameObject comboCounterUI;
    [SerializeField] private GameObject aboutMeUI;
    [SerializeField] private GameObject quickSkillBTN;
    [SerializeField] private GameObject pauseBTN;
    [SerializeField] private GameObject muteBTN;
    [SerializeField] private GameObject abilityShopUI;
    [SerializeField] private GameObject donateShopUI;
    [SerializeField] private GameObject colorShopUI;
    [SerializeField] private GameObject quickSkillUI;
    [SerializeField] private GameObject achievementsUI;
    [SerializeField] private GameObject ranksUI;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject endGameUI;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject tapToStopRewindUI;

    [SerializeField] private GameObject[] menuItems;
    [SerializeField] private GameObject[] onTheseInPause;
    [SerializeField] private GameObject[] offTheseInPause;
    #endregion

    #region Coin and Score info
    [SerializeField] private Text lastScore;
    [SerializeField] private Text lastHighScore;
    [SerializeField] private Text[] currentScore;
    [SerializeField] private Text[] calculatedScore;
    [SerializeField] private Text[] coinsCollected;
    [SerializeField] private Text[] coinsInBank;
    [SerializeField] private Text[] amountOfRewinds;
    #endregion

    [Header("Particles")]

    [SerializeField] private ParticleSystem[] particleFX;

    public bool isMuted;
    public bool gameIsPaused;
    void Start()
    {
        SwitchUI(mainMenuUI);

        playerRewinder = GameObject.Find("Player").GetComponent<RewindController>();
    }

    // Update is called once per frame
    void Update()
    {
        CoinInfoUpdate();
        ScoreInfoUpdate();
        RewindsInfoUpdate();

        if (Input.GetKeyDown(KeyCode.K))
            TapToStartBTN();


        if (playerRewinder.isRewinding)
            tapToStopRewindUI.SetActive(true);
        else
            tapToStopRewindUI.SetActive(false);

    }

    #region Button's region


    public void GoQuickSkillPanelBTN()
    {
        quickSkillUI.GetComponent<UI_QuickSkillPanel>().countdownTime = 4;
        quickSkillUI.SetActive(true);
        quickSkillBTN.SetActive(false);
        muteBTN.SetActive(false);
        pauseBTN.SetActive(false);
        
        Time.timeScale = 0.4f;
    }
    public void CloseQuickSkillPanelBTN()
    {
        quickSkillUI.SetActive(false);
        quickSkillBTN.SetActive(true);
        muteBTN.SetActive(true);
        pauseBTN.SetActive(true);
        Time.timeScale = 1f;
    }

    public void GoAboutMeBTN()
    {
        SwitchUI(aboutMeUI);
    }
    public void GoDonateShopBTN()
    {
        SwitchUI(donateShopUI);
    }
    public void GoPassiveAbilityShopBTN()
    {
        SwitchUI(abilityShopUI);
    }
    public void GoColorShopBTN()
    {
        SwitchUI(colorShopUI);
    }

    public void GoAchivementsBTN()
    {
        AchievementManager.instance.UploadAchivementsInUI();

        SwitchUI(achievementsUI);
    }
    public void GoRanksBTN()
    {
        int coinsOwned = PlayerPrefs.GetInt("coinsInBank");
        int highScore = PlayerPrefs.GetInt("highScore");

        GPGSLeaderboardManager.instance.LeaderboardTopCoinsPostBTN();
        GPGSLeaderboardManager.instance.LeaderboardTopRankPostBTN();

        PlayfabManager.instance.SendCoinsToLeaderboard(coinsOwned);
        PlayfabManager.instance.SendScoreToLeaderboard(highScore);

        PlayfabManager.instance.GetScoreLbAroundPlayer();
        PlayfabManager.instance.GetСoinLbAroundPlayer();
        SwitchUI(ranksUI);
    }
    public void TapToStopRewindBTN()
    {
        playerRewinder.StopRewind();
    }
    public void TapToStartBTN()
    {
        GameManager.instance.GameStart();

        particleFX[0].Play();
        particleFX[1].Play();
        particleFX[2].Play();

        muteBTN.SetActive(true);
        pauseBTN.SetActive(true);
        quickSkillBTN.SetActive(true);

        SwitchUI(inGameUI);
    }
    public void GoSettingsBTN()
    {
        SwitchUI(settingsUI);
        muteBTN.SetActive(false);
    }
    public void GoMainMenuBTN()
    {
        AchievementManager.instance.SendAchiveUpdate(7, 1);
        AchievementManager.instance.DoIncrementalAchievemnt(GPGSIDs.achievement_game_has_just_begun);

        UI_DarkScreenController.instance.darkScreenOn = true;
        //GameManager.instance.GameRestart();
        Time.timeScale = 1;
        endGameUI.SetActive(false);
        pauseBTN.SetActive(false);
        GameManager.instance.SaveValues();

    }
    public void CloseBTN()
    {
        SwitchUI(mainMenuUI);
        muteBTN.SetActive(true);
    }
    public void GoPauseBTN()
    {
        gameIsPaused = !gameIsPaused;

        for (int i = 0; i < onTheseInPause.Length; i++)
        {
            onTheseInPause[i].SetActive(gameIsPaused);
        }
        for (int i = 0; i < offTheseInPause.Length; i++)
        {
            if(gameIsPaused)
                offTheseInPause[i].SetActive(false);
            else
                offTheseInPause[i].SetActive(true);
        }


        if (gameIsPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void MuteBTN()
    {
        isMuted = !isMuted; // works like a switcher
        AudioListener.pause = isMuted;

        #region Mute Icon Controller

        Image muteIcon = GameObject.Find("muteIcon").GetComponent<Image>(); // creating temp image and assigns it to image of muteIcon

        if (isMuted)
        {
            muteIcon.color = new Color(muteIcon.color.r, muteIcon.color.g, muteIcon.color.b, 0.6f);
        }
        else
        {
            muteIcon.color = new Color(muteIcon.color.r, muteIcon.color.g, muteIcon.color.b, 1f);
        }


        #endregion
    }


    #endregion

    public void SwitchUI(GameObject uiToActivate)
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].SetActive(false);
        }

        AudioManager.instance.PlaySFX(1);
        uiToActivate.SetActive(true);
    }


    public void SwitchToEndGameUI()
    {   
        SwitchUI(endGameUI);

        //int coinsCollected = GameManager.instance.coinsCollected;
    }
    private void CoinInfoUpdate()
    {
        for (int i = 0; i < coinsCollected.Length; i++)
        {
            if (GameManager.instance.coinsCollected == 0)
                coinsCollected[i].text = "0";
            else
                coinsCollected[i].text = GameManager.instance.coinsCollected.ToString("#,#");
        }

        for (int i = 0; i < coinsInBank.Length; i++)
        {
                coinsInBank[i].text = GameManager.instance.coinsInBank.ToString("#,#");
        }
    }
    private void ScoreInfoUpdate()
    {
        for (int i = 0; i < currentScore.Length; i++)
        {
            currentScore[i].text = Mathf.Round(GameManager.instance.currentMetersRan).ToString("#,#");
        }

        for (int i = 0; i < calculatedScore.Length; i++)
        {
            calculatedScore[i].text = (GameManager.instance.currentMetersRan * GameManager.instance.coinsCollected).ToString("#,#");
        }
        lastScore.text = PlayerPrefs.GetInt("lastScore", 0).ToString("#,#");
        lastHighScore.text = PlayerPrefs.GetInt("highScore").ToString("#,#");
    }
    private void RewindsInfoUpdate()
    {
        for (int i = 0; i < amountOfRewinds.Length; i++)
        {
            amountOfRewinds[i].text = GameManager.instance.amountOfRewinds.ToString();
        }
    }
}
