using UnityEngine;
using System;
using System.Collections.Generic;
using AudienceNetwork.Utility;
using System.Runtime.InteropServices;
using AOT;

namespace AudienceNetwork
{
    public delegate void FBAdViewBridgeCallback();
    public delegate void FBAdViewBridgeErrorCallback(string error);
    internal delegate void FBAdViewBridgeExternalCallback(int uniqueId);
    internal delegate void FBAdViewBridgeErrorExternalCallback(int uniqueId, string error);

    public enum AdSize
    {
        BANNER_HEIGHT_50,
        BANNER_HEIGHT_90,
        RECTANGLE_HEIGHT_250
    }
    ;

    public enum AdPosition
    {
        CUSTOM,
        TOP,
        BOTTOM
    };

    public sealed class AdView : IDisposable
    {

        private readonly int uniqueId;
        private bool isLoaded;
        private readonly AdSize size;
        private AdHandler handler;

        public string PlacementId { get; private set; }

        public FBAdViewBridgeCallback AdViewDidLoad
        {
            internal get
            {
                return adViewDidLoad;
            }
            set
            {
                adViewDidLoad = value;
                AdViewBridge.Instance.OnLoad(uniqueId, adViewDidLoad);
            }
        }

        public FBAdViewBridgeCallback AdViewWillLogImpression
        {
            internal get
            {
                return adViewWillLogImpression;
            }
            set
            {
                adViewWillLogImpression = value;
                AdViewBridge.Instance.OnImpression(uniqueId, adViewWillLogImpression);
            }
        }

        public FBAdViewBridgeErrorCallback AdViewDidFailWithError
        {
            internal get
            {
                return adViewDidFailWithError;
            }
            set
            {
                adViewDidFailWithError = value;
                AdViewBridge.Instance.OnError(uniqueId, adViewDidFailWithError);
            }
        }

        public FBAdViewBridgeCallback AdViewDidClick
        {
            internal get
            {
                return adViewDidClick;
            }
            set
            {
                adViewDidClick = value;
                AdViewBridge.Instance.OnClick(uniqueId, adViewDidClick);
            }
        }

        public FBAdViewBridgeCallback AdViewDidFinishClick
        {
            internal get
            {
                return adViewDidFinishClick;
            }
            set
            {
                adViewDidFinishClick = value;
                AdViewBridge.Instance.OnFinishedClick(uniqueId, adViewDidFinishClick);
            }
        }

        public FBAdViewBridgeCallback adViewDidLoad;
        public FBAdViewBridgeCallback adViewWillLogImpression;
        public FBAdViewBridgeErrorCallback adViewDidFailWithError;
        public FBAdViewBridgeCallback adViewDidClick;
        public FBAdViewBridgeCallback adViewDidFinishClick;

        public AdView(string placementId,
                      AdSize size)
        {
            AudienceNetworkAds.Initialize();

            PlacementId = placementId;
            this.size = size;

            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                uniqueId = AdViewBridge.Instance.Create(placementId, this, size);

                AdViewBridge.Instance.OnLoad(uniqueId, AdViewDidLoad);
                AdViewBridge.Instance.OnImpression(uniqueId, AdViewWillLogImpression);
                AdViewBridge.Instance.OnClick(uniqueId, AdViewDidClick);
                AdViewBridge.Instance.OnError(uniqueId, AdViewDidFailWithError);
                AdViewBridge.Instance.OnFinishedClick(uniqueId, AdViewDidFinishClick);
            }
        }

        ~AdView()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(Boolean iAmBeingCalledFromDisposeAndNotFinalize)
        {
            Debug.Log("Banner Ad Disposed.");
            AdViewBridge.Instance.Release(uniqueId);
        }

        public override string ToString()
        {
            return string.Format(
                       "[AdView: " +
                       "PlacementId={0}, " +
                       "AdViewDidLoad={1}, " +
                       "AdViewWillLogImpression={2}, " +
                       "AdViewDidFailWithError={3}, " +
                       "AdViewDidClick={4}, " +
                       "adViewDidFinishClick={5}]",
                       PlacementId,
                       AdViewDidLoad,
                       AdViewWillLogImpression,
                       AdViewDidFailWithError,
                       AdViewDidClick,
                       adViewDidFinishClick);
        }

