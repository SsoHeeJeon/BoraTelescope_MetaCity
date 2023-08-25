using UnityEngine;

public class ImageMove : MonoBehaviour
{
    RaycastHit hitinfo;
    [SerializeField]
    RenEvent ren;
    [SerializeField]
    StiManager sti;
    [SerializeField]
    Camera Visitcam;


    public GameObject Group;

    public float value;
    public float rotvalue;
    private void Start()
    {
        print(transform.position);
    }

    private void Update()
    {

        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -300, -5),
            Mathf.Clamp(transform.localPosition.y, -30, 265), transform.localPosition.z);
    }

    public void OnMouseDrag()
    {
        print(1);
        if(ren.state == RenEvent.State.None || ren.state == RenEvent.State.Sti)
        {
            Ray ray = Visitcam.ScreenPointToRay( Input.mousePosition );
           
            int layerMask = 1 << LayerMask.NameToLayer("Sti");
            if (Physics.Raycast(ray, out hitinfo, Mathf.Infinity, layerMask))
            {
                transform.position = hitinfo.point;
                Group.SetActive(false);
            }
        }
        else if (ren.state == RenEvent.State.Scale)
        {
            if (Input.touchCount==1)
            {
                Vector3 moveposition =  Input.mousePosition;

                float x = Mathf.Abs(lastPosition.x - moveposition.x);
                float y = Mathf.Abs(lastPosition.y - moveposition.y);
                float xy = (x + y) / 2;
                transform.localScale = new Vector3(xy, xy*0.68f, 1);
                transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 50, 350),
                Mathf.Clamp(transform.localScale.y, 34, 238), Mathf.Clamp(transform.localScale.z, 50, 350));
            }
        }
        else if(ren.state == RenEvent.State.Rot)
        {
            Vector2 moveTouch = Input.GetTouch(0).position;

            float changerotation = Mathf.Atan2((moveTouch.y - Visitcam.WorldToScreenPoint(transform.position).y), (moveTouch.x - Visitcam.WorldToScreenPoint(transform.position).x)) * 180 / Mathf.PI - 90;
            transform.rotation = Quaternion.Euler(0, 0, changerotation);
        }
    }

    private void OnMouseUp()
    {
        for(int i=0; i<sti.Stilist.Count; i++)
        {
            try
            {
                sti.Stilist[i].GetComponent<ImageMove>().Group.SetActive(false);
            }
            catch
            {
                Group.SetActive(true);
            }
        }
        if(sti.Stilist.Count ==0)
        {
            Group.SetActive(true);
        }
        Group.SetActive(true);
        ren.state = RenEvent.State.None;
    }

    private void OnMouseDown()
    {
        for (int i = 0; i < sti.Stilist.Count; i++)
        {
            try
            {
                sti.Stilist[i].GetComponent<ImageMove>().Group.SetActive(false);
                ren.PenTle.SetActive(false);
            }
            catch
            {
                Group.SetActive(true);
            }
        }
        if (sti.Stilist.Count == 0)
        {
            Group.SetActive(true);
        }
        Group.SetActive(true);
        ren.state = RenEvent.State.Sti;
    }

    Vector3 lastPosition;
    public void OnClickScaleBtn()
    {
        ren.state = RenEvent.State.Scale;
        lastPosition = transform.position;
        lastPosition = Visitcam.WorldToScreenPoint(lastPosition);
    }

    public void OnClickRotBtn()
    {
        ren.state = RenEvent.State.Rot;
    }

    public void OnClickClose()
    {
        if(ren.state !=RenEvent.State.Scale && ren.state != RenEvent.State.Rot)
        {
            ren.Wholelist.RemoveAt(ren.Wholelist.Count - 1);
            sti.Stilist.RemoveAt(sti.Stilist.Count - 1);
            ren.state = RenEvent.State.None;
            Destroy(this.gameObject);
        }
    }
}
