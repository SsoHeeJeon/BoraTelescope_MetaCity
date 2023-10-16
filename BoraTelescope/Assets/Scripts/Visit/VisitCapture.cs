using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class VisitCapture : MonoBehaviour
{
    public float x;
    public float y;
    public int width;
    public int height;


    public RenEvent ren;

    [SerializeField]
    public static GameObject PopObject;
    public void OnClickScreenShot()
    {
        StartCoroutine("TakePicture");
    }

    IEnumerator TakePicture()
    {
        ren.PencilImg.transform.localPosition = new Vector3(16, 480, -300);
        ren.EarserImg.transform.localPosition = new Vector3(16, 480, -300);
        PopObject.SetActive(false);
        for (int i=0; i<ren.Sti.Stilist.Count; i++)
        {
            ren.Sti.Stilist[i].GetComponent<ImageMove>().Group.SetActive(false);
        }
        yield return new WaitForEndOfFrame();
        Texture2D screenTex = new Texture2D(width, height, TextureFormat.RGB24, false);
        Rect area = new Rect(x, y, width, height);
        screenTex.ReadPixels(area, 0, 0);

        if(!Directory.Exists("C:/Visit/"+ DateTime.Now.ToString("MM")))
        {
            Directory.CreateDirectory("C:/Visit/" + DateTime.Now.ToString("MM"));
        }
        File.WriteAllBytes("C:/Visit/" + DateTime.Now.ToString("MM")+"/" + DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss")+".png", screenTex.EncodeToPNG());

        GetComponent<Visitmanager>().gamemanager.GetComponent<Visitinfo>().list[int.Parse(DateTime.Now.ToString("MM")) - 1].Insert(0, DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss") + ".png");

        Destroy(screenTex);
        ren.OnClickReset();
        GetComponent<Visitmanager>().OnclickStoreBtn();
        ren.PencilImg.GetComponent<RectTransform>().anchoredPosition = new Vector3(16, 80, -300);
        ren.EarserImg.GetComponent<RectTransform>().anchoredPosition = new Vector3(16, 80, -300);
    }
}
