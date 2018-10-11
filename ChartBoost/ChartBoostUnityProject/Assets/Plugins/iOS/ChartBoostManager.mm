/*
 * ChartboostManager.mm
 * Chartboost
 *
 * Copyright 2011 Chartboost. All rights reserved.
 */

#import "ChartBoostManager.h"
#import "Chartboost.h"
#import "CBInPlay.h"

void UnitySendMessage(const char *className, const char *methodName, const char *param);

@interface ChartBoostManager() <ChartboostDelegate>

@end

@implementation ChartBoostManager
@synthesize shouldPauseClick, shouldRequestFirstSession;

@synthesize hasCheckedWithUnityToDisplayInterstitial, unityResponseShouldDisplayInterstitial;
@synthesize hasCheckedWithUnityToDisplayRewardedVideo, unityResponseShouldDisplayRewardedVideo;
@synthesize gameObjectName;

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

+ (ChartBoostManager*)sharedManager
{
	static ChartBoostManager *sharedSingleton;

	if (!sharedSingleton)
		sharedSingleton = [[ChartBoostManager alloc] init];

	return sharedSingleton;
}

- (id)init {
    self = [super init];

    if (self) {
        self.shouldPauseClick = NO;
        self.shouldRequestFirstSession = YES;

        self.hasCheckedWithUnityToDisplayInterstitial = NO;
        self.hasCheckedWithUnityToDisplayRewardedVideo = NO;
        self.unityResponseShouldDisplayInterstitial = YES;
        self.unityResponseShouldDisplayRewardedVideo = YES;
    }

    return self;
}

