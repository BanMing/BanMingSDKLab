# Chartboost Android and iOS Unity Plugin

Integrate the Chartboost plugin for Unity to let your mobile games display interstitials, reward users for video views, and enable more Chartboost features.

### Getting Started

##### Requirements

Please note that Chartboost supports Android 4.0+ and iOS 8+, so make sure to set the minimum OS versions of your Unity project accordingly.

##### Installation

After you set up your app on the Chartboost web dashboard, you can integrate Chartboost into your Unity project.

First, import all the provided files from the `Assets` folder into your Unity project.  If you are just building for iOS, you can skip the Plugins/Android subdirectory. If you're building for Android, you can skip the Plugins/iOS subdirectory.  

The files will be organized into a few different directories:

 -  `/Assets/Chartboost/Editor` : Chartboost C# code for Unity Editor integration and the post-process buildscript.
 -  `/Assets/Chartboost/Resources` : This folder contains the ChartboostSettings file, which contains the AppId and AppSignature for your iOS and Android app.
 -  `/Assets/Chartboost/Scripts` : C# code for Chartboost use. All our public-facing functions are present in Chartboost.cs within this folder.
 -  `/Assets/ChartboostExample` : An example scene demonstrating how to use the Chartboost SDK.
 -  `/Assets/Plugins/iOS` : Chartboost Objective-C code that aids integration within Unity.
 -  `/Assets/Plugins/Android` : Chartboost Android library files.

Next, if you're planning to support Android, go to the menu option *Chartboost > Edit Settings* and click the **Setup Android SDK** button. This will link the Google Play Services library into your Unity project, which is required for using the Chartboost SDK. This requires that the Google Play Services library is installed via the Android SDK Manager. (See the [Android Developer website](https://developer.android.com/google/play-services/setup.html#Install) for further instructions.)

### Sample

If you want to jump straight into things, just examine the files in the aforementioned `/Assets/ChartboostExample` folder.  Be sure to watch the log when playing with the demo scenes, as some buttons will not have obvious effects.

We highly recommend that you read over the rest of this document to understand several important concepts when integrating the Chartboost plugin with your Unity app.

### Usage

##### Chartboost Setup

The easiest way to integrate Chartboost SDK in your game is to drag and drop the Chartboost prefab present at location `/Assets/Chartboost/Chartboost`.

Next, add your AppID and AppSignature from https://dashboard.chartboost.com/<YourApp> to the settings window by going to the menu option *Chartboost > Edit Settings*.

Once you are done with the two steps above, you can show ads in your game by calling all the public functions present in the file `/Assets/Chartboost/Scripts/Chartboost.cs` belonging to the Chartboost class. Have a look at `ChartboostExample.cs` for a better idea regarding the same.

##### Calling Chartboost methods

In `/Assets/Chartboost/Scripts/Chartboost.cs` you will find the Unity-to-native methods used to interact with the Chartboost plugin:

