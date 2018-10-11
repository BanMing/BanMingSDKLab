using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    private AndroidJavaClass phonePush;
    private AndroidJavaObject activity;
    // Use this for initialization
    void Start()
    {
        phonePush = new AndroidJavaClass("com.unityplugin.phonepush.PhonePushMgr");
        AndroidJavaClass Player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        activity = Player.GetStatic<AndroidJavaObject>("currentActivity");
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Context context, final int seconds, final String message, final int notificationId
    public void Push()
    {
        phonePush.CallStatic("registerLocalNotification",activity,5,"狗！",1);
    }
}
