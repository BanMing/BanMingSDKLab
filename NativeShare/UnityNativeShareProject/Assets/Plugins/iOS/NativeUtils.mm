//
//  NativeUtils.mm
//


#import <Foundation/Foundation.h>
#import <MessageUI/MessageUI.h>

//Hide error of undef function which is available in unity generated xCode project
//#define FMDEBUG
//#ifdef FMDEBUG
//inline UIViewController* UnityGetGLViewController()
//{
//    return nil;
//}
//#endif

@interface NativeUtils : NSObject<MFMailComposeViewControllerDelegate>

+ (id)sharedInstance;

-(void) shareText: (NSString*) body withURL: (NSString*) urlString withImage:(NSString*) imageDataString withSubject: (NSString*) subject;
-(void) ShareWeb: (NSString*) body withURL: (NSString*) urlString withImage:(NSString*) imageDataString withSubject: (NSString*) subject;
-(BOOL) isStringValideBase64:(NSString*)string;
@end


@implementation NativeUtils

static NativeUtils * _sharedInstance;

+ (id)sharedInstance {
    
    if (_sharedInstance == nil)  {
        _sharedInstance = [[self alloc] init];
    }
    
    return _sharedInstance;
}


+(NSString*) charToNSString: (char*)text {
    return text ? [[NSString alloc] initWithUTF8String:text] : [[NSString alloc] initWithUTF8String:""];
}


//分享图片 分享文字
-(void) SocialSharing: (NSString*) body withURL: (NSString*) urlString withImage:(NSString*) imageDataString withSubject: (NSString*) subject {
    
    NSMutableArray *sharingItems = [NSMutableArray new];
    if (body && body.length > 0) {
        [sharingItems addObject:body];
    }
    if (imageDataString && imageDataString.length > 0) {
        UIImage *image = NULL;
        if([self isStringValideBase64:imageDataString]){
            NSData *imageData = [[NSData alloc] initWithBase64EncodedString:imageDataString options:0];
            image = [[UIImage alloc] initWithData:imageData];
        }else{
            NSData *dataImage = [NSData dataWithContentsOfFile:imageDataString];
            image = [[UIImage alloc] initWithData:dataImage];
        }
        [sharingItems addObject:image];
    }
    if (urlString && urlString.length > 0) {
        [sharingItems addObject:urlString];
    }
    
    UIActivityViewController *activityViewController = [[UIActivityViewController alloc]                                                                initWithActivityItems:sharingItems applicationActivities:nil];
    activityViewController.popoverPresentationController.sourceView = UnityGetGLViewController().view;
    activityViewController.popoverPresentationController.sourceRect = CGRectMake(UnityGetGLViewController().view.frame.size.width/2, UnityGetGLViewController().view.frame.size.height/4, 0, 0);
    
    if(subject && subject.length > 0)
    {
        [activityViewController setValue:subject forKey:@"subject"];
    }
    
    [UnityGetGLViewController() presentViewController:activityViewController animated:YES completion:nil];
    
    
}
//所有参数都必须有分享网页链接
-(void) ShareWeb: (NSString*) body withURL: (NSString*) urlString withImage:(NSString*) imageDataString withSubject: (NSString*) subject  {

    if(body==NULL||body.length<0||urlString==NULL||urlString.length<0||imageDataString==NULL||imageDataString.length<0||subject==NULL||subject.length<0){
        return;
    }
    NSURL *urlToShare=[NSURL URLWithString:urlString];
    UIImage *image = NULL;
    if([self isStringValideBase64:imageDataString]){
        NSData *imageData = [[NSData alloc] initWithBase64EncodedString:imageDataString options:0];
        image = [[UIImage alloc] initWithData:imageData];
    }
    else{
        NSData *dataImage = [NSData dataWithContentsOfFile:imageDataString];
        image = [[UIImage alloc] initWithData:dataImage];
    }
    NSArray *shareItems=@[body,urlToShare,image];

    UIActivityViewController *activityViewController = [[UIActivityViewController alloc]                                                                initWithActivityItems:shareItems applicationActivities:nil];
    activityViewController.popoverPresentationController.sourceView = UnityGetGLViewController().view;
    activityViewController.popoverPresentationController.sourceRect = CGRectMake(UnityGetGLViewController().view.frame.size.width/2, UnityGetGLViewController().view.frame.size.height/4, 0, 0);
    
    if(subject && subject.length > 0)
    {
        [activityViewController setValue:subject forKey:@"subject"];
    }
    
    [UnityGetGLViewController() presentViewController:activityViewController animated:YES completion:nil];
}

-(BOOL) isStringValideBase64:(NSString*)string{
    
    NSString *regExPattern = @"^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$";
    
    NSRegularExpression *regEx = [[NSRegularExpression alloc] initWithPattern:regExPattern options:NSRegularExpressionCaseInsensitive error:nil];
    NSUInteger regExMatches = [regEx numberOfMatchesInString:string options:0 range:NSMakeRange(0, [string length])];
    return regExMatches != 0;
}

@end

extern "C"
{
    void SocialSharing(char* body, char* url, char* imageDataString, char* subject) {
        
        [[NativeUtils sharedInstance] SocialSharing:[NativeUtils charToNSString:body] withURL:[NativeUtils charToNSString:url] withImage:[NativeUtils charToNSString:imageDataString] withSubject:[NativeUtils charToNSString:subject]];
    }
     void ShareWeb(char* body, char* url, char* imageDataString, char* subject) {
        
        [[NativeUtils sharedInstance] ShareWeb:[NativeUtils charToNSString:body] withURL:[NativeUtils charToNSString:url] withImage:[NativeUtils charToNSString:imageDataString] withSubject:[NativeUtils charToNSString:subject]];
    }
}


