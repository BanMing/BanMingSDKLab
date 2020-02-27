using UnityEngine;
using System;
using System.Collections.Generic;
using AudienceNetwork.Utility;
using System.Runtime.InteropServices;
using AOT;

namespace AudienceNetwork
{
    public delegate void FBRewardedVideoAdBridgeCallback();
    public delegate void FBRewardedVideoAdBridgeErrorCallback(string error);
    internal delegate void FBRewardedVideoAdBridgeExternalCallback(int uniqueId);
    internal delegate void FBRewardedVideoAdBridgeErrorExternalCallback(int uniqueId, string error);

    public sealed class RewardData
    {
        public string UserId
        {
            internal get;
            set;
        }

        public string Currency
        {
            internal get;
            set;
        }
    }

    public sealed class RewardedVideoAd : IDisposable
    {
        private readonly int uniqueId;
        private bool isLoaded;
        private AdHandler handler;

        public string PlacementId { get; private set; }
        public RewardData RewardData { get; private set; }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdDidLoad
        {
            internal get
            {
                return rewardedVideoAdDidLoad;
            }
            set
            {
                rewardedVideoAdDidLoad = value;
                RewardedVideoAdBridge.Instance.OnLoad(uniqueId, rewardedVideoAdDidLoad);
            }
        }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdWillLogImpression
        {
            internal get
            {
                return rewardedVideoAdWillLogImpression;
            }
            set
            {
                rewardedVideoAdWillLogImpression = value;
                RewardedVideoAdBridge.Instance.OnImpression(uniqueId, rewardedVideoAdWillLogImpression);
            }
        }

        public FBRewardedVideoAdBridgeErrorCallback RewardedVideoAdDidFailWithError
        {
            internal get
            {
                return rewardedVideoAdDidFailWithError;
            }
            set
            {
                rewardedVideoAdDidFailWithError = value;
                RewardedVideoAdBridge.Instance.OnError(uniqueId, rewardedVideoAdDidFailWithError);
            }
        }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdDidClick
        {
            internal get
            {
                return rewardedVideoAdDidClick;
            }
            set
            {
                rewardedVideoAdDidClick = value;
                RewardedVideoAdBridge.Instance.OnClick(uniqueId, rewardedVideoAdDidClick);
            }
        }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdWillClose
        {
            internal get
            {
                return rewardedVideoAdWillClose;
            }
            set
            {
                rewardedVideoAdWillClose = value;
                RewardedVideoAdBridge.Instance.OnWillClose(uniqueId, rewardedVideoAdWillClose);
            }
        }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdDidClose
        {
            internal get
            {
                return rewardedVideoAdDidClose;
            }
            set
            {
                rewardedVideoAdDidClose = value;
                RewardedVideoAdBridge.Instance.OnDidClose(uniqueId, rewardedVideoAdDidClose);
            }
        }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdComplete
        {
            internal get
            {
                return rewardedVideoAdComplete;
            }
            set
            {
                rewardedVideoAdComplete = value;
                RewardedVideoAdBridge.Instance.OnComplete(uniqueId, rewardedVideoAdComplete);
            }
        }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdDidSucceed
        {
            internal get
            {
                return rewardedVideoAdDidSucceed;
            }
            set
            {
                rewardedVideoAdDidSucceed = value;
                RewardedVideoAdBridge.Instance.OnDidSucceed(uniqueId, rewardedVideoAdDidSucceed);
            }
        }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdDidFail
        {
            internal get
            {
                return rewardedVideoAdDidFail;
            }
            set
            {
                rewardedVideoAdDidFail = value;
                RewardedVideoAdBridge.Instance.OnDidFail(uniqueId, rewardedVideoAdDidFail);
            }
        }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdActivityDestroyed
        {
            internal get
            {
                return rewardedVideoAdActivityDestroyed;
            }
            set
            {
                rewardedVideoAdActivityDestroyed = value;
                RewardedVideoAdBridge.Instance.OnActivityDestroyed(uniqueId, rewardedVideoAdActivityDestroyed);
            }
        }

        public FBRewardedVideoAdBridgeCallback rewardedVideoAdDidLoad;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdWillLogImpression;
        public FBRewardedVideoAdBridgeErrorCallback rewardedVideoAdDidFailWithError;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdDidClick;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdWillClose;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdDidClose;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdComplete;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdDidSucceed;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdDidFail;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdActivityDestroyed;

