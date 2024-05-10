using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.UI;

public class iteminventoryinfoslot : MonoBehaviour
{
    public UIView panel;
    public string itemid;
    public Image ItemImage;
    public Text ItemCount;
    public Text Itemtype;
    public Text ItemName;
    public Text ItemInfo;

    public GameObject goldpanel;
    public Text goldtext;
    
    public GameObject[] Buttons;

    public ConsumeSlot Hpslot;
    public ConsumeSlot Mpslot;
    ItemdatabasecsvDB.Row item;
    public void Refresh(string ID, bool ismine = true)
    {
        if(ID.Equals(""))
            return;
        
        itemid = ID;
        item = ItemdatabasecsvDB.Instance.Find_id(ID);
        ItemImage.sprite = SpriteManager.Instance.GetSprite(item.sprite);

        if (ismine)
        {
            int index = PlayerBackendData.Instance.GetItemIndex(ID);
            if (PlayerBackendData.Instance.ItemInventory[index].Howmany > 1)
                ItemCount.text = PlayerBackendData.Instance.ItemInventory[index].Howmany.ToString("N0");
            else
                ItemCount.text = "";
        }
        else
        {
            ItemCount.text = "";
        }
        ItemName.text = Inventory.GetTranslate(item.name);
        Inventory.Instance.ChangeItemRareColor(ItemName,item.rare );
        switch (ItemdatabasecsvDB.Instance.Find_id(ID).itemsubtype)
        {
            //��ų��
            case"999":
                ItemInfo.text = $"{Inventory.GetTranslate(item.description)}\n\n\n<color=cyan>{Inventory.GetTranslate(SkillDB.Instance.Find_Id(ItemdatabasecsvDB.Instance.Find_id(ID).A).Name)}</color>\n{Inventory.GetTranslate(SkillDB.Instance.Find_Id(ItemdatabasecsvDB.Instance.Find_id(ID).A).Info)}";
                break;
            default:
                ItemInfo.text = Inventory.GetTranslate(item.description);
                break;
        }
        
        Itemtype.text =
            $"{Inventory.GetTranslate($"Inventory/{item.itemtype}")}({Inventory.GetTranslate($"Inventory/{item.itemsubtype}")})";
        panel.Show(false);

        if (item.sell != "0")
        {
            goldpanel.SetActive(true);
            goldtext.text = $"{decimal.Parse(item.sell):N0} Gold";
        }
        else
        {
            goldpanel.SetActive(false);            
        }

        foreach (var t in Buttons)
        {
            t.SetActive(false);
        }
        if (ismine)
        {
            switch (item.itemtype)
            {
                case "1": //��Ÿ ������
                    break;
                case "2": //�Һ� ������
                    switch (item.itemsubtype)
                    {
                        case "101": //ü�� ������
                            Buttons[(int)itemtypes.������].SetActive(true);
                            break;
                        case "102": //���ŷ� ������
                            Buttons[(int)itemtypes.������].SetActive(true);
                            break;
                        case "400": //���� ������
                            Buttons[(int)itemtypes.����].SetActive(true);
                            Buttons[(int)itemtypes.��ǰ����].SetActive(true);
                            break;
                        case "300": //������ �ູ 
                            Buttons[(int)itemtypes.���].SetActive(true);
                            break;
                        case "401": //������
                        case "999": //������ 
                        case "402": //������ 
                            Buttons[(int)itemtypes.���].SetActive(true);
                            break;
                        case "403": //����
                            Buttons[(int)itemtypes.���].SetActive(true);
                            break;
                        case "405": //����
                            Buttons[(int)itemtypes.���].SetActive(true);
                            break;
                        case "406": //��
                            Buttons[(int)itemtypes.���].SetActive(true);
                            break;
                    }
                    break;
                case "3": //��Ÿ      
                    break;
                case "4": //����ǰ.
                    break;
            }
            

            if(item.IsCrafting == "TRUE")
            {
                Buttons[(int)itemtypes.���۰˻�].SetActive(true);
            }
            if(item.sell != "0")
            {
                Buttons[(int)itemtypes.�Ǹ�].SetActive(true);
            }
        }
        else
        {
            switch (item.itemtype)
            {
                case "2": //�Һ� ������
                    switch (item.itemsubtype)
                    {
                   
                        case "400": //���� ������
                            Buttons[(int)itemtypes.��ǰ����].SetActive(true);
                            break;
                    }
                    break;
                case "3": //��Ÿ      
                    break;
                case "4": //����ǰ.
                    break;
            }
        }
    }

    public void Bt_UseQuickSlot()
    {
        Debug.Log(item.itemsubtype);
        switch (item.itemsubtype)
        {
            
            case "101": //ü�� ������
                Hpslot.SetPotion(itemid);
                Hpslot.Refresh();
                Savemanager.Instance.SaveHpQuickSlot();
                break;
            case "102": //���ŷ� ������
                Mpslot.SetPotion(itemid);
                Mpslot.Refresh();
                Savemanager.Instance.SaveMpQuickSlot();
                break;
        }
        
        //���̵� ����Ʈ
        Tutorialmanager.Instance.CheckTutorial("equippotion");
    }

    enum itemtypes
    {
        ���,
        ����,
        ������,
        �Ǹ�,
        ���۰˻�,
        ��ǰ����,
    }


   

}
