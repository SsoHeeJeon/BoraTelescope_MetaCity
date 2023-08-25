using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosBackGround : MonoBehaviour
{
    Image img;
    List<string> list = new List<string>();

    public List<Sprite> splist = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        list.Add("Apsan");
        list.Add("Aegibong");
        print(ContentsInfo.ContentsName);
        for(int i=0; i<list.Count; i++)
        {
            if(ContentsInfo.ContentsName == list[i])
            {
                img.sprite = splist[i];
            }
        }
    }
}
