using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentItemData : MonoBehaviour
{
    public EquipDatabase data;
    //장비의 이미지와 등급 그리고 제련을 나타낸다.
    public Image ItemImage; //아이템 이미지
    public GameObject[] ItemCraftRare; //걸작레벨
    public SmeltSlot[] SmeltSlots;
    public Text EnchantText;
    public GameObject AllEskill;
    public GameObject AllEskill2;
    public void Refresh(EquipDatabase data2)
    {
        if (data2.MaxStoneCount1 > 10)
            data2.MaxStoneCount1 = 10;
        if (AllEskill != null)
        {
            AllEskill.SetActive(false);
        }
        if (AllEskill2 != null)
        {
            AllEskill2.SetActive(false);
        }
        this.data = data2;

////        Debug.Log(data2.Itemid);
        ItemImage.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(data2.Itemid).Sprite);

        foreach (var t in ItemCraftRare)
            t.SetActive(false);

        try
        {
            if (data2.CraftRare1 >= 1)
            {
                ItemCraftRare[data2.CraftRare1 - 2].SetActive(true);
            }
        }
        catch (Exception e)
        {
            ItemCraftRare[0].SetActive(false);
        }
       
        
        int num = EquipItemDB.Instance.Find_id(data2.Itemid).SpeMehodP != "0" ? 1 : 0;

        if (AllEskill != null)
        {
            //            Debug.Log("개수는 "+data.EquipSkill1.Count + num);
            if (data2.EquipSkill1.Count - num == 5)
            {
                AllEskill.SetActive(true);
            }
        }if (AllEskill != null)
        {
            //            Debug.Log("개수는 "+data.EquipSkill1.Count + num);
            if (data2.EquipSkill1.Count - num == 5)
            {
                AllEskill.SetActive(true);
            }
        }
        if (AllEskill2 != null)
        {
            //            Debug.Log("개수는 "+data.EquipSkill1.Count + num);
            int numss = 0;
            for (int i = 0; i < data2.EquipSkill1.Count - num; i++)
            {
                if (data2.EquipSkill1[i] == "")
                    continue;
                if (EquipSkillDB.Instance.Find_id(data2.EquipSkill1[i]).lv ==
                    EquipSkillDB.Instance.Find_id(data2.EquipSkill1[i]).maxlv)
                {
                    numss++;
                }
            }
            if (numss==5)
            {
                AllEskill2.SetActive(true);
            }
        }

        if(data2.EnchantNum1 !=0)
        {
            EnchantText.text = $"+{data2.EnchantNum1}";
        }
        else
        {
            EnchantText.text = "";
        }

        for (int i = 0; i < SmeltSlots.Length; i++)
        {
            SmeltSlots[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < data2.MaxStoneCount1; i++)
        {
            SmeltSlots[i].Refresh(data2.SmeltStatCount1[i]);
                SmeltSlots[i].SmeltShow(data2.SmeltStatCount1[i]);
                SmeltSlots[i].gameObject.SetActive(true);
        }

    }

}
