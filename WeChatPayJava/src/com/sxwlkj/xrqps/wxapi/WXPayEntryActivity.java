package com.sxwlkj.xrqps.wxapi;

import com.tencent.mm.opensdk.constants.ConstantsAPI;
import com.tencent.mm.opensdk.modelbase.BaseReq;
import com.tencent.mm.opensdk.modelbase.BaseResp;
import com.tencent.mm.opensdk.openapi.IWXAPI;
import com.tencent.mm.opensdk.openapi.IWXAPIEventHandler;
import com.tencent.mm.opensdk.openapi.WXAPIFactory;
import com.tencent.mm.opensdk.utils.Log;
import com.unity3d.player.UnityPlayer;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.widget.Toast;

public class WXPayEntryActivity extends Activity implements IWXAPIEventHandler{
	
	private static final String TAG = " com.sxwlkj.xrqps";
	private static final String APP_ID = "wxe00c563826126a3f";
    
    private IWXAPI api;
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        
    	api = WXAPIFactory.createWXAPI(this, APP_ID);
        api.handleIntent(getIntent(), this);
    }

	@Override
	protected void onNewIntent(Intent intent) {
		super.onNewIntent(intent);
		setIntent(intent);
        api.handleIntent(intent, this);
	}

	@Override
	public void onReq(BaseReq req) {
		
	}

	@Override
	public void onResp(BaseResp resp) {

		Log.d(TAG, "onPayFinish, errCode = " + resp.errCode);
		if (resp.getType() == ConstantsAPI.COMMAND_PAY_BY_WX) {
			String msg = "";
			if (resp.errCode == 0) {
		       msg = "支付成功";
			} else if (resp.errCode == -1) {
		       msg = "支付错误";
		    } else if (resp.errCode == -2) {
		       msg = "已取消支付";
		    }
			Toast.makeText(WXPayEntryActivity.this, msg, Toast.LENGTH_SHORT).show();
			
			String errCode = String.valueOf(resp.errCode);
			String errDesc = resp.errStr;
			String transaction = resp.transaction;

			String errmsg = errCode + "&" + errDesc + "&" + transaction;
		    UnityPlayer.UnitySendMessage("SDK", "OnWechatPayHandler", errmsg);
		    
		    finish();
		}
	}
}