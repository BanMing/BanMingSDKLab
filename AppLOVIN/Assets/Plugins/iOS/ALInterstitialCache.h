//
//  ALInterstitialCache.h
//  Unity-iPhone
//
//  Created by Matt Szaro on 1/17/14.
//
//

#import <Foundation/Foundation.h>
#import "ALAdLoadDelegate.h"
#import "ALInterstitialCache.h"
#import "ALAdDelegateWrapper.h"
#import "ALManagedLoadDelegate.h"

@interface ALInterstitialCache : NSObject


-(ALAd *)adForZoneIdentifier: (NSString *)zoneIdentifier;

-(void)removeAd: (ALAd *)ad;

-(BOOL)hasAdForZoneIdentifier: (NSString *)zoneIdentifier;

-(void)setAd: (ALAd *)ad forZoneIdentifier: (NSString *)zoneIdentifier;

+ (instancetype)shared;

@end