        public RewardedVideoAd(string placementId) : this(placementId, null)
        {

        }

        public RewardedVideoAd(string placementId, RewardData rewardData)
        {
            AudienceNetworkAds.Initialize();

            PlacementId = placementId;
            RewardData = rewardData;

            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                uniqueId = RewardedVideoAdBridge.Instance.Create(placementId, RewardData, this);

                RewardedVideoAdBridge.Instance.OnLoad(uniqueId, RewardedVideoAdDidLoad);
                RewardedVideoAdBridge.Instance.OnImpression(uniqueId, RewardedVideoAdWillLogImpression);
                RewardedVideoAdBridge.Instance.OnClick(uniqueId, RewardedVideoAdDidClick);
                RewardedVideoAdBridge.Instance.OnError(uniqueId, RewardedVideoAdDidFailWithError);
                RewardedVideoAdBridge.Instance.OnWillClose(uniqueId, RewardedVideoAdWillClose);
                RewardedVideoAdBridge.Instance.OnDidClose(uniqueId, RewardedVideoAdDidClose);
                RewardedVideoAdBridge.Instance.OnComplete(uniqueId, RewardedVideoAdComplete);
                RewardedVideoAdBridge.Instance.OnDidSucceed(uniqueId, RewardedVideoAdDidSucceed);
                RewardedVideoAdBridge.Instance.OnDidFail(uniqueId, RewardedVideoAdDidFail);
                RewardedVideoAdBridge.Instance.OnActivityDestroyed(uniqueId, RewardedVideoAdActivityDestroyed);
            }
        }

