//
//  SendMsgToWeChatViewController.m
//  ApiClient
//
//  Created by Tencent on 12-2-27.
//  Copyright (c) 2012年 Tencent. All rights reserved.
//

#import "SendMsgToWeChatViewController.h"
#import "WXApiRequestHandler.h"
#import "WXApiManager.h"
#import "RespForWeChatViewController.h"
#import "Constant.h"
#import "WechatAuthSDK.h"
#import "UIAlertView+WX.h"

@interface SendMsgToWeChatViewController ()<WXApiManagerDelegate,UITextViewDelegate, WechatAuthAPIDelegate>

@property (nonatomic) enum WXScene currentScene;
@property (nonatomic, strong) UIScrollView *footView;

@end

@implementation SendMsgToWeChatViewController

@synthesize currentScene = _currentScene;
//@synthesize appId = _appId;

#pragma mark - View Lifecycle
- (void)viewDidLoad {
    [super viewDidLoad];
    [self setupHeadView];
    [self setupLineView];
    [self setupSceneView];
    [self setupFootView];
    [WXApiManager sharedManager].delegate = self;
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    // Return YES for supported orientations
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        return (interfaceOrientation != UIInterfaceOrientationPortraitUpsideDown);
    } else {
        return YES;
    }
}

#pragma mark - 设置场景
- (void)onSelectSessionScene {
    _currentScene = WXSceneSession;
    
    UILabel *tips = (UILabel *)[self.view viewWithTag:TIPSLABEL_TAG];
    tips.text = @"分享场景:会话";
}

- (void)onSelectTimelineScene {
    _currentScene = WXSceneTimeline;
    
    UILabel *tips = (UILabel *)[self.view viewWithTag:TIPSLABEL_TAG];
    tips.text = @"分享场景:朋友圈";
}

- (void)onSelectFavoriteScene {
    _currentScene = WXSceneFavorite;
    
    UILabel *tips = (UILabel *)[self.view viewWithTag:TIPSLABEL_TAG];
    tips.text = @"分享场景:收藏";
}

#pragma mark - Action
- (void)sendTextContent {
    [WXApiRequestHandler sendText:kTextMessage
                          InScene:_currentScene];
}

- (void)sendImageContent {
    NSString *filePath = [[NSBundle mainBundle] pathForResource:@"res1" ofType:@"jpg"];
    NSData *imageData = [NSData dataWithContentsOfFile:filePath];
    
    UIImage *thumbImage = [UIImage imageNamed:@"res1thumb.png"];
    [WXApiRequestHandler sendImageData:imageData
                               TagName:kImageTagName
                            MessageExt:kMessageExt
                                Action:kMessageAction
                            ThumbImage:thumbImage
                               InScene:_currentScene];
}

- (void)sendLinkContent {
    UIImage *thumbImage = [UIImage imageNamed:@"res2.png"];
    [WXApiRequestHandler sendLinkURL:kLinkURL
                             TagName:kLinkTagName
                               Title:kLinkTitle
                         Description:kLinkDescription
                          ThumbImage:thumbImage
                             InScene:_currentScene];
}

- (void)sendMusicContent {
    UIImage *thumbImage = [UIImage imageNamed:@"res3.jpg"];
    [WXApiRequestHandler sendMusicURL:kMusicURL
                              dataURL:kMusicDataURL
                                Title:kMusicTitle
                          Description:kMusicDescription
                           ThumbImage:thumbImage
                              InScene:_currentScene];
}

