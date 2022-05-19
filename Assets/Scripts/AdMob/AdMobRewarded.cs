using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GoogleMobileAds.Api;
using System;

public class AdMobRewarded : MonoBehaviour
{
    public static AdMobRewarded instance;
    private RewindController rewindController;

    private RewardedAd rewardedAdForRewinds;
    private RewardedAd rewardedAdForCoins;
    private RewardedAd noRewardAd;

    public bool givingRewind;
    public bool givingCoins;

    //[SerializeField] private Text coinsText;
    //private int coins = 0;

#if UNITY_ANDROID
    private const string rewardedUnitIdForRewinds = "ca-app-pub-2915613541301682/7839387992"; // rewarded id or test id
    private const string rewardedUnitIdForCoins = "ca-app-pub-2915613541301682/9453289356"; // rewarded id or test id
    private const string rewardedUnitIdNoReward = "ca-app-pub-2915613541301682/3921761071"; // rewarded id or test id
#elif UNITY_IPHONE
    private const string rewardedUnitId = "";
#else
    private const string rewardedUnitId = "unexpected_platform" ;
#endif

    private void Awake()
    {
        instance = this;

        rewindController = GameObject.Find("Player").GetComponent<RewindController>();

        PlayerPrefs.SetInt("RewindAdWatched", 0);
    }

    private void OnEnable()
    {
        rewardedAdForRewinds = new RewardedAd(rewardedUnitIdForRewinds);
        AdRequest adRequestForRewind = new AdRequest.Builder().Build();
        rewardedAdForRewinds.LoadAd(adRequestForRewind);
        rewardedAdForRewinds.OnUserEarnedReward += HandlUserEarnRewardForRewinds;

        rewardedAdForCoins = new RewardedAd(rewardedUnitIdForCoins);
        AdRequest adReqestForCoins = new AdRequest.Builder().Build();
        rewardedAdForCoins.LoadAd(adReqestForCoins);
        rewardedAdForCoins.OnUserEarnedReward += HandlUserEarnRewardForCoins;

        noRewardAd = new RewardedAd(rewardedUnitIdNoReward);
        AdRequest adRequestNoReward = new AdRequest.Builder().Build();
        noRewardAd.LoadAd(adRequestNoReward);
        noRewardAd.OnUserEarnedReward += DoNothing;

    }

    private void DoNothing(object sender, Reward e)
    {
        throw new NotImplementedException();
    }

    private void RewardedAdForRewinds_OnAdClosed(object sender, EventArgs e)
    {
        PlayerPrefs.SetInt("RewindAdWatched", 1);
    }

    public void ShowRewardedAdForRewinds()
    {
        if (rewardedAdForRewinds.IsLoaded())
        {
            rewardedAdForRewinds.Show();
        }
    }
    
    public void ShowRewardedAdForCoins()
    {
        if (rewardedAdForCoins.IsLoaded())
        {
            rewardedAdForCoins.Show();
        }
    }

    public void ShowRewaredAdNoReward()
    {
        if (!GameManager.instance.noAdPurchased)
        {
            if (noRewardAd.IsLoaded())
            {
                noRewardAd.Show();
            }
        }

    }

    private void HandlUserEarnRewardForRewinds(object sender, Reward e)
    {         
        GameManager.instance.changeAmountOfRewinds(1);
    }
    private void HandlUserEarnRewardForCoins(object sender, Reward e)
    {
        int givingCoins = GameManager.instance.coinsCollected * 3;

        GameManager.instance.coinsCollected = givingCoins;
    }


}
