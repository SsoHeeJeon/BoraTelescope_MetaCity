using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.Assertions.Must;

public class WaitingMode : MonoBehaviour
{
    public GameManager gamemanager;
    public VideoPlayer BackGround_Video;
    private int videonum;
    private bool SeeVideo = false;
    public Text DateT;
    public Text TimeT;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gamemanager.UISetting();
        gamemanager.alreadywaitingLog = false;
        gamemanager.WriteLog(LogSendServer.NormalLogCode.ChangeMode, "ChangeMode : Finish(" + GameManager.PrevMode + " - " + "WaitingMode)", GetType().ToString());
        GameManager.PrevMode = "WaitingMode";
    }

    // Update is called once per frame
    void Update()
    {
        DateT.text = System.DateTime.Now.ToString("yyyy.MM.dd ");
        switch (System.DateTime.Now.ToString("ddd"))
        {
            /*
            case "월":
                DateT.text += "Mon";
                break;
            case "화":
                DateT.text += "Tue";
                break;
            case "수":
                DateT.text += "Wed";
                break;
            case "목":
                DateT.text += "Thu";
                break;
            case "금":
                DateT.text += "Fri";
                break;
            case "토":
                DateT.text += "Sat";
                break;
            case "일":
                DateT.text += "Sun";
                break;*/
            default:
                DateT.text += System.DateTime.Now.ToString("ddd");
                break;
        }

        TimeT.text = System.DateTime.Now.ToString("HH:mm");

        if (BackGround_Video.isPlaying == false && SeeVideo == false)
        {
            //LoadViedo();
            SeeVideo = true;
        }
    }

    public void PlayButtonClick()
    {
        gamemanager.ButtonClickSound();
    }

    public void LoadViedo()
    {
        if (BackGround_Video.isPlaying == false && gamemanager.WaitingVideo_path.Length != 0)
        {
            BackGround_Video.source = VideoSource.Url;
            if (BackGround_Video.url == "")
            {
                videonum = 0;
            }
            else if (BackGround_Video.url != "")
            {
                videonum += 1;
            }
        }
        PlayVideo();
    }

    public void PlayVideo()
    {
        BackGround_Video.Stop();

        if (videonum < gamemanager.WaitingVideo_path.Length)
        {
            BackGround_Video.url = gamemanager.WaitingVideo_path[videonum];
            BackGround_Video.Play();
        }
        //else if (videonum == gamemanager.WaitingVideo.Length)
        else if (videonum == gamemanager.WaitingVideo_path.Length)
        {
            videonum = 0;
            BackGround_Video.url = gamemanager.WaitingVideo_path[videonum];
            BackGround_Video.Play();
        }
        Invoke("waitVideoTime", 2f);
    }

    public void waitVideoTime()
    {
        SeeVideo = false;
    }

    public void AnyErrorTime()
    {
        //if (!gamemanager.UI_All.gameObject.activeSelf)
        //{
        //    gamemanager.UISetting();
        //}

        if (GameManager.AnyError == true && !gamemanager.ErrorMessage.gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                gamemanager.MenuBar.gameObject.SetActive(true);
                for (int index = 0; index < gamemanager.MenuBar.transform.childCount; index++)
                {
                    gamemanager.MenuBar.transform.GetChild(index).gameObject.SetActive(false);
                }
                //gamemanager.ErrorMessage.gameObject.SetActive(true);
            }
        }
    }

    public void OutWaitingMode()
    {
        for(int i=0; i< gamemanager.MenuBar.transform.GetChild(0).childCount; i++)
        {
            gamemanager.MenuBar.transform.GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
        gamemanager.MenuBar.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(transform);
        gamemanager.MenuBar.GetComponent<Image>().enabled = true;
        //BackGround_Video.clip = null;
        ////BackGround_Video.url = "";
        //BackGround_Video.Stop();

        //if (gamemanager.NaviRect.sizeDelta.x < 472f)
        //{
        //    gamemanager.navi_t = 0;
        //    gamemanager.moveNavi = true;
        //    gamemanager.NaviOn = false;
        //}
        // DB에서 받은 콘텐츠 정보를 이용해서 어디에 설치되어있는지 확인하여
        // 해당 ClearMode에 연결
        if (ContentsInfo.ContentsName == "Cartoon")
        {
            Loading.nextScene = "CartoonMode";
        } else if (ContentsInfo.ContentsName == "Jaemilang")
        {
            Loading.nextScene = "JaemilangMode";
        }

        gamemanager.WriteLog(LogSendServer.NormalLogCode.ChangeMode, "ChangeMode : Start(" + GameManager.PrevMode + " - " + Loading.nextScene + ")", GetType().ToString());
        SceneManager.LoadScene("Loading");
        
    }
}
        