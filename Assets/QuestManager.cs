using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
   public List<string> id = new List<string>();
   public List<int> hw = new List<int>();
    //싱글톤만들기.
    private static QuestManager _instance = null;
    public static QuestManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(QuestManager)) as QuestManager;

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
        ResetCheck();
    }

    public QuestFinishPanel[] FinishPanel;

    public void ShowFinish(string Questid)
    {
        foreach (var t in FinishPanel)
        {
            if (!t.gameObject.activeSelf)
            {
                t.Refresh(Inventory.GetTranslate(QuestDB.Instance.Find_id(Questid).name));
                t.gameObject.SetActive(true);
                return;
            }
        }
    }
    

    public QuestSlot[] Questslots;


    public Text QuestLv;
    public Text QuestExp;
    public Text QuestStat;

    public questrewarddata[] Questdatas; //0일일 1주간 2이벤트

    public void AddCount(int counts,string subType)
    {
        foreach (var VARIABLE in Questslots)
        {
            if (VARIABLE.subtype.Equals(subType))
            {
                VARIABLE.AddCount(counts);
            }
        }
        
        Savemanager.Instance.SaveAchieve();
        Savemanager.Instance.Save();
    }
    
    private bool ischeckachieve;
    public void ResetCheck()
    {
        if(PlayerBackendData.Instance.PlayerAchieveData.Count ==0)
        {
            //업적이없다
            ischeckachieve = true;
            return;
        }
        if (Timemanager.Instance.isachieveresetdaily)
        {
            Timemanager.Instance.isachievereset = false;
            Timemanager.Instance.LoginTimeSecToday = 0;
            Savemanager.Instance.SaveLoginTime();
            Savemanager.Instance.Save();
            
            PlayerBackendData.Instance.QuestTotalCount[0] = 0;
            //업적 초기화
            for (int i = 0; i < QuestDB.Instance.NumRows(); i++)
            {
                if (QuestDB.Instance.GetAt(i).type.Equals("daily"))
                {
                    PlayerBackendData.Instance.QuestCount[i] = 0;
                    PlayerBackendData.Instance.QuestIsFinish[i] = false;
                }
            }
        }

        if (Timemanager.Instance.isachieveresetweekly)
        {
            PlayerBackendData.Instance.QuestTotalCount[1] = 0;
            //업적 초기화
            for (int i = 0; i < QuestDB.Instance.NumRows(); i++)
            {
                if (QuestDB.Instance.GetAt(i).type.Equals("weekly"))
                {
                    PlayerBackendData.Instance.QuestCount[i] = 0;
                    PlayerBackendData.Instance.QuestIsFinish[i] = false;
                }
            }
        }
        
        foreach (var t in Questslots)
        {
            t.initdata();
        }
        alertmanager.Instance.NotiCheck_Quest();
        Savemanager.Instance.SaveInventory();
        Savemanager.Instance.SaveCash();
        Savemanager.Instance.SaveAchieve();
        Savemanager.Instance.Save();
    }

    public void Bt_AllQuest()
    {
        id.Clear();
        hw.Clear();
        LogManager.EarnCrystal = 0;
        for (int i = 0; i < Questslots.Length; i++)
        {
            if (bool.Parse(QuestDB.Instance.Find_id(Questslots[i].Questid.ToString()).isseasonpremium)
                && PlayerBackendData.Instance.SeasonPassPremium)
            {
               
                Questslots[i].GetReward_All();
                
            }
            else
            {
                Questslots[i].GetReward_All();
            }
        }

        if (id.Count != 0)
        {
            Inventory.Instance.ShowEarnItem3(QuestManager.Instance.id.ToArray(), QuestManager.Instance.hw.ToArray(),
                false);
            
            LogManager.Log_CrystalEarn($"퀘스트 {id.Count}개 완료");
            alertmanager.Instance.NotiCheck_Quest();
            Savemanager.Instance.SaveInventory();
            Savemanager.Instance.SaveCash();
            Savemanager.Instance.SaveAchieve();
            Savemanager.Instance.Save();
        }
       
    }

}
