//
//  UIAlertView+WX.h
//  SDKSample
//
//  Created by liuyunxuan on 2017/2/10.
//
//

#import <UIKit/UIKit.h>

typedef void(^WXAlertSureBlock)(UIAlertView *alertView,NSString *text);

@interface UIAlertView (WX)<UIAlertViewDelegate>

///请求输入内容的alert
+ (void)requestWithTitle:(NSString *)title
                 message:(NSString *)message
             defaultText:(NSString *)defaultText
                    sure:(WXAlertSureBlock)sure;

// 只返回确定的结果
+ (void)showWithTitle:(NSString *)title
              message:(NSString *)message
                 sure:(WXAlertSureBlock)sure;
@end
