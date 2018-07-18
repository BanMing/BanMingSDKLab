//
//  WxDelegate.m
//  WechatIOSProject
//
//  Created by 吴鹏 on 2018/7/4.
//  Copyright © 2018年 BanMing. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "WxDelegate.h"
//#import "UnityInterface.h"

@implementation WxDelegate

#pragma mark - LifeCycle
+(instancetype)sharedManager {
    static dispatch_once_t onceToken;
    static WxDelegate *instance;
    dispatch_once(&onceToken, ^{
        instance = [[WxDelegate alloc] init];
    });
    return instance;
}

#pragma mark - WXApiDelegate
- (void)onResp:(BaseResp *)resp {
    if ([resp isKindOfClass:[SendAuthResp class]]) {
        if (_delegate
            && [_delegate respondsToSelector:@selector(managerDidRecvAuthResponse:)]) {
            SendAuthResp *authResp = (SendAuthResp *)resp;
//            [_delegate managerDidRecvAuthResponse:authResp];
//            UnitySendMessage("IosTest","GetAccessToken",authResp.code);
            NSLog(authResp.code);
        }}}

- (void)onReq:(BaseReq *)req {}

@end
