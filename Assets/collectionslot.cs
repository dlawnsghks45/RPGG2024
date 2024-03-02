using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class collectionslot : MonoBehaviour
{
    public GameObject FinishObj;
    public Image ItemImage;
    public Image ColorTeduri;
    public GameObject Noti;
    public bool isequip;

    public string itemid;
    public int curhowmany;
    public int maxhowmany;
    public bool isfinish;
    public Text MaxcountText;
    public void Refresh(string id,int curcount,int maxcount,bool isfinish, bool isequip)
    {
        this.isequip = isequip;
        Noti.SetActive(false);

        itemid = id;
        this.curhowmany = curcount;
        this.maxhowmany = maxcount;
        if (maxcount != 1)
            MaxcountText.text = maxcount.ToString();
        else
        {
            MaxcountText.text = "";
        }
        this.isfinish = isfinish;

        if (isequip)
        {
            ItemImage.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(
                id).Sprite);
        }
        else
        {
            ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(
                id).sprite);
        }
        RefreshNum();
    }


   public void RefreshNum()
    {
        if (isfinish)
        {
            FinishObj.SetActive(true);
            Noti.SetActive(false);
            ColorTeduri.color = Color.cyan;
            return;
        }
        else
        {
            FinishObj.SetActive(false);
        }
        
        if (isequip)
        {
            //장비라면
            EquipItemDB.Row eqdata = EquipItemDB.Instance.Find_id(itemid);

                ColorTeduri.color = Color.gray;
                Noti.SetActive(false);

            foreach (var VARIABLE in PlayerBackendData.Instance.GetTypeEquipment(eqdata.Type).Where(VARIABLE => VARIABLE.Value.Itemid.Equals(itemid)))
            {
                Collectmanager.Instance.AllCollectButton.Interactable = true;
                Noti.SetActive(true);
                ColorTeduri.color = Color.white;
            }
            
        }
        else
        {
            //아이템이라
            if (PlayerBackendData.Instance.CheckItemCount(itemid) >= maxhowmany)
            {
                Collectmanager.Instance.AllCollectButton.Interactable = true;
                Noti.SetActive(true);
                ColorTeduri.color = Color.white;
            }
            else
            {
                ColorTeduri.color = Color.gray;
                Noti.SetActive(false);
            }
            //완
        }
        
    }

    private void OnEnable()
    {
        RefreshNum();
    }

    public void Bt_ShowwPanel()
    {
        if (isequip)
        {
            Inventory.Instance.ShowInventoryItem_NotMine(new EquipDatabase(itemid));   
            uimanager.Instance.AddUiview(Collectmanager.Instance.CollectPanel,true);
        }
        else
        {
            Inventory.Instance.ShowInventoryItem_NoMine(itemid);
        }
    }
    
}




