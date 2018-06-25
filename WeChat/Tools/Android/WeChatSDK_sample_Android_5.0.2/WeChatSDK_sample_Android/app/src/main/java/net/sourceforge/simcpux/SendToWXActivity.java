package net.sourceforge.simcpux;

import java.io.File;
import java.net.URL;

import net.sourceforge.simcpux.uikit.CameraUtil;
import net.sourceforge.simcpux.uikit.MMAlert;
import android.app.Activity;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Bundle;
import android.os.Environment;
import android.view.View;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.Toast;

import com.tencent.mm.opensdk.modelmsg.SendAuth;
import com.tencent.mm.opensdk.modelmsg.SendMessageToWX;
import com.tencent.mm.opensdk.modelmsg.WXAppExtendObject;
import com.tencent.mm.opensdk.modelmsg.WXEmojiObject;
import com.tencent.mm.opensdk.modelmsg.WXImageObject;
import com.tencent.mm.opensdk.modelmsg.WXMediaMessage;
import com.tencent.mm.opensdk.modelmsg.WXMiniProgramObject;
import com.tencent.mm.opensdk.modelmsg.WXMusicObject;
import com.tencent.mm.opensdk.modelmsg.WXTextObject;
import com.tencent.mm.opensdk.modelmsg.WXVideoObject;
import com.tencent.mm.opensdk.modelmsg.WXWebpageObject;
import com.tencent.mm.opensdk.openapi.IWXAPI;
import com.tencent.mm.opensdk.openapi.WXAPIFactory;

public class SendToWXActivity extends Activity {

	private static final int THUMB_SIZE = 150;

	private static final String SDCARD_ROOT = Environment.getExternalStorageDirectory().getAbsolutePath();
	
	private IWXAPI api;
	private static final int MMAlertSelect1  =  0;
	private static final int MMAlertSelect2  =  1;
	private static final int MMAlertSelect3  =  2;
	
	private int mTargetScene = SendMessageToWX.Req.WXSceneSession;
	
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		api = WXAPIFactory.createWXAPI(this, Constants.APP_ID);
		
