using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using LitJson;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class alertmanager : MonoBehaviour
{
    private static alertmanager _instance = null;
    public static alertmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(alertmanager)) as alertmanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    public GameObject alertobj;
    public Text AlertText;

    public void ShowAlert(string text ,alertenum alerttype)
    {
        if (alertobj.activeSelf)
            alertobj.SetActive(false);
        AlertText.text = text;
        switch (alerttype)
        {
            case alertenum.일반:
                AlertText.color = Color.white;
                break;

            case alertenum.주의:
                AlertText.color = Color.red;
                
                break;
        }
        alertobj.SetActive(true);
    }
    public enum alertenum
    {
        주의,
        일반
    }

    
    public GameObject alertobj2;
    public Text AlertText2;

    public void ShowAlert2(string text ,alertenum alerttype)
    {
        if (alertobj2.activeSelf)
            alertobj2.SetActive(false);
        AlertText2.text = text;
        switch (alerttype)
        {
            case alertenum.일반:
                AlertText.color = Color.white;
                break;

            case alertenum.주의:
                AlertText.color = Color.red;
                
                break;
        }
        alertobj2.SetActive(true);
    }


    public GameObject[] Alert_Post;
    public GameObject[] Alert_PremiumShop;
    public GameObject Alert_Menu;

    public GameObject[] Alert_AutoFarm;
    public GameObject[] Alert_ClassAlert; //아래가 하나라도 있으면 전부
    public GameObject[] Alert_Craft; //아래가 하나라도 있으면 전부
    public GameObject[] Alert_Achieve; //아래가 하나라도 있으면 전부
    public GameObject[] Alert_Collect; //아래가 하나라도 있으면 전부
    public GameObject[] Alert_Guild_Apply; //아래가 하나라도 있으면 전부
    public GameObject[] Alert_Guild_Check; //아래가 하나라도 있으면 전부
    public GameObject[] Alert_GuidequestCheck; //가이드퀘스트
    public GameObject[] Alert_SeasonPass; //가이드퀘스트

    public GameObject[] Alert_Pet; //가이드퀘스트
    public GameObject[] Alert_Roullet; //가이드퀘스트
    public GameObject[] Alert_Raid; //가이드퀘스트
    public GameObject[] Alert_Ability; //가이드퀘스트
    public GameObject[] Alert_Altar; //아래가 하나라도 있으면 전부

    
    private void Start()
    {
        NotiCheck_Post();
        NotiCheck_PremiumShop();
        Bt_ClickMenu();
        NotiCheck_Achieve();
        NotiCheck_GuideQuest();
    }

    void NotiCheck_base()
    {
        foreach (var t in Alert_Post)
        {
            t.SetActive(false);;
        }
        
        
        bool ishave = false;
    }


    //길드 승인가입신청이 오면.
    public void NotiGuildApplyTomine()
    {
        foreach (var t in Alert_Guild_Apply)
        {
            t.SetActive(true);
        }
    }

    public void FinishApply()
    {
        foreach (var t in Alert_Guild_Apply)
        {
            t.SetActive(false);
        }
    }
    
    
    public void NotiCheck_Collection()
    {
     return;
        bool haveequip = false;
        bool haveitem = false;
        foreach (var t in PlayerBackendData.Instance.CollectData )
        {
                switch(CollectionDB.Instance.Find_id(t.Key).collecttype)
                {
                    case "equip":
                        if (t.Value.CheckCountEquip() && !haveequip)
                        {
                            haveequip = true;
                            break;
                        }
                        
                        if (!Alert_Menu.activeSelf)
                            Alert_Menu.SetActive(true);
                        break;
                    case "item":
                        if (t.Value.CheckCountItem()&& !haveitem)
                        {
                            haveitem = true;
                            break;
                        }
                        break;
                }

                if (haveequip && haveitem)
                    break;
                //완료된 것.
        }

        if (haveequip || haveitem)
        {
            Alert_Collect[2].SetActive(haveequip); //재료버튼
            Alert_Collect[1].SetActive(haveitem); //재료버튼
            Alert_Collect[0].SetActive(true); //수집버튼
            if (!Alert_Menu.activeSelf)
                Alert_Menu.SetActive(true);
        }
        
   
        
       
       
       
    }
    
    
    //업적
    public void NotiCheck_Achieve()
    {
        foreach (var t in Alert_Achieve)
        {
            t.SetActive(false);
        }
        achievemanager.Instance.allfinishnoti.SetActive(false);

//        Debug.Log(achievemanager.Instance.acheveslots.Count);
        foreach (var t in achievemanager.Instance.acheveslots.Where(t => t.data.Curcount >= t.data.Maxcount &&
                                                                         !t.data.Isfinish
                                                                         && AchievementDB.Instance.Find_id(t.data.Id)
                                                                             .islast == "0"))
        {
//            Debug.Log(t.data.Id + "업적");
            if (AchievementDB.Instance.Find_id(t.data.Id).paneltype == "daily")
            {
                Alert_Achieve[0].SetActive(true);
                Alert_Achieve[1].SetActive(true);
                achievemanager.Instance.allfinishnoti.SetActive(true);
                if (!Alert_Menu.activeSelf)
                    Alert_Menu.SetActive(true);
            }
            else if (AchievementDB.Instance.Find_id(t.data.Id).paneltype == "forever")
            {
                Alert_Achieve[0].SetActive(true);
                Alert_Achieve[2].SetActive(true);
                achievemanager.Instance.allfinishnoti.SetActive(true);


                if (!Alert_Menu.activeSelf)
                    Alert_Menu.SetActive(true);
            }
            else if (AchievementDB.Instance.Find_id(t.data.Id).paneltype == "event")
            {
                Alert_Achieve[0].SetActive(true);
                Alert_Achieve[3].SetActive(true);
                achievemanager.Instance.allfinishnoti.SetActive(true);

                if (!Alert_Menu.activeSelf)
                    Alert_Menu.SetActive(true);
            }
            else if (AchievementDB.Instance.Find_id(t.data.Id).paneltype == "crystal")
            {
                Alert_Achieve[0].SetActive(true);
                Alert_Achieve[4].SetActive(true);
                achievemanager.Instance.allfinishnoti.SetActive(true);

                if (!Alert_Menu.activeSelf)
                    Alert_Menu.SetActive(true);
            }
        }
    }


    //직업 -해금 가능한 직업이 있다면
    public void NotiCheck_Class()
    {
        foreach (var t in Alert_ClassAlert)
        {
            t.SetActive(false);;
        }
        //1클래스 
         foreach(var t in Classmanager.Instance.JobSlots)
        {
            if (t.CheckNoti())
            {
                if(!Alert_Menu.activeSelf)
                    Alert_Menu.SetActive(true);
                foreach (var a in Alert_ClassAlert)
                {
                    a.SetActive(true);
                }
                
                return;
            }
        }

        
    }
    
    //자동 보상
    public void NotiCheck_AutoFarm(bool istrue)
    {
        if (istrue)
        {
            foreach (var t in Alert_AutoFarm)
            {
                t.SetActive(true);
            }      
            if(!Alert_Menu.activeSelf)
                Alert_Menu.SetActive(true);
            return;
        }
        
        foreach (var t in Alert_AutoFarm)
        {
            t.SetActive(false);;
        }
        if (Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.빠른전투] > 0
            || Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.광고_사냥1시간] > 0)
        {
            foreach (var t in Alert_AutoFarm)
            {
                t.SetActive(true);
            }       
            if(!Alert_Menu.activeSelf)
                Alert_Menu.SetActive(true);
        }
    }
    
    
    public void NotiCheck_PremiumShop()
    {
        foreach (var t in Alert_PremiumShop)
        {
            t.SetActive(false);
        }

        if (Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.광고_등급재설정] > 0
            || Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.광고_도전장] > 0
            || Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.광고_엘리축복] > 0
            || Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.광고_제련석] > 0
            || Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.광고_특효재설정] > 0
            || Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.광고_품질재설정] > 0
           )
        {
                
            foreach (var t in Alert_PremiumShop)
            {
                t.SetActive(true);
            }
        }
    }
    
    
    
    
      
    //모험패
    public void NotiCheck_Pass()
    {
        foreach (var t in Alert_SeasonPass)
        {
            t.SetActive(false);
        }
        if ( SeasonPass.Instance. ishavereward())
        {
            foreach (var t in Alert_SeasonPass)
            {
                t.SetActive(true);
            }
        }

        LoginEventManager.Instance.RefreshPanel();
    }
    
    
    //성장 가이드 퀘스트 
    public void NotiCheck_GuideQuest()
    {
        foreach (var t in Alert_GuidequestCheck)
        {
            t.SetActive(false);
        }

        TutorialTotalManager.Instance.CheckFinish();

        if (PlayerBackendData.Instance.tutoguideisfinish)
        {
            foreach (var t in Alert_GuidequestCheck)
            {
                t.SetActive(true);
            }
        }
    }

    public void NotiCheck_Altar()
    {
        foreach (var t in Alert_Altar)
        {
            t.SetActive(false);
        }
        
        altarmanager.Instance.RefreshCounts();

        if (PlayerBackendData.Instance.CheckItemCount("1712") >= altarmanager.Instance.CheckAltarCount[0])
        {
            if(!Alert_Menu.activeSelf)
                Alert_Menu.SetActive(true);
            Alert_Altar[0].SetActive(true);
            Alert_Altar[1].SetActive(true);
        }
        
        if (PlayerBackendData.Instance.GetMoney() >= altarmanager.Instance.CheckAltarCount[1])
        {
            if(!Alert_Menu.activeSelf)
                Alert_Menu.SetActive(true);
            Alert_Altar[0].SetActive(true);
            Alert_Altar[2].SetActive(true);
        }
        
        if (PlayerBackendData.Instance.CheckItemCount("2908") >= altarmanager.Instance.CheckAltarCount[2])
        {
            if(!Alert_Menu.activeSelf)
                Alert_Menu.SetActive(true);
            Alert_Altar[0].SetActive(true);
            Alert_Altar[3].SetActive(true);
        }
        
        if (PlayerBackendData.Instance.CheckItemCount("1714") >= altarmanager.Instance.CheckAltarCount[3])
        {
            if(!Alert_Menu.activeSelf)
                Alert_Menu.SetActive(true);
            Alert_Altar[0].SetActive(true);
            Alert_Altar[4].SetActive(true);
        }
    }

    public void NotiCheck_Roullet()
    {
        foreach (var t in Alert_Roullet)
        {
            t.SetActive(false);
        }
        
        if (PlayerBackendData.Instance.CheckItemCount("40003") >= 1)
        {
            if(!Alert_Menu.activeSelf)
                Alert_Menu.SetActive(true);
            Alert_Roullet[0].SetActive(true);
            Alert_Roullet[1].SetActive(true);
        }
        
        if (PlayerBackendData.Instance.CheckItemCount("40004") >= 1)
        {
            if(!Alert_Menu.activeSelf)
                Alert_Menu.SetActive(true);
            Alert_Roullet[0].SetActive(true);
            Alert_Roullet[2].SetActive(true);
        }
        
        if (PlayerBackendData.Instance.CheckItemCount("40005") >= 1)
        {
            if(!Alert_Menu.activeSelf)
                Alert_Menu.SetActive(true);
            Alert_Roullet[0].SetActive(true);
            Alert_Roullet[3].SetActive(true);
        }
    }


    public void NotiCheck_Ability()
    {
        foreach (var t in Alert_Ability)
        {
            t.SetActive(false);
        }

        int[] lv = new int[] { 300, 450,600,750,900,1200,1500 };
        for (int i = 0; i < lv.Length; i++)
        {
            if (lv[i] <= PlayerBackendData.Instance.GetLv() && PlayerBackendData.Instance.Abilitys[i] == "")
            {
                if(!Alert_Menu.activeSelf)
                    Alert_Menu.SetActive(true);
                Alert_Ability[0].SetActive(true);
                Alert_Ability[i+1].SetActive(true);
            }
        }
    }
    
    public void NotiCheck_Pet()
    {
        foreach (var t in Alert_Pet)
        {
            t.SetActive(false);
        }

        if (PlayerBackendData.Instance.CheckItemCount("50006") >= 1)
        {
            foreach (var t in Alert_Pet)
            {
                if(!Alert_Menu.activeSelf)
                    Alert_Menu.SetActive(true);
                t.SetActive(true);
            }
        }
    }

    
    public void PostNotiOn()
    {
        foreach (var t in Alert_Post)
        {
            t.SetActive(true);
        }
    }
    //우편 noti
    public void NotiCheck_Post(BackendReturnObject admin = null,BackendReturnObject rank = null)
    {
        foreach (var t in Alert_Post)
        {
            t.SetActive(false);
        }

        bool ishave = false;
        if (admin == null)
        {
            SendQueue.Enqueue(Backend.UPost.GetPostList, PostType.Admin, 1, callback =>
            {
                if (callback.IsSuccess())
                {
                    JsonData json = callback.GetReturnValuetoJSON()["postList"];

                    if (json.Count > 0)
                    {
                        ishave = true;
                        foreach (var t in Alert_Post)
                        {
                            t.SetActive(true);
                        }

                        return;
                    }
                }
            });
        }
        else
        {
            if (admin.IsSuccess())
            {
                JsonData json = admin.GetReturnValuetoJSON()["postList"];

                if (json.Count > 0)
                {
                    ishave = true;
                    foreach (var t in Alert_Post)
                    {
                        t.SetActive(true);
                    }

                    return;
                }
            }
        }

        if (rank == null)
        {
            SendQueue.Enqueue(Backend.UPost.GetPostList, PostType.Rank, 1, callback =>
            {
                if (callback.IsSuccess())
                {
                    JsonData json = callback.GetReturnValuetoJSON()["postList"];

                    if (json.Count > 0)
                    {
                        ishave = true;
                        foreach (var t in Alert_Post)
                        {
                            t.SetActive(true);
                            ;
                        }

                        return;
                    }
                }
            });
        }
        else
        {
            if (rank.IsSuccess())
            {
                JsonData json = rank.GetReturnValuetoJSON()["postList"];

                if (json.Count > 0)
                {
                    ishave = true;
                    foreach (var t in Alert_Post)
                    {
                        t.SetActive(true);
                    }

                    return;
                }
            }
        }
    }


    public void Bt_ClickMenu()
    {
        Alert_Menu.SetActive(false);
        NotiCheck_AutoFarm(false);
        NotiCheck_Class();
        NotiCheck_AutoFarm(false);
        NotiCheck_Achieve();
        NotiCheck_Altar();
        NotiCheck_Collection();
        CollectionRenewalManager.Instance.RefreshTotalCount();
        NotiCheck_PremiumShop();
        NotiCheck_Pass();
        NotiCheck_Pet();
        NotiCheck_Roullet();
        NotiCheck_Ability();
    }

    public GameObject EventNoti;
}
