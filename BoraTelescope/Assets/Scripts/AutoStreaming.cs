using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;
using System.IO;
using System.Threading.Tasks;

public class AutoStreaming : MonoBehaviour
{
    public GameManager gamemanager;
    public GameObject rawim;

    FileInfo tsfilelenth;

    private Process _ClientProcess;
    string realtimeadd;
    public List<string> realtiimeadd_list = new List<string>();

    string str;
    List<string> SeeOutput = new List<string>();

    public void makefile()
    {
        gamemanager.WriteLog(GameManager.NormalLogCode.Jamilang_Streaming, "Start LiveStreaming", GetType().ToString());

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (File.Exists("D:/Project/BORA/ffmpeg-5.0.1-full_build/ffmpeg-5.0.1-full_build/bin/output.ts") == true)
            {
                File.Delete("D:/Project/BORA/ffmpeg-5.0.1-full_build/ffmpeg-5.0.1-full_build/bin/output.ts");
                gamemanager.WriteLog(GameManager.NormalLogCode.Jamilang_Streaming, "Delete ts file", GetType().ToString());
            }
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (File.Exists("C:/ffmpeg-5.0.1-full_build/bin/output.ts") == true)
            {
                File.Delete("C:/ffmpeg-5.0.1-full_build/bin/output.ts");
                gamemanager.WriteLog(GameManager.NormalLogCode.Jamilang_Streaming, "Delete ts file", GetType().ToString());
            }
        }

        //Debug.Log("start makefile");
        Process _ClientProcess = new Process();
        ProcessStartInfo info = new ProcessStartInfo("cmd");
        info.CreateNoWindow = true;
        info.UseShellExecute = false;
        info.RedirectStandardOutput = true;
        info.RedirectStandardInput = true;
        info.RedirectStandardError = true;
        _ClientProcess.StartInfo = info;

        _ClientProcess.Start();
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            _ClientProcess.StandardInput.WriteLine("cd D:/Project/BORA/ffmpeg-5.0.1-full_build/ffmpeg-5.0.1-full_build/bin");
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            _ClientProcess.StandardInput.WriteLine("cd C:/ffmpeg-5.0.1-full_build/bin");
        }
        SeeOutput.Add(_ClientProcess.StandardOutput.ReadLine());

        gamemanager.WriteLog(LogSendServer.NormalLogCode.Jamilang_Streaming, "Input m3u8 Make ts File", GetType().ToString());
        _ClientProcess.StandardInput.WriteLine($"ffmpeg -i https://0678e6374a55.ap-northeast-2.playback.live-video.net/api/video/v1/ap-northeast-2.975933023183.channel.yk0CZzcz9ARu.m3u8 -c copy output.ts");

        gamemanager.WriteLog(LogSendServer.NormalLogCode.Jamilang_Streaming, "Get tSFile", GetType().ToString());
    }

    public void MaketsFIle()
    {
        while (true)
        {
            Debug.Log("jl");
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                if (File.Exists("C:/ffmpeg-5.0.1-full_build/bin/output.ts") == true)
                {
                    Debug.Log("yes");
                    gamemanager.WriteLog(LogSendServer.NormalLogCode.Jamilang_Streaming, "confirm TSFile Start", GetType().ToString());

                    this.gameObject.GetComponent<MinimalPlayback>().path = "C:/ffmpeg-5.0.1-full_build/bin/output.ts";

                    //Debug.Log(this.gameObject.GetComponent<MinimalPlayback>().path);
                    gamemanager.WriteLog(GameManager.NormalLogCode.Jamilang_Streaming, "VideoPlayer Connect", GetType().ToString());
                    break;
                }
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (File.Exists("D:/Project/BORA/ffmpeg-5.0.1-full_build/ffmpeg-5.0.1-full_build/bin/output.ts") == true)
                {
                    Debug.Log("yes");
                    gamemanager.WriteLog(LogSendServer.NormalLogCode.Jamilang_Streaming, "confirm TSFile Start", GetType().ToString());

                    this.gameObject.GetComponent<MinimalPlayback>().path = "D:/Project/BORA/ffmpeg-5.0.1-full_build/ffmpeg-5.0.1-full_build/bin/output.ts";

                    //Debug.Log(this.gameObject.GetComponent<MinimalPlayback>().path);
                    gamemanager.WriteLog(GameManager.NormalLogCode.Jamilang_Streaming, "VideoPlayer Connect", GetType().ToString());
                    break;
                }
            }
        }

        LoadandPlay();
        //Invoke("LoadandPlay", 2f);
    }

    public void LoadandPlay()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            tsfilelenth = new FileInfo("C:/ffmpeg-5.0.1-full_build/bin/output.ts");
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            tsfilelenth = new FileInfo("D:/Project/BORA/ffmpeg-5.0.1-full_build/ffmpeg-5.0.1-full_build/bin/output.ts");
        }


        gamemanager.WriteLog(LogSendServer.NormalLogCode.Jamilang_Streaming, tsfilelenth.Length.ToString(), GetType().ToString());
        if (tsfilelenth.Length != 0)
        {
            this.gameObject.GetComponent<MinimalPlayback>().ReadyPlay();
            gamemanager.WriteLog(GameManager.NormalLogCode.Jamilang_Streaming, "VideoPlayer Size Check", GetType().ToString());
            //gamemanager.WriteLog(LogSendServer.NormalLogCode.ChangeMode, this.gameObject.GetComponent<MPMP>().IsLoading().ToString(), GetType().ToString());
        }
        else
        {
            Invoke("LoadandPlay", 0.8f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<MinimalPlayback>().path == "")
        {
            MaketsFIle();
        }
    }

    public void FinishStream()
    {
        this.gameObject.GetComponent<MinimalPlayback>().Stop();

        //gamemanager.WriteLog(GameManager.NormalLogCode.Etc_YoutubeLive, "Close VideoPlayer", GetType().ToString());

        rawim.gameObject.SetActive(false);


        Process _ClientProcess_f = new Process();
        ProcessStartInfo info = new ProcessStartInfo("cmd");
        info.CreateNoWindow = true;
        info.UseShellExecute = false;
        info.RedirectStandardOutput = true;
        info.RedirectStandardInput = true;
        info.RedirectStandardError = true;
        _ClientProcess_f.StartInfo = info;

        _ClientProcess_f.Start();
        _ClientProcess_f.StandardInput.WriteLine("taskkill /im ffmpeg.exe /f");
        Debug.Log(_ClientProcess_f.StandardOutput.ReadLine());
        Debug.Log(_ClientProcess_f.StandardOutput.ReadLine());
        Debug.Log(_ClientProcess_f.StandardOutput.ReadLine());
        Debug.Log(_ClientProcess_f.StandardOutput.ReadLine());
        Debug.Log(_ClientProcess_f.StandardOutput.ReadLine());

        this.gameObject.GetComponent<MinimalPlayback>().path = "";

        _ClientProcess_f.Kill();
        gamemanager.WriteLog(GameManager.NormalLogCode.Jamilang_Streaming, "Finish LiveStreaming", GetType().ToString());
    }
}