- (void)sendAppBrand{
    UIImage *thumbImage = nil;
    NSData *thumbData = UIImageJPEGRepresentation([UIImage imageNamed:@"Default.png"], 0.7);
    
    
    [UIAlertView requestWithTitle:@"brandUserName" message:nil defaultText:@"gh_d43f693ca31f" sure:^(UIAlertView *alertView, NSString *text) {
        NSString *brandUserName = text;
        [UIAlertView requestWithTitle:@"brandPath" message:nil defaultText:nil sure:^(UIAlertView *alertView, NSString *text) {
            NSString *brandPath = text;
            [UIAlertView requestWithTitle:@"url" message:nil defaultText:@"https://www.baidu.com" sure:^(UIAlertView *alertView, NSString *text) {
                NSString *webUrl = text;
                [UIAlertView requestWithTitle:@"是否带shareTicket" message:nil defaultText:@"Yes" sure:^(UIAlertView *alertView, NSString *text) {
                    BOOL withShareTicket = NO;
                    if ([text isEqualToString:@"Yes"])
                    {
                        withShareTicket = YES;
                    }
                    [UIAlertView requestWithTitle:@"分享版本" message:@"0是正式版，1是开发版，2是体验版" defaultText:@"0" sure:^(UIAlertView *alertView, NSString *text) {
                        WXMiniProgramType miniProgramType = (WXMiniProgramType)[text integerValue];
                        [WXApiRequestHandler sendMiniProgramWebpageUrl:webUrl
                                                              userName:brandUserName
                                                                  path:brandPath
                                                                 title:kMiniProgramTitle
                                                           Description:kMiniProgramDesc
                                                            ThumbImage:thumbImage
                                                           hdImageData:thumbData
                                                       withShareTicket:withShareTicket
                                                       miniProgramType:miniProgramType
                                                               InScene:_currentScene];
                    }];
                }];
            }];
        }];
    }];
}

- (void)launchMiniProgram
{
    [UIAlertView requestWithTitle:@"请输入小程序username" message:@"username" defaultText:@"gh_d43f693ca31f@app" sure:^(UIAlertView *alertView, NSString *text) {
        NSString *username = text;
        [UIAlertView requestWithTitle:@"输入path(可选)" message:@"path" defaultText:@"" sure:^(UIAlertView *alertView, NSString *text) {
            NSString *path = text;
            [UIAlertView requestWithTitle:@"分享版本" message:@"0是正式版，1是开发版，2是体验版" defaultText:@"0" sure:^(UIAlertView *alertView, NSString *text) {
                WXMiniProgramType type = (WXMiniProgramType)[text integerValue];
                [WXApiRequestHandler launchMiniProgramWithUserName:username path:path type:type];
            }];
            
        }];
    }];
}


- (void)sendVideoContent {
    UIImage *thumbImage = [UIImage imageNamed:@"res7.jpg"];
    [WXApiRequestHandler sendVideoURL:kVideoURL
                                Title:kVideoTitle
                          Description:kVideoDescription
                           ThumbImage:thumbImage
                              InScene:_currentScene];
}

- (void)sendAppContent {
    Byte* pBuffer = (Byte *)malloc(BUFFER_SIZE);
    memset(pBuffer, 0, BUFFER_SIZE);
    NSData* data = [NSData dataWithBytes:pBuffer length:BUFFER_SIZE];
    free(pBuffer);

    UIImage *thumbImage = [UIImage imageNamed:@"res2.jpg"];
    [WXApiRequestHandler sendAppContentData:data
                                    ExtInfo:kAppContentExInfo
                                     ExtURL:kAppContnetExURL
                                      Title:kAPPContentTitle
                                Description:kAPPContentDescription
                                 MessageExt:kAppMessageExt
                              MessageAction:kAppMessageAction
                                 ThumbImage:thumbImage
                                    InScene:_currentScene];
}

- (void)sendNonGifContent {
    NSString *filePath = [[NSBundle mainBundle] pathForResource:@"res5"
                                                         ofType:@"jpg"];
    NSData *emoticonData = [NSData dataWithContentsOfFile:filePath];
    
    UIImage *thumbImage = [UIImage imageNamed:@"res5thumb.png"];
    [WXApiRequestHandler sendEmotionData:emoticonData
                              ThumbImage:thumbImage
                                 InScene:_currentScene];
}

- (void)sendGifContent {
    NSString *filePath = [[NSBundle mainBundle] pathForResource:@"res6"
                                                         ofType:@"gif"];
    NSData *emoticonData = [NSData dataWithContentsOfFile:filePath];
    
    UIImage *thumbImage = [UIImage imageNamed:@"res6thumb.png"];
    [WXApiRequestHandler sendEmotionData:emoticonData
                              ThumbImage:thumbImage
                                 InScene:_currentScene];
}