```c#
/// <summary>
/// Cache an interstitial at the given CBLocation.
/// This method will first check if there is a locally cached interstitial
/// for the given CBLocation and, if found, will do nothing. If no locally cached data exists
///	the method will attempt to fetch data from the Chartboost API server.
/// </summary>
/// <param name="location">The location for the Chartboost impression type.</param>
public static void cacheInterstitial(CBLocation location) {
    CBExternal.cacheInterstitial(location);
}

/// <summary>
/// Determine if a locally cached interstitial exists for the given CBLocation.
/// A return value of true here indicates that the corresponding
/// showInterstitial:(CBLocation)location method will present without making
///	additional Chartboost API server requests to fetch data to present.
/// </summary>
/// <returns>true if the interstitial is cached, false if not.</returns>
/// <param name="location">The location for the Chartboost impression type.</param>
public static bool hasInterstitial(CBLocation location) {
    return CBExternal.hasInterstitial(location);
}

/// <summary>
/// Present an interstitial for the given CBLocation.
/// This method will first check if there is a locally cached interstitial
///	for the given CBLocation and, if found, will present using the locally cached data.
///	If no locally cached data exists the method will attempt to fetch data from the
///	Chartboost API server and present it.  If the Chartboost API server is unavailable
///	or there is no eligible interstitial to present in the given CBLocation this method
///	is a no-op.
/// </summary>
/// <param name="location">The location for the Chartboost impression type.</param>
public static void showInterstitial(CBLocation location) {
    CBExternal.showInterstitial(location);
}

/// <summary>
/// Cache a rewarded video at the given CBLocation.
/// This method will first check if there is a locally cached rewarded video
/// for the given CBLocation and, if found, will do nothing. If no locally cached data exists
///	the method will attempt to fetch data from the Chartboost API server.
/// </summary>
/// <param name="location">The location for the Chartboost impression type.</param>
public static void cacheRewardedVideo(CBLocation location) {
    CBExternal.cacheRewardedVideo(location);
}

/// <summary>
/// Determine if a locally cached rewarded video exists for the given CBLocation.
/// A return value of true here indicates that the corresponding
/// showRewardedVideo:(CBLocation)location method will present without making
///	additional Chartboost API server requests to fetch data to present.
/// </summary>
/// <returns>true if the rewarded video is cached, false if not.</returns>
/// <param name="location">The location for the Chartboost impression type.</param>
public static bool hasRewardedVideo(CBLocation location) {
    return CBExternal.hasRewardedVideo(location);
}

/// <summary>
/// Present a rewarded video for the given CBLocation.
/// This method will first check if there is a locally cached rewarded video
///	for the given CBLocation and, if found, will present using the locally cached data.
///	If no locally cached data exists the method will attempt to fetch data from the
///	Chartboost API server and present it.  If the Chartboost API server is unavailable
///	or there is no eligible rewarded video to present in the given CBLocation this method
///	is a no-op.
/// </summary>
/// <param name="location">The location for the Chartboost impression type.</param>
public static void showRewardedVideo(CBLocation location) {
CBExternal.showRewardedVideo(location);
}

/// <summary>
/// Cache an in play ad at the given CBLocation.
/// This method will first check if there is a locally cached in play ad
/// for the given CBLocation and, if found, will do nothing. If no locally cached data exists
///	the method will attempt to fetch data from the Chartboost API server.
/// </summary>
/// <param name="location">The location for the Chartboost impression type.</param>
public static void cacheInPlay(CBLocation location) {
    CBExternal.cacheInPlay(location);
}

/// <summary>
/// Determine if a locally cached in play ad exists for the given CBLocation.
/// A return value of true here indicates that the corresponding
/// getInPlay:(CBLocation)location method will present without making
///	additional Chartboost API server requests to fetch data to present.
/// </summary>
/// <returns>true if the in play ad is cached, false if not.</returns>
/// <param name="location">The location for the Chartboost impression type.</param>
public static bool hasInPlay(CBLocation location) {
    return CBExternal.hasInPlay(location);
}

/// <summary>
/// Gets an in play ad for the given CBLocation.
/// This method will first check if there is a locally cached in play ad
///	for the given CBLocation and, if found, will present using the locally cached data.
///	If no locally cached data exists the method will attempt to fetch data from the
///	Chartboost API server and present it.  If the Chartboost API server is unavailable
///	or there is no eligible in play ad to present in the given CBLocation this method
///	will return null.
/// </summary>
/// <returns>Returns an object of CBInPlay class, which contains a Texture2D of the appIcon and a String AppName. Look at CBInPlay.cs for more information.</returns>
/// <param name="location">The location for the Chartboost impression type.</param>
public static CBInPlay getInPlay(CBLocation location) {
    return CBExternal.getInPlay(location);
}
```
##### Additional Config functions provided by Chartboost SDK

