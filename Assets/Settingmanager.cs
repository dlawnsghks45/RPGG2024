using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using BackEnd.Tcp;
using Doozy.Engine.Soundy;
using Doozy.Engine.UI;
using Doozy.Engine.Utils.ColorModels;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using JetBrains.Annotations;
using Newtonsoft.Json;

public class Settingmanager : MonoBehaviour
{
//싱글톤만들기.
    private static Settingmanager _instance = null;

    public static Settingmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Settingmanager)) as Settingmanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }



   
    //계정 설정
    public Text GamerIndateText;


    public void Bt_CopyIndate()
    {
        UniClipboard.SetText(GamerIndateText.text); //클립보드에 텍스트 복사
        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/설정_회원번호복사성공"), alertmanager.alertenum.일반);
    }

    private void Start()
    {
        GamerIndateText.text = PlayerBackendData.Instance.playerindate;
        LoadNextFieldToggle();
        //리프레쉬토큰 가지기
        StartCoroutine(RefreshToken());
        CheckDailyCount();
        CheckOffLine();
    }

    public void Bt_OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void Bt_CouponURL()
    {
        string str =
            $"https://storage.thebackend.io/1ea3f14d34e89530ea88b3245bc82dc17d5f52ce1554049f19fce9219a847cfce18bb8891bcea99fe737b8b9a3b9d18d6cc5513c1dd400f79c0e0fa6bc88f3eb49303e222268864de6987842/coupon.html?lng=ko&uid={Backend.UID}";
        Application.OpenURL(str);
    }

    public UIView panel;

    public void ShowNotice()
    {
        uimanager.Instance.AddUiview(panel, true);
    }

    #region 서버저장

    public Text RecentServerDateTimeText;
    public Text RecentServerDataCanLoadText; //불러올수있는지 알 수 있는 테이블
    public GameObject Loadingbar_savedata;
    public GameObject PlayerServerDatapanel;
    private bool initserverdata = false;
    private DateTime Savedtime;


    public void RefreshServerSavedData()
    {
        if (initserverdata)

        PlayerServerDatapanel.SetActive(false);
        Loadingbar_savedata.SetActive(true);
        initserverdata = true;

        //데이터를 받아온다.
        Where where1 = new Where();
        where1.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

        string[] select =
        {
            "PlayerSaveTime",
            "PlayerCanLoadBool",
        };

        SendQueue.Enqueue(Backend.GameData.GetMyData, "PlayerData", where1, select, 1, bro =>
        {
            Debug.Log(bro);
            if (bro.IsSuccess())
            {
                PlayerServerDatapanel.SetActive(true);
                Loadingbar_savedata.SetActive(false);
                //시간 가져오기
                JsonData data = bro.FlattenRows()[0];
                if (data.ContainsKey("PlayerSaveTime"))
                {
                    RecentServerDateTimeText.text = DateTime.Parse(data["PlayerSaveTime"].ToString())
                        .ToString("yyyy-MM-dd HH:mm:ss");
                    Savedtime = DateTime.Parse(data["PlayerSaveTime"].ToString());
                }
                else
                {
                    RecentServerDateTimeText.text = "-";
                }

                RecentServerDataCanLoadText.text = data.ContainsKey("PlayerCanLoadBool")
                    ? string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"),
                        bool.Parse(data["PlayerCanLoadBool"].ToString())
                            ? "<Color=lime>On</color>"
                            : "<Color=red>Off</color>")
                    : string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"), "<Color=red>Off</color>");
                //Debug.Log("불러옴 성공");
            }
            else
            {
                PlayerServerDatapanel.SetActive(true);
                Loadingbar_savedata.SetActive(false);
                //Debug.Log("실패");
            }
        });

    }

    public void Bt_SaveData()
    {

        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/사냥터만가능"), alertmanager.alertenum.주의);
            return;
        }

        if (!Timemanager.Instance.ConSumeCount_DailyAscny((int)Timemanager.ContentEnumDaily.일일저장가능횟수, 1))
        {
            return;
        }

        DateTime nowtime = Timemanager.Instance.GetServerTime();

        if (!CheckServerOn())
        {
            return;
        }

        SaveDataALl(true);
        TutorialTotalManager.Instance.CheckGuideQuest("savedata");
    }

    public void SaveDataInventory()
    {
        PlayerBackendData userData = PlayerBackendData.Instance;
        Param paramEquip = new Param();
        Savemanager.Instance.SaveSaveNum();
        //셋팅필수
        paramEquip.Add("EquipmentNow", userData.EquipEquiptment0);
        paramEquip.Add("Gold", userData.GetMoney());
        paramEquip.Add("Crystal", userData.GetCash());
        paramEquip.Add("avata_avata", userData.avata_avata);
        paramEquip.Add("avata_weapon", userData.avata_weapon);
        paramEquip.Add("avata_subweapon", userData.avata_subweapon);
        paramEquip.Add("playeravata", userData.playeravata);
        paramEquip.Add("inventory", userData.ItemInventory);
        paramEquip.Add("Roulette", userData.RouletteCount);
        paramEquip.Add("SaveNum", userData.ClientSaveNum);

        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramEquip, (callback) =>
        {
            // 이후 처리
            if (!callback.IsSuccess())
            {
                PlayerBackendData.Instance.ClientSaveNum--;
                Savemanager.Instance.SaveSaveNum2();
                Savemanager.Instance.Save();
                return;
            }
            LogManager.LogSaveInven();
            Savemanager.Instance.Save();

        });
    }
    public void SaveDataAchieveInven()
    {
        PlayerBackendData userData = PlayerBackendData.Instance;
        Savemanager.Instance.SaveSaveNum();
        Param paramEquip = new Param
        {
            //셋팅필수
            { "Achievement", PlayerBackendData.Instance.PlayerAchieveData },
            { "Gold", userData.GetMoney() },
            { "Crystal", userData.GetCash() },
            { "avata_avata", userData.avata_avata },
            { "avata_weapon", userData.avata_weapon },
            { "avata_subweapon", userData.avata_subweapon },
            { "playeravata", userData.playeravata },
            { "inventory", userData.ItemInventory },
            { "Tutoid", PlayerBackendData.Instance.tutoid },
        { "tutoguideid", PlayerBackendData.Instance.tutoguideid },
        { "tutoguideisfinish", PlayerBackendData.Instance.tutoguideisfinish },
        { "tutoguidepremium", PlayerBackendData.Instance.tutoguidepremium },
        { "SaveNum", PlayerBackendData.Instance.ClientSaveNum }
        };
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramEquip, (callback) =>
        {
            // 이후 처리
            if (!callback.IsSuccess())
            {
                PlayerBackendData.Instance.ClientSaveNum--;
                Savemanager.Instance.SaveSaveNum2();
                Savemanager.Instance.Save();
                return;
            }
            
            LogManager.LogSaveInven();
            Savemanager.Instance.Save();

        });
    }
    public GameObject ServerDataLoad_Lv;
    public Text ServerDataLoad_Text;

    public void Bt_ServerDataLoadStart()
    {
        PlayerBackendData userData = PlayerBackendData.Instance;
        Savemanager.Instance.SaveSaveNum();
        Param param = new Param
        {
            { "PlayerCanLoadBool", "trues" },
            { "SaveNum", PlayerBackendData.Instance.ClientSaveNum }

        };
        

        
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
        {
            if (!callback.IsSuccess())
            {
                PlayerBackendData.Instance.ClientSaveNum--;
                Savemanager.Instance.SaveSaveNum2();
                Savemanager.Instance.Save();
                return;
            }
            Savemanager.Instance.Save();
            Application.Quit();

        });
    }

    public void SaveTutoData()
    {
        PlayerBackendData userData = PlayerBackendData.Instance;
        Savemanager.Instance.SaveSaveNum();
        Param param = new Param
        {
            { "Gold", userData.GetMoney() }, 
            { "Crystal", userData.GetCash() },
            { "avata_avata", userData.avata_avata },
            { "avata_weapon", userData.avata_weapon },
            { "avata_subweapon", userData.avata_subweapon },
            { "playeravata", userData.playeravata },
            //레벨
            { "level", userData.GetLv() },
            { "levelExp", userData.GetExp() },
            //업적레벨
            { "level_ach", userData.GetAchLv() },
            { "level_achExp", userData.GetAchExp() },
            //제단레벨
            { "Altar_Lvs", userData.GetAllAltarlv() },
            //랭크
            { "level_adven", userData.GetAdLv() },
            //보유 스킬
            { "SkillInven", userData.Skills },
            //클래스 데이타
            { "Class", userData.ClassData },
            { "NowClass", userData.ClassData[userData.ClassId] },
            //패시브
            { "Passive", userData.PassiveClassId },
            //플레이어 스탯 정보
            { "Playerstat", PlayerData.Instance.GetPlayerStatForSave() },
         
            //가방
            { "inventory", userData.ItemInventory },
            //전투력
            { "BattlePoint", (int)userData.GetBattlePoint() },
            //현재맵
            { "NowMap", mapmanager.Instance.savemapid },
            //스테이지
            { "Stage", PlayerBackendData.Instance.nowstage },
            { "FieldCount", PlayerBackendData.Instance.spawncount },
            { "FieldLv", PlayerBackendData.Instance.GetFieldLv() },
            //제작
            { "craftmakingid", PlayerBackendData.Instance.craftmakingid },
            { "craftdatetime", PlayerBackendData.Instance.craftdatetime },
            { "craftdatecount", PlayerBackendData.Instance.craftdatecount },
            { "Ability", PlayerBackendData.Instance.Abilitys },
            { "Roulette", PlayerBackendData.Instance.RouletteCount },
            //소탕정보
            { "sotang_raid", PlayerBackendData.Instance.sotang_raid },
            { "sotang_dungeon", PlayerBackendData.Instance.sotang_dungeon },
            //가이드
            { "Tutoid", PlayerBackendData.Instance.tutoid },
            { "tutoguideid", PlayerBackendData.Instance.tutoguideid },
            { "tutoguideisfinish", PlayerBackendData.Instance.tutoguideisfinish },
            { "tutoguidepremium", PlayerBackendData.Instance.tutoguidepremium },
            
            { "LoginTimeSecToday", Timemanager.Instance.LoginTimeSecToday },
            { "LoginTimeSecEvery", Timemanager.Instance.LoginTimeSecEvery },
            { "SaveNum", PlayerBackendData.Instance.ClientSaveNum }
        };
        
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
        {
            Debug.Log(callback);
            if(callback.IsSuccess())
                Savemanager.Instance.Save();
            else
            {
                PlayerBackendData.Instance.ClientSaveNum--;
                Savemanager.Instance.SaveSaveNum2();
                Savemanager.Instance.Save();
            }
        });
    }

    public void SaveSotangs()
    {
         PlayerBackendData userData = PlayerBackendData.Instance;

         PlayerBackendData.Instance.sotang_raid = PlayerBackendData.Instance.sotang_raid.Distinct().ToList();
         PlayerBackendData.Instance.sotang_dungeon = PlayerBackendData.Instance.sotang_dungeon.Distinct().ToList();
        Param param = new Param
        {
            { "sotang_raid", PlayerBackendData.Instance.sotang_raid },
            { "sotang_dungeon", PlayerBackendData.Instance.sotang_dungeon },
            { "LoginTimeSecToday", Timemanager.Instance.LoginTimeSecToday },
            { "LoginTimeSecEvery", Timemanager.Instance.LoginTimeSecEvery },
        };
        
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
        {
            Debug.Log(callback);
        });
    }

    public void SaveAllNoLog()
    {
        PlayerBackendData userData = PlayerBackendData.Instance;
        //데이터는 3시간에 한번 저장된다.
        //최근 저장시간을 기준으로 잡느다.
        Savemanager.Instance.SaveSaveNum();
        Param param = new Param
        {
            { "Gold", userData.GetMoney() }, 
            { "Crystal", userData.GetCash() },
            { "avata_avata", userData.avata_avata },
            { "avata_weapon", userData.avata_weapon },
            { "avata_subweapon", userData.avata_subweapon },
            { "playeravata", userData.playeravata },
            //레벨
            { "level", userData.GetLv() },
            { "levelExp", userData.GetExp() },
            //업적레벨
            { "level_ach", userData.GetAchLv() },
            { "level_achExp", userData.GetAchExp() },
            //제단레벨
            { "Altar_Lvs", userData.GetAllAltarlv() },
            //랭크
            { "level_adven", userData.GetAdLv() },
            //보유 스킬
            { "SkillInven", userData.Skills },
            //클래스 데이타
            { "Class", userData.ClassData },
            { "NowClass", userData.ClassData[userData.ClassId] },
            //패시브
            { "Passive", userData.PassiveClassId },
            //플레이어 스탯 정보
            { "Playerstat", PlayerData.Instance.GetPlayerStatForSave() },
         
            //가방
            { "inventory", userData.ItemInventory },
            //전투력
            { "BattlePoint", (int)userData.GetBattlePoint() },
            //현재맵
            { "NowMap", mapmanager.Instance.savemapid },
            //일일횟수
            { "Playertime", userData.PlayerTimes },
            //스테이지
            { "Stage", PlayerBackendData.Instance.nowstage },
            { "FieldCount", PlayerBackendData.Instance.spawncount },
            { "FieldLv", PlayerBackendData.Instance.GetFieldLv() },
            //제작
            { "craftmakingid", PlayerBackendData.Instance.craftmakingid },
            { "craftdatetime", PlayerBackendData.Instance.craftdatetime },
            { "craftdatecount", PlayerBackendData.Instance.craftdatecount },
            { "Ability", PlayerBackendData.Instance.Abilitys },
            { "Roulette", PlayerBackendData.Instance.RouletteCount },
            { "LoginTimeSecToday", Timemanager.Instance.LoginTimeSecToday },
            { "LoginTimeSecEvery", Timemanager.Instance.LoginTimeSecEvery },
            { "SaveNum", PlayerBackendData.Instance.ClientSaveNum }
        };
        PlayerBackendData.Instance.sotang_raid = PlayerBackendData.Instance.sotang_raid.Distinct().ToList();
        PlayerBackendData.Instance.sotang_dungeon = PlayerBackendData.Instance.sotang_dungeon.Distinct().ToList();
        Timemanager.Instance.OnceTradePackage = Timemanager.Instance.OnceTradePackage.Distinct().ToList();
        Timemanager.Instance.OncePremiumPackage = Timemanager.Instance.OncePremiumPackage.Distinct().ToList();
        Param paramB = new Param
        {     //업적
            { "Achievement", PlayerBackendData.Instance.PlayerAchieveData },
            //기간제 자동사냥 엘리
            { "PlayerTimes", PlayerBackendData.Instance.PlayerTimes },
            //상점
            { "OnceShopData", Timemanager.Instance.OncePremiumPackage },
            { "OnceTradeData", Timemanager.Instance.OnceTradePackage },
            //소탕정보
            { "sotang_raid", PlayerBackendData.Instance.sotang_raid },
            { "sotang_dungeon", PlayerBackendData.Instance.sotang_dungeon },
            //가이드
            { "Tutoid", PlayerBackendData.Instance.tutoid },
            { "tutoguideid", PlayerBackendData.Instance.tutoguideid },
            { "tutoguideisfinish", PlayerBackendData.Instance.tutoguideisfinish },
            { "tutoguidepremium", PlayerBackendData.Instance.tutoguidepremium },

            { "AdsFree", PlayerBackendData.Instance.isadsfree },
            { "SavedCheckTime", Timemanager.Instance.NowTime.ToString() },
        };
        Param paramC = new Param
        {
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
        
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
        {
            if(callback.IsSuccess())
                Savemanager.Instance.Save();
            else
            {
                PlayerBackendData.Instance.ClientSaveNum--;
                Savemanager.Instance.SaveSaveNum2();
                Savemanager.Instance.Save();
            }
            string paramToString = JsonConvert.SerializeObject(param.GetValue());
            int count = System.Text.Encoding.Default.GetByteCount(paramToString);
            Debug.Log("param의 크기(byte) : " + count);
        });
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramB, (callback) =>
        {
            string paramToString = JsonConvert.SerializeObject(paramB.GetValue());
            int count = System.Text.Encoding.Default.GetByteCount(paramToString);
            Debug.Log("paramB의 크기(byte) : " + count);
            Debug.Log(callback);
        });
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramC, (callback) =>
        {
            string paramToString = JsonConvert.SerializeObject(paramC.GetValue());
            int count = System.Text.Encoding.Default.GetByteCount(paramToString);
            Debug.Log("paramC의 크기(byte) : " + count);
            Debug.Log(callback);
        });
    }

    public void SaveContent()
    {
        PlayerBackendData userData = PlayerBackendData.Instance;
        Param param = new Param
        {
            { "ContentLevel", PlayerBackendData.Instance.ContentLevel },
            { "SkillInven", userData.Skills },
            { "inventory", userData.ItemInventory },
        };
        
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
        {
            
        });
    }
    
    public void SaveDataALl(bool issettingsave =false,bool isexit = false,bool ischange = false)
    {
        Savemanager.Instance.SaveEvery();
        if (!CheckServerOn())
        {
            SaveDataALl(issettingsave, isexit);
            Debug.Log("여기");
            return;
        }

        if (PlayerBackendData.Instance.ServerLv > PlayerBackendData.Instance.GetLv())
        {
            Debug.Log("여기2");
            return;
        }
        
        
        Savemanager.Instance.SaveSaveNum();
        Timemanager.Instance.RefreshNowTIme();
           PlayerBackendData userData = PlayerBackendData.Instance;
           
        //데이터는 3시간에 한번 저장된다.
        //최근 저장시간을 기준으로 잡느다.
        Param param = new Param
        {
            { "Achievement", PlayerBackendData.Instance.PlayerAchieveData },
            { "Gold", userData.GetMoney() }, 
            { "Crystal", userData.GetCash() },
            { "avata_avata", userData.avata_avata },
            { "avata_weapon", userData.avata_weapon },
            { "avata_subweapon", userData.avata_subweapon },
            { "playeravata", userData.playeravata },
            //레벨
            { "level", userData.GetLv() },
            { "levelExp", userData.GetExp() },
            //업적레벨
            { "level_ach", userData.GetAchLv() },
            { "level_achExp", userData.GetAchExp() },
            //제단레벨
            { "Altar_Lvs", userData.GetAllAltarlv() },
            //랭크
            { "level_adven", userData.GetAdLv() },
            //보유 스킬
            { "SkillInven", userData.Skills },
            //클래스 데이타
            { "Class", userData.ClassData },
            { "NowClass", userData.ClassData[userData.ClassId] },
            //패시브
            { "Passive", userData.PassiveClassId },
            //플레이어 스탯 정보
            { "Playerstat", PlayerData.Instance.GetPlayerStatForSave() },
            //가방
            { "inventory", userData.ItemInventory },
            //전투력
            { "BattlePoint", (int)userData.GetBattlePoint() },
            //현재맵
            { "NowMap", mapmanager.Instance.savemapid },
           
            //스테이지
            { "Stage", PlayerBackendData.Instance.nowstage },
            { "FieldCount", PlayerBackendData.Instance.spawncount },
            { "FieldLv", PlayerBackendData.Instance.GetFieldLv() },
         
            { "ContentLevel", PlayerBackendData.Instance.ContentLevel },
            { "Ability", PlayerBackendData.Instance.Abilitys },
            { "Roulette", PlayerBackendData.Instance.RouletteCount },
            { "LoginTimeSecToday", Timemanager.Instance.LoginTimeSecToday },
            { "LoginTimeSecEvery", Timemanager.Instance.LoginTimeSecEvery },
            { "SaveNum", PlayerBackendData.Instance.ClientSaveNum },
                       //제작
            { "craftmakingid", PlayerBackendData.Instance.craftmakingid },
            { "craftdatetime", PlayerBackendData.Instance.craftdatetime },
            { "craftdatecount", PlayerBackendData.Instance.craftdatecount },
            //상점
            { "OnceShopData", Timemanager.Instance.OncePremiumPackage },
            { "OnceTradeData", Timemanager.Instance.OnceTradePackage },
            //소탕정보
            { "sotang_raid", PlayerBackendData.Instance.sotang_raid },
            { "sotang_dungeon", PlayerBackendData.Instance.sotang_dungeon },
            //가이드
            { "Tutoid", PlayerBackendData.Instance.tutoid },
            { "tutoguideid", PlayerBackendData.Instance.tutoguideid },
            { "tutoguideisfinish", PlayerBackendData.Instance.tutoguideisfinish },
            { "tutoguidepremium", PlayerBackendData.Instance.tutoguidepremium },
            { "AdsFree", PlayerBackendData.Instance.isadsfree },
            { "SavedCheckTime", Timemanager.Instance.NowTime.ToString() },
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
            { "OfflineTime", Timemanager.Instance.NowTime },
            
        };
        Debug.Log("저장 시간은" + Timemanager.Instance.NowTime);
     

      
        PlayerBackendData.Instance.sotang_raid = PlayerBackendData.Instance.sotang_raid.Distinct().ToList();
        PlayerBackendData.Instance.sotang_dungeon = PlayerBackendData.Instance.sotang_dungeon.Distinct().ToList();
        Timemanager.Instance.OnceTradePackage = Timemanager.Instance.OnceTradePackage.Distinct().ToList();
        Timemanager.Instance.OncePremiumPackage = Timemanager.Instance.OncePremiumPackage.Distinct().ToList();
        string paramToString = JsonConvert.SerializeObject(param.GetValue());
        int count = System.Text.Encoding.Default.GetByteCount(paramToString);
         
         Debug.Log("param의 크기(byte) : " + count);

        if(issettingsave)
        {//저장 상태
       
            Loadingbar_savedata.SetActive(true);
            PlayerServerDatapanel.SetActive(false);
        }
        param.Add("PlayerSaveTime",Timemanager.Instance.NowTime);
        if (ischange)
        {
            param.Add("PlayerCanLoadBool","trues");
        }
        else
        {
            param.Add("PlayerCanLoadBool","true");
        }
        if (!Inventory.Instance.istrue)
            Inventory.Instance.istrue = true;
        if (isexit)
        {
            ExitLoading.SetActive(true);
            ExitSavePanel.SetActive(false);
        }
        
        // key 컬럼의 값이 keyCode인 데이터 검색
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
        {
            Debug.Log(callback);
            if(callback.IsSuccess())
                Savemanager.Instance.Save();
            else
            {
                PlayerBackendData.Instance.ClientSaveNum--;
                Savemanager.Instance.SaveSaveNum2();
                Savemanager.Instance.Save();
            }
            Debug.Log(issettingsave);
            if (issettingsave)
            {
                //저장 상태
                LogManager.LogSave_UserData(param);
                RefreshServerSavedData();
            }
            
            if (ischange)
            {
                //저장 상태
                LogManager.LogSaveChange();
            }
            LogManager.LogSaveAuto(param);
            if (isexit)
            {
                Debug.Log("게임을 종료합니다");
                Application.Quit();
            }
            RankingManager.Instance.RankInsert(userData.GetBattlePoint().ToString(), RankingManager.RankEnum.전투력);
            RankingManager.Instance.RankInsert(userData.GetLv().ToString(), RankingManager.RankEnum.레벨);
            initserverdata = false;

            if (!callback.IsSuccess()) return;
        });
    }

    public void OnlyInvenSave()
    {
        if (PlayerBackendData.Instance.ServerLv > PlayerBackendData.Instance.GetLv())
        {
            return;
        }
        PlayerBackendData userData = PlayerBackendData.Instance;
        Param paramData = new Param
        {
            //가방
            { "inventory", userData.ItemInventory },
            { "Gold", userData.GetMoney() },
            { "Crystal", userData.GetCash() },
            { "avata_avata", userData.avata_avata },
            { "avata_weapon", userData.avata_weapon },
            { "avata_subweapon", userData.avata_subweapon },
            { "playeravata", userData.playeravata },
            //레벨
            { "level", userData.GetLv() },
            { "levelExp", userData.GetExp() },
            { "Class", userData.ClassData },
            { "NowClass", userData.ClassData[userData.ClassId] },
            { "Playertime", userData.PlayerTimes },
            //업적레벨
            { "level_ach", userData.GetAchLv() },
            { "level_achExp", userData.GetAchExp() },
        };
        
        
        // key 컬럼의 값이 keyCode인 데이터 검색
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramData, (callback) =>
        {
            // 이후 처리
            if (!callback.IsSuccess()) return;
            LogManager.UserInventoryLog(paramData);
        });
    }
    
    #endregion

    public void Bt_SaveNextFieldToggle()
    {
        Savemanager.Instance._es3File.Save($"{PlayerBackendData.Instance.Id}FieldNextToggle", mapmanager.Instance.stageautouptoggle.isOn);
    }

    void LoadNextFieldToggle()
    {
        if (ES3.KeyExists($"{PlayerBackendData.Instance.Id}FieldNextToggle", Savemanager.Instance.GetFileName()))
        {
            mapmanager.Instance.stageautouptoggle.isOn =
                (ES3.Load<bool>($"{PlayerBackendData.Instance.Id}FieldNextToggle", Savemanager.Instance.GetFileName()));
        }
    }
    public bool ischatbool_public; //나갔다들어왔을때 최근목록은 안보이게.
    public bool ischatbool_guild;

    IEnumerator RefreshToken()
    {
        int count = 0;
        WaitForSeconds waitForRefreshTokenCycle = new WaitForSeconds(7200); // 60초 x 60분 x 8시간
        // WaitForSeconds waitForRefreshTokenCycle = new WaitForSeconds(20); // 60초 x 60분 x 8시간
        WaitForSeconds waitForRetryCycle = new WaitForSeconds(15f);
        // 첫 호출시에는 리프레시 토큰하지 않도록 8시간을 기다리게 해준다.
//        Debug.Log("리프레쉬 시작");
        bool isStart = false;
        if (!isStart)
        {
            isStart = true;
            yield return waitForRefreshTokenCycle; // 8시간 기다린 후 반복문 시작
        }


        BackendReturnObject bro = null;
        bool isRefreshSuccess = false;
        // 이후부터는 반복문을 돌면서 8시간마다 최대 3번의 리프레시 토큰을 수행하게 된다.
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                bro = Backend.BMember.RefreshTheBackendToken();
                Debug.Log("리프레시 토큰 성공 여부 : " + bro);
                if (bro.IsSuccess())
                {
                    SaveDataALl(false);
                    Debug.Log("토큰 재발급 완료");
                    isRefreshSuccess = true;
                    break;
                }
                else
                {
                    if (bro.GetMessage().Contains("bad refreshToken"))
                    {
                        Debug.LogError("중복 로그인 발생");
                        isRefreshSuccess = false;
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("15초 뒤에 토큰 재발급 다시 시도");
                    }
                }

                yield return waitForRetryCycle; // 15초 뒤에 다시시도
            }
            // 유저들이 스스로 토큰 리프레시를 할수 있도록 구현해주시거나 수동 로그인을 하도록 구현해주시기 바랍니다.
            Debug.Log("8시간 토큰 재 호출");
            isRefreshSuccess = false;
            count = 0;
            yield return waitForRefreshTokenCycle;
        }
    }

    public void Bt_GiveCrystal(string hwa)
    {
        List<string> id = new List<string>();
        List<string> hw = new List<string>();
        id.Add("1001");
        hw.Add(hwa);
        Inventory.Instance.AddItem(id[0], int.Parse(hw[0]), true, false);
        if (hwa == "2000")
        {
            LogManager.LogReview(true);
        }
        else
        {
            LogManager.LogReview(false);
        }

        Inventory.Instance.ShowEarnItem(id.ToArray(), hw.ToArray(), false);
    }

    public void Bt_GiveCrystal()
    {
        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/리뷰2000"), alertmanager.alertenum.일반);

    }

    public void BtOnLogOut()
    {
        BackendReturnObject BRO = Backend.BMember.Logout();

        if (BRO.IsSuccess())
        {
            LogoutGoogle();
        }
    }

    public void LogoutGoogle()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated() == true)
        {
            PlayGamesPlatform.Instance.SignOut();
            PlayGamesPlatform.Activate();
            Application.Quit();
        }
    }

    //앱의 활성화 상태를 저장하는 변수
    bool isPaused = false;
    private int a = 0;

    void RefreshTokens()
    {

        if (!internetcheck.isinternet())
        {
            uimanager.Instance.internetobj.SetActive(true);
            Time.timeScale = 0;
            return;
        }
        else
        {
            Time.timeScale = 1;
            uimanager.Instance.internetobj.SetActive(false);
        }

        SendQueue.Enqueue(Backend.BMember.RefreshTheBackendToken, (callback) =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("토큰 재발급 완료");
                a = 0;
                isserveron = true;
                //채팅 연결.
                bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
                bool isConnect_guild = Backend.Chat.IsChatConnect(ChannelType.Guild);

                if (!isConnect)
                {
                    ischatbool_public = true;
                    if (!isConnect_guild)
                    {
                        ischatbool_guild = true;
                        //  chatmanager.Instance.JoinGuildChannel();
                    }

                    chatmanager.Instance.GetChannelList();
                }
            }
            else
            {
                isserveron = false;
                if (callback.GetMessage().Contains("bad refreshToken"))
                {
                    Debug.LogError("중복 로그인 발생");
                    //ReLogin.SetActive(true);
                    //Time.timeScale = 0;
                }
                else
                {

                    Invoke(nameof(RefreshToken), 15f);
                    a++;
                    if (a == 10)
                    {
                        LogManager.TokkenFail();
                    }
                    Debug.LogWarning("15초 뒤에 토큰 재발급 다시 시도");
                }
            }
        });
    }

    public GameObject ReLogin;

    private void OnApplicationQuit()
    {
        Savemanager.Instance.SaveEvery();
        SaveOffline();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Savemanager.Instance.SaveEvery();
            // LogManager.OfflineRewardLog();
            isPaused = true;
            PartyRaidRoommanager.Instance.Bt_ExitRoom();
            SaveOffline();
            /* 앱이 비활성화 되었을 때 처리 */
        }

        else
        {
            if (isPaused)
            {
                isPaused = false;
                RefreshTokens();
                CheckDailyCount();
                CheckOffLine();
                if (timechecktimecheck != null)
                    StopCoroutine(timechecktimecheck);
                if (timechecktimecheckRank != null)
                    StopCoroutine(timechecktimecheckRank);
                if (timechecktimecheckWBStart != null)
                    StopCoroutine(timechecktimecheckWBStart);
                if (timechecktimecheckWBEnd != null)
                    StopCoroutine(timechecktimecheckWBEnd);
            }
        }
    }


    private IEnumerator timechecktimecheck;
    private DateTime nowtime;
    private DateTime nowtimeNow;
    public void CheckDailyCount()
    {
        nowtimeNow = Timemanager.Instance.GetServerTime();
        nowtime = nowtimeNow.Date;
//        Debug.Log(nowtime);
  //      Debug.Log(nowtimeNow);
        DateTime nexttime = nowtime.AddDays(1).AddSeconds(2);
        TimeSpan timeSpan = nexttime -nowtimeNow;
        timechecktimecheck = ResetCheckStart((float)timeSpan.TotalSeconds);
        StartCoroutine(timechecktimecheck);
        CheckRank();
        CheckWorldBoss();
    }
    private IEnumerator timechecktimecheckRank;
    public void CheckRank()
    {
        DateTime nexttime = nowtime.AddDays(1).AddHours(6).AddSeconds(2);
        TimeSpan timeSpan = nexttime -nowtimeNow;
        timechecktimecheckRank = ResetCheckStartRank((float)timeSpan.TotalSeconds);
        StartCoroutine(timechecktimecheckRank);
    }

    
    private IEnumerator timechecktimecheckWBStart;
    private IEnumerator timechecktimecheckWBEnd;


    public void CheckWorldBoss()
    {
        DateTime nexttime;
        //12시부터 0시 사이
        if (nowtimeNow.Hour is >= 12 and <= 23)
        {
            //월드보스시간
            WorldBossManager.Instance.RefreshAllSlots();
            //alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/월드보스시간임"), alertmanager.alertenum.일반);

           
            nexttime = nowtime.AddHours(23).AddSeconds(2);
            //nexttime = nowtime.AddHours(16).AddMinutes(9).AddSeconds(2);
            TimeSpan timeSpan = nexttime - nowtimeNow;
            timechecktimecheckWBEnd = IenumerWBEnd((float)timeSpan.TotalSeconds);
            //11시 종료 시간 
            StartCoroutine(timechecktimecheckWBEnd);
            return;
        }
        //12시 이전이라면
        else if (nowtimeNow.Hour is < 12)
        {
            nexttime = nowtime.AddHours(12).AddSeconds(2);
            TimeSpan timeSpan = nexttime - nowtimeNow;
            timechecktimecheckWBStart = IenumerWBStart((float)timeSpan.TotalSeconds);
            StartCoroutine(timechecktimecheckWBStart);
        }
        else
        {
            nexttime = nowtime.AddDays(1).AddHours(12).AddSeconds(2);
            TimeSpan timeSpan = nexttime - nowtime;
            timechecktimecheckWBStart = IenumerWBStart((float)timeSpan.TotalSeconds);
            StartCoroutine(timechecktimecheckWBStart);
        }
    }
    IEnumerator ResetCheckStartRank(float totalsecond)
    {
        yield return new WaitForSeconds(totalsecond);
        //데이터를 받아온다.
        Where where1 = new Where();
        where1.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

        string[] selects =
        {
            "AntcaveLv"
        };

        var broj = Backend.GameData.GetMyData("RankData", where1,selects, 1);
        JsonData jdatas = null;
        if (broj.IsSuccess())
        {
            Debug.Log("랭킹데이터 불렁모" + broj);

            if (broj.GetReturnValuetoJSON()["rows"].Count == 0)
            {
                yield break;
            }
                        
            jdatas = broj.FlattenRows()[0];
            
            if (jdatas.ContainsKey("AntcaveLv"))
            {
                Debug.Log("개미굴랭킹가져옴"+jdatas["AntcaveLv"].ToString());
                
                PlayerBackendData.Instance.AntCaveLv = int.Parse(jdatas["AntcaveLv"].ToString());
            }
        }
    }
    
    
    IEnumerator ResetCheckStart(float totalsecond)
    {
        yield return new WaitForSeconds(totalsecond);
        Timemanager.Instance.GetTime();
        GuildManager.Instance.Guildchecksetting();
        if (Timemanager.Instance.isachievereset)
        {
            achievemanager.Instance.ResetCheck();
        }
        SaveDataALl();
    }
    
    
    //월드보스 시작
    IEnumerator IenumerWBStart(float totalsecond)
    {
        yield return new WaitForSeconds(totalsecond);
        Timemanager.Instance.GetTime();
        BossStart.SetTrigger(Show);
       // alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/월드보스시간임"),alertmanager.alertenum.일반);
        WorldBossManager.Instance.RefreshAllSlots();
        SaveDataALl();
    }

    public Animator BossStart;
    public Animator BossEnd;
    public Image[] WorldBossImage;
    public GameObject[] WorldBossKilled;
    IEnumerator IenumerWBEnd(float totalsecond)
    {
        yield return new WaitForSeconds(4f);
        BossStart.SetTrigger(Show);
        yield return new WaitForSeconds(totalsecond);
        WorldBossManager.Instance.RefreshAllSlots();
        
        yield return new WaitForSeconds(3);
        WorldBossManager.Instance.CloseAllWorldboss();

        for (int i = 0; i < WorldBossManager.Instance.bossslots.Length; i++)
        {
            if (WorldBossManager.Instance.bossslots[i].curhp <= 0)
            {
                WorldBossImage[i].color = Color.gray;
                WorldBossKilled[i].SetActive(true);
            }
            else
            {
                WorldBossImage[i].color = Color.white;
                WorldBossKilled[i].SetActive(false);
            }
        }
        BossEnd.SetTrigger(Show);
        //alertmanager.Instance.ShowAlert("UI6/월드보스시간종료",alertmanager.alertenum.일반);
        SaveDataALl();
    }
    
    
    //처음 접속 혹은 나갔다가올때마다 0시까지의 시간을 만든다.
    public void Bt_CheckServer()
    {
        BackendReturnObject bro = Backend.BMember.IsAccessTokenAlive();
        if(bro.IsSuccess())
        {
            Debug.Log("액세스 토큰이 살아있습니다");
        }
        else
        {
            Debug.Log("죽음");
        }
    }

    private bool isserveron;

    public void falseserveron()
    {
        //검사는 처음에만한다.
        isserveron = false;
    }

    public bool justcheck()
    {
        if (!internetcheck.isinternet())
        {
            Savemanager.Instance.SaveEvery();
            uimanager.Instance.internetobj.SetActive(true);
            Time.timeScale = 0;
            return false;
        }

        return true;
    }
    public bool CheckServerOn()
    {
        if (!internetcheck.isinternet())
        {
            Savemanager.Instance.SaveEvery();
            uimanager.Instance.internetobj.SetActive(true);
            Time.timeScale = 0;
            return false;
        }
        
        if (isserveron)
            return true;
     //   Debug.Log("응애");
        BackendReturnObject bro = Backend.BMember.IsAccessTokenAlive();
        if(bro.IsSuccess())
        {
            Debug.Log("액세스 토큰이 살아있습니다");
            isserveron = true;
            return true;
        }
        else
        {
            //서버를 불러옴
            RefreshTokens();
        }
        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/서버재연결"),alertmanager.alertenum.일반);
        return false;
    }


    public GameObject ExitPanel;
    public GameObject ExitLoading;
    public GameObject ExitSavePanel;
    private static readonly int Show = Animator.StringToHash("show");

    public void Bt_ShowExit()
    {
        ExitPanel.SetActive(true);
        ExitLoading.SetActive(false);
        ExitSavePanel.SetActive(true);
        falseserveron();
    }

    public void SaveAllLocal()
    {
        Savemanager.Instance.SaveEvery();
        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/저장됨"),alertmanager.alertenum.일반);
    }
    
    public void Bt_ExitGame()
    {
        Savemanager.Instance.SaveEvery();
        if (!CheckServerOn())
        {
            return;
        }
        

        if (CheckServerOn())
        {
            SaveDataALl(true,true);   
        }
    }
    
    public void Bt_ExitGame_JustQuit()
    {
        Application.Quit();
    }
    
    
    public void Bt_ExitGame_Change()
    {
        if (!CheckServerOn())
        {
            return;
        }
        

        if (CheckServerOn())
        {
            SaveDataALl(false,true,true);   
        }
    }
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //if (UIView.VisibleViews.Count != 0)
                //{
                //    UIView.VisibleViews[^1].Hide(true);
                //}
                //else
                //{
                    Bt_ShowExit();
                //}
            }
    }

    public void Bt_OnTrue()
    {
        if (!Inventory.Instance.istrue)
        {
            PlayerBackendData userData = PlayerBackendData.Instance;
            
            if (PlayerBackendData.Instance.ServerLv > PlayerBackendData.Instance.GetLv())
            {
                return;
            }
            
            Where where = new Where();
            where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
            Savemanager.Instance.SaveSaveNum();
            Param param = new Param
            {
                { "PlayerCanLoadBool", "true" },
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
                { "SaveNum", userData.ClientSaveNum },
            };
            Inventory.Instance.istrue = true;
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
                    
                    Savemanager.Instance.Save();
                }
                else
                {
                    PlayerBackendData.Instance.ClientSaveNum--;
                    Savemanager.Instance.SaveSaveNum2();
                    Savemanager.Instance.Save();
                }
            });
        }
    }

    public void Bt_WithdrawAccount()
    {
        BackendReturnObject bro =  Backend.BMember.WithdrawAccount();
        if (bro.IsSuccess())
        {
            Application.Quit();
        }
    }
    
    //최소 시간30분

    public const float minofftime = 1800;
    public void SaveOffline()
    {
        if (!CheckServerOn())
        {
            return;
        }
        nowtimeNow = Timemanager.Instance.GetServerTime();
        Param param = new Param
        {
            { "OfflineTime", nowtimeNow },
        };
        
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
        {
            Debug.Log("저장시간은" + nowtimeNow);
        });
    }

    
    public void CheckOffLine()
    {
        if (!CheckServerOn())
        {
            Debug.Log("하이");
            return;
        }
        
        if (PlayerBackendData.Instance.ServerLv > PlayerBackendData.Instance.GetLv())
        {
            Debug.Log("하이");
            return;
        }

        if (Timemanager.Instance.isstop)
        {
            Debug.Log("하이");
            return;
        }

        //데이터를 받아온다.
        Where where1 = new Where();
        where1.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        string[] select =
        {
            "OfflineTime",
        };
        SendQueue.Enqueue(Backend.GameData.GetMyData, "PlayerData", where1, select, 1, bro =>
        {
            Debug.Log("하이");
            Debug.Log(bro);
            if (bro.IsSuccess())
            {
                JsonData data = bro.FlattenRows()[0];
                if (data.ContainsKey("OfflineTime"))
                {
                    nowtimeNow = Timemanager.Instance.GetServerTime();
                    DateTime datetime = System.DateTime.Parse(data["OfflineTime"].ToString());
                    TimeSpan dateDiff = nowtimeNow - datetime;
                    Debug.Log(nowtimeNow);
                   Debug.Log(datetime);
                    Debug.Log("총안들어온 시간" +  dateDiff.TotalSeconds);

                    if (PlayerBackendData.Instance.Offlinedata != null)
                    {
                        if (dateDiff.TotalSeconds >= minofftime)
                        {
                            //오프라인 보상 출력
                            Debug.Log("오프라인 보상을 출력한다.");
                            Autofarmmanager.Instance.Bt_OpenAuto_Offline(nowtimeNow, datetime, dateDiff.TotalSeconds);
                            Autofarmmanager.Instance.Bt_EarnOfflineReward();
                        }
                        else
                        {
//                            Debug.Log("받을 보상이 없다ㅔ");
                        }
                    }
                }
            }
        });
    }
}
