using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using UnityEngine;
using UnityEngine.UI;

using GooglePlayGames;



public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;


    public Text logText;

    [SerializeField] Transform achivParent;
    [SerializeField] GameObject achivPref;

    public string[] achivTittle;
    public string[] achivDescirption;
    public int[] achiveMaxCount;
    public int[] achiveCounter;
    public int[] achiveRewardAmount;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < achiveCounter.Length; i++)
        {
            achiveCounter[i] =  PlayerPrefs.GetInt(achivTittle[i]);
        }

        


        //PlayerPrefs.SetInt(achivTittle[3], 5000);
    }
    public void UploadAchivementsInUI()
    {
        foreach (Transform item in achivParent)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < achivTittle.Length; i++)
        {
            GameObject newAchiveContent = Instantiate(achivPref, achivParent);
            Text[] achiveInfo = newAchiveContent.GetComponentsInChildren<Text>();
            Slider achievSlider = newAchiveContent.GetComponentInChildren<Slider>();
           

            UI_AchiveRewardButton rewardButtonScript = newAchiveContent.GetComponentInChildren<UI_AchiveRewardButton>();
            rewardButtonScript.achivName = achivTittle[i];

            int myReward = achiveRewardAmount[i];
            int maxValue = achiveMaxCount[i];
            int value = PlayerPrefs.GetInt(achivTittle[i]);

            rewardButtonScript.SetMyValues(myReward, value, maxValue);

            if (achivTittle[i].Length >= 30)
            {
                achiveInfo[0].fontSize = 45;
            }
            else if (achivTittle[i].Length >= 20)
            {
                achiveInfo[0].fontSize = 50;
            }
                achiveInfo[0].text = achivTittle[i];
            achiveInfo[1].text = achivDescirption[i];
            achiveInfo[2].text = value.ToString("#,#") + " / " + maxValue.ToString("#,#");

            achievSlider.maxValue = maxValue;
            achievSlider.value = value;
        }
    }

    public void GiveReward(int reward)
    {
        GameManager.instance.coinsInBank += reward;
    }

    public void SendAchiveUpdate(int achiveID, int achiveValue)
    {
        int currentAchiveValue = PlayerPrefs.GetInt(achivTittle[achiveID]);

        PlayerPrefs.SetInt(achivTittle[achiveID], currentAchiveValue += achiveValue);

        //Debug.Log("<color=green>Playfab: </color> Achievemnt Tittle:" + achivTittle[achiveID]);
        //Debug.Log("<color=green>Playfab: </color> Achievemnt status was:" + currentAchiveValue);
        //Debug.Log("<color=green>Playfab: </color> Achievemnt status is:" + currentAchiveValue);

        #region Check For 50,000 coins collected

        int totalCollected = PlayerPrefs.GetInt(achivTittle[5]);

        if (totalCollected >= achiveMaxCount[5])
        {
            DoGrantAchievement(GPGSIDs.achievement_scrooge_mcduck_say_what);
        }

        #endregion
        #region Check For 50,000 coins spent

        int totalSpent = PlayerPrefs.GetInt(achivTittle[6]);

        if (totalCollected >= achiveMaxCount[6])
        {
            DoGrantAchievement(GPGSIDs.achievement_spendthrifter);
        }
        #endregion
        #region Check For 15,000 coins in bank

        int coinsOwned = PlayerPrefs.GetInt("coinsOwned");
        if (coinsOwned >= achiveMaxCount[4])
        {
            DoGrantAchievement(GPGSIDs.achievement_ill_build_a_houseplant_a_tree_and_buy_ps5);
        }

        #endregion
        #region Check For 100,000 colored paltforms

        bool shouldBeUnlocked = PlayerPrefs.GetInt(achivTittle[13]) > achiveMaxCount[13];
        if (shouldBeUnlocked)
        {
            DoGrantAchievement(GPGSIDs.achievement_you_are_hell_of_a_painterarent_you);
        }

        #endregion

    }


    #region googleAchivements
    public void ShowAchievementsUI()
    {
        Social.ShowAchievementsUI();
    }

    public void DoGrantAchievement(string achievement)
    {
        Social.ReportProgress(achievement,
            100.00f,
            (bool success) =>
            {
                if (success) // 
                {
                    logText.text = achievement + " : " + success.ToString();
                }
                else
                {
                    logText.text = achievement + " : " + success.ToString();
                }
            });
    }

    public void DoIncrementalAchievemnt(string achievement)
    {
        PlayGamesPlatform platform = (PlayGamesPlatform)Social.Active;

        platform.IncrementAchievement(achievement,
            1, (bool success) =>
            {
                if (success) // 
                {
                    logText.text = achievement + " : " + success.ToString();
                }
                else
                {
                    logText.text = achievement + " : " + success.ToString();
                }
            });
    }

    public void DoRevealAchievement(string achievement)
    {
        Social.ReportProgress(achievement,
            0.00f,
            (bool success) =>
            {
                if (success) // 
                {
                    logText.text = achievement + " : " + success.ToString();
                }
                else
                {
                    logText.text = achievement + " : " + success.ToString();
                }
            });
    }

    public void ListAchievements()
    {
        Social.LoadAchievements(achievements =>
        {
            logText.text = "Loaded achievements" + achievements.Length;
            foreach (IAchievement ach in achievements)
            {
                logText.text += "/n" + ach.id + " " + ach.completed;
            }
        });
    }

    public void ListDescriptions()
    {
        Social.LoadAchievementDescriptions(achievements =>
        {
            logText.text = "Loaded achievements" + achievements.Length;
            foreach (IAchievementDescription ach in achievements)
            {
                logText.text += "/n" + ach.id + " " + ach.title;
            }
        });
    }


    #endregion

}
