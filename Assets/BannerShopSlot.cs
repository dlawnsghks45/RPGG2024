
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BannerShopSlot : MonoBehaviour
{
    public string shopid;
    public TMP_Text ProductName;
    public int buycountarraynum;

    public itemiconslot[] RewardIcon;
    private void Start()
    {
        Refresh();
    }

    void Refresh()
    {
        ShopDB.Row shopdata = ShopDB.Instance.Find_ids(shopid);

        string[] id = shopdata.items.Split(';');
        string[] hw = shopdata.howmanys.Split(';');


        foreach (var VARIABLE in RewardIcon)
        {
            VARIABLE.gameObject.SetActive(false);
        }

        for (int i = 0; i < id.Length; i++)
        {
            RewardIcon[i].Refresh(id[i], hw[i], false);
            RewardIcon[i].gameObject.SetActive(true);
        }
    }
    public void Bt_ShowShopInfo()
    {
        Shopmanager.Instance.timenum = buycountarraynum;
        Shopmanager.Instance.ShowShopInfos(shopid);
    }
    
    

}
