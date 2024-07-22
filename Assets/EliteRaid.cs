using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EliteRaid : MonoBehaviour
{
    //싱글톤만들기.
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

    public GameObject  SotangButtons;

    private void Start()
    {
        LevelCount.SetCount(PlayerBackendData.Instance.ContentLevel[10] + 1);
        LevelCount.Maxcount = LevelCount.nowcount;
        Refresh();
        SotangCount.nowcount = 1;
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
            LockRank.text = $"{Inventory.GetTranslate("UI8/잠김랭크")} {32}";
        }
        
        RefreshHp();
        ShowReward();
        ButtonCheck();
    }

    void ButtonCheck()
    {
        SotangButtons.SetActive(false);
        if (PlayerBackendData.Instance.ContentLevel[10] != 0)
        {
       //     Debug.Log(LevelCount.nowcount + "현재");
//            Debug.Log(PlayerBackendData.Instance.ContentLevel[10] + "미래");
            if (LevelCount.nowcount-1 < PlayerBackendData.Instance.ContentLevel[10])
                SotangButtons.SetActive(true);
        }
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
        if (LevelCount.nowcount is >= 0 and < 16)
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
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/콘텐츠 중 불가"), alertmanager.alertenum.주의);
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
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/아이템이부족"), alertmanager.alertenum.일반);

    }

    public UIView SotangView;
    public itemshowcountslot SotangItem;
    public countpanel SotangCount;

    public void Bt_StartSotang()
    {
        RaidManager.Instance.raidmonsterid = "5031";
        RaidManager.Instance.dungeondropsid.Clear();
        RaidManager.Instance.dungeondropshowmany.Clear();
        
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/사냥터만가능"), alertmanager.alertenum.주의);
            return;
        }

        if (mapmanager.Instance.islocating)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/맵이동중불가"), alertmanager.alertenum.주의);
            return;
        }

        if (!Settingmanager.Instance.CheckServerOn())
        {
            return;
            //다시 시도
        }
        
        
        if (PlayerBackendData.Instance.CheckItemAndRemove(needitemid, SotangCount.nowcount))
        {
            // achievemanager.Instance.AddCount(Acheves.레이드격파, sotangcount);
            QuestManager.Instance.AddCount(SotangCount.nowcount, "singleraid");

            mondropmanager.Instance.GiveDropToInvenToryBossPercentUp(null,
                "5031",EliteRaid.Instance.GetPercent(),SotangCount.nowcount,true);
            
            LogManager.Stang_Raid(SotangCount.nowcount);
           
            StartCoroutine(RaidManager.Instance.FinishRaidReward2());
        }
        else
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/아이템부족"), alertmanager.alertenum.주의);
        }
    }
    
    public void Bt_ShowSotang()
    {
        SotangItem.id = needitemid;
        SotangItem.InitData();
        SotangItem.SetData();
        SotangView.Show(false);
        SotangCount.Maxcount = PlayerBackendData.Instance.CheckItemCount(needitemid);
        SotangCount.SetCount(1);
    }
}
