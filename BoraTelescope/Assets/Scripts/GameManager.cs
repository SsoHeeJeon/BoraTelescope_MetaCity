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
    //public UILanguage uilang;
    //public Visitmanager visitmanager;
    public SelfiFunction selfifunction;

    public GameObject UI_All;
    public GameObject MenuBar;
    public GameObject CaptureBtn;
    public GameObject LanguageBar;
    public GameObject ErrorMessage;
    public GameObject BackGround;

    public GameObject Selfi_Obj;

    public AudioSource ButtonEffect;
    public AudioClip ButtonSound;

    public static float waitingTime = 300;
    string ManagerModePassword = "025697178";
    public static string PrevMode;
    public static string MainMode;
    public static float touchCount;

    public bool touchfinish = false;
    public static bool UITouch = false;
    public static bool InternetConnectState = false;
    public static bool internetCon = false;
    public bool alreadywaitingLog = false;
    public static bool AnyError = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        InternetConnectState = false;
        ScreenCapture.startflasheffect = false;

        // 시간 초기화
        touchCount = 0;
        touchfinish = false;
        UITouch = false;
    }

    // Update is called once per frame
    void Update()
    {
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
                    loading = GameObject.FindGameObjectWithTag("Loading").GetComponent<Loading>();
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
                break;
            case "GuideMode":
                break;
            case "VisitMode":
                break;
            case "Language":
                break;
            case "Tip":
                break;
            case "Capture":
                break;
            case "Cartoon":
                break;
            case "SearchBook":
                break;
            case "Suggestion":
                break;
            case "Live":
                break;
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
}
