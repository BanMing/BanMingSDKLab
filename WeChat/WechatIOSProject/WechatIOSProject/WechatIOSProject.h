//
//  WechatIOSProject.h
//  WechatIOSProject
//
//  Created by 吴鹏 on 2018/6/26.
//  Copyright © 2018年 BanMing. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface WechatIOSProject :NSObject

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
    
#if defined (__cplusplus)
}
#endif
