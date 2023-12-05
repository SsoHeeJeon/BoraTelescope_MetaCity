using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JaemilangMode : MonoBehaviour
{
    public enum MetaBackGroundState
    {
        Close,
        Open,
        Closing,
        Opening,
    }
    public MetaBackGroundState metabackgroundstate = 0;

    public enum ImageBackGroundState
    {
        Close,
        Open,
        Closing,
        Opening,
    }
    public ImageBackGroundState imagebackgroundstate = 0;

    public GameManager gamemanager;
    public SelfiFunction selfifunction;
    public CaptureMode capturemode;
    public MinimalPlayback minimalplayback;
    public GameObject CaptueObject;

    public GameObject Liveobj;
    public GameObject Jaemilang_background;
    public GameObject Graffiti_background;
    public GameObject Meta_Bakcground;

    public GameObject MetaCityBackGround;
    public GameObject ImageBackGround;

    public Sprite Tip_K;
    public Sprite Tip_E;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        selfifunction.gamemanager = gamemanager;
        capturemode.gamemanager = gamemanager;
        gamemanager.selfifunction = selfifunction;
        minimalplayback.gamemanager = gamemanager;

        minimalplayback.ReadyImg.SetActive(true);

        gamemanager.UISetting();
        gamemanager.WriteLog(LogSendServer.NormalLogCode.ChangeMode, "Start JaemilangMode", GetType().ToString());
        //autostreaming.makefile();
    }

    void Update()
    {
        switch(metabackgroundstate)
        {
            case MetaBackGroundState.Opening:
                //if(!MetaCityBackGround.activeSelf)
                //{
                //    MetaCityBackGround.SetActive(true);
                //}
                MetaCityBackGround.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(MetaCityBackGround.GetComponent<RectTransform>().anchoredPosition, new Vector3(0, -875, 0), Time.deltaTime*3);
                if(MetaCityBackGround.GetComponent<RectTransform>().anchoredPosition.y > -865)
                {
                    metabackgroundstate = MetaBackGroundState.Open;
                }
                break;
            case MetaBackGroundState.Closing:
                MetaCityBackGround.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(MetaCityBackGround.GetComponent<RectTransform>().anchoredPosition, new Vector3(0, -1075, 0), Time.deltaTime*3);
                if (MetaCityBackGround.GetComponent<RectTransform>().anchoredPosition.y < -1072)
                {
                    //MetaCityBackGround.SetActive(false);
                    metabackgroundstate = MetaBackGroundState.Close;
                }
                break;
        }

        switch (imagebackgroundstate)
        {
            case ImageBackGroundState.Opening:
                //if(!MetaCityBackGround.activeSelf)
                //{
                //    MetaCityBackGround.SetActive(true);
                //}
                ImageBackGround.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(ImageBackGround.GetComponent<RectTransform>().anchoredPosition, new Vector3(0, -875, 0), Time.deltaTime * 3);
                if (ImageBackGround.GetComponent<RectTransform>().anchoredPosition.y > -865)
                {
                    imagebackgroundstate = ImageBackGroundState.Open;
                }
                break;
            case ImageBackGroundState.Closing:
                ImageBackGround.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(ImageBackGround.GetComponent<RectTransform>().anchoredPosition, new Vector3(0, -1075, 0), Time.deltaTime * 3);
                if (ImageBackGround.GetComponent<RectTransform>().anchoredPosition.y < -1072)
                {
                    //MetaCityBackGround.SetActive(false);
                    imagebackgroundstate = ImageBackGroundState.Close;
                }
                break;
        }

    }

    public void OnClickMetaBackGroundBtn(GameObject btn)
    {
        for (int index = 0; index < btn.transform.parent.childCount; index++)
        {
            if (btn.transform.parent.GetChild(index).gameObject.name != "Gray")
            {
                btn.transform.parent.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        if (imagebackgroundstate == ImageBackGroundState.Open || imagebackgroundstate == ImageBackGroundState.Opening)
        {
            imagebackgroundstate = ImageBackGroundState.Closing;
        }


        if(metabackgroundstate == MetaBackGroundState.Open || metabackgroundstate == MetaBackGroundState.Opening) 
        {
            MetaCityBackGround.GetComponent<MetaBackGround>().StopTex();
            metabackgroundstate = MetaBackGroundState.Closing;
        }
        else if(metabackgroundstate == MetaBackGroundState.Close || metabackgroundstate == MetaBackGroundState.Closing)
        {
            MetaCityBackGround.GetComponent<MetaBackGround>().GetTex();
            btn.transform.GetChild(0).gameObject.SetActive(true);
            metabackgroundstate = MetaBackGroundState.Opening;
        }
    }

    public void OnClickImageBackGroundBtn(GameObject btn)
    {
        for (int index = 0; index < btn.transform.parent.childCount; index++)
        {
            if (btn.transform.parent.GetChild(index).gameObject.name != "Gray")
            {
                btn.transform.parent.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        if (metabackgroundstate == MetaBackGroundState.Open || metabackgroundstate == MetaBackGroundState.Opening)
        {
            metabackgroundstate= MetaBackGroundState.Closing;
        }

        if(imagebackgroundstate == ImageBackGroundState.Open || imagebackgroundstate == ImageBackGroundState.Opening)
        {
            imagebackgroundstate = ImageBackGroundState.Closing;
        }
        else if(imagebackgroundstate == ImageBackGroundState.Close || imagebackgroundstate == ImageBackGroundState.Closing)
        {
            btn.transform.GetChild(0).gameObject.SetActive(true);
            imagebackgroundstate = ImageBackGroundState.Opening;
        }
    }

    public void SelectBackground(GameObject btn)
    {
        for (int index = 0; index < ImageBackGround.transform.childCount; index++)
        {
            ImageBackGround.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        for(int index = 0; index < MetaCityBackGround.transform.childCount; index++)
        {
            MetaCityBackGround.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        switch (btn.name)
        {
            case "Live":
                metabackgroundstate = MetaBackGroundState.Closing;
                imagebackgroundstate = ImageBackGroundState.Closing;
                MetaCityBackGround.GetComponent<MetaBackGround>().StopTex();
                metabackgroundstate = MetaBackGroundState.Closing;
                Liveobj.SetActive(true);
                Jaemilang_background.SetActive(false);
                Graffiti_background.SetActive(false);
                Meta_Bakcground.SetActive(false);
                for (int index = 0; index < btn.transform.parent.childCount; index++)
                {
                    if(btn.transform.parent.GetChild(index).gameObject.name != "Gray")
                    {
                        btn.transform.parent.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
                btn.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "Jaemilang":
                MetaCityBackGround.GetComponent<MetaBackGround>().StopTex();
                metabackgroundstate = MetaBackGroundState.Closing;
                Liveobj.SetActive(false);
                Jaemilang_background.SetActive(true);
                Graffiti_background.SetActive(false);
                Meta_Bakcground.SetActive(false);
                btn.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "Graffiti":
                MetaCityBackGround.GetComponent<MetaBackGround>().StopTex();
                metabackgroundstate = MetaBackGroundState.Closing;
                Liveobj.SetActive(false);
                Jaemilang_background.SetActive(false);
                Graffiti_background.SetActive(true);
                Meta_Bakcground.SetActive(false);
                btn.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "MetaCity":
                Liveobj.SetActive(false);
                Jaemilang_background.SetActive(false);
                Graffiti_background.SetActive(false);
                Meta_Bakcground.SetActive(true);
                Meta_Bakcground.GetComponent<Image>().sprite = btn.GetComponent<Image>().sprite;
                btn.transform.GetChild(0).gameObject.SetActive(true);
                break;
        }
    }
}
