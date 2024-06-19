using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using BackEnd;
using Doozy.Engine.UI;
using JetBrains.Annotations;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class otherusermanager : MonoBehaviour
{
   //�̱��游���.
   private static otherusermanager _instance = null;

   public static otherusermanager Instance
   {
      get
      {
         if (_instance == null)
         {
            _instance = FindObjectOfType(typeof(otherusermanager)) as otherusermanager;
            if (_instance == null)
            {
               //Debug.Log("Player script Error");
            }
         }
         return _instance;
      }
   }
   public UIView Panel;

   public GameObject PlayerUserPanel;

   //UI
   public Image PlayerAvarta;
   public Image MainWeapon;
   public Image SubWeapon;
   public Image PetImage;

   public Text NameandJobText;
   public Text ClassText;
   public Text GuildText;
   public Text AdventureRankText;
   public Text LastStage;
   public Text lastlogintext;
   public Text battlePointtext;

   [SerializeField] private equipitemslot[] Equipslots;
   [SerializeField] private Skillslot[] Skillslots;

   //ĳ���� �ɷ�ġ
   public Text[] PlayerStat;
   //�нú�
   public Text[] Passivename;
   public Text[] Passiveinfo;


   private string userindate;
   private string guildName;
   private string lastlogin;
   public GameObject Loading;

   public string test;

   public abilityinfoslot[] AbilitySlots;

   public void Bt_ShowmytrainingData()
   {
       uimanager.Instance.AddUiview(dpsmanager.Instance.Trainingobj,true);
       ShowPlayerData_Training(PlayerBackendData.Instance.nickname);
   }
   
   public void ShowPlayerData(string nickname)
   {
       //Debug.Log("�г���" + nickname);
      Panel.Show(false);
      Loading.SetActive(true);
      PlayerUserPanel.SetActive(false);
      PlayerInfoToggles[2].gameObject.SetActive(false);
      PlayerInfoToggles[0].ToggleOn();
      PlayerInfoToggles[0].ExecuteClick();

      foreach (var VARIABLE in PlayerInfoObj)
      {
          VARIABLE.SetActive(false);
      }
      PlayerInfoObj[0].SetActive(true);
      
      
      userindate = null;

      SendQueue.Enqueue(Backend.Social.GetUserInfoByNickName, nickname, (callback) =>
      {
         if (!callback.IsSuccess()) return;
         //Debug.Log(nickname);
         //Debug.Log(callback.GetReturnValuetoJSON()["row"]["inDate"].ToString());
         userindate = callback.GetReturnValuetoJSON()["row"]["inDate"].ToString();
         if (callback.GetReturnValuetoJSON()["row"]["guildName"] != null) 
         {
            guildName = callback.GetReturnValuetoJSON()["row"]["guildName"].ToString();
            lastlogin = callback.GetReturnValuetoJSON()["row"]["lastLogin"].ToString();
         }

         
         for (int i = 0; i < AbilitySlots.Length; i++)
         {
             AbilitySlots[i].NoRefresh();
         }
         
         
         Where where = new Where();
         where.Equal("owner_inDate", userindate);

         string[] select =
         {
            "level",
            "level_adven",
            "NowClass",
            "EquipmentNow",
            "Passive",
            "Playerstat",
            "NowMap",
            "updatedAt",
            "Ability",
            "avata_weapon",
            "avata_subweapon",
            "avata_avata",
            "nowPetid",
            "nowPetData",
            
         };

         SendQueue.Enqueue(Backend.GameData.Get, "PlayerData", where, select, 1, bro =>
         {
            if (bro.IsSuccess() == false)
            {
               // ��û ���� ó��
               //Debug.Log(bro);
               return;
            }

            if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
            {
             
               return;
            }

       

            
            Loading.SetActive(false);
            PlayerUserPanel.SetActive(true);
           // Debug.Log(bro);

            //������ �Է�
            JsonData data = bro.FlattenRows()[0];
            //Bro.Rows()[i]["inDate"]["S"].ToString();
            
            DateTime nowtime = Timemanager.Instance.GetServerTime();
            DateTime dateTime = DateTime.Parse(data["updatedAt"].ToString());

            if (nowtime.Date == dateTime.Date)
            {
               // Debug.Log("������ ����");
                lastlogintext.text = Inventory.GetTranslate("UI2/마지막접속시간오늘");
            }
            else
            {
               // Debug.Log("������ �ٸ���");
                lastlogintext.text = 
                    string.Format(Inventory.GetTranslate("UI2/마지막접속시간"),
                        (nowtime - dateTime).Days);
            }
            
            
            //�̸��� ����
            NameandJobText.text =
               $"Lv.{data["level"].ToString()} {nickname}";
            ClassText.text =
               Inventory.GetTranslate(ClassDB.Instance.Find_id(data["NowClass"]["ClassId1"].ToString()).name);
            Inventory.Instance.ChangeItemRareColor(ClassText,ClassDB.Instance.Find_id(data["NowClass"]["ClassId1"].ToString()).tier);

            AdventureRankText.text = PlayerData.Instance.gettierstarText("UI2/모험랭크",data["level_adven"].ToString());
            GuildText.text = guildName == null ? "" : $"[{guildName}]";
            LastStage.text = string.Format(Inventory.GetTranslate("UI2/마지막사냥터"),
                Inventory.GetTranslate(MapDB.Instance.Find_id(data["NowMap"].ToString()).name));

       
            
            
            #region �������
            //��� ����
            EquipDatabase[] PlayerEquip = new EquipDatabase[15];


            for (int i = 0; i < PlayerBackendData.Instance.EquipEquiptment0.Length; i++)
            {
               try
               {
                  PlayerEquip[i] = new EquipDatabase(data["EquipmentNow"][i]);
//                   Debug.Log("��� �ִ´�" + i);
               }
               catch
               {
                    
               }
            }


            battlePointtext.text = PlayerData.Instance.GetEquipPoint(PlayerEquip).ToString("N0");
            
            for (int i = 0; i < Equipslots.Length; i++)
            {
               if (PlayerEquip[i] != null)
                  Equipslots[i].SetItem(PlayerEquip[i], false);
            }


            for (int i = 0; i < Equipslots.Length; i++)
            {
                if (PlayerEquip[i] != null)
                {
                    Equipslots[i].SetItem(PlayerEquip[i], false);
                   // CheckSetItem(PlayerEquip, PlayerEquip[i].Itemid, true);
                }
                else
                {
                    Equipslots[i].SetItem(); //�������� ǥ��         
                }
            }
            PlayerData.Instance.SetAvartaImage(PlayerAvarta,
               SpriteManager.Instance.GetSprite(ClassDB.Instance.Find_id(data["NowClass"]["ClassId1"].ToString())
                  .classsprite));

            if (PlayerEquip[0] != null)
            {
               PlayerData.Instance.SetWeaponImage(MainWeapon,
                  SpriteManager.Instance.GetSprite(EquipItemDB.Instance
                     .Find_id(PlayerEquip[0].Itemid).EquipSprite));
               PlayerData.Instance.SetWeaponRareOther(MainWeapon.material, PlayerEquip[0].CraftRare1);
            }
            else
               PlayerData.Instance.SetWeaponImage_remove_Other(MainWeapon);

            if (PlayerEquip[1] != null)
            {
               PlayerData.Instance.SetSubWeaponImage(SubWeapon,
                  SpriteManager.Instance.GetSprite(EquipItemDB.Instance
                     .Find_id(PlayerEquip[1].Itemid).EquipSprite));
               PlayerData.Instance.SetWeaponRareOther(SubWeapon.material, PlayerEquip[1].CraftRare1);
            }
            else
               PlayerData.Instance.SetSubWeaponImage_remove(SubWeapon);

            #endregion

            #region  ��

            if (data.ContainsKey("nowPetData"))
            {

                if (data["nowPetData"]["Petid"].ToString() != "")
                {
                    PetImage.sprite =
                        SpriteManager.Instance.GetSprite(PetDB.Instance.Find_id(data["nowPetData"]["Petid"].ToString()).sprite);
                    PetImage.enabled = true;
                    PlayerData.Instance.SetPetStarOther(PetImage.material,
                       int.Parse(data["nowPetData"]["Petstar"].ToString()));
                }
                else
                {
                    PetImage.enabled = false;
                }
            }
            else
            {
                if (data.ContainsKey("nowPetid"))
                {
                    if (data["nowPetid"].ToString() != "")
                    {
                        PetImage.sprite =
                            SpriteManager.Instance.GetSprite(PetDB.Instance.Find_id(data["nowPetid"].ToString()).sprite);
                        PetImage.enabled = true;
                     //   PlayerData.Instance.SetPetStarOther(PetImage.material,PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar);
                    }
                    else
                    {
                        PetImage.enabled = false;
                    }
                }   
                else
                {
                    PetImage.enabled = false;
                }
            }
          

            #endregion
            #region ��ų

            //��ų.
            foreach (var t in Skillslots)
            {
               t.islock = true;
               t.RefreshSkillOtherUser();
            }

            int classskillslotcount =
               int.Parse(ClassDB.Instance.Find_id(data["NowClass"]["ClassId1"].ToString()).skillslotcount);

            for (int i = 0;
                 i < classskillslotcount +
                 PlayerData.Instance.mainplayer.AdventureRankSkillSlot(int.Parse(data["level_adven"].ToString()));
                 i++)
            {
               Skillslots[i].islock = false;
            }

            for (int i = 0; i < data["NowClass"]["Skills1"].Count; i++)
            {
               if (data["NowClass"]["Skills1"][i] == null) continue;
               if (data["NowClass"]["Skills1"][i].ToString().Equals("")) continue;
               if (data["NowClass"]["Skills1"][i].ToString().Equals("NULL")) continue;
               if (data["NowClass"]["Skills1"][i].ToString().Equals("True")) continue;
               Skillslots[i].skillid = data["NowClass"]["Skills1"][i].ToString();
               Skillslots[i].RefreshSkillOtherUser();
            }

            #endregion
            #region �ɷ�ġ
            foreach (var t in PlayerStat)
            {
                t.text = "-";
            }
            for (int i = 0; i < 17; i++)
            {
                decimal da = decimal.Parse(data["Playerstat"][i].ToString());
                PlayerStat[i].text = i is 2 or 10 or 11 or 12 or 13 or 14 or 15 or 16 ? $"{da:N0}%" : $"{da:N0}";
            }
            
       
            decimal str = decimal.Parse(data["Playerstat"][5].ToString());
            decimal dex= decimal.Parse(data["Playerstat"][6].ToString());
            decimal ints= decimal.Parse(data["Playerstat"][7].ToString());
            decimal wis= decimal.Parse(data["Playerstat"][8].ToString());

            if (str + dex > ints + wis)
            {
                DamagerTypeObj[0].SetActive(true);
                DamagerTypeObj[1].SetActive(false);
                DamagerTypeObj[2].SetActive(false);
            }
            else if(ints > wis)
            {
                DamagerTypeObj[0].SetActive(false);
                DamagerTypeObj[1].SetActive(true);
                DamagerTypeObj[2].SetActive(false);
            }
            else
            {
                DamagerTypeObj[0].SetActive(false);
                DamagerTypeObj[1].SetActive(false);
                DamagerTypeObj[2].SetActive(true);
            }
            
            #endregion
            #region �нú�

            for (int i = 0; i < data["Passive"].Count; i++)
            {
//                Debug.Log(data["Passive"][i].ToString());
                if (data["Passive"][i].ToString() == "" || data["Passive"][i].ToString() == "True")
                {
                    Passivename[i].text = "-";
                    Passivename[i].color = Color.white;
                    Passiveinfo[i].text = "-";
                }
                else
                {
                    Passivename[i].text =
                        Inventory.GetTranslate(PassiveDB.Instance.Find_id(data["Passive"][i].ToString()).name);
                    Inventory.Instance.ChangeItemRareColor(Passivename[i],
                        ClassDB.Instance.Find_id(data["Passive"][i].ToString()).tier);
                    Passiveinfo[i].text =
                        Inventory.GetTranslate(PassiveDB.Instance.Find_id(data["Passive"][i].ToString()).info);
                }
            }
            
           
            
            #endregion
            #region �����Ƽ
            
            if (data.ContainsKey("Ability"))
            {
    //            Debug.Log(data["Ability"].Count);

                for (int i = 0; i < data["Ability"].Count; i++)
                {
                    if (data["Ability"][i].ToString() == "" || data["Ability"][i].ToString() == "True")
                    {
                        AbilitySlots[i].NoRefresh();
                    }
                    else
                    {
                        AbilitySlots[i].Refresh(data["Ability"][i].ToString());
                    }
                }
            }
            else
            {
                for (int i = 0; i < PlayerBackendData.Instance.Abilitys.Length; i++)
                {
                    AbilitySlots[i].NoRefresh();
                }
            }
            #endregion
            #region  ����
            if (data.ContainsKey("avata_avata"))
            {
                 
                    if (data["avata_weapon"].ToString() != "")
                    {
                        PlayerData.Instance.SetAvartaImage(MainWeapon,
                            SpriteManager.Instance.GetSprite(AvartaDB.Instance.Find_id(data["avata_weapon"].ToString())
                                .sprite));
                    }
                    if (data["avata_subweapon"].ToString() != "")
                    {
                        PlayerData.Instance.SetAvartaImage(SubWeapon,
                            SpriteManager.Instance.GetSprite(AvartaDB.Instance.Find_id(data["avata_subweapon"].ToString())
                                .sprite));
                    }
                    if (data["avata_avata"].ToString() != "")
                    {
                        PlayerData.Instance.SetAvartaImage(PlayerAvarta,
                            SpriteManager.Instance.GetSprite(AvartaDB.Instance.Find_id(data["avata_avata"].ToString())
                                .sprite));
                    }
    
                    
                    if (bool.Parse(AvartaDB.Instance.Find_id(data["avata_avata"].ToString()).iswhole))
                    {
                        SubWeapon.enabled = false;
                        MainWeapon.enabled = false;
                    }
                    else
                    {
                        SubWeapon.enabled = true;
                        MainWeapon.enabled = true;
                    }
            }
            #endregion
         });
      });
   }

   public GameObject[] DamagerTypeObj;
   [SerializeField] private dpsslot[] DPSSlots;
   public Text TotalDmg;
   public Text DPSDmg;
   public Text TotalTime;
   public Dictionary<string, DPS> Dps_Train = new Dictionary<string, DPS>();
   public UIToggle[] PlayerInfoToggles;
   public GameObject[] PlayerInfoObj;


   public void ShowPlayerData_Training(string nickname)
   {
       Dps_Train.Clear();
       PlayerInfoToggles[2].gameObject.SetActive(true);
       PlayerInfoToggles[2].ToggleOn();
       PlayerInfoToggles[2].ExecuteClick();

       foreach (var VARIABLE in PlayerInfoObj)
       {
           VARIABLE.SetActive(false);
       }

       PlayerInfoObj[0].SetActive(true);

       Panel.Show(false);
       Loading.SetActive(true);
       PlayerUserPanel.SetActive(false);

       userindate = null;

       SendQueue.Enqueue(Backend.Social.GetUserInfoByNickName, nickname, (callback) =>
       {
           if (!callback.IsSuccess()) return;
           //Debug.Log(nickname);
           //Debug.Log(callback.GetReturnValuetoJSON()["row"]["inDate"].ToString());
           userindate = callback.GetReturnValuetoJSON()["row"]["inDate"].ToString();
           if (callback.GetReturnValuetoJSON()["row"]["guildName"] != null) // ��� ������ ��ϵǾ� ���� ���� ��� null�� ��ȯ
           {
               //����̸��� �̸� �޾ƿ�
               guildName = callback.GetReturnValuetoJSON()["row"]["guildName"].ToString();
               lastlogin = callback.GetReturnValuetoJSON()["row"]["lastLogin"].ToString();
           }

           Where where = new Where();
           where.Equal("owner_inDate", userindate);

           string[] select =
           {
               "level_train",
               "level_adven_train",
               "NowClass_train",
               "EquipmentNow_train",
               "Passive_train",
               "Playerstat_train",
               "NowMap",
               "updatedAt",
               "TrainDmg",
               "TrainTime",
               "BreakTime",
               "TrainTotalDmg",
               "DPSDmgText",
               "TrainAbility",
               "avata_weapon",
               "avata_subweapon",
               "avata_avata",
               "nowPetid_train",
               "nowPetData_train",
           };

           SendQueue.Enqueue(Backend.GameData.Get, "PlayerData", where, select, 1, bro =>
           {
               if (bro.IsSuccess() == false)
               {
                   // ��û ���� ó��
                   //Debug.Log(bro);
                   return;
               }

               if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
               {
             
                   return;
               }



               Loading.SetActive(false);
               PlayerUserPanel.SetActive(true);
               // Debug.Log(bro);

               //������ �Է�
               JsonData data = bro.FlattenRows()[0];
               //Bro.Rows()[i]["inDate"]["S"].ToString();

               if (!data.ContainsKey("NowClass_train"))
               {
             //      alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/����ƺ����;���"), alertmanager.alertenum.�Ϲ�);
                   Panel.Hide(true);
                   return;
               }


               DateTime nowtime = Timemanager.Instance.GetServerTime();
               DateTime dateTime = DateTime.Parse(data["updatedAt"].ToString());

               if (nowtime.Date == dateTime.Date)
               {
                   // Debug.Log("������ ����");
                   lastlogintext.text = Inventory.GetTranslate("UI2/마지막접속시간오늘");
               }
               else
               {
                   // Debug.Log("������ �ٸ���");
                   lastlogintext.text =
                       string.Format(Inventory.GetTranslate("UI2/마지막접속시간"),
                           (nowtime - dateTime).Days);
               }


               //�̸��� ����
               NameandJobText.text =
                   $"Lv.{data["level_train"].ToString()} {nickname}";
               ClassText.text =
                   Inventory.GetTranslate(ClassDB.Instance.Find_id(data["NowClass_train"]["ClassId1"].ToString()).name);
               Inventory.Instance.ChangeItemRareColor(ClassText,
                   ClassDB.Instance.Find_id(data["NowClass_train"]["ClassId1"].ToString()).tier);

               AdventureRankText.text =
                   PlayerData.Instance.gettierstarText("UI2/모험랭크", data["level_adven_train"].ToString());
               GuildText.text = guildName == null ? "" : $"[{guildName}]";
               LastStage.text = string.Format(Inventory.GetTranslate("UI2/마지막사냥터"),
                   Inventory.GetTranslate(MapDB.Instance.Find_id(data["NowMap"].ToString()).name));




               #region �������

               //��� ����
               EquipDatabase[] PlayerEquip = new EquipDatabase[15];


               for (int i = 0; i < PlayerBackendData.Instance.EquipEquiptment0.Length; i++)
               {
                   try
                   {
                       PlayerEquip[i] = new EquipDatabase(data["EquipmentNow_train"][i]);
                       // Debug.Log("��� �ִ´�" + i);
                   }
                   catch
                   {

                   }
               }


               battlePointtext.text = PlayerData.Instance.GetEquipPoint(PlayerEquip).ToString("N0");

               for (int i = 0; i < Equipslots.Length; i++)
               {
                   if (PlayerEquip[i] != null)
                       Equipslots[i].SetItem(PlayerEquip[i], false);
               }


               for (int i = 0; i < Equipslots.Length; i++)
               {
                   if (PlayerEquip[i] != null)
                   {
                       Equipslots[i].SetItem(PlayerEquip[i], false);
                       // CheckSetItem(PlayerEquip, PlayerEquip[i].Itemid, true);
                   }
                   else
                   {
                       Equipslots[i].SetItem(); //�������� ǥ��         
                   }
               }


               PlayerData.Instance.SetAvartaImage(PlayerAvarta,
                   SpriteManager.Instance.GetSprite(ClassDB.Instance
                       .Find_id(data["NowClass_train"]["ClassId1"].ToString())
                       .classsprite));

               if (PlayerEquip[0] != null)
               {
                   PlayerData.Instance.SetWeaponImage(MainWeapon,
                       SpriteManager.Instance.GetSprite(EquipItemDB.Instance
                           .Find_id(PlayerEquip[0].Itemid).EquipSprite));
                   PlayerData.Instance.SetWeaponRareOther(MainWeapon.material, PlayerEquip[0].CraftRare1);
               }
               else
                   PlayerData.Instance.SetWeaponImage_remove_Other(MainWeapon);

               if (PlayerEquip[1] != null)
               {
                   PlayerData.Instance.SetSubWeaponImage(SubWeapon,
                       SpriteManager.Instance.GetSprite(EquipItemDB.Instance
                           .Find_id(PlayerEquip[1].Itemid).EquipSprite));
                   PlayerData.Instance.SetWeaponRareOther(SubWeapon.material, PlayerEquip[1].CraftRare1);
               }
               else
                   PlayerData.Instance.SetSubWeaponImage_remove(SubWeapon);

               
               
               #endregion

               #region ��ų

               //��ų.
               foreach (var t in Skillslots)
               {
                   t.islock = true;
                   t.RefreshSkillOtherUser();
               }

               int classskillslotcount =
                   int.Parse(ClassDB.Instance.Find_id(data["NowClass_train"]["ClassId1"].ToString()).skillslotcount);

               for (int i = 0;
                    i < classskillslotcount +
                    PlayerData.Instance.mainplayer.AdventureRankSkillSlot(
                        int.Parse(data["level_adven_train"].ToString()));
                    i++)
               {
                   Skillslots[i].islock = false;
               }

               for (int i = 0; i < data["NowClass_train"]["Skills1"].Count; i++)
               {
                   // Debug.Log("�����ʹ�" + data["NowClass"]["Skills1"][i]);
                   if (data["NowClass_train"]["Skills1"][i] == null ||
                       data["NowClass_train"]["Skills1"][i].ToString() == "") continue;
                   Skillslots[i].skillid = data["NowClass_train"]["Skills1"][i].ToString();
                   Skillslots[i].RefreshSkillOtherUser();
               }

               #endregion

               #region �ɷ�ġ

               foreach (var t in PlayerStat)
               {
                   t.text = "-";
               }
               
               for (int i = 0; i < 17; i++)
               {
                   float da = float.Parse(data["Playerstat_train"][i].ToString());

                   PlayerStat[i].text = i is 2 or 10 or 11 or 12 or 13 or 14 or 15 or 16? $"{da:N0}%" : $"{da:N0}";
               }

               decimal str = decimal.Parse(data["Playerstat_train"][5].ToString());
               decimal dex= decimal.Parse(data["Playerstat_train"][6].ToString());
               decimal ints= decimal.Parse(data["Playerstat_train"][7].ToString());
               decimal wis= decimal.Parse(data["Playerstat_train"][8].ToString());

               if (str + dex > ints + wis)
               {
                   DamagerTypeObj[0].SetActive(true);
                   DamagerTypeObj[1].SetActive(false);
                   DamagerTypeObj[2].SetActive(false);
               }
               else if(ints > wis)
               {
                   DamagerTypeObj[0].SetActive(false);
                   DamagerTypeObj[1].SetActive(true);
                   DamagerTypeObj[2].SetActive(false);
               }
               else
               {
                   DamagerTypeObj[0].SetActive(false);
                   DamagerTypeObj[1].SetActive(false);
                   DamagerTypeObj[2].SetActive(true);
               }

               #endregion


               #region �нú�

               for (int i = 0; i < data["Passive_train"].Count; i++)
               {
//                Debug.Log(data["Passive"][i].ToString());
                   if (data["Passive_train"][i].ToString() == "" || data["Passive_train"][i].ToString() == "True")
                   {
                       Passivename[i].text = "-";
                       Passivename[i].color = Color.white;
                       Passiveinfo[i].text = "-";
                   }
                   else
                   {
                       //  Debug.Log(data["Passive_train"][i].ToString());
                       Passivename[i].text =
                           Inventory.GetTranslate(PassiveDB.Instance.Find_id(data["Passive_train"][i].ToString()).name);
                       //  Debug.Log("-00");
                       Inventory.Instance.ChangeItemRareColor(Passivename[i],
                           ClassDB.Instance.Find_id(data["Passive_train"][i].ToString()).tier);
                       //   Debug.Log("-00");

                       Passiveinfo[i].text =
                           Inventory.GetTranslate(PassiveDB.Instance.Find_id(data["Passive_train"][i].ToString()).info);
//                    Debug.Log("-00");

                   }
               }

               foreach (var VARIABLE in DPSSlots)
               {
                   VARIABLE.gameObject.SetActive(false);
               }


               foreach (string key in data["TrainDmg"].Keys)
               {
                   Dps_Train.Add(key, new DPS(data["TrainDmg"][key]));
               }

               dpsmanager.Instance.ShowDpsList(Dps_Train, DPSSlots);
               Debug.Log(data["TrainTotalDmg"].ToString());
               decimal a = decimal.Parse(data["TrainTotalDmg"].ToString(), System.Globalization.NumberStyles.Float);
               TotalDmg.text = dpsmanager.convertNumber(a);
               DPSDmg.text = dpsmanager.convertNumber(decimal.Parse(data["DPSDmgText"].ToString(),NumberStyles.Float));
               TotalTime.text =
                   $"{data["TrainTime"].ToString()}\n<color=yellow><size=23>({float.Parse(data["BreakTime"].ToString()):N0})</size></color>";

               #endregion

               #region �����Ƽ

               if (data.ContainsKey("TrainAbility"))
               {
                   for (int i = 0; i < data["TrainAbility"].Count; i++)
                   {
                       //                Debug.Log(data["Passive"][i].ToString());
                       if (data["TrainAbility"][i].ToString() == "" || data["TrainAbility"][i].ToString() == "True")
                       {
                           AbilitySlots[i].NoRefresh();
                       }
                       else
                       {
                           AbilitySlots[i].Refresh(data["TrainAbility"][i].ToString());
                       }
                   }
               }
               else
               {
                   for (int i = 0; i < PlayerBackendData.Instance.Abilitys.Length; i++)
                   {
                       AbilitySlots[i].NoRefresh();
                   }
               }

               #endregion
               
               #region  ��

               if (data.ContainsKey("nowPetData_train"))
               {
                   if (data["nowPetData_train"]["Petid"].ToString() != "")
                   {
                       PetImage.sprite =
                           SpriteManager.Instance.GetSprite(PetDB.Instance.Find_id(data["nowPetData_train"]["Petid"].ToString()).sprite);
                       PetImage.enabled = true;
                       PlayerData.Instance.SetPetStarOther(PetImage.material,int.Parse(data["nowPetData_train"]["Petstar"].ToString()));

                   }
                   else
                   {
                       PetImage.enabled = false;
                   }
               }
               else
               {
                   if (data.ContainsKey("nowPetid_train"))
                   {
                       if (data["nowPetid_train"].ToString() != "")
                       {
                           PetImage.sprite =
                               SpriteManager.Instance.GetSprite(PetDB.Instance.Find_id(data["nowPetid_train"].ToString()).sprite);
                           PetImage.enabled = true;
                           PlayerData.Instance.SetPetStarOther(PetImage.material,PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar);

                       }
                       else
                       {
                           PetImage.enabled = false;
                       }
                   }   
                   else
                   {
                       PetImage.enabled = false;
                   }
                   
               }
               

               #endregion

               
               #region  ����
               if (data.ContainsKey("avata_avata"))
               {
                   if (data["avata_avata"].ToString() != "")
                   {
                       PlayerData.Instance.SetAvartaImage(PlayerAvarta,
                           SpriteManager.Instance.GetSprite(AvartaDB.Instance.Find_id(data["avata_avata"].ToString())
                               .sprite));
                   }
                   if (data["avata_weapon"].ToString() != "")
                   {
                       PlayerData.Instance.SetAvartaImage(MainWeapon,
                           SpriteManager.Instance.GetSprite(AvartaDB.Instance.Find_id(data["avata_weapon"].ToString())
                               .sprite));
                   }
                   if (data["avata_subweapon"].ToString() != "")
                   {
                       PlayerData.Instance.SetAvartaImage(SubWeapon,
                           SpriteManager.Instance.GetSprite(AvartaDB.Instance.Find_id(data["avata_subweapon"].ToString())
                               .sprite));
                   }
                   
                   
                   if (bool.Parse(AvartaDB.Instance.Find_sprite(data["avata_avata"].ToString()).iswhole))
                   {
                       SubWeapon.enabled = false;
                       MainWeapon.enabled = false;
                   }
                   else
                   {
                       SubWeapon.enabled = true;
                       MainWeapon.enabled = true;
                   }
                    
               }
               
               
               #endregion
           });
       });
   }


   bool[] Setbool;
   string[] SetString;
   
   List<string> EquipA = new List<string>();
   List<string> EquipB = new List<string>();
   List<string> EquipC = new List<string>();
   List<string> EquipD = new List<string>();
   List<string> EquipE = new List<string>();
   List<string> EquipF = new List<string>();

   
    void CheckSetItem(EquipDatabase[] equipdata, string EquipID,bool isslotparticle = false)
    {
        //Debug.Log(EquipID);
        SetDBDB.Row  Data = SetDBDB.Instance.Find_id(EquipItemDB.Instance.Find_id(EquipID).SetID);
        Setbool = new bool[int.Parse(Data.setcount)];
        SetString = new string[int.Parse(Data.setcount)];
        EquipA.Clear();
        EquipB.Clear();
        EquipC.Clear();
        EquipD.Clear();
        EquipE.Clear();
        EquipF.Clear();
        //��� ��� Ÿ�� �˻�
        foreach (var t in equipdata)
        {
            if (t == null) continue;
            string id = t.Itemid;
            if (EquipA.Contains(id))
            {
                Setbool[0] = true;
                SetString[0] = id;
            }

            if (EquipB.Contains(id))
            {
                Setbool[1] = true;
                SetString[1] = id;
            }

            if (EquipC.Contains(id))
            {
                Setbool[2] = true;
                SetString[2] = id;
            }

            if (EquipD.Contains(id))
            {
                Setbool[3] = true;
                SetString[3] = id;
            }

            if (EquipE.Contains(id))
            {
                Setbool[4] = true;
                SetString[4] = id;
            }

            if (EquipF.Contains(id))
            {
                Setbool[5] = true;
                SetString[5] = id;
            }
        }
        //Debug.Log("DD");

        bool isallon = true;
        foreach (var t in Setbool)
        {
            if (!t)
                isallon = false;
        }
       // Debug.Log("DD");
        
        foreach (var t in Equipslots)
            t.ShowSetParticle(false);
        //Debug.Log("DD");
        if(isallon)
        {
            if (!isslotparticle) return;
            
            foreach (var t in SetString)
            {
                //�ش� ������ �ҷ��´�.
                equipitemslot slots = GetEquipSlots(EquipItemDB.Instance.Find_id(t).Type);
                if (slots.data != null)
                {
                    slots.ShowSetParticle(slots.data.Itemid.Equals(t));
                }
                else
                    slots.ShowSetParticle(false);
            }
        }
       // Debug.Log("DD");

    }

    private equipitemslot GetEquipSlots(string type)
    {
        return type switch
        {
            "Weapon" => Equipslots[0],
            "SWeapon" => Equipslots[1],
            "Helmet" => Equipslots[2],
            "Chest" => Equipslots[3],
            "Glove" => Equipslots[4],
            "Boot" => Equipslots[5],
            "Ring" => Equipslots[6],
            "Necklace" => Equipslots[7],
            "Wing" => Equipslots[8],
            "Pet" => Equipslots[9],
            "Rune" => Equipslots[10],
            "Insignia" => Equipslots[11],
            _ => Equipslots[10]
        };
    }
     public void AddViewForOtherPanel()
     {
         uimanager.Instance.AddUiview(otherusermanager.Instance.Panel, true);
         if (MyGuildManager.Instance.MyGuildPanel.IsActive())
         {
             uimanager.Instance.AddUiview(MyGuildManager.Instance.MyGuildPanel, true);
         }
         if (MyGuildManager.Instance.ApplicantPanel.IsActive())
         {
             uimanager.Instance.AddUiview(MyGuildManager.Instance.ApplicantPanel, true);
         }
     }


     public UIView ChatUserPanel;
     public Image[] ChatUserImage;
     public Text ChatUserNameText;
     public Text ChatReportUserNameText;
     public GameObject[] ChatuserButton;

     
     public GameObject InviteButton;
     public GameObject WithdrawpartyButton;
     
     public InputField ChatReportInput;
     public InputField ChatReportInput2;
     //ä�� ���� ����
     public void Bt_ShowChatUserData(Sprite body,Sprite main,Sprite sub,string username,int adlv = 0)
     {
         if(username == PlayerBackendData.Instance.nickname)
             return; 
         ChatUserImage[0].sprite = body;
         ChatUserImage[1].sprite = main;
         ChatUserImage[2].sprite = sub;
         ChatUserNameText.text = username;
         ChatReportUserNameText.text = username;
         bool isUnblock = Backend.Chat.IsUserBlocked(username);
         InviteButton.SetActive(false);
         WithdrawpartyButton.SetActive(false);
         if (isUnblock)
         {
             ChatuserButton[0].SetActive(false);
             ChatuserButton[1].SetActive(true);
         }
         else
         {
             ChatuserButton[0].SetActive(true);
             ChatuserButton[1].SetActive(false);
         }

         if (PartyRaidRoommanager.Instance.nowmyleadernickname == PlayerBackendData.Instance.nickname)
         {
             bool ishave = false;
             for (int i = 0; i < PartyRaidRoommanager.Instance.PartyMember.Length; i++)
             {
                 if (PartyRaidRoommanager.Instance.PartyMember[i].data != null)
                 {
                     if (PartyRaidRoommanager.Instance.PartyMember[i].data.nickname == username)
                     {
                         ishave = true;
                         break;
                     }
                 }
             }
             if (ishave)
             {
                 WithdrawpartyButton.SetActive(true);
             }
             else
             {
                 if (PlayerBackendData.Instance.GetAdLv() >= 20 && adlv >= 20)
                     InviteButton.SetActive(true);
             }
             //�� �������
         }
         
         ChatUserPanel.Show(false);
     }

     public void RefreshChatUserData()
     {
         bool isUnblock = Backend.Chat.IsUserBlocked(ChatUserNameText.text);

         if (isUnblock)
         {
             ChatuserButton[0].SetActive(false);
             ChatuserButton[1].SetActive(true);
         }
         else
         {
             ChatuserButton[0].SetActive(true);
             ChatuserButton[1].SetActive(false);
         }
     }
     public void Bt_ChatUser_Data()
     {
         ShowPlayerData(ChatUserNameText.text);
         uimanager.Instance.AddUiview(ChatUserPanel,true);
     }

     public void Bt_BanChat()
     {
         chatmanager.Instance.BlockUser(ChatUserNameText.text);
     }
     public void Bt_UnBanChat()
     {
         chatmanager.Instance.UnBlockUser(ChatUserNameText.text);
     }

     public void Bt_InviteParty()
     {
         for (int i = 0; i < PartyRaidRoommanager.Instance.PartyMember.Length; i++)
         {
             if (PartyRaidRoommanager.Instance.PartyMember[i].data == null)
             {
                 PartyraidChatManager.Instance.Chat_invitePlayer(ChatUserNameText.text);
                 break;
             }
         }
     }

     public void Bt_ExitParty()
     {
         Debug.Log(ChatUserNameText.text);

         Debug.Log(PartyRaidRoommanager.Instance.PartyMember[PartyRaidRoommanager.Instance.SelectPartyUsernum].data.nickname);
         if (PartyRaidRoommanager.Instance.PartyMember[PartyRaidRoommanager.Instance.SelectPartyUsernum].data.nickname == ChatUserNameText.text)
         {
             PartyraidChatManager.Instance.Chat_WithdrawMember(ChatUserNameText.text);
             ChatUserPanel.Hide(false);
         }
     }

     public void Bt_ReportPlayer()
     {
         SendQueue.Enqueue(
             Backend.Chat.ReportUser,
             ChatUserNameText.text,
             ChatReportInput2.text,
             ChatReportInput.text,
             (callback) => {
                 if (callback.IsSuccess())
                 {
                     alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI4/신고 완료"), alertmanager.alertenum.일반);
                 }
                 // ���� ó��
             }
         );
     }
}
