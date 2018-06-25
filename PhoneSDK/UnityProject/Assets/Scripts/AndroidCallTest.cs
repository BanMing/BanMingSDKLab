using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidCallTest : MonoBehaviour
{
    private float level = 0;
    private int current = 0;
    private int total = 0;
    private AndroidJavaObject activity;
    private AndroidJavaObject androidHelper;
#if  UNITY_ANDROID
    void OnGUI()
    {
        GUILayout.Label("Level:" + level);
        GUILayout.Label("current:" + current);
        GUILayout.Label("total:" + total);
    }
    private AndroidJavaObject Activity
    {
        get
        {
            if (activity == null)
            {
                AndroidJavaClass Player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                activity = Player.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return activity;
        }
    }
    private AndroidJavaObject AndroidHelper
    {
        get
        {
            if (androidHelper == null)
            {
                AndroidJavaClass Helper = new AndroidJavaClass("com.banming.phonesdk.AndroidHelper");
                androidHelper = Helper.CallStatic<AndroidJavaObject>("Instance");
            }
            return androidHelper;
        }
    }
    /// <summary>
    /// 通过包名打开已安装的应用，若未安装则产生异常
    /// </summary>
    /// <param name="PKName">包名</param>
    /// <returns>bool</returns>
    public void OpenInstalledApp(string PKName)
    {
        if (string.IsNullOrEmpty(PKName))
        {
            Debug.Log("PKname is empty");
            // return false;
        }
        try
        {
            AndroidJavaObject it = new AndroidJavaObject("android.content.Intent");
            AndroidJavaObject packageManager = Activity.Call<AndroidJavaObject>("getPackageManager");
            it = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", PKName);
            Activity.Call("startActivity", it);
            // return true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            // return false;
        }
    }

    public void OpenOtherApp(string PKName)
    {
        Activity.Call("CallThirdApp", PKName);
    }
    public void CreateToast(string PKName)
    {

        Activity.Call("CreateToast", PKName);
    }
    public void ShareText(string message, string body)
    {
        Activity.Call("ShareText", message, body);
    }

    public void OpenOtherAppHelper(string PKName)
    {

        AndroidHelper.Call("CallThirdApp", Activity,PKName);
    }
    public void CreateToastHelper(string PKName)
    {
        AndroidHelper.Call("CreateToast", Activity,PKName);
    }
    public void HelperAdd(){
        current=AndroidHelper.Call<int>("TestAdd", 9,8);
    }
     public void HelperStaticAdd(){
         AndroidJavaClass Helper = new AndroidJavaClass("com.banming.phonesdk.AndroidHelper");
        total=Helper.CallStatic<int>("TestStaticAdd", 2,8);
    }
    public void HelperStaticOpenWeChat(){
         AndroidJavaClass Helper = new AndroidJavaClass("com.banming.phonesdk.AndroidHelper");
        Helper.CallStatic("CallThirdApp",Activity, "com.tencent.mm");
    }
    public void HelperStaticToast(){
         AndroidJavaClass Helper = new AndroidJavaClass("com.banming.phonesdk.AndroidHelper");
        Helper.CallStatic("CreateToastStatic",Activity, "Helper Static Toast!");
    }
#else
  public void OpenOtherApp(string PKName)
    {
    }
    public void OpenInstalledApp(string PKName)
    {
    }
     public void CreateToast(string PKName)
    {


    }
    public void ShareText(string message, string body)
    {

    }
       public void OpenOtherAppHelper(string PKName)
    {


    }
    public void CreateToastHelper(string PKName)
    {

    }
    public void HelperAdd(){
    }
     public void HelperStaticAdd(){
    }
     public void HelperStaticOpenWeChat(){

    }
    public void HelperStaticToast(){

    }
#endif
}
