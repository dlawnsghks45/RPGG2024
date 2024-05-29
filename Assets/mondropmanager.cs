using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Doozy;
using UnityEngine;
using UnityEngine.UI;

public class mondropmanager : MonoBehaviour
{
    //싱글톤만들기.
    private static mondropmanager _instance = null;
    public static mondropmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(mondropmanager)) as mondropmanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }


    public itemdropslot[] dropslots;
    int dropnum;

    private void Dropitem(string Id,int count)
    {
        if(!SettingReNewal.Instance.ItemPanel[0].IsOn)
            return;
        
        foreach (var t in dropslots)
        {
      
            dropslots[dropnum].SetDrop(Id,count);
            dropslots[dropnum].transform.SetAsLastSibling();
            dropnum++;
            if (dropnum.Equals(dropslots.Length))
                dropnum = 0;
            break;
        }
    }
    [Button (Name = "Test")]
    public void Test()
    {
        Dropitem("1001", 100);
    }

    int Ran_rate;
    //인벤에 아이템들을 전송함.
    public void GiveDropToInvenTory(Transform monpos,List<string> dropid , List<int>minhowmany,List<int> maxhowmany,List<float> percent,bool issotang =false)
    {
        if (nowdropid == "")
            return;
        for (int i = 0; i < dropid.Count; i++)
        {
            Ran_rate = Random.Range(0, 1000000);// 1,000,000이 100%이다.
            if (Ran_rate <= getpercent(percent[i]))
            {
                int Howmany = Random.Range(minhowmany[i], maxhowmany[i]);
                Inventory.Instance.AddItem(dropid[i], Howmany,false,true);
                if(monpos != null)
                    ItemDropManager.Instance.SpawnItem(ItemdatabasecsvDB.Instance.Find_id(dropid[i]).sprite,bool.Parse(ItemdatabasecsvDB.Instance.Find_id(dropid[i]).droprare),monpos.position);

                switch (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype)
                {
                    case "0":
                        if (issotang)
                        {
                           // DungeonManager.Instance. AddLoot(dropid[i], Howmany);
                        }
                        else
                        {
                            mondropmanager.Instance.Dropitem(dropid[i], Howmany);
                        }
                        break;
                        
                    //던전FAc
                    case "1":
                        mondropmanager.Instance.Dropitem(dropid[i], Howmany);
                      //  DungeonManager.Instance.AddLoot(dropid[i], Howmany);
                        break;
                  
                }
            }
        }
    }

    private int Ran_rate_event;
    public void GiveEventDropToInvenTory(Transform monpos,List<string> dropid , List<int>minhowmany,List<int> maxhowmany,List<float> percent,bool issotang =false)
    {
        if (nowmaptype == "")
            return;

        for (int i = 0; i < dropid.Count; i++)
        {
            Random.InitState((int)Time.deltaTime + PlayerBackendData.Instance.GetRandomSeed());
            Ran_rate_event = Random.Range(0, 1000000);// 1,000,000이 100%이다.
            if (Ran_rate_event <= percent[i])
            {
                int Howmany = Random.Range(minhowmany[i], maxhowmany[i]);
                Inventory.Instance.AddItem(dropid[i], Howmany,false,true);
                //아이템 드롭
                if(monpos != null)
                    ItemDropManager.Instance.SpawnItem(ItemdatabasecsvDB.Instance.Find_id(dropid[i]).sprite,bool.Parse(ItemdatabasecsvDB.Instance.Find_id(dropid[i]).droprare),monpos.position);

                
                switch (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype)
                {
                    case "0":
                        if (issotang)
                        {
                          //  DungeonManager.Instance.AddLoot(dropid[i], Howmany);
                        }
                        else
                        {
                            mondropmanager.Instance.Dropitem(dropid[i], Howmany);
                        }
                        break;
                        
                    //던전FAc
                    case "1":
                        mondropmanager.Instance.Dropitem(dropid[i], Howmany);
                       // DungeonManager.Instance.AddLoot(dropid[i], Howmany);
                        break;
                }
            }
        }
    }

    public float getpercent(float basepercent)
    {
        float dropper = basepercent + (basepercent * Battlemanager.Instance.mainplayer.Stat_ExtraDrop);
        return dropper;
    }
    
    
    public void GiveDropToInvenToryBoss(Transform monpos,List<string> dropid , List<int>minhowmany,List<int> maxhowmany,List<float> percent,bool issotang =false)
    {
   // Debug.Log("보승 아이템 시작");
        if (nowdropbossid is "" or "0")
            return;
      //  Debug.Log("보승 아이템 시작줘라");

        for (int i = 0; i < dropid.Count; i++)
        {
            Random.InitState((int)Time.deltaTime + PlayerBackendData.Instance.GetRandomSeed());
            Ran_rate = Random.Range(0, 1000000);// 1,000,000이 100%이다.
            if (Ran_rate <= getpercent(percent[i]))
            { 
//                Debug.Log("줬다!");

                int Howmany = Random.Range(minhowmany[i], maxhowmany[i]);
                Inventory.Instance.AddItem(dropid[i], Howmany,false,true);
                if(monpos != null)
                    ItemDropManager.Instance.SpawnItem(ItemdatabasecsvDB.Instance.Find_id(dropid[i]).sprite,bool.Parse(ItemdatabasecsvDB.Instance.Find_id(dropid[i]).droprare),monpos.position);

                switch (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype)
                {
                    case "0":
                        if (issotang)
                        {
                           // DungeonManager.Instance.AddLoot(dropid[i], Howmany);
                        }
                        else
                        {
                            mondropmanager.Instance.Dropitem(dropid[i], Howmany);
                        }
                        break;
                    //던전
                    case "1":
                       // DungeonManager.Instance.AddLoot(dropid[i], Howmany);
                        mondropmanager.Instance.Dropitem(dropid[i], Howmany);
                        break;
                    case "2"://승
                        uimanager.Instance.AddLoot(dropid[i], Howmany);
                        break;
                    //레이드
                    case "3":
                        RaidManager.Instance.AddLoot(dropid[i], Howmany);
                        break;
                    case "7":
                        GuildRaidManager.Instance.AddLoot(dropid[i], Howmany);
                        break;
                    case "10":
                        mondropmanager.Instance.Dropitem(dropid[i], Howmany);
                        break;
                }
            }

          
        }
    }

    
      public void GiveDropToInvenToryBossPercentUp(Transform monpos,string monid,decimal addper)
    {
        if (nowdropbossid is "" or "0")
            return;

        List<MonDropDB.Row> data = MonDropDB.Instance.FindAll_id(monsterDB.Instance.Find_id(monid).dropid);

        for (int i = 0; i < data.Count; i++)
        {

            decimal howmanycount = bool.Parse(data[i].Ispercent)
                ? decimal.Parse(data[i].maxhowmany) * addper
                : decimal.Parse(data[i].maxhowmany);

            float percent = float.Parse(data[i].rate);

            decimal minh = decimal.Parse(data[i].minhowmany) * addper;
            decimal maxh = decimal.Parse(data[i].maxhowmany) * addper;
            
            Random.InitState((int)Time.deltaTime + PlayerBackendData.Instance.GetRandomSeed());
            Ran_rate = Random.Range(0, 1000000);// 1,000,000이 100%이다.
            if (Ran_rate <= getpercent(percent))
            { 
                int Howmany = Random.Range((int)minh,(int)maxh);
                Inventory.Instance.AddItem(data[i].itemid, Howmany,false,true);
                if(monpos != null)
                    ItemDropManager.Instance.SpawnItem(ItemdatabasecsvDB.Instance.Find_id(data[i].itemid).sprite,bool.Parse(ItemdatabasecsvDB.Instance.Find_id(data[i].itemid).droprare),monpos.position);

                switch (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype)
                {
                    //던전
                    case "1":
                        break;
                    //레이드
                    case "3":
                        RaidManager.Instance.AddLoot(data[i].itemid, Howmany);
                        break;
                   
                }
            }
            
        }
    }

      
    public void GiveDropToInvenToryBoss(Transform monpos,string maptype,List<string> dropid , List<int>minhowmany,List<int> maxhowmany,List<float> percent,bool issotang =false)
    {
        if (nowdropbossid == "")
            return;
//        Debug.Log("드랍했다");
        for (int i = 0; i < dropid.Count; i++)
        {
            Random.InitState((int)Time.deltaTime + PlayerBackendData.Instance.GetRandomSeed());
            Ran_rate = Random.Range(0, 1000000);// 1,000,000이 100%이다.
            if (Ran_rate <= getpercent(percent[i]))
            {
                int Howmany = Random.Range(minhowmany[i], maxhowmany[i]);

                Inventory.Instance.AddItem(dropid[i], Howmany,false,true);
                if(monpos != null)
                    ItemDropManager.Instance.SpawnItem(ItemdatabasecsvDB.Instance.Find_id(dropid[i]).sprite,bool.Parse(ItemdatabasecsvDB.Instance.Find_id(dropid[i]).droprare),monpos.position);

                switch (maptype)
                {
                    case "0":
                        if (issotang)
                        {
                           // DungeonManager.Instance.AddLoot(dropid[i], Howmany);
                        }
                        else
                        {
                            mondropmanager.Instance.Dropitem(dropid[i], Howmany);
                        }
                        break;
                    //던전
                    case "1":
                      //  DungeonManager.Instance.AddLoot(dropid[i], Howmany);
                        mondropmanager.Instance.Dropitem(dropid[i], Howmany);
                        break;
                    case "2"://승
                        uimanager.Instance.AddLoot(dropid[i], Howmany);
                        break;
                    //레이드
                    case "3":
                        RaidManager.Instance.AddLoot(dropid[i], Howmany);
                        break;
                    case "7":
                        GuildRaidManager.Instance.AddLoot(dropid[i], Howmany);
                        break;
                }
            }

          
        }
        Savemanager.Instance.SaveFieldEnd();
    }

    public void GiveRewardExcepDropPanel()
    {
        Debug.Log("현재 맵타입"  + MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype);
        switch (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype)
        {
            //성전
            case "4":
                int Howmany = 25 * altarwarmanager.Instance.GetDmgReward();
                Inventory.Instance.AddItem("1712", Howmany,false);
                altarwarmanager.Instance.AddLoot("1712",Howmany );
                LogManager.LogAltarWar(Howmany,altarwarmanager.Instance.totaldmg);

                break;
        }
        Savemanager.Instance.SaveInventory_SaveOn();

    }
    [SerializeField]
    string nowdropid = "";
    string nowdropbossid = "";
    public List<string> Mon_DropItemID = new List<string>();
    public List<int> Mon_DropItemMinHowmany = new List<int>();
    public List<int> Mon_DropItemMaxHowmany = new List<int>();
    public List<float> Mon_DropItemPercent = new List<float>();

    public List<string> Mon_DropItemIDboss = new List<string>();
    public List<int> Mon_DropItemMinHowmanyboss = new List<int>();
    public List<int> Mon_DropItemMaxHowmanyboss = new List<int>();
    public List<float> Mon_DropItemPercentboss = new List<float>();

    
    public List<string> Event_DropItemID = new List<string>();
    public List<int> Event_DropItemMinHowmany = new List<int>();
    public List<int> Event_DropItemMaxHowmany = new List<int>();
    public List<float> Event_DropItemPercent = new List<float>();

    private string nowmaptype = "";

    public void SetEventDrop(string maptype)
    {
        if (nowmaptype.Equals(maptype)) return;
        nowmaptype = maptype;
        Event_DropItemID.Clear();
        Event_DropItemMinHowmany.Clear();
        Event_DropItemMaxHowmany.Clear();
        Event_DropItemPercent.Clear();

        bool isfind = false;
        for (int i = 0; i < EventDropDB.Instance.NumRows(); i++)
        {
            if (nowmaptype != EventDropDB.Instance.GetAt(i).maptype) continue;
            Event_DropItemID.Add(EventDropDB.Instance.GetAt(i).itemid);
            Event_DropItemMinHowmany.Add(int.Parse(EventDropDB.Instance.GetAt(i).mincount));
            Event_DropItemMaxHowmany.Add(int.Parse(EventDropDB.Instance.GetAt(i).maxcount));
            Event_DropItemPercent.Add(float.Parse(EventDropDB.Instance.GetAt(i).droprate));
        }
    }
    
    public void SetDropDataDropId(string Dropid)
    {
       if (checkDrop)
       {
           return;
       }
       checkDrop = true;

        nowdropbossid = Dropid;
        
        Mon_DropItemID.Clear();
        Mon_DropItemMinHowmany.Clear();
        Mon_DropItemMaxHowmany.Clear();
        Mon_DropItemPercent.Clear();

        Mon_DropItemIDboss.Clear();
        Mon_DropItemMinHowmanyboss.Clear();
        Mon_DropItemMaxHowmanyboss.Clear();
        Mon_DropItemPercentboss.Clear();

        if (nowdropbossid != "0")
        {
            MonDropDB.Row[] data2 = MonDropDB.Instance.FindAll_id(nowdropbossid).ToArray();

        
            foreach (var t in data2)
            {
                Mon_DropItemIDboss.Add(t.itemid);
                Mon_DropItemMinHowmanyboss.Add(int.Parse(t.minhowmany));
                Mon_DropItemMaxHowmanyboss.Add(int.Parse(t.maxhowmany));
                Mon_DropItemPercentboss.Add(int.Parse(t.rate));
            }
        }
    }




       public bool checkDrop =false;
    public void SetDropData(string MonID)
    {
        if (checkDrop)
        {
            return;
        }
        checkDrop = true;
        
        nowdropid = monsterDB.Instance.Find_id(MonID.ToString()).dropid;
        nowdropbossid = monsterDB.Instance.Find_id(MonID.ToString()).bossdrop;
        
      
        Mon_DropItemID.Clear();
        Mon_DropItemMinHowmany.Clear();
        Mon_DropItemMaxHowmany.Clear();
        Mon_DropItemPercent.Clear();

        Mon_DropItemIDboss.Clear();
        Mon_DropItemMinHowmanyboss.Clear();
        Mon_DropItemMaxHowmanyboss.Clear();
        Mon_DropItemPercentboss.Clear();

            //Debug.Log("체크");
        if (nowdropid != "0")
        {
            MonDropDB.Row[] data = MonDropDB.Instance.FindAll_id(nowdropid).ToArray();
            foreach (var t in data)
            {
                Mon_DropItemID.Add(t.itemid);
                Mon_DropItemMinHowmany.Add(int.Parse(t.minhowmany));
                Mon_DropItemMaxHowmany.Add(int.Parse(t.maxhowmany));
                Mon_DropItemPercent.Add(int.Parse(t.rate));
            }
        }

        if (nowdropbossid != "0")
        {
          //  Debug.Log("체3크");
          MonDropDB.Row[] data2 = MonDropDB.Instance.FindAll_id(nowdropbossid).ToArray();
          
          foreach (var t in data2)
          {
              Mon_DropItemIDboss.Add(t.itemid);
              Mon_DropItemMinHowmanyboss.Add(int.Parse(t.minhowmany));
              Mon_DropItemMaxHowmanyboss.Add(int.Parse(t.maxhowmany));
              Mon_DropItemPercentboss.Add(int.Parse(t.rate));
          }
        }
    }
}