		setContentView(R.layout.send_to_wx);
		initView();
	}

	private void initView() {
		
		// send to weixin
		findViewById(R.id.send_text).setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				final EditText editor = new EditText(SendToWXActivity.this);
				editor.setLayoutParams(new LinearLayout.LayoutParams(LinearLayout.LayoutParams.FILL_PARENT, LinearLayout.LayoutParams.WRAP_CONTENT));
				editor.setText(R.string.send_text_default);
								
				MMAlert.showAlert(SendToWXActivity.this, "send text", editor, getString(R.string.app_share), getString(R.string.app_cancel), new DialogInterface.OnClickListener() {

					@Override
					public void onClick(DialogInterface dialog, int which) {
						String text = editor.getText().toString();
						if (text == null || text.length() == 0) {
							return;
						}
						
						// ��ʼ��һ��WXTextObject����
						WXTextObject textObj = new WXTextObject();
						textObj.text = text;

						// ��WXTextObject�����ʼ��һ��WXMediaMessage����
						WXMediaMessage msg = new WXMediaMessage();
						msg.mediaObject = textObj;
						// �����ı����͵���Ϣʱ��title�ֶβ�������
						// msg.title = "Will be ignored";
						msg.description = text;

						// ����һ��Req
						SendMessageToWX.Req req = new SendMessageToWX.Req();
						req.transaction = buildTransaction("text"); // transaction�ֶ�����Ψһ��ʶһ������
						req.message = msg;
						req.scene = mTargetScene;
						
						// ����api�ӿڷ�����ݵ�΢��
						api.sendReq(req);
						finish();
					}
				}, null);
			}
		});

		findViewById(R.id.send_img).setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				MMAlert.showAlert(SendToWXActivity.this, getString(R.string.send_img), 
						SendToWXActivity.this.getResources().getStringArray(R.array.send_img_item),
						null, new MMAlert.OnAlertSelectId(){

					@Override
					public void onClick(int whichButton) {						
						switch(whichButton){
						case MMAlertSelect1: {
							Bitmap bmp = BitmapFactory.decodeResource(getResources(), R.drawable.send_img);
							WXImageObject imgObj = new WXImageObject(bmp);
							
							WXMediaMessage msg = new WXMediaMessage();
							msg.mediaObject = imgObj;
							
							Bitmap thumbBmp = Bitmap.createScaledBitmap(bmp, THUMB_SIZE, THUMB_SIZE, true);
							bmp.recycle();
							msg.thumbData = Util.bmpToByteArray(thumbBmp, true);  // ��������ͼ

							SendMessageToWX.Req req = new SendMessageToWX.Req();
							req.transaction = buildTransaction("img");
							req.message = msg;
							req.scene = mTargetScene;
							api.sendReq(req);
							
							finish();
							break;
						}
						case MMAlertSelect2: {
							String path = SDCARD_ROOT + "/test.png";
							File file = new File(path);
							if (!file.exists()) {
								String tip = SendToWXActivity.this.getString(R.string.send_img_file_not_exist);
								Toast.makeText(SendToWXActivity.this, tip + " path = " + path, Toast.LENGTH_LONG).show();
								break;
							}
							
							WXImageObject imgObj = new WXImageObject();
							imgObj.setImagePath(path);
							
							WXMediaMessage msg = new WXMediaMessage();
							msg.mediaObject = imgObj;
							
							Bitmap bmp = BitmapFactory.decodeFile(path);
							Bitmap thumbBmp = Bitmap.createScaledBitmap(bmp, THUMB_SIZE, THUMB_SIZE, true);
							bmp.recycle();
							msg.thumbData = Util.bmpToByteArray(thumbBmp, true);
							
							SendMessageToWX.Req req = new SendMessageToWX.Req();
							req.transaction = buildTransaction("img");
							req.message = msg;
							req.scene = mTargetScene;
							api.sendReq(req);
							
							finish();
							break;
						}
						// deprecated imageurl
						/*case MMAlertSelect3: {
							String url = "http://weixin.qq.com/zh_CN/htmledition/images/weixin/weixin_logo20f761.png";
								
							try{
								WXImageObject imgObj = new WXImageObject();
								imgObj.imageUrl = url;
								
								WXMediaMessage msg = new WXMediaMessage();
								msg.mediaObject = imgObj;

								Bitmap bmp = BitmapFactory.decodeStream(new URL(url).openStream());
								Bitmap thumbBmp = Bitmap.createScaledBitmap(bmp, THUMB_SIZE, THUMB_SIZE, true);
								bmp.recycle();
								msg.thumbData = Util.bmpToByteArray(thumbBmp, true);
								
								SendMessageToWX.Req req = new SendMessageToWX.Req();
								req.transaction = buildTransaction("img");
								req.message = msg;
								req.scene = mTargetScene;
								api.sendReq(req);
								
								finish();
							} catch(Exception e) {
								e.printStackTrace();
							}
					
							break;
						}*/
						default:
							break;
						}
					}
					
				});
			}
		});

		findViewById(R.id.send_music).setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				
				MMAlert.showAlert(SendToWXActivity.this, getString(R.string.send_music),
						SendToWXActivity.this.getResources().getStringArray(R.array.send_music_item),
						null, new MMAlert.OnAlertSelectId(){

					@Override
					public void onClick(int whichButton) {						
						switch(whichButton){
						case MMAlertSelect1: {
							WXMusicObject music = new WXMusicObject();
							//music.musicUrl = "http://www.baidu.com";
							music.musicUrl="http://staff2.ustc.edu.cn/~wdw/softdown/index.asp/0042515_05.ANDY.mp3";
							//music.musicUrl="http://120.196.211.49/XlFNM14sois/AKVPrOJ9CBnIN556OrWEuGhZvlDF02p5zIXwrZqLUTti4o6MOJ4g7C6FPXmtlh6vPtgbKQ==/31353278.mp3";

							WXMediaMessage msg = new WXMediaMessage();
							msg.mediaObject = music;
							msg.title = "Music Title Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long";
							msg.description = "Music Album Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long";

							Bitmap bmp = BitmapFactory.decodeResource(getResources(), R.drawable.send_music_thumb);
							Bitmap thumbBmp = Bitmap.createScaledBitmap(bmp, THUMB_SIZE, THUMB_SIZE, true);
							bmp.recycle();
							msg.thumbData = Util.bmpToByteArray(thumbBmp, true);

							SendMessageToWX.Req req = new SendMessageToWX.Req();
							req.transaction = buildTransaction("music");
							req.message = msg;
							req.scene = mTargetScene;
							api.sendReq(req);
							
							finish();
							break;
						}
						case MMAlertSelect2: {
							WXMusicObject music = new WXMusicObject();
							music.musicLowBandUrl = "http://www.qq.com";

							WXMediaMessage msg = new WXMediaMessage();
							msg.mediaObject = music;
							msg.title = "Music Title";
							msg.description = "Music Album";

							Bitmap bmp = BitmapFactory.decodeResource(getResources(), R.drawable.send_music_thumb);
							Bitmap thumbBmp = Bitmap.createScaledBitmap(bmp, THUMB_SIZE, THUMB_SIZE, true);
							bmp.recycle();
							msg.thumbData = Util.bmpToByteArray(thumbBmp, true);

							SendMessageToWX.Req req = new SendMessageToWX.Req();
							req.transaction = buildTransaction("music");
							req.message = msg;
							req.scene = mTargetScene;
							api.sendReq(req);
							
							finish();
							break;
						}
						default:
							break;
						}
					}
				});
			}
		});
		
		findViewById(R.id.send_video).setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				MMAlert.showAlert(SendToWXActivity.this, getString(R.string.send_video), 
						SendToWXActivity.this.getResources().getStringArray(R.array.send_video_item),
						null, new MMAlert.OnAlertSelectId(){

					@Override
					public void onClick(int whichButton) {						
						switch(whichButton){
						case MMAlertSelect1: {
							WXVideoObject video = new WXVideoObject();
							video.videoUrl = "http://www.qq.com";

							WXMediaMessage msg = new WXMediaMessage(video);
							msg.title = "Video Title Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long";
							msg.description = "Video Description Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long";
							Bitmap bmp = BitmapFactory.decodeResource(getResources(), R.drawable.send_music_thumb);
							Bitmap thumbBmp = Bitmap.createScaledBitmap(bmp, THUMB_SIZE, THUMB_SIZE, true);
							bmp.recycle();
							msg.thumbData = Util.bmpToByteArray(thumbBmp, true);
							
							SendMessageToWX.Req req = new SendMessageToWX.Req();
							req.transaction = buildTransaction("video");
							req.message = msg;
							req.scene = mTargetScene;
							api.sendReq(req);
							
							finish();
							break;
						}
						case MMAlertSelect2: {
							WXVideoObject video = new WXVideoObject();
							video.videoLowBandUrl = "http://www.qq.com";

							WXMediaMessage msg = new WXMediaMessage(video);
							msg.title = "Video Title";
							msg.description = "Video Description";

							SendMessageToWX.Req req = new SendMessageToWX.Req();
							req.transaction = buildTransaction("video");
							req.message = msg;
							req.scene = mTargetScene;
							api.sendReq(req);
							
							finish();
							break;
						}
						default:
							break;
						}
					}
				});
			}
		});

		findViewById(R.id.send_webpage).setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {				
				MMAlert.showAlert(SendToWXActivity.this, getString(R.string.send_webpage),
						SendToWXActivity.this.getResources().getStringArray(R.array.send_webpage_item),
						null, new MMAlert.OnAlertSelectId(){

					@Override
					public void onClick(int whichButton) {						
						switch(whichButton){
						case MMAlertSelect1:
							WXWebpageObject webpage = new WXWebpageObject();
							webpage.webpageUrl = "http://www.qq.com";
							WXMediaMessage msg = new WXMediaMessage(webpage);
							msg.title = "WebPage Title WebPage Title WebPage Title WebPage Title WebPage Title WebPage Title WebPage Title WebPage Title WebPage Title Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long Very Long";
							msg.description = "WebPage Description WebPage Description WebPage Description WebPage Description WebPage Description WebPage Description WebPage Description WebPage Description WebPage Description Very Long Very Long Very Long Very Long Very Long Very Long Very Long";
							Bitmap bmp = BitmapFactory.decodeResource(getResources(), R.drawable.send_music_thumb);
							Bitmap thumbBmp = Bitmap.createScaledBitmap(bmp, THUMB_SIZE, THUMB_SIZE, true);
							bmp.recycle();
							msg.thumbData = Util.bmpToByteArray(thumbBmp, true);
							
							SendMessageToWX.Req req = new SendMessageToWX.Req();
							req.transaction = buildTransaction("webpage");
							req.message = msg;
							req.scene = mTargetScene;
							api.sendReq(req);
							
							finish();
							break;
						default:
							break;
						}
					}
				});
			}
		});

		findViewById(R.id.send_appbrand).setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				MMAlert.showAlert(SendToWXActivity.this, getString(R.string.send_appbrand),
						SendToWXActivity.this.getResources().getStringArray(R.array.send_appbrand_item),
						null, new MMAlert.OnAlertSelectId(){

							@Override
							public void onClick(int whichButton) {
								switch(whichButton){
									case MMAlertSelect1:
										WXMiniProgramObject miniProgram = new WXMiniProgramObject();
										miniProgram.webpageUrl = "http://www.qq.com";
										miniProgram.userName = "gh_d43f693ca31f";
										miniProgram.path = "pages/play/index?cid=fvue88y1fsnk4w2&ptag=vicyao&seek=3219";
										WXMediaMessage msg = new WXMediaMessage(miniProgram);
										msg.title = "分享小程序Title";
										msg.description = "分享小程序描述信息";
										Bitmap bmp = BitmapFactory.decodeResource(getResources(), R.drawable.send_music_thumb);
										Bitmap thumbBmp = Bitmap.createScaledBitmap(bmp, THUMB_SIZE, THUMB_SIZE, true);
										bmp.recycle();
										msg.thumbData = Util.bmpToByteArray(thumbBmp, true);

										SendMessageToWX.Req req = new SendMessageToWX.Req();
										req.transaction = buildTransaction("webpage");
										req.message = msg;
										req.scene = SendMessageToWX.Req.WXSceneTimeline;
										api.sendReq(req);

										finish();
										break;
									default:
										break;
								}
							}
						});
			}
		});

		findViewById(R.id.send_appdata).setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				MMAlert.showAlert(SendToWXActivity.this, getString(R.string.send_appdata), 
					SendToWXActivity.this.getResources().getStringArray(R.array.send_appdata_item),
					null, new MMAlert.OnAlertSelectId(){

					@Override
					public void onClick(int whichButton) {
						switch(whichButton){
						case MMAlertSelect1:
							final String dir = SDCARD_ROOT + "/tencent/";
							File file = new File(dir);
							if (!file.exists()) {
								file.mkdirs();
							}
							CameraUtil.takePhoto(SendToWXActivity.this, dir, "send_appdata", 0x101);
							break;
						case MMAlertSelect2: {
							final WXAppExtendObject appdata = new WXAppExtendObject();
							final String path = SDCARD_ROOT + "/test.png";
							appdata.fileData = Util.readFromFile(path, 0, -1);
							appdata.extInfo = "this is ext info";

							final WXMediaMessage msg = new WXMediaMessage();
							msg.setThumbImage(Util.extractThumbNail(path, 150, 150, true));
							msg.title = "this is title";
							msg.description = "this is description sjgksgj sklgjl sjgsgskl gslgj sklgj sjglsjgs kl gjksss ssssssss sjskgs kgjsj jskgjs kjgk sgjsk Very Long Very Long Very Long Very Longgj skjgks kgsk lgskg jslgj";
							msg.mediaObject = appdata;
							
							SendMessageToWX.Req req = new SendMessageToWX.Req();
							req.transaction = buildTransaction("appdata");
							req.message = msg;
							req.scene = mTargetScene;
							api.sendReq(req);
							
							finish();
							break;
						}
						case MMAlertSelect3: {
							// send appdata with no attachment
							final WXAppExtendObject appdata = new WXAppExtendObject();
							appdata.extInfo = "this is ext info";
							final WXMediaMessage msg = new WXMediaMessage();
							msg.title = "this is title";
							msg.description = "this is description";
							msg.mediaObject = appdata;
							
							SendMessageToWX.Req req = new SendMessageToWX.Req();
							req.transaction = buildTransaction("appdata");
							req.message = msg;
							req.scene = mTargetScene;
							api.sendReq(req);
							
							finish();
							break;
						}
						default:
							break;
						}
					}
					
				});
			}
		});
		
		findViewById(R.id.send_emoji).setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {				
				MMAlert.showAlert(SendToWXActivity.this, getString(R.string.send_emoji),
						SendToWXActivity.this.getResources().getStringArray(R.array.send_emoji_item),
						null, new MMAlert.OnAlertSelectId(){

					@Override
					public void onClick(int whichButton) {						
						final String EMOJI_FILE_PATH = SDCARD_ROOT + "/emoji.gif";
						final String EMOJI_FILE_THUMB_PATH = SDCARD_ROOT + "/emojithumb.jpg";				
						switch(whichButton){
						case MMAlertSelect1: {
							WXEmojiObject emoji = new WXEmojiObject();
							emoji.emojiPath = EMOJI_FILE_PATH;
							
							WXMediaMessage msg = new WXMediaMessage(emoji);
							msg.title = "Emoji Title";
							msg.description = "Emoji Description";
							msg.thumbData = Util.readFromFile(EMOJI_FILE_THUMB_PATH, 0, (int) new File(EMOJI_FILE_THUMB_PATH).length());
				
							
							SendMessageToWX.Req req = new SendMessageToWX.Req();
							req.transaction = buildTransaction("emoji");
							req.message = msg;
							req.scene = mTargetScene;
							api.sendReq(req);
							
							finish();
							break;
						}
						
						case MMAlertSelect2: {
							WXEmojiObject emoji = new WXEmojiObject();
							emoji.emojiData = Util.readFromFile(EMOJI_FILE_PATH, 0, (int) new File(EMOJI_FILE_PATH).length());
							WXMediaMessage msg = new WXMediaMessage(emoji);
							
							msg.title = "Emoji Title";
							msg.description = "Emoji Description";
							msg.thumbData = Util.readFromFile(EMOJI_FILE_THUMB_PATH, 0, (int) new File(EMOJI_FILE_THUMB_PATH).length());
							
							SendMessageToWX.Req req = new SendMessageToWX.Req();
							req.transaction = buildTransaction("emoji");
							req.message = msg;
							req.scene = mTargetScene;
							api.sendReq(req);
							
							finish();
							break;
						}
						default:
							break;
						}
					}
				});
			}
		});

		// get token
		findViewById(R.id.get_token).setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				// send oauth request
				final SendAuth.Req req = new SendAuth.Req();
				req.scope = "snsapi_userinfo";
				req.state = "none";
				api.sendReq(req);
				finish();
			}
		});
		
		// unregister from weixin
		findViewById(R.id.unregister).setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
				api.unregisterApp();
			}
		});
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		super.onActivityResult(requestCode, resultCode, data);

		switch (requestCode) {

		case 0x101: {
			if (resultCode == RESULT_OK) {
				final WXAppExtendObject appdata = new WXAppExtendObject();
				final String path = CameraUtil.getResultPhotoPath(this, data, SDCARD_ROOT + "/tencent/");
				appdata.filePath = path;
				appdata.extInfo = "this is ext info";

				final WXMediaMessage msg = new WXMediaMessage();
				msg.setThumbImage(Util.extractThumbNail(path, 150, 150, true));
				msg.title = "this is title";
				msg.description = "this is description";
				msg.mediaObject = appdata;

				SendMessageToWX.Req req = new SendMessageToWX.Req();
				req.transaction = buildTransaction("appdata");
				req.message = msg;
				req.scene = mTargetScene;
				api.sendReq(req);

				finish();
			}
			break;
		}
		default:
			break;
		}
	}

	private String buildTransaction(final String type) {
		return (type == null) ? String.valueOf(System.currentTimeMillis()) : type + System.currentTimeMillis();
	}
	
	public void onRadioButtonClicked(View view) {
		if (!(view instanceof RadioButton)) {
			return;
		}

		boolean checked = ((RadioButton) view).isChecked();

		switch (view.getId()) {
		case R.id.target_scene_session:
			if (checked) {
				mTargetScene = SendMessageToWX.Req.WXSceneSession;
			}
			break;
		case R.id.target_scene_timeline:
			if (checked) {
				mTargetScene = SendMessageToWX.Req.WXSceneTimeline;
			}
			break;
		case R.id.target_scene_favorite:
			if (checked) {
				mTargetScene = SendMessageToWX.Req.WXSceneFavorite;
			}			
			break;
		}
	}
}