        ~RewardedVideoAd()
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
            if (handler)
            {
                handler.RemoveFromParent();
            }
            Debug.Log("RewardedVideo Ad Disposed.");
            RewardedVideoAdBridge.Instance.Release(uniqueId);
        }

        public override string ToString()
        {
            return string.Format(
                       "[RewardedVideoAd: " +
                       "PlacementId={0}, " +
                       "RewardedVideoAdDidLoad={1}, " +
                       "RewardedVideoAdWillLogImpression={2}, " +
                       "RewardedVideoAdDidFailWithError={3}, " +
                       "RewardedVideoAdDidClick={4}, " +
                       "RewardedVideoAdWillClose={5}, " +
                       "RewardedVideoAdDidClose={6}, " +
                       "RewardedVideoAdComplete={7}, " +
                       "RewardedVideoAdDidSucceed={8}, " +
                       "RewardedVideoAdDidFail={9}," +
                       "RewardedVideoAdActivityDestroyed={10}]",
                       PlacementId,
                       RewardedVideoAdDidLoad,
                       RewardedVideoAdWillLogImpression,
                       RewardedVideoAdDidFailWithError,
                       RewardedVideoAdDidClick,
                       RewardedVideoAdWillClose,
                       RewardedVideoAdDidClose,
                       RewardedVideoAdComplete,
                       RewardedVideoAdDidSucceed,
                       RewardedVideoAdDidFail,
                       RewardedVideoAdActivityDestroyed);
        }

        public void Register(GameObject gameObject)
        {
            handler = gameObject.AddComponent<AdHandler>();
        }

        public void LoadAd()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                RewardedVideoAdBridge.Instance.Load(uniqueId);
            }
            else
            {
                RewardedVideoAdDidLoad();
            }
        }

        public void LoadAd(String bidPayload)
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                RewardedVideoAdBridge.Instance.Load(uniqueId, bidPayload);
            }
            else
            {
                RewardedVideoAdDidLoad();
            }
        }

        public bool IsValid()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                return (isLoaded && RewardedVideoAdBridge.Instance.IsValid(uniqueId));
            }
            else
            {
                return true;
            }
        }

        internal void LoadAdFromData()
        {
            isLoaded = true;

            if (RewardedVideoAdDidLoad != null)
            {
                handler.ExecuteOnMainThread(() =>
                {
                    RewardedVideoAdDidLoad();
                });
            }
        }

        public bool Show()
        {
            return RewardedVideoAdBridge.Instance.Show(uniqueId);
        }

        public void SetExtraHints(ExtraHints extraHints)
        {
            RewardedVideoAdBridge.Instance.SetExtraHints(uniqueId, extraHints);
        }

        internal void ExecuteOnMainThread(Action action)
        {
            if (handler)
            {
                handler.ExecuteOnMainThread(action);
            }
        }

        public static implicit operator bool(RewardedVideoAd obj)
        {
            return !(object.ReferenceEquals(obj, null));
        }
    }

    internal interface IRewardedVideoAdBridge
    {
        int Create(string placementId, RewardData rewardData,
                   RewardedVideoAd rewardedVideoAd);

        int Load(int uniqueId);

        int Load(int uniqueId, String bidPayload);

        bool IsValid(int uniqueId);

        bool Show(int uniqueId);

        void SetExtraHints(int uniqueId, ExtraHints extraHints);

        void Release(int uniqueId);

        void OnLoad(int uniqueId,
                    FBRewardedVideoAdBridgeCallback callback);

        void OnImpression(int uniqueId,
                          FBRewardedVideoAdBridgeCallback callback);

        void OnClick(int uniqueId,
                     FBRewardedVideoAdBridgeCallback callback);

        void OnError(int uniqueId,
                     FBRewardedVideoAdBridgeErrorCallback callback);

        void OnWillClose(int uniqueId,
                         FBRewardedVideoAdBridgeCallback callback);

        void OnDidClose(int uniqueId,
                        FBRewardedVideoAdBridgeCallback callback);

        void OnComplete(int uniqueId,
                        FBRewardedVideoAdBridgeCallback callback);

        void OnDidSucceed(int uniqueId,
                          FBRewardedVideoAdBridgeCallback callback);

        void OnDidFail(int uniqueId,
                       FBRewardedVideoAdBridgeCallback callback);

        void OnActivityDestroyed(int uniqueId,
                                 FBRewardedVideoAdBridgeCallback callback);
    }

    internal class RewardedVideoAdBridge : IRewardedVideoAdBridge
    {

        /* Interface to RewardedVideo implementation */

        public static readonly IRewardedVideoAdBridge Instance;

        internal RewardedVideoAdBridge()
        {
        }

        static RewardedVideoAdBridge()
        {
            Instance = RewardedVideoAdBridge.CreateInstance();
        }

        private static IRewardedVideoAdBridge CreateInstance()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
#if UNITY_IOS
                return new RewardedVideoAdBridgeIOS();
#elif UNITY_ANDROID
                return new RewardedVideoAdBridgeAndroid();
#endif
            }
            return new RewardedVideoAdBridge();

        }

        public virtual int Create(string placementId, RewardData rewardData,
                                  RewardedVideoAd RewardedVideoAd)
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

        public virtual bool Show(int uniqueId)
        {
            return true;
        }

        public virtual void SetExtraHints(int uniqueId, ExtraHints extraHints)
        {
        }

        public virtual void Release(int uniqueId)
        {
        }

        public virtual void OnLoad(int uniqueId,
                                   FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void OnImpression(int uniqueId,
                                         FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void OnClick(int uniqueId,
                                    FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void OnError(int uniqueId,
                                    FBRewardedVideoAdBridgeErrorCallback callback)
        {
        }

        public virtual void OnWillClose(int uniqueId,
                                        FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void OnDidClose(int uniqueId,
                                       FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void OnComplete(int uniqueId,
                                       FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void OnDidSucceed(int uniqueId,
                                         FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void OnDidFail(int uniqueId,
                                      FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void OnActivityDestroyed(int uniqueId,
                                                FBRewardedVideoAdBridgeCallback callback)
        {
        }

    }

#if UNITY_ANDROID
    internal class RewardedVideoAdBridgeAndroid : RewardedVideoAdBridge
    {

        private static Dictionary<int, RewardedVideoAdContainer> rewardedVideoAds = new Dictionary<int, RewardedVideoAdContainer>();
        private static int lastKey;

        private AndroidJavaObject RewardedVideoAdForUniqueId(int uniqueId)
        {
            RewardedVideoAdContainer rewardedVideoAdContainer = null;
            bool success = RewardedVideoAdBridgeAndroid.rewardedVideoAds.TryGetValue(uniqueId, out rewardedVideoAdContainer);
            if (success) {
                return rewardedVideoAdContainer.bridgedRewardedVideoAd;
            } else {
                return null;
            }
        }

        private RewardedVideoAdContainer RewardedVideoAdContainerForUniqueId(int uniqueId)
        {
            RewardedVideoAdContainer rewardedVideoAdContainer = null;
            bool success = RewardedVideoAdBridgeAndroid.rewardedVideoAds.TryGetValue(uniqueId, out rewardedVideoAdContainer);
            if (success) {
                return rewardedVideoAdContainer;
            } else {
                return null;
            }
        }

        private string GetStringForuniqueId(int uniqueId,
                                            string method)
        {
            AndroidJavaObject rewardedVideoAd = RewardedVideoAdForUniqueId(uniqueId);
            if (rewardedVideoAd != null) {
                return rewardedVideoAd.Call<string> (method);
            } else {
                return null;
            }
        }

        private string GetImageURLForuniqueId(int uniqueId,
                                              string method)
        {
            AndroidJavaObject rewardedVideoAd = RewardedVideoAdForUniqueId(uniqueId);
            if (rewardedVideoAd != null) {
                AndroidJavaObject image = rewardedVideoAd.Call<AndroidJavaObject> (method);
                if (image != null) {
                    return image.Call<string> ("getUrl");
                }
            }
            return null;
        }

        public override int Create(string placementId, RewardData rewardData,
                                   RewardedVideoAd rewardedVideoAd)
        {

            AdUtility.Prepare();
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

            AndroidJavaObject bridgedRewardedVideoAd = new AndroidJavaObject("com.facebook.ads.RewardedVideoAd", context, placementId);

            RewardedVideoAdBridgeListenerProxy proxy = new RewardedVideoAdBridgeListenerProxy(rewardedVideoAd, bridgedRewardedVideoAd);
            bridgedRewardedVideoAd.Call("setAdListener", proxy);

            if (rewardData != null) {
                AndroidJavaObject rewardDataObj = new AndroidJavaObject("com.facebook.ads.RewardData", rewardData.UserId, rewardData.Currency);
                bridgedRewardedVideoAd.Call("setRewardData", rewardDataObj);
            }

            RewardedVideoAdContainer rewardedVideoAdContainer = new RewardedVideoAdContainer(rewardedVideoAd)
            {
                bridgedRewardedVideoAd = bridgedRewardedVideoAd,
                listenerProxy = proxy
            };

            int key = RewardedVideoAdBridgeAndroid.lastKey;
            RewardedVideoAdBridgeAndroid.rewardedVideoAds.Add(key, rewardedVideoAdContainer);
            RewardedVideoAdBridgeAndroid.lastKey++;
            return key;
        }

        public override int Load(int uniqueId)
        {
            AdUtility.Prepare();
            AndroidJavaObject rewardedVideoAd = RewardedVideoAdForUniqueId(uniqueId);
            if (rewardedVideoAd != null) {
                rewardedVideoAd.Call("loadAd");
            }
            return uniqueId;
        }

        public override int Load(int uniqueId, String bidPayload)
        {
            AdUtility.Prepare();
            AndroidJavaObject rewardedVideoAd = RewardedVideoAdForUniqueId(uniqueId);
            if (rewardedVideoAd != null)
            {
                rewardedVideoAd.Call("loadAdFromBid", bidPayload);
            }
            return uniqueId;
        }

        public override bool IsValid(int uniqueId)
        {
            AndroidJavaObject rewardedVideoAd = RewardedVideoAdForUniqueId(uniqueId);
            if (rewardedVideoAd != null) {
                return !rewardedVideoAd.Call<bool> ("isAdInvalidated");
            } else {
                return false;
            }
        }

        public override bool Show(int uniqueId)
        {
            RewardedVideoAdContainer container = RewardedVideoAdContainerForUniqueId(uniqueId);
            AndroidJavaObject rewardedVideoAd = RewardedVideoAdForUniqueId(uniqueId);
            container.rewardedVideoAd.ExecuteOnMainThread(() => {
                if (rewardedVideoAd != null) {
                    rewardedVideoAd.Call<bool> ("show");
                }
            });
            return true;
        }

        public override void SetExtraHints(int uniqueId, ExtraHints extraHints)
        {
            AdUtility.Prepare();
            AndroidJavaObject rewardedVideoAd = RewardedVideoAdForUniqueId(uniqueId);

            if (rewardedVideoAd != null)
            {
                rewardedVideoAd.Call("setExtraHints", extraHints.GetAndroidObject());
            }
        }

        public override void Release(int uniqueId)
        {
            AndroidJavaObject rewardedVideoAd = RewardedVideoAdForUniqueId(uniqueId);
            if (rewardedVideoAd != null) {
                rewardedVideoAd.Call("destroy");
            }
            RewardedVideoAdBridgeAndroid.rewardedVideoAds.Remove(uniqueId);
        }

        public override void OnLoad(int uniqueId, FBRewardedVideoAdBridgeCallback callback) {}
        public override void OnImpression(int uniqueId, FBRewardedVideoAdBridgeCallback callback) {}
        public override void OnClick(int uniqueId, FBRewardedVideoAdBridgeCallback callback) {}
        public override void OnError(int uniqueId, FBRewardedVideoAdBridgeErrorCallback callback) {}
        public override void OnWillClose(int uniqueId, FBRewardedVideoAdBridgeCallback callback) {}
        public override void OnDidClose(int uniqueId, FBRewardedVideoAdBridgeCallback callback) {}
        public override void OnActivityDestroyed(int uniqueId, FBRewardedVideoAdBridgeCallback callback) { }

    }

#endif

#if UNITY_IOS
    internal class RewardedVideoAdBridgeIOS : RewardedVideoAdBridge
    {

        private static Dictionary<int, RewardedVideoAdContainer> rewardedVideoAds = new Dictionary<int, RewardedVideoAdContainer>();

        private static RewardedVideoAdContainer rewardedVideoAdContainerForuniqueId(int uniqueId)
        {
            RewardedVideoAdContainer rewardedVideoAd = null;
            bool success = RewardedVideoAdBridgeIOS.rewardedVideoAds.TryGetValue(uniqueId, out rewardedVideoAd);
            if (success)
            {
                return rewardedVideoAd;
            }
            else
            {
                return null;
            }
        }

        [DllImport("__Internal")]
        private static extern int FBRewardedVideoAdBridgeCreate(string placementId);

        [DllImport("__Internal")]
        private static extern int FBRewardedVideoAdBridgeCreateWithReward(string placementId, string userID, string currency);

        [DllImport("__Internal")]
        private static extern int FBRewardedVideoAdBridgeLoad(int uniqueId);

        [DllImport("__Internal")]
        private static extern int FBRewardedVideoAdBridgeLoadWithBidPayload(int uniqueId, string bidPayload);

        [DllImport("__Internal")]
        private static extern bool FBRewardedVideoAdBridgeIsValid(int uniqueId);

        [DllImport("__Internal")]
        private static extern void FBRewardedVideoAdBridgeSetExtraHints(int uniqueId, string extraHints);

        [DllImport("__Internal")]
        private static extern bool FBRewardedVideoAdBridgeShow(int uniqueId);

        [DllImport("__Internal")]
        private static extern void FBRewardedVideoAdBridgeRelease(int uniqueId);

        [DllImport("__Internal")]
        private static extern void FBRewardedVideoAdBridgeOnLoad(int uniqueId,
                FBRewardedVideoAdBridgeExternalCallback callback);

        [DllImport("__Internal")]
        private static extern void FBRewardedVideoAdBridgeOnImpression(int uniqueId,
                FBRewardedVideoAdBridgeExternalCallback callback);

        [DllImport("__Internal")]
        private static extern void FBRewardedVideoAdBridgeOnClick(int uniqueId,
                FBRewardedVideoAdBridgeExternalCallback callback);

        [DllImport("__Internal")]
        private static extern void FBRewardedVideoAdBridgeOnError(int uniqueId,
                FBRewardedVideoAdBridgeErrorExternalCallback callback);

        [DllImport("__Internal")]
        private static extern void FBRewardedVideoAdBridgeOnDidClose(int uniqueId,
                FBRewardedVideoAdBridgeExternalCallback callback);

        [DllImport("__Internal")]
        private static extern void FBRewardedVideoAdBridgeOnWillClose(int uniqueId,
                FBRewardedVideoAdBridgeExternalCallback callback);

        [DllImport("__Internal")]
        private static extern void FBRewardedVideoAdBridgeOnVideoComplete(int uniqueId,
                FBRewardedVideoAdBridgeExternalCallback callback);

        [DllImport("__Internal")]
        private static extern void FBRewardedVideoAdBridgeOnServerRewardSuccess(int uniqueId,
                FBRewardedVideoAdBridgeExternalCallback callback);

        [DllImport("__Internal")]
        private static extern void FBRewardedVideoAdBridgeOnServerRewardFailure(int uniqueId,
                FBRewardedVideoAdBridgeExternalCallback callback);

        public override int Create(string placementId, RewardData rewardData,
                                   RewardedVideoAd rewardedVideoAd)
        {
            int uniqueId = 0;
            if (rewardData != null)
            {
                uniqueId = RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeCreateWithReward(placementId, rewardData.UserId, rewardData.Currency);
            }
            else
            {
                uniqueId = RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeCreate(placementId);
            }

            RewardedVideoAdBridgeIOS.rewardedVideoAds.Add(uniqueId, new RewardedVideoAdContainer(rewardedVideoAd));
            RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeOnLoad(uniqueId, rewardedVideoAdDidLoadBridgeCallback);
            RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeOnImpression(uniqueId, rewardedVideoAdWillLogImpressionBridgeCallback);
            RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeOnClick(uniqueId, rewardedVideoAdDidClickBridgeCallback);
            RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeOnError(uniqueId, rewardedVideoAdDidFailWithErrorBridgeCallback);
            RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeOnDidClose(uniqueId, rewardedVideoAdDidCloseBridgeCallback);
            RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeOnWillClose(uniqueId, rewardedVideoAdWillCloseBridgeCallback);
            RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeOnVideoComplete(uniqueId, rewardedVideoAdCompleteBridgeCallback);
            RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeOnServerRewardSuccess(uniqueId, rewardedVideoAdDidSucceedBridgeCallback);
            RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeOnServerRewardFailure(uniqueId, rewardedVideoAdDidFailBridgeCallback);

            return uniqueId;
        }

        public override int Load(int uniqueId)
        {
            return RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeLoad(uniqueId);
        }

        public override int Load(int uniqueId, string bidPayload)
        {
            return RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeLoadWithBidPayload(uniqueId, bidPayload);
        }

        public override bool IsValid(int uniqueId)
        {
            return RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeIsValid(uniqueId);
        }

        public override void SetExtraHints(int uniqueId, ExtraHints extraHints)
        {
            RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeSetExtraHints(uniqueId, JsonUtility.ToJson(extraHints));
        }

        public override bool Show(int uniqueId)
        {
            return RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeShow(uniqueId);
        }

        public override void Release(int uniqueId)
        {
            RewardedVideoAdBridgeIOS.rewardedVideoAds.Remove(uniqueId);
            RewardedVideoAdBridgeIOS.FBRewardedVideoAdBridgeRelease(uniqueId);
        }

        // Sets up internal managed callbacks

        public override void OnLoad(int uniqueId,
                                    FBRewardedVideoAdBridgeCallback callback)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container)
            {
                container.onLoad = (container.rewardedVideoAd.LoadAdFromData);
            }
        }

        public override void OnImpression(int uniqueId,
                                          FBRewardedVideoAdBridgeCallback callback)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container)
            {
                container.onImpression = callback;
            }
        }

        public override void OnClick(int uniqueId,
                                     FBRewardedVideoAdBridgeCallback callback)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container)
            {
                container.onClick = callback;
            }
        }

        public override void OnError(int uniqueId,
                                     FBRewardedVideoAdBridgeErrorCallback callback)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container)
            {
                container.onError = callback;
            }
        }

        public override void OnDidClose(int uniqueId,
                                        FBRewardedVideoAdBridgeCallback callback)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container)
            {
                container.onDidClose = callback;
            }
        }

        public override void OnWillClose(int uniqueId,
                                         FBRewardedVideoAdBridgeCallback callback)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container)
            {
                container.onWillClose = callback;
            }
        }

        public override void OnComplete(int uniqueId,
                                        FBRewardedVideoAdBridgeCallback callback)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container)
            {
                container.onComplete = callback;
            }
        }

        public override void OnDidSucceed(int uniqueId,
                                          FBRewardedVideoAdBridgeCallback callback)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container)
            {
                container.onDidSucceed = callback;
            }
        }

        public override void OnDidFail(int uniqueId,
                                       FBRewardedVideoAdBridgeCallback callback)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container)
            {
                container.onDidFail = callback;
            }
        }

        // External unmanaged callbacks (must be static)

        [MonoPInvokeCallback(typeof(FBRewardedVideoAdBridgeExternalCallback))]
        private static void rewardedVideoAdDidLoadBridgeCallback(int uniqueId)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container && container.onLoad != null)
            {
                container.onLoad();
            }
        }

        [MonoPInvokeCallback(typeof(FBRewardedVideoAdBridgeExternalCallback))]
        private static void rewardedVideoAdWillLogImpressionBridgeCallback(int uniqueId)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container && container.onImpression != null)
            {
                container.onImpression();
            }
        }

        [MonoPInvokeCallback(typeof(FBRewardedVideoAdBridgeErrorExternalCallback))]
        private static void rewardedVideoAdDidFailWithErrorBridgeCallback(int uniqueId, string error)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container && container.onError != null)
            {
                container.onError(error);
            }
        }

        [MonoPInvokeCallback(typeof(FBRewardedVideoAdBridgeExternalCallback))]
        private static void rewardedVideoAdDidClickBridgeCallback(int uniqueId)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container && container.onClick != null)
            {
                container.onClick();
            }
        }

        [MonoPInvokeCallback(typeof(FBRewardedVideoAdBridgeExternalCallback))]
        private static void rewardedVideoAdDidCloseBridgeCallback(int uniqueId)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container && container.onDidClose != null)
            {
                container.onDidClose();
            }
        }

        [MonoPInvokeCallback(typeof(FBRewardedVideoAdBridgeExternalCallback))]
        private static void rewardedVideoAdWillCloseBridgeCallback(int uniqueId)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container && container.onWillClose != null)
            {
                container.onWillClose();
            }
        }

        [MonoPInvokeCallback(typeof(FBRewardedVideoAdBridgeExternalCallback))]
        private static void rewardedVideoAdCompleteBridgeCallback(int uniqueId)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container && container.onComplete != null)
            {
                container.onComplete();
            }
        }

        [MonoPInvokeCallback(typeof(FBRewardedVideoAdBridgeExternalCallback))]
        private static void rewardedVideoAdDidSucceedBridgeCallback(int uniqueId)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container && container.onDidSucceed != null)
            {
                container.onDidSucceed();
            }
        }

        [MonoPInvokeCallback(typeof(FBRewardedVideoAdBridgeExternalCallback))]
        private static void rewardedVideoAdDidFailBridgeCallback(int uniqueId)
        {
            RewardedVideoAdContainer container = RewardedVideoAdBridgeIOS.rewardedVideoAdContainerForuniqueId(uniqueId);
            if (container && container.onDidFail != null)
            {
                container.onDidFail();
            }
        }

    }
