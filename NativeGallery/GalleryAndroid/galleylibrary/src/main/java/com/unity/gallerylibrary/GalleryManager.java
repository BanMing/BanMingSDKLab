package com.unity.gallerylibrary;

import android.Manifest;
import android.app.Activity;
import android.content.ContentResolver;
import android.content.ContentValues;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.provider.MediaStore;
import android.provider.Settings;
import android.util.Log;
import android.view.KeyEvent;
import android.widget.Toast;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;

public class GalleryManager extends Activity {

    public static final int PHOTOHRAPH = 2;// 拍照
    public static final int NONE = 0;
    public static final int CAMERA_OK = 1;//请求相机权限
    public static final int STORAGE_OK = 2;//存储权限
    public static final int PHOTORESOULT = 3;// 结果
    public static final String IMAGE_UNSPECIFIED = "image/*";
    private static final int PHOTO_REQUEST_CODE = 1;//相册
    private static String UnityPersistentDataPath;//unity中的沙盒文件路径
    //沙盒文件下生成默认图片路径
    private static String UnityUsePicturePath;

    private boolean isCutPicture;

    //打开手机设置
    // Credit: https://stackoverflow.com/a/35456817/2373034
    public static void OpenSettings(final Context context) {
        Uri uri = Uri.fromParts("package", context.getPackageName(), null);

        Intent intent = new Intent();
        intent.setAction(Settings.ACTION_APPLICATION_DETAILS_SETTINGS);
        intent.setData(uri);

        context.startActivity(intent);
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        String type = this.getIntent().getStringExtra("type");
        UnityPersistentDataPath = this.getIntent().getStringExtra("UnityPersistentDataPath");
        UnityUsePicturePath = UnityPersistentDataPath + "/UNITY_GALLERY_PICTUER.png";
        isCutPicture = this.getIntent().getBooleanExtra("isCutPicture", false);
//        Toast.makeText(this, type, Toast.LENGTH_SHORT).show();
        if (type.equals("Camera")) {
            OpenTakePhoto();
        } else if (type.equals("Gallery")) {
            CheckAndOpenStoragePermission();
        }else {
            finish();
        }

    }
//    @Override
//    protected void onNewIntent(Intent intent) {
//        super.onNewIntent(intent);
//        setIntent(intent);
//    }

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event){

        if (keyCode == KeyEvent.KEYCODE_BACK && event.getRepeatCount() == 0){
//            finish();
            UnityPlayer.UnitySendMessage("GallerySDKCallBack", "GetImagePath", "");
//            Back2Unity();
        }

//        KeyEvent.KEYCODE_
        return super.onKeyDown(keyCode,event);
    }

    //先检测权限
    public void OpenTakePhoto() {
        if (Build.VERSION.SDK_INT > 22) {
            if ((this.checkSelfPermission(android.Manifest.permission.CAMERA) != PackageManager.PERMISSION_GRANTED)||(this.checkSelfPermission(android.Manifest.permission.WRITE_EXTERNAL_STORAGE) != PackageManager.PERMISSION_GRANTED)) {

                //先判断有没有权限 ，没有就在这里进行权限的申请
                this.requestPermissions(
                        new String[]{android.Manifest.permission.CAMERA,android.Manifest.permission.WRITE_EXTERNAL_STORAGE}, CAMERA_OK);
            } else {
                TakePhoto();
            }
        } else {
            TakePhoto();
        }

    }

    //检测是否开到存储权限
    public void CheckAndOpenStoragePermission() {
        if (Build.VERSION.SDK_INT > 22) {
            if (this.checkSelfPermission(Manifest.permission.WRITE_EXTERNAL_STORAGE) != PackageManager.PERMISSION_GRANTED) {

                //先判断有没有权限 ，没有就在这里进行权限的申请
                this.requestPermissions(
                        new String[]{android.Manifest.permission.WRITE_EXTERNAL_STORAGE}, STORAGE_OK);
            } else {
                OpenGallery();
            }
        } else {
            OpenGallery();
        }
    }

    //获得权限回调
    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        if (requestCode == CAMERA_OK) {
            //获得相机权限
            if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED&&grantResults[1] == PackageManager.PERMISSION_GRANTED) {
                TakePhoto();
            } else {
                //这里是拒绝给APP摄像头权限，给个提示什么的说明一下都可以。
                Toast.makeText(this, "请手动打开相机或储存权限", Toast.LENGTH_SHORT).show();
                OpenSettings(this);
                finish();
            }
        } else if (requestCode == STORAGE_OK) {
            if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                OpenGallery();
            } else {
                //这里是拒绝给存储权限，给个提示什么的说明一下都可以。
                Toast.makeText(this, "请手动打开存储权限", Toast.LENGTH_SHORT).show();
                OpenSettings(this);
                finish();
            }
        }
    }

    //打开相机
    private void TakePhoto() {
        Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.N) {
            File outFile = new File(UnityUsePicturePath);
            intent.putExtra(MediaStore.EXTRA_OUTPUT, getImageContentUri(outFile));
        } else {
            intent.putExtra(MediaStore.EXTRA_OUTPUT, Uri.fromFile(new File(UnityUsePicturePath)));
        }

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
            UnityPlayer.UnitySendMessage("GallerySDKCallBack", "GetImagePath", "");
            finish();
            return;
        }
        if (PHOTO_REQUEST_CODE == requestCode) {
            if (data == null) {
                finish();
                return;
            }
            if (isCutPicture) {
                StartPhotoZoom(data.getData());
            } else {
                String oldPath = GetImagePath((data.getData()));
                File imageFlie = new File(oldPath);
                String newPath = UnityUsePicturePath;
                CopyFile(imageFlie.getPath(), newPath);
                //调用unity中方法 GetImagePath（path）
                UnityPlayer.UnitySendMessage("GallerySDKCallBack", "GetImagePath", newPath);
                finish();
            }


        } else if (requestCode == PHOTOHRAPH) {

            String path = UnityUsePicturePath;
            UnityPlayer.UnitySendMessage("GallerySDKCallBack", "DebugLog", path);
            if (isCutPicture) {
                File picture = new File(path);
                StartPhotoZoom(Uri.fromFile(picture));
            } else {
                //调用unity中方法 GetImagePath（path）
                UnityPlayer.UnitySendMessage("GallerySDKCallBack", "GetImagePath", path);
                finish();
//                Back2Unity();
            }


        }//存图片
        else if (requestCode == PHOTORESOULT) {

            Bundle extras = data.getExtras();
            if (extras != null) {

                Bitmap photo = extras.getParcelable("data");

                try {
                    SaveBitmap(photo);
                } catch (IOException e) {
                    finish();
                    e.printStackTrace();
                }
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

    //存储图片并且通知unity更新
    public void SaveBitmap(Bitmap bitmap) throws IOException {
        FileOutputStream fOut = null;
        //            String FILE_NAME = System.currentTimeMillis() + ".jpg";
        // 一直设置一张图片
        String path = UnityUsePicturePath;
        try {
            //判断路径，如果没有则创建
            File destDir = new File(UnityPersistentDataPath);
            if (!destDir.exists()) {
                destDir.mkdir();
            }

            fOut = new FileOutputStream(path);
        } catch (FileNotFoundException e) {
            e.printStackTrace();
            finish();
        }
        //将bitmap对象写入本地路径中
        bitmap.compress(Bitmap.CompressFormat.PNG, 100, fOut);
        try {
            fOut.flush();
        } catch (IOException e) {
            e.printStackTrace();
            finish();
        }
        try {
            fOut.close();
        } catch (IOException e) {
            e.printStackTrace();
            finish();
        }
        //调用unity中方法 GetImagePath（path）
        UnityPlayer.UnitySendMessage("GallerySDKCallBack", "GetImagePath", path);
        finish();
    }

    /**
     * 复制单个文件
     *
     * @param oldPath String 原文件路径 如：c:/fqf.txt
     * @param newPath String 复制后路径 如：f:/fqf.txt
     * @return boolean
     */
    public void CopyFile(String oldPath, String newPath) {
        try {
            File oldfile = new File(oldPath);
            if (oldfile.exists()) { //文件存在时
                //获得原文件流
                FileInputStream inputStream = new FileInputStream(oldfile);
                byte[] data = new byte[1024];
                //输出流
                FileOutputStream outputStream = new FileOutputStream(newPath);
//                开始处理流
                while (inputStream.read(data) != -1) {
                    outputStream.write(data);
                }

                inputStream.close();
                outputStream.close();
//                Toast.makeText(this, "文件拷贝成功！！！", Toast.LENGTH_SHORT).show();
            } else {
//                Toast.makeText(this, "文件不存在！！！", Toast.LENGTH_SHORT).show();
            }

        } catch (Exception e) {
//            Toast.makeText(this, "复制单个文件操作出错！！！", Toast.LENGTH_SHORT).show();
            System.out.println("复制单个文件操作出错");
            e.printStackTrace();

        }

    }


    /**
     * 转换 content:// uri
     * 7.0以后使用
     *
     * @param imageFile
     * @return
     */
    public Uri getImageContentUri(File imageFile) {
        String filePath = imageFile.getAbsolutePath();
        Cursor cursor = getContentResolver().query(
                MediaStore.Images.Media.EXTERNAL_CONTENT_URI,
                new String[]{MediaStore.Images.Media._ID},
                MediaStore.Images.Media.DATA + "=? ",
                new String[]{filePath}, null);

        if (cursor != null && cursor.moveToFirst()) {
            int id = cursor.getInt(cursor
                    .getColumnIndex(MediaStore.MediaColumns._ID));
            Uri baseUri = Uri.parse("content://media/external/images/media");
            return Uri.withAppendedPath(baseUri, "" + id);
        } else {
            if (imageFile.exists()) {
                ContentValues values = new ContentValues();
                values.put(MediaStore.Images.Media.DATA, filePath);
                return getContentResolver().insert(
                        MediaStore.Images.Media.EXTERNAL_CONTENT_URI, values);
            } else {
                return null;
            }
        }

    }

    private  void Back2Unity(){
        Intent intent=new Intent(this,UnityPlayerActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        startActivity(intent);
    }
}
