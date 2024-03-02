using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class craftresultslot : MonoBehaviour
{
    public Image CraftingImage;
    public Text ItemNameText;

    public GameObject Equipskillpanel;
    public TMProHyperLink[] Equipskills;

    public void SetEnd(Sprite sprite,string text,string rare,EquipDatabase equips = null)
    {
        CraftingImage.sprite = sprite;
        ItemNameText.text = text;
        Inventory.Instance.ChangeItemRareColor(ItemNameText, rare);

        if (Equipskillpanel != null)
        {
            Equipskillpanel.SetActive(false);
            foreach (var t in Equipskills)
            {
                t.gameObject.SetActive(false);
            }
        }


        if (equips == null) return;
        if (!equips.IshaveEquipSkill) return;
        Equipskillpanel.SetActive(true);
        Debug.Log(equips.EquipSkill1.Count);
        for (var i = 0; i < equips.EquipSkill1.Count; i++)
        {
           Debug.Log(equips.EquipSkill1[i]);
            Equipskills[i].SetEquipSkill(equips.EquipSkill1[i]);
            Equipskills[i].gameObject.SetActive(true);
        }


    }
}
