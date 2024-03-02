using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyRaidRoommanager : MonoBehaviour
{
    public GameObject StartButton;
    public GameObject ReadyButton;
    public GameObject UnReadyButton;


    public PartyMemberslot[] PartyMember;

    
    
    
    //��Ƽ��
    public bool ispartyleader;
    
    
    //���̵� ����
    public void Bt_StartRaid()
    {
        int nowcount = 0;

        for (int i = 0; i < PartyMember.Length; i++)
        {
            if (PartyMember[i].data != null)
            {
                nowcount++;
            }
        }

        int readycount = 0;
        for (int i = 0; i < PartyMember.Length; i++)
        {
            if (PartyMember[i].isready)
                readycount++;
        }

        if (readycount + 1 == nowcount)
        {
            Debug.Log("��� �غ� �Ϸ�");
        }
        else
        {
            Debug.Log("���� ���� ����");
        }
    }

    
    //�游��
    public void Bt_MakeRoom()
    {
        ispartyleader = true;
        for (int i = 0; i < PartyMember.Length; i++)
        {
            PartyMember[i].ExitPlayer();
        }
        PartyMember[0].SetPlayerData(GiveMyPartyData());
    }

    //�÷��̾� �̹��� ������ ��.
    public string GiveMyPartyData()
    {
        PlayerBackendData a = PlayerBackendData.Instance;
        string s = $"{a.nickname};{a.playerindate};{a.GetLv()};{a.GetPlayerAvatadataPartyRaid()};";
        return s;
    }
    
    
    
}

public class PartyRoom
{
    private string nowmap;
    private int level;
    
    
    
}

class PartyRaidBoss
{
    string monid;
    private decimal curhp;
    private decimal maxhp;

    public bool[] MonsterBuff;
    public int[] MonsterBuffStack;

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
}