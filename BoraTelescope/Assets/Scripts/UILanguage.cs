using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILanguage : MonoBehaviour
{
    public Image Home_Btn;
    public Image Jamilang_Btn;
    public Image Jamilang_Btn_1;
    public Image Language_Btn;
    public Image Language_Btn_1;
    public Image Capture_Btn;
    public Image Capture_Btn_1;
    public Image Guide_Btn;
    public Image Guide_Btn_1;
    public Image Visit_Btn;
    public Image Visit_Btn_1;

    public Sprite Home_idle_K;
    public Sprite Jamilang_idle_K;
    public Sprite Jamilang_Select_K;
    public Sprite Language_idle_K;
    public Sprite Language_Select_K;
    public Sprite Capture_idle_K;
    public Sprite Capture_Select_K;
    public Sprite Guide_idle_K;
    public Sprite Guide_Select_K;
    public Sprite Visit_Idle_K;
    public Sprite Visit_Select_K;

    public Sprite Home_idle_E;
    public Sprite Jamilang_idle_E;
    public Sprite Jamilang_Select_E;
    public Sprite Language_idle_E;
    public Sprite Language_Select_E;
    public Sprite Capture_idle_E;
    public Sprite Capture_Select_E;
    public Sprite Guide_idle_E;
    public Sprite Guide_Select_E;
    public Sprite Visit_Idle_E;
    public Sprite Visit_Select_E;

    private void Start()
    {
        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            SelectKorea();
        }
        else if (GameManager.currentLang != GameManager.Language_enum.Korea)
        {
            NotSelectKorea();
        }
    }

    public void SelectKorea()
    {
        Home_Btn.sprite = Home_idle_K;
        Jamilang_Btn.sprite = Jamilang_idle_K;
        Jamilang_Btn_1.sprite = Jamilang_Select_K;
        Language_Btn.sprite = Language_idle_K;
        Language_Btn_1.sprite = Language_Select_K;
        Capture_Btn.sprite = Capture_idle_K;
        Capture_Btn_1.sprite = Capture_Select_K;
        Guide_Btn.sprite = Guide_idle_K;
        Guide_Btn_1.sprite = Guide_Select_K;
        Visit_Btn.sprite = Visit_Idle_K;
        Visit_Btn_1.sprite = Visit_Select_K;

        SetSize();
    }

    public void NotSelectKorea()
    {
        Home_Btn.sprite = Home_idle_E;
        Jamilang_Btn.sprite = Jamilang_idle_E;
        Jamilang_Btn_1.sprite = Jamilang_Select_E;
        Language_Btn.sprite = Language_idle_E;
        Language_Btn_1.sprite = Language_Select_E;
        Capture_Btn.sprite = Capture_idle_E;
        Capture_Btn_1.sprite = Capture_Select_E;
        Guide_Btn.sprite = Guide_idle_E;
        Guide_Btn_1.sprite = Guide_Select_E;
        Visit_Btn.sprite = Visit_Idle_E;
        Visit_Btn_1.sprite = Visit_Select_E;

        SetSize();
    }

    public void SetSize()
    {
        Home_Btn.SetNativeSize();
        Jamilang_Btn.SetNativeSize();
        Jamilang_Btn_1.SetNativeSize();
        Language_Btn.SetNativeSize();
        Language_Btn_1.SetNativeSize();
        Capture_Btn.SetNativeSize();
        Capture_Btn_1.SetNativeSize();
        Guide_Btn.SetNativeSize();
        Guide_Btn_1.SetNativeSize();
        Visit_Btn.SetNativeSize();
        Visit_Btn_1.SetNativeSize();
    }

    public void SetSel(GameObject btn)
    {
        switch (btn.name)
        {
            case "JaemilangMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Jamilang_Select_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Jamilang_Select_E;
                }
                break;
            case "Capture":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Capture_Select_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Capture_Select_E;
                }
                break;
            case "Language":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Language_Select_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Language_Select_E;
                }
                break;
            case "GuideMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Guide_Select_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Guide_Select_E;
                }
                break;
            case "VisitMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Visit_Select_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Visit_Select_E;
                }
                break;
        }
    }

    public void SetOrigin(GameObject btn)
    {
        switch (btn.name)
        {
            case "JaemilangMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Jamilang_idle_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Jamilang_idle_E;
                }
                break;
            case "Capture":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Capture_idle_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Capture_idle_E;
                }
                break;
            case "Language":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Language_idle_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Language_idle_E;
                }
                break;
            case "GuideMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Guide_idle_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Guide_idle_E;
                }
                break;
            case "VisitMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Visit_Idle_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Visit_Idle_E;
                }
                break;
        }
    }


    public void SceneChagneSetOrigin()
    {
        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            SelectKorea();
        }
        else if (GameManager.currentLang != GameManager.Language_enum.Korea)
        {
            NotSelectKorea();
        }
    }
}
