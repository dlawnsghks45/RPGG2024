using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class itemiconslot : MonoBehaviour
{
    public bool isguide; //성장지표용
    [SerializeField]
    private bool isequip; 
    public string id;
    [SerializeField]
    public decimal count;
    public GameObject NewItem;
    public Image ItemImage;
    public Text ItemCount;
    public Image ItemRare;
    public Animator ani;
    public GameObject[] rareeffect;
    public CanvasGroup canvas;
    private static readonly int Show = Animator.StringToHash("show");

    public void Refresh(string ID, decimal counts, bool isnew, bool isani = false,bool EQ = false)
    {
        this.id = ID;
        NewItem.SetActive(isnew);
        foreach (var t in rareeffect)
            t.SetActive(false);
        isequip = EQ;
        if (isequip)
        {
            ItemImage.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(id).Sprite);
            this.count = 1;

            ItemCount.text = "";
            ItemRare.color = Inventory.Instance.GetRareColor(EquipItemDB.Instance.Find_id(id).Rare);
            
            if (!isani) return;
            ani.SetTrigger(Show);
        }
        else
        {
            ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(id).sprite);
            this.count = counts;

//            Debug.Log("캉누트" + counts);

           
                ItemCount.text = count == 0 ? "" :  FormatNumber(counts);
            
            ItemRare.color = Inventory.Instance.GetRareColor(ItemdatabasecsvDB.Instance.Find_id(id).rare);


            if (isnew)
            {
                switch (ItemdatabasecsvDB.Instance.Find_id(id).itemiconnew)
                {
                    case "1":
                        rareeffect[0].SetActive(true);
                        break;
                    case "2":
                        rareeffect[1].SetActive(true);
                        break;
                    case "3":
                        rareeffect[2].SetActive(true);
                        break;
                }
            }

            if (!isani) return;
            ani.SetTrigger(Show);
        }
    }
    
    public void Refresh(string ID, string counts, bool isnew, bool isani = false,bool EQ = false)
    {
        this.id = ID;
        NewItem.SetActive(isnew);
        foreach (var t in rareeffect)
            t.SetActive(false);
        isequip = EQ;
        if (isequip)
        {
            ItemImage.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(id).Sprite);
            this.count = 1;

            ItemCount.text = "";
            ItemRare.color = Inventory.Instance.GetRareColor(EquipItemDB.Instance.Find_id(id).Rare);
            
            if (!isani) return;
            ani.SetTrigger(Show);
        }
        else
        {
            ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(id).sprite);

            ItemCount.text = counts == "0" ? "" : FormatNumber(decimal.Parse(counts));
            
            ItemRare.color = Inventory.Instance.GetRareColor(ItemdatabasecsvDB.Instance.Find_id(id).rare);


            if (isnew)
            {
                switch (ItemdatabasecsvDB.Instance.Find_id(id).itemiconnew)
                {
                    case "1":
                        rareeffect[0].SetActive(true);
                        break;
                    case "2":
                        rareeffect[1].SetActive(true);
                        break;
                    case "3":
                        rareeffect[2].SetActive(true);
                        break;
                }
            }

            if (!isani) return;
            ani.SetTrigger(Show);
        }
    }
    private void Awake()
    {
        if (id != "")
        {
            Refresh(id, count, false, false);
        }
    }

    public void Bt_ShowInven()
    {
        if (isequip)
        {
            if (isguide)
            {
                uimanager.Instance.AddUiview(growthmanager.Instance.panel,true);
            }
            Inventory.Instance.ShowInventoryItem_NotMine(new EquipDatabase(id));
        }
        else
        {
            Inventory.Instance.ShowInventoryItem_NoMine(id);
        }
    }
    
    
  
    static string FormatNumber(decimal num)
    {
        if (num >= 100000000) {
            return (num / 1000000M).ToString("0.#M");
        }
        if (num >= 1000000) {
            return (num / 1000000M).ToString("0.##M");
        }
        if (num >= 100000) {
            return (num / 1000M).ToString("0.#k");
        }
        if (num >= 10000) {
            return (num / 1000M).ToString("0.##k");
        }

        return num.ToString("#,0");
    }
}


