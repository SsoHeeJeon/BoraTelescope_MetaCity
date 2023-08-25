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

    // Start is called before the first frame update
    public void ReadytoStart()
    {
        if (LightControl.IsConnected == false)
        {
            LightControl.Connect("COM8", 38400);
            CheckConnect();
        }
    }

    public void CheckConnect()
    {
        if (LightControl.IsConnected == false)
        {
            GameManager.AnyError = true;
            NoticeWindow.NoticeWindowOpen("ErrorLight");
            //gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_Connect_Pantilt, "Fail_Connect_Pantilt", GetType().ToString());
        }
        else if (LightControl.IsConnected == true)
        {
            //gamemanager.WriteLog(LogSendServer.NormalLogCode.Connect_Pantilt, "Connect_Pantilt:On", GetType().ToString());
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
        LightControl.LightUpDown(21-(int)ControlLIght.value);
    }
}
