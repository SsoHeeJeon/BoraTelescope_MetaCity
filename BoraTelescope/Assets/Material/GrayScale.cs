using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayScale : MonoBehaviour
{
    Material cameraMaterial;
    public static bool grayscaleOn = false;
    void Start()
    {
        cameraMaterial = new Material(Shader.Find("Hidden/Grayscale"));
        grayscaleOn = true;
    }

    //후처리 효과. src 이미지(현재 화면)를 dest 이미지로 교체
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        cameraMaterial.SetFloat("_Grayscale", 1);
        Graphics.Blit(src, dest, cameraMaterial);
    }
}
