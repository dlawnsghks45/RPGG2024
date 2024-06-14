using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillChangeSlot : MonoBehaviour
{
    public int index;
    public GameObject HaveSkill;
    public GameObject NoSkill;
    public GameObject LockSkill;

    public string skillid;
    public Image SkillImage;
    public Image SkillRare;
    private SkillDB.Row skilldata;

    public bool islock;

    public void LockSkillSlot()
    {
        HaveSkill.SetActive(false);
        NoSkill.SetActive(false);
        LockSkill.SetActive(true);
        islock = true;
    }
    public void Refresh(string id)
    {
        SkillRare.color = Color.white;

        islock = false;
        if (string.IsNullOrEmpty(id))
        {
            HaveSkill.SetActive(false);
            NoSkill.SetActive(true);
            LockSkill.SetActive(false);
        }
        else
        {
            HaveSkill.SetActive(true);
            NoSkill.SetActive(false);
            LockSkill.SetActive(false);
            skillid = id;

           // Debug.Log(skillid);
            //스킬이있다
            skilldata = SkillDB.Instance.Find_Id(skillid);
            SkillImage.sprite = SpriteManager.Instance.GetSprite(skilldata.Sprite);
            SkillRare.color = Inventory.Instance.GetRareColor(skilldata.Rare);

        }
    }

    public void EquipSkill()
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI4/사냥터에서만변경가능"), alertmanager.alertenum.주의);
            return;
        }
        
        
        if(!islock)
        {
            SkillDB.Row selectskilldata = SkillDB.Instance.Find_Id(SkillInventory.Instance.SelectSkillid);
            // Debug.Log(selectskilldata.RangeType);
            // Debug.Log(EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid).Rangetype);
            bool canequip = false; //밑에스위치에서 true가되면 스킬장착가능 
            switch(selectskilldata.RangeType)
            {
                case "M":
                    if (EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid).Rangetype == "M")
                        canequip = true;
                    break;
                case "MRR":
                    //근접원거리
                    if (EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid).Rangetype == "M" ||
                        EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid).Rangetype == "R")
                        canequip = true;
                    break;
                case "R":
                    //원거리
                    if (EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid).Rangetype == "R")
                        canequip = true;
                    break;
                case "MR":
                    //마법
                    if (EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid).Rangetype == "MR")
                        canequip = true;
                    break;
                case "ALL":
                    //전체
                    canequip = true;
                    break;
            }
            
            if(canequip)
            {
                for (int i = 0; i < PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1.Length; i++)
                {
                    //장착한 스킬이있다면 그걸 뺀다.
                    if (SkillInventory.Instance.SelectSkillid == PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[i])
                    {
                        SkillRare.color = Color.white;
                        PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[i] = "";
                        Skillmanager.Instance.mainplayer.castingmanager.skillslots[i].RefreshSkill();
                    }
                }

                if (int.Parse(SkillInventory.Instance.SelectSkillid) >= 5000)
                {
                    TutorialTotalManager.Instance.CheckGuideQuest("equipskillbook");
                }
                
                //스킬을 장착
                PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[index] = SkillInventory.Instance.SelectSkillid;
                Skillmanager.Instance.mainplayer.castingmanager.skillslots[index].RefreshSkill();
                
                //가이드퀘스트
                Tutorialmanager.Instance.CheckTutorial("equipskill");
                Tutorialmanager.Instance.CheckTutorial("equipskill2");


                PlayerData.Instance.RefreshPlayerstat();
                
                
                
                Savemanager.Instance.SaveClassData();
                SkillInventory.Instance.ShowSkillInventory();
                SkillInventory.Instance.CloseChangePanel();
                Savemanager.Instance.Save();
                alertmanager.Instance.ShowAlert(TranslateManager.Instance.GetTranslate("UI2/스킬장착완료"), alertmanager.alertenum.일반);
            }
            else
            {
                alertmanager.Instance.ShowAlert(TranslateManager.Instance.GetTranslate("UI/무기타입이다름"), alertmanager.alertenum.주의);
               // Debug.Log("무기 타입과 스킬 타입이 다릅니다.");
            }
          
        }
        else
        {
            //스킬저장불까
        }
    }
}
