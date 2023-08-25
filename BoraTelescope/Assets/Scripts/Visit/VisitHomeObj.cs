using Amazon.IoTSiteWise.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisitHomeObj : MonoBehaviour
{
    public enum State
    {
        Idle,
        Animation,
    }
    public State state = 0;
    public float speed;
    RectTransform rec;
    [SerializeField]
    GameObject BackGround;
    [SerializeField]
    public GameObject CloseBtn;
    [SerializeField]
    Visitmanager visitmanager;
    public Vector2 PrePos;
    // Start is called before the first frame update
    void Start()
    {
        rec = GetComponent<RectTransform>();    
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Animation)
        {
            rec.anchoredPosition = Vector2.Lerp(rec.anchoredPosition, new Vector2(509, -200), Time.deltaTime* speed);
            rec.sizeDelta = Vector3.Lerp(rec.sizeDelta, new Vector3(640, 640, 640), Time.deltaTime* speed);

            float dist = Vector2.Distance(rec.anchoredPosition, new Vector2(509, -200));
            if(dist<10 && rec.sizeDelta.x>635)
            {
                rec.anchoredPosition = new Vector2(509, -200);
                rec.sizeDelta = new Vector3(640, 640, 640);
                CloseBtn.SetActive(true);
                state = State.Idle;
            }
        }
    }

    public void OnCLikcBtn()
    {
        visitmanager.gamemanager.WriteLog(LogSendServer.NormalLogCode.Visit_See, "GuestSee", GetType().ToString());
        PrePos = transform.localPosition;
        transform.parent = BackGround.transform;
        BackGround.GetComponent<ScrollRect>().content.gameObject.SetActive(false);
        //for(int i=0; i< visitmanager.VisitList.Count; i++)
        //{
        //    if (visitmanager.VisitList[i] != this.gameObject)
        //    {
        //        Destroy(visitmanager.VisitList[i]);
        //    }
        //}
        //visitmanager.VisitList.Clear();
        state = State.Animation;
        GetComponent<Button>().enabled = false;
    }

    public void OnClickCloseBtn()
    {
        transform.parent = BackGround.GetComponent<ScrollRect>().content;
        transform.localPosition = PrePos;
        transform.GetComponent<RectTransform>().sizeDelta = new Vector3(360f, 360f, 360f);
        PrePos = Vector2.zero;
        BackGround.GetComponent<ScrollRect>().content.gameObject.SetActive(true);
        GetComponent<Button>().enabled = true;
        CloseBtn.SetActive(false);
        //visitmanager.ReadImage();
        //Destroy(this.gameObject);
    }
}
