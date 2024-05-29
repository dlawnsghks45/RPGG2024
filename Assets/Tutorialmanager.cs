using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class Tutorialmanager : MonoBehaviour
{
    //싱글톤만들기.
    private static Tutorialmanager _instance = null;

    public static Tutorialmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Tutorialmanager)) as Tutorialmanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    public GameObject SkipTutorialbutton;
    public GameObject SkipTutorialbutton_GuideQuest;

    public string maxlv = "11";


    public GameObject Tutopanel;
    public Text infotext;
    public itemiconslot item;
    private bool isfinish;
    private int maxcount;
    public GameObject finishbtobj;


    public GameObject startturoobj;
    
    
    void RefreshMax()
    {
        //Debug.Log(maxlv + "맥렙");
        if (int.Parse(PlayerBackendData.Instance.tutoid) >= int.Parse(maxlv))
        {
            Tutopanel.SetActive(false);
            return;
        }

        maxcount = int.Parse(TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).count);
        //Invoke("timestart", 2f);
    }

    public void timestart()
    {
        ShowArrowObj(PlayerBackendData.Instance.tutoid);
    }

    private void Start()
    {
        nowstep = 0;
        tutocoroutine.Add(Tutorial_0());
        tutocoroutine.Add(Tutorial_1());
        tutocoroutine.Add(Tutorial_2());
        tutocoroutine.Add(Tutorial_3());
        tutocoroutine.Add(Tutorial_4());
        tutocoroutine.Add(Tutorial_5());
        tutocoroutine.Add(Tutorial_6());
        tutocoroutine.Add(Tutorial_7());
        tutocoroutine.Add(Tutorial_8());
        tutocoroutine.Add(Tutorial_9());
        tutocoroutine.Add(Tutorial_10());
        tutocoroutine.Add(Tutorial_11());
        tutocoroutine.Add(Tutorial_12());
        tutocoroutine.Add(Tutorial_Adpass());
        tutocoroutine.Add(Tutorial_Adpass());
        tutocoroutine.Add(Tutorial_Guild());
        tutocoroutine.Add(Tutorial_Smelt());
        tutocoroutine.Add(Tutorial_AntCave());
        tutocoroutine.Add(Tutorial_18());
        tutocoroutine.Add(Tutorial_content());
        tutocoroutine.Add(Tutorial_collect());
        tutocoroutine.Add(Tutorial_WorldBoss());
        tutocoroutine.Add(Tutorial_Pet());


        if (int.Parse(PlayerBackendData.Instance.tutoid) >= int.Parse(maxlv))
        {
            Tutopanel.SetActive(false);
    
            return;
        }

        TutorialTotalManager.Instance.Tutopanel.SetActive(false);

        RefreshMax();
        Refresh();

        if (maxcount <= PlayerBackendData.Instance.tutocount)
        {
            //완료;
            finishbtobj.SetActive(true);
            isfinish = true;
        }
        else
        {
            finishbtobj.SetActive(false);
        }
        TutorialTotalManager.Instance.RefreshInfo();

        if (PlayerBackendData.Instance.tutoid.Equals("0") && !isnotstarttuto)
        {
            SelectTutorialLevel.SetActive(true);
        }

    }


    public Animator ani;
    private bool show;
    private static readonly int On = Animator.StringToHash("on");
    private static readonly int Off = Animator.StringToHash("off");

    public void bt_showapenl()
    {
        if (show)
        {
            show = false;
            ani.SetTrigger(Off);
        }
        else
        {
            show = true;
            ani.SetTrigger(On);
        }
    }

    public void Refresh()
    {
        if (int.Parse(PlayerBackendData.Instance.tutoid) >= int.Parse(maxlv))
        {
            Tutopanel.SetActive(false);
            return;
        }
        
        startturoobj.SetActive(false);
        finishbtobj.SetActive(false);

        if (PlayerBackendData.Instance.tutocount >= maxcount)
            isnotstarttuto = true;
        
        //튜토를 시작하지 안흥ㅁ
        if (!isnotstarttuto)
        {
            startturoobj.SetActive(true);
            infotext.text =
                string.Format("<color=cyan>[{0}]</color>\n{1}",
                    Inventory.GetTranslate(TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).info+"a"),
                    Inventory.GetTranslate("UI8/튜토리얼시작하기"));
        }
        else
        {
            if (isfinish)
            {
                infotext.text =
                    string.Format("<color=cyan>[{0}]\n{1}/{2} {3}</color>",
                        Inventory.GetTranslate(TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).info),
                        PlayerBackendData.Instance.tutocount, maxcount,Inventory.GetTranslate("UI8/완료하기"));
            }
            else
            {
                infotext.text =
                    string.Format("<color=white>[{0}]</color>\n{1}/{2}",
                        Inventory.GetTranslate(TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).info),
                        PlayerBackendData.Instance.tutocount, maxcount);
            }
          
        }
        
        if (isfinish)
        {
            infotext.color = Color.cyan;
            finishbtobj.SetActive(true);
        }
        else
        {
            infotext.color = Color.white;
        }

        string reward = TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).itemid;
        int howmany = int.Parse(TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).itemhowmany);
        item.Refresh(reward, howmany, false);
    }

    public void FinishTutoPotion()
    {
        CheckTutorial("equippotion");
    }
    public void FinishTutoMapChange()
    {
        CheckTutorial("changemaplv");

    }

    private bool isnotstarttuto;
    public void CheckTutorial(string trigger)
    {
        if (int.Parse(PlayerBackendData.Instance.tutoid) >= int.Parse(maxlv))
        {
            Tutopanel.SetActive(false);
            return;
        }

        if (isfinish) return;
        if(!isnotstarttuto) return;
            //   Debug.Log(PlayerBackendData.Instance.tutoid);
      //  Debug.Log(TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).type);
        if (TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).type == (trigger))
        {
            for (int i = 0; i < NewTuto1.Length; i++)
                NewTuto1[i].SetActive(false);
            
            hidealluiview();
            
            PlayerBackendData.Instance.tutocount++;

            if (maxcount <= PlayerBackendData.Instance.tutocount)
            {
                switch (TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).type)
                {
                    case "adlvup":
                        EndTutorial();
                        break;
                }
                //완료;
                finishbtobj.SetActive(true);
                isfinish = true;
            
            }
            else
            {
                finishbtobj.SetActive(false);
            }
            Refresh();
            Savemanager.Instance.SaveGuideQuest();
        }
    }

    public GameObject SelectTutorialLevel;

    public void Bt_SelectNewbie()
    {
        SelectTutorialLevel.SetActive(false);
        SkipTutorialbutton.SetActive(false);
        SkipTutorialbutton_GuideQuest.SetActive(false);
        hidealluiview();
        Inventory.Instance.AddItem("996",1);
        LogManager.Log_SelectTutoType("초보자");
    }

    public void Bt_SelectVeteran()
    {
        SelectTutorialLevel.SetActive(false);
        int tutoid = 11;
        Tutopanel.SetActive(false);
        PlayerBackendData.Instance.tutoid = tutoid.ToString();
        PlayerBackendData.Instance.tutocount = 0;

        List<string> id = new List<string>();
        List<int> hw = new List<int>();

        for (int i = 0; i < TutorialDB.Instance.NumRows(); i++)
        {
            id.Add(TutorialDB.Instance.GetAt(i).itemid);
            hw.Add(int.Parse(TutorialDB.Instance.GetAt(i).itemhowmany));
            Inventory.Instance.AddItem(TutorialDB.Instance.GetAt(i).itemid,int.Parse(TutorialDB.Instance.GetAt(i).itemhowmany));
        }
        alertmanager.Instance.ShowAlert( Inventory.GetTranslate("UI7/베테랑선택함"),alertmanager.alertenum.일반);
        Inventory.Instance.ShowEarnItem3(id.ToArray(), hw.ToArray(), false);
        Inventory.Instance.AddItem("997",1);
        hidealluiview();
        TutorialTotalManager.Instance.RefreshInfo();
        TutorialTotalManager.Instance.RefreshNow();
        GrowEventmanager.Instance.Bt_ShowPanel();
        Savemanager.Instance.SaveInventory();
        Savemanager.Instance.SaveCash();
        Savemanager.Instance.SaveGuideQuest();
        Savemanager.Instance.Save();
        LogManager.Log_SelectTutoType("베테랑");
    }

    public void bt_starttutiobutton()
    {
        isnotstarttuto = true;
        ShowArrowObj(PlayerBackendData.Instance.tutoid);
        Refresh();
    }

    public GameObject[] NewTuto1;

    void SetNewTuto(int num)
    {
        switch (num)
        {
            case 0:
                NewTuto1[0].SetActive(true);
                NewTuto1[1].SetActive(true);
                NewTuto1[5].SetActive(true);
                break;
            case 1:
                NewTuto1[5].SetActive(true);
                NewTuto1[7].SetActive(true);
                NewTuto1[11].SetActive(true);
                NewTuto1[16].SetActive(true);

                break;
            case 3:
                NewTuto1[5].SetActive(true);
                NewTuto1[8].SetActive(true);
                NewTuto1[15].SetActive(true);

                
                switch (PlayerBackendData.Instance.ClassId)
                {
                    case "C1000":
                        NewTuto1[12].SetActive(true);
                        break;
                    case "C1001":
                        NewTuto1[13].SetActive(true);
                        break;
                    case "C1002":
                        NewTuto1[14].SetActive(true);
                        break;
                }
                break;
            case 6:
                Debug.Log("여기다");
                NewTuto1[0].SetActive(true);
                NewTuto1[5].SetActive(true);
                NewTuto1[19].SetActive(true);
                break;
        }
    }
    
    public void ShowArrowObj(string id)
    {
        for (int i = 0; i < NewTuto1.Length; i++)
        {
            NewTuto1[i].SetActive(false);
        }
        SkipTutorialbutton.SetActive(false);
        SkipTutorialbutton_GuideQuest.SetActive(false);
        Debug.Log("튜토리얼 시작" + id);
        hidealluiview();
        switch (id)
        {
            case "0":
                Debug.Log("튜토레어" + isnotstarttuto);
                SetNewTuto(0);
                
                break;
            case "1":
             StartTutorial(3);
              // StartTutorial(6); //2
             
                break;
            case "2":
                Classmanager.Instance.Bt_BuyClass("C1000");
                Classmanager.Instance.Bt_BuyClass("C1001");
                Classmanager.Instance.Bt_BuyClass("C1002");
                SetNewTuto(1);
                break;
            case "3": //스킬
                SetNewTuto(3);
               // StartTutorial(7); //3
                break;
            case "4": //몬스터 처치
               
                break;
            case "5":
                StartTutorial(4);
                break;
            case "6":
                SetNewTuto(6);
                break;
            case "7":
                StartTutorial(10);
                break;
            case "8":
                StartTutorial(5);
                break;
            case "9":
                StartTutorial(11);
                break;
            case "10":
                StartTutorial(18);
                break;
            case "11": //포셔ㅑㄴ퀵슬릇
                StartTutorial(10);
                break;
            case "12":
                StartTutorial(12);
                break;
            case "13":
                StartTutorial(11);
                break;
            case "20":
                StartTutorial(14);
                break;
            case "21":
                StartTutorial(15);
                break;
            case "22":
                StartTutorial(16);
                break;
            case "23":
                StartTutorial(17);
                break;
            case "14":
            case "24":
                StartTutorial(18);
                break;
            
        }
/*

switch (id)
{
case "0":
//스킬 장착
Tutoobj[3].SetActive(true);
Tutoobj[4].SetActive(true);
Tutoobj[5].SetActive(true);
Tutoobj2[4].SetActive(true);

break;
case "1":
//지도 보기 0~2
Tutoobj[0].SetActive(true);
Tutoobj[1].SetActive(true);
Tutoobj[2].SetActive(true);
Tutoobj2[5].SetActive(true);
break;
case "3":
//직업보기
Tutoobj[3].SetActive(true);
Tutoobj[6].SetActive(true);
break;
case "4":
//패시브 설정ㅇ
Tutoobj[3].SetActive(true);
Tutoobj[6].SetActive(true);
Tutoobj[7].SetActive(true);
//패시브가 이미 설정ㅇ돼있다면 올린다.
if (PlayerBackendData.Instance.PassiveClassId.Any(t => t != ""))
{
 CheckTutorial("setpassive");
 return;
}
break;
case "5":
//모험 랭크 2이상 달성
Tutoobj[3].SetActive(true);
Tutoobj[8].SetActive(true);
Tutoobj[9].SetActive(true);
if (PlayerBackendData.Instance.GetAdLv() >= 2)
{
 CheckTutorial("adlvup");
}

break;

case "6":
//직업 구매
Tutoobj[3].SetActive(true);
Tutoobj[6].SetActive(true);
Tutoobj[10].SetActive(true);
int count = PlayerBackendData.Instance.ClassData.Values.Count(VARIABLE => VARIABLE.Isown);

if (count > 1)
{
 CheckTutorial("buyclass");
}
break;

case "7":
//스킬 장착
Tutoobj[3].SetActive(true);
Tutoobj[5].SetActive(true);
Tutoobj[38].SetActive(true);
Tutoobj2[7].SetActive(true);
break;


case "8":
//제작하기
Tutoobj[3].SetActive(true);
Tutoobj[11].SetActive(true);
Tutoobj[12].SetActive(true);
Tutoobj[13].SetActive(true);
Tutoobj[14].SetActive(true);
Tutoobj[15].SetActive(true);
break;

case "9":
//제작수령
Tutoobj[3].SetActive(true);
Tutoobj[11].SetActive(true);
Tutoobj[17].SetActive(true);
Tutoobj[18].SetActive(true);

foreach (var VARIABLE in PlayerBackendData.Instance.GetTypeEquipment("2").Where(VARIABLE => VARIABLE.Value.Itemid.Equals("E1004")))
{
 CheckTutorial("getcraft");
 break;
}

break;


case "10":
//장착
Tutoobj[3].SetActive(true);
Tutoobj[19].SetActive(true);
Tutoobj[20].SetActive(true);
Tutoobj[21].SetActive(true);
Tutoobj[22].SetActive(true);

if (PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E1004"))
{
 CheckTutorial("equipitem");
}

//Tutoobj[23].SetActive(true);
break;

case "11":
//포션제작
Tutoobj[3].SetActive(true); //메뉴
Tutoobj[11].SetActive(true); //제작
Tutoobj[24].SetActive(true);
Tutoobj[25].SetActive(true);
Tutoobj[44].SetActive(true);
Tutoobj[18].SetActive(true);
Tutoobj[14].SetActive(true);
break;
case "12":
//포션장착
Tutoobj[27].SetActive(true);
Tutoobj[37].SetActive(true);
Tutoobj[46].SetActive(true);
break;
case "13":
//교환소 물약 창열기
Tutoobj[3].SetActive(true); //메뉴
Tutoobj[28].SetActive(true);
Tutoobj[29].SetActive(true);
Tutoobj[30].SetActive(true);
Tutoobj[39].SetActive(true);
break;

case "14":
//승급3
Tutoobj[3].SetActive(true);
Tutoobj[8].SetActive(true);
Tutoobj[9].SetActive(true);
if (PlayerBackendData.Instance.GetAdLv() >= 3)
{
 CheckTutorial("adlvup3");
}
break;
case "15":
//직업 구매
Tutoobj[3].SetActive(true);
Tutoobj[6].SetActive(true);
Tutoobj[10].SetActive(true);
Tutoobj2[15].SetActive(true);
int count3 = PlayerBackendData.Instance.ClassData.Values.Count(VARIABLE => VARIABLE.Isown);

if (count3 > 2)
{
 CheckTutorial("buyclass");
}
break;
case "16":
//스킬 장착
Tutoobj[3].SetActive(true);
Tutoobj[5].SetActive(true);
Tutoobj[12].SetActive(true);
Tutoobj2[13].SetActive(true);
break;
case "17":
//던전
Tutoobj[3].SetActive(true);
Tutoobj[31].SetActive(true);
Tutoobj[32].SetActive(true);
Tutoobj[33].SetActive(true);
Tutoobj[34].SetActive(true);
break;
case "18":
//수집하기
Tutoobj[3].SetActive(true);
Tutoobj[35].SetActive(true);
Tutoobj[36].SetActive(true);

int count2 = PlayerBackendData.Instance.CollectData.Values.Count(VARIABLE => VARIABLE.Isfinishall);

if (count2 > 1)
{
 CheckTutorial("collect");
}
break;
case "19":
//레이즈 무기 제작 후 수령하기
Tutoobj[3].SetActive(true);
Tutoobj[11].SetActive(true);
Tutoobj2[1].SetActive(true); //제작 던전버튼
Tutoobj2[2].SetActive(true);
Tutoobj[14].SetActive(true);

foreach (var VARIABLE in PlayerBackendData.Instance.Equiptment0)
{
 if (VARIABLE.Value.Itemid.Equals("E5500") ||
     VARIABLE.Value.Itemid.Equals("E5501") ||
     VARIABLE.Value.Itemid.Equals("E5502") ||
     VARIABLE.Value.Itemid.Equals("E5503") ||
     VARIABLE.Value.Itemid.Equals("E5504") ||
     VARIABLE.Value.Itemid.Equals("E5505") ||
     VARIABLE.Value.Itemid.Equals("E5506") ||
     VARIABLE.Value.Itemid.Equals("E5507"))
 {
     Tutorialmanager.Instance.CheckTutorial("makerageweapon");
     break;
 }
}

break;

case "20":
//제작수령
Tutoobj[3].SetActive(true);
Tutoobj[11].SetActive(true);
Tutoobj[17].SetActive(true);


foreach (var VARIABLE in PlayerBackendData.Instance.Equiptment0)
{
 if (VARIABLE.Value.Itemid.Equals("E5500") ||
     VARIABLE.Value.Itemid.Equals("E5501") ||
     VARIABLE.Value.Itemid.Equals("E5502") ||
     VARIABLE.Value.Itemid.Equals("E5503") ||
     VARIABLE.Value.Itemid.Equals("E5504") ||
     VARIABLE.Value.Itemid.Equals("E5505") ||
     VARIABLE.Value.Itemid.Equals("E5506") ||
     VARIABLE.Value.Itemid.Equals("E5507"))
 {
     Tutorialmanager.Instance.CheckTutorial("getcraftraze");
     break;
 }
}
break;

case "21":
//장착
//장착
Tutoobj[3].SetActive(true);
Tutoobj[19].SetActive(true);
Tutoobj[20].SetActive(true);
Tutoobj[22].SetActive(true);
Tutoobj2[8].SetActive(true);


if (PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5500") ||
 PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5501") ||
 PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5502") ||
 PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5503") ||
 PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5504") ||
 PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5505") ||
 PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5506") ||
 PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5507"))
{
 CheckTutorial("equipitemraze");
}
break;


case "22":
//승급4
Tutoobj[3].SetActive(true);
Tutoobj[8].SetActive(true);
Tutoobj[9].SetActive(true);
if (PlayerBackendData.Instance.GetAdLv() >= 4)
{
 CheckTutorial("adlvup4");
}
break;


case "23":
//성장가이드
Tutoobj[3].SetActive(true);
Tutoobj2[0].SetActive(true);
break;
}

}

public void Bt_ClassShow()
{
Tutoobj[41].SetActive(false);
Tutoobj[42].SetActive(false);
Tutoobj[43].SetActive(false);

switch (PlayerBackendData.Instance.tutoid)
{
//직업 정보보기
case "3":
switch (PlayerBackendData.Instance.ClassId)
{
  case "C1000" :
      Tutoobj[41].SetActive(true);
      break;
  case "C1001":
      Tutoobj[42].SetActive(true);
      break;
  case "C1002":
      Tutoobj[43].SetActive(true);
      break;
}
break;
//직업 정보보기
case "4":
switch (PlayerBackendData.Instance.ClassId)
{
  case "C1000" :
      Tutoobj[41].SetActive(true);
      break;
  case "C1001":
      Tutoobj[42].SetActive(true);
      break;
  case "C1002":
      Tutoobj[43].SetActive(true);
      break;
}
break;
//직업구매
case "6":

switch (PlayerBackendData.Instance.ClassId)
{
  case "C1000" :
      Tutoobj[42].SetActive(true);
      Tutoobj[43].SetActive(true);
      break;
  case "C1001":
      Tutoobj[41].SetActive(true);
      Tutoobj[43].SetActive(true);
      break;
  case "C1002":
      Tutoobj[41].SetActive(true);
      Tutoobj[42].SetActive(true);
      break;
}

break;
//직업 정보보기
case "15":
Tutoobj2[9].SetActive(true);
Tutoobj2[10].SetActive(true);
Tutoobj2[11].SetActive(true);
break;
}
      */

    }

    public GameObject[] tutoPanel;
    public UIButton craftbutton;
    public UIButton classbutton;
    public void Bt_TutorialPanelOpen()
    {
        switch (PlayerBackendData.Instance.tutoid)
        {
            case "0":
                break;
            case "1":
                break;
            case "2":
                break;
            case "3":
                SkillInventory.Instance.ShowSkillInventory();
                break;
            case "4":
                break;
            case "5":
                break;
            case "6":
                break;
            case "7":
                AdventureLvManager.Instance.Bt_OpenAdPanel();
                break;
            case "8":
                AdventureLvManager.Instance.Bt_OpenAdPanel();
                break;
            case "9":
                DungeonManager.Instance.DungeonPanel.Show(false);
                DungeonManager.Instance.RefreshCount();
                break;
            case "10":
                CheckTutorial("tutoguide");
                TutorialTotalManager.Instance.guidepanel.Show(false);
                TutorialTotalManager.Instance.CheckFinish();
                TutorialTotalManager.Instance.RefreshScrolbar();
                break;
        }
    }


    public void bt_finish()
    {
        if (maxcount > PlayerBackendData.Instance.tutocount) return;
        if(!isnotstarttuto) return;
        List<string> id = new List<string>();
        List<string> hw = new List<string>();

//완료
        isfinish = false;
        finishbtobj.SetActive(false);

        string reward = TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).itemid;
        int howmany = int.Parse(TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).itemhowmany);
        id.Add(reward);
        hw.Add(TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).itemhowmany);
        Inventory.Instance.AddItem(reward, howmany, true);
        Inventory.Instance.ShowEarnItem(id.ToArray(), hw.ToArray(), false);

        LogManager.LogTutorial(PlayerBackendData.Instance.tutoid);
        int tutoid = int.Parse(PlayerBackendData.Instance.tutoid) + 1;

        isnotstarttuto = false;
        PlayerBackendData.Instance.tutoid = tutoid.ToString();
        PlayerBackendData.Instance.tutocount = 0;
        Savemanager.Instance.SaveInventory();
        Savemanager.Instance.SaveGuideQuest();
        Savemanager.Instance.Save();
        RefreshMax();
        Refresh();
        Tutorial_End.SetActive(false);

        if (PlayerBackendData.Instance.tutoid.Equals(maxlv))
        {
            Debug.Log("아아아아");
            TutorialTotalManager.Instance.RefreshInfo();
            TutorialTotalManager.Instance.RefreshNow();
            GrowEventmanager.Instance.Bt_ShowPanel();
        }
        
    }


    public void ShowReviewGoogle()
    {
        GoolgeManager.Instance.LaunchReview();
    }

    public GameObject review;



    public List<IEnumerator> tutocoroutine = new List<IEnumerator>();

    private Coroutine core;
    private int nowtuto;

    public void StartTutorial(int num)
    {
        nowstep = 0;

//열린 창 전부 닫음.
        for (int i = 0; i < UIView.VisibleViews.Count; i++)
        {
            UIView.VisibleViews[i].Hide(true);
        }

        for (int i = 0; i < Tutorial_objAll.Length; i++)
        {
            Tutorial_objAll[i].SetActive(false);
        }

        nowtuto = num;
        StartCoroutine(tutocoroutine[num]);
    }

    void EndTutorial()
    {
        StopCoroutine(tutocoroutine[nowtuto
        ]);
        for (int i = 0; i < UIView.VisibleViews.Count; i++)
        {
            UIView.VisibleViews[i].Hide(true);
        }
        uimanager.Instance.UIVIEWS.Clear();
        uimanager.Instance.UIVIEWSGameObject.Clear();

        for (int i = 0; i < Tutorial_objAll.Length; i++)
        {
            Tutorial_objAll[i].SetActive(false);
        }

        Tutorial_End.SetActive(true);
        SkipTutorialbutton_GuideQuest.SetActive(false);
        SkipTutorialbutton.SetActive(false);
    }

    public void Bt_ShowSkill()
    {
        SkillInventory.Instance.SelectSkillid = PlayerBackendData.Instance.Skills[0];
        SkillInventory.Instance.ShowChangePanel();
    }
    public void Bt_ShowSkill2()
    {
        SkillInventory.Instance.SelectSkillid = PlayerBackendData.Instance.Skills[1];
        SkillInventory.Instance.ShowChangePanel();
    }
    public GameObject Tutorial_End;
    public GameObject[] Tutorial_objAll;
    public GameObject[] Tutorial_obj0;
    public GameObject[] Tutorial_obj1;
    public GameObject[] Tutorial_obj2;
    public GameObject[] Tutorial_obj3;
    public GameObject[] Tutorial_obj4;
    public GameObject[] Tutorial_obj5;
    public GameObject[] Tutorial_obj6;
    public GameObject[] Tutorial_obj7;
    public GameObject[] Tutorial_obj8;
    public GameObject[] Tutorial_obj9;
    public GameObject[] Tutorial_obj10;
    public GameObject[] Tutorial_obj11;
    public GameObject[] Tutorial_obj12;
    
    
    //모험가 패스
    public GameObject[] Tutorial_obj14;
    public GameObject[] Tutorial_obj15;
    public GameObject[] Tutorial_obj16;
    public GameObject[] Tutorial_obj17;
    public GameObject[] Tutorial_obj18;
    public GameObject[] Tutorial_obj19;
    public GameObject[] Tutorial_obj20;
    public GameObject[] Tutorial_obj21;
    public GameObject[] Tutorial_obj22; //펫 튜토

