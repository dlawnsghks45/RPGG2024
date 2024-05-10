using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    [SerializeField] private itemiconslot itemreward;
    [SerializeField] private itemiconslot achexpreward;
    [SerializeField] private itemiconslot seoasonexpreward;
    public string type;
    public string subtype;
    public int Questid;
    public float maxcount;
    public Text QuestName;
    public Text QuestInfo;

    public Image Gauge;
    public Text Count;
    
    public GameObject DoingBt;
    public GameObject RewardBt;
    public GameObject FinishBt;
    
    public void initdata()
    {
        type = QuestDB.Instance.Find_id(Questid.ToString()).type;
        subtype = QuestDB.Instance.Find_id(Questid.ToString()).subtype;
        itemreward.Refresh(QuestDB.Instance.Find_id(Questid.ToString()).itemid,
            decimal.Parse(QuestDB.Instance.Find_id(Questid.ToString()).itemhw),false,false);
        achexpreward.Refresh("999",
            decimal.Parse(QuestDB.Instance.Find_id(Questid.ToString()).questexp),false,false);
        seoasonexpreward.Refresh("998",
            decimal.Parse(QuestDB.Instance.Find_id(Questid.ToString()).seasonpt),false,false);

        
        
        maxcount = int.Parse(QuestDB.Instance.Find_id(Questid.ToString()).count);
        QuestName.text = Inventory.GetTranslate(QuestDB.Instance.Find_id(Questid.ToString()).name);
        QuestInfo.text = Inventory.GetTranslate(QuestDB.Instance.Find_id(Questid.ToString()).info);
        Refresh();
    }

    private bool showreward;

    public void Refresh()
    {
        Count.text = $"{PlayerBackendData.Instance.QuestCount[Questid]:N0} / {maxcount:N0}";
        Gauge.fillAmount = PlayerBackendData.Instance.QuestCount[Questid] / maxcount;
        DoingBt.SetActive(false);
        RewardBt.SetActive(false);
        FinishBt.SetActive(false);

        if (!PlayerBackendData.Instance.QuestIsFinish[Questid])
        {
            if (PlayerBackendData.Instance.QuestCount[Questid] >= maxcount)
            {
                if (!showreward)
                {
                    //1번만나오게
                    showreward = true;
                    QuestManager.Instance.ShowFinish(Questid.ToString());
                }

                RewardBt.SetActive(true);
               // transform.SetAsFirstSibling();

            }
            else
            {
                DoingBt.SetActive(true);
                showreward = false;
            }
        }
        else
        {
            FinishBt.SetActive(true);
           // transform.SetAsLastSibling();
        }
    }

    public void AddCount(int count)
    {
        if(PlayerBackendData.Instance.QuestCount[Questid] > maxcount)
            return;
        
        PlayerBackendData.Instance.QuestCount[Questid] += count;
        if (PlayerBackendData.Instance.QuestCount[Questid] > maxcount)
            PlayerBackendData.Instance.QuestCount[Questid] = maxcount;
        
        Refresh();
    }
    
    public void Bt_GetReward()
    {
        if(PlayerBackendData.Instance.QuestIsFinish[Questid])
            return;
        QuestManager.Instance.id.Clear();
        QuestManager.Instance.hw.Clear();
        
        //보상지급
        if (PlayerBackendData.Instance.QuestCount[Questid] >= maxcount)
        {
            PlayerBackendData.Instance.QuestIsFinish[Questid] = true;
            Inventory.Instance.AddItem(QuestDB.Instance.Find_id(Questid.ToString()).itemid, int.Parse(
                QuestDB.Instance.Find_id(Questid.ToString()).itemhw), false, false);
          
            Inventory.Instance.AddItem("998", int.Parse(
                QuestDB.Instance.Find_id(Questid.ToString()).seasonpt), false, false);

            if (QuestDB.Instance.Find_id(Questid.ToString()).itemid.Equals("1001"))
            {
                     LogManager.EarnCrystal += int.Parse(
                    QuestDB.Instance.Find_id(Questid.ToString()).itemhw);
            }
           
            QuestManager.Instance.id.Add(QuestDB.Instance.Find_id(Questid.ToString()).itemid);
            QuestManager.Instance.hw.Add(int.Parse(
                QuestDB.Instance.Find_id(Questid.ToString()).itemhw));

            int pointnum = int.Parse(QuestDB.Instance.Find_id(Questid.ToString()).pointtotalnum);
            QuestManager.Instance.Questdatas[pointnum].AddTotalExp(float.Parse(QuestDB.Instance.Find_id(Questid.ToString()).givept));
            PlayerData.Instance.EarnAchExp(decimal.Parse(QuestDB.Instance.Find_id(Questid.ToString()).questexp));
            
            Refresh();
            
            LogManager.Log_CrystalEarn("퀘스트단일");
            alertmanager.Instance.NotiCheck_Quest();
        }
    }
    
    public void GetReward_All()
    {
        if(PlayerBackendData.Instance.QuestIsFinish[Questid])
            return;
        //보상지급
        if (PlayerBackendData.Instance.QuestCount[Questid] >= maxcount)
        {
            PlayerBackendData.Instance.QuestIsFinish[Questid] = true;
            Inventory.Instance.AddItem(QuestDB.Instance.Find_id(Questid.ToString()).itemid, int.Parse(
                QuestDB.Instance.Find_id(Questid.ToString()).itemhw), false, false);
           
            Inventory.Instance.AddItem("998", int.Parse(
                QuestDB.Instance.Find_id(Questid.ToString()).seasonpt), false, false);
            
            if (QuestDB.Instance.Find_id(Questid.ToString()).itemid.Equals("1001"))
            {
                LogManager.EarnCrystal += int.Parse(
                    QuestDB.Instance.Find_id(Questid.ToString()).itemhw);
            }
            
            QuestManager.Instance.id.Add(QuestDB.Instance.Find_id(Questid.ToString()).itemid);
            QuestManager.Instance.hw.Add(int.Parse(
            QuestDB.Instance.Find_id(Questid.ToString()).itemhw));
            int pointnum = int.Parse(QuestDB.Instance.Find_id(Questid.ToString()).pointtotalnum);
            QuestManager.Instance.Questdatas[pointnum].AddTotalExpAll(float.Parse(QuestDB.Instance.Find_id(Questid.ToString()).givept));
            PlayerData.Instance.EarnAchExp(decimal.Parse(QuestDB.Instance.Find_id(Questid.ToString()).questexp));
            SeasonPass.Instance.
            Refresh();
            Settingmanager.Instance.SaveQuest();
        }
    }
}
