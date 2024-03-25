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
        
        param.Add("��������" , Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(UI).name));
        param.Add("����Ѱ���" , UIcount);
        
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "��ȯ�Ұŷ�", param, (callback) =>
        {
            // ���� ó��
        });
    }
    public static void LogDailyRewardCry()
    {
        Param param = new Param();
        param.Add("Crystal ",PlayerBackendData.Instance.GetCash());
        
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "�⼮üũ����", param, (callback) =>
        {
            // ���� ó��
        });
    }
    public static void LogSpeedHack(float A,float B)
    {
        Param param = new Param();
        param.Add("elapsedSpan", A.ToString("N2"));
        param.Add("realtimeSinceStartup", B.ToString("N2"));
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "���ǵ���", param, (callback) =>
        {
            // ���� ó��
            if(callback.IsSuccess())
                Time.timeScale = 0;
        });
    }

    public static void InsertIAPBuyProduct(string shopid)
    {
        Param param = new Param();
        param.Add("���δ�Ʈ ���̵� ", ShopDB.Instance.Find_ids(shopid).productid);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "�����̾���������2", param, (callback) =>
        {
            // ���� ó��
        });
        LogManager.Log_CrystalEarn("�����̾�����");
    }
    public static void InsertIAPBuyProductpass()
    {
        Param param = new Param();
        param.Add("���δ�Ʈ ���̵� ", "�����н�");
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "�����̾���������2", param, (callback) =>
        {
            // ���� ó��
        });
    }
    public static void LogTutorial(string id)
    {
        Param param = new Param ();
        param.Add ( "ID", id);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "TutoTableNEW", param, ( callback ) => 
        {
            
            // ���� ó��
            
        });
    }
    
    public static void LogTutorialGuide(string id)
    {
        Param param = new Param ();
        param.Add ( "ID", id);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "GuideQuest", param, ( callback ) => 
        {
            
            // ���� ó��
            
        });
    }
    public static void LogCraft(string id,string result,int count)
    {
        Param param = new Param ();
        param.Add ( "���۹�ȣ", id);
        param.Add ( "�������", id);
        param.Add ( "���۰���", id);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "Craft", param, ( callback ) => 
        {
            
            // ���� ó��
            
        });
    }
    
    public static void LogSucc(string ResourceKeyid,string resultkeyid,string type)
    {
        Param param = new Param ();
        param.Add ( "���", ResourceKeyid);
        param.Add ( "���", resultkeyid);
        param.Add ( "Ÿ��", type);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "����", param, ( callback ) => 
        {
            
            // ���� ó��
            
        });
    }
    public static void LogReview(bool ison)
    {
        Param param = new Param ();
        param.Add ( "����", ison);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "��������", param, ( callback ) => 
        {
            // ���� ó��
        });
    }

    public static void SaveAltar( string Altarname,int suc,int fail,decimal usecount)
    {
        Param param = new Param ();
        param.Add ( "����Ÿ��" , Altarname);
        param.Add ( "����" , suc);
        param.Add ( "����" , fail);
        param.Add ( "Ƚ��" , suc+fail);
        param.Add ( "����Ѱ�" , suc+fail);
        param.Add ( "����" , PlayerBackendData.Instance.Altar_Lvs);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "����", param, ( callback ) => 
        {
            // ���� ó��
            Debug.Log(callback);
        });
    }
    
    public static void StartCraft( string craftid ,int count)
    {
        Param param = new Param ();
        param.Add ( "���̵�" , craftid);
        param.Add ( "Ƚ��" , count);
        param.Add ( "ũ����Ż" , PlayerBackendData.Instance.GetCash());

        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "���۽���", param, ( callback ) => 
        {
            // ���� ó��
            Debug.Log(callback);
        });
    }
    public static void FinishCraft( string craftid ,List<int> count ,List<string> result,bool isequip)
    {
        Param param = new Param ();
        param.Add ( "���̵�" , craftid);
        param.Add ( "Ƚ��" , count);
        param.Add ( "���" , result);
        param.Add ( "ũ����Ż" , PlayerBackendData.Instance.GetCash());

        if (isequip)
        {
            SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "��������_���", param, (callback) =>
            {
                // ���� ó��
                Debug.Log(callback);
            });
        }
        else
        {
            SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "��������_������", param, (callback) =>
            {
                // ���� ó��
                Debug.Log(callback);
            });
        }
        
        equipoptionchanger.Instance.Bt_SaveServerData();
    }
    
    public static void LogAdlv(int Adlv)
    {
        Param param = new Param ();
        param.Add ( "�޼��±޷���", PlayerBackendData.Instance.GetAdLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "������ũ�޼�", param, ( callback ) => 
        {
            // ���� ó��
        });
    }
    
    public static void LogAltarWar(int GetCount,decimal dmg)
    {
        Param param = new Param ();
        param.Add ( "����", GetCount);
        param.Add ( "���ط�", dmg.ToString());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "�����ı�", param, ( callback ) => 
        {
            // ���� ó��
        });
    }
    public static void LogSave_UserData(Param param)
    {
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "��������", param, 14,( callback ) => 
        {
            // ���� ó��
        });
    }
    public static void LogSave_Roulette(string roulettenum,string[] id,int[] hw,int count)
    {
        string[] ids = id;
        int[] hws = hw;
        Param param = new Param();
        param.Add ( "UID", SystemInfo.deviceUniqueIdentifier);
        param.Add ( "�귿ID",roulettenum );
        param.Add ( "������", ids);
        param.Add ( "����", hws);
        param.Add ( "Ƚ��", count);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "�귿", param, 20,( callback ) => 
        {
            // ���� ó��
        });
    }
    
    
    public static void LogSaveCollectData_Auto(Param param)
    {
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "����������������", param, 14,( callback ) => 
        {
            // ���� ó��
        });
    }
     public static void LogSaveAuto(Param param)
    {
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "���������ڵ�", param,7, ( callback ) => 
        {
            // ���� ó��
        });
    }
     
     public static void LogSaveChange()
     {
         Param param = new Param();
         param.Add ( "UID", SystemInfo.deviceUniqueIdentifier);
         param.Add ( "������ �ߴ� ��", SystemInfo.deviceModel);
         param.Add ( "�̸�", SystemInfo.deviceName);
         param.Add ( "ũ����Ż",PlayerBackendData.Instance.GetCash());
         SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "��⺯��õ�", param, ( callback ) => 
         {
             // ���� ó��
         });
     }
    
    public static void LogSaveInven()
    {
        Param param = new Param();
        param.Add ( "ũ����Ż" , PlayerBackendData.Instance.GetCash());
        param.Add ( "����" , PlayerBackendData.Instance.ItemInventory);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "�κ��丮", param,7, ( callback ) => 
        {
            // ���� ó��
        });
    }
    public static void LogLoad()
    {
        Param param = new Param ();
        param.Add ( "�ҷ���", "�ҷ���");
        param.Add ( "UID", SystemInfo.deviceUniqueIdentifier);
        param.Add ( "��", SystemInfo.deviceModel);
        param.Add ( "�̸�", SystemInfo.deviceName);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "�����ҷ���", param,7, ( callback ) => 
        {
            // ���� ó��
        });
    }

    private static bool isuseeskillbug = false;
    public static void EskillLog(EquipDatabase data)
    {
        Param param = new Param();
        param.Add("��� ����", data);
        param.Add("���� ������", PlayerBackendData.Instance.CheckItemCount("54"));

        if (PlayerBackendData.Instance.CheckItemCount("54") > 500 && isuseeskillbug)
        {
         
            isuseeskillbug = true;
            Param param2 = new Param ();
            param2.Add("ũ�����t", PlayerBackendData.Instance.GetCash());
            param2.Add("������", Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id("54").name));
            SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "���׾ǿ�", param2, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    if(PlayerBackendData.Instance.CheckItemCount("54") > 10000)
                        Application.Quit();
                }
            });
        }
        
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "Ư��ȿ���缳��", param,14, (callback) =>
        {
            // ���� ó��
        });
    }
    
    public static void BoxOpen(string ItemName,int count)
    {
        Param param = new Param();
        param.Add("ũ����Ż" ,PlayerBackendData.Instance.GetCash());
        param.Add("����" ,PlayerBackendData.Instance.GetLv());
        param.Add("�������̸�", ItemName);
        param.Add("Ƚ��",count);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "�ڽ�����", param, ( callback ) => 
        {
            // ���� ó��
        });
    }
    
    
    public static void UserSmeltCheck(string LogName)
    {
        Param param = new Param();
        param.Add("ũ����Ż" ,PlayerBackendData.Instance.GetCash());
        param.Add("����" ,PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, LogName, param, ( callback ) => 
        {
            // ���� ó��
        });
    }
    public static void UserInventoryLog(Param param)
    {
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "������������", param, ( callback ) => 
        {
            // ���� ó��
        });
    }
    
    public static void Stang_Dungeon(int count)
    {
        Param param = new Param();
        param.Add("����Ƚ��" ,count);
        param.Add("����ǰ���" ,PlayerBackendData.Instance.CheckItemCount("1711"));
        param.Add("����" ,PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "����_����", param, ( callback ) => 
        {
            // ���� ó��
        });
    }
    public static void Stang_Raid(int count)
    {
        Param param = new Param();
        param.Add("����Ƚ��" ,count);
        param.Add("����ǰ���" ,PlayerBackendData.Instance.CheckItemCount("200000"));
        param.Add("����" ,PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "����_���̵�", param, ( callback ) => 
        {
            // ���� ó��
        });
    }
    
    
    
    public static void RareLog(string prevrare,string nextrare,string equipid)
    {
        Param param = new Param ();
        param.Add ( "��� ���̵�", equipid);
        param.Add ( "���� ����", prevrare);
        param.Add ( "���� ����", nextrare);
        param.Add ( "���� ������", PlayerBackendData.Instance.CheckItemCount("53"));
        
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "����缳��", param, ( callback ) => 
        {
            // ���� ó��
        });
    }
    public static void CraftRareLog(int prevrare,int nextrare,string equipid)
    {
        Param param = new Param ();
        param.Add ( "��� ���̵�", equipid);
        param.Add ( "���� ǰ��", prevrare);
        param.Add ( "���� ǰ��", nextrare);

        if (PlayerBackendData.Instance.CheckItemCount("52") > 1000)
        {
            param.Add("���� ������", PlayerBackendData.Instance.CheckItemCount("52"));
            param.Add("������", Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id("52").name));
        }
        
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "ǰ���缳��", param, ( callback ) => 
        {
            // ���� ó��
        });
    }

    private bool isalertone =false;

    void falsealertone()
    {
        isalertone = false;
    }
    public void CheckBug()
    {
//        Debug.Log("���ȿ�");
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
            param.Add("����", PlayerBackendData.Instance.GetLv());
            param.Add("���", PlayerBackendData.Instance.CheckItemCount("52"));
            param.Add("ǰ��", PlayerBackendData.Instance.CheckItemCount("53"));
            param.Add("Ưȿ", PlayerBackendData.Instance.CheckItemCount("54"));
            param.Add("��Ưȿ", PlayerBackendData.Instance.CheckItemCount("57"));
            param.Add("���嵵��", PlayerBackendData.Instance.CheckItemCount("1711"));
            param.Add("���̵�����", PlayerBackendData.Instance.CheckItemCount("200000"));
            param.Add("���ܰ�ȭ", PlayerBackendData.Instance.CheckItemCount("1712"));
            param.Add("ũ����Ż", PlayerBackendData.Instance.GetCash());
            SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "���׾ǿ������", param,14, (callback) =>
            {
                // ���� ó��
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
            param2.Add("����", PlayerBackendData.Instance.Altar_Lvs);
            SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "���׾ǿ�����", param2, (callback) =>
            {
                // ���� ó��
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
                name = "�븶�����ǹ�";
                break;
            case "1":
                name = "���";
                break;
            case "2":
                name = "����ġ";
                break;
            case "3":
                name = "���";
                break;
        }
        param.Add ( "������", name);
        param.Add ( "����� ����", PlayerBackendData.Instance.CheckItemCount(needitem));
        param.Add ( "ũ����Ż", PlayerBackendData.Instance.GetCash());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "����������", param, ( callback ) => 
        {
            // ���� ó��
        });
    }
    public static void ContentSotangLog(string num,string needitem,int count)
    {
        Param param = new Param ();

        string name = "";

        switch (num)
        {
            case "0":
                name = "�븶�����ǹ�";
                break;
            case "1":
                name = "���";
                break;
            case "2":
                name = "����ġ";
                break;
            case "3":
                name = "���";
                break;
        }
        param.Add ( "����������", name);
        param.Add ( "����� ����", PlayerBackendData.Instance.CheckItemCount(needitem));
        param.Add ( "����Ƚ��", count);
        param.Add ( "ũ����Ż", PlayerBackendData.Instance.GetCash());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "����������", param, ( callback ) => 
        {
            // ���� ó��
        });
    }

    public static void SellLog(string id, decimal howmany)
    {
        Param param = new Param();
        param.Add("��� ���̵�", Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(id).name));
        param.Add("����", howmany);

        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "�������Ǹ�", param, (callback) =>
        {
            // ���� ó��
        });
    }
    
    public static void CollectionLog(string collectid, bool isequip,string itemid ,EquipDatabase data =  null)
    {
        Param param = new Param();
        param.Add("���� Ÿ��",isequip ? "���" : "������");
        if (isequip)
        {
            param.Add("���", Inventory.GetTranslate(EquipItemDB.Instance.Find_id(itemid).Name));
            param.Add("�������", data);
        }
        else
        {
            param.Add("������", Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(itemid).name));
        }

        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "����_������", param, (callback) =>
        {
            // ���� ó��
        });
    }

    public static void CollectionLogAll()
    {
        Param param = new Param();
        param.Add("������", PlayerBackendData.Instance.RenewalCollectData);

        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "����_��������ü", param, (callback) =>
        {
            // ���� ó��
        });
    }

    public static void EquipChangeLog(string previd,string nextid,string id)
    {
        Param param = new Param();
        param.Add("���� ���̵�",id);
        param.Add("���������",Inventory.GetTranslate(EquipItemDB.Instance.Find_id(previd).Name));
        param.Add("���������",Inventory.GetTranslate(EquipItemDB.Instance.Find_id(nextid).Name));
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "��񺯰�", param, (callback) =>
        {
            // ���� ó��
        });
    }
    
    public static void PetLog(string[] petname)
    {
        Param param = new Param();
        param.Add("���� ��",petname);
        param.Add("ũ����Ż",PlayerBackendData.Instance.GetCash());
        param.Add("����",PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "��̱�", param, (callback) =>
        {
            // ���� ó��
        });
    }
    
    public static void PetMixLog(string[] petremoved , string[] rewardid)
    {
        Param param = new Param();
        param.Add("������",petremoved);
        param.Add("���� ��",rewardid);
        
        param.Add("ũ����Ż",PlayerBackendData.Instance.GetCash());
        param.Add("����",PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "���ռ�", param, (callback) =>
        {
            // ���� ó��
        });
    }
    
    public static void UpgradeLog(EquipDatabase data,string[] petname)
    {
        Param param = new Param();
        param.Add("��ȭ�� ���",data);
        param.Add("��������",petname);
        param.Add("Ƚ��",petname.Length);
        param.Add("ũ����Ż",PlayerBackendData.Instance.GetCash());
        param.Add("����",PlayerBackendData.Instance.CheckItemCount("50005"));
        param.Add("����",PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "���ȭ", param, (callback) =>
        {
            // ���� ó��
        });
    }
    public static void OfflineRewardLog(int time,List<string>dropid,List<int>drophw)
    {
        Param param = new Param();
        param.Add("ũ����Ż",PlayerBackendData.Instance.GetCash());
        param.Add("����",PlayerBackendData.Instance.GetLv());
        param.Add("�ð���" ,time);
        param.Add("���̵�" ,dropid);
        param.Add("����" ,drophw);
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "�������κ�������", param, (callback) =>
        {
            // ���� ó��
        });
    }

    public static void TokkenFail()
    {
        Param param = new Param();
        param.Add("ũ����Ż",PlayerBackendData.Instance.GetCash());
        param.Add("����",PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "��ū���ΰ�ħ����", param, (callback) =>
        {
            // ���� ó��
            if (callback.IsSuccess())
            {
                Debug.Log("��ū ����");
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
        param.Add("����",Reason);
        param.Add("�����ũ����Ż",PlayerBackendData.Instance.GetCash() - EarnCrystal);
        param.Add("����ũ����Ż",EarnCrystal);
        param.Add("������ũ����Ż",PlayerBackendData.Instance.GetCash());
        param.Add("����",PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "ũ����Żȹ��", param, (callback) =>
        {
            // ���� ó��
            if (callback.IsSuccess())
            {
                EarnCrystal = 0;
                Debug.Log("ũ����Żȹ��");
            }
        });
    }
    
    public static void Log_ClearRaid(string level)
    {
        Param param = new Param();
        param.Add("���̵�", level);
        param.Add("����",PlayerBackendData.Instance.GetLv());
        SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "��Ƽ���̵庸��ȹ��", param, (callback) =>
        {
            // ���� ó��
            if (callback.IsSuccess())
            {
            }
        });
    }
}
