//
//  ALNativeAdService.h
//  sdk
//
//  Created by Thomas So on 5/21/15.
//
//

#import <Foundation/Foundation.h>
#import "ALAnnotations.h"
#import "ALNativeAdLoadDelegate.h"
#import "ALNativeAdPrecacheDelegate.h"

AL_ASSUME_NONNULL_BEGIN

@class ALSdk;
@class ALNativeAd;

@interface ALNativeAdService : NSObject

/**
 *  Load a batch of native ads, which are guaranteed not to repeat, asynchronously.
 *
 *  @param  numberOfAdsToLoad  The number of native ads to load.
 *  @param  delegate           The native ad load delegate to notify upon completion.
 */
- (void)loadNativeAdGroupOfCount:(NSUInteger)numberOfAdsToLoad andNotify:(alnullable id <ALNativeAdLoadDelegate>)delegate;

/**
 *  Load a batch of native ads, which are guaranteed not to repeat, asynchronously.
 *
 *  @param  numberOfAdsToLoad  The number of native ads to load.
 *  @param  zoneIdentifier     The identifier of the zone to load the native ads for.
 *  @param  delegate           The native ad load delegate to notify upon completion.
 */
- (void)loadNativeAdGroupOfCount:(NSUInteger)numberOfAdsToLoad forZoneIdentifier:(alnullable NSString *)zoneIdentifier andNotify:(alnullable id <ALNativeAdLoadDelegate>)delegate;

/**
 *  Pre-cache image and video resources of a native ad.
 *
 *  @param  ad        The native ad whose resources should be cached.
 *  @param  delegate  The delegate to be notified upon completion.
 */
- (void)precacheResourcesForNativeAd:(ALNativeAd *)ad andNotify:(alnullable id <ALNativeAdPrecacheDelegate>)delegate;

- (instancetype)init __attribute__((unavailable("Don't instantiate ALNativeAdService, access one via [sdk nativeAdService] instead.")));

@end

@interface ALNativeAdService(ALDeprecated)
- (void)preloadAdForZoneIdentifier:(NSString *)zoneIdentifier __deprecated_msg("Manually preloading ads in the background has been deprecated and will be removed in a future SDK version. Please use [ALNativeAdService loadNativeAdGroupOfCount:andNotify:] or [ALNativeAdService loadNativeAdGroupOfCount:forZoneIdentifier:andNotify:] to load ads.");
- (BOOL)hasPreloadedAdForZoneIdentifier:(NSString *)zoneIdentifier __deprecated_msg("Manually preloading ads in the background has been deprecated and will be removed in a future SDK version. Please use [ALNativeAdService loadNextAd:andNotify:] or [ALNativeAdService loadNativeAdGroupOfCount:forZoneIdentifier:andNotify:] to load ads.");
@end

AL_ASSUME_NONNULL_END