The Chartboost SDK also provides these additional functions to configure SDK behavior:
```c#
/// <summary>
/// Confirm if an age gate passed or failed. When specified Chartboost will wait for
/// this call before showing the IOS App Store/Google Play Store.
///	If you have configured your Chartboost experience to use the age gate feature
///	then this method must be executed after the user has confirmed their age.  The Chartboost SDK
///	will halt until this is done.
/// </summary>
/// <param name="pass">The result of successfully passing the age confirmation.</param>
public static void didPassAgeGate(bool pass) {
	if(Chartboost.showingAgeGate) {
		Chartboost.doShowAgeGate(false);
		CBExternal.didPassAgeGate(pass);
	}
}

/// <summary>
/// Decide if Chartboost SDK should block for an age gate. Only available in iOS
/// </summary>
/// <param name="shouldPause">true if Chartboost should pause for an age gate, false otherwise.</param>
#if UNITY_ANDROID
[Obsolete("Age Gate is only available in iOS")]
#endif
public static void setShouldPauseClickForConfirmation(bool shouldPause) {
	#if UNITY_IPHONE
	CBExternal.setShouldPauseClickForConfirmation(shouldPause);
	#endif
}

/// <summary>
/// Get the current custom identifier being sent in the POST body for all Chartboost API server requests.
/// Use this method to get the custom identifier that can be used later in the Chartboost
/// dashboard to group information by.
/// </summary>
/// <returns>The identifier being sent with all Chartboost API server requests.</returns>
public static String getCustomId() {
    return CBExternal.getCustomId();
}

/// <summary>
/// Set a custom identifier to send in the POST body for all Chartboost API server requests.
/// Use this method to set a custom identifier that can be used later in the Chartboost
/// dashboard to group information by.
/// </summary>
/// <param name="customId">The identifier to send with all Chartboost API server requests.</param>
public static void setCustomId(String customId) {
    CBExternal.setCustomId(customId);
}

/// <summary>
/// Get the current auto cache behavior (Enabled by default).
/// If set to true the Chartboost SDK will automatically attempt to cache an impression
/// once one has been consumed via a "show" call.  If set to false, it is the responsibility of the
///	developer to manage the caching behavior of Chartboost impressions.
/// </summary>
/// <returns>true if the auto cache is enabled, false if it is not.</returns>
public static bool getAutoCacheAds() {
    return CBExternal.getAutoCacheAds();
}

/// <summary>
/// Set to enable and disable the auto cache feature (Enabled by default).
/// If set to true the Chartboost SDK will automatically attempt to cache an impression
/// once one has been consumed via a "show" call.  If set to false, it is the responsibility of the
///	developer to manage the caching behavior of Chartboost impressions.
/// </summary>
/// <param name="autoCacheAds">The param to enable or disable auto caching.</param>
public static void setAutoCacheAds(bool autoCacheAds) {
    CBExternal.setAutoCacheAds(autoCacheAds);
}

/// <summary>
/// Decide if Chartboost SDK should show interstitials in the first session.
/// Set to control if Chartboost SDK can show interstitials in the first session.
/// The session count is controlled via the startWithAppId:appSignature:delegate: method in the Chartboost class.
///	Default is true.
/// </summary>
/// <param name="shouldRequest">true if allowed to show interstitials in first session, false otherwise.</param>
public static void setShouldRequestInterstitialsInFirstSession(bool shouldRequest) {
    CBExternal.setShouldRequestInterstitialsInFirstSession(shouldRequest);
}

/// <summary>
/// Decide if Chartboost SDK will attempt to fetch videos from the Chartboost API servers.
/// Set to control if Chartboost SDK control if videos should be prefetched.
/// Default is YES.
/// </summary>
/// <param name="shouldPrefetch">true if Chartboost should prefetch video content, false otherwise.</param>
public static void setShouldPrefetchVideoContent(bool shouldPrefetch) {
    CBExternal.setShouldPrefetchVideoContent(shouldPrefetch);
}

/// <summary>
/// Send in-game level information to track user current game level activity.
/// </summary>
/// <param name="eventLabel">Event label information.</param>
/// <param name="eventField">Event level type information.</param>
/// <param name="mainLevel">Current levele mainLevel information.</param>
/// <param name="subLevel">Current levele subLevel information.</param>
/// <param name="description">Description about the level.</param>
public static void trackLevelInfo(String eventLabel, CBLevelType type, int mainLevel, int subLevel, String description) {
	CBExternal.trackLevelInfo(eventLabel, type, mainLevel, subLevel, description);
}

/// <summary>
/// Send in-game level information to track user current game level activity.
/// </summary>
/// <param name="eventLabel">Event label information.</param>
/// <param name="eventField">Event level type information.</param>
/// <param name="mainLevel">Current levele mainLevel information.</param>
/// <param name="description">Description about the level.</param>
public static void trackLevelInfo(String eventLabel, CBLevelType type, int mainLevel, String description) {
    CBExternal.trackLevelInfo(eventLabel, type, mainLevel, description);
}

//////////////////////////////////////////////////////
/// Post Install Tracking Functions
//////////////////////////////////////////////////////

#if UNITY_ANDROID

/// <summary>
/// Track an In App Purchase Event for Google Play Store.
/// Tracks In App Purchases for later use with user segmentation and targeting.
/// </summary>
/// <param name="title">The localized title of the product.</param>
/// <param name="description">The localized description of the product.</param>
/// <param name="price">The price of the product.</param>
/// <param name="currency">The localized currency of the product.</param>
/// <param name="productId">The google play identifier for the product.</param>
/// <param name="purchaseData">The purchase data string for the transaction.</param>
/// <param name="purchaseSignature">The purchase signature for the transaction.</param>
public static void trackInAppGooglePlayPurchaseEvent(string title, string description, string price, string currency, string productID, string purchaseData, string purchaseSignature) {
    CBExternal.trackInAppGooglePlayPurchaseEvent(title,description,price,currency,productID,purchaseData,purchaseSignature);
}

/// <summary>
/// Track an In App Purchase Event for Amazon Store.
/// Tracks In App Purchases for later use with user segmentation and targeting.
/// </summary>
/// <param name="title">The localized title of the product.</param>
/// <param name="description">The localized description of the product.</param>
/// <param name="price">The price of the product.</param>
/// <param name="currency">The localized currency of the product.</param>
/// <param name="productID">The amazon identifier for the product.</param>
/// <param name="userID">The user identifier for the transaction.</param>
/// <param name="purchaseToken">The purchase token for the transaction.</param>
public static void trackInAppAmazonStorePurchaseEvent(string title, string description, string price, string currency, string productID, string userID, string purchaseToken) {
    CBExternal.trackInAppAmazonStorePurchaseEvent(title,description,price,currency,productID,userID,purchaseToken);
}

#elif UNITY_IPHONE

/// <summary>
/// Track an In App Purchase Event for iOS App Store.
/// Tracks In App Purchases for later use with user segmentation and targeting.
/// </summary>
/// <param name="receipt">The transaction receipt used to validate the purchase.</param>
/// <param name="productTitle">The localized title of the product.</param>
/// <param name="productDescription">The localized description of the product.</param>
/// <param name="productPrice">The price of the product.</param>
/// <param name="productCurrency">The localized currency of the product.</param>
/// <param name="productIdentifier">The IOS identifier for the product.</param>
public static void trackInAppAppleStorePurchaseEvent(string receipt, string productTitle, string productDescription, string productPrice, string productCurrency, string productIdentifier) {
    CBExternal.trackInAppAppleStorePurchaseEvent(receipt, productTitle, productDescription, productPrice, productCurrency, productIdentifier);
}

/// <summary>
/// Set to control how the fullscreen ad units should interact with the status bar. (CBStatusBarBehaviorIgnore by default).
/// See the enum value comments for descriptions on the values and their behavior.  Only use this feature if your application has the status bar enabled.
/// </summary>
/// <param name="statusBarBehavior">The param to set if fullscreen video should respect the status bar.</param>
public static void setStatusBarBehavior(CBStatusBarBehavior statusBarBehavior) {
	CBExternal.setStatusBarBehavior(statusBarBehavior);
}

#endif
```

