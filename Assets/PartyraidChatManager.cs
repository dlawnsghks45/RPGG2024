using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using BackEnd.Tcp;
using UnityEngine;

public class PartyraidChatManager : MonoBehaviour
{
    //싱글톤만들기.
    private static PartyraidChatManager _instance = null;

    public static PartyraidChatManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(PartyraidChatManager)) as PartyraidChatManager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    const string publicsystem = "#publicsys#";

    public void Chat_Admentise(string adstring,string rank)
    {
        //자기 자신은 안됨.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            int membercount = 0;
            foreach (var VARIABLE in PartyRaidRoommanager.Instance.PartyMember)
            {
                if (VARIABLE.data != null)
                    membercount++;
            }
            //system;PRAD;내이름;레벨;랭크레벨;adstring;mapid;level;Rank;membercount;내용
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRAD;{PlayerBackendData.Instance.nickname};{PlayerBackendData.Instance.GetLv()};{RankingManager.Instance.getmybprank()};{adstring};{PartyRaidRoommanager.Instance.partyroomdata.nowmap};{PartyRaidRoommanager.Instance.partyroomdata.level};{rank};{membercount};{PartyRaidRoommanager.Instance.AdInputText.text}");
        }
    }
    
    //가입신청을 보냄
    public void Chat_SendJoin()
    {
        if (PlayerBackendData.Instance.nickname != PartyRaidRoommanager.Instance.nowmyleadernickname)
        {
            //이미 가입된 유저
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/이미가입함"),alertmanager.alertenum.일반);
            return;
        }
        //자기 자신은 안됨.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //system;PRSJ;내이름;레벨;랭크레벨;위장;무기;갑옷
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRSJ;{PlayerBackendData.Instance.nickname};{PlayerBackendData.Instance.GetLv()};{PlayerBackendData.Instance.GetAdLv()};{chatmanager.Instance.GetUserVisualData()}");
        }
    }
    //가입신청자 해당 유저를 받았다.
    public void Chat_SendJoinAccept(string nickname)
    {
        //자기 자신은 안됨.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //system;PRSJ;내이름;레벨;랭크레벨;위장;무기;갑옷
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRSJA;{PlayerBackendData.Instance.nickname};{nickname}");
        }
    }
    
    #region 파티초대전용

    //유저에게 초대 보냄.
    public void Chat_invitePlayer(string invited)
    {
        //자기 자신은 안됨.

        if (invited == PlayerBackendData.Instance.nickname)
            return;
        //system;PI;내이름;유저이름;mapname;level
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Debug.Log("초대를 보냈다");
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PI;{PlayerBackendData.Instance.nickname};{invited};{PartyRaidRoommanager.Instance.partyroomdata.nowmap};{PartyRaidRoommanager.Instance.partyroomdata.level}");
        }
    }

    //초대를 수락해서 파장에게 수락여부를 보냄.
    public void Chat_AcceptInvite()
    {
        //수락한다는걸 보낸다.
        //system;PA;내이름;내데이터
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            PartyRaidRoommanager.Instance.nowmyleadernickname = PartyRaidRoommanager.Instance.invitednickname;
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PA;{PartyRaidRoommanager.Instance.invitednickname};{PlayerBackendData.Instance.nickname}&{PartyRaidRoommanager.Instance.GiveMyPartyData()}");
        }
    }

    public void Chat_AreadyHaveParty(string nickname)
    {
        //수락한다는걸 보낸다.
        //system;PAH;파장이름
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PAH;{nickname};{PlayerBackendData.Instance.nickname}");
        }
    }

    //초대를 수락해서 파장에게 수락여부를 보냄.
    public void Chat_GivePartyData(string partymembernick)
    {
        //수락한다는걸 보낸다.
        //system;PGD;파장이름;해당유저;맵아이디;맵난이도&유저1&유저2&유저3&유저4
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Debug.Log("수락해서 현재 파티 데이터를 유저에게 보낸다");
            Debug.Log("멤버 닉네임은" + partymembernick);
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PGD;{PlayerBackendData.Instance.nickname};{partymembernick};{PartyRaidRoommanager.Instance.partyroomdata.nowmap};{PartyRaidRoommanager.Instance.partyroomdata.level}&" +
                $"{PartyRaidRoommanager.Instance.GivePartyData(0)}" +
                $"#{PartyRaidRoommanager.Instance.GivePartyData(1)}" +
                $"#{PartyRaidRoommanager.Instance.GivePartyData(2)}" +
                $"#{PartyRaidRoommanager.Instance.GivePartyData(3)}");
        }
    }

    #endregion

    #region 맵변경

    public void Chat_ChangeMap()
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //system;PRNR;파티장이름;맵아이디;난이도
            //PartyRaidRoommanager.Instance.partyroomdata.nowmap = 선택한맵

            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRCM;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PartyRaidRoommanager.Instance.partyroomdata.nowmap};{PartyRaidRoommanager.Instance.partyroomdata.level}");

        }
    }



    #endregion


    #region 파티나가기

    //파티장이 나가기를 누르면
    public void Chat_ExitLeader()
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Debug.Log("파티장이 나가서 다른유저에게 보낸다.");
            //PRB = party room break
            //system;PRB;파티장이름

            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRB;{PartyRaidRoommanager.Instance.nowmyleadernickname}");
        }
    }
    public void Chat_ExitLeader2()
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //PRB = party room break
            //system;PRB;파티장이름

            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRBF;{PartyRaidRoommanager.Instance.nowmyleadernickname}");
        }
    }
    //파티원이 나가기를 누르면
    public void Chat_ExitMember()
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Debug.Log("파티원 나가서 다른유저에게 내가 나간걸 보낸다.");
            //PRO = party room out
            //system;PRO;파티장이름;내이름;내파티자리

            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRO;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidRoommanager.Instance.mypartynum}");
        }
    }

    public void Chat_WithdrawMember(string nickname)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Debug.Log("파티원 나가서 다른유저에게 내가 나간걸 보낸다.");
            //PRO = party room out
            //system;PRO;파티장이름;내이름;내파티자리

            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PWD;{PartyRaidRoommanager.Instance.nowmyleadernickname};{nickname};{PartyRaidRoommanager.Instance.mypartynum}");
        }
    }

    #endregion


    #region 레이드시작

    public void Chat_CheckStartRaid()
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Debug.Log("레이드시작을 알린다.");
            //system;PRC;파티장이름;
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRC;{PartyRaidRoommanager.Instance.nowmyleadernickname}");
        }
    }

    public void Chat_YesReady()
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            PartyRaidRoommanager.Instance.readyyesnopanel.SetActive(false);
            Debug.Log("준비");
            //system;PRYR;파티장이름;내이름;내자리
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRYR;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidRoommanager.Instance.mypartynum}");
        }
    }

    public void Chat_NoReady()
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Debug.Log("준비취소.");
            PartyRaidRoommanager.Instance.RaidReadyPanel.SetActive(false);
            //system;PRNR;파티장이름;내이름;내자리
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRNR;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidRoommanager.Instance.mypartynum}");
        }
    }

    public void TrggetNoready()
    {
        Invoke(nameof(NoReady), 12);
    }

    //11초 뒤에 내가 레디를 안했다면 강퇴
    public void NoReady()
    {
        //창이 나와있는데 레디도안했다면
        if (!PartyRaidRoommanager.Instance.readyslots[PartyRaidRoommanager.Instance.mypartynum].isready &&
            PartyRaidRoommanager.Instance.RaidReadyPanel.activeSelf &&
            PartyRaidRoommanager.Instance.RaidReadyPanel.activeSelf)
        {
            PartyRaidRoommanager.Instance.Bt_ExitRoom();
            PartyRaidRoommanager.Instance.RaidReadyPanel.SetActive(false);
        }
    }

    #endregion


    /////////레이드진행////////

    #region 레이드 진행

    public void Chat_ChatSystemOnlyContent(string content)
    {
        //모두가 레디해서 레이드가 시작헀다.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            PartyRaidRoommanager.Instance.RaidReadyPanel.SetActive(false);
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PSMOC;{PlayerBackendData.Instance.nickname};{content}");
        }
    }

    public void Chat_RaidRealStart()
    {
        //모두가 레디해서 레이드가 시작헀다.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            PartyRaidRoommanager.Instance.RaidReadyPanel.SetActive(false);
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRRS;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidRoommanager.Instance.mypartynum}");
           // Chat_ChatSystemOnlyContent("파티 레이드가 시작됩니다.");
        }
    }


    public void Chat_MiddleRaidStart(int num)
    {
        //레디를 했다.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //시스;PRMS;리더;자리/맵이름
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRMS;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{num};{PartyRaidRoommanager.Instance.mypartynum}");
        }
    }

    
    //중간보스 죽임.
    public void Chat_MiddleRaidClear()
    {
        //레디를 했다.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //시스;PRKMB;리더;자리/사람자리
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRKMB;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidBattlemanager.Instance.nowmiddlenum};{PartyRaidRoommanager.Instance.mypartynum}");
        }
    }
    
    public void Chat_MiddleRaidFail()
    {
        //레디를 했다.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //시스;PRKMB;리더;자리/사람자리
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRKMF;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidBattlemanager.Instance.nowmiddlenum};{PartyRaidRoommanager.Instance.mypartynum}");
        }
    }

    public void Chat_MainBossRaidStart()
    {
          //레디를 했다.
                bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
                if (isConnect)
                {
                    //시스;PRMBS;리더;시작이름;내파티자리
                    Backend.Chat.ChatToChannel(ChannelType.Public,
                        $"{publicsystem};PRMBS;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidRoommanager.Instance.mypartynum}");
                }
    }
    public void Chat_MainBossRaidFinish(decimal dmg)
    {
        //레디를 했다.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //시스;PRMBS;리더;시작이름;내파티자리;피해
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRMBF;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidRoommanager.Instance.mypartynum};{dmg}");
        }
    }
    public void Chat_GiveReward(string dropid)
    {
        //레디를 했다.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //시스;PRMBS;리더;시작이름;내파티자리;드랍아이디&유저들;;;
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRGR;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{dropid}&{GetPartyName()}");
        }
    }
    
    string GetPartyName()
    {
        string a1 = "";
        string a2 = "";
        string a3 = "";
        string a4 = "";

        if (PartyRaidRoommanager.Instance.PartyMember[0].data != null)
        {
            a1 = PartyRaidRoommanager.Instance.PartyMember[0].data.nickname;
        }

        if (PartyRaidRoommanager.Instance.PartyMember[1].data != null)
        {
            a2 = PartyRaidRoommanager.Instance.PartyMember[1].data.nickname;
        }

        if (PartyRaidRoommanager.Instance.PartyMember[2].data != null)
        {
            a3 = PartyRaidRoommanager.Instance.PartyMember[2].data.nickname;
        }

        if (PartyRaidRoommanager.Instance.PartyMember[3].data != null)
        {
            a4 = PartyRaidRoommanager.Instance.PartyMember[3].data.nickname;
        }


        return $"{a1};{a2};{a3};{a4}";
    }

    #endregion


}
