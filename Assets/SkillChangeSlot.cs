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
            //��ų���ִ�
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
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI4/����Ϳ��������氡��"), alertmanager.alertenum.����);
            return;
        }
        
        
        if(!islock)
        {
            SkillDB.Row selectskilldata = SkillDB.Instance.Find_Id(SkillInventory.Instance.SelectSkillid);
            // Debug.Log(selectskilldata.RangeType);
            // Debug.Log(EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid).Rangetype);
            bool canequip = false; //�ؿ�����ġ���� true���Ǹ� ��ų�������� 
            switch(selectskilldata.RangeType)
            {
                case "M":
                    if (EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid).Rangetype == "M")
                        canequip = true;
                    break;
                case "MRR":
                    //�������Ÿ�
                    if (EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid).Rangetype == "M" ||
                        EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid).Rangetype == "R")
                        canequip = true;
                    break;
                case "R":
                    //���Ÿ�
                    if (EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid).Rangetype == "R")
                        canequip = true;
                    break;
                case "MR":
                    //����
                    if (EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid).Rangetype == "MR")
                        canequip = true;
                    break;
                case "ALL":
                    //��ü
                    canequip = true;
                    break;
            }
            
            if(canequip)
            {
                for (int i = 0; i < PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1.Length; i++)
                {
                    //������ ��ų���ִٸ� �װ� ����.
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
                
                //��ų�� ����
                PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[index] = SkillInventory.Instance.SelectSkillid;
                Skillmanager.Instance.mainplayer.castingmanager.skillslots[index].RefreshSkill();
                
                //���̵�����Ʈ
                Tutorialmanager.Instance.CheckTutorial("equipskill");
                Tutorialmanager.Instance.CheckTutorial("equipskill2");


                PlayerData.Instance.RefreshPlayerstat();
                
                
                
                Savemanager.Instance.SaveClassData();
                SkillInventory.Instance.ShowSkillInventory();
                SkillInventory.Instance.CloseChangePanel();
                Savemanager.Instance.Save();
                alertmanager.Instance.ShowAlert(TranslateManager.Instance.GetTranslate("UI2/��ų�����Ϸ�"), alertmanager.alertenum.�Ϲ�);
            }
            else
            {
                alertmanager.Instance.ShowAlert(TranslateManager.Instance.GetTranslate("UI/����Ÿ���̴ٸ�"), alertmanager.alertenum.����);
               // Debug.Log("���� Ÿ�԰� ��ų Ÿ���� �ٸ��ϴ�.");
            }
          
        }
        else
        {
            //��ų����ұ�
        }
    }
}