#endif

    internal class RewardedVideoAdContainer
    {
        internal RewardedVideoAd rewardedVideoAd { get; set; }

        // iOS
        internal FBRewardedVideoAdBridgeCallback onLoad { get; set; }

        internal FBRewardedVideoAdBridgeCallback onImpression { get; set; }

        internal FBRewardedVideoAdBridgeCallback onClick { get; set; }

        internal FBRewardedVideoAdBridgeErrorCallback onError { get; set; }

        internal FBRewardedVideoAdBridgeCallback onDidClose { get; set; }

        internal FBRewardedVideoAdBridgeCallback onWillClose { get; set; }

        internal FBRewardedVideoAdBridgeCallback onComplete { get; set; }

        internal FBRewardedVideoAdBridgeCallback onDidSucceed { get; set; }

        internal FBRewardedVideoAdBridgeCallback onDidFail { get; set; }

        // Android
#if UNITY_ANDROID
        internal AndroidJavaProxy listenerProxy;
        internal AndroidJavaObject bridgedRewardedVideoAd;
#endif

        internal RewardedVideoAdContainer(RewardedVideoAd rewardedVideoAd)
        {
            this.rewardedVideoAd = rewardedVideoAd;
        }

        public override string ToString()
        {
            return string.Format("[RewardedVideoAdContainer: rewardedVideoAd={0}, onLoad={1}]", rewardedVideoAd, onLoad);
        }

        public static implicit operator bool(RewardedVideoAdContainer obj)
        {
            return !(object.ReferenceEquals(obj, null));
        }
    }

