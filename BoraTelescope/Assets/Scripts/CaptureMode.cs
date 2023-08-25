using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CaptureMode : ScreenCapture
{
    public GameObject CaptueObject;
    public GameObject BackGround;

    public static Vector3 originPos;

    public static bool CheckStart = false;
    
    public void CaptureCamera()
    {
        BackGround.SetActive(true);
        GameManager.InternetConnectState = true;      // 캡처버튼 선택했을 때
        CheckStart = true;
    }

    public void CheckInternet()
    {
        if (GameManager.internetCon == true)
        {
            CaptueObject.gameObject.SetActive(true);

            if (SelfiFunction.selfimode == false)
            {
                customMark.gameObject.SetActive(true);

                SetMark();
                FlashEffect();
            } else if(SelfiFunction.selfimode == true)
            {
                if(SelfiFunction.selfistate == SelfiFunction.SelfiState.Download)
                {
                    customMark.gameObject.SetActive(true);
                    SetMark();
                    WaitStartCap();
                } else if(SelfiFunction.selfistate == SelfiFunction.SelfiState.Take)
                {
                    GameManager.InternetConnectState = false;
                    FlashEffect();
                }
            }
            BackGround.SetActive(true);
        }
        else if (GameManager.internetCon == false)
        {
            BackGround.SetActive(false);
            NoticeWindow.NoticeWindowOpen("ErrorInternet");
            //gamemanager.ErrorMessage.SetActive(true);
            CaptureEndCamera();
        }
    }

    public void FlashEffect()
    {
        startflasheffect = true;
        flasheffect.SetActive(true);
        flasheffect.GetComponent<Image>().color = flashColor;
    }

    public void CaptureEndCamera()
    {
        customMark.transform.parent = flasheffect.transform.parent.gameObject.transform;

        if (SelfiFunction.selfimode == true && gamemanager.selfifunction.Selfi_Obj.activeSelf)
        {
            gamemanager.selfifunction.FinishSelfi();
        }

        gamemanager.CaptureBtn.transform.GetChild(0).gameObject.SetActive(false);
        customMark.transform.localPosition = originPos;
        customMark.gameObject.SetActive(false);

        QRCodeImage.texture = null;
        gamemanager.jaemilangmode.capturemode.BackGround.SetActive(false);
        CaptueObject.gameObject.SetActive(false);
        GameManager.internetCon = false;
        GameManager.InternetConnectState = false;
    }

    public void WaitStartCap()
    {
        Invoke("waitcapture", 0.1f);
        //waitcapture();
    }

    public void SetMark()
    {
        customMark.transform.GetChild(0).gameObject.GetComponent<Text>().text = DateTime.Now.ToString("yyyy.MM.dd HH:mm");

        originPos = customMark.transform.localPosition;

        customMark.transform.parent = gamemanager.selfifunction.PhotoOrigin.transform;
        customMark.transform.localPosition = new Vector3(0, 0,0);
        customMark.transform.localScale = new Vector3(1,1,1);
    }

    public void waitcapture()
    {
        ScreenCapture.startflasheffect = false;
        ClickScreenShot();
        ReadyToCapture();
        gamemanager.ButtonClickSound();
    }
}
