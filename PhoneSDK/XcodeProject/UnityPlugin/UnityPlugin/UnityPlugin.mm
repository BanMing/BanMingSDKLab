//
//  UnityPlugin.m
//  UnityPlugin
//
//  Created by ios on 2018/5/21.
//  Copyright © 2018年 ios. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "UnityPlugin.h"


@implementation UnityPlugin
extern "C"
{
    void NativeLog(){
        NSLog(@"Log called from Unity");
    }
    
    double GetBatteryLevel(){
        [UIDevice currentDevice].batteryMonitoringEnabled=YES;
        double deviceLevel = [UIDevice currentDevice].batteryLevel;
        return deviceLevel;
    }
    
    void CopyStringToPastboard(char *text){
        NSString *textStr=[NSString stringWithCString:text encoding:NSUTF8StringEncoding];
        UIPasteboard *pasteboard =[UIPasteboard generalPasteboard];
        pasteboard.string=textStr;
    }
    BOOL OpenInstallApp(NSURL *schemeUrl){
        
        static BOOL reslut=NO;
        if([[[UIDevice currentDevice] systemVersion] floatValue] >= 10)
        {
            [[UIApplication sharedApplication] openURL:schemeUrl options:[[NSDictionary alloc] init] completionHandler:^(BOOL success){
                reslut=success;
            }];
        }
        else
        {
            @try{
                [[UIApplication sharedApplication] openURL:schemeUrl];
                reslut= YES;
            }
            @catch(NSException *e){
                reslut= NO;
            }
            
        }
  
        return reslut;
    }
}
@end
