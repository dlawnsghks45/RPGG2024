using System;
using CodeStage.AntiCheat.ObscuredTypes;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using BackEnd;
using Doozy.Engine.Soundy;
using Doozy.Engine.Utils.ColorModels;
using GooglePlayGames;

public class Savemanager : MonoBehaviour
{

    private static Savemanager _instance = null;

    public static Savemanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Savemanager)) as Savemanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    public ES3File _es3FileSystem;
    public ES3File _es3File;
    public void Init()
    { 
        _es3File = new ES3File($"{PlayerBackendData.Instance.nickname}3.es3", new ES3Settings(ES3.EncryptionType.AES));
        _es3FileSystem = new ES3File($"systemlog.es3", new ES3Settings(ES3.EncryptionType.AES));
    }


   
    
    
    public void LoadData()
    {
        LoadClassData();
        LoadEquip();
        LoadSkillData();
        LoadMoneyData();
        LoadStageData();
        LoadInventory();
        LoadExpData();
        LoadAchieve();
        LoadCollection();
        LoadShopData();
        LoadLoadTime();
        LoadGuideQuest();
        LoadadsFree();
        LoadSotang();
        LoadDailyPanel();
        LoadCraft();
        LoadAbility();
        LoadRoulette();
        LoadLoginTime();
        LoadSaveNum();
        LoadAvataData();
        LoadTalisman();
        //LoadTime();
    }

    public void SaveEvery()
    {
        SaveOnlyLv();
        SaveLoadTime();
        SaveClassData();
        SaveEquip();
        SaveExpData();
        SaveInventory();
        SaveSkillData();
        SaveMoneyDataDirect();
        SaveCash();
        SaveCraft();
        SaveAchieve();
        SaveStageData();
        SaveGuideQuest();
        SaveCollection();
        SaveShopData();
        SaveadsFree();
        SaveSotang();
        SaveAbility();
        SaveRoulette();
        SaveLoginTime();
        SaveAvataData();
        SaveTalisman();
        Save();
    }
    public void SaveSaveNum()
    {
        PlayerBackendData.Instance.ClientSaveNum++;
        _es3File.Save($"{PlayerBackendData.Instance.Id}ClientSaveNum", PlayerBackendData.Instance.ClientSaveNum);
//        Debug.Log("세이브넘" + PlayerBackendData.Instance.ClientSaveNum);
    }
    public void SaveSaveNum2()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}ClientSaveNum", PlayerBackendData.Instance.ClientSaveNum);
    }
    public void LoadSaveNum()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}ClientSaveNum"))
        {
            PlayerBackendData.Instance.ClientSaveNum =
                _es3File.Load<int>($"{PlayerBackendData.Instance.Id}ClientSaveNum");
        }
    }

    public void SaveAbility()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}Ability", PlayerBackendData.Instance.Abilitys);
    }

    public void LoadAbility()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}Ability"))
        {
            string[] abilitys = _es3File.Load<string[]>($"{PlayerBackendData.Instance.Id}Ability");

            for (int i = 0; i < abilitys.Length; i++)
            {
                PlayerBackendData.Instance.Abilitys[i] = abilitys[i];
            }
        }
    }
    

    public void SaveTypeEquip()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}equipdata0", PlayerBackendData.Instance.EquipEquiptment0);
        switch (Inventory.Instance.nowsettype)
        {
           case 0:
               _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven0", PlayerBackendData.Instance.Equiptment0);
               break;
           case 1:
               _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven1", PlayerBackendData.Instance.Equiptment1);

               break;
           
           case 2:
               _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven2", PlayerBackendData.Instance.Equiptment2);

               break;
           case 3:
               _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven3", PlayerBackendData.Instance.Equiptment3);

               break;
           case 4:
               _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven4", PlayerBackendData.Instance.Equiptment4);

               break;
           case 5:
               _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven5", PlayerBackendData.Instance.Equiptment5);

               break;
           case 6:
               _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven6", PlayerBackendData.Instance.Equiptment6);
               break;
           case 7:
               _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven7", PlayerBackendData.Instance.Equiptment7);

               break;
           case 8:
               _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven8", PlayerBackendData.Instance.Equiptment8);

               break;
           case 9:
               _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven9", PlayerBackendData.Instance.Equiptment9);

               break;
           case 10:
               _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven10", PlayerBackendData.Instance.Equiptment10);

               break;
           case 11:
               _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven11", PlayerBackendData.Instance.Equiptment11);

               break;
           case 12:
     //          _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven12", PlayerBackendData.Instance.Equiptment12);

               break;
           case 13:
       //        _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven13", PlayerBackendData.Instance.Equiptment13);

               break;
           case 14:
           //    _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven0", PlayerBackendData.Instance.Equiptment0);

               break;
        }
        _es3File.Save($"{PlayerBackendData.Instance.Id}equipdata0", PlayerBackendData.Instance.EquipEquiptment0);
    }
    // ReSharper disable Unity.PerformanceAnalysis
    public void SaveEquip()
    {

        _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven0", PlayerBackendData.Instance.Equiptment0);
        _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven1", PlayerBackendData.Instance.Equiptment1);
        _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven2", PlayerBackendData.Instance.Equiptment2);
        _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven3", PlayerBackendData.Instance.Equiptment3);
        _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven4", PlayerBackendData.Instance.Equiptment4);
        _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven5", PlayerBackendData.Instance.Equiptment5);
        _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven6", PlayerBackendData.Instance.Equiptment6);
        _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven7", PlayerBackendData.Instance.Equiptment7);
        _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven8", PlayerBackendData.Instance.Equiptment8);
        _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven9", PlayerBackendData.Instance.Equiptment9);
        _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven10", PlayerBackendData.Instance.Equiptment10);
        _es3File.Save($"{PlayerBackendData.Instance.Id}equipInven11", PlayerBackendData.Instance.Equiptment11);

        _es3File.Save($"{PlayerBackendData.Instance.Id}equipdata0", PlayerBackendData.Instance.EquipEquiptment0);
        _es3File.Save($"{PlayerBackendData.Instance.Id}equipdata1", PlayerBackendData.Instance.EquipEquiptment1);

        _es3File.Save($"{PlayerBackendData.Instance.Id}EquipPreset", PlayerBackendData.Instance.nowpreset);

        //Debug.Log("저장성공");
    }

    public void SaveTalisman()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}TalismanData", PlayerBackendData.Instance.TalismanData);
        _es3File.Save($"{PlayerBackendData.Instance.Id}TalismanPreset", PlayerBackendData.Instance.TalismanPreset);
        _es3File.Save($"{PlayerBackendData.Instance.Id}nowtalismanpreset",
            PlayerBackendData.Instance.nowtalismanpreset);
    }

    public void LoadTalisman()
    {
//        Debug.Log("장비" + _es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipInven0"));
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}TalismanData"))
        {
            PlayerBackendData.Instance.TalismanData = _es3File.Load<Dictionary<string, Talismandatabase>>(
                $"{PlayerBackendData.Instance.Id}TalismanData");
            

        }
        
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}TalismanPreset"))
        {
            PlayerBackendData.Instance.TalismanPreset = _es3File.Load<PresetTalisman[]>(
                $"{PlayerBackendData.Instance.Id}TalismanPreset");

            PlayerBackendData.Instance.nowtalismanpreset = _es3File.Load<int>(
                $"{PlayerBackendData.Instance.Id}nowtalismanpreset");
        }
    }

    public void SaveTalismanPreset()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}nowtalismanpreset", PlayerBackendData.Instance.nowtalismanpreset);
        _es3File.Save($"{PlayerBackendData.Instance.Id}TalismanPreset", PlayerBackendData.Instance.TalismanPreset);
    }

    public void SaveContentLevel()
    {
        //_es3File.Save($"{PlayerBackendData.Instance.Id}contentlevel", PlayerBackendData.Instance.ContentLevel);
    }

    public void LoadContentLevel()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}contentlevel"))
        {
            PlayerBackendData.Instance.ContentLevel =
                _es3File.Load<int[]>($"{PlayerBackendData.Instance.Id}contentlevel");
        }
    }

    public void SaveSotang()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}sotang_raid", PlayerBackendData.Instance.sotang_raid);
        _es3File.Save($"{PlayerBackendData.Instance.Id}sotang_dungeon", PlayerBackendData.Instance.sotang_dungeon);

    }
    public void LoadSotang()
    {
        return;
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}sotang_raid"))
        {
            PlayerBackendData.Instance.sotang_raid =
                _es3File.Load<List<string>>($"{PlayerBackendData.Instance.Id}sotang_raid");
        }
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}sotang_dungeon"))
        {
            PlayerBackendData.Instance.sotang_dungeon =
                _es3File.Load<List<string>>($"{PlayerBackendData.Instance.Id}sotang_dungeon");
        }

       // LoadContentLevel();
    }
    
    public void SaveGuideQuest()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}tutoid", PlayerBackendData.Instance.tutoid);
        _es3File.Save($"{PlayerBackendData.Instance.Id}tutocount", PlayerBackendData.Instance.tutocount);
        
        _es3File.Save($"{PlayerBackendData.Instance.Id}tutoguideids", PlayerBackendData.Instance.tutoguideid);
        _es3File.Save($"{PlayerBackendData.Instance.Id}tutoguideisfinish", PlayerBackendData.Instance.tutoguideisfinish);
        _es3File.Save($"{PlayerBackendData.Instance.Id}tutoguidepremium", PlayerBackendData.Instance.tutoguidepremium);
    }


    public void LoadGuideQuest()
    {
        if (!_es3File.KeyExists($"{PlayerBackendData.Instance.Id}tutoid")) return;
        PlayerBackendData.Instance.tutoid =
            _es3File.Load<string>($"{PlayerBackendData.Instance.Id}tutoid");
        PlayerBackendData.Instance.tutocount =
            _es3File.Load<int>($"{PlayerBackendData.Instance.Id}tutocount");
        
        if (!_es3File.KeyExists($"{PlayerBackendData.Instance.Id}tutoguideids")) return;
        PlayerBackendData.Instance.tutoguideid =
            _es3File.Load<int>($"{PlayerBackendData.Instance.Id}tutoguideids");
      
        if (!_es3File.KeyExists($"{PlayerBackendData.Instance.Id}tutoguideisfinish")) return;
        PlayerBackendData.Instance.tutoguideisfinish =
            _es3File.Load<bool>($"{PlayerBackendData.Instance.Id}tutoguideisfinish");
        
        if (!_es3File.KeyExists($"{PlayerBackendData.Instance.Id}tutoguidepremium")) return;
        PlayerBackendData.Instance.tutoguidepremium =
            _es3File.Load<bool>($"{PlayerBackendData.Instance.Id}tutoguidepremium");
    }

 

    public void SaveDailyPanel()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}DailyOffBool", PlayerBackendData.Instance.DailyOffBool);
    }
    public void LoadDailyPanel()
    {
        if (!_es3File.KeyExists($"{PlayerBackendData.Instance.Id}DailyOffBool")) return;
        PlayerBackendData.Instance.DailyOffBool =
            _es3File.Load<bool[]>($"{PlayerBackendData.Instance.Id}DailyOffBool");
    }

    public void LoadEquip()
    {
//        Debug.Log("장비" + _es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipInven0"));
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipInven0"))
        {
            PlayerBackendData.Instance.Equiptment0 = _es3File.Load<Dictionary<string, EquipDatabase>>(
                $"{PlayerBackendData.Instance.Id}equipInven0");

        }
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipInven1"))
        {
            PlayerBackendData.Instance.Equiptment1 = _es3File.Load<Dictionary<string, EquipDatabase>>(
                $"{PlayerBackendData.Instance.Id}equipInven1");

        }
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipInven2"))
        {

            PlayerBackendData.Instance.Equiptment2 = _es3File.Load<Dictionary<string, EquipDatabase>>(
                $"{PlayerBackendData.Instance.Id}equipInven2");
        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipInven3"))
        {
            PlayerBackendData.Instance.Equiptment3 = _es3File.Load<Dictionary<string, EquipDatabase>>(
                $"{PlayerBackendData.Instance.Id}equipInven3");

        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipInven4"))
        {
            PlayerBackendData.Instance.Equiptment4 = _es3File.Load<Dictionary<string, EquipDatabase>>(
                $"{PlayerBackendData.Instance.Id}equipInven4");

        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipInven5"))
        {
            PlayerBackendData.Instance.Equiptment5 = _es3File.Load<Dictionary<string, EquipDatabase>>(
                $"{PlayerBackendData.Instance.Id}equipInven5");

        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipInven6"))
        {
            PlayerBackendData.Instance.Equiptment6 = _es3File.Load<Dictionary<string, EquipDatabase>>(
                $"{PlayerBackendData.Instance.Id}equipInven6");

        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipInven7"))
        {
            PlayerBackendData.Instance.Equiptment7 = _es3File.Load<Dictionary<string, EquipDatabase>>(
                $"{PlayerBackendData.Instance.Id}equipInven7");

        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipInven8"))
        {
            PlayerBackendData.Instance.Equiptment8 = _es3File.Load<Dictionary<string, EquipDatabase>>(
                $"{PlayerBackendData.Instance.Id}equipInven8");

        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipInven9"))
        {
            PlayerBackendData.Instance.Equiptment9 = _es3File.Load<Dictionary<string, EquipDatabase>>(
                $"{PlayerBackendData.Instance.Id}equipInven9");

        }
        
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipInven10"))
        {
            PlayerBackendData.Instance.Equiptment10 = _es3File.Load<Dictionary<string, EquipDatabase>>(
                $"{PlayerBackendData.Instance.Id}equipInven10");

        }
        
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipInven11"))
        {
            PlayerBackendData.Instance.Equiptment11 = _es3File.Load<Dictionary<string, EquipDatabase>>(
                $"{PlayerBackendData.Instance.Id}equipInven11");

        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipdata0"))
        {
            //Debug.Log("자입정보");
            PlayerBackendData.Instance.EquipEquiptment0 = _es3File.Load<EquipDatabase[]>(
                $"{PlayerBackendData.Instance.Id}equipdata0");

            for (int i = 0; i < PlayerBackendData.Instance.EquipEquiptment0.Length; i++)
            {
                if (PlayerBackendData.Instance.EquipEquiptment0[i] != null)
                {
                    string types = EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.EquipEquiptment0[i].Itemid)
                        .Type;
                    //장비칸에는 있는데 인벤엥벗다면
                    if (!PlayerBackendData.Instance.GetTypeEquipment(types)
                        .ContainsKey(PlayerBackendData.Instance.EquipEquiptment0[i].KeyId1))
                    {
                        Debug.Log("인벤에 없어서 만들었다"+ PlayerBackendData.Instance.EquipEquiptment0[i].Itemid);
                        PlayerBackendData.Instance.AddEquipment(PlayerBackendData.Instance.EquipEquiptment0[i]);
                    }
                }
            }
            
            
        }
        
        
        

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipdata1"))
        {
            //Debug.Log("자입정보");
            PlayerBackendData.Instance.EquipEquiptment1 = _es3File.Load<EquipDatabase[]>(
                $"{PlayerBackendData.Instance.Id}equipdata1");
        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}equipdata2"))
        {
            //  Debug.Log("자입정보");
            PlayerBackendData.Instance.EquipEquiptment2 = _es3File.Load<EquipDatabase[]>(
                $"{PlayerBackendData.Instance.Id}equipdata2");
        }

//        _es3File.Save($"{PlayerBackendData.Instance.Id}EquipPreset", PlayerBackendData.Instance;

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}EquipPreset"))
        {
            PlayerBackendData.Instance.nowpreset =
                _es3File.Load<int>($"{PlayerBackendData.Instance.Id}EquipPreset");
        }

    }

    public void GetEquipItemToInven()
    {
        foreach (var t in PlayerBackendData.Instance.EquipEquiptment0)
        {
            if (t == null) continue;
            string types = EquipItemDB.Instance.Find_id(t.Itemid)
                .Type;
            //장비칸에는 있는데 인벤엥벗다면
            if (PlayerBackendData.Instance.GetTypeEquipment(types)
                .ContainsKey(t.KeyId1)) continue;
            Debug.Log("인벤에 없어서 만들었다"+ t.Itemid);
            PlayerBackendData.Instance.AddEquipment(t);
            Savemanager.Instance.SaveEquip();
            Savemanager.Instance.Save();
        }
    }

    public void SaveClassData()
    {
        _es3File.Save<string>($"{PlayerBackendData.Instance.Id}NowClass",
            userData.ClassData[userData.ClassId].ClassId1);
        _es3File.Save<Dictionary<string, ClassDatabase>>($"{PlayerBackendData.Instance.Id}Class",
            PlayerBackendData.Instance.ClassData);
        _es3File.Save<string[]>($"{PlayerBackendData.Instance.Id}Passive", PlayerBackendData.Instance.PassiveClassId);
    }

    public void SaveAvataData()
    {
        _es3File.Save<string>($"{PlayerBackendData.Instance.Id}avata_avata",
            userData.avata_avata);
        _es3File.Save<string>($"{PlayerBackendData.Instance.Id}avata_weapon",
            userData.avata_weapon);
        _es3File.Save<string>($"{PlayerBackendData.Instance.Id}avata_subweapon",
            userData.avata_subweapon);
        _es3File.Save<bool[]>($"{PlayerBackendData.Instance.Id}playeravata", PlayerBackendData.Instance.playeravata);
    }
    
    private void LoadAvataData()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}avata_avata"))
        {
            PlayerBackendData.Instance.avata_avata = (_es3File.Load<string>($"{PlayerBackendData.Instance.Id}avata_avata"));
        }
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}avata_weapon"))
        {
            PlayerBackendData.Instance.avata_weapon = (_es3File.Load<string>($"{PlayerBackendData.Instance.Id}avata_weapon"));
        }
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}avata_subweapon"))
        {
            PlayerBackendData.Instance.avata_subweapon = (_es3File.Load<string>($"{PlayerBackendData.Instance.Id}avata_subweapon"));
        }
        
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}playeravata"))
        {
            bool[] data =
                _es3File.Load<bool[]>(
                    $"{PlayerBackendData.Instance.Id}playeravata");

            for (int i = 0; i < data.Length; i++)
            {
                PlayerBackendData.Instance.playeravata[i] = data[i];
            }
        }
    }

    public void LoadClassData()
    {
        try
        {
            if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}Class"))
            {
                PlayerBackendData.Instance.ClassId =
                    _es3File.Load<string>($"{PlayerBackendData.Instance.Id}NowClass");


                Dictionary<string, ClassDatabase> temp = new Dictionary<string, ClassDatabase>();
                
                temp =  _es3File.Load<Dictionary<string, ClassDatabase>>(
                    $"{PlayerBackendData.Instance.Id}Class");

                foreach (var VARIABLE in temp)
                {
                    if (PlayerBackendData.Instance.ClassData.ContainsKey(VARIABLE.Key))
                    {
                        PlayerBackendData.Instance.ClassData[VARIABLE.Key] = VARIABLE.Value;
                    }
                }
                //  Debug.Log("데이터가 있다");
            }
            else
            {
                PlayerBackendData.Instance.ClassId = "0";
            }




            PlayerBackendData.Instance.PassiveClassId = new string[7];
            if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}Passive"))
            {
                string[] Temp = _es3File.Load<string[]>($"{PlayerBackendData.Instance.Id}Passive");
                for (int i = 0; i < Temp.Length; i++)
                {
                    if (Temp[i] != "" && Temp[i] != "True")
                    {
                        PlayerBackendData.Instance.PassiveClassId[i] = Temp[i];
                    }
                }
                for (int i = 0; i < PlayerBackendData.Instance.PassiveClassId.Length; i++)
                {
//                    Debug.Log(PlayerBackendData.Instance.PassiveClassId[i]);
                    if (PlayerBackendData.Instance.PassiveClassId[i] == ("True"))
                    {
                        PlayerBackendData.Instance.PassiveClassId[i] = "";
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("에러" + e);
            // if(ES3.RestoreBackup(GetFileName()))
            //     Debug.Log("백업 복구");
            //  else
            //      Debug.Log("데이터 복구 실패");
            //   if (ES3.RestoreBackup(GetFileNameTimeCheck()))
            //    {
            //        Debug.Log("타임백어복구");
            //        LoadClassData();
            //    }
            //    else
            //        Debug.Log("타임백업복구실패");
            //}
        }
    }


    public void SaveSkillData()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}Skills", PlayerBackendData.Instance.Skills);

    }


    public void LoadSkillData()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}Skills"))
        {
            List<string> tempskill =
                _es3File.Load<List<string>>($"{PlayerBackendData.Instance.Id}Skills");

            for (int i = 0; i < tempskill.Count; i++)
            {
                if(tempskill[i] == "")
                    continue;
                if (!PlayerBackendData.Instance.Skills.Contains(tempskill[i]))
                {
                    PlayerBackendData.Instance.Skills.Add(tempskill[i]);
                }
            }

            foreach (var VARIABLE in PlayerBackendData.Instance.ClassData)
            {
                if (!VARIABLE.Value.Isown) continue;
                if (PlayerBackendData.Instance.Skills.Contains(ClassDB.Instance.Find_id(VARIABLE.Value.ClassId1)
                        .giveskill)) continue;

                PlayerBackendData.Instance.Skills.Add(ClassDB.Instance.Find_id(VARIABLE.Value.ClassId1).giveskill);
            }
            //Debug.Log("스킬 데이터 불러오기");
        }
    }

    //재화저장

    private bool issavecooldown_money = false;

    // ReSharper disable Unity.PerformanceAnalysis
    public void SaveMoneyData()
    {
        if (issavecooldown_money) return;
        issavecooldown_money = true;
        _es3File.Save($"{PlayerBackendData.Instance.Id}Money", PlayerBackendData.Instance.GetMoney());

    }

    public void SaveMoneyCashDirect()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}Money", PlayerBackendData.Instance.GetMoney());
        _es3File.Save($"{PlayerBackendData.Instance.Id}Cash", PlayerBackendData.Instance.GetCash());

    }

    public void SaveMoneyDataDirect()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}Money", PlayerBackendData.Instance.GetMoney());

    }

    public void SaveCash()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}Cash", PlayerBackendData.Instance.GetCash());

    }

    private void LoadMoneyData()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}Money"))
        {
            // Debug.Log("돈");
            PlayerBackendData.Instance.SetMoney(_es3File.Load<decimal>($"{PlayerBackendData.Instance.Id}Money"));
        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}Cash"))
        {
            PlayerBackendData.Instance.SetCash(_es3File.Load<decimal>($"{PlayerBackendData.Instance.Id}Cash"));
        }
    }

    public void SaveadsFree()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}isadsfree", PlayerBackendData.Instance.isadsfree);

    }

    private void LoadadsFree()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}isadsfree"))
        {
            // Debug.Log("돈");
            PlayerBackendData.Instance.isadsfree =
                (_es3File.Load<bool>($"{PlayerBackendData.Instance.Id}isadsfree"));
        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}Cash"))
        {
            PlayerBackendData.Instance.SetCash(_es3File.Load<decimal>($"{PlayerBackendData.Instance.Id}Cash"));
        }
    }

    public void Saveonlystage()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}Stage", PlayerBackendData.Instance.nowstage);
        _es3File.Save($"{PlayerBackendData.Instance.Id}FieldCount", PlayerBackendData.Instance.spawncount);
