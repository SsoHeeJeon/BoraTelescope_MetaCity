using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaemilangMode : MonoBehaviour
{
    public GameManager gamemanager;
    public SelfiFunction selfifunction;
    public CaptureMode capturemode;
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

        gamemanager.UISetting();
    }

    public void SelectBackground(GameObject btn)
    {
        switch (btn.name)
        {
            case "Live":
                Liveobj.SetActive(true);
                Jaemilang_background.SetActive(false);
                Graffiti_background.SetActive(false);
                break;
            case "Jaemilang":
                Liveobj.SetActive(false);
                Jaemilang_background.SetActive(true);
                Graffiti_background.SetActive(false);
                break;
            case "Graffiti":
                Liveobj.SetActive(false);
                Jaemilang_background.SetActive(false);
                Graffiti_background.SetActive(true);
                break;
        }
    }
}
