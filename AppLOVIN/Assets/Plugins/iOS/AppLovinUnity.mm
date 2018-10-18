//
//  AppLovinUnity.mm
//  sdk
//

#import "ALSdk.h"
#import "ALAdView.h"
#import "ALInterstitialAd.h"
#import "ALSdkSettings.h"
#import "ALAdDelegateWrapper.h"
#import "ALIncentivizedInterstitialAd.h"
#import "ALInterstitialCache.h"
#import "ALAdType.h"
#import "ALManagedLoadDelegate.h"
#import "ALEventTypes.h"
#import "ALPrivacySettings.h"

#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wdeprecated-declarations"

UIView* UnityGetGLView();

NSString * kDefaultZoneIdForRegularBanner = @"zone_id_regular_banner";
NSString * kDefaultZoneIdForRegularInterstitial = @"zone_id_regular_interstitial";
NSString * kDefaultZoneIdForIncentInterstitial = @"zone_id_incent_interstitial";

// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform C function naming rules
extern "C" {
    static const NSString * UNITY_PLUGIN_VERSION = @"5.1.3";
    static const NSString * UNITY_BUILD_NUMBER = @"50103";
    
    static const CGFloat POSITION_CENTER = -10000;
    static const CGFloat POSITION_LEFT = -20000;
    static const CGFloat POSITION_RIGHT = -30000;
    static const CGFloat POSITION_TOP = -40000;
    static const CGFloat POSITION_BOTTOM = -50000;
    static const CGFloat POSITION_SAFE_BOTTOM = -60000;

    static NSString * const SERIALIZED_KEY_VALUE_PAIR_SEPARATOR = [NSString stringWithFormat: @"%c", 28];
    static NSString * const SERIALIZED_KEY_VALUE_SEPARATOR = [NSString stringWithFormat: @"%c", 29];

    static CGFloat adX;
    static CGFloat adY;

    static ALInterstitialCache* interstitialCache;

    static ALAdView *adView;
    static ALAdDelegateWrapper* delegateWrapper;
    
    /**
     * Helper method definitions
     */
    NSString * _AppLovinGetRegularInterstitialZoneIdentifier(const char *zoneId);
    NSString * _AppLovinGetIncentInterstitialZoneIdentifier(const char *zoneId);
    void _AppLovinLoadShowIntersitital( const char *zoneId, BOOL isIncentivized, BOOL showWhenLoaded );

    /**
     *  For internal use only
     */

    void maybeInitializeDelegateWrapper()
    {
        if(!delegateWrapper) { delegateWrapper = [[ALAdDelegateWrapper alloc] init]; }
        if(!interstitialCache) { interstitialCache = [ALInterstitialCache shared]; }
    }
    
#pragma mark - SDK Initialization
    
    void _AppLovinInitializeSdk()
    {
        [[ALSdk shared] initializeSdk];
        [[ALSdk shared] setPluginVersion: [NSString stringWithFormat:@"unity-%@", UNITY_PLUGIN_VERSION]];
        
        maybeInitializeDelegateWrapper();
    }
    
    void _AppLovinSetUnityAdListener(const char* gameObjectToNotify)
    {
        maybeInitializeDelegateWrapper();
        delegateWrapper.gameObjectToNotify = [NSString stringWithCString: gameObjectToNotify encoding: NSUTF8StringEncoding];
    }
    
#pragma mark - AdView Methods

    ALAdView *SharedAdview()
    {
        if (!adView)
        {
            [[ALSdk shared] setPluginVersion:[NSString stringWithFormat:@"unity-%@", UNITY_PLUGIN_VERSION]];
            adView = [[ALAdView alloc] initWithSize: [ALAdSize sizeBanner]];

            maybeInitializeDelegateWrapper();
            ALManagedLoadDelegate *managedLoadDelegate = [ALManagedLoadDelegate sharedDelegateForZoneIdentifier: kDefaultZoneIdForRegularBanner
                                                                                                           size: [ALAdSize sizeBanner]
                                                                                                           type: [ALAdType typeRegular]
                                                                                                        wrapper: delegateWrapper];
            managedLoadDelegate.adView = adView;
            [adView setAdLoadDelegate: managedLoadDelegate];
            [adView setAdDisplayDelegate: delegateWrapper];
            [adView setAdEventDelegate: delegateWrapper];
        }

        return adView;
    }

    /**
     *  Show AppLovin Banner or MRec Ad
     */
    void _AppLovinShowAd(const char *zoneId)
    {
        [SharedAdview() setHidden:false];
        
        if ( zoneId != NULL )
        {
            ALManagedLoadDelegate *delegate = [ALManagedLoadDelegate sharedDelegateForZoneIdentifier: kDefaultZoneIdForRegularBanner
                                                                                                size: [ALAdSize sizeBanner]
                                                                                                type: [ALAdType typeRegular]
                                                                                             wrapper: delegateWrapper];
            delegate.showBannerOnLoad = YES;
            
            NSString *zoneIdentifier = [NSString stringWithCString: zoneId encoding: NSStringEncodingConversionAllowLossy];
            [[ALSdk shared].adService loadNextAdForZoneIdentifier: zoneIdentifier andNotify: delegate];
        }
        else
        {
            [SharedAdview() loadNextAd];
        }
        
        [UnityGetGLView() addSubview:SharedAdview()];
    }

    /**
     *  Hide AppLovin Banner Ad
     */
    void _AppLovinHideAd()
    {
        [SharedAdview() setHidden:true];
    }

    /**
     *  For internal use only
     */
    CGFloat getAvailableScreenWidth()
    {
        CGRect screenBounds = [[UIScreen mainScreen] applicationFrame];

        UIInterfaceOrientation orientation = [[UIApplication sharedApplication] statusBarOrientation];

        CGFloat width = screenBounds.size.width;

        // Don't trust the system
        if ((UIInterfaceOrientationIsLandscape(orientation) && screenBounds.size.height > screenBounds.size.width) || (UIInterfaceOrientationIsPortrait(orientation) && screenBounds.size.width > screenBounds.size.height))
        {
            width = screenBounds.size.height;
        }

        return width;
    }

    /**
     *  For internal use only
     */
    CGFloat getAvailableScreenHeight()
    {
        CGRect screenBounds = [[UIScreen mainScreen] applicationFrame];

        UIInterfaceOrientation orientation = [[UIApplication sharedApplication] statusBarOrientation];

        CGFloat height = screenBounds.size.height;

        if ((UIInterfaceOrientationIsLandscape(orientation) && screenBounds.size.height > screenBounds.size.width) || (UIInterfaceOrientationIsPortrait(orientation) && screenBounds.size.width > screenBounds.size.height))
        {
            height = screenBounds.size.width;
        }

        return height;
    }

    /**
     *  For internal use only
     */
    void updateAdPosition()
    {
        CGRect newRect = SharedAdview().frame;

        if (adX == POSITION_CENTER)
        {
            newRect.origin.x = getAvailableScreenWidth() / 2 - newRect.size.width / 2;
        }
        else if (adX == POSITION_LEFT)
        {
            newRect.origin.x = 0;
        }
        else if (adX == POSITION_RIGHT)
        {
            newRect.origin.x = getAvailableScreenWidth() - newRect.size.width;
        }
        else
        {
            newRect.origin.x = adX;
        }

        if (adY == POSITION_TOP)
        {
            newRect.origin.y = 0;
        }
        else if (adY == POSITION_BOTTOM)
        {
            newRect.origin.y = getAvailableScreenHeight() - newRect.size.height;
        }
        else if ( adY == POSITION_SAFE_BOTTOM )
        {
            // If this was used on iOS 11+
            if ( [UIWindow instancesRespondToSelector: @selector(safeAreaInsets)] )
            {
                NSValue *safeAreaInsetsValue = [[UIApplication sharedApplication].keyWindow valueForKey: @"safeAreaInsets"];
                UIEdgeInsets safeAreaInsets = safeAreaInsetsValue.UIEdgeInsetsValue;
                newRect.origin.y = getAvailableScreenHeight() - newRect.size.height - safeAreaInsets.bottom;
            }
            else
            {
                newRect.origin.y = getAvailableScreenHeight() - newRect.size.height;
            }
        }
        else
        {
            newRect.origin.y = adY;
        }

        SharedAdview().frame = newRect;
    }

    /**
     * Set the position of the banner ad
     *
     * @param x   Horizontal position of the ad in dp or constant (POSITION_LEFT, POSITION_CENTER, POSITION_RIGHT)
     * @param y   Veritcal position of the ad in dp or constant (POSITION_TOP, POSITION_BOTTOM)
     */
    void _AppLovinSetAdPosition(float x, float y)
    {
        adX = x;
        adY = y;

        updateAdPosition();
    }

    /**
     * Set the width of the banner ad
     *
     * @param width   Width of the ad in dp
     */
    void _AppLovinSetAdWidth(int width)
    {
        CGRect newRect = [SharedAdview() frame];

        newRect.size.width = width;

        SharedAdview().frame = newRect;

        updateAdPosition();
    }

#pragma mark - SDK Settings
    
    /**
     * Set the AppLovin SDK key for the application
     *
     * @param sdkKey    The SDK key for the application
     */
    void _AppLovinSetSdkKey(const char * sdkKey)
    {
        if (!sdkKey) { return; };

        NSString *sdkKeyStr = [NSString stringWithUTF8String: sdkKey];

        NSDictionary *infoDict = [[NSBundle mainBundle] infoDictionary];
        [infoDict setValue: sdkKeyStr forKey: @"AppLovinSdkKey"];
    }
    
    void _AppLovinSetVerboseLoggingOn(const char * verboseLogging)
    {
        NSString* verboseLoggingStr = [NSString stringWithUTF8String: verboseLogging];
        [[ALSdk shared] settings].isVerboseLogging = [verboseLoggingStr boolValue];
    }

    void _AppLovinSetMuted(const char * muted)
    {
        NSString *mutedStr = [NSString stringWithUTF8String: muted];
        [[ALSdk shared] settings].muted = [mutedStr boolValue];
    }
    
    bool _AppLovinIsMuted()
    {
        return [[ALSdk shared] settings].muted;
    }
    
    void _AppLovinSetTestAdsEnabled(const char * enabled)
    {
        NSString *enabledStr = [NSString stringWithUTF8String: enabled];
        [[ALSdk shared] settings].isTestAdsEnabled = [enabledStr boolValue];
    }
    
    bool _AppLovinIsTestAdsEnabled()
    {
        return [[ALSdk shared] settings].isTestAdsEnabled;
    }
    
    //
    // Interstitial ad loading / showing
    //
    
    void _AppLovinPreloadInterstitial( const char *zoneId )
    {
        _AppLovinLoadShowIntersitital( zoneId, NO, NO);
    }
    
    bool _AppLovinHasPreloadedInterstitial(const char *zoneId)
    {
        NSString * zoneIdentifier = _AppLovinGetRegularInterstitialZoneIdentifier( zoneId );
        
        ALAdService * adService = [ALSdk shared].adService;
        BOOL hasPreloadedInter = NO;
        if ( zoneId != NULL )
        {
            hasPreloadedInter = [adService hasPreloadedAdForZoneIdentifier: zoneIdentifier];
        }
        else
        {
            hasPreloadedInter = [adService hasPreloadedAdOfSize: [ALAdSize sizeInterstitial]];
        }
        
        return [interstitialCache hasAdForZoneIdentifier: zoneIdentifier] || hasPreloadedInter;
    }
    
    void _AppLovinShowInterstitialForZoneIdAndPlacement(const char *zoneId, const char *placement)
    {
        maybeInitializeDelegateWrapper();
        
        [ALInterstitialAd shared].adDisplayDelegate = delegateWrapper;
        [ALInterstitialAd shared].adVideoPlaybackDelegate = delegateWrapper;
        
        NSString * zoneIdentifier = _AppLovinGetRegularInterstitialZoneIdentifier( zoneId );
        ALAd *ad = [interstitialCache adForZoneIdentifier: zoneIdentifier];
        
        // If the ad is already loaded
        if ( ad )
        {
            // Just display it
            [[ALInterstitialAd shared] showOver: [UIApplication sharedApplication].keyWindow
                                      andRender: ad];
        }
        // Otherwise, we need to load the ad
        else
        {
            _AppLovinLoadShowIntersitital( zoneId, NO, YES );
        }
    }
    
    void _AppLovinShowInterstitial(const char *placement)
    {
        _AppLovinShowInterstitialForZoneIdAndPlacement( NULL, placement );
    }
    
    void _AppLovinShowInterstitialForZoneId(const char *zoneId)
    {
        _AppLovinShowInterstitialForZoneIdAndPlacement( zoneId, NULL );
    }
    
    bool _AppLovinIsInterstitialShowing()
    {
        return ([delegateWrapper isInterstitialShowing] ? true : false);
    }
    
    bool _AppLovinIsCurrentInterstitialVideo()
    {
        maybeInitializeDelegateWrapper();
        
        NSString * zoneIdentifier = _AppLovinGetRegularInterstitialZoneIdentifier( NULL );
        ALAd * cachedAd = [interstitialCache adForZoneIdentifier: zoneIdentifier];
        if ( cachedAd )
        {
            return cachedAd.videoAd ? true : false;
        }
        
        return false;
    }
    
#pragma mark - Incentivized ad loading / showing
    
    void _AppLovinSetIncentivizedUserName(const char *username)
    {
        NSString *usernameString = [NSString stringWithCString: username encoding: NSStringEncodingConversionAllowLossy];
        [ALIncentivizedInterstitialAd setUserIdentifier: usernameString];
    }
    
    bool _AppLovinIsIncentReady(const char *zoneId)
    {
        maybeInitializeDelegateWrapper();
        
        if ( zoneId != NULL )
        {
            NSString * zoneIdentifier = _AppLovinGetIncentInterstitialZoneIdentifier( zoneId );
            return [interstitialCache hasAdForZoneIdentifier: zoneIdentifier];
        }
        else
        {
            return [[ALIncentivizedInterstitialAd shared] isReadyForDisplay];
        }
    }
    
    void _AppLovinLoadIncentInterstitial(const char *zoneId)
    {
        maybeInitializeDelegateWrapper();
        
        _AppLovinLoadShowIntersitital( zoneId, YES, NO);
    }
    
    void _AppLovinShowIncentInterstitialForZoneIdAndPlacement(const char *zoneId, const char *placement)
    {
        maybeInitializeDelegateWrapper();
        
        [ALIncentivizedInterstitialAd shared].adDisplayDelegate = delegateWrapper;
        [ALIncentivizedInterstitialAd shared].adVideoPlaybackDelegate = delegateWrapper;
        
        // If we need to use zone-based API
        if ( zoneId != NULL )
        {
            NSString * zoneIdentifier = _AppLovinGetIncentInterstitialZoneIdentifier( zoneId );
            ALAd *ad = [interstitialCache adForZoneIdentifier: zoneIdentifier];
            if ( ad )
            {
                // Just display it
                [[ALIncentivizedInterstitialAd shared] showOver: [UIApplication sharedApplication].keyWindow
                                                       renderAd: ad
                                                      andNotify: delegateWrapper];
            }
        }
        else
        {
            [[ALIncentivizedInterstitialAd shared] showOver: [UIApplication sharedApplication].keyWindow
                                                  andNotify: delegateWrapper];
        }
    }
    
    void _AppLovinLoadShowIntersitital( const char *zoneId, BOOL isIncentivized, BOOL showWhenLoaded )
    {
        // Determine ad type and
        ALAdType * adType;
        NSString * zoneIdentifier;
        
        if ( isIncentivized )
        {
            adType = [ALAdType typeIncentivized];
            zoneIdentifier = _AppLovinGetIncentInterstitialZoneIdentifier( zoneId );
        }
        else
        {
            adType = [ALAdType typeRegular];
            zoneIdentifier = _AppLovinGetRegularInterstitialZoneIdentifier( zoneId );
        }
        
        ALManagedLoadDelegate *delegate = [ALManagedLoadDelegate sharedDelegateForZoneIdentifier: zoneIdentifier
                                                                                            size: [ALAdSize sizeInterstitial]
                                                                                            type: adType
                                                                                         wrapper: delegateWrapper];
        delegate.showInterstitialOnLoad = showWhenLoaded;
        delegate.cacheInterstitialOnLoad = YES;
        
        
        // If we are using zone-based load
        if ( zoneId != NULL )
        {
            // Use zone-based API
            [[ALSdk shared].adService loadNextAdForZoneIdentifier: zoneIdentifier andNotify: delegate];
        }
        // Otherwise (regular ad requested)
        else
        {
            // If we need an incentivized ad
            if ( isIncentivized )
            {
                // Use incentivized API
                [[ALIncentivizedInterstitialAd shared] preloadAndNotify: delegate];
            }
            // Otherwise (using regular intersitital)
            else
            {
                // Use regular API
                [[ALSdk shared].adService loadNextAd: [ALAdSize sizeInterstitial] andNotify: delegate];
            }
        }
    }
    
    void _AppLovinShowIncentInterstitial(const char *placement)
    {
        _AppLovinShowIncentInterstitialForZoneIdAndPlacement( NULL, placement );
    }
    
    void _AppLovinShowIncentInterstitialForZoneId(const char *zoneId)
    {
        _AppLovinShowIncentInterstitialForZoneIdAndPlacement( zoneId, NULL );
    }
    
#pragma mark - Analytics
    
    NSDictionary* deserializeParameters(const char * serializedParameters)
    {
        if (serializedParameters != NULL)
        {
            NSString* objcSerializedParameters = [NSString stringWithCString: serializedParameters encoding: NSUTF8StringEncoding];
            NSArray* keyValuePairs = [objcSerializedParameters componentsSeparatedByString: SERIALIZED_KEY_VALUE_PAIR_SEPARATOR];
            NSMutableDictionary* deserializedParameters = [NSMutableDictionary dictionary];
            
            for (NSString* keyValuePair in keyValuePairs)
            {
                NSArray* splitPair = [keyValuePair componentsSeparatedByString: SERIALIZED_KEY_VALUE_SEPARATOR];
                
                if ([splitPair count] > 1)
                {
                    NSString* key = [splitPair objectAtIndex: 0];
                    NSString* value = [splitPair objectAtIndex: 1];
                    
                    if (key && value)
                    {
                        [deserializedParameters setObject: value forKey: key];
                    }
                }
            }
            
            return deserializedParameters;
        }
        
        return [NSDictionary dictionary];
    }
    
    void _AppLovinTrackAnalyticEvent(const char * eventType, const char * serializedParameters)
    {
        // Some versions of Unity 5 have a bug where strings that come in via C# (e.g. the arguments here) get free()'d mid-method body.
        // Seems like a concurrency issue on their part. To avoid, we copy the strings and use the copies.

        const int eventTypeCopySize = 128;
        const int serializedParametersCopySize = 2048;

        char eventTypeCopy[eventTypeCopySize];
        char serializedParametersCopy[serializedParametersCopySize];

        size_t eventCopyLen = strlcpy(eventTypeCopy, eventType, sizeof(eventTypeCopy));
        size_t serializedParamsCopyLen = strlcpy(serializedParametersCopy, serializedParameters, sizeof(serializedParametersCopy));

        if (eventCopyLen > eventTypeCopySize)
        {
            NSLog(@"[AppLovinUnity] Event type has been truncated to %@", [NSString stringWithCString: eventTypeCopy encoding: NSUTF8StringEncoding]);
        }

        if (serializedParamsCopyLen > serializedParametersCopySize)
        {
            NSLog(@"[AppLovinUnity] Event parameters were too large and have been truncated, large key-value pairs may be dropped...");
        }

        NSDictionary* deserializedParameters = deserializeParameters(serializedParametersCopy);
        NSString* objcEventType = [NSString stringWithCString: eventTypeCopy encoding: NSUTF8StringEncoding];

        if ([objcEventType isEqualToString: kALEventTypeUserCompletedInAppPurchase])
        {
            NSString * transactionIdentifier = deserializedParameters[kALEventParameterStoreKitTransactionIdentifierKey];
            [[ALSdk shared].eventService trackInAppPurchaseWithTransactionIdentifier: transactionIdentifier parameters: deserializedParameters];
        }
        else
        {
            [[ALSdk shared].eventService trackEvent: objcEventType parameters: deserializedParameters];
        }
    }
    
    NSString * _AppLovinGetRegularInterstitialZoneIdentifier(const char *zoneId)
    {
        if ( zoneId != NULL )
        {
            return [NSString stringWithCString: zoneId encoding: NSStringEncodingConversionAllowLossy];
        }
        else
        {
            return kDefaultZoneIdForRegularInterstitial;
        }
    }
    
    NSString * _AppLovinGetIncentInterstitialZoneIdentifier(const char *zoneId)
    {        
        if ( zoneId != NULL )
        {
            return [NSString stringWithCString: zoneId encoding: NSStringEncodingConversionAllowLossy];
        }
        else
        {
            return kDefaultZoneIdForIncentInterstitial;
        }
    }
    
#pragma mark - User Privacy
    
    void _AppLovinSetHasUserConsent(const char *hasUserConsent)
    {
        NSString *hasUserConsentString = [NSString stringWithUTF8String: hasUserConsent];
        [ALPrivacySettings setHasUserConsent: [hasUserConsentString boolValue]];
    }
    
    bool _AppLovinHasUserConsent()
    {
        return [ALPrivacySettings hasUserConsent];
    }
    
    void _AppLovinSetIsAgeRestrictedUser(const char *isAgeRestrictedUser)
    {
        NSString *isAgeRestrictedUserString = [NSString stringWithUTF8String: isAgeRestrictedUser];
        [ALPrivacySettings setIsAgeRestrictedUser: [isAgeRestrictedUserString boolValue]];
    }
    
    bool _AppLovinIsAgeRestrictedUser()
    {
        return [ALPrivacySettings isAgeRestrictedUser];
    }
}

#pragma clang diagnostic pop
