using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContentsInfo : LogSendServer
{
    public static string ContentsName;

    public GameObject GM;
    public GameManager gamemanager;
    public string[] WaitingVideo_path;
    public static bool[] ModeActive = new bool[3];

    public static bool AwakeOnce = false;

    private void Awake()
    {
        if(AwakeOnce == false)
        {
            //ContentsName = "Cartoon";
            ContentsName = "Jaemilang";

            TelescopeInfo();
            WriteLog(NormalLogCode.StartContents, "StartContents", GetType().ToString());       // 콘텐츠 시작 로그 생성

            Connect_Button();       // 시스템 컨트롤러 프로그램에 접속하여 모드상태플래그 불러오기
            WriteLog(NormalLogCode.Connect_SystemControl, "Connect_SystemControl_On", GetType().ToString());        // 불러온 모드상태 플래그 로그로 표현
            gamemanager.GetComponent<GameManager>().UISetting();       // UI 셋팅
            AwakeOnce = true;
        }
    }

    /// <summary>
    /// 받아온 콘텐츠 정보에 따라 해당 스크립트를 이용해서 각종 리소스 불러오기
    /// </summary>
    /// <param name="Telescope"></param>
    public void TelescopeInfo()
    {
        switch (ContentsName)
        {
            case "Cartoon":
                //BasicLabel.LoadLabelInfo();

                WriteLog(LogSendServer.NormalLogCode.Load_ResourceFile, "Load_ResourceFile", GetType().ToString());
                GameManager.MainMode = "CartoonMode";
                gamemanager.uilang = gamemanager.MenuBar.transform.GetChild(1).gameObject.GetComponent<UILanguage>();
                //ModeActive = new bool[BasicLabel.ModeActive.Length];
                break;
            case "Jaemilang":
                //ApsanLabel.LoadLabelInfo();
                Readfile();
                WriteLog(LogSendServer.NormalLogCode.Load_ResourceFile, "Load_ResourceFile", GetType().ToString());
                GameManager.MainMode = "JaemilangMode";
                gamemanager.uilang = gamemanager.MenuBar.transform.GetChild(0).gameObject.GetComponent<UILanguage>();
                //ModeActive = new bool[ApsanLabel.ModeActive.Length];
                break;
        }
        WaitingVideo_path = Directory.GetFiles(Application.dataPath + "/Resources/Video", "*.mp4");
        //gamemanager.GetComponent<ReadJson>().Readfile();

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            for (int index = 0; index < ModeActive.Length; index++)
            {
                ModeActive[index] = true;
            }
        }

        GameManager.currentLang = GameManager.Language_enum.Korea;
        //gamemanager.Tip_Obj.GetComponent<Image>().sprite = label_open.Tip_K;
        //label_open.SelectCategortButton(label_open.CategoryContent.transform.GetChild(0).gameObject);
    }

    string Allstr_rec;
    string[] GOTEXT_arr;
    string GetText;

    public void Readfile()
    {
        if (File.Exists(Application.dataPath + "/StreamingURL.json"))
        {
            Allstr_rec = File.ReadAllText(Application.dataPath + "/StreamingURL.json");
            
            if (Allstr_rec.Contains("}"))
            {
                GetText = Allstr_rec.Substring(1);
                MinimalPlayback.path = GetText.Replace("}", string.Empty);
            }
        }
        else
        {
            gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.UnLoad_Jsonfile, "UnLoad_Jsonfile", GetType().ToString());
            GameManager.AnyError = true;
            /// 에러로그만 보내고 정상작동할지 아니면 에러로그 보내고 ClearMode로 전환해서 할지
        }
    }
}