- (void)sendAuthRequest {
    [WXApiRequestHandler sendAuthRequestScope: kAuthScope
                                        State:kAuthState
                                       OpenID:kAuthOpenID
                             InViewController:self];
}

- (void)sendFileContent {
    UIImage *thumbImage = [UIImage imageNamed:@"res2.jpg"];
    NSString* filePath = [[NSBundle mainBundle] pathForResource:kFileName
                                                         ofType:kFileExtension];
    NSData *fileData = [NSData dataWithContentsOfFile:filePath];

    [WXApiRequestHandler sendFileData:fileData
                        fileExtension:kFileExtension
                                Title:kFileTitle
                          Description:kFileDescription
                           ThumbImage:thumbImage
                              InScene:_currentScene];
}

- (void)addCardToWXCardPackage {
    NSDictionary *extDic = @{
                             @"code":@"",
                             @"openid":@"",
                             @"timestamp":@"1418301401",
                             @"signature":@"ad9cf9463610bc8752c95084716581d52cd33aa0"
                             };
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:extDic
                                                       options:NSJSONWritingPrettyPrinted
                                                         error:nil];
    NSString *extStr = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    
    [WXApiRequestHandler addCardsToCardPackage:@[@"pDF3iY9tv9zCGCj4jTXFOo1DxHdo"] cardExts:@[extStr]];
}

- (void)batchAddCardToWXCardPackage {
    NSDictionary *extDic = @{
                             @"code":@"",
                             @"openid":@"",
                             @"timestamp":@"1418301401",
                             @"signature":@"ad9cf9463610bc8752c95084716581d52cd33aa0"
                             };
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:extDic
                                                       options:NSJSONWritingPrettyPrinted
                                                         error:nil];
    NSString *extStr = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    
    [WXApiRequestHandler addCardsToCardPackage:@[@"pDF3iY9tv9zCGCj4jTXFOo1DxHdo",
                                                 @"pDF3iY9tv9zCGCj4jTXFOo1DxHdo"]
                                       cardExts:@[extStr,[extStr copy]]];
}

- (void)chooseCard {
    [WXApiRequestHandler chooseCard:@"wxf8b4f85f3a794e77"
                           cardSign:@"6caa49f4a5af3d64ac247e1f563e5b5eb94619ad"
                           nonceStr:@"k0hGdSXKZEj3Min5"
                           signType:@"SHA1" timestamp:1437997723];
}

- (void)chooseInvoiceTicket
{
    NSString *urlStr = @"http://203.195.235.76:8082/testjsapi/cardsign.php?appid=wx0673f9fda880509e&cardtype=INVOICE";
    NSString *newStr = [urlStr stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
    NSURL *url = [NSURL URLWithString:newStr];
    NSURLRequest *requst = [NSURLRequest requestWithURL:url cachePolicy:NSURLRequestReloadIgnoringLocalCacheData timeoutInterval:10];
    //异步链接(形式1,较少用)
    [NSURLConnection sendAsynchronousRequest:requst queue:[NSOperationQueue mainQueue] completionHandler:^(NSURLResponse *response, NSData *data, NSError *connectionError) {
        // 解析
        NSDictionary *dic = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingMutableContainers error:nil];
        NSLog(@"%@", dic);
        [WXApiRequestHandler chooseInvoice:@"wx0673f9fda880509e"
                                  cardSign:[dic objectForKey:@"cardSign"]
                                  nonceStr:[dic objectForKey:@"nonceStr"]
                                  signType:@"SHA1"
                                 timestamp:[[dic objectForKey:@"timestamp"] intValue]];
    }];
}

