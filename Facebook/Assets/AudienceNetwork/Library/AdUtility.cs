using UnityEngine;
using System.Runtime.InteropServices;
using System;

namespace AudienceNetwork.Utility
{
    public static class AdUtility
    {
        internal static double Width()
        {
            return AdUtilityBridge.Instance.Width();
        }

        internal static double Height()
        {
            return AdUtilityBridge.Instance.Height();
        }

        internal static double Convert(double deviceSize)
        {
            return AdUtilityBridge.Instance.Convert(deviceSize);
        }

        internal static void Prepare()
        {
            AdUtilityBridge.Instance.Prepare();
        }

        internal static bool IsLandscape()
        {
            return Screen.orientation == ScreenOrientation.Landscape || Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight;
        }
    }

    internal interface IAdUtilityBridge
    {
        double DeviceWidth();
        double DeviceHeight();
        double Width();
        double Height();
        double Convert(double deviceSize);
        void Prepare();
    }

    internal class AdUtilityBridge : IAdUtilityBridge
    {
        public static readonly IAdUtilityBridge Instance;

        internal AdUtilityBridge()
        {
        }

        static AdUtilityBridge()
        {
            Instance = CreateInstance();
        }

        private static IAdUtilityBridge CreateInstance()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
#if UNITY_IOS
                return new AdUtilityBridgeIOS();
#elif UNITY_ANDROID
                return new AdUtilityBridgeAndroid();
#endif
            }
            return new AdUtilityBridge();
        }

        public virtual double DeviceWidth()
        {
            return 2208;
        }

        public virtual double DeviceHeight()
        {
            return 1242;
        }

        public virtual double Width()
        {
            return 1104;
        }

        public virtual double Height()
        {
            return 621;
        }

        public virtual double Convert(double deviceSize)
        {
            return 2;
        }

        public virtual void Prepare()
        {
        }
    }

#if UNITY_ANDROID
    internal class AdUtilityBridgeAndroid : AdUtilityBridge
    {

        private T GetPropertyOfDisplayMetrics<T>(string property)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaObject resources = context.Call<AndroidJavaObject>("getResources");
            AndroidJavaObject displayMetrics = resources.Call<AndroidJavaObject>("getDisplayMetrics");
            return displayMetrics.Get<T>(property);
        }

        private double Density()
        {
            return GetPropertyOfDisplayMetrics<float>("density");
        }

        public override double DeviceWidth()
        {
            return GetPropertyOfDisplayMetrics<int>("widthPixels");
        }

        public override double DeviceHeight()
        {
            return GetPropertyOfDisplayMetrics<int>("heightPixels");
        }

        public override double Width()
        {
            // Leaving the original code commented out for now - for reference. Will remove it eventually.
            // return this.convert(this.deviceWidth());
            return Convert(Screen.width);
        }

        public override double Height()
        {
            // Leaving the original code commented out for now - for reference. Will remove it eventually.
            //return this.convert(this.deviceHeight());
            return Convert(Screen.height);
        }

        public override double Convert(double deviceSize)
        {
            return deviceSize / Density();
        }

        public override void Prepare()
        {
#if UNITY_ANDROID

            try
            {
                AndroidJavaClass looperClass = new AndroidJavaClass("android.os.Looper");
                looperClass.CallStatic("prepare");
            }
            catch (Exception)
            {

            }
#endif
        }
    }
#endif

#if UNITY_IOS
    internal class AdUtilityBridgeIOS : AdUtilityBridge
    {
        [DllImport("__Internal")]
        private static extern double FBAdUtilityBridgeGetDeviceWidth();

        [DllImport("__Internal")]
        private static extern double FBAdUtilityBridgeGetDeviceHeight();

        [DllImport("__Internal")]
        private static extern double FBAdUtilityBridgeGetWidth();

        [DllImport("__Internal")]
        private static extern double FBAdUtilityBridgeGetHeight();

        [DllImport("__Internal")]
        private static extern double FBAdUtilityBridgeConvertFromDeviceSize(double deviceSize);

        public override double DeviceWidth()
        {
            return FBAdUtilityBridgeGetDeviceWidth();
        }

        public override double DeviceHeight()
        {
            return FBAdUtilityBridgeGetDeviceHeight();
        }

        public override double Width()
        {
            return FBAdUtilityBridgeGetWidth();
        }

        public override double Height()
        {
            return FBAdUtilityBridgeGetHeight();
        }

        public override double Convert(double deviceSize)
        {
            return FBAdUtilityBridgeConvertFromDeviceSize(deviceSize);
        }
    }
#endif
}
