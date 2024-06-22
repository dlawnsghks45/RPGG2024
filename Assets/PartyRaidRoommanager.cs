using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Doozy.Engine.UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PartyRaidRoommanager : MonoBehaviour
{
    public Color[] syscolor;

    public UIView PartyradPanel;

    //싱글톤만들기.
    private static PartyRaidRoommanager _instance = null;

    public static PartyRaidRoommanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(PartyRaidRoommanager)) as PartyRaidRoommanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    public GameObject PartyNotStartObj;
    public GameObject PartyStartObj;


    public int SelectPartyUsernum;

    //파티룸 데이터
    public PartyRoom partyroomdata = new PartyRoom();

    //내 파티 데이터
    public int mypartynum; //내 파티 자리
    public bool isready;
    public countpanel LevelCount;


    public GameObject ExitButtons;
    //버튼
    public GameObject StartButton;
    public GameObject ChangemapButton;
    public GameObject AdmentiseButton;
    public GameObject JoinUserButton;

    public GameObject ChatInput;

    //정보
    public PartyMemberslot[] PartyMember;

    //초대
    public UIView InvitePanel;

    public InputField InviteNicknameinput;

    //초대 받음
    public GameObject PartyInvitedPanel;
    public Text PartyInvitedMap;
    public Text PartyInvitedNickname;


    public string[] DropId;

    //레이드 시작확인
    public GameObject RaidReadyPanel;




    #region 파티초대

    //파티초대
    public void ShowInvitePanel()
    {
        if (nowmyleadernickname != PlayerBackendData.Instance.nickname)
        {
            alertmanager.Instance.ShowAlert(
                Inventory.GetTranslate("UI7/파장아님"),
                alertmanager.alertenum.일반);
            return;
        }
        InvitePanel.Show(false);
        InviteNicknameinput.text = "";
    }

    public void Bt_InvitePlayer()
    {
        InvitePanel.Hide(false);
        PartyraidChatManager.Instance.Chat_invitePlayer(InviteNicknameinput.text);
    }


    public string invitednickname;
    public string nowmyleadernickname;

    public void ShowInvitedPanel(string mapid, int level, string leadernick)
    {
        invitednickname = leadernick;
        //system;PI;내이름;유저이름;mapname;level
        PartyInvitedMap.text = string.Format(Inventory.GetTranslate("UI7/초대내용"),
            Inventory.GetTranslate(MapDB.Instance.Find_id(mapid).name), level);
        Debug.Log("난이도는" + PartyInvitedMap.text);
        readymaptext.text = string.Format(Inventory.GetTranslate("UI7/초대내용"),
            Inventory.GetTranslate(MapDB.Instance.Find_id(mapid).name), level);
        PartyInvitedNickname.text = string.Format(Inventory.GetTranslate("UI7/누구의 파티"),
            leadernick);

        PartyInvitedPanel.SetActive(true);
    }

    public void Bt_AcceptParty()
    {
        PartyraidChatManager.Instance.Chat_AcceptInvite();
    }


    #endregion



    public partyraidreadyslot[] readyslots;
    public GameObject readyyesnopanel;

    public Text readymaptext;

    //레이드 시작
    public void Bt_StartRaid()
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/사냥터만가능"), alertmanager.alertenum.주의);
            return;
        }
        
        if (mapmanager.Instance.islocating)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/맵이동중불가"),alertmanager.alertenum.주의);
            return;
        }

        PartyraidChatManager.Instance.Chat_CheckStartRaid();
    }

    public void ShowRaidReadyPanel()
    {
        PartyradPanel.Show(false);
        for (int i = 0; i < readyslots.Length; i++)
        {
            readyslots[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < PartyMember.Length; i++)
        {
            if (PartyMember[i].data != null)
            {
                readyslots[i].InitData(PartyMember[i].data.nickname);
                readyslots[i].gameObject.SetActive(true);
            }
        }

        readymaptext.text = string.Format(Inventory.GetTranslate("UI7/초대내용"),
            Inventory.GetTranslate(MapDB.Instance.Find_id(partyroomdata.nowmap).name), partyroomdata.level);

        RaidReadyPanel.SetActive(true);
        if (nowmyleadernickname == PlayerBackendData.Instance.nickname)
        {
            Invoke(nameof(RaidStartCheck), 11f);
        }
        else
        {
            PartyraidChatManager.Instance.TrggetNoready();
        }
    }

    //준비
    public void Bt_YesReady()
    {
        //내 자리가 레디라고 보냄.
        PartyraidChatManager.Instance.Chat_YesReady();
    }

    //준비취소
    public void Bt_NoReady()
    {
        //전체 유저를 취소 시킴. 탈퇴하지는 않음.
        PartyraidChatManager.Instance.Chat_NoReady();
    }

    public void SetReadyPanel(int num)
    {
        readyslots[num].SetReady();
    }

    //타이머가 종료되면 레디에 따라 달라진다.
    void RaidStartCheck()
    {
        int num = 0;
        bool[] isnoready = new bool[4];
        for (int i = 0; i < PartyMember.Length; i++)
        {
            if (PartyMember[i].data != null)
            {
                num++;
                isnoready[i] = true;
            }
        }

        int readynum = 0;
        for (int i = 0; i < readyslots.Length; i++)
        {
            if (readyslots[i].isready)
                readynum++;
        }

        Debug.Log("레디는" + readynum);
        Debug.Log("인원은" + num);
        if (readynum == num)
        {
            Debug.Log("모두가 레디했다");
            RaidReadyPanel.SetActive(false);

            //레이드를 시작
            PartyraidChatManager.Instance.Chat_RaidRealStart();
        }
        else
        {
            for (int i = 0; i < readyslots.Length; i++)
            {
                if (PartyMember[i].data != null)
                {
                    if (!readyslots[i].isready)
                    {
                        //해당유저 탈퇴
                        Debug.Log("유저를 삭젷나다.");
                        RaidReadyPanel.SetActive(false);
                        alertmanager.Instance.ShowAlert(
                            string.Format(Inventory.GetTranslate("UI7/준비안해서 탈퇴"), PartyMember[i].data.nickname),
                            alertmanager.alertenum.일반);
                        PartyMember[i].ExitPlayer();
                    }
                }
            }

            RaidReadyPanel.SetActive(false);
        }
    }




    //방 나가기



    private void Start()
    {
        Bt_MakeRoom();
    }

    public GameObject ExitButton;

    //방 나가기
    public void Bt_ExitRoom()
    {
        //리더라면 유저를 모두 나가게함.
        if (nowmyleadernickname == PlayerBackendData.Instance.nickname)
        {
            PartyraidChatManager.Instance.Chat_ExitLeader();
        }
        else
        {
            //리더가 아니면 나혼자 나감.
            PartyraidChatManager.Instance.Chat_ExitMember();
        }

        Bt_MakeRoom();
    }

    public void Bt_MakeRoom()
    {
//        Debug.Log("파티를 만든다");

        foreach (var t in chatmanager.Instance.partyraidchatslot)
        {
            t.gameObject.SetActive(false);
            chatmanager.Instance.party_num = 0;
        }

        foreach (var t in chatmanager.Instance.partyraidSystemchatslot)
        {
            t.gameObject.SetActive(false);
            chatmanager.Instance.partySystem_num = 0;
        }

        for (int i = 0; i < PartyMember.Length; i++)
        {
            PartyMember[i].ExitPlayer();
        }

        StartButton.SetActive(true);
        ChangemapButton.SetActive(false);
        JoinUserButton.SetActive(false);
        AdmentiseButton.SetActive(false);

        
        
        PartyMember[0].SetPlayerData(GiveMyPartyData(), 0);
        PartyMember[0].BuffPercent = Battlemanager.Instance.mainplayer.Stat_totalbuff;
        Debug.Log("버프는" + "ㅇㅇ");
        nowmyleadernickname = PlayerBackendData.Instance.nickname;
        mypartynum = 0;
        partyroomdata = new PartyRoom();
        DropId = PartyRaidDB.Instance.Find_id(partyroomdata.nowmap).dropid.Split(';');
        RefreshCount();
        ClearAllJoinUser();
       ExitButtons.SetActive(true);
    }


    public int MinAdLv = 20;
    
    //플레이어 이미지 정보를 줌.
    public string GiveMyPartyData()
    {
        PlayerBackendData a = PlayerBackendData.Instance;
        string s = $"{a.nickname};{a.playerindate};{a.GetLv()};{a.GetPlayerAvatadataPartyRaid()};{false};";
        return s;
    }

    public string GivePartyData(int num)
    {

        //이름;인데이트;레벨;아바타정보;준비정보
        if (PartyRaidRoommanager.Instance.PartyMember[num].data != null)
        {
            return PartyRaidRoommanager.Instance.PartyMember[num].data.GiveData();
        }

        return "";
    }


    public void RefreshPartyData()
    {
        if (!partyroomdata.isstart)
        {
            PartyNotStartObj.SetActive(true);
            PartyStartObj.SetActive(false);
        }
        else
        {
            PartyNotStartObj.SetActive(false);
        }

        if (nowmyleadernickname == PlayerBackendData.Instance.nickname)
        {
            LevelCount.gameObject.SetActive(true);
            StartButton.SetActive(true);
            JoinUserButton.SetActive(true);
            AdmentiseButton.SetActive(true);
        }
        else
        {
            LevelCount.gameObject.SetActive(false);
            StartButton.SetActive(false);
            ChangemapButton.SetActive(false);
            JoinUserButton.SetActive(false);
            AdmentiseButton.SetActive(false);
        }

        if (partyroomdata.usercount == 1)
        {
            //ExitButton.SetActive(false);
            ChatInput.SetActive(false);
        }
        else
        {
            //ExitButton.SetActive(true);
            ChatInput.SetActive(true);
        }

        RefreshMapData();
    }

    public GameObject[] BuffsObj;

    public void RefreshCount()
    {
        PartyRaidRoommanager.Instance.partyroomdata.level = PartyRaidRoommanager.Instance.LevelCount.nowcount;

        int[] playerbuff = new int[(int)PartyRaidBuffEnemy.Length];
        int[] enemybuff = new int[(int)PartyRaidBuffEnemy.Length];
        switch (PartyRaidRoommanager.Instance.partyroomdata.level)
        {
            case 1:
                enemybuff[0] = 0;
                enemybuff[1] = 0;
                enemybuff[2] = 0;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 1;
                break;
            case 2:
                enemybuff[0] = 0;
                enemybuff[1] = 0;
                enemybuff[2] = 0;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 1;
                break;
            case 3:
                enemybuff[0] = 1;
                enemybuff[1] = 0;
                enemybuff[2] = 0;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 1;
                break;
            case 4:
                enemybuff[0] = 1;
                enemybuff[1] = 1;
                enemybuff[2] = 0;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 1;
                break;
            case 5:
                enemybuff[0] = 1;
                enemybuff[1] = 1;
                enemybuff[2] = 1;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 1;
                break;
            case 6:
                enemybuff[0] = 1;
                enemybuff[1] = 1;
                enemybuff[2] = 1;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 1;
                break;
            case 7:
                enemybuff[0] = 1;
                enemybuff[1] = 1;
                enemybuff[2] = 1;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 1;
                break;
        }

        for (int i = 0; i < BuffsObj.Length; i++)
        {
            BuffsObj[i].SetActive(false);
        }

        for (int i = 0; i < enemybuff.Length; i++)
        {
            if (enemybuff[i] != 0)
            {
                BuffsObj[i].SetActive(true);
            }
        }

        BuffInfo(enemybuff);
        PartyRaidRoommanager.Instance.RefreshPartyData();
    }

    public void Bt_mapChange()
    {
        PartyraidChatManager.Instance.Chat_ChangeMap();
        PartyRaidRoommanager.Instance.RefreshPartyData();

        alertmanager.Instance.ShowAlert(
            Inventory.GetTranslate("UI7/난이도변경"), alertmanager.alertenum.일반);
    }

    public Image[] Background;
    public Image[] MonImage;
    public Text[] MapName;
    public Text[] MapLevel;
    public itemslot[] itemdrops;
    public Text BossHp;

    public float[] percentlevel; 
    
    
    public decimal GetMaxHp()
    {
        decimal a =
            (decimal.Parse(monsterDB.Instance.Find_id(MapDB.Instance.Find_id(partyroomdata.nowmap).monsterid).hp) *
             (decimal)math.pow(percentlevel[partyroomdata.level-1], partyroomdata.level-1));
        return a;
    }

    void RefreshMapData()
    {
        //보스 생명력
        decimal a = GetMaxHp();
        BossHp.text = $"HP: {dpsmanager.convertNumber(a)}";

        foreach (var VARIABLE in Background)
        {
            VARIABLE.sprite = SpriteManager.Instance.GetSprite(MapDB.Instance.Find_id(partyroomdata.nowmap).maplayer0);
        }

        foreach (var VARIABLE in MonImage)
        {
            VARIABLE.sprite = SpriteManager.Instance.GetSprite(monsterDB.Instance
                .Find_id(MapDB.Instance.Find_id(partyroomdata.nowmap).monsterid).sprite);
        }

        foreach (var VARIABLE in MapName)
        {
            VARIABLE.text = Inventory.GetTranslate(MapDB.Instance.Find_id(partyroomdata.nowmap).name);
        }

        foreach (var VARIABLE in MapLevel)
        {
            VARIABLE.text = string.Format(Inventory.GetTranslate("UI7/난이도"), partyroomdata.level);
        }

        for (int i = 0; i < itemdrops.Length; i++)
        {
            itemdrops[i].gameObject.SetActive(false);
        }

        MapDB.Row mapdata = MapDB.Instance.Find_id(partyroomdata.nowmap);

        string dropid = DropId[partyroomdata.level - 1];
        //일반몹 드롭테이블
        List<MonDropDB.Row> dropdatas_basic = MonDropDB.Instance.FindAll_id(dropid);
//          Debug.Log(dropdatas_basic.Count);
        for (int i = 0; i < dropdatas_basic.Count; i++)
        {
            itemdrops[i].SetItem(dropdatas_basic[i].itemid, 0);
            itemdrops[i].gameObject.SetActive(true);
        }
    }

    public UIView BuffInfoPanel;
    public GameObject[] BuffPanel;

    public void BuffInfo(int[] buffnum)
    {
        foreach (var VARIABLE in BuffPanel)
        {
            VARIABLE.SetActive(false);
        }

        for (int i = 0; i < buffnum.Length; i++)
        {
            if (buffnum[i] != 0)
            {
                BuffPanel[i].SetActive(true);
            }
        }
    }

    public void Bt_ShowBuff()
    {
        BuffInfoPanel.Show(false);
    }

    
    public UIView Adpanel;
    public InputField AdInputText;
    public InputField AdRankInputText;
    public string adstring; //홍보 번호

    public void Bt_ShowAdTisePanel()
    {
        Adpanel.Show(false);
    }

    public void Bt_Adtise()
    {
        int ran = Random.Range(1000, 50000);

        adstring = $"{PlayerBackendData.Instance.nickname}{ran}";

        if (AdRankInputText.text == "")
        {
            //랭크입력
            return;
        }
        if (int.Parse(AdRankInputText.text) < PartyRaidRoommanager.Instance.MinAdLv)
        {
            //랭크최소제한15
            return;
        }
        Adpanel.Hide(false);
        PartyraidChatManager.Instance.Chat_Admentise(adstring,AdRankInputText.text);
    }

    public string joinname;
    public int rankjoin;
    public GameObject JoinPanel;
    public void ShowAdmenPanel()
    {
        JoinPanel.SetActive(true);
    }
    
    //파티 가입신청넣기
    public void Bt_JoinParty()
    {
        if (rankjoin > PlayerBackendData.Instance.GetAdLv())
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/0_가입제한됨"),alertmanager.alertenum.주의);
            chatmanager.Instance.Panel.Hide(true);
            return;
        }
        
        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/0_파티가입신청함"),alertmanager.alertenum.일반);
        PartyraidChatManager.Instance.Chat_SendJoin();
        chatmanager.Instance.Panel.Hide(true);
    }


    public GameObject PartyRaidMemberJoinPanel;
    public GameObject PartyRaidMemberCountObj;
    public GameObject PartyRaidMemberCountObj2;
    public Text PartyRaidMemberCountText;

    public partyraidjoinmemberslot[] Joinmemberslots;

    public List<string> joinusername = new List<string>();
    //파티가입신청자확인

    public void Bt_ShowJoinMemberPanel()
    {
        RefreshJoinmemberCount();
        PartyRaidMemberJoinPanel.SetActive(true);
    }
    
    public void AddJoinUser(string nick, string[] Avatadata,int lv,int rank)
    {
        if(joinusername.Contains(nick))
            return;
        foreach (var t in Joinmemberslots)
        {
            if (!t.gameObject.activeSelf)
            {
                //닫혀있으니 빈것이다 넣어도 된다.
                t.SetData(nick,Avatadata,lv,rank);
                joinusername.Add(nick);
                t.gameObject.SetActive(true);
                break;
            }
        }

        RefreshJoinmemberCount();
    }

    public void RefreshJoinmemberCount()
    {
        int JoinMemberCount = 0;
        for (int i = 0; i < Joinmemberslots.Length; i++)
        {
            if (Joinmemberslots[i].gameObject.activeSelf)
                JoinMemberCount++;
        }

        if (JoinMemberCount == 0)
        {
            PartyRaidMemberCountObj.SetActive(false);
            PartyRaidMemberCountObj2.SetActive(false);
        }
        else
        {
            PartyRaidMemberCountObj.SetActive(true);
            PartyRaidMemberCountObj2.SetActive(true);
            PartyRaidMemberCountText.text = JoinMemberCount.ToString();
        }

    }
    public void ClearAllJoinUser()
    {
        for (int i = 0; i < Joinmemberslots.Length; i++)
        {
            Joinmemberslots[i].gameObject.SetActive(false);
        }
        joinusername.Clear();
        RefreshJoinmemberCount();
    }

    public GameObject RecentUserInvitePanel;
    public Text RecentUserText;
    public void ShowRecentPanel()
    {
        if (PartyraidChatManager.Instance.recentusers != "")
        {
            string[] username = PartyraidChatManager.Instance.recentusers.Split(';');
            RecentUserInvitePanel.SetActive(true);
            RecentUserText.text = "";
            for (int i = 0; i < username.Length; i++)
            {
                if (username[i] != "")
                    RecentUserText.text += $"{username[i]}\n";
            }
        }
    }



}

