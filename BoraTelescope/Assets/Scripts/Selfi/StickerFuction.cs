using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerFuction : MonoBehaviour
{
    public SelfiFunction selfifunc;
    public GameObject rotbtn;
    public GameObject removebtn;

    private void Start()
    {
        selfifunc = GameObject.Find("Selfi").GetComponent<SelfiFunction>();
    }

    public void selectSkicker(GameObject stick)
    {
        if (selfifunc.StickerObj.transform.childCount != 0)
        {
            for (int index = 0; index < selfifunc.StickerObj.transform.childCount; index++)
            {
                selfifunc.StickerObj.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                selfifunc.StickerObj.transform.GetChild(index).gameObject.GetComponent<Button>().enabled = true;
            }
        }

        if (!stick.transform.GetChild(0).gameObject.activeSelf)
        {
            stick.transform.GetChild(0).gameObject.SetActive(true);
            stick.GetComponent<Button>().enabled = false;
        }
        selfifunc.SelectItem = stick;
    }

    public void SetSticker(GameObject btn)
    {
        SelfiFunction.s1++;
        selfifunc.drawing.enabled = false;

        if (selfifunc.SelectPenImg.gameObject.activeSelf)
        {
            selfifunc.SelectPenImg.gameObject.SetActive(false);
        }

        for (int index = 0; index < selfifunc.UIPenColorBtn.transform.childCount; index++)
        {
            selfifunc.UIPenColorBtn.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (selfifunc.StickerObj.transform.childCount != 0)
        {
            for (int index = 0; index < selfifunc.StickerObj.transform.childCount; index++)
            {
                selfifunc.StickerObj.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                selfifunc.StickerObj.transform.GetChild(index).gameObject.GetComponent<Button>().enabled = true;
            }
        }

        for (int index = 0; index < selfifunc.StrickerList.Count; index++)
        {
            if (btn.name == selfifunc.StrickerList[index].name)
            {
                GameObject obj = Instantiate(selfifunc.StrickerList[index]);
                obj.transform.SetParent(selfifunc.StickerObj.transform);
                obj.name = btn.name + "_" + SelfiFunction.s1;
                obj.transform.localPosition = new Vector3(0, 0, -30 - SelfiFunction.s1);
                obj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                obj.SetActive(true);

                selfifunc.SaveUndo(obj.name, obj, "Make");
            }
        }
    }

    public void SetPositionChange(GameObject obj)
    {
        selfifunc.drawing.enabled = false;
        PositionChange.PositonchangeStart = true;
        PositionChange.Imageobj = obj;
        selfifunc.rotation_obj.gameObject.SetActive(false);
        selfifunc.Scale_obj[0].gameObject.transform.parent.gameObject.SetActive(false);
        selfifunc.rotation_obj.raycastTarget = false;
        for (int index = 0; index < 4; index++)
        {
            selfifunc.Scale_obj[index].raycastTarget = false;
        }
        //selfifunc.SaveUndo(Imageobj, "PRS");
    }

    public void FinishPositionChange()
    {
        selfifunc.SaveUndo(PositionChange.Imageobj.name, PositionChange.Imageobj, "PRS");
        PositionChange.PositonchangeStart = false;
        PositionChange.Imageobj = null;
        selfifunc.SelectItem = null;
        selfifunc.rotation_obj.gameObject.SetActive(true);
        selfifunc.Scale_obj[0].gameObject.transform.parent.gameObject.SetActive(true);
        selfifunc.rotation_obj.raycastTarget = true;
        for (int index = 0; index < 4; index++)
        {
            selfifunc.Scale_obj[index].raycastTarget = true;
        }
    }

    public void SetRotationChange(GameObject obj)
    {
        selfifunc.drawing.enabled = false;
        RotationChange.RotationchangeStart = true;
        RotationChange.Imageobj = obj;
        //selfifunc.rotation_obj.color = new Color(1, 1, 1, 1);
        selfifunc.position_obj.raycastTarget = false;
        for (int index = 0; index < 4; index++)
        {
            selfifunc.Scale_obj[index].raycastTarget = false;
        }
        selfifunc.position_obj.gameObject.SetActive(false);
        selfifunc.Scale_obj[0].gameObject.transform.parent.gameObject.SetActive(false);
        rotbtn.transform.GetChild(0).gameObject.SetActive(true);
        //selfifunc.SaveUndo(Imageobj, "PRS");
    }

    public void FinishRotationChange()
    {
        selfifunc.SaveUndo(RotationChange.Imageobj.name, RotationChange.Imageobj, "PRS");
        RotationChange.RotationchangeStart = false;
        RotationChange.Imageobj = null;
        selfifunc.SelectItem = null;
        //selfifunc.rotation_obj.color = new Color(1, 1, 1, 0);
        selfifunc.position_obj.gameObject.SetActive(true);
        selfifunc.Scale_obj[0].gameObject.transform.parent.gameObject.SetActive(true);
        selfifunc.position_obj.raycastTarget = true;
        for (int index = 0; index < 4; index++)
        {
            selfifunc.Scale_obj[index].raycastTarget = true;
        }
        rotbtn.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetScaleChange(GameObject obj)
    {
        selfifunc.drawing.enabled = false;
        ScaleChange.ScalechangeStart = true;
        ScaleChange.Imageobj = obj;
        //selfifunc.Scale_obj.color = new Color(1, 1, 1, 1);
        selfifunc.rotation_obj.gameObject.SetActive(false);
        selfifunc.position_obj.gameObject.SetActive(false);
        selfifunc.rotation_obj.raycastTarget = false;
        selfifunc.position_obj.raycastTarget = false;

        //selfifunc.SaveUndo(Imageobj, "PRS");
    }

    public void FinishScaleChange()
    {
        selfifunc.SaveUndo(ScaleChange.Imageobj.name, ScaleChange.Imageobj, "PRS");
        ScaleChange.ScalechangeStart = false;
        ScaleChange.Imageobj = null;
        selfifunc.SelectItem = null;
        //selfifunc.Scale_obj.color = new Color(1, 1, 1, 1);
        selfifunc.rotation_obj.gameObject.SetActive(true);
        selfifunc.position_obj.gameObject.SetActive(true);
        selfifunc.rotation_obj.raycastTarget = true;
        selfifunc.position_obj.raycastTarget = true;
    }

    public void RemoveSticker(GameObject obj)
    {
        selfifunc.RemoveItem(obj);
    }
}
