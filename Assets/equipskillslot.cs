using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 
    equipskillslot : MonoBehaviour
{
    public Text nametext;
    public Image rareimage;
    public Animator ani;
    public void SetSkill(string id,string rare,string lv)
    {
        //Debug.Log(id);
        nametext.text =
            $"{Inventory.GetTranslate(EquipSkillDB.Instance.Find_id(id).name)} Lv.{lv}";
//        Debug.Log("Rare "+  rare  + "ID" + id);
        rareimage.color = Inventory.Instance.GetRareColor(rare);
        ani.SetTrigger("start");
    }
}
