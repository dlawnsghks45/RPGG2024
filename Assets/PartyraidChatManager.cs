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
    #region 파티초대전용

    //유저에게 초대 보냄.
    public void Chat_invitePlayer(string invited)
    {
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
            Debug.Log("초대를 수락했다");
            PartyRaidRoommanager.Instance.nowmyleadernickname = PartyRaidRoommanager.Instance.invitednickname;
            Debug.Log("리더는" + PartyRaidRoommanager.Instance.nowmyleadernickname);
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PA;{PartyRaidRoommanager.Instance.invitednickname};{PlayerBackendData.Instance.nickname}&{PartyRaidRoommanager.Instance.GiveMyPartyData()}");
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
    #endregion
    
}

