using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class VisitCapture : MonoBehaviour
{
    public float x;
    public float y;
    public int width;
    public int height;


    public RenEvent ren;
    string path;
    string filename;
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

        string currenttime = DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss");
        path = "C:/Visit/" + DateTime.Now.ToString("MM") + "/" + currenttime + ".png";
        filename = currenttime + ".png";
        if (!Directory.Exists("C:/Visit/"+ DateTime.Now.ToString("MM")))
        {
            Directory.CreateDirectory("C:/Visit/" + DateTime.Now.ToString("MM"));
        }
        File.WriteAllBytes(path, screenTex.EncodeToPNG());

        GetComponent<Visitmanager>().gamemanager.GetComponent<Visitinfo>().list[int.Parse(DateTime.Now.ToString("MM")) - 1].Insert(0, DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss") + ".png");

        Destroy(screenTex);
        ren.OnClickReset();
        GetComponent<Visitmanager>().OnclickStoreBtn();
        ren.PencilImg.GetComponent<RectTransform>().anchoredPosition = new Vector3(16, 80, -300);
        ren.EarserImg.GetComponent<RectTransform>().anchoredPosition = new Vector3(16, 80, -300);
        StartCoroutine("Upload");
    }


    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", File.ReadAllBytes(path), filename);
        form.AddField("placeName ", "憮選-營嘐嫌");

        UnityWebRequest www = UnityWebRequest.Post("https://xr.awesomepia.com/v1/guestBook/imageUpload", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete! " + www.downloadHandler.text);

        }
        path = "";
        filename = "";
    }

    //[SerializeField] RawImage img;
    //IEnumerator GetTexture()
    //{
    //    UnityWebRequest ww = UnityWebRequest.Get("https://xr.awesomepia.com/v1/guestBook/imageFileList?limitStart=0&recordSize=1&placeName=憮選-營嘐嫌");
    //    yield return ww.SendWebRequest();
    //    if (ww.isNetworkError || ww.isHttpError)
    //    {
    //        Debug.Log(ww.error);
    //    }
    //    else
    //    {
    //        print(ww.downloadHandler.text);
    //    }

    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://xr-data.s3.ap-northeast-2.amazonaws.com/guestbook/prod/2023/10/a53d69a1-6e2f-4226-983e-714d0546c51c.png");
    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        Rect rect = new Rect(0, 0, myTexture.width, myTexture.height);
    //        Sprite sp = Sprite.Create((Texture2D)myTexture, rect, new Vector2(0.5f, 0.5f));
    //        img.texture = myTexture;
    //    }
    //}

    //public void OnClickTex()
    //{
    //    print(11);
    //    StartCoroutine("GetTexture");
    //}
}
