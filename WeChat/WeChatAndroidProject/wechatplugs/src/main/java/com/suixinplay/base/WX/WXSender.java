package com.suixinplay.base.WX;

import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;

import com.tencent.mm.opensdk.modelmsg.SendAuth;
import com.tencent.mm.opensdk.modelmsg.SendMessageToWX;
import com.tencent.mm.opensdk.modelmsg.WXImageObject;
import com.tencent.mm.opensdk.modelmsg.WXMediaMessage;
import com.tencent.mm.opensdk.modelmsg.WXWebpageObject;
import com.tencent.mm.opensdk.modelpay.PayReq;
import com.tencent.mm.opensdk.openapi.IWXAPI;
import com.tencent.mm.opensdk.openapi.WXAPIFactory;
import com.unity3d.player.UnityPlayer;

import java.io.File;

public class WXSender {
    public IWXAPI api;

    private WXSender() {
    }

    public static WXSender Instance() {
        return singlon.INSTANCE;
    }

    public void Init(Activity activity) {
        api = WXAPIFactory.createWXAPI(activity, Constants.APP_ID);
        api.registerApp(Constants.APP_ID);
    }

    //登录微信
    public void LoginWeChat() {
        SendAuth.Req req = new SendAuth.Req();
        req.scope = "snsapi_userinfo";
        req.state = "none";
        api.sendReq(req);
    }

    //检测是否安装微信
    public boolean IsIntallWeChat() {
        boolean bIsWXAppInstalledAndSupported = api.isWXAppInstalled() && api.isWXAppSupportAPI();
        return bIsWXAppInstalledAndSupported;
    }

    //分享图文链接---朋友---这里使用本地图片--先由unity下载到沙盒路径然后直接使用图片
    public void ShareContentToFriend(String webUrl, String title, String content, String imgPath) {
        WXWebpageObject webPage = new WXWebpageObject();
        webPage.webpageUrl = webUrl;

        WXMediaMessage msg = new WXMediaMessage(webPage);
        msg.title = title;
        msg.description = content;
        Bitmap bmp = BitmapFactory.decodeFile(imgPath);
        Bitmap thumbBmp = Bitmap.createScaledBitmap(bmp, 150, 150, true);
        bmp.recycle();
        msg.thumbData = Util.bmpToByteArray(thumbBmp, true);

        ShareReq(msg, SendMessageToWX.Req.WXSceneSession);
    }

    //分享图文链接---朋友圈
    public void ShareContentToMoments(String webUrl, String title, String content, String imgPath) {
        WXWebpageObject webPage = new WXWebpageObject();
        webPage.webpageUrl = webUrl;


        WXMediaMessage msg = new WXMediaMessage(webPage);
        msg.title = title;
        msg.description = content;
        Bitmap bmp = BitmapFactory.decodeFile(imgPath);
        Bitmap thumbBmp = Bitmap.createScaledBitmap(bmp, 150, 150, true);
        bmp.recycle();
        msg.thumbData = Util.bmpToByteArray(thumbBmp, true);
        ShareReq(msg, SendMessageToWX.Req.WXSceneTimeline);
    }

    //分享本地图片---朋友
    public void ShareLocalPicToFriend(String imgPath) {
        WXImageObject imgObj = new WXImageObject();
        imgObj.setImagePath(imgPath);

        WXMediaMessage msg = new WXMediaMessage();
        msg.mediaObject = imgObj;
        Bitmap bmp = BitmapFactory.decodeFile(imgPath);
        Bitmap thumbBmp = Bitmap.createScaledBitmap(bmp, 150, 150, true);
        bmp.recycle();
        msg.thumbData = Util.bmpToByteArray(thumbBmp, true);

        ShareReq(msg, SendMessageToWX.Req.WXSceneSession);
    }

    //分享本地图片---朋友圈
    public void ShareLocalPicToMoments(String imgPath) {
        WXImageObject imgObj = new WXImageObject();
        imgObj.setImagePath(imgPath);

        WXMediaMessage msg = new WXMediaMessage();
        msg.mediaObject = imgObj;
        Bitmap bmp = BitmapFactory.decodeFile(imgPath);
        Bitmap thumbBmp = Bitmap.createScaledBitmap(bmp, 150, 150, true);
        bmp.recycle();
        msg.thumbData = Util.bmpToByteArray(thumbBmp, true);

        ShareReq(msg, SendMessageToWX.Req.WXSceneTimeline);
    }

    //发送
    private void ShareReq(WXMediaMessage msg, int scene) {
        SendMessageToWX.Req req = new SendMessageToWX.Req();
        req.transaction = String.valueOf(System.currentTimeMillis());
        req.message = msg;
        req.scene = scene;
        api.sendReq(req);
    }

    //微信支付
    public void WeChatPay(String openId, String packageValue, String nonceStr, String partnerId, String prepayId, String sign, String timeStamp) {
        PayReq req = new PayReq();
        req.appId = Constants.APP_ID;
        req.openId = openId;
        req.packageValue = packageValue;
        req.nonceStr = nonceStr;
        req.partnerId = partnerId;
        req.prepayId = prepayId;
        req.sign = sign;
        req.timeStamp = timeStamp;
        api.sendReq(req);
    }

    //测试使用
    public void CheckWeChatSdk() {
        UnityPlayer.UnitySendMessage("AndroidTest", "AndroidCall", "Jar CheckWeChatSdk");
        if (IsIntallWeChat()) {
            UnityPlayer.UnitySendMessage("AndroidTest", "AndroidCall", "微信已安装！");
        } else {
            UnityPlayer.UnitySendMessage("AndroidTest", "AndroidCall", "微信未安装！");
        }
    }

    private static class singlon {
        private static final WXSender INSTANCE = new WXSender();
    }
}
