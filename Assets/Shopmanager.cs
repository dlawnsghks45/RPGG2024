using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class Shopmanager : MonoBehaviour
{
    
    private static Shopmanager _instance = null;
    public static Shopmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Shopmanager)) as Shopmanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    public UIView infopanel;
    public Image ShopInfoImage;
    public Text ShopName;
    public Text ShopPrice;
    
    
    public shopitemslot[] ShopItemSlots;
    public Text ShopInfo;

    public int timenum;
    
    
    
    [SerializeField] private Button Button;
    
    
    
    public string nowshopid;
    public shopslot nowshopslot;
    public int selecttimenum;
    public bool istimeshop;
    public void ShowShopInfos(string shopid,shopslot slot,int timenum,bool istimeshops)
    {
        nowshopid = shopid;
        nowshopslot = slot;
        selecttimenum = timenum;
        this.istimeshop = istimeshops;
        infopanel.Show(false);
        ShopDB.Row data = ShopDB.Instance.Find_ids(shopid);
        
        
        ShopInfoImage.sprite = SpriteManager.Instance.GetSprite(ShopDB.Instance.Find_ids(shopid).sprite);
        ShopPrice.text = $"{int.Parse(ShopDB.Instance.Find_ids(shopid).price):N0} KRW";
        ShopName.text = Inventory.GetTranslate(ShopDB.Instance.Find_ids(shopid).name);
        ShopInfo.text = Inventory.GetTranslate(ShopDB.Instance.Find_ids(shopid).info);

        //아이템 설정
        string[] items = data.items.Split(';');
        string[] howmany = data.howmanys.Split(';');

        foreach (var VARIABLE in ShopItemSlots)
        {
            VARIABLE.gameObject.SetActive(false);
        }

        for (int i = 0; i < items.Length; i++)
        {
            ShopItemSlots[i].SetItems(items[i], int.Parse(howmany[i]));
            ShopItemSlots[i].gameObject.SetActive(true);
        }
        //아이템 설정끝
        Button.interactable = true;

        switch (ShopDB.Instance.Find_ids(shopid).producttype)
        {
            case "one":
                Button.interactable = !Timemanager.Instance.OncePremiumPackage.Contains(shopid);
                break;
            case "daily":
                Button.interactable = Timemanager.Instance.DailyContentCount[timenum] > 0;
                break;
            
            case "weekly":
                Button.interactable = Timemanager.Instance.WeeklyContentCount[timenum] > 0;
                break;
            case "monthly":
                Button.interactable = Timemanager.Instance.MonthlyContentCount[timenum] > 0;
                break;
            case "UnLimit":
                Button.interactable = true;
                break;
        }
    }
    
    

}
