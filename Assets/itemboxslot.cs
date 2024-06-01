using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class itemboxslot : MonoBehaviour
{
    public Image ItemSprite;
    public Text ItemName;
    public GameObject Recoobj;
    private bool isequip;
    public string id;
    public int itemcount;
    public UIToggle ChoiceToggle;
    public void SetData(string ID, int howmany, bool ISEQUIP,string BoxType,bool ismine)
    {
        Recoobj.SetActive(false);

        this.isequip = ISEQUIP;
        this.id = ID;
        itemcount = howmany;
        
        ChoiceToggle.IsOn = false;
        ChoiceToggle.gameObject.SetActive(false);
        ChoiceToggle.Interactable = true;
        
        if (isequip)
        {
            ItemSprite.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(id).Sprite);
            ItemName.text =  Inventory.GetTranslate(EquipItemDB.Instance.Find_id(id).Name);
            Inventory.Instance.ChangeItemRareColor(ItemName,EquipItemDB.Instance.Find_id(id).Rare);
        }
        else
        {
            if (ItemdatabasecsvDB.Instance.Find_id(id).Recotype == EquipItemDB.Instance
                    .Find_id(PlayerBackendData.Instance.EquipEquiptment0[0].Itemid).SubType
                || ItemdatabasecsvDB.Instance.Find_id(id).Recotype == "all")
            {
                Recoobj.SetActive(true);
            }
//            Debug.Log(id);
            ItemSprite.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(id).sprite);
            ItemName.text =  $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(id).name)} x {itemcount.ToString()}";
            Inventory.Instance.ChangeItemRareColor(ItemName,ItemdatabasecsvDB.Instance.Find_id(id).rare);
        }

        switch (BoxType)
        {
            case "All":
                break;
            case "Choice":
                if (ismine)
                    ChoiceToggle.gameObject.SetActive(true);
                break;
            case "Random":
                break;
        }

    }

    public void Bt_ToggleSelect()
    {
        Inventory.Instance.Box_SelectItem();
    }
    public void Bt_ToggleUnSelect()
    {
        Inventory.Instance.Box_UnSelectItem();
    }
    
    
    public void settoggleoff()
    {
        if (!ChoiceToggle.IsOn)
            ChoiceToggle.Interactable = false;
    }
    
    public void settoggleon()
    {
        ChoiceToggle.Interactable = true;
    }
    public void Bt_ItemShow()
    {
        if (isequip)
        {
            Inventory.Instance.ShowInventoryItem_NotMine(new EquipDatabase(id));
        }
        else
        {
            Inventory.Instance.ShowInventoryItem_NoMine(id);
        }
    }
}
