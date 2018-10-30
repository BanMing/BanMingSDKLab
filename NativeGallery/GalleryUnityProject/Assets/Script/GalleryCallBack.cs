using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryCallBack : MonoBehaviour
{

    public void DebugInfo(string info)
    {
        Debug.Log("Android Debug:" + info);
    }

	public void GetImagePath(string path){
		DebugInfo("GetImagePath:"+path);
	}
}