Save();
    }
    //맵정보
    public void SaveStageData()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}FieldLv", PlayerBackendData.Instance.GetFieldLv());

    }

    public void LoadStageData()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}Stage"))
        {
            PlayerBackendData.Instance.nowstage =
                _es3File.Load<string>($"{PlayerBackendData.Instance.Id}Stage");
        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}FieldCount"))
        {
            PlayerBackendData.Instance.spawncount =
                (_es3File.Load<int>($"{PlayerBackendData.Instance.Id}FieldCount"));
            //  Debug.Log("카웆ㄴ트는" + PlayerBackendData.Instance.spawncount);
        }
        
      
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}FieldLv"))
        {
            PlayerBackendData.Instance.SetFieldLV(_es3File.Load<int>($"{PlayerBackendData.Instance.Id}FieldLv"));
        }
    }

    private bool issavecooldown = false;
    private bool issavecooldown_achieve = false;

    //맵정보
    public void SaveInventory()
    {
        if (issavecooldown) return;
        issavecooldown = true;
        Invoke("truecooldown", 1.5f);
        _es3File.Save($"{PlayerBackendData.Instance.Id}InventoryItem", PlayerBackendData.Instance.ItemInventory);
    }
    public void SaveInventory_SaveOn()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}InventoryItem", PlayerBackendData.Instance.ItemInventory);
        Save();
    }
    
    public void SaveRoulette()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}RouletteCount", PlayerBackendData.Instance.RouletteCount);
    }
    public void LoadRoulette()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}RouletteCount"))
        {
            PlayerBackendData.Instance.RouletteCount = (_es3File.Load<int[]>($"{PlayerBackendData.Instance.Id}RouletteCount"));
        }
    }
    void truecooldown()
    {
        issavecooldown = false;
    }

    void truecooldown_money()
    {
        issavecooldown_money = false;
    }

    void truecooldown_achieve()
    {
        issavecooldown_achieve = false;
    }

    private void LoadInventory()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}InventoryItem"))
        {
            PlayerBackendData.Instance.ItemInventory = _es3File.Load<List<ItemInven>>(
                $"{PlayerBackendData.Instance.Id}InventoryItem");
        }
    }

    //SaveHpQuickSlot
    public void SaveHpQuickSlot()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}HpQuickSlot", Inventory.Instance.iteminfopanel.Hpslot.HpItemId);

    }

    public void LoadHpQuickSlot()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}HpQuickSlot"))
        {
            Inventory.Instance.iteminfopanel.Hpslot.SetPotion(_es3File.Load<string>(
                $"{PlayerBackendData.Instance.Id}HpQuickSlot"));
        }
    }

    //SaveHpQuickSlot
    public void SaveMpQuickSlot()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}MpQuickSlot", Inventory.Instance.iteminfopanel.Mpslot.HpItemId);

    }

    public void LoadMpQuickSlot()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}MpQuickSlot"))
        {
            Inventory.Instance.iteminfopanel.Mpslot.SetPotion(_es3File.Load<string>(
                $"{PlayerBackendData.Instance.Id}MpQuickSlot"));
        }
    }

    //SaveTIme


    //서버를 불러올때 저장한다.
    public void SaveLoadTime()
    {

    }

    public void LoadLoadTime()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}playersaveindate"))
        {
            PlayerBackendData.Instance.playersaveindate =
                _es3File.Load<string>(
                    $"{PlayerBackendData.Instance.Id}playersaveindate");
        }
        //로드 스트링과 서버로드가 같으면 같은거다
    }

    public void SaveCraft()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}craftmakingid", PlayerBackendData.Instance.craftmakingid);
        _es3File.Save($"{PlayerBackendData.Instance.Id}craftdatetime", PlayerBackendData.Instance.craftdatetime);
        _es3File.Save($"{PlayerBackendData.Instance.Id}craftdatecount", PlayerBackendData.Instance.craftdatecount);

    }

    public void LoadCraft()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}craftmakingid"))
        {
            PlayerBackendData.Instance.craftmakingid = _es3File.Load<string[]>(
                $"{PlayerBackendData.Instance.Id}craftmakingid");
            PlayerBackendData.Instance.craftdatetime = _es3File.Load<string[]>(
                $"{PlayerBackendData.Instance.Id}craftdatetime");
            PlayerBackendData.Instance.craftdatecount = _es3File.Load<int[]>(
                $"{PlayerBackendData.Instance.Id}craftdatecount");
        }
    }


    public void SaveShopData()
    {
        Timemanager.Instance.OnceTradePackage = Timemanager.Instance.OnceTradePackage.Distinct().ToList();
        Timemanager.Instance.OncePremiumPackage = Timemanager.Instance.OncePremiumPackage.Distinct().ToList();
        _es3File.Save($"{PlayerBackendData.Instance.Id}OncePremiumPackage", Timemanager.Instance.OncePremiumPackage);
        _es3File.Save($"{PlayerBackendData.Instance.Id}OnceTradePackage", Timemanager.Instance.OnceTradePackage);

    }

    public void LoadShopData()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}OncePremiumPackage"))
        {
            Timemanager.Instance.OncePremiumPackage = _es3File.Load<List<string>>(
                $"{PlayerBackendData.Instance.Id}OncePremiumPackage");
        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}OnceTradePackage"))
        {
            Timemanager.Instance.OnceTradePackage = _es3File.Load<List<string>>(
                $"{PlayerBackendData.Instance.Id}OnceTradePackage");
       //     Debug.Log("개수"+ Timemanager.Instance.OnceTradePackage.Count);
            Timemanager.Instance.OnceTradePackage = Timemanager.Instance.OnceTradePackage.Distinct().ToList();
//            Debug.Log("개수"+ Timemanager.Instance.OnceTradePackage.Count);
        }
    }


    public void SaveDeathPenalty()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}DeathPenalty", PlayerBackendData.Instance.DeathPenaltySecond);

    }

    public void LoadDeathPenalty()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}DeathPenalty"))
        {
            PlayerBackendData.Instance.DeathPenaltySecond = _es3File.Load<int>(
                $"{PlayerBackendData.Instance.Id}DeathPenalty");

            Battlemanager.Instance.RefreshDeath();
        }
    }
    public void SaveQuest()
    {
        // _es3File.Save($"{PlayerBackendData.Instance.Id}Achievedata", PlayerBackendData.Instance.PlayerAchieveData);
        _es3File.Save($"{PlayerBackendData.Instance.Id}QuestCount", PlayerBackendData.Instance.QuestCount);
        _es3File.Save($"{PlayerBackendData.Instance.Id}QuestIsFinish", PlayerBackendData.Instance.QuestIsFinish);
        _es3File.Save($"{PlayerBackendData.Instance.Id}QuestTotalCount", PlayerBackendData.Instance.QuestTotalCount);
    }
    public void LoadQuest()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}QuestCount"))
        {
            float[] temp = _es3File.Load<float[]>($"{PlayerBackendData.Instance.Id}QuestCount");
            for (int i = 0; i < temp.Length; i++)
            {
                PlayerBackendData.Instance.QuestCount[i] = temp[i];
            }
            
            bool[] temp2 = _es3File.Load<bool[]>($"{PlayerBackendData.Instance.Id}QuestIsFinish");
            for (int i = 0; i < temp2.Length; i++)
            {
                PlayerBackendData.Instance.QuestIsFinish[i] = temp2[i];
            }
            
            float[] temp3 = _es3File.Load<float[]>($"{PlayerBackendData.Instance.Id}QuestTotalCount");
            for (int i = 0; i < temp3.Length; i++)
            {
                PlayerBackendData.Instance.QuestTotalCount[i] = temp3[i];
            }
            
            
        }
    }
    
    public void SaveAchieve() 
    {
        //_es3File.Save($"{PlayerBackendData.Instance.Id}Achievedata", PlayerBackendData.Instance.PlayerAchieveData);
        SaveQuest();
    }
    public void SaveAchieveDirect()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}Achievedata", PlayerBackendData.Instance.PlayerAchieveData);
        SaveQuest();
    }

    public void LoadAchieve()
    {
        LoadQuest();
        /*
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}Achievedata"))
        {
            Dictionary<string, Achievedata> TempAchieve = new Dictionary<string, Achievedata>();


            TempAchieve = _es3File.Load<Dictionary<string, Achievedata>>(
                $"{PlayerBackendData.Instance.Id}Achievedata");

            foreach (var a in TempAchieve)
            {
                if (a.Value.Coreid == "A1074")
                {
                    try
                    {
                        string b = AchievementDB.Instance.Find_id(a.Value.Id).id;
                    }
                    catch (Exception e)
                    {
                        Debug.Log(a.Value.Id);
                        a.Value.Id = "A2200";
                        Debug.Log("업적변경");                    
                    }
                }
                PlayerBackendData.Instance.PlayerAchieveData[a.Key] = a.Value;
            }
        }
        */
    }


    public void SaveCollection()
    {
       // _es3File.Save($"{PlayerBackendData.Instance.Id}CollectData", PlayerBackendData.Instance.CollectData);
        _es3File.Save($"{PlayerBackendData.Instance.Id}CollectData2", PlayerBackendData.Instance.RenewalCollectData);
    }



    public void LoadCollection()
    {
        return;
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}CollectData2"))
        {
            bool[] data =
                _es3File.Load<bool[]>(
                    $"{PlayerBackendData.Instance.Id}CollectData2");

            for (int i = 0; i < data.Length; i++)
            {
                PlayerBackendData.Instance.RenewalCollectData[i] = data[i];
            }
        }
