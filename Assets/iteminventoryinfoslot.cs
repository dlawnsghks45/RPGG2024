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
            //스킬북
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
                case "1": //기타 아이템
                    break;
                case "2": //소비 아이템
                    switch (item.itemsubtype)
                    {
                        case "101": //체력 아이템
                            Buttons[(int)itemtypes.퀵슬릇].SetActive(true);
                            break;
                        case "102": //정신력 아이템
                            Buttons[(int)itemtypes.퀵슬릇].SetActive(true);
                            break;
                        case "400": //상자 아이템
                            Buttons[(int)itemtypes.열기].SetActive(true);
                            Buttons[(int)itemtypes.물품보기].SetActive(true);
                            break;
                        case "300": //엘리의 축복 
                            Buttons[(int)itemtypes.사용].SetActive(true);
                            break;
                        case "401": //충전권
                        case "999": //충전권 
                        case "402": //충전권 
                            Buttons[(int)itemtypes.사용].SetActive(true);
                            break;
                        case "403": //위장
                            Buttons[(int)itemtypes.사용].SetActive(true);
                            break;
                        case "405": //위장
                            Buttons[(int)itemtypes.사용].SetActive(true);
                            break;
                        case "406": //펫
                            Buttons[(int)itemtypes.사용].SetActive(true);
                            break;
                    }
                    break;
                case "3": //기타      
                    break;
                case "4": //수집품.
                    break;
            }
            

            if(item.IsCrafting == "TRUE")
            {
                Buttons[(int)itemtypes.제작검색].SetActive(true);
            }
            if(item.sell != "0")
            {
                Buttons[(int)itemtypes.판매].SetActive(true);
            }
        }
        else
        {
            switch (item.itemtype)
            {
                case "2": //소비 아이템
                    switch (item.itemsubtype)
                    {
                   
                        case "400": //상자 아이템
                            Buttons[(int)itemtypes.물품보기].SetActive(true);
                            break;
                    }
                    break;
                case "3": //기타      
                    break;
                case "4": //수집품.
                    break;
            }
        }
    }

    public void Bt_UseQuickSlot()
    {
        Debug.Log(item.itemsubtype);
        switch (item.itemsubtype)
        {
            
            case "101": //체력 아이템
                Hpslot.SetPotion(itemid);
                Hpslot.Refresh();
                Savemanager.Instance.SaveHpQuickSlot();
                break;
            case "102": //정신력 아이템
                Mpslot.SetPotion(itemid);
                Mpslot.Refresh();
                Savemanager.Instance.SaveMpQuickSlot();
                break;
        }
        
        //가이드 퀘스트
        Tutorialmanager.Instance.CheckTutorial("equippotion");
    }

    enum itemtypes
    {
        사용,
        열기,
        퀵슬릇,
        판매,
        제작검색,
        물품보기,
    }


   

}
