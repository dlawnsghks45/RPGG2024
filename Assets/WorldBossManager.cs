using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldBossManager : MonoBehaviour
{
    public WorldBossSlot[] bossslots;

    public void RefreshAllSlots()
    {
        for (int i = 0; i < bossslots.Length; i++)
        {
            bossslots[i].InitData();
        }
    }

    public void RefreshAllRank()
    {
        for (int i = 0; i < bossslots.Length; i++)
        {
            bossslots[i].GetRankData(false);
        }
    }
    public void CloseAllWorldboss()
    {
        WorldbossPanel.Hide(false);
        WorldbossInfoPanel.Hide(false);
        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/월드보스끝"), alertmanager.alertenum.주의);
    }

    public GameObject WorldbossLockPanel;

    public void CheckWorldBossLock()
    {
        DateTime date = Timemanager.Instance.GetServerTime();

        if (date.Hour is >= 23 or < 12)
        {
            WorldbossLockPanel.SetActive(true);
            return;
        }
        else
        {
            WorldbossLockPanel.SetActive(false);
        }
    }
    
    public void BtShowBossPanel()
    {
        DateTime date = Timemanager.Instance.GetServerTime();

        if (date.Hour is >= 23 or < 12)
        {
            Debug.Log("현재 입장이 불가능하다.");
            WorldbossLockPanel.SetActive(true);
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/월드보스불가"), alertmanager.alertenum.주의);
            return;
        }
        
        if (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/월드보스중 불가"), alertmanager.alertenum.주의);
            return;
        }
        WorldbossLockPanel.SetActive(false);
        WorldbossPanel.Show(false);
    }
    public void ShowAllReward()
    {
        for (int i = 0; i < bossslots.Length; i++)
        {
            bossslots[i].RewardObj.SetActive(true);
        }
    }
    public decimal TotalDmg; //여기에 피해 쌓임

    public void AddDmg(decimal dmg)
    {
        TotalDmg += dmg;
    }

    private static WorldBossManager _instance = null;


    public static WorldBossManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(WorldBossManager)) as WorldBossManager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    public itemiconslot[] rewardslot;

    public static decimal devidetime = 100000000;
    public List<RankItem> rankItemListNow = new List<RankItem>();


    [Button(Name = "데이터넣기")]
    public void ShopGive2()
    {
        PlayerBackendData userData = PlayerBackendData.Instance;

        Param paramData = new Param
        {
            { "WB1", 0d },
            { "WB2", 0d },
            { "WB3", 0d },
            { "WB4", 0d },
            { "WB5", 0d },
            { "WB6", 0d },
            { "WB7", 0d },
            { "WB8", 0d },
            { "WB9", 0d },
            { "WB10", 0d },
            { "WB11", 0d },
        };


        // key 컬럼의 값이 keyCode인 데이터 검색
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

        SendQueue.Enqueue(Backend.GameData.Update, "RankData", where, paramData, (callback) =>
        {
            // 이후 처리
            Debug.Log(callback);
        });
    }

    [Button(Name = "피떨구기")]
    public void Test()
    {
        RankInsert("123123129999999999999999999");
    }

    public static void RankInsert(string score)
    {
        string tableName = "RankData";
        string rowInDate;
        decimal endscore = 0;
        var bro = Backend.GameData.GetMyData(tableName, new Where());

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("데이터 조회 중 문제가 발생했습니다 : " + bro);
            return;
        }

        if (bro.FlattenRows().Count > 0)
        {
            rowInDate = bro.FlattenRows()[0]["inDate"].ToString();
        }
        else
        {
            Debug.Log("데이터가 존재하지 않습니다. 데이터 삽입을 시도합니다.");
            var bro2 = Backend.GameData.Insert(tableName);

            if (bro2.IsSuccess() == false)
            {
                Debug.LogError("데이터 삽입 중 문제가 발생했습니다 : " + bro2);
                return;
            }

            Debug.Log("데이터 삽입에 성공했습니다 : " + bro2);

            rowInDate = bro2.GetInDate();
        }

        Param param = new Param();
        string names = WorldBossDB.Instance.Find_id(playingplaydata.bossid).Dataname;
        param.Add(names, decimal.Parse(score) / devidetime);
        param.Add("PlayerAvatadata", PlayerBackendData.Instance.GetPlayerAvatadata()); //클래스;무기;보조무기;아바타;아바타무기;아바타보조무기
        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, playingplaydata.RankingID, "RankData", rowInDate, param,
            rankBro =>
            {
                Debug.Log(rankBro);
                if (rankBro.IsSuccess() == false)
                {
                    Debug.Log("산입실패");
                    return;
                }
                else
                {
                    WorldBossManager.playingplaydata.GetRankData(true);
                }
            });
    }

    [SerializeField] private WorldBossSlot data;
    public static WorldBossSlot nowplaydata = null;
    public static WorldBossSlot playingplaydata = null;
    public UIView WorldbossInfoPanel;
    public UIView WorldbossPanel;

    public void ShowBossdata(WorldBossSlot slot)
    {
        nowplaydata = slot;
        data.bossid = slot.bossid;
        data.rankItemList = slot.rankItemList;
        data.InitData2();
        ShowRank(data.rankItemList);
        WorldbossInfoPanel.Show(false);

        for (int i = 0; i < rewardslot.Length; i++)
        {
            rewardslot[i].gameObject.SetActive(false);
        }

        bool isdrop = false;
        var num = 0;
        string dropid = WorldBossDB.Instance.Find_id(data.bossid).mondropid;
        int num2 = int.Parse(MonDropDB.Instance.Find_id(dropid).num);

        for (var i = num2; i < MonDropDB.Instance.NumRows() - 1; i++)
        {
            if (MonDropDB.Instance.Find_num(i.ToString()).id.Equals(dropid))
            {
                isdrop = true;
                rewardslot[num].Refresh(MonDropDB.Instance.Find_num(i.ToString()).itemid,
                    int.Parse(MonDropDB.Instance.Find_num(i.ToString()).minhowmany), false);
                rewardslot[num].gameObject.SetActive(true);
                num++;
            }
            if (isdrop && !MonDropDB.Instance.Find_num(i.ToString()).id.Equals(dropid))
                break;
        }

    }


    public rankslot[] rankslots;

    void ShowRank(List<RankItem> list)
    {
        for (int i = 0; i < rankslots.Length; i++)
        {
            rankslots[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < list.Count; i++)
        {
            rankslots[i].SetRank(list[i].rank, list[i].nickname, dpsmanager.convertNumber(list[i].score),
                list[i].gamerInDate, list[i].extraData);
            rankslots[i].gameObject.SetActive(true);
        }
    }



    public void Bt_StartWorldBoss()
    {
        if (!Settingmanager.Instance.CheckServerOn())
        {
            return;
            //다시 시도
        }
        
        int num = (int)Enum.Parse(typeof(Timemanager.ContentEnumDaily),
            WorldBossDB.Instance.Find_id(data.bossid).arrynum);



        if (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype != "0")
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
        }

        if (int.Parse(WorldBossDB.Instance.Find_id(data.bossid).level) > PlayerBackendData.Instance.GetLv())
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/월드보스레벨불가"), alertmanager.alertenum.주의);
            return;
        }
        
        playingplaydata = nowplaydata;

        if (playingplaydata.curhp == 0)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/월드보스이미사망"), alertmanager.alertenum.주의);
            return;
        }

        Debug.Log((int)Timemanager.ContentEnumDaily.월드보스공격횟수);
        if (Timemanager.Instance.GetNowCount_daily(Timemanager.ContentEnumDaily.월드보스공격횟수) <= 0)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/월드보스횟수부족"), alertmanager.alertenum.주의);
            //횟수가 부족합니다.
            return;
        }
        
        if (Timemanager.Instance.GetNowCount_daily2(num) <= 0)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/월드보스횟수부족"), alertmanager.alertenum.주의);
            //횟수가 부족 합니다.
            return;
        }
        
     
        
        if (Timemanager.Instance.ConSumeCount_DailyAscny((int)Timemanager.ContentEnumDaily.월드보스공격횟수, 1))
        {
            if (Timemanager.Instance.ConSumeCount_DailyAscny(num, 1))
            {
                WorldbossInfoPanel.Hide(false);
                WorldbossPanel.Hide(false);
                TotalDmg = 0;
                Debug.Log(WorldBossDB.Instance.Find_id(playingplaydata.bossid).monsterid);
                mapmanager.Instance.LocateMap(WorldBossDB.Instance.Find_id(playingplaydata.bossid).monsterid);
                attacktimetext.Refresh();
            }
            else
            {
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/월드보스횟수부족"), alertmanager.alertenum.주의);
                return;
            }
        }
     
    }

    public void Bt_OpenWorldBossPanel()
    {
        if (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/월드보스중 불가"), alertmanager.alertenum.주의);
            return;
        }

        WorldbossPanel.Show(false);
    }

    public Settimetext rewardtimetext;
    public Settimetext attacktimetext;
    public void SetDropDataDropId(string Dropid)
    {
        bool isfindboss = false;

        List<string> Mon_DropItemIDboss = new List<string>();
        List<int> Mon_DropItemMinHowmanyboss = new List<int>();
        List<int> Mon_DropItemMaxHowmanyboss = new List<int>();
        List<float> Mon_DropItemPercentboss = new List<float>();

        int num2 = int.Parse(MonDropDB.Instance.Find_id(Dropid).num);
        for (int i = num2; i < MonDropDB.Instance.NumRows() - 1; i++)
        {
            if (Dropid != "0")
            {
                if (MonDropDB.Instance.Find_num(i.ToString()).id.Equals(Dropid))
                {
                    //     Debug.Log("들어갔다" + MonDropDB.Instance.Find_num(i.ToString()).itemid);
                    isfindboss = true;
                    Mon_DropItemIDboss.Add(MonDropDB.Instance.Find_num(i.ToString()).itemid);
                    Mon_DropItemMinHowmanyboss.Add(int.Parse(MonDropDB.Instance.Find_num(i.ToString()).minhowmany));
                    Mon_DropItemMaxHowmanyboss.Add(int.Parse(MonDropDB.Instance.Find_num(i.ToString()).maxhowmany));
                    Mon_DropItemPercentboss.Add(float.Parse(MonDropDB.Instance.Find_num(i.ToString()).rate));
                }
                else
                {
                    break;
                }
            }
        }

        List<string> id = new List<string>();
        List<int> hw = new List<int>();
        //보상지급
        for (int i = 0; i < Mon_DropItemIDboss.Count; i++)
        {
            Random.InitState((int)Time.deltaTime + PlayerBackendData.Instance.GetRandomSeed());
            int Ran_rate = Random.Range(0, 1000000); // 1,000,000이 100%이다.
            if (Ran_rate <= Mon_DropItemPercentboss[i])
            {
//                Debug.Log("줬다!");
                int Howmany = Random.Range(Mon_DropItemMinHowmanyboss[i], Mon_DropItemMaxHowmanyboss[i]);
                Inventory.Instance.AddItem(Mon_DropItemIDboss[i], Howmany, false, true);

                int num = id.IndexOf(Mon_DropItemIDboss[i]);
                if (num != -1)
                {
                    hw[num] += Howmany;
                }
                else
                {
                    id.Add(Mon_DropItemIDboss[i]);
                    hw.Add(Howmany);
                }
            }
        }
        QuestManager.Instance.AddCount(1, "worldboss");
        Inventory.Instance.ShowEarnItem3(id.ToArray(),hw.ToArray(),false);
        Savemanager.Instance.SaveInventory();
        Savemanager.Instance.Save();
    }


}

public class RankItem
{
    public string gamerInDate;
    public string nickname;
    public decimal score;
    public string index;
    public string rank;
    public string extraData = string.Empty;
    public string extraName = string.Empty;
    public string totalCount;

}