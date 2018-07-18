//
//  WxSender.m
//  WxSender
//
//  Created by 吴鹏 on 2018/6/26.
//  Copyright © 2018年 BanMing. All rights reserved.
//

#import "WxSender.h"
#import "WXApi.h"
#import "WxDelegate.h"

#define CharToNSString(x) [NSString stringWithUTF8String:x]

@implementation WxSender :NSObject

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
        SendAuthReq* req=[[SendAuthReq alloc]init];
        req.scope=@"snsapi_message,snsapi_userinfo,snsapi_friend,snsapi_contact";
        req.state=@"xxx";
        [WXApi sendReq:req];
    }
    //微信分享图文
    void ShareContent(bool isFriend,char* title,char* url,char* contentStr,char* iconPath){
        WXMediaMessage *message=[WXMediaMessage message];
        [message setThumbImage:[UIImage imageNamed:CharToNSString(iconPath)]];
        message.title=CharToNSString(title);
        message.description=CharToNSString(contentStr);
        
        WXWebpageObject *webpageObject=[WXWebpageObject object];
        webpageObject.webpageUrl=CharToNSString(url);
        message.mediaObject=webpageObject;
        
        SendMessageToWXReq *req=[[SendMessageToWXReq alloc] init];
        req.bText=NO;
        req.message=message;
        req.scene=isFriend?WXSceneSession:WXSceneTimeline;
        [WXApi sendReq:req];
    }
    //微信分享截图
    void SharePic(bool isFriend,char* imgPath){
        WXMediaMessage *message=[WXMediaMessage message];
        [message setThumbImage:[UIImage imageNamed:CharToNSString(imgPath)]];

        WXImageObject *imageObject=[WXImageObject object];
        imageObject.imageData=[NSData dataWithContentsOfFile:CharToNSString(imgPath)];
        message.mediaObject=imageObject;
        
        SendMessageToWXReq *req=[[SendMessageToWXReq alloc] init];
        req.bText=NO;
        req.message=message;
        req.scene=isFriend?WXSceneSession:WXSceneTimeline;
        [WXApi sendReq:req];
    }
    //微信支付
    void WxPay(char * partnerId,char *prepayId,char* package,char* nonceStr,char* timeStamp,char* sign){
        PayReq *req=[[PayReq alloc] init];
        
        req.partnerId=CharToNSString(partnerId);
        req.prepayId=CharToNSString(prepayId);
        req.package=CharToNSString(package);
        req.nonceStr=CharToNSString(nonceStr);
        req.timeStamp=CharToNSString(timeStamp);
        req.sign=CharToNSString(sign);
    }
#if defined (__cplusplus)
}
#endif
