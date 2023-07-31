using NNCam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomProcedure
{
    public string CustomObj;
    public enum customtype
    {
        Make, Remove, PRS
    }
    public customtype CustomType;
    public Vector3 ObjPos;
    public Quaternion ObjRot;
    public float ObjScal;

    public CustomProcedure(string customobj, customtype typmename, Vector3 position, Quaternion rotation, float scale)
    {
        CustomObj = customobj;
        CustomType = typmename;
        ObjPos = position;
        ObjRot = rotation;
        ObjScal = scale;
    }
}
public class SelfiFunction : MonoBehaviour
{
    public GameManager gamemanager;

    public enum SelfiState
    {
        None, Take, Download
    }
    public static SelfiState selfistate;

    public Drawing drawing;

    public GameObject Selfi_Obj;
    public GameObject lightcontrol;
    public GameObject SelfiPhoto;
    public GameObject PRS;      //Position/Rotation/Scale
    public GameObject CustomUI;      //Sticker/Frame
    public GameObject ControlUI;      //Sticker/Frame
    public GameObject PhotoCount;
    public Sprite[] Count_num;
    public GameObject TakePhoto_Btn;
    public GameObject Confirm_Btn;
    public GameObject Download_Btn;
    public RawImage PhotoOrigin;
    public GameObject PhotoPreview;

    public Camera SelfiCam;
    public Camera PreviewCam;
    public Camera FinalCam;

    public Slider Lightvalue;
    public Slider Totalvalue;

    public Image position_obj;
    public Image rotation_obj;
    public Image[] Scale_obj;

    public GameObject[] StrickerList;
    public GameObject[] FrameList;
    public GameObject CustomObj;
    public GameObject StickerObj;
    public GameObject UIStickBtn;
    public GameObject UIFrameBtn;
    public GameObject UIPenColorBtn;

    public GameObject SelectItem;
    public GameObject Reset_Btn;
    public static int s1;
    int count_a;

    public Image countI;
    public float countF;
    public bool TakeCountStart;

    public int ProcedureNum;
    public Dictionary<int, CustomProcedure> SelfiUndo = new Dictionary<int, CustomProcedure>();
    public CustomProcedure state;

    public GameObject undoobj;
    CustomProcedure prestate;

    public Scrollbar CustomScrollbar;
    public GameObject CustomCategory;
    public Sprite[] Btn_language;

    public static bool selfimode = false;

