using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GalleryManager
{
    //单例
    public static GalleryManager Instance
    {
        get { return Nest.instance; }
    }
    private class Nest
    {
        static Nest() { }
        internal static readonly GalleryManager instance = new GalleryManager();
    }

    //安卓回调
    private GallerySDKCallBack gallerySDKCallBack;

    private IGallery gallerySdk;

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        gallerySDKCallBack = new GameObject("GallerySDKCallBack").AddComponent<GallerySDKCallBack>();
#if UNITY_ANDROID
        gallerySdk = new AndroidGallery();
#elif UNITY_IPHONE
        gallerySdk=new IOSGallery();
#endif
        gallerySdk.Init();
    }


    /// <summary>
    /// 无用方法，用来打开摄像头权限
    /// </summary>
    /// <returns></returns>
    private IEnumerator OpenCameraPermisson()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) yield break;
        WebCamDevice[] devices = WebCamTexture.devices;
    }

    /// <summary>
    /// 获得照片
    /// </summary>
    /// <param name="strType"></param>
    /// <param name="isCutPicture">头像正方形</param>
    public void GetPhoto(GetPhotoType strType, Action<Texture> callBack = null, bool isCutPicture = false)
    {
        if (callBack != null)
        {
            gallerySDKCallBack.SetRawImageActon = callBack;
        }
        gallerySdk.GetPhoto(strType, isCutPicture);
    }
}
