using Amazon.Runtime.Internal.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RenEvent : MonoBehaviour
{
    public enum State
    {
        None,
        Ren,
        Sti,
        Scale,
        Rot,
    }
    public State state = 0;

    public GameObject Prefab;
    [SerializeField]
    GameObject RenImage;
    [SerializeField]
    Camera Visitcam;
    [SerializeField]
    GameObject Back;
    [SerializeField]
    GameObject Back2;
    [SerializeField]
    GameObject Back3;
    [SerializeField]
    GameObject MenuReset;
    [SerializeField]
    GameObject MenuEraser;
    public Material WhiteMat;
    public Material BlackMat;

    [SerializeField]
    Material[] BackGroundMat;
    [SerializeField]
    Material[] PencilMat;
    public GameObject PencilImg;
    public GameObject EarserImg;

    public GameObject BackGround;
    public StiManager Sti;
    public Visitmanager visitmanager;

    List<Vector3> Renlist = new List<Vector3>();
    int index;
    LineRenderer CurrentRen;

    public List<LineRenderer> LRenlist = new List<LineRenderer>();
    public List<string> Wholelist = new List<string>();

    int sortindex = 0;

    public float z = 77.8f;

    private void Update()
    {
        if (Input.touchCount>=1)
        {
            if (Input.touches[0].phase == TouchPhase.Began && state == State.None && (PenTle.activeSelf|| !Sti.Sticheck))
            {
                //if(Sti.Stilist.Count>=1)
                //{
                //    try
                //    {
                //        for(int i=0; i<Sti.Stilist.Count; i++)
                //        {
                //            Sti.Stilist[i].GetComponent<ImageMove>().Group.SetActive(false);
                //            Sti.Stilist[i].GetComponent<MeshCollider>().enabled = false;
                //        }
                //    }
                //    catch
                //    {

                //    }
                //}
                RaycastHit hit;
                Ray ray = Visitcam.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.transform.gameObject.name == "RenPlane")
                    {
                        PencilImg.SetActive(false);
                        EarserImg.SetActive(false);
                        state= State.Ren;
                        GameObject obj = Instantiate(Prefab);
                        obj.SetActive(true);
                        obj.transform.parent = RenImage.transform;
                        LineRenderer ren = obj.GetComponent<LineRenderer>();
                        ren.sortingOrder = sortindex;
                        sortindex++;
                        Wholelist.Add("Ren");                       
                        LRenlist.Add(ren);
                        CurrentRen = ren;
                        //77.86166f
                        z -= 0.01f;
                        Renlist.Add(new Vector3(hit.point.x, hit.point.y, z));
                        index++;
                        CurrentRen.positionCount = index;
                        Vector3[] RenArr = Renlist.ToArray();
                        ren.SetPositions(RenArr);
                        print("dragStart");
                    }
                }
            }
            else if(Input.touches[0].phase == TouchPhase.Moved && state == State.Ren)
            {
                RaycastHit hit;
                Ray ray = Visitcam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.name == "RenPlane")
                    {
                        print(hit.point);
                        float x = 0;
                        if (hit.point.x < -28)
                        {
                            x = -28;
                        }
                        else
                        {
                            if (hit.point.x > 31.5f)
                            {
                                x = 31.5f;
                            }
                            else
                            {
                                x = hit.point.x;
                            }
                        }

                        float y = 0;
                        if (hit.point.y < -20)
                        {
                            y = -20;
                        }
                        else
                        {
                            if (hit.point.y > 40)
                            {
                                y = 40;
                            }
                            else
                            {
                                y = hit.point.y;
                            }
                        }

                        index++;
                        CurrentRen.positionCount = index;
                        print("z" + z);
                        Renlist.Add(new Vector3(x, y, z));
                        Vector3[] RenArr = Renlist.ToArray();
                        CurrentRen.SetPositions(RenArr);
                        print("draging");
                    }
                }
            }
            else if(Input.touches[0].phase == TouchPhase.Ended && state == State.Ren)
            {
                RaycastHit hit;
                Ray ray = Visitcam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.name == "RenPlane")
                    {
                        index = 0;
                        Renlist.Clear();
                        CurrentRen = null;
                        print("dragend");
                        state = State.None;
                    }
                }
                //if (Sti.Stilist.Count >= 1)
                //{
                //    try
                //    {
                //        for (int i = 0; i < Sti.Stilist.Count; i++)
                //        {
                //            Sti.Stilist[i].GetComponent<MeshCollider>().enabled = true;
                //        }
                //    }
                //    catch
                //    {

                //    }
                //}
            }
            else if(Input.touches[0].phase == TouchPhase.Ended)
            {
                //if (Sti.Stilist.Count >= 1)
                //{
                //    try
                //    {
                //        for (int i = 0; i < Sti.Stilist.Count; i++)
                //        {
                //            Sti.Stilist[i].GetComponent<MeshCollider>().enabled = true;
                //        }
                //    }
                //    catch
                //    {

                //    }
                //}
            }
        }
        else if(Input.touchCount==0)
        {
            if(CurrentRen!=null)
            {
                index = 0;
                Renlist.Clear();
                CurrentRen = null;
                print("dragend");
                state = State.None;
            }
        }
    }

    public void OnclickBack()
    {
        if(Input.touchCount <= 1 && CurrentRen == null)
        {
            if(Wholelist.Count>=1)
            {
                if (Wholelist[Wholelist.Count-1] == "Ren")
                {
                    if(LRenlist.Count>=1)
                    {
                        Destroy(LRenlist[LRenlist.Count - 1].gameObject);
                        LRenlist.RemoveAt(LRenlist.Count - 1);
                        sortindex--;
                    }
                }
                else
                {
                    if (Sti.Stilist.Count >= 1)
                    {
                        Destroy(Sti.Stilist[Sti.Stilist.Count-1].gameObject);
                        Sti.Stilist.RemoveAt(Sti.Stilist.Count-1);
                    }
                }
                Wholelist.RemoveAt(Wholelist.Count-1);
            }
        }
    }

    public void OnClickReset()
    {
        MenuReset.SetActive(false);
        MenuEraser.SetActive(true);
        PencilImg.SetActive(false);
        EarserImg.SetActive(false);
        EraserSprite.SetActive(false);
        for (int i=0; i<LRenlist.Count; i++)
        {
            Destroy(LRenlist[i].gameObject);
        }
        LRenlist.Clear();
        Wholelist.Clear();
        sortindex = 0;
        z = 77.8f;
        for (int i=0; i<Sti.Stilist.Count; i++)
        {
            Destroy(Sti.Stilist[i].gameObject);
        }
        Sti.Stilist.Clear();

        PenTle.SetActive(false);
        BackTle.SetActive(false);
        Sti.Sticheck = true;
        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[9];
    }
    [SerializeField]
    public GameObject PenTle;
    [SerializeField]
    GameObject BackTle;
    public void OnClcikColor(GameObject obj)
    {
        EarserImg.SetActive(false);
        PencilImg.SetActive(true);
        EraserSprite.SetActive(false);
        if (Sti.Stilist.Count >= 1)
        {
            try
            {
                for (int i = 0; i < Sti.Stilist.Count; i++)
                {
                    Sti.Stilist[i].GetComponent<ImageMove>().Group.SetActive(false);
                    Sti.Stilist[i].GetComponent<MeshCollider>().enabled = false;
                }
            }
            catch
            {

            }
        }
        if(visitmanager.WholeColor.activeSelf)
        {
            PenTle.transform.parent = visitmanager.WholeColor.transform.GetChild(0);
            PenTle.SetActive(true);
            PenTle.GetComponent<RectTransform>().anchoredPosition = new Vector2(obj.GetComponent<RectTransform>().anchoredPosition.x - 5, -75);
        }
        else if(visitmanager.PencilColor.activeSelf)
        {
            PenTle.transform.parent = visitmanager.PencilColor.transform;
            PenTle.SetActive(true);
            PenTle.GetComponent<RectTransform>().anchoredPosition = new Vector2(obj.GetComponent<RectTransform>().anchoredPosition.x - 5, -0);
        }
        Prefab.GetComponent<LineRenderer>().startWidth = 0.7f;
        Prefab.GetComponent<LineRenderer>().endWidth = 0.7f;
        switch (obj.name)
        {
            case "Red":
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (PencilMat[i].name.Contains("Red"))
                    {
                        Prefab.GetComponent<LineRenderer>().material = PencilMat[i];
                        PencilImg.GetComponent<Image>().sprite = PencilImg.GetComponent<PenColor>().red;
                        break;
                    }
                }
                break;
            case "Orange":
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (PencilMat[i].name.Contains("Orange"))
                    {
                        Prefab.GetComponent<LineRenderer>().material = PencilMat[i];
                        PencilImg.GetComponent<Image>().sprite = PencilImg.GetComponent<PenColor>().Orange;
                        break;
                    }
                }
                break;
            case "Yellow":
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (PencilMat[i].name.Contains("Yellow"))
                    {
                        Prefab.GetComponent<LineRenderer>().material = PencilMat[i];
                        PencilImg.GetComponent<Image>().sprite = PencilImg.GetComponent<PenColor>().Yellow;
                        break;
                    }
                }
                break;
            case "LightGreen":
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (PencilMat[i].name.Contains("LightGreen"))
                    {
                        Prefab.GetComponent<LineRenderer>().material = PencilMat[i];
                        PencilImg.GetComponent<Image>().sprite = PencilImg.GetComponent<PenColor>().LightGreen;
                        break;
                    }
                }
                break;
            case "Green":
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (PencilMat[i].name.Contains("Green"))
                    {
                        Prefab.GetComponent<LineRenderer>().material = PencilMat[i];
                        PencilImg.GetComponent<Image>().sprite = PencilImg.GetComponent<PenColor>().Green;
                        break;
                    }
                }
                break;
            case "LightBlue":
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (PencilMat[i].name.Contains("LightBlue"))
                    {
                        Prefab.GetComponent<LineRenderer>().material = PencilMat[i];
                        PencilImg.GetComponent<Image>().sprite = PencilImg.GetComponent<PenColor>().LightBlue;
                        break;
                    }
                }
                break;
            case "Blue":
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (PencilMat[i].name.Contains("Blue"))
                    {
                        Prefab.GetComponent<LineRenderer>().material = PencilMat[i];
                        PencilImg.GetComponent<Image>().sprite = PencilImg.GetComponent<PenColor>().Blue;
                        break;
                    }
                }
                break;
            case "PurPle":
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (PencilMat[i].name.Contains("PurPle"))
                    {
                        Prefab.GetComponent<LineRenderer>().material = PencilMat[i];
                        PencilImg.GetComponent<Image>().sprite = PencilImg.GetComponent<PenColor>().Purple;
                        break;
                    }
                }
                break;
            case "Black":
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (PencilMat[i].name.Contains("Black"))
                    {
                        Prefab.GetComponent<LineRenderer>().material = PencilMat[i];
                        PencilImg.GetComponent<Image>().sprite = PencilImg.GetComponent<PenColor>().Black;
                        break;
                    }
                }
                break;
        }
        index = 0;
        Renlist.Clear();
        CurrentRen = null;
        print("dragend");
        state = State.None;
    }

    public void OnClickBackGround(GameObject obj)
    {
        MenuEraser.SetActive(false);
        MenuReset.SetActive(false);
        state = State.None;
        if (visitmanager.WholeColor.activeSelf)
        {
            BackTle.transform.parent = visitmanager.WholeColor.transform.GetChild(0);
            BackTle.SetActive(true);
            BackTle.GetComponent<RectTransform>().anchoredPosition = new Vector2(obj.GetComponent<RectTransform>().anchoredPosition.x - 5, -75);
            BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 136);
        }
        else if (visitmanager.PaperColor.activeSelf)
        {
            BackTle.transform.parent = visitmanager.PaperColor.transform;
            BackTle.SetActive(true);
            BackTle.GetComponent<RectTransform>().anchoredPosition = new Vector2(obj.GetComponent<RectTransform>().anchoredPosition.x - 5, 3);
            BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 136);
        }
        else if (visitmanager.BackGroundColor.activeSelf)
        {
            BackTle.transform.parent = visitmanager.BackGroundColor.transform;
            BackTle.SetActive(true);
            BackTle.GetComponent<RectTransform>().anchoredPosition = new Vector2(obj.GetComponent<RectTransform>().anchoredPosition.x - 5, 5);
            BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 175);
        }
        Back.SetActive(false);
        Back2.SetActive(false);
        Back3.SetActive(false);
        switch (obj.name)
        {
            case "BackRed":
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 136);
                MenuEraser.SetActive(true);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("Red"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            try
                            {
                                if (LRenlist[j].material.color == BackGround.GetComponent<MeshRenderer>().material.color)
                                {
                                    LRenlist[j].material = BackGroundMat[i];
                                }
                            }
                            catch
                            {

                            }
                        }
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        if (Prefab.GetComponent<LineRenderer>().startWidth == 3)
                        {
                            Prefab.GetComponent<LineRenderer>().material = BackGroundMat[i];
                        }
                        break;
                    }
                }
                break;
            case "BackOrange":
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 136);
                MenuEraser.SetActive(true);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("Orange"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            try
                            {
                                if (LRenlist[j].material.color == BackGround.GetComponent<MeshRenderer>().material.color)
                                {
                                    LRenlist[j].material = BackGroundMat[i];
                                }
                            }
                            catch
                            {

                            }
                        }
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        if (Prefab.GetComponent<LineRenderer>().startWidth == 3)
                        {
                            Prefab.GetComponent<LineRenderer>().material = BackGroundMat[i];
                        }
                        break;
                    }
                }
                break;
            case "BackYellow":
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 136);
                MenuEraser.SetActive(true);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("Yellow"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            try
                            {
                                if (LRenlist[j].material.color == BackGround.GetComponent<MeshRenderer>().material.color)
                                {
                                    LRenlist[j].material = BackGroundMat[i];
                                }
                            }
                            catch
                            {

                            }
                        }
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        if (Prefab.GetComponent<LineRenderer>().startWidth == 3)
                        {
                            Prefab.GetComponent<LineRenderer>().material = BackGroundMat[i];
                        }
                        break;
                    }
                }
                break;
            case "BackLightGreen":
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 136);
                MenuEraser.SetActive(true);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("LightGreen"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            try
                            {
                                if (LRenlist[j].material.color == BackGround.GetComponent<MeshRenderer>().material.color)
                                {
                                    LRenlist[j].material = BackGroundMat[i];
                                }
                            }
                            catch
                            {

                            }
                        }
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        if (Prefab.GetComponent<LineRenderer>().startWidth == 3)
                        {
                            Prefab.GetComponent<LineRenderer>().material = BackGroundMat[i];
                        }
                        break;
                    }
                }
                break;
            case "BackGreen":
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 136);
                MenuEraser.SetActive(true);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("Green"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            try
                            {
                                if (LRenlist[j].material.color == BackGround.GetComponent<MeshRenderer>().material.color)
                                {
                                    LRenlist[j].material = BackGroundMat[i];
                                }
                            }
                            catch
                            {

                            }
                        }
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        if (Prefab.GetComponent<LineRenderer>().startWidth == 3)
                        {
                            Prefab.GetComponent<LineRenderer>().material = BackGroundMat[i];
                        }
                        break;
                    }
                }
                break;
            case "BackLightBlue":
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 136);
                MenuEraser.SetActive(true);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("LightBlue"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            try
                            {
                                if (LRenlist[j].material.color == BackGround.GetComponent<MeshRenderer>().material.color)
                                {
                                    LRenlist[j].material = BackGroundMat[i];
                                }
                            }
                            catch
                            {

                            }
                        }
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        if (Prefab.GetComponent<LineRenderer>().startWidth == 3)
                        {
                            Prefab.GetComponent<LineRenderer>().material = BackGroundMat[i];
                        }
                        break;
                    }
                }
                break;
            case "BackBlue":
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 136);
                MenuEraser.SetActive(true);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("Blue"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            try
                            {
                                if (LRenlist[j].material.color == BackGround.GetComponent<MeshRenderer>().material.color)
                                {
                                    LRenlist[j].material = BackGroundMat[i];
                                }
                            }
                            catch
                            {

                            }
                        }
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        if (Prefab.GetComponent<LineRenderer>().startWidth == 3)
                        {
                            Prefab.GetComponent<LineRenderer>().material = BackGroundMat[i];
                        }
                        break;
                    }
                }
                break;
            case "BackPurPle":
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 136);
                MenuEraser.SetActive(true);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("PurPle"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            try
                            {
                                if (LRenlist[j].material.color == BackGround.GetComponent<MeshRenderer>().material.color)
                                {
                                    LRenlist[j].material = BackGroundMat[i];
                                }
                            }
                            catch
                            {

                            }
                        }
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        if (Prefab.GetComponent<LineRenderer>().startWidth == 3)
                        {
                            Prefab.GetComponent<LineRenderer>().material = BackGroundMat[i];
                        }
                        break;
                    }
                }
                break;
            case "BackBlack":
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 136);
                MenuEraser.SetActive(true);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("Black"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            try
                            {
                                if (LRenlist[j].material.color == BackGround.GetComponent<MeshRenderer>().material.color)
                                {
                                    LRenlist[j].material = BackGroundMat[i];
                                }
                            }
                            catch
                            {

                            }
                        }
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        if (Prefab.GetComponent<LineRenderer>().startWidth == 3)
                        {
                            Prefab.GetComponent<LineRenderer>().material = BackGroundMat[i];
                        }
                        break;
                    }
                }
                break;
            case "BackGround1":
                MenuReset.SetActive(true);
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 175);
                Back.SetActive(true);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("BackGround1"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            Destroy(LRenlist[j].gameObject);
                        }
                        LRenlist.Clear();

                        for (int k = 0; k < Sti.Stilist.Count; k++)
                        {
                            Destroy(Sti.Stilist[k].gameObject);
                        }
                        Sti.Stilist.Clear();
                        Sti.Sticheck = true;
                        Wholelist.Clear();
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        break;
                    }
                }
                break;
            case "BackGround2":
                MenuReset.SetActive(true);
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 175);
                Back2.SetActive(true);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("BackGround2"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            Destroy(LRenlist[j].gameObject);
                        }
                        LRenlist.Clear();

                        for (int k = 0; k < Sti.Stilist.Count; k++)
                        {
                            Destroy(Sti.Stilist[k].gameObject);
                        }
                        Sti.Stilist.Clear();
                        Sti.Sticheck = true;
                        Wholelist.Clear();
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        break;
                    }
                }
                break;
            case "BackGround3":
                MenuReset.SetActive(true);
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 175);
                Back3.SetActive(true);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("BackGround3"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            Destroy(LRenlist[j].gameObject);
                        }
                        LRenlist.Clear();

                        for (int k = 0; k < Sti.Stilist.Count; k++)
                        {
                            Destroy(Sti.Stilist[k].gameObject);
                        }
                        Sti.Stilist.Clear();
                        Sti.Sticheck = true;
                        Wholelist.Clear();
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        break;
                    }
                }
                break;
            case "BackGround4":
                MenuReset.SetActive(true);
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 175);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("BackGround4"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            Destroy(LRenlist[j].gameObject);
                        }
                        LRenlist.Clear();

                        for (int k = 0; k < Sti.Stilist.Count; k++)
                        {
                            Destroy(Sti.Stilist[k].gameObject);
                        }
                        Sti.Stilist.Clear();
                        Sti.Sticheck = true;
                        Wholelist.Clear();
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        break;
                    }
                }
                break;
            case "BackGround5":
                MenuReset.SetActive(true);
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 175);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("BackGround5"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            Destroy(LRenlist[j].gameObject);
                        }
                        LRenlist.Clear();

                        for (int k = 0; k < Sti.Stilist.Count; k++)
                        {
                            Destroy(Sti.Stilist[k].gameObject);
                        }
                        Sti.Stilist.Clear();
                        Sti.Sticheck = true;
                        Wholelist.Clear();
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        break;
                    }
                }
                break;
            case "BackGround6":
                MenuReset.SetActive(true);
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 175);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("BackGround6"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            Destroy(LRenlist[j].gameObject);
                        }
                        LRenlist.Clear();

                        for (int k = 0; k < Sti.Stilist.Count; k++)
                        {
                            Destroy(Sti.Stilist[k].gameObject);
                        }
                        Sti.Stilist.Clear();
                        Sti.Sticheck = true;
                        Wholelist.Clear();
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        break;
                    }
                }
                break;
            case "BackGround7":
                MenuReset.SetActive(true);
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 175);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("BackGround7"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            Destroy(LRenlist[j].gameObject);
                        }
                        LRenlist.Clear();

                        for (int k = 0; k < Sti.Stilist.Count; k++)
                        {
                            Destroy(Sti.Stilist[k].gameObject);
                        }
                        Sti.Stilist.Clear();
                        Sti.Sticheck = true;
                        Wholelist.Clear();
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        break;
                    }
                }
                break;
            case "BackGround8":
                MenuReset.SetActive(true);
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 175);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("BackGround8"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            Destroy(LRenlist[j].gameObject);
                        }
                        LRenlist.Clear();

                        for (int k = 0; k < Sti.Stilist.Count; k++)
                        {
                            Destroy(Sti.Stilist[k].gameObject);
                        }
                        Sti.Stilist.Clear();
                        Sti.Sticheck = true;
                        Wholelist.Clear();
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        break;
                    }
                }
                break;
            case "BackGround9":
                MenuReset.SetActive(true);
                BackTle.GetComponent<RectTransform>().sizeDelta = new Vector2(197, 175);
                for (int i = 0; i < BackGroundMat.Length; i++)
                {
                    if (BackGroundMat[i].name.Contains("BackGround9"))
                    {
                        for (int j = 0; j < LRenlist.Count; j++)
                        {
                            Destroy(LRenlist[j].gameObject);
                        }
                        LRenlist.Clear();

                        for (int k = 0; k < Sti.Stilist.Count; k++)
                        {
                            Destroy(Sti.Stilist[k].gameObject);
                        }
                        Sti.Stilist.Clear();
                        Sti.Sticheck = true;
                        Wholelist.Clear();
                        BackGround.GetComponent<MeshRenderer>().material = BackGroundMat[i];
                        break;
                    }
                }
                break;
        }
    }

    public GameObject EraserSprite;
    public void OnClickEraser()
    {
        EarserImg.SetActive(true);
        PencilImg.SetActive(false);
        Sti.Sticheck = false;
        if (Sti.Stilist.Count >= 1)
        {
            try
            {
                for (int i = 0; i < Sti.Stilist.Count; i++)
                {
                    Sti.Stilist[i].GetComponent<ImageMove>().Group.SetActive(false);
                    Sti.Stilist[i].GetComponent<MeshCollider>().enabled = false;
                }
            }
            catch
            {
            }
        }
        Prefab.GetComponent<LineRenderer>().material = BackGround.GetComponent<MeshRenderer>().material;
        Prefab.GetComponent<LineRenderer>().startWidth = 3;
        Prefab.GetComponent<LineRenderer>().endWidth = 3;
        PenTle.SetActive(false);
        index = 0;
        Renlist.Clear();
        CurrentRen = null;
        print("dragend");
        state = State.None;
        EraserSprite.SetActive(true);
    }
}
