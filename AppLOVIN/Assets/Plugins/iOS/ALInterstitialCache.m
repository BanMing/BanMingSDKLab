//
//  ALInterstitialCache.m
//  Unity-iPhone
//
//  Created by Matt Szaro on 1/17/14.
//
//

#import "ALInterstitialCache.h"
#import "ALAd.h"
#import "ALAdLoadDelegate.h"


@interface ALInterstitialCache()

@property (strong, nonatomic) NSMutableDictionary<NSString *, ALAd *> *cache;

@end

@implementation ALInterstitialCache


static ALInterstitialCache *instance;



+(instancetype) shared
{
    if(!instance)
    {
        instance = [[ALInterstitialCache alloc] init];
    }
    return instance;
}

- (instancetype)init
{
    self = [super init];
    if (self) {
        self.cache = [NSMutableDictionary dictionary];
    }
    return self;
}


-(ALAd *)adForZoneIdentifier: (NSString *)zoneIdentifier
{
    return self.cache[zoneIdentifier];
}

-(void)removeAd: (ALAd *)ad
{
    __block NSString *keyToRemove;
    [self.cache enumerateKeysAndObjectsUsingBlock:^(NSString *key, ALAd *obj, BOOL *stop) {
        if ( [obj isEqual: ad] )
        {
            keyToRemove = key;
            *stop = YES;
        }
    }];
    
    if ( keyToRemove )
    {
        [self.cache removeObjectForKey: keyToRemove];
    }
}

-(BOOL)hasAdForZoneIdentifier: (NSString *)zoneIdentifier
{
    if ( zoneIdentifier )
    {
        return self.cache[zoneIdentifier] != nil;
    }
    
    return NO;
}

-(void)setAd: (ALAd *)ad forZoneIdentifier: (NSString *)zoneIdentifier
{
    if ( ad && zoneIdentifier )
    {
        self.cache[zoneIdentifier] = ad;
    }
}

@end

