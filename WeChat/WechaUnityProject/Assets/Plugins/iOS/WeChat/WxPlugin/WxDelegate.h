//
//  WxDelegate.h
//  WechatIOSProject
//
//  Created by 吴鹏 on 2018/7/2.
//  Copyright © 2018年 BanMing. All rights reserved.
//


#import <Foundation/Foundation.h>
#import "WXApi.h"


@interface WxDelegate : UIResponder<UIApplicationDelegate,WXApiDelegate>

+ (instancetype)sharedManager;
@end
