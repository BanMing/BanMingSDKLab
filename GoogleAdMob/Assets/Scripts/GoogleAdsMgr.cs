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
    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void InitAdMob()
    {
#if UNITY_IOS
		appId="ca-app-pub-8417465030722115~3231391826";
		bannerId="ca-app-pub-8417465030722115/2618391578";
		interstitialId="ca-app-pub-8417465030722115/2624887166";
		videoId="ca-app-pub-8417465030722115/9489678418";
#elif UNITY_ANDROId
        appId = "ca-app-pub-8417465030722115~8890912616";
        bannerId = "ca-app-pub-8417465030722115/6073177587";
        interstitialId = "ca-app-pub-8417465030722115/3419094089";
        videoId = "ca-app-pub-8417465030722115/2709605508";
#endif

        MobileAds.Initialize(appId);
        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);
        adRequest = new AdRequest.Builder().Build();
        interstitialAd = new InterstitialAd(interstitialId);
        this.rewardBasedVideoAd = RewardBasedVideoAd.Instance;
    }

    private void RegisterBannerAdAction()
    {
        bannerView.OnAdLoaded += BannerAdOnAdLoaded;
        bannerView.OnAdFailedToLoad += BannerAdFailedToLoad;
        bannerView.OnAdOpening += BannerAdOpening;
        bannerView.OnAdClosed += BannerAdCliosed;
        bannerView.OnAdLeavingApplication += BannerAdLeavingApplication;
    }

    private void RegisterInterstitialAdAction()
    {
        interstitialAd.OnAdLoaded += OnInterstitialAdLoaded;
        interstitialAd.OnAdFailedToLoad += OnInterstitialAdFailedToLoad;
        interstitialAd.OnAdOpening += OnInterstitialAdOpening;
        interstitialAd.OnAdLeavingApplication += OnInterstitialAdLeacingApplication;
    }
    private void RegisterRewardedVideoAdAction()
    {
		rewardBasedVideoAd.OnAdLoaded+=OnRewardBaseVideoAdLoad;
		rewardBasedVideoAd.OnAdFailedToLoad+=OnRewardBaseVideoAdFailedToLoad;
		rewardBasedVideoAd.OnAdOpening+=OnRewardBaseVideoAdOpening;
		rewardBasedVideoAd.OnAdStarted+=OnRewardBaseVideoAdStarted;
		rewardBasedVideoAd.OnAdRewarded+=OnRewardBaseVideoAdRewarded;
		rewardBasedVideoAd.OnAdClosed+=OnRewardBaseVideoAdClosed;
		rewardBasedVideoAd.OnAdLeavingApplication+=OnRewardBaseVideoAdLeavingApplication;
	}

    //加载横幅广告
    private void LoadBanner()
    {
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
        if (rewardBasedVideoAd != null)
        {
            rewardBasedVideoAd.LoadAd(adRequest, rewardedVideoId);
        }
    }
    private void BannerAdLeavingApplication(object sender, EventArgs e)
    {
        // throw new NotImplementedException();
    }

    private void BannerAdCliosed(object sender, EventArgs e)
    {
        isBannerLoaded = false;
    }

    private void BannerAdOpening(object sender, EventArgs e)
    {
        isBannerLoaded = false;
    }

    private void BannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        isBannerLoaded = false;
    }

    private void BannerAdOnAdLoaded(object sender, EventArgs e)
    {
        isBannerLoaded = true;
    }

    private void OnInterstitialAdLeacingApplication(object sender, EventArgs e)
    {
        // throw new NotImplementedException();
    }

    private void OnInterstitialAdOpening(object sender, EventArgs e)
    {
        // throw new NotImplementedException();
#if UNITY_IOS
        interstitialAd = null;
#endif
    }

    private void OnInterstitialAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        // throw new NotImplementedException();
    }

    private void OnInterstitialAdLoaded(object sender, EventArgs e)
    {
        // throw new NotImplementedException();
    }
	    private void OnRewardBaseVideoAdLeavingApplication(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnRewardBaseVideoAdClosed(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnRewardBaseVideoAdRewarded(object sender, Reward e)
    {
        throw new NotImplementedException();
    }

    private void OnRewardBaseVideoAdStarted(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnRewardBaseVideoAdOpening(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnRewardBaseVideoAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnRewardBaseVideoAdLoad(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}