    private void Update()
    {
        if (CustomScrollbar.value == 0)
        {
            for (int index = 0; index < CustomCategory.transform.childCount; index++)
            {
                CustomCategory.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            CustomCategory.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (CustomScrollbar.value >= 0 && CustomScrollbar.value < 0.597f)
        {
            for (int index = 0; index < CustomCategory.transform.childCount; index++)
            {
                CustomCategory.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            CustomCategory.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (CustomScrollbar.value >= 0.597f && CustomScrollbar.value < 0.928f)
        {
            for (int index = 0; index < CustomCategory.transform.childCount; index++)
            {
                CustomCategory.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            CustomCategory.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (CustomScrollbar.value >= 0.928f)
        {
            for (int index = 0; index < CustomCategory.transform.childCount; index++)
            {
                CustomCategory.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            CustomCategory.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void ChangeFace(GameObject btn)
    {
        switch (btn.name)
        {
            case "Basic":
                Lightvalue.value = 0.1f;
                Totalvalue.value = 0;
                if (SelfiCam.GetComponent<GrayScale>().enabled == true)
                {
                    SelfiCam.GetComponent<GrayScale>().enabled = false;
                }
                break;
            case "Light":
                Lightvalue.value = 0.1f;
                Totalvalue.value = 2;
                if (SelfiCam.GetComponent<GrayScale>().enabled == true)
                {
                    SelfiCam.GetComponent<GrayScale>().enabled = false;
                }
                break;
            case "Dark":
                Lightvalue.value = 0.1f;
                Totalvalue.value = -2f;
                if (SelfiCam.GetComponent<GrayScale>().enabled == true)
                {
                    SelfiCam.GetComponent<GrayScale>().enabled = false;
                }
                break;
            case "Gray":
                if (SelfiCam.GetComponent<GrayScale>().enabled == false)
                {
                    SelfiCam.GetComponent<GrayScale>().enabled = true;
                }
                else if (SelfiCam.GetComponent<GrayScale>().enabled == true)
                {
                    SelfiCam.GetComponent<GrayScale>().enabled = false;
                }
                break;
        }
        for (int index = 0; index < ControlUI.transform.childCount; index++)
        {
            ControlUI.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        btn.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SetSticker(GameObject btn)
    {
        s1++;
        drawing.enabled = false;
        for (int index = 0; index < UIPenColorBtn.transform.childCount; index++)
        {
            UIPenColorBtn.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (StickerObj.transform.childCount != 0)
        {
            for (int index = 0; index < StickerObj.transform.childCount; index++)
            {
                StickerObj.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                StickerObj.transform.GetChild(index).gameObject.GetComponent<Button>().enabled = true;
            }
        }

        for (int index = 0; index < StrickerList.Length; index++)
        {
            if (btn.name == StrickerList[index].name)
            {
                GameObject obj = Instantiate(StrickerList[index]);
                obj.transform.SetParent(StickerObj.transform);
                obj.name = btn.name + "_" + s1;
                obj.transform.localPosition = new Vector3(0,0,-30-s1);

                obj.SetActive(true);

                SaveUndo(obj.name, obj, "Make");
            }
        }
        //btn.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void setFrame(GameObject btn)
    {
        drawing.enabled = false;
        for (int index = 0; index < UIPenColorBtn.transform.childCount; index++)
        {
            UIPenColorBtn.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        for (int index = 0; index < btn.transform.parent.gameObject.transform.childCount; index++)
        {
            btn.transform.parent.gameObject.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (StickerObj.transform.childCount != 0)
        {
            for (int index = 0; index < StickerObj.transform.childCount; index++)
            {
                StickerObj.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                StickerObj.transform.GetChild(index).gameObject.GetComponent<Button>().enabled = true;
            }
        }

        for (int index = 0; index < FrameList.Length; index++)
        {
            if (btn.name == FrameList[index].name)
            {
                if (!FrameList[index].activeSelf)
                {
                    for (int os = 0; os < FrameList.Length; os++)
                    {
                        FrameList[os].SetActive(false);
                    }
                    FrameList[index].SetActive(true);
                    btn.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (FrameList[index].activeSelf)
                {
                    FrameList[index].SetActive(false);
                }
            }
        }
    }

    public void ChangeColor(GameObject btn)
    {
        switch (btn.name)
        {
            case "Red":
                drawing.SelectColor = drawing.Pen_Red;
                break;
            case "Orange":
                drawing.SelectColor = drawing.Pen_Orange;
                break;
            case "Yellow":
                drawing.SelectColor = drawing.Pen_Yellow;
                break;
            case "Lightgreen":
                drawing.SelectColor = drawing.Pen_Lightgreen;
                break;
            case "Green":
                drawing.SelectColor = drawing.Pen_Green;
                break;
            case "LightBlue":
                drawing.SelectColor = drawing.Pen_LightBlue;
                break;
            case "Blue":
                drawing.SelectColor = drawing.Pen_Blue;
                break;
            case "Purple":
                drawing.SelectColor = drawing.Pen_Purple;
                break;
            case "Black":
                drawing.SelectColor = drawing.Pen_Black;
                break;
            case "White":
                drawing.SelectColor = drawing.Pen_White;
                break;
        }

        if (StickerObj.transform.childCount != 0)
        {
            for (int index = 0; index < StickerObj.transform.childCount; index++)
            {
                StickerObj.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                StickerObj.transform.GetChild(index).gameObject.GetComponent<Button>().enabled = true;
            }
        }
        for (int index = 0; index < UIPenColorBtn.transform.childCount; index++)
        {
            UIPenColorBtn.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        btn.transform.GetChild(0).gameObject.SetActive(true);
        drawing.enabled = true;
    }

    public void selectSkicker(GameObject stick)
    {
        if (StickerObj.transform.childCount != 0)
        {
            for (int index = 0; index < StickerObj.transform.childCount; index++)
            {
                StickerObj.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                StickerObj.transform.GetChild(index).gameObject.GetComponent<Button>().enabled = true;
            }
        }

        if (!stick.transform.GetChild(0).gameObject.activeSelf)
        {
            stick.transform.GetChild(0).gameObject.SetActive(true);
            stick.GetComponent<Button>().enabled = false;
        }
        SelectItem = stick;
    }

    public void ResetAll()
    {
        if (selfistate == SelfiState.Take)
        {
            if (StickerObj.transform.childCount != 0)
            {
                for (int index = StickerObj.transform.childCount - 1; index >= 0; index--)
                {
                    Destroy(StickerObj.transform.GetChild(index).gameObject);
                }
                s1 = 0;
            }
            SelectItem = null;

            for (int index = 0; index < FrameList.Length; index++)
            {
                FrameList[index].SetActive(false);
            }

            if (FinalCam.transform.childCount != 0)
            {
                for (int index = FinalCam.transform.childCount - 1; index >= 0; index--)
                {
                    Destroy(FinalCam.transform.GetChild(index).gameObject);
                }
            }

            SelfiUndo.Clear();
            ProcedureNum = 0;
            for (int index = 0; index < UIFrameBtn.transform.childCount; index++)
            {
                UIFrameBtn.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            for (int index = 0; index < UIStickBtn.transform.childCount; index++)
            {
                UIStickBtn.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            for (int index = 0; index < UIPenColorBtn.transform.childCount; index++)
            {
                UIPenColorBtn.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else if (selfistate == SelfiState.None)
        {
            Lightvalue.value = 0.1f;
            Totalvalue.value = 0;

            if (SelfiCam.GetComponent<GrayScale>().enabled == true)
            {
                SelfiCam.GetComponent<GrayScale>().enabled = false;
            }

            PhotoPreview.transform.localPosition = new Vector3(0, 0, 927);
            PhotoPreview.transform.rotation = Quaternion.Euler(0, 0, 0);
            PhotoPreview.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void StartSelfi()
    {
        selfistate = SelfiState.None;

        SelfiUndo.Clear();
        ProcedureNum = 0;

        //gamemanager.NavigationBar.SetActive(false);
        //gamemanager.Arrow.SetActive(false);
        //gamemanager.MiniMap_Background.transform.parent.parent.gameObject.SetActive(false);

        this.transform.GetChild(0).gameObject.SetActive(true);
        this.transform.GetChild(0).gameObject.GetComponent<Controller>().enabled = true;

        CustomUI.transform.parent.gameObject.GetComponent<Canvas>().worldCamera = SelfiCam;

        SelfiCam.gameObject.SetActive(true);
        PreviewCam.gameObject.SetActive(true);
        SelfiPhoto.transform.parent.gameObject.SetActive(true);
        PhotoPreview.SetActive(true);
        PhotoPreview.transform.GetChild(0).gameObject.SetActive(true);
        lightcontrol.SetActive(true);
        TakePhoto_Btn.SetActive(true);
        Invoke("CloseTakeBtn", 5f);
        Reset_Btn.SetActive(true);
        TakePhoto_Btn.transform.GetChild(0).gameObject.SetActive(true);
        Confirm_Btn.transform.GetChild(0).gameObject.SetActive(true);
        Download_Btn.transform.GetChild(0).gameObject.SetActive(true);

        if (FinalCam.transform.childCount != 0)
        {
            for (int index = FinalCam.transform.childCount - 1; index >= 0; index--)
            {
                Destroy(FinalCam.transform.GetChild(index).gameObject);
            }
        }

        if (SelfiCam.GetComponent<GrayScale>().enabled == true)
        {
            SelfiCam.GetComponent<GrayScale>().enabled = false;
        }

        SelfiPhoto.transform.position = Vector3.zero;
        SelfiPhoto.transform.rotation = Quaternion.Euler(0, 0, 0);
        SelfiPhoto.transform.localScale = new Vector3(1, 1, 1);

        PhotoPreview.transform.localPosition = new Vector3(0, 0, 927);
        PhotoPreview.transform.rotation = Quaternion.Euler(0, 0, 0);
        PhotoPreview.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        SelfiPhoto.transform.parent.gameObject.GetComponent<Canvas>().worldCamera = SelfiCam;

        Lightvalue.value = 0.1f;
        Totalvalue.value = 0;
   }

    public void CloseTakeBtn()
    {
        TakePhoto_Btn.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void TakePhoto()
    {
        selfistate = SelfiState.Take;
        //gamemanager.Selfi_Obj.transform.position = jaemilangmode.CameraWindow.transform.position;
        PRS.SetActive(false);
        gamemanager.jaemilangmode.CaptueObject.gameObject.SetActive(true);
        //gamemanager.FlashEffect();
        PhotoCount.SetActive(true);
        TakeCountStart = true;
        countF = 3.2f;
        TakeCount();
        //Invoke("countdelay", 1.5f);
        //gamemanager.CaptureCamera();
        TakePhoto_Btn.SetActive(false);
        lightcontrol.SetActive(false);
        CustomScrollbar.value = 0;
        selfimode = true;
    }

    public void TakeCount()
    {
        if (TakeCountStart == true)
        {
            countF -= Time.deltaTime;

            //countT.text = ((int)countF).ToString();
            if ((int)countF - 1 >= 0)
            {
                countI.sprite = Count_num[(int)countF - 1];
            }

            if ((int)countF == 0)
            {
                TakeCountStart = false;
                gamemanager.jaemilangmode.capturemode.FlashEffect();
                Invoke("countdelay", 0.01f);
            }
            else
            {
                Invoke("TakeCount", 0.01f);
            }
        }
    }

    public void GetPhoto()
    {
        selfistate = SelfiState.Download;

        PhotoOrigin.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);
        PhotoOrigin.gameObject.transform.localPosition = new Vector3(0,0, 0);

        this.transform.GetChild(0).gameObject.GetComponent<Controller>().enabled = false;
        this.transform.GetChild(0).gameObject.SetActive(false);
        for (int index = 0; index < StickerObj.transform.childCount; index++)
        {
            if (StickerObj.transform.GetChild(index).transform.GetChild(0).gameObject.activeSelf)
            {
                count_a++;
            }
        }

        for (int index = 0; index < StickerObj.transform.childCount; index++)
        {
            if (StickerObj.transform.GetChild(index).transform.GetChild(0).gameObject.activeSelf)
            {
                StickerObj.transform.GetChild(index).gameObject.GetComponent<Button>().enabled = true;
                StickerObj.transform.GetChild(index).transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        Confirm_Btn.SetActive(false);
        Download_Btn.SetActive(true);
        CustomUI.SetActive(false);
        drawing.enabled = false;
        Invoke("CloseDownBtn", 5f);
        count_a = 0;
        SelectItem = null;
    }

    public void DownloadPhoto()
    {
        selfistate = SelfiState.Download;
        this.transform.GetChild(0).gameObject.GetComponent<Controller>().enabled = false;
        this.transform.GetChild(0).gameObject.SetActive(false);
        for (int index = 0; index < StickerObj.transform.childCount; index++)
        {
            if (StickerObj.transform.GetChild(index).transform.GetChild(0).gameObject.activeSelf)
            {
                count_a++;
            }
        }

        Reset_Btn.SetActive(false);
        CustomUI.SetActive(false);
        for (int index = 0; index < StickerObj.transform.childCount; index++)
        {
            StickerObj.transform.GetChild(index).transform.GetChild(0).gameObject.SetActive(false);
        }
        countdelay();
        Download_Btn.SetActive(false);
        //gamemanager.CaptureCamera();
    }

    public void CloseDownBtn()
    {
        Download_Btn.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void countdelay()
    {
        if (selfistate == SelfiState.Take)
        {
            PhotoCount.SetActive(false);
            //gamemanager.screencapture.ClickScreenShot();
            gamemanager.jaemilangmode.capturemode.ReadyToCapture();
            gamemanager.ButtonClickSound();
        }
        else if (selfistate == SelfiState.Download)
        {
            gamemanager.jaemilangmode.capturemode.CaptureCamera();
            for (int index = 0; index < StickerObj.transform.childCount; index++)
            {
                Destroy(StickerObj.transform.GetChild(index).gameObject);
            }
            for (int os = 0; os < FrameList.Length; os++)
            {
                FrameList[os].SetActive(false);
            }
            for (int index = 0; index < UIFrameBtn.transform.childCount; index++)
            {
                UIFrameBtn.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }

            for (int index = 0; index < UIPenColorBtn.transform.childCount; index++)
            {
                UIPenColorBtn.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void FinishSelfi()
    {
        selfistate = SelfiState.None;

        gamemanager.jaemilangmode.capturemode.QRCodeImage.transform.parent.gameObject.SetActive(false);
        countI.gameObject.SetActive(false);

        Lightvalue.value = 0;
        Totalvalue.value = 0;

        if (SelfiPhoto.GetComponent<RawImage>().texture != null)
        {
            SelfiPhoto.GetComponent<RawImage>().texture = null;
        }
        if (SelfiCam.gameObject.GetComponent<GrayScale>().enabled == true)
        {
            SelfiCam.gameObject.GetComponent<GrayScale>().enabled = false;
        }

        if (drawing.enabled == true)
        {
            drawing.enabled = false;
        }

        Download_Btn.SetActive(false);
        gamemanager.Selfi_Obj.SetActive(false);
        gamemanager.CaptureBtn.transform.GetChild(0).gameObject.SetActive(false);
        CustomUI.SetActive(false);
        FinalCam.gameObject.SetActive(false);
        PhotoOrigin.gameObject.SetActive(false);



        if (FinalCam.transform.childCount != 0)
        {
            for (int index = FinalCam.transform.childCount - 1; index >= 0; index--)
            {
                Destroy(FinalCam.transform.GetChild(index).gameObject);
            }
        }

        if (SelfiCam.GetComponent<GrayScale>().enabled == true)
        {
            SelfiCam.GetComponent<GrayScale>().enabled = false;
        }

    }

    public void RemoveItem(GameObject item)
    {
        //Destroy(item.transform.parent.parent.gameObject);
        for (int index = 0; index < UIStickBtn.transform.childCount; index++)
        {
            if (item.transform.parent.parent.name.Contains(UIStickBtn.transform.GetChild(index).gameObject.name))
            {
                UIStickBtn.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        item.transform.parent.parent.gameObject.SetActive(false);

        s1--;

        SaveUndo(item.transform.parent.parent.gameObject.name, item.transform.parent.parent.gameObject, "Remove");

        SelectItem = null;
    }

    public void DontDrag()
    {
        if (SceneManager.GetActiveScene().name.Contains("XRMode"))
        {
            GameManager.UITouch = true;
        }
        else if (SceneManager.GetActiveScene().name.Contains("ClearMode"))
        {
            GameManager.UITouch = true;
        }

    }

    public void StartDrag()
    {
        if (SceneManager.GetActiveScene().name.Contains("XRMode"))
        {
            GameManager.UITouch = false;
        }
        else if (SceneManager.GetActiveScene().name.Contains("ClearMode"))
        {
            GameManager.UITouch = false;
        }
    }

    public void SaveUndo(string objname, GameObject obj, string type)
    {
        ProcedureNum++;
        switch (type)
        {
            case "Make":
                state = new CustomProcedure(objname, CustomProcedure.customtype.Make, obj.transform.position, obj.transform.rotation, obj.transform.localScale.x);
                break;
            case "Remove":
                state = new CustomProcedure(objname, CustomProcedure.customtype.Remove, obj.transform.position, obj.transform.rotation, obj.transform.localScale.x);
                break;
            case "PRS":
                state = new CustomProcedure(objname, CustomProcedure.customtype.PRS, obj.transform.position, obj.transform.rotation, obj.transform.localScale.x);
                break;
        }

        //Debug.Log(ProcedureNum + " / " + state.CustomObj + " / " + state.CustomType + " / " + state.ObjPos + " / " + state.ObjRot + " / " + state.ObjScal);
        SelfiUndo.Add(ProcedureNum, state);
        //state = null;
    }

    public void DoSelfiundo()
    {
        if (ProcedureNum > 1)
        {
            if (SelfiUndo[ProcedureNum].CustomObj == SelfiUndo[ProcedureNum - 1].CustomObj)
            {
                if (SelfiUndo.TryGetValue(ProcedureNum - 1, out prestate))
                {
                    if (prestate.CustomObj.Contains("Line"))
                    {
                        for (int index = 0; index < FinalCam.transform.childCount; index++)
                        {
                            if (prestate.CustomObj == FinalCam.transform.GetChild(index).gameObject.name)
                            {
                                undoobj = FinalCam.transform.GetChild(index).gameObject;
                            }
                        }
                    }
                    else
                    {
                        for (int index = 0; index < StickerObj.transform.childCount; index++)
                        {
                            if (prestate.CustomObj == StickerObj.transform.GetChild(index).gameObject.name)
                            {
                                undoobj = StickerObj.transform.GetChild(index).gameObject;
                            }
                        }
                    }

                    switch (state.CustomType)
                    {
                        case CustomProcedure.customtype.Make:
                            Destroy(undoobj);
                            if (undoobj.name.Contains("Sticker"))
                            {
                                for (int index = 0; index < UIStickBtn.transform.childCount; index++)
                                {
                                    if (undoobj.name.Contains(UIStickBtn.transform.GetChild(index).gameObject.name))
                                    {
                                        UIStickBtn.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                    }
                                }
                            }
                            else if (undoobj.name.Contains("Frame"))
                            {
                                for (int index = 0; index < UIFrameBtn.transform.childCount; index++)
                                {
                                    if (undoobj.name.Contains(UIFrameBtn.transform.GetChild(index).name))
                                    {
                                        UIFrameBtn.transform.GetChild(index).transform.GetChild(0).gameObject.SetActive(false);
                                    }
                                }
                            }
                            break;
                        case CustomProcedure.customtype.Remove:
                            undoobj.SetActive(true);
                            break;
                        case CustomProcedure.customtype.PRS:
                            undoobj.transform.position = prestate.ObjPos;
                            undoobj.transform.rotation = prestate.ObjRot;
                            undoobj.transform.localScale = new Vector3(prestate.ObjScal, prestate.ObjScal, prestate.ObjScal);
                            break;
                    }
                }
            }
            else if (SelfiUndo[ProcedureNum].CustomObj != SelfiUndo[ProcedureNum - 1].CustomObj)
            {
                if (SelfiUndo[ProcedureNum].CustomObj.Contains("Line"))
                {
                    for (int index = 0; index < FinalCam.transform.childCount; index++)
                    {
                        if (SelfiUndo[ProcedureNum].CustomObj == FinalCam.transform.GetChild(index).gameObject.name)
                        {
                            undoobj = FinalCam.transform.GetChild(index).gameObject;
                        }
                    }
                    switch (SelfiUndo[ProcedureNum].CustomType)
                    {
                        case CustomProcedure.customtype.Make:
                            Destroy(undoobj);
                            break;
                        case CustomProcedure.customtype.Remove:
                            undoobj.SetActive(true);
                            break;
                    }
                }
                else if (!SelfiUndo[ProcedureNum].CustomObj.Contains("Line"))
                {
                    for (int index = 0; index < StickerObj.transform.childCount; index++)
                    {
                        if (SelfiUndo[ProcedureNum].CustomObj == StickerObj.transform.GetChild(index).gameObject.name)
                        {
                            undoobj = StickerObj.transform.GetChild(index).gameObject;
                        }
                    }

                    switch (SelfiUndo[ProcedureNum].CustomType)
                    {
                        case CustomProcedure.customtype.Make:
                            Destroy(undoobj);
                            if (undoobj.name.Contains("Sticker"))
                            {
                                for (int index = 0; index < UIStickBtn.transform.childCount; index++)
                                {
                                    if (undoobj.name.Contains(UIStickBtn.transform.GetChild(index).gameObject.name))
                                    {
                                        UIStickBtn.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                    }
                                }
                            }
                            else if (undoobj.name.Contains("Frame"))
                            {
                                for (int index = 0; index < UIFrameBtn.transform.childCount; index++)
                                {
                                    if (undoobj.name.Contains(UIFrameBtn.transform.GetChild(index).name))
                                    {
                                        UIFrameBtn.transform.GetChild(index).transform.GetChild(0).gameObject.SetActive(false);
                                    }
                                }
                            }
                            break;
                        case CustomProcedure.customtype.Remove:
                            undoobj.SetActive(true);
                            break;
                        case CustomProcedure.customtype.PRS:
                            for (int index = ProcedureNum - 1; index >= 0; index--)
                            {
                                if (SelfiUndo[index].CustomObj == SelfiUndo[ProcedureNum].CustomObj)
                                {
                                    if (SelfiUndo.TryGetValue(index, out prestate))
                                    {
                                        undoobj.transform.position = prestate.ObjPos;
                                        undoobj.transform.rotation = prestate.ObjRot;
                                        undoobj.transform.localScale = new Vector3(prestate.ObjScal, prestate.ObjScal, prestate.ObjScal);
                                    }
                                    break;
                                }
                            }
                            break;
                    }
                }
            }
        }
        else if (ProcedureNum == 1)
        {
            if (SelfiUndo[1].CustomObj.Contains("Line"))
            {
                undoobj = FinalCam.transform.GetChild(0).gameObject;
            }
            else
            {
                undoobj = StickerObj.transform.GetChild(0).gameObject;
                if (undoobj.name.Contains("Sticker"))
                {
                    for (int index = 0; index < UIStickBtn.transform.childCount; index++)
                    {
                        if (undoobj.name.Contains(UIStickBtn.transform.GetChild(index).gameObject.name))
                        {
                            UIStickBtn.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        }
                    }
                }
                else if (undoobj.name.Contains("Frame"))
                {
                    for (int index = 0; index < UIFrameBtn.transform.childCount; index++)
                    {
                        if (undoobj.name.Contains(UIFrameBtn.transform.GetChild(index).name))
                        {
                            UIFrameBtn.transform.GetChild(index).transform.GetChild(0).gameObject.SetActive(false);
                        }
                    }
                }
            }

            Destroy(undoobj);
        }
        undoobj = null;
        SelfiUndo.Remove(ProcedureNum);
        if (ProcedureNum != 0)
        {
            ProcedureNum--;
        }
        else if (ProcedureNum == 0)
        {
            if (FinalCam.transform.childCount != 0)
            {
                for (int index = 0; index < FinalCam.transform.childCount; index++)
                {
                    Destroy(FinalCam.transform.GetChild(index).gameObject);
                }
            }

            if (StickerObj.transform.childCount != 0)
            {
                for (int index = 0; index < StickerObj.transform.childCount; index++)
                {
                    Destroy(StickerObj.transform.GetChild(index).gameObject);
                }
            }
        }
    }

    public void ChangeLanguage()
    {
        for (int index = 0; index < CustomCategory.transform.childCount; index++)
        {
            if (GameManager.currentLang == GameManager.Language_enum.Korea)
            {
                for (int indexs = 0; indexs < 4; indexs++)
                {
                    if (index == indexs)
                    {
                        CustomCategory.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = Btn_language[indexs];
                    }
                }
            }
            else if (GameManager.currentLang != GameManager.Language_enum.Korea)
            {
                for (int indexs = 4; indexs < 8; indexs++)
                {
                    if (index == indexs - 4)
                    {
                        CustomCategory.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = Btn_language[indexs];
                    }
                }
            }
        }

        // 카테고리별 이미지 사이즈 수정
        CustomCategory.transform.GetChild(0).gameObject.GetComponent<Image>().SetNativeSize();
        CustomCategory.transform.GetChild(1).gameObject.GetComponent<Image>().SetNativeSize();
        CustomCategory.transform.GetChild(2).gameObject.GetComponent<Image>().SetNativeSize();
        CustomCategory.transform.GetChild(3).gameObject.GetComponent<Image>().SetNativeSize();

        // 카테고리별 위치 수정
        float firstcate_x = -837;
        float secondcate_x = firstcate_x + CustomCategory.transform.GetChild(0).gameObject.GetComponent<RectTransform>().rect.width + 12;
        float thirdcate_x = secondcate_x + CustomCategory.transform.GetChild(1).gameObject.GetComponent<RectTransform>().rect.width + 12;
        float fourthcate_x = thirdcate_x + CustomCategory.transform.GetChild(2).gameObject.GetComponent<RectTransform>().rect.width + 12;
        float total_Width = CustomCategory.transform.GetChild(0).gameObject.GetComponent<RectTransform>().rect.width + CustomCategory.transform.GetChild(1).gameObject.GetComponent<RectTransform>().rect.width
                            + CustomCategory.transform.GetChild(2).gameObject.GetComponent<RectTransform>().rect.width + CustomCategory.transform.GetChild(3).gameObject.GetComponent<RectTransform>().rect.width + 30;

        CustomCategory.transform.GetChild(0).localPosition = new Vector3(firstcate_x, CustomCategory.transform.GetChild(0).localPosition.y, 0);
        CustomCategory.transform.GetChild(1).localPosition = new Vector3(secondcate_x, CustomCategory.transform.GetChild(0).localPosition.y, 0);
        CustomCategory.transform.GetChild(2).localPosition = new Vector3(thirdcate_x, CustomCategory.transform.GetChild(0).localPosition.y, 0);
        CustomCategory.transform.GetChild(3).localPosition = new Vector3(fourthcate_x, CustomCategory.transform.GetChild(0).localPosition.y, 0);
        CustomCategory.GetComponent<RectTransform>().sizeDelta = new Vector2(total_Width, CustomCategory.GetComponent<RectTransform>().sizeDelta.y);  // 카테고리 스크롤뷰 크기 수정
    }

    public void CustomScroll(GameObject btn)
    {
        switch (btn.name)
        {
            case "All":
                CustomScrollbar.value = 0;
                break;
            case "Sticker":
                CustomScrollbar.value = 0.001f;
                break;
            case "Frame":
                CustomScrollbar.value = 0.597f;
                break;
            case "PenColor":
                CustomScrollbar.value = 0.928f;
                break;
        }

        for (int index = 0; index < CustomCategory.transform.childCount; index++)
        {
            CustomCategory.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        btn.transform.GetChild(0).gameObject.SetActive(true);
    }
}
