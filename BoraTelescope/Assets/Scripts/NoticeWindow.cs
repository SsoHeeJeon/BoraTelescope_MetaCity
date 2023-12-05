using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeWindow : MonoBehaviour
{
    public static GameManager gamemanager;
    public static NoticeWindow noticewindow;
    public static string NoticeState;
    public static Image NoticeIcon;
    public static Text NoticeText;
    public static Font KEfont;
    public static Font CJfont;

    public static GameObject ButType_1;
    public static GameObject ButType_2;

    public Sprite CameraIcon;
    public Sprite ErrorIcon;
    public Sprite CheckIcon;

    public Font Titlefont_KE;
    public Font Titlefont_CJ;
    public Font Detailfont_KE;
    public Font Detailfont_CJ;

    private void ReadytoStart()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        noticewindow = gamemanager.gameObject.GetComponent<NoticeWindow>();
        KEfont = Titlefont_KE;
        CJfont = Titlefont_CJ;
    }

    public static void NoticeWindowOpen(string Window)
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        noticewindow = gamemanager.gameObject.GetComponent<NoticeWindow>();
        if (!gamemanager.ErrorMessage.activeSelf)
        {
            gamemanager.ErrorMessage.SetActive(true);
        }

        NoticeIcon = gamemanager.ErrorMessage.transform.GetChild(0).gameObject.GetComponent<Image>();
        NoticeText = gamemanager.ErrorMessage.transform.GetChild(1).gameObject.GetComponent<Text>();
        ButType_1 = gamemanager.ErrorMessage.transform.GetChild(2).gameObject;
        ButType_2 = gamemanager.ErrorMessage.transform.GetChild(3).gameObject;

        NoticeState = Window;
        
        //switch (GameManager.currentLang)
        //{
        //    case GameManager.Language_enum.Korea:
        //        NoticeText.font = KEfont;
        //        break;
        //    case GameManager.Language_enum.English:
        //        NoticeText.font = KEfont;
        //        break;
        //    case GameManager.Language_enum.Chinese:
        //        NoticeText.font = CJfont;
        //        break;
        //    case GameManager.Language_enum.Japanese:
        //        NoticeText.font = CJfont;
        //        break;
        //}

        noticewindow.Setanything();
        Debug.Log(ButType_1 + " / " + ButType_2);
    }

    public void Setanything()
    {
        switch (NoticeState)
        {
            case "ErrorMessage":
                switch (GameManager.currentLang)
                {
                    case GameManager.Language_enum.Korea:
                        NoticeText.text = "죄송합니다.\r\n현재 해당 모드를 이용하실 수 없습니다.";
                        NoticeText.font = Titlefont_KE;
                        break;
                    case GameManager.Language_enum.English:
                        NoticeText.text = "I'm sorry.\r\nYou can't use mode.";
                        NoticeText.font = Titlefont_KE;
                        break;
                    case GameManager.Language_enum.Chinese:
                        NoticeText.text = "绐您添麻烦子, 真抱歉。 目前不能使用。";
                        NoticeText.font = Titlefont_CJ;
                        break;
                    case GameManager.Language_enum.Japanese:
                        NoticeText.text = "すみません。 現在のモードは利用できません。";
                        NoticeText.font = Titlefont_CJ;
                        break;
                }
                ButType_1.SetActive(true);
                ButType_2.SetActive(false);
                NoticeIcon.sprite = ErrorIcon;
                break;
            case "ErrorInternet":
                switch (GameManager.currentLang)
                {
                    case GameManager.Language_enum.Korea:
                        NoticeText.text = "죄송합니다.\r\n현재 인터넷이 불안정하여 QR코드 제공이 어렵습니다.";
                        NoticeText.font = Titlefont_KE;
                        break;
                    case GameManager.Language_enum.English:
                        NoticeText.text = "I'm sorry.\r\nYou can't use mode.";
                        NoticeText.font = Titlefont_KE;
                        break;
                }
                ButType_1.SetActive(true);
                ButType_2.SetActive(false);
                NoticeIcon.sprite = ErrorIcon;
                gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_InternetConnect, "InternetConnect Fail", GetType().ToString());
                break;
            case "VisitClickCap":
                switch (GameManager.currentLang)
                {
                    case GameManager.Language_enum.Korea:
                        NoticeText.text = "사진촬영 기능이 제공되지 않는 모드입니다.\r\n(재미랑 모드 내에서 사용가능합니다.)";
                        NoticeText.font = Titlefont_KE;
                        break;
                    case GameManager.Language_enum.English:
                        NoticeText.text = "Mode Not Supported\r\n(Use only for Jaemilang Mode)";
                        NoticeText.font = Titlefont_KE;
                        break;
                }
                ButType_1.SetActive(true);
                ButType_2.SetActive(false);
                NoticeIcon.sprite = CheckIcon;
                break;
            case "VisitCancel":
                switch (GameManager.currentLang)
                {
                    case GameManager.Language_enum.Korea:
                        NoticeText.text = "작성을 취소하시겠습니까?\r\n작성중인 내용은 삭제됩니다.";
                        NoticeText.font = Titlefont_KE;
                        break;
                    case GameManager.Language_enum.English:
                        NoticeText.text = "Are you sure you want to cancel?\r\nType will be deleted.";
                        NoticeText.font = Titlefont_KE;
                        break;
                }
                ButType_1.SetActive(false);
                ButType_2.SetActive(true);
                NoticeIcon.sprite = CheckIcon;
                break;
            case "StopSelfi":
                switch (GameManager.currentLang)
                {
                    case GameManager.Language_enum.Korea:
                        NoticeText.text = "사진찍기를 그만하시겠습니까?";
                        break;
                    case GameManager.Language_enum.English:
                        NoticeText.text = "Would you like to stop taking pictures?";
                        break;
                }
                ButType_1.SetActive(false);
                ButType_2.SetActive(true);
                NoticeIcon.sprite = CheckIcon;
                break;
            case "StopSelfiCustom":
                switch (GameManager.currentLang)
                {
                    case GameManager.Language_enum.Korea:
                        NoticeText.text = "사진꾸미기를 그만하시겠습니까?\r\n해당 사진은 삭제됩니다.";
                        break;
                    case GameManager.Language_enum.English:
                        NoticeText.text = "Do you want to stop decorating pictures?\r\nThe photo will be deleted.";
                        break;
                }
                ButType_1.SetActive(false);
                ButType_2.SetActive(true);
                NoticeIcon.sprite = CheckIcon;
                break;
            case "DontSaveSelfi":
                switch (GameManager.currentLang)
                {
                    case GameManager.Language_enum.Korea:
                        NoticeText.text = "셀피모드에서 나가시겠습니까?\r\n해당 사진은 삭제됩니다.";
                        break;
                    case GameManager.Language_enum.English:
                        NoticeText.text = "Would you like to leave selfie mode?\r\nThe photo will be deleted.";
                        break;
                }
                ButType_1.SetActive(false);
                ButType_2.SetActive(true);
                NoticeIcon.sprite = CheckIcon;
                break;
            case "ResetSelfiPhoto":
                switch (GameManager.currentLang)
                {
                    case GameManager.Language_enum.Korea:
                        NoticeText.text = "사진을 다시 촬영하시겠습니까?";
                        break;
                    case GameManager.Language_enum.English:
                        NoticeText.text = "Would you like to take the picture again?";
                        break;
                }
                ButType_1.SetActive(false);
                ButType_2.SetActive(true);
                NoticeIcon.sprite = CameraIcon;
                break;
            case "ErrorLight":
                switch (GameManager.currentLang)
                {
                    case GameManager.Language_enum.Korea:
                        NoticeText.text = "죄송합니다. 현재 조명 사용이 어렵습니다.";
                        break;
                    case GameManager.Language_enum.English:
                        NoticeText.text = "I'm sorry, but the lighting is currently difficult to use.";
                        break;
                }
                ButType_1.SetActive(true);
                ButType_2.SetActive(false);
                NoticeIcon.sprite = ErrorIcon;
                break;
        }

        if(GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            ButType_1.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
            ButType_2.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
            ButType_2.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
        } else if(GameManager.currentLang != GameManager.Language_enum.Korea)
        {
            ButType_1.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
            ButType_2.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
            ButType_2.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public static void NoticeYes()
    {
        switch (NoticeState)
        {
            case "ErrorMessage":
                break;
            case "ErrorInternet":
                gamemanager.jaemilangmode.capturemode.CaptureEndCamera();
                break;
            case "SeasonWaiting":
                break;
            case "See360View":
                break;
            case "ChangeOperation":

                break;
            case "VisitClickCap":
                
                break;
            case "VisitCancel":
                gamemanager.visitmanager.RealOut();
                gamemanager.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case "StopSelfi":
                gamemanager.jaemilangmode.selfifunction.FinishSelfi();
                gamemanager.CaptureBtn.transform.GetChild(0).gameObject.SetActive(false);

                if(GameManager.MoveVisit == true)
                {
                    gamemanager.Menu(gamemanager.MenuBar.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject);
                } else if(GameManager.MoveVisit == false)
                {
                    gamemanager.MenuBar.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                }
                gamemanager.jaemilangmode.Jaemilang_background.SetActive(false);
                gamemanager.jaemilangmode.Graffiti_background.SetActive(false);
                gamemanager.jaemilangmode.Meta_Bakcground.SetActive(false);
                break;
            case "StopSelfiCustom":
                gamemanager.jaemilangmode.selfifunction.FinishSelfi();
                gamemanager.CaptureBtn.transform.GetChild(0).gameObject.SetActive(false);

                if (GameManager.MoveVisit == true)
                {
                    gamemanager.Menu(gamemanager.MenuBar.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject);
                }
                else if (GameManager.MoveVisit == false)
                {
                    gamemanager.MenuBar.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                }
                break;
            case "DontSaveSelfi":
                gamemanager.jaemilangmode.selfifunction.FinishSelfi();
                gamemanager.CaptureBtn.transform.GetChild(0).gameObject.SetActive(false);
                if (GameManager.MoveVisit == true)
                {
                    gamemanager.Menu(gamemanager.MenuBar.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject);
                }
                else if (GameManager.MoveVisit == false)
                {
                    gamemanager.MenuBar.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                }
                break;
            case "ResetSelfiPhoto":
                gamemanager.jaemilangmode.selfifunction.StartSelfi();
                break;
        }
        gamemanager.ErrorMessage.SetActive(false);
        gamemanager.ButtonClickSound();
    }

    public static void NoticeNo()
    {
        switch (NoticeState)
        {
            case "ResetSelfiPhoto":
                gamemanager.jaemilangmode.selfifunction.TakePhoto();
                break;
        }
        gamemanager.ErrorMessage.SetActive(false);
        gamemanager.ButtonClickSound();
    }
}
