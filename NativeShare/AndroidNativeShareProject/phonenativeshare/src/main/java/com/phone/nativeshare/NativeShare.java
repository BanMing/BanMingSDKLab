package com.phone.nativeshare;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import android.text.TextUtils;
import android.widget.Toast;


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

    //分享纯文字
    public void ShareText(String content) {
        Intent intent = new Intent();
        intent.setAction(Intent.ACTION_SEND);
        intent.setType("text/plain");
        intent.putExtra(Intent.EXTRA_TEXT, content);
        Intent chooserIntent = Intent.createChooser(intent, "share to：");
        this.activity.startActivity(chooserIntent);
    }

    //分享链接
    public void ShareHtml(String content, String url) {
        Intent intent = new Intent();
        intent.setAction(Intent.ACTION_SEND);
        intent.setType("text/html");
        Uri uri = Uri.parse(url);
        intent.putExtra(Intent.EXTRA_TEXT, content);
        intent.putExtra(Intent.EXTRA_STREAM, uri);
        intent.putExtra(Intent.EXTRA_SUBJECT, "share");
        Intent chooserIntent = Intent.createChooser(intent, "share to：");
        this.activity.startActivity(chooserIntent);
    }

    //分享文本类文件
    public void ShareAllText(String content, String filePath) {
        Intent intent = new Intent();
        intent.setAction(Intent.ACTION_SEND);
        intent.setType("text/*");
        if (filePath != null && TextUtils.isEmpty(filePath)) {
            Uri uri = Uri.parse(filePath);
        }
        if (content != null && TextUtils.isEmpty(content)) {
            intent.putExtra(Intent.EXTRA_TEXT, content);
        }

        Intent chooserIntent = Intent.createChooser(intent, "share to：");
        this.activity.startActivity(chooserIntent);
    }

    //分享图片 这个路径是沙盒下的路径 重新建
    public void ShareImage(String content, String path) {
        Intent intent = new Intent();
        Uri uri = Uri.parse(path);
        intent.setAction(Intent.ACTION_SEND);
        intent.setType("image/*");
        intent.putExtra(Intent.EXTRA_TEXT, content);
        intent.putExtra(Intent.EXTRA_STREAM, uri);
        intent.putExtra(Intent.EXTRA_SUBJECT, "share");
        Intent chooserIntent = Intent.createChooser(intent, "share to：");
        this.activity.startActivity(chooserIntent);
    }

    //公用分享 content 文字 | filePath 文件路径 | intent 分享类型 | 分享标题
    public void Share(String content, String filePath, String intentType, String subject, String title) {
        if (intentType == null || TextUtils.isEmpty(intentType)) {
            return;
        }
        Intent intent = new Intent();
        intent.setAction(Intent.ACTION_SEND);
        intent.setType(intentType);
        if (filePath != null && TextUtils.isEmpty(filePath)) {
            Uri uri = Uri.parse(filePath);
            intent.putExtra(Intent.EXTRA_STREAM, uri);
        }
        if (content != null && TextUtils.isEmpty(content)) {
            intent.putExtra(Intent.EXTRA_TEXT, content);
        }
        if (subject != null && TextUtils.isEmpty(subject)) {
            intent.putExtra(Intent.EXTRA_SUBJECT, subject);
        }
        Intent chooserIntent = Intent.createChooser(intent, title == null ? "share" : title);
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