public class PartyRoom
{
    public string nowmap;
    public int level;
    public string leadername;
    public int usercount;
    public bool isstart;
    public PartyRoom()
    {
        nowmap = "20000";
        level = 1;
        //leadername = PlayerBackendData.Instance.nickname;
        usercount = 1;
        isstart = false;
    }

    

    public PartyRoom(string nowmap, int level, string leadername, int usercount,bool isstarts)
    {
        this.nowmap = nowmap;
        this.level = level;
        this.leadername = leadername;
        this.usercount = usercount;
        this.isstart = isstarts;
    }
}

public class PartyRaidBoss
{
    public string monid;
    public decimal curhp;
    public decimal maxhp;
    public List<decimal> DmgPlayer;
    public List<string> DmgPlayerNickname;
    
    public int[] MonsterBuff;
    public int[] playerBuff;

    public int BossAttackcount ;
    public int MiddleBossAttackcount ;
    public PartyRaidBoss(string monid, decimal maxhp,int[] debuffcount,int[] playerbuffs)
    {
        this.monid = monid;
        this.curhp = maxhp;
        this.maxhp = maxhp;

        MonsterBuff = debuffcount;
        this.playerBuff = playerbuffs;
        BossAttackcount = 1;
        MiddleBossAttackcount = 1;

        DmgPlayer = new List<decimal>();
        DmgPlayerNickname = new List<string>();
    }
}

public enum PartyRaidBuffEnemy
{
    강인함,//받는 피해 감소(무력화 시 삭제).
    민첩함,//치명타 방어 30% (치명타 확률 % 만큼 깎음).
    발화,//일정 시간 마다 생명력 회복 (일정 스테이지 클리어 시 삭제.
    해,//물리,마법 스킬 피해량 감소 (해의 토템 파괴 시 삭제).
    달,//상태 이상 피해량 감소 (달의 토템 파괴 시 삭제).
    불멸,//무력화가 잠금일 때 피해를 입지 않음. (삭제 불가).
    광폭, // 일정 시간 마다 몬스터 피해량 상승.
    Length
}

enum PartyRaidBuffPlayer
{
    공격,
    방어,
    Length
}