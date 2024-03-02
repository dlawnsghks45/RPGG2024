using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class postitemslot : MonoBehaviour
{
    public Image itemimage;
    public Text itemname;
    public Text itemcount;

    string id;

    public void refresh(UPostChartItem data)
    {
        id = data.itemID;
        itemimage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(data.itemID).sprite);
        itemname.text = Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(data.itemID).name);
        Inventory.Instance.ChangeItemRareColor(itemname, ItemdatabasecsvDB.Instance.Find_id(data.itemID).rare);
        itemcount.text = $"x{data.itemCount}";
    }

    public void bt_ShowInfo()
    {
        Inventory.Instance.ShowInventoryItem_NoMine(id);
    }
}
