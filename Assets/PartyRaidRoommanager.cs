using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyRaidRoommanager : MonoBehaviour
{
    public GameObject StartButton;
    public GameObject ReadyButton;
    public GameObject UnReadyButton;


    public PartyMemberslot[] PartyMember;

    
    
    
    //파티장
    public bool ispartyleader;
    
    
    //레이드 시작
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
            Debug.Log("모두 준비 완료");
        }
        else
        {
            Debug.Log("누가 레디 안함");
        }
    }

    
    //방만듬
    public void Bt_MakeRoom()
    {
        ispartyleader = true;
        for (int i = 0; i < PartyMember.Length; i++)
        {
            PartyMember[i].ExitPlayer();
        }
        PartyMember[0].SetPlayerData(GiveMyPartyData());
    }

    //플레이어 이미지 정보를 줌.
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
    광폭, // 일정 시간 마다 몬스터 피해량 상승.
    강인함,//받는 피해 감소(무력화 시 삭제).
    민첩함,//치명타 방어 30% (치명타 확률 % 만큼 깎음).
    발화,//일정 시간 마다 생명력 회복 (일정 스테이지 클리어 시 삭제.
    해,//물리,마법 스킬 피해량 감소 (해의 토템 파괴 시 삭제).
    달,//상태 이상 피해량 감소 (달의 토템 파괴 시 삭제).
    불멸,//무력화가 잠금일 때 피해를 입지 않음. (삭제 불가).
}