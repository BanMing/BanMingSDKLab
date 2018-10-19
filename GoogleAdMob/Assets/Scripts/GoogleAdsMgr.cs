using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class GoogleAdsMgr : MonoBehaviour
{
    private string appId;
    private string bannerId;
    private string interstitialId;
    private string rewardedVideoId;
    private AdRequest adRequest;
    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardBasedVideoAd rewardBasedVideoAd;
    private bool isBannerLoaded = false;

    // Use this for initialization
    void Start()
    {
        InitAdMob();

    }
    //初始化sdk
    private void InitAdMob()
    {
#if UNITY_IOS
		appId="ca-app-pub-8417465030722115~3231391826";
		bannerId="ca-app-pub-8417465030722115/2618391578";
		interstitialId="ca-app-pub-8417465030722115/2624887166";
		rewardedVideoId="ca-app-pub-8417465030722115/9489678418";
#elif UNITY_ANDROID
        appId = "ca-app-pub-8417465030722115~8890912616";
        bannerId = "ca-app-pub-8417465030722115/6073177587";
        interstitialId = "ca-app-pub-8417465030722115/3419094089";
        rewardedVideoId = "ca-app-pub-8417465030722115/2709605508";
#endif

        MobileAds.Initialize(appId);
        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);
        adRequest = new AdRequest.Builder().Build();
        interstitialAd = new InterstitialAd(interstitialId);
        this.rewardBasedVideoAd = RewardBasedVideoAd.Instance;
        RegisterBannerAdAction();
        RegisterInterstitialAdAction();
        RegisterRewardedVideoAdAction();
        LoadBanner();
        LoadInterstitialAd();
        LoadRewardBasedVideo();

    }
    //注册横幅回调
    private void RegisterBannerAdAction()
    {
        bannerView.OnAdLoaded += BannerAdOnAdLoaded;
        bannerView.OnAdFailedToLoad += BannerAdFailedToLoad;
        bannerView.OnAdOpening += BannerAdOpening;
        bannerView.OnAdClosed += BannerAdCliosed;
        bannerView.OnAdLeavingApplication += BannerAdLeavingApplication;
    }

    //注册插页回调
    private void RegisterInterstitialAdAction()
    {
        interstitialAd.OnAdLoaded += OnInterstitialAdLoaded;
        interstitialAd.OnAdFailedToLoad += OnInterstitialAdFailedToLoad;
        interstitialAd.OnAdOpening += OnInterstitialAdOpening;
        interstitialAd.OnAdLeavingApplication += OnInterstitialAdLeacingApplication;
    }

    //注册奖励广告回调
    private void RegisterRewardedVideoAdAction()
    {
        rewardBasedVideoAd.OnAdLoaded += OnRewardBaseVideoAdLoad;
        rewardBasedVideoAd.OnAdFailedToLoad += OnRewardBaseVideoAdFailedToLoad;
        rewardBasedVideoAd.OnAdOpening += OnRewardBaseVideoAdOpening;
        rewardBasedVideoAd.OnAdStarted += OnRewardBaseVideoAdStarted;
        rewardBasedVideoAd.OnAdRewarded += OnRewardBaseVideoAdRewarded;
        rewardBasedVideoAd.OnAdClosed += OnRewardBaseVideoAdClosed;
        rewardBasedVideoAd.OnAdLeavingApplication += OnRewardBaseVideoAdLeavingApplication;
    }

    void OnDestroy()
    {
        bannerView.OnAdLoaded -= BannerAdOnAdLoaded;
        bannerView.OnAdFailedToLoad -= BannerAdFailedToLoad;
        bannerView.OnAdOpening -= BannerAdOpening;
        bannerView.OnAdClosed -= BannerAdCliosed;
        bannerView.OnAdLeavingApplication -= BannerAdLeavingApplication;
        interstitialAd.OnAdLoaded -= OnInterstitialAdLoaded;
        interstitialAd.OnAdFailedToLoad -= OnInterstitialAdFailedToLoad;
        interstitialAd.OnAdOpening -= OnInterstitialAdOpening;
        interstitialAd.OnAdLeavingApplication -= OnInterstitialAdLeacingApplication;
        rewardBasedVideoAd.OnAdLoaded -= OnRewardBaseVideoAdLoad;
        rewardBasedVideoAd.OnAdFailedToLoad -= OnRewardBaseVideoAdFailedToLoad;
        rewardBasedVideoAd.OnAdOpening -= OnRewardBaseVideoAdOpening;
        rewardBasedVideoAd.OnAdStarted -= OnRewardBaseVideoAdStarted;
        rewardBasedVideoAd.OnAdRewarded -= OnRewardBaseVideoAdRewarded;
        rewardBasedVideoAd.OnAdClosed -= OnRewardBaseVideoAdClosed;
        rewardBasedVideoAd.OnAdLeavingApplication -= OnRewardBaseVideoAdLeavingApplication;
    }
    //加载横幅广告
    private void LoadBanner()
    {
        if (bannerView == null)
        {
            bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);
            RegisterBannerAdAction();
        }
        if (bannerView != null && !isBannerLoaded)
        {
            isBannerLoaded = false;
            bannerView.LoadAd(adRequest);
        }
    }

    //加载一个插页广告
    private void LoadInterstitialAd()
    {
        if (interstitialAd == null)
        {
            interstitialAd = new InterstitialAd(interstitialId);
            RegisterInterstitialAdAction();
        }
        if (!interstitialAd.IsLoaded())
        {
            interstitialAd.LoadAd(adRequest);
        }
    }

    //加载一个视频广告
    private void LoadRewardBasedVideo()
    {
        if (rewardBasedVideoAd == null)
        {
            rewardBasedVideoAd = RewardBasedVideoAd.Instance;
        }
        if (rewardBasedVideoAd != null && !rewardBasedVideoAd.IsLoaded())
        {
            rewardBasedVideoAd.LoadAd(adRequest, rewardedVideoId);
        }
    }

    //显示一个横幅广告
    public void ShowBannerAd()
    {
        if (bannerView != null && isBannerLoaded)
        {
            bannerView.Show();
        }
        if (!isBannerLoaded)
        {
            LoadBanner();
        }
    }
    //显示插页广告
    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            LoadInterstitialAd();
        }
    }
    public void ShowRewardBaseVideo()
    {
        if (rewardBasedVideoAd != null && rewardBasedVideoAd.IsLoaded())
        {
            rewardBasedVideoAd.Show();
        }
        else
        {
            LoadRewardBasedVideo();
        }
    }
    ////////////////////////////////////////////回调/////////////////////////////////////////////////////
    private void BannerAdLeavingApplication(object sender, EventArgs e)
    {
        DebugInfo("BannerAdLeavingApplication");
    }

    private void BannerAdCliosed(object sender, EventArgs e)
    {
        isBannerLoaded = false;
        DebugInfo("BannerAdCliosed");
    }

    private void BannerAdOpening(object sender, EventArgs e)
    {
        isBannerLoaded = false;
        DebugInfo("BannerAdOpening");
    }

    private void BannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        isBannerLoaded = false;
        DebugInfo("BannerAdFailedToLoad");
    }

    private void BannerAdOnAdLoaded(object sender, EventArgs e)
    {
        isBannerLoaded = true;
        DebugInfo("BannerAdOnAdLoaded");
    }

    private void OnInterstitialAdLeacingApplication(object sender, EventArgs e)
    {
        //  
        DebugInfo("OnInterstitialAdLeacingApplication");
    }

    private void OnInterstitialAdOpening(object sender, EventArgs e)
    {
        DebugInfo("OnInterstitialAdOpening");
        //  
#if UNITY_IOS
        interstitialAd = null;
#endif
        LoadInterstitialAd();
    }

    private void OnInterstitialAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        DebugInfo("OnInterstitialAdFailedToLoad");
        LoadInterstitialAd();
    }

    private void OnInterstitialAdLoaded(object sender, EventArgs e)
    { 
        DebugInfo("OnInterstitialAdLoaded");
    }
    private void OnRewardBaseVideoAdLeavingApplication(object sender, EventArgs e)
    {
        DebugInfo("OnRewardBaseVideoAdLeavingApplication");
    }

    private void OnRewardBaseVideoAdClosed(object sender, EventArgs e)
    {
        DebugInfo("OnRewardBaseVideoAdClosed");
    }

    private void OnRewardBaseVideoAdRewarded(object sender, Reward e)
    {
        DebugInfo("OnRewardBaseVideoAdRewarded");
    }

    private void OnRewardBaseVideoAdStarted(object sender, EventArgs e)
    {
        LoadRewardBasedVideo();
        DebugInfo("OnRewardBaseVideoAdStarted");
    }

    private void OnRewardBaseVideoAdOpening(object sender, EventArgs e)
    {
        DebugInfo("OnRewardBaseVideoAdOpening");
    }

    private void OnRewardBaseVideoAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        DebugInfo("OnRewardBaseVideoAdFailedToLoad");
    }

    private void OnRewardBaseVideoAdLoad(object sender, EventArgs e)
    {
        DebugInfo("OnRewardBaseVideoAdLoad");
    }
    private void DebugInfo(string str)
    {
        Debug.Log("GoogleAd:" + str);
    }
}
