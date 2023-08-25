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
            WriteLog(NormalLogCode.StartContents, "StartContents", GetType().ToString());       // ������ ���� �α� ����

            Connect_Button();       // �ý��� ��Ʈ�ѷ� ���α׷��� �����Ͽ� �������÷��� �ҷ�����
            WriteLog(NormalLogCode.Connect_SystemControl, "Connect_SystemControl_On", GetType().ToString());        // �ҷ��� ������ �÷��� �α׷� ǥ��
            gamemanager.GetComponent<GameManager>().UISetting();       // UI ����
            //AwakeOnce = true;
        }
    }

    /// <summary>
    /// �޾ƿ� ������ ������ ���� �ش� ��ũ��Ʈ�� �̿��ؼ� ���� ���ҽ� �ҷ�����
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
}
