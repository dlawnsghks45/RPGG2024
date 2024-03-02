using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class shopitemslot : MonoBehaviour
{
   string itemid;
   private int Count;
   public Image ItemImage;
   public Text ItemName;
   public Text ItemCount;


   public void SetItems(string id, int howmany)
   {
      itemid = id;
      Count = howmany;

      ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(itemid).sprite);
      ItemName.text = Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(itemid).name);
      Inventory.Instance.ChangeItemRareColor(ItemName,ItemdatabasecsvDB.Instance.Find_id(itemid).rare);
      
      ItemCount.text = Count.ToString("N0");
      
   
   }


   public void Bt_ShowItemInfo()
   {
      Inventory.Instance.ShowInventoryItem_NoMine(itemid);
   }
}
