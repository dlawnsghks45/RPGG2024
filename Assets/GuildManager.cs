using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Doozy.Engine.UI;
using Google.Play.Common.LoadingScreen;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class GuildManager : MonoBehaviour
{
    
    //싱글톤만들기.
    private static GuildManager _instance = null;
    public static GuildManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(GuildManager)) as GuildManager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }
    
    //길드 생성 조회 등을 관리 한다.
    //내길드 관련은 MYGuildManager

    
    //길드 엠블렘 데이터
    public Sprite[] Banners;
    public Sprite[] Flags;

    #region  길드 생성 관련

    public UIView GuildCreat_Panel;
    public GuildEmblemSlot Creat_emblem;
    public Text Creat_Banner_Text;
    public Text Creat_Flag_Text;

    public InputField Create_GuildNameInput;
    
    private int Creat_BannerNum =0;
    private int Creat_FlagNum = 0;

    public UIView GuildCreatAcceptPanel;
    public GuildEmblemSlot AcceptEmblem;
    public Text GuildAcceptName;


    public void Bt_ShowAcceptPanel()
    {
        if (Create_GuildNameInput.text == "")
        {
            //길드 이름 없음
            return;
        }
        
        GuildCreatAcceptPanel.Show(false);
        AcceptEmblem.SetEmblem(Creat_FlagNum, Creat_BannerNum);
        GuildAcceptName.text = Create_GuildNameInput.text;
    }
    
    
    public void Bt_InitCreateView()
    {
        GuildCreat_Panel.Show(false);
        Creat_FlagNum = 0;
        Creat_BannerNum = 0;
        Creat_emblem.SetEmblem(0,0);

        Creat_Banner_Text.text = "0";
        Creat_Flag_Text.text = "0";


        Create_GuildNameInput.text = "";



    }

    //문약
    public void Bt_Create_BannerRight()
    {
        if (Creat_BannerNum >= Banners.Length-1)
        {
            //못나감
            return;
        }

        Creat_BannerNum++;
        Creat_Banner_Text.text = Creat_BannerNum.ToString();
        Creat_emblem.SetEmblem(Creat_FlagNum, Creat_BannerNum);
    }
    public void Bt_Create_BannerLeft()
    {
        if (Creat_BannerNum == 0)
        {
            //못나감
            return;
        }

        Creat_BannerNum--;
        Creat_Banner_Text.text = Creat_BannerNum.ToString();
        Creat_emblem.SetEmblem(Creat_FlagNum, Creat_BannerNum);
    }
    
    //깃발
    public void Bt_Create_FLagRight()
    {
        if (Creat_FlagNum >= Flags.Length-1)
        {
            //못나감
            return;
        }

        Creat_FlagNum++;
        Creat_Flag_Text.text = Creat_FlagNum.ToString();
        Creat_emblem.SetEmblem(Creat_FlagNum, Creat_BannerNum);
    }
    public void Bt_Create_FlagLeft()
    {
        if (Creat_FlagNum == 0)
        {
            //못나감
            return;
        }

        Creat_FlagNum--;
        Creat_Flag_Text.text = Creat_FlagNum.ToString();
        Creat_emblem.SetEmblem(Creat_FlagNum, Creat_BannerNum);
    }


    public enum GuildBuffEnum
    {
        모든능력치증가,
        생명력증가,
        정신력증가,
        재사용시간감소,
        치명타확률증가,
        치명타피해증가,
        보스피해증가,
        무력화피해증가,
        경험치증가,
        골드획득량증가,
        길드원수증가,
        자동보상효율증가,
        드롭율증가,
        제작시간감소,
    }
