Unity Change Log
================
Version 7.2.0 *(2018-5-1)*
----------------------------
This version of the Unity SDK includes the following native SDKs:
>- *Android: Version 7.2.0*
>- *iOS: Version 7.2.0*

New Features:
- Added support GDPR consent.
- Added ability to mute ads in iOS.

Version 7.1.0 *(2018-4-2)*
----------------------------
This version of the Unity SDK includes the following native SDKs:
>- *Android: Version 7.1.0*
>- *iOS: Version 7.1.2*

New Features:
- Enabled Moat viewability technology to allow serving of brand-based ads.

Unity Fixes:
- Updated support messaging.
- Updated from deprecated UnityPlayerNativeActivity class to UnityPlayerActivity.
- Updated Android minSDKVersion from 9 to 16.

Version 7.0.2 *(2018-3-6)*
----------------------------
This version of the Unity SDK includes the following native SDKs:
>- *Android: Version 7.0.1*
>- *iOS: Version 7.1.2*

Unity Fixes:
- didDismissInterstitial delegate no longer fires early.

Version 7.0.1 *(2017-11-29)*
----------------------------
This version of the Unity SDK includes the following native SDKs:
>- *Android: Version 7.0.1*
>- *iOS: Version 7.0.4*

Unity Fixes:
- Project can now be built with the "Gradle (New)" setting.

Version 7.0.0 *(2017-08-31)*
----------------------------
This version of the Unity SDK includes the following native SDKs:
>- *Android: Version 7.0.0*
>- *iOS: Version 7.0.0*

Unity Fixes:
- Updated some conditional compilations to include Unity 2017.
- Removed MoreApps from the Example Scene.

Version 6.6.5 *(2017-08-14)*
----------------------------
This version of the Unity SDK includes the following native SDKs:
>- *Android: Version 6.6.3*
>- *iOS: Version 6.6.3*

Unity Fixes:
- Reverted the plugin JDK target to 1.7.

Version 6.6.4 *(2017-07-27)*
----------------------------
This version of the Unity SDK includes the following native SDKs:
>- *Android: Version 6.6.3*
>- *iOS: Version 6.6.3*

Unity Fixes:
- Fixed compile issue with Unity 2017.1

Version 6.6.3 *(2017-05-05)*
----------------------------
This version of the Unity SDK includes the following native SDKs:
>- *Android: Version 6.6.3*
>- *iOS: Version 6.6.3*

Unity Fixes:
- Removed duplicate .jar file.

Version 6.6.2 *(2017-04-04)*
----------------------------
This version of the Unity SDK includes the following native SDKs:
>- *Android: Version 6.6.3*
>- *iOS: Version 6.6.2*

Unity Features & Improvements:
- Removed references to MoreApps.


Version 6.6.1 *(2017-01-06)*
----------------------------
This version of the Unity SDK includes the following native SDKs:
>- *Android: Version 6.6.1*
>- *iOS: Version 6.6.1*

Unity Features & Improvements:
- Added AerServ to mediation enumeration.

iOS Features & Improvements:
- Updated the underlying headers and shared lib for the Unity iOS plugin.


Version 6.6.0 *(2016-11-03)*
----------------------------
This version of the Unity SDK includes the following native SDKs:
>- *Android: Version 6.6.1*
>- *iOS: Version 6.6.0*

Unity:
- Deprecated the didDisplay delegate. willDisplay delegates will now get called just before
Chartboost ads are placed on the screen.

Android:
*Features:*
- Added support for AerServ mediation.
- Added support for HeyZap mediation.

*Improvements:*
- Added reasoning for certain app permissions in the manifest.
- Reduced DEX method count by 606 in com.chartboost and 629 overall.

*Fixes*
- MoreApps page no longer fail to show when the loading bar is enabled.
- didShow delegate is no longer called when an ad is not shown.
- Device identifiers updated on every request.
- Fixed an issue with close button placement.
- Optimized template parameter replacement.
- Cached ads are no longer shared between activities.
- Fixed a NullPointerException when backgrounding the app.
- Fixed issues with ironSource mediation.
- Fixed issues with Fyber mediation.
- Fixed issues with Corona mediation rewarded video.

iOS:
*Important*
- iOS 6 is no longer supported. iOS 7.0 is the minimum supported iOS version.

*Features*
- Added support for AerServ mediation.