##### Listening to Chartboost Events

Chartboost fires off many different events to inform you of the status of impressions.  To react to these events, you must explicitly listen for them.  The best place to do this is the `OnEnable()` method of the Chartboost `MonoBehaviour` that you have created.  It's also good practice to stop listening to  events in `OnDisable()`.  Here is an example:

```c#
void OnEnable() {
	// Listen to all impression-related events
	Chartboost.didFailToLoadInterstitial += didFailToLoadInterstitial;
	Chartboost.didDismissInterstitial += didDismissInterstitial;
	Chartboost.didCloseInterstitial += didCloseInterstitial;
	Chartboost.didClickInterstitial += didClickInterstitial;
	Chartboost.didCacheInterstitial += didCacheInterstitial;
	Chartboost.shouldDisplayInterstitial += shouldDisplayInterstitial;
	Chartboost.didDisplayInterstitial += didDisplayInterstitial;
	Chartboost.didFailToRecordClick += didFailToRecordClick;
	Chartboost.didFailToLoadRewardedVideo += didFailToLoadRewardedVideo;
	Chartboost.didDismissRewardedVideo += didDismissRewardedVideo;
	Chartboost.didCloseRewardedVideo += didCloseRewardedVideo;
	Chartboost.didClickRewardedVideo += didClickRewardedVideo;
	Chartboost.didCacheRewardedVideo += didCacheRewardedVideo;
	Chartboost.shouldDisplayRewardedVideo += shouldDisplayRewardedVideo;
	Chartboost.didCompleteRewardedVideo += didCompleteRewardedVideo;
	Chartboost.didDisplayRewardedVideo += didDisplayRewardedVideo;
	Chartboost.didCacheInPlay += didCacheInPlay;
	Chartboost.didFailToLoadInPlay += didFailToLoadInPlay;
	Chartboost.didPauseClickForConfirmation += didPauseClickForConfirmation;
#if UNITY_IPHONE
	Chartboost.didCompleteAppStoreSheetFlow += didCompleteAppStoreSheetFlow;
#endif
}

void OnDisable() {
	// Remove event handlers
	Chartboost.didFailToLoadInterstitial -= didFailToLoadInterstitial;
	Chartboost.didDismissInterstitial -= didDismissInterstitial;
	Chartboost.didCloseInterstitial -= didCloseInterstitial;
	Chartboost.didClickInterstitial -= didClickInterstitial;
	Chartboost.didCacheInterstitial -= didCacheInterstitial;
	Chartboost.shouldDisplayInterstitial -= shouldDisplayInterstitial;
	Chartboost.didDisplayInterstitial -= didDisplayInterstitial;
	Chartboost.didFailToRecordClick -= didFailToRecordClick;
	Chartboost.didFailToLoadRewardedVideo -= didFailToLoadRewardedVideo;
	Chartboost.didDismissRewardedVideo -= didDismissRewardedVideo;
	Chartboost.didCloseRewardedVideo -= didCloseRewardedVideo;
	Chartboost.didClickRewardedVideo -= didClickRewardedVideo;
	Chartboost.didCacheRewardedVideo -= didCacheRewardedVideo;
	Chartboost.shouldDisplayRewardedVideo -= shouldDisplayRewardedVideo;
	Chartboost.didCompleteRewardedVideo -= didCompleteRewardedVideo;
	Chartboost.didDisplayRewardedVideo -= didDisplayRewardedVideo;
	Chartboost.didCacheInPlay -= didCacheInPlay;
	Chartboost.didFailToLoadInPlay -= didFailToLoadInPlay;
	Chartboost.didPauseClickForConfirmation -= didPauseClickForConfirmation;
#if UNITY_IPHONE
	Chartboost.didCompleteAppStoreSheetFlow -= didCompleteAppStoreSheetFlow;
#endif
}
```

