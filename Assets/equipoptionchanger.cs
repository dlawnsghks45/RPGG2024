using System;
using System.Collections;
using BackEnd;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class equipoptionchanger : MonoBehaviour
{
   //싱글톤만들기.
   private static equipoptionchanger _instance = null;

   public static equipoptionchanger Instance
   {
      get
      {
         if (_instance == null)
         {
            _instance = FindObjectOfType(typeof(equipoptionchanger)) as equipoptionchanger;

            if (_instance == null)
            {
               //Debug.Log("Player script Error");
            }
         }

         return _instance;
      }
   }



   public GameObject EquipBlind;

   public UIButton RareButtons; //등급 품질 특수효과 순
   public UIButton CraftButtons; //등급 품질 특수효과 순
   public UIButton ESkillButtons; //등급 품질 특수효과 순
   public UIButton SmeltButtons; //등급 품질 특수효과 순
   public UIButton ESkillLessButtons; //불안정한 특수효과 순
   public UIButton AdvanButtons; //승급 순
   public UIButton ChangeButtons; //장비 변경
   public UIButton UpgradeButtons; //장비 변경


   public RectTransform[] RefreshItembars;

   public UIView Panel;

   //품질및 등급은 정해진 확률로 간다.
   //품질및 등급은 현재 등급 아래로 떨어지지 않는다.


   public void ShowPanel()
   {
      Settingmanager.Instance.falseserveron();
   }

   public void Checkpanel()
   {
      EquipItemDB.Row equipdata = EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid);

      //등급 버튼 설정
      string[] getrare = equipdata.RarePercent.Split(';');

      string minrare = Inventory.Instance.data.Itemrare; //자기레벨이 현재 레벨 실패 시 유짖 
      string maxrare = "0";


      for (var index = 0; index < getrare.Length; index++)
      {
         if (getrare[index] != "0")
         {
            if (int.Parse(maxrare) < index)
            {
               maxrare = index.ToString();
            }
         }
      }

      if (EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid).UpgradeNeedID != "")
      {
         UpgradeButtons.gameObject.SetActive(true);
      }
      else
      {
         UpgradeButtons.gameObject.SetActive(false);
      }
      
      if (int.Parse(minrare) < int.Parse(maxrare))
      {
         RareButtons.gameObject.SetActive(true);
         Inventory.Instance.InvenslotButtons[(int)Inventory.invenslotenumbutton.재설정].SetActive(true);
      }
      else
         RareButtons.gameObject.SetActive(false);

      if (Inventory.Instance.data.CraftRare1 != 5)
      {
         //가능
         CraftButtons.gameObject.SetActive(true);
         Inventory.Instance.InvenslotButtons[(int)Inventory.invenslotenumbutton.재설정].SetActive(true);
      }
      else
      {
         //불가 
         CraftButtons.gameObject.SetActive(false);
      }


      if (equipdata.SpeMehod != "0")
      {
         ESkillButtons.gameObject.SetActive(true);
         Inventory.Instance.InvenslotButtons[(int)Inventory.invenslotenumbutton.재설정].SetActive(true);
      }
      else
         ESkillButtons.gameObject.SetActive(false);

      ESkillButtons.gameObject.SetActive(equipdata.SpeMehod != "0" ? true : false);


      //제련 
      if (Inventory.Instance.data.MaxStoneCount1 != 10)
      {
         SmeltButtons.gameObject.SetActive(true);
         Inventory.Instance.InvenslotButtons[(int)Inventory.invenslotenumbutton.재설정].SetActive(true);
      }
      else
      {
         SmeltButtons.gameObject.SetActive(false);
      }


      //승급 확인
      if (equipdata.AdvanEquipID != "")
      {
         AdvanButtons.gameObject.SetActive(true);
         Inventory.Instance.InvenslotButtons[(int)Inventory.invenslotenumbutton.재설정].SetActive(true);
      }
      else
      {
         AdvanButtons.gameObject.SetActive(false);
      }
      
      //장비변경
      if (equipdata.ChangeId != "" )
      {
         ChangeButtons.gameObject.SetActive(true);
         Inventory.Instance.InvenslotButtons[(int)Inventory.invenslotenumbutton.재설정].SetActive(true);
      }
      else
      {
         ChangeButtons.gameObject.SetActive(false);
      }
      
      
      //불안정한 특수효과
      if (Inventory.Instance.data.GetEquipSkillCount() < 4 && equipdata.SpeMehod != "0")
      {
         ESkillLessButtons.gameObject.SetActive(true);
         Inventory.Instance.InvenslotButtons[(int)Inventory.invenslotenumbutton.재설정].SetActive(true);
      }
      else
      {
         ESkillLessButtons.gameObject.SetActive(false);
      }

      //아이디를 가져옴 
      //등급이 되는지 품질이 되는지 본다.
   }


   public UIView RarePanel;
   public GameObject[] Rareobj;
   public Text[] RareText;
   public Text RareWeaponName;
   public Text RareItemCount;

   public ParticleSystem RareEffect_Start;
   public ParticleSystem RareShowEffect_Succ;
   public ParticleSystem RareShowEffect_Fail;

   public int lockcountEs = 0;

   public void ReducelockcountEs()
   {
      lockcountEs--;
      if (lockcountEs < 0)
         lockcountEs = 0;
   }

   public void ShowRarePanel()
   {
      isstart = false;
      issucc = false;
      RarePanel.Show(false);

      EquipItemDB.Row equipdata = EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid);


      RareWeaponName.text = Inventory.GetTranslate(equipdata.Name);
      Inventory.Instance.ChangeItemRareColor(RareWeaponName, Inventory.Instance.data.Itemrare);

      //등급 버튼 설정
      string[] getrare = equipdata.RarePercent.Split(';');

      int minrare = int.Parse(Inventory.Instance.data.Itemrare); //자기레벨이 현재 레벨 실패 시 유짖 

      string maxrare = "0";

      for (int i = 0; i < RareText.Length; i++)
      {
         RareText[i].text = "";
         Rareobj[i].gameObject.SetActive(false);
      }

      bool ishavenext = false;
      for (int i = minrare + 1; i < RareText.Length; i++)
      {
         if (getrare[i] == "0") continue;
         ishavenext = true;
         RareText[i].text = $"{getrare[i]}%";
         Rareobj[i].gameObject.SetActive(true);
      }

      if (!ishavenext)
      {
         //최대치
         //최대레벨입니다
         RarePanel.Hide(true);
      }

      RareItemCount.text =
         $"{Inventory.GetTranslate("Itemname/53")} x {1} ({PlayerBackendData.Instance.CheckItemCount("53")})";
      LayoutRebuilder.ForceRebuildLayoutImmediate(RefreshItembars[0]);
   }

   int GetNeedESItem()
   {
      int num = 0;
      for (int i = 0; i < eskillnowpanel.Length; i++)
      {
         if (eskillnowpanel[i].LockEs.gameObject.activeSelf)
         {
            if (eskillnowpanel[i].LockEs.IsOn)
            {
               num++;
            }
         }
      }

      return num switch
      {
         0 => 1,
         1 => 2,
         2 => 4,
         _ => 1
      };
   }



   private bool isstart;
   private bool issucc = false;

   public void Bt_StartRareChanger()
   {
      if (isstart)
         return;
      if (PlayerBackendData.Instance.CheckItemCount("53") <= 0)
      {
         //없음
         Debug.Log("안됨");
         return;
      }

      if (!Settingmanager.Instance.CheckServerOn())
      {
         return;

         //다시 시도
      }

      isstart = true;
      RareEffect_Start.Play();
      issucc = Inventory.Instance.data.GetRareUp();

      Invoke(nameof(ShowResultRare), 0.7f);

      RefreshEquipEquipGear();
      Inventory.Instance.ShowInventoryItem(Inventory.Instance.data, true);
      PlayerBackendData.Instance.RemoveItem("53", 1);
      TutorialTotalManager.Instance.CheckGuideQuest("changerare");
      
      if (Inventory.Instance.istrue)
      {
         Where where = new Where();
         where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
         Param param = new Param { { "PlayerCanLoadBool", "false" } };
         Inventory.Instance.istrue = false;
         SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
         {
            if (callback.IsSuccess())
            {
               Settingmanager.Instance.RecentServerDataCanLoadText.text = Inventory.Instance.istrue
                  ? string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"),
                     Inventory.Instance.istrue
                        ? "<Color=lime>On</color>"
                        : "<Color=red>Off</color>")
                  : string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"), "<Color=red>Off</color>");
               LogManager.UserSmeltCheck("체크_등급");
            }
         });
      }
      
      Savemanager.Instance.SaveTypeEquip();
      Savemanager.Instance.SaveInventory();
      Savemanager.Instance.Save();
   }

   void ShowResultRare()
   {
      if (issucc)
      {
         RareShowEffect_Succ.Play();
      }
      else
      {
         RareShowEffect_Fail.Play();
      }

      ShowRarePanel();
   }


   //품질
   public UIView CraftPanel;
   public GameObject[] Craftobj;
   public Text CraftLevelText;
   public Text CraftItemCount;

   public ParticleSystem CraftEffect_Start;
   public ParticleSystem CraftShowEffect_Succ;
   public ParticleSystem CraftShowEffect_Fail;

   public void ShowCraftPanel()
   {
      isstart = false;
      issucc = false;
      CraftPanel.Show(false);

      CraftLevelText.text = Inventory.GetTranslate(
         $"CraftRare/{Inventory.Instance.data.CraftRare1}");
      //등급 버튼 설정




      for (int i = 0; i < Craftobj.Length; i++)
      {
         Craftobj[i].gameObject.SetActive(false);
      }

      for (int i = Inventory.Instance.data.CraftRare1 + 1; i < Craftobj.Length; i++)
      {
         Craftobj[i].gameObject.SetActive(true);
      }

      if (Inventory.Instance.data.CraftRare1 == 5)
      {
         //최대치
         //최대레벨입니다
         CraftPanel.Hide(true);
      }

      CraftItemCount.text =
         $"{Inventory.GetTranslate("Itemname/52")} x 1 ({PlayerBackendData.Instance.CheckItemCount("52")})";
      LayoutRebuilder.ForceRebuildLayoutImmediate(RefreshItembars[1]);

   }


   public void Bt_StartCraftChanger()
   {
      if (isstart)
         return;
      if (PlayerBackendData.Instance.CheckItemCount("52") <= 0)
      {
         //없음
         Debug.Log("안됨");
         return;
      }

      if (!Settingmanager.Instance.CheckServerOn())
      {
         //다시 시도
         return;
      }


      isstart = true;
      CraftEffect_Start.Play();
      issucc = Inventory.Instance.data.GetCraftTierUp();

      Invoke(nameof(ShowResultCraft), 0.7f);


      RefreshEquipEquipGear();
      Inventory.Instance.ShowInventoryItem(Inventory.Instance.data, true);
      PlayerBackendData.Instance.RemoveItem("52", 1);
      TutorialTotalManager.Instance.CheckGuideQuest("changecraft");

      if (Inventory.Instance.istrue)
      {
         Where where = new Where();
         where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
         Param param = new Param { { "PlayerCanLoadBool", "false" } };
         Inventory.Instance.istrue = false;
         SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
         {
            if (callback.IsSuccess())
            {
               Settingmanager.Instance.RecentServerDataCanLoadText.text = Inventory.Instance.istrue
                  ? string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"),
                     Inventory.Instance.istrue
                        ? "<Color=lime>On</color>"
                        : "<Color=red>Off</color>")
                  : string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"), "<Color=red>Off</color>");
               LogManager.UserSmeltCheck("체크_품질");
            }
         });
      }
      
      Savemanager.Instance.SaveTypeEquip();
      Savemanager.Instance.SaveInventory();
      Savemanager.Instance.Save();

   }

   void ShowResultCraft()
   {
      if (issucc)
      {
         CraftShowEffect_Succ.Play();
      }
      else
      {
         CraftShowEffect_Fail.Play();
      }

      ShowCraftPanel();
   }



   public GameObject FirstChangeButton;
   public GameObject AcceptButton;
   public GameObject RerollButton;

   public UIView Eskillpanel;
   public Image EskillItemImage;
   public Text EskillItemCount;
   public Eskillchanger[] eskillnowpanel;

   public Eskillchanger[] eskillchangepanel;

   public bool islessequipskill;
   
   
   //특수효과 변경
   public void ShowEskillPanel(bool isnew = false)
   {
      Eskillpanel.Show(false);
      FirstChangeButton.SetActive(true);
      AcceptButton.SetActive(false);
      RerollButton.SetActive(false);
      EquipBlind.SetActive(false);
      islessequipskill = false;
      for (int i = 0; i < eskillnowpanel.Length; i++)
      {
         eskillnowpanel[i].gameObject.SetActive(false);
         eskillchangepanel[i].gameObject.SetActive(false);
      }

      EquipDatabase data = Inventory.Instance.data;


      //고유효과 확인여부
      int num = EquipItemDB.Instance.Find_id(data.Itemid).SpeMehodP != "0" ? 1 : 0;

      if (data.EquipSkill1.Count - num == 0)
      {

         eskillnowpanel[0].NoData();
         eskillnowpanel[0].gameObject.SetActive(true);

         eskillchangepanel[0].gameObject.SetActive(true);
         eskillchangepanel[0].NoData();
      }
      else
      {
         for (int i = num; i < data.EquipSkill1.Count; i++)
         {
            eskillnowpanel[i - num].gameObject.SetActive(true);
            if (isnew)
            {
               eskillnowpanel[i - num].ShowDataNew(data.EquipSkill1[i]);

            }
            else
            {
               eskillnowpanel[i - num].ShowData(data.EquipSkill1[i]);
            }
         }

         for (int i = 0; i < data.EquipSkill1.Count - num; i++)
         {
            eskillchangepanel[i].gameObject.SetActive(true);
            eskillchangepanel[i].NoData();
         }
      }

      EskillItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id("54").sprite);
      EskillItemCount.text =
         $"{Inventory.GetTranslate("Itemname/54")} x {GetNeedESItem()} ({PlayerBackendData.Instance.CheckItemCount("54")})";
      LayoutRebuilder.ForceRebuildLayoutImmediate(RefreshItembars[2]);
      SavedOption = Inventory.Instance.data.EquipSkill1.ToArray();
      CheckLock();
   }

   //낡은
   public void ShowEskillLessPanel(bool isnew = false)
   {
      Eskillpanel.Show(false);
      FirstChangeButton.SetActive(true);
      AcceptButton.SetActive(false);
      EquipBlind.SetActive(false);

      RerollButton.SetActive(false);
      islessequipskill = true;

      for (int i = 0; i < eskillnowpanel.Length; i++)
      {
         eskillnowpanel[i].gameObject.SetActive(false);
         eskillchangepanel[i].gameObject.SetActive(false);
      }

      EquipDatabase data = Inventory.Instance.data;


      //고유효과 확인여부
      int num = EquipItemDB.Instance.Find_id(data.Itemid).SpeMehodP != "0" ? 1 : 0;

      if (data.EquipSkill1.Count - num == 0)
      {
         eskillnowpanel[0].NoData();
         eskillnowpanel[0].gameObject.SetActive(true);

         eskillchangepanel[0].gameObject.SetActive(true);
         eskillchangepanel[0].NoData();
      }
      else
      {
         for (int i = num; i < data.EquipSkill1.Count; i++)
         {
            eskillnowpanel[i - num].gameObject.SetActive(true);
            if (isnew)
            {
               eskillnowpanel[i - num].ShowDataNew(data.EquipSkill1[i]);

            }
            else
            {
               eskillnowpanel[i - num].ShowData(data.EquipSkill1[i]);
            }
         }

         for (int i = 0; i < data.EquipSkill1.Count - num; i++)
         {
            eskillchangepanel[i].gameObject.SetActive(true);
            eskillchangepanel[i].NoData();
         }
      }


      EskillItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id("57").sprite);
      EskillItemCount.text =
         $"{Inventory.GetTranslate("Itemname/57")} x {GetNeedESItem()} ({PlayerBackendData.Instance.CheckItemCount("57")})";
      LayoutRebuilder.ForceRebuildLayoutImmediate(RefreshItembars[2]);
      SavedOption = Inventory.Instance.data.EquipSkill1.ToArray();
      
      //Bt_RemoveAllLock();
      CheckLock();
   }

   public void Bt_ResetAllLock()
   {
      /*
      if (islessequipskill)
      {
         for (int i = 0; i < eskillnowpanel.Length; i++)
         {
            eskillnowpanel[i].LockEs.Interactable = false;
            eskillnowpanel[i].RemoveEquipSkillLock();
         }
      }
      else
      {
      */
         for (int i = 0; i < eskillnowpanel.Length; i++)
         {
            eskillnowpanel[i].LockEs.Interactable = true;
            eskillnowpanel[i].UnLockEquipSkill();
         }
      //}
     

      lockcountEs = 0;
   }
   public void Bt_RemoveAllLock()
   {
      for (int i = 0; i < eskillnowpanel.Length; i++)
      {
         eskillnowpanel[i].LockEs.Interactable = false;
         eskillnowpanel[i].RemoveEquipSkillLock();
      }

      lockcountEs = 0;
   }
   public void CheckLock()
   {

      int onnum = 0;
      for (int i = 0; i < eskillnowpanel.Length; i++)
      {
         if (eskillnowpanel[i].gameObject.activeSelf)
            onnum++;
      }

      if (onnum <= 0)
      {
         //안됨.
         for (int i = 0; i < eskillnowpanel.Length; i++)
         {
            eskillnowpanel[i].LockEs.gameObject.SetActive(false);
         }

         return;
      }
      else
      {
         for (int i = 0; i < eskillnowpanel.Length; i++)
         {
            eskillnowpanel[i].LockEs.gameObject.SetActive(true);
         }
      }

      //2개일때는 온된거빼고는 다 비활성화
      if (lockcountEs.Equals(islessequipskill ? 1:2))
      {
         for (int i = 0; i < eskillnowpanel.Length; i++)
         {
            if (!eskillnowpanel[i].LockEs.IsOn)
               eskillnowpanel[i].LockEs.Interactable = false;
         }
      }
      else
      {
         for (int i = 0; i < eskillnowpanel.Length; i++)
         {
            eskillnowpanel[i].LockEs.Interactable = true;
         }
      }
   }

   private string[] ChangedOption;
   private string[] SavedOption;

   public void Bt_StartEskillChange()
   {
      if (!Settingmanager.Instance.CheckServerOn())
      {
         //다시 시도
         return;

      }
      
      if (islessequipskill)
      {
         //늘어날 확률은 10퍼
         if (PlayerBackendData.Instance.CheckItemCount("57") < GetNeedESItem())
         {
            //없음
          //  Debug.Log("안됨");
            return;
         }
      }
      else
      {
         //늘어날 확률은 10퍼
         if (PlayerBackendData.Instance.CheckItemCount("54") < GetNeedESItem())
         {
            //없음
           // Debug.Log("안됨");
            return;
         }
      }
    

  

      for (int i = 0; i < eskillnowpanel.Length; i++)
      {
         eskillchangepanel[i].gameObject.SetActive(false);
      }

      isstart = true;
      ChangedOption = Inventory.Instance.data.GetESkill().ToArray();


      int num = EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid).SpeMehodP != "0" ? 1 : 0;

      for (int i = num; i < ChangedOption.Length; i++)
      {
         eskillchangepanel[i - num].gameObject.SetActive(true);
         eskillchangepanel[i - num].ShowDataNew(ChangedOption[i]);
      }
      EquipBlind.SetActive(true);
      AcceptButton.SetActive(true);
      RerollButton.SetActive(true);
      FirstChangeButton.SetActive(false);
      if (islessequipskill)
      {
         PlayerBackendData.Instance.RemoveItem("57", GetNeedESItem());
      }
      else
      {
         PlayerBackendData.Instance.RemoveItem("54", GetNeedESItem());
      }
      TutorialTotalManager.Instance.CheckGuideQuest("changeeskill");

      if (Inventory.Instance.istrue)
      {
         Where where = new Where();
         where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
         Param param = new Param { { "PlayerCanLoadBool", "false" } };
         Inventory.Instance.istrue = false;
         SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
         {
            if (callback.IsSuccess())
            {
               Settingmanager.Instance.RecentServerDataCanLoadText.text = Inventory.Instance.istrue
                  ? string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"),
                     Inventory.Instance.istrue
                        ? "<Color=lime>On</color>"
                        : "<Color=red>Off</color>")
                  : string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"), "<Color=red>Off</color>");
            }
         });
      }
      
      RefreshESSKILLCount();
      Savemanager.Instance.SaveInventory();
      Savemanager.Instance.Save();

   }

   //특수효과를 바꾼다.
   public void Bt_AcceptESkill()
   {
//     Debug.Log("체인지 스킬은 "+ ChangedOption.Length);
      Inventory.Instance.data.SetEquipSkills(ChangedOption);

      LogManager.EskillLog(Inventory.Instance.data);
      LogManager.Instance.CheckBug();
      Settingmanager.Instance.OnlyInvenSave();
      EquipBlind.SetActive(false);
      AcceptButton.SetActive(false);
      RerollButton.SetActive(false);
      FirstChangeButton.SetActive(true);
      if (islessequipskill)
      {
         ShowEskillLessPanel(true);
      }
      else
      {
         ShowEskillPanel(true);
      }
      LogManager.UserSmeltCheck("체크_특효");
      
      
      RefreshEquipEquipGear();
      Inventory.Instance.ShowInventoryItem(Inventory.Instance.data, true);
      Savemanager.Instance.SaveTypeEquip();
      //Bt_ResetAllLock();
      PlayerData.Instance.RefreshPlayerstat();
      
   }

   [SerializeField] private UIView cancelpanel;

   public void RefreshESSKILLCount()
   {
      EskillItemCount.text = islessequipskill ? $"{Inventory.GetTranslate("Itemname/57")} x {GetNeedESItem()} ({PlayerBackendData.Instance.CheckItemCount("57")})" : $"{Inventory.GetTranslate("Itemname/54")} x {GetNeedESItem()} ({PlayerBackendData.Instance.CheckItemCount("54")})";

      LayoutRebuilder.ForceRebuildLayoutImmediate(RefreshItembars[2]);
   }

   public void Bt_CheckCancelEskill()
   {
      //  Debug.Log("스,킬개수"+Inventory.Instance.data.EquipSkill1.Count);

      for (int i = 0; i < Inventory.Instance.data.EquipSkill1.Count; i++)
      {
//         Debug.Log("스,킬"+Inventory.Instance.data.EquipSkill1[i] + i);
      }

      try
      {
         if (ChangedOption.Length != 0)
         {
            cancelpanel.Show(false);
         }
         else
         {
            Eskillpanel.Hide(false);
         }
      }
      catch
      {
         Eskillpanel.Hide(false);
      }
   }

   public void CloseEsSkills()
   {
      for (int i = 0; i < eskillnowpanel.Length; i++)
      {
         eskillnowpanel[i].UnLockEquipSkill();
         eskillnowpanel[i].LockEs.Interactable = false;
      }

      lockcountEs = 0;
      Inventory.Instance.data.SetEquipSkills(SavedOption);
     
   }

   public void RefreshEquipEquipGear()
   {
      if (PlayerBackendData.Instance.GetEquipData()[Inventory.Instance.nowsettype] == null) return;

      if(!Inventory.Instance.ismines)
         return;
      //같은 아이템을 장착중이면 장착아이템 변경
      if (Inventory.Instance.data.IsEquip)
      {
         PlayerBackendData.Instance.GetEquipData()[Inventory.Instance.nowsettype] = Inventory.Instance.data;
         Inventory.Instance.EquipSlots[Inventory.Instance.nowsettype].SetItem(Inventory.Instance.data);
         switch (EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid).Type)
         {
            case "Weapon":
               PlayerData.Instance.SetWeaponImage(
                  SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid)
                     .EquipSprite), EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid).EquipSprite);
               PlayerData.Instance.SetMainWeaponRare(Inventory.Instance.data.CraftRare1);
               Inventory.Instance.mainplayer.InitAttackData(); //공격 횟수 설정

               switch (EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid).attacktype)
               {
                  case "0":
                     Inventory.Instance.mainplayer.attacktype = AttackType.Melee;
                     EquipItemDB.Row data =
                        EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid);
                     //히트사운드
                     if (data.HitSound != "")
                     {

                        Inventory.Instance.mainplayer.attacktrigger = data.HitSound.Split(';');
                     }

                     break;
                  case "1":
                     Inventory.Instance.mainplayer.attacktype = AttackType.Range;
                     Inventory.Instance.mainplayer.InitArrow();
                     break;

               }

               break;
            case "SWeapon":
               PlayerData.Instance.SetSubWeaponImage(
                  SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid)
                     .EquipSprite), EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid).EquipSprite);
               PlayerData.Instance.SetSubWeaponRare(Inventory.Instance.data.CraftRare1);
               break;
         }

         //Inventory.Instance.nowequipslot.Refresh(Inventory.Instance.data);
         EquipSetmanager.Instance.EquipSetItem();
         PlayerData.Instance.RefreshPlayerstat();
      }
   }




   public bool iscrystalSmelt;
   public UIView SmeltPanel;

   public ParticleSystem SmeltEffect_Start;
   public ParticleSystem SmeltShowEffect_Succ;
   public ParticleSystem SmeltShowEffect_Fail;
   public GameObject ToggleBlinder;
   public SmeltSlot[] SmeltSlots;

   public GameObject[] SmeltSlotsHave;

   //특수효과 변경
   public void ToggleIsCrystalSmelt(bool ison)
   {
      iscrystalSmelt = ison;
   }

   public void ShowSmeltPanel()
   {
      isstart = false;
      Panel.Hide(true);
      ToggleBlinder.SetActive(false);
      issucc = false;
      SmeltPanel.Show(false);

      if (Inventory.Instance.data.MaxStoneCount1.Equals(10))
      {
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/최대제련수치"), alertmanager.alertenum.일반);
         SmeltPanel.Hide(false);
         return;
      }

      for (int i = 0; i < SmeltSlots.Length; i++)
      {
         //전부끔
         SmeltSlots[i].SmeltShow(3);
         SmeltSlotsHave[i].SetActive(false);
      }

      for (int i = 0; i < Inventory.Instance.data.MaxStoneCount1; i++)
      {
         SmeltSlotsHave[i].SetActive(true);
         SmeltSlots[i].SmeltShow(Inventory.Instance.data.SmeltStatCount1[i]);
      }
   }

   public const int CashCount = 1000;

   public void Bt_StartSmeltUp()
   {
      if (isstart)
         return;

      if (!Settingmanager.Instance.CheckServerOn())
      {
         return;

         //다시 시도
      }

      isstart = true;


      if (iscrystalSmelt)
      {
         if (PlayerBackendData.Instance.GetCash() < CashCount)
         {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/불꽃부족"), alertmanager.alertenum.주의);
            return;
         }

         //크리사용
         issucc = true;
         PlayerData.Instance.DownCash(1000);
         Savemanager.Instance.SaveCash();
      }
      else
      {
         if (PlayerBackendData.Instance.GetMoney() < 20000000)
         {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/골드부족"), alertmanager.alertenum.주의);
            return;
         }

         Random.InitState(PlayerBackendData.Instance.GetRandomSeed() + (int)Time.deltaTime);
         issucc = Random.Range(1, 101) <= 15 ? true : false;
         PlayerData.Instance.DownGold(20000000);
         Savemanager.Instance.SaveMoneyData();
      }


      if (issucc)
      {
         Inventory.Instance.data.MaxStoneCount1++;
         if (Inventory.Instance.data.MaxStoneCount1 > 10)
            Inventory.Instance.data.MaxStoneCount1 = 10;
         int[] tempstats = Inventory.Instance.data.SmeltStatCount1;
         Inventory.Instance.data.SmeltStatCount1 = new int[Inventory.Instance.data.MaxStoneCount1];
         for (int i = 0; i < tempstats.Length; i++)
         {
            Inventory.Instance.data.SmeltStatCount1[i] = tempstats[i];
         }


      }

      SmeltEffect_Start.Play();

      ToggleBlinder.SetActive(true);
      
      Invoke(nameof(ShowResultSmelt), 0.7f);
      Savemanager.Instance.SaveTypeEquip();
      Savemanager.Instance.SaveMoneyCashDirect();
      Inventory.Instance.ShowInventoryItem(Inventory.Instance.data, true);
      RefreshEquipEquipGear();

   }

   void ShowResultSmelt()
   {
      if (issucc)
      {
         SmeltShowEffect_Succ.Play();
      }
      else
      {
         SmeltShowEffect_Fail.Play();
      }

      isstart = false;

      ShowSmeltPanel();
   }

   public void SaveEquip()
   {
      Savemanager.Instance.SaveEquip();
   }

   public void Bt_SaveServerData()
   {
      // Savemanager.Instance.SaveEquip();

      PlayerBackendData userData = PlayerBackendData.Instance;
      Param paramEquip = new Param();
      //셋팅필수
      paramEquip.Add("EquipmentNow", userData.EquipEquiptment0);
      paramEquip.Add("Gold", userData.GetMoney());
      paramEquip.Add("Crystal", userData.GetCash());
      paramEquip.Add("inventory", userData.ItemInventory);

      switch (Inventory.Instance.nowsettype)
      {
         case 0:
            paramEquip.Add("equipment_Weapon", userData.Equiptment0);
            break;
         case 1:
            paramEquip.Add("equipment_SubWeapon", userData.Equiptment1);

            break;
         case 2:
            paramEquip.Add("equipment_Helmet", userData.Equiptment2);

            break;
         case 3:
            paramEquip.Add("equipment_Armor", userData.Equiptment3);

            break;
         case 4:
            paramEquip.Add("equipment_Glove", userData.Equiptment4);

            break;
         case 5:
            paramEquip.Add("equipment_Boot", userData.Equiptment5);

            break;
         case 6:
            paramEquip.Add("equipment_Ring", userData.Equiptment6);

            break;
         case 7:
            paramEquip.Add("equipment_Necklace", userData.Equiptment7);
            break;
         case 8:
            paramEquip.Add("equipment_Wing", userData.Equiptment8);
            break;
         case 9:
            paramEquip.Add("equipment_Pet", userData.Equiptment9);

            break;
         case 10:
            paramEquip.Add("equipment_Rune", userData.Equiptment10);

            break;
         case 11:
            paramEquip.Add("equipment_Insignia", userData.Equiptment11);

            break;
      }

      Where where = new Where();
      where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

      SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramEquip, (callback) =>
      {
         // 이후 처리
         if (!callback.IsSuccess()) return;

      });
   }


   //장비 승급

   public UIView Advanpanel;

   public Image AdvanEquipPrevImage;
   public Text AdvanEquipPrevText;

   public Image AdvanEquipImage;
   public Text AdvanEquipText;

   public Image AdvanNeedImage;
   public Text AdvanNeedNameText;
   public Text AdvanNeedGoldText;
   public Text AdvanPercentText;

   public ParticleSystem AdvanEffect_Start;
   public ParticleSystem AdvanShowEffect_Succ;
   public ParticleSystem AdvanShowEffect_Succ2;
   public ParticleSystem AdvanShowEffect_Fail;

   private EquipItemDB.Row Data;
   private EquipItemDB.Row AdvanData;
   private ItemdatabasecsvDB.Row itemdata;


   private bool isstart_advan;
   private bool issucc_advan = false;

   public void Bt_ShowAdvanPanel()
   {
      Advanpanel.Show(false);

      //승급 장비 데이터
      Data = EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid);
      AdvanData = EquipItemDB.Instance.Find_id(
         EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid).AdvanEquipID);

      //승급장비
      AdvanEquipPrevImage.sprite = SpriteManager.Instance.GetSprite(Data.Sprite);
      AdvanEquipPrevText.text = Inventory.GetTranslate(Data.Name);
      Inventory.Instance.ChangeItemRareColor(AdvanEquipPrevText, Inventory.Instance.data.Itemrare);

      AdvanEquipImage.sprite = SpriteManager.Instance.GetSprite(AdvanData.Sprite);
      AdvanEquipText.text = Inventory.GetTranslate(AdvanData.Name);
      Inventory.Instance.ChangeItemRareColor(AdvanEquipText, Inventory.Instance.data.Itemrare);


      itemdata = ItemdatabasecsvDB.Instance.Find_id(Data.AdvanNeedItem);
      //재료
      AdvanNeedImage.sprite = SpriteManager.Instance.GetSprite(itemdata.sprite);
      AdvanNeedNameText.text = $"{Inventory.GetTranslate(itemdata.name)} X {Data.AdvanNeedItemHowmany} ({PlayerBackendData.Instance.CheckItemCount(itemdata.id)})";
      Inventory.Instance.ChangeItemRareColor(AdvanNeedNameText, itemdata.rare);

      AdvanNeedGoldText.text = $"{Data.AdvanGold} Gold";
      AdvanPercentText.text = string.Format(Inventory.GetTranslate("UI3/장비승급확률"), Data.AdvanPercent);
   }

   public void Bt_StartAdvan()
   {
      //재료랑 이게 있는가
      if (PlayerBackendData.Instance.CheckItemCount(itemdata.id) < int.Parse(Data.AdvanNeedItemHowmany))
      {
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/아이템부족"), alertmanager.alertenum.일반);
         //아이템이 없다
         return;
      }

      if (PlayerBackendData.Instance.GetMoney() < decimal.Parse(Data.AdvanGold))
      {
         //골드가 없다
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/골드부족"), alertmanager.alertenum.일반);
         return;
      }

      if (!Settingmanager.Instance.CheckServerOn())
      {
         return;
      }
      
      Advanpanel.Hide(true);
         
      //가능하다.
      PlayerData.Instance.DownGold(Decimal.Parse(Data.AdvanGold));
      PlayerBackendData.Instance.RemoveItem(itemdata.id, int.Parse(Data.AdvanNeedItemHowmany));

      Random.InitState(PlayerBackendData.Instance.GetRandomSeed() + (int)Time.deltaTime);
      issucc = Random.Range(1, 101) <= int.Parse(Data.AdvanPercent);

      if (issucc)
      {
         PlayerBackendData.Instance.GetTypeEquipment(Data.Type)[Inventory.Instance.data.KeyId1].Itemid = AdvanData.id;

         if (EquipItemDB.Instance
                .Find_id(PlayerBackendData.Instance.GetTypeEquipment(Data.Type)[Inventory.Instance.data.KeyId1].Itemid)
                .SpeMehodP != "0")
         {
            PlayerBackendData.Instance.GetTypeEquipment(Data.Type)[Inventory.Instance.data.KeyId1].EquipSkill1[0] =
               EquipItemDB.Instance.Find_id(AdvanData.id).SpeMehodP;
         }
      
         
         string Prevkeyname = PlayerBackendData.Instance.GetTypeEquipment(Data.Type)[Inventory.Instance.data.KeyId1].KeyId1;
         string Nextkeyname = PlayerBackendData.Instance.GetTypeEquipment(Data.Type)[Inventory.Instance.data.KeyId1].KeyId1.Replace(Data.id,AdvanData.id);
         PlayerBackendData.Instance.GetTypeEquipment(Data.Type)[Inventory.Instance.data.KeyId1].KeyId1 = Nextkeyname;
        RenameKey(PlayerBackendData.Instance.GetTypeEquipment(Data.Type), Prevkeyname, Nextkeyname);
         Inventory.Instance.data =
            PlayerBackendData.Instance.GetTypeEquipment(Data.Type)[Inventory.Instance.data.KeyId1];
         Inventory.Instance.ShowInventoryItem(Inventory.Instance.data, true);
         RefreshEquipEquipGear();
         if (Inventory.Instance.data.IsEquip)
         {
            PlayerBackendData.Instance.GetEquipData()[Inventory.Instance.nowsettype] = Inventory.Instance.data;
            PlayerData.Instance.RefreshPlayerstat();
         }
      }

      if (Inventory.Instance.istrue)
      {
         Where where = new Where();
         where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
         Param param = new Param { { "PlayerCanLoadBool", "false" } };
         Inventory.Instance.istrue = false;
         SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
         {
            if (callback.IsSuccess())
            {
               Settingmanager.Instance.RecentServerDataCanLoadText.text = Inventory.Instance.istrue
                  ? string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"),
                     Inventory.Instance.istrue
                        ? "<Color=lime>On</color>"
                        : "<Color=red>Off</color>")
                  : string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"), "<Color=red>Off</color>");
            }
         });
      }
      TutorialTotalManager.Instance.CheckGuideQuest("upgradeequip");
      StartCoroutine(Result_advan(issucc));

      Savemanager.Instance.SaveInventory();
      Savemanager.Instance.SaveMoneyData();
      Savemanager.Instance.SaveTypeEquip();
      Savemanager.Instance.SaveEquip();
      Savemanager.Instance.Save();
      Bt_SaveServerData();
      
   }


   public UIView AdvanResultPanel; //패널
   public Image EquipImage_advanresult; //장비 이미지
   public Text EquipNameText_advanresult; //이름
   public Text EquipResult_advanresult; //성공 or 실패를 보여줌


   // ReSharper disable Unity.PerformanceAnalysis
   IEnumerator Result_advan(bool issucc)
   {
      AdvanResultPanel.Show(false);
      EquipImage_advanresult.sprite = AdvanEquipPrevImage.sprite;
      if (issucc)
      {
         EquipNameText_advanresult.text = AdvanEquipText.text;
         Inventory.Instance.ChangeItemRareColor(EquipNameText_advanresult,Inventory.Instance.data.Itemrare);
         EquipResult_advanresult.text = Inventory.GetTranslate("UI3/장비승급성공");
         EquipResult_advanresult.color = Color.cyan;
      }
      else
      {
         EquipNameText_advanresult.text = AdvanEquipPrevText.text;
         EquipNameText_advanresult.color = AdvanEquipPrevText.color;
         Inventory.Instance.ChangeItemRareColor(EquipNameText_advanresult,Inventory.Instance.data.Itemrare);
         EquipResult_advanresult.text = Inventory.GetTranslate("UI3/장비승급실패");
         EquipResult_advanresult.color = Color.red;
      }

      yield return SpriteManager.Instance.GetWaitforSecond(0.8f);
      AdvanEffect_Start.Play();
      Soundmanager.Instance.PlayerSound("Sound/강화확인전");

      yield return SpriteManager.Instance.GetWaitforSecond(1.2f);

      if (issucc)
      {
         AdvanShowEffect_Succ.Play();
         AdvanShowEffect_Succ2.Play();
         Soundmanager.Instance.PlayerSound("Sound/강화성공");
      }
      else
      {
         AdvanShowEffect_Fail.Play();
         Soundmanager.Instance.PlayerSound("Sound/강화실패");
      }
   }

   public static void RenameKey<TKey, TValue>(System.Collections.Generic.IDictionary<TKey, TValue> dic, TKey fromKey, TKey toKey)
   {
      TValue value = dic[fromKey];
      dic.Remove(fromKey);
      dic[toKey] = value;
   }
   public void Bt_UpgradeEquip()
   {
      EquipUpgradeManager.Instance.ShowEqupData();
   }
}
