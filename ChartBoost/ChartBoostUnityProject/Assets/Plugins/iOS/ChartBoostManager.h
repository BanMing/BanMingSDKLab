/*
 * ChartboostManager.h
 * Chartboost
 *
 * Copyright 2011 Chartboost. All rights reserved.
 */

#import <Foundation/Foundation.h>

#if !defined(CB_UNITY_SDK_VERSION_STRING)
  #define CB_UNITY_SDK_VERSION_STRING @"7.2.0"
#endif


@interface ChartBoostManager : NSObject

@property (nonatomic) BOOL shouldPauseClick;
@property (nonatomic) BOOL shouldRequestFirstSession;

// Properties used by delegates
@property (nonatomic) BOOL hasCheckedWithUnityToDisplayInterstitial;
@property (nonatomic) BOOL hasCheckedWithUnityToDisplayRewardedVideo;
@property (nonatomic) BOOL unityResponseShouldDisplayInterstitial;
@property (nonatomic) BOOL unityResponseShouldDisplayRewardedVideo;

@property (nonatomic, retain) NSString *gameObjectName;

+ (ChartBoostManager*)sharedManager;

- (void)startChartBoostWithAppId:(NSString*)appId appSignature:(NSString*)appSignature unityVersion:(NSString *)unityVersion;

@end
