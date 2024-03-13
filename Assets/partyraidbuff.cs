using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class partyraidbuff : MonoBehaviour
{
   public GameObject[] Buff;
   public GameObject[] Penalty;


   public void SetBuff(int penalty,int buffnum)
   {
      foreach (var VARIABLE in Penalty)
      {
         VARIABLE.SetActive(false);
      }
      foreach (var VARIABLE in Buff)
      {
         VARIABLE.SetActive(false);
      }

      if (PartyRaidBattlemanager.Instance.battledata.MonsterBuff[penalty] != 0)
      {
         Penalty[penalty].SetActive(true);
      }
      Penalty[6].SetActive(true);

      Buff[buffnum].SetActive(true);
   }
}

public enum BuffEnum
{
   ����,
   ���
}

public enum PenaltyEnum
{
   ����,
   ����,
   ��ø,
   ��ȭ,
   ��,
   ��,
   �Ҹ�
}