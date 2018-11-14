using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{

    private AndroidJavaObject unityActivity;
    private AndroidJavaClass gallerySdk;
    public GameObject DebugCanvas;
    public RawImage rawImage;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        DebugCanvas.SetActive(true);
    }
    // Use this for initialization
    void Start()
    {
        AndroidJavaClass Player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = Player.GetStatic<AndroidJavaObject>("currentActivity");
        gallerySdk = new AndroidJavaClass("com.unity.gallerylibrary.GalleryManager");
        var gallerCallback = new GameObject("GallerySDKCallBack").AddComponent<GallerySDKCallBack>();
        gallerCallback.SetRawImageActon = (texture) => { rawImage.texture = texture; };

    }

    void OnGUI()
    {
        if (GUILayout.Button("Take Photo", GUILayout.Width(200), GUILayout.Height(200)))
        {
            GetPhoto("takePhoto");
        }
        if (GUILayout.Button("Open Gallery", GUILayout.Width(200), GUILayout.Height(200)))
        {
            GetPhoto("openGallery");
        }
        if (GUILayout.Button("Show Image", GUILayout.Width(200), GUILayout.Height(200)))
        {

            StartCoroutine(GetImageByPath(Application.persistentDataPath + "/UNITY_GALLERY_PICTUER.png"));
        }
        if (GUILayout.Button("Open Permission", GUILayout.Width(200), GUILayout.Height(200)))
        {
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", unityActivity, gallerySdk);

            intentObject.Call<AndroidJavaObject>("putExtra", "type", "openGallery");
            intentObject.Call<AndroidJavaObject>("putExtra", "UnityPersistentDataPath", Application.persistentDataPath);
            intentObject.Call<AndroidJavaObject>("putExtra", "isCutPicture", true);
            unityActivity.Call("startActivity", intentObject);
        }
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

    public void TestDebug(string str)
    {
        Debug.Log("TestDebug:" + str);
    }

    public void GetPhoto(string strType)
    {

        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", unityActivity, gallerySdk);

        intentObject.Call<AndroidJavaObject>("putExtra", "type", strType);
        intentObject.Call<AndroidJavaObject>("putExtra", "UnityPersistentDataPath", Application.persistentDataPath);
        // intentObject.Call<AndroidJavaObject>("putExtra", "isCutPicture", true);
        unityActivity.Call("startActivity", intentObject);

    }
    private IEnumerator GetImageByPath(string path)
    {
        yield return new WaitForSeconds(1);
        WWW www = new WWW("file://" + path);

        while (!www.isDone)
        {

        }
        yield return www;
        if (www.error == null)
        {
            rawImage.texture = www.texture;
        }
        else
        {
            Debug.LogError("LoadImage>>>>www.error" + www.error);
        }
    }

    public void GetImagePath(string path)
    {
        StartCoroutine(GetImageByPath(path));
    }

}