- (void)subscription
{
    [UIAlertView requestWithTitle:@"请输入scene" message:@"scene" defaultText:@"1000" sure:^(UIAlertView *alertView, NSString *text) {
        UInt32 scene = (UInt32)[text integerValue];
        [UIAlertView requestWithTitle:@"输入templateId" message:@"templateId" defaultText:@"7YuTL__ilzyZB9DXcDt2mHx-CAS_E7KtsQkhIGVhhRM" sure:^(UIAlertView *alertView, NSString *text) {
            NSString *templateId = text;
            
            [UIAlertView requestWithTitle:@"请输入reserved" message:nil defaultText:@"" sure:^(UIAlertView *alertView, NSString *text) {
                NSString *reserved = text;
                
                WXSubscribeMsgReq *req = [[WXSubscribeMsgReq alloc] init];
                req.scene = scene;
                req.templateId = templateId;
                req.reserved = reserved;
                
                [WXApi sendReq:req];
            }];
        }];
    }];
}

- (void)invoiceAuthInsert
{
    [UIAlertView requestWithTitle:@"请输入url" message:nil defaultText:@"" sure:^(UIAlertView *alertView, NSString *text) {
        WXInvoiceAuthInsertReq *req = [[WXInvoiceAuthInsertReq alloc] init];
        req.urlString = text;
        [WXApi sendReq:req];
    }];
}

- (void)nonTaxPay
{
    [UIAlertView requestWithTitle:@"请输入url" message:nil defaultText:@"" sure:^(UIAlertView *alertView, NSString *text) {
        WXNontaxPayReq *req = [[WXNontaxPayReq alloc] init];
        req.urlString = text;
        [WXApi sendReq:req];
    }];
}

- (void)payInsurance
{
    [UIAlertView requestWithTitle:@"请输入url" message:nil defaultText:@"" sure:^(UIAlertView *alertView, NSString *text) {
        WXPayInsuranceReq *req = [[WXPayInsuranceReq alloc] init];
        req.urlString = text;
        [WXApi sendReq:req];
    }];
}

#pragma mark -WechatAuthAPIDelegate
//得到二维码
- (void)onAuthGotQrcode:(UIImage *)image
{
    NSLog(@"onAuthGotQrcode");
}

//二维码被扫描
- (void)onQrcodeScanned
{
    NSLog(@"onQrcodeScanned");
}

//成功登录
- (void)onAuthFinish:(int)errCode AuthCode:(NSString *)authCode
{
    NSLog(@"onAuthFinish");
    
    UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"onAuthFinish"
                                                    message:[NSString stringWithFormat:@"authCode:%@ errCode:%d", authCode, errCode]
                                                   delegate:self
                                          cancelButtonTitle:@"OK"
                                          otherButtonTitles:nil, nil];
    [alert show];
}

#pragma mark - WXApiManagerDelegate
- (void)managerDidRecvGetMessageReq:(GetMessageFromWXReq *)req {
    // 微信请求App提供内容， 需要app提供内容后使用sendRsp返回
    NSString *strTitle = [NSString stringWithFormat:@"微信请求App提供内容"];
    NSString *strMsg = [NSString stringWithFormat:@"openID: %@", req.openID];
    [UIAlertView showWithTitle:strTitle message:strMsg sure:^(UIAlertView *alertView, NSString *text) {
        RespForWeChatViewController* controller = [[RespForWeChatViewController alloc] init];
        [self presentViewController:controller animated:YES completion:nil];
    }];
}