- (void) dealloc {
    [super dealloc];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)startChartBoostWithAppId:(NSString*)appId appSignature:(NSString*)appSignature unityVersion:(NSString *)unityVersion
{
    [Chartboost setFramework: (CBFramework)CBFrameworkUnity withVersion:unityVersion];
    [Chartboost setChartboostWrapperVersion:CB_UNITY_SDK_VERSION_STRING];
    [Chartboost startWithAppId: appId appSignature: appSignature delegate: self];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark ChartboostInterstitialDelegate

- (void)didInitialize:(BOOL)status
{
    UnitySendMessage([gameObjectName UTF8String], "didInitializeEvent", status ? "true" : "false");
}

- (BOOL)shouldDisplayInterstitial:(CBLocation)location
{
    if (self.hasCheckedWithUnityToDisplayInterstitial)
    {
        self.hasCheckedWithUnityToDisplayInterstitial = false;
        return self.unityResponseShouldDisplayInterstitial;
    }
    else
    {
        self.hasCheckedWithUnityToDisplayInterstitial = true;
        UnitySendMessage([gameObjectName UTF8String], "shouldDisplayInterstitialEvent", location.UTF8String);
    }
    return NO;
}

- (void)didDisplayInterstitial:(CBLocation)location
{
    UnitySendMessage([gameObjectName UTF8String], "didDisplayInterstitialEvent", location.UTF8String);
}

- (void)didFailToLoadInterstitial:(CBLocation)location withError:(CBLoadError)error
{
	NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
						  location ? location : [NSNull null], @"location",
						  [NSNumber numberWithInt: error], @"errorCode",
						  nil];
	NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
	NSString *json = [[[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding] autorelease];
	UnitySendMessage([gameObjectName UTF8String], "didFailToLoadInterstitialEvent", json.UTF8String);
}

- (void)didCacheInterstitial:(CBLocation)location
{
	UnitySendMessage([gameObjectName UTF8String], "didCacheInterstitialEvent", location.UTF8String);
}

- (void)didDismissInterstitial:(CBLocation)location
{
    UnitySendMessage([gameObjectName UTF8String], "didDismissInterstitialEvent", location.UTF8String);
}

- (void)didCloseInterstitial:(CBLocation)location
{
    UnitySendMessage([gameObjectName UTF8String], "didCloseInterstitialEvent", location.UTF8String);
}

- (void)didClickInterstitial:(CBLocation)location
{
    UnitySendMessage([gameObjectName UTF8String], "didClickInterstitialEvent", location.UTF8String);
}

- (void)didFailToRecordClick:(CBLocation)location withError:(CBClickError)error
{
	NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
						  location ? location : [NSNull null], @"location",
						  [NSNumber numberWithInt: error], @"errorCode",
						  nil];
	NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
	NSString *json = [[[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding] autorelease];
    UnitySendMessage([gameObjectName UTF8String], "didFailToRecordClickEvent", json.UTF8String);
}

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark CBRewardedDelegate

- (BOOL)shouldDisplayRewardedVideo:(CBLocation)location
{
	if (self.hasCheckedWithUnityToDisplayRewardedVideo)
    {
        self.hasCheckedWithUnityToDisplayRewardedVideo = false;
        return self.unityResponseShouldDisplayRewardedVideo;
    }
    else
    {
        self.hasCheckedWithUnityToDisplayRewardedVideo = true;
        UnitySendMessage([gameObjectName UTF8String], "shouldDisplayRewardedVideoEvent", location.UTF8String);
    }
    return NO;
}

- (void)didDisplayRewardedVideo:(CBLocation)location
{
    UnitySendMessage([gameObjectName UTF8String], "didDisplayRewardedVideoEvent", location.UTF8String);
}

- (void)didFailToLoadRewardedVideo:(CBLocation)location withError:(CBLoadError)error
{
	NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
						  location ? location : [NSNull null], @"location",
						  [NSNumber numberWithInt: error], @"errorCode",
						  nil];
	NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
	NSString *json = [[[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding] autorelease];
	UnitySendMessage([gameObjectName UTF8String], "didFailToLoadRewardedVideoEvent", json.UTF8String);
}

- (void)didCacheRewardedVideo:(CBLocation)location
{
	UnitySendMessage([gameObjectName UTF8String], "didCacheRewardedVideoEvent", location.UTF8String);
}

- (void)didDismissRewardedVideo:(CBLocation)location
{
    UnitySendMessage([gameObjectName UTF8String], "didDismissRewardedVideoEvent", location.UTF8String);
}

- (void)didCloseRewardedVideo:(CBLocation)location
{
    UnitySendMessage([gameObjectName UTF8String], "didCloseRewardedVideoEvent", location.UTF8String);
}

- (void)didClickRewardedVideo:(CBLocation)location
{
    UnitySendMessage([gameObjectName UTF8String], "didClickRewardedVideoEvent", location.UTF8String);
}

- (void)didCompleteRewardedVideo:(CBLocation)location withReward:(int)reward
{
	NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
						  location ? location : [NSNull null], @"location",
						  [NSNumber numberWithInt: reward], @"reward",
						  nil];
	NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
	NSString *json = [[[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding] autorelease];
	UnitySendMessage([gameObjectName UTF8String], "didCompleteRewardedVideoEvent", json.UTF8String);
}

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark CBInPlayDelegate

- (void)didCacheInPlay:(CBLocation)location
{
    UnitySendMessage([gameObjectName UTF8String], "didCacheInPlayEvent", location.UTF8String);
}

- (void)didFailToLoadInPlay:(CBLocation)location withError:(CBLoadError)error
{
    NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
						  location ? location : [NSNull null], @"location",
						  [NSNumber numberWithInt: error], @"errorCode",
						  nil];
	NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
	NSString *json = [[[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding] autorelease];
    UnitySendMessage([gameObjectName UTF8String], "didFailToLoadInPlayEvent", json.UTF8String);
}

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark general delegates

- (void)didCompleteAppStoreSheetFlow
{
	UnitySendMessage([gameObjectName UTF8String], "didCompleteAppStoreSheetFlowEvent", "");
}

- (void)didPauseClickForConfirmation
{
    UnitySendMessage([gameObjectName UTF8String], "didPauseClickForConfirmationEvent", "");
}

- (void)willDisplayVideo:(CBLocation)location
{
    UnitySendMessage([gameObjectName UTF8String], "willDisplayVideoEvent", location.UTF8String);
}

@end
