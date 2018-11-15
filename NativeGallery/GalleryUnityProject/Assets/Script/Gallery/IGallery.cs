using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IGallery
{
    void Init();
    void GetPhoto(GetPhotoType photoType, bool isCutPicture = false);
}

/// <summary>
/// 获得照片途径
/// </summary>
public enum GetPhotoType
{
    Carmera,
    Gallery
}