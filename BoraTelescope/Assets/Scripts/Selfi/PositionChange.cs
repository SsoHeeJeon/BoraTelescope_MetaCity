using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChange : MonoBehaviour
{
    public SelfiFunction selfifunc;

    public static bool PositonchangeStart = false;
    Vector3 startTouch;
    Vector3 moveposition;
    Vector3 changeposition;
    public static GameObject Imageobj;
    Vector3 startposition;

    // Update is called once per frame
    void Update()
    {
        if (PositonchangeStart == true)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                startTouch = Input.GetTouch(0).position;
                startposition = Imageobj.transform.position;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //터치정도에 따라 사각형 이동
                moveposition = Input.GetTouch(0).position;
                changeposition = (moveposition - startTouch);

                if (changeposition != new Vector3(0,0))
                {
                    if (startposition + changeposition != startposition)
                    {
                        Imageobj.transform.position = startposition + changeposition;
                    }
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                startposition = Imageobj.transform.position;
                FinishChange();
            }
        }
    }

    public void SetChange(GameObject obj)
    {
        selfifunc.drawing.enabled = false;
        PositonchangeStart = true;
        Imageobj = obj;
        selfifunc.rotation_obj.gameObject.SetActive(false);
        selfifunc.Scale_obj[0].gameObject.transform.parent.gameObject.SetActive(false);
        selfifunc.rotation_obj.raycastTarget = false;
        for (int index = 0; index < 4; index++)
        {
            selfifunc.Scale_obj[index].raycastTarget = false;
        }
        //selfifunc.SaveUndo(Imageobj, "PRS");
    }

    public void FinishChange()
    {
        selfifunc.SaveUndo(Imageobj.name, Imageobj, "PRS");
        PositonchangeStart = false;
        Imageobj = null;
        selfifunc.SelectItem = null;
        selfifunc.rotation_obj.gameObject.SetActive(true);
        selfifunc.Scale_obj[0].gameObject.transform.parent.gameObject.SetActive(true);
        selfifunc.rotation_obj.raycastTarget = true;
        for (int index = 0; index < 4; index++)
        {
            selfifunc.Scale_obj[index].raycastTarget = true;
        }
    }
}
