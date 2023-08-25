using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerMake : MonoBehaviour
{
    public SelfiFunction selfifuc;

    public Sprite[] Sticker;
    public GameObject Sticker_obj;
    public GameObject Stickerbtn_obj;
    public GameObject Stickerbtn_all_obj;

    public GameObject sticker_P;
    public GameObject stickerbtn_P;
    public GameObject stickerallbtn_P;

    public GameObject All_Frame;
    public GameObject All_Pen;

    public void ReadytoStart()
    {
        Sticker = new Sprite[Resources.LoadAll<Sprite>("Jamilang/Sprite/bora_Vetc_s.sticker").Length];
        Sticker = Resources.LoadAll<Sprite>("Jamilang/Sprite/bora_Vetc_s.sticker");

        selfifuc.StrickerList.Clear();
        All_Frame.transform.parent.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(196* (Sticker.Length + 14), All_Frame.transform.parent.parent.gameObject.GetComponent<RectTransform>().rect.height);
        All_Frame.transform.localPosition = new Vector3(196 * Sticker.Length, 0, 0);
        All_Pen.transform.localPosition = new Vector3(196 * (Sticker.Length+4), 0, 0);

        for (int index = 0; index < Sticker.Length; index++)
        {
            GameObject obj = Instantiate(Sticker_obj);
            GameObject obj_btn = Instantiate(Stickerbtn_obj);
            GameObject objall_btn = Instantiate(Stickerbtn_all_obj);

            obj.transform.SetParent(sticker_P.transform);
            obj.transform.position = new Vector3(0,0,0);
            obj.transform.localScale = new Vector3(1,1,1);
            obj_btn.transform.SetParent(stickerbtn_P.transform);
            obj_btn.transform.localScale = new Vector3(1, 1, 1);
            objall_btn.transform.SetParent(stickerallbtn_P.transform);
            objall_btn.transform.localScale = new Vector3(1, 1, 1);

            obj.name = "Sticker" + (index + 1).ToString();
            obj_btn.name = "Sticker" + (index + 1).ToString();
            objall_btn.name = "Sticker" + (index + 1).ToString();

            obj.GetComponent<Image>().sprite = Sticker[index];
            obj_btn.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = Sticker[index];
            obj_btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = obj_btn.name;
            objall_btn.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = Sticker[index];
            objall_btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = objall_btn.name;

            obj_btn.transform.localPosition = new Vector3(89 + 196 * index, 75, 0);
            objall_btn.transform.localPosition = new Vector3(89 + 196 * index, 75, 0);

            selfifuc.StrickerList.Add(obj);
            obj.SetActive(false);
        }
    }
}
