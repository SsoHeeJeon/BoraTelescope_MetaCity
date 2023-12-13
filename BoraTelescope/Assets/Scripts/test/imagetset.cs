using Amazon.Organizations.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class imagetset : MonoBehaviour
{
    [XmlRoot(ElementName = "data")]
    public class Data
    {

        [XmlElement(ElementName = "relayCartoonPageNo")]
        public int RelayCartoonPageNo { get; set; }

        [XmlElement(ElementName = "cartoonPageImgPath")]
        public string CartoonPageImgPath { get; set; }

        [XmlElement(ElementName = "delYn")]
        public string DelYn { get; set; }

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


    // Start is called before the first frame update
    void Start()
    {
        img.gameObject.SetActive(false);
        StartCoroutine("GetTexture");
    }

    // Update is called once per frame
    void Update()
    {

    }

    [SerializeField] RawImage img;
    IEnumerator GetTexture()
    {
        print(111);
        UnityWebRequest ww = UnityWebRequest.Get("https://mcity.meti.world/api/draw/recentDrawList");
        yield return ww.SendWebRequest();
        if (ww.isNetworkError || ww.isHttpError)
        {
            print(1);
            Debug.Log(ww.error);
        }
        else
        {
            JSONObject Img = JsonConvert.DeserializeObject<JSONObject>(ww.downloadHandler.text);
            print(333);
            for(int i=0; i< Img.Data.Count; i++)
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(Img.Data[i].CartoonPageImgPath);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    print(2);
                    Debug.Log(www.error);
                }
                else
                {
                    Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                    Rect rect = new Rect(0, 0, myTexture.width, myTexture.height);
                    Sprite sp = Sprite.Create((Texture2D)myTexture, rect, new Vector2(0.5f, 0.5f));

                    GameObject obj = Instantiate(img.gameObject);
                    obj.transform.parent = img.transform.parent;
                    obj.SetActive(true);
                    obj.GetComponent<RawImage>().texture = myTexture;
                }
            }
        }



    }
}