//메뉴 튜토
    public IEnumerator Tutorial_0()
    {
        SkipTutorialbutton.SetActive(true);
        SkipTutorialbutton_GuideQuest.SetActive(false);
        Inventory.Instance.AddItem("1700",1);
        Inventory.Instance.AddItem("10",1);
        Debug.Log("튜토0");
//메뉴로 유도함
        Tutorial_objAll[0].SetActive(true);
        Tutorial_obj0[0].SetActive(true);
        yield return new WaitWhile(() => nowstep < 1);
//메뉴창에서 스킬누르기
        Tutorial_obj0[0].SetActive(false);
        Tutorial_obj0[1].SetActive(true);
        yield return new WaitWhile(() => nowstep < 2);
//스킬인벤에서 스킬 누르기
        Tutorial_obj0[1].SetActive(false);
        Tutorial_obj0[2].SetActive(true);
        yield return new WaitWhile(() => nowstep < 3);
//스킬 장착칸에서 스킬 누르기.
        Tutorial_obj0[2].SetActive(false);
        Tutorial_obj0[3].SetActive(true);
        yield return new WaitWhile(() => nowstep < 4);
//완료
        EndTutorial();
        for (int i = 0; i < Tutorial_obj0.Length; i++)
            Tutorial_obj0[i].SetActive(false);
    }

