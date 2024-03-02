using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skillminislot : MonoBehaviour
{
    private string skillid;
    public Image SkillImge;
    public Image Rare;
    public Text equippednum;
    public Image skilltypeimage;

    public void ShowSlotNUM()
    {
        equippednum.text = "";
        for (int i = 0;
             i < PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1.Length;
             i++)
        {
            if (PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[i] == "")
            {
                continue;
            }

            if (PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[i] == (skillid))
            {
                equippednum.text = (i+1).ToString();
            }
        }
    }
    
    public void RefreshSkill(SkillDB.Row data)
    {
        skillid = data.Id;
        ShowSlotNUM();
        SkillImge.sprite = SpriteManager.Instance.GetSprite(data.Sprite);
        Rare.color = Inventory.Instance.GetRareColor(data.Rare);
        switch (data.Type)
        {
            case"Physic":
                skilltypeimage.color = Color.red;
                break;
            case"Magic":
                skilltypeimage.color = Color.cyan;
                break;
        }
    }

    public void Bt_ShowSkill()
    {
        SkillInventory.Instance.SelectSkillid = skillid;
        SkillInventory.Instance.ShowChangePanel();
    }
}
