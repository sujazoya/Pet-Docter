using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System.Collections;
using System;
using UnityEngine.Events;


public class RewardedAdManager : MonoBehaviour
{
    private string _REWadUnitId = "ca-app-pub-3940256099942544/5224354917";
    bool show_ad_as_index;
    private bool showAds;    
    public static bool show_rewarded;
    public static int show_rewarded_onrequest_count;  
    public static RewardedAdManager Instance;
    public RewardedButton[] rewardedButtons;
    public Image signal_img;
    [SerializeField] string rewID;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
         MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });       
        StartCoroutine(TryToFetch());       
    }

    IEnumerator TryToFetch()
    {
        
        yield return new WaitUntil(() => GoogleSheetHandler.googlesheetInitilized);
        show_rewarded = GoogleSheetHandler.show_rewarded;
        show_ad_as_index = GoogleSheetHandler.show_ad_as_index;
        show_rewarded_onrequest_count = int.Parse(GoogleSheetHandler.show_rewarded_onrequest_count);
        showAds = GoogleSheetHandler.showAds;
        rewID = GoogleSheetHandler.g_rewarded1;
        if (show_rewarded == true)
        {
            LoadRewardedAd();
        }
        
    }
    public static void Initialize(Action<InitializationStatus> initCompleteAction) { }
    #region REWARDED
   [HideInInspector]public RewardedAd Rewarded; 
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (Rewarded != null)
        {
            Rewarded.Destroy();
            Rewarded = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(rewID, adRequest,
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

                Rewarded = ad;
                RegisterReloadHandler(Rewarded);
                RegisterEventHandlers(Rewarded);
            });
    }   

    private void Update()
    {
        if (GoogleSheetHandler.googlesheetInitilized&&signal_img)
        {
            if(Rewarded.CanShowAd())
            {
                signal_img.color = Color.green;
            }
            else
            {
                signal_img.color = Color.red;
            }
        }
    }

    public void ShowRewardedAd()
    {
        if (Rewarded != null && Rewarded.CanShowAd())
        {
            Rewarded.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                // Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
        
    }    
  
    public void RegisterEventHandlers(RewardedAd ad)
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

    private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += ()=>
    {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }
    #endregion
}