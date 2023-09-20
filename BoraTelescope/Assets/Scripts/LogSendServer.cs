using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Linq;

public class LogData
{
    public string Timestamp;
    public string Type;
    public int LogCode;
    public string LogInformation;
    public string ScriptName;
    public string ContentsVersion;

    public LogData(string timestamp, string type, int codenum, string loginfo, string scriptname, string contentsversion)
    {
        Timestamp = timestamp;
        Type = type;
        LogCode = codenum;
        LogInformation = loginfo;
        ScriptName = scriptname;
        ContentsVersion = contentsversion;
    }
}

public class LogSendServer : bora_client_test
{
    // 일반로그 : Timestamp [Normal] (로그코드) 간단한 설명/스크립트 이름/콘텐츠 버전
    // 에러로그 : Timestamp [Error] (에러코드) 간단한 설명-원인/스크립트 이름/콘텐츠 버전

    public enum NormalLogCode
    {
        StartContents = 1001,
        EndContents = 1002,
        Connect_SystemControl = 1003,
        Connect_Camera = 1005,
        ChangeMode = 1006,
        Load_ResourceFile = 1008,
        ChangeLanguage = 1010,
        ClickHomeBtn = 1011,

        Cartoon_Start = 2001,

        Jaemilang_Start = 3001,
        Jamilang_Capture = 3002,
        Jamilang_Streaming = 3003,
        Jamilang_QRCode = 3005,


        Visit_Start = 5011,
        Visit_See = 5012,
        Visit_Write = 5013,
        Visit_Out = 5014,
        Visit_Past = 5015,
        Visit_future = 5016,
        Visit_Save = 5017,

        Selfi_Start = 5018,
        Selfi_Photo = 5019,
        Selfi_Custom = 5020,
        Selfi_Download = 5021,
        Selfi_RePhoto = 5022,
        Selfi_Cancel = 5023,
        Selfi_QRCode = 5024,
        selfi_LightControl = 5025
    }
    public NormalLogCode lognum;

    public enum ErrorLogCode
    {
        DisConnect_SystemControl = 1001,
        Fail_Connect_Camera = 1003,
        UnLoad_Jsonfile = 1004,
        UnLoad_ResourceFile = 1005,
        Fail_ChangeMode = 1006,
        Fail_ImageUpload = 1007,
        Fail_ImageListConfirm = 1008,
        Fail_EnterMode = 1009,
        Fail_LightControl = 1010,
        Fail_RecordUpload = 1011,
        Fail_QRUpload = 1013,
        Fail_InternetConnect = 1014,

        UnityError = 2001,
        UnityException = 2002
    }
    public ErrorLogCode errornum;

    private string timestamp;
    private string logType;
    private int LogCode;
    public static string ContentsVersion;

    /// <summary>
    /// 로그전송
    /// </summary>
    /// <param name="lognum"></param>
    /// <param name="loginfo"></param>
    /// <param name="scriptname"></param>
    public void WriteLog(NormalLogCode lognum, string loginfo, string scriptname)
    {
        logType = "[NORMAL]";
        //ContentsVersion = Application.version;

        LogCode = (int)lognum;

        timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

        LogData logdata = new LogData(timestamp, logType, LogCode, loginfo, scriptname, ContentsVersion);
        //string str = JsonUtility.ToJson(logdata);
        //saveLog(str);
        //savestringLog(timestamp + "`^" + logType + "`^" + LogCode + "`^" + loginfo + "`^" + scriptname + "`^" + ContentsVersion);
        Send_Log_Button(timestamp + "`^" + logType + "`^" + LogCode + "`^" + loginfo + "`^" + scriptname + "`^" + ContentsVersion);
    }

    /// <summary>
    /// 오류로그 전송
    /// </summary>
    /// <param name="errornum"></param>
    /// <param name="loginfo"></param>
    /// <param name="scriptname"></param>
    public void WriteErrorLog(ErrorLogCode errornum, string loginfo, string scriptname)
    {
        logType = "[ERROR]";
        //ContentsVersion = Application.version;

        LogCode = (int)errornum;

        timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

        LogData logdata = new LogData(timestamp, logType, LogCode, loginfo, scriptname, ContentsVersion);
        //string str = JsonUtility.ToJson(logdata);
        //saveLog(str);
        //savestringLog(timestamp + "`^" + logType + "`^" + LogCode +"`^" + loginfo + "`^" + scriptname + "`^" + ContentsVersion);
        Send_Error_Button(timestamp + "`^" + logType + "`^" + LogCode + "`^" + loginfo + "`^" + scriptname + "`^" + ContentsVersion);
    }

    List<string> Log_json = new List<string>();
    List<string> Log_Text = new List<string>();
    string allLog;
    int filenum;

    /// <summary>
    /// 모든로그 로컬 파일에 저장
    /// </summary>
    /// <param name="str"></param>
    public void saveLog(string str)
    {
        allLog = "";
        Log_json.Add(str);

        for (int index = 0; index < Log_json.Count; index++)
        {
            allLog += Log_json[index] + System.Environment.NewLine;
        }

        File.WriteAllText(Application.dataPath + ("/LogData_" + ContentsInfo.ContentsName + "_" + DateTime.Now.ToString("yyyyMMdd") + ".json"), allLog);
        //Debug.Log(str);
    }

    /// <summary>
    /// 모든 로그 로컬 파일에 저장(Txt파일)
    /// </summary>
    /// <param name="str"></param>
    public void savestringLog(string str)
    {
        allLog = "";
        Log_Text.Add(str);

        for (int index = 0; index < Log_Text.Count; index++)
        {
            allLog += Log_Text[index] + System.Environment.NewLine;
        }

        File.WriteAllText(Application.dataPath + ("/LogData_" + ContentsInfo.ContentsName + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt"), allLog);
    }

    public bool IsInternetConnected()
    {
        const string NCSI_TEST_URL = "http://www.msftncsi.com/ncsi.txt";
        const string NCSI_TEST_RESULT = "Microsoft NCSI";
        const string NCSI_DNS = "dns.msftncsi.com";
        const string NCSI_DNS_IP_ADDRESS = "131.107.255.255";

        try
        {
            // Check NCSI test link
            var webClient = new WebClient();
            //string result = webClient.DownloadString(NCSI_TEST_URL);
            string result = new TimedWebClient { Timeout = 500 }.DownloadString(NCSI_TEST_URL);
            if (result != NCSI_TEST_RESULT)
            {
                return false;
            }

            // Check NCSI DNS IP
            var dnsHost = Dns.GetHostEntry(NCSI_DNS);
            if (dnsHost.AddressList.Count() < 0 || dnsHost.AddressList[0].ToString() != NCSI_DNS_IP_ADDRESS)
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            return false;
        }

        return true;
    }

    public class TimedWebClient : WebClient
    {
        // Timeout in milliseconds, default = 600,000 msec
        public int Timeout { get; set; }

        public TimedWebClient()
        {
            this.Timeout = 100;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var objWebRequest = base.GetWebRequest(address);
            objWebRequest.Timeout = this.Timeout;
            return objWebRequest;
        }
    }
}
