//
//  ALTypeRememberingLoadDelegate.h
//  AppLovin Air Extension
//
//  Created by Matt Szaro, Thomas So on 5/20/14.
//  Copyright (c) 2014 AppLovin. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ALAdLoadDelegate.h"
#import "ALAdView.h"

@protocol ALUnityTypedLoadFailureDelegate <NSObject>

- (void)adService:(ALAdService *)adService didFailToLoadAdOfSize:(ALAdSize *)size type:(ALAdType *)type withError:(NSInteger)error;

@end

@interface ALManagedLoadDelegate : NSObject<ALAdLoadDelegate>

@property (copy, readonly, atomic) NSString *      zoneIdentifier;
@property (strong, readonly, nonatomic) ALAdSize * size;
@property (strong, readonly, nonatomic) ALAdType * type;

@property (assign, atomic) BOOL showInterstitialOnLoad;
@property (assign, atomic) BOOL showBannerOnLoad;
@property (assign, atomic) BOOL cacheInterstitialOnLoad;

@property (nonatomic, weak) ALAdView *adView;

+ (instancetype)sharedDelegateForZoneIdentifier:(NSString *)zoneIdentifier
                                           size:(ALAdSize *)size
                                           type:(ALAdType *)type
                                        wrapper:(id<ALAdLoadDelegate, ALUnityTypedLoadFailureDelegate>)wrapper;

- (instancetype)init __attribute__((unavailable("Use +sharedDelegate.. instead of alloc-init pattern.")));

@end
