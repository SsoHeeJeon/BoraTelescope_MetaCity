using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HtmlAgilityPack;
using System;
using System.Text;
using UnityEngine.Networking;

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
    public Text Degree;
    public Text Date;

    public AudioSource ButtonEffect;
    public AudioClip ButtonSound;

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
    public bool WriteLogStart = false;

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

        // 시간 초기화
        touchCount = 0;
        touchfinish = false;
        UITouch = false;

        StartCoroutine(GetWeather("Seoul"));
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

        // 터치 안하는 시간을 측정하여 대기모드로 전환하기 위함
        if (Input.GetMouseButtonDown(0))        // 마우스 클릭시
        {
            touchCount = 0;
            touchfinish = true;
        }
        else if (Input.GetMouseButtonUp(0))     // 마우스 버튼에서 떼면
        {
            touchfinish = false;
        }

        // 터치를 안하고 있고 현재 씬이 대기모드가 아니라면
        // 터치 안한지 5분(데모기준) 이 되는 시간에 대기모드로 전환
        //if (touchfinish == false && SceneManager.GetActiveScene().name != "WaitingMode" && FunctionCustom.SetPayment == false)
        if (!(SceneManager.GetActiveScene().name == "WaitingMode" || SceneManager.GetActiveScene().name == "Loading"))
        {
            // 터치 안하는 시간을 측정하여 대기모드로 전환하기 위함
            if (Input.GetMouseButtonDown(0) || Input.touchCount >= 1)        // 마우스 클릭시
            {
                touchCount = 0;
                touchfinish = true;
            }
            else if (Input.GetMouseButtonUp(0) || Input.touchCount == 0)     // 마우스 버튼에서 떼면
            {
                touchfinish = false;
            }

            if (touchCount < waitingTime + 10)
            {
                touchCount += Time.deltaTime;
            }

            if ((int)touchCount >= waitingTime)
            {
                //Readpulse = false;
                touchCount = 0;
                Debug.Log("today waiting");
                Loading.nextScene = "WaitingMode";
                SceneManager.LoadScene("WaitingMode");
            }
            else if ((int)touchCount >= 60 && (int)touchCount < 61)
            {
                if (!Tip_Obj.gameObject.activeSelf)
                {
                    if (WriteLogStart == false)
                    {
                        gamemanager.WriteLog(LogSendServer.NormalLogCode.ClickHomeBtn, "Reset All Function", GetType().ToString());
                        WriteLogStart = true;
                    }
                    ClickHome();
                }
            }
        }

        WriteDegree();

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
        jaemilangmode.minimalplayback.Stop();
        if (GameManager.AnyError == false)
        {
            selfifunction.selfilightcontrol.LightOff();
        }
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

                gamemanager.WriteLog(LogSendServer.NormalLogCode.ChangeMode, "UI Setting " + UI_All.activeSelf, GetType().ToString());
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

                    WriteLog(NormalLogCode.ChangeMode, "ChangeMode : Start(" + PrevMode + " - " + "JaemilangMode)", GetType().ToString());

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
                        WriteLog(NormalLogCode.ChangeMode, "GuideMode Open", GetType().ToString());
                        GuideMode.SetActive(true);
                    }
                    else if (GuideMode.activeSelf)
                    {
                        MenuBar.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        WriteLog(NormalLogCode.ChangeMode, "GuideMode Off", GetType().ToString());
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

                        jaemilangmode.minimalplayback.Stop();

                        WriteLog(NormalLogCode.ChangeMode, "ChangeMode : Start(JaemilangMode - VisitMode)", GetType().ToString());

                        Loading.nextScene = "VisitMode";
                        SceneManager.LoadScene("Loading");
                    }
                }
                break;
            case "Language":
                LanguageBar.SetActive(true);
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
                LanguageBar.SetActive(false);

                LanguageBtn.transform.GetChild(0).gameObject.SetActive(false);
                break;
        }
    }

    string mintext;

    public void WriteDegree()
    {
        if (System.DateTime.Now.ToString("tt") == "오전")
        {
            Date.text = "am ";
            Date.text += System.DateTime.Now.ToString("hh:mm");
        }
        else if (System.DateTime.Now.ToString("tt") == "오후")
        {
            Date.text = "pm ";
            Date.text += System.DateTime.Now.ToString("hh:mm");
        }
        else
        {
            Date.text = System.DateTime.Now.ToString("tt hh:mm");
        }

        //string url = "https://search.naver.com/search.naver?sm=tab_hty.top&where=nexearch&query=%EB%AA%85%EB%8F%99+%EA%B8%B0%EC%98%A8&oquery=%EA%B8%B0%EC%98%A8&tqi=in8aclp0J1sssUUVUPNsssssse8-117799";
        //HtmlWeb web = new HtmlWeb();
        //web.OverrideEncoding = System.Text.Encoding.UTF8;
        //HtmlDocument htmlDoc = web.Load(url);

        //if (Date.text.Substring(6) != System.DateTime.Now.ToString("mm"))
        //{
        //    var CurWeather = htmlDoc.DocumentNode.SelectSingleNode("*[@id=\"main_pack\"]/section[1]/div[1]/div[2]/div[1]/div[1]/div/div[2]/div/div[1]/div[1]/div[2]/strong/text()");
        //    Degree.text = CurWeather.InnerText + "℃";
        //}
        //if (mintext != System.DateTime.Now.ToString("mm"))
        //{
        //    StartCoroutine(GetWeather("Seoul"));
            ////string url = "https://search.naver.com/search.naver?sm=tab_hty.top&where=nexearch&query=%EC%84%9C%EC%9A%B8+%EB%82%A0%EC%94%A8&oquery=%EA%B8%B0%EC%83%81%EC%B2%AD&tqi=iM1Gjwp0YidssRSoQ8Kssssst78-002538";
            //string url = "https://www.google.com/search?q=%EB%AA%85%EB%8F%99%EB%82%A0%EC%94%A8&oq=%EB%AA%85%EB%8F%99%EB%82%A0%EC%94%A8&aqs=chrome..69i57.6002j1j4&sourceid=chrome&ie=UTF-8";
            //HtmlWeb web = new HtmlWeb();
            //web.OverrideEncoding = Encoding.UTF8;
            //HtmlDocument htmlDoc = web.Load(url);
            //HtmlNode SearchCount = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"wob_tm\"]");
            //Debug.Log(SearchCount);
            ////HtmlNode SearchCount = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"main_pack\"]/section[1]/div[1]/div[2]/div[1]/div[1]/div/div[2]/div/div[1]/div[1]/div[2]/strong/text()");
            //try
            //{
            //    Degree.text = SearchCount.InnerText + "℃";
            //}
            //catch
            //{

            //}
            //mintext = System.DateTime.Now.ToString("mm");
        //}
    }

    public WeatherData weatherInfo;

    IEnumerator GetWeather(string city)
    {
        city = UnityWebRequest.EscapeURL(city);
        string url = "http://api.openweathermap.org/data/2.5/weather?q=Seoul&units=metric&appid=0bf0abd508d5b27f11cdf4e3f9d55de0";

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        string json = www.downloadHandler.text;
        json = json.Replace("\"base\":", "\"basem\":");
        weatherInfo = JsonUtility.FromJson<WeatherData>(json);

        if (weatherInfo.weather.Length > 0)
        {
            Degree.text = weatherInfo.main.temp.ToString() + "℃";
            //Degree.text = weatherInfo.weather[0].main;
        }

    }

    /// <summary>
    /// 메뉴바에서 언어선택 아이콘 선택하면 언어선택창 활성화/비활성화
    /// </summary>
    public void SelectLanguageChange()
    {
        LanguageBar.SetActive(true);
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
                btn.transform.GetChild(0).gameObject.SetActive(true);
                btn.transform.parent.GetChild(1).GetChild(0).gameObject.SetActive(false);
                break;
            case "English":
                currentLang = Language_enum.English;
                uilang.NotSelectKorea();

                btn.transform.GetChild(0).gameObject.SetActive(true);
                btn.transform.parent.GetChild(0).GetChild(0).gameObject.SetActive(false);
                break;
        }
        selfifunction.ChangeLanguage();
        WriteLog(NormalLogCode.ChangeLanguage, "ChangeLanguage : " + currentLang, GetType().ToString());

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
