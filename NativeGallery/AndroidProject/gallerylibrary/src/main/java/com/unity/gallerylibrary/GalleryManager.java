package com.unity.gallerylibrary;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;
import android.app.Activity;
import android.content.ContentResolver;
import android.content.Intent;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.provider.MediaStore;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;

public class GalleryManager extends Activity {
    public static final int PHOTOHRAPH = 2;// 拍照
    public static final int NONE = 0;
    public static final int PHOTORESOULT = 3;// 结果
    public static final String IMAGE_UNSPECIFIED = "image/*";
    private static final int PHOTO_REQUEST_CODE = 1;//相册
    private String unitygameobjectName = "GallerySDKCallBack"; //Unity 中对应挂脚本对象的名称

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        String type = this.getIntent().getStringExtra("type");
        if (type.equals("takePhoto")) {
            OpenTakePhoto();
        } else if (type.equals("openGallery")) {
            OpenGallery();
        }
    }

    //打开相机
    public void OpenTakePhoto() {
        Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
        intent.putExtra(MediaStore.EXTRA_OUTPUT, Uri.fromFile(new File(Environment.getExternalStorageDirectory(), "temp.jpg")));
        startActivityForResult(intent, PHOTOHRAPH);
    }

    //打开相册
    public void OpenGallery() {
        Intent intent = new Intent(Intent.ACTION_PICK, null);
        intent.setDataAndType(MediaStore.Images.Media.EXTERNAL_CONTENT_URI, "image/*");
        startActivityForResult(intent, PHOTO_REQUEST_CODE);
    }

    //选择照片的回到
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (resultCode == NONE) {
            return;
        }
        if (PHOTO_REQUEST_CODE == requestCode) {
            if (data == null) {
                return;
            }
            StartPhotoZoom(data.getData());
        } else if (requestCode == PHOTOHRAPH) {
            String path = Environment.getExternalStorageDirectory() + "/temp.jpg";
            File picture = new File(path);
            StartPhotoZoom(Uri.fromFile(picture));
        }//存图片
        else if (requestCode == PHOTORESOULT) {

            try {
                String path = Environment.getExternalStorageDirectory() + "/temp.jpg";
                //调用unity中方法 GetImagePath（path）
            UnityPlayer.UnitySendMessage(unitygameobjectName, "GetImagePath", path);
                Bitmap bitmap = BitmapFactory.decodeFile(path);
                SaveBitmap(bitmap);
            } catch (IOException e) {
                e.printStackTrace();
            }

        }
    }

    //对图片修改
    private void StartPhotoZoom(Uri uri) {
        Intent intent = new Intent("com.android.camera.action.CROP");
        intent.setDataAndType(uri, IMAGE_UNSPECIFIED);
        intent.putExtra("crop", "true");
        // aspectX aspectY 是宽高的比例
        intent.putExtra("aspectX", 1);
        intent.putExtra("aspectY", 1);
        // outputX outputY 是裁剪图片宽高
        intent.putExtra("outputX", 300);
        intent.putExtra("outputY", 300);
        intent.putExtra("return-data", true);
        startActivityForResult(intent, PHOTORESOULT);
    }

    //获得图片路径从相册
    private String GetImagePath(Uri uri) {
        if (uri == null) {
            return null;
        }
        String path = null;
        final String scheme = uri.getScheme();
        if (scheme == null) {
            path = uri.getPath();
        } else if (ContentResolver.SCHEME_FILE.equals(scheme)) {
            path = uri.getPath();
        } else if (ContentResolver.SCHEME_CONTENT.equals(scheme)) {
            String[] proj = {MediaStore.Images.Media.DATA};
            Cursor cursor = getContentResolver().query(uri, proj, null, null, null);
            int nPhotoColumn = cursor.getColumnIndexOrThrow(MediaStore.Images.Media.DATA);
            if (null != cursor) {
                cursor.moveToFirst();
                path = cursor.getString(nPhotoColumn);
            }
            cursor.close();
        }
        return path;
    }

    public void SaveBitmap(Bitmap bitmap) throws IOException {
        FileOutputStream fOut = null;
        String path = "/mnt/sdcard/DCIM/";
        try {
            //判断路径，如果没有则创建
            File destDir = new File(path);
            if (!destDir.exists()) {
                destDir.mkdir();
            }
            String FILE_NAME = System.currentTimeMillis() + ".jpg";
            fOut = new FileOutputStream(path + "/" + FILE_NAME);
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }
        //将bitmap对象写入本地路径中
        bitmap.compress(Bitmap.CompressFormat.JPEG, 100, fOut);
        try {
            fOut.flush();
        } catch (IOException e) {
            e.printStackTrace();
        }
        try {
            fOut.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
