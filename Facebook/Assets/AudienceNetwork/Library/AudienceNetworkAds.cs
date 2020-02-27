using UnityEngine;
using System.Runtime.InteropServices;
using System;

namespace AudienceNetwork
{
    public static class AudienceNetworkAds
    {
#pragma warning disable 0414
        private static bool isInitialized;
#pragma warning restore 0414

        internal static void Initialize()
        {
            if (IsInitialized()) { return; }

            PlayerPrefs.SetString("an_isUnitySDK", SdkVersion.Build);

#if UNITY_ANDROID
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaClass audienceNetworkAds = new AndroidJavaClass("com.facebook.ads.AudienceNetworkAds");
            audienceNetworkAds.CallStatic("initialize", context);
#endif
            isInitialized = true;
        }

        internal static bool IsInitialized()
        {
#if UNITY_ANDROID
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaClass audienceNetworkAds = new AndroidJavaClass("com.facebook.ads.AudienceNetworkAds");
            return audienceNetworkAds.CallStatic<bool>("isInitialized", context);
#else
            return isInitialized;
#endif
        }
    }
}
