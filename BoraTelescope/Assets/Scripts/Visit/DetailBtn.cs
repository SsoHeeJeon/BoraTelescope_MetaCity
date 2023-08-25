using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailBtn : MonoBehaviour
{
    RectTransform rec;
    // Start is called before the first frame update
    void Start()
    {
        rec = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            rec.anchoredPosition = new Vector2(105, rec.anchoredPosition.y);
        }
        else
        {
            rec.anchoredPosition = new Vector2(55, rec.anchoredPosition.y);
        }
    }
}
