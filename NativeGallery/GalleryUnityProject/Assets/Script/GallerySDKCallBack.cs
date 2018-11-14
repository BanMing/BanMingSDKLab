using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GallerySDKCallBack : MonoBehaviour
{
    public Action<Texture> SetRawImageActon;
    public void DebugInfo(string info)
    {
        Debug.Log("Android Debug:" + info);
    }

	public void GetImagePath(string path){
		DebugInfo("GetImagePath:"+path);
        StartCoroutine(GetImageByPath(path));
	}
    private IEnumerator GetImageByPath(string path){
        yield return new WaitForSeconds(1);
        WWW www=new WWW("file://"+path);
        yield return www;
        if (www.error==null)
        {
            if (SetRawImageActon!=null)
            {
                SetRawImageActon(www.texture);
            }
        }else
        {
            Debug.LogError("LoadImage>>>>www.error"+www.error);
        }
    }
}
