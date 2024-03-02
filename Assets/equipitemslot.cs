using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class equipitemslot : MonoBehaviour
{
    public bool isMine;
    public EquipmenyEnum SlotType;

    public EquipDatabase data;
    [SerializeField]
    EquipmentItemData equipslotdata; 
    public Text ItemName; //아이템 이미지
    public GameObject[] Itemhaveobj; //없으면 0활성화 있으면 1활성화
    public string itemkeyid;
    public GameObject SetEffect;

    public void SetItem(EquipDatabase data,bool ismine = true,bool isname = true)
    {
        isMine = ismine;
        itemkeyid = data.KeyId1;
        this.data = data;
        equipslotdata.Refresh(this.data);
        ItemName.text = isMine ? data.GetItemName() : "";
        Inventory.Instance.ChangeItemRareColor(ItemName, data.Itemrare);
        Refresh();
    }
    public void SetItem()
    {
        data = null;
        Refresh();
    }
    public void Refresh()
    {
        if(data == null)
        {
            Itemhaveobj[0].SetActive(true);
            Itemhaveobj[1].SetActive(false);
        }
        else
        {
            Itemhaveobj[0].SetActive(false);
            Itemhaveobj[1].SetActive(true);
        }
    }

    public void Bt_ShowEquiptmentInven()
    {
        if (data == null) return;


        if (isMine)
            Inventory.Instance.ShowEquipInventory((int)SlotType);
        else
        {
            Inventory.Instance.ShowInventoryItem(data, isMine);
        }
    }

    public void BtShowEquiptInfo()
    {
        if (data != null)
            Inventory.Instance.ShowInventoryItem(data,isMine);
    }

    public void ShowSetParticle (bool ison)
    {
        SetEffect.SetActive(ison);
    }

   
}


