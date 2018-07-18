//
//  WechatIOSProject.m
//  WechatIOSProject
//
//  Created by 吴鹏 on 2018/6/26.
//  Copyright © 2018年 BanMing. All rights reserved.
//

#import "WechatIOSProject.h"
#import "WXApi.h"
#import "WxDelegate.h"

#define CharToNSString(x) [NSString stringWithUTF8String:x]

@implementation WechatIOSProject :NSObject

+ (BOOL)sendAuthRequestScope:(NSString *)scope
                       State:(NSString *)state{
    SendAuthReq* req = [[SendAuthReq alloc] init];
    req.scope = scope; // @"post_timeline,sns"
    req.state = state;
//    req.openID = openID;
    
    return [WXApi sendAuthReq:req
               viewController:[WechatIOSProject getPresentViewController]
                     delegate:[WxDelegate sharedManager]];
}
+ (UIViewController *)getPresentViewController{
    UIViewController *appRootVC=[UIApplication sharedApplication].keyWindow.rootViewController;
    UIViewController *topVc=appRootVC;
    if (topVc.presentingViewController) {
        topVc=topVc.presentingViewController;
    }
    return topVc;
}
@end
#if defined (__cplusplus)
extern "C"{
#endif
    
    //测试用
    int GetAdd(int a,int b){
        return a+b;
    }
    
    //注册微信
    BOOL RegisterWeChatApp(char* appId){
       return  [WXApi registerApp:CharToNSString(appId)];
    }
    
    //微信登陆
    void LoginWeChat(){
//        SendAuthReq* req=[[SendAuthReq alloc]init];
//        req.scope=@"snsapi_userinfo";
//        req.state=@"123";
////        [WXApi sendAuthReq:req
////            viewController:viewController
////                  delegate:[WxDelegate sharedManager]];
//        [WXApi sendReq:req];
        [WechatIOSProject sendAuthRequestScope:@"snsapi_userinfo"State:@"123"];
    }
    
#if defined (__cplusplus)
}
#endif
