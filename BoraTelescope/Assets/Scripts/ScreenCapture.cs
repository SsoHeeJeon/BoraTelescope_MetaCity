using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class ScreenCapture : UploadImage
{
    private Camera Objcamera;       //보여지는 카메라.

    public GameObject flasheffect;
    public GameObject customMark;
    public static Sprite MarkImg;

    private int resWidth;
    private int resHeight;
    string path;

    public static float count;
    public static bool counttime = false;
    // Use this for initialization
    public void ReadyToCapture()
    {
        Objcamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        customMark.GetComponent<Image>().sprite = MarkImg;

        resWidth = Screen.width;
        resHeight = Screen.height;
        path = Application.dataPath + "/ScreenShot/";
        QRCodeImage.transform.parent.gameObject.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 0;
        Debug.Log(path);
    }

    public void ClickScreenShot()
    {
        GetBoranum();
        //flasheffect.gameObject.SetActive(false);
        counttime = true;
        
        if (SelfiFunction.selfimode == true && SelfiFunction.selfistate == SelfiFunction.SelfiState.Download)
        {
            //customMark.gameObject.SetActive(true);
            QRCodeImage.transform.parent.gameObject.SetActive(true);
            QRCodeImage.transform.parent.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            QRCodeImage.transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            QRCodeImage.transform.parent.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
        //flasheffect.gameObject.SetActive(false);
        //startflasheffect = false;
        StartCoroutine(CaptureandSave());
    }

    private IEnumerator CaptureandSave()
    {
        yield return new WaitForEndOfFrame();

        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }
        string name;
        string filename;
        name = path + boranum + "-" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        filename = boranum + "-" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        Debug.Log("NOW" + name);
        //startflasheffect = false;

        if (SelfiFunction.selfimode == false)
        {
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            Objcamera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
            Objcamera.Render();
            RenderTexture.active = rt;

            screenShot.ReadPixels(rec, 0, 0);
            //screenShot.Apply();

            byte[] bytes = screenShot.EncodeToPNG();
            //startflasheffect = false;
            Debug.Log(bytes);
            File.WriteAllBytes(name, bytes);

            gamemanager.WriteLog(LogSendServer.NormalLogCode.Jamilang_Capture, "Jamilang_Capture", GetType().ToString());

            if (GameManager.internetCon == true)
            {
                PutImageObject(name, filename);
            }
            else if (GameManager.internetCon == false)
            {
                gamemanager.jaemilangmode.capturemode.CaptureEndCamera();
                NoticeWindow.NoticeWindowOpen("ErrorInternet");
                //gamemanager.ErrorMessage.SetActive(true);
            }
            Objcamera.targetTexture = null;

            Destroy(screenShot);

            //startflasheffect = false;
        }
        else if (SelfiFunction.selfimode == true)
        {
            if (SelfiFunction.selfistate == SelfiFunction.SelfiState.Download)
            {
                RenderTexture fianlRT = new RenderTexture(resWidth, resHeight, 24);
                gamemanager.selfifunction.FinalCam.targetTexture = fianlRT;
                Texture2D final_screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
                Rect final_rec = new Rect(0, 0, final_screenShot.width, final_screenShot.height);
                gamemanager.selfifunction.FinalCam.Render();
                RenderTexture.active = fianlRT;

                final_screenShot.ReadPixels(final_rec, 0, 0);
                //screenShot.Apply();

                byte[] bytes = final_screenShot.EncodeToPNG();

                gamemanager.selfifunction.SelfiPhoto.GetComponent<RawImage>().texture = fianlRT;
                gamemanager.selfifunction.FinalCam.targetTexture = null;

                gamemanager.selfifunction.PhotoOrigin.gameObject.SetActive(false);
                gamemanager.selfifunction.SelfiPhoto.SetActive(true);
                gamemanager.selfifunction.FinalCam.gameObject.SetActive(false);
                gamemanager.selfifunction.CustomUI.SetActive(false);
                gamemanager.selfifunction.Download_Btn.SetActive(false);
                gamemanager.selfifunction.SelfiPhoto.transform.parent.gameObject.GetComponent<Canvas>().worldCamera = gamemanager.selfifunction.SelfiCam;

                for (int index = 0; index < gamemanager.selfifunction.StickerObj.transform.childCount; index++)
                {
                    Destroy(gamemanager.selfifunction.StickerObj.transform.GetChild(index).gameObject);
                }

                //QR생성
                File.WriteAllBytes(name, bytes);
                PutImageObject(name, filename);

                Destroy(final_screenShot);

                gamemanager.selfifunction.FinishSelfi();
                gamemanager.WriteLog(LogSendServer.NormalLogCode.Selfi_Download, "Selfi_Download", GetType().ToString());
            }
            else if (SelfiFunction.selfistate == SelfiFunction.SelfiState.Take)
            {
                RenderTexture selfiRT = new RenderTexture(resWidth, resHeight, 24);
                gamemanager.selfifunction.SelfiCam.targetTexture = selfiRT;
                Texture2D selfi_screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
                Rect selfi_rec = new Rect(0, 0, selfi_screenShot.width, selfi_screenShot.height);
                gamemanager.selfifunction.SelfiCam.Render();
                RenderTexture.active = selfiRT;

                selfi_screenShot.ReadPixels(selfi_rec, 0, 0);
                //screenShot.Apply();

                //byte[] bytes = screenShot.EncodeToPNG();

                gamemanager.selfifunction.CustomUI.transform.parent.gameObject.GetComponent<Canvas>().worldCamera = gamemanager.selfifunction.FinalCam;
                gamemanager.selfifunction.CustomUI.transform.parent.gameObject.GetComponent<Canvas>().planeDistance = 900;

                //gamemanager.selfifunction.PhotoOrigin.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1344,756);
                gamemanager.selfifunction.PhotoOrigin.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1920,1080);
                gamemanager.selfifunction.PhotoOrigin.gameObject.transform.localPosition = new Vector3(0, 0, 0);

                gamemanager.selfifunction.PhotoOrigin.GetComponent<RawImage>().texture = selfiRT;
                gamemanager.selfifunction.SelfiCam.targetTexture = null;

                gamemanager.jaemilangmode.capturemode.BackGround.SetActive(false);
                gamemanager.selfifunction.Download_Btn.gameObject.SetActive(false);
                gamemanager.selfifunction.FinalCam.gameObject.SetActive(true);
                gamemanager.selfifunction.SelfiCam.gameObject.SetActive(false);
                gamemanager.selfifunction.PreviewCam.gameObject.SetActive(false);
                gamemanager.selfifunction.PhotoPreview.SetActive(false);
                gamemanager.selfifunction.PhotoOrigin.gameObject.SetActive(true);
                gamemanager.selfifunction.PRS.SetActive(false);
                gamemanager.selfifunction.CustomUI.SetActive(true);
                gamemanager.selfifunction.CustomUI.transform.GetChild(0).gameObject.SetActive(false);
                gamemanager.selfifunction.ConfirmUI.SetActive(true);
                gamemanager.selfifunction.SelfiPhoto.transform.parent.gameObject.GetComponent<Canvas>().worldCamera = gamemanager.selfifunction.FinalCam;
                Invoke("CloseBtn", 5f);
                
                if (gamemanager.jaemilangmode.Liveobj.activeSelf)
                {
                    gamemanager.jaemilangmode.Liveobj.SetActive(false);
                }
                else if (gamemanager.jaemilangmode.Jaemilang_background.activeSelf)
                {
                    gamemanager.jaemilangmode.Jaemilang_background.SetActive(false);
                } else if (gamemanager.jaemilangmode.Graffiti_background.activeSelf)
                {
                    gamemanager.jaemilangmode.Graffiti_background.SetActive(false);
                }
                
                gamemanager.WriteLog(LogSendServer.NormalLogCode.Selfi_Photo, "Selfi_Photo", GetType().ToString());
                //Destroy(screenShot);
            }
        }
    }

    public void CloseBtn()
    {
        gamemanager.selfifunction.Confirm_Btn.transform.GetChild(0).gameObject.SetActive(false);
    }

    public static bool startflasheffect = false;
    bool changed = true;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1, 1, 1, 0.745f);

    private void Start()
    {
        customMark.GetComponent<Image>().sprite = MarkImg;
    }

    public void playFlashEffect()
    {
        if (startflasheffect == true)
        {
            if (changed)
            {
                flasheffect.GetComponent<Image>().color = flashColor;
                changed = false;
            }
            else
            {
                flasheffect.GetComponent<Image>().color = Color.Lerp(flasheffect.GetComponent<Image>().color, Color.clear, flashSpeed * Time.deltaTime);
                if (flasheffect.GetComponent<Image>().color.a < 0.1f)
                {
                    startflasheffect = false;
                    flasheffect.gameObject.SetActive(false);
                    //gamemanager.jaemilangmode.capturemode.WaitStartCap();
                }
            }
            //changed = false;
        }
        //else
        //{
        //    flasheffect.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        //}
    }

    //    /***********************************************************************
    //    *                               Button Event Handlers
    //    ***********************************************************************/
    //    #region .
    //    /// <summary> UI 포함 전체 화면 캡쳐 </summary>
    //    public void TakeScreenShotFull()
    //    {
    //#if UNITY_ANDROID
    //        CheckAndroidPermissionAndDo(Permission.ExternalStorageWrite, () => StartCoroutine(TakeScreenShotRoutine()));
    //#else
    //        StartCoroutine(TakeScreenShotRoutine());
    //#endif
    //    }

    //    /// <summary> UI 미포함, 현재 카메라가 렌더링하는 화면만 캡쳐 </summary>
    //    public void TakeScreenShotWithoutUI()
    //    {
    //#if UNITY_ANDROID
    //        CheckAndroidPermissionAndDo(Permission.ExternalStorageWrite, () => _willTakeScreenShot = true);
    //#else
    //        _willTakeScreenShot = true;
    //#endif
    //    }

    //    private void ReadScreenShotAndShow()
    //    {
    //#if UNITY_ANDROID
    //        CheckAndroidPermissionAndDo(Permission.ExternalStorageRead, () => ReadScreenShotFileAndShow(imageToShow));
    //#else
    //        ReadScreenShotFileAndShow(imageToShow);
    //#endif
    //    }
    //    #endregion
    //    /***********************************************************************
    //    *                               Methods
    //    ***********************************************************************/
    //    #region .

    //    // UI 포함하여 현재 화면에 보이는 모든 것 캡쳐
    //    private IEnumerator TakeScreenShotRoutine()
    //    {
    //        yield return new WaitForEndOfFrame();
    //        CaptureScreenAndSave();
    //    }

    //    // UI 제외하고 현재 카메라가 렌더링하는 모습 캡쳐
    //    private void OnPostRender()
    //    {
    //        if (_willTakeScreenShot)
    //        {
    //            _willTakeScreenShot = false;
    //            CaptureScreenAndSave();
    //        }
    //    }
    //    /// <summary> 스크린샷을 찍고 경로에 저장하기 </summary>
    //    private void CaptureScreenAndSave()
    //    {
    //        string totalPath = TotalPath; // 프로퍼티 참조 시 시간에 따라 이름이 결정되므로 캐싱

    //        Texture2D screenTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    //        Rect area = new Rect(0f, 0f, Screen.width, Screen.height);

    //        // 현재 스크린으로부터 지정 영역의 픽셀들을 텍스쳐에 저장
    //        screenTex.ReadPixels(area, 0, 0);

    //        bool succeeded = true;
    //        try
    //        {
    //            // 폴더가 존재하지 않으면 새로 생성
    //            if (Directory.Exists(FolderPath) == false)
    //            {
    //                Directory.CreateDirectory(FolderPath);
    //            }

    //            // 스크린샷 저장
    //            File.WriteAllBytes(totalPath, screenTex.EncodeToPNG());
    //        }
    //        catch (Exception e)
    //        {
    //            succeeded = false;
    //            Debug.LogWarning($"Screen Shot Save Failed : {totalPath}");
    //            Debug.LogWarning(e);
    //        }

    //        // 마무리 작업
    //        Destroy(screenTex);

    //        if (succeeded)
    //        {
    //            Debug.Log($"Screen Shot Saved : {totalPath}");
    //            //flash.Show(); // 화면 번쩍
    //            lastSavedPath = totalPath; // 최근 경로에 저장
    //        }

    //        // 갤러리 갱신
    //        RefreshAndroidGallery(totalPath);
    //    }


    //    // 가장 최근에 저장된 이미지 보여주기
    //    /// <summary> 경로로부터 저장된 스크린샷 파일을 읽어서 이미지에 보여주기 </summary>
    //    private void ReadScreenShotFileAndShow(Image destination)
    //    {
    //        string folderPath = FolderPath;
    //        string totalPath = lastSavedPath;

    //        if (Directory.Exists(folderPath) == false)
    //        {
    //            Debug.LogWarning($"{folderPath} 폴더가 존재하지 않습니다.");
    //            return;
    //        }
    //        if (File.Exists(totalPath) == false)
    //        {
    //            Debug.LogWarning($"{totalPath} 파일이 존재하지 않습니다.");
    //            return;
    //        }

    //        // 기존의 텍스쳐 소스 제거
    //        if (_imageTexture != null)
    //            Destroy(_imageTexture);
    //        if (destination.sprite != null)
    //        {
    //            Destroy(destination.sprite);
    //            destination.sprite = null;
    //        }

    //        // 저장된 스크린샷 파일 경로로부터 읽어오기
    //        try
    //        {
    //            byte[] texBuffer = File.ReadAllBytes(totalPath);

    //            _imageTexture = new Texture2D(1, 1, TextureFormat.RGB24, false);
    //            _imageTexture.LoadImage(texBuffer);
    //        }
    //        catch (Exception e)
    //        {
    //            Debug.LogWarning($"스크린샷 파일을 읽는 데 실패하였습니다.");
    //            Debug.LogWarning(e);
    //            return;
    //        }

    //        // 이미지 스프라이트에 적용
    //        Rect rect = new Rect(0, 0, _imageTexture.width, _imageTexture.height);
    //        Sprite sprite = Sprite.Create(_imageTexture, rect, Vector2.one * 0.5f);
    //        destination.sprite = sprite;
    //    }
    //    #endregion
}