//장비 제작 튜토
    public void ShowRing()
    {
        CraftManager.Instance.Bt_ShowCraftResourceInfo("1021");
    }
    public void ShowpOTION()
    {
        CraftManager.Instance.Bt_ShowCraftResourceInfo("10");
    }
    public IEnumerator Tutorial_1()
    {
        SkipTutorialbutton.SetActive(true);
        SkipTutorialbutton_GuideQuest.SetActive(false);
        Debug.Log("튜토1");
//메뉴로 유도함
        Tutorial_objAll[1].SetActive(true);
        Tutorial_obj1[0].SetActive(true);
        yield return new WaitWhile(() => nowstep < 1);
//메뉴창에서 제작누르기
        Tutorial_obj1[0].SetActive(false);
        Tutorial_obj1[1].SetActive(true);
        yield return new WaitWhile(() => nowstep < 2);
//일반 누르,기
        Tutorial_obj1[1].SetActive(false);
        Tutorial_obj1[2].SetActive(true);
        yield return new WaitWhile(() => nowstep < 3);
//반지 누르기.
        Tutorial_obj1[2].SetActive(false);
        Tutorial_obj1[3].SetActive(true);
        yield return new WaitWhile(() => nowstep < 4);
        Tutorial_obj1[3].SetActive(false);
        Tutorial_obj1[4].SetActive(true);
        yield return new WaitWhile(() => nowstep < 5);
        Tutorial_obj1[4].SetActive(false);
        Tutorial_obj1[5].SetActive(true);
        yield return new WaitWhile(() => nowstep < 6);
        Tutorial_obj1[5].SetActive(false);
        Tutorial_obj1[6].SetActive(true);
        yield return new WaitWhile(() => nowstep < 7);
//제작슬릇 누르기
        EndTutorial();
        for (int i = 0; i < Tutorial_obj1.Length; i++)
            Tutorial_obj1[i].SetActive(false);
    }

    public IEnumerator Tutorial_2()
    {
        SkipTutorialbutton.SetActive(true);
        SkipTutorialbutton_GuideQuest.SetActive(false);
//메뉴로 유도함
        Tutorial_objAll[2].SetActive(true);
        Tutorial_obj2[0].SetActive(true);
        yield return new WaitWhile(() => nowstep < 1);
//메뉴창에서 제작누르기
        Tutorial_obj2[0].SetActive(false);
        Tutorial_obj2[1].SetActive(true);
        yield return new WaitWhile(() => nowstep < 2);
//일반 누르,기
        Tutorial_obj2[1].SetActive(false);
        Tutorial_obj2[2].SetActive(true);
        yield return new WaitWhile(() => nowstep < 3);
//반지 누르기.
        Tutorial_obj2[2].SetActive(false);
        Tutorial_obj2[3].SetActive(true);
        yield return new WaitWhile(() => nowstep < 4);
        Tutorial_obj2[3].SetActive(false);
        Tutorial_obj2[4].SetActive(true);
        yield return new WaitWhile(() => nowstep < 5);
//제작슬릇 누르기
        EndTutorial();
        for (int i = 0; i < Tutorial_obj2.Length; i++)
            Tutorial_obj2[i].SetActive(false);
    }
    //장비장착
    public IEnumerator Tutorial_3()
    {
        SkipTutorialbutton.SetActive(true);
        SkipTutorialbutton_GuideQuest.SetActive(false);
        GameObject[] obj = Tutorial_obj3;
        Tutorial_objAll[3].SetActive(true);
        obj[0].SetActive(true);
        yield return new WaitWhile(() => nowstep < 1);
        obj[0].SetActive(false);
        obj[1].SetActive(true);
        yield return new WaitWhile(() => nowstep < 2);
        obj[1].SetActive(false);
        obj[2].SetActive(true);
        yield return new WaitWhile(() => nowstep < 3);
        obj[2].SetActive(false);
        obj[3].SetActive(true);
        yield return new WaitWhile(() => nowstep < 4);
        obj[3].SetActive(false);
        obj[4].SetActive(true);
        yield return new WaitWhile(() => nowstep < 5);
        obj[4].SetActive(false);
        yield return new WaitForSeconds(1.2f);
        obj[5].SetActive(true);
        yield return new WaitWhile(() => nowstep < 6);
        EndTutorial();
        for (int i = 0; i < obj.Length; i++)
            obj[i].SetActive(false);
/*
        obj[5].SetActive(false);
        obj[6].SetActive(true);
        yield return new WaitWhile(() => nowstep < 7);
        obj[6].SetActive(false);
        obj[7].SetActive(true);
        yield return new WaitWhile(() => nowstep < 8);
        obj[7].SetActive(false);
        obj[8].SetActive(true);
        yield return new WaitWhile(() => nowstep < 9);
        obj[8].SetActive(false);
        obj[9].SetActive(true);
        yield return new WaitWhile(() => nowstep < 10);
        obj[9].SetActive(false);
        obj[10].SetActive(true);
        yield return new WaitWhile(() => nowstep < 11);
        obj[10].SetActive(false);
        obj[11].SetActive(true);
        yield return new WaitWhile(() => nowstep < 12);
        obj[11].SetActive(false);
        obj[12].SetActive(true);
        yield return new WaitWhile(() => nowstep < 13);
//제작슬릇 누르기
        EndTutorial();
        for (int i = 0; i < obj.Length; i++)
            obj[i].SetActive(false);*/
    }

    public IEnumerator Tutorial_4()
    {
        SkipTutorialbutton.SetActive(true);
        SkipTutorialbutton_GuideQuest.SetActive(false);
        GameObject[] obj = Tutorial_obj4;
        for (int i = 0; i < obj.Length; i++)
            obj[i].SetActive(false);
        Tutorial_objAll[4].SetActive(true);
        obj[0].SetActive(true);
        yield return new WaitWhile(() => nowstep < 1);
        obj[0].SetActive(false);
        obj[1].SetActive(true);
        yield return new WaitWhile(() => nowstep < 2);
        obj[1].SetActive(false);
        obj[2].SetActive(true);
        yield return new WaitWhile(() => nowstep < 3);
        obj[2].SetActive(false);
        obj[3].SetActive(true);
        yield return new WaitWhile(() => nowstep < 4);
        obj[3].SetActive(false);
        obj[4].SetActive(true);
        yield return new WaitWhile(() => nowstep < 5);
        obj[4].SetActive(false);
        obj[5].SetActive(true);
        yield return new WaitWhile(() => nowstep < 6);
        obj[5].SetActive(false);
        obj[6].SetActive(true);
        yield return new WaitWhile(() => nowstep < 7);
        hidealluiview();
        //obj[6].SetActive(false);
       // obj[7].SetActive(true);
      //  yield return new WaitWhile(() => nowstep < 8);
//제작슬릇 누르기
        EndTutorial();
        for (int i = 0; i < obj.Length; i++)
            obj[i].SetActive(false);
    }

    //모험랭크달성
    public IEnumerator Tutorial_5()
    {
        SkipTutorialbutton.SetActive(true);
        SkipTutorialbutton_GuideQuest.SetActive(false);
        GameObject[] obj = Tutorial_obj5;
        for (int i = 0; i < obj.Length; i++)
            obj[i].SetActive(false);
        Tutorial_objAll[5].SetActive(true);
        obj[0].SetActive(true);
        yield return new WaitWhile(() => nowstep < 1);
        obj[0].SetActive(false);
        obj[1].SetActive(true);
        yield return new WaitWhile(() => nowstep < 2);
        obj[1].SetActive(false);
        obj[2].SetActive(true);
        yield return new WaitWhile(() => nowstep < 3);
        obj[2].SetActive(false);
        obj[3].SetActive(true);
        yield return new WaitWhile(() => nowstep < 4);
        obj[3].SetActive(false);
        yield return new WaitForSeconds(3.5f);
        obj[4].SetActive(true);
        Time.timeScale = 0;
        yield return new WaitWhile(() => nowstep < 5);
        obj[4].SetActive(false);
        obj[5].SetActive(true);
        yield return new WaitWhile(() => nowstep < 6);
        obj[5].SetActive(false);
        obj[6].SetActive(true);
        yield return new WaitWhile(() => nowstep < 7);
        obj[6].SetActive(false);
        obj[7].SetActive(true);
        yield return new WaitWhile(() => nowstep < 8);
        obj[7].SetActive(false);
        obj[8].SetActive(true);
        yield return new WaitWhile(() => nowstep < 9);
        Time.timeScale = 1;
        //제작슬릇 누르기
        for (int i = 0; i < obj.Length; i++)
            obj[i].SetActive(false);
    }

    public GameObject[] NotUIviewpanel;
    void hidealluiview()
    {
        for (int i = 0; i <NotUIviewpanel.Length; i++)
        {
            NotUIviewpanel[i].SetActive(false);
        }
        for (int i = 0; i < UIView.VisibleViews.Count; i++)
        {
            UIView.VisibleViews[i].Hide(true);
        }
        uimanager.Instance.UIVIEWS.Clear();
        uimanager.Instance.UIVIEWSGameObject.Clear();
    }
