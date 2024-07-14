using System;
using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DungeonManager : MonoBehaviour
{
    private static DungeonManager _instance = null;
    public static DungeonManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(DungeonManager)) as DungeonManager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    public UIView DungeonPanel;
    public GameObject DungeonAgain;
    public bool isstang;
public GameObject DungeonRewardPanel;
    public List<string> dungeondropsid = new List<string>();
    public List<decimal> dungeondropshowmany = new List<decimal>();
    public itemiconslot[] DungeonItemSlots;

    
    //�ڵ�����
    public Text DungeonAutoCountShow;
    public Text DungeonFinishCountText;


    public bool isautodungeon;
    public int FinishAutoDungeonCount; //�ڵ� ��� �� ���� Ƚ��
    public int autocount =1;
    public int autocount_Doing =1;
    public Text AutoCountText;
    public Text AutoItemNeedCount;
    public Text AutoItemNeedCount_Sotang;
    private DungeonDB.Row data;
    public void NextAutoNum(int count)
    {
        autocount+=count;
        
        if ((PlayerBackendData.Instance.CheckItemCount(data.needitem)) <= getneedautocount())
        {
            autocount -= count;
        }

        if (autocount > 200)
        {
            autocount = 200;
        }
        
        AutoItemNeedCount.text = $"X {getneedautocount()}";
        AutoItemNeedCount_Sotang.text = $"X {getneedautocount() * 1 }";
        AutoCountText.text = autocount.ToString();
    }
    
    public void PrevAutoNum(int count)
    {
        autocount-= count;
        if (autocount <= 0)
        {
            autocount = 1;
        }
        AutoItemNeedCount.text = $"X {getneedautocount()}";
        AutoItemNeedCount_Sotang.text = $"X {getneedautocount() * 1 }";
        AutoCountText.text = autocount.ToString();
    }
    
    
    //������ 1ȸ�������� 1�� ������
    public void DownAutoCount()
    {
        autocount_Doing--;
        DungeonManager.Instance.DungeonAutoCountShow.text =
            string.Format(Inventory.GetTranslate("UI2/��������Ƚ��"),
                DungeonManager.Instance.autocount_Doing.ToString());
    }
    
    //�ʿ��� Ƚ�������� ������ ������
    int getneedautocount()
    {
        int a = autocount * int.Parse(data.needhowmany);

        return a;
    }

    public void BtEnDAuto()
    {
        autocount_Doing = 1;
        if (isautodungeon)
        {
                DungeonAutoCountShow.text = "";
            mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
            DungeonManager.Instance.FinishDungeonLoot();
        }

    }

    public void BtEnDAuto_Reserve()
   {
       autocount_Doing = 1;
       DungeonAutoCountShow.text = Inventory.GetTranslate("UI2/���������⿹����");
   }

    private List<string> sotangdataid = new List<string>();
    private List<decimal> sotangdatahw = new List<decimal>();
    public void AddLoot(string id, int count)
    {
        var i = sotangdataid.IndexOf(id);
//        Debug.Log("�������� �ִ´�"+id);
        if (i != -1)
        {
            sotangdatahw[i] += count;
        }
        else
        {
            sotangdataid.Add(id);
            sotangdatahw.Add(count);
        }
    }

    public void FinishDungeonLoot()
    {
        DungeonAgain.SetActive(false);
        StartCoroutine(showitemicon());
    }

    readonly WaitForSeconds waits = new WaitForSeconds(0.05f);
    readonly WaitForSeconds waits2 = new WaitForSeconds(0.7f);
    readonly WaitForSeconds waits3 = new WaitForSeconds(1f);
    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator showitemicon()
    {
        RefreshCount();
        foreach (var t in DungeonItemSlots)
        {
            t.gameObject.SetActive(false);
        }
        yield return waits3;

        DungeonRewardPanel.SetActive(true);
            yield return waits2;
        for (int i = 0; i < sotangdataid.Count;i++)
        {
            yield return waits;
            DungeonItemSlots[i].gameObject.SetActive(true);
            DungeonItemSlots[i].Refresh(sotangdataid[i],sotangdatahw[i],false,true);
//            Debug.Log(dungeondropshowmany[i]);
        }
        yield return waits;
        DungeonAgain.SetActive(true);
        DungeonFinishCountText.text = FinishAutoDungeonCount.ToString();
       /* 
        if (isstang || !isautodungeon)
        {
            SotangAgain.SetActive(true);
        }
        else
        {  
            DungeonAgain.SetActive(true);
            DungeonFinishCountText.text = FinishAutoDungeonCount.ToString();
        }
        */
        isautodungeon = false;
    }


    public string nowselectmapid;


    public Image Panel_NowBossImage;
    public Text Panel_NowDungeonName;
    public Text Panel_NowDungeonRank;
    public Image Panel_NowBackground;

    public itemslot[] BasicDungeonItemDrop;
    public itemslot[] BossDungeonItemDrop;

    public Toggle[] LevelToggle; //���̵� ���
    int LvIndex; //���̵� 0 �Ϲ� 1 ����� 2�Ǹ� 3��
    public Text[] LevelTiers; //���̵��� ���� ��ũ
    public GameObject[] ToggleLocks; //���̵��� ���
    public Text[] LevelLockTiers; //���̵��� ��� ����


    bool isshowingpanel;
    public GameObject DungeonShowPanel;
    public DungeonSlot[] dungeonslots;
    private void Start()
    {
        string dungeonid = "3000"; ;
        try
        {
            dungeonid = PlayerBackendData.Instance.sotang_dungeon[^1];
        }
        catch (Exception e)
        {
            dungeonid = "3000";
        }
        Bt_SelectDungeon(dungeonid);

    }

    //���� ��� ������
    public void Bt_ShowDungeonPanel()
    {
     
        
        RefreshCount();
        if (isshowingpanel)
        {
            DungeonShowPanel.SetActive(false);
            isshowingpanel = false;
        }
        else
        {
            DungeonShowPanel.SetActive(true);
            isshowingpanel = true;
        }
    }

    //���� �޴����� ��������
    public void Bt_SelectDungeon(string dungeonid)
    {
//        Debug.Log(dungeonid);
        nowselectmapid = dungeonid;

        Panel_NowDungeonRank.text = string.Format(Inventory.GetTranslate("UI2/���跩ũ"),PlayerData.Instance.gettierstar(MapDB.Instance.Find_id(nowselectmapid).maprank));

        Panel_NowDungeonName.text = Inventory.GetTranslate(MapDB.Instance.Find_id(nowselectmapid).name);
        Panel_NowBossImage.sprite = SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id(MapDB.Instance.Find_id(nowselectmapid).monsterid.Split(';')[1]).sprite);
        Panel_NowBackground.sprite = SpriteManager.Instance.GetSprite(MapDB.Instance.Find_id(nowselectmapid).maplayer0);

        for (int i = 0; i < BasicDungeonItemDrop.Length; i++)
        {
            BasicDungeonItemDrop[i].gameObject.SetActive(false);
            BossDungeonItemDrop[i].gameObject.SetActive(false);
        }

        data = DungeonDB.Instance.Find_id(dungeonid);
        MapDB.Row mapdata = MapDB.Instance.Find_id(nowselectmapid);
        monsterDB.Row mondata = monsterDB.Instance.Find_id(mapdata.monsterid);

        string dropid = monsterDB.Instance.Find_id(mapdata.monsterid.Split(';')[0]).dropid;
        string dropid_boss = monsterDB.Instance.Find_id(mapdata.monsterid.Split(';')[1]).bossdrop;
        //�Ϲݸ� ������̺�
        List<MonDropDB.Row> dropdatas_basic = MonDropDB.Instance.FindAll_id(dropid);