*Fixes*
- Black background image no longer appears during video playback.
- App no longer freezes when backgrounding right after video playback.
- MoreApps respects the status bar app settings.

*Improvements*
- Improved rendering performance when rotating an ad.

Version 6.5.1 *(2016-09-29)*
----------------------------
This version of the Unity SDK includes the following native SDKs:
>- *Android: Version 6.5.1*
>- *iOS: Version 6.5.2*

Android:
*Fixes:*
- Fixed a bug where tilting the screen while an ad was playing could change the app orientation when it should not.

iOS:
*Fixes:*
- Fixed a bug that could cause visual defects on ads when the app lost and regained focus.

Version 6.5.0 *(2016-09-22)*
----------------------------
Important:
- Support for iOS 6 has been dropped. iOS 7.0 is the minimum supported iOS Version.
This version of the Unity SDK includes the following native SDKs:
>- *Android: Version 6.5.1*
>- *iOS: Version 6.5.1*

Unity:
- Deprecated Android methods have been removed; please see Android notes below.
- Added HyprMX as a CBMediation value.

Android:
*Features:*
- Chartboost SDK now uses network compression for improved performance.

*Improvements:*
- ```void setFrameworkVersion()``` has been deprecated. Please use ```void setChartboostWrapperVersion()``` to set wrapper version.
- Added new mediation enum value HyprMX.
- Error codes are more accurate and descriptive.
- Previously deprecated methods and classes now removed:
    - ```ChartboostActivity``` class
    - ```boolean getIgnoreErrors()```
    - ```void setIgnoreErrors(boolean ignoreErrors)```
    - ```void didPassAgeGate(boolean pass)```
    - ```void setShouldPauseClickForConfirmation(boolean shouldPause)```
    - ```void clearCache()```
    - ```void setFramework(final CBFramework framework)```
    - ```boolean getImpressionsUseActivities()```
    - ```void setImpressionsUseActivities(final boolean impressionsUseActivities)```
    - ```void didPauseClickForConfirmation()```
    - ```void didPauseClickForConfirmation(Activity activity)```

*Fixes:*
- Invalid server responses no longer accepted as valid.
- Fixed an issue with the back button not calling the dismiss/close delegates.
- Cached ads will no longer fail to show.
- SDK will no longer keep 0 byte files in cache when a download fails.
- Video ads no longer stay paused after maximizing a minimized app.

iOS:
*Updates*
- Networking stack has been upgraded to AFNetworking 3.0.
- Added features to help support iOS changes to limit of ad tracking.
- iOS 10 compatibility.
*Fixes*
- Fixed IFV reporting bug.
- Fixed bug that could retain a bad ad unit in the cache.
- Fixed bug that could send an inaccurate list of local videos to the server.
- Changed timeout for bad ad units to 3 seconds to avoid excessive loading bar wait time.

Version 6.4.5 *(2016-06-30)*
----------------------------
This version of the Unity SDK includes the following native SDKs:
>- *Android: Version 6.4.2*
>- *iOS: Version 6.4.6*

Android:
*Features:*
- The Chartboost Android SDK no longer supports the age gate feature. API methods related to age gate are being deprecated and will be removed in a future release.

- Publishers only: All Chartboost ads are now shown using CBImpressionActivity. You must add CBImpressionActivity to your AndroidManifest.xml file, like this:

<activity android:name="com.chartboost.sdk.CBImpressionActivity"
                 android:excludeFromRecents="true"
                 android:hardwareAccelerated="true"
                 android:theme="@android:style/Theme.Translucent.NoTitleBar.Fullscreen"
                 android:configChanges="keyboardHidden|orientation|screenSize"/>

*Note:* Make sure hardware acceleration is added and enabled when declaring the CBImpressionActivity in the manifest.

Android:
*Fixes:*
- Media loading is now more efficient.
- Memory usage on devices has been optimized.
- Video download is now more efficient.
- Fixed ConcurrentModificationException occurring in some devices.
- Fixed issue with video playback when backgrounding.
- Fixed IMPRESSION_ALREADY_VISIBLE error.

iOS:
*Fixes:*
- Fixed Native Ads crash when clicking.
- Fixed intermittent issue when plugging or unplugging headphones.
- Fixed several threading issues leading to a EXC_BAD_ACCESS KERN_INVALID_ADDRESS error.

Version 6.4.4 *(2016-05-04)*
----------------------------
>- *Android: Version 6.4.1*
>- *iOS: Version 6.4.4*

