using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Itemsell : MonoBehaviour
{
    public Image ItemImage;
    public string itemid;

    private decimal totalcost;
    private int sellcount = 0;
    private decimal itemcost = 0;
    string count; //입력한 개수 스트링

    public void SetSell(string id)
    {
        ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(id).sprite);
        itemid = id;

        //가격
        itemcost = decimal.Parse(ItemdatabasecsvDB.Instance.Find_id(id).sell);
        itemcostText.text = $"{itemcost:N0} Gold";

        count = "0";
        
        sellcount = 0; //처음은 0으로 잡는다.
        sellcountText.text = sellcount.ToString();

        totalcost = 0;
        TotalcostText.text = totalcost.ToString(CultureInfo.InvariantCulture);


        RefreshData();
    }

    void RefreshData()
    {
        if (PlayerBackendData.Instance.CheckItemCount(itemid) < int.Parse(count))
        {
            count = PlayerBackendData.Instance.CheckItemCount(itemid).ToString();
            sellcount = PlayerBackendData.Instance.CheckItemCount(itemid);
        }
        else
        {
            sellcount = int.Parse(count);
        }

        sellcountText.text = sellcount.ToString("N0");

        totalcost = itemcost * sellcount;
        TotalcostText.text = $"{totalcost:N0} Gold";
    }


    public Text TotalcostText; //판매가격 x 판매개수
    public Text itemcostText; //아이템판매가격
    public Text sellcountText; //판매개수


    public void Bt_RemoveText()
    {
        count = count[..^1];

        if (count == "")
            count = "0";

        RefreshData();
    }

    public void Bt_MaxCount()
    {
        int max = 2000000000;
        if (PlayerBackendData.Instance.CheckItemCount(itemid) >= max)
            max = PlayerBackendData.Instance.CheckItemCount(itemid);
        
        count = max.ToString();
        
        RefreshData();
    }

    //0~9입력용
    public void Bt_ButtonNumber(string num)
    {
        count = count + num;

        RefreshData();
    }

    public void Bt_SellItem()
    {
        PlayerData.Instance.UpGold(totalcost);
        PlayerBackendData.Instance.RemoveItem(itemid, sellcount);
        Inventory.Instance.RefreshInventory();
        Inventory.Instance.SellPanel.Hide(true);
        Inventory.Instance.iteminfopanel.panel.Hide(true);
        Savemanager.Instance.SaveInventory_SaveOn();
        LogManager.SellLog(itemid, sellcount);

    }
}
