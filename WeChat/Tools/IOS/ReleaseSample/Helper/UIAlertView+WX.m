//
//  UIAlertView+WX.m
//  SDKSample
//
//  Created by liuyunxuan on 2017/2/10.
//
//

#import "UIAlertView+WX.h"
#import <objc/runtime.h>


static const void *WXAlertSureKey = &WXAlertSureKey;
static const NSInteger kSureTag = 1010;
static const NSInteger kRequestTag = 1020;

@implementation UIAlertView (WX)

/// withOut delegate
+ (void)requestWithTitle:(NSString *)title
                 message:(NSString *)message
             defaultText:(NSString *)defaultText
                    sure:(WXAlertSureBlock)sure;
{
    UIAlertView *view = [[UIAlertView alloc] initWithTitle:title
                                                   message:message
                                                  delegate:nil
                                         cancelButtonTitle:@"取消"
                                         otherButtonTitles:@"确认", nil];
    view.delegate = view;
    view.alertViewStyle = UIAlertViewStylePlainTextInput;
    [view setSureBlock:sure];
    [view textFieldAtIndex:0].text = defaultText;
    view.tag = kRequestTag;
    [view show];
}

+ (void)showWithTitle:(NSString *)title
              message:(NSString *)message
                 sure:(WXAlertSureBlock)sure
{
    UIAlertView *view = [[UIAlertView alloc] initWithTitle:title
                                                   message:message
                                                  delegate:nil
                                         cancelButtonTitle:@"取消"
                                         otherButtonTitles:@"确认", nil];
    view.delegate = view;
    view.tag = kSureTag;
    [view setSureBlock:sure];
    [view show];
}

#pragma mark - delegate
- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    if (buttonIndex == alertView.cancelButtonIndex)
    {
        return;
    }
    WXAlertSureBlock sureBlock = [self sureBlock];
    if (alertView.tag == kRequestTag)
    {
        if (sureBlock)
        {
            sureBlock(alertView,[alertView textFieldAtIndex:0].text);
        }
    }
    else if (alertView.tag == kSureTag)
    {
        if (sureBlock)
        {
            sureBlock(alertView,nil);
        }
    }
}

#pragma mark - private method
-(void)setSureBlock:(WXAlertSureBlock)block
{
    objc_setAssociatedObject(self, WXAlertSureKey, block, OBJC_ASSOCIATION_RETAIN_NONATOMIC);
}

-(WXAlertSureBlock)sureBlock
{
    return objc_getAssociatedObject(self, WXAlertSureKey);
}
@end
