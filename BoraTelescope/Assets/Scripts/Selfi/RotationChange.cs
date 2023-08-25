using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationChange : MonoBehaviour
{
    public SelfiFunction selfifunc;

    public static bool RotationchangeStart = false;
    Vector3 startTouch;
    Vector3 moveTouch;
    float startrotation;
    float changerotation;
    public static GameObject Imageobj;

    // Update is called once per frame
    void Update()
    {
        if (RotationchangeStart == true)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                startTouch = Input.GetTouch(0).position;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //터치정도에 따라 사각형 회전
                moveTouch = Input.GetTouch(0).position;

                changerotation = Mathf.Atan2((moveTouch.y - Camera.main.WorldToScreenPoint(Imageobj.transform.position).y), (moveTouch.x - Camera.main.WorldToScreenPoint(Imageobj.transform.position).x)) * 180 / Mathf.PI - 90;
                Imageobj.transform.rotation = Quaternion.Euler(0, 0, changerotation);
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                Imageobj.transform.rotation = Imageobj.transform.rotation;
                FinishChange();
            }
        }
    }

    public void SetChange(GameObject obj)
    {
        selfifunc.drawing.enabled = false;
        RotationchangeStart = true;
        Imageobj = obj;
        //selfifunc.rotation_obj.color = new Color(1, 1, 1, 1);
        selfifunc.position_obj.raycastTarget = false;
        for (int index = 0; index < 4; index++)
        {
            selfifunc.Scale_obj[index].raycastTarget = false;
        }
        selfifunc.position_obj.gameObject.SetActive(false);
        selfifunc.Scale_obj[0].gameObject.transform.parent.gameObject.SetActive(false);

        //selfifunc.SaveUndo(Imageobj, "PRS");
    }

    public void FinishChange()
    {
        selfifunc.SaveUndo(Imageobj.name, Imageobj, "PRS");
        RotationchangeStart = false;
        Imageobj = null;
        selfifunc.SelectItem = null;
        //selfifunc.rotation_obj.color = new Color(1, 1, 1, 0);
        selfifunc.position_obj.gameObject.SetActive(true);
        selfifunc.Scale_obj[0].gameObject.transform.parent.gameObject.SetActive(true);
        selfifunc.position_obj.raycastTarget = true;
        for (int index = 0; index < 4; index++)
        {
            selfifunc.Scale_obj[index].raycastTarget = true;
        }
    }
}
