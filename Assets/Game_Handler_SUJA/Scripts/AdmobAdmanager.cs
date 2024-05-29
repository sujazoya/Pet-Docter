

using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System.Collections;
using System;

public class AdmobAdmanager : MonoBehaviour
{  

    private int bannerIndex = 2;
    [HideInInspector] public BannerView[] bannerView;
    public string[] bannerID;
    public AdPosition bannerPose = AdPosition.Top;


    private int intIndex = 3;
    public InterstitialAd[] interstitialAd;
    public string[] interstitiaAdID;   
    private int intRewIndex = 3;
    public string[] rewardedIntadUnitId;   
    [HideInInspector]public AdRequest[] requestInterstitial;
    [HideInInspector] public AdRequest[] requestRewarded;
    [HideInInspector] public AdRequest[] requestRewardedInterstitialAd;
    public RewardedInterstitialAd rewardedInterstitialAd;
    [SerializeField] Text timerText;
    [SerializeField] Button noThanks_Button;
    [SerializeField] Animator popAnim;
    [SerializeField] GameObject popupPanel;
    private bool showAds;
    public static bool readyToShoAd;
    bool show_ad_as_index;

    #region GestAdIDsAndRequestAds()
    
    public void GestAdIDsAndRequestAds()
    {
        showAds = GoogleSheetHandler.showAds;
        //Set refrencess here
        bannerView = new BannerView[2];
        interstitialAd = new InterstitialAd[3];      
        requestInterstitial = new AdRequest[3];       
        requestRewarded = new AdRequest[3];
        requestRewardedInterstitialAd = new AdRequest[3];
       


        // BANNER SECTION
        bannerID = new string[bannerIndex];
        bannerID[0] = GoogleSheetHandler.g_banner1;

        //Debug.Log("Banner1 ->" + bannerID[0]);

        RequestBanner(bannerID[0], 0);
        bannerID[1] = GoogleSheetHandler.g_banner2;
        RequestBanner(bannerID[1], 1);
        //bannerView = new BannerView[bannerIndex];

        // interstitia SECTION

        interstitiaAdID = new string[intIndex];
        interstitiaAdID[0] = GoogleSheetHandler.g_inter1;
        RequestInterstitialAdAd(interstitiaAdID[0], 0);
        interstitiaAdID[1] = GoogleSheetHandler.g_inter2;
        RequestInterstitialAdAd(interstitiaAdID[1], 1);
        interstitiaAdID[2] = GoogleSheetHandler.g_inter3;
        RequestInterstitialAdAd(interstitiaAdID[2], 2);

        // rewardedIntadUnit SECTION
        rewardedIntadUnitId = new string[intRewIndex];
        rewardedIntadUnitId[0] = GoogleSheetHandler.g_rewardedint1;
        LoadRewardedInterstitialAd(rewardedIntadUnitId[0]);
        rewardedIntadUnitId[1] = GoogleSheetHandler.g_rewardedint2;
        LoadRewardedInterstitialAd(rewardedIntadUnitId[1]);
        rewardedIntadUnitId[2] = GoogleSheetHandler.g_rewardedint3;
        LoadRewardedInterstitialAd(rewardedIntadUnitId[2]);       

        show_ad_as_index = GoogleSheetHandler.show_ad_as_index;
          //Debug.Log($"---{rewardedAdID[0]}\n{rewardedAdID[1]}\n{rewardedAdID[2]}");
        readyToShoAd = true;       
        //StartCoroutine(ShowRewardedInt());
        //Invoke("ShowInterstitial", 3);

    }
    #endregion
   


    #region SHOW AD IN GAP   
    IEnumerator ShowRewardedInt()
    {
        yield return new WaitForSeconds(5);
        ShowRewardedInterstitialAd();
        //StartCoroutine(ShowRewarded());
    }

    private static AdmobAdmanager _instance;

    public static AdmobAdmanager Instance { get { return _instance; } }

    #endregion
    private void Awake()
    {
       
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        StartCoroutine(TryInitAds());        
    }
    //private bool loaded()
    //{
    //    return string.IsNullOrEmpty(bannerID) && string.IsNullOrEmpty(interstitiaAdID) && string.IsNullOrEmpty(rewardedAdID);
    //}
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
    private void Start()
    {       
        if (noThanks_Button)
        {
            noThanks_Button.onClick.AddListener(DontShowAd);
        }
        if (popupPanel)
        {
            popupPanel.SetActive(false);
        }      
        //ShowInterstitial();
        StartCoroutine(ShowBanner());       
    }

