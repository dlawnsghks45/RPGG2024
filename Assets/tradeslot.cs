using System;
using UnityEngine;
using UnityEngine.UI;

public class tradeslot : MonoBehaviour
{
    [SerializeField]
    public string tradeid; //교환 아이디
    public Image Sprite;

    public Button BuyButton;

    public string Needid; //필요아디ㅣ
    public itemneedslot needitems;
    public Text Titlename;
    public Text GiveCountText;


    public int buycountarraynum;
    public GameObject buycountobj;
    public Text BuyCountText;


    private string giveid;
    private string givehowmany;

    public GameObject Lock;
    public Text LockText;
    
    private void Start()
    {
        
        if (tradeid == "")
            return;
        
        uimanager.Instance.TradeSlots.Add(this);
        
        
        buycountarraynum = TradeShopDB.Instance.Find_id(tradeid).buytype switch
        {
            "daily" => (int)Enum.Parse(typeof(Timemanager.ContentEnumDaily),
                TradeShopDB.Instance.Find_id(tradeid).arrynum),
            "weekly" => (int)Enum.Parse(typeof(Timemanager.ContentEnumWeekly),
                TradeShopDB.Instance.Find_id(tradeid).arrynum),
            "one" => 1,
            _ => buycountarraynum
        };

        giveid = TradeShopDB.Instance.Find_id(tradeid).giveid;
         givehowmany = TradeShopDB.Instance.Find_id(tradeid).givehowmany;

        Titlename.text =  $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(giveid).name)}";
        Inventory.Instance.ChangeItemRareColor(Titlename, ItemdatabasecsvDB.Instance.Find_id(giveid).rare);
        
        GiveCountText.text = givehowmany;
        Sprite.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(giveid).sprite);
        needitems.Refresh(TradeShopDB.Instance.Find_id(tradeid).needid, int.Parse(TradeShopDB.Instance.Find_id(tradeid).needhowmany));

        RefreshBuyCount();
    }

    private void OnEnable()
    {
        RefreshBuyCount();
        CheckLock();
    }

    void CheckLock()
    {
        int locknum = int.Parse(TradeShopDB.Instance.Find_id(tradeid).buyrank);

        if (PlayerBackendData.Instance.GetAdLv() >= locknum)
        {
            Lock.SetActive(false);
        }
        else
        {
            Lock.SetActive(true);
            LockText.text = string.Format(Inventory.GetTranslate("ButtonUI/제작잠금"),PlayerData.Instance.gettierstar(locknum.ToString()));
        }
    }
   public void RefreshBuyCount()
    {
        switch (TradeShopDB.Instance.Find_id(tradeid).buytype)
        {
            case "one":
                buycountobj.SetActive(true);
                BuyCountText.text =
                    string.Format(Inventory.GetTranslate("Shop/가능횟수"),
                        Timemanager.Instance.OnceTradePackage.Contains(tradeid) ? 0 : 1
                        ,1);
                
                if (Timemanager.Instance.OnceTradePackage.Contains(tradeid))
                {
                    BuyCountText.color = Color.red;
                    transform.SetAsLastSibling();
                    BuyButton.interactable = false;
                    
                }
                else
                {
                    BuyButton.interactable = true;
                    BuyCountText.color = Color.cyan;
                }
                break;
            
            case "daily":
                buycountobj.SetActive(true);
                BuyCountText.text =
                    string.Format(Inventory.GetTranslate("Trade/일가능횟수"),
                        Timemanager.Instance.DailyContentCount[buycountarraynum]
                        , Timemanager.Instance.DailyContentCount_Standard[buycountarraynum]);
                
//                Debug.Log( Timemanager.Instance.DailyContentCount_Standard[buycountarraynum]+ "카운트" + buycountarraynum);

                if (Timemanager.Instance.DailyContentCount[buycountarraynum] == 0)
                {
                    BuyButton.interactable = false;
                    BuyCountText.color = Color.red;
                    transform.SetAsLastSibling();

                }
                else
                {
                    BuyButton.interactable = true;
                    BuyCountText.color = Color.cyan;
                }
                break;
            
            case "weekly":
                buycountobj.SetActive(true);
                BuyCountText.text =
                    string.Format(Inventory.GetTranslate("Trade/주가능횟수"),
                        Timemanager.Instance.WeeklyContentCount[buycountarraynum]
                        , Timemanager.Instance.WeeklyContentCount_Standard[buycountarraynum]);
                if (Timemanager.Instance.WeeklyContentCount[buycountarraynum] == 0)
                {
                    BuyButton.interactable = false;
                    BuyCountText.color = Color.red;
                    transform.SetAsLastSibling();

                }
                else
                {
                    BuyButton.interactable = true;
                    BuyCountText.color = Color.cyan;
                }
                break;
            case "UnLimit":
                buycountobj.SetActive(false);
                break;
        }
    }

   public void Bt_ShowItem()
   {
       Inventory.Instance.ShowInventoryItem_NoMine(giveid);
           
   }
    public void Bt_BuyItem()
    {
        Inventory.Instance.TradeItem(tradeid,this);
    }
}
