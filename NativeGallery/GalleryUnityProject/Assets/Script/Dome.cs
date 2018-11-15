using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class Dome : MonoBehaviour
{
    public GameObject DebugCanvas;
    public RawImage rawImage;

    void Awake()
    {
        DebugCanvas.SetActive(true);
    }

      void OnGUI()
    {
        if (GUILayout.Button("Take Photo", GUILayout.Width(200), GUILayout.Height(200)))
        {
            // GetPhoto("takePhoto");
        }
        if (GUILayout.Button("Open Gallery", GUILayout.Width(200), GUILayout.Height(200)))
        {
            // GetPhoto("openGallery");
        }
        if (GUILayout.Button("Show Image", GUILayout.Width(200), GUILayout.Height(200)))
        {

            StartCoroutine(GetImageByPath(Application.persistentDataPath + "/UNITY_GALLERY_PICTUER.png"));
        }
        if (GUILayout.Button("Open Permission", GUILayout.Width(200), GUILayout.Height(200)))
        {
            // AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", unityActivity, gallerySdk);

            // intentObject.Call<AndroidJavaObject>("putExtra", "type", "openGallery");
            // intentObject.Call<AndroidJavaObject>("putExtra", "UnityPersistentDataPath", Application.persistentDataPath);
            // intentObject.Call<AndroidJavaObject>("putExtra", "isCutPicture", true);
            // unityActivity.Call("startActivity", intentObject);
        }
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