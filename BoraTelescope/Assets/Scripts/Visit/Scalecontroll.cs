using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;

public class Scalecontroll : MonoBehaviour
{
    RaycastHit hitinfo;
    [SerializeField]
    RenEvent ren;
    float startScale = 100;
    Vector3 lastPosition;
    public float value;

    public void OnMouseDrag()
    {
        print(1123124);
        if(ren.state == RenEvent.State.Scale)
        {
            if (Input.touchCount == 1)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                int layerMask = 1 << LayerMask.NameToLayer("Scale");
                if (Physics.Raycast(ray, out hitinfo, Mathf.Infinity, layerMask))
                {
                    Vector3 moveposition = hitinfo.point;
                    float changescale = Vector2.Distance(lastPosition, moveposition) * value;// - startScale;
                    print("chagescale = " + changescale);
                    //if (moveposition.x - lastPosition.x < 0 || moveposition.y - lastPosition.y < 0)
                    //{
                    //    changescale *= -1;
                    //}
                    if (changescale != 0)
                    {

                        transform.localScale = new Vector3(changescale, changescale, changescale);
                        transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 30, 200),
                        Mathf.Clamp(transform.localScale.y, 30, 200), Mathf.Clamp(transform.localScale.z, 30, 200));
                        //startScale = transform.localScale.x;


                    }
                }
                //transform.localScale = new Vector3(transform.localScale.x * deltaPosition.x*0.0001f, transform.localScale.y * deltaPosition.y*0.0001f, transform.localScale.z);
            }
        }
    }

    public void OnClickScaleBtn()
    {
        ren.state = RenEvent.State.Scale;
        startScale = transform.localScale.x;
        lastPosition = transform.GetComponent<RectTransform>().anchoredPosition;
        print("lastPosition = " + lastPosition + "123");
    }

    private void OnMouseUp()
    {
        ren.state = RenEvent.State.None;
    }
}
