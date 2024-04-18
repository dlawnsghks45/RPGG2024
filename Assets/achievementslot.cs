using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class achievementslot : MonoBehaviour
{
    public int num = 0;
    public GameObject noti;
    public string type;
    public int cur;
    public int max;

    [SerializeField]
    public Achievedata data;

    public string id;
    public string coreid;
    public itemiconslot Reward;
    public Text Achname;
    public gaugebarslot gauge;
    public GameObject[] RewardButtons; //0은 진행 //1은 완료
   

    private void Start()
    {
     
        
        type = AchievementDB.Instance.Find_coreid(coreid).type;
        Refresh();
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        data = PlayerBackendData.Instance.PlayerAchieveData[coreid];
        
        if (num.Equals(13) && data.Id.Equals("A1144"))
        {
            data.Id = "A3332";
            data.Curcount = 0;
            data.Maxcount = int.Parse(AchievementDB.Instance.Find_id(data.Id).count);
            Savemanager.Instance.SaveAchieve();
            Savemanager.Instance
                .Save();
        }
        if (num.Equals(13) && data.Id.Equals("A1145"))
        {
            data.Id = "A3332";
            data.Curcount = 0;
            data.Maxcount = int.Parse(AchievementDB.Instance.Find_id(data.Id).count);
            Savemanager.Instance.SaveAchieve();
            Savemanager.Instance
                .Save();
        }
        if (num.Equals(13) && data.Id.Equals("A1146"))
        {
            data.Id = "A3332";
            data.Curcount = 0;
            data.Maxcount = int.Parse(AchievementDB.Instance.Find_id(data.Id).count);
            Savemanager.Instance.SaveAchieve();
            Savemanager.Instance
                .Save();
        }
        if (num.Equals(13) && data.Id.Equals("A1147"))
        {
            data.Id = "A3332";
            data.Curcount = 0;
            data.Maxcount = int.Parse(AchievementDB.Instance.Find_id(data.Id).count);
            Savemanager.Instance.SaveAchieve();
            Savemanager.Instance
                .Save();
        }


        if (num.Equals(11))
        {
//            Debug.Log(data.Id);
//            Debug.Log(coreid);
//            Debug.Log(type);
        }
        if (num.Equals(11) && type.Equals("adlvup"))
        {
            data.Id = "A1146";
            data.Achievetype = "killmonster";
            data.Curcount = 76000;
            data.Maxcount = int.Parse(AchievementDB.Instance.Find_id(data.Id).count);
            Savemanager.Instance.SaveAchieve();
            Savemanager.Instance
                .Save();
        }
//       Debug.Log(data.Id);
        //승급이면
        if (AchievementDB.Instance.Find_id(data.Id).type.Equals("adlvup"))
        {
            if (PlayerBackendData.Instance.GetAdLv() >= int.Parse(AchievementDB.Instance.Find_id(data.Id).count))
            {
                data.Curcount = PlayerBackendData.Instance.GetAdLv();
            }
        }
//        Debug.Log(data.Id);
        if(Inventory.GetTranslate(AchievementDB.Instance.Find_id(data.Id).name).Contains("{0}"))
        {
            Achname.text = string.Format(Inventory.GetTranslate(AchievementDB.Instance.Find_id(data.Id).name), AchievementDB.Instance.Find_id(data.Id).count);
        }
        else
        {
            Achname.text = Inventory.GetTranslate(AchievementDB.Instance.Find_id(data.Id).name);
        }
        gauge.RefreshBar((float)data.Curcount, (float)data.Maxcount);
        noti.SetActive(false);
       
        if (AchievementDB.Instance.Find_id(data.Id).nextlevel.Equals("0") && 
            AchievementDB.Instance.Find_id(data.Id).subtype.Equals("forever"))
        {
            //RewardButtons[0].SetActive(false);
            RewardButtons[1].SetActive(false);
            RewardButtons[2].SetActive(false);
        }
        
        if (!AchievementDB.Instance.Find_id(data.Id).nextlevel.Equals("0") && 
            AchievementDB.Instance.Find_id(data.Id).subtype.Equals("forever") &&
            data.Curcount >= data.Maxcount)
        {
            //RewardButtons[0].SetActive(false);
            RewardButtons[1].SetActive(true);
            RewardButtons[2].SetActive(false);
            noti.SetActive(true);

        }
        else if(data.Isfinish)
        {
            //RewardButtons[0].SetActive(false);
            RewardButtons[1].SetActive(false);
            RewardButtons[2].SetActive(true);
        }
        else if(data.Curcount >= data.Maxcount)
        {
           // RewardButtons[0].SetActive(false);
            RewardButtons[1].SetActive(true);
            RewardButtons[2].SetActive(false);
            noti.SetActive(true);
        }
        else
        {
            RewardButtons[1].SetActive(false);
           // RewardButtons[0].SetActive(true);
            RewardButtons[2].SetActive(false);
        }

        Reward.Refresh(AchievementDB.Instance.Find_id(data.Id).item, int.Parse(AchievementDB.Instance.Find_id(data.Id).itemhowmany), false);

    }

    public void Bt_Bt_GetReward()
    {
        Bt_GetReward(false);
        Settingmanager.Instance.SaveDataAchieveInven();
        Savemanager.Instance.SaveAchieve();
        Savemanager.Instance.Save();
    }

    public void Bt_Bt_GetReward2()
    {
        Bt_GetReward(true);
    }
    public void Bt_GetReward(bool isnotcsave = true)
    {
        if (data.Curcount < data.Maxcount) return;
        if (AchievementDB.Instance.Find_id(data.Id).nextlevel != "0" && data.Achievetype == "forever")
        {
//            Debug.Log("완료했다");
            Inventory.Instance.AddItem(Reward.id, (int)Reward.count, isnotcsave);

            if (achievemanager.Instance.ispanel)
                achievemanager.Instance.Additem(Reward.id, (int)Reward.count);
            
            data.LevelUp(isnotcsave);
            //영구 업적이면
            Refresh();
        }
        else if (AchievementDB.Instance.Find_id(data.Id).subtype == "daily")
        {
            Inventory.Instance.AddItem(Reward.id, (int)Reward.count, !isnotcsave);
            if (achievemanager.Instance.ispanel)
                achievemanager.Instance.Additem(Reward.id, (int)Reward.count);
            //영구 업적이면
            data.Isfinish = true;
            Refresh();
        }
        else
        {
            Inventory.Instance.AddItem(Reward.id, (int)Reward.count, !isnotcsave);
            if (achievemanager.Instance.ispanel)
                achievemanager.Instance.Additem(Reward.id, (int)Reward.count);
            data.Isfinish = true;
        }

        if (!isnotcsave)
        {
            Savemanager.Instance.SaveAchieveDirect();
        }
    }

}

