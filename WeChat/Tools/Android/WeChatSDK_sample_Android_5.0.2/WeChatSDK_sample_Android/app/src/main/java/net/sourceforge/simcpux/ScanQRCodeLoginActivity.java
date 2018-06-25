package net.sourceforge.simcpux;

import java.util.Random;

import net.sourceforge.simcpux.R;

import com.tencent.mm.opensdk.diffdev.DiffDevOAuthFactory;
import com.tencent.mm.opensdk.diffdev.IDiffDevOAuth;
import com.tencent.mm.opensdk.diffdev.OAuthErrCode;
import com.tencent.mm.opensdk.diffdev.OAuthListener;

import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

public class ScanQRCodeLoginActivity extends Activity implements OAuthListener{

	private static final String TAG = "MicroMsg.SDKSample.ScanQRCodeLoginActivity";
	
	private static final String APP_ID = "wx929be39cf668f738";//"wxd930ea5d5a258f4f";
	
	private IDiffDevOAuth oauth;
	
	private ImageView qrcodeIv;
	private TextView qrcodeStatusTv;
	private EditText signEt;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.scan_qrcode_login);
		
		oauth = DiffDevOAuthFactory.getDiffDevOAuth();
		
		signEt = (EditText)findViewById(R.id.sign_et);
		
		Button authBtn = (Button) findViewById(R.id.start_oauth_btn);
		authBtn.setOnClickListener(new View.OnClickListener() {
			
			@Override
			public void onClick(View v) {
				oauth.stopAuth();
				//boolean authRet = oauth.auth(APP_ID, "snsapi_userinfo", "helloworld", MainAct.this);
				
				String signature = signEt.getText().toString();//"fce891bb7d181766f92172395802a21f24de93e4";
				if (signature == null || signature.length() == 0) {
					return;
				}
				
				boolean authRet = oauth.auth(APP_ID, //应用唯一标识
						"snsapi_login", //应用授权作用域，如果有多个，用逗号(,)分隔
						genNonceStr(), //随机串
						genTimestamp(), //时间戳
						signature, //签名
						ScanQRCodeLoginActivity.this); // 授权完成回调接口（OAuthListener）
				
				Log.d(TAG, "authRet = %b" + authRet);
			}
		});
		
		Button cancelBtn = (Button) findViewById(R.id.stop_oauth_btn);
		cancelBtn.setOnClickListener(new View.OnClickListener() {
			
			@Override
			public void onClick(View v) {
				boolean cancelRet = oauth.stopAuth();
				Toast.makeText(ScanQRCodeLoginActivity.this, "cancel ret = " + cancelRet, Toast.LENGTH_SHORT).show();
			}
		});
		
		qrcodeIv = (ImageView) findViewById(R.id.qrcode_iv);
		qrcodeStatusTv = (TextView) findViewById(R.id.qrcode_status_tv);
	}

	@Override
	protected void onDestroy() {
		super.onDestroy();
		oauth.removeAllListeners();
		oauth.detach();
	}

	@Override
	public void onAuthGotQrcode(String qrcodeImgPath, byte[] imgBuf) {
		Toast.makeText(this, "onAuthGotQrcode, img path:" + qrcodeImgPath, Toast.LENGTH_SHORT).show();
		
		Bitmap bmp = BitmapFactory.decodeFile(qrcodeImgPath);
		if (bmp == null) {
			Log.e(TAG, "onAuthGotQrcode, decode bitmap is null");
			return;
		}

		qrcodeIv.setImageBitmap(bmp); // 展示二维码
		qrcodeIv.setVisibility(View.VISIBLE);
		
		qrcodeStatusTv.setText(R.string.qrcode_wait_for_scan);
		qrcodeStatusTv.setVisibility(View.VISIBLE);
	}

	@Override
	public void onQrcodeScanned() {
		Toast.makeText(this, "onQrcodeScanned", Toast.LENGTH_SHORT).show();

		qrcodeStatusTv.setText(R.string.qrcode_scanned);
		qrcodeStatusTv.setVisibility(View.VISIBLE);
	}

	@Override
	public void onAuthFinish(OAuthErrCode errCode, String authCode) {
		Log.d(TAG, "onAuthFinish, errCode = " + errCode.toString() + ", authCode = " + authCode);
		
		String tips = null;
		switch (errCode) {
		case WechatAuth_Err_OK:
			tips = getString(R.string.result_succ, authCode);
			break;
		case WechatAuth_Err_NormalErr:
			tips = getString(R.string.result_normal_err);
			break;
		case WechatAuth_Err_NetworkErr:
			tips = getString(R.string.result_network_err);
			break;
		case WechatAuth_Err_JsonDecodeErr:
			tips = getString(R.string.result_json_decode_err);
			break;
		case WechatAuth_Err_Cancel:
			tips = getString(R.string.result_user_cancel);
			break;
		case WechatAuth_Err_Timeout:
			tips = getString(R.string.result_timeout_err);
			break;
		default:
			break;
		}
		
		Toast.makeText(this, tips, Toast.LENGTH_LONG).show();
		
		qrcodeIv.setVisibility(View.GONE);
		qrcodeStatusTv.setVisibility(View.GONE);
	}
	
	private String genNonceStr() {
		//Random r = new Random(System.currentTimeMillis());
		//return MD5.getMessageDigest((APP_ID + r.nextInt(10000) + System.currentTimeMillis()).getBytes());
		return "noncestr";
	}
	
	private String genTimestamp() {
		//return System.currentTimeMillis() + "";
		return "timestamp";
	}
}
