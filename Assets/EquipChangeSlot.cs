using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipChangeSlot : MonoBehaviour
{
    private string id;
    public Image Equipimage;
    public Text EquipName;
    public GameObject SelectButton;
    
    public void Refresh(EquipItemDB.Row Data,string rare)
    {
        id = Data.id;

        Equipimage.sprite = SpriteManager.Instance.GetSprite(Data.Sprite);
        EquipName.text = Inventory.GetTranslate(Data.Name);
        EquipName.color = Inventory.Instance.GetRareColor(rare);
    }
    
    public void Bt_SelectEquip()
    {
        Equipchangemanager.Instance.Bt_SelectItem(id);
        SelectButton.gameObject.SetActive(true);
    }

    public void OffPanel()
    {
        SelectButton.gameObject.SetActive(false);
    }

}
