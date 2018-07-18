//
//  WxDelegate.h
//  WechatIOSProject
//
//  Created by 吴鹏 on 2018/7/2.
//  Copyright © 2018年 BanMing. All rights reserved.
//


#import <Foundation/Foundation.h>
#import "WXApi.h"

@protocol WXApiManagerDelegate <NSObject>

@optional

- (void)managerDidRecvAuthResponse:(SendAuthResp *)response;


@end

@interface WxDelegate : NSObject<WXApiDelegate>

@property (nonatomic, assign) id<WXApiManagerDelegate> delegate;
+ (instancetype)sharedManager;
@end
