using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using BackEnd.Tcp;
using UnityEngine;

public class PartyraidChatManager : MonoBehaviour
{
    //�̱��游���.
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
    #region ��Ƽ�ʴ�����

    //�������� �ʴ� ����.
    public void Chat_invitePlayer(string invited)
    {
        //system;PI;���̸�;�����̸�;mapname;level
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Debug.Log("�ʴ븦 ���´�");
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PI;{PlayerBackendData.Instance.nickname};{invited};{PartyRaidRoommanager.Instance.partyroomdata.nowmap};{PartyRaidRoommanager.Instance.partyroomdata.level}");
        }
    }

    //�ʴ븦 �����ؼ� ���忡�� �������θ� ����.
    public void Chat_AcceptInvite()
    {
        //�����Ѵٴ°� ������.
        //system;PA;���̸�;��������
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Debug.Log("�ʴ븦 �����ߴ�");
            PartyRaidRoommanager.Instance.nowmyleadernickname = PartyRaidRoommanager.Instance.invitednickname;
            Debug.Log("������" + PartyRaidRoommanager.Instance.nowmyleadernickname);
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PA;{PartyRaidRoommanager.Instance.invitednickname};{PlayerBackendData.Instance.nickname}&{PartyRaidRoommanager.Instance.GiveMyPartyData()}");
        }
    }
    
    //�ʴ븦 �����ؼ� ���忡�� �������θ� ����.
    public void Chat_GivePartyData(string partymembernick)
    {
        //�����Ѵٴ°� ������.
        //system;PGD;�����̸�;�ش�����;�ʾ��̵�;�ʳ��̵�&����1&����2&����3&����4
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Debug.Log("�����ؼ� ���� ��Ƽ �����͸� �������� ������");
            Debug.Log("��� �г�����" + partymembernick);
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PGD;{PlayerBackendData.Instance.nickname};{partymembernick};{PartyRaidRoommanager.Instance.partyroomdata.nowmap};{PartyRaidRoommanager.Instance.partyroomdata.level}&" +
                $"{PartyRaidRoommanager.Instance.GivePartyData(0)}" +
                $"#{PartyRaidRoommanager.Instance.GivePartyData(1)}" +
                $"#{PartyRaidRoommanager.Instance.GivePartyData(2)}" +
                $"#{PartyRaidRoommanager.Instance.GivePartyData(3)}");
        }
    }
    
    #endregion

    #region ��Ƽ������
    //��Ƽ���� �����⸦ ������
    public void Chat_ExitLeader()
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Debug.Log("��Ƽ���� ������ �ٸ��������� ������.");
            //PRB = party room break
            //system;PRB;��Ƽ���̸�

            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRB;{PartyRaidRoommanager.Instance.nowmyleadernickname}");
        }
    }
    //��Ƽ���� �����⸦ ������
    public void Chat_ExitMember()
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Debug.Log("��Ƽ�� ������ �ٸ��������� ���� ������ ������."); 
            //PRO = party room out
            //system;PRO;��Ƽ���̸�;���̸�;����Ƽ�ڸ�

            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRO;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidRoommanager.Instance.mypartynum}");
        }
    }
    
    public void Chat_WithdrawMember(string nickname)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Debug.Log("��Ƽ�� ������ �ٸ��������� ���� ������ ������."); 
            //PRO = party room out
            //system;PRO;��Ƽ���̸�;���̸�;����Ƽ�ڸ�

            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PWD;{PartyRaidRoommanager.Instance.nowmyleadernickname};{nickname};{PartyRaidRoommanager.Instance.mypartynum}");
        }
    }
    #endregion


    #region ���̵����
    
    public void Chat_CheckStartRaid()
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Debug.Log("���̵������ �˸���."); 
            //system;PRC;��Ƽ���̸�;
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
            Debug.Log("�غ�"); 
            //system;PRYR;��Ƽ���̸�;���̸�;���ڸ�
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRYR;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidRoommanager.Instance.mypartynum}");
        }
    }
    public void Chat_NoReady()
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Debug.Log("�غ����."); 
            PartyRaidRoommanager.Instance.RaidReadyPanel.SetActive(false);
            //system;PRNR;��Ƽ���̸�;���̸�;���ڸ�
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRNR;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidRoommanager.Instance.mypartynum}");
        }
    }
    #endregion
    
}

