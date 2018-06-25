
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
public class WeChatTool
{
    public const string WeChatAppId="wx57fdd2cd0b0a8440";
    public const string WeChatSecret="811f0d10866600cca8a834f156facc23";
    public static string AccessTokenUrl="https://api.weixin.qq.com/sns/oauth2/access_token?appid="+WeChatAppId+"&secret="+WeChatSecret+"&code=#CODE#&grant_type=authorization_code";

    public static string UserInfoUrl="https://api.weixin.qq.com/sns/userinfo?access_token=#ACCESS_TOKEN#&openid=#OPENID#";
    public static void GetAccessToken(string code){

    }
    
}

[Serializable]
public struct WeChatUserInfo
{
    public string unionid;
    public string openid;
    //1为男性，2为女性
    public int sex;
    public string nickname;
    public string headimgurl;
    public string province;
    public string city;
    public string country;
    public string [] privilege;
}