- (void)managerDidRecvShowMessageReq:(ShowMessageFromWXReq *)req {
    WXMediaMessage *msg = req.message;
    
    //显示微信传过来的内容
    NSString *strTitle = [NSString stringWithFormat:@"微信请求App显示内容"];
    NSString *strMsg = nil;
    
    if ([msg.mediaObject isKindOfClass:[WXAppExtendObject class]]) {
        WXAppExtendObject *obj = msg.mediaObject;
        strMsg = [NSString stringWithFormat:@"openID: %@, 标题：%@ \n描述：%@ \n附带信息：%@ \n文件大小:%lu bytes\n附加消息:%@\n", req.openID, msg.title, msg.description, obj.extInfo, (unsigned long)obj.fileData.length, msg.messageExt];
    }
    else if ([msg.mediaObject isKindOfClass:[WXTextObject class]]) {
        WXTextObject *obj = msg.mediaObject;
        strMsg = [NSString stringWithFormat:@"openID: %@, 标题：%@ \n描述：%@ \n内容：%@\n", req.openID, msg.title, msg.description, obj.contentText];
    }
    else if ([msg.mediaObject isKindOfClass:[WXImageObject class]]) {
        WXImageObject *obj = msg.mediaObject;
        strMsg = [NSString stringWithFormat:@"openID: %@, 标题：%@ \n描述：%@ \n图片大小:%lu bytes\n", req.openID, msg.title, msg.description, (unsigned long)obj.imageData.length];
    }
    else if ([msg.mediaObject isKindOfClass:[WXLocationObject class]]) {
        WXLocationObject *obj = msg.mediaObject;
        strMsg = [NSString stringWithFormat:@"openID: %@, 标题：%@ \n描述：%@ \n经纬度：lng:%f_lat:%f\n", req.openID, msg.title, msg.description, obj.lng, obj.lat];
    }
    else if ([msg.mediaObject isKindOfClass:[WXFileObject class]]) {
        WXFileObject *obj = msg.mediaObject;
        strMsg = [NSString stringWithFormat:@"openID: %@, 标题：%@ \n描述：%@ \n文件类型：%@ 文件大小:%lu\n", req.openID, msg.title, msg.description, obj.fileExtension, (unsigned long)obj.fileData.length];
    }
    else if ([msg.mediaObject isKindOfClass:[WXWebpageObject class]]) {
        WXWebpageObject *obj = msg.mediaObject;
        strMsg = [NSString stringWithFormat:@"openID: %@, 标题：%@ \n描述：%@ \n网页地址：%@\n", req.openID, msg.title, msg.description, obj.webpageUrl];
    }
    [UIAlertView showWithTitle:strTitle message:strMsg sure:nil];
}

- (void)managerDidRecvLaunchFromWXReq:(LaunchFromWXReq *)req {
    WXMediaMessage *msg = req.message;
    
    //从微信启动App
    NSString *strTitle = [NSString stringWithFormat:@"从微信启动"];
    NSString *strMsg = [NSString stringWithFormat:@"openID: %@, messageExt:%@", req.openID, msg.messageExt];
    [UIAlertView showWithTitle:strTitle message:strMsg sure:nil];
}

- (void)managerDidRecvMessageResponse:(SendMessageToWXResp *)response {
    NSString *strTitle = [NSString stringWithFormat:@"发送媒体消息结果"];
    NSString *strMsg = [NSString stringWithFormat:@"errcode:%d", response.errCode];
    [UIAlertView showWithTitle:strTitle message:strMsg sure:nil];
}

- (void)managerDidRecvAddCardResponse:(AddCardToWXCardPackageResp *)response {
        NSMutableString* cardStr = [[NSMutableString alloc] init];
    for (WXCardItem* cardItem in response.cardAry) {
        [cardStr appendString:[NSString stringWithFormat:@"code:%@ cardid:%@ cardext:%@ cardstate:%u\n",cardItem.encryptCode,cardItem.cardId,cardItem.extMsg,(unsigned int)cardItem.cardState]];
    }
    [UIAlertView showWithTitle:@"add card resp" message:cardStr sure:nil];
}

- (void)managerDidRecvChooseCardResponse:(WXChooseCardResp *)response {
    NSMutableString* cardStr = [[NSMutableString alloc] init];
    for (WXCardItem* cardItem in response.cardAry) {
        [cardStr appendString:[NSString stringWithFormat:@"cardid:%@, encryptCode:%@, appId:%@\n",cardItem.cardId,cardItem.encryptCode,cardItem.appID]];
    }
    [UIAlertView showWithTitle:@"choose card resp" message:cardStr sure:nil];
}

- (void)managerDidRecvChooseInvoiceResponse:(WXChooseInvoiceResp *)response {
    NSMutableString* cardStr = [[NSMutableString alloc] init];
    for (WXInvoiceItem* cardItem in response.cardAry) {
        [cardStr appendString:[NSString stringWithFormat:@"cardid:%@, encryptCode:%@, appId:%@\n",cardItem.cardId,cardItem.encryptCode,cardItem.appID]];
    }
    [UIAlertView showWithTitle:@"choose invoice resp" message:cardStr sure:nil];
}

