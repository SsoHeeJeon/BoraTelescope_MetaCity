using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LightControl_v1;

public class SelfiLightControl : MonoBehaviour
{
    public enum LightONOFF
    {
        On, Off
    }
    public LightONOFF lightonoff;
    public GameManager gamemanager;
    public Slider ControlLIght;
    public Text LightState;

    // Start is called before the first frame update
    public void ReadytoStart()
    {
        gamemanager.WriteLog(LogSendServer.NormalLogCode.selfi_LightControl, "selfi_LightControl", GetType().ToString());
        if (LightControl.IsConnected == false)
        {
            gamemanager.WriteLog(LogSendServer.NormalLogCode.selfi_LightControl, "selfi_LightControl:Off", GetType().ToString());
            LightControl.Connect("COM8", 38400);
            CheckConnect();
        } else if(LightControl.IsConnected == true)
        {
            gamemanager.WriteLog(LogSendServer.NormalLogCode.selfi_LightControl, "selfi_LightControl:On", GetType().ToString());
            CheckConnect();
        }
    }

    public void CheckConnect()
    {
        if (LightControl.IsConnected == false)
        {
            GameManager.AnyError = true;
            NoticeWindow.NoticeWindowOpen("ErrorLight");
            gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_LightControl, "Fail_LightControl", GetType().ToString());
        }
        else if (LightControl.IsConnected == true)
        {
            GameManager.AnyError = false;
            gamemanager.WriteLog(LogSendServer.NormalLogCode.selfi_LightControl, "selfi_LightControl:On", GetType().ToString());
            LightOn();
        }
    }

    public void LightOn()
    {
        LightControl.LightON();
        ControlLIght.value = LightControl.LightState;
        lightonoff = LightONOFF.On;
    }

    public void OnOffControl()
    {
        switch (lightonoff)
        {
            case LightONOFF.On:
                LightControl.LightOFF();
                lightonoff = LightONOFF.Off;
                break;
            case LightONOFF.Off:
                ControlLIght.value = 10;
                LightControl.LightON();
                lightonoff = LightONOFF.On;
                break;
        }
    }

    public void LightOff()
    {
        LightControl.LightOFF();
        lightonoff = LightONOFF.Off;
        LightControl.DisConnect();
    }

    public void ChangeLightValue()
    {
        if (GameManager.AnyError == false)
        {
            LightControl.LightUpDown(20 - (int)ControlLIght.value);
            LightState.text = ((int)ControlLIght.value).ToString();
            gamemanager.WriteLog(LogSendServer.NormalLogCode.selfi_LightControl, "selfi_LightControl:" + (20 - (int)ControlLIght.value), GetType().ToString());
        }
    }
}
