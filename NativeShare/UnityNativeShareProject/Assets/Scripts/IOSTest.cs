using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
public class IOSTest : MonoBehaviour
{
#if UNITY_IOS
    void OnGUI()
    {
        if (GUILayout.Button("ShareAlertMessage", GUILayout.Width(200), GUILayout.Height(100)))
        {
            ShareIOS("测试", "test");
        }
        if (GUILayout.Button("ShareAlertMessage", GUILayout.Width(200), GUILayout.Height(100)))
        {
            ShareIOS("测试", "test", "https://www.baidu.com",new string [] {});
        }
    }


    public struct ConfigStruct
    {
        public string title;
        public string message;
    }

    [DllImport("__Internal")]
    private static extern void showAlertMessage(ref ConfigStruct conf);

    public struct SocialSharingStruct
    {
        public string text;
        public string subject;
        public string filePaths;
    }

    [DllImport("__Internal")]
    private static extern void showSocialSharing(ref SocialSharingStruct conf);

    public static void ShareIOS(string title, string message)
    {
        ConfigStruct conf = new ConfigStruct();
        conf.title = title;
        conf.message = message;
        showAlertMessage(ref conf);
    }

    public static void ShareIOS(string body, string subject, string url, string[] filePaths)
    {
        SocialSharingStruct conf = new SocialSharingStruct();
        conf.text = body;
        string paths = string.Join(";", filePaths);
        if (string.IsNullOrEmpty(paths))
            paths = url;
        else if (!string.IsNullOrEmpty(url))
            paths += ";" + url;
        conf.filePaths = paths;
        conf.subject = subject;

        showSocialSharing(ref conf);
    }
#endif
}