- (void)managerDidRecvAuthResponse:(SendAuthResp *)response {
    NSString *strTitle = [NSString stringWithFormat:@"Auth结果"];
    NSString *strMsg = [NSString stringWithFormat:@"code:%@,state:%@,errcode:%d", response.code, response.state, response.errCode];
    
    [UIAlertView showWithTitle:strTitle message:strMsg sure:nil];
}

- (void)managerDidRecvSubscribeMsgResponse:(WXSubscribeMsgResp *)response
{
    NSString *title = [NSString stringWithFormat:@"templateId:%@,scene:%@,action:%@,reserved:%@,openId:%@",response.templateId,[NSNumber numberWithInteger:response.scene],response.action,response.reserved,response.openId];
    [UIAlertView showWithTitle:title message:nil sure:nil];
}

- (void)managerDidRecvLaunchMiniProgram:(WXLaunchMiniProgramResp *)response
{
    NSString *strTitle = [NSString stringWithFormat:@"LaunchMiniProgram结果"];
    NSString *strMsg = [NSString stringWithFormat:@"errMsg:%@,errcode:%d", response.extMsg, response.errCode];
    [UIAlertView showWithTitle:strTitle message:strMsg sure:nil];
}

- (void)managerDidRecvInvoiceAuthInsertResponse:(WXInvoiceAuthInsertResp *)response
{
    NSString *strTitle = [NSString stringWithFormat:@"电子发票授权开票"];
    NSString *strMsg = [NSString stringWithFormat:@"errcode:%d,wxorderid:%@", response.errCode, response.wxOrderId];
    [UIAlertView showWithTitle:strTitle message:strMsg sure:nil];
}

- (void)managerDidRecvNonTaxpayResponse:(WXNontaxPayResp *)response
{
    NSString *strTitle = [NSString stringWithFormat:@"非税支付结果"];
    NSString *strMsg = [NSString stringWithFormat:@"errcode:%d,wxorderid:%@", response.errCode, response.wxOrderId];
    [UIAlertView showWithTitle:strTitle message:strMsg sure:nil];
}

- (void)managerDidRecvPayInsuranceResponse:(WXPayInsuranceResp *)response
{
    NSString *strTitle = [NSString stringWithFormat:@"医保支付结果"];
    NSString *strMsg = [NSString stringWithFormat:@"errcode:%d, wxorderid:%@", response.errCode, response.wxOrderId];
    [UIAlertView showWithTitle:strTitle message:strMsg sure:nil];
}



#pragma mark - Private Methods
- (void)setupHeadView {
    UIView *headView = [[UIView alloc]initWithFrame:CGRectMake(0, 0, SCREEN_WIDTH, kHeadViewHeight)];
    [headView setBackgroundColor:RGBCOLOR(0xe1, 0xe0, 0xde)];
    UIImage *image = [UIImage imageNamed:@"micro_messenger.png"];
    NSInteger tlx = (headView.frame.size.width -  image.size.width) / 2;
    NSInteger tly = 20;
    
    UIImageView *imageView = [[UIImageView alloc]initWithFrame:CGRectMake(tlx, tly, image.size.width, image.size.height)];
    [imageView setImage:image];
    [headView addSubview:imageView];
    
    UILabel *title = [[UILabel alloc]initWithFrame:CGRectMake(0, tly + image.size.height, SCREEN_WIDTH, 40)];
    [title setText:[NSString stringWithFormat:@"微信OpenAPI(%@) Sample Demo",[WXApi getApiVersion]]];
    title.font = [UIFont systemFontOfSize:17];
    title.textColor = RGBCOLOR(0x11, 0x11, 0x11);
    title.textAlignment = NSTextAlignmentCenter;
    title.backgroundColor = [UIColor clearColor];
    [headView addSubview:title];
    
    [self.view addSubview:headView];
}