#if UNITY_ANDROID
    internal class RewardedVideoAdBridgeListenerProxy : AndroidJavaProxy
    {
        private RewardedVideoAd rewardedVideoAd;
#pragma warning disable 0414
        private readonly AndroidJavaObject bridgedRewardedVideoAd;
#pragma warning restore 0414

        public RewardedVideoAdBridgeListenerProxy(RewardedVideoAd rewardedVideoAd,
                AndroidJavaObject bridgedRewardedVideoAd)
            : base("com.facebook.ads.S2SRewardedVideoAdExtendedListener")
        {
            this.rewardedVideoAd = rewardedVideoAd;
            this.bridgedRewardedVideoAd = bridgedRewardedVideoAd;
        }

        void onError(AndroidJavaObject ad,
                     AndroidJavaObject error)
        {
            string errorMessage = error.Call<string> ("getErrorMessage");
            if (rewardedVideoAd.RewardedVideoAdDidFailWithError != null)
            {
                rewardedVideoAd.ExecuteOnMainThread(() =>
                {
                    rewardedVideoAd.RewardedVideoAdDidFailWithError(errorMessage);
                });
            }
        }

        void onAdLoaded(AndroidJavaObject ad)
        {
            rewardedVideoAd.LoadAdFromData();
        }

        void onAdClicked(AndroidJavaObject ad)
        {
            if (rewardedVideoAd.RewardedVideoAdDidClick != null)
            {
                rewardedVideoAd.ExecuteOnMainThread(() =>
                {
                    rewardedVideoAd.RewardedVideoAdDidClick();
                });
            }
        }

        void onRewardedVideoDisplayed(AndroidJavaObject ad)
        {
            if (rewardedVideoAd.RewardedVideoAdWillLogImpression != null)
            {
                rewardedVideoAd.ExecuteOnMainThread(() =>
                {
                    rewardedVideoAd.RewardedVideoAdWillLogImpression();
                });
            }
        }

        void onRewardedVideoClosed()
        {
            if (rewardedVideoAd.RewardedVideoAdDidClose != null)
            {
                rewardedVideoAd.ExecuteOnMainThread(() =>
                {
                    rewardedVideoAd.RewardedVideoAdDidClose();
                });
            }
        }

        void onRewardedVideoCompleted()
        {
            if (rewardedVideoAd.RewardedVideoAdComplete != null)
            {
                rewardedVideoAd.ExecuteOnMainThread(() =>
                {
                    rewardedVideoAd.RewardedVideoAdComplete();
                });
            }
        }

        void onRewardServerSuccess()
        {
            if (rewardedVideoAd.RewardedVideoAdDidSucceed != null)
            {
                rewardedVideoAd.ExecuteOnMainThread(() =>
                {
                    rewardedVideoAd.RewardedVideoAdDidSucceed();
                });
            }
        }

        void onRewardServerFailed()
        {
            if (rewardedVideoAd.RewardedVideoAdDidFail != null)
            {
                rewardedVideoAd.ExecuteOnMainThread(() =>
                {
                    rewardedVideoAd.RewardedVideoAdDidFail();
                });
            }
        }

        void onLoggingImpression(AndroidJavaObject ad)
        {
            if (rewardedVideoAd.RewardedVideoAdWillLogImpression != null)
            {
                rewardedVideoAd.ExecuteOnMainThread(() =>
                {
                    rewardedVideoAd.RewardedVideoAdWillLogImpression();
                });
            }
        }

        void onRewardedVideoActivityDestroyed()
        {
            if (rewardedVideoAd.RewardedVideoAdActivityDestroyed != null)
            {
                rewardedVideoAd.ExecuteOnMainThread(() =>
                {
                    rewardedVideoAd.RewardedVideoAdActivityDestroyed();
                });
            }
        }
    }
#endif

}
