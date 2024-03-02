using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemcountslot : MonoBehaviour
{
    public Image ItemImage;
    public Text ItemName;
    // Start is called before the first frame update

    public void SetData(string itemid,int maxcount)
    {
        ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(itemid).sprite);
        int index = PlayerBackendData.Instance.GetItemIndex(itemid);
        int curcount = 0;

        if(index == -1)
        {
            //아이템이 없다면
            curcount = 0;
        }
        else
        {
            curcount = PlayerBackendData.Instance.ItemInventory[index].Howmany;
        }

        if (curcount >= maxcount)
            ItemName.color = Color.cyan;
        else
            ItemName.color = Color.red;

        ItemName.text =
            $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(itemid).name)} {curcount}/{maxcount}";
    }



}