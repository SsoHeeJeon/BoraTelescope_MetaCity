using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Visitmanager : MonoBehaviour
{
    enum State
    {
        None,
        Left,
        Right,
    }
    State state = 0;
    [SerializeField]
    ScrollbarVisit scrollbarvisit;
    [SerializeField]
    GameObject Prefab;
    [SerializeField]
    GameObject BackGround;
    [SerializeField]
    GameObject DrawVisit;
    [SerializeField]
    GameObject VisitHome;
    [SerializeField]
    Image img;
    float ContentX = 100;
    float ContentY = -70;

    [SerializeField]
    GameObject[] LangObj;
    [SerializeField]
    GameObject BlackPencilColor;

    public List<GameObject> VisitList = new List<GameObject>();
    public GameManager gamemanager;
    // Start is called before the first frame update
    public string Currentyear;
    public string Currentmonth;
    private void Awake()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gamemanager.visitmanager = this;
    }

    void Start()
    {
        gamemanager.WriteLog(LogSendServer.NormalLogCode.Visit_Start, "GuestModeStart", GetType().ToString());
        Right.enabled = false;
        Right.gameObject.SetActive(false);
        DateTime StartTime = Convert.ToDateTime("2023-06-01 00:00:00");
        int result = DateTime.Compare(DateTime.Now, StartTime);
        if (result < 0)
        {
            Left.enabled = false;
            Left.gameObject.SetActive(false);
            state = State.None;
        }
        Currentmonth = DateTime.Now.ToString("MM");
        Currentyear = DateTime.Now.ToString("yyyy");
        string monthtext = Currentmonth;
        CurrentDate.text = Currentyear + "." + monthtext;
        ReadImage(Currentmonth);
        //gamemanager.visitmanager = this;
        gamemanager.UI_All.SetActive(true);
        if (gamemanager.Tip_Obj.activeSelf)
        {
            gamemanager.Tip_Obj.SetActive(false);
        }

        gamemanager.LanguageBar.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            LangObj[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(120, -24);
            LangObj[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(201, -24);
            LangObj[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(341, -24);
            LangObj[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(481, -24);
            LangObj[4].GetComponent<RectTransform>().anchoredPosition = new Vector2(604, -24);
        }
        else
        {
            LangObj[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(120, -24);
            LangObj[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(201, -24);
            LangObj[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(379.9f, -24);
            LangObj[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(560, -24);
            LangObj[4].GetComponent<RectTransform>().anchoredPosition = new Vector2(696, -24);
        }
    }

    [SerializeField]
    Text CurrentDate;
    [SerializeField]
    Button Right;
    [SerializeField]
    Button Left;

    public void MonthPlus()
    {
        ResetVisitList();
        Scrollindex = 0;
        scrollbarvisit.ResetPos();
        state = State.Right;
        gamemanager.WriteLog(LogSendServer.NormalLogCode.Visit_future, "Guestfuture", GetType().ToString());
        float a = int.Parse(Currentmonth);
        float b = int.Parse(Currentyear);
        a += 1;
        if (a >= 13)
        {
            a = 1;
            b += 1;
        }
        Currentmonth = a.ToString();
        Currentyear = b.ToString();
        Currentmonth = "0" + Currentmonth;
        string monthtext = Currentmonth;
        if (int.Parse(Currentmonth) >= 10)
        {
            monthtext = Currentmonth.Substring(1, 2);
        }
        CurrentDate.text = Currentyear + "." + monthtext;
        if (monthtext == DateTime.Now.ToString("MM") || "0" + Currentmonth == DateTime.Now.ToString("MM"))
        {
            BackGround.transform.GetChild(0).gameObject.SetActive(true);
            BackGround.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            BackGround.transform.GetChild(0).gameObject.SetActive(false);
            BackGround.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (a == int.Parse(DateTime.Now.ToString("MM")) && b == int.Parse(DateTime.Now.ToString("yyyy")))
        {
            Right.enabled = false;
            Right.gameObject.SetActive(false);
            state = State.None;
        }

        StartCoroutine(IEReadImage(Currentmonth));

        Left.enabled = true;
        Left.gameObject.SetActive(true);
    }

    public void MonthMinus()
    {
        ResetVisitList();
        Scrollindex = 0;
        scrollbarvisit.ResetPos();
        state = State.Left;
        gamemanager.WriteLog(LogSendServer.NormalLogCode.Visit_Past, "GuestPast", GetType().ToString());
        float a = int.Parse(Currentmonth);
        float b = int.Parse(Currentyear);
        a -= 1;
        if (a <= 0)
        {
            a = 12;
            b -= 1;
        }
        Currentmonth = a.ToString();
        Currentyear = b.ToString();
        Currentmonth = "0" + Currentmonth;
        string monthtext = Currentmonth;
        if (int.Parse(Currentmonth) >= 10)
        {
            monthtext = Currentmonth.Substring(1, 2);
        }
        CurrentDate.text = Currentyear + "." + monthtext;
        if (monthtext == DateTime.Now.ToString("MM") || "0" + Currentmonth == DateTime.Now.ToString("MM"))
        {
            BackGround.transform.GetChild(0).gameObject.SetActive(true);
            BackGround.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            BackGround.transform.GetChild(0).gameObject.SetActive(false);
            BackGround.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (a - 1 == int.Parse(DateTime.Now.ToString("MM")) && b < int.Parse(DateTime.Now.ToString("yyyy")))
        {
            Left.enabled = false;
            Left.gameObject.SetActive(false);
            state = State.None;
        }
        DateTime StartTime = Convert.ToDateTime("2023-06-01 00:00:00");
        int result = DateTime.Compare(Convert.ToDateTime(Currentyear + "/" + monthtext), StartTime);
        if (result < 0)
        {
            Left.enabled = false;
            Left.gameObject.SetActive(false);
            state = State.None;
        }
        StartCoroutine(IEReadImage(Currentmonth));
        Right.enabled = true;
        Right.gameObject.SetActive(true);
    }

    public void ReturnMonth()
    {
        ResetVisitList();
        Scrollindex = 0;
        scrollbarvisit.ResetPos();
        state = State.None;
        Left.enabled = true;
        Left.gameObject.SetActive(true);
        Right.enabled = false;
        Right.gameObject.SetActive(false);
        Currentmonth = DateTime.Now.ToString("MM");
        Currentyear = DateTime.Now.ToString("yyyy");
        CurrentDate.text = Currentyear + "." + Currentmonth;
        BackGround.transform.GetChild(0).gameObject.SetActive(true);
        BackGround.transform.GetChild(1).gameObject.SetActive(false);
        StartCoroutine(IEReadImage(Currentmonth));
    }

    public void ResetVisitList()
    {
        for (int i = 0; i < VisitList.Count; i++)
        {
            Destroy(VisitList[i].gameObject);
        }
        VisitList.Clear();
    }

    int Scrollindex = 0;
    bool scrollbarcheck;
    public void ScrollRectValue(Vector2 value)
    {
        if (value.y < 0.01f)
        {
            if (!scrollbarcheck)
            {
                scrollbarcheck = true;
                StartCoroutine(IEReadImage(Currentmonth));
            }
        }

    }

    IEnumerator IEReadImage(string month)
    {
        yield return new WaitForSeconds(0.1f);
        ReadImage(month);
    }


    public void ReadImage(string Month)
    {
        if (Month.Substring(0, 1) == "0")
        {
            if (int.Parse(Month) >= 10)
            {
                Month = Month.Substring(1, 2);
            }
        }

        int index = 0;
        if (Scrollindex == 0)
        {
            ContentX = 468;
            ContentY = -183;
        }

        int indexcount = -1;
        if (Scrollindex == 0)
        {

            if (Scrollindex + 7 >= gamemanager.gameObject.GetComponent<Visitinfo>().list[int.Parse(Month) - 1].Count)
            {
                indexcount = gamemanager.gameObject.GetComponent<Visitinfo>().list[int.Parse(Month) - 1].Count;
            }
            else
            {
                indexcount = Scrollindex + 7;
            }
        }
        else
        {
            if (Scrollindex + 8 >= gamemanager.gameObject.GetComponent<Visitinfo>().list[int.Parse(Month) - 1].Count)
            {
                indexcount = gamemanager.gameObject.GetComponent<Visitinfo>().list[int.Parse(Month) - 1].Count;
            }
            else
            {
                indexcount = Scrollindex + 8;
            }
        }
        for (int i = Scrollindex; i < indexcount; i++)
        {
            print(int.Parse(Month) - 1);
            if (gamemanager.gameObject.GetComponent<Visitinfo>().list[int.Parse(Month) - 1][i].Contains(".png"))
            {
                byte[] byteTexture = System.IO.File.ReadAllBytes("C:/Visit/" + Month + "/" + gamemanager.gameObject.GetComponent<Visitinfo>().list[int.Parse(Month) - 1][i]);
                if (byteTexture.Length > 0)
                {
                    string str = gamemanager.gameObject.GetComponent<Visitinfo>().list[int.Parse(Month) - 1][i].Substring(0, 4);
                    print(str);
                    if (str != DateTime.Now.ToString("yyyy"))
                    {
                        if (Month == DateTime.Now.ToString("MM"))
                        {
                            Directory.Delete("C:/Visit/" + Month, true);
                            break;
                        }
                    }
                    Texture2D tex = new Texture2D(0, 0);
                    tex.LoadImage(byteTexture);
                    Rect rect = new Rect(0, 0, tex.width, tex.height);
                    Sprite sp = Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f));
                    GameObject obj = Instantiate(Prefab);
                    VisitList.Add(obj);
                    obj.SetActive(true);
                    obj.transform.parent = BackGround.transform;
                    obj.GetComponent<Image>().sprite = sp;
                    obj.GetComponent<RectTransform>().sizeDelta = new Vector2(360, 360);
                    if (DrawVisit.activeSelf)
                    {
                        if (index == 0)
                        {
                            img.sprite = sp;
                            img.GetComponent<VisitImg>().state = VisitImg.State.Small;
                            img.gameObject.SetActive(true);
                            index++;
                        }
                    }

                    obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(ContentX, ContentY);
                    ContentX += obj.GetComponent<RectTransform>().sizeDelta.x;
                    ContentX += 48;
                    BackGround.GetComponent<RectTransform>().sizeDelta = new Vector2(1704, -ContentY + ((360) + 60));
                    if (ContentX + obj.GetComponent<RectTransform>().sizeDelta.x >= 1704)
                    {
                        ContentX = 60;
                        ContentY -= obj.GetComponent<RectTransform>().sizeDelta.y;
                        ContentY -= 60;
                    }
                }
            }
        }

        if (indexcount == 0)
        {
            BackGround.GetComponent<RectTransform>().sizeDelta = new Vector2(1704, -ContentY + ((360) + 60));
            BackGround.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        //foreach (FileInfo File in di.GetFiles().Reverse())
        //{
        //    if (File.Name.Contains(".png"))
        //    {
        //        byte[] byteTexture = System.IO.File.ReadAllBytes("C:/Visit/" + Month + "/" + File.Name);
        //        if (byteTexture.Length > 0)
        //        {
        //            string str = File.Name.Substring(0, 4);
        //            print(str);
        //            if (str != DateTime.Now.ToString("yyyy"))
        //            {
        //                if(Month == DateTime.Now.ToString("MM"))
        //                {
        //                    Directory.Delete("C:/Visit/" + Month, true);
        //                    print("연도 다름");
        //                    break;
        //                }
        //                else
        //                {
        //                    print("월 다름");
        //                }

        //            }
        //            else
        //            {
        //                print("연도 같음");
        //            }
        //            Texture2D tex = new Texture2D(0, 0);
        //            tex.LoadImage(byteTexture);
        //            Rect rect = new Rect(0, 0, tex.width, tex.height);
        //            Sprite sp = Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f));
        //            GameObject obj = Instantiate(Prefab);
        //            VisitList.Add(obj);
        //            obj.SetActive(true);
        //            obj.transform.parent = BackGround.transform;
        //            obj.GetComponent<Image>().sprite = sp;
        //            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(360, 360);
        //            if (DrawVisit.activeSelf)
        //            {
        //                if (index == 0)
        //                {
        //                    img.sprite = sp;
        //                    img.GetComponent<VisitImg>().state = VisitImg.State.Small;
        //                    gamemanager.transform.GetChild(1).gameObject.SetActive(false);
        //                    img.gameObject.SetActive(true);
        //                    index++;
        //                }
        //            }

        //            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(ContentX, ContentY);
        //            ContentX += obj.GetComponent<RectTransform>().sizeDelta.x;
        //            ContentX += 48;
        //            BackGround.GetComponent<RectTransform>().sizeDelta = new Vector2(1704, -ContentY + ((360) + 60));
        //            BackGround.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //            if (ContentX + obj.GetComponent<RectTransform>().sizeDelta.x >= 1704)
        //            {
        //                ContentX = 60;
        //                ContentY -= obj.GetComponent<RectTransform>().sizeDelta.y;
        //                ContentY -= 60;
        //            }
        //            //Destroy(sp);
        //            //Destroy(tex);
        //            //0.571
        //        }
        //    }
        //}
        scrollbarcheck = false;
        Scrollindex += 8;
    }

    public void ReadImageList(string Month)
    {
        if (!Directory.Exists("C:/Visit/" + Month))
        {
            Directory.CreateDirectory("C:/Visit/" + Month);
        }

        int index = 0;
        ContentX = 468;
        ContentY = -183;

        DirectoryInfo di = new DirectoryInfo("C:/Visit/" + Month);
        foreach (FileInfo File in di.GetFiles().Reverse())
        {
            if (File.Name.Contains(".png"))
            {
                byte[] byteTexture = System.IO.File.ReadAllBytes("C:/Visit/" + Month + "/" + File.Name);
                if (byteTexture.Length > 0)
                {
                    Texture2D tex = new Texture2D(0, 0);
                    tex.LoadImage(byteTexture);
                    Rect rect = new Rect(0, 0, tex.width, tex.height);
                    Sprite sp = Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f));
                    GameObject obj = Instantiate(Prefab);
                    VisitList.Insert(0, obj);// Add(obj);
                    obj.SetActive(true);
                    obj.transform.parent = BackGround.transform;
                    obj.GetComponent<Image>().sprite = sp;
                    obj.GetComponent<RectTransform>().sizeDelta = new Vector2(360, 360);
                    if (DrawVisit.activeSelf)
                    {
                        if (index == 0)
                        {
                            img.sprite = sp;
                            img.GetComponent<VisitImg>().state = VisitImg.State.Small;
                            img.gameObject.SetActive(true);
                            index++;
                        }
                    }

                    obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(ContentX, ContentY);
                    ContentX += obj.GetComponent<RectTransform>().sizeDelta.x;
                    ContentX += 48;
                    BackGround.GetComponent<RectTransform>().sizeDelta = new Vector2(1704, -ContentY + ((360) + 60));
                    BackGround.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    if (ContentX + obj.GetComponent<RectTransform>().sizeDelta.x >= 1704)
                    {
                        ContentX = 60;
                        ContentY -= obj.GetComponent<RectTransform>().sizeDelta.y;
                        ContentY -= 60;
                    }
                    //Destroy(sp);
                    //Destroy(tex);
                    //0.571
                }
                break;
            }
        }

        for (int i = 1; i < VisitList.Count; i++)
        {
            VisitList[i].gameObject.SetActive(true);
            VisitList[i].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(ContentX, ContentY);
            ContentX += VisitList[i].gameObject.GetComponent<RectTransform>().sizeDelta.x;
            ContentX += 48;
            BackGround.GetComponent<RectTransform>().sizeDelta = new Vector2(1704, -ContentY + ((360) + 60));
            BackGround.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            if (ContentX + VisitList[i].gameObject.GetComponent<RectTransform>().sizeDelta.x >= 1704)
            {
                ContentX = 60;
                ContentY -= VisitList[i].gameObject.GetComponent<RectTransform>().sizeDelta.y;
                ContentY -= 60;
            }
        }
    }


    public void OnClickAddBtn()
    {
        ResetVisitList();
        Scrollindex = 0;
        scrollbarvisit.ResetPos();
        gamemanager.WriteLog(LogSendServer.NormalLogCode.Visit_Write, "GuestWrite", GetType().ToString());
        //DrawVisit.transform.position = Camera.main.transform.position;
        GetComponent<VisitCapture>().ren.BackGround.GetComponent<MeshRenderer>().material = GetComponent<VisitCapture>().ren.WhiteMat;
        GetComponent<VisitCapture>().ren.Prefab.GetComponent<LineRenderer>().material = GetComponent<VisitCapture>().ren.BlackMat;
        for (int i = 0; i < VisitList.Count; i++)
        {
            VisitList[i].gameObject.SetActive(true);
        }
        //VisitList.Clear();
        VisitHome.SetActive(false);
        DrawVisit.SetActive(true);
        GetComponent<VisitCapture>().ren.OnClcikColor(BlackPencilColor);
    }

    public void OnclickStoreBtn()
    {
        //ReadImage();
        ReadImageList(Currentmonth);
        VisitHome.SetActive(true);
        DrawVisit.SetActive(false);
        gamemanager.WriteLog(LogSendServer.NormalLogCode.Visit_Save, "GuestSave", GetType().ToString());
    }

    public void OnCLickVisitBtn()
    {
        //if (VisitList.Count == 0)
        //{
        //    ReadImage(Currentmonth);
        //}

        if (DrawVisit.transform.gameObject.activeSelf)
        {
            NoticeWindow.NoticeWindowOpen("VisitCancel");
        } else if (VisitHome.activeSelf)
        {
            try
            {
                if (BackGround.transform.parent.parent.GetChild(3).GetComponent<VisitHomeObj>().CloseBtn.activeSelf)
                {
                    BackGround.transform.parent.parent.GetChild(3).GetComponent<VisitHomeObj>().OnClickCloseBtn();
                    BackGround.GetComponent<RectTransform>().anchoredPosition = new Vector2(BackGround.GetComponent<RectTransform>().anchoredPosition.x, 0);
                }
            }
            catch
            {
                BackGround.transform.parent.parent.GetComponent<ScrollRect>().enabled = false;
                BackGround.transform.parent.parent.GetComponent<ScrollRect>().enabled = true;
                BackGround.GetComponent<RectTransform>().anchoredPosition = new Vector2(BackGround.GetComponent<RectTransform>().anchoredPosition.x, 0);
            }
        }
    }

    public void RealOut()
    {
        DrawVisit.transform.gameObject.SetActive(false);
        VisitHome.SetActive(true);
        GetComponent<VisitCapture>().ren.OnClickReset();
        BackGround.GetComponent<RectTransform>().anchoredPosition = new Vector2(BackGround.GetComponent<RectTransform>().anchoredPosition.x, 0);
        try
        {
            if (BackGround.transform.parent.parent.GetChild(3).GetComponent<VisitHomeObj>().CloseBtn.activeSelf)
            {
                BackGround.transform.parent.parent.GetChild(3).GetComponent<VisitHomeObj>().OnClickCloseBtn();
            }
        }
        catch
        {

        }
    }

    public void MenuBtn(GameObject Obj)
    {
        for (int i = 0; i < Obj.transform.parent.childCount; i++)
        {
            Obj.transform.parent.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
        Obj.transform.GetChild(0).gameObject.SetActive(true);
    }


    [SerializeField]
    RectTransform Screc;
    [SerializeField]
    GameObject Menu;
    public void MenuScrollRange()
    {
        //if(Screc.anchoredPosition.x > -51)
        //{
        //    for (int i = 0; i < Menu.transform.childCount; i++)
        //    {
        //        Menu.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        //    }
        //    Menu.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        //}
        //else if(Screc.anchoredPosition.x >-1800)
        //{
        //    for (int i = 0; i < Menu.transform.childCount; i++)
        //    {
        //        Menu.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        //    }
        //    Menu.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        //}
        //else if(Screc.anchoredPosition.x > -2800)
        //{
        //    for (int i = 0; i < Menu.transform.childCount; i++)
        //    {
        //        Menu.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        //    }
        //    Menu.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        //}
        //else
        //{
        //    for (int i = 0; i < Menu.transform.childCount; i++)
        //    {
        //        Menu.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        //    }
        //    Menu.transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
        //}
    }
    [SerializeField]
    public GameObject WholeColor;
    [SerializeField]
    public GameObject PaperColor;
    [SerializeField]
    public GameObject StickerColor;
    [SerializeField]
    public GameObject PencilColor;
    [SerializeField]
    public GameObject BackGroundColor;

    public void OnClickWholeBtn()
    {
        Screc.parent.GetComponent<ScrollRect>().enabled = false;
        Screc.anchoredPosition = new Vector2(-50, 50);
        Screc.parent.GetComponent<ScrollRect>().enabled = true;
        WholeColor.SetActive(true);
        PaperColor.SetActive(false);
        StickerColor.SetActive(false);
        PencilColor.SetActive(false);
        BackGroundColor.SetActive(false);
    }

    public void OnClickPaperBtn()
    {
        WholeColor.SetActive(false);
        PaperColor.SetActive(true);
        StickerColor.SetActive(false);
        PencilColor.SetActive(false);
        BackGroundColor.SetActive(false);
    }

    public void OnClickStickerBtn()
    {
        //Screc.parent.GetComponent<ScrollRect>().enabled = false;
        //Screc.anchoredPosition = new Vector2(-1800, 50);
        //Screc.parent.GetComponent<ScrollRect>().enabled = true;
        WholeColor.SetActive(false);
        PaperColor.SetActive(false);
        StickerColor.SetActive(true);
        PencilColor.SetActive(false);
        BackGroundColor.SetActive(false);
    }

    public void OnClickrPencilBtn()
    {
        WholeColor.SetActive(false);
        PaperColor.SetActive(false);
        StickerColor.SetActive(false);
        PencilColor.SetActive(true);
        BackGroundColor.SetActive(false);
        //Screc.parent.GetComponent<ScrollRect>().enabled = false;
        //Screc.anchoredPosition = new Vector2(-3370, 50);
        //Screc.parent.GetComponent<ScrollRect>().enabled = true;
    }

    public void OnClickBackGroundBtn()
    {
        WholeColor.SetActive(false);
        PaperColor.SetActive(false);
        StickerColor.SetActive(false);
        PencilColor.SetActive(false);
        BackGroundColor.SetActive(true);
    }

    [SerializeField]
    GameObject Leftsp;
    [SerializeField]
    GameObject Rightsp;
    [SerializeField]
    RectTransform IconMenu;

    public void OnClickLeftRight(GameObject obj)
    {
        if (obj.name == "Right")
        {
            IconMenu.anchoredPosition = new Vector2(1309, 439);
            Leftsp.SetActive(true);
            Rightsp.SetActive(false);
            Leftsp.transform.GetChild(0).gameObject.SetActive(false);
            Rightsp.transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (obj.name == "Left")
        {
            IconMenu.anchoredPosition = new Vector2(553, 439);
            Leftsp.SetActive(false);
            Rightsp.SetActive(true);
            Leftsp.transform.GetChild(0).gameObject.SetActive(false);
            Rightsp.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public GameObject Confirm_K;
    public GameObject Confirm_E;

    public void OnClickComplete()
    {
        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            Confirm_K.SetActive(true);
            Confirm_E.SetActive(false);
            VisitCapture.PopObject = Confirm_K;
        }
        else
        {
            Confirm_K.SetActive(false);
            Confirm_E.SetActive(true);
            VisitCapture.PopObject = Confirm_E;
        }
    }

    public void OnClickUpbtn()
    {
        BackGround.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }
}
