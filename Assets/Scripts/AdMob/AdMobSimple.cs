using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleMobileAds.Api;
public class AdMobSimple : MonoBehaviour
{
    private InterstitialAd interstitialAd;

#if UNITY_ANDROID
    private const string interstitialUnitId = "ca-app-pub-2915613541301682/5827522075"; // your ID or test id;
#elif UNITY_IPHONE
    private const string interstitialUnitId ="";
#else
    private const string interstitialUnitId = "unexpected_platform";
#endif
    private void OnEnable()
    {
        interstitialAd = new InterstitialAd(interstitialUnitId);
        AdRequest adRequest = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(adRequest);
    }

    public void ShowAd()
    {
        if (!GameManager.instance.noAdPurchased)
        {
            if (interstitialAd.IsLoaded())
            {
                interstitialAd.Show();
            }
        }

    }
}
