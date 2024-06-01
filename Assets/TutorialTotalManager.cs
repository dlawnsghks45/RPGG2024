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

    public string tutoids;
    [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void BtShow2()
    {
        PlayerBackendData.Instance.tutocount = 0;
        PlayerBackendData.Instance.tutoid = tutoids;
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

    public GameObject FinishObj;
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
            case 0: //진로 선택
                Tutorialmanager.Instance.CheckTutorial("tutoguide");
                break;
            case 1:
                //초보자 점핑 지원 물품 상자 사용
                if (PlayerBackendData.Instance.CheckItemCount("531") == 0)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }
                break;
            case 2: //물약 퀵슬릇에 모두 등록
                if (Inventory.Instance.iteminfopanel.Hpslot.HpItemId != "" &&
                    Inventory.Instance.iteminfopanel.Mpslot.HpItemId != "")
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }
                break;
            case 4: //물약 퀵슬릇에 모두 등록
                Tutorialmanager.Instance.SetNewTuto(100);
                break;

            case 5:
                //[어빌리티]어빌리티 선택하기
                if (
                         PlayerBackendData.Instance.Abilitys[0] != "" 
                    &&   PlayerBackendData.Instance.Abilitys[1] != "" 
                    &&   PlayerBackendData.Instance.Abilitys[2] != "" 
                    &&   PlayerBackendData.Instance.Abilitys[3] != "" 
                    &&   PlayerBackendData.Instance.Abilitys[4] != "" 
                    )
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }
                break;
            case 10:
                Tutorialmanager.Instance.SetNewTuto(100);
                break;
            case 11:
                Tutorialmanager.Instance.SetNewTuto(101);

                break;
            case 12:
                Tutorialmanager.Instance.SetNewTuto(102);

                break;
            case 16:
                Tutorialmanager.Instance.SetNewTuto(103);
                break;
            
            case 45:
                Tutorialmanager.Instance.SetNewTuto(105);
                break;
            case 46:
                Tutorialmanager.Instance.SetNewTuto(103);
                break;
            case 47:
                Tutorialmanager.Instance.SetNewTuto(103);
                break;
            case 18:
                //성물 전쟁 콘텐츠 하기
                //제단 레벨이 1이상이면 완료
                if (Timemanager.Instance.GetNowCount_daily(Timemanager.ContentEnumDaily.성물전쟁) == 0)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }

                break;
            case 19:
                //성물 전쟁 콘텐츠 하기
                //제단 레벨이 1이상이면 완료
                if (PlayerBackendData.Instance.Altar_Lvs[0] >= 2001)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }
                break;
            case 20: //사냥터 심해 마을 이상 진입
                if (PlayerBackendData.Instance.GetFieldLv() >= 48)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();
                }

                break;
         
            case 27: //[펫] 펫 장착하기
                if (PlayerBackendData.Instance.nowPetid != "")
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();
                }
                break;
            
            case 29:
                //[개미굴] 개미굴 10F 이상 진행하기
                if (PlayerBackendData.Instance.AntCaveLv >= 10)
                {
                    if(PlayerBackendData.Instance.nowstage == "9999")
                    {
                    Antcavemanager.Instance.Bt_FinishAuto();
                    }
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }

                break;
            
            case 31:
                //장비 점수 50000 달성
                if (PlayerData.Instance.GetEquipPoint(PlayerBackendData.Instance.EquipEquiptment0) >= 40000)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();


                }
                break;
            
            case 33:
                //[룰렛] 초보자 룰렛 10회 진행하기
                if (PlayerBackendData.Instance.RouletteCount[0] >= 10)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }

                break;
            case 34:
                //[승급] 모험 랭크 16 달성하기
                if (PlayerBackendData.Instance.GetAdLv() >= 16)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }

                break;

            case 36:
                //[룬] 룬 장착하기
                if (PlayerBackendData.Instance.EquipEquiptment0[10] != null)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }

                break;
            case 39:
                //장비 점수 50000 달성
                if (PlayerData.Instance.GetEquipPoint(PlayerBackendData.Instance.EquipEquiptment0) >= 50000)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();


                }
                break;
            
            case 8:
                int smeltnum2 = 0;
                for (int i = 0; i < PlayerBackendData.Instance.EquipEquiptment0.Length; i++)
                {
                    if (PlayerBackendData.Instance.EquipEquiptment0[i] != null)
                    {
                        smeltnum2 += PlayerBackendData.Instance.EquipEquiptment0[i].SmeltSuccCount1;
                    }
                }
                
                if (smeltnum2 >= 2)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }
                break;
            
            case 40:
                int smeltnum = 0;
                for (int i = 0; i < PlayerBackendData.Instance.EquipEquiptment0.Length; i++)
                {
                    if (PlayerBackendData.Instance.EquipEquiptment0[i] != null)
                    {
                        smeltnum += PlayerBackendData.Instance.EquipEquiptment0[i].SmeltSuccCount1;
                    }
                }

                //제련은
                Debug.Log("제련은" + smeltnum);
                
                if (smeltnum >= 25)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();

                }
                break;
            case 41:
                //[승급] 모험 랭크 16 달성하기
                if (PlayerBackendData.Instance.GetAdLv() >= 20)
                {
                    PlayerBackendData.Instance.tutoguideisfinish = true;
                    RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.Save();
                }
                break;

        }
        Savemanager.Instance.Save();
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
       
       
       FinishObj.SetActive(false);
        getbuttons.Interactable = false;
        finishendobj.SetActive(false);
        TutoButton.SetActive(false);
        nowquestpanel.SetActive(false);
        selectclass.SetActive(false);
        Tutopanel.SetActive(false);

            premiumpanel.SetActive(!PlayerBackendData.Instance.tutoguidepremium);
            nowpremiumobj.SetActive(PlayerBackendData.Instance.tutoguidepremium);
            if (PlayerBackendData.Instance.tutoguideid >= 48)
            {
                if (PlayerBackendData.Instance.tutoguideid <= int.Parse(slots[^1].id) + 1)
                {
                    PlayerBackendData.Instance.tutoguideid = int.Parse(slots[^1].id)+1;
                    Debug.Log("저장한 저" + PlayerBackendData.Instance.tutoguideid);
                    Settingmanager.Instance.SaveDataAchieveInven();

                }
                finishendobj.SetActive(true);
                nowquestpanel.SetActive(false);

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
                    FinishObj.SetActive(true);
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
   public RectTransform rectreward;
   float rectperlv = 91.8f;

   public void Bt_GetBox()
   {
       if (isnotrigger)
           return;

       if (!PlayerBackendData.Instance.tutoguideisfinish)
           return;
       List<string> ids = new List<string>();
       List<decimal> hw = new List<decimal>();
       for (int j = 0; j < (PlayerBackendData.Instance.tutoguidepremium ? 2 : 1); j++)
       {
           for (int i = 0; i < now_reward.Length; i++)
           {
               Debug.Log(now_reward[i].id);
               if (now_reward[i].id == "1011" || now_reward[i].id == "1000")
               {
                   Inventory.Instance.AddItemExp(now_reward[i].id, (decimal)now_reward[i].count, false);
               }
               else
               {
                   Inventory.Instance.AddItem(now_reward[i].id, (int)now_reward[i].count, false);
               }
               ids.Add(now_reward[i].id);
               hw.Add((decimal)now_reward[i].count);
           }
       }

       switch (PlayerBackendData.Instance.tutoguideid)
       {
           case 19:
               Levelshop.Instance.GiveTime2(0, 2);
               break;
           
          // case 18: 스킬북
           //    Levelshop.Instance.GiveTime2(3, 2);
            //   break;
           case 14:
               Tutorialmanager.Instance.review.SetActive(true);
               break;
           case 30:
               Levelshop.Instance.GiveTime2(6, 2);
               break;
           
           case 33:
               Levelshop.Instance.GiveTime2(9, 2);
               break;
           
           case 23:
               Levelshop.Instance.GiveTime2(12, 2);
               break;
           
           //case 47:
          //     Levelshop.Instance.GiveTime2(15, 2);
            //   break;
           
           case 24:
               Levelshop.Instance.GiveTime2(18, 2);
               break;
           case 36:
               Levelshop.Instance.GiveTime2(21, 2);
               break;
           case 27:
               Levelshop.Instance.GiveTime2(24, 2);
               break;
       }



       Inventory.Instance.ShowEarnItem4(ids.ToArray(), hw.ToArray(), false);
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
    public void Bt_BuyGuidePass()
    {
        InAppPurchasing.Purchase("rpgg2.growthguide.premium");
    }

    public UIToggle[] toggles_quest;
    public UIButton[] button_quest;
    public UIView[] panel_quest;
    public GameObject[] obj_quest;
    
    public void Bt_OpenTutoQuestPanel()
    {
        Tutorialmanager.Instance.hidealluiview();
        switch (PlayerBackendData.Instance.tutoguideid)
        {
            case 0 :
                break;
            case 1 : //초보자 점핑 지원 물품 상자 사용
            case 2 : //물약 퀵슬릇에 모두 등록
                Inventory.Instance.RefreshInventory(1);
                toggles_quest[0].ExecuteClick();
                break;
            case 3 : //[모험가 패스] 튜토리얼 진행
            case 6 : //초보자 점핑 지원 물품 상자 사용
            case 7 : //초보자 점핑 지원 물품 상자 사용
                guidepanel.Show(false);
                RefreshScrolbar();
                break;
            case 4 : //[서버저장]서버에 데이터 저장하기
                button_quest[0].ExecuteClick();
                break;
            case 5 : //[어빌리티]어빌리티 선택하기
                panel_quest[0].Show(false);
                break;
            case 8 : //[어빌리티]어빌리티 선택하기
            case 10 : //[개조]장비 등급 변경 시도하기
            case 11 : //[개조]장비 품질 변경 시도하기
            case 12 : //[개조]장비 특수효과 변경 시도하기
            case 16 : //[장비 승급]장비 무기 승급 시도
            case 45 : //[장비 승급]장비 무기 승급 시도
            case 46 : //[장비 승급]장비 무기 승급 시도
            case 47 : //[장비 승급]장비 무기 승급 시도
            case 31 : //[장비 승급]장비 무기 승급 시도
            case 39 : //[장비 승급]장비 무기 승급 시도
                panel_quest[1].Show(false);
                toggles_quest[1].ExecuteClick();
                break;
            case 22: //[성장 던전]스킬북 던전 1회 진행하기
            case 29: //[성장 던전]스킬북 던전 1회 진행하기
            case 43: //[성장 던전]스킬북 던전 1회 진행하기
                panel_quest[3].Show(false);
                WorldBossManager.Instance.CheckWorldBossLock();
                break;
            
            case 23 : //[성장 던전]스킬북 제작 1회 시도하기
                panel_quest[2].Show(false);
                CraftManager.Instance.RefreshNowCraftingCount();
                break;
            case 34 : //[승급] 모험 랭크 16 달성하기
            case 41 : //[승급] 모험 랭크 20 달성하기
               AdventureLvManager.Instance.Bt_OpenAdPanel();
               break;
            case 26 : //[펫] 펫 소환 시도하기
            case 27 : //[승급] 모험 랭크 20 달성하기
                panel_quest[4].Show(false);
                petmanager.Instance.Bt_SetState(0);
                break;
            default:
                guidepanel.Show(false);
                RefreshScrolbar();
                break;
        }
    }
}