/*
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}CollectData"))
        {
            Dictionary<string, CollectDatabase> temp = _es3File.Load<Dictionary<string, CollectDatabase>>(
                $"{PlayerBackendData.Instance.Id}CollectData");
            foreach (var VARIABLE in temp)
            {
                if (PlayerBackendData.Instance.CollectData.ContainsKey(VARIABLE.Key))
                {
                    if (VARIABLE.Value.ItemID[0].Equals("3002"))
                        VARIABLE.Value.ItemID[0] = "3003";
                    if (VARIABLE.Value.ItemID[0].Equals("2900")||
                        VARIABLE.Value.ItemID[0].Equals("2901")||
                        VARIABLE.Value.ItemID[0].Equals("2902")||
                        VARIABLE.Value.ItemID[0].Equals("2903")||
                        VARIABLE.Value.ItemID[0].Equals("2904")||
                        VARIABLE.Value.ItemID[0].Equals("2905")||
                        VARIABLE.Value.ItemID[0].Equals("2906")||
                        VARIABLE.Value.ItemID[0].Equals("2907"))
                        VARIABLE.Value.ItemID[0] = "2908";
                    
                    PlayerBackendData.Instance.CollectData[VARIABLE.Key] = VARIABLE.Value;
                }
            }
        }

*/

    }



