using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckContentCount : MonoBehaviour
{
    public void CheckContentCountS()
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            return;
        }
        
        if (PlayerBackendData.Instance.PlayerAchieveData["A2001"].Curcount == 0 &&
            Timemanager.Instance.DailyContentCount[0].Equals(0))
        {
            Timemanager.Instance.AddDailyCount(Timemanager.ContentEnumDaily.º∫π∞¿¸¿Ô, 1);
        }
    }
}
