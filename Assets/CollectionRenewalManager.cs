using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;


public class CollectionRenewalManager : MonoBehaviour
{
   public collectionrenewalslot obj;
   public List<collectionrenewalslot> collections = new List<collectionrenewalslot>();
   [SerializeField] private Transform Trans;

   private static CollectionRenewalManager _instance = null;

   public static CollectionRenewalManager Instance
   {
      get
      {
         if (_instance == null)
         {
            _instance = FindObjectOfType(typeof(CollectionRenewalManager)) as CollectionRenewalManager;

            if (_instance == null)
            {
               //Debug.Log("Player script Error");
            }
         }

         return _instance;
      }
   }

   private string nowpanelnum;

   public void Bt_SelectSubPanel(string num)
   {
      nowpanelnum = num;
      List<string> str = new List<string>();
      for (int i = 0; i < CollectionRenewalDB.Instance.NumRows(); i++)
      {
         // Debug.Log(CraftTableDB.Instance.GetAt(i).PanelType);
         if (CollectionRenewalDB.Instance.GetAt(i).type.Equals(num))
         {
            str.Add(CollectionRenewalDB.Instance.GetAt(i).id);
         }
      }

      //개수 체크 안맞으면 늘림
      if (collections.Count < str.Count)
      {
         for (int i = 0; i < str.Count; i++)
         {
            collectionrenewalslot item = Instantiate(obj, Trans);
            item.gameObject.SetActive(false);
            collections.Add(item);

         }
      }

      //제작슬릇보여주기
      for (int i = 0; i < collections.Count; i++)
      {
         if (str.Count > i)
         {
            collections[i].Refresh(str[i]);
            collections[i].gameObject.SetActive(true);
         }
         else
         {

            if (!collections[i].gameObject.activeSelf)
               break;

            collections[i].gameObject.SetActive(false);
         }
      }
   }

   public GameObject[] SubObj;

   public void Bt_SelectMainPanel(int num)
   {
      for (int i = 0; i < SubObj.Length; i++)
      {
         SubObj[i].SetActive(false);
      }

      SubObj[num].SetActive(true);
   }

   //응
   public Image SelectImage;
   public Text SelectText;
   public Text SelectStat;



   public Text TotalCount;

   public GameObject[] NotiMain;
   public GameObject[] NotiSub_Item;
   public GameObject[] NotiSub_Equip;
   public GameObject MenuNoti;
   public Text StatText;

   public void RefreshStat()
   {
      float statnum = 0;
      for (int i = 0; i < PlayerBackendData.Instance.RenewalCollectData.Length; i++)
      {
         if (PlayerBackendData.Instance.RenewalCollectData[i])
         {
            statnum += float.Parse(CollectionRenewalDB.Instance.GetAt(i).stat);
         }
      }

      Battlemanager.Instance.mainplayer.Stat_collection = statnum;
   }


