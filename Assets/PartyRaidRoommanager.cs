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

    public GameObject PartyNotStartObj;
    public GameObject PartyStartObj;
    

    public int SelectPartyUsernum;
    //��Ƽ�� ������
    public PartyRoom partyroomdata = new PartyRoom();
    //�� ��Ƽ ������
    public int mypartynum; //�� ��Ƽ �ڸ�
    public bool isready;
    public countpanel LevelCount;
    
    
    
    //��ư
    public GameObject StartButton;
    public GameObject ChangemapButton;
    public GameObject ChatInput;
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
        if (nowmyleadernickname == PlayerBackendData.Instance.nickname)
        {
            Invoke(nameof(RaidStartCheck),11f);
        }
        else
        {
            PartyraidChatManager.Instance.TrggetNoready();
        }
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
                        //�ش����� Ż��
                        Debug.Log("������ �蠫����.");
                        RaidReadyPanel.SetActive(false);
                        alertmanager.Instance.ShowAlert(
                            string.Format(Inventory.GetTranslate("UI7/�غ���ؼ� Ż��"), PartyMember[i].data.nickname),
                            alertmanager.alertenum.�Ϲ�);
                        PartyMember[i].ExitPlayer();
                    }
                }
            }
            RaidReadyPanel.SetActive(false);
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
            Inventory.GetTranslate("UI7/���̵�����"),alertmanager.alertenum.�Ϲ�);
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
        //���� �����
        decimal a = GetMaxHp();
        Debug.Log("�������" + a);
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
            VARIABLE.text = string.Format(Inventory.GetTranslate("UI7/���̵�"), partyroomdata.level);
        }
        
        for (int i = 0; i < itemdrops.Length; i++)
        {
            itemdrops[i].gameObject.SetActive(false);
        }

        MapDB.Row mapdata = MapDB.Instance.Find_id(partyroomdata.nowmap);
        monsterDB.Row mondata = monsterDB.Instance.Find_id(mapdata.monsterid);

        string dropid = monsterDB.Instance.Find_id(mapdata.monsterid.Split(';')[0]).dropid;
        //�Ϲݸ� ������̺�
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
    ������,//�޴� ���� ����(����ȭ �� ����).
    ��ø��,//ġ��Ÿ ��� 30% (ġ��Ÿ Ȯ�� % ��ŭ ����).
    ��ȭ,//���� �ð� ���� ����� ȸ�� (���� �������� Ŭ���� �� ����.
    ��,//����,���� ��ų ���ط� ���� (���� ���� �ı� �� ����).
    ��,//���� �̻� ���ط� ���� (���� ���� �ı� �� ����).
    ����, // ���� �ð� ���� ���� ���ط� ���.
    �Ҹ�,//����ȭ�� ����� �� ���ظ� ���� ����. (���� �Ұ�).
    Length
}

enum PartyRaidBuffPlayer
{
    ����,
    ���,
    Length
}