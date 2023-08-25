using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarVisit : MonoBehaviour
{

    void Start()
    {
        ResetPos();
    }

    public void SeeValue(Vector2 value)
    {
        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetComponent<RectTransform>().anchoredPosition.x, -750 + (570 * value.y));
    }

    public void ResetPos()
    {
        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetComponent<RectTransform>().anchoredPosition.x, -180);
    }
}