On the left side of each line is the name of the Chartboost event.  All events will start with `Chartboost`.  On the right side of each line is the name of the method you have created, which will be informed of the event.  Methods don't need to be named the same as the event, but we recommend it.

In `/Assets/Chartboost/Scripts/Chartboost.cs` you'll find all of the events that are available for listening:
```c#
		/// <summary>
		///   Called after an the SDK has been initialized
		/// </summary>
		/// <param name="status">False if initialization failed</param>
		public static event Action<bool> didInitialize;

		/// <summary>
		///  Called before an interstitial will be displayed on the screen.
		///  Implement to control if the Charboost SDK should display an interstitial
		///  for the given CBLocation.  This is evaluated if the showInterstitial:(CBLocation)
		///	 is called.  If true is returned the operation will proceed, if false, then the
		///	operation is treated as a no-op and nothing is displayed. Default return is true.
		/// </summary>
		/// <returns>true if execution should proceed, false if not.</returns>
		/// <param name="location">The location for the Chartboost impression type.</param>
		public static event Func<CBLocation, bool> shouldDisplayInterstitial;

		/// <summary>
		///  Called after an interstitial has been displayed on the screen.
		///  Implement to be notified of when an interstitial has
		///  been displayed on the screen for a given CBLocation.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		public static event Action<CBLocation> didDisplayInterstitial;

		/// <summary>
		///   Called after an interstitial has been loaded from the Chartboost API
		///   servers and cached locally. Implement to be notified of when an interstitial has
		///	  been loaded from the Chartboost API servers and cached locally for a given CBLocation.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		public static event Action<CBLocation> didCacheInterstitial;

		/// <summary>
		///  Called after an interstitial has been clicked.
		///  Implement to be notified of when an interstitial has been click for a given CBLocation.
		///  "Clicked" is defined as clicking the creative interface for the interstitial.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		public static event Action<CBLocation> didClickInterstitial;

		/// <summary>
		///  Called after an interstitial has been closed.
		///  Implement to be notified of when an interstitial has been closed for a given CBLocation.
		///  "Closed" is defined as clicking the close interface for the interstitial.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		public static event Action<CBLocation> didCloseInterstitial;

		/// <summary>
		///  Called after an interstitial has been dismissed.
		///  Implement to be notified of when an interstitial has been dismissed for a given CBLocation.
		///  "Dismissal" is defined as any action that removed the interstitial UI such as a click or close.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		public static event Action<CBLocation> didDismissInterstitial;

		/// <summary>
		///   Called after an interstitial has attempted to load from the Chartboost API
		///   servers but failed. Implement to be notified of when an interstitial has attempted
		///	  to load from the Chartboost API servers but failed for a given CBLocation.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		/// <param name="CBImpressionError">The reason for the error defined via a CBImpressionError.</param>
		public static event Action<CBLocation,CBImpressionError> didFailToLoadInterstitial;

		/// <summary>
		///  Called after a click is registered, but the user is not forwarded to the IOS App Store.
		///  Implement to be notified of when a click is registered, but the user is not fowrwarded
		///  to the IOS App Store for a given CBLocation.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		/// <param name="CBClickError">The reason for the error defined via a CBClickError.</param>
		public static event Action<CBLocation, CBClickError> didFailToRecordClick;

		//// <summary>
		///  Called before a rewarded video will be displayed on the screen.
		///  Implement to control if the Charboost SDK should display a rewarded video
		///  for the given CBLocation.  This is evaluated if the showRewardedVideo:(CBLocation)
		///	 is called.  If true is returned the operation will proceed, if false, then the
		///	operation is treated as a no-op and nothing is displayed. Default return is true.
		/// </summary>
		/// <returns>true if execution should proceed, false if not.</returns>
		/// <param name="location">The location for the Chartboost impression type.</param>
		public static event Func<CBLocation, bool> shouldDisplayRewardedVideo;

		/// <summary>
		///  Called after a rewarded video has been displayed on the screen.
		///  Implement to be notified of when a rewarded video has
		///  been displayed on the screen for a given CBLocation.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		public static event Action<CBLocation> didDisplayRewardedVideo;

		/// <summary>
		///   Called after a rewarded video has been loaded from the Chartboost API
		///   servers and cached locally. Implement to be notified of when a rewarded video has
		///	  been loaded from the Chartboost API servers and cached locally for a given CBLocation.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		public static event Action<CBLocation> didCacheRewardedVideo;

		/// <summary>
		///  Called after a rewarded video has been clicked.
		///  Implement to be notified of when a rewarded video has been click for a given CBLocation.
		///  "Clicked" is defined as clicking the creative interface for the rewarded video.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		public static event Action<CBLocation> didClickRewardedVideo;

		/// <summary>
		///  Called after a rewarded video has been closed.
		///  Implement to be notified of when a rewarded video has been closed for a given CBLocation.
		///  "Closed" is defined as clicking the close interface for the rewarded video.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		public static event Action<CBLocation> didCloseRewardedVideo;

		// <summary>
		///  Called after a rewarded video has been dismissed.
		///  Implement to be notified of when a rewarded video has been dismissed for a given CBLocation.
		///  "Dismissal" is defined as any action that removed the rewarded video UI such as a click or close.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		public static event Action<CBLocation> didDismissRewardedVideo;

		/// <summary>
		///  Called after a rewarded video has been viewed completely and user is eligible for reward.
		///  Implement to be notified of when a rewarded video has been viewed completely and user is eligible for reward.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		/// <param name="reward">The reward for watching the video.</param>
		public static event Action<CBLocation,int> didCompleteRewardedVideo;

		/// <summary>
		///   Called after a rewarded video has attempted to load from the Chartboost API
		///   servers but failed. Implement to be notified of when a rewarded video has attempted
		///	  to load from the Chartboost API servers but failed for a given CBLocation.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		/// <param name="CBImpressionError">The reason for the error defined via a CBImpressionError.</param>
		public static event Action<CBLocation,CBImpressionError> didFailToLoadRewardedVideo;

		/// <summary>
		///   Called after an in play ad has been loaded from the Chartboost API
		///   servers and cached locally. Implement to be notified of when an in play has
		///	  been loaded from the Chartboost API servers and cached locally for a given CBLocation.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		public static event Action<CBLocation> didCacheInPlay;

		/// <summary>
		///   Called after an in play ad has attempted to load from the Chartboost API
		///   servers but failed. Implement to be notified of when an in play ad has attempted
		///	  to load from the Chartboost API servers but failed for a given CBLocation.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		/// <param name="CBImpressionError">The reason for the error defined via a CBImpressionError.</param>
		public static event Action<CBLocation, CBImpressionError> didFailToLoadInPlay;

		/// <summary>
		///  Called just before a video will be displayed.
		///  Implement to be notified of when a video will be displayed for a given CBLocation.
		/// </summary>
		/// <param name="location">The location for the Chartboost impression type.</param>
		public static event Action<CBLocation> willDisplayVideo;

		/// <summary>
		///  Called if Chartboost SDK pauses click actions awaiting confirmation from the user.
		///  Use this method to display any gating you would like to prompt the user for input.
		///  Once confirmed call didPassAgeGate:(BOOL)pass to continue execution.
		/// </summary>
		public static event Action didPauseClickForConfirmation;

#if UNITY_IPHONE

		/// <summary>
		///  Called after the App Store sheet is dismissed, when displaying the embedded app sheet.
		///  Implement to be notified of when the App Store sheet is dismissed.
		/// </summary>
		public static event Action didCompleteAppStoreSheetFlow;
#endif
```

