//
//  ALAdDelegateWrapper.h
//  Unity-iPhone
//
//  Created by Matt Szaro on 1/16/14.
//
//

#import <Foundation/Foundation.h>
#import "ALAdLoadDelegate.h"
#import "ALAdDisplayDelegate.h"
#import "ALAdVideoPlaybackDelegate.h"
#import "ALAdRewardDelegate.h"
#import "ALManagedLoadDelegate.h"
#import "ALAdViewEventDelegate.h"

@interface ALAdDelegateWrapper : NSObject <ALAdLoadDelegate, ALAdDisplayDelegate, ALAdRewardDelegate, ALAdVideoPlaybackDelegate, ALUnityTypedLoadFailureDelegate, ALAdViewEventDelegate>

@property (assign, atomic) BOOL isInterstitialShowing;

@property (strong, nonatomic) NSString* gameObjectToNotify;

@end