//          Debug.Log(dropdatas_basic.Count);
        for (int i = 0; i < dropdatas_basic.Count; i++)
        {
            BasicDungeonItemDrop[i].SetItem(dropdatas_basic[i].itemid, 0);
            BasicDungeonItemDrop[i].gameObject.SetActive(true);
        }
        //���� ������̺�
        List<MonDropDB.Row> dropdatas_boss = MonDropDB.Instance.FindAll_id(dropid_boss);
        for (int i = 0; i < dropdatas_boss.Count; i++)
        {
            BossDungeonItemDrop[i].SetItem(dropdatas_boss[i].itemid, 0);
            BossDungeonItemDrop[i].gameObject.SetActive(true);
        }

        //���̵� üũ
        string[] lvs = DungeonDB.Instance.Find_id(nowselectmapid).levelid.Split(';');
        
        //�ڵ� 
            autocount = 1;
            AutoItemNeedCount.text = $"X {getneedautocount()}";
            AutoItemNeedCount_Sotang.text = $"X {getneedautocount() * 1 }";
            AutoCountText.text = autocount.ToString();
            
        
        //�Ϲ� Ȯ��
        LevelToggle[0].isOn = true;

        LevelToggle[0].gameObject.SetActive(false);
        LevelToggle[1].gameObject.SetActive(false);
        LevelToggle[2].gameObject.SetActive(false);
        LevelToggle[3].gameObject.SetActive(false);
        ToggleLocks[0].gameObject.SetActive(false);
        ToggleLocks[1].gameObject.SetActive(false);
        ToggleLocks[2].gameObject.SetActive(false);
        ToggleLocks[3].gameObject.SetActive(false);
        for (int i = 0; i < lvs.Length; i++)
        {
            LevelToggle[i].gameObject.SetActive(true);

            LevelTiers[i].text = PlayerData.Instance.gettierstar(DungeonDB.Instance.Find_id(lvs[i]).maprank);
            LevelLockTiers[i].text = PlayerData.Instance.gettierstar(DungeonDB.Instance.Find_id(lvs[i]).maprank);
            if (PlayerBackendData.Instance.GetAdLv() < int.Parse(DungeonDB.Instance.Find_id(lvs[i]).maprank))
            {
                //���ǰ
                ToggleLocks[i].gameObject.SetActive(true);
                LevelToggle[i].interactable = false;
            }
            else
            {
                //���
                ToggleLocks[i].gameObject.SetActive(false);
                LevelToggle[i].interactable = true;
            }
        }
    }

    public Text EnterItemCount;
    public Text[] StageCount;


    public void RefreshCount()
    {
        EnterItemCount.text = $"{PlayerBackendData.Instance.CheckItemCount("1711")}/{1}";
        StageCount[0].text = "";
/*
        for (int i = 0; i < StageCount.Length; i++)
        {
            StageCount[i].text =
                string.Format(Inventory.GetTranslate("UI2/��������Ƚ��"),
                    Timemanager.Instance.DailyContentCount[7]
                    , Timemanager.Instance.DailyContentCount_Standard[7]);

            if (Timemanager.Instance.DailyContentCount[7] != 0)
                StageCount[i].color = Color.cyan;
            else if (Timemanager.Instance.DailyContentCount[7] == 0)
            {
                StageCount[i].color = Color.red;
            }
        }
        */
    }

    public void Bt_StartDungeon()
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/����͸�����"),alertmanager.alertenum.����);
            return;
        }
        
        if (mapmanager.Instance.islocating)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/���̵��ߺҰ�"),alertmanager.alertenum.����);
            return;
        }

        isautodungeon = false;
        if (autocount >= 2)
        {
            //�ڵ� ������
            isautodungeon = true;
        }
        autocount_Doing = autocount;
        FinishAutoDungeonCount = 0;
        
        DungeonDB.Row data = DungeonDB.Instance.Find_id(nowselectmapid);
       if(PlayerBackendData.Instance.CheckItemAndRemove(data.needitem,int.Parse(data.needhowmany)))
       {
           dungeondropsid.Clear();
           dungeondropshowmany.Clear();
           mapmanager.Instance.LocateMap(nowselectmapid);
           PlayerBackendData.Instance.spawncount = 3;
           //���̵� ����Ʈ
           Tutorialmanager.Instance.CheckTutorial("finishdungeon");
       }
       else
       {
           alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/����Ǻ���"),alertmanager.alertenum.����);
       }
    }

    public void Bt_SotangDungeon()
    {
        if (!Settingmanager.Instance.CheckServerOn())
        {
            return;
            //�ٽ� �õ�
        }
        
        data = DungeonDB.Instance.Find_id(nowselectmapid);
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        MapDB.Row mapdata_Dun = MapDB.Instance.Find_id(nowselectmapid);

        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/����͸���������"),alertmanager.alertenum.����);
            return;
        }
        
        if (int.Parse(mapdata_Dun.maprank) > PlayerBackendData.Instance.GetAdLv())
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/��ũ������"),alertmanager.alertenum.����);
            return;
        }

        Debug.Log("�� " + nowselectmapid);
        if (!PlayerBackendData.Instance.sotang_dungeon.Contains(nowselectmapid))
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/1ȸŬ�����������"), alertmanager.alertenum.�Ϲ�);
            return;
        }
        
        if (PlayerBackendData.Instance.CheckItemCount(data.needitem) < getneedautocount() * 1)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/����Ǻ���"), alertmanager.alertenum.����);
            return;
        }

        autocount_Doing = autocount;
        FinishAutoDungeonCount = autocount_Doing;
        
        if(PlayerBackendData.Instance.CheckItemAndRemove(data.needitem,getneedautocount() * 1  ))
        {
            sotangdataid.Clear();
            sotangdatahw.Clear();
            dungeondropsid.Clear();
            dungeondropshowmany.Clear();
            EarnDungeonAll(nowselectmapid);
        }
        else
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/����Ǻ���"),alertmanager.alertenum.����);
        }
    }

    int Ran_rate;

    public void GiveDropToInvenTory(Transform monpos, List<string> dropid, List<int> minhowmany, List<int> maxhowmany,
        List<int> percent, bool issotang = false)
    {
        for (int i = 0; i < dropid.Count; i++)
        {
            Random.InitState((int)Time.deltaTime + PlayerBackendData.Instance.GetRandomSeed());
            Ran_rate = Random.Range(0, 1000000); // 1,000,000�� 100%�̴�.
//            Debug.Log(dropid[i] + "�̱�õ�" + Ran_rate) ;
            if (Ran_rate <= mondropmanager.Instance.getpercent(percent[i]))
            {
                int Howmany = Random.Range(minhowmany[i], maxhowmany[i]);
                AddLoot(dropid[i], Howmany);
            }
        }
    }


    void EarnDungeonAll(string mapid)
    {
        data = DungeonDB.Instance.Find_id(nowselectmapid);

        SetDropData(nowselectmapid);
      
        for (int j = 0; j < autocount_Doing; j++)
        {
            for (int i = 0; i < 27; i++)
            {
                GiveDropToInvenTory(null, Mon_DropItemID
                    , Mon_DropItemMinHowmany,
                    Mon_DropItemMaxHowmany,
                    Mon_DropItemPercent, true);
            }
        }
        for (int i = 0; i < autocount_Doing; i++)
        {
            GiveDropToInvenTory(null,Mon_DropItemIDboss
                , Mon_DropItemMinHowmanyboss,
                Mon_DropItemMaxHowmanyboss,
                Mon_DropItemPercentboss, true);
        }


        
        for (int i = 0; i < sotangdataid.Count; i++)
        {
            Inventory.Instance.AddItemDungeon(sotangdataid[i],sotangdatahw[i],false,false);
        }
            
        LogManager.Stang_Dungeon(autocount_Doing);
        isstang = true;
        
   //     achievemanager.Instance.AddCount(Acheves.��������,autocount_Doing);
        QuestManager.Instance.AddCount(autocount_Doing, "dungeon");

        FinishDungeonLoot();
        /*
        if (Inventory.Instance.istrue)
        {
            Where where = new Where();
            where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
            Param param = new Param { { "PlayerCanLoadBool", "false" } };
            Inventory.Instance.istrue = false;
            SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Settingmanager.Instance.RecentServerDataCanLoadText.text = Inventory.Instance.istrue
                        ? string.Format(Inventory.GetTranslate("UI/����_����_�ҷ���������"),
                            Inventory.Instance.istrue
                                ? "<Color=lime>On</color>"
                                : "<Color=red>Off</color>")
                        : string.Format(Inventory.GetTranslate("UI/����_����_�ҷ���������"), "<Color=red>Off</color>");
                }
            });
        }*/
        Settingmanager.Instance.SaveAllNoLog();
        Savemanager.Instance.SaveInventory_SaveOn();
    }
    string nowdropid = "";
    string nowdropbossid = "";
     public List<string> Mon_DropItemID = new List<string>();
    public List<int> Mon_DropItemMinHowmany = new List<int>();
    public List<int> Mon_DropItemMaxHowmany = new List<int>();
    public List<int> Mon_DropItemPercent = new List<int>();

    public List<string> Mon_DropItemIDboss = new List<string>();
    public List<int> Mon_DropItemMinHowmanyboss = new List<int>();
    public List<int> Mon_DropItemMaxHowmanyboss = new List<int>();
    public List<int> Mon_DropItemPercentboss = new List<int>();

    void SetDropData(string DungeonID)
    {
        nowdropid = DungeonDB.Instance.Find_id(DungeonID).mondrop;
        nowdropbossid = DungeonDB.Instance.Find_id(DungeonID).monbossdrop;
      
        Mon_DropItemID.Clear();
        Mon_DropItemMinHowmany.Clear();
        Mon_DropItemMaxHowmany.Clear();
        Mon_DropItemPercent.Clear();

        Mon_DropItemIDboss.Clear();
        Mon_DropItemMinHowmanyboss.Clear();
        Mon_DropItemMaxHowmanyboss.Clear();
        Mon_DropItemPercentboss.Clear();


        MonDropDB.Row[] data = MonDropDB.Instance.FindAll_id(nowdropid).ToArray();
        MonDropDB.Row[] data2 = MonDropDB.Instance.FindAll_id(nowdropbossid).ToArray();

        
        foreach (var t in data)
        {
            Mon_DropItemID.Add(t.itemid);
            Mon_DropItemMinHowmany.Add(int.Parse(t.minhowmany));
            Mon_DropItemMaxHowmany.Add(int.Parse(t.maxhowmany));
            Mon_DropItemPercent.Add(int.Parse(t.rate));
        }
        
        foreach (var t in data2)
        {
            Mon_DropItemIDboss.Add(t.itemid);
            Mon_DropItemMinHowmanyboss.Add(int.Parse(t.minhowmany));
            Mon_DropItemMaxHowmanyboss.Add(int.Parse(t.maxhowmany));
            Mon_DropItemPercentboss.Add(int.Parse(t.rate));
        }
    }
}
