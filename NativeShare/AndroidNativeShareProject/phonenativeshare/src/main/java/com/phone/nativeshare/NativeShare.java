package com.phone.nativeshare;

import android.app.Activity;
import android.content.ComponentName;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.text.TextUtils;
import android.util.Base64;
import android.widget.Toast;

import java.io.File;
import java.nio.file.FileSystem;


public class NativeShare {

    private Activity activity;

    private NativeShare() {
    }

    public static NativeShare Instance() {
        return singlon.INSTANCE;
    }

    public void Init(Activity activity) {
        this.activity = activity;
    }

    //分享文字
    public void ShareText(String content) {
        Intent intent = new Intent();
        intent.setAction(Intent.ACTION_SEND);
        intent.setType("text/plain");
        intent.putExtra(Intent.EXTRA_TEXT, content);
        Intent chooserIntent = Intent.createChooser(intent, "share to：");
        this.activity.startActivity(chooserIntent);
    }
    //分享链接
    public void ShareHtml(String content,String url) {
//        File file=new File(url);
//        CreateToast(url);
//        CreateToast(String.valueOf(file.exists()));
        Intent intent = new Intent();
        intent.setAction(Intent.ACTION_SEND);
        intent.setType("text/html");
        Uri uri= Uri.parse(url);
        intent.putExtra(Intent.EXTRA_TEXT, content);
        intent.putExtra(Intent.EXTRA_STREAM, uri);
        intent.putExtra(Intent.EXTRA_SUBJECT, "share");
        Intent chooserIntent = Intent.createChooser(intent, "share to：");
        this.activity.startActivity(chooserIntent);
    }
    //分享链接
    public void ShareTextStar(String content,String Uri) {
        Intent intent = new Intent();
        intent.setAction(Intent.ACTION_SEND);
        intent.setType("text/*");

        intent.putExtra(Intent.EXTRA_TEXT, content);
        Intent chooserIntent = Intent.createChooser(intent, "share to：");
        this.activity.startActivity(chooserIntent);
    }
    //分享图片
    public void ShareImage(String content,String path) {
        Intent intent = new Intent();
//        Base64.decode("app_icon");
//        Bitmap bmp = BitmapFactory.decodeFile("app_icon");
//        Bitmap thumbBmp = Bitmap.createScaledBitmap(bmp, 150, 150, true);
        File file=new File(path);
        CreateToast(String.valueOf(file.exists()));
        Uri uri=Uri.parse(path);
        intent.setAction(Intent.ACTION_SEND);
        intent.setType("image/*");
        intent.putExtra(Intent.EXTRA_TEXT, content);
        intent.putExtra(Intent.EXTRA_STREAM, uri);
        intent.putExtra(Intent.EXTRA_SUBJECT, "share");
        Intent chooserIntent = Intent.createChooser(intent, "share to：");
        this.activity.startActivity(chooserIntent);
    }



    /*
     * 创建Toast
     */
    public void CreateToast(final String toast) {
        this.activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                //onCoderReturn("CreateToast()");
                Toast.makeText(activity,
                        toast, Toast.LENGTH_LONG).show();
            }
        });
    }

    private static class singlon {
        private static final NativeShare INSTANCE = new NativeShare();
    }
}
