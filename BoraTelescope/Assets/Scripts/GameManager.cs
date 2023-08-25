using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : ContentsInfo
{
    public enum Language_enum
    {
        Korea, English, Chinese, Japanese
    }
    public static Language_enum currentLang;

    public Loading loading;
    public WaitingMode waitingmode;
    public JaemilangMode jaemilangmode;
    public CartoonMode cartoonmode;
    public UILanguage uilang;
    public Visitmanager visitmanager;
    public SelfiFunction selfifunction;

    public GameObject UI_All;
    public GameObject MenuBar;
    public GameObject CaptureBtn;
    public GameObject LanguageBtn;
    public GameObject LanguageBar;
    public RectTransform LangRect;
    public Image LangChildImg;
    public GameObject Tip_Obj;
    public GameObject ErrorMessage;
    public GameObject GuideMode;

    public GameObject Selfi_Obj;

    public AudioSource ButtonEffect;
    public AudioClip ButtonSound;

    public float langnavi_t;
    public bool movelangNavi = false;
    public bool langNaviOn = false;

    public static float waitingTime = 300;
    string ManagerModePassword = "025697178";
    public static string PrevMode;
    public static string MainMode;
    public static float touchCount;
    public static float barOpen = 472f;
    public static float barClose = 60f;

    public bool touchfinish = false;
    public static bool UITouch = false;
    public static bool InternetConnectState = false;
    public static bool internetCon = false;
    public bool alreadywaitingLog = false;
    public static bool AnyError = false;
    public static bool MoveVisit = false;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager");
        DontDestroyOnLoad(GM);
        UISetting();

        InternetConnectState = false;
        ScreenCapture.startflasheffect = false;

        if (ContentsName == "Jaemilang")
        {
            LanguageBtn = MenuBar.transform.GetChild(0).gameObject.GetComponent<UILanguage>().Language_Btn.gameObject;
        }
        else if (ContentsName == "Cartoon")
        {
            LanguageBtn = MenuBar.transform.GetChild(1).gameObject.GetComponent<UILanguage>().Language_Btn.gameObject;
        }

        // 언어선택창 닫아놓기(로딩화면에서 안보임.)
        LangRect.sizeDelta = new Vector2(barClose, 1080);
        LangChildImg.fillAmount = 0;
        LanguageBar.transform.GetChild(0).gameObject.SetActive(false);
        langNaviOn = false;
        movelangNavi = false;

        // 시간 초기화
        touchCount = 0;
        touchfinish = false;
        UITouch = false;
        langnavi_t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (InternetConnectState == true)
        {
            internetCon = gamemanager.IsInternetConnected();

            if (CaptureMode.CheckStart == true)
            {
                jaemilangmode.capturemode.CheckInternet();
                CaptureMode.CheckStart = false;
            }
        }

        langnavi_t += Time.deltaTime * 0.1f;
        if (movelangNavi == true)
        {
            SelectLanguageChange();
        }

        if (Input.GetKeyDown("a"))
        {
            OnApplicationQuit();
        }
    }
    /// <summary>
    /// 콘텐츠 종료할때
    /// </summary>
    private void OnApplicationQuit()
    {
        AwakeOnce = false;
        WriteLog(NormalLogCode.Connect_SystemControl, "Connect_SystemControl_Off", GetType().ToString());
        selfifunction.selfilightcontrol.LightOff();
        Disconnect_Button();
    }

    public void UISetting()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "JaemilangMode":
                jaemilangmode = GameObject.Find("JaemilangMode").GetComponent<JaemilangMode>();
                UI_All.SetActive(true);
                MenuBar.transform.GetChild(0).gameObject.SetActive(true);
                MenuBar.transform.GetChild(1).gameObject.SetActive(false);

                jaemilangmode.selfifunction.Selfi_Obj.SetActive(false);
                break;
            case "CartoonMode":
                cartoonmode = GameObject.Find("CartoonMode").GetComponent<CartoonMode>();
                MenuBar.transform.GetChild(0).gameObject.SetActive(false);
                MenuBar.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case "Loading":
                if (loading == null)
                {
                    loading = GameObject.Find("Loading").GetComponent<Loading>();
                }
                UI_All.gameObject.SetActive(false);
                break;
            case "WaitingMode":
                if (AnyError == false)
                {
                    UI_All.gameObject.SetActive(false);
                    ErrorMessage.SetActive(false);
                }
                else if (AnyError == true)
                {
                    UI_All.gameObject.SetActive(true);
                    for (int index = 0; index < UI_All.transform.childCount; index++)
                    {
                        UI_All.transform.GetChild(index).gameObject.SetActive(false);
                    }
                    MenuBar.gameObject.SetActive(true);
                    MenuBar.gameObject.GetComponent<Image>().enabled = false;
                    for (int index = 0; index < MenuBar.gameObject.transform.childCount; index++)
                    {
                        MenuBar.transform.GetChild(index).gameObject.SetActive(false);
                    }
                    NoticeWindow.NoticeWindowOpen("ErrorMessage");
                }
                break;
            case "VisitMode":
                break;
        }
    }

    public void Menu(GameObject Btn)
    {
        switch (Btn.name)
        {
            case "JaemilangMode":
                if (SceneManager.GetActiveScene().name == "JaemilangMode")
                {
                    if (selfifunction.Selfi_Obj.activeSelf)
                    {
                        MoveVisit = false;

                        if (SelfiFunction.selfistate == SelfiFunction.SelfiState.None)
                        {
                            NoticeWindow.NoticeWindowOpen("StopSelfi");
                        }
                        else if (SelfiFunction.selfistate == SelfiFunction.SelfiState.Take)
                        {
                            if (!selfifunction.ConfirmUI.activeSelf)
                            {
                                NoticeWindow.NoticeWindowOpen("StopSelfiCustom");
                            }
                            else if (selfifunction.ConfirmUI.activeSelf)
                            {
                                NoticeWindow.NoticeWindowOpen("StopSelfi");
                            }
                        }
                        else if (SelfiFunction.selfistate == SelfiFunction.SelfiState.Download)
                        {
                            NoticeWindow.NoticeWindowOpen("DontSaveSelfi");
                        }
                    }
                }
                else if (SceneManager.GetActiveScene().name != "JaemilangMode")
                {
                    for (int index = 0; index < MenuBar.transform.GetChild(0).gameObject.transform.childCount; index++)
                    {
                        MenuBar.transform.GetChild(0).gameObject.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    MenuBar.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);

                    Loading.nextScene = "JaemilangMode";
                    SceneManager.LoadScene("Loading");
                }
                break;
            case "GuideMode":
                if (SceneManager.GetActiveScene().name == "JaemilangMode")
                {
                    if (!GuideMode.activeSelf)
                    {
                        MenuBar.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        GuideMode.SetActive(true);
                    }
                    else if (GuideMode.activeSelf)
                    {
                        MenuBar.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        GuideMode.SetActive(false);
                    }
                }
                break;
            case "VisitMode":
                if (SceneManager.GetActiveScene().name == "JaemilangMode")
                {
                    MoveVisit = true;
                    if (selfifunction.Selfi_Obj.activeSelf)
                    {
                        if (SelfiFunction.selfistate == SelfiFunction.SelfiState.None)
                        {
                            NoticeWindow.NoticeWindowOpen("StopSelfi");
                        }
                        else if (SelfiFunction.selfistate == SelfiFunction.SelfiState.Take)
                        {
                            if (!selfifunction.ConfirmUI.activeSelf)
                            {
                                NoticeWindow.NoticeWindowOpen("StopSelfiCustom");
                            }
                            else if (selfifunction.ConfirmUI.activeSelf)
                            {
                                NoticeWindow.NoticeWindowOpen("StopSelfi");
                            }
                        }
                        else if (SelfiFunction.selfistate == SelfiFunction.SelfiState.Download)
                        {
                            NoticeWindow.NoticeWindowOpen("DontSaveSelfi");
                        }
                    }
                    else if (!selfifunction.Selfi_Obj.activeSelf)
                    {
                        for (int index = 0; index < MenuBar.transform.GetChild(0).gameObject.transform.childCount; index++)
                        {
                            MenuBar.transform.GetChild(0).gameObject.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        }
                        MenuBar.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(true);

                        Loading.nextScene = "VisitMode";
                        SceneManager.LoadScene("Loading");
                    }
                }
                break;
            case "Language":
                langnavi_t = 0;
                if (LangRect.sizeDelta.x > barClose)        // 언어선택 비활성화
                {
                    LanguageBtn.transform.GetChild(0).gameObject.SetActive(false);
                    //langnavi_t = 0;
                    langNaviOn = true;
                    movelangNavi = true;
                }
                else if (LangRect.sizeDelta.x < barOpen)      // 언어선택 활성화
                {
                    LanguageBtn.transform.GetChild(0).gameObject.SetActive(true);
                    //langnavi_t = 0;
                    movelangNavi = true;
                    langNaviOn = false;
                }
                break;
            case "Tip":
                if (!Tip_Obj.activeSelf)
                {
                    Tip_Obj.SetActive(true);
                }
                else if (Tip_Obj.activeSelf)
                {
                    Tip_Obj.SetActive(false);
                }
                break;
            case "Capture":
                if (SceneManager.GetActiveScene().name == "JaemilangMode")
                {
                    MoveVisit = false;

                    if (SelfiFunction.selfistate == SelfiFunction.SelfiState.None)
                    {
                        if (!selfifunction.Selfi_Obj.activeSelf)
                        {
                            for (int index = 0; index < MenuBar.transform.GetChild(0).gameObject.transform.childCount; index++)
                            {
                                MenuBar.transform.GetChild(0).gameObject.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                            }
                            MenuBar.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true);

                            if (GuideMode.activeSelf)
                            {
                                GuideMode.SetActive(false);
                            }

                            jaemilangmode.selfifunction.Selfi_Obj.SetActive(true);
                            jaemilangmode.selfifunction.StartSelfi();
                        }
                        else if (selfifunction.Selfi_Obj.activeSelf)
                        {
                            NoticeWindow.NoticeWindowOpen("StopSelfi");
                        }
                    }
                    else if (SelfiFunction.selfistate == SelfiFunction.SelfiState.Take)
                    {
                        if (!selfifunction.ConfirmUI.activeSelf)
                        {
                            NoticeWindow.NoticeWindowOpen("StopSelfiCustom");
                        }
                        else if (selfifunction.ConfirmUI.activeSelf)
                        {
                            NoticeWindow.NoticeWindowOpen("StopSelfi");
                        }
                    }
                    else if (SelfiFunction.selfistate == SelfiFunction.SelfiState.Download)
                    {
                        NoticeWindow.NoticeWindowOpen("DontSaveSelfi");
                    }
                }
                else if (SceneManager.GetActiveScene().name != "JaemilangMode")
                {
                    if (SceneManager.GetActiveScene().name == "VisitMode")
                    {
                        NoticeWindow.NoticeWindowOpen("VisitClickCap");
                    }
                }
                break;
            case "Cartoon":
                Loading.nextScene = "CartoonMode";
                SceneManager.LoadScene("Loading");
                break;
            case "SearchBook":
                break;
            case "Suggestion":
                break;
            case "Live":
                break;
            case "LangNavi_Close":
                langnavi_t = 0;
                movelangNavi = true;

                LanguageBtn.transform.GetChild(0).gameObject.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// 메뉴바에서 언어선택 아이콘 선택하면 언어선택창 활성화/비활성화
    /// </summary>
    public void SelectLanguageChange()
    {
        if (langNaviOn == true)     // 언어선택 창 비활성화
        {
            // 언어선택 창 비활성화 진행
            if (LangRect.sizeDelta.x > barClose)
            {
                LangRect.sizeDelta = Vector2.Lerp(LangRect.sizeDelta, new Vector2(barClose - 5f, 1080), langnavi_t);
                LangChildImg.fillAmount -= 0.5f * langnavi_t;
            }
            else if (LangRect.sizeDelta.x <= barClose)
            {
                LangRect.sizeDelta = new Vector2(barClose, 1080);
                LangChildImg.fillAmount = 0;
                LanguageBar.transform.GetChild(0).gameObject.SetActive(false);
                LanguageBtn.transform.GetChild(0).gameObject.SetActive(false);
                langNaviOn = false;
                movelangNavi = false;
            }
        }
        else if (langNaviOn == false)       // 언어선택 활성화
        {
            LanguageBar.transform.GetChild(0).gameObject.SetActive(true);       // 언어선택 창 활성화
            if (LangRect.sizeDelta.x < barOpen)
            {
                LangRect.sizeDelta = Vector2.Lerp(LangRect.sizeDelta, new Vector2(barOpen + 5f, 1080), langnavi_t);
                LangChildImg.fillAmount += 0.5f * langnavi_t;
            }
            else if (LangRect.sizeDelta.x >= barOpen)
            {
                //LanguageBar.gameObject.SetActive(true);
                LanguageBar.transform.GetChild(0).gameObject.SetActive(true);
                LangRect.sizeDelta = new Vector2(barOpen, 1080);
                LangChildImg.fillAmount = 1;
                langNaviOn = true;
                movelangNavi = false;
            }
        }
    }

    /// <summary>
    /// 변경할 언어선택
    /// </summary>
    /// <param name="btn"></param>
    public void ChangeLanguage(GameObject btn)
    {
        switch (btn.name)
        {
            case "Korea":
                currentLang = Language_enum.Korea;
                uilang.SelectKorea();
                Tip_Obj.GetComponent<Image>().sprite = jaemilangmode.Tip_K;
                break;
            case "English":
                currentLang = Language_enum.English;
                uilang.NotSelectKorea();
                Tip_Obj.GetComponent<Image>().sprite = jaemilangmode.Tip_E;
                break;
        }

        WriteLog(NormalLogCode.ChangeLanguage, "ChangeLanguage : " + currentLang, GetType().ToString());

        langnavi_t = 0;
        langNaviOn = true;
        movelangNavi = true;
        LanguageBtn.transform.GetChild(0).gameObject.SetActive(false);

        if (Tip_Obj.activeSelf)
        {
            TipOpen();
        }
    }

    public void TipOpen()
    {
        if (SceneManager.GetActiveScene().name == "JaemilangMode")
        {
            if (GuideMode.activeSelf)
            {
                GuideMode.SetActive(false);
            }

            if (jaemilangmode.selfifunction.Selfi_Obj.activeSelf)
            {
                jaemilangmode.selfifunction.FinishSelfi();
            }
        }
    }

    /// <summary>
    /// 버튼 클릭하면 효과음 재생
    /// </summary>
    public void ButtonClickSound()
    {
        ButtonEffect.clip = ButtonSound;
        ButtonEffect.Play();
    }

    public void ClickHome()
    {
        if (ContentsName == "Jaemilang")
        {
            if (SceneManager.GetActiveScene().name == "JaemilangMode")
            {
                if (GuideMode.activeSelf)
                {
                    GuideMode.SetActive(false);
                }

                if (selfifunction.Selfi_Obj.activeSelf)
                {
                    MoveVisit = false;

                    if (SelfiFunction.selfistate == SelfiFunction.SelfiState.None)
                    {
                        NoticeWindow.NoticeWindowOpen("StopSelfi");
                    }
                    else if (SelfiFunction.selfistate == SelfiFunction.SelfiState.Take)
                    {
                        if (!selfifunction.ConfirmUI.activeSelf)
                        {
                            NoticeWindow.NoticeWindowOpen("StopSelfiCustom");
                        }
                        else if (selfifunction.ConfirmUI.activeSelf)
                        {
                            NoticeWindow.NoticeWindowOpen("StopSelfi");
                        }
                    }
                    else if (SelfiFunction.selfistate == SelfiFunction.SelfiState.Download)
                    {
                        NoticeWindow.NoticeWindowOpen("DontSaveSelfi");
                    }
                }
            }
            else if (SceneManager.GetActiveScene().name == "VisitMode")
            {
                visitmanager.OnCLickVisitBtn();
            }
        }
        else if (ContentsName == "Cartoon")
        {

        }
    }
}