   public void RefreshTotalCount()
   {
      int num = 0;

      for (int i = 0; i < PlayerBackendData.Instance.RenewalCollectData.Length; i++)
      {
         if (PlayerBackendData.Instance.RenewalCollectData[i])
            num++;
      }

      TotalCount.text =
         $"{num.ToString()} / {CollectionRenewalDB.Instance.NumRows()}\n(<color=yellow>{(((float)num / CollectionRenewalDB.Instance.NumRows()) * 100f):N2}%)</color>";
      RefreshStat();
      StatText.text = $"+{Battlemanager.Instance.mainplayer.Stat_collection}";
      //Noti체크
      foreach (var t in alertmanager.Instance.Alert_Collect)
         t.SetActive(false);
      foreach (var t in NotiMain)
         t.SetActive(false);
      foreach (var t in NotiSub_Item)
         t.SetActive(false);
      foreach (var t in NotiSub_Equip)
         t.SetActive(false);

      for (int i = 0; i < CollectionRenewalDB.Instance.NumRows(); i++)
      {
         CollectionRenewalDB.Row data = CollectionRenewalDB.Instance.GetAt(i);

         if (PlayerBackendData.Instance.RenewalCollectData[int.Parse(data.num)])
            continue;
         bool ie = bool.Parse(data.isequip);
         if (ie)
         {
            EquipItemDB.Row ed = EquipItemDB.Instance.Find_id(data.itemid);
            string type = ed.Type;
            //장비
            if (PlayerBackendData.Instance.GetTypeEquipment(type).Any(VARIABLE =>
                   VARIABLE.Value.Itemid.Equals(data.itemid) && !VARIABLE.Value.IsEquip && !VARIABLE.Value.IsLock))
            {
               switch (type)
               {
                  case "Weapon":
                     if (NotiSub_Equip[0].activeSelf)
                        continue;
                     NotiSub_Equip[0].SetActive(true);
                     NotiMain[1].SetActive(true);
                     MenuNoti.SetActive(true);
                     if (!alertmanager.Instance.Alert_Menu.activeSelf)
                        alertmanager.Instance.Alert_Menu.SetActive(true);
                     break;
                  case "SWeapon":
                     if (NotiSub_Equip[1].activeSelf)
                        continue;
                     NotiSub_Equip[1].SetActive(true);
                     NotiMain[1].SetActive(true);
                     MenuNoti.SetActive(true);
                     if (!alertmanager.Instance.Alert_Menu.activeSelf)
                        alertmanager.Instance.Alert_Menu.SetActive(true);

                     break;
                  case "Helmet":
                     if (NotiSub_Equip[2].activeSelf)
                        continue;
                     NotiSub_Equip[2].SetActive(true);
                     NotiMain[1].SetActive(true);
                     MenuNoti.SetActive(true);
                     if (!alertmanager.Instance.Alert_Menu.activeSelf)
                        alertmanager.Instance.Alert_Menu.SetActive(true);

                     break;
                  case "Chest":
                     if (NotiSub_Equip[3].activeSelf)
                        continue;
                     NotiSub_Equip[3].SetActive(true);
                     NotiMain[1].SetActive(true);
                     MenuNoti.SetActive(true);
                     if (!alertmanager.Instance.Alert_Menu.activeSelf)
                        alertmanager.Instance.Alert_Menu.SetActive(true);
                     break;
                  case "Glove":
                     if (NotiSub_Equip[4].activeSelf)
                        continue;
                     NotiSub_Equip[4].SetActive(true);
                     NotiMain[1].SetActive(true);
                     MenuNoti.SetActive(true);
                     if (!alertmanager.Instance.Alert_Menu.activeSelf)
                        alertmanager.Instance.Alert_Menu.SetActive(true);
                     break;
                  case "Boot":
                     if (NotiSub_Equip[5].activeSelf)
                        continue;
                     NotiSub_Equip[5].SetActive(true);
                     NotiMain[1].SetActive(true);
                     MenuNoti.SetActive(true);
                     if (!alertmanager.Instance.Alert_Menu.activeSelf)
                        alertmanager.Instance.Alert_Menu.SetActive(true);
                     break;
                  case "Ring":
                     if (NotiSub_Equip[6].activeSelf)
                        continue;
                     NotiSub_Equip[6].SetActive(true);
                     NotiMain[1].SetActive(true);
                     MenuNoti.SetActive(true);
                     if (!alertmanager.Instance.Alert_Menu.activeSelf)
                        alertmanager.Instance.Alert_Menu.SetActive(true);
                     break;
                  case "Necklace":
                     if (NotiSub_Equip[7].activeSelf)
                        continue;
                     NotiSub_Equip[7].SetActive(true);
                     NotiMain[1].SetActive(true);
                     MenuNoti.SetActive(true);
                     if (!alertmanager.Instance.Alert_Menu.activeSelf)
                        alertmanager.Instance.Alert_Menu.SetActive(true);
                     break;
               }
            }
         }
         else
         {
            //아이템
            if (PlayerBackendData.Instance.CheckItemCount(data.itemid) >= int.Parse(data.hw))
            {
               int a = int.Parse(data.type);

               if (NotiSub_Item[a].activeSelf)
                  continue;

               NotiMain[0].SetActive(true); //아이템
               NotiSub_Item[a].SetActive(true); //던전
               MenuNoti.SetActive(true);
               if (!alertmanager.Instance.Alert_Menu.activeSelf)
                  alertmanager.Instance.Alert_Menu.SetActive(true);
            }
         }


      }


   }


   public GameObject OnPanel;
   public GameObject OffPanel;


   public GameObject FinishButton;
   public GameObject AlreadyFinishButton;
   private string nowselectid;
   public Text haveitem;
   private CollectionRenewalDB.Row data;

