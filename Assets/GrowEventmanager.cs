using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using EasyMobile.Demo;
using Sirenix.Utilities;
using UnityEngine;

public class GrowEventmanager : MonoBehaviour
{
   
   private static GrowEventmanager _instance = null;
   public static GrowEventmanager Instance
   {
      get
      {
         if (_instance == null)
         {
            _instance = FindObjectOfType(typeof(GrowEventmanager)) as GrowEventmanager;

            if (_instance == null)
            {
               //Debug.Log("Player script Error");
            }
         }
         return _instance;
      }
   }

   
   
   public UIView GrowChoicePanel;
   public GameObject GrowChoice;

   public GameObject[] WeaponShow;

   public GameObject[] Recomend_First;
   public GameObject[] Recomend_WeaponMelee;
   public GameObject[] Recomend_WeaponMagic;
   public void Bt_ShowPanel()
   {
      if (PlayerBackendData.Instance.GetLv() >= 1500)
         return;
      
      
      //직업 계산
      GrowChoicePanel.Show(false);

      foreach (var VARIABLE in Recomend_First)
      {
         VARIABLE.SetActive(false);
      }
      
      foreach (var VARIABLE in Recomend_WeaponMelee)
      {
         VARIABLE.SetActive(false);
      }
      
      foreach (var VARIABLE in Recomend_WeaponMagic)
      {
         VARIABLE.SetActive(false);
      }
      switch (ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassId).mainstat)
      {
         case "str":
            Recomend_First[0].SetActive(true);
           // Recomend_WeaponMelee[0].SetActive(true);
            Recomend_WeaponMelee[1].SetActive(true);
            break;
         case "dex":
            Recomend_First[0].SetActive(true);
            //Recomend_WeaponMelee[2].SetActive(true);
            Recomend_WeaponMelee[3].SetActive(true);
            break;
         case "int":
            Recomend_First[1].SetActive(true);
            Recomend_First[2].SetActive(true);
            Recomend_WeaponMagic[0].SetActive(true);
            //Recomend_WeaponMagic[1].SetActive(true);
            //Recomend_WeaponMagic[3].SetActive(true);
            break;
      }
   }

   public void Start()
   {
      //복귀 유저
      if (PlayerBackendData.Instance.GetLv() < 1500 &&
          PlayerBackendData.Instance.tutoguideid != 0)
      {
         PlayerBackendData.Instance.tutoguideid = 0;
         PlayerBackendData.Instance.tutoguideisfinish = false;
         
         Savemanager.Instance.SaveGuideQuest();
         Savemanager.Instance.Save();
         Tutorialmanager.Instance.hidealluiview();
         Bt_ShowPanel();
      }
   }


   public void Bt_SelectPanel(int num)
   {
      GrowChoice.SetActive(false);
      foreach (var VARIABLE in WeaponShow)
      {
         VARIABLE.SetActive(false);
      }

      WeaponShow[num].gameObject.SetActive(true);
   }

   public void Bt_BackOne()
   {
      foreach (var VARIABLE in WeaponShow)
      {
         VARIABLE.SetActive(false);
      }
      GrowChoice.SetActive(true);
   }


   public string nowid = "1000";
   public GameObject GivePanel;
   public void Bt_SetEquipID(string id)
   {
      nowid = id;
//      Debug.Log(nowid);
      GivePanel.SetActive(true);
      JumpDB.Row data = JumpDB.Instance.Find_id(nowid);

      string[] equip = data.equipid.Split(';');

      for (int i = 0; i < equip.Length; i++)
      {
         itemicons[i].Refresh(equip[i],1,false,false,true);
      }
   }

   public itemiconslot[] itemicons;
    List<string> id = new List<string>();
      List<int> hw = new List<int>();

      public void Bt_GiveAll()
      {
         //아이템으로 지급
         id.Clear();
         hw.TrimExcess();
         //장비 지급
         JumpDB.Row data = JumpDB.Instance.Find_id(nowid);

         string[] equip = data.equipid.Split(';');

         for (int i = 0; i < equip.Length; i++)
         {
            PlayerBackendData.Instance.MakeEquipment_Min(equip[i]);
            id.Add(equip[i]);
            hw.Add(1);
         }

         Inventory.Instance.ShowEarnItem3(id.ToArray(), hw.ToArray(), true);

         //레벨업
         PlayerBackendData.Instance.SetLv(1500);
         PlayerData.Instance.RefreshInitData();

         PlayerBackendData.Instance.SetGoldAltarLv(3000, altarmanager.AltarType.제단);
         PlayerBackendData.Instance.SetGoldAltarLv(6000, altarmanager.AltarType.골드);
         PlayerBackendData.Instance.SetGoldAltarLv(1500, altarmanager.AltarType.레이드);
         PlayerBackendData.Instance.SetGoldAltarLv(2000, altarmanager.AltarType.개미굴);
         PlayerBackendData.Instance.SetAdLv(15);
         PlayerBackendData.Instance.SetFieldLV(64);

         id.Clear();
         hw.Clear();

         List<string> skills = new List<string>();
         //직업 지급
         if (ClassSelectToggle.IsOn)
         {
            //직업해금및 패시브 장착및 스킬자착
            string[] giveclass = data.openclass.Split(';');

            for (int i = 0; i < giveclass.Length; i++)
            {
               Classmanager.Instance.Bt_BuyClass(giveclass[i]);
//w               Debug.Log("준 스킬 " + giveclass[i]  + "ㅋ" + ClassDB.Instance.Find_id(giveclass[i]).giveskill);
            }

            //직업해금및 패시브 장착및 스킬자착
            string[] giveclass2 = data.equipskill.Split(';');

            for (int i = 0; i < giveclass2.Length; i++)
            {
               skills.Add(ClassDB.Instance.Find_id(giveclass2[i]).giveskill);
               Classmanager.Instance.Bt_SelectPassive(giveclass2[i]);
            }
            
            Classmanager.Instance.Bt_SelectClass(data.selectclass, skills.ToArray(), true);
            Skillmanager.Instance.mainplayer.SetClass_start();
            Battlemanager.Instance.mainplayer.hpmanager.HealAll();
            Passivemanager.Instance.Refresh();
            Classmanager.Instance.RefreshJobSlot();
            Classmanager.Instance.RefreshJobitemslots();
            PlayerData.Instance.RefreshClassName();
            mapmanager.Instance.LocateMap("1033");
            abilitymanager.Instance.Bt_RefreshReco();
         }
         else
         {


            string[] giveclassitem = data.itemid.Split(';');
            string[] giveclassitemhw = data.itemhw.Split(';');

            for (int i = 0; i < giveclassitem.Length; i++)
            {
               id.Add(giveclassitem[i]);
               hw.Add(int.Parse(giveclassitemhw[i]));
               Inventory.Instance.AddItem(giveclassitem[i], int.Parse(giveclassitemhw[i]), false);
            }

            Invoke("ShowItemEarn", 2f);
         }

         DamageManager.Instance.ShowEffect_LvUp(Battlemanager.Instance.mainplayer.transform);
         Soundmanager.Instance.PlayerSound("Sound/Special Click 05");

         Invoke("ShowGrowGuide", 1.5f);
         //장비 장착
         Inventory.Instance.bt_autoequip();
         PlayerData.Instance.RefreshPlayerstat();
         alertmanager.Instance.NotiCheck_Altar();
         altarmanager.Instance.RefreshAltarLv();
         GivePanel.SetActive(false);
         GrowChoicePanel.Hide(false);
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/1_진로선택완료"), alertmanager.alertenum.일반);
         Settingmanager.Instance.SaveAllNoLog();
         Savemanager.Instance.SaveExpData();
         Savemanager.Instance.SaveClassData();
         Savemanager.Instance.Save();
         TutorialTotalManager.Instance.CheckGuideQuest("SelectClass");
      }

      public void ShowItemEarn()
      {
         Inventory.Instance.ShowEarnItem3(id.ToArray(), hw.ToArray(), false);
      }

      public void ShowGrowGuide()
   {
      TutorialTotalManager.Instance.guidepanel.Show(false);
   }

   public UIToggle ClassSelectToggle;
   public void Bt_Close()
   {
      foreach (var VARIABLE in WeaponShow)
      {
         VARIABLE.SetActive(false);
      }
      GrowChoice.SetActive(true);
      GivePanel.SetActive(false);
      
   }
   
   
}
