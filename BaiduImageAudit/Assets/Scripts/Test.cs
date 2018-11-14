using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Test : MonoBehaviour
{

    const string APP_ID = "14738100";
    const string API_KEY = "vVftotywSAS1SibvhOq0sMGK";
    const string SECRET_KEY = "LcGaTVYCw45Osiy6XhxRSQkkYItmE7ig";
    private string imagePath;
    // private ÷
    /// <summary>
    /// 识别黄图
    /// </summary>
    /// <param name="path"></param>
    private string DetectImage(string path)
    {
        var client = new Baidu.Aip.ContentCensor.AntiPorn(API_KEY, SECRET_KEY);
        client.Timeout = 6000;//修改超时时间
        var image = File.ReadAllBytes(path);
        var result = client.Detect(image);
        return result.ToString();
    }
    /// <summary>
    /// 图像审核
    /// </summary>
    /// <param name="path"></param>
    private string ImageCensor(string path)
    {
        var client = new Baidu.Aip.ContentCensor.ImageCensor(API_KEY, SECRET_KEY);
        var image = File.ReadAllBytes(path);
        Debug.Log(image.Length);
        var res = client.UserDefined(image);
        return res.ToString();
    }
    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// This function can be called multiple times per frame (one call per event).
    /// </summary>
    void OnGUI()
    {
        imagePath = GUILayout.TextField(imagePath, GUILayout.Width(Screen.width), GUILayout.Height(50));

        if (GUILayout.Button("Image Censor", GUILayout.Width(100), GUILayout.Height(100)))
        {

            imagePath = Application.dataPath + "Resources/image/" + imagePath;
            Debug.Log("imagePath:" + imagePath);
            if (!File.Exists(imagePath))
            {
                Debug.Log("Path No Exists");
                return;
            }
            if (!string.IsNullOrEmpty(imagePath))
            {
                var res = ImageCensor(imagePath);
                Debug.Log("res:" + res);
            }

        }
        if (GUILayout.Button("Image Path", GUILayout.Width(100), GUILayout.Height(100)))
        {

            var textures = Resources.LoadAll<Texture2D>("image");
            Debug.Log(textures.Length);
            var data = textures[0].EncodeToJPG();
            var client = new Baidu.Aip.ContentCensor.ImageCensor(API_KEY, SECRET_KEY);
            var res = client.UserDefined(data);
            // Texture
        }
    }

    IEnumerator GetImageDefined(string path){
        WWW www=new WWW(path);
        
    }
}