//직업구매
    public IEnumerator Tutorial_6()
    {
        SkipTutorialbutton.SetActive(true);
        SkipTutorialbutton_GuideQuest.SetActive(false);
        Classmanager.Instance.Bt_BuyClass("C1000");
        Classmanager.Instance.Bt_BuyClass("C1001");
        Classmanager.Instance.Bt_BuyClass("C1002");
        Debug.Log("튜토6");
        yield return new WaitForSeconds(1f);
        hidealluiview();
        GameObject[] obj = Tutorial_obj6;
        for (int i = 0; i < obj.Length; i++)
            obj[i].SetActive(false);
        Tutorial_objAll[6].SetActive(true);
//메뉴
        obj[0].SetActive(true);
        yield return new WaitWhile(() => nowstep < 1);
//직업 버튼
        obj[0].SetActive(false);
        obj[1].SetActive(true);
        yield return new WaitWhile(() => nowstep < 2);
//여기서 계산
        obj[1].SetActive(false);
        obj[2].SetActive(true);

        /*
        switch (PlayerBackendData.Instance.ClassId)
        {
            case "C1000": //전사면 도적
                obj[3].SetActive(true);
                break;
            case "C1001": //도적이면 전사
                obj[2].SetActive(true);
                break;
            case "C1002": //마법사면 전사
                obj[2].SetActive(true);
                break;
        }
*/
        yield return new WaitWhile(() => nowstep < 3);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[4].SetActive(true);
        yield return new WaitWhile(() => nowstep < 4);
        obj[4].SetActive(false);
        obj[5].SetActive(true);
        yield return new WaitWhile(() => nowstep < 5);
        obj[5].SetActive(false);
        obj[6].SetActive(true);
        yield return new WaitWhile(() => nowstep < 6);
        obj[6].SetActive(false);
        obj[7].SetActive(true);
        yield return new WaitWhile(() => nowstep < 7);
        obj[7].SetActive(false);
        obj[8].SetActive(true);
        yield return new WaitWhile(() => nowstep < 8);
        obj[11].SetActive(false);
        obj[12].SetActive(false);
        switch (PlayerBackendData.Instance.ClassId)
        {
            case "C1000": //전사면 도적
                obj[2].SetActive(true);
                break;
            case "C1001": //도적이면 전사
                obj[3].SetActive(true);
                break;
            case "C1002": //마법사면 전사
                obj[9].SetActive(true);
                break;
        }
        obj[8].SetActive(false);
        yield return new WaitWhile(() => nowstep < 9);
        obj[2].SetActive(false);
        obj[9].SetActive(false);
        obj[3].SetActive(false);
        obj[10].SetActive(true);
        yield return new WaitWhile(() => nowstep < 10);
//제작슬릇 누르기
        for (int i = 0; i < obj.Length; i++)
            obj[i].SetActive(false);
        EndTutorial();
    }

    
    //스킬장착2
    public IEnumerator Tutorial_7()
    {
        SkipTutorialbutton.SetActive(true);
        SkipTutorialbutton_GuideQuest.SetActive(false);
       
        GameObject[] obj = Tutorial_obj7;
        for (int i = 0; i < obj.Length; i++)
            obj[i].SetActive(false);
        Tutorial_objAll[7].SetActive(true);
        obj[0].SetActive(true);
        yield return new WaitWhile(() => nowstep < 1);
        obj[0].SetActive(false);
        obj[1].SetActive(true);
        yield return new WaitWhile(() => nowstep < 2);
        obj[1].SetActive(false);
        yield return new WaitWhile(() => nowstep < 3);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[4].SetActive(true);
        yield return new WaitWhile(() => nowstep < 4);
        obj[4].SetActive(false);
        for (int i = 0; i < obj.Length; i++)
            obj[i].SetActive(false);
        
        EndTutorial();
    }

 public IEnumerator Tutorial_8()
    {
        SkipTutorialbutton.SetActive(true);
        SkipTutorialbutton_GuideQuest.SetActive(false);
        Debug.Log("물약 제작 튜토리얼");
//메뉴로 유도함
        Tutorial_objAll[8].SetActive(true);
        Tutorial_obj8[0].SetActive(true);
        yield return new WaitWhile(() => nowstep < 1);
//메뉴창에서 제작누르기
        Tutorial_obj8[0].SetActive(false);
        Tutorial_obj8[1].SetActive(true);
        yield return new WaitWhile(() => nowstep < 2);
//일반 누르,기
        Tutorial_obj8[1].SetActive(false);
        Tutorial_obj8[2].SetActive(true);
        yield return new WaitWhile(() => nowstep < 3);
//반지 누르기.
        Tutorial_obj8[2].SetActive(false);
        Tutorial_obj8[3].SetActive(true);
        yield return new WaitWhile(() => nowstep < 4);
        Tutorial_obj8[3].SetActive(false);
        Tutorial_obj8[4].SetActive(true);
        yield return new WaitWhile(() => nowstep < 5);
        Tutorial_obj8[4].SetActive(false);
        Tutorial_obj8[5].SetActive(true);
        yield return new WaitWhile(() => nowstep < 6);
        Tutorial_obj8[5].SetActive(false);
        Tutorial_obj8[6].SetActive(true);
        yield return new WaitWhile(() => nowstep < 7);
//제작슬릇 누르기
        EndTutorial();
        for (int i = 0; i < Tutorial_obj8.Length; i++)
            Tutorial_obj8[i].SetActive(false);
    }

 public IEnumerator Tutorial_9()
 {
     SkipTutorialbutton.SetActive(true);
     SkipTutorialbutton_GuideQuest.SetActive(false);
//메뉴로 유도함
     Tutorial_objAll[9].SetActive(true);
     Tutorial_obj9[0].SetActive(true);
     yield return new WaitWhile(() => nowstep < 1);
//메뉴창에서 제작누르기
     Tutorial_obj9[0].SetActive(false);
     Tutorial_obj9[1].SetActive(true);
     yield return new WaitWhile(() => nowstep < 2);
//일반 누르,기
     Tutorial_obj9[1].SetActive(false);
     Tutorial_obj9[2].SetActive(true);
     yield return new WaitWhile(() => nowstep < 3);
//반지 누르기.
     Tutorial_obj9[2].SetActive(false);
     Tutorial_obj9[3].SetActive(true);
     yield return new WaitWhile(() => nowstep < 4);
     Tutorial_obj9[3].SetActive(false);
     Tutorial_obj9[4].SetActive(true);
     yield return new WaitWhile(() => nowstep < 5);
//제작슬릇 누르기
     EndTutorial();
     for (int i = 0; i < Tutorial_obj9.Length; i++)
         Tutorial_obj9[i].SetActive(false);
 }
 
 //물약 퀵슬릇
 public IEnumerator Tutorial_10()
 {
     SkipTutorialbutton.SetActive(true);
     SkipTutorialbutton_GuideQuest.SetActive(false);
     Inventory.Instance.AddItem("1501",10);
     GameObject[] obj = Tutorial_obj10;
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     Tutorial_objAll[10].SetActive(true);
     obj[0].SetActive(true);
     yield return new WaitWhile(() => nowstep < 1);
     obj[0].SetActive(false);
     obj[1].SetActive(true);
     yield return new WaitWhile(() => nowstep < 2);
     obj[1].SetActive(false);
     obj[2].SetActive(true);
     yield return new WaitWhile(() => nowstep < 3);
     obj[2].SetActive(false);
     obj[3].SetActive(true);
     yield return new WaitWhile(() => nowstep < 4);
     obj[3].SetActive(false);
     obj[4].SetActive(true);
     yield return new WaitWhile(() => nowstep < 5);
     obj[4].SetActive(false);
     obj[5].SetActive(true);
     yield return new WaitWhile(() => nowstep < 6);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     EndTutorial();
 }

 public IEnumerator Tutorial_11()
 {
     SkipTutorialbutton.SetActive(true);
     SkipTutorialbutton_GuideQuest.SetActive(false);
     GameObject[] obj = Tutorial_obj11;
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     Tutorial_objAll[11].SetActive(true);
     obj[0].SetActive(true);
     yield return new WaitWhile(() => nowstep < 1);
     obj[0].SetActive(false);
     obj[1].SetActive(true);
     yield return new WaitWhile(() => nowstep < 2);
     obj[1].SetActive(false);
     obj[2].SetActive(true);
     yield return new WaitWhile(() => nowstep < 3);
     obj[2].SetActive(false);
     obj[3].SetActive(true);
     yield return new WaitWhile(() => nowstep < 4);
     obj[3].SetActive(false);
     obj[4].SetActive(true);
     yield return new WaitWhile(() => nowstep < 5);
     obj[4].SetActive(false);
     obj[5].SetActive(true);
     yield return new WaitWhile(() => nowstep < 6);
     obj[5].SetActive(false);
     obj[6].SetActive(true);
     yield return new WaitWhile(() => nowstep < 7);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     hidealluiview();
     EndTutorial();
 }

 public IEnumerator Tutorial_12()
 {
     SkipTutorialbutton.SetActive(true);
     SkipTutorialbutton_GuideQuest.SetActive(false);
     GameObject[] obj = Tutorial_obj12;
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     Tutorial_objAll[12].SetActive(true);
     obj[0].SetActive(true);
     yield return new WaitWhile(() => nowstep < 1);
     obj[0].SetActive(false);
     obj[1].SetActive(true);
     yield return new WaitWhile(() => nowstep < 2);
     obj[1].SetActive(false);
     obj[2].SetActive(true);
     yield return new WaitWhile(() => nowstep < 3);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
 }

 //성장 가이드 
 
 
 //tutorial 13 
 public IEnumerator Tutorial_Adpass()
 {
     SkipTutorialbutton.SetActive(false);
     SkipTutorialbutton_GuideQuest.SetActive(true);
     Debug.Log("모험가 패스 ");
     GameObject[] obj = Tutorial_obj14;
     Tutorial_objAll[14].SetActive(true);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     obj[0].SetActive(true);
     yield return new WaitWhile(() => nowstep < 1);
     obj[0].SetActive(false);
     obj[1].SetActive(true);
     yield return new WaitWhile(() => nowstep < 2);
     obj[1].SetActive(false);
     obj[2].SetActive(true);
     yield return new WaitWhile(() => nowstep < 3);
     obj[2].SetActive(false);
     obj[3].SetActive(true);
     yield return new WaitWhile(() => nowstep < 4);
     obj[3].SetActive(false);
     obj[4].SetActive(true);
     yield return new WaitWhile(() => nowstep < 5);
     obj[4].SetActive(false);
     obj[5].SetActive(true);
     yield return new WaitWhile(() => nowstep < 6);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     TutorialTotalManager.Instance.CheckGuideQuest("tutopass");
     
     for (int i = 0; i < UIView.VisibleViews.Count; i++)
     {
         UIView.VisibleViews[i].Hide(true);
     }
     uimanager.Instance.UIVIEWS.Clear();
     uimanager.Instance.UIVIEWSGameObject.Clear();

     for (int i = 0; i < Tutorial_objAll.Length; i++)
     {
         Tutorial_objAll[i].SetActive(false);
     }
     SkipTutorialbutton_GuideQuest.SetActive(false);
     TutorialTotalManager.Instance.guidepanel.Show(false);
 }
 
 public IEnumerator Tutorial_Guild()
 {
     SkipTutorialbutton.SetActive(false);
     SkipTutorialbutton_GuideQuest.SetActive(true);
     Debug.Log("길드");
     GameObject[] obj = Tutorial_obj15;
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     Tutorial_objAll[15].SetActive(true);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     obj[0].SetActive(true);
     yield return new WaitWhile(() => nowstep < 1);
     obj[0].SetActive(false);
     obj[1].SetActive(true);
     yield return new WaitWhile(() => nowstep < 2);
     obj[1].SetActive(false);
     obj[2].SetActive(true);
     yield return new WaitWhile(() => nowstep < 3);
     obj[2].SetActive(false);
     obj[3].SetActive(true);
     yield return new WaitWhile(() => nowstep < 4);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     TutorialTotalManager.Instance.CheckGuideQuest("tutoguild");
     
     for (int i = 0; i < UIView.VisibleViews.Count; i++)
     {
         UIView.VisibleViews[i].Hide(true);
     }
     uimanager.Instance.UIVIEWS.Clear();
     uimanager.Instance.UIVIEWSGameObject.Clear();

     for (int i = 0; i < Tutorial_objAll.Length; i++)
     {
         Tutorial_objAll[i].SetActive(false);
     }
     SkipTutorialbutton_GuideQuest.SetActive(false);
     TutorialTotalManager.Instance.guidepanel.Show(false);
 }
 
 public IEnumerator Tutorial_Smelt()
 {
     SkipTutorialbutton.SetActive(false);
     SkipTutorialbutton_GuideQuest.SetActive(true);
     Debug.Log("제련");
     GameObject[] obj = Tutorial_obj16;
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     Tutorial_objAll[16].SetActive(true);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     obj[0].SetActive(true);
     yield return new WaitWhile(() => nowstep < 1);
     obj[0].SetActive(false);
     obj[1].SetActive(true);
     yield return new WaitWhile(() => nowstep < 2);
     obj[1].SetActive(false);
     obj[2].SetActive(true);
     yield return new WaitWhile(() => nowstep < 3);
     obj[2].SetActive(false);
     obj[3].SetActive(true);
     yield return new WaitWhile(() => nowstep < 4);
     obj[3].SetActive(false);
     obj[4].SetActive(true);
     yield return new WaitWhile(() => nowstep < 5);
     obj[4].SetActive(false);
     obj[5].SetActive(true);
     yield return new WaitWhile(() => nowstep < 6);
     obj[5].SetActive(false);
     obj[6].SetActive(true);
     yield return new WaitWhile(() => nowstep < 7);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     TutorialTotalManager.Instance.CheckGuideQuest("tutosmelt");
     for (int i = 0; i < UIView.VisibleViews.Count; i++)
     {
         UIView.VisibleViews[i].Hide(true);
     }
     uimanager.Instance.UIVIEWS.Clear();
     uimanager.Instance.UIVIEWSGameObject.Clear();

     for (int i = 0; i < Tutorial_objAll.Length; i++)
     {
         Tutorial_objAll[i].SetActive(false);
     }
     SkipTutorialbutton_GuideQuest.SetActive(false);
     TutorialTotalManager.Instance.guidepanel.Show(false);
 }
 
 public IEnumerator Tutorial_AntCave()
 {
     SkipTutorialbutton.SetActive(false);
     SkipTutorialbutton_GuideQuest.SetActive(true);
     Debug.Log("개미굴");
     GameObject[] obj = Tutorial_obj17;
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     Tutorial_objAll[17].SetActive(true);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     obj[0].SetActive(true);
     yield return new WaitWhile(() => nowstep < 1);
     obj[0].SetActive(false);
     obj[1].SetActive(true);
     yield return new WaitWhile(() => nowstep < 2);
     obj[1].SetActive(false);
     obj[2].SetActive(true);
     yield return new WaitWhile(() => nowstep < 3);
     obj[2].SetActive(false);
     obj[3].SetActive(true);
     yield return new WaitWhile(() => nowstep < 4);
     obj[3].SetActive(false);
     obj[4].SetActive(true);
     yield return new WaitWhile(() => nowstep < 5);
     obj[4].SetActive(false);
     obj[5].SetActive(true);
     yield return new WaitWhile(() => nowstep < 6);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     TutorialTotalManager.Instance.CheckGuideQuest("tutoant");
     for (int i = 0; i < UIView.VisibleViews.Count; i++)
     {
         UIView.VisibleViews[i].Hide(true);
     }
     uimanager.Instance.UIVIEWS.Clear();
     uimanager.Instance.UIVIEWSGameObject.Clear();

     for (int i = 0; i < Tutorial_objAll.Length; i++)
     {
         Tutorial_objAll[i].SetActive(false);
     }
     TutorialTotalManager.Instance.guidepanel.Show(false);
     SkipTutorialbutton_GuideQuest.SetActive(false);

 }
 public IEnumerator Tutorial_18()
 {
     TutorialTotalManager.Instance.RefreshNow();
     TutorialTotalManager.Instance.CheckFinish();
     SkipTutorialbutton.SetActive(true);
     SkipTutorialbutton_GuideQuest.SetActive(false);
     Debug.Log("성장가이드");
     GameObject[] obj = Tutorial_obj18;
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     Tutorial_objAll[18].SetActive(true);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     obj[0].SetActive(true);
     yield return new WaitWhile(() => nowstep < 1);
     obj[0].SetActive(false);
     obj[1].SetActive(true);
     yield return new WaitWhile(() => nowstep < 2);
     obj[1].SetActive(false);
     obj[2].SetActive(true);
     yield return new WaitWhile(() => nowstep < 3);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     TutorialTotalManager.Instance.CheckGuideQuest("tutosmelt");
     for (int i = 0; i < UIView.VisibleViews.Count; i++)
     {
         UIView.VisibleViews[i].Hide(true);
     }
     uimanager.Instance.UIVIEWS.Clear();
     uimanager.Instance.UIVIEWSGameObject.Clear();

     for (int i = 0; i < Tutorial_objAll.Length; i++)
     {
         Tutorial_objAll[i].SetActive(false);
     }
     CheckTutorial("tutoguide");
     EndTutorial();
    
 }
 
 public IEnumerator Tutorial_content()
 {
     SkipTutorialbutton.SetActive(false);
     SkipTutorialbutton_GuideQuest.SetActive(true);
     Debug.Log("성장가이드");
     GameObject[] obj = Tutorial_obj19;
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     Tutorial_objAll[19].SetActive(true);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     obj[0].SetActive(true);
     yield return new WaitWhile(() => nowstep < 1);
     obj[0].SetActive(false);
     obj[1].SetActive(true);
     yield return new WaitWhile(() => nowstep < 2);
     obj[1].SetActive(false);
     obj[2].SetActive(true);
     yield return new WaitWhile(() => nowstep < 3);
     obj[2].SetActive(false);
     obj[3].SetActive(true);
     yield return new WaitWhile(() => nowstep < 4);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     TutorialTotalManager.Instance.CheckGuideQuest("contenttuto");
     for (int i = 0; i < UIView.VisibleViews.Count; i++)
     {
         UIView.VisibleViews[i].Hide(true);
     }
     uimanager.Instance.UIVIEWS.Clear();
     uimanager.Instance.UIVIEWSGameObject.Clear();

     for (int i = 0; i < Tutorial_objAll.Length; i++)
     {
         Tutorial_objAll[i].SetActive(false);
     }
     SkipTutorialbutton_GuideQuest.SetActive(false);

     TutorialTotalManager.Instance.guidepanel.Show(false);
     
     SkipTutorialbutton_GuideQuest.SetActive(false);

 }
 
 public IEnumerator Tutorial_collect()
 {
     SkipTutorialbutton.SetActive(false);
     SkipTutorialbutton_GuideQuest.SetActive(true);
     Debug.Log("수집");
     GameObject[] obj = Tutorial_obj20;
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     Tutorial_objAll[20].SetActive(true);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     obj[0].SetActive(true);
     yield return new WaitWhile(() => nowstep < 1);
     obj[0].SetActive(false);
     obj[1].SetActive(true);
     yield return new WaitWhile(() => nowstep < 2);
     obj[1].SetActive(false);
     obj[2].SetActive(true);
     yield return new WaitWhile(() => nowstep < 3);
     obj[2].SetActive(false);
     obj[3].SetActive(true);
     yield return new WaitWhile(() => nowstep < 4);
     obj[3].SetActive(false);
     obj[4].SetActive(true);
     yield return new WaitWhile(() => nowstep < 5);
     obj[4].SetActive(false);
     obj[5].SetActive(true);
     yield return new WaitWhile(() => nowstep < 6);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     TutorialTotalManager.Instance.CheckGuideQuest("contentcollect");
     for (int i = 0; i < UIView.VisibleViews.Count; i++)
     {
         UIView.VisibleViews[i].Hide(true);
     }
     uimanager.Instance.UIVIEWS.Clear();
     uimanager.Instance.UIVIEWSGameObject.Clear();

     for (int i = 0; i < Tutorial_objAll.Length; i++)
     {
         Tutorial_objAll[i].SetActive(false);
     }
     SkipTutorialbutton_GuideQuest.SetActive(false);
     TutorialTotalManager.Instance.guidepanel.Show(false);
 }
 
 public IEnumerator Tutorial_WorldBoss()
 {
     SkipTutorialbutton.SetActive(false);
     SkipTutorialbutton_GuideQuest.SetActive(true);
     Debug.Log("월드보스");
     GameObject[] obj = Tutorial_obj21;
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     Tutorial_objAll[21].SetActive(true);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     obj[0].SetActive(true);
     yield return new WaitWhile(() => nowstep < 1);
     obj[0].SetActive(false);
     obj[1].SetActive(true);
     yield return new WaitWhile(() => nowstep < 2);
     obj[1].SetActive(false);
     obj[2].SetActive(true);
     yield return new WaitWhile(() => nowstep < 3);
     obj[2].SetActive(false);
     obj[3].SetActive(true);
     yield return new WaitWhile(() => nowstep < 4);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     TutorialTotalManager.Instance.CheckGuideQuest("worldboss");
     for (int i = 0; i < UIView.VisibleViews.Count; i++)
     {
         UIView.VisibleViews[i].Hide(true);
     }
     uimanager.Instance.UIVIEWS.Clear();
     uimanager.Instance.UIVIEWSGameObject.Clear();

     for (int i = 0; i < Tutorial_objAll.Length; i++)
     {
         Tutorial_objAll[i].SetActive(false);
     }
     SkipTutorialbutton_GuideQuest.SetActive(false);
     TutorialTotalManager.Instance.guidepanel.Show(false);
 }
 
 public IEnumerator Tutorial_Pet()
 {
     SkipTutorialbutton.SetActive(false);
     SkipTutorialbutton_GuideQuest.SetActive(true);
     Debug.Log("펫");
     GameObject[] obj = Tutorial_obj22;
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     Tutorial_objAll[22].SetActive(true);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     obj[0].SetActive(true);
     yield return new WaitWhile(() => nowstep < 1);
     obj[0].SetActive(false);
     obj[1].SetActive(true);
     yield return new WaitWhile(() => nowstep < 2);
     obj[1].SetActive(false);
     obj[2].SetActive(true);
     yield return new WaitWhile(() => nowstep < 3);
     obj[2].SetActive(false);
     obj[3].SetActive(true);
     yield return new WaitWhile(() => nowstep < 4);
     for (int i = 0; i < obj.Length; i++)
         obj[i].SetActive(false);
     TutorialTotalManager.Instance.CheckGuideQuest("pettuto");
     for (int i = 0; i < UIView.VisibleViews.Count; i++)
     {
         UIView.VisibleViews[i].Hide(true);
     }
     uimanager.Instance.UIVIEWS.Clear();
     uimanager.Instance.UIVIEWSGameObject.Clear();

     for (int i = 0; i < Tutorial_objAll.Length; i++)
     {
         Tutorial_objAll[i].SetActive(false);
     }
     SkipTutorialbutton_GuideQuest.SetActive(false);
     TutorialTotalManager.Instance.guidepanel.Show(false);
 }

 
 
 public void ShowInvenPotion()
 {
         Inventory.Instance.ShowInventoryItem("1501");
 }
 
    public void ShowStage()
    {
        mapmanager.Instance.ShowDropItemField("1000");
    }


    public void Bt_FinishNow_Tuto()
    {
        //건너 뛰기
        PlayerBackendData.Instance.tutocount = maxcount;
        switch (TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).type)
        {
            case "adlvup":
                EndTutorial();
                break;
        }

        //완료;
        finishbtobj.SetActive(true);
        isfinish = true;
        nowstep = 100;
    }

    public void Bt_FinishNow_Tuto2()
    {
        //완료;
        nowstep = 100;
        TutorialTotalManager.Instance.RefreshNow();
        Savemanager.Instance.SaveGuideQuest();
        alertmanager.Instance.NotiCheck_GuideQuest();
    }

    public int nowstep;

    public void bt_nextstep()
    {
        nowstep++;
    }
}
