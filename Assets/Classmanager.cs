using System;
using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Classmanager : MonoBehaviour
{
    //싱글톤만들기.
    private static Classmanager _instance = null;
    public static Classmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Classmanager)) as Classmanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    public jobslot[] JobSlots;
    [SerializeField]
    Transform ClassPanel;


    public UIView ClassMotherPanel;
    public void Bt_RefreshClass()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ClassPanel.transform);
    }
    public void RefreshJobSlot()
    {
      //  Debug.Log("잡슬릇" + JobSlots.Length);
        for (var index = 0; index < JobSlots.Length; index++)
        {
            var t = JobSlots[index];
//            Debug.Log(index);
            t.Refresh();
        }
    }


    public GameObject[] ClassHaveObj; //0은 없다면 1은 있는데 사용중아님 2는 사용중
    public Button PassiveButton; //0은 없다면 1은 있는데 사용중아님 2는 사용중
    public Text PassiveText; 
    public Text NoClassTextNeedItem; 
    public Image NoClassTextItemImage; 
    public UIView Classpanel;
    int nowpage = 0;
    int maxpage = 0;
    public Image[] PlayerImage;
    public Image ClassSkillImage;

    public Button Prevbutton;
    public Button Nextbutton;

    public Text PrevClassname;
    public Text NextClassname;

    public Text ClassName;
    public Text ClassInfo;
    public Text[] ClassLvStat;
    public Text ClassTier;
    public Text ClassSkill;
    public GameObject[] ClassAttackType;

    public Text PassiveName;
    public Text PassiveInfo;


    public void OpenClassSelection()
    {
        for(int i  =  0; i < ClassDB.Instance.NumRows(); i++)
        {
            if (ClassDB.Instance.GetAt(i).id == PlayerBackendData.Instance.ClassId)
            {
                nowpage = i;
            }
        }

        maxpage = ClassDB.Instance.NumRows()-1;

        if (nowpage == 0)
            Prevbutton.interactable = false;
        else
            Prevbutton.interactable = true;

        if (nowpage == maxpage)
            Nextbutton.interactable = false;
        else
            Nextbutton.interactable = true;
        ShowClassData();
    }

    public void OpenClassSelection(string id)
    {
        for (int i = 0; i < ClassDB.Instance.NumRows(); i++)
        {
            if (ClassDB.Instance.GetAt(i).id == id)
            {
                nowpage = i-1;
            }
        }

        
        
        maxpage = ClassDB.Instance.NumRows() - 1;

        if (nowpage == 0)
            Prevbutton.interactable = false;
        else
            Prevbutton.interactable = true;

        if (nowpage == maxpage)
            Nextbutton.interactable = false;
        else
            Nextbutton.interactable = true;
        ShowClassData();
    }
    string needitem;
    int needitemhowmany;

    public GameObject[] Novicepanels;
    
    public void ShowClassData()
    {
        ClassDB.Row data =
            ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1);
        //Debug.Log("1");
        //현재 사용중

        //가이드 퀘스트
        Tutorialmanager.Instance.CheckTutorial("seeclass");


        if (PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1 == PlayerBackendData.Instance.ClassId)
        {
            ClassHaveObj[0].SetActive(false);
            ClassHaveObj[1].SetActive(false);
            ClassHaveObj[2].SetActive(true);
            ClassHaveObj[3].SetActive(true);


            if (data.id.Equals("C999"))
            {
                foreach (var VARIABLE in Novicepanels)
                {
                    VARIABLE.SetActive(false);
                }
            }
            else
            {
                foreach (var VARIABLE in Novicepanels)
                {
                    VARIABLE.SetActive(true);
                }
                if (PlayerBackendData.Instance.PassiveClassId.Length == 0)
                {
                    PassiveButton.interactable = true;
                    PassiveText.text = Inventory.GetTranslate("UI/패시브선택");
                }
                else
                {
                    if (PlayerBackendData.Instance.PassiveClassId[int.Parse(data.tier) - 1] == data.passive)
                    {
                        PassiveButton.interactable = false;
                        PassiveText.text = Inventory.GetTranslate("UI/패시브선택됨");
                    }
                    else
                    {
                        PassiveButton.interactable = true;
                        PassiveText.text = Inventory.GetTranslate("UI/패시브선택");

                    }
                }
            }
        }
        else if (PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.Isown)
        {
            //있지만 사용중아님
            ClassHaveObj[0].SetActive(false);
            ClassHaveObj[1].SetActive(true);
            ClassHaveObj[2].SetActive(false);
            ClassHaveObj[3].SetActive(true);
            if (data.id.Equals("C999"))
            {
                foreach (var VARIABLE in Novicepanels)
                {
                    VARIABLE.SetActive(false);
                }
            }
            else
            {
                foreach (var VARIABLE in Novicepanels)
                {
                    VARIABLE.SetActive(true);
                }

                if (PlayerBackendData.Instance.PassiveClassId.Length == 0)
                {
                    PassiveButton.interactable = true;
                    PassiveText.text = Inventory.GetTranslate("UI/패시브선택");
                }
                else
                {
                    if (PlayerBackendData.Instance.PassiveClassId[int.Parse(data.tier) - 1] == data.passive)
                    {
                        PassiveButton.interactable = false;
                        PassiveText.text = Inventory.GetTranslate("UI/패시브선택됨");
                    }
                    else
                    {
                        PassiveButton.interactable = true;
                        PassiveText.text = Inventory.GetTranslate("UI/패시브선택");
                    }
                }
            }
        }
        else
        {
            
            //아예없음
            ClassHaveObj[0].SetActive(true);
            ClassHaveObj[1].SetActive(false);
            ClassHaveObj[2].SetActive(false);
            ClassHaveObj[3].SetActive(false);
            Novicepanels[3].SetActive(false);
            //없다면
            needitem = ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1)
                .RequiredItemID;
            needitemhowmany = int.Parse(ClassDB.Instance
                .Find_id(PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1).RequiredItemHowmany);
            NoClassTextItemImage.sprite =
                SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(needitem).sprite);
            //필요한 아이템 출력

            //PassiveButton.gameObject.SetActive(false);
            Debug.Log("없당");
            NoClassTextNeedItem.text =
                $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(needitem).name)} {PlayerBackendData.Instance.CheckItemCount(needitem)}/{ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1).RequiredItemHowmany}";

            if (PlayerBackendData.Instance.CheckItemCount(needitem) >= needitemhowmany)
            {
                //있으면
                NoClassTextNeedItem.color = Color.cyan;
            }
            else
            {
                //없으면
                NoClassTextNeedItem.color = Color.red;
            }

            //가이드 퀘스트
            Tutorialmanager.Instance.CheckTutorial("seeclass");


        }
        //Debug.Log("1");

        //아바타 세팅
        PlayerImage[0].sprite = SpriteManager.Instance.GetSprite(data.classsprite);
        PlayerImage[1].sprite =
            SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(data.standweapon).Sprite);
        PlayerImage[2].sprite =
            SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(data.standsubweapon).Sprite);
        PlayerImage[1].enabled = false;
        PlayerImage[2].enabled = false;

        ClassName.text = Inventory.GetTranslate(data.name);
        Inventory.Instance.ChangeItemRareColor(ClassName, ClassDB.Instance.Find_id(data.id).tier);

        ClassInfo.text = Inventory.GetTranslate(data.info);
        ClassTier.text = PlayerData.Instance.getRank(data.tier);

        ClassLvStat[0].text = data.strperlv;
        ClassLvStat[1].text = data.dexperlv;
        ClassLvStat[2].text = data.intperlv;
        ClassLvStat[3].text = data.wisperlv;
        ClassLvStat[4].text = data.hpperlv;
        ClassLvStat[5].text = data.mpperlv;

        ClassLvStat[6].text = data.skillslotcount;
        ClassLvStat[7].text = data.skillcastingcount;


        if (data.id.Equals("C999"))
        {
            foreach (var VARIABLE in Novicepanels)
            {
                VARIABLE.SetActive(false);
            }
        }
        else
        {
          //  foreach (var VARIABLE in Novicepanels)
         //   {
          //      VARIABLE.SetActive(true);
          //  }
            
            SkillDB.Row skilldata = SkillDB.Instance.Find_Id(data.giveskill);
            ClassSkillImage.sprite = SpriteManager.Instance.GetSprite(skilldata.Sprite);
            switch (skilldata.RangeType)
            {
                case "M":
                    ClassAttackType[0].SetActive(true);
                    ClassAttackType[1].SetActive(false);
                    ClassAttackType[2].SetActive(false);
                    break;

                case "R":
                    ClassAttackType[0].SetActive(false);
                    ClassAttackType[1].SetActive(true);
                    ClassAttackType[2].SetActive(false);
                    break;
                case "MR":
                    ClassAttackType[0].SetActive(false);
                    ClassAttackType[1].SetActive(false);
                    ClassAttackType[2].SetActive(true);
                    break;
                case "MRR":
                    ClassAttackType[0].SetActive(true);
                    ClassAttackType[1].SetActive(true);
                    ClassAttackType[2].SetActive(false);
                    break;
                case "ALL":
                    ClassAttackType[0].SetActive(true);
                    ClassAttackType[1].SetActive(true);
                    ClassAttackType[2].SetActive(true);
                    break;
            }

            ClassSkill.text = Inventory.GetTranslate(skilldata.Name);
            ClassSkill.color = Inventory.Instance.GetRareColor(skilldata.Rare);

            //패시브
            PassiveInfo.text = Inventory.GetTranslate(PassiveDB.Instance.Find_id(data.passive).info);

        }

        if (nowpage == 0)
            Prevbutton.interactable = false;
        else
            Prevbutton.interactable = true;

        if (nowpage == maxpage)
            Nextbutton.interactable = false;
        else
            Nextbutton.interactable = true;
        //Debug.Log("1");

        if (nowpage != 0)
        {
            //왼쪽있음
            PrevClassname.text =
                $"{Inventory.GetTranslate(ClassDB.Instance.GetAt(nowpage - 1).name)}\n{PlayerData.Instance.getRank(ClassDB.Instance.GetAt(nowpage - 1).tier)}";
        }
        else
        {
            PrevClassname.text = "";
        }

        if (nowpage != maxpage)
        {
            //오른쪽직업있음
            NextClassname.text =
                $"{Inventory.GetTranslate(ClassDB.Instance.GetAt(nowpage + 1).name)}\n{PlayerData.Instance.getRank(ClassDB.Instance.GetAt(nowpage + 1).tier)}";
        }
        else
        {
            NextClassname.text = "";
        }

        //Debug.Log("1");
        Classpanel.Show(false);
    }

     public void ShowClassData_Preset()
    {
        Settingmanager.Instance.falseserveron();
        ClassDB.Row data =
            ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1);
        //Debug.Log("1");
        //현재 사용중


        if (PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1 == PlayerBackendData.Instance.ClassId)
        {
            ClassHaveObj[0].SetActive(false);
            ClassHaveObj[1].SetActive(false);
            ClassHaveObj[2].SetActive(true);
            ClassHaveObj[3].SetActive(true);
            if (PlayerBackendData.Instance.PassiveClassId.Length == 0)
            {
                PassiveButton.interactable = true;
                PassiveText.text = Inventory.GetTranslate("UI/패시브선택");
            }
            else
            {
                if (PlayerBackendData.Instance.PassiveClassId[int.Parse(data.tier) - 1] == data.passive)
                {
                    PassiveButton.interactable = false;
                    PassiveText.text = Inventory.GetTranslate("UI/패시브선택됨");
                }
                else
                {
                    PassiveButton.interactable = true;
                    PassiveText.text = Inventory.GetTranslate("UI/패시브선택");

                }
            }
        }
        else if (PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.Isown)
        {
            //있지만 사용중아님
            ClassHaveObj[0].SetActive(false);
            ClassHaveObj[1].SetActive(true);
            ClassHaveObj[2].SetActive(false);
            ClassHaveObj[3].SetActive(true);
            if (PlayerBackendData.Instance.PassiveClassId.Length == 0)
            {
                PassiveButton.interactable = true;
                PassiveText.text = Inventory.GetTranslate("UI/패시브선택");
            }
            else
            {
                if (PlayerBackendData.Instance.PassiveClassId[int.Parse(data.tier) - 1] == data.passive)
                {
                    PassiveButton.interactable = false;
                    PassiveText.text = Inventory.GetTranslate("UI/패시브선택됨");
                }
                else
                {
                    PassiveButton.interactable = true;
                    PassiveText.text = Inventory.GetTranslate("UI/패시브선택");
                }
            }
        }
        else
        {
            //없다면
            needitem = ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1)
                .RequiredItemID;
            needitemhowmany = int.Parse(ClassDB.Instance
                .Find_id(PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1).RequiredItemHowmany);
            NoClassTextItemImage.sprite =
                SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(needitem).sprite);
            //필요한 아이템 출력


            NoClassTextNeedItem.text =
                $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(needitem).name)} {PlayerBackendData.Instance.CheckItemCount(needitem)}/{ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1).RequiredItemHowmany}";

            if (PlayerBackendData.Instance.CheckItemCount(needitem) >= needitemhowmany)
            {
                //있으면
                NoClassTextNeedItem.color = Color.cyan;
            }
            else
            {
                //없으면
                NoClassTextNeedItem.color = Color.red;
            }

            //가이드 퀘스트
            Tutorialmanager.Instance.CheckTutorial("seeclass");

            ClassHaveObj[3].SetActive(false);
            //아예없음
            ClassHaveObj[0].SetActive(true);
            ClassHaveObj[1].SetActive(false);
            ClassHaveObj[2].SetActive(false);
            ClassHaveObj[3].SetActive(false);
        }
        //Debug.Log("1");

        //아바타 세팅
        PlayerImage[0].sprite = SpriteManager.Instance.GetSprite(data.classsprite);
        PlayerImage[1].sprite =
            SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(data.standweapon).EquipSprite);
        PlayerImage[2].sprite =
            SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(data.standsubweapon).EquipSprite);
        PlayerImage[1].enabled = false;
        PlayerImage[2].enabled = false;

        ClassName.text = Inventory.GetTranslate(data.name);
        Inventory.Instance.ChangeItemRareColor(ClassName, ClassDB.Instance.Find_id(data.id).tier);

        ClassInfo.text = Inventory.GetTranslate(data.info);
        ClassTier.text = PlayerData.Instance.getRank(data.tier);

        ClassLvStat[0].text = data.strperlv;
        ClassLvStat[1].text = data.dexperlv;
        ClassLvStat[2].text = data.intperlv;
        ClassLvStat[3].text = data.wisperlv;
        ClassLvStat[4].text = data.hpperlv;
        ClassLvStat[5].text = data.mpperlv;

        ClassLvStat[6].text = data.skillslotcount;
        ClassLvStat[7].text = data.skillcastingcount;

        if (data.giveskill != "")
        {
            //  ClassPassive.text = GetTranslate(data.name);
            SkillDB.Row skilldata = SkillDB.Instance.Find_Id(data.giveskill);
            ClassSkillImage.sprite = SpriteManager.Instance.GetSprite(skilldata.Sprite);
            switch (skilldata.RangeType)
            {
                case "M":
                    ClassAttackType[0].SetActive(true);
                    ClassAttackType[1].SetActive(false);
                    ClassAttackType[2].SetActive(false);
                    break;

                case "R":
                    ClassAttackType[0].SetActive(false);
                    ClassAttackType[1].SetActive(true);
                    ClassAttackType[2].SetActive(false);
                    break;
                case "MR":
                    ClassAttackType[0].SetActive(false);
                    ClassAttackType[1].SetActive(false);
                    ClassAttackType[2].SetActive(true);
                    break;
                case "MRR":
                    ClassAttackType[0].SetActive(true);
                    ClassAttackType[1].SetActive(true);
                    ClassAttackType[2].SetActive(false);
                    break;
                case "ALL":
                    ClassAttackType[0].SetActive(true);
                    ClassAttackType[1].SetActive(true);
                    ClassAttackType[2].SetActive(true);
                    break;
            }

            ClassSkill.text = Inventory.GetTranslate(skilldata.Name);
            Debug.Log(data.giveskill);
            ClassSkill.color = Inventory.Instance.GetRareColor(skilldata.Rare);


            //패시브
            PassiveInfo.text = Inventory.GetTranslate(PassiveDB.Instance.Find_id(data.passive).info);

        }

        if (nowpage == 0)
            Prevbutton.interactable = false;
        else
            Prevbutton.interactable = true;

        if (nowpage == maxpage)
            Nextbutton.interactable = false;
        else
            Nextbutton.interactable = true;
        //Debug.Log("1");

        if (nowpage != 0)
        {
            //왼쪽있음
            PrevClassname.text =
                $"{Inventory.GetTranslate(ClassDB.Instance.GetAt(nowpage - 1).name)}\n{PlayerData.Instance.getRank(ClassDB.Instance.GetAt(nowpage - 1).tier)}";
        }
        else
        {
            PrevClassname.text = "";
        }

        if (nowpage != maxpage)
        {
            //오른쪽직업있음
            NextClassname.text =
                $"{Inventory.GetTranslate(ClassDB.Instance.GetAt(nowpage + 1).name)}\n{PlayerData.Instance.getRank(ClassDB.Instance.GetAt(nowpage + 1).tier)}";
        }
        else
        {
            NextClassname.text = "";
        }
    }
     
    public void UpPage()
    {
        //다음페이지
        if (nowpage != maxpage)

        {
            nowpage++;
            if (nowpage-1 == maxpage)
                Nextbutton.interactable = false;
            else
                Nextbutton.interactable = true;

            Prevbutton.interactable = true;
            ShowClassData();
        }

    }
    public void DownPage()
    {
        //다음페이지
        if (nowpage != 0)
        {
            nowpage--;
            if (nowpage == 0)
            {
                Prevbutton.interactable = false;

            }
            else
            {
                Prevbutton.interactable = true;

            }
            Nextbutton.interactable = true;
            ShowClassData();
        }
    }

    //클래스 선택
    public void Bt_SelectClass()
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (PartyRaidRoommanager.Instance.partyroomdata.isstart)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/콘텐츠 중 불가능"), alertmanager.alertenum.주의);
            return;
        }
        
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/사냥터만가능"), alertmanager.alertenum.주의);
            return;
        }
        
        //가이드 퀘스트
        Tutorialmanager.Instance.CheckTutorial("buyclass");
        
        string[] skills = PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1;
        PlayerBackendData.Instance.ClassId = PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1;

        for (int i = 0; i < PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1.Length; i++)
        {
            if (PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[i] == "")
            {
                if (!PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1
                        .Contains(skills[i]))
                    PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[i] = skills[i];
            }
        }

        if (PlayerBackendData.Instance.tutoid != "11")
        {
            if (PlayerBackendData.Instance.tutoid.Equals("2") &&
                TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).type.Equals("buyclass"))
            {
                Tutorialmanager.Instance.NewTuto1[25].SetActive(true);
            }
        }

        Tutorialmanager.Instance.NewTuto1[11].SetActive(false);
        //가이드 퀘스트
        Tutorialmanager.Instance.CheckTutorial("buyclass3");

        
        Skillmanager.Instance.mainplayer.SetClass_start();
        PlayerData.Instance.RefreshPlayerstat();
        PlayerData.Instance.RefreshClassName();
        Savemanager.Instance.SaveClassData();
        Savemanager.Instance.Save();
        Settingmanager.Instance.SaveAllNoLog();

        ShowClassData();
        RefreshJobSlot();
        
       
        
    }
    public void Bt_SelectClass(string classid,string[] skills,bool justequip = false)
    {
        PlayerBackendData.Instance.ClassId = classid;

        for (int i = 0;
             i < PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1.Length;
             i++)
        {
            PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[i] = "";
        }

        if (justequip)
        {
            for (int i = 0; i < PlayerBackendData.Instance.Skills.Count; i++)
            {
                if(PlayerBackendData.Instance.Skills[i] == "")
                    continue;
                try
                {
                    if (PlayerBackendData.Instance.Skills[i] != null && PlayerBackendData.Instance.Skills[i] != "")
                    {
                    
                        Debug.Log(PlayerBackendData.Instance.Skills[i]);
                        PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[i] = PlayerBackendData.Instance.Skills[i];
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
               
            }
        }
        else
        {
            for (int i = 0; i < Skillmanager.Instance.mainplayer.skillslotcount + Skillmanager.Instance.mainplayer.classskillslotcount+Skillmanager.Instance.mainplayer.AdventureRankSkillSlot(PlayerBackendData.Instance.GetAdLv()); i++)
            {
                if(skills[i] == ("True"))
                    continue;
                if (skills[i] != null && skills[i] != "")
                {
                    PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[i] = skills[i];
                }
            }
        }
      
        
    }
    public void Bt_SelectPassive()
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI4/사냥터에서만변경가능"), alertmanager.alertenum.주의);
            return;
        }
        
        ClassDB.Row data = ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1);
        PlayerBackendData.Instance.PassiveClassId[int.Parse(data.tier) - 1] = data.passive;

        //가이드 퀘스트
        Tutorialmanager.Instance.CheckTutorial("setpassive");
        
        
        if (PlayerBackendData.Instance.tutoid.Equals("2") &&
            TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).type.Equals("buyclass"))
        {
            Tutorialmanager.Instance.NewTuto1[16].SetActive(false);
            Tutorialmanager.Instance.NewTuto1[17].SetActive(true);
        }
        
        Passivemanager.Instance.Refresh();
        PlayerData.Instance.RefreshPlayerstat();
        ShowClassData();
        RefreshJobSlot();
        Savemanager.Instance.SaveClassData();
        Savemanager.Instance.Save();

    }
    public void Bt_SelectPassive(string classid)
    {
        ClassDB.Row data = ClassDB.Instance.Find_id(classid);
        PlayerBackendData.Instance.PassiveClassId[int.Parse(data.tier) - 1] = data.passive;
    }
    public void Bt_SelectPassive_Preset(string[] passiveid)
    {
        for (int i = 0; i < passiveid.Length; i++)
        {
            if (passiveid[i] == null || passiveid[i] == "" || passiveid[i] == "True")
                continue;
           // Debug.Log(passiveid[i]);
            PlayerBackendData.Instance.PassiveClassId[i] = passiveid[i];
        }
    }
    //클래스 선택
    public void Bt_BuyClass()
    {
        //아이템이 있다면.
        if (PlayerBackendData.Instance.CheckItemCount(needitem) >= needitemhowmany)
        {
            PlayerBackendData.Instance.RemoveItem(needitem, needitemhowmany);
            PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.Isown = true;
            //기본스킬 지급
            PlayerBackendData.Instance.AddSkill(ClassDB.Instance
                .Find_id(PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1).giveskill);
            ShowClassData();
            Skillmanager.Instance.mainplayer.SetClass_start();

        

            int num =
                int.Parse(ClassDB.Instance
                    .Find_id(PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1).tier) - 1;
            if (PlayerBackendData.Instance.PassiveClassId[num] == "" && num != 0)
            {
               Bt_SelectPassive();
            }
            Savemanager.Instance.SaveClassData();
            Savemanager.Instance.SaveInventory();
            alertmanager.Instance.NotiCheck_Class();
            RefreshJobSlot();
            RefreshJobitemslots();
            Savemanager.Instance.SaveClassData();
            Savemanager.Instance.Save();
            Settingmanager.Instance.SaveAllNoLog();
        }
        else
        {
            //없다면
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/아이템부족"), alertmanager.alertenum.주의);
        }
     
    }

    
    
    
    public void Bt_BuyClass(string id)
    {
            PlayerBackendData.Instance.ClassData[id].Isown = true;
            //기본스킬 지급
            PlayerBackendData.Instance.AddSkill(ClassDB.Instance
                .Find_id(PlayerBackendData.Instance.ClassData[id].ClassId1).giveskill);

            int num =
                int.Parse(ClassDB.Instance
                    .Find_id(PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1).tier) - 1;
                //Bt_SelectPassive(id);
        
                RefreshJobSlot();
            RefreshJobitemslots();
         
    }
    [SerializeField]
    itemjobslot[] jobitemslots;

    public void RefreshJobitemslots()
    {
        for(int i = 0; i < jobitemslots.Length;i++)
        {
            jobitemslots[i].Refresh();
        }
    }

    public void Bt_ShowClassSkill()
    {
        string skillid = ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassData.ToList()[nowpage].Value.ClassId1)
            .giveskill;
        SkillInventory.Instance.ShowChangePanel(skillid);
    }

}