//길드 창설 최종창 열기
    public void Bt_OpenGuildCreateAcceptPanel()
    {
        NoGuildPanel.Hide(true);
        GuildCreatAcceptPanel.Show(false);
        AcceptEmblem.SetEmblem(Creat_FlagNum,Creat_BannerNum);
        GuildAcceptName.text = Create_GuildNameInput.text;
    }

    //길드 창설 로직
    public void Bt_CreatGuild()
    {
        //크리스탈 계산
        if (PlayerBackendData.Instance.GetCash() < 2000)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/불꽃부족"),alertmanager.alertenum.주의);
            return;
        }
        
        string[] GuildBuffLv = new[]
        {
            "1",
            "1",
            "1",
            "1",
            "1",
            "1",
            "1",
            "1",
            "1",
            "1",
            "1",
            "1",
            "1",
            "1",
            "1",
            "1",
            "1"
        };
        
        //길드 생성 시도
        Param param = new Param
        {
            { "level", 1 }, //레벨 1 
            //   { "Exp", 0 }, //레벨 1 
            { "GuildBuffLV", GuildBuffLv },
            { "GuildFlag", Creat_FlagNum },
            { "GuildBanner", Creat_BannerNum},
            { "JoinLv", 1},
            { "JoinBp", 0},
            { "GuildMaxMemer", 23},
           // { "GuildGold", 0},
            { "GuildPt", 0},
            { "GuildNotice",Inventory.GetTranslate("Guild/길드공지글기본") },
            { "GuildWelcome", Inventory.GetTranslate("Guild/길드소개글기본") },
            { "GuildRaidCurHPs", "0"}
        };
        
        var bro = Backend.Guild.CreateGuildV3(Create_GuildNameInput.text, 4,param);

        if (bro.IsSuccess() == false) {
            Debug.LogError("길드를 생성하는중 오류가 발생했습니다. : " + bro);
            return;
        }

     
        
        
        PlayerData.Instance.DownCash(2000);
        Savemanager.Instance.SaveCash();
        GuildCreat_Panel.Hide(true);
        GuildCreatAcceptPanel.Hide(true);
        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드가창설됨"),alertmanager.alertenum.일반);
        Debug.Log("길드가 생성되었습니다. : " + bro);
        PlayerBackendData.Instance.ishaveguild = true;
        GuildJoinSucc();
        Bt_OpenGuild();
        //길드 채팅 연결
        chatmanager.Instance.OnGuild();

    }
    
    
    #endregion
    

    private void Start()
    {
        //초기길드데이터를 불러온다.
        SendQueue.Enqueue(Backend.Guild.GetMyGuildInfoV3, (callback) =>
        {
        //    Debug.Log("길wwwww드체크");
         //   Debug.Log(callback);
            if (!callback.IsSuccess())
            {
        //        Debug.Log("길드없다");
                MyGuildManager.Instance.OffSetBuffStat();
                return;
            }
            PlayerBackendData.Instance.ishaveguild = true;
            MyGuildManager.Instance.myguilddata = callback;
            MyGuildManager.Instance.SetMyGuildData(); //내 길드 데이터 불러온거 넣음
//            Debug.Log("길드가 있다");
            MyGuildManager.Instance.SetBuffStat(); //길드가 있으면 버프 설정
            //길드 버프 로직 시작
        });
    }

    public void GuildJoinSucc()
    {
        //초기길드데이터를 불러온다.
        SendQueue.Enqueue(Backend.Guild.GetMyGuildInfoV3, (callback) =>
        {
            if (!callback.IsSuccess())
            {
                MyGuildManager.Instance.OffSetBuffStat();
                return;
            }
            PlayerBackendData.Instance.ishaveguild = true;
            MyGuildManager.Instance.myguilddata = callback;
            MyGuildManager.Instance.SetMyGuildData(); //내 길드 데이터 불러온거 넣음
            GuildRecommendPanel.Hide(true);
            OtherGuildInfoPanel.Hide(true);
            NoGuildPanel.Hide(true);
            MyGuildManager.Instance.RefreshGuildData();
            MyGuildManager.Instance.OpenMyGuildPanel();
            //길드버프적용
            MyGuildManager.Instance.SetBuffStat();
            PlayerData.Instance.RefreshPlayerstat();
            //길드 버프 로직 시작
            //alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드가입됨"),alertmanager.alertenum.일반);
        });
    }
    public void Guildchecksetting()
    {
        //초기길드데이터를 불러온다.
        SendQueue.Enqueue(Backend.Guild.GetMyGuildInfoV3, (callback) =>
        {
            if (!callback.IsSuccess())
            {
                MyGuildManager.Instance.OffSetBuffStat();
                return;
            }
            PlayerBackendData.Instance.ishaveguild = true;
            MyGuildManager.Instance.myguilddata = callback;
            MyGuildManager.Instance.SetMyGuildData(); //내 길드 데이터 불러온거 넣음
            MyGuildManager.Instance.RefreshGuildData();
            //길드버프적용
            MyGuildManager.Instance.SetBuffStat();
            PlayerData.Instance.RefreshPlayerstat();
            //길드 버프 로직 시작
            //alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드가입됨"),alertmanager.alertenum.일반);
        });
    }
    public UIView NoGuildPanel;
    public void Bt_OpenGuild()
    {
        if (PlayerBackendData.Instance.ishaveguild)
        {
            MyGuildManager.Instance.OpenMyGuildPanel();
        }
        else
        {
            NoGuildPanel.Show(false);
        }
    }
    
    
    //길드 추천

    #region 길드추천

    public List<GuildItem> recoguilddata = new List<GuildItem>();
    public UIView GuildRecommendPanel;
    public GameObject Loadingbar_reco;
    public GameObject GuildRecoSlotPanel;
    public GuildrecoSlot[] Guildslots;
    
    
    private bool RefreshCooldown;
    private void falseRefreshCooldown()
    {
        RefreshCooldown = false;
    }
    
    
    public void Bt_ShowGuildRecommend()
    {
        if (RefreshCooldown)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/잠시후시도"),alertmanager.alertenum.일반);
            return;
        }
        Invoke(nameof(falseRefreshCooldown), 10);
        RefreshCooldown = true;

        
        foreach (var VARIABLE in Guildslots)
        {
            VARIABLE.gameObject.SetActive(false);
        }
        Loadingbar_reco.SetActive(true);
        GuildRecoSlotPanel.SetActive(false);
        recoguilddata.Clear();
        GuildRecommendPanel.Show(false);
        SendQueue.Enqueue(Backend.RandomInfo.GetRandomData, RandomType.Guild, "cb5a9f10-069d-11ee-b7ff-45e1842bef31", 1,
            1, 5, callback =>
            {
                // 이후 처리
                if (!callback.IsSuccess()) return;

                int num = 0;
                foreach (JsonData guildDataJson in callback.Rows())
                {
                    // 1. 랜덤 테이블 조회를 통해 길드 inDate가져오기
                    string guildInDate = guildDataJson["guildInDate"].ToString();

                    SendQueue.Enqueue(Backend.Guild.GetGuildInfoV3, guildInDate, callback2 => 
                    {
                        if (callback2.IsSuccess())
                        {
                            GuildItem item = GetGuildItem(callback2);
                            recoguilddata.Add(item);
                            Loadingbar_reco.SetActive(false);
                            GuildRecoSlotPanel.SetActive(true);
                            Guildslots[num].Refresh(item);
                            Guildslots[num].gameObject.SetActive(true);
                            num++;
                        }
                    });
                }

            });
    }

    GuildItem GetGuildItem(BackendReturnObject bro)
    {
        JsonData json = bro.GetFlattenJSON();
        GuildItem guildItem = new GuildItem();
        guildItem.memberCount = Int32.Parse(json["guild"]["memberCount"].ToString());
        guildItem.masterNickname = json["guild"]["masterNickname"].ToString();
        guildItem.inDate = json["guild"]["inDate"].ToString();
        guildItem.guildName = json["guild"]["guildName"].ToString();
        guildItem.level = Int32.Parse(json["guild"]["level"].ToString());
        guildItem.GuildMaxMemer = Int32.Parse(json["guild"]["GuildMaxMemer"].ToString());
        guildItem.GuildFlag = Int32.Parse(json["guild"]["GuildFlag"].ToString());
        guildItem.GuildBanner = Int32.Parse(json["guild"]["GuildBanner"].ToString());
        guildItem.JoinLv = Int32.Parse(json["guild"]["JoinLv"].ToString());
        guildItem.JoinBp = Int32.Parse(json["guild"]["JoinBp"].ToString());
        guildItem.GuildWelcome = json["guild"]["GuildWelcome"].ToString();
        guildItem.GuildBuffLV = new int[json["guild"]["GuildBuffLV"].Count];
        guildItem.GuildBuffLV.Initialize();
        for (int i = 0; i < json["guild"]["GuildBuffLV"].Count; i++)
        {
            guildItem.GuildBuffLV[i] = Int32.Parse(json["guild"]["GuildBuffLV"][i].ToString());
        }

        if (json["guild"].ContainsKey("_immediateRegistration"))
        {
            guildItem._immediateRegistration =
                json["guild"]["_immediateRegistration"].ToString() == "True" ? true : false;
        }

        return guildItem;
    }

    public UIView OtherGuildInfoPanel;
    public GuildEmblemSlot otheremblem;
    public Text otherguildlv;
    public Text otherguildname;
    public Text othermembercount;
    public Text othermasternickname;
    public Text otherWelcome;
    public Text otherNeedLv;
    public Text otherNeedBp;
    public Text otherJoinType;

    public void Bt_ApplyGuild()
    {
        if (PlayerBackendData.Instance.GetLv() < nowotherGuildItem.JoinLv)
        {
            //레벨딸림
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/가입레벨이부족"),alertmanager.alertenum.일반);
            return;
        }
        if (PlayerBackendData.Instance.GetBattlePoint() < nowotherGuildItem.JoinBp)
        {
            //전투력딸림
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/가입전투력이부족"),alertmanager.alertenum.일반);
            return;
        }
        if (nowotherGuildItem.memberCount == nowotherGuildItem.GuildMaxMemer)
        {
            //전투력딸림
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/인원이꽉참"),alertmanager.alertenum.일반);
            return;
        }
        SendQueue.Enqueue(Backend.Guild.ApplyGuildV3,nowotherGuildItem.inDate, ( callback ) => 
        {
            // 이후 처리
            if (callback.IsSuccess())
            {
                Debug.Log("가입신청성공");
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드가입신청완료"),alertmanager.alertenum.일반);
                //즉시가입여부 확ㅇ인 
                GuildManager.Instance.GuildJoinSucc();
            }
            else
            {
                switch (callback.GetStatusCode())
                {
                    case "409":
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/이미요청길드"),alertmanager.alertenum.일반);
                        break;
                }
            }
        });
    }

    [SerializeField] private string[] guild_tuto;

    public void JoinTutorialGuld()
    {
        for (int i = 0; i < guild_tuto.Length; i++)
        {
            SendQueue.Enqueue(Backend.Guild.ApplyGuildV3, guild_tuto[i], (callback) =>
            {
                // 이후 처리
                if (callback.IsSuccess())
                {
                    //즉시가입여부 확ㅇ인 
                    GuildManager.Instance.GuildJoinSucc();
                }
            });
        }
    }

    private GuildItem nowotherGuildItem;
    public void Bt_ShowSelectGuild(GuildItem info)
    {
        nowotherGuildItem = info;
        OtherGuildInfoPanel.Show(false);
        otheremblem.SetEmblem(info.GuildFlag,info.GuildBanner);
        otherguildlv.text = $"Lv.{info.level}";
        otherguildname.text = info.guildName;
        othermasternickname.text = info.masterNickname;
        othermembercount.text = $"{info.memberCount}/{info.GuildMaxMemer}";
        otherWelcome.text = info.GuildWelcome;
        otherNeedLv.text = $"Lv.{info.JoinLv}";
        otherNeedBp.text = $"{info.JoinBp:N0}";
        otherJoinType.text = info._immediateRegistration
            ? Inventory.GetTranslate("Guild/즉시가입")
            : Inventory.GetTranslate("Guild/승인가입");
    }
    public void Bt_ShowSelectGuild(string guildindate)
    {
        SendQueue.Enqueue(Backend.Guild.GetGuildInfoV3, guildindate, callback2 => 
        {
            if (callback2.IsSuccess())
            {
                GuildItem info = GetGuildItem(callback2);
                nowotherGuildItem = info;
                OtherGuildInfoPanel.Show(false);
                otheremblem.SetEmblem(info.GuildFlag,info.GuildBanner);
                otherguildlv.text = $"Lv.{info.level}";
                otherguildname.text = info.guildName;
                othermasternickname.text = info.masterNickname;
                othermembercount.text = $"{info.memberCount}/{info.GuildMaxMemer}";
                otherWelcome.text = info.GuildWelcome;
                otherNeedLv.text = $"Lv.{info.JoinLv}";
                otherNeedBp.text = $"{info.JoinBp:N0}";
                otherJoinType.text = info._immediateRegistration
                    ? Inventory.GetTranslate("Guild/즉시가입")
                    : Inventory.GetTranslate("Guild/승인가입");
            }
        });
        
        
      
    }


    public InputField GuildSearchInput;
    private bool SearchCooldown;
    private void falseEarchCooldown()
    {
        SearchCooldown = false;
    }
    public void SearchGuild()
    {
        if (SearchCooldown)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/10초뒤"),alertmanager.alertenum.일반);
            return;
        }

        Invoke(nameof(falseEarchCooldown), 10);
        SearchCooldown = true;
        
        
        
        SendQueue.Enqueue(Backend.Guild.GetGuildIndateByGuildNameV3, GuildSearchInput.text, ( callback ) => 
        {
            //이후 처리
            if (callback.IsSuccess())
            {
                string guildIndate = callback.GetReturnValuetoJSON()["guildInDate"]["S"].ToString();
                SendQueue.Enqueue(Backend.Guild.GetGuildInfoV3, guildIndate, callback2 => 
                {
                    if (callback2.IsSuccess())
                    {
                        //정보를 불러옴
                        Bt_ShowSelectGuild(GetGuildItem(callback2));
                    }
                    else
                    {
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/10초뒤"),alertmanager.alertenum.일반);
                    }
                    // 이후 처리
                });
            }
            else
            {
                switch (callback.GetStatusCode())
                {
                    //이름으로 불러오는거 체크
                    case "404": //길드이름이 없다
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/없는길드다"),alertmanager.alertenum.일반);
                        break;
                }
            }
        });
    }

    #endregion



}
