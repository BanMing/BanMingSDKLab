using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public GameObject DebugCanvas;
    void Start()
    {
        DebugCanvas.SetActive(true);
    }

    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// This function can be called multiple times per frame (one call per event).
    /// </summary>
    void OnGUI()
    {
        if (GUILayout.Button("Save Galler/Photos", GUILayout.Width(100), GUILayout.Height(100)))
        {
            StartCoroutine(TakeScreenshotAndSave());
        }
        if (GUILayout.Button("Pick Image", GUILayout.Width(100), GUILayout.Height(100)))
        {
            //另一个媒体选择器占用了进程
            if (NativeGallery.IsMediaPickerBusy())
            {
                return;
            }
            PickImage(512);
        }
        if (GUILayout.Button("Pick Video", GUILayout.Width(100), GUILayout.Height(100)))
        {
            //另一个媒体选择器占用了进程
            if (NativeGallery.IsMediaPickerBusy())
            {
                return;
            }
            PickVideo();
        }
    }
    //截图保存
    private IEnumerator TakeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();
        //Save
        Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, "GalleryTest", "My img {0}.png"));
        Destroy(ss);

    }

    //选择一个png图片
    private void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image Path:" + path);
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture form: " + path);
                    return;
                }
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

                quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                quad.transform.forward = Camera.main.transform.forward;
                quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                Material material = quad.GetComponent<Renderer>().material;
                if (!material.shader.isSupported)
                {
                    material.shader = Shader.Find("Legacy Shaders/Diffuse");
                }
                material.mainTexture = texture;
                Destroy(quad, 5f);
                Destroy(texture, 5f);
            }
        }, "Select a PNG image", "image/png", maxSize);
        Debug.Log("Permisson result :" + permission);
    }

    private void PickVideo()
    {
        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
        {
            Debug.Log("Video path: " + path);
            if (path != null)
            {
                Handheld.PlayFullScreenMovie("file://" + path);
            }
        }, "Select a Video");
        Debug.Log("Permission result:" + permission);
    }
}