        public void Register(GameObject gameObject)
        {
            handler = gameObject.AddComponent<AdHandler>();
        }

        public void LoadAd()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                AdViewBridge.Instance.Load(uniqueId);
            }
            else
            {
                AdViewDidLoad();
            }
        }

        public void LoadAd(String bidPayload)
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                AdViewBridge.Instance.Load(uniqueId, bidPayload);
            }
            else
            {
                AdViewDidLoad();
            }
        }

        public bool IsValid()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                return (isLoaded && AdViewBridge.Instance.IsValid(uniqueId));
            }
            else
            {
                return true;
            }
        }

        internal void LoadAdFromData()
        {
            isLoaded = true;

            if (AdViewDidLoad != null)
            {
                handler.ExecuteOnMainThread(() =>
                {
                    AdViewDidLoad();
                });
            }
        }

        private static double HeightFromType(AdView instance, AdSize size)
        {
            switch (size)
            {
                case AdSize.BANNER_HEIGHT_50:
                    {
                        return 50;
                    }
                case AdSize.BANNER_HEIGHT_90:
                    {
                        return 90;
                    }
                case AdSize.RECTANGLE_HEIGHT_250:
                    {
                        return 250;
                    }
            }
            return 0;
        }

        public bool Show(AdPosition position)
        {
            double y = 0; // default y-asix to 0
            switch (position)
            {
                case AdPosition.TOP:
                    {
                        break;
                    }
                case AdPosition.BOTTOM:
                    {
                        y = AdUtility.Height() - AdView.HeightFromType(this, size);
                        break;
                    }
                case AdPosition.CUSTOM:
                    {
                        Debug.LogWarning("Use Show(double y) instead");
                        break;
                    }
            }
            return Show(y);
        }

        public bool Show(double y)
        {
            return Show(0, y);
        }

        public bool Show(double x,
                         double y)
        {
            return Show(x, y, AdUtility.Width(), AdView.HeightFromType(this, size));
        }

        private bool Show(double x,
                          double y,
                          double width,
                          double height)
        {
            return AdViewBridge.Instance.Show(uniqueId, x, y, width, height);
        }

        public void SetExtraHints(ExtraHints extraHints)
        {
            AdViewBridge.Instance.SetExtraHints(uniqueId, extraHints);
        }

        public void DisableAutoRefresh()
        {
            AdViewBridge.Instance.DisableAutoRefresh(uniqueId);
        }

        internal void ExecuteOnMainThread(Action action)
        {
            if (handler)
            {
                handler.ExecuteOnMainThread(action);
            }
        }

        public static implicit operator bool(AdView obj)
        {
            return !(object.ReferenceEquals(obj, null));
        }
    }

    internal interface IAdViewBridge
    {
        int Create(string placementId,
                   AdView adView,
                   AdSize size);

        int Load(int uniqueId);

        int Load(int uniqueId, String bidPayload);

        bool IsValid(int uniqueId);

        bool Show(int uniqueId,
                  double x,
                  double y,
                  double width,
                  double height);

        void SetExtraHints(int uniqueId, ExtraHints extraHints);

        void DisableAutoRefresh(int uniqueId);

        void Release(int uniqueId);

        void OnLoad(int uniqueId,
                    FBAdViewBridgeCallback callback);

        void OnImpression(int uniqueId,
                          FBAdViewBridgeCallback callback);

        void OnClick(int uniqueId,
                     FBAdViewBridgeCallback callback);

        void OnError(int uniqueId,
                     FBAdViewBridgeErrorCallback callback);

        void OnFinishedClick(int uniqueId,
                             FBAdViewBridgeCallback callback);
    }

    internal class AdViewBridge : IAdViewBridge
    {

        /* Interface to Interstitial implementation */

        public static readonly IAdViewBridge Instance;

        internal AdViewBridge()
        {
        }

        static AdViewBridge()
        {
            Instance = AdViewBridge.CreateInstance();
        }

        private static IAdViewBridge CreateInstance()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
#if UNITY_IOS
                return new AdViewBridgeIOS();
#elif UNITY_ANDROID
                return new AdViewBridgeAndroid();
#endif
            }
            return new AdViewBridge();

        }

        public virtual int Create(string placementId,
                                  AdView AdView,
                                  AdSize size)
        {
            return 123;
        }

        public virtual int Load(int uniqueId)
        {
            return 123;
        }

        public virtual int Load(int uniqueId, String bidPayload)
        {
            return 123;
        }

        public virtual bool IsValid(int uniqueId)
        {
            return true;
        }

        public virtual bool Show(int uniqueId,
                                 double x,
                                 double y,
                                 double width,
                                 double height)
        {
            return true;
        }

        public virtual void SetExtraHints(int uniqueId, ExtraHints extraHints)
        {
        }

        public virtual void DisableAutoRefresh(int uniqueId)
        {
        }

        public virtual void Release(int uniqueId)
        {
        }

        public virtual void OnLoad(int uniqueId,
                                   FBAdViewBridgeCallback callback)
        {
        }

        public virtual void OnImpression(int uniqueId,
                                         FBAdViewBridgeCallback callback)
        {
        }

        public virtual void OnClick(int uniqueId,
                                    FBAdViewBridgeCallback callback)
        {
        }

        public virtual void OnError(int uniqueId,
                                    FBAdViewBridgeErrorCallback callback)
        {
        }

        public virtual void OnFinishedClick(int uniqueId,
                                            FBAdViewBridgeCallback callback)
        {
        }

    }

