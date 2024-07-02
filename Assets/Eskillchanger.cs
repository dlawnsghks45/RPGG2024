using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class Eskillchanger : MonoBehaviour
{
    private string id;
    public bool issucc;
    public Text Name;
    public Text Info;
    public Image Rare;
    public ParticleSystem Effect;

    public UIToggle LockEs;
    public GameObject LockEsTextobj; // 이름 + 잠금
    public Text LockEsText; // 이름 + 잠금
    public void ShowData(string id)
    {
        this.id = id;
//        Debug.Log(id);
        Name.text =
            $"[{Inventory.GetTranslate(EquipSkillDB.Instance.Find_id(id).name)} Lv.{EquipSkillDB.Instance.Find_id(id).lv}]";
        Info.text = Inventory.GetTranslate(EquipSkillDB.Instance.Find_id(id).info);
        Rare.color = Inventory.Instance.GetRareColor(EquipSkillDB.Instance.Find_id(id).rare);
    }
    public void ShowDataNew(string id)
    {
        this.id = id;
        Name.text = 
            $"[{Inventory.GetTranslate(EquipSkillDB.Instance.Find_id(id).name)} Lv.{EquipSkillDB.Instance.Find_id(id).lv}]";

        Info.text = Inventory.GetTranslate(EquipSkillDB.Instance.Find_id(id).info);
        Rare.color = Inventory.Instance.GetRareColor(EquipSkillDB.Instance.Find_id(id).rare);
        
        Effect.Play();
    }
    public void NoData()
    {
        id = "";
        Name.text = "?";
        Info.text = "?";
        Rare.color = Color.white;
    }

    public void LockEquipSkill()
    {
        if (!issucc)
        {
            if (equipoptionchanger.Instance.lockcountEs.Equals(equipoptionchanger.Instance.islessequipskill ? 1:2))
            {
                Debug.Log("더이상안됨");
            }

            if (LockEs.gameObject.activeSelf)
            {
                LockEsText.text = string.Format(Inventory.GetTranslate("UI3/특수효과잠금"), Name.text);
                equipoptionchanger.Instance.lockcountEs++;
                LockEsTextobj.SetActive(true);
            }
            equipoptionchanger.Instance.RefreshESSKILLCount();
        }
        else
        {
            if (SuccManager.Instance.lockcountEs.Equals(SuccManager.Instance.maxcount))
            {
                Debug.Log("더이상안됨");
            }

            if (LockEs.gameObject.activeSelf)
            {
                LockEsText.text = string.Format(Inventory.GetTranslate("UI3/특수효과잠금"), Name.text);
                SuccManager.Instance.lockcountEs++;
                LockEsTextobj.SetActive(true);
            }
        }

    }

    public void UnLockEquipSkill()
    {
            equipoptionchanger.Instance.ReducelockcountEs();
            LockEsTextobj.SetActive(false);
            LockEs.gameObject.SetActive(true);
            LockEs.IsOn = false;
            equipoptionchanger.Instance.RefreshESSKILLCount();
    }
    public void RemoveEquipSkillLock()
    {
        equipoptionchanger.Instance.ReducelockcountEs();
        LockEsTextobj.SetActive(false);
        LockEs.gameObject.SetActive(false);
        LockEs.IsOn = false;
        equipoptionchanger.Instance.RefreshESSKILLCount();
    }
    public void UnLockEquipSkillSucc()
    {
        SuccManager.Instance.ReducelockcountEs();
        LockEsTextobj.SetActive(false);
        LockEs.IsOn = false;
    }


    public void Bt_ChangeName()
    {
        if (id != "")
            Inventory.Instance.ShowEquipskill(id);
        return;
        if (Name.gameObject.activeSelf)
        {
            Name.gameObject.SetActive(false);
            Info.gameObject.SetActive(true);
        }
        else
        {
            Name.gameObject.SetActive(true);
            Info.gameObject.SetActive(false);
        }
    }




}
