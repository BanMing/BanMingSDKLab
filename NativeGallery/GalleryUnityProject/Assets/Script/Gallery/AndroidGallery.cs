using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AndroidGallery : IGallery
{

    private AndroidJavaObject unityActivity;
    //安卓调用相册sdk
    private AndroidJavaClass gallerySdk;


    public void Init()
    {
        AndroidJavaClass Player = new AndroidJavaClass("com.unity3d.player.UnityPlsayer");
        unityActivity = Player.GetStatic<AndroidJavaObject>("currentActivity");
        gallerySdk = new AndroidJavaClass("com.unity.gallerylibrary.GalleryManager");
    }

    //获得照片
    public void GetPhoto(GetPhotoType photoType, bool isCutPicture = false)
    {
        var strType = Enum.GetName(typeof(GetPhotoType), photoType);
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", unityActivity, gallerySdk);

        intentObject.Call<AndroidJavaObject>("putExtra", "type", strType);
        intentObject.Call<AndroidJavaObject>("putExtra", "UnityPersistentDataPath", Application.persistentDataPath);
        intentObject.Call<AndroidJavaObject>("putExtra", "isCutPicture", isCutPicture);
        unityActivity.Call("startActivity", intentObject);
    }
}