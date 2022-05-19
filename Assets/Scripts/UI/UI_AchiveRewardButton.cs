using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AchiveRewardButton : MonoBehaviour
{
    Text[] rewardTexts;
    Image[] icons;

    bool canGrantReward;

    public int achivCount;
    public int achivMaxCount;
    public int myReward;
    public string achivName;
    private bool rewardGranted;


    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<Button>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(Reward);


        rewardGranted = PlayerPrefs.GetInt(achivName) == 1;

    }



    public void SetMyValues(int reward, int _achiveCount, int _achieMaxCount)
    {
        rewardTexts = GetComponentsInChildren<Text>();
        icons = transform.parent.GetComponentsInChildren<Image>();


        myReward = reward;
        achivCount = _achiveCount;
        achivMaxCount = _achieMaxCount;
        rewardTexts[0].text = reward.ToString("#,#");

        canGrantReward = achivCount >= achivMaxCount;

        if (canGrantReward)
            rewardTexts[1].text = "Reward avalible!";
        else
            icons[1].gameObject.SetActive(false);
        
    }
    private void Reward()
    {
        if (canGrantReward && !rewardGranted)
        {
            int tempCoins = PlayerPrefs.GetInt("coinsInBank") + myReward;
            PlayerPrefs.SetInt("coinsInBank", tempCoins);
            PlayerPrefs.SetInt(achivName, 1);

            rewardGranted = PlayerPrefs.GetInt(achivName) == 1;

            Debug.Log("<color=purple>Playfab: </color>granted" + tempCoins + " coins!");
        }
        else if (rewardGranted && canGrantReward)
        {
            rewardTexts[1].text = "One time is enough";
            StartCoroutine(switchOffNotification());
        }
        else
        {
            rewardTexts[1].text = "Not completed!";
            StartCoroutine(switchOffNotification());
        }
        

    }

    IEnumerator switchOffNotification()
    {
        yield return new WaitForSeconds(0.5f);
        rewardTexts[1].text = "";
    }


}
