//
//  WxDelegate.m
//  WechatIOSProject
//
//  Created by 吴鹏 on 2018/7/4.
//  Copyright © 2018年 BanMing. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "WxDelegate.h"

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
    //登陆
    if ([resp isKindOfClass:[SendAuthResp class]]) {
        SendAuthResp *authResp = (SendAuthResp *)resp;
        NSLog(@"%s", &"authResp.errCode:"[authResp.errCode]);
        if(authResp.errCode==0){
                UnitySendMessage("IosTest","GetAccessToken",[authResp.code UTF8String]);
        }
    }else if ([resp isKindOfClass:[SendMessageToWXResp class]]){
        SendMessageToWXResp *msgResp=(SendMessageToWXResp *)resp;
        NSLog(@"%s", &"msgResp.errCode:"[msgResp.errCode]);
        if (msgResp.errCode==0) {
            //unity分享成功
            UnitySendMessage("IosTest","IosCall","分享成功");
        }else{
            //分享失败
            UnitySendMessage("IosTest","IosCall","分享失败");
        }
    }else if ([resp isKindOfClass:[PayResp class]]){
        
        PayResp *payResp=(PayResp * ) resp;
        NSLog(@"%s", &"payResp.errCode:"[payResp.errCode]);
        if (payResp.errCode==0) {
            //支付成功
            UnitySendMessage("IosTest","IosCall","支付成功");
        }else{
            //支付失败
            UnitySendMessage("IosTest","IosCall","支付失败");
        }
    }
    
}

- (void)onReq:(BaseReq *)req {}

@end
