using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countpanel : MonoBehaviour
{
   public int Maxcount;
   public int nowcount;
   public Text CountText;
   
   public void Bt_AddCount(int num)
   {
      if (nowcount + num > Maxcount)
      {
         nowcount = Maxcount;
      }
      else
      {
         nowcount += num;
      }
      
      RefreshCountText();
   }

   public void SetCount(int num)
   {
      nowcount = num;
      RefreshCountText();
   }
   
   public void Bt_MinusCount(int num)
   {
      if (nowcount - num < 1)
      {
         nowcount = 1;
      }
      else
      {
         nowcount -= num;
      }
      
      RefreshCountText();
   }

   void RefreshCountText()
   {
      CountText.text = nowcount.ToString();
   }
}
