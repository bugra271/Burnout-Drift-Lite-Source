using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMob {

    private static bool initialized = false;
    public static bool EligibleReward = false;

    public static BannerView bannerView;
    public static InterstitialAd interstitialAd;
    public static RewardedAd rewardedAd;

    public static void Init() {

        if (initialized)
            return;

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        Debug.Log("Ads Initialized");
        initialized = true;

    }

    public static void RequestBanner() {

        bannerView = new BannerView(AdMob_Ids.bannerId, AdSize.SmartBanner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
        bannerView.Hide();

        Debug.Log("Banner Loaded");

    }

    public static void RequestInterstitial() {

        // Initialize an InterstitialAd.
        interstitialAd = new InterstitialAd(AdMob_Ids.interstitialId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the interstitial with the request.
        interstitialAd.LoadAd(request);

        interstitialAd.OnAdClosed += InterstitialAd_OnAdClosed;

        Debug.Log("Interstitial Loaded");

    }

    public static void RequestRewarded() {

        rewardedAd = new RewardedAd(AdMob_Ids.rewardedId);

        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;


        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);


    }

    private static void InterstitialAd_OnAdClosed(object sender, System.EventArgs e) {

        interstitialAd.OnAdClosed -= InterstitialAd_OnAdClosed;
        RequestInterstitial();

    }

    public static void ShowInterstitial() {

        if (interstitialAd == null)
            return;

        if (interstitialAd.IsLoaded())
            interstitialAd.Show();

        Debug.Log("Interstitial Showed");

    }

    public static void ShowRewarded() {

        if (rewardedAd == null)
            return;

        if (rewardedAd.IsLoaded())
            rewardedAd.Show();

        Debug.Log("Rewarded Showed");

    }

    public static void ShowBanner() {

        if (bannerView != null)
            bannerView.Show();

        Debug.Log("Banner Showed");

    }

    public static void HideBanner() {

        if (bannerView != null)
            bannerView.Hide();

        Debug.Log("Banner Hidden");

    }

    // EVENTS

    public static void HandleRewardedAdLoaded(object sender, EventArgs args) {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public static void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: ");
    }

    public static void HandleRewardedAdOpening(object sender, EventArgs args) {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public static void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args) {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: ");
    }

    public static void HandleRewardedAdClosed(object sender, EventArgs args) {

        RequestRewarded();
        MonoBehaviour.print("HandleRewardedAdClosed event received");

    }

    public static void HandleUserEarnedReward(object sender, Reward args) {

        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

        EligibleReward = true;

    }

}