    public void InitAds()
    {
        MobileAds.Initialize(initStatus => { });    
 
        //Debug.Log("Admob Initializes");
      
    }
    private RewardedInterstitialAd _rewardedInterstitialAd;

    public void LoadRewardedInterstitialAd(string rewIntID)
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedInterstitialAd != null)
        {
            _rewardedInterstitialAd.Destroy();
            _rewardedInterstitialAd = null;
        }

        Debug.Log("Loading the rewarded interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        RewardedInterstitialAd.Load(rewIntID, adRequest,
            (RewardedInterstitialAd ad, LoadAdError error) =>
            {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
                {
                    Debug.LogError("rewarded interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedInterstitialAd = ad;
            });
    }

    #region REWARDED INTERTITIL

    bool showRewardIntAd;
    IEnumerator ShowRewardedInterstitialStarting_Popup()
    {
        popupPanel.SetActive(true);
        popAnim.enabled = true;
        timerText.text = "00.5";
        yield return new WaitForSeconds(0.5f);
        popAnim.enabled = false;
        showRewardIntAd = true;
        int t = 5;
        while (t > 0)
        {
            t--;
            timerText.text = "00: "
                + t;
            yield return new WaitForSeconds(1);
        }
        popAnim.enabled = true;
        if (showRewardIntAd)
        {
            ContineuShowRewardedInterstitialAd();
        }
        StartCoroutine(popup());
    }
    void DontShowAd()
    {
        showRewardIntAd = false;
        popAnim.enabled = true;
        StartCoroutine(popup());
    }
    IEnumerator popup()
    {
        yield return new WaitForSeconds(1);
        popupPanel.SetActive(false);
    }    
     public void ShowRewardedInterstitialAd()
    {
        const string rewardMsg =
            "Rewarded interstitial ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedInterstitialAd != null && rewardedInterstitialAd.CanShowAd())
        {
            rewardedInterstitialAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }
    private void ContineuShowRewardedInterstitialAd()
    {
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Show(userEarnedRewardCallback);
        }
    }
    private void userEarnedRewardCallback(Reward reward)
    {
        // TODO: Reward the user.
        //if (FindObjectOfType<GameScene>())
        {
           // FindObjectOfType<GameScene>().OnVideoRewarded();
        }
    }
    public int CurrentReawIntIndex()
    {
        if (GoogleSheetHandler.show_g_rewardedint1 == true)
        {
            return 0;
        }
        else if (GoogleSheetHandler.show_g_rewardedint1 == true)
        {
            return 1;
        }
        else
             if (GoogleSheetHandler.show_g_rewardedint1 == true)
        {
            return 2;
        }
        else
            return 0;

    }
    private void RegisterEventHandlers(RewardedInterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded interstitial ad failed to open " +
                           "full screen content with error : " + error);
        };
    }
    private void RegisterReloadHandler(RewardedInterstitialAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += ()=>
    {
            Debug.Log("Rewarded interstitial ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedInterstitialAd(rewardedIntadUnitId[0]);
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded interstitial ad failed to open " +
                           "full screen content with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedInterstitialAd(rewardedIntadUnitId[0]);
        };
    }
    private void HandleAdFailedToPresent(object sender, AdErrorEventArgs args)
    {
        //MonoBehavior.print("Rewarded interstitial ad has failed to present.");
    }

    private void HandleAdDidPresent(object sender, EventArgs args)
    {
        //MonoBehavior.print("Rewarded interstitial ad has presented.");
    }
    #endregion   

   public int CurrentIntIndex()
    {
        if (GoogleSheetHandler.show_g_inter1 == true)
        {
            return 0;
        }
        else if (GoogleSheetHandler.show_g_inter2 == true)
        {
            return 1;
        }else
             if (GoogleSheetHandler.show_g_inter3 == true)
        {
            return 2;
        }else
            return 0;

    }
    public int CurrentRewIntIndex()
    {
        if (GoogleSheetHandler.show_g_rewardedint1 == true)
        {
            return 0;
        }
        else if (GoogleSheetHandler.show_g_rewardedint1 == true)
        {
            return 1;
        }
        else
             if (GoogleSheetHandler.show_g_rewardedint1 == true)
        {
            return 2;
        }
        else
            return 0;

    }
    public static int Banner_Index
    {
        get { return PlayerPrefs.GetInt("Banner_Index", 0); }
        set { PlayerPrefs.SetInt("Banner_Index", value); }
    }
    public static int Int_Index
    {
        get { return PlayerPrefs.GetInt("Int_Index", 0); }
        set { PlayerPrefs.SetInt("Int_Index", value); }
    }
    public static int Rew_Index
    {
        get { return PlayerPrefs.GetInt("Rew_Index", 0); }
        set { PlayerPrefs.SetInt("Rew_Index", value); }
    }
    private static int maxBannerIndex = 2;
    private static int maxIntIndex = 3;
    private static int maxRewIndex = 3;

    void CheckBannerIndex()
    {
        Banner_Index++;
        if(Banner_Index >= maxBannerIndex)
        {
            Banner_Index=0;           
        }       
    }
    void CheckIntIndex()
    {
        Int_Index++;
        if (Int_Index >= maxIntIndex)
        {
            Int_Index = 0;
        }
    }
    
    public void ShowInterstitial()
    {
        if (!showAds)
            return;
        StartCoroutine(WaitAplayInterstitialAd());
        //// RequestFullScreenAd();       
        //if (!interstitialAd.IsLoaded())
        //{
        //    unity_AdManager.ShowInterstitialVideo();
        //}
        //else
        //{
        //    interstitialAd.Show();
        //}

    }
    int myRequestCount=1;
    IEnumerator WaitAplayInterstitialAd()
    {
        if (myRequestCount == ad_show_onrequest_count())
        {
            if (show_ad_as_index == false)
            {
                while (!interstitialAd[CurrentIntIndex()].CanShowAd())
                {
                    yield return null;
                }
                interstitialAd[CurrentIntIndex()].Show();
            }
            else
            {
                while (!interstitialAd[Int_Index].CanShowAd())
                {
                    yield return null;
                }
                interstitialAd[Int_Index].Show();
                CheckIntIndex();
            }
            myRequestCount = 1;
        }
        else
        {
            myRequestCount++;
        }
       
    }
   public void ShowInterstitial_Instant()
    {
        if (show_ad_as_index == false)
        {
            if (!interstitialAd[CurrentIntIndex()].CanShowAd())
                 return;
            interstitialAd[CurrentIntIndex()].Show();
        }
        else
        {
            if (!interstitialAd[Int_Index].CanShowAd())
                return;            
            interstitialAd[Int_Index].Show();
            CheckIntIndex();
        }
    }
    int ad_show_onrequest_count()
    {
        if (GoogleSheetHandler.ad_show_onrequest_count == "1")
        {
            return 1;
        }
        else
             if (GoogleSheetHandler.ad_show_onrequest_count == "2")
        {
            return 2;
        }
        else
             if (GoogleSheetHandler.ad_show_onrequest_count == "3")
        {
            return 3;
        }
        else
             if (GoogleSheetHandler.ad_show_onrequest_count == "4")
        {
            return 4;
        }
        else
             if (GoogleSheetHandler.ad_show_onrequest_count == "5")
        {
            return 5;
        }
        else
            return 1;
    }


    #region Native Ad Mehods ------------------------------------------------
    /*
    private void RequestNativeAd()
    {
        AdLoader adLoader = new AdLoader.Builder(idNative).ForUnifiedNativeAd().Build();
        adLoader.OnUnifiedNativeAdLoaded += this.HandleOnUnifiedNativeAdLoaded;
        adLoader.LoadAd(AdRequestBuild());
    }



    //events
    private void HandleOnUnifiedNativeAdLoaded(object sender, UnifiedNativeAdEventArgs args)
    {
        this.adNative = args.nativeAd;
        nativeLoaded = true;
    }

  

    //------------------------------------------------------------------------
    AdRequest AdRequestBuild()
    {
        return new AdRequest.Builder().Build();
    }
    private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("Native ad failed to load: " + args.Message);
    }
    void Update()
    {
        if (nativeLoaded)
        {
            nativeLoaded = false;

            Texture2D iconTexture = this.adNative.GetIconTexture();
            Texture2D iconAdChoices = this.adNative.GetAdChoicesLogoTexture();
            string headline = this.adNative.GetHeadlineText();
            string cta = this.adNative.GetCallToActionText();
            string advertiser = this.adNative.GetAdvertiserText();
           nativeItem.adIcon.texture = iconTexture;
           nativeItem.adChoices.texture = iconAdChoices;
           nativeItem.adHeadline.text = headline;
           nativeItem.adAdvertiser.text = advertiser;
           nativeItem.adCallToAction.text = cta;

            //register gameobjects
            adNative.RegisterIconImageGameObject(nativeItem.adIcon.gameObject);
            adNative.RegisterAdChoicesLogoGameObject(nativeItem.adChoices.gameObject);
            adNative.RegisterHeadlineTextGameObject(nativeItem.adHeadline.gameObject);
            adNative.RegisterCallToActionGameObject(nativeItem.adCallToAction.gameObject);
            adNative.RegisterAdvertiserTextGameObject(nativeItem.adAdvertiser.gameObject);

            nativeItem.adNativePanel.SetActive(true); //show ad panel
        }
    }
    */
    #endregion
    public static void Initialize(Action<InitializationStatus> initCompleteAction) { }

    #region BANNER

    public void RequestBanner(string bannerID ,int bannerIndex)
    {
        bannerView[bannerIndex] = new BannerView(bannerID, AdSize.Banner, bannerPose);

        AdRequest request = new AdRequest.Builder().Build();

        bannerView[bannerIndex].LoadAd(request);
        
       
      }
    int BannerIndex()
    {
        if (GoogleSheetHandler.show_g_banner1 == true)
        {
            return 0;
        }
        else if (GoogleSheetHandler.show_g_banner2 == true)
        {
            return 1;
        }
        else
        return 0;

    }
    public IEnumerator ShowBanner()
    {
        yield return new WaitUntil(() => readyToShoAd);
        if (show_ad_as_index == false)
        {
            if (showAds == true)
            {
                bannerView[BannerIndex()].Show();
            }
        }
        else
        {
            if (showAds == true)
            {
                bannerView[Banner_Index].Show();
                CheckBannerIndex();
            }
        }
        
       
    }

    public void HideBanner()
    {
        bannerView[BannerIndex()].Hide();

    }
    #endregion

    
    public void RequestInterstitialAdAd(string interstitiaAdID,int int_index)
    {
        var adRequest = new AdRequest();
        InterstitialAd.Load(interstitiaAdID, adRequest,
          (InterstitialAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("interstitial ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Interstitial ad loaded with response : "
                        + ad.GetResponseInfo());

              interstitialAd[int_index] = ad;
              RegisterEventHandlers(ad);
              RegisterReloadHandler(ad);
          });   

    }
    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
    private void RegisterReloadHandler(InterstitialAd interstitialAd)
    {
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += ()=>
    {
            Debug.Log("Interstitial Ad full screen content closed.");

        // Reload the ad so that we can show another as soon as possible.
        RequestInterstitialAdAd(interstitiaAdID[0], 0);
    };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            RequestInterstitialAdAd(interstitiaAdID[0], 0);
        };
    }

    public int CurrentRewIndex()
    {
        if (GoogleSheetHandler.show_g_rewarded1 == true)
        {
            return 0;
        }
        else if (GoogleSheetHandler.show_g_rewarded2 == true)
        {
            return 1;
        }
        else
             if (GoogleSheetHandler.show_g_rewarded3 == true)
        {
            return 2;
        }
        else
            return 0;

    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
       /* MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);*/
    }
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        // MonoBehaviour.print("HandleAdClosed event received");
        //adManager.CallRewardedAdClosedEvent();       
        RequestInterstitialAdAd(interstitiaAdID[0], 0);
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        //  Debug.Log("Rewarded Video ad loaded successfully");

    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
      /*  MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);9*/
    }  


    private void OnDestroy()
    {
        //ConfigManager.FetchCompleted -= GetAdId;
    }
   
}
