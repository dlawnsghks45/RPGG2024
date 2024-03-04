using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Doozy.Engine.UI;
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

    public int SelectPartyUsernum;
    //파티룸 데이터
    public PartyRoom partyroomdata = new PartyRoom();
    //내 파티 데이터
    public int mypartynum; //내 파티 자리
    public bool isready;
    public countpanel LevelCount;
    
    
    
    //버튼
    public GameObject StartButton;
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
        Invoke(nameof(RaidStartCheck),11f);
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
        }
        else
        {
            for (int i = 0; i < isnoready.Length; i++)
            {
                if (PartyMember[i].data != null)
                {
                    if (!isnoready[i])
                    {
                        //해당유저 탈퇴
                        PartyMember[i].ExitPlayer();
                        Debug.Log("유저를 삭젷나다.");
                        if (PartyMember[i].data.nickname == PlayerBackendData.Instance.nickname)
                        {
                            //내아이디가 레디를 안했다면.
                            PartyRaidRoommanager.Instance.Bt_ExitRoom();
                        }
                        RaidReadyPanel.SetActive(false);
                        alertmanager.Instance.ShowAlert(string.Format(Inventory.GetTranslate("UI7/준비안해서 탈퇴"),PartyMember[i].data.nickname),alertmanager.alertenum.일반);
                    }
                }
            }
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
        if (nowmyleadernickname == PlayerBackendData.Instance.nickname)
        {
            LevelCount.gameObject.SetActive(true);
            StartButton.SetActive(true);
        }
        else
        {
            LevelCount.gameObject.SetActive(true);
        }
        if (partyroomdata.usercount == 1)
        {
            ExitButton.SetActive(false);
        }
        else
        {
            ExitButton.SetActive(true);
        }
    }
}

public class PartyRoom
{
    public string nowmap;
    public int level;
    public string leadername;
    public int usercount;
    public PartyRoom()
    {
        nowmap = "1000";
        level = 1;
        leadername = PlayerBackendData.Instance.nickname;
        usercount = 1;
    }

    

    public PartyRoom(string nowmap, int level, string leadername, int usercount)
    {
        this.nowmap = nowmap;
        this.level = level;
        this.leadername = leadername;
        this.usercount = usercount;
    }
}

class PartyRaidBoss
{
    string monid;
    private decimal curhp;
    private decimal maxhp;
    public bool[] MonsterBuff= new bool[(int)PartyRaidBuffEnum.Length];
    public int[] MonsterBuffStack = new int[(int)PartyRaidBuffEnum.Length];
}

enum PartyRaidBuffEnum
{
    광폭, // 일정 시간 마다 몬스터 피해량 상승.
    강인함,//받는 피해 감소(무력화 시 삭제).
    민첩함,//치명타 방어 30% (치명타 확률 % 만큼 깎음).
    발화,//일정 시간 마다 생명력 회복 (일정 스테이지 클리어 시 삭제.
    해,//물리,마법 스킬 피해량 감소 (해의 토템 파괴 시 삭제).
    달,//상태 이상 피해량 감소 (달의 토템 파괴 시 삭제).
    불멸,//무력화가 잠금일 때 피해를 입지 않음. (삭제 불가).
    Length
}