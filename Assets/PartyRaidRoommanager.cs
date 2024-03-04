using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class PartyRaidRoommanager : MonoBehaviour
{
    //�̱��游���.
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
    //��Ƽ�� ������
    public PartyRoom partyroomdata = new PartyRoom();
    //�� ��Ƽ ������
    public int mypartynum; //�� ��Ƽ �ڸ�
    public bool isready;
    public countpanel LevelCount;
    
    
    
    //��ư
    public GameObject StartButton;
    //����
    public PartyMemberslot[] PartyMember;

    //�ʴ�
    public UIView InvitePanel;
    public InputField InviteNicknameinput;
    //�ʴ� ����
    public GameObject PartyInvitedPanel;
    public Text PartyInvitedMap;
    public Text PartyInvitedNickname;

    
    
    //���̵� ����Ȯ��
    public GameObject RaidReadyPanel;
    
    


    #region ��Ƽ�ʴ�

    //��Ƽ�ʴ�
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
        //system;PI;���̸�;�����̸�;mapname;level
        PartyInvitedMap.text = string.Format(Inventory.GetTranslate("UI7/�ʴ볻��"),
            Inventory.GetTranslate(MapDB.Instance.Find_id(mapid).name), level);
        readymaptext.text = PartyInvitedMap.text;
        PartyInvitedNickname.text = string.Format(Inventory.GetTranslate("UI7/������ ��Ƽ"),
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
    //���̵� ����
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

    //�غ�
    public void Bt_YesReady()
    {
        //�� �ڸ��� ������ ����.
        PartyraidChatManager.Instance.Chat_YesReady();
    }
    //�غ����
    public void Bt_NoReady()
    {
        //��ü ������ ��� ��Ŵ. Ż�������� ����.
        PartyraidChatManager.Instance.Chat_NoReady();
    }
    
    public void SetReadyPanel(int num)
    {
        readyslots[num].SetReady();
    }
    //Ÿ�̸Ӱ� ����Ǹ� ���� ���� �޶�����.
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
    
        Debug.Log("�����" + readynum);
        Debug.Log("�ο���" + num);
        if (readynum == num)
        {
            Debug.Log("��ΰ� �����ߴ�");
            RaidReadyPanel.SetActive(false);
            //���̵带 ����
        }
        else
        {
            for (int i = 0; i < isnoready.Length; i++)
            {
                if (PartyMember[i].data != null)
                {
                    if (!isnoready[i])
                    {
                        //�ش����� Ż��
                        PartyMember[i].ExitPlayer();
                        Debug.Log("������ �蠫����.");
                        if (PartyMember[i].data.nickname == PlayerBackendData.Instance.nickname)
                        {
                            //�����̵� ���� ���ߴٸ�.
                            PartyRaidRoommanager.Instance.Bt_ExitRoom();
                        }
                        RaidReadyPanel.SetActive(false);
                        alertmanager.Instance.ShowAlert(string.Format(Inventory.GetTranslate("UI7/�غ���ؼ� Ż��"),PartyMember[i].data.nickname),alertmanager.alertenum.�Ϲ�);
                    }
                }
            }
        }
    }
    
    
    
    
    //�� ������
  
    

    private void Start()
    {
        Bt_MakeRoom();
    }

    public GameObject ExitButton;
    //�� ������
    public void Bt_ExitRoom()
    {
        //������� ������ ��� ��������.
        if (nowmyleadernickname == PlayerBackendData.Instance.nickname)
        {
            PartyraidChatManager.Instance.Chat_ExitLeader();
        }
        else
        {
            //������ �ƴϸ� ��ȥ�� ����.
            PartyraidChatManager.Instance.Chat_ExitMember();
        }
        Bt_MakeRoom();

    }
    
    public void Bt_MakeRoom()
    {
        Debug.Log("��Ƽ�� �����");
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

    //�÷��̾� �̹��� ������ ��.
    public string GiveMyPartyData()
    {
        PlayerBackendData a = PlayerBackendData.Instance;
        string s = $"{a.nickname};{a.playerindate};{a.GetLv()};{a.GetPlayerAvatadataPartyRaid()};{false};";
        return s;
    }

    public string GivePartyData(int num)
    {
        
        //�̸�;�ε���Ʈ;����;�ƹ�Ÿ����;�غ�����
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
    ����, // ���� �ð� ���� ���� ���ط� ���.
    ������,//�޴� ���� ����(����ȭ �� ����).
    ��ø��,//ġ��Ÿ ��� 30% (ġ��Ÿ Ȯ�� % ��ŭ ����).
    ��ȭ,//���� �ð� ���� ����� ȸ�� (���� �������� Ŭ���� �� ����.
    ��,//����,���� ��ų ���ط� ���� (���� ���� �ı� �� ����).
    ��,//���� �̻� ���ط� ���� (���� ���� �ı� �� ����).
    �Ҹ�,//����ȭ�� ����� �� ���ظ� ���� ����. (���� �Ұ�).
    Length
}