using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class craftResourceSlot : MonoBehaviour
{
    public string itemid;

    public Image ItemImage;
    public Text ItemName;
    public Text CountText; //���� 1000/2000 ��
    // Start is called before the first frame update
    public void Refresh(string itemid,string count)
    {
        this.itemid = itemid;

            ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(itemid).sprite);
            ItemName.text = Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(itemid).name);
            ItemName.color = Inventory.Instance.GetRareColor(ItemdatabasecsvDB.Instance.Find_id(itemid).rare);
        int index = PlayerBackendData.Instance.ItemInventory.FindIndex(r => r.Id == itemid); //ItemInventory.IndexOf(itemid);
            if (index == -1)
            {
                //�������� ����
                CountText.text = $"0/{int.Parse(count) * CraftManager.Instance.nowselectcount}";
                CountText.color = Color.red;
                CraftManager.Instance.cancraft = false;
            }
            else
            {
            //�������� ����
                CountText.text =
                    $"{PlayerBackendData.Instance.ItemInventory[index].Howmany}/{int.Parse(count) * CraftManager.Instance.nowselectcount}";
                if(PlayerBackendData.Instance.ItemInventory[index].Howmany >= int.Parse(count) * CraftManager.Instance.nowselectcount)
                {
                    //����
                    CountText.color = Color.cyan;
                }
                else
                {
                    CountText.color = Color.red;
                    CraftManager.Instance.cancraft = false;
                }
            }
    }

    public void Bt_ShowItemInfo()
    {
        Inventory.Instance.ShowInventoryItem_NoMine(itemid);
    }
}