*Features & Improvements:*

iOS:
- Small bug fixes

Version 6.4.2 *(2016-03-31)*
----------------------------
>- *Android: Version 6.4.1*
>- *iOS: Version 6.4.2*

*Features & Improvements:*

iOS:
- Small bug fixes

Version 6.4.1 *(2016-03-24)*
----------------------------
>- *Android: Version 6.4.1*
>- *iOS: Version 6.4.0*

*Features & Improvements:*

Android:
- Various bug fixes

Version 6.4.0 *(2016-03-14)*
----------------------------
>- *Android: Version 6.4.0*
>- *iOS: Version 6.4.0*

- With this SDK we will begin rolling out a brand new Chartboost Video experience, tailored specially for games. Upgrade now!
- We've also improved key features like video caching, memory management, and data usage for optimal SDK performance.

*Features & Improvements:*
- Added a retry mechanism for downloading failed assets.
- iOS: Calls to 'cacheInterstitial' and 'cacheRewarded' have been optimized to require less memory.
- iOS: Interstitials will now fail gracefully instead of showing with missing assets.
- iOS: Closing the "loading" view has moved to the main thread.
- Android: Cached impressions on soft bootups are invalidated if any shared assets are deleted.
- Android: Fixed issues causing "Could Not Delete Cache Entry for key" log message on startup.


Version 6.3.0 *(2016-02-16)*
----------------------------
>- *Android: Version 6.3.0*
>- *iOS: Version 6.3.0*

- Upgrade now to enable Chartboost's behind-the-scenes video optimization.

*Improvements*
- Android: Older devices in the networking library no longer crash on occasion.
- Android: MoreApps rotation issue has been resolved.
- Android: Removed "Could Not Delete Cache Entry for key" notification from logs.
- Android: Fixed a warning about "Internal_Create can only be called from the main thread"
- iOS: Certain network request failures no longer cause a crash.




Version 6.2.0 *(2016-01-25)*
----------------------------
>- *Android: Version 6.2.0*
>- *iOS: Version 6.2.0*

*Features*  

- Added didInitialize() delegate callback to notify when the SDK is initialized and ready to use.  

*Improvements*  

- Android: Screen no longer turns black on occasion when playing videos.  
- Android: Chartboost impressions no longer have a translucent view behind them.  
- Android: Improved handling of views when the app is minimized after clicking an ad.  
- Android: More efficient handling of cached assets now reduces network usage.  
- Android: InPlay now triggers the failure delegate on failure.  
- iOS: Blank ads no longer appear when the device is rotated into an unsupported orientation.  
- iOS: App no longer crashes on startup when CoreData does not initialize (frequently occurred if device was out of memory).  
- iOS: Fixed a rare crash that occurred when reopening the app after an ad is displayed.  


Version 6.0.2 *(2015-10-30)*
----------------------------

>- *Android: Version 6.0.2*
>- *iOS: Version 6.0.1*

- Feature
    Video experience rebuilt from the ground up, upgrade to this SDK to enable Chartboost's behind-the-scenes video optimization (highly recommended!)

Android:

- *Recommended Permission*
    Pause/Resume playback during phone calls. SDK need to know when a phone call come in and when it is finished when we show video ads.
    ```<uses-permission android:name="android.permission.READ_PHONE_STATE"/>```

- *Required*
    Build SDK Version - 23
    Compile SDK Version - 23

- *Optional*
    Target SDK Version - 23

iOS:

- cacheRewardedVideo would stop audio of the playing video. 
- pulling headphones out during the confirmation screen of rewarded videos no longer starts the video. 
- accepting or declining a phonecall during the confirmation screen of rewarded videos no longer starts the video. 


Version 5.5.1 *(2015-07-08)*
----------------------------

>- *Android: Version 5.5.0*
>- *iOS: Version 5.5.1*

Fixes:
- iOS: Fixed an issue that crashed or hung the SDK during initialization. 

Version 5.5.0 *(2015-06-28)*
----------------------------
>- *Android: Version 5.5.0*
>- *iOS: Version 5.5.0*

Features:
- The Chartboost gameObject no longer creates a second instance if one already exists. 
- Added a Chartboost.Create() method that will create a Chartboost gameObject if one does not already exist. It is no longer necessary to drag one into your first scene as long as you call this method at least once. 
- Android: Fixed an issue with age gate appearing behind the ad display. 