#if UNITY_ANDROID
    internal class AdViewBridgeAndroid : AdViewBridge
    {

        private static Dictionary<int, AdViewContainer> adViews = new Dictionary<int, AdViewContainer>();
        private static int lastKey;

        private AndroidJavaObject AdViewForAdViewId(int uniqueId)
        {
            AdViewContainer adViewContainer = null;
            bool success = AdViewBridgeAndroid.adViews.TryGetValue(uniqueId, out adViewContainer);
            if (success) {
                return adViewContainer.bridgedAdView;
            } else {
                return null;
            }
        }

        private string GetStringForAdViewId(int uniqueId,
                                            string method)
        {
            AndroidJavaObject adView = AdViewForAdViewId(uniqueId);
            if (adView != null) {
                return adView.Call<string> (method);
            } else {
                return null;
            }
        }

        private string GetImageURLForAdViewId(int uniqueId,
                                              string method)
        {
            AndroidJavaObject adView = AdViewForAdViewId(uniqueId);
            if (adView != null) {
                AndroidJavaObject image = adView.Call<AndroidJavaObject> (method);
                if (image != null) {
                    return image.Call<string> ("getUrl");
                }
            }
            return null;
        }

        private AndroidJavaObject JavaAdSizeFromAdSize(AdSize size)
        {
            AndroidJavaObject retValue = null;
            AndroidJavaClass sizeEnum = new AndroidJavaClass("com.facebook.ads.AdSize");
            switch (size) {
            case AdSize.BANNER_HEIGHT_50:
                retValue = sizeEnum.GetStatic<AndroidJavaObject>("BANNER_HEIGHT_50");
                break;
            case AdSize.BANNER_HEIGHT_90:
                retValue = sizeEnum.GetStatic<AndroidJavaObject>("BANNER_HEIGHT_90");
                break;
            case AdSize.RECTANGLE_HEIGHT_250:
                retValue = sizeEnum.GetStatic<AndroidJavaObject>("RECTANGLE_HEIGHT_250");
                break;
            }
            return retValue;
        }

        public override int Create(string placementId,
                                   AdView adView,
                                   AdSize size)
        {
            AdUtility.Prepare();
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

            AndroidJavaObject bridgedAdView = new AndroidJavaObject("com.facebook.ads.AdView",
                    context,
                    placementId,
                    JavaAdSizeFromAdSize(size));

            AdViewBridgeListenerProxy proxy = new AdViewBridgeListenerProxy(adView, bridgedAdView);
            bridgedAdView.Call("setAdListener", proxy);

            AdViewContainer adViewContainer = new AdViewContainer(adView)
            {
                bridgedAdView = bridgedAdView,
                listenerProxy = proxy
            };

            int key = AdViewBridgeAndroid.lastKey;
            AdViewBridgeAndroid.adViews.Add(key, adViewContainer);
            AdViewBridgeAndroid.lastKey++;
            return key;
        }

        public override int Load(int uniqueId)
        {
            AdUtility.Prepare();
            AndroidJavaObject adView = AdViewForAdViewId(uniqueId);
            if (adView != null) {
                adView.Call("loadAd");
            }
            return uniqueId;
        }

        public override int Load(int uniqueId, String bidPayload)
        {
            AdUtility.Prepare();
            AndroidJavaObject adView = AdViewForAdViewId(uniqueId);
            if (adView != null)
            {
                adView.Call("loadAdFromBid", bidPayload);
            }
            return uniqueId;
        }

        public override bool IsValid(int uniqueId)
        {
            AndroidJavaObject adView = AdViewForAdViewId(uniqueId);
            if (adView != null)
            {
                return !adView.Call<bool>("isAdInvalidated");
            }
            else
            {
                return false;
            }
        }

        public override bool Show(int uniqueId,
                                  double x,
                                  double y,
                                  double width,
                                  double height)
        {
            AndroidJavaObject adView = AdViewForAdViewId(uniqueId);
            if (adView == null) {
                return false;
            }
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
                AndroidJavaObject resources = context.Call<AndroidJavaObject> ("getResources");
                AndroidJavaObject displayMetrics =
                    resources.Call<AndroidJavaObject> ("getDisplayMetrics");
                float density = displayMetrics.Get<float> ("density");

                AndroidJavaObject layoutParams = new AndroidJavaObject("android.widget.LinearLayout$LayoutParams",
                        (int)(width * density),
                        (int)(height * density));
                AndroidJavaObject linearLayout = new AndroidJavaObject("android.widget.LinearLayout", activity);
                AndroidJavaClass R = new AndroidJavaClass("android.R$id");
                AndroidJavaObject view = activity.Call<AndroidJavaObject> ("findViewById", R.GetStatic<int> ("content"));

                AndroidJavaObject parent  = adView.Call<AndroidJavaObject>("getParent");
                if (parent != null) {
                    IntPtr ptr = AndroidJNI.GetMethodID(parent.GetRawClass(), "removeView", "(Landroid/view/View;)V");
                    if (ptr != IntPtr.Zero) {
                        parent.Call("removeView", adView);
                    } else {
                        AndroidJNI.ExceptionClear();
                    }
                }

                layoutParams.Call("setMargins", (int)(x * density), (int)(y * density), 0, 0);
                linearLayout.Call("addView", adView, layoutParams);
                view.Call("addView", linearLayout);
            }));
            return true;
        }

        public override void SetExtraHints(int uniqueId, ExtraHints extraHints)
        {
            AndroidJavaObject adView = AdViewForAdViewId(uniqueId);

            if (adView != null)
            {
                adView.Call("setExtraHints", extraHints.GetAndroidObject());
            }
        }

        public override void DisableAutoRefresh(int uniqueId)
        {
            AndroidJavaObject adView = AdViewForAdViewId(uniqueId);
            if (adView != null) {
                adView.Call("disableAutoRefresh");
            }
        }

        public override void Release(int uniqueId)
        {
            AndroidJavaObject activity =
                new AndroidJavaClass("com.unity3d.player.UnityPlayer")
            .GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject adView = AdViewForAdViewId(uniqueId);
            AdViewBridgeAndroid.adViews.Remove(uniqueId);
            if (adView != null) {
                activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                    adView.Call("destroy");
                    AndroidJavaObject parent = adView.Call<AndroidJavaObject> ("getParent");
                    parent.Call("removeView", adView);
                }));
            }
        }

        public override void OnLoad(int uniqueId,
                                    FBAdViewBridgeCallback callback) {}
        public override void OnImpression(int uniqueId,
                                          FBAdViewBridgeCallback callback) {}
        public override void OnClick(int uniqueId,
                                     FBAdViewBridgeCallback callback) {}
        public override void OnError(int uniqueId,
                                     FBAdViewBridgeErrorCallback callback) {}
        public override void OnFinishedClick(int uniqueId,
                                             FBAdViewBridgeCallback callback) {}

    }

