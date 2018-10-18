//
//  ALAdDelegateWrapper.m
//  Unity-iPhone
//
//  Created by Matt Szaro on 1/16/14.
//
//

#import "ALAdDelegateWrapper.h"
#import "ALInterstitialCache.h"
#import "ALErrorCodes.h"
#import "ALManagedLoadDelegate.h"

// Forward declaration - do not modify
extern void UnitySendMessage(const char *, const char *, const char *);

@implementation ALAdDelegateWrapper
@synthesize isInterstitialShowing, gameObjectToNotify;

const static NSString* methodName = @"onAppLovinEventReceived";

-(void) callCSharpWithMessage: (NSString*) message
{
    if(gameObjectToNotify)
    {
        UnitySendMessage([gameObjectToNotify cStringUsingEncoding: NSStringEncodingConversionAllowLossy],
                         [methodName cStringUsingEncoding: NSStringEncodingConversionAllowLossy],
                         [message cStringUsingEncoding: NSStringEncodingConversionAllowLossy]
                         );
    }
}

-(void) adService:(ALAdService *)adService didLoadAd:(ALAd *)ad
{
    if([ad.type isEqual: [ALAdType typeIncentivized]])
    {
        [self callCSharpWithMessage: @"LOADEDREWARDED"];
    }
    else
    {
        // Fix for Unity 3 releasing object incorrectly
        if(ad.size.label)
        {
            [self callCSharpWithMessage: [@"LOADED" stringByAppendingString: [[[ad size] label] uppercaseString]]];
        }
    }
}

-(void) adService:(ALAdService *)adService didFailToLoadAdWithError:(int)code
{
    [self callCSharpWithMessage: @"LOADFAILED"];
    NSLog(@"[ALAdDelegateWrapper] Non-typed load callback was used from: %@", [NSThread callStackSymbols]);
}

-(void) adService: (ALAdService*) adService didFailToLoadAdOfSize:(ALAdSize *)size type:(ALAdType *)type withError:(NSInteger)error
{
    // Call old non-typed fail callback for backwards compatibility
    [self callCSharpWithMessage: @"LOADFAILED"];
    
    // For newer integrations, call with a typed failure
    NSString* typeStr = [type isEqual: [ALAdType typeIncentivized]] ? @"REWARDED" : [size.label uppercaseString];
    [self callCSharpWithMessage: [NSString stringWithFormat: @"LOAD%@FAILED", typeStr]];
}

-(void) ad:(ALAd *)ad wasDisplayedIn:(UIView *)view
{
    if([[ad size] isEqual: [ALAdSize sizeInterstitial]])
        isInterstitialShowing = YES;
    
    // Fix for Unity 3 releasing object incorrectly
    if(ad.size.label && ad.type)
    {
        NSString* typeStr = [ad.type isEqual: [ALAdType typeIncentivized]] ? @"REWARDED" : [ad.size.label uppercaseString];
        [self callCSharpWithMessage: [@"DISPLAYED" stringByAppendingString: typeStr]];
    }
}

-(void) ad:(ALAd *)ad wasHiddenIn:(UIView *)view
{
    if([[ad size] isEqual: [ALAdSize sizeInterstitial]])
    {
        isInterstitialShowing = NO;
        
        [[ALInterstitialCache shared] removeAd: ad];        
    }        
    
    if(ad.size.label && ad.type)
    {
        NSString* typeStr = [ad.type isEqual: [ALAdType typeIncentivized]] ? @"REWARDED" : [ad.size.label uppercaseString];
        [self callCSharpWithMessage: [@"HIDDEN" stringByAppendingString: typeStr]];
    }
}

-(void) ad:(ALAd *)ad wasClickedIn:(UIView *)view
{
    [self callCSharpWithMessage: @"CLICKED"];
}

-(void) videoPlaybackBeganInAd:(ALAd *)ad
{
    [self callCSharpWithMessage: @"VIDEOBEGAN"];
}

-(void) videoPlaybackEndedInAd:(ALAd *)ad atPlaybackPercent:(NSNumber *)percentPlayed fullyWatched:(BOOL)wasFullyWatched
{
    [self callCSharpWithMessage: @"VIDEOSTOPPED"];
}

-(void) rewardValidationRequestForAd:(ALAd *)ad didSucceedWithResponse:(NSDictionary *)response
{
    [self callCSharpWithMessage: @"REWARDAPPROVED"];
    
    NSString* amountStr = [response objectForKey: @"amount"];
    NSString* currencyName = [response objectForKey: @"currency"];
    
    if(amountStr && currencyName)
    {
        [self callCSharpWithMessage: [NSString stringWithFormat: @"REWARDAPPROVEDINFO|%@|%@", amountStr, currencyName]];
    }
}

-(void) rewardValidationRequestForAd:(ALAd *)ad didExceedQuotaWithResponse:(NSDictionary *)response
{
    [self callCSharpWithMessage: @"REWARDQUOTAEXCEEDED"];
}

-(void) rewardValidationRequestForAd:(ALAd *)ad wasRejectedWithResponse:(NSDictionary *)response
{
    [self callCSharpWithMessage: @"REWARDREJECTED"];
}

-(void) rewardValidationRequestForAd:(ALAd *)ad didFailWithError:(NSInteger)responseCode
{
    if(responseCode == kALErrorCodeIncentivizedUserClosedVideo)
    {
        [self callCSharpWithMessage: @"USERCLOSEDEARLY"];
    }
    else
    {
        [self callCSharpWithMessage: @"REWARDTIMEOUT"];
    }
}

-(void) userDeclinedToViewAd: (ALAd*) ad
{
    [self callCSharpWithMessage: @"USERDECLINED"];
}

- (void)ad:(ALAd *)ad didPresentFullscreenForAdView:(ALAdView *)adView
{
    [self callCSharpWithMessage: @"OPENEDFULLSCREEN"];
}

- (void)ad:(ALAd *)ad willDismissFullscreenForAdView:(ALAdView *)adView {}

- (void)ad:(ALAd *)ad didDismissFullscreenForAdView:(ALAdView *)adView
{
    [self callCSharpWithMessage: @"CLOSEDFULLSCREEN"];
}

- (void)ad:(ALAd *)ad willLeaveApplicationForAdView:(ALAdView *)adView
{
    [self callCSharpWithMessage: @"LEFTAPPLICATION"];
}

- (void)ad:(ALAd *)ad didFailToDisplayInAdView:(ALAdView *)adView withError:(ALAdViewDisplayErrorCode)code
{
    [self callCSharpWithMessage: @"DISPLAYFAILED"];
}

@end
