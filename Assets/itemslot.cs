using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class itemslot : MonoBehaviour
{
    public string Itemid;
    public int itemcount;
    public Image ItemImage;
    public Image ItemRare;
    public TextMeshProUGUI itemhowmany;
    public ItemdatabasecsvDB.Row item;

    public void SetItem(string itemid, int count)
    {
        this.Itemid = itemid;
        item = ItemdatabasecsvDB.Instance.Find_id(itemid);
        this.itemcount = count;
        ItemImage.sprite = SpriteManager.Instance.GetSprite(item.sprite);
        ItemRare.color = Inventory.Instance.GetRareColor(item.rare);
        if (count == 0)
            itemhowmany.text = "";
        else
            itemhowmany.text = count.ToString();
    }
    public void Bt_ShowItemInfo()
    {
        Inventory.Instance.ShowInventoryItem_NoMine(Itemid);
    }
}
