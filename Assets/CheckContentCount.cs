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
        
        if (PlayerBackendData.Instance.QuestCount[6] == 0 &&
            Timemanager.Instance.DailyContentCount[0].Equals(0))
        {
            Timemanager.Instance.AddDailyCount(Timemanager.ContentEnumDaily.¼º¹°ÀüÀï, 1);
            Debug.Log("È½¼ö ÃæÀü");
            Debug.Log(PlayerBackendData.Instance.QuestCount[6] );
            Debug.Log(Timemanager.Instance.DailyContentCount[0]);
        }
    }
}
