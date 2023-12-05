using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MetaBackGround : MonoBehaviour
{
    [XmlRoot(ElementName = "data")]
    public class Data
    {

        [XmlElement(ElementName = "imgNo")]
        public int ImgNo { get; set; }

        [XmlElement(ElementName = "userCodeNo")]
        public int UserCodeNo { get; set; }

        [XmlElement(ElementName = "position")]
        public string Position { get; set; }

        [XmlElement(ElementName = "imgPath")]
        public string ImgPath { get; set; }

        [XmlElement(ElementName = "useYn")]
        public string UseYn { get; set; }

        [XmlElement(ElementName = "insertTs")]
        public DateTime InsertTs { get; set; }

        [XmlElement(ElementName = "updateTs")]
        public DateTime UpdateTs { get; set; }
    }

    [XmlRoot(ElementName = "JSONObject")]
    public class JSONObject
    {

        [XmlElement(ElementName = "result")]
        public bool Result { get; set; }

        [XmlElement(ElementName = "data")]
        public List<Data> Data { get; set; }
    }

    public List<GameObject> MetaImgList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetTex()
    {
        for (int i = 0; i < MetaImgList.Count; i++)
        {
            MetaImgList[i].gameObject.SetActive(false);
        }
        StartCoroutine("GetTexture");
    }

    public void StopTex()
    {
        StopCoroutine("GetTexture");
    }

    IEnumerator GetTexture()
    {
        UnityWebRequest ww = UnityWebRequest.Get("https://mcity.meti.world/api/draw/recentDrawList");
        yield return ww.SendWebRequest();
        if (ww.isNetworkError || ww.isHttpError)
        {
            Debug.Log(ww.error);
            for(int i=0; i<MetaImgList.Count; i++)
            {
                MetaImgList[i].gameObject.SetActive(false);
            }
        }
        else
        {
            JSONObject Img = JsonConvert.DeserializeObject<JSONObject>(ww.downloadHandler.text);
            for (int i = 0; i < Img.Data.Count; i++)
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(Img.Data[i].ImgPath);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                    Rect rect = new Rect(0, 0, myTexture.width, myTexture.height);
                    Sprite sp = Sprite.Create((Texture2D)myTexture, rect, new Vector2(0.5f, 0.5f));

                    MetaImgList[i+1].gameObject.SetActive(true);
                    MetaImgList[i + 1].GetComponent<Image>().sprite = sp;
                    //GameObject obj = Instantiate(img.gameObject);
                    //obj.transform.parent = img.transform.parent;
                    //obj.SetActive(true);
                    //obj.GetComponent<RawImage>().texture = myTexture;
                }
            }

            for(int i=0; i<MetaImgList.Count; i++)
            {
                if (i > Img.Data.Count)
                {
                    MetaImgList[i].gameObject.SetActive(false);
                }
            }
        }



    }
}
