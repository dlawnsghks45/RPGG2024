using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    private static LogManager _instance = null;

    
    public static LogManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(LogManager)) as LogManager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    
    
    public static void LogTrade(string tradeid,string itemid , int count,string UI,int UIcount)
    {
        Param param = new Param();
        param.Add("TI ",tradeid);
        param.Add("ID" , Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(itemid).name));
        param.Add("HW" , count);
        
        param.Add("사용아이템" , Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(UI).name));
        param.Add("사용한개수" , UIcount);
        
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "교환소거래", param, (callback) =>
        {
            // 이후 처리
        });
    }
    public static void LogDailyRewardCry()
    {
        Param param = new Param();
        param.Add("Crystal ",PlayerBackendData.Instance.GetCash());
        
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "출석체크오픈", param, (callback) =>
        {
            // 이후 처리
        });
    }
    public static void LogSpeedHack(float A,float B)
    {
        Param param = new Param();
        param.Add("elapsedSpan", A.ToString("N2"));
        param.Add("realtimeSinceStartup", B.ToString("N2"));
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "스피드핵", param, (callback) =>
        {
            // 이후 처리
            if(callback.IsSuccess())
                Time.timeScale = 0;
        });
    }

    public static void InsertIAPBuyProduct(string shopid)
    {
        Param param = new Param();
        param.Add("프로덕트 아이디 ", ShopDB.Instance.Find_ids(shopid).productid);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "프리미엄상점구매2", param, (callback) =>
        {
            // 이후 처리
        });
        LogManager.Log_CrystalEarn("프리미엄상점");
    }
    public static void InsertIAPBuyProductpass()
    {
        Param param = new Param();
        param.Add("프로덕트 아이디 ", "시즌패스");
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "프리미엄상점구매2", param, (callback) =>
        {
            // 이후 처리
        });
    }
    public static void LogTutorial(string id)
    {
        Param param = new Param ();
        param.Add ( "ID", id);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "TutoTableNEW", param, ( callback ) => 
        {
            
            // 이후 처리
            
        });
    }
    
    public static void LogTutorialGuide(string id)
    {
        Param param = new Param ();
        param.Add ( "ID", id);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "GuideQuest", param, ( callback ) => 
        {
            
            // 이후 처리
            
        });
    }
    public static void LogCraft(string id,string result,int count)
    {
        Param param = new Param ();
        param.Add ( "제작번호", id);
        param.Add ( "제작장비", id);
        param.Add ( "제작개수", id);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "Craft", param, ( callback ) => 
        {
            
            // 이후 처리
            
        });
    }
    
    public static void LogSucc(string ResourceKeyid,string resultkeyid,string type)
    {
        Param param = new Param ();
        param.Add ( "재료", ResourceKeyid);
        param.Add ( "결과", resultkeyid);
        param.Add ( "타입", type);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "전승", param, ( callback ) => 
        {
            
            // 이후 처리
            
        });
    }
    public static void LogReview(bool ison)
    {
        Param param = new Param ();
        param.Add ( "리뷰", ison);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "유저리뷰", param, ( callback ) => 
        {
            // 이후 처리
        });
    }

    public static void SaveAltar( string Altarname,int suc,int fail,decimal usecount)
    {
        Param param = new Param ();
        param.Add ( "제단타입" , Altarname);
        param.Add ( "성공" , suc);
        param.Add ( "실패" , fail);
        param.Add ( "횟수" , suc+fail);
        param.Add ( "사용한거" , suc+fail);
        param.Add ( "제단" , PlayerBackendData.Instance.Altar_Lvs);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "제단", param, ( callback ) => 
        {
            // 이후 처리
            Debug.Log(callback);
        });
    }
    
    public static void StartCraft( string craftid ,int count)
    {
        Param param = new Param ();
        param.Add ( "아이디" , craftid);
        param.Add ( "횟수" , count);
        param.Add ( "크리스탈" , PlayerBackendData.Instance.GetCash());

        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "제작시작", param, ( callback ) => 
        {
            // 이후 처리
            Debug.Log(callback);
        });
    }
    public static void FinishCraft( string craftid ,List<int> count ,List<string> result,bool isequip)
    {
        Param param = new Param ();
        param.Add ( "아이디" , craftid);
        param.Add ( "횟수" , count);
        param.Add ( "결과" , result);
        param.Add ( "크리스탈" , PlayerBackendData.Instance.GetCash());

        if (isequip)
        {
            SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "제작종료_장비", param, (callback) =>
            {
                // 이후 처리
                Debug.Log(callback);
            });
        }
        else
        {
            SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "제작종료_아이템", param, (callback) =>
            {
                // 이후 처리
                Debug.Log(callback);
            });
        }
        
        equipoptionchanger.Instance.Bt_SaveServerData();
    }
    
    public static void LogAdlv(int Adlv)
    {
        Param param = new Param ();
        param.Add ( "달성승급레벨", PlayerBackendData.Instance.GetAdLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "유저랭크달성", param, ( callback ) => 
        {
            // 이후 처리
        });
    }
    
    public static void LogAltarWar(int GetCount,decimal dmg)
    {
        Param param = new Param ();
        param.Add ( "개수", GetCount);
        param.Add ( "피해량", dmg.ToString());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "성물파괴", param, ( callback ) => 
        {
            // 이후 처리
        });
    }
    public static void LogSave_UserData(Param param)
    {
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "유저저장", param, 14,( callback ) => 
        {
            // 이후 처리
        });
    }
    public static void LogSave_Roulette(string roulettenum,string[] id,int[] hw,int count)
    {
        string[] ids = id;
        int[] hws = hw;
        Param param = new Param();
        param.Add ( "UID", SystemInfo.deviceUniqueIdentifier);
        param.Add ( "룰렛ID",roulettenum );
        param.Add ( "아이템", ids);
        param.Add ( "개수", hws);
        param.Add ( "횟수", count);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "룰렛", param, 20,( callback ) => 
        {
            // 이후 처리
        });
    }
    
    
    public static void LogSaveCollectData_Auto(Param param)
    {
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "유저업적수집저장", param, 14,( callback ) => 
        {
            // 이후 처리
        });
    }
     public static void LogSaveAuto(Param param)
    {
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "서버저장자동", param,7, ( callback ) => 
        {
            // 이후 처리
        });
    }
     
     public static void LogSaveChange()
     {
         Param param = new Param();
         param.Add ( "UID", SystemInfo.deviceUniqueIdentifier);
         param.Add ( "저장을 했던 모델", SystemInfo.deviceModel);
         param.Add ( "이름", SystemInfo.deviceName);
         param.Add ( "크리스탈",PlayerBackendData.Instance.GetCash());
         SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "기기변경시도", param, ( callback ) => 
         {
             // 이후 처리
         });
     }
    
    public static void LogSaveInven()
    {
        Param param = new Param();
        param.Add ( "크리스탈" , PlayerBackendData.Instance.GetCash());
        param.Add ( "가방" , PlayerBackendData.Instance.ItemInventory);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "인벤토리", param,7, ( callback ) => 
        {
            // 이후 처리
        });
    }
    public static void LogLoad()
    {
        Param param = new Param ();
        param.Add ( "불러옴", "불렀다");
        param.Add ( "UID", SystemInfo.deviceUniqueIdentifier);
        param.Add ( "모델", SystemInfo.deviceModel);
        param.Add ( "이름", SystemInfo.deviceName);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "서버불러옴", param,7, ( callback ) => 
        {
            // 이후 처리
        });
    }

    private static bool isuseeskillbug = false;
    public static void EskillLog(EquipDatabase data)
    {
        Param param = new Param();
        param.Add("장비 정보", data);
        param.Add("가진 아이템", PlayerBackendData.Instance.CheckItemCount("54"));

        if (PlayerBackendData.Instance.CheckItemCount("54") > 500 && isuseeskillbug)
        {
         
            isuseeskillbug = true;
            Param param2 = new Param ();
            param2.Add("크리스탏", PlayerBackendData.Instance.GetCash());
            param2.Add("아이템", Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id("54").name));
            SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "버그악용", param2, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    if(PlayerBackendData.Instance.CheckItemCount("54") > 10000)
                        Application.Quit();
                }
            });
        }
        
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "특수효과재설정", param,14, (callback) =>
        {
            // 이후 처리
        });
    }
    
    public static void BoxOpen(string ItemName,int count)
    {
        Param param = new Param();
        param.Add("크리스탈" ,PlayerBackendData.Instance.GetCash());
        param.Add("레벨" ,PlayerBackendData.Instance.GetLv());
        param.Add("아이템이름", ItemName);
        param.Add("횟수",count);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "박스오픈", param, ( callback ) => 
        {
            // 이후 처리
        });
    }
    
    
    public static void UserSmeltCheck(string LogName)
    {
        Param param = new Param();
        param.Add("크리스탈" ,PlayerBackendData.Instance.GetCash());
        param.Add("레벨" ,PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, LogName, param, ( callback ) => 
        {
            // 이후 처리
        });
    }
    public static void UserInventoryLog(Param param)
    {
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "유저가방저장", param, ( callback ) => 
        {
            // 이후 처리
        });
    }
    
    public static void Stang_Dungeon(int count)
    {
        Param param = new Param();
        param.Add("소탕횟수" ,count);
        param.Add("입장권개수" ,PlayerBackendData.Instance.CheckItemCount("1711"));
        param.Add("레벨" ,PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "소탕_던전", param, ( callback ) => 
        {
            // 이후 처리
        });
    }
    public static void Stang_Raid(int count)
    {
        Param param = new Param();
        param.Add("소탕횟수" ,count);
        param.Add("입장권개수" ,PlayerBackendData.Instance.CheckItemCount("200000"));
        param.Add("레벨" ,PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "소탕_레이드", param, ( callback ) => 
        {
            // 이후 처리
        });
    }
    
    
    
    public static void RareLog(string prevrare,string nextrare,string equipid)
    {
        Param param = new Param ();
        param.Add ( "장비 아이디", equipid);
        param.Add ( "원래 레어", prevrare);
        param.Add ( "나온 레어", nextrare);
        param.Add ( "가진 아이템", PlayerBackendData.Instance.CheckItemCount("53"));
        
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "등급재설정", param, ( callback ) => 
        {
            // 이후 처리
        });
    }
    public static void CraftRareLog(int prevrare,int nextrare,string equipid)
    {
        Param param = new Param ();
        param.Add ( "장비 아이디", equipid);
        param.Add ( "원래 품질", prevrare);
        param.Add ( "나온 품질", nextrare);

        if (PlayerBackendData.Instance.CheckItemCount("52") > 1000)
        {
            param.Add("가진 아이템", PlayerBackendData.Instance.CheckItemCount("52"));
            param.Add("아이템", Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id("52").name));
        }
        
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "품질재설정", param, ( callback ) => 
        {
            // 이후 처리
        });
    }

    private bool isalertone =false;

    void falsealertone()
    {
        isalertone = false;
    }
    public void CheckBug()
    {
//        Debug.Log("보안온");
        Param param = new Param ();

        
        
        if (PlayerBackendData.Instance.CheckItemCount("52") >= 1000 ||
            PlayerBackendData.Instance.CheckItemCount("53") >= 1000 ||
            PlayerBackendData.Instance.CheckItemCount("54") >= 1000 ||
            PlayerBackendData.Instance.CheckItemCount("200000") >= 500 ||
            PlayerBackendData.Instance.CheckItemCount("1712") >= 500 ||
            PlayerBackendData.Instance.GetCash() >= 600000)
        {
            if (isalertone)
                return;
            param.Add("레벨", PlayerBackendData.Instance.GetLv());
            param.Add("등급", PlayerBackendData.Instance.CheckItemCount("52"));
            param.Add("품질", PlayerBackendData.Instance.CheckItemCount("53"));
            param.Add("특효", PlayerBackendData.Instance.CheckItemCount("54"));
            param.Add("낡특효", PlayerBackendData.Instance.CheckItemCount("57"));
            param.Add("입장도전", PlayerBackendData.Instance.CheckItemCount("1711"));
            param.Add("레이드입장", PlayerBackendData.Instance.CheckItemCount("200000"));
            param.Add("제단강화", PlayerBackendData.Instance.CheckItemCount("1712"));
            param.Add("크리스탈", PlayerBackendData.Instance.GetCash());
            SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "버그악용아이템", param,14, (callback) =>
            {
                // 이후 처리
//                Debug.Log(callback);
                if (callback.IsSuccess())
                {
                    Invoke("falsealertone", 30);
                    isalertone = true;
                }
                

            });
        }
        Param param2 = new Param ();

        if (PlayerBackendData.Instance.Altar_Lvs[3] > 2000 || PlayerBackendData.Instance.Altar_Lvs[0] > 2000 || PlayerBackendData.Instance.Altar_Lvs[2] > 2000)
        {
            param2.Add("제단", PlayerBackendData.Instance.Altar_Lvs);
            SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "버그악용제단", param2, (callback) =>
            {
                // 이후 처리
            });
        } 
    }
    
    public static void ContentLog(string num,string needitem)
    {
        Param param = new Param ();

        string name = "";

        switch (num)
        {
            case "0":
                name = "대마법사의묘";
                break;
            case "1":
                name = "골드";
                break;
            case "2":
                name = "경험치";
                break;
            case "3":
                name = "비법";
                break;
        }
        param.Add ( "콘텐츠", name);
        param.Add ( "입장권 개수", PlayerBackendData.Instance.CheckItemCount(needitem));
        param.Add ( "크리스탈", PlayerBackendData.Instance.GetCash());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "콘텐츠입장", param, ( callback ) => 
        {
            // 이후 처리
        });
    }
    public static void ContentSotangLog(string num,string needitem,int count)
    {
        Param param = new Param ();

        string name = "";

        switch (num)
        {
            case "0":
                name = "대마법사의묘";
                break;
            case "1":
                name = "골드";
                break;
            case "2":
                name = "경험치";
                break;
            case "3":
                name = "비법";
                break;
        }
        param.Add ( "콘텐츠소탕", name);
        param.Add ( "입장권 개수", PlayerBackendData.Instance.CheckItemCount(needitem));
        param.Add ( "소탕횟수", count);
        param.Add ( "크리스탈", PlayerBackendData.Instance.GetCash());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "콘텐츠소탕", param, ( callback ) => 
        {
            // 이후 처리
        });
    }

    public static void SellLog(string id, decimal howmany)
    {
        Param param = new Param();
        param.Add("장비 아이디", Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(id).name));
        param.Add("개수", howmany);

        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "아이템판매", param, (callback) =>
        {
            // 이후 처리
        });
    }
    
    public static void CollectionLog(string collectid, bool isequip,string itemid ,EquipDatabase data =  null)
    {
        Param param = new Param();
        param.Add("수집 타입",isequip ? "장비" : "아이템");
        if (isequip)
        {
            param.Add("장비", Inventory.GetTranslate(EquipItemDB.Instance.Find_id(itemid).Name));
            param.Add("장비데이터", data);
        }
        else
        {
            param.Add("아이템", Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(itemid).name));
        }

        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "수집_리뉴얼", param, (callback) =>
        {
            // 이후 처리
        });
    }

    public static void CollectionLogAll()
    {
        Param param = new Param();
        param.Add("아이템", PlayerBackendData.Instance.RenewalCollectData);

        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "수집_리뉴얼전체", param, (callback) =>
        {
            // 이후 처리
        });
    }

    public static void EquipChangeLog(string previd,string nextid,string id)
    {
        Param param = new Param();
        param.Add("변경 아이디",id);
        param.Add("변경전장비",Inventory.GetTranslate(EquipItemDB.Instance.Find_id(previd).Name));
        param.Add("변경후장비",Inventory.GetTranslate(EquipItemDB.Instance.Find_id(nextid).Name));
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "장비변경", param, (callback) =>
        {
            // 이후 처리
        });
    }
    
    public static void PetLog(string[] petname)
    {
        Param param = new Param();
        param.Add("뽑은 펫",petname);
        param.Add("크리스탈",PlayerBackendData.Instance.GetCash());
        param.Add("레벨",PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "펫뽑기", param, (callback) =>
        {
            // 이후 처리
        });
    }
    
    public static void PetMixLog(string[] petremoved , string[] rewardid)
    {
        Param param = new Param();
        param.Add("삭제펫",petremoved);
        param.Add("뽑은 펫",rewardid);
        
        param.Add("크리스탈",PlayerBackendData.Instance.GetCash());
        param.Add("레벨",PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "펫합성", param, (callback) =>
        {
            // 이후 처리
        });
    }
    
    public static void UpgradeLog(EquipDatabase data,string[] petname)
    {
        Param param = new Param();
        param.Add("강화후 장비",data);
        param.Add("성공여부",petname);
        param.Add("횟수",petname.Length);
        param.Add("크리스탈",PlayerBackendData.Instance.GetCash());
        param.Add("가루",PlayerBackendData.Instance.CheckItemCount("50005"));
        param.Add("레벨",PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "장비강화", param, (callback) =>
        {
            // 이후 처리
        });
    }
    public static void OfflineRewardLog(int time,List<string>dropid,List<int>drophw)
    {
        Param param = new Param();
        param.Add("크리스탈",PlayerBackendData.Instance.GetCash());
        param.Add("레벨",PlayerBackendData.Instance.GetLv());
        param.Add("시간초" ,time);
        param.Add("아이디" ,dropid);
        param.Add("개수" ,drophw);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "오프라인보상지급", param, (callback) =>
        {
            // 이후 처리
        });
    }

    public static void TokkenFail()
    {
        Param param = new Param();
        param.Add("크리스탈",PlayerBackendData.Instance.GetCash());
        param.Add("레벨",PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "토큰새로고침실패", param, (callback) =>
        {
            // 이후 처리
            if (callback.IsSuccess())
            {
                Debug.Log("토큰 실패");
                Application.Quit();
            }
        });
    }

    public static int EarnCrystal;
    public static void Log_CrystalEarn(string Reason)
    {
        if(EarnCrystal ==0)
            return;
        Param param = new Param();
        param.Add("얻기곳",Reason);
        param.Add("얻기전크리스탈",PlayerBackendData.Instance.GetCash() - EarnCrystal);
        param.Add("얻은크리스탈",EarnCrystal);
        param.Add("얻은후크리스탈",PlayerBackendData.Instance.GetCash());
        param.Add("레벨",PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "크리스탈획득", param, (callback) =>
        {
            // 이후 처리
            if (callback.IsSuccess())
            {
                EarnCrystal = 0;
                Debug.Log("크리스탈획득");
            }
        });
    }
    
    public static void Log_ClearRaid(string level)
    {
        Param param = new Param();
        param.Add("난이도", level);
        param.Add("레벨",PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "파티레이드보상획득", param, (callback) =>
        {
            // 이후 처리
            if (callback.IsSuccess())
            {
            }
        });
    }
}