#endif

#if UNITY_IOS
    internal class AdViewBridgeIOS : AdViewBridge
    {

        private static Dictionary<int, AdViewContainer> adViews = new Dictionary<int, AdViewContainer>();

        private static AdViewContainer adViewContainerForAdViewId(int uniqueId)
        {
            AdViewContainer adView = null;
            bool success = AdViewBridgeIOS.adViews.TryGetValue(uniqueId, out adView);
            if (success)
            {
                return adView;
            }
            else
            {
                return null;
            }
        }

        [DllImport("__Internal")]
        private static extern int FBAdViewBridgeCreate(string placementId,
                int size);

        [DllImport("__Internal")]
        private static extern int FBAdViewBridgeLoad(int uniqueId);

        [DllImport("__Internal")]
        private static extern int FBAdViewBridgeLoadWithBidPayload(int uniqueId, string bidPayload);

        [DllImport("__Internal")]
        private static extern bool FBAdViewBridgeIsValid(int uniqueId);

        [DllImport("__Internal")]
        private static extern bool FBAdViewBridgeShow(int uniqueId,
                double x,
                double y,
                double width,
                double height);

        [DllImport("__Internal")]
        private static extern void FBAdViewBridgeDisableAutoRefresh(int uniqueId);

        [DllImport("__Internal")]
        private static extern void FBAdViewBridgeSetExtraHints(int uniqueId, string extraHints);

        [DllImport("__Internal")]
        private static extern void FBAdViewBridgeRelease(int uniqueId);

        [DllImport("__Internal")]
        private static extern void FBAdViewBridgeOnLoad(int uniqueId,
                FBAdViewBridgeExternalCallback callback);

        [DllImport("__Internal")]
        private static extern void FBAdViewBridgeOnImpression(int uniqueId,
                FBAdViewBridgeExternalCallback callback);

        [DllImport("__Internal")]
        private static extern void FBAdViewBridgeOnClick(int uniqueId,
                FBAdViewBridgeExternalCallback callback);

        [DllImport("__Internal")]
        private static extern void FBAdViewBridgeOnError(int uniqueId,
                FBAdViewBridgeErrorExternalCallback callback);

        [DllImport("__Internal")]
        private static extern void FBAdViewBridgeOnFinishedClick(int uniqueId,
                FBAdViewBridgeExternalCallback callback);

        [DllImport("__Internal")]
        private static extern int FBAdViewBridgeSizeHeight50Banner();
        [DllImport("__Internal")]
        private static extern int FBAdViewBridgeSizeHeight90Banner();
        [DllImport("__Internal")]
        private static extern int FBAdViewBridgeSizeInterstital();
        [DllImport("__Internal")]
        private static extern int FBAdViewBridgeSizeHeight250Rectangle();

        int intFromAdSize(AdSize size)
        {
            int retValue = -1;
            switch (size)
            {
                case AdSize.BANNER_HEIGHT_50:
                    retValue = AdViewBridgeIOS.FBAdViewBridgeSizeHeight50Banner();
                    break;
                case AdSize.BANNER_HEIGHT_90:
                    retValue = AdViewBridgeIOS.FBAdViewBridgeSizeHeight90Banner();
                    break;
                case AdSize.RECTANGLE_HEIGHT_250:
                    retValue = AdViewBridgeIOS.FBAdViewBridgeSizeHeight250Rectangle();
                    break;
            }
            return retValue;
        }

        public override int Create(string placementId,
                                   AdView adView,
                                   AdSize size)
        {
            int uniqueId = AdViewBridgeIOS.FBAdViewBridgeCreate(placementId, intFromAdSize(size));
            AdViewBridgeIOS.adViews.Add(uniqueId, new AdViewContainer(adView));
            AdViewBridgeIOS.FBAdViewBridgeOnLoad(uniqueId, adViewDidLoadBridgeCallback);
            AdViewBridgeIOS.FBAdViewBridgeOnImpression(uniqueId, adViewWillLogImpressionBridgeCallback);
            AdViewBridgeIOS.FBAdViewBridgeOnClick(uniqueId, adViewDidClickBridgeCallback);
            AdViewBridgeIOS.FBAdViewBridgeOnError(uniqueId, adViewDidFailWithErrorBridgeCallback);
            AdViewBridgeIOS.FBAdViewBridgeOnFinishedClick(uniqueId, adViewDidFinishClickBridgeCallback);

            return uniqueId;
        }

        public override int Load(int uniqueId)
        {
            return AdViewBridgeIOS.FBAdViewBridgeLoad(uniqueId);
        }

        public override int Load(int uniqueId, string bidPayload)
        {
            return AdViewBridgeIOS.FBAdViewBridgeLoadWithBidPayload(uniqueId, bidPayload);
        }

        public override bool IsValid(int uniqueId)
        {
            return AdViewBridgeIOS.FBAdViewBridgeIsValid(uniqueId);
        }

        public override bool Show(int uniqueId,
                                  double x,
                                  double y,
                                  double width,
                                  double height)
        {
            return AdViewBridgeIOS.FBAdViewBridgeShow(uniqueId, x, y, width, height);
        }

        public override void DisableAutoRefresh(int uniqueId)
        {
            AdViewBridgeIOS.FBAdViewBridgeDisableAutoRefresh(uniqueId);
        }

        public override void SetExtraHints(int uniqueId, ExtraHints extraHints)
        {
            AdViewBridgeIOS.FBAdViewBridgeSetExtraHints(uniqueId, JsonUtility.ToJson(extraHints));
        }

        public override void Release(int uniqueId)
        {
            AdViewBridgeIOS.adViews.Remove(uniqueId);
            AdViewBridgeIOS.FBAdViewBridgeRelease(uniqueId);
        }

        // Sets up internal managed callbacks

        public override void OnLoad(int uniqueId,
                                    FBAdViewBridgeCallback callback)
        {
            AdViewContainer container = AdViewBridgeIOS.adViewContainerForAdViewId(uniqueId);
            if (container)
            {
                container.onLoad = (container.adView.LoadAdFromData);
            }
        }

        public override void OnImpression(int uniqueId,
                                          FBAdViewBridgeCallback callback)
        {
            AdViewContainer container = AdViewBridgeIOS.adViewContainerForAdViewId(uniqueId);
            if (container)
            {
                container.onImpression = callback;
            }
        }

        public override void OnClick(int uniqueId,
                                     FBAdViewBridgeCallback callback)
        {
            AdViewContainer container = AdViewBridgeIOS.adViewContainerForAdViewId(uniqueId);
            if (container)
            {
                container.onClick = callback;
            }
        }

        public override void OnError(int uniqueId,
                                     FBAdViewBridgeErrorCallback callback)
        {
            AdViewContainer container = AdViewBridgeIOS.adViewContainerForAdViewId(uniqueId);
            if (container)
            {
                container.onError = callback;
            }
        }

        public override void OnFinishedClick(int uniqueId,
                                             FBAdViewBridgeCallback callback)
        {
            AdViewContainer container = AdViewBridgeIOS.adViewContainerForAdViewId(uniqueId);
            if (container)
            {
                container.onFinishedClick = callback;
            }
        }

        // External unmanaged callbacks (must be static)

        [MonoPInvokeCallback(typeof(FBAdViewBridgeExternalCallback))]
        private static void adViewDidLoadBridgeCallback(int uniqueId)
        {
            AdViewContainer container = AdViewBridgeIOS.adViewContainerForAdViewId(uniqueId);
            if (container && container.onLoad != null)
            {
                container.onLoad();
            }
        }

        [MonoPInvokeCallback(typeof(FBAdViewBridgeExternalCallback))]
        private static void adViewWillLogImpressionBridgeCallback(int uniqueId)
        {
            AdViewContainer container = AdViewBridgeIOS.adViewContainerForAdViewId(uniqueId);
            if (container && container.onImpression != null)
            {
                container.onImpression();
            }
        }

        [MonoPInvokeCallback(typeof(FBAdViewBridgeErrorExternalCallback))]
        private static void adViewDidFailWithErrorBridgeCallback(int uniqueId, string error)
        {
            AdViewContainer container = AdViewBridgeIOS.adViewContainerForAdViewId(uniqueId);
            if (container && container.onError != null)
            {
                container.onError(error);
            }
        }

        [MonoPInvokeCallback(typeof(FBAdViewBridgeExternalCallback))]
        private static void adViewDidClickBridgeCallback(int uniqueId)
        {
            AdViewContainer container = AdViewBridgeIOS.adViewContainerForAdViewId(uniqueId);
            if (container && container.onClick != null)
            {
                container.onClick();
            }
        }

        [MonoPInvokeCallback(typeof(FBAdViewBridgeExternalCallback))]
        private static void adViewDidFinishClickBridgeCallback(int uniqueId)
        {
            AdViewContainer container = AdViewBridgeIOS.adViewContainerForAdViewId(uniqueId);
            if (container && container.onFinishedClick != null)
            {
                container.onFinishedClick();
            }
        }

    }
