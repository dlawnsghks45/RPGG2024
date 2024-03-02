using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;using UnityEngine.UI;

public class Equipchangemanager : MonoBehaviour
{
   private static Equipchangemanager _instance = null;

   public static Equipchangemanager Instance
   {
      get
      {
         if (_instance == null)
         {
            _instance = FindObjectOfType(typeof(Equipchangemanager)) as Equipchangemanager;

            if (_instance == null)
            {
               //Debug.Log("Player script Error");
            }
         }

         return _instance;
      }
   }
   
   //장비 변경 시스템
   public EquipmentItemData MyEquip;
   public EquipmentItemData ChangeEquip;
   public Text MyEquipName;
   public Text ChangeEquipName;
   public Text NeedItem;
   public Text HaveItem;
   [SerializeField] private UIView ChangePanel;

   public EquipChangeSlot[] Slots;
   
   EquipItemDB.Row equipdata ;
   EquipChangeDB.Row ChangeData ;
   private EquipDatabase A;
   private EquipDatabase B;
   public void Bt_ShowChange()
   {
      foreach (var t in Slots)
      {
         t.gameObject.SetActive(false);
      }

      EquipDatabase nowselectdata = Inventory.Instance.data;
      
      ChangePanel.Show(false);

      A = nowselectdata;
      B = nowselectdata;
      
     equipdata = EquipItemDB.Instance.Find_id(nowselectdata.Itemid);
      ChangeData = EquipChangeDB.Instance.Find_id(equipdata.ChangeId);
    

      //
      MyEquip.Refresh(A);
      ChangeEquip.Refresh(B);
      MyEquipName.text = nowselectdata.GetItemName();
      MyEquipName.color = Inventory.Instance.GetRareColor(MyEquip.data.Itemrare);
      ChangeEquip.gameObject.SetActive(false);
      ChangeEquipName.text = "";
      
      
      //변경 정보
      string[] stringid;
      stringid = ChangeData.equip.Split(';');
      int num = 0;
      for (int i = 0; i < stringid.Length; i++)
      {
         if (stringid[i] != nowselectdata.Itemid)
         {
            Slots[num].Refresh(EquipItemDB.Instance.Find_id(stringid[i]),nowselectdata.Itemrare);
            Slots[num].OffPanel();
            Slots[num].gameObject.SetActive(true);
            num++;
         }
      }

      NeedItem.text =
         $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(ChangeData.itemid).name)}X{ChangeData.hw}";
      HaveItem.text = string.Format(Inventory.GetTranslate("UI6/수집_보유"),PlayerBackendData.Instance.CheckItemCount(ChangeData.itemid));
   }

   private string selectID;
   public void Bt_SelectItem(string selectid)
   {
      foreach (var t in Slots)
      {
         if (t.gameObject.activeSelf)
         {
            t.OffPanel();
         }
      }

      selectID = selectid;
      ChangeEquip.ItemImage.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(selectid).Sprite);
      ChangeEquipName.text = $"+{ Inventory.Instance.data.EnchantNum1} {Inventory.GetTranslate(EquipItemDB.Instance.Find_id(selectid).Name)}";
      ChangeEquipName.color = Inventory.Instance.GetRareColor(MyEquip.data.Itemrare);
      ChangeEquip.gameObject.SetActive(true);
   }
   public void Bt_ChangeGear()
   {
      if (selectID == null)
      {
         return;
      }

      if (PlayerBackendData.Instance.CheckItemCount("1751") < int.Parse(ChangeData.hw))
      {
         Debug.Log("아이템이 부족하다");
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/장비변경불가"), alertmanager.alertenum.주의);
         return;
      }

      PlayerBackendData.Instance.RemoveItem("1751",int.Parse(ChangeData.hw));
      B.Itemid = selectID;
      Inventory.Instance.data = B;
      if (EquipItemDB.Instance.Find_id(ChangeEquip.data.Itemid).SpeMehodP != "0")
      {
         Inventory.Instance.data.EquipSkill1[0] = EquipSkillDB.Instance
            .Find_id(EquipItemDB.Instance.Find_id(ChangeEquip.data.Itemid).SpeMehodP).id;
      }
      
      string Prevkeyname = PlayerBackendData.Instance.GetTypeEquipment(equipdata.Type)[Inventory.Instance.data.KeyId1].KeyId1;
      string Nextkeyname = PlayerBackendData.Instance.GetTypeEquipment(equipdata.Type)[Inventory.Instance.data.KeyId1].KeyId1.Replace(equipdata.id,selectID);
      PlayerBackendData.Instance.GetTypeEquipment(equipdata.Type)[Inventory.Instance.data.KeyId1].KeyId1 = Nextkeyname;
      equipoptionchanger.RenameKey(PlayerBackendData.Instance.GetTypeEquipment(equipdata.Type), Prevkeyname, Nextkeyname);
      
      
      alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/장비변경완료"),alertmanager.alertenum.일반);
      LogManager.EquipChangeLog(A.Itemid,selectID,ChangeData.id);
      ChangePanel.Hide(false);
      Inventory.Instance.ShowInventoryItem(Inventory.Instance.data);
      Inventory.Instance.Bt_RefreshNowEquipInven();
      Inventory.Instance.GetEquipSlots(EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid).Type).SetItem(Inventory.Instance.data,true,false);
      PlayerData.Instance.RefreshPlayerstat();
      Savemanager.Instance.SaveEquip();
      Savemanager.Instance.SaveInventory();
      Savemanager.Instance.Save();
   }

   public void ExitPanel()
   {
   }
}
