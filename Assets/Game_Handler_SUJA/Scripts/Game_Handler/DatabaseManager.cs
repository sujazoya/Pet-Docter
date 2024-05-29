using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;
public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;

   // private string apiURL = "https://script.google.com/macros/s/AKfycbxL0I7frAcwsF6dCT0VokwN5chl4eXxAZu8sPtmPshcm4jx3O40wT2lFuG5LFyHvMme/exec";
    private string apiURL = "https://script.google.com/macros/s/AKfycbyIi08JEHztPsf_gQebygCY3at1KTJddpObNO2QCVFTwyDwSgdOT3NppyQ5juZsh5mp5g/exec";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        //UIController.Instance.CheckInternet(RetriveDataFromAPI);
        StartCoroutine(CheckForAPICall());

    }
    IEnumerator CheckForAPICall()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            yield return new WaitUntil(() => Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
            Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork);
            RetriveDataFromAPI();
        }
        else
        {
            RetriveDataFromAPI();
        }
    }

    //Get Data From Database on App Start(use this method on spash screen to load data
    public void RetriveDataFromAPI()
    {
        StartCoroutine(ApiCallSet(null, SetApiData));
    }

    void SetApiData(string value)
    {
        GoogleSheetHandler.GetDataFromAPI(value);
       
        //AdsManager.Instance.LoadInterstitialAd(0);
        //AdsManager.Instance.LoadInterstitialAd(1);
        //AdsManager.Instance.LoadInterstitialAd(2);

        //AdsManager.Instance.LoadRewardedAd(0);
        //AdsManager.Instance.LoadRewardedAd(1);
    }   
    private string Checker(string value)
    {
        string tmpstring = value.ToLower();
       
        return tmpstring;
    }
    IEnumerator ApiCallSet(Action<string> OnFailCallBack, Action<string> OnSuccessCallBack)
    {

        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiURL))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = apiURL.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
            OnSuccessCallBack?.Invoke(webRequest.downloadHandler.text);
        }
       
    }
   
    ///Its Check Internet Connection
/*
    public bool internetConnectivity()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://www.google.com/");
        request.SendWebRequest();

        if (request.error != null)
        {
           
            Debug.LogWarning("No internet connection");
            return false;
        }
        else
        {
            return true;
        }
    }*/

    ///

}