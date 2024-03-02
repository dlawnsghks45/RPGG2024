using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BackEnd;
using Doozy;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class GuildRaidManager : MonoBehaviour
{
    //싱글톤만들기.
    private static GuildRaidManager _instance = null;

    public static GuildRaidManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(GuildRaidManager)) as GuildRaidManager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    public GameObject haveraid;
    public GameObject noraid;
    public gaugebarslot guildpanel_raidbar;
    public Image guildpane_RaidImage;
    public Text guildpanel_raidtext;

    [SerializeField]
    itemiconslot[] reward;


    public decimal TotalDmg; //여기에 피해 쌓임

    public void AddDmg(decimal dmg)
    {
        TotalDmg += dmg;
    }
    
    
    public GameObject[] RaidButtons; //0번 전투 1번 보상 2번 보정 보상
    //피가 0이고 내 피해량이 1퍼 넘으면 바로 받기 나오게함
    //피가 0초과하고 횟수가 0이라면 보정
    //피가 0초과하고 횟수가 있다면 전투
    
    
    public void RefreshRaidRaidHp()
    {
        GuildItem data = MyGuildManager.Instance.myguildclassdata;
        monsterDB.Row mondata = monsterDB.Instance.Find_id(data.GuildRaidIDs);
        if (data.GuildRaidIDs == "")
        {
            haveraid.SetActive(false);
            noraid.SetActive(true);
            GuildRaidmini.SetActive(false);
            return;
        }

        haveraid.SetActive(true);
        noraid.SetActive(false);
        
        
        decimal curhp = data.GuildRaidCurHPs;
        decimal maxhp = decimal.Parse(mondata.hp);

        guildpane_RaidImage.sprite =
            SpriteManager.Instance.GetSprite((mondata.sprite));
        guildpanel_raidbar.RefreshBarD(curhp,maxhp);
        guildpanel_raidtext.text = Inventory.GetTranslate(MapDB.Instance.Find_id(data.GuildRaidIDs).name);

        GuildRaidMon.sprite = guildpane_RaidImage.sprite;
        GuildRaidHpmini.RefreshBarD(curhp,maxhp);


        GuildRaidmini.SetActive(true);
        
        if(curhp == 0)
            GuildRaiddieobj.SetActive(true);
        else
        {
            GuildRaiddieobj.SetActive(false);
        }
        foreach (var t in reward)
        {
            t.gameObject.SetActive(false);
        }
        bool isdrop= false;
        var num = 0;
        
        //피가 0이고 내 피해량이 1퍼 넘으면 바로 받기 나오게함
        //피가 0초과하고 횟수가 0이라면 보정
        //피가 0초과하고 횟수가 있다면 전투
        int index = -1;
        //내 피해량가져오기
        if (MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames.Count != 0)
        {
            index =
                MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames.IndexOf(
                    PlayerBackendData.Instance.nickname);
        }

        foreach (var VARIABLE in RaidButtons)
        {
            VARIABLE.gameObject.SetActive(false);
        }
        
        if (MyGuildManager.Instance.myguildclassdata.GuildRaidCurHPs == 0)
        {
            if (index != -1)
            {
                //피해가 있다 1퍼 이상이면 보상 아니면 보정을 소환
                if (MyGuildManager.Instance.myguildclassdata.GuildRaidRankDmgs[index] 
                    * guildraidscale >=
                    decimal.Parse(mondata.hp) * 0.01m)
                {
                    //1퍼가 넘는다.
                    RaidButtons[1].SetActive(true); //보상창출력
                }
                else
                {
                    //보정이 뜬다.
                    RaidButtons[2].SetActive(true);
                    if (Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.길드레이드시도] > 0)
                        RaidButtons[0].SetActive(true);
                }
            }
            else
            {
                RaidButtons[0].SetActive(true);
            }
        }
        else
        {
            if (Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.길드레이드시도] > 0)
                RaidButtons[0].SetActive(true);
        }

        if (Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.길드레이드시도] == 0)
            RaidButtons[0].SetActive(false);
        if (Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.길드레이드보상] == 0)
        {
            RaidButtons[1].SetActive(false);
            RaidButtons[2].SetActive(false);
            RaidButtons[3].SetActive(true); //완료
        }
        
        for (var i = 0; i < MonDropDB.Instance.NumRows()-1; i++)
        {
            if (MonDropDB.Instance.Find_num(i.ToString()).id.Equals(mondata.bossdrop))
            {
                isdrop = true;
                reward[num].Refresh(MonDropDB.Instance.Find_num(i.ToString()).itemid,int.Parse(MonDropDB.Instance.Find_num(i.ToString()).minhowmany),false);
                reward[num].gameObject.SetActive(true);
                num++;
            }
            if (isdrop && !MonDropDB.Instance.Find_num(i.ToString()).id.Equals(mondata.bossdrop))
                break;
        }
    }

    public Image raidmemberbossimage;
    public Text raidmemberbossname;
    public gaugebarslot raidmemberbosshp;
    public UIView raidmemberview;
    public GuildRaidmemberslot[] raidmemberslots;
    public void ShowDmgPlayer()
    {
        raidmemberbosshp.RefreshBarD(guildpanel_raidbar.dcur,guildpanel_raidbar.dmax);
        raidmemberbossimage.sprite = guildpane_RaidImage.sprite;
        raidmemberbossname.text =guildpanel_raidtext.text;
        
        foreach (var VARIABLE in raidmemberslots)
        {
            VARIABLE.gameObject.SetActive(false);
        }

        Dictionary<string, decimal> dmgplayer = new Dictionary<string, decimal>(); 
    //    Debug.Log("멤버"+MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames.Count);
    for (int i = 0; i < MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames.Count; i++)
    {
//            Debug.Log("나ㅓㅎ었다");
        dmgplayer.Add(MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames[i],
            MyGuildManager.Instance.myguildclassdata.GuildRaidRankDmgs[i]);
//        Debug.Log(MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames[i] + MyGuildManager.Instance.myguildclassdata.GuildRaidRankDmgs[i]);
    }

    //정렬
        var items = from pair in dmgplayer
            orderby pair.Value descending 
            select pair;


        int slotsnum = 0;
        decimal max =
            decimal.Parse(monsterDB.Instance.Find_id(MyGuildManager.Instance.myguildclassdata.GuildRaidIDs).hp);
        // Display results.
        foreach (KeyValuePair<string, decimal> pair in items)
        {
            raidmemberslots[slotsnum].Refresh(pair.Key, pair.Value * GuildRaidManager.Instance.guildraidscale,max
                );
            raidmemberslots[slotsnum].gameObject.SetActive(true);
            slotsnum++;
        }
        raidmemberview.Show();
    }

    public void Bt_GoAdMap()
    {
        if (MyGuildManager.Instance.myguildclassdata.GuildRaidIDs == "") return;
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

        if (!Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.길드레이드시도)) return;
        TotalDmg = 0;//피해초기화
        MyGuildManager.Instance.MyGuildPanel.Hide(false);
        mapmanager.Instance.LocateMap(MyGuildManager.Instance.myguildclassdata.GuildRaidIDs);
    }


    void SetGuildRaid(string mapid)
    {
        if (!MyGuildManager.Instance.iscanaccept(1))
        {
            //길드원임
            //   Debug.Log("길드원임");
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/부마스터이상가능"), alertmanager.alertenum.일반);
            return;
        }
        
        Debug.Log(MyGuildManager.Instance.myguildclassdata.GuildRaidIDs);
        if (MyGuildManager.Instance.myguildclassdata.GuildRaidIDs == "")
        {
            SendQueue.Enqueue(Backend.Guild.GetMyGuildInfoV3, (callback) =>
            {
                Debug.Log(callback);

                // 이후 처리
                if (callback.IsSuccess())
                {
                    SendQueue.Enqueue(Backend.Guild.GetMyGuildGoodsV3, (callbackgoods) =>
                    {
                        Debug.Log(callbackgoods);

                        // 이후 처리
                        // 이후 처리
                        //내굿즈
                        if (callbackgoods.IsSuccess())
                        {
                            MyGuildManager.Instance.SetGoods(callbackgoods);
                            MyGuildManager.Instance.myguilddata = callback;
                            MyGuildManager.Instance.SetMyGuildData();
                            MyGuildManager.Instance.Loadingbar.SetActive(false);
                            MyGuildManager.Instance.GuildMainPanel.SetActive(true);
                            MyGuildManager.Instance.RefreshGuildData();

                            MyGuildManager.Instance.myguildclassdata.SetGuildRaid(mapid);

                            Timemanager.Instance.RefreshNowTIme();
                            Param param = new Param
                            {
                                { "GuildRaidIDs", mapid },
                                { "GuildRaidCurHPs", monsterDB.Instance.Find_id(MapDB.Instance.Find_id(mapid).monsterid).hp},
                                { "isRaidfinish", false },
                                { "raidacceptdate", Timemanager.Instance.NowTime.Date },
                                { "GuildRaidRankNames",  MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames },
                                { "GuildRaidRankDmgs", MyGuildManager.Instance.myguildclassdata.GuildRaidRankDmgs }
                            };

                            SendQueue.Enqueue(Backend.Guild.ModifyGuildV3, param, (callbackmodi) =>
                            {
                                if (callbackmodi.IsSuccess())
                                {
                                    Debug.Log("레이드 시작");
                                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드레이드수락"),
                                        alertmanager.alertenum.일반);
                                    chatmanager.Instance.ChattoStartGuildRaid(mapid);
                                    //가능하다
                                    RefreshRaidRaidHp();
                                }
                            });
                        }
                    });
                }
            });
        }
        else
        {
            //레이드가 있음
            return;
        }

        RefreshRaidRaidHp();
    }



    public decimal guildraidscale = 100000000;
    //피해를 줒ㅁ
    public void DmgtoGuildMob()
    {
        achievemanager.Instance.AddCount(Acheves.길드레이드공격);
        decimal dmg = TotalDmg;
        string saveraiddid = MyGuildManager.Instance.myguildclassdata.GuildRaidIDs;
        SendQueue.Enqueue(Backend.Guild.GetMyGuildInfoV3, (callback) =>
        {
            // 이후 처리
            if (callback.IsSuccess())
            {
                Debug.Log(callback);
                SendQueue.Enqueue(Backend.Guild.GetMyGuildGoodsV3, (callbackgoods) =>
                {
                    Debug.Log(callbackgoods);
                    // 이후 처리
                    // 이후 처리
                    //내굿즈
                    if (callbackgoods.IsSuccess())
                    {
                        MyGuildManager.Instance.SetGoods(callbackgoods);
                        MyGuildManager.Instance.myguilddata = callback;
                        MyGuildManager.Instance.SetMyGuildData();
                        MyGuildManager.Instance.GuildMainPanel.SetActive(true);
                        MyGuildManager.Instance.RefreshGuildData();
                        decimal alldmg = 0;


                        if (!MyGuildManager.Instance.myguildclassdata.isRaidfinish &&
                            MyGuildManager.Instance.myguildclassdata.GuildRaidIDs != saveraiddid
                            && saveraiddid != "")
                        {
                            //진행중인 퀘스트와 다르면 안된다.
                            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드레이드가다름"),
                                alertmanager.alertenum.일반);
                            //GivePanel.Hide(true);
                            RefreshRaidRaidHp();
                            return;
                        }
                        else if (MyGuildManager.Instance.myguildclassdata.GuildRaidIDs == saveraiddid)
                        {

                            if (MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames.Contains(PlayerBackendData
                                    .Instance
                                    .nickname))
                            {
                                int index = MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames.IndexOf(
                                    PlayerBackendData
                                        .Instance.nickname);
                                MyGuildManager.Instance.myguildclassdata.GuildRaidRankDmgs[index] += 
                                  (Decimal.Truncate(dmg) / guildraidscale);
                                alldmg = MyGuildManager.Instance.myguildclassdata.GuildRaidRankDmgs[index];
                            }
                            else
                            {
                                MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames.Add(PlayerBackendData
                                    .Instance.nickname);
                                MyGuildManager.Instance.myguildclassdata.GuildRaidRankDmgs.Add(
                                    (Decimal.Truncate(dmg) / guildraidscale));
                                alldmg = dmg;
                            }
                            

                            decimal mobhp = Decimal.Truncate(MyGuildManager.Instance.myguildclassdata.GuildRaidCurHPs -
                                                             dmg);
                            if (mobhp < 0)
                                mobhp = 0;

                            Param param = new Param
                            {
                                { "GuildRaidCurHPs", mobhp.ToString(CultureInfo.InvariantCulture) },
                                { "GuildRaidRankNames", MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames },
                                { "GuildRaidRankDmgs", MyGuildManager.Instance.myguildclassdata.GuildRaidRankDmgs }
                            };

                            SendQueue.Enqueue(Backend.Guild.ModifyGuildV3, param, (callbackmodi) =>
                            {
                                if (callbackmodi.IsSuccess())
                                {

                                    MyGuildManager.Instance.myguildclassdata.GuildRaidCurHPs = mobhp;
                                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드레이드전투가완료"),
                                        alertmanager.alertenum.일반);
                                    chatmanager.Instance.ChattoGuildRaidAttack(PlayerBackendData.Instance.nickname,dmg,alldmg,mobhp);
                                    //가능하다
                                    RefreshRaidRaidHp();
                                    ShowDmgPlayer();
                                }
                            });
                        }
                    }
                });
            }
        });
    }

    public UIView RaidSelectPanel;
    public string nowselectraidmapid;
    public GameObject RaidAcceptPanel;
    public Text RaidAcceptName;
    public void Bt_ShowGuildRaidInfo(string mapid)
    {
        nowselectraidmapid = mapid;
        RaidAcceptPanel.SetActive(true);
        RaidAcceptName.text = Inventory.GetTranslate(MapDB.Instance.Find_id(mapid).name);
    }

    public void Bt_AcceptRaid()
    {
        SetGuildRaid(nowselectraidmapid);
        RaidAcceptPanel.SetActive(false);
        RaidSelectPanel.Hide(true);
    }

    public List<string> guildraidid = new List<string>();

    public void Bt_GetRewardLvScale()
    {
        if (!Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.길드레이드보상)) return;


        int index =
            MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames.IndexOf(
                PlayerBackendData.Instance.nickname);

        int maxindex = guildraidid.IndexOf(MyGuildManager.Instance.myguildclassdata.GuildRaidIDs);

        string lastmob = "10003";
        
        for (int i = maxindex; i >= 0; i--)
        {
            decimal hp = 0; //제일작게
            if (MyGuildManager.Instance.myguildclassdata.GuildRaidRankDmgs[index] * guildraidscale >=
                decimal.Parse(monsterDB.Instance.Find_id(guildraidid[i]).hp) * 0.01m)
            {
                Debug.Log(guildraidid[i] + "당첨");
                lastmob = guildraidid[i];
                break;
            }
        }
        
        
        
        SetDropData(lastmob);
        dungeondropsid.Clear();
        dungeondropshowmany.Clear();
        
        alertmanager.Instance.ShowAlert2(string.Format(Inventory.GetTranslate("Guild/길드레이드보상획득"),
                Inventory.GetTranslate(MapDB.Instance.Find_id(lastmob)
                    .name)),
            alertmanager.alertenum.일반);
        
        
        mondropmanager.Instance.GiveDropToInvenToryBoss(null,"7",Mon_DropItemIDboss
            , Mon_DropItemMinHowmanyboss,
            Mon_DropItemMaxHowmanyboss,
            Mon_DropItemPercentboss, true);

        Inventory.Instance.ShowEarnItem(dungeondropsid.ToArray(), dungeondropshowmany.ToArray(),false);
    }
    public void Bt_GetReward()
    {
        if (!Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.길드레이드보상)) return;

        SetDropData(MyGuildManager.Instance.myguildclassdata.GuildRaidIDs);
        dungeondropsid.Clear();
        dungeondropshowmany.Clear();
        
        mondropmanager.Instance.GiveDropToInvenToryBoss(null,"7",Mon_DropItemIDboss
            , Mon_DropItemMinHowmanyboss,
            Mon_DropItemMaxHowmanyboss,
            Mon_DropItemPercentboss, true);


        alertmanager.Instance.ShowAlert2(string.Format(Inventory.GetTranslate("Guild/길드레이드보상획득"),
                Inventory.GetTranslate(MapDB.Instance.Find_id(MyGuildManager.Instance.myguildclassdata.GuildRaidIDs)
                    .name)),
            alertmanager.alertenum.일반);
     Inventory.Instance.ShowEarnItem(dungeondropsid.ToArray(), dungeondropshowmany.ToArray(), false);
        RefreshRaidRaidHp();
    }

    public List<string> dungeondropsid = new List<string>();
    public List<string> dungeondropshowmany = new List<string>();

    public void AddLoot(string id, int count)
    {
        dungeondropsid.Add(id);
        dungeondropshowmany.Add(count.ToString());
    }

    string nowdropbossid = "";

    public List<string> Mon_DropItemIDboss = new List<string>();
    public List<int> Mon_DropItemMinHowmanyboss = new List<int>();
    public List<int> Mon_DropItemMaxHowmanyboss = new List<int>();
    public List<float> Mon_DropItemPercentboss = new List<float>();

    void SetDropData(string MonID)
    {
//        Debug.Log(monsterDB.Instance.Find_id(MonID).dropid);
        bool isfindboss = false;

        nowdropbossid = monsterDB.Instance.Find_id(MonID).bossdrop;
        //Debug.Log(nowdropbossid);

        Mon_DropItemIDboss.Clear();
        Mon_DropItemMinHowmanyboss.Clear();
        Mon_DropItemMaxHowmanyboss.Clear();
        Mon_DropItemPercentboss.Clear();

        int num2 = int.Parse(MonDropDB.Instance.Find_id(nowdropbossid).num);
        for (int i = num2; i < MonDropDB.Instance.NumRows() - 1; i++)
        {
            if (nowdropbossid != "0")
            {
                if (MonDropDB.Instance.Find_num(i.ToString()).id.Equals(nowdropbossid))
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
        
    }

    public GameObject GuildRaidmini;
    public Image GuildRaidMon;
    public gaugebarslot GuildRaidHpmini;
    public GameObject GuildRaiddieobj;

}
