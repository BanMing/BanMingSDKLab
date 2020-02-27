using UnityEngine;
using UnityEngine.UI;
using AudienceNetwork;
using UnityEngine.SceneManagement;
using AudienceNetwork.Utility;

public class RewardedVideoAdScene : MonoBehaviour
{

    private RewardedVideoAd rewardedVideoAd;
    private bool isLoaded;
#pragma warning disable 0414
    private bool didClose;
#pragma warning restore 0414

    // UI elements in scene
    public Text statusLabel;

    private void Awake()
    {
        AudienceNetworkAds.Initialize();
    }

    // Load button
    public void LoadRewardedVideo()
    {
        statusLabel.text = "Loading rewardedVideo ad...";

        // Create the rewarded video unit with a placement ID (generate your own on the Facebook app settings).
        // Use different ID for each ad placement in your app.
        rewardedVideoAd = new RewardedVideoAd("YOUR_PLACEMENT_ID");

        // For S2S validation you can create the rewarded video ad with the reward data
        // Refer to documentation here:
        // https://developers.facebook.com/docs/audience-network/android/rewarded-video#server-side-reward-validation
        // https://developers.facebook.com/docs/audience-network/ios/rewarded-video#server-side-reward-validation
        RewardData rewardData = new RewardData
        {
            UserId = "USER_ID",
            Currency = "REWARD_ID"
        };
#pragma warning disable 0219
        RewardedVideoAd s2sRewardedVideoAd = new RewardedVideoAd("YOUR_PLACEMENT_ID", rewardData);
#pragma warning restore 0219

        rewardedVideoAd.Register(gameObject);

        // Set delegates to get notified on changes or when the user interacts with the ad.
        rewardedVideoAd.RewardedVideoAdDidLoad = delegate ()
        {
            Debug.Log("RewardedVideo ad loaded.");
            isLoaded = true;
            didClose = false;
            string isAdValid = rewardedVideoAd.IsValid() ? "valid" : "invalid";
            statusLabel.text = "Ad loaded and is " + isAdValid + ". Click show to present!";
        };
        rewardedVideoAd.RewardedVideoAdDidFailWithError = delegate (string error)
        {
            Debug.Log("RewardedVideo ad failed to load with error: " + error);
            statusLabel.text = "RewardedVideo ad failed to load. Check console for details.";
        };
        rewardedVideoAd.RewardedVideoAdWillLogImpression = delegate ()
        {
            Debug.Log("RewardedVideo ad logged impression.");
        };
        rewardedVideoAd.RewardedVideoAdDidClick = delegate ()
        {
            Debug.Log("RewardedVideo ad clicked.");
        };

        // For S2S validation you need to register the following two callback
        // Refer to documentation here:
        // https://developers.facebook.com/docs/audience-network/android/rewarded-video#server-side-reward-validation
        // https://developers.facebook.com/docs/audience-network/ios/rewarded-video#server-side-reward-validation
        rewardedVideoAd.RewardedVideoAdDidSucceed = delegate ()
        {
            Debug.Log("Rewarded video ad validated by server");
        };

        rewardedVideoAd.RewardedVideoAdDidFail = delegate ()
        {
            Debug.Log("Rewarded video ad not validated, or no response from server");
        };

        rewardedVideoAd.RewardedVideoAdDidClose = delegate ()
        {
            Debug.Log("Rewarded video ad did close.");
            didClose = true;
            if (rewardedVideoAd != null)
            {
                rewardedVideoAd.Dispose();
            }
        };

#if UNITY_ANDROID
        /*
         * Only relevant to Android.
         * This callback will only be triggered if the Rewarded Video activity
         * has been destroyed without being properly closed. This can happen if
         * an app with launchMode:singleTask (such as a Unity game) goes to
         * background and is then relaunched by tapping the icon.
         */
        rewardedVideoAd.RewardedVideoAdActivityDestroyed = delegate ()
        {
            if (!didClose)
            {
                Debug.Log("Rewarded video activity destroyed without being closed first.");
                Debug.Log("Game should resume. User should not get a reward.");
            }
        };
#endif

        // Initiate the request to load the ad.
        rewardedVideoAd.LoadAd();
    }

    // Show button
    public void ShowRewardedVideo()
    {
        if (isLoaded)
        {
            rewardedVideoAd.Show();
            isLoaded = false;
            statusLabel.text = "";
        }
        else
        {
            statusLabel.text = "Ad not loaded. Click load to request an ad.";
        }
    }

    void OnDestroy()
    {
        // Dispose of rewardedVideo ad when the scene is destroyed
        if (rewardedVideoAd != null)
        {
            rewardedVideoAd.Dispose();
        }
        Debug.Log("RewardedVideoAdTest was destroyed!");
    }

    // Next button
    public void NextScene()
    {
        SceneManager.LoadScene("InterstitialAdScene");
    }
}
