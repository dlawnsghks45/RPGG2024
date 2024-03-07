using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Doozy.Engine.UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PartyRaidRoommanager : MonoBehaviour
{
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
    
    
    
    //버튼
    public GameObject StartButton;
    public GameObject ChangemapButton;
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

    
    
    //레이드 시작확인
    public GameObject RaidReadyPanel;
    
    


    #region 파티초대

    //파티초대
    public void ShowInvitePanel()
    {
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
    public void ShowInvitedPanel(string mapid,int level,string leadernick)
    {
        invitednickname = leadernick;
        //system;PI;내이름;유저이름;mapname;level
        PartyInvitedMap.text = string.Format(Inventory.GetTranslate("UI7/초대내용"),
            Inventory.GetTranslate(MapDB.Instance.Find_id(mapid).name), level);
        readymaptext.text = PartyInvitedMap.text;
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
        PartyraidChatManager.Instance.Chat_CheckStartRaid();
    }

    public void ShowRaidReadyPanel()
    {
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
        
        RaidReadyPanel.SetActive(true);
        if (nowmyleadernickname == PlayerBackendData.Instance.nickname)
        {
            Invoke(nameof(RaidStartCheck),11f);
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
        Debug.Log("파티를 만든다");

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
        PartyMember[0].SetPlayerData(GiveMyPartyData(),0);
        nowmyleadernickname = PlayerBackendData.Instance.nickname;
        mypartynum = 0;
        partyroomdata = new PartyRoom();
        RefreshPartyData();
    }

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
            ChangemapButton.SetActive(true);
        }
        else
        {
            LevelCount.gameObject.SetActive(false);
            StartButton.SetActive(false);
            ChangemapButton.SetActive(false);
        }
        if (partyroomdata.usercount == 1)
        {
            ExitButton.SetActive(false);
            ChatInput.SetActive(false);
        }
        else
        {
            ExitButton.SetActive(true);
            ChatInput.SetActive(true);
        }
        RefreshMapData();
    }

    public void Bt_mapChange()
    {
        PartyraidChatManager.Instance.Chat_ChangeMap();
        
        alertmanager.Instance.ShowAlert(
            Inventory.GetTranslate("UI7/난이도변경"),alertmanager.alertenum.일반);
    }

    public Image[] Background;
    public Image[] MonImage;
    public Text[] MapName;
    public Text[] MapLevel;
    public itemslot[] itemdrops;
    public Text BossHp;


    public decimal GetMaxHp()
    {
        decimal a = (decimal.Parse(monsterDB.Instance.Find_id(MapDB.Instance.Find_id(partyroomdata.nowmap).monsterid).hp) * (decimal)math.pow(1.6f, partyroomdata.level));
        return a;
    }
    void RefreshMapData()
    {
        //보스 생명력
        decimal a = GetMaxHp();
        Debug.Log("생명력은" + a);
        BossHp.text = $"HP: {dpsmanager.convertNumber(a)}" ;
        
        foreach (var VARIABLE in Background)
        {
            VARIABLE.sprite = SpriteManager.Instance.GetSprite(MapDB.Instance.Find_id(partyroomdata.nowmap).maplayer0);
        }
        foreach (var VARIABLE in MonImage)
        {
            VARIABLE.sprite = SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id(MapDB.Instance.Find_id(partyroomdata.nowmap).monsterid).sprite);
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
        monsterDB.Row mondata = monsterDB.Instance.Find_id(mapdata.monsterid);

        string dropid = monsterDB.Instance.Find_id(mapdata.monsterid.Split(';')[0]).dropid;
        //일반몹 드롭테이블
        List<MonDropDB.Row> dropdatas_basic = MonDropDB.Instance.FindAll_id(dropid);
//          Debug.Log(dropdatas_basic.Count);
        for (int i = 0; i < dropdatas_basic.Count; i++)
        {
            itemdrops[i].SetItem(dropdatas_basic[i].itemid, 0);
            itemdrops[i].gameObject.SetActive(true);
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
        leadername = PlayerBackendData.Instance.nickname;
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
    public int[] MonsterBuff;
    public int[] playerBuff;

    public int[] BattleCount = new int[7];

    public PartyRaidBoss(string monid, decimal maxhp,int[] debuffcount,int[] playerbuffs)
    {
        this.monid = monid;
        this.curhp = maxhp;
        this.maxhp = maxhp;

        MonsterBuff = debuffcount;
        this.playerBuff = playerbuffs;
        BattleCount  =new int[] {1,1,1,1,1,1,1}; 
    }
}

enum PartyRaidBuffEnemy
{
    강인함,//받는 피해 감소(무력화 시 삭제).
    민첩함,//치명타 방어 30% (치명타 확률 % 만큼 깎음).
    발화,//일정 시간 마다 생명력 회복 (일정 스테이지 클리어 시 삭제.
    해,//물리,마법 스킬 피해량 감소 (해의 토템 파괴 시 삭제).
    달,//상태 이상 피해량 감소 (달의 토템 파괴 시 삭제).
    광폭, // 일정 시간 마다 몬스터 피해량 상승.
    불멸,//무력화가 잠금일 때 피해를 입지 않음. (삭제 불가).
    Length
}

enum PartyRaidBuffPlayer
{
    공격,
    방어,
    Length
}