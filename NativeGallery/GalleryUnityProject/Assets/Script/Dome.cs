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
            GalleryManager.Instance.GetPhoto(GetPhotoType.Carmera, (texture) => { rawImage.texture = texture; });
        }
        if (GUILayout.Button("Open Gallery", GUILayout.Width(200), GUILayout.Height(200)))
        {
            GalleryManager.Instance.GetPhoto(GetPhotoType.Gallery, (texture) => { rawImage.texture = texture; });
        }
        if (GUILayout.Button("Show Image", GUILayout.Width(200), GUILayout.Height(200)))
        {

            StartCoroutine(GetImageByPath(Application.persistentDataPath + "/UNITY_GALLERY_PICTUER.png"));
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