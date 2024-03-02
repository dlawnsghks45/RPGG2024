using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftSuccesssSlot : MonoBehaviour
{
    public string itemid;
    string isequip;
    public Image ItemImage;
    public Text ItemName;
    public Text CountText; //개수 1000/2000 등
    public Text Percent;
    // Start is called before the first frame update
    public void Refresh(string craftid,string isequip,string itemid,string min,string max,string percent)
    {
        this.itemid = itemid;
        this.isequip = isequip;


        if (isequip == "TRUE")
        {
            ItemImage.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(itemid).Sprite);
            ItemName.text = Inventory.GetTranslate(EquipItemDB.Instance.Find_id(itemid).Name);
            ItemName.color = Inventory.Instance.GetRareColor(EquipItemDB.Instance.Find_id(itemid).Rare);

            CraftTableDB.Row craftdata = CraftTableDB.Instance.Find_id(craftid);
            Percent.text = $"{percent}%";
            if (min.Equals(max))
            {
                //같으면 하나만 
                CountText.text = min.ToString();
            }
            else
            {
                //다르면 물결
                CountText.text = $"{min}~{max}";
            }
        }
        else
        {
            ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(itemid).sprite);
            ItemName.text = Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(itemid).name);
            ItemName.color = Inventory.Instance.GetRareColor(ItemdatabasecsvDB.Instance.Find_id(itemid).rare);
            CraftTableDB.Row craftdata = CraftTableDB.Instance.Find_id(craftid);
            Percent.text = $"{percent}%";

            if (min.Equals(max))
            {
                //같으면 하나만 
                CountText.text = min.ToString();
            }
            else
            {
                //다르면 물결
                CountText.text = $"{min}~{max}";
            }
        }
    }

    public void Bt_ShowItem()
    {
        if (isequip == "TRUE")
        {
            Inventory.Instance.ShowInventoryItem_NotMine(new EquipDatabase(itemid));
        }
        else
        {
            Inventory.Instance.ShowInventoryItem_NoMine(itemid);
        }
    }

}
