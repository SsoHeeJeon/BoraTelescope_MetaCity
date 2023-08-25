using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Drawing : MonoBehaviour
{
    public SelfiFunction selfifunction;
    public GameObject LineBase;

    public Material SelectColor; //Material for Line Renderer
    public Material Pen_Red; //Material for Line Renderer
    public Material Pen_Orange; //Material for Line Renderer
    public Material Pen_Yellow; //Material for Line Renderer
    public Material Pen_LightGreen; //Material for Line Renderer
    public Material Pen_Green; //Material for Line Renderer
    public Material Pen_LightBlue; //Material for Line Renderer
    public Material Pen_Blue; //Material for Line Renderer
    public Material Pen_Purple; //Material for Line Renderer
    public Material Pen_Black; //Material for Line Renderer
    public Material Pen_White; //Material for Line Renderer

    private LineRenderer curLine;  //Line which draws now
    private int positionCount = 2;  //Initial start and end position
    private Vector3 PrevPos = Vector3.zero; // 0,0,0 position variable
    int layercount;
    Vector3 mousePos;
    bool PauseDraw = false;

    // Update is called once per frame
    void Update()
    {
        if (PauseDraw == false)
        {
            DrawMouse();
        }
    }

    void DrawMouse()
    {
        if (Input.touchCount != 0)
        {
            mousePos = selfifunction.FinalCam.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 3f));

            Debug.Log(mousePos.x + " / " + mousePos.y);


            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //if (mousePos.y > -175)
                {
                    createLine(mousePos);
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                connectLine(mousePos);
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (curLine != null)
                {
                    //SelfiFunction.s1++;
                    curLine.gameObject.SetActive(true);
                    curLine.transform.parent = selfifunction.FinalCam.transform;
                    curLine.transform.localPosition = new Vector3(0, 0, 0);
                    //selfifunction.FinalCam.gameObject.GetComponent<Drawing>().enabled = false;
                    selfifunction.SaveUndo(curLine.gameObject.name, curLine.gameObject, "Make");
                }
            }
        }
        /*
        Vector3 mousePos = selfifunction.FinalCam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            createLine(mousePos);
        }
        else if (Input.GetMouseButton(0))
        {
            connectLine(mousePos);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            curLine.gameObject.SetActive(true);
            curLine.transform.parent = selfifunction.FinalCam.transform;
            curLine.transform.localPosition = new Vector3(0, 0, 0);
        }*/
    }

    public void Dontdraw()
    {
        GameManager.UITouch = true;
        PauseDraw = true;
    }

    public void RestartDraw()
    {
        GameManager.UITouch = true;
        PauseDraw = false;
    }

    void createLine(Vector3 mousePos)
    {
        positionCount = 2;
        GameObject line = Instantiate(LineBase);
        line.name = "Line_" + SelfiFunction.s1;
        LineRenderer lineRend = line.GetComponent<LineRenderer>();
        line.layer = 10;
        if (SelectColor == null)
        {
            SelectColor = Pen_White;
        }

        if (selfifunction.SelectPenImg.gameObject.activeSelf)
        {
            selfifunction.SelectPenImg.gameObject.SetActive(false);
        }
        
        if(selfifunction.FinalCam.transform.childCount == 0)
        {
            layercount = 0;
        }
        
        line.transform.parent = selfifunction.FinalCam.transform;
        line.transform.position = mousePos;

        lineRend.startWidth = 5f;
        lineRend.endWidth = 5f;
        lineRend.numCornerVertices = 10;
        lineRend.numCapVertices = 10;
        lineRend.material = SelectColor;

        SelfiFunction.s1 += 1;
        lineRend.sortingOrder = SelfiFunction.s1;

        lineRend.SetPosition(0, new Vector3(mousePos.x, mousePos.y, 480 - SelfiFunction.s1));
        lineRend.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 480 - SelfiFunction.s1));
        /*
        if (SceneManager.GetActiveScene().name.Contains("XRMode"))
        {
            lineRend.SetPosition(0, new Vector3(mousePos.x, mousePos.y, 480 - SelfiFunction.s1));
            lineRend.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 480 - SelfiFunction.s1));
        } else if (SceneManager.GetActiveScene().name.Contains("ClearMode"))
        {
            lineRend.SetPosition(0, new Vector3(mousePos.x, mousePos.y, -1180 - SelfiFunction.s1));
            lineRend.SetPosition(1, new Vector3(mousePos.x, mousePos.y, -1180 - SelfiFunction.s1));
        }
        */
        curLine = lineRend;
    }

    void connectLine(Vector3 mousePos)
    {
        if (PrevPos != null && Mathf.Abs(Vector3.Distance(PrevPos, mousePos)) >= 0.0001f)
        {
            PrevPos = mousePos;
            positionCount++;
            curLine.positionCount = positionCount;

            curLine.SetPosition(positionCount - 1, new Vector3(mousePos.x, mousePos.y, 480 - SelfiFunction.s1));

            /*
            if (SceneManager.GetActiveScene().name.Contains("XRMode"))
            {
                curLine.SetPosition(positionCount - 1, new Vector3(mousePos.x, mousePos.y, 480 - SelfiFunction.s1));
            } else if (SceneManager.GetActiveScene().name.Contains("ClearMode"))
            {
                curLine.SetPosition(positionCount - 1, new Vector3(mousePos.x, mousePos.y, -1180 - SelfiFunction.s1));
            }*/
        }

    }
}