   public void Bt_Select(string id)
   {
      nowselectid = id;
      OnPanel.SetActive(true);
      OffPanel.SetActive(false);

      data = CollectionRenewalDB.Instance.Find_id(id);

      bool isequip = bool.Parse(data.isequip);

      if (isequip)
      {
         SelectImage.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(data.itemid).Sprite);
         SelectText.text = Inventory.GetTranslate(EquipItemDB.Instance.Find_id(data.itemid).Name);
         SelectText.color = Inventory.Instance.GetRareColor(EquipItemDB.Instance.Find_id(data.itemid).Rare);
         haveitem.text = "";
      }
      else
      {
         SelectImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(data.itemid).sprite);
         SelectText.text =
            $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(data.itemid).name)} X {data.hw}";
         SelectText.color = Inventory.Instance.GetRareColor(ItemdatabasecsvDB.Instance.Find_id(data.itemid).rare);
         haveitem.text = string.Format(Inventory.GetTranslate("UI6/수집_보유"),
            PlayerBackendData.Instance.CheckItemCount(data.itemid));

      }

      if (PlayerBackendData.Instance.RenewalCollectData[int.Parse(data.num)])
      {
         //수집함
         FinishButton.SetActive(false);
         AlreadyFinishButton.SetActive(true);
         SelectStat.text = string.Format(Inventory.GetTranslate("collectinfoone2/1"), data.stat);
         SelectStat.color = Color.cyan;
      }
      else
      {
         //수집안함
         FinishButton.SetActive(true);
         AlreadyFinishButton.SetActive(false);
         SelectStat.text = string.Format(Inventory.GetTranslate("collectinfoone2/1"), data.stat);
         SelectStat.color = Color.gray;
      }
   }

   public UIView Panel;
   public GameObject Subpanel;

   public void Bt_ShowCollectionPanel()
   {
      Panel.Show(false);

      OnPanel.SetActive(false);
      OffPanel.SetActive(true);
      RefreshTotalCount();

      Subpanel.SetActive(false);

      for (int i = 0; i < collections.Count; i++)
      {
         if (collections[i].gameObject.activeSelf)
         {
            collections[i].gameObject.SetActive(false);
         }
      }
   }


   public void Bt_OnCollectItem()
   {
      data = CollectionRenewalDB.Instance.Find_id(nowselectid);

      bool isequip = bool.Parse(data.isequip);

      if (isequip)
      {
         ShowEquipCollect(data.itemid);
         return;
      }
      else
      {
         if (PlayerBackendData.Instance.CheckItemCount(data.itemid) >= int.Parse(data.hw))
         {
            PlayerBackendData.Instance.RemoveItem(data.itemid, int.Parse(data.hw));
            PlayerBackendData.Instance.RenewalCollectData[int.Parse(data.num)] = true;
            Savemanager.Instance.SaveInventory();
            Savemanager.Instance.Save();
            SaveCollection_INven();
            //로그
            PlayerData.Instance.RefreshPlayerstat();
            LogManager.CollectionLog(data.id, false, data.itemid);

         }
         else
         {
            //없다
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/수집_없음"), alertmanager.alertenum.주의);
            return;
         }
      }

      Bt_Select(nowselectid);
      Bt_SelectSubPanel(nowpanelnum);
      RefreshTotalCount();
   }

   public UIView EquipShowPanel;
   public Text EquipName;
   public Text[] EquipES;
   public EquipmentItemData Equipslots;
   public Text EquipCount;
   public UIToggle equiptoggle;
   private int nowpage;
   List<EquipDatabase> equipdatabase = new List<EquipDatabase>();

   public void ShowEquipCollect(string equipid)
   {
      EquipItemDB.Row equipdata = EquipItemDB.Instance.Find_id(equipid);
      equipdatabase.Clear();
      nowpage = 0;
      foreach (var VARIABLE in PlayerBackendData.Instance.GetTypeEquipment(equipdata.Type))
      {
         if (VARIABLE.Value.Itemid.Equals(equipdata.id))
         {
            if (!VARIABLE.Value.IsEquip && !VARIABLE.Value.IsLock)
               equipdatabase.Add(VARIABLE.Value);
         }
      }

      if (equipdatabase.Count == 0)
      {
         //수집 불가
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/수집_없음"), alertmanager.alertenum.주의);
         return;
      }


      EquipShowPanel.Show(false);
      RefreshEquipData();
   }

   void RefreshEquipData()
   {
      Equipslots.Refresh(equipdatabase[nowpage]);
      EquipName.text = Inventory.GetTranslate(EquipItemDB.Instance.Find_id(equipdatabase[nowpage].Itemid).Name);
      EquipName.color = Inventory.Instance.GetRareColor(equipdatabase[nowpage].Itemrare);

      for (int i = 0; i < EquipES.Length; i++)
         EquipES[i].text = "";

      for (int i = 0; i < equipdatabase[nowpage].EquipSkill1.Count; i++)
      {
         string id = equipdatabase[nowpage].EquipSkill1[i];
         EquipES[i].text =
            $"[{Inventory.GetTranslate(EquipSkillDB.Instance.Find_id(id).name)} Lv.{EquipSkillDB.Instance.Find_id(id).lv}]";
         Inventory.Instance.ChangeItemRareColor(EquipES[i], EquipSkillDB.Instance.Find_id(id).rare);
      }


      equiptoggle.IsOn = false;

      EquipCount.text = $"{nowpage + 1}/{equipdatabase.Count}";
   }

   public void Bt_NextEquip()
   {
      if (nowpage == equipdatabase.Count - 1)
      {
         return;
      }
      else
      {
         nowpage++;
         RefreshEquipData();
      }
   }

   public void Bt_PrevEquip()
   {
      if (nowpage == 0)
      {
         return;
      }
      else
      {
         nowpage--;
         RefreshEquipData();
      }
   }

   public void Bt_EquipCollectFinish()
   {
      if (!equiptoggle.IsOn)
         return;

      EquipShowPanel.Hide(false);
      LogManager.CollectionLog(data.id, true, data.itemid,
         PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(equipdatabase[nowpage].Itemid).Type)[
            equipdatabase[nowpage].KeyId1]);
      PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(equipdatabase[nowpage].Itemid).Type)
         .Remove(equipdatabase[nowpage].KeyId1);
      PlayerBackendData.Instance.RenewalCollectData[int.Parse(data.num)] = true;
      Savemanager.Instance.SaveEquip();
      Savemanager.Instance.SaveCollection();
      SaveCollection_Equip();
      Savemanager.Instance.Save();
      PlayerData.Instance.RefreshPlayerstat();
      Bt_Select(nowselectid);
      Bt_SelectSubPanel(nowpanelnum);
      RefreshTotalCount();
   }

   void SaveCollection_INven()
   {
      Savemanager.Instance.SaveEvery();
      PlayerBackendData userData = PlayerBackendData.Instance;
      Param paramB = new Param
      {
         { "CollectionNew", PlayerBackendData.Instance.RenewalCollectData },
         { "inventory", userData.ItemInventory },
      };
      Where where = new Where();
      where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

      SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramB, (callback) =>
      {
       //  Debug.Log(callback);
         if (!callback.IsSuccess()) return;
      });
      issave = true;
   }

   
   void SaveCollection_Equip()
   {
      Savemanager.Instance.SaveEvery();
      PlayerBackendData userData = PlayerBackendData.Instance;

      Param paramB = new Param
      {
         { "CollectionNew", PlayerBackendData.Instance.RenewalCollectData },
         //가방
         { "equipment_Weapon", userData.Equiptment0 },
         { "equipment_SubWeapon", userData.Equiptment1 },
         { "equipment_Helmet", userData.Equiptment2 },
         { "equipment_Armor", userData.Equiptment3 },
         { "equipment_Glove", userData.Equiptment4 },
         { "equipment_Boot", userData.Equiptment5 },
         { "equipment_Ring", userData.Equiptment6 },
         { "equipment_Necklace", userData.Equiptment7 },
         { "equipment_Wing", userData.Equiptment8 },
         { "equipment_Pet", userData.Equiptment9 },
         { "equipment_Rune", userData.Equiptment10 },
         { "equipment_Insignia", userData.Equiptment11 },
         { "EquipmentNow", userData.EquipEquiptment0 },
      };
      Where where = new Where();
      where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

      SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramB, (callback) =>
      {
        // Debug.Log(callback);
         if (!callback.IsSuccess()) return;
      });
      issave = true;
   }

   private bool issave = false;
   
   public void Bt_SaveAll()
   {
      if(!issave)
         return;

      issave = false;
      LogManager.CollectionLogAll();
   }
   
}
