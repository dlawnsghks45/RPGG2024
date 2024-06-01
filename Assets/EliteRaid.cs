using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EliteRaid : MonoBehaviour
{
    //½Ì±ÛÅæ¸¸µé±â.
    private static EliteRaid _instance = null;
    public static EliteRaid Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(EliteRaid)) as EliteRaid;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }
    
    public countpanel LevelCount;
    public itemiconslot[] Reward;
    public itemiconslot Needitem;

    public Text Hp;

    public GameObject Lock;
    public Text LockRank;
    
    public string nowmon = "5031";

    private void Start()
    {
        LevelCount.SetCount(PlayerBackendData.Instance.ContentLevel[10] + 1);
        LevelCount.Maxcount = LevelCount.nowcount;
        Refresh();
    }


    public void Refresh()
    {
        if (PlayerBackendData.Instance.GetAdLv() >= 32)
        {
            Lock.SetActive(false);
        }
        else
        {
            Lock.SetActive(true);
            LockRank.text = $"{Inventory.GetTranslate("UI8/Àá±è·©Å©")} {32}";
        }
        
        RefreshHp();
        ShowReward();
    }

    void RefreshHp()
    {
        Hp.text = dpsmanager.convertNumber(GetHp(decimal.Parse(monsterDB.Instance.Find_id(nowmon).hp),LevelCount.nowcount-1));
    }

    private decimal nowhp;
    public Decimal GetHp(decimal hp,int num)
    {
        decimal a = (decimal)(hp* (decimal)math.pow(1.5f,num));
        nowhp = a;
        return a;
    }
    public Decimal GetHp()
    {
        return nowhp;
    }

    public float GetAtk()
    {
        float atk = float.Parse(monsterDB.Instance.Find_id(nowmon).dmg) +  (float.Parse(monsterDB.Instance.Find_id(nowmon).dmg) * (0.05f * LevelCount.nowcount-1));
        return atk;
    }

    public decimal GetPercent()
    {
        return 1+(0.2m * (LevelCount.nowcount-1));
    }

    public string needitemid;
    public void ShowReward()
    {
        if (LevelCount.nowcount is >= 0 and < 15)
        {
            needitemid = "1752";
        }
        else  if (LevelCount.nowcount is >= 15 and < 30)
        {
            needitemid = "1753";
        }

        Needitem.Refresh(needitemid, 1,false,false,false);

        foreach (var VARIABLE in Reward)
        {
            VARIABLE.gameObject.SetActive(false);
        }

        List<MonDropDB.Row> data = MonDropDB.Instance.FindAll_id(monsterDB.Instance.Find_id(nowmon).dropid);
        

        for (int i = 0; i < data.Count; i++)
        {

            decimal howmanycount = bool.Parse(data[i].Ispercent)
                ? decimal.Parse(data[i].maxhowmany) * GetPercent()
                : decimal.Parse(data[i].maxhowmany);

//            Debug.Log(howmanycount);
            Reward[i].Refresh(data[i].itemid, howmanycount, false, false, false);
            Reward[i].gameObject.SetActive(true);
        }
    }

    public void Bt_StartRaid()
    {
        RaidManager.Instance.dungeondropsid.Clear();
        RaidManager.Instance.dungeondropshowmany.Clear();
        
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/ÄÜÅÙÃ÷ Áß ºÒ°¡"), alertmanager.alertenum.ÁÖÀÇ);
            return;
        }

        int index = PlayerBackendData.Instance.CheckItemCount(needitemid);

        if (index != 0)
        {
            RaidManager.Instance.raidmonsterid = "5031";
            PlayerBackendData.Instance.RemoveItem(needitemid, 1);
            mapmanager.Instance.LocateMap("5031");
            Savemanager.Instance.SaveInventory();
            Savemanager.Instance.Save();
        }
        else
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/¾ÆÀÌÅÛÀÌºÎÁ·"), alertmanager.alertenum.ÀÏ¹Ý);

    }
}
