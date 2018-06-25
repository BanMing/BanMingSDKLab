package com.suixinplay.base.wxapi;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.widget.Toast;

import com.suixinplay.base.WX.Constants;
import com.suixinplay.base.WX.WXSender;
import com.tencent.mm.opensdk.constants.ConstantsAPI;
import com.tencent.mm.opensdk.modelbase.BaseReq;
import com.tencent.mm.opensdk.modelbase.BaseResp;
import com.tencent.mm.opensdk.modelmsg.SendAuth;
import com.tencent.mm.opensdk.openapi.IWXAPI;
import com.tencent.mm.opensdk.openapi.IWXAPIEventHandler;
import com.tencent.mm.opensdk.openapi.WXAPIFactory;
import com.unity3d.player.UnityPlayer;

public class WXEntryActivity extends Activity implements IWXAPIEventHandler {

    private IWXAPI api;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (WXSender.Instance().api != null) {
            WXSender.Instance().api.handleIntent(getIntent(), this);
        }
        api = WXAPIFactory.createWXAPI(this, Constants.APP_ID, false);
        api.handleIntent(getIntent(), this);
    }

    @Override
    public void onReq(BaseReq baseReq) {

    }

    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
        setIntent(intent);
        api.handleIntent(intent, this);
    }

    @Override
    public void onResp(BaseResp baseResp) {
        Toast.makeText(this, "baseresp.getType = " + baseResp.getType(), Toast.LENGTH_SHORT).show();
        switch (baseResp.getType()) {
            //支付
            case ConstantsAPI.COMMAND_PAY_BY_WX:
                OnPayResp(baseResp);
                break;
            //分享
            case ConstantsAPI.COMMAND_SENDMESSAGE_TO_WX:
                OnShareResp(baseResp);
                break;
            //玩家登陆
            case ConstantsAPI.COMMAND_SENDAUTH:
                OnLoginResp(baseResp);
                break;
        }
        finish();
    }

    //支付回调
    private void OnPayResp(BaseResp baseResp) {
        if (baseResp.errCode == BaseResp.ErrCode.ERR_OK) {
            //支付成功
        } else {
            //支付失败
        }
    }

    //分享回调
    private void OnShareResp(BaseResp baseResp) {
        if (baseResp.errCode == BaseResp.ErrCode.ERR_OK) {
            //分享成功
            UnityPlayer.UnitySendMessage("AndroidTest", "AndroidCall", "分享成功");
        } else {
            //分享失败
            UnityPlayer.UnitySendMessage("AndroidTest", "AndroidCall", "分享失败");
        }
        finish();
    }

    //登录回调
    private void OnLoginResp(BaseResp baseResp) {
        switch (baseResp.errCode) {
            //登陆成功
            case BaseResp.ErrCode.ERR_OK:
                SendAuth.Resp sendResp = (SendAuth.Resp) baseResp;
                GetUserInfo(((SendAuth.Resp) baseResp).code);
                break;
            //取消或者失败
            default:
                break;
        }
    }

    //这里直接使用unity中的webrequest,不使用安卓中的webrequest，这样就可以不再写ios的webrequest
    private void GetUserInfo(String code) {
        UnityPlayer.UnitySendMessage("AndroidTest", "GetAccessToken", code);
    }
}
