using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleMobileAds.Api;

public class AdMobInitialize : MonoBehaviour
{
    private void Awake()
    {
        MobileAds.Initialize(InitializationStatus => { });
    }
}
