using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Doozy.Engine.UI;
using EasyMobile.Scripts.Modules.InAppPurchasing;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTotalManager : MonoBehaviour
{
    
    //싱글톤만들기.
    private static TutorialTotalManager _instance = null;

    public static TutorialTotalManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(TutorialTotalManager)) as TutorialTotalManager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    public GuideQuestSlot obj;
    public Transform trans;
    public List<GuideQuestSlot> slots = new List<GuideQuestSlot>();

    public UIView guidepanel;




    [Button(ButtonSizes.Large),GUIColor(0,1,0)]
    public void BtShow()
    {
        Tutorialmanager.Instance.StartTutorial(21);
    }

    [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void BtShow2()
    {
        PlayerBackendData.Instance.tutocount = 0;
        PlayerBackendData.Instance.tutoid = "14";
        Tutorialmanager.Instance.Refresh();
        Tutorialmanager.Instance.ShowArrowObj(PlayerBackendData.Instance.tutoid);
    }

[SerializeField]
    private bool isnotrigger;
    
    
    
    public void CheckGuideQuest(string trigger)
    {
        if (isnotrigger)
            return;
        
        if (PlayerBackendData.Instance.tutoguideisfinish) return;
        
        if(TutorialTotalManager.Instance.slots.Count < PlayerBackendData.Instance.tutoguideid)
            return;
        
            if (growthguideDB.Instance.Find_id(PlayerBackendData.Instance.tutoguideid.ToString()).type.Equals(trigger))
            {
                PlayerBackendData.Instance.tutoguideisfinish = true;
                RefreshNow();
                Savemanager.Instance.SaveGuideQuest();
                alertmanager.Instance.NotiCheck_GuideQuest();
            }
    }

    //즉시완료 체크
    public void CheckFinish()
    {

        if (isnotrigger)
            return;
        if (int.Parse(Tutorialmanager.Instance.maxlv) > int.Parse(PlayerBackendData.Instance.tutoid))
        {
            return;
        }

        int num = 0;

        if (PlayerBackendData.Instance.tutoguideisfinish)
            return;
        switch (PlayerBackendData.Instance.tutoguideid)
        {
            case 1:
                //아이템이 없다면 완료
                if (PlayerBackendData.Instance.CheckItemCount("531") == 0)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                }
                break;
            case 2:
                //장비 점수 22000
                if (PlayerData.Instance.GetEquipPoint(PlayerBackendData.Instance.EquipEquiptment0) >= 30000)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                }

                break;
            case 3:
                //모험 랭크 4
                if (PlayerBackendData.Instance.GetAdLv() >= 15)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                }

                break;
          
            case 6:
                //300 레벨 어빌리티 선택
                if (PlayerBackendData.Instance.Abilitys[0] != "")
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                }

                break;
            case 7:
                num = 0;

                for (int i = 0; i < PlayerBackendData.Instance.PassiveClassId.Length; i++)
                {
                    if (PlayerBackendData.Instance.PassiveClassId[i] != "")
                    {
                        num++;
                    }
                }
                //패시브5개
                if (num >= 6)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                }

                break;
            case 8: //★5 이상 직업 선택
                if (PlayerBackendData.Instance.ClassId == "C1018"
                    || PlayerBackendData.Instance.ClassId == "C1019"
                    || PlayerBackendData.Instance.ClassId == "C1020"
                    || PlayerBackendData.Instance.ClassId == "C1021"
                    || PlayerBackendData.Instance.ClassId == "C1022"
                    || PlayerBackendData.Instance.ClassId == "C1023"
                    || PlayerBackendData.Instance.ClassId == "C1024"
                    || PlayerBackendData.Instance.ClassId == "C1025"
                    || PlayerBackendData.Instance.ClassId == "C1026"
                    || PlayerBackendData.Instance.ClassId == "C1027"
                    || PlayerBackendData.Instance.ClassId == "C1028"
                    || PlayerBackendData.Instance.ClassId == "C1029"
                    || PlayerBackendData.Instance.ClassId == "C1030"
                    || PlayerBackendData.Instance.ClassId == "C1031"
                    || PlayerBackendData.Instance.ClassId == "C1032"
                    || PlayerBackendData.Instance.ClassId == "C1033"
                    || PlayerBackendData.Instance.ClassId == "C1034"
                    || PlayerBackendData.Instance.ClassId == "C1035"
                    )
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                }

                break;
            case 9:
                //스킬 4개 이상 장착
                for (int i = 0;
                     i < PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1.Length;
                     i++)
                {
                    if (PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[i] != "")
                    {
                        num++;
                    }
                }

                if (num >= 4)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                }

                break;
            case 12:
                //성물 전쟁 콘텐츠 하기
                //제단 레벨이 1이상이면 완료
                if (PlayerBackendData.Instance.GetAllAltarlv()[0] > 0)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                }

                break;
            case 13:
            case 14:
                /*
                성장 패스 튜토리얼 진행
                서버에 데이터 하기
                길드 튜토리얼 진행
                */
                break;
            case 15:
                /*
                성장 패스 튜토리얼 진행
                서버에 데이터 하기
                길드 튜토리얼 진행
                */
                if (PlayerBackendData.Instance.ishaveguild)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();
                }
                break;
            case 16: //사냥터 광신도 마을 이상 진입
                if (PlayerBackendData.Instance.GetFieldLv() >= 32)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();
                }

                break;

            case 20: //★6 이상 직업 선택
                if (PlayerBackendData.Instance.ClassId == "C1023"
                    || PlayerBackendData.Instance.ClassId == "C1024"
                    || PlayerBackendData.Instance.ClassId == "C1025"
                    || PlayerBackendData.Instance.ClassId == "C1026"
                    || PlayerBackendData.Instance.ClassId == "C1027"
                    || PlayerBackendData.Instance.ClassId == "C1028"
                    || PlayerBackendData.Instance.ClassId == "C1029"
                       || PlayerBackendData.Instance.ClassId == "C1030"
                    || PlayerBackendData.Instance.ClassId == "C1031"
                    || PlayerBackendData.Instance.ClassId == "C1032"
                    || PlayerBackendData.Instance.ClassId == "C1033"
                    || PlayerBackendData.Instance.ClassId == "C1034"
                    || PlayerBackendData.Instance.ClassId == "C1035"
                    )
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }

                break;

            case 21:
                //스킬 4개 이상 장착

                for (int i = 0;
                     i < PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1.Length;
                     i++)
                {
                    if (PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[i] != "")
                    {
                        num++;
                    }
                }

                if (num >= 6)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();
                }

                break;

            case 22:
                if (Battlemanager.Instance.mainplayer.Stat_SmeltPoint >= 10)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();
                }
                break;

            case 23: 
                //레이드 - 엘더 본 드래곤 처치
                if (PlayerBackendData.Instance.sotang_raid.Contains("5007"))
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }
                break;
            case 25:
                //장비 점수 26000
                if (PlayerData.Instance.GetEquipPoint(PlayerBackendData.Instance.EquipEquiptment0) >= 35000)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }

                break;
            case 26:
                //개미굴 10층까지 진행
                if (PlayerBackendData.Instance.AntCaveLv >= 10)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }

                break;
            case 27:
                //성물 전쟁 콘텐츠 하기
                //제단 레벨이 1이상이면 완료
                if (PlayerBackendData.Instance.GetAllAltarlv()[3] > 0)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }

                break;
            case 28:
                if (PlayerBackendData.Instance.sotang_raid.Contains("5008"))
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }
                break;
            case 29:
                //레이드 - 엘더 본 드래곤 처치
                if (PlayerBackendData.Instance.sotang_dungeon.Contains("3006"))
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }

                break;
            
            case 30:
                //룰렛
                if (PlayerBackendData.Instance.RouletteCount[0] > 0)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }

                break;
            case 34: //레이드 - 홀리 그리폰
                if (PlayerBackendData.Instance.sotang_raid.Contains("5009"))
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }
                break;
            case 35:
                //장비 점수 35000
                if (PlayerData.Instance.GetEquipPoint(PlayerBackendData.Instance.EquipEquiptment0) >= 35000)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                }

                break;
            case 36:
                if (Battlemanager.Instance.mainplayer.Stat_SmeltPoint >= 15)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();
                }
                break;
            case 38:
                //모험 랭크 4
                if (PlayerBackendData.Instance.GetAdLv() >= 16)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                }

                break;

            case 39: //레이드 - 홀리 아켄젤
                if (PlayerBackendData.Instance.sotang_raid.Contains("5010"))
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }
                break;
            case 40:
                //장비 점수 45000
                if (PlayerData.Instance.GetEquipPoint(PlayerBackendData.Instance.EquipEquiptment0) >= 45000)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                }

                break;
            
            case 41:
                if (Battlemanager.Instance.mainplayer.Stat_SmeltPoint >= 20)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();
                }
                break;
            
            case 44: //레이드 - 홀리 아켄젤
                if (PlayerBackendData.Instance.sotang_raid.Contains("5011"))
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }
                break;
            case 46:
                //장비 점수 26000
                if (PlayerData.Instance.GetEquipPoint(PlayerBackendData.Instance.EquipEquiptment0) >= 50000)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();
                }
                break;
            case 47: //노스테라 클리어
                if (PlayerBackendData.Instance.sotang_dungeon.Contains("3009"))
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }
                break;
            case 48: //레이드 - 엔트로 위버
                if (PlayerBackendData.Instance.sotang_raid.Contains("5012"))
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();
                }
                break;
            case 49: //잊혀진 공허 클리어
                if (PlayerBackendData.Instance.sotang_dungeon.Contains("3010"))
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }
                break;
        }
    }

    //현잭 가이드
    public UIButton getbuttons;
    public Text now_title;
    public Text now_info;
    public itemiconslot[] now_reward;

    private void Start()
    {
        for (int i = 0; i < growthguideDB.Instance.NumRows(); i++)
        {
            GuideQuestSlot slot = Instantiate(obj, trans);
            slots.Add(slot);
        }
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < slots.Count; i++)
        {  
            slots[i].Refresh(growthguideDB.Instance.GetAt(i));
            slots[i].gameObject.SetActive(true);
        }
        TutorialTotalManager.Instance.RefreshNow();
        TutorialTotalManager.Instance.CheckFinish();
    }

    private growthguideDB.Row nowdata;
    public GameObject finishendobj;
    public GameObject nowquestpanel;
    public GameObject premiumpanel;
    public GameObject nowpremiumobj;
    public GameObject selectclass;
    public GameObject gobutton;
    //91.8
   public void RefreshNow()
    {
        getbuttons.Interactable = false;
        finishendobj.SetActive(false);
        TutoButton.SetActive(false);
        nowquestpanel.SetActive(false);
        selectclass.SetActive(false);
        try
        {
            premiumpanel.SetActive(!PlayerBackendData.Instance.tutoguidepremium);
            nowpremiumobj.SetActive(PlayerBackendData.Instance.tutoguidepremium);
            if (PlayerBackendData.Instance.tutoguideid.Equals(int.Parse(slots[^1].id)+1))
            {
                finishendobj.SetActive(true);
                nowquestpanel.SetActive(false);
                Tutopanel.SetActive(false);

                for (int i = 0; i < slots.Count; i++)
                {
                    slots[i].RefreshQuest();
                }
                isnotrigger = true;
                return;
            }
            else
            {
                nowquestpanel.SetActive(true);
                Tutopanel.SetActive(true);
            }
            RefreshInfo();
            if (!isnotrigger)
            {
                nowdata = growthguideDB.Instance.Find_id(PlayerBackendData.Instance.tutoguideid.ToString());
                now_title.text = Inventory.GetTranslate(nowdata.name);
                now_info.text = Inventory.GetTranslate(nowdata.info);
                now_reward[0].Refresh(nowdata.itemid, decimal.Parse(nowdata.howmany), false);
                now_reward[0].gameObject.SetActive(true);

                now_reward[1].Refresh("1011", decimal.Parse(nowdata.exp), false);
                now_reward[1].gameObject.SetActive(true);

                if (PlayerBackendData.Instance.tutoguideisfinish)
                {
                    getbuttons.Interactable = true;
                }
            }
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].RefreshQuest();
            }
            if (nowdata.id.Equals("0"))
            {
                selectclass.SetActive(true);
                
                if(PlayerBackendData.Instance.tutoguideisfinish)
                    selectclass.SetActive(false);
            }
            if (nowdata.istuto.Equals("TRUE"))
            {
                TutoButton.SetActive(true);
                
                if(PlayerBackendData.Instance.tutoguideisfinish)
                    TutoButton.SetActive(false);
            }
        }
        catch (Exception e)
        {
            isnotrigger = true;
            Debug.Log("여기");
            Console.WriteLine(e);
            throw;
        }

    }
   public RectTransform rectreward;
   float rectperlv = 91.8f;

   public void Bt_GetBox()
   {
       if (isnotrigger)
           return;

       if (!PlayerBackendData.Instance.tutoguideisfinish)
           return;
       List<string> ids = new List<string>();
       List<int> hw = new List<int>();
       for (int j = 0; j < (PlayerBackendData.Instance.tutoguidepremium ? 2 : 1); j++)
       {
           for (int i = 0; i < now_reward.Length; i++)
           {
               Inventory.Instance.AddItem(now_reward[i].id, (int)now_reward[i].count, false);
               ids.Add(now_reward[i].id);
               hw.Add((int)now_reward[i].count);
           }
       }

       switch (PlayerBackendData.Instance.tutoguideid)
       {
           case 12:
               Levelshop.Instance.GiveTime2(0, 2);
               break;
           
           case 18:
               Levelshop.Instance.GiveTime2(3, 2);
               break;
           case 19:
               Tutorialmanager.Instance.review.SetActive(true);
               break;
           case 24:
               Levelshop.Instance.GiveTime2(6, 2);
               break;
           
           case 30:
               Levelshop.Instance.GiveTime2(9, 2);
               break;
           
           case 43:
               Levelshop.Instance.GiveTime2(12, 2);
               break;
           
           case 47:
               Levelshop.Instance.GiveTime2(15, 2);
               break;
           
           case 50:
               Levelshop.Instance.GiveTime2(18, 2);
               break;
           case 51:
               Levelshop.Instance.GiveTime2(21, 2);
               break;
           case 52:
               Levelshop.Instance.GiveTime2(24, 2);
               break;
       }



       Inventory.Instance.ShowEarnItem3(ids.ToArray(), hw.ToArray(), false);
       PlayerBackendData.Instance.tutoguideisfinish = false;
       LogManager.LogTutorialGuide(PlayerBackendData.Instance.tutoguideid.ToString());
       PlayerBackendData.Instance.tutoguideid++;
       CheckFinish();
       RefreshNow();
       Savemanager.Instance.SaveGuideQuest();
       alertmanager.Instance.NotiCheck_GuideQuest();
       Savemanager.Instance.Save();
       Settingmanager.Instance.SaveTutoData();
       RefreshScrolbar();
   }

   public void RefreshScrolbar()
    {
        rectreward.anchoredPosition = new Vector3(0f,(rectperlv * PlayerBackendData.Instance.tutoguideid),0f);
    }


    public GameObject TutoButton;
    public void Bt_StartTutorial()
    {
        if(PlayerBackendData.Instance.tutoguideisfinish)
            TutoButton.SetActive(false);
        string num = growthguideDB.Instance.Find_id(PlayerBackendData.Instance.tutoguideid.ToString()).count;
        
        Debug.Log("튜토" + num);
        Tutorialmanager.Instance.StartTutorial(int.Parse(num));
    }


    public GameObject Tutopanel;
    public Text infotext;
    
    
    public void RefreshInfo()
    {
        if (!Tutorialmanager.Instance.Tutopanel.activeSelf &&nowquestpanel.activeSelf)
        {
            Tutopanel.SetActive(true);
        }
        else
        {
            Tutopanel.SetActive(false);
            return;
        }
        infotext.text = 
            $"{Inventory.GetTranslate(growthguideDB.Instance.Find_id(PlayerBackendData.Instance.tutoguideid.ToString()).name)}";

    }
    
    //바로가기
    public void Bt_Go()
    {
        
    }
    
    
    public void Bt_BuyGuidePass()
    {
        InAppPurchasing.Purchase("rpgg2.growthguide.premium");
    }
}
