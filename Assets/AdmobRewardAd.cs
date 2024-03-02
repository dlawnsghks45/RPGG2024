using System.Collections.Generic;
using UnityEngine;

public class AdmobRewardAd : MonoBehaviour
{
   //싱글톤만들기.
   private static AdmobRewardAd _instance = null;
   public static AdmobRewardAd Instance
   {
      get
      {
         if (_instance == null)
         {
            _instance = FindObjectOfType(typeof(AdmobRewardAd)) as AdmobRewardAd;

            if (_instance == null)
            {
               //Debug.Log("Player script Error");
            }
         }
         return _instance;
      }
   }
   
   private void Start()
   {
     Advertisements.Instance.Initialize();
   }
[SerializeField]
   private string nowsetenumid;

   private adsrewardslot slots;
   public void  ShowRewardVideo(string enumname,adsrewardslot slot)
   {
      nowsetenumid = enumname;
      slots = slot;
      Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
   }
   public void  ShowRewardVideo(string enumname)
   {
      nowsetenumid = enumname;
      Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
   }
   
   // ReSharper disable Unity.PerformanceAnalysis
   private void CompleteMethod(bool completed, string advertiser)
   {
      Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
      if (completed == true)
      {
         //give the reward
         GiveReward(nowsetenumid);
      }
      else
      {
         Debug.Log("보상이없다");
         //no reward
      }

      if (slots != null)
         slots.RefreshBuyCount();
   }

   public void SetSlots(adsrewardslot slots)
   {
      this.slots = slots;
      if(slots != null)
         slots.RefreshBuyCount();
   }
   
   // ReSharper disable Unity.PerformanceAnalysis
   public void GiveReward(string enumname)
   {
      List<string> id = new List<string>();
      List<string>howmany = new List<string>();
      switch (enumname)
      {  
         case "광고_엘리축복":
            Inventory.Instance.AddTime_min(PlayerBackendData.timesenum.ellibless, 30);
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/엘리축복광고온"),alertmanager.alertenum.일반);
            alertmanager.Instance.NotiCheck_PremiumShop();
            break;
         case "광고_도전장":
            Inventory.Instance.AddItem("1711",15,true);
            id.Add("1711");
            howmany.Add("15");
            Inventory.Instance.ShowEarnItem(id.ToArray(), howmany.ToArray(),false);
            alertmanager.Instance.NotiCheck_PremiumShop();

            break;
         case"광고_등급재설정":
            Inventory.Instance.AddItem("53",2,true);
            id.Add("53");
            howmany.Add("2");
            Inventory.Instance.ShowEarnItem(id.ToArray(), howmany.ToArray(),false);
            alertmanager.Instance.NotiCheck_PremiumShop();

            break;
         case"광고_품질재설정":
            Inventory.Instance.AddItem("52",2,true);
            id.Add("52");
            howmany.Add("2");
            Inventory.Instance.ShowEarnItem(id.ToArray(), howmany.ToArray(),false);
            alertmanager.Instance.NotiCheck_PremiumShop();

            break;
         case"광고_특효재설정":
            Inventory.Instance.AddItem("54",2,true);
            id.Add("54");
            howmany.Add("2");
            Inventory.Instance.ShowEarnItem(id.ToArray(), howmany.ToArray(),false);
            alertmanager.Instance.NotiCheck_PremiumShop();
            break;
         case"광고_낡은특효재설정":
            Inventory.Instance.AddItem("57",4,true);
            id.Add("57");
            howmany.Add("4");
            Inventory.Instance.ShowEarnItem(id.ToArray(), howmany.ToArray(),false);
            alertmanager.Instance.NotiCheck_PremiumShop();
            break;
         case "광고_제련석":
            Inventory.Instance.AddItem("99000",15,true);
            id.Add("99000");
            howmany.Add("15");
            Inventory.Instance.ShowEarnItem(id.ToArray(), howmany.ToArray(),false);
            alertmanager.Instance.NotiCheck_PremiumShop();
            break;
         case "광고_사냥1시간":
            Autofarmmanager.Instance.GiveReward1hr();
            break;
      }
      
      Savemanager.Instance.SaveInventory_SaveOn();
   }

}