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

    //��ó�� ȿ��. src �̹���(���� ȭ��)�� dest �̹����� ��ü
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        cameraMaterial.SetFloat("_Grayscale", 1);
        Graphics.Blit(src, dest, cameraMaterial);
    }
}
