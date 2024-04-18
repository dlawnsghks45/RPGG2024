using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;

public class Panelcheck : MonoBehaviour
{
    public UIToggle[] FieldTG;
    public UIToggle[] DungeonTG;
    public UIToggle[] RaidTG;



    public void RefreshFieldToggle()
    {
        if (PlayerBackendData.Instance.GetFieldLv() >= 0)
        {
            FieldTG[0].IsOn = true;
            FieldTG[0].ExecuteClick();
        }
        if (PlayerBackendData.Instance.GetFieldLv() >= 33)
        {
            FieldTG[1].IsOn = true;
            FieldTG[1].ExecuteClick();
        }
        if (PlayerBackendData.Instance.GetFieldLv() >= 67)
        {
            FieldTG[2].IsOn = true;
            FieldTG[2].ExecuteClick();
        }
        if (PlayerBackendData.Instance.GetFieldLv() >= 101)
        {
            FieldTG[3].IsOn = true;
            FieldTG[3].ExecuteClick();
        }
    }

    public void RefreshDungeonToggle()
    {
        if(PlayerBackendData.Instance.GetAdLv() > 30)
            DungeonManager.Instance.Bt_SelectDungeon(DungeonDB.Instance.Find_maprank("30").id);
        else
        {
            if(PlayerBackendData.Instance.GetAdLv() > 2)
            DungeonManager.Instance.Bt_SelectDungeon(DungeonDB.Instance.Find_maprank(PlayerBackendData.Instance.GetAdLv().ToString()).id);
        }
        
        if (PlayerBackendData.Instance.GetAdLv() >= 0)
        {
            DungeonTG[0].IsOn = true;
            DungeonTG[0].ExecuteClick();
        }
        if (PlayerBackendData.Instance.GetAdLv() >= 11)
        {
            DungeonTG[1].IsOn = true;
            DungeonTG[1].ExecuteClick();
        }
        if (PlayerBackendData.Instance.GetAdLv() >= 20)
        {
            DungeonTG[2].IsOn = true;
            DungeonTG[2].ExecuteClick();
        }
    }

    public void RefreshRaidToggle()
    {
        if (PlayerBackendData.Instance.GetAdLv() >= 0)
        {
            RaidTG[0].IsOn = true;
            RaidTG[0].ExecuteClick();
        }
        
        if (PlayerBackendData.Instance.GetAdLv() >= 11)
        {
            RaidTG[1].IsOn = true;
            RaidTG[1].ExecuteClick();
        }
        
        if (PlayerBackendData.Instance.GetAdLv() >= 17)
        {
            RaidTG[2].IsOn = true;
            RaidTG[2].ExecuteClick();
        }
        
        if (PlayerBackendData.Instance.GetAdLv() >= 24)
        {
            RaidTG[3].IsOn = true;
            RaidTG[3].ExecuteClick();
        }
    }
}
