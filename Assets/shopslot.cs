using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class shopslot : MonoBehaviour
{
    public int shoptimenum;
    public bool istimeshop;
    public string shopid;
    public Image Sprite;
    public Text CostText;
    public Text ProductName;

    public int buycountarraynum;
    public GameObject buycountobj;
    public Text BuyCountText;

    public GameObject IsNew;
    
    private void Start()
    {
        RefreshAll();
    }

    public void RefreshTimeshop(int num,string id)
    {
        shoptimenum = num;
        shopid = id;
        RefreshAll();
    }
    public void RefreshAll()
    {
        if(shopid == "")
            return;

        switch (ShopDB.Instance.Find_ids(shopid).producttype)
        {
            case "daily":
                buycountarraynum = (int)Enum.Parse(typeof(Timemanager.ContentEnumDaily),
                    ShopDB.Instance.Find_ids(shopid).enumname);
                break;
            case "weekly":
                buycountarraynum = (int)Enum.Parse(typeof(Timemanager.ContentEnumWeekly),
                    ShopDB.Instance.Find_ids(shopid).enumname);
                break;
            case "monthly":
                buycountarraynum = (int)Enum.Parse(typeof(Timemanager.ContentEnumMonthly),
                    ShopDB.Instance.Find_ids(shopid).enumname);
                break;
            default:
                buycountarraynum = buycountarraynum;
                break;
        }
        IsNew.SetActive(bool.Parse(ShopDB.Instance.Find_ids(shopid).isnew));
            
        Sprite.sprite = SpriteManager.Instance.GetSprite(ShopDB.Instance.Find_ids(shopid).sprite);
       
        CostText.text = $"{int.Parse(ShopDB.Instance.Find_ids(shopid).price):N0} KRW";
     
        ProductName.text = Inventory.GetTranslate(ShopDB.Instance.Find_ids(shopid).name);
        RefreshBuyCount();
    }


    private void OnEnable()
    {
        if (!istimeshop)
        {
            //구매횟수 보이기
            RefreshBuyCount();
        }
        else
        {
            TimeSpan dateDiff = PlayerBackendData.Instance.PlayerShopTimes[shoptimenum] - Timemanager.Instance.NowTime;
            nowsecond = dateDiff.TotalSeconds;
//            Debug.Log(nowsecond);
            dt = new DateTime(dateDiff.Ticks);
            dt.AddSeconds(nowsecond);

            StartCoroutine(TimeStart());
            //남은 시간 보이기
            Refresh();
        }
    }
    
    public double nowsecond;
    DateTime dt;
    private bool isfinish;
    WaitForSeconds wait = new WaitForSeconds(1f);
    //시간제용
    IEnumerator TimeStart()
    {
        while (!isfinish)
        {
            yield return wait;
            nowsecond--;
            if (nowsecond <= 0)
            {
                isfinish = true;
                Refresh();
            }
            else
            {
                dt = dt.AddSeconds(-1);
                Refresh();
            }
        }
    }
    public void Refresh()
    {
        if (isfinish)
        {
            BuyCountText.text = "00:00:00:00";
        }
        else
        {
            BuyCountText.text = string.Format(Inventory.GetTranslate("UI6/남은 구매 가능 시간"), dt.ToString("dd:HH:mm:ss"));
        }
    }
    
    
    public void RefreshBuyCount()
    {
//        Debug.Log(ShopDB.Instance.Find_ids(shopid).producttype);
        switch (ShopDB.Instance.Find_ids(shopid).producttype)
        {
            case "one":
                buycountobj.SetActive(true);
                BuyCountText.text =
                    string.Format(Inventory.GetTranslate("Shop/가능횟수"),
                        Timemanager.Instance.OncePremiumPackage.Contains(shopid) ? 0 : 1
                        ,1);
                
                if (Timemanager.Instance.OncePremiumPackage.Contains(shopid))
                {
                    BuyCountText.color = Color.red;
                    transform.SetAsLastSibling();
                }
                else
                {
                    BuyCountText.color = Color.cyan;
                }
                break;
            case "daily":
                buycountobj.SetActive(true);
                BuyCountText.text =
                    string.Format(Inventory.GetTranslate("Shop/일가능횟수"),
                        Timemanager.Instance.DailyContentCount[buycountarraynum]
                        , Timemanager.Instance.DailyContentCount_Standard[buycountarraynum]);
                
//                Debug.Log( Timemanager.Instance.DailyContentCount_Standard[buycountarraynum]+ "카운트" + buycountarraynum);

                if (Timemanager.Instance.DailyContentCount[buycountarraynum] == 0)
                {
                    BuyCountText.color = Color.red;
                    transform.SetAsLastSibling();
                }
                else
                {
                    BuyCountText.color = Color.cyan;
                }
                break;
            
            case "weekly":
                buycountobj.SetActive(true);
                BuyCountText.text =
                    string.Format(Inventory.GetTranslate("Shop/주가능횟수"),
                        Timemanager.Instance.WeeklyContentCount[buycountarraynum]
                        , Timemanager.Instance.WeeklyContentCount_Standard[buycountarraynum]);
                if (Timemanager.Instance.WeeklyContentCount[buycountarraynum] == 0)
                {
                    BuyCountText.color = Color.red;
                    transform.SetAsLastSibling();
                }
                else
                {
                    BuyCountText.color = Color.cyan;
                }
                break;
            case "monthly":
                buycountobj.SetActive(true);
                BuyCountText.text =
                    string.Format(Inventory.GetTranslate("Shop/월가능횟수"),
                        Timemanager.Instance.MonthlyContentCount[buycountarraynum]
                        , Timemanager.Instance.MonthlyContentCount_Standard[buycountarraynum]);
                if (Timemanager.Instance.MonthlyContentCount[buycountarraynum] == 0)
                {
                    BuyCountText.color = Color.red;
                    transform.SetAsLastSibling();
                }
                else
                {
                    BuyCountText.color = Color.cyan;
                }
                break;
            case "UnLimit":
                buycountobj.SetActive(false);
                break;
        }
    }


    public void ShowShopInfo()
    {
        Shopmanager.Instance.timenum = buycountarraynum;
        Shopmanager.Instance.ShowShopInfos(shopid,this,shoptimenum,istimeshop);
    }
    
    

}