//셋팅
    // ReSharper disable Unity.PerformanceAnalysis
    public void SaveExpData()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}adLv", PlayerBackendData.Instance.GetAdLv());
        _es3File.Save($"{PlayerBackendData.Instance.Id}AchLv", PlayerBackendData.Instance.GetAchLv());
        _es3File.Save($"{PlayerBackendData.Instance.Id}AchExp", PlayerBackendData.Instance.GetAchExp());
        _es3File.Save($"{PlayerBackendData.Instance.Id}Altar_Lvs", PlayerBackendData.Instance.GetAllAltarlv());
    }

    public void SaveFieldEnd()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}InventoryItem", PlayerBackendData.Instance.ItemInventory);
        _es3File.Save($"{PlayerBackendData.Instance.Id}Money", PlayerBackendData.Instance.GetMoney());
        _es3File.Save($"{PlayerBackendData.Instance.Id}Exp", PlayerBackendData.Instance.GetExp());
        Save();
    }
    
    public void SaveOnlyExp()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}Exp", PlayerBackendData.Instance.GetExp());
        Save();
    }

    public void SaveOnlyLv()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}Lv", PlayerBackendData.Instance.GetLv());
        _es3File.Save($"{PlayerBackendData.Instance.Id}Exp", PlayerBackendData.Instance.GetExp());

    }

    public void LoadExpData()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}Exp"))
        {
            PlayerBackendData.Instance.AddExp(_es3File.Load<decimal>($"{PlayerBackendData.Instance.Id}Exp"));
        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}Lv"))
        {
            PlayerBackendData.Instance.SetLv(_es3File.Load<int>($"{PlayerBackendData.Instance.Id}Lv"));
        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}adLv"))
        {
            PlayerBackendData.Instance.SetAdLv(_es3File.Load<int>($"{PlayerBackendData.Instance.Id}adLv"));
        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}AchExp"))
        {
            PlayerBackendData.Instance.AddAchExp(_es3File.Load<decimal>($"{PlayerBackendData.Instance.Id}AchExp"));
        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}AchLv"))
        {
            PlayerBackendData.Instance.SetAchLv(_es3File.Load<int>($"{PlayerBackendData.Instance.Id}AchLv"));
        }

        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}AltarLv"))
        {
            PlayerBackendData.Instance.SetAltarLv(_es3File.Load<int>($"{PlayerBackendData.Instance.Id}AltarLv"));
        }
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}Altar_Lvs"))
        {
            PlayerBackendData.Instance.Altar_Lvs = (_es3File.Load<int[]>($"{PlayerBackendData.Instance.Id}Altar_Lvs"));
        }
        else
        {
            if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}AltarLv"))
            {
                PlayerBackendData.Instance.Altar_Lvs[0] = _es3File.Load<int>($"{PlayerBackendData.Instance.Id}AltarLv");
            }
         
        }
    }
    public void SaveLoginTime()
    {
        _es3File.Save($"{PlayerBackendData.Instance.Id}LoginTimeSecToday", Timemanager.Instance.LoginTimeSecToday);
        _es3File.Save($"{PlayerBackendData.Instance.Id}LoginTimeSecEvery", Timemanager.Instance.LoginTimeSecEvery);
    }
 
    public void LoadLoginTime()
    {
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}LoginTimeSecToday"))
        {
            Timemanager.Instance.LoginTimeSecToday =(_es3File.Load<int>($"{PlayerBackendData.Instance.Id}LoginTimeSecToday"));
        }
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}LoginTimeSecEvery"))
        {
            Timemanager.Instance.LoginTimeSecEvery = (_es3File.Load<int>($"{PlayerBackendData.Instance.Id}LoginTimeSecEvery"));
        }
    }
    public bool LoadCheckSaveTime()
    {

        //둘의 시간이 True여야 한다.
        string time1 = "";
        string time2 = "";
        if (_es3File.KeyExists($"{PlayerBackendData.Instance.Id}SavedCheckTime"))
        {
            try
            {
                time1 = _es3FileSystem.Load<string>($"{PlayerBackendData.Instance.Id}SavedCheckTime");
            }
            catch (Exception e)
            {
                BackendFerderationAuth.Instance.FileDifferentobj.SetActive(true);
                throw;
            }
        }
        else
        {
            //일단 없으니 봐준다.
            return true;
        }

        if (_es3FileSystem.KeyExists($"{PlayerBackendData.Instance.Id}SavedCheckTime"))
        {
            time2 = _es3FileSystem.Load<string>($"{PlayerBackendData.Instance.Id}SavedCheckTime", GetFileNameTimeCheck());
        }
        else
        {
            return false;
        }

        Debug.Log("클라시간" + time1);
        Debug.Log("저장시간" + time2);
        if (time1 != time2)
            return false;
        else
        {
            return true;
        }
    }

    
    
    public void Save()
    {
        SaveCheckTime();
        //ES3.StoreCachedFile();
        _es3File.Sync();
        _es3FileSystem.Sync();
        ES3.CreateBackup(GetFileName());
        ES3.CreateBackup(GetFileNameTimeCheck());
    }

    public ES3Settings settings;
 

    private string filename  = "";
    private string filename2;
    public string GetFileName()
    {
        return $"{PlayerBackendData.Instance.nickname}3.es3";
    }

    public string GetFileName2()
    {
        return $"{PlayerBackendData.Instance.nickname}3.es3";
    }

    public void SaveCheckTime()
    {
        PlayerBackendData.Instance.SavedCheckTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        _es3FileSystem.Save($"{PlayerBackendData.Instance.Id}SavedCheckTime", PlayerBackendData.Instance.SavedCheckTime);
//system
        _es3File.Save($"{PlayerBackendData.Instance.Id}SavedCheckTime", PlayerBackendData.Instance.SavedCheckTime);

      
    }

    public string GetFileNameTimeCheck()
    {
        return $"systemlog.es3";
    }

    #region 플레이어서버데이터

    private string gameDataRowInDate = string.Empty;
    [SerializeField] PlayerBackendData userData;


    private void InsertRankData()
    {
        Param param = new Param
        {
            { "AltarWar", 0 },
            { "Level", PlayerBackendData.Instance.GetLv() },
            { "BattlePoint", 0 },
            { "Dojun", 0 },
            { "TrainTotalDmg", 0 }
        };

        var bro = Backend.GameData.Insert("RankData", param);

        if (bro.IsSuccess())
        {
            Debug.Log("랭킹 데이터 삽입에 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("게임정보 데이터 삽입에 실패했습니다. : " + bro);
        }
    }

    //5분에 한번씩
    public void StartAutoSave()
    {
        if (Timemanager.Instance.OncePremiumPackage.Contains("1021"))
        {
            PlayerBackendData.Instance.isadsfree = true;
            SaveadsFree();
            Save();
        }
            StartCoroutine(AutoSaveToServer());
            StartCoroutine(AutoSaveToServer_Collect());
    }

    //600분에 한 번씩 한다.
    private WaitForSeconds autowait = new WaitForSeconds(1200f);
    private WaitForSeconds autowait2 = new WaitForSeconds(1800f);
    private WaitForSeconds autowait1 = new WaitForSeconds(2f);
    public DateTime hackchecksavedtime;

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator AutoSaveToServer()
    {
        yield return autowait1;
        UpdatePlayerData();
        yield return autowait;
    }

    private IEnumerator AutoSaveToServer_Collect()
    {
        yield return autowait2;

        Param paramCollect = new Param
        {
            //가방
            { "Achievement", PlayerBackendData.Instance.PlayerAchieveData },
        };

        LogManager.LogSaveCollectData_Auto(paramCollect);
        yield return autowait;
    }

    private void UpdatePlayerData()
    {
        Settingmanager.Instance.SaveDataALl(false);
    }


    public void GameDataInsert_ToServer()
    {
        Debug.Log("데이터를 초기화합니다.");

        Debug.Log("뒤끝 업데이트 목록에 해당 데이터들을 추가합니다.");
        Param param = new Param();
        //레벨


        param.Add("level", userData.GetLv());

        param.Add("level_ach", userData.GetAchLv());
        param.Add("levelExp", userData.GetExp());
        param.Add("level_achExp", userData.GetAchExp());
        //param.Add("level_altar", userData.GetAltarlv());
        param.Add("Altar_Lvs", userData.GetAllAltarlv());
        param.Add("level_adven", userData.GetAdLv());
        param.Add("Gold", userData.GetMoney());
        param.Add("Crystal", userData.GetCash());
        //보유 스킬
        param.Add("SkillInven", userData.Skills);


        //클래스 데이타
        param.Add("Class", userData.ClassData);
        param.Add("NowClass", userData.ClassData[userData.ClassId]);


        //장비 
        param.Add("equipment_Weapon", userData.Equiptment0);
        param.Add("equipment_SubWeapon", userData.Equiptment1);
        param.Add("equipment_Helmet", userData.Equiptment2);
        param.Add("equipment_Armor", userData.Equiptment3);
        param.Add("equipment_Glove", userData.Equiptment4);
        param.Add("equipment_Boot", userData.Equiptment5);
        param.Add("equipment_Ring", userData.Equiptment6);
        param.Add("equipment_Necklace", userData.Equiptment7);
        param.Add("equipment_Wing", userData.Equiptment8);
        param.Add("equipment_Pet", userData.Equiptment9);
        param.Add("equipment_Rune", userData.Equiptment10);
        param.Add("equipment_Insignia", userData.Equiptment11);
        param.Add("EquipmentNow", userData.EquipEquiptment0);


        
        
        param.Add("inventory", userData.ItemInventory);


        Debug.Log("게임정보 데이터 삽입을 요청합니다.");
        var bro = Backend.GameData.Insert("PlayerData", param);

        if (bro.IsSuccess())
        {
            Debug.Log("게임정보 데이터 삽입에 성공했습니다. : " + bro);

            //삽입한 게임정보의 고유값입니다.
            gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError("게임정보 데이터 삽입에 실패했습니다. : " + bro);
        }

        InsertRankData();
    }


public bool GameDataGet()
    {
        // Step 3. 게임정보 불러오기 구현하기
        Debug.Log("게임 정보 조회 함수를 호출합니다.");
        var bro = Backend.GameData.GetMyData("PlayerData", new Where());
        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 조회에 성공했습니다. : " + bro);

            

            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json으로 리턴된 데이터를 받아옵니다.

            // 받아온 데이터의 갯수가 0이라면 데이터가 존재하지 않는 것입니다.
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("데이터가 존재하지 않습니다.");
            }
            else
            {
                gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //불러온 게임정보의 고유값입니다.
                //레벨
                userData.SetLv(int.Parse(gameDataJson[0]["level"].ToString()));
                userData.AddExp(decimal.Parse(gameDataJson[0]["levelExp"].ToString()));
                userData.SetAchLv(int.Parse(gameDataJson[0]["level_ach"].ToString()));
                userData.AddAchExp(decimal.Parse(gameDataJson[0]["level_achExp"].ToString()));
                //제단
                if (gameDataJson[0].ContainsKey("Altar_Lvs"))
                {
                    //클래스 데이터
                    for (int i = 0; i < gameDataJson[0]["Altar_Lvs"].Count; i++)
                    {
                        //  Debug.Log("직업입력" + key);
                        PlayerBackendData.Instance.Altar_Lvs[i] = int.Parse(gameDataJson[0]["Altar_Lvs"][i].ToString());
                    }
                }
                else
                {
                    userData.SetAltarLv(int.Parse(gameDataJson[0]["level_altar"].ToString()));
                    PlayerBackendData.Instance.Altar_Lvs[0] = int.Parse(gameDataJson[0]["level_altar"].ToString());
                }


                userData.SetAdLv(int.Parse(gameDataJson[0]["level_adven"].ToString()));
                userData.SetFieldLV(int.Parse(gameDataJson[0]["FieldLv"].ToString()));

                //Debug.Log(gameDataJson[0]["inventory"]);

                //인벤토리 가져오기
                userData.ItemInventory.Clear();
                for (int i = 0; i < gameDataJson[0]["inventory"].Count; i++)
                {
                    userData.ItemInventory.Add(new ItemInven(gameDataJson[0]["inventory"][i]["Id"].ToString(),
                        int.Parse(gameDataJson[0]["inventory"][i]["Howmany"].ToString()),
                        gameDataJson[0]["inventory"][i]["Expiredate"].ToString()));
                }



                for (int i = 0; i < gameDataJson[0]["EquipmentNow"].Count; i++)
                {
                    if (gameDataJson[0]["EquipmentNow"][i] != null)
                    {
                        try
                        {
                            //Debug.Log("키는");
                            //Debug.Log(gameDataJson[0]["EquipmentNow"][i]["Itemid"].ToString());
                            userData.EquipEquiptment0[i] = new EquipDatabase(gameDataJson[0]["EquipmentNow"][i]);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }



                //장비 가져오기
                userData.Equiptment0.Clear();
                foreach (string key in gameDataJson[0]["equipment_Weapon"].Keys)
                {
                    userData.Equiptment0.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Weapon"][key]));
                }

                userData.Equiptment1.Clear();
                foreach (string key in gameDataJson[0]["equipment_SubWeapon"].Keys)
                {
                    userData.Equiptment1.Add(key, new EquipDatabase(gameDataJson[0]["equipment_SubWeapon"][key]));
                }

                userData.Equiptment2.Clear();
                foreach (string key in gameDataJson[0]["equipment_Helmet"].Keys)
                {
                    userData.Equiptment2.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Helmet"][key]));
                }

                userData.Equiptment3.Clear();
                foreach (string key in gameDataJson[0]["equipment_Armor"].Keys)
                {
                    userData.Equiptment3.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Armor"][key]));
                }

                userData.Equiptment4.Clear();
                foreach (string key in gameDataJson[0]["equipment_Glove"].Keys)
                {
                    userData.Equiptment4.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Glove"][key]));
                }

                userData.Equiptment5.Clear();
                foreach (string key in gameDataJson[0]["equipment_Boot"].Keys)
                {
                    userData.Equiptment5.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Boot"][key]));
                }

                userData.Equiptment6.Clear();
                foreach (string key in gameDataJson[0]["equipment_Ring"].Keys)
                {
                    userData.Equiptment6.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Ring"][key]));
                }

                userData.Equiptment7.Clear();
                foreach (string key in gameDataJson[0]["equipment_Necklace"].Keys)
                {
                    userData.Equiptment7.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Necklace"][key]));
                }

                userData.Equiptment8.Clear();
                if (gameDataJson[0].ContainsKey("equipment_Wing"))
                {
                    foreach (string key in gameDataJson[0]["equipment_Wing"].Keys)
                    {
                        userData.Equiptment8.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Wing"][key]));
                    }
                }

                userData.Equiptment9.Clear();
                if (gameDataJson[0].ContainsKey("equipment_Pet"))
                {
                    foreach (string key in gameDataJson[0]["equipment_Pet"].Keys)
                    {
                        userData.Equiptment9.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Pet"][key]));
                    }
                }

                userData.Equiptment10.Clear();
                if (gameDataJson[0].ContainsKey("equipment_Rune"))
                {
                    foreach (string key in gameDataJson[0]["equipment_Rune"].Keys)
                    {
                        userData.Equiptment10.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Rune"][key]));
                    }
                }

                userData.Equiptment11.Clear();
                if (gameDataJson[0].ContainsKey("equipment_Insignia"))
                {
                    foreach (string key in gameDataJson[0]["equipment_Insignia"].Keys)
                    {
                        userData.Equiptment11.Add(key, new EquipDatabase(gameDataJson[0]["equipment_Insignia"][key]));
                    }
                }

                //스킬
                if (gameDataJson[0].ContainsKey("SkillInven"))
                {
                    //클래스 데이터
                    for(int i = 0 ; i < gameDataJson[0]["SkillInven"].Count;i++)
                    {
                        //  Debug.Log("직업입력" + key);
                        PlayerBackendData.Instance.Skills.Add(gameDataJson[0]["SkillInven"][i].ToString());
                    }
                }

                //버그로 없다면
                if (PlayerBackendData.Instance.Skills.Count.Equals(0))
                {
                    foreach (var VARIABLE in PlayerBackendData.Instance.ClassData)
                    {
                        if (!VARIABLE.Value.Isown) continue;
                            PlayerBackendData.Instance.Skills.Add(ClassDB.Instance.Find_id(VARIABLE.Value.ClassId1)
                                .giveskill);
                    }
                }

                //클래스 데이터
                foreach (string key in gameDataJson[0]["Class"].Keys)
                {
                    //  Debug.Log("직업입력" + key);
                    PlayerBackendData.Instance.ClassData[key] = new ClassDatabase(gameDataJson[0]["Class"][key]);
                }
             

            /*   //업적
                if (gameDataJson[0].ContainsKey("Achievement"))
                {
                    foreach (string key in gameDataJson[0]["Achievement"].Keys)
                    {
                        //  Debug.Log("직업입력" + key);
                        PlayerBackendData.Instance.PlayerAchieveData[key] = new Achievedata(gameDataJson[0]["Achievement"][key]);
                    }
                }*/
                //퀘스트
                if (gameDataJson[0].ContainsKey("QuestCount"))
                {
                    for (int i = 0; i < gameDataJson[0]["QuestCount"].Count; i++)
                    {
                        PlayerBackendData.Instance.QuestCount[i] = float.Parse(gameDataJson[0]["QuestCount"][i].ToString());
                    }
                    for (int i = 0; i < gameDataJson[0]["QuestIsFinish"].Count; i++)
                    {
                        PlayerBackendData.Instance.QuestIsFinish[i] = bool.Parse(gameDataJson[0]["QuestIsFinish"][i].ToString());
                    }
                    for (int i = 0; i < gameDataJson[0]["QuestTotalCount"].Count; i++)
                    {
                        PlayerBackendData.Instance.QuestTotalCount[i] = float.Parse(gameDataJson[0]["QuestTotalCount"][i].ToString());
                    }
                }

                //탈리스만
                if (gameDataJson[0].ContainsKey("TalismanData"))
                {
                    userData.TalismanData.Clear();
                    foreach (string key in gameDataJson[0]["TalismanData"].Keys)
                    {
                        Debug.Log(gameDataJson[0]["TalismanData"][key]);
                        userData.TalismanData.Add(key, new Talismandatabase(gameDataJson[0]["TalismanData"][key]));
                    }

                    for (int i = 0; i < gameDataJson[0]["TalismanPreset"].Count; i++)
                    {
                        if(gameDataJson[0]["TalismanPreset"][i] != null)
                        PlayerBackendData.Instance.TalismanPreset[i] = new PresetTalisman(gameDataJson[0]["TalismanPreset"][i]);
                    }

                    PlayerBackendData.Instance.nowtalismanpreset = int.Parse(gameDataJson[0]["nowtalismanpreset"].ToString());
                }


                //  Debug.Log("클래스 아이디" + gameDataJson[0]["NowClass"]["ClassId1"].ToString());
                PlayerBackendData.Instance.ClassId = gameDataJson[0]["NowClass"]["ClassId1"].ToString();

                if (gameDataJson[0].ContainsKey("Stage"))
                {
                    PlayerBackendData.Instance.nowstage = gameDataJson[0]["Stage"].ToString();
                    PlayerBackendData.Instance.spawncount = (int.Parse(gameDataJson[0]["FieldCount"].ToString()));
                }
                else
                {
                    PlayerBackendData.Instance.nowstage = "1000";
                    PlayerBackendData.Instance.spawncount = 1;

                }
                
                if (gameDataJson[0].ContainsKey("Gold"))
                {
                    PlayerBackendData.Instance.SetMoney(decimal.Parse(gameDataJson[0]["Gold"].ToString()));
                    PlayerBackendData.Instance.SetCash(decimal.Parse(gameDataJson[0]["Crystal"].ToString()));
                    
                }
                if (gameDataJson[0].ContainsKey("LoginTimeSecToday"))
                {
                    Timemanager.Instance.LoginTimeSecToday = int.Parse(gameDataJson[0]["LoginTimeSecToday"].ToString());
                    Timemanager.Instance.LoginTimeSecEvery = int.Parse(gameDataJson[0]["LoginTimeSecEvery"].ToString());
                    
                }
                if (gameDataJson[0].ContainsKey("OnceShopData"))
                {
                    //클래스 데이터
                    for(int i = 0 ; i < gameDataJson[0]["OnceShopData"].Count;i++)
                    {
                        //  Debug.Log("직업입력" + key);
                        Timemanager.Instance.OncePremiumPackage.Add(gameDataJson[0]["OnceShopData"][i].ToString());
                    }
                }
                //ㄱ1회 구매 상점
                if (gameDataJson[0].ContainsKey("OnceTradeData"))
                {
                    //클래스 데이터
                    for (int i = 0; i < gameDataJson[0]["OnceTradeData"].Count; i++)
                    {
                        if (!Timemanager.Instance.OnceTradePackage.Contains(gameDataJson[0]["OnceTradeData"][i]
                                .ToString()))
                            Timemanager.Instance.OnceTradePackage.Add(gameDataJson[0]["OnceTradeData"][i].ToString());
                    }
                }


                //ㄱ1회 구매 상점
                if (gameDataJson[0].ContainsKey("Ability"))
                {
                    Debug.Log("어빌리티 불러옴");
                    //클래스 데이터
                    for(int i = 0 ; i < gameDataJson[0]["Ability"].Count;i++)
                    {
                        PlayerBackendData.Instance.Abilitys[i] = gameDataJson[0]["Ability"][i].ToString();
                    }
                }
                
                //ㄱ1회 구매 상점
                if (gameDataJson[0].ContainsKey("Passive"))
                {
                    //클래스 데이터
                    for(int i = 0 ; i < gameDataJson[0]["Passive"].Count;i++)
                    {
                        PlayerBackendData.Instance.PassiveClassId[i] = gameDataJson[0]["Passive"][i].ToString();
                    }
                }

                
                //ㄱ1회 구매 상점
                if (gameDataJson[0].ContainsKey("playeravata"))
                {
                    //클래스 데이터
                    for(int i = 0 ; i < gameDataJson[0]["playeravata"].Count;i++)
                    {
                        PlayerBackendData.Instance.playeravata[i] = bool.Parse(gameDataJson[0]["playeravata"][i].ToString());
                    }
                    PlayerBackendData.Instance.avata_weapon = gameDataJson[0]["avata_weapon"].ToString();
                    PlayerBackendData.Instance.avata_subweapon = gameDataJson[0]["avata_subweapon"].ToString();
                    PlayerBackendData.Instance.avata_avata = gameDataJson[0]["avata_avata"].ToString();
                }
             
                //TutoId
                if (gameDataJson[0].ContainsKey("Tutoid"))
                {
                    //클래스 데이터
                    PlayerBackendData.Instance.tutoid = gameDataJson[0]["Tutoid"].ToString();
                }
                //TutoId
                if (gameDataJson[0].ContainsKey("tutoguideid"))
                {
                    //클래스 데이터
                    PlayerBackendData.Instance.tutoguideid = int.Parse(gameDataJson[0]["tutoguideid"].ToString());
                }
                //TutoId
                if (gameDataJson[0].ContainsKey("tutoguideisfinish"))
                {
                    //클래스 데이터
                    PlayerBackendData.Instance.tutoguideisfinish = bool.Parse(gameDataJson[0]["tutoguideisfinish"].ToString());
                }
                //TutoId
                if (gameDataJson[0].ContainsKey("tutoguidepremium"))
                {
                    //클래스 데이터
                    PlayerBackendData.Instance.tutoguidepremium = bool.Parse(gameDataJson[0]["tutoguidepremium"].ToString());
                }

                //데이터 저장시간
                if (gameDataJson[0].ContainsKey("SavedCheckTime"))
                {
                    //클래스 데이터
                    PlayerBackendData.Instance.SavedCheckTime = gameDataJson[0]["SavedCheckTime"].ToString();
                }
                //광고
                if (gameDataJson[0].ContainsKey("AdsFree"))
                {
                    //클래스 데이터
                    PlayerBackendData.Instance.isadsfree = bool.Parse(gameDataJson[0]["AdsFree"].ToString());
                }
                if ((gameDataJson[0].ContainsKey("craftmakingid")))
                {
                    //저장기능
                    for (int i = 0; i < gameDataJson[0]["craftmakingid"].Count; i++)
                    {
                        if (gameDataJson[0]["craftmakingid"][i] != null)
                        {
                            try
                            {
                                PlayerBackendData.Instance.craftmakingid[i] =
                                    gameDataJson[0]["craftmakingid"][i].ToString();

                                PlayerBackendData.Instance.craftdatetime[i] =
                                    gameDataJson[0]["craftdatetime"][i].ToString();

                                PlayerBackendData.Instance.craftdatecount[i] =
                                    int.Parse(gameDataJson[0]["craftdatecount"][i].ToString());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                    }
                }
                
                if (gameDataJson[0].ContainsKey("Roulette"))
                {
//                    Debug.Log("룰렛가져옴");
                    for (int i = 0; i < gameDataJson[0]["Roulette"].Count; i++)
                    {
                        PlayerBackendData.Instance.RouletteCount[i] =
                            int.Parse(gameDataJson[0]["Roulette"][i].ToString());
                    }
                }
                
                string loadstring = Timemanager.Instance.GetServerTime().ToString();
                Param param = new Param
                {
                    //레벨
                    { "PlayerCanLoadBool", "false" },
                    { "PlayerLoadUID", SystemInfo.deviceUniqueIdentifier }, 
                    { "PlayerLoadModel", SystemInfo.deviceModel }, //모델
                    { "PlayerLoadTime", loadstring} //불러온시간
                };
                
                PlayerBackendData.Instance.playersaveindate = loadstring;
                
                _es3File.Clear();
               // _es3FileSystem.Clear();
               Savemanager.Instance.SaveSaveNum2();
              SaveEvery();
                Save();
                LogManager.LogLoad();
                // key 컬럼의 값이 keyCode인 데이터 검색
                Where where = new Where();
                where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
            
                SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, ( callback ) =>
                {
                    // 이후 처리
                    if (!callback.IsSuccess()) return;

                });
                
            return true;

            }
            return false;

        }
        else
        {
            Debug.LogError("게임 정보 조회에 실패했습니다. : " + bro);
            return false;
        }
    }
    public void LevelUp()
    {
        // Step 4. 게임정보 수정 구현하기
    }

    public void GameDataUpdate()
    {
        // Step 4. 게임정보 수정 구현하기
    }

    #endregion

    
    public void BtOnLogOut()
    {
        BackendReturnObject BRO = Backend.BMember.Logout();

        if (BRO.IsSuccess())
        {
            LogoutGoogle();
        }
    }

     void LogoutGoogle()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated() == true)
        {
            BackendFerderationAuth.Instance.RemovingPanel.SetActive(false);
            PlayGamesPlatform.Instance.SignOut();
            PlayGamesPlatform.Activate();
        }
    }
}