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
        string dungeonid;
        if (PlayerBackendData.Instance.sotang_dungeon.Count == 0)
        {
            dungeonid = "3000";
        }
        else
        {
            dungeonid = PlayerBackendData.Instance.sotang_dungeon[^1];
            DungeonManager.Instance.Bt_SelectDungeon(dungeonid);
        }
  

        if (int.Parse(MapDB.Instance.Find_id(dungeonid).maprank) >= 0)
        {
            DungeonTG[0].IsOn = true;
        }

        if (int.Parse(MapDB.Instance.Find_id(dungeonid).maprank) >= 11)
        {
            DungeonTG[1].IsOn = true;
        }

        if (int.Parse(MapDB.Instance.Find_id(dungeonid).maprank) >= 20)
        {
            DungeonTG[2].IsOn = true;
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