Fixes:
- Sample app now shows InPlay ads with the rest of the interface. 

- Android: Age gate now hides the ad between the `didPauseClickForConfirmation` call and `didPassAgeGate` response.  
- Android: The Chartboost.cs onGUI() call has been optimized to reduce memory allocations per frame. 
- Android: Video ads stop playing when didPauseClickForConfirmation() is called. 
- Android: Removed an unnecessary video prefetch call. 
- Android: Fixed Chartboost activity leaking in some cases.  
- Android: Loading screens were improperly showing for interstitial and rewarded videos.  
- Android: Typos fixed in some post install event responses. 
- Android: Timezone format fixed for Android 4.1 and early. 

- iOS: Phone calls during video ads caused video to pause with no way to resume.  
- iOS: The video prefetcher would sometimes delete the requested video causing the ad to fail.  
- iOS: MoreApps now correctly caches the next page on the first call instead of reusing the data. 
- iOS: CoreData warning about not being able to load CBHTTPRequestEntity has been fixed.  

Improvements:
- iOS: Rewarded videos no longer need to wait until the video prefetcher is complete before showing an existing video. 
- iOS: AFNetworking library updated to version 2.5.4. 

Version 5.4.0 *(2015-05-28)*
----------------------------
>- *Android: Version 5.4.0*
>- *iOS: Version 5.4.0*

Features:

Fixes:

- Android: Ads will no longer disappear on rotation if using Activities 
- Android: Reward video pre roll screen didn't't show on cached calls. 
- Android: Fixed null pointer exception when initializing Chartboost and making calls on different threads. 
- Android: Fixed ads disappearing on rotation when using activities.  
- Android: Under certain circumstances, the SDK was failing to prefetch videos. 

Improvements:
- ChartboostExample UI layout that shows delegate logs, cache state, and allows the user to set various options including Show Age Gate. 
- Added CBClickError enums for didFailToRecordClick delegate. 
- Added IAP button for Android in example. 
- Android:Added error logging for all network calls. 
- Sends the timezone and network type along with every request to be used by analytics and the ad server. 

Version 5.3.0 *(2015-04-30)*
----------------------------
>- *Android: Version 5.3.0*
>- *iOS: Version 5.3.0*

Features:

- Added [Weeby](http://www.weeby.co) to available frameworks. 

Fixes:

- Android: Ad will no longer disappear in some devices if app is backgrounded 

Improvements:

- Added a way to detect rooted devices. 
- Native Chartboost SDKs now send the version of the Unity wrapper SDK along with every request. 

Version 5.2.1 *(2015-04-09)*
----------------------------
>- *Android: Version 5.2.0*
>- *iOS: Version 5.2.1*

Features:

Fixes:

- Fix issue for symbol collision on `MakeStringCopy` with other third party SDKs. 
- Fix issue for applications not building in XCode if built using Unity 5. 
- Fix issue for CoreData crash occuring on the first bootup of an app. 
- Fix an issue for a symbol collision on `audioRouteChangeListenerCallback` with other third party SDKs. 
- Fix issue for more apps not resizing when changing orientation. 

Improvements:

Version 5.2.0 *(2015-04-03)*
----------------------------
>- *Android: Version 5.2.0*
>- *iOS: Version 5.2.0*

Features:

- Add a developer facing function to force close any visible impression and or loading views. 
- Added a new method to CBAnalytics 'trackInAppPurchaseEventWithString' that acts like the trackInAppPurchaseEvent but takes a string instead of raw receipt data to allow frameworks to pass in base64 encoded receipts. 
- PIA Level Tracking available in CBAnalytics 

Fixes:

- iOS: The receipt parameter to Chartboost.trackInAppAppleStorePurchaseEvent() is now assumed to be a base64 encoded string.  Added a method to the example app to demonstrate this. 
- Fixed an issue with age gate being covered up by rewarded video & video interstitial ad types. The impression will pause and disappear so the user can complete age gate and re-appear and resume after a user selection. 
- Fix headphone unplug pausing rewarded video & video interstitial campaigns. This would lead to a deadlock situation in rewarded video and poor user experience in video interstitial 
- Change the conditions for when we fire the didFailToLoadRewardedVideo delegate. It will now fire if a rewarded video is requested before prefetching finishes. 
- When setShouldDisplayLoadingViewForMoreApps is set to YES, the loading view was not being displayed in a timely manner. This is now fixed by running it on the main thread. 
- Fix Android SDK stops showing ads if dismissing too fast when setImpressionsUseActivities set to true. 
- Fix Android 5.1.3 native SDK hasRewardedVideo can return true but show call fails with Error: VIDEO_UNAVAILABLE. 
- Fix Android SDK 5.1.1 crashes sometimes if back button pressed after interstitial is shown. 
- Fix Android 5.1.1 crashing sporadically on showInterstitial calls. 
- Fix Android 5.1.3 native SDK Crashing sometimes after viewing interstitial. 
- Remove all exception thrown from the SDK. 
- Fix /api/install not being called on every bootup for Android SDK. 
- Fix video should no longer leave any transparent space when fullscreen. 

Improvements:

- New Core Data backed persistence layer for the Chartboost request manager. 
- New way for SDK to batch requests for an endpoint. Cuts down on outgoing network requests. Only enabled for level tracking. 
- Update Amazon IAP library to v2.0. 

Version 5.1.4 *(2015-03-13)*
----------------------------
>- *Android: Version 5.1.3*
>- *iOS: Version 5.1.5*

Features:

Fixes:

- Builds with Unity 5.0. 
- Android now blocks UI clicks behind the ads 
- Fix Seeing duplicate calls for showInterstitial on Unity Android. 
- Fix Seeing duplicate calls for showRewardedVideo on Unity Android. 
- Fix issue where close buttons for video were not working on startup. 

Improvements:

Version 5.1.3 *(2015-02-26)*
----------------------------
>- *Android: Version 5.1.2*
>- *iOS: Version 5.1.4*

Features:

- Added a new method 'setStatusBarBehavior' to control how fullscreen video ads interact with the status bar. 

Fixes:

- Fix application force quiting on back pressed when no ads shown. 
- Fix didDismissInterstitial and didDismissRewardedVideo not executing on Android. 
- Fix incorrect debug message in CBManifestEditor.cs. 
- Fix for duplicate calls to the creative url from the SDK. This should fix issues with third party click tracking reporting click numbers twice what we report. 
- Fix for max ads not being respected when campaign is set to show once per hour and autocache is set to YES. There is now a small delay after successful show call. 
- Fix issue for interstitial video and rewarded video calling didDismissInterstitial or didDismissRewardedVideo during a click event. 
- Fix didCacheInterstitial not being called if an ad was already cached at the location. 
- Fix issue where didClickInterstitial not firing for interstitial video. 
- Fix issue where close buttons for fullscreen video were appearing behing the status bar. 

Improvements:

- Added the location parameter, when available, to more relevant network requests. This should provide more information to analytics. 

Version 5.1.2 *(2015-02-20)*
----------------------------
>- *Android: Version 5.1.1*
>- *iOS: Version 5.1.3*

Features:

Fixes:

- Fix back button issue. 
- Fix for devkit engine issue on video playback. 
- Fix video crash issue on crash exception. 
- Fixes for video issues. 

Improvements:


Version 5.1.1 *(2015-01-16)*
----------------------------
>- *Android: Version 5.1.0*
>- *iOS: Version 5.1.3*

Features:

Fixes:

- Disabled request retries by default, fix for multithreaded crashes 
- Fix for when the device is in an orientation we do not have assets for. Instead of failing silently a CBLoadError is now called.  

Improvements:

- Better inplay caching
- Added orientation information to api-click, and video-complete calls. Allows for better analytics 
- Remove hardcoded affiliate token. Now pulled from the server 
- Added example usage of isAnyViewVisible: delegate method into the sample project 
- Added inplay button to the chartboost example app 
- Improved logging for when someone tries to show a rewarded video with prefetching disabled. Instead of failing silently a CBLoadError is called. 


Version 5.1.0 *(2014-12-09)*
----------------------------
>- *Android: Version 5.1.0*
>- *iOS: Version 5.1.2*

Features:

Fixes:

- Fix race condition between actions for video on replay. 
- Fix loading screen causing issues with video and app sheet. 
- Fix interstitial video close button appearing at incorrect time in portrait orientation. 
- Fix rewarded video autoplays when previous display of video is dismissed instead of watched. 
- Fix api/config not executing on soft bootups. 
- Fix close button clipping the video player in corner. 
- Fix api/install not being sent for soft bootups. 
- Fix for various crashes due to memory pressure and concurrency. 
- Fix for api/track executing on hidden files for older devices. 
- Fix for rotating iPhone 6/6+ can cause video to display off screen. 
- Fix for incorrect error code enumerations being used. 
- Fix loading view not appearing for more apps page on slow connections. 
- Fix crash in CBAnalytics if sent an invalid NSDecimalNumber. 
- Fix age gate with More Apps so that it is not behind the More Apps view. 
- Fix SKStoreProductViewController rotation issue with Unity. 
- Fix build for armv7s architectures. 
- Fix CBAppCall crash if no resource path sent with URL. 
- Fix bug with SKStoreProductViewController crashing due to race condition. 
- Fix SKStoreProductViewController rotation issue with Unity. 
- Fix concurrency issue in CBConfig. 
- Fix Android SDK 5.0.3 crashing on Video Interstitial due to NullPointerException. 
- Fix Android SDK: Proper error enum names. 
- Fix MoreApps Unity 5.0.2 and 5.0 for Android doesn't work. 
- Fix Unity: Parity Android error enum. 
- Fix SDK sends "custom-id" but ad server expects "custom_id" field. 
- Fix See black screen and not video when reward/get has been modified. 
- Fix Android Video: Black screen for invalid response. 
- Fix Rewarded Video request freezes app if pre-roll popup gets closed after watching at least one video. 

Improvements:

- Added new framework tracking values for Cordova and CocoonJS. 
- Added new API to check visibility of Chartboost UI. 
- Changed delegate callbacks for click and close to be sent after closing or clicking the impression. 
- Changed autocache calls to delay execution for better performance. 

Version 5.0.3 *(2014-09-22)*
----------------------------

- Bugfixes and stability improvements.

Version 5.0.2 *(2014-09-12)*
----------------------------

- Higher eCPM for Android.
- Bugfixes and stability improvements.

Version 5.0.1 *(2014-09-09)*
----------------------------

- Compatibility with various third party packages.
- Post processing will remove legacy Chartboost files.
- Bugfixes and stability improvements.

Version 5.0 *(2014-08-07)*
----------------------------

- Rewrote the whole SDK to organize the code in a much better way
- Android and iOS parity has been achieved
- The SDK has become much much easier to use, one can simply import a prefab in their scene and they are done
- Added features like InPlay and Video
- The delegates work properly now, they return the value we expect them to return by asking the game instead of default values

Version 4.1.0 *(2014-05-15)*
----------------------------

 - iOS: Updated to iOS library version 4.4
 - iOS: Added rewarded video impressions
 - iOS: Added age gate feature
 - iOS: Added control over first session interstitials and a few other small features from native SDK
 - Added named locations for impressions, see CBLocation

Version 4.0.1 *(2014-04-22)*
----------------------------

 - Android: Fix for a crash due to Google Play Services security permission check

Version 4.0.0 *(2014-04-17)*
----------------------------

 - Updated Android SDK to 4.0.0 (Compatible with Android versions 2.3+)
 - Updated iOS SDK to 4.1 (Compatible with iOS versions 5.1+)
 - Added CBImpressionError parameters to "didFailToLoad" EventListener methods
 - Simplified plugin in Android, does not override main activity -- this should eliminate conflicts with other plugins
 - Initialize the Android plugin almost entirely in code -- use the setup dialog from the file menu and call `CBBinding.init(appID, appSignature)`
 - Android: Removed option to use activities for Chartboost Impressions
 - iOS: Removed unused iOS methods

Version 3.4.0 *(2014-02-25)*
----------------------------

 - Updated Android SDK to 3.4.0

Version 3.3.0 *(2013-09-12)*
----------------------------

 - Merged iOS and Android plugins - now just one almost identical API intelligently works on both platforms
 - Repackaged all Chartboost C
 - Android: Renamed `ChartBoostAndroid` to `CBBinding` and `ChartBoostAndroidManager` to `CBManager`
 - Android: Fixed issues related to touch input passing through Chartboost impressions
 - Android: Added ability to customize pause behavior while Chartboost impressions are visible
 - iOS: Renamed `ChartBoostBinding` to `CBBinding` and `ChartBoostManager` to `CBManager`
 - iOS: Updated plugin to avoid possible Xcode build error with Unity 4.2
 - Unity meta files added to allow test scene to find scripts on import
