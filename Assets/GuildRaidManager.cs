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
    //�̱��游���.
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


    public decimal TotalDmg; //���⿡ ���� ����

    public void AddDmg(decimal dmg)
    {
        TotalDmg += dmg;
    }
    
    
    public GameObject[] RaidButtons; //0�� ���� 1�� ���� 2�� ���� ����
    //�ǰ� 0�̰� �� ���ط��� 1�� ������ �ٷ� �ޱ� ��������
    //�ǰ� 0�ʰ��ϰ� Ƚ���� 0�̶�� ����
    //�ǰ� 0�ʰ��ϰ� Ƚ���� �ִٸ� ����
    
    
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
        
        //�ǰ� 0�̰� �� ���ط��� 1�� ������ �ٷ� �ޱ� ��������
        //�ǰ� 0�ʰ��ϰ� Ƚ���� 0�̶�� ����
        //�ǰ� 0�ʰ��ϰ� Ƚ���� �ִٸ� ����
        int index = -1;
        //�� ���ط���������
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
                //���ذ� �ִ� 1�� �̻��̸� ���� �ƴϸ� ������ ��ȯ
                if (MyGuildManager.Instance.myguildclassdata.GuildRaidRankDmgs[index] 
                    * guildraidscale >=
                    decimal.Parse(mondata.hp) * 0.01m)
                {
                    //1�۰� �Ѵ´�.
                    RaidButtons[1].SetActive(true); //����â���
                }
                else
                {
                    //������ ���.
                    RaidButtons[2].SetActive(true);
                    if (Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.��巹�̵�õ�] > 0)
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
            if (Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.��巹�̵�õ�] > 0)
                RaidButtons[0].SetActive(true);
        }

        if (Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.��巹�̵�õ�] == 0)
            RaidButtons[0].SetActive(false);
        if (Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.��巹�̵庸��] == 0)
        {
            RaidButtons[1].SetActive(false);
            RaidButtons[2].SetActive(false);
            RaidButtons[3].SetActive(true); //�Ϸ�
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
    //    Debug.Log("���"+MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames.Count);
    for (int i = 0; i < MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames.Count; i++)
    {
//            Debug.Log("���ä�����");
        dmgplayer.Add(MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames[i],
            MyGuildManager.Instance.myguildclassdata.GuildRaidRankDmgs[i]);
//        Debug.Log(MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames[i] + MyGuildManager.Instance.myguildclassdata.GuildRaidRankDmgs[i]);
    }

    //����
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
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/����͸�����"), alertmanager.alertenum.����);
            return;
        }

        if (mapmanager.Instance.islocating)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/���̵��ߺҰ�"), alertmanager.alertenum.����);
            return;
        }

        if (!Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.��巹�̵�õ�)) return;
        TotalDmg = 0;//�����ʱ�ȭ
        MyGuildManager.Instance.MyGuildPanel.Hide(false);
        mapmanager.Instance.LocateMap(MyGuildManager.Instance.myguildclassdata.GuildRaidIDs);
    }


    void SetGuildRaid(string mapid)
    {
        if (!MyGuildManager.Instance.iscanaccept(1))
        {
            //������
            //   Debug.Log("������");
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/�θ������̻󰡴�"), alertmanager.alertenum.�Ϲ�);
            return;
        }
        
        Debug.Log(MyGuildManager.Instance.myguildclassdata.GuildRaidIDs);
        if (MyGuildManager.Instance.myguildclassdata.GuildRaidIDs == "")
        {
            SendQueue.Enqueue(Backend.Guild.GetMyGuildInfoV3, (callback) =>
            {
                Debug.Log(callback);

                // ���� ó��
                if (callback.IsSuccess())
                {
                    SendQueue.Enqueue(Backend.Guild.GetMyGuildGoodsV3, (callbackgoods) =>
                    {
                        Debug.Log(callbackgoods);

                        // ���� ó��
                        // ���� ó��
                        //������
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
                                    Debug.Log("���̵� ����");
                                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/��巹�̵����"),
                                        alertmanager.alertenum.�Ϲ�);
                                    chatmanager.Instance.ChattoStartGuildRaid(mapid);
                                    //�����ϴ�
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
            //���̵尡 ����
            return;
        }

        RefreshRaidRaidHp();
    }



    public decimal guildraidscale = 100000000;
    //���ظ� �B��
    public void DmgtoGuildMob()
    {
        achievemanager.Instance.AddCount(Acheves.��巹�̵����);
        decimal dmg = TotalDmg;
        string saveraiddid = MyGuildManager.Instance.myguildclassdata.GuildRaidIDs;
        SendQueue.Enqueue(Backend.Guild.GetMyGuildInfoV3, (callback) =>
        {
            // ���� ó��
            if (callback.IsSuccess())
            {
                Debug.Log(callback);
                SendQueue.Enqueue(Backend.Guild.GetMyGuildGoodsV3, (callbackgoods) =>
                {
                    Debug.Log(callbackgoods);
                    // ���� ó��
                    // ���� ó��
                    //������
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
                            //�������� ����Ʈ�� �ٸ��� �ȵȴ�.
                            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/��巹�̵尡�ٸ�"),
                                alertmanager.alertenum.�Ϲ�);
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
                                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/��巹�̵��������Ϸ�"),
                                        alertmanager.alertenum.�Ϲ�);
                                    chatmanager.Instance.ChattoGuildRaidAttack(PlayerBackendData.Instance.nickname,dmg,alldmg,mobhp);
                                    //�����ϴ�
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
        if (!Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.��巹�̵庸��)) return;


        int index =
            MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames.IndexOf(
                PlayerBackendData.Instance.nickname);

        int maxindex = guildraidid.IndexOf(MyGuildManager.Instance.myguildclassdata.GuildRaidIDs);

        string lastmob = "10003";
        
        for (int i = maxindex; i >= 0; i--)
        {
            decimal hp = 0; //�����۰�
            if (MyGuildManager.Instance.myguildclassdata.GuildRaidRankDmgs[index] * guildraidscale >=
                decimal.Parse(monsterDB.Instance.Find_id(guildraidid[i]).hp) * 0.01m)
            {
                Debug.Log(guildraidid[i] + "��÷");
                lastmob = guildraidid[i];
                break;
            }
        }
        
        
        
        SetDropData(lastmob);
        dungeondropsid.Clear();
        dungeondropshowmany.Clear();
        
        alertmanager.Instance.ShowAlert2(string.Format(Inventory.GetTranslate("Guild/��巹�̵庸��ȹ��"),
                Inventory.GetTranslate(MapDB.Instance.Find_id(lastmob)
                    .name)),
            alertmanager.alertenum.�Ϲ�);
        
        
        mondropmanager.Instance.GiveDropToInvenToryBoss(null,"7",Mon_DropItemIDboss
            , Mon_DropItemMinHowmanyboss,
            Mon_DropItemMaxHowmanyboss,
            Mon_DropItemPercentboss, true);

        Inventory.Instance.ShowEarnItem(dungeondropsid.ToArray(), dungeondropshowmany.ToArray(),false);
    }
    public void Bt_GetReward()
    {
        if (!Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.��巹�̵庸��)) return;

        SetDropData(MyGuildManager.Instance.myguildclassdata.GuildRaidIDs);
        dungeondropsid.Clear();
        dungeondropshowmany.Clear();
        
        mondropmanager.Instance.GiveDropToInvenToryBoss(null,"7",Mon_DropItemIDboss
            , Mon_DropItemMinHowmanyboss,
            Mon_DropItemMaxHowmanyboss,
            Mon_DropItemPercentboss, true);


        alertmanager.Instance.ShowAlert2(string.Format(Inventory.GetTranslate("Guild/��巹�̵庸��ȹ��"),
                Inventory.GetTranslate(MapDB.Instance.Find_id(MyGuildManager.Instance.myguildclassdata.GuildRaidIDs)
                    .name)),
            alertmanager.alertenum.�Ϲ�);
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
                    //     Debug.Log("����" + MonDropDB.Instance.Find_num(i.ToString()).itemid);
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
