using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class iteminventoryslot : MonoBehaviour
{
    public Image ItemImage;
    public Text itemname;
    public Text itemCount;

    public string Itemid;
    private ItemdatabasecsvDB.Row item;

    public void Refresh(string itemid, int count)
    {
     
        this.Itemid = itemid;
        
////        Debug.Log(itemid);
        item = ItemdatabasecsvDB.Instance.Find_id(itemid);
        itemname.text = Inventory.GetTranslate(item.name);
        ItemImage.sprite = SpriteManager.Instance.GetSprite(item.sprite);
        Inventory.Instance.ChangeItemRareColor(itemname, item.rare);
        itemCount.text = count.ToString("N0");
    }

    public void Bt_ShowItem()
    {
       Inventory.Instance.ShowInventoryItem(Itemid);
    }
}