- (void)setupLineView {
    UIView *lineView1 = [[UIView alloc] initWithFrame:CGRectMake(0, kHeadViewHeight, SCREEN_WIDTH, 1)];
    lineView1.backgroundColor = [UIColor blackColor];
    lineView1.alpha = 0.1f;
    [self.view addSubview:lineView1];
    
    UIView *lineView2 = [[UIView alloc]initWithFrame:CGRectMake(0, kHeadViewHeight + 1, SCREEN_WIDTH, 1)];
    lineView2.backgroundColor = [UIColor whiteColor];
    lineView2.alpha = 0.25f;
    [self.view addSubview:lineView2];
    
    UIView *lineView3 = [[UIView alloc] initWithFrame:CGRectMake(0, kHeadViewHeight + 2 + kSceneViewHeight, SCREEN_WIDTH, 1)];
    lineView3.backgroundColor = [UIColor blackColor];
    lineView3.alpha = 0.1f;
    [self.view addSubview:lineView3];
    
    UIView *lineView4 = [[UIView alloc]initWithFrame:CGRectMake(0, kHeadViewHeight + 2 + kSceneViewHeight + 1, SCREEN_WIDTH, 1)];
    lineView4.backgroundColor = [UIColor whiteColor];
    lineView4.alpha = 0.25f;
    [self.view addSubview:lineView4];
}

