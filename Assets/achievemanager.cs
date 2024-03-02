using Doozy.Engine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using UnityEngine;

public class achievemanager : MonoBehaviour
{
    public bool isachievePanelOn;
    private static achievemanager _instance = null;
    public static achievemanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(achievemanager)) as achievemanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }
    public UIView Achievepanel;
    private void Start()
    {
        InvokeRepeating(nameof(save), 10f,60f);
        ResetCheck();
        CheckReSetIfNO();
    }


    public void CheckReSetIfNO()
    {
        bool havenewachieve = false;
        //�ű� ���� Ȯ��
        foreach (var t in achievemanager.Instance.acheveslots.Where(t =>t.data.Curcount >= t.data.Maxcount &&
                                                                         t.data.Isfinish
                                                                         && AchievementDB.Instance.Find_id(t.data.Id)
                                                                             .islast == "0" &&
                                                                         AchievementDB.Instance.Find_id(t.data.Id)
                                                                             .subtype == "forever"))
        {
            havenewachieve = true;
            Debug.Log("�űԾ��� Ȯ��" + t.data.Id);
            t.data.Isfinish = false;
        }
        if (havenewachieve)
        {
            Savemanager.Instance.SaveAchieve();
            Savemanager.Instance.Save();
            Settingmanager.Instance.SaveDataAchieveInven();
        }
        
        
        if (Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.��������] >= 1
            && PlayerBackendData.Instance.PlayerAchieveData["A2001"].Curcount >= 1)
        {
            Debug.Log("���� ����");
            Timemanager.Instance.isachievereset = true;
            ResetCheck();
        }
    }
    private bool ischeckachieve;
    public void ResetCheck()
    {
        if(PlayerBackendData.Instance.PlayerAchieveData.Count ==0)
        {
            //�����̾���
            ischeckachieve = true;
            //PlayerBackendData.Instance.InitAchieveData();
            return;
        }
        if (Timemanager.Instance.isachievereset)
        {
            Timemanager.Instance.isachievereset = false;
            Timemanager.Instance.LoginTimeSecToday = 0;
            Savemanager.Instance.SaveLoginTime();
            Savemanager.Instance.Save();
            
            int achnum = 0;
            //���� �ʱ�ȭ
            foreach (var VARIABLE in PlayerBackendData.Instance.PlayerAchieveData.Values)
            {
                if (VARIABLE.Achievetype == "daily")
                {
                    VARIABLE.ResetDailyAchieve();
                }
            }

            for (var index = 0; index < acheveslots.Count; index++)
            {
                var t = acheveslots[index];
                t.num = index;
                t.Refresh();
            }

            foreach (var t in Seasonacheveslots)
            {
                t.Refresh();
            }
            Savemanager.Instance.SaveAchieve();
            Savemanager.Instance.Save();
        }
        else
        {
            for (var index = 0; index < acheveslots.Count; index++)
            {
              
                var t = acheveslots[index];
                t.num = index;
                t.Refresh();
            }

            foreach (var t in Seasonacheveslots)
            {
                t.Refresh();
            }
        }
    }

    
    public void OpenPanel()
    {
        if (ischeckachieve)
        {
            alertmanager.Instance.ShowAlert("���� ������ �ҷ��� �� �����ϴ�. ��������ּ���.",alertmanager.alertenum.�Ϲ�);
            return;
        }
        Achievepanel.Show(false);
        foreach (var t in acheveslots)
        {
            t.Refresh();
        }
        isachievePanelOn = true;
    }

    public void Closepanel()
    {
        Achievepanel.Hide(false);
        isachievePanelOn = false;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void AddCount(Acheves type,int count =1)
    {
        if (ischeckachieve)
        {
            alertmanager.Instance.ShowAlert("���� ������ �ҷ��� �� �����ϴ�. ��������ּ���.",alertmanager.alertenum.�Ϲ�);
            return;
        }
        switch (type)
        {
            case Acheves.����óġ:
            {
                foreach (var data in PlayerBackendData.Instance.PlayerAchieveData.Where(data => AchievementDB.Instance.Find_id(data.Value.Id).type == "killmonster"))
                {
//                    Debug.Log(data.Value.Id);
                    data.Value.Curcount+= count;
                }
                //save();
                if (!isachievePanelOn) return;
                foreach (var t in acheveslots.Where(t => t.type.Equals("killmonster")))
                {
                    t.Refresh();
                }
                foreach (var t in Seasonacheveslots.Where(t => t.type.Equals("killmonster")))
                {
                    t.Refresh();
                }
                break;
            }
            case Acheves.��������:
            {
                foreach (var data in PlayerBackendData.Instance.PlayerAchieveData.Where(data =>
                             AchievementDB.Instance.Find_id(data.Value.Id).type == "dungeonclear"))
                {
                    data.Value.Curcount+=count;
                }
                save();
                if (!isachievePanelOn) return;
                foreach (var t in acheveslots.Where(t => t.type.Equals("dungeonclear")))
                {
                    t.Refresh();
                }
                foreach (var t in Seasonacheveslots.Where(t => t.type.Equals("dungeonclear")))
                {
                    t.Refresh();
                }
                break;
            }
            case Acheves.����������:
            {
                foreach (var data in PlayerBackendData.Instance.PlayerAchieveData.Where(data =>
                             AchievementDB.Instance.Find_id(data.Value.Id).type == "crafting"))
                {
                    data.Value.Curcount+=count;
                }
                save();
                if (!isachievePanelOn) return;
                foreach (var t in acheveslots.Where(t => t.type.Equals("crafting")))
                {
                    t.Refresh();
                }
                foreach (var t in Seasonacheveslots.Where(t => t.type.Equals("crafting")))
                {
                    t.Refresh();
                }
                break;
            }
            case Acheves.����óġ:
            {
                foreach (var data in PlayerBackendData.Instance.PlayerAchieveData.Where(data =>
                             AchievementDB.Instance.Find_id(data.Value.Id).type == "bosskill"))
                {
                    data.Value.Curcount+=count;
                }
                save();
                if (!isachievePanelOn) return;
                foreach (var t in acheveslots.Where(t => t.type.Equals("bosskill")))
                {
                    t.Refresh();
                }
                foreach (var t in Seasonacheveslots.Where(t => t.type.Equals("bosskill")))
                {
                    t.Refresh();
                }
                break;
            }
            case Acheves.�����Ŭ����:
            {
                foreach (var data in PlayerBackendData.Instance.PlayerAchieveData.Where(data => AchievementDB.Instance.Find_id(data.Value.Id).type == "StageClear"))
                {
                    data.Value.Curcount+=count;
                }
                save();
                if (!isachievePanelOn) return;
                foreach (var t in acheveslots.Where(t => t.type.Equals("StageClear")))
                {
                    t.Refresh();
                }
 foreach (var t in Seasonacheveslots.Where(t => t.type.Equals("StageClear")))
                {
                    t.Refresh();
                }
                break;
            }
            case Acheves.�±�:
            {
                foreach (var data in PlayerBackendData.Instance.PlayerAchieveData.Where(data => AchievementDB.Instance.Find_id(data.Value.Id).type == "adlvup"))
                {
                    data.Value.Curcount+=count;
                }
                save();
                if (!isachievePanelOn) return;
                foreach (var t in acheveslots.Where(t => t.type.Equals("adlvup")))
                {
                    t.Refresh();
                }
                foreach (var t in Seasonacheveslots.Where(t => t.type.Equals("adlvup")))
                {
                    t.Refresh();
                }
                break;
            }
            case Acheves.���̵����:
            {
                foreach (var data in PlayerBackendData.Instance.PlayerAchieveData.Where(data => AchievementDB.Instance.Find_id(data.Value.Id).type == "raidclear"))
                {
                    data.Value.Curcount+=count;
                }
                save();
                if (!isachievePanelOn) return;
                foreach (var t in acheveslots.Where(t => t.type.Equals("raidclear")))
                {
                    t.Refresh();
                }
                foreach (var t in Seasonacheveslots.Where(t => t.type.Equals("raidclear")))
                {
                    t.Refresh();
                }
                break;
            }
            case Acheves.��������Ϸ�:
            {
                foreach (var data in PlayerBackendData.Instance.PlayerAchieveData.Where(data => AchievementDB.Instance.Find_id(data.Value.Id).type == "altarwar"))
                {
                    data.Value.Curcount+=count;
                }
                save();
                if (!isachievePanelOn) return;
                foreach (var t in acheveslots.Where(t => t.type.Equals("altarwar")))
                {
                    t.Refresh();
                }
                foreach (var t in Seasonacheveslots.Where(t => t.type.Equals("altarwar")))
                {
                    t.Refresh();
                }
                break;
            }
            case Acheves.��巹�̵����:
            {
                foreach (var data in PlayerBackendData.Instance.PlayerAchieveData.Where(data => AchievementDB.Instance.Find_id(data.Value.Id).type == "guildraid"))
                {
                    data.Value.Curcount+=count;
                }
                save();
                if (!isachievePanelOn) return;
                foreach (var t in acheveslots.Where(t => t.type.Equals("guildraid")))
                {
                    t.Refresh();
                }
                foreach (var t in Seasonacheveslots.Where(t => t.type.Equals("guildraid")))
                {
                    t.Refresh();
                }
                break;
            }
            case Acheves.�븶�����ǹ�:
            {
                foreach (var data in PlayerBackendData.Instance.PlayerAchieveData.Where(data => AchievementDB.Instance.Find_id(data.Value.Id).type == "contentskill"))
                {
                    data.Value.Curcount+=count;
                }
                save();
                if (!isachievePanelOn) return;
                foreach (var t in acheveslots.Where(t => t.type.Equals("contentskill")))
                {
                    t.Refresh();
                }
                foreach (var t in Seasonacheveslots.Where(t => t.type.Equals("contentskill")))
                {
                    t.Refresh();
                }
                break;
            }
            case Acheves.Length:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public List<achievementslot> acheveslots = new List<achievementslot>();

    public List<achievementslot> Seasonacheveslots = new List<achievementslot>();

    public void RefreshDailyAchieve()
    {

    }

    public List<string> id = new List<string>();
    public List<int> hw = new List<int>();
   public bool ispanel = false;

   public void FinishAll()
   {
       id.Clear();
       hw.Clear();
       bool isclear = true;
       bool ishavefinish= false;
       while (true)
       {
           isclear = false;
           foreach (var t in acheveslots.Where(t => t.data.Curcount >= t.data.Maxcount && !t.data.Isfinish))
           {
//               Debug.Log("�����ִ�");
               ishavefinish = true;
               isclear = true;
               ispanel = true;
               t.Bt_Bt_GetReward2();
           }

           if (!isclear)
               break;
       }

       if (ispanel)
       {
           if (id.Count != 0)
           {
               List<string> gw = new List<string>();

               for (int i = 0; i < hw.Count; i++)
               {
                   gw.Add(hw[i].ToString());
               }

               Inventory.Instance.ShowEarnItem(id.ToArray(), gw.ToArray(), false);
           }

           ispanel = false;
       }

       if (ishavefinish)
       {
           Debug.Log("��������");
           Settingmanager.Instance.SaveDataAchieveInven();
           alertmanager.Instance.NotiCheck_Achieve();
           Savemanager.Instance.SaveAchieveDirect();
           Savemanager.Instance.Save();
       }
   }

   public void CheckFinishSeasonPass()
   {
       if(SeasonPass.Instance.GetLv() >= 50)
           return;


       foreach (var t in Seasonacheveslots.Where(t => t.data.Curcount >= t.data.Maxcount && !t.data.Isfinish))
       {
//               Debug.Log("�����ִ�");
           alertmanager.Instance.Alert_SeasonPass[0].SetActive(true);
           alertmanager.Instance.Alert_SeasonPass[1].SetActive(true);
           return;
       }
   }
   public void FinishAllSeason()
   {
       if(SeasonPass.Instance.GetLv() >= 50)
           return;
       
       id.Clear();
       hw.Clear();
       bool isclear = true;
       bool ishavefinish= false;
       while (true)
       {
           isclear = false;
           foreach (var t in Seasonacheveslots.Where(t => t.data.Curcount >= t.data.Maxcount && !t.data.Isfinish))
           {
//               Debug.Log("�����ִ�");
               ishavefinish = true;
               isclear = true;
               ispanel = true;
               t.Bt_Bt_GetReward2();
           }

           if (!isclear)
               break;
       }

       if (ispanel)
       {
           if (id.Count != 0)
           {
               List<string> gw = new List<string>();

               for (int i = 0; i < hw.Count; i++)
               {
                   gw.Add(hw[i].ToString());
               }

               Inventory.Instance.ShowEarnItem(id.ToArray(), gw.ToArray(), false);
           }

           ispanel = false;
       }

       if (ishavefinish)
       {
           Debug.Log("��������");
           SeasonPass.Instance.SaveSeasonReward();
           alertmanager.Instance.NotiCheck_Pass();
           Savemanager.Instance.SaveAchieveDirect();
           Savemanager.Instance.Save();
       }
   }
   
   public GameObject allfinishnoti;
    public void Additem(string itemid, int counts)
    {
        int index = id.IndexOf(itemid);
        if (index == -1)
        {
            id.Add(itemid);
            hw.Add(counts);
        }
        else
        {
            hw[index] += counts;
        }
    }
    void save()
    {
        Savemanager.Instance.SaveAchieve();
    }
}

public enum Acheves
{
    ����óġ,
    ��������,
    ����������,
    ����óġ,
    �����Ŭ����,
    �±�,
    ���̵����,
    ��������Ϸ�,
    ��巹�̵����,
    �븶�����ǹ�, 
    Length
}


public class Achievedata
{
    private string id;
    private string coreid;
    private int curcount;
    private int maxcount;
    private string achievetype;
    private bool isfinish;
    public Achievedata()
    {

    }
    public Achievedata(JsonData data)
    {
        Id = data["Id"].ToString();
        Coreid = data["Coreid"].ToString();
        Curcount = int.Parse(data["Curcount"].ToString());
        Maxcount = int.Parse(data["Maxcount"].ToString());
        Isfinish =  bool.Parse(data["Isfinish"].ToString());
        Achievetype = data["Achievetype"].ToString();
    }

    public Achievedata(string id, string coreid, int curcount, int maxcount,string achevetype,bool isfinish)
    {
        this.id = id;
        this.coreid = coreid;
        this.curcount = curcount;
        this.maxcount = maxcount;
        this.achievetype = achevetype;
        this.isfinish = isfinish;
    }

    public void LevelUp(bool issave)
    {
        curcount -= maxcount;
        this.id = AchievementDB.Instance.Find_id(Id).nextlevel;
//        Debug.Log(Id);
     //   Debug.Log(AchievementDB.Instance.Find_id(Id).count);
     isfinish = false;
        this.maxcount = int.Parse(AchievementDB.Instance.Find_id(Id).count);
        if (!issave)
        {
            Savemanager.Instance.SaveAchieve();
            Savemanager.Instance.Save();
        }
    }

    public void ResetDailyAchieve()
    {
        //Debug.Log("���µȰ��� " + id);
        curcount = 0;
        isfinish = false;
    }

    public string Id { get => id; set => id = value; }
    public string Coreid { get => coreid; set => coreid = value; }
    public int Curcount { get => curcount; set => curcount = value; }
    public int Maxcount { get => maxcount; set => maxcount = value; }
    public string Achievetype { get => achievetype; set => achievetype = value; }
    public bool Isfinish { get => isfinish; set => isfinish = value; }
}


enum Achievetype
{
   daily,
   forever
}