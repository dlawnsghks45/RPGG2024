using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questrewarddata : MonoBehaviour
{
   public int Stringnum;
   public int[]  Points;
   public string[] RewardID;
   public int[] RewardHw;
   public Text TotalCount;
   public int maxtotalcount;

   [SerializeField] private questtotalrewardslot[] totalrewardslot;
   
   public Image Bar;
   private void Start()
   {
      initdata();
   }

   public void initdata()
   {
      RefreshCount();
   }

   public void RefreshCount()
   {
      for (int i = 0; i <  totalrewardslot.Length; i++)
      {
         totalrewardslot[i].PointText.text = Points[i].ToString();
         totalrewardslot[i].Items.Refresh(RewardID[i],RewardHw[i],false);
         
         if (Points[i] > PlayerBackendData.Instance.QuestTotalCount[Stringnum])
         {
            //보상을 안받아서 열려있음
            totalrewardslot[i].Effect.SetActive(true);
            totalrewardslot[i].EarnedPanel.SetActive(false);
         }
         else
         {
            //보상을 받아서 없음.
            totalrewardslot[i].EarnedPanel.SetActive(true);
            totalrewardslot[i].Effect.SetActive(false);
         }
      }
      TotalCount.text = PlayerBackendData.Instance.QuestTotalCount[Stringnum].ToString("N0");
      Bar.fillAmount = PlayerBackendData.Instance.QuestTotalCount[Stringnum]/maxtotalcount;
   }

   public void AddTotalExp(float exp)
   {
      float prevexp = PlayerBackendData.Instance.QuestTotalCount[Stringnum];
      PlayerBackendData.Instance.QuestTotalCount[Stringnum] += exp;

      for (int i = 0; i < Points.Length; i++)
      {
         //넘어야함
         if (prevexp < Points[i])
         {
            if (PlayerBackendData.Instance.QuestTotalCount[Stringnum] >= Points[i])
            {
               QuestManager.Instance.id.Add(RewardID[i]);
               QuestManager.Instance.hw.Add(RewardHw[i]);
               Inventory.Instance.AddItem(RewardID[i],RewardHw[i],false);
            }
         }
      }
      RefreshCount();
      Inventory.Instance.ShowEarnItem3(QuestManager.Instance.id.ToArray(), QuestManager.Instance.hw.ToArray(), false);
   }

   public void AddTotalExpAll(float exp)
   {
      if (PlayerBackendData.Instance.QuestTotalCount[Stringnum] >= maxtotalcount)
         return;
         
      float prevexp = PlayerBackendData.Instance.QuestTotalCount[Stringnum];
      
      PlayerBackendData.Instance.QuestTotalCount[Stringnum] += exp;

      if (PlayerBackendData.Instance.QuestTotalCount[Stringnum] > maxtotalcount)
         PlayerBackendData.Instance.QuestTotalCount[Stringnum] = maxtotalcount;
      
      for (int i = 0; i < Points.Length; i++)
      {
         //넘어야함
         if (prevexp < Points[i])
         {
            if (PlayerBackendData.Instance.QuestTotalCount[Stringnum] >= Points[i])
            {
               QuestManager.Instance.id.Add(RewardID[i]);
               QuestManager.Instance.hw.Add(RewardHw[i]);
               Inventory.Instance.AddItem(RewardID[i],RewardHw[i],false);
            }
         }
      }
      RefreshCount();
     
   }
}


public enum QuestType
{
   일일,
   주간,
   일일이벤트,
   이벤트,
}
