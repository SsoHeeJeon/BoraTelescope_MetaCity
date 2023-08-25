using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitImg : MonoBehaviour
{
    public enum State
    {
        Idle,
        Small,
    }
    public State state = 0;
    RectTransform rec;
    public float speed = 1;
    [SerializeField]
    Visitmanager visitmanager;
    // Start is called before the first frame update
    void Start()
    {
        rec = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Small)
        {
            rec.anchoredPosition = Vector2.Lerp(rec.anchoredPosition, new Vector2(468, -183), Time.deltaTime * speed/1.5f);
            rec.sizeDelta = Vector3.Lerp(rec.sizeDelta, new Vector3(360, 360, 360), Time.deltaTime * speed/2);

            float dist = Vector2.Distance(rec.anchoredPosition, new Vector2(468, -183));
            print(dist);
            if (dist < 3 && rec.sizeDelta.x < 365f)
            {
                rec.anchoredPosition = new Vector2(468, -60);
                rec.sizeDelta = new Vector3(360, 360, 360);
                state = State.Idle;
                visitmanager.gamemanager.transform.GetChild(1).gameObject.SetActive(true);
                gameObject.SetActive(false);
                rec.anchoredPosition = new Vector2(509, -120);
                rec.sizeDelta = new Vector2(640, 640);
            }
        }
    }
}
