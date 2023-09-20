using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaemilangMode : MonoBehaviour
{
    public GameManager gamemanager;
    public SelfiFunction selfifunction;
    public CaptureMode capturemode;
    public MinimalPlayback minimalplayback;
    public GameObject CaptueObject;

    public GameObject Liveobj;
    public GameObject Jaemilang_background;
    public GameObject Graffiti_background;

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

    public void SelectBackground(GameObject btn)
    {
        switch (btn.name)
        {
            case "Live":
                Liveobj.SetActive(true);
                Jaemilang_background.SetActive(false);
                Graffiti_background.SetActive(false);
                for (int index = 0; index < btn.transform.parent.childCount; index++)
                {
                    btn.transform.parent.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                btn.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "Jaemilang":
                Liveobj.SetActive(false);
                Jaemilang_background.SetActive(true);
                Graffiti_background.SetActive(false);
                for (int index = 0; index < btn.transform.parent.childCount; index++)
                {
                    btn.transform.parent.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                btn.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "Graffiti":
                Liveobj.SetActive(false);
                Jaemilang_background.SetActive(false);
                Graffiti_background.SetActive(true);
                for (int index = 0; index < btn.transform.parent.childCount; index++)
                {
                    btn.transform.parent.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                btn.transform.GetChild(0).gameObject.SetActive(true);
                break;
        }
    }
}