- (void)setupSceneView {
    UIView *sceceView = [[UIView alloc] initWithFrame:CGRectMake(0, kHeadViewHeight + 2, SCREEN_WIDTH, 100)];
    [sceceView setBackgroundColor:RGBCOLOR(0xef, 0xef, 0xef)];
    
    UILabel *tips = [[UILabel alloc]init];
    tips.tag = TIPSLABEL_TAG;
    tips.text = @"分享场景:会话";
    tips.textColor = [UIColor blackColor];
    tips.backgroundColor = [UIColor clearColor];
    tips.textAlignment = NSTextAlignmentLeft;
    tips.frame = CGRectMake(10, 5, 200, 40);
    [sceceView addSubview:tips];
    
    UIButton *btn_x = [UIButton buttonWithType:UIButtonTypeRoundedRect];
    [btn_x setTitle:@"会话" forState:UIControlStateNormal];
    btn_x.titleLabel.font = [UIFont systemFontOfSize:15];
    [btn_x setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
    [btn_x setTitleColor:[UIColor grayColor] forState:UIControlStateDisabled];
    [btn_x setFrame:CGRectMake(20, 50, 80, 40)];
    [btn_x addTarget:self action:@selector(onSelectSessionScene) forControlEvents:UIControlEventTouchUpInside];
    [sceceView addSubview:btn_x];
    
    UIButton *btn_y = [UIButton buttonWithType:UIButtonTypeRoundedRect];
    [btn_y setTitle:@"朋友圈" forState:UIControlStateNormal];
    btn_y.titleLabel.font = [UIFont systemFontOfSize:15];
    [btn_y setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
    [btn_y setTitleColor:[UIColor grayColor] forState:UIControlStateDisabled];
    [btn_y setFrame:CGRectMake(120, 50, 80, 40)];
    [btn_y addTarget:self action:@selector(onSelectTimelineScene) forControlEvents:UIControlEventTouchUpInside];
    [sceceView addSubview:btn_y];
    
    UIButton *btn_z = [UIButton buttonWithType:UIButtonTypeRoundedRect];
    [btn_z setTitle:@"收藏" forState:UIControlStateNormal];
    btn_z.titleLabel.font = [UIFont systemFontOfSize:15];
    [btn_z setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
    [btn_z setTitleColor:[UIColor grayColor] forState:UIControlStateDisabled];
    [btn_z setFrame:CGRectMake(220, 50, 80, 40)];
    [btn_z addTarget:self action:@selector(onSelectFavoriteScene) forControlEvents:UIControlEventTouchUpInside];
    [sceceView addSubview:btn_z];
    
    [self.view addSubview:sceceView];
}

#define LEFTMARGIN			12
#define TOPMARGIN			15
#define BTNWIDTH			140
#define BTNHEIGHT			40
#define ADDBUTTON_AUTORELEASE(idx, title, sel) [self addBtn:idx tit:title selector:sel]

-(UIButton*) addBtn:(int)idx tit:(NSString*)title selector:(SEL)sel
{
    CGRect rect;
    if(idx % 2 == 1) {
        rect = CGRectMake(LEFTMARGIN, 25*(idx/2) + TOPMARGIN*(idx/2+1), BTNWIDTH, BTNHEIGHT - 4);
    } else {
        rect = CGRectMake(LEFTMARGIN*2 + BTNWIDTH, 25*(idx/2-1) + TOPMARGIN*(idx/2), BTNWIDTH, BTNHEIGHT - 4);
    }
    UIButton *button = [UIButton buttonWithType:UIButtonTypeRoundedRect];
    button.frame = rect;
    button.tag = idx;
    [button setTitle:title forState:UIControlStateNormal];
    button.titleLabel.font = [UIFont systemFontOfSize:14.0];
    [button addTarget:self action:sel forControlEvents:UIControlEventTouchUpInside];
    [self.footView addSubview:button];
    return button;
}

- (void)setupFootView {
    self.footView = [[UIScrollView alloc]initWithFrame:CGRectMake(0, kHeadViewHeight + 2 + kSceneViewHeight + 2, SCREEN_WIDTH, SCREEN_HEIGHT - kHeadViewHeight - 2 - kSceneViewHeight - 2)];
    [self.footView setBackgroundColor:RGBCOLOR(0xef, 0xef, 0xef)];
    self.footView.contentSize = CGSizeMake(SCREEN_WIDTH, SCREEN_HEIGHT);
    [self.view addSubview:self.footView];
    
    int index = 1;
    ADDBUTTON_AUTORELEASE(index++,@"发送Text消息给微信",@selector(sendTextContent));
    ADDBUTTON_AUTORELEASE(index++,@"发送Photo消息给微信",@selector(sendImageContent));
    ADDBUTTON_AUTORELEASE(index++,@"发送Link消息给微信",@selector(sendLinkContent));
    ADDBUTTON_AUTORELEASE(index++,@"发送Music消息给微",@selector(sendMusicContent));
    ADDBUTTON_AUTORELEASE(index++,@"发送Video消息给微信",@selector(sendVideoContent));
    ADDBUTTON_AUTORELEASE(index++,@"发送App消息给微信",@selector(sendAppContent));
    ADDBUTTON_AUTORELEASE(index++,@"发送非gif表情给微信",@selector(sendNonGifContent));
    ADDBUTTON_AUTORELEASE(index++,@"发送gif表情给微信",@selector(sendGifContent));
    ADDBUTTON_AUTORELEASE(index++,@"微信授权登录",@selector(sendAuthRequest));
    ADDBUTTON_AUTORELEASE(index++,@"发送文件消息给微信",@selector(sendFileContent));
    ADDBUTTON_AUTORELEASE(index++,@"添加单张卡券至卡包",@selector(addCardToWXCardPackage));
    ADDBUTTON_AUTORELEASE(index++,@"添加多张卡券至卡包",@selector(batchAddCardToWXCardPackage));
    ADDBUTTON_AUTORELEASE(index++,@"选择卡券",@selector(chooseCard));
    ADDBUTTON_AUTORELEASE(index++,@"选择发票",@selector(chooseInvoiceTicket));
    ADDBUTTON_AUTORELEASE(index++,@"小程序分享",@selector(sendAppBrand));
    ADDBUTTON_AUTORELEASE(index++,@"订阅消息",@selector(subscription));
    ADDBUTTON_AUTORELEASE(index++, @"拉起小程序", @selector(launchMiniProgram));
    ADDBUTTON_AUTORELEASE(index++, @"电子发票授权开票", @selector(invoiceAuthInsert));
    ADDBUTTON_AUTORELEASE(index++, @"调用非税", @selector(nonTaxPay));
    ADDBUTTON_AUTORELEASE(index++, @"医保支付", @selector(payInsurance));
    
    self.footView.contentSize = CGSizeMake(SCREEN_WIDTH, 25*(index/2) + TOPMARGIN*(index/2));
}

@end