#endif

    internal class AdViewContainer
    {
        internal AdView adView { get; set; }

        // iOS
        internal FBAdViewBridgeCallback onLoad { get; set; }
        internal FBAdViewBridgeCallback onImpression { get; set; }
        internal FBAdViewBridgeCallback onClick { get; set; }
        internal FBAdViewBridgeErrorCallback onError { get; set; }
        internal FBAdViewBridgeCallback onFinishedClick { get; set; }

        // Android
#if UNITY_ANDROID
        internal AndroidJavaProxy listenerProxy;
        internal AndroidJavaObject bridgedAdView;
#endif

        internal AdViewContainer(AdView adView)
        {
            this.adView = adView;
        }

        public override string ToString()
        {
            return string.Format("[AdViewContainer: adView={0}, onLoad={1}]", adView, onLoad);
        }

        public static implicit operator bool(AdViewContainer obj)
        {
            return !(object.ReferenceEquals(obj, null));
        }
    }

#if UNITY_ANDROID
    internal class AdViewBridgeListenerProxy : AndroidJavaProxy
    {
#pragma warning disable 0414
        private AdView adView;
        private readonly AndroidJavaObject bridgedAdView;
#pragma warning restore 0414

        public AdViewBridgeListenerProxy(AdView adView, AndroidJavaObject bridgedAdView)
            : base("com.facebook.ads.AdListener")
        {
            this.adView = adView;
            this.bridgedAdView = bridgedAdView;
        }

        void onError(AndroidJavaObject ad,
                     AndroidJavaObject error)
        {
            string errorMessage = error.Call<string> ("getErrorMessage");
            if (adView.AdViewDidFailWithError != null) {
                adView.ExecuteOnMainThread(() => {
                    adView.AdViewDidFailWithError(errorMessage);
                });
            }
        }

        void onAdLoaded(AndroidJavaObject ad)
        {
            adView.LoadAdFromData();
        }

        void onAdClicked(AndroidJavaObject ad)
        {
            if (adView.AdViewDidClick != null) {
                adView.ExecuteOnMainThread(() => {
                    adView.AdViewDidClick();
                });
            }
        }

        void onLoggingImpression(AndroidJavaObject ad)
        {
            if (adView.AdViewWillLogImpression != null) {
                adView.ExecuteOnMainThread(() => {
                    adView.AdViewWillLogImpression();
                });
            }
        }
    }
#endif
}
