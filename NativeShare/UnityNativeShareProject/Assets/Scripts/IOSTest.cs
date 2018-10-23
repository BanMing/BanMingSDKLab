using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
public class IOSTest : MonoBehaviour
{
    private string url = "https://www.baidu.com";
    private string imagePath;
    private string imageDataString;
#if UNITY_IOS

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        
        imagePath = Application.persistentDataPath + "/ScreensShot.png";
    }
    void OnGUI()
    {
        if (GUILayout.Button("SocialSharing", GUILayout.Width(200), GUILayout.Height(100)))
        {
            // ShareIOS("测试", "test");
            SocialSharing("测试"+Environment.NewLine+"wwwww"+Environment.NewLine+"sssssssssss",url,imageDataString,"subject");
        }
        if (GUILayout.Button("ShareWeb", GUILayout.Width(200), GUILayout.Height(100)))
        {
            ShareWeb("测试"+Environment.NewLine+""+Environment.NewLine+"sssssssssss",url,imageDataString,"subject");
        }
        if (GUILayout.Button("Share image", GUILayout.Width(200), GUILayout.Height(100)))
        {
            // 分享图片
            SocialSharing("","",imageDataString,"subject");
        }
         if (GUILayout.Button("Share image path", GUILayout.Width(200), GUILayout.Height(100)))
        {
            // 分享图片
            SocialSharing("","",imagePath,"subject");
        }
        if (GUILayout.Button("Screen Shot", GUILayout.Width(200), GUILayout.Height(100)))
        {
           StartCoroutine(ShareScreenShot());
        }
    }

    IEnumerator ShareScreenShot()
    {
        yield return new WaitForSeconds(1f);
        ScreenCapture.CaptureScreenshot("ScreensShot.png");
        yield return new WaitForSeconds(1f);
        // NativeShare.Share("测试", Application.persistentDataPath + "/ScreenShot.png", "https://www.baidu.com");
        // showSocialSharing("测试",Application.persistentDataPath+"/ScreenShot.png");
        // (char* body, char* url, char* imageDataString, char* subject)
        using (WWW www = new WWW(imagePath))
        {
            yield return www;
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(www.bytes);
            imageDataString = Convert.ToBase64String(texture.EncodeToPNG());
        }


    }
    [DllImport("__Internal")]
    private static extern void SocialSharing(string body, string url, string imageDataString, string subject);
    [DllImport("__Internal")]
    private static extern void ShareWeb(string body, string url, string imagePath, string subject);
#endif
}

