using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class equipshowslot : MonoBehaviour
{
    // Start is called before the first frame update


    public Image EquipImage;
    public Image EquipRare;

    public string id;

    public void Init(string id)
    {
        this.id = id;
        if (id != "")
        {
            EquipImage.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(id).Sprite);
            EquipRare.color = Inventory.Instance.GetRareColor(EquipItemDB.Instance.Find_id(id).craftrarelist);
        }
    }

    public void Bt_ShowEquipData()
    {
        Inventory.Instance.ShowInventoryItem_NotMine(new EquipDatabase(id));
    }
    
}
