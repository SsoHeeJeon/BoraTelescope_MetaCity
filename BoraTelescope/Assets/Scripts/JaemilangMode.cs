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

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        selfifunction.gamemanager = gamemanager;
        capturemode.gamemanager = gamemanager;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectBackground(GameObject btn)
    {
        switch (btn.name)
        {
            case "Live":
                break;
            case "Jaemilang":
                break;
            case "Graffiti":
                break;
        }
    }
}
