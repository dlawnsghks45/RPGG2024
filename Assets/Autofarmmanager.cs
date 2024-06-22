using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using BackEnd;
using Doozy.Engine.UI;
using Doozy.Engine.Utils.ColorModels;
using UnityEngine;
using UnityEngine.UI;

public class Autofarmmanager : MonoBehaviour
{
    //싱글톤만들기.
    private static Autofarmmanager _instance = null;

    public static Autofarmmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Autofarmmanager)) as Autofarmmanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    public GameObject AutofarmFastobj; //로비화면에 8시간잉면 버튼이 뜨게한다.
    public itemiconslot[] items;

    public GameObject FastBattleAds;
    public GameObject FastBattleCrystal;
    public Text FastBattleText;

    //게임=처음만들떄 시작

    //필드 레벨에 맞는 사냥터를 불러옴
    string GetMapIdByFI()
    {
        if (PlayerBackendData.Instance.GetFieldLv().Equals(0))
        {
            return "1000";
        }

        for (int i = 0; i < MapDB.Instance.NumRows(); i++)
        {
            if (MapDB.Instance.GetAt(i).maptype.Equals("0") &&
                MapDB.Instance.GetAt(i).mapneednum.Equals((PlayerBackendData.Instance.GetFieldLv() - 1).ToString()))
            {
                return MapDB.Instance.GetAt(i).id;
            }
        }

        return "1000";
    }

    //최대 8시간 계산
    //초당 0.277
    //시간 당 천마리 저장
    //최대 8000마리 치  

    //필드레벨에 따른 사냥터 계산ㄹㄹㄹㄿㄱ
    //최소 보상을 불러오고
    //시간을 초로 나눠서 x을한다

    public Image MonImage;

    public Text MapName;
    // 1초 당 

    public decimal savedexp;
    public decimal savedgold;
    public Text Exp;
    public Text Gold;
    public Text TimeText;

    public UIView panel;
    public int TotalCount;
    private const int MaxTime = 28800;
    private DateTime dateTime;

    [SerializeField] private List<string> dropitemid = new List<string>();
    [SerializeField] private List<int> drophowmany = new List<int>();
    [SerializeField] private List<int> droppercent = new List<int>();

    void RefreshCountFast()
    {
        int adscount = (int)Enum.Parse(typeof(Timemanager.ContentEnumDaily),
            "광고_사냥1시간");

        int crycount = (int)Enum.Parse(typeof(Timemanager.ContentEnumDaily),
            "빠른전투");
        FastBattleAds.SetActive(false);
        FastBattleCrystal.SetActive(false);
        if (Timemanager.Instance.DailyContentCount[adscount] != 0)
        {
            FastBattleAds.SetActive(true);
            FastBattleCrystal.SetActive(false);
        }
        else if (Timemanager.Instance.DailyContentCount[crycount] != 0)
        {
            FastBattleAds.SetActive(false);
            FastBattleCrystal.SetActive(true);

            FastBattleText.text =
                string.Format(Inventory.GetTranslate("UI2/일가능횟수"),
                    Timemanager.Instance.DailyContentCount[crycount]
                    , Timemanager.Instance.DailyContentCount_Standard[crycount]);

        }
    }

    public void Bt_OpenAuto()
    {
        RefreshCountFast();
        this.dropitemid.Clear();
        this.drophowmany.Clear();
        panel.Show(false);
        string mapid = GetMapIdByFI();

        MapDB.Row mapdata = MapDB.Instance.Find_id(mapid);
        MapName.text = Inventory.GetTranslate(mapdata.name);
        string[] sprite = monsterDB.Instance.Find_id(mapdata.monsterid).sprite.Split(';');
        MonImage.sprite = SpriteManager.Instance.GetSprite(sprite[0]);

        dateTime = Timemanager.Instance.GetServerTime();
        TimeSpan timeDiff =
            dateTime - PlayerBackendData.Instance.PlayerTimes[(int)PlayerBackendData.timesenum.autofarm];

        if (timeDiff.TotalSeconds < 0)
        {
            dateTime = Timemanager.Instance.GetServerTime().AddHours(8);
                timeDiff =
                dateTime - PlayerBackendData.Instance.PlayerTimes[(int)PlayerBackendData.timesenum.autofarm];
            
        }

        if (timeDiff.TotalSeconds > MaxTime)
        {
            //시간초과
            TotalCount = MaxTime;
            TimeText.text = $"08:00:00";
        }
        else
        {
            TotalCount = (int)timeDiff.TotalSeconds;
            TimeText.text = $"{timeDiff.Hours:D2}:{timeDiff.Minutes:D2}:{timeDiff.Seconds:D2}";
        }

        //보상정보를 불러온다 몬스터의 드랍 테이블
        string dropid = monsterDB.Instance.Find_id(mapdata.monsterid).dropid;
        //Debug.Log("드로ㅗㅂ 아이 "+dropid);

        bool isfinddrop = false;
        for (int i = 0; i < MonDropDB.Instance.NumRows(); i++)
        {
            if (MonDropDB.Instance.GetAt(i).id.Equals(dropid))
            {
                //Debug.Log("asdasd" + i);
                isfinddrop = true;
                dropitemid.Add(MonDropDB.Instance.GetAt(i).itemid);
                drophowmany.Add(int.Parse(MonDropDB.Instance.GetAt(i).minhowmany));
                droppercent.Add(int.Parse(MonDropDB.Instance.GetAt(i).rate));
            }
            else
            {
                if (isfinddrop)
                {
                    //찾은 후 못찾게 된다면 끝났다는뜻
                    //    break;
                }
            }
        }

        foreach (var VARIABLE in items)
        {
            VARIABLE.gameObject.SetActive(false);
        }

        for (int i = 0; i < dropitemid.Count; i++)
        {
            switch (dropitemid[i])
            {
                //여ㅑ기수정
                case "1000":
                    if (MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.자동보상효율증가) != 0)
                    {
                        decimal gbadd = (decimal)(drophowmany[i] * (decimal)GetCount(i)) *
                                        (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum
                                            .자동보상효율증가);

                        Gold.text =
                            $"{((decimal)(drophowmany[i] * (decimal)GetCount(i)) + gbadd):N0} ({(decimal)(drophowmany[i] * (decimal)GetCount(i))}+{gbadd})";

                    }
                    else
                    {
                        Gold.text = ((decimal)drophowmany[i] * (decimal)GetCount(i)).ToString("N0");
                    }
                    break;
                case "1002":
                    if (MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.자동보상효율증가) != 0)
                    {
                        float gbadd = (drophowmany[i] * GetCount(i)) *
                                      MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.자동보상효율증가);

                        Exp.text =
                            $"{((drophowmany[i] * GetCount(i)) + gbadd):N0} ({(drophowmany[i] * GetCount(i))}+{gbadd})";

                    }
                    else
                    {
                        Exp.text = (drophowmany[i] * GetCount(i)).ToString("N0");
                    }

                    break;
                default:
                    int total = drophowmany[i] * GetCount(i);
                    if (MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.자동보상효율증가) != 0)
                    {
                        float gbadd = (drophowmany[i] * GetCount(i)) *
                                      MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.자동보상효율증가);

                        total += (int)gbadd;
                        items[i].ItemCount.color = Color.green;
                    }
                    else
                    {
                        items[i].ItemCount.color = Color.white;
                    }

                    items[i].Refresh(dropitemid[i], total, false);
                    items[i].gameObject.SetActive(true);
                    break;
            }
        }

        //Debug.Log("진행 시간은" + timeDiff.TotalSeconds);


        StartCoroutine(timer());
        //현재 시간으로 계산
    }

    int GetCount(int num)
    {
        float rate = droppercent[num] * 0.000001f;
        return (int)((TotalCount * 0.1388f) * rate);
    }

    int GetCount1Hr(int num)
    {
        float rate = droppercent[num] * 0.000001f;
        return (int)((3600 * 0.1388f) * rate);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator timer()
    {
        while (panel.gameObject.activeSelf)
        {
            yield return SpriteManager.Instance.GetWaitforSecond(1f);
            if (TotalCount < MaxTime)
            {
                TotalCount++;
            }
            else
            {
                yield return null;
            }

            int hours, minute, second;

            //시간공식

            hours = TotalCount / 3600; //시 공식

            minute = TotalCount % 3600 / 60; //분을 구하기위해서 입력되고 남은값에서 또 60을 나눈다.

            second = TotalCount % 3600 % 60; //마지막 남은 시간에서 분을 뺀 나머지 시간을 초로 계산함

            TimeText.text = $"{hours:D2}:{minute:D2}:{second:D2}";

            for (int i = 0; i < dropitemid.Count; i++)
            {
                switch (dropitemid[i])
                {
                    case "1000":
                        if (MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.자동보상효율증가) != 0)
                        {
                            decimal gbadd = (decimal)(drophowmany[i] * (decimal)GetCount(i)) *
                                            (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum
                                                .자동보상효율증가);

                            Gold.text =
                                $"{(decimal)((drophowmany[i] * (decimal)GetCount(i)) + gbadd):N0} ({(decimal)(drophowmany[i] * (decimal)GetCount(i))}+{gbadd})";

                        }
                        else
                        {
                            Gold.text = ((decimal)(drophowmany[i] * (decimal)GetCount(i))).ToString("N0");
                        }

                        break;
                    case "1002":
                        if (MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.자동보상효율증가) != 0)
                        {
                            decimal gbadd = (decimal)(drophowmany[i] * GetCount(i)) *
                                            (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum
                                                .자동보상효율증가);

                            Exp.text =
                                $"{((drophowmany[i] * GetCount(i)) + gbadd):N0} ({(drophowmany[i] * GetCount(i))}+{gbadd})";

                        }
                        else
                        {
                            Exp.text = (drophowmany[i] * GetCount(i)).ToString("N0");
                        }

                        break;
                    default:
                        int total = drophowmany[i] * GetCount(i);
                        if (MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.자동보상효율증가) != 0)
                        {
                            float gbadd = (drophowmany[i] * GetCount(i)) *
                                          MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.자동보상효율증가);

                            total += (int)gbadd;
                            items[i].ItemCount.color = Color.green;
                        }
                        else
                        {
                            items[i].ItemCount.color = Color.white;
                        }

                        items[i].Refresh(dropitemid[i], total, false);
                        items[i].gameObject.SetActive(true);
                        break;
                }
                /*
                switch (dropitemid[i])
                {
                    case "1000":
                        Gold.text = (drophowmany[i] * GetCount(i)).ToString("N0");
                        break;
                    case "1002":
                        Exp.text = (drophowmany[i] * GetCount(i)).ToString("N0");
                        break;
                    default:
                        items[i].Refresh(dropitemid[i],drophowmany[i] * GetCount(i),false);
                        break;
                }
                */
            }
        }
    }

    public void BtGetReward()
    {
        if (TotalCount < 3600)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/자동보상최소1시간"), alertmanager.alertenum.일반);
            return;
        }


        List<decimal> howmanystring = new List<decimal>();



        panel.Hide(false);
        alertmanager.Instance.Bt_ClickMenu();
        AutofarmFastobj.SetActive(false);
        dateTime = Timemanager.Instance.GetServerTime();
        PlayerBackendData.Instance.PlayerTimes[(int)PlayerBackendData.timesenum.autofarm]
            = dateTime;
        Settingmanager.Instance.OnlyInvenSave();

        for (int i = 0; i < dropitemid.Count; i++)
        {
            decimal total = (decimal)drophowmany[i] * (decimal)GetCount(i);
            if (MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.자동보상효율증가) != 0)
            {
                decimal gbadd = (decimal)((decimal)drophowmany[i] * (decimal)GetCount(i)) *
                                (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.자동보상효율증가);

                total += gbadd;
            }

            switch (dropitemid[i])
            {
                case "1000":
                    PlayerData.Instance.UpGold(total);
                    break;
                case "1002":
                    Inventory.Instance.AddItem(dropitemid[i], (int)total);

                    break;
                default:
                    Inventory.Instance.AddItem(dropitemid[i], (int)total);
                    break;
            }


            howmanystring.Add(total);
        }

        Inventory.Instance.ShowEarnItem4(dropitemid.ToArray(), howmanystring.ToArray(), false);
        Savemanager.Instance.SaveInventory();
        Savemanager.Instance.Save();
    }

    public void BtGetReward1hrAds()
    {
        if (PlayerBackendData.Instance.isadsfree)
        {
            if (Timemanager.Instance.ConSumeCount_DailyAscny((int)Timemanager.ContentEnumDaily.광고_사냥1시간, 1))
            {
                GiveReward1hr();
            }
        }
        else if (Advertisements.Instance.IsRewardVideoAvailable())
        {
            if (Timemanager.Instance.ConSumeCount_DailyAscny((int)Timemanager.ContentEnumDaily.광고_사냥1시간, 1))
            {
                AdmobRewardAd.Instance.ShowRewardVideo("광고_사냥1시간");
            }
        }
    }

    public void GiveReward1hr()
    {
        List<decimal> howmanystring = new List<decimal>();
        for (int i = 0; i < dropitemid.Count; i++)
        {
            decimal total = (decimal)drophowmany[i] * (decimal)GetCount1Hr(i);
            if (MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.자동보상효율증가) != 0)
            {
                decimal gbadd = (decimal)((decimal)drophowmany[i] * (decimal)GetCount1Hr(i)) *
                                (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.자동보상효율증가);

                total += gbadd;
            }

            switch (dropitemid[i])
            {
                case "1000":
                    PlayerData.Instance.UpGold(total);
                    break;
                case "1002":
                    Inventory.Instance.AddItem(dropitemid[i], (int)total);

                    break;
                default:
                    Inventory.Instance.AddItem(dropitemid[i], (int)total);
                    break;
            }

            howmanystring.Add(total);
        }

        Inventory.Instance.ShowEarnItem4(dropitemid.ToArray(), howmanystring.ToArray(), false);
        panel.Hide(false);
        alertmanager.Instance.Bt_ClickMenu();

        AutofarmFastobj.SetActive(false);
    }

    public void BtGetReward1hrCry()
    {
        if (PlayerBackendData.Instance.GetCash() >= 1000)
        {
            if (Timemanager.Instance.ConSumeCount_DailyAscny((int)Timemanager.ContentEnumDaily.빠른전투, 1))
            {
                PlayerData.Instance.DownCash(1000);
                Savemanager.Instance.SaveCash();
                GiveReward1hr();
            }
        }
    }

    public void Start()
    {
        if (PlayerBackendData.Instance.PlayerTimes[(int)PlayerBackendData.timesenum.autofarm].Equals(DateTime.MinValue))
        {
            //Debug.Log("아직 시작안해싿");
            //처음 시작 현재시작으로 조정
            dateTime = Timemanager.Instance.GetServerTime();
            PlayerBackendData.Instance.PlayerTimes[(int)PlayerBackendData.timesenum.autofarm]
                = dateTime;

        }
        else
        {
            //Debug.Log("시작했다");
        }

        TimeSpan timeDiff =
            Timemanager.Instance.NowTime -
            PlayerBackendData.Instance.PlayerTimes[(int)PlayerBackendData.timesenum.autofarm];

        if (timeDiff.TotalSeconds > MaxTime)
        {
            AutofarmFastobj.SetActive(true);
            alertmanager.Instance.NotiCheck_AutoFarm(timeDiff.TotalSeconds >= 3600);
        }
        else
        {
            AutofarmFastobj.SetActive(false);
            alertmanager.Instance.NotiCheck_AutoFarm(false);
        }
    }


    #region 오프라인 보상

    public UIView Off_panel;
    public Image Off_MonImage;
    public Text Off_MapName;

    public decimal savedexp_Offline;
    public decimal savedgold_Offline;
    public Text Exp_Offline;
    public Text Gold_Offline;
    public Text Kill_Offline;
    public Text TimeText_Offline;

    public UIView panel_Offline;
    public double TotalCount_Offline;
    private const int MaxTime_Offline = 21600;
    private DateTime dateTime_Offline;

    [SerializeField] private List<string> dropitemid_Offline = new List<string>();
    [SerializeField] private List<int> drophowmany_Offline = new List<int>();
    [SerializeField] private List<int> droppercent_Offline = new List<int>();
    public itemiconslot[] items_Offline;

     public void Bt_OpenAuto_Offline(DateTime servertime,DateTime OfflineTime,double totalsecond)
     {
         TotalCount_Offline = totalsecond;
        this.dropitemid_Offline.Clear();
        this.drophowmany_Offline.Clear();
        panel_Offline.Show(false);
        string mapid_Offline = GetMapIdBy_Offline();

        MapDB.Row mapdata = MapDB.Instance.Find_id(mapid_Offline);
        Off_MapName.text = Inventory.GetTranslate(mapdata.name);
        string[] sprite = monsterDB.Instance.Find_id(mapdata.monsterid).sprite.Split(';');
        Off_MonImage.sprite = SpriteManager.Instance.GetSprite(sprite[0]);

        TimeSpan timeDiff =
            servertime - OfflineTime;

        if (timeDiff.TotalSeconds > MaxTime_Offline)
        {
            //시간초과
            TotalCount_Offline = MaxTime_Offline;
            TimeText_Offline.text = $"06:00:00";
        }
        else
        {
            TotalCount_Offline = (int)timeDiff.TotalSeconds;
            TimeText_Offline.text = $"{timeDiff.Hours:D2}:{timeDiff.Minutes:D2}:{timeDiff.Seconds:D2}";
        }

        //보상정보를 불러온다 몬스터의 드랍 테이블
        string dropid = monsterDB.Instance.Find_id(mapdata.monsterid).dropid;
        //Debug.Log("드로ㅗㅂ 아이 "+dropid);
        MonDropDB.Row[] data = MonDropDB.Instance.FindAll_id(dropid).ToArray();
        int hour, minute, second;
        //시간공식
        hour = (int)TotalCount_Offline / 3600; //분을 구하기위해서 입력되고 남은값에서 또 60을 나눈다.
        minute = (int)TotalCount_Offline % 3600 / 60; //분을 구하기위해서 입력되고 남은값에서 또 60을 나눈다.
        second = (int)TotalCount_Offline % 3600 % 60; //마지막 남은 시간에서 분을 뺀 나머지 시간을 초로 계산함
        TimeText_Offline.text = $"{hour:D2}:{minute:D2}:{second:D2}";
        for (int i = 0; i < data.Length; i++)
        {
                Debug.Log("asdasd" + i);
                dropitemid_Offline.Add(data[i].itemid);
                drophowmany_Offline.Add(int.Parse(data[i].minhowmany));
                droppercent_Offline.Add(int.Parse(data[i].rate));
        }

        foreach (var VARIABLE in items_Offline)
        {
            VARIABLE.gameObject.SetActive(false);
        }

        Kill_Offline.text = $"{GetCount_OfflineKill():N0} Kills";
        for (int i = 0; i < dropitemid_Offline.Count; i++)
        {
            switch (dropitemid_Offline[i])
            {
                //여ㅑ기수정
                case "1000":
                        decimal gbadd = (decimal)(drophowmany_Offline[i] * (decimal)GetCount_Offline(i)) *
                                        (decimal)Battlemanager.Instance.mainplayer.Stat_ExtraGold;
                        Gold_Offline.text =
                            $"{((decimal)(drophowmany_Offline[i] * (decimal)GetCount_Offline(i)) + gbadd):N0}"; //({(decimal)(drophowmany_Offline[i] * (decimal)GetCount_Offline(i))}+{gbadd})";
                        Debug.Log("A골드" + drophowmany_Offline[i]);
                        Debug.Log("B골드" + GetCount_Offline(i));
                    break;
                case "1002":
                        Exp_Offline.text =
                            $"{(PlayerData.Instance.GetExp(drophowmany_Offline[i] * GetCount_Offline(i))):N0} "; // ({(drophowmany_Offline[i] * GetCount_Offline(i))}+{gbadd})";
                        Debug.Log("A" + drophowmany_Offline[i]);
                        Debug.Log("B" + GetCount_Offline(i));
                        break;
                default:
                    int total = drophowmany_Offline[i] * (int)GetCount_Offline(i);
                    Debug.Log("A" + drophowmany_Offline[i]);
                    Debug.Log("B" + GetCount_Offline(i));
                    items_Offline[i].Refresh(dropitemid_Offline[i], total, false);
                    items_Offline[i].gameObject.SetActive(true);
                    break;
            }
        }
Debug.Log("추가 경험치" + PlayerData.Instance.mainplayer.Stat_ExtraExp);
Debug.Log("추가 골드" + PlayerData.Instance.mainplayer.Stat_ExtraGold);
        //Debug.Log("진행 시간은" + timeDiff.TotalSeconds);

    //    StartCoroutine(timer());
        //현재 시간으로 계산
    }

     int GetCount_OfflineKill()
     {
         int a = 24;
        
         double b = a / (double)PlayerBackendData.Instance.Offlinedata.time;

         return (int)(TotalCount_Offline * b);
     }
     decimal GetCount_Offline(int num)
     {
         float rate = (float)droppercent_Offline[num] * 0.000001f;

         float a = 0;
         Debug.Log("데이터 레벨은" + PlayerBackendData.Instance.Offlinedata.level);
         switch (PlayerBackendData.Instance.Offlinedata.level)
         {
             case 1:
                 a = 8;
                 break;
             case 2:
                 a = 16;
                 break;
             case 3:
                 a = 24;
                 break;
         }

         double b = a / PlayerBackendData.Instance.Offlinedata.time;
         
         return (int)((TotalCount_Offline * b) * rate);
     }

     
    #endregion

    string GetMapIdBy_Offline()
    {
        return PlayerBackendData.Instance.Offlinedata.mapid;
    }

    //보상
    public GameObject offlineCheckPanel;
    public Text offlineCheckTime;

    public bool isChecking;

    public int totalchecktime;
    public void StartCheckOffline()
    {
        offlineCheckPanel.SetActive(true);
        offlineCheckTime.text = "00:00";
        isChecking = true;
        StartCoroutine(timer_CheckOffline());
    }

    public void FinishCheckOffline()
    {
        if (isChecking)
        {
            isChecking = false;
            offlineCheckPanel.SetActive(false);
            PlayerBackendData.Instance.Offlinedata = new OfflineData();

            PlayerBackendData.Instance.Offlinedata.mapid = PlayerBackendData.Instance.nowstage;
            PlayerBackendData.Instance.Offlinedata.time = totalchecktime;
            PlayerBackendData.Instance.Offlinedata.level = mapmanager.Instance.savespawncount;

            Param param = new Param();

            param.Add("OfflineData", PlayerBackendData.Instance.Offlinedata);
            Where where = new Where();
            where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
            if (PlayerBackendData.Instance.Offlinedata != null)
            {
                SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param,
                    (callback) =>
                    {
                        if (callback.IsSuccess())
                        {
                //            Debug.Log("저장완료");
                            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/오프라인기록완료"),alertmanager.alertenum.일반);
                        }
                    });
            }
        }
    }

    public void initCheckOffline()
    {
        offlineCheckPanel.SetActive(false);
        isChecking = false;
        totalchecktime = 0;
    }

    public GameObject OffPanel;
    public Text Data_offlinemapid;
    public Text Data_offlinelevel;
    public Text Data_offlinetime;
    public void Bt_RefreshOfflineData()
    {
        
        if (PlayerBackendData.Instance.Offlinedata != null)
        {
            Data_offlinemapid.text =
                Inventory.GetTranslate(MapDB.Instance.Find_id(PlayerBackendData.Instance.Offlinedata.mapid).name);
            Data_offlinelevel.text = PlayerBackendData.Instance.Offlinedata.level.ToString();
            Data_offlinetime.text = PlayerBackendData.Instance.Offlinedata.time.ToString();
        }
        else
        {
            Data_offlinemapid.text = "";
            Data_offlinelevel.text = "";
            Data_offlinetime.text = "";
        }
    }
        IEnumerator timer_CheckOffline()
    {
        while (offlineCheckPanel.activeSelf)
        {
            yield return SpriteManager.Instance.GetWaitforSecond(1f);
            totalchecktime++;
            int  minute, second;
            //시간공식
            minute = totalchecktime % 3600 / 60; //분을 구하기위해서 입력되고 남은값에서 또 60을 나눈다.
            second = totalchecktime % 3600 % 60; //마지막 남은 시간에서 분을 뺀 나머지 시간을 초로 계산함
            offlineCheckTime.text = $"{minute:D2}:{second:D2}";
        }
    }

        public void Bt_EarnOfflineReward()
        {
            PlayerData.Instance.RefreshPlayerstat();
            List<decimal> howmanystring = new List<decimal>();


            DateTime nowtimeNow = Timemanager.Instance.GetServerTime();
            Param param = new Param
            {
                { "OfflineTime", nowtimeNow },
            };
        
            Where where = new Where();
            where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
            SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    for (int i = 0; i < dropitemid_Offline.Count; i++)
                    {
                        //    Debug.Log("지급 아이템");
                        decimal total = (decimal)drophowmany_Offline[i] * GetCount_Offline(i);

//                        Debug.Log("개수는" + total);
                        
                        switch (dropitemid_Offline[i])
                        {
                            case "1000":
                                PlayerData.Instance.UpGoldMon(total);
                                break;
                            case "1002":
                                Inventory.Instance.AddItem(dropitemid_Offline[i], (int)total);
                                //   Debug.Log(PlayerData.Instance.GetExp(total) + "경험치");
                                howmanystring.Add(PlayerData.Instance.GetExp(total));
                                break;
                            default:
                                Inventory.Instance.AddItem(dropitemid_Offline[i], (int)total);
                                howmanystring.Add(total);
                                break;
                        }
                    }

                   // Inventory.Instance.ShowEarnItem4(dropitemid_Offline.ToArray(), howmanystring.ToArray(), false);
                                Savemanager.Instance.SaveInventory();
                                Savemanager.Instance.Save();
                                LogManager.OfflineRewardLog((int)TotalCount_Offline,dropitemid_Offline,drophowmany_Offline);
                                TotalCount_Offline = 0;
                }
                else
                {
                    //
                }
            });
            
        
        }
}