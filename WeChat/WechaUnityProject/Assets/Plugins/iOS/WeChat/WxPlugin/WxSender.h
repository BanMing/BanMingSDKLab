//
//  WxSender.h
//  WxSender
//
//  Created by 吴鹏 on 2018/6/26.
//  Copyright © 2018年 BanMing. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface WxSender :NSObject

@end

#if defined (__cplusplus)
extern "C"{
#endif
    
    //测试用
    int GetAdd(int a,int b);
    
    //注册微信
    BOOL RegisterWeChatApp(char* appId);
    
    //微信登陆
    void LoginWeChat(void);
    
    //微信分享图文
    void ShareContent(bool isFriend,char* title,char* url,char* contentStr,char* iconPath);
    //微信分享截图
    void SharePic(bool isFriend,char* imgPath);
    //微信支付
    void WxPay(char * partnerId,char *prepayId,char* package,char* nonceStr,char* timeStamp,char* sign);
    
#if defined (__cplusplus)
}
#endif
