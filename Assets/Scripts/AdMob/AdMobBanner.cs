using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleMobileAds.Api;
using System;

public class AdMobBanner : MonoBehaviour
{
    private BannerView bannerView;

#if UNITY_ANDROID
    private const string bannerUnitId = "ca-app-pub-2915613541301682/7715201270"; // here is your ID
#elif Unity_IPHONE
    private const string bannerUnitId = "";
#else
    private const string bannerUnitId = "unexpected_Platform";
#endif
    private void OnEnable()
    {

        
        bannerView = new BannerView(bannerUnitId, AdSize.Banner, AdPosition.Top);
        AdRequest adRequest = new AdRequest.Builder().Build();

        if (!GameManager.instance.noAdPurchased)
        {
            bannerView.LoadAd(adRequest);
            bannerView.Show();
        }
        else
        {
            bannerView.Destroy();
        }
       
       // StartCoroutine(ShowBanner());
    }

    //IEnumerator ShowBanner()
    //{
    //    yield return new WaitForSecondsRealtime(5f);
    //    bannerView.Show();

    //}

}
