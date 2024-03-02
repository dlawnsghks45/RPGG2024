using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    EquipDatabase data;
    public bool isEquipBag;
    public EquipmentItemData Equipslot;
    public Text Itemname;
    public GameObject isEquipobj;
    public GameObject lockui;
    public CanvasGroup canvas;
    public void Refresh(EquipDatabase data)
    {
        if (lockui != null)
        {
            lockui.SetActive(data.IsLock);
        }
        this.data = data;
        Equipslot.Refresh(data);
        Itemname.text = data.GetItemName();
        Inventory.Instance.ChangeItemRareColor(Itemname, data.Itemrare);
        isEquipobj.SetActive(data.IsEquip);
    }
    public void RefreshNoName(EquipDatabase data)
    {
        if (lockui != null)
        {
            lockui.SetActive(data.IsLock);
        }
        this.data = data;
        Equipslot.Refresh(data);
        Itemname.text = "";
        isEquipobj.SetActive(data.IsEquip);
    }
    public void ShowObject()
    {
        if (canvas != null)
        {
            canvas.alpha = 1;
            canvas.interactable = true;
            transform.SetAsFirstSibling();
        }
        else
        {
            gameObject.SetActive(true);
            transform.SetAsFirstSibling();

        }
    }

    public void RemoveObject()
    {
        if (canvas != null)
        {
            canvas.alpha = 0;
            canvas.interactable = false;
            transform.SetAsLastSibling();
        }
        else
        {
            gameObject.SetActive(false);
            transform.SetAsLastSibling();
        }
    }
    
    public void Bt_ShowInventory()
    {
        if (presetmanager.Instance.Panel.IsActive())
        {
            uimanager.Instance.AddUiview(presetmanager.Instance.Panel,true);
            Inventory.Instance.ShowInventoryItem(data);
        }
        else if (isEquipBag && SuccManager.Instance.IsSucc)
        {
            //전승아이템 선택
            SuccManager.Instance.SelectSuccEquip(data);
        }
        else
        {
            Inventory.Instance.nowequipslot = this;
            Inventory.Instance.ShowInventoryItem(data);
        }
    }

   
}
