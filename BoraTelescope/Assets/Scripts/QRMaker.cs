using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;
using System.IO;

public class QRMaker : MonoBehaviour
{
    public GameManager gamemanager;
    public RawImage QRCodeImage;
    public string boranum;
    public string url;

    private static Color32[] EncodeURL(string UrlText, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Width = width,
                Height = height
            }
        };
        return writer.Write(UrlText);
    }

    public Texture2D CreateQR(string URL)
    {
        var encoded = new Texture2D(256, 256);
        var color32 = EncodeURL(URL, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();

        return encoded;
    }
    public void MakeQRCode()
    {
        if (GameManager.internetCon == true)
        {
            //url = "http://211.104.146.87:78/info/boraphotodownload/be890630-088e-4760-8cc7-905c6a91bdf1-1-2022-07-07-18-27-27-661.png";
            //string url = "https://borabucket.s3.ap-northeast-2.amazonaws.com/" + filename;
            Texture2D QRImage = CreateQR(url);

            QRCodeImage.texture = QRImage;
            Invoke("waitQRcode", 1f);
        } else if(GameManager.internetCon == false)
        {
            gamemanager.jaemilangmode.capturemode.CaptureEndCamera();
            NoticeWindow.NoticeWindowOpen("ErrorInternet");
            //gamemanager.ErrorMessage.SetActive(true);
        }
    }
    /*
    public void MakeQRCode(string filename)
    {
        //url = "http://211.104.146.87:78/info/boraphotodownload/be890630-088e-4760-8cc7-905c6a91bdf1-1-2022-07-07-18-27-27-661.png";
        //string url = "https://borabucket.s3.ap-northeast-2.amazonaws.com/" + filename;
        Texture2D QRImage = CreateQR(url);

        QRCodeImage.texture = QRImage;
        Invoke("waitQRcode", 1f);
    }
    */
    public void waitQRcode()
    {
        if (GameManager.internetCon == true)
        {
            QRCodeImage.transform.parent.gameObject.SetActive(true);
            QRCodeImage.gameObject.SetActive(true);

            gamemanager.WriteLog(LogSendServer.NormalLogCode.Jamilang_QRCode, "Jamilang_QRCode:On", GetType().ToString());

            ScreenCapture.counttime = false;
            QRCodeImage.transform.parent.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
            QRCodeImage.transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            QRCodeImage.transform.parent.gameObject.transform.GetChild(2).gameObject.SetActive(true);

            gamemanager.jaemilangmode.capturemode.customMark.transform.parent = gamemanager.jaemilangmode.capturemode.flasheffect.transform.parent.gameObject.transform;

            gamemanager.jaemilangmode.capturemode.customMark.transform.localPosition = CaptureMode.originPos;

            gamemanager.jaemilangmode.capturemode.customMark.gameObject.SetActive(false);
            Invoke("SetCloseBut", 1f);
            Invoke("CloseQRCode", 30f);
        }
        else if (GameManager.internetCon == false)
        {
            gamemanager.jaemilangmode.capturemode.CaptureEndCamera();
            NoticeWindow.NoticeWindowOpen("ErrorInternet");
            //gamemanager.ErrorMessage.SetActive(true);
        }
    }

    public void SetCloseBut()
    {
        QRCodeImage.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void CloseQRCode()
    {
        gamemanager.WriteLog(LogSendServer.NormalLogCode.Jamilang_QRCode, "Jamilang_QRCode:Off", GetType().ToString());
        QRCodeImage.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        QRCodeImage.gameObject.SetActive(false);
        QRCodeImage.transform.parent.gameObject.SetActive(false);
        gamemanager.BackGround.SetActive(false);
    }

    public void QRCloseLog()
    {
        gamemanager.WriteLog(LogSendServer.NormalLogCode.Jamilang_QRCode, "Jamilang_QRCode:Off", GetType().ToString());
    }

    public void GetBoranum()
    {
        string borainfo = File.ReadAllText("C:/XRTeleSpinCam/bora_info.txt");
        borainfo.Replace("\r\n", string.Empty);
        boranum = borainfo.Substring(13, borainfo.IndexOf("\r\n") - 13);
    }
}
