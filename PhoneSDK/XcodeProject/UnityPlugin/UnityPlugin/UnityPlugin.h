//
//  UnityPlugin.h
//  UnityPlugin
//
//  Created by ios on 2018/5/21.
//  Copyright © 2018年 ios. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface UnityPlugin : NSObject

extern "C"
{
    double GetBatteryLevel();
    
    void CopyStringToPastboard(char *text);
    
    void NativeLog();
    
    BOOL OpenInstallApp(NSURL *schemeUrl);
}
@end
