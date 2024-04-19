using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable All

public class  altarwarmanager : MonoBehaviour
{
    private static altarwarmanager _instance = null;
    public static altarwarmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(altarwarmanager)) as altarwarmanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }
    public decimal totaldmg;



    public Text MyDmg;
    public Text MyRank;

    public void Bt_StartAltarWar()
    {
        if (mapmanager.Instance.islocating)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/맵이동중불가"),alertmanager.alertenum.주의);
            return;
        }
        DateTime nowtime = DateTime.Now;


        if (nowtime.Hour >= 0 && nowtime.Hour < 6)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/성물파괴이용불가"), alertmanager.alertenum.일반);
            return;
        }

            MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
            if (mapdata_Now.maptype != "0")
            {
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/사냥터만가능"), alertmanager.alertenum.주의);
                return;
            }

            if (Application.platform == RuntimePlatform.Android)
            {

                if (!Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.성물전쟁)) return;

            }
            QuestManager.Instance.AddCount(1, "content1");
            Debug.Log("성물전쟁시작");
        totaldmg = 0;
        dropsid.Clear();
        dropshowmany.Clear();
        mapmanager.Instance.LocateMap("10000");
    }

    public GameObject RewardPanel;
    public List<string> dropsid = new List<string>();
    public List<int> dropshowmany = new List<int>();
    public itemiconslot[] ItemSlots;

    public void AddLoot(string id, int count)
    {
        if(dropsid.Contains(id))
        {
            var i = dropsid.IndexOf(id);
            dropshowmany[i] += count;
        }
        else
        {
            dropsid.Add(id);
            dropshowmany.Add(count);
        }
    }

    public void FinishAltarWarLoot()
    {
        StartCoroutine(showitemicon());
    }
    WaitForSeconds waits = new WaitForSeconds(0.2f);
    WaitForSeconds waits2 = new WaitForSeconds(1.2f);
    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator showitemicon()
    {
        int Howmany = 25 * altarwarmanager.Instance.GetDmgReward();
        Inventory.Instance.AddItem("1712", Howmany,false);
        altarwarmanager.Instance.AddLoot("1712",Howmany );
        LogManager.LogAltarWar(Howmany,altarwarmanager.Instance.totaldmg);
        Savemanager.Instance.SaveInventory();
        foreach (var t in ItemSlots)
        {
            t.gameObject.SetActive(false);
        }

        MyDmg.text = dpsmanager.convertNumber(totaldmg);//;.ToString("N0");
        MyRank.text = "";
        SendQueue.Enqueue(Backend.URank.User.GetMyRank, RankingManager.Instance.RankUUID[0], callback =>
        {
            // 이후 처리
            if (callback.IsSuccess())
            {
                JsonData jsondata = callback.FlattenRows();
                MyRank.text = string.Format(Inventory.GetTranslate("UI/위"), jsondata[0]["rank"].ToString());
            }
        });
       // achievemanager.Instance.AddCount(Acheves.성물전쟁완료);
        RewardPanel.SetActive(true);
        yield return waits2;
        for (int i = 0; i < dropsid.Count;i++)
        {
            yield return waits;
            ItemSlots[i].gameObject.SetActive(true);
            Debug.Log(dropshowmany[i]);
            ItemSlots[i].Refresh(dropsid[i],dropshowmany[i],false,true);
        }
        alertmanager.Instance.NotiCheck_Altar();
        Savemanager.Instance.Save();
    }



    public int GetDmgReward()
    {
        if (totaldmg > 100000 && totaldmg < 10000000)
        {
            return 1;
        }
        else if (totaldmg > 10000000 && totaldmg <= 50000000)
        {
            return 2;
        }
        else if (totaldmg > 50000000 && totaldmg <= 200000000)
        {
            return 3;
        }
        else if (totaldmg > 200000000 && totaldmg <= 50000000)
        {
            return 4;
        }
        else if (totaldmg > 200000000 && totaldmg <= 400000000)
        {
            return 5;
        }
        else if (totaldmg > 400000000 && totaldmg < 1000000000)
        {
            return 6;
        }
        else if (totaldmg > 1000000000 && totaldmg < 10000000000)
        {
            return 7;
        }
        else if (totaldmg > 10000000000 && totaldmg < 5000000000)
        {
            return 8;
        }
        else if (totaldmg > 5000000000 && totaldmg < 10000000000)
        {
            return 9;
        }
        else if (totaldmg > 10000000000 && totaldmg < 50000000000)
        {
            return 10;
        }
        else if (totaldmg > 50000000000 && totaldmg < 100000000000)
        {
            return 11;
        }
        else if (totaldmg > 100000000000 && totaldmg < 500000000000)
        {
            return 12;
        }
        else if (totaldmg > 500000000000 && totaldmg < 1000000000000)
        {
            return 13;
        }
        else if (totaldmg > 1000000000000 && totaldmg < 5000000000000)
        {
            return 14;
        }
        else if (totaldmg > 5000000000000 && totaldmg < 10000000000000)
        {
            return 15;
        }
        else if (totaldmg > 10000000000000 && totaldmg < 100000000000000)
        {
            return 16;
        }
        else if (totaldmg > 100000000000000 && totaldmg < 300000000000000)
        {
            return 17;
        }
        else if (totaldmg > 300000000000000 && totaldmg < 500000000000000)
        {
            return 18;
        }
        else if (totaldmg > 500000000000000 && totaldmg < 800000000000000)
        {
            return 19;
        }
        else if (totaldmg > 800000000000000 && totaldmg < 1000000000000000)
        { 
            return 20;
        }
        else if (totaldmg > 1000000000000000 && totaldmg < 1500000000000000)
        {
            return 21;
        }
        else if (totaldmg > 1500000000000000 && totaldmg < 2000000000000000)
        {
            return 22;
        }
        else if (totaldmg > 2000000000000000 && totaldmg < 2500000000000000)
        {
            return 23;
        }
        else if (totaldmg > 2500000000000000 && totaldmg < 3000000000000000)
        {
            return 24;
        }
        else if (totaldmg > 3000000000000000 && totaldmg < 3500000000000000)
        {
            return 25;
        }
        else if (totaldmg > 3500000000000000 && totaldmg < 4000000000000000)
        {
            return 26;
        }
        else if (totaldmg > 4000000000000000 && totaldmg < 4500000000000000)
        {
            return 27;
        }
        else if (totaldmg > 4500000000000000 && totaldmg < 5000000000000000)
        {
            return 28;
        }
        else if (totaldmg > 5000000000000000 && totaldmg < 5500000000000000)
        {
            return 29;
        }
        else if (totaldmg > 5500000000000000 && totaldmg < 6000000000000000)
        {
            return 30;
        }
        else if (totaldmg > 6000000000000000 && totaldmg < 6500000000000000)
        {
            return 31;
        }
        else if (totaldmg > 6500000000000000 && totaldmg < 7000000000000000)
        {
            return 32;
        }
        else if (totaldmg > 7000000000000000 && totaldmg < 7500000000000000)
        {
            return 33;
        }
        else if (totaldmg > 7500000000000000 && totaldmg < 8000000000000000)
        {
            return 34;
        }
        else if (totaldmg > 8000000000000000 && totaldmg < 8500000000000000)
        {
            return 35;
        }
        else if (totaldmg > 8500000000000000 && totaldmg < 9000000000000000)
        {
            return 36;
        }
        else if (totaldmg > 9000000000000000 && totaldmg < 9500000000000000)
        {
            return 37;
        }
        else if (totaldmg > 9500000000000000 && totaldmg < 10000000000000000)
        {
            return 38;
        }
        else if (totaldmg > 10000000000000000 && totaldmg < 11000000000000000)
        {
            return 39;
        }
        else if (totaldmg > 11000000000000000 && totaldmg < 12000000000000000)
        {
            return 40;
        }
        else if (totaldmg > 12000000000000000 && totaldmg < 13000000000000000)
        {
            return 41;
        }
        else if (totaldmg > 13000000000000000 && totaldmg < 14000000000000000)
        {
            return 42;
        }
        else if (totaldmg > 14000000000000000 && totaldmg < 15000000000000000)
        {
            return 43;
        }
        else if (totaldmg > 15000000000000000 && totaldmg < 16000000000000000)
        {
            return 44;
        }
        else if (totaldmg > 16000000000000000 && totaldmg < 17000000000000000)
        {
            return 45;
        }
        else if (totaldmg > 17000000000000000 && totaldmg < 18000000000000000)
        {
            return 46;
        }
        else if (totaldmg > 18000000000000000 && totaldmg < 19000000000000000)
        {
            return 47;
        }
        else if (totaldmg > 19000000000000000 && totaldmg < 20000000000000000)
        {
            return 48;
        }
        else if (totaldmg > 20000000000000000)
        {
            return 50;
        }
        return 1;
    }
    
}
