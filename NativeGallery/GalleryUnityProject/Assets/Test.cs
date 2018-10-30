using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    private AndroidJavaObject unityActivity;
    private AndroidJavaClass gallerySdk;
    // Use this for initialization
    void Start()
    {
        AndroidJavaClass Player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = Player.GetStatic<AndroidJavaObject>("currentActivity");
        gallerySdk = new AndroidJavaClass("com.unity.gallerylibrary.GallerManager");
		new GameObject("GalleryCallBack").AddComponent<GalleryCallBack>();

    }
	/// <summary>
	/// OnGUI is called for rendering and handling GUI events.
	/// This function can be called multiple times per frame (one call per event).
	/// </summary>
	void OnGUI()
	{
		if(GUILayout.Button("Open Gallery",GUILayout.Width(200),GUILayout.Height(200))){
			GetPhoto("takePhoto");
		}
		if(GUILayout.Button("Open Gallery",GUILayout.Width(200),GUILayout.Height(200))){
			GetPhoto("openGallery");
		}
	}
    public void GetPhoto(string strType)
    {
        AndroidJavaClass IntentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent",unityActivity,gallerySdk);
        // var intent=new IntentClass(unityActivity,);
		intentObject.Call("putExtra",strType);
		unityActivity.Call("startActivity",intentObject);

    }

}
