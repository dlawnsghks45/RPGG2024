using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class percentdataslot : MonoBehaviour
{
    public string id;
    public Image Itemimage;

    public Text Name;
    public Text Percent;
    // Start is called before the first frame update
    public void Refresh(string ids,string howmany,string Per)
    {
        id = ids;
        Itemimage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(id).sprite);
        Name.text = $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(id).name)} X {howmany}";
        Percent.text = $"{Per}%";
    }

    public void Bt_Show()
    {
        Inventory.Instance.ShowInventoryItem_NoMine(id);
    }
}