##### InPlay (aka Native Ads) for iOS

The `getInPlay()` call returns a CBInPlay object. The declaration of CBInPlay class can be found in `/Assets/Plugins/Chartboost/CBInPlay.cs`.

```c#
public class CBInPlay {

        // Class variables
        public Texture2D appIcon;
        public string appName;
        private IntPtr inPlayUniqueId;

        //Class functions
        public void show()
        public void click()
    }
```
An InPlay ad (aka Native Ad) consists of `appIcon` and `appName` of the game being advertised. The `appIcon` is provided as a Texture2D, and `appName` as a string. Also, the `CBInPlay` object has two functions — `show()` and `click()` — which can be called when the InPlay ad (aka Native Ad) is shown to the user and when a user clicks on the ad.

### Troubleshooting

##### Building for iOS

The first time you build your iOS project, perform a *Build* (not a *Build and Run*) to ensure that everything is set up properly.  Once the build finishes, load up the Xcode project that is created and make sure the following frameworks are linked in the *Build Phases* settings tab for your target: `StoreKit`, `Security` and `CoreData` frameworks.

##### Building for Android

To build your Android project, build it directly to an APK file, or export to an Android Project if you want to make further modifications in Java.

By default, we have included an **AndroidManifest.xml** file that will work fine for most use cases.  If you already have your own manifest, you can copy the following lines into your manifest's `<application>` element:

```
    <!-- Required by Chartboost -->
    <meta-data android:name="com.google.android.gms.version"
        android:value="@integer/google_play_services_version" />
```

You must also make sure that the main `<activity>` element — the one that contains an `<intent-filter>` with action `android.intent.action.MAIN` — contains the following line:

```
    <!-- Required by Chartboost to be 'true' -->
    <meta-data android:name="unityplayer.ForwardNativeEventsToDalvik" android:value="true" />
```

Recent versions of Unity have set this value to `false` by default, but it must be set `true` for Chartboost to receive touch events.

Our PostProcesBuildScript attempts these changes, but it might fail in certain cases when the manifest file added in the `/Assets/Plugins/Android` is quite complex.
