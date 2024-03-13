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

    public void Chat_Admentise(string adstring,string rank)
    {
        //�ڱ� �ڽ��� �ȵ�.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            int membercount = 0;
            foreach (var VARIABLE in PartyRaidRoommanager.Instance.PartyMember)
            {
                if (VARIABLE.data != null)
                    membercount++;
            }
            //system;PRAD;���̸�;����;��ũ����;adstring;mapid;level;Rank;membercount;����
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRAD;{PlayerBackendData.Instance.nickname};{PlayerBackendData.Instance.GetLv()};{RankingManager.Instance.getmybprank()};{adstring};{PartyRaidRoommanager.Instance.partyroomdata.nowmap};{PartyRaidRoommanager.Instance.partyroomdata.level};{rank};{membercount};{PartyRaidRoommanager.Instance.AdInputText.text}");
        }
    }
    
    //���Խ�û�� ����
    public void Chat_SendJoin()
    {
        if (PlayerBackendData.Instance.nickname != PartyRaidRoommanager.Instance.nowmyleadernickname)
        {
            //�̹� ���Ե� ����
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/�̹̰�����"),alertmanager.alertenum.�Ϲ�);
            return;
        }
        //�ڱ� �ڽ��� �ȵ�.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //system;PRSJ;���̸�;����;��ũ����;����;����;����
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRSJ;{PlayerBackendData.Instance.nickname};{PlayerBackendData.Instance.GetLv()};{PlayerBackendData.Instance.GetAdLv()};{chatmanager.Instance.GetUserVisualData()}");
        }
    }
    //���Խ�û�� �ش� ������ �޾Ҵ�.
    public void Chat_SendJoinAccept(string nickname)
    {
        //�ڱ� �ڽ��� �ȵ�.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //system;PRSJ;���̸�;����;��ũ����;����;����;����
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRSJA;{PlayerBackendData.Instance.nickname};{nickname}");
        }
    }
    
    #region ��Ƽ�ʴ�����

    //�������� �ʴ� ����.
    public void Chat_invitePlayer(string invited)
    {
        //�ڱ� �ڽ��� �ȵ�.

        if (invited == PlayerBackendData.Instance.nickname)
            return;
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
            PartyRaidRoommanager.Instance.nowmyleadernickname = PartyRaidRoommanager.Instance.invitednickname;
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PA;{PartyRaidRoommanager.Instance.invitednickname};{PlayerBackendData.Instance.nickname}&{PartyRaidRoommanager.Instance.GiveMyPartyData()}");
        }
    }

    public void Chat_AreadyHaveParty(string nickname)
    {
        //�����Ѵٴ°� ������.
        //system;PAH;�����̸�
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PAH;{nickname};{PlayerBackendData.Instance.nickname}");
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

    #region �ʺ���

    public void Chat_ChangeMap()
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //system;PRNR;��Ƽ���̸�;�ʾ��̵�;���̵�
            //PartyRaidRoommanager.Instance.partyroomdata.nowmap = �����Ѹ�

            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRCM;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PartyRaidRoommanager.Instance.partyroomdata.nowmap};{PartyRaidRoommanager.Instance.partyroomdata.level}");

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
    public void Chat_ExitLeader2()
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //PRB = party room break
            //system;PRB;��Ƽ���̸�

            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRBF;{PartyRaidRoommanager.Instance.nowmyleadernickname}");
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

    public void TrggetNoready()
    {
        Invoke(nameof(NoReady), 12);
    }

    //11�� �ڿ� ���� ���� ���ߴٸ� ����
    public void NoReady()
    {
        //â�� �����ִµ� ���𵵾��ߴٸ�
        if (!PartyRaidRoommanager.Instance.readyslots[PartyRaidRoommanager.Instance.mypartynum].isready &&
            PartyRaidRoommanager.Instance.RaidReadyPanel.activeSelf &&
            PartyRaidRoommanager.Instance.RaidReadyPanel.activeSelf)
        {
            PartyRaidRoommanager.Instance.Bt_ExitRoom();
            PartyRaidRoommanager.Instance.RaidReadyPanel.SetActive(false);
        }
    }

    #endregion


    /////////���̵�����////////

    #region ���̵� ����

    public void Chat_ChatSystemOnlyContent(string content)
    {
        //��ΰ� �����ؼ� ���̵尡 ��������.
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
        //��ΰ� �����ؼ� ���̵尡 ��������.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            PartyRaidRoommanager.Instance.RaidReadyPanel.SetActive(false);
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRRS;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidRoommanager.Instance.mypartynum}");
           // Chat_ChatSystemOnlyContent("��Ƽ ���̵尡 ���۵˴ϴ�.");
        }
    }


    public void Chat_MiddleRaidStart(int num)
    {
        //���� �ߴ�.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //�ý�;PRMS;����;�ڸ�/���̸�
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRMS;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{num};{PartyRaidRoommanager.Instance.mypartynum}");
        }
    }

    
    //�߰����� ����.
    public void Chat_MiddleRaidClear()
    {
        //���� �ߴ�.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //�ý�;PRKMB;����;�ڸ�/����ڸ�
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRKMB;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidBattlemanager.Instance.nowmiddlenum};{PartyRaidRoommanager.Instance.mypartynum}");
        }
    }
    
    public void Chat_MiddleRaidFail()
    {
        //���� �ߴ�.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //�ý�;PRKMB;����;�ڸ�/����ڸ�
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRKMF;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidBattlemanager.Instance.nowmiddlenum};{PartyRaidRoommanager.Instance.mypartynum}");
        }
    }

    public void Chat_MainBossRaidStart()
    {
          //���� �ߴ�.
                bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
                if (isConnect)
                {
                    //�ý�;PRMBS;����;�����̸�;����Ƽ�ڸ�
                    Backend.Chat.ChatToChannel(ChannelType.Public,
                        $"{publicsystem};PRMBS;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidRoommanager.Instance.mypartynum}");
                }
    }
    public void Chat_MainBossRaidFinish(decimal dmg)
    {
        //���� �ߴ�.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //�ý�;PRMBS;����;�����̸�;����Ƽ�ڸ�;����
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PRMBF;{PartyRaidRoommanager.Instance.nowmyleadernickname};{PlayerBackendData.Instance.nickname};{PartyRaidRoommanager.Instance.mypartynum};{dmg}");
        }
    }
    public void Chat_GiveReward(string dropid)
    {
        //���� �ߴ�.
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //�ý�;PRMBS;����;�����̸�;����Ƽ�ڸ�;������̵�&������;;;
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
