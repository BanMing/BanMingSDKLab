using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOSCallTest : MonoBehaviour
{

    public void IosOpenApp(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return;
        }
        Application.OpenURL(url);
    }
}
