using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;
public class AdmobRewardedAd : MonoBehaviour
{
    private RewardedAd rewardedAd1;
    private RewardedAd rewardedAd2;
    private RewardedAd rewardedAd3;
    string adUnitId= "ca-app-pub-3940256099942544/5224354917";

    public Image rew1;
    public Image rew2;
    public Image rew3;

    private void Awake()
    {
        StartCoroutine(TryInitAds());
    }
    public IEnumerator TryInitAds()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            yield return new WaitUntil(() => Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
            Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork);
            InitAds();
        }
        else
        {
            InitAds();
        }

    }
    public void InitAds()
    {
        MobileAds.Initialize(initStatus => { });

        //Debug.Log("Admob Initializes");

    }
    public void Start()
    {
       rewardedAd1 = RequestRewardedAd(adUnitId);
       rewardedAd2 = RequestRewardedAd(adUnitId);
       rewardedAd3 = RequestRewardedAd(adUnitId);
       StartCoroutine(CheckAdLoaded());
    }
    public RewardedAd RequestRewardedAd(string adUnitId)
    {
        if (rewardedAd1 != null)
        {
            rewardedAd1.Destroy();
            rewardedAd1 = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd1 = ad;
            });    

        return rewardedAd1;
    }

    IEnumerator  CheckAdLoaded()
    {
        rew1.color = Color.red;
        while (rewardedAd1.CanShowAd())
        {
            yield return null;
        }
        //yield return new WaitUntil(() => AdmobAdmanager.Instance.interstitialAd[AdmobAdmanager.currentIntIndex].IsLoaded());
        rew1.color = Color.green;
        rew2.color = Color.red;
        while (rewardedAd2.CanShowAd())
        {
            yield return null;
        }
        //yield return new WaitUntil(() => AdmobAdmanager.Instance.interstitialAd[AdmobAdmanager.currentIntIndex].IsLoaded());
        rew2.color = Color.green;
        rew3.color = Color.red;
        while (rewardedAd3.CanShowAd())
        {
            yield return null;
        }
        //yield return new WaitUntil(() => AdmobAdmanager.Instance.interstitialAd[AdmobAdmanager.currentIntIndex].IsLoaded());
        rew3.color = Color.green;
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
}

