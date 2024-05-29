﻿using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System;
[RequireComponent(typeof(Button))]
public class RewardedButton : MonoBehaviour
{
    Button button;
    [SerializeField]  RewardedAdManager rewardedAdManager;
	[SerializeField] private InHouse_Ad_Handler inHouse_Ad;
	public UnityEvent onRewarded;
	public UnityEvent onClose;
	[Space]
	public UnityEvent OnInHouseAdComplete;
	public UnityEvent OnInHouseAdClosed;
	private void Start()
	 {
		GetScripts();
	}
	void GetScripts()
	{
		if(!rewardedAdManager){
        rewardedAdManager=FindObjectOfType<RewardedAdManager>();
		}
		if(!inHouse_Ad){
		inHouse_Ad=FindObjectOfType<InHouse_Ad_Handler>();
		}
	}

	private void OnEnable()
	{
		GetScripts();
		button = GetComponent<Button>();		
		button.onClick.AddListener(OnClick);
		button.interactable = true;
		//StartCoroutine("AddEvent", 3);
		InHouseAdManager.onAdCompleted += OnAdCompleted;
		InHouseAdManager.onAdClosed += OnAdClosed;
	}     
    
	void ShowInHouseAd()
	{
		inHouse_Ad.ShowInHouseAd();
	}
	void OnAdCompleted()
	{
			OnInHouseAdComplete.Invoke();
	}
	void OnAdClosed()
	{
			OnInHouseAdClosed.Invoke();
	}
	//Timer.Schedule(this, 5f, AddEvents);
	//public void CallAddEvent()
	//   {
	//	StartCoroutine(AddEvent());
	//}
    public IEnumerator AddEvent()
	{		
		//button.interactable = false;
		yield return new WaitUntil(() => rewardedAdManager.Rewarded.CanShowAd() && gameObject.activeSelf);		
		button.interactable = true;
		//AddEvents();

	}
	//private void AddEvents()
	//{
	//	if (rewardedAdManager.IsReadyToShowAd())
	//	{
	//		rewardedAd().OnUserEarnedReward += HandleRewardBasedVideoRewarded;
	//		rewardedAd().OnAdClosed += HandleRewardedAdClosed;

	//	}
	//}
	int adCount;
	public void OnClick()
	{
		GetScripts();
		adCount++;
		if(adCount> RewardedAdManager.show_rewarded_onrequest_count)
        {
			adCount = 1;
		}

		if (RewardedAdManager.show_rewarded && adCount == RewardedAdManager.show_rewarded_onrequest_count)
        {
			if (rewardedAdManager.Rewarded.CanShowAd())
            {
                
				rewardedAdManager.ShowRewardedAd();
			}
			
		}else if (InHouseAdManager.show_inhouse_ad)
        {
			ShowInHouseAd();
		}		
	}
	public void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
		onRewarded.Invoke();
		//rewardedAdManager.RequestRewarded();

	}
	public void HandleRewardedAdClosed(object sender, EventArgs args)
	{
		onClose.Invoke();
		//rewardedAdManager.RequestRewarded();
	}
	private void OnDisable()
	{
		//if (rewardedAdManager.IsReadyToShowAd())
		//{
		//	rewardedAd().OnUserEarnedReward -= HandleRewardBasedVideoRewarded;
		//	rewardedAd().OnAdClosed -= HandleRewardedAdClosed;
		//}
		InHouseAdManager.onAdCompleted -= OnAdCompleted;
		InHouseAdManager.onAdClosed -= OnAdClosed;
	}
}


