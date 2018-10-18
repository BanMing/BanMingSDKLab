using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Test : MonoBehaviour
{

    public GameObject debugCanvs;
    private AndroidJavaObject unityAcitivity;
    private AndroidJavaObject phoneNativeShare;

    public Texture2D picture;
    public AndroidJavaObject UnityActivity
    {
        get
        {
            if (unityAcitivity == null)
            {
                AndroidJavaClass Player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                unityAcitivity = Player.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return unityAcitivity;
        }
    }
    public AndroidJavaObject PhoneNativeShare
    {
        get
        {
            if (phoneNativeShare == null)
            {
                AndroidJavaClass Helper = new AndroidJavaClass("com.phone.nativeshare.NativeShare");
                phoneNativeShare = Helper.CallStatic<AndroidJavaObject>("Instance");
            }
            return phoneNativeShare;
        }
    }

    void Awake()
    {
        debugCanvs.gameObject.SetActive(true);
    }
    // Use this for initialization
    void Start()
    {
        PhoneNativeShare.Call("Init", UnityActivity);
        Debug.Log("file://" + Application.persistentDataPath + "/money.png");
        Debug.Log(File.Exists("file://" + Application.persistentDataPath + "money.png"));
        Debug.Log(File.Exists("file:/" + Application.persistentDataPath + "/money.png"));
    }
    //String packageName, String className, String content, String title, String subject
    public void ShareText()
    {
        PhoneNativeShare.Call("ShareText", "https://www.baidu.com/");
    }
    string html="<html><head><meta charset=\"utf-8\"><title>菜鸟教程(runoob.com)</title></head><body><h1>我的第一个标题</h1><p>我的第一个段落。</p></body></html>";
    void OnGUI()
    {
        if (GUILayout.Button("Share", GUILayout.Width(200), GUILayout.Height(100)))
        {
            ShareText();
        }
        if (GUILayout.Button("Share Html", GUILayout.Width(200), GUILayout.Height(100)))
        {
            PhoneNativeShare.Call("ShareHtml",html,Application.persistentDataPath+"/test.html");
        }
        if (GUILayout.Button("Share Star", GUILayout.Width(200), GUILayout.Height(100)))
        {
            PhoneNativeShare.Call("ShareTextStar", Application.persistentDataPath+"/test.html");
        }
        if (GUILayout.Button("Screen Shot", GUILayout.Width(200), GUILayout.Height(100)))
        {
            StartCoroutine(TakeScreenshotAndroid());
        }
        if (GUILayout.Button("Shareurl", GUILayout.Width(200), GUILayout.Height(100)))
        {
            PhoneNativeShare.Call("ShareImage", "这是一条分享信息", Application.persistentDataPath + "/ScreensShot.png");
        }
        if (GUILayout.Button("Shareurl", GUILayout.Width(200), GUILayout.Height(100)))
        {
            if (!File.Exists(Application.persistentDataPath+"/test.html"))
            {
               File.WriteAllText(Application.persistentDataPath+"/test.html",html); 
            }
           
        }

    }
    private void CreateHtmlFlie(){
        File.WriteAllText(Application.persistentDataPath+"test.html",html);
    }
    private IEnumerator TakeScreenshotAndroid()
    {

        if (!Application.isEditor)
        {
            // AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            // AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            // intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            // // AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            // // AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + |);
            // intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "EXTRA_TITLE");
            // intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "uriObject");
            // intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "shareDefaultText");
            // intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            // AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            // AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            // AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share");
            // currentActivity.Call("startActivity", chooser);
            ScreenCapture.CaptureScreenshot("ScreensShot.png");
            // WWW www = new WWW("https://ss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=873212881,2139295536&fm=26&gp=0.jpg");  
            // while(!www.isDone){  
            //     yield return null;  
            // }  
            // File.Create()
            // Texture2D picture = www.texture;
            // byte[] bytes = new AndroidJavaObject("android.util.Base64").CallStatic<byte[]>("decode",System.Convert.ToBase64String (picture.EncodeToPNG()),0);
            // AndroidJavaObject bitmap =  new AndroidJavaObject("android.graphics.BitmapFactory").CallStatic<AndroidJavaObject>("decodeByteArray",bytes,0,bytes.Length);
            // AndroidJavaObject compress = new AndroidJavaClass("android.graphics.Bitmap$CompressFormat").GetStatic<AndroidJavaObject>("JPEG");
            // bitmap.Call<bool>("compress",compress,100,new AndroidJavaObject("java.io.ByteArrayOutputStream"));
            // string path = new AndroidJavaClass("android.provider.MediaStore$Images$Media").CallStatic<string>("insertImage",currentActivity.Call<AndroidJavaObject>("getContentResolver"),bitmap,picture.name,"");
            // AndroidJavaObject uri = new AndroidJavaClass("android.net.Uri").CallStatic<AndroidJavaObject>("parse",path);
            // AndroidJavaObject sharingIntent  = new AndroidJavaObject("android.content.Intent");
            // sharingIntent.Call<AndroidJavaObject>("setAction","android.intent.action.SEND");
            // sharingIntent.Call<AndroidJavaObject>("setType","image/*");
            // sharingIntent.Call<AndroidJavaObject>("putExtra","android.intent.extra.STREAM",uri);
            // sharingIntent.Call<AndroidJavaObject>("putExtra","android.intent.extra.TEXT","ssss");
            // // sharingIntent.Call<AndroidJavaObject>("putExtra","android.intent.extra.SUBJECT","Share to");
            // AndroidJavaObject createChooser = sharingIntent.CallStatic<AndroidJavaObject>("createChooser",sharingIntent, "Share to");
            // currentActivity.Call("startActivity",createChooser);
            yield return new WaitForSeconds(1f);
            Debug.Log(File.Exists(Application.persistentDataPath + "/ScreenShot.png"));
        }
        yield return new WaitUntil(() => Application.isFocused);
    }
    private static AndroidJavaObject currentActivity
    {
        get
        {
            return new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        }
    }
}
