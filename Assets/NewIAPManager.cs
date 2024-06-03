using System;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using EasyMobile.Scripts.Modules.InAppPurchasing;
#if EM_UIAP
using UnityEngine.Purchasing.Security;
#endif
public class NewIAPManager : MonoBehaviour
{
    //싱글톤생성
    private static NewIAPManager _instance = null;
    public static NewIAPManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(NewIAPManager)) as NewIAPManager;

                if (_instance == null)
                {
                    //   Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    public static bool isInitialized;
    // Start is called before the first frame update
    private void Awake()
    {
        if (!RuntimeManager.IsInitialized())
            RuntimeManager.Init();
    }


    void Start()
    {
        isInitialized = InAppPurchasing.IsInitialized();
    }

    // Subscribe to IAP purchase events
    void OnEnable()
    {
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    // Unsubscribe when the game object is disabled
    void OnDisable()
    {
        InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;
    }
    // Read the GooglePlay receipt of the sample product on Android.
    // Receipt validation is required.
    bool ReadGooglePlayReceipt(IAPProduct product)
    {
#if EM_UIAP
        if (Application.platform == RuntimePlatform.Android )
       {
           // EM_IAPConstants.Sample_Product is the generated name constant of a product named "Sample Product".
        GooglePlayReceipt receipt = InAppPurchasing.GetGooglePlayReceipt(product.Name);

           if (receipt != null)
           {
               Debug.Log("Package Name: " + receipt.packageName);
               Debug.Log("Product ID: " + receipt.productID);
               Debug.Log("Purchase Date: " + receipt.purchaseDate.ToShortDateString());
               Debug.Log("Purchase State: " + receipt.purchaseState.ToString());
               Debug.Log("Transaction ID: " + receipt.transactionID);
               Debug.Log("Purchase Token: " + receipt.purchaseToken);
               return true;
           }
           else
           {
               //Debug.Log("영수증이 없다");
               return false;
           }
       }
#endif
        //Debug.Log("안드로이드가 아냐!");
        return false;
    }
#region 인앱구매


    
    public void BtBuyProduct()
    {
//        Debug.Log(ShopDB.Instance.Find_ids(Shopmanager.Instance.nowshopid).productid);
        InAppPurchasing.Purchase(ShopDB.Instance.Find_ids(Shopmanager.Instance.nowshopid).productid);
    }

    #endregion
    // Successful purchase handler
    public void PurchaseCompletedHandler(IAPProduct product)
    {
     List<string> id = new List<string>();
     List<string> howmany = new List<string>();
        
        if (ReadGooglePlayReceipt(product) || Application.platform == RuntimePlatform.WindowsEditor)
        {
            ShopDB.Row data = ShopDB.Instance.Find_ids(Shopmanager.Instance.nowshopid);
            string[] ids = null;
            string[] howmanys = null;
            if (product.Id != "rpgg2.package.seasonpass1" && product.Id != "rpgg2.growthguide.premium")
            {
               ids = data.items.Split(';');
                howmanys = data.howmanys.Split(';');
//            Debug.Log(data.producttype);
            switch (data.producttype)
            {
                case "one":
                    Timemanager.Instance.OncePremiumPackage.Add(data.ids);
                    Shopmanager.Instance.nowshopslot.RefreshBuyCount();
                    Savemanager.Instance.SaveShopData();
                    //저장
                    break;
                case "daily":
                    Timemanager.Instance.ConSumeCount_DailyAscny(Shopmanager.Instance.timenum, 1);
                    Shopmanager.Instance.nowshopslot.RefreshBuyCount();
                    break;
                case "weekly":
                   // Debug.Log("너무");
                    Timemanager.Instance.ConSumeCount_WeeklyAscny(Shopmanager.Instance.timenum, 1);
                    Shopmanager.Instance.nowshopslot.RefreshBuyCount();
                    break;
                case "monthly":
                 //   Debug.Log("너무");
                    Timemanager.Instance.ConSumeCount_MonthlyAscny(Shopmanager.Instance.timenum, 1);
                    Shopmanager.Instance.nowshopslot.RefreshBuyCount();
                    break;
            }
                Shopmanager.Instance.infopanel.Hide(false);
                LogManager.InsertIAPBuyProduct(Shopmanager.Instance.nowshopid);
            }

            if (Shopmanager.Instance.istimeshop)
            {
                PlayerBackendData.Instance.PlayerShopTimesbuys[Shopmanager.Instance.selecttimenum] = true;
                Levelshop.Instance.SaveTime();
                Shopmanager.Instance.istimeshop = false;
                Levelshop.Instance.ShowPanel();
                //Levelshop.Instance.NewProductButton.ExecuteClick(false);
            }
            
            
            // Compare product name to the generated name constants to determine which product was bought
            switch (product.Name)
            {
                case EM_IAPConstants.Product_rpgg2_ads_free:
                    PlayerBackendData.Instance.isadsfree = true;
                    Savemanager.Instance.SaveadsFree();
                    Inventory.Instance.Refreshadsfree();
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/광고활성화"), alertmanager.alertenum.일반);
                    break;
                case EM_IAPConstants.Product_rpgg2_ellibless15:
                case EM_IAPConstants.Product_rpgg2_ellibless30:
                case EM_IAPConstants.Product_rpgg2_equip_package:
                case EM_IAPConstants.Product_rpgg2_dungeonenter10:
                case EM_IAPConstants.Product_rpgg2_fire_4000:
                case EM_IAPConstants.Product_rpgg2_fire_8500:
                case EM_IAPConstants.Product_rpgg2_fire_25500:
                case EM_IAPConstants.Product_rpgg2_fire_45000:
                case EM_IAPConstants.Product_rpgg2_fire_95000:
                case EM_IAPConstants.Product_rpgg2_package_1100:
                case EM_IAPConstants.Product_rpgg2_package_5500:
                case EM_IAPConstants.Product_rpgg2_package_11000:
                case EM_IAPConstants.Product_rpgg2_package_33000:
                case EM_IAPConstants.Product_rpgg2_smelt_package:
                case EM_IAPConstants.Product_rpgg2_class1:
                case EM_IAPConstants.Product_rpgg2_class2:
                case EM_IAPConstants.Product_rpgg2_class3:
                case EM_IAPConstants.Product_rpgg2_class4:
                case EM_IAPConstants.Product_rpgg2_class5:
                case EM_IAPConstants.Product_rpgg2_class6:
                case EM_IAPConstants.Product_rpgg2_equip_package1:
                case EM_IAPConstants.Product_rpgg2_equip_package2:
                case EM_IAPConstants.Product_rpgg2_equip_package3:
                case EM_IAPConstants.Product_rpgg2_dungeonenter200:
                case EM_IAPConstants.Product_rpgg2_fire_40002:
                case EM_IAPConstants.Product_rpgg2_fire_85002:
                case EM_IAPConstants.Product_rpgg2_fire_255002:
                case EM_IAPConstants.Product_rpgg2_fire_450002:
                case EM_IAPConstants.Product_rpgg2_fire_950002:
                case EM_IAPConstants.Product_rpgg2_equip_packagew1:
                case EM_IAPConstants.Product_rpgg2_equip_packagew2:
                case EM_IAPConstants.Product_rpgg2_equip_packagew3:
                case EM_IAPConstants.Product_rpgg2_equip_packagew12:
                case EM_IAPConstants.Product_rpgg2_equip_packagew22:
                case EM_IAPConstants.Product_rpgg2_equip_packagew32:
                case EM_IAPConstants.Product_rpgg2_consume_raid1:
                case EM_IAPConstants.Product_rpgg2_consume_raid2:
                case EM_IAPConstants.Product_rpgg2_all_package1:
                case EM_IAPConstants.Product_rpgg2_all_package2:
                case EM_IAPConstants.Product_rpgg2_altar_package1:
                case EM_IAPConstants.Product_rpgg2_altar_package2:
                case EM_IAPConstants.Product_rpgg2_resource_package1:
                case EM_IAPConstants.Product_rpgg2_package_skill1:
                case EM_IAPConstants.Product_rpgg2_package_skill2:
                case EM_IAPConstants.Product_rpgg2_package_skill3:
                case EM_IAPConstants.Product_rpgg2_package_content1:
                case EM_IAPConstants.Product_rpgg2_package_content2:
                case EM_IAPConstants.Product_rpgg2_package_etc1:
                case EM_IAPConstants.Product_rpgg2_package_etc2:
                case EM_IAPConstants.Product_rpgg2_package_advan1:
                case EM_IAPConstants.Product_rpgg2_roulette_01:
                case EM_IAPConstants.Product_rpgg2_roulette_02:
                case EM_IAPConstants.Product_rpgg2_roulette_03:
                case EM_IAPConstants.Product_rpgg2_roulette_04:
                case EM_IAPConstants.Product_rpgg2_roulette_05:
                case EM_IAPConstants.Product_rpgg2_roulette_06:
                case EM_IAPConstants.Product_rpgg2_roulette_07:
                case EM_IAPConstants.Product_rpgg2_roulette_08:
                case EM_IAPConstants.Product_rpgg2_roulette_09:
                    /*
                case EM_IAPConstants.Product_rpgg2_package2023_bf1:
                case EM_IAPConstants.Product_rpgg2_package2023_bf2:
                case EM_IAPConstants.Product_rpgg2_package2023_bf3:
                case EM_IAPConstants.Product_rpgg2_package2023_bf4:
                case EM_IAPConstants.Product_rpgg2_package2023_bf5:
                case EM_IAPConstants.Product_rpgg2_package2023_bf6:
                case EM_IAPConstants.Product_rpgg2_package2023_bf7:
                case EM_IAPConstants.Product_rpgg2_package2023_bf8:
                case EM_IAPConstants.Product_rpgg2_package2023_bf9:
                case EM_IAPConstants.Product_rpgg2_package2023_bf10:
                case EM_IAPConstants.Product_rpgg2_package2023_bf11:
                case EM_IAPConstants.Product_rpgg2_package2023_bf12:
                case EM_IAPConstants.Product_rpgg2_package2023_bf13:
                case EM_IAPConstants.Product_rpgg2_package2023_bf14:
                case EM_IAPConstants.Product_rpgg2_package2023_bf15:
                    */
                case EM_IAPConstants.Product_rpgg2_packagetime_1:
                case EM_IAPConstants.Product_rpgg2_packagetime_2:
                case EM_IAPConstants.Product_rpgg2_packagetime_3:
                case EM_IAPConstants.Product_rpgg2_packagetime_4:
                case EM_IAPConstants.Product_rpgg2_packagetime_5:
                case EM_IAPConstants.Product_rpgg2_packagetime_6:
                case EM_IAPConstants.Product_rpgg2_packagetime_7:
                case EM_IAPConstants.Product_rpgg2_packagetime_8:
                case EM_IAPConstants.Product_rpgg2_packagetime_9:
                case EM_IAPConstants.Product_rpgg2_packagetime_10:
                case EM_IAPConstants.Product_rpgg2_packagetime_11:
                case EM_IAPConstants.Product_rpgg2_packagetime_12:
                case EM_IAPConstants.Product_rpgg2_packagetime_13:
                case EM_IAPConstants.Product_rpgg2_packagetime_14:
                case EM_IAPConstants.Product_rpgg2_packagetime_15:
                case EM_IAPConstants.Product_rpgg2_packagetime_16:
                case EM_IAPConstants.Product_rpgg2_packagetime_17:
                case EM_IAPConstants.Product_rpgg2_packagetime_18:
                case EM_IAPConstants.Product_rpgg2_packagetime_19:
                case EM_IAPConstants.Product_rpgg2_packagetime_20:
                case EM_IAPConstants.Product_rpgg2_packagetime_21:
                case EM_IAPConstants.Product_rpgg2_packagetime_22:
                case EM_IAPConstants.Product_rpgg2_packagetime_23:
                case EM_IAPConstants.Product_rpgg2_packagetime_24:
                case EM_IAPConstants.Product_rpgg2_packagetime_25:
                case EM_IAPConstants.Product_rpgg2_packagetime_26:
                case EM_IAPConstants.Product_rpgg2_packagetime_27:
                case EM_IAPConstants.Product_rpgg2_package2023_ch1:
                case EM_IAPConstants.Product_rpgg2_package2023_ch2:
                case EM_IAPConstants.Product_rpgg2_package2023_ch3:
                case EM_IAPConstants.Product_rpgg2_package2023_ch4:
                case EM_IAPConstants.Product_rpgg2_package2024_ny1:
                case EM_IAPConstants.Product_rpgg2_package2024_ny2:
                case EM_IAPConstants.Product_rpgg2_package2024_ny3:
                case EM_IAPConstants.Product_rpgg2_package2024_ny4:
                case EM_IAPConstants.Product_rpgg2_package2024_fire1:
                case EM_IAPConstants.Product_rpgg2_package2024_fire2:
                case EM_IAPConstants.Product_rpgg2_package2024_fire3:
                case EM_IAPConstants.Product_rpgg2_package2024_fire4:
                case EM_IAPConstants.Product_rpgg2_package2024_party1:
                case EM_IAPConstants.Product_rpgg2_package2024_party2:
                case EM_IAPConstants.Product_rpgg2_package2024_party3:
                case EM_IAPConstants.Product_rpgg2_package_up_1:
                case EM_IAPConstants.Product_rpgg2_package_pet_1:
                case EM_IAPConstants.Product_rpgg2_package_pet_2:
                case EM_IAPConstants.Product_rpgg2_package_pet_3:
                case EM_IAPConstants.Product_rpgg2_package_pet_4:
                case EM_IAPConstants.Product_rpgg2_package_pet_5:
                case EM_IAPConstants.Product_rpgg2_package2024_event1:
                case EM_IAPConstants.Product_rpgg2_package2024_event2:
                case EM_IAPConstants.Product_rpgg2_package2024_event3:
                case EM_IAPConstants.Product_rpgg2_package2024_event4:
                case EM_IAPConstants.Product_rpgg2_package2024_event5:
                case EM_IAPConstants.Product_rpgg2_class7:
                    switch (product.Name)
                    {
                        case EM_IAPConstants.Product_rpgg2_packagetime_1:
                            Levelshop.Instance.GiveTime(1, 2);
                            break;
                        case EM_IAPConstants.Product_rpgg2_packagetime_2:
                            Levelshop.Instance.GiveTime(2, 1);
                            break;
                   
                        case EM_IAPConstants.Product_rpgg2_packagetime_4:
                            Levelshop.Instance.GiveTime(4, 2);
                            break;
                        case EM_IAPConstants.Product_rpgg2_packagetime_5:
                            Levelshop.Instance.GiveTime(5, 1);
                            break;
                    
                        case EM_IAPConstants.Product_rpgg2_packagetime_7:
                            Levelshop.Instance.GiveTime(7, 2);
                            break;
                        case EM_IAPConstants.Product_rpgg2_packagetime_8:
                            Levelshop.Instance.GiveTime(8, 1);
                            break;
                    
                        case EM_IAPConstants.Product_rpgg2_packagetime_10:
                            Levelshop.Instance.GiveTime(10, 2);
                            break;
                        case EM_IAPConstants.Product_rpgg2_packagetime_11:
                            Levelshop.Instance.GiveTime(11, 1);
                            break;
                 
                        case EM_IAPConstants.Product_rpgg2_packagetime_13:
                            Levelshop.Instance.GiveTime(13, 2);
                            break;
                        case EM_IAPConstants.Product_rpgg2_packagetime_14:
                            Levelshop.Instance.GiveTime(14, 1);
                            break;
                    
                        case EM_IAPConstants.Product_rpgg2_packagetime_16:
                            Levelshop.Instance.GiveTime(16, 2);
                            break;
                        case EM_IAPConstants.Product_rpgg2_packagetime_17:
                            Levelshop.Instance.GiveTime(17, 1);
                            break;
                  
                        case EM_IAPConstants.Product_rpgg2_packagetime_19:
                            Levelshop.Instance.GiveTime(19, 2);
                            break;
                        case EM_IAPConstants.Product_rpgg2_packagetime_20:
                            Levelshop.Instance.GiveTime(20, 1);
                            break;
                        
                        case EM_IAPConstants.Product_rpgg2_packagetime_22:
                            Levelshop.Instance.GiveTime(22, 1);
                            break;
                        case EM_IAPConstants.Product_rpgg2_packagetime_23:
                            Levelshop.Instance.GiveTime(23, 1);
                            break;
                        case EM_IAPConstants.Product_rpgg2_packagetime_25:
                            Levelshop.Instance.GiveTime(25, 1);
                            break;
                        case EM_IAPConstants.Product_rpgg2_packagetime_26:
                            Levelshop.Instance.GiveTime(26, 1);
                            break;
                    }
                    
                    
                    for (int i = 0; i < ids.Length; i++)
                    {
                        Inventory.Instance.AddItem(ids[i], int.Parse(howmanys[i]));
                        id.Add(ids[i]);
                        howmany.Add(howmanys[i]);
                    }

                    alertmanager.Instance.NotiCheck_Class();
                    Settingmanager.Instance.SaveAllNoLog();
                    Inventory.Instance.ShowEarnItem(id.ToArray(), howmany.ToArray(), false);
                    Savemanager.Instance.SaveInventory_SaveOn();
                    break;
                case EM_IAPConstants.Product_rpgg2_package_seasonpass1:
                    Inventory.Instance.AddItem("998", 40000);
                    id.Add("998");
                    howmany.Add("40000");
                    PlayerBackendData.Instance.SeasonPassPremium = true;
                    SeasonPass.Instance.BuyPremiumPanel.Hide(true);
                    SeasonPass.Instance.Refresh();
                    SeasonPass.Instance.SaveSeasonReward();
                    Inventory.Instance.ShowEarnItem(id.ToArray(), howmany.ToArray(), false);
                    Savemanager.Instance.SaveInventory_SaveOn();
                    LogManager.InsertIAPBuyProductpass();
                    break;
                case EM_IAPConstants.Product_rpgg2_growthguide_premium:
                    PlayerBackendData.Instance.tutoguidepremium = true;
                    //현재 레벨아래 보상을 모두 다시 받음
                    List<string> idss = new List<string>();
                    List<decimal> hw = new List<decimal>();
                    if (PlayerBackendData.Instance.tutoguideid != 0)
                    {
                        for (int i = 0; i < PlayerBackendData.Instance.tutoguideid ; i++)
                        {
////                            Debug.Log(TutorialTotalManager.Instance.slots[i].Reward[0].id);
                            if (TutorialTotalManager.Instance.slots[i].Reward[0].id== "1000")
                            {
                                Inventory.Instance.AddItemExp(TutorialTotalManager.Instance.slots[i].Reward[0].id, (decimal)TutorialTotalManager.Instance.slots[i].Reward[0].count, false);
                            }
                            else if (TutorialTotalManager.Instance.slots[i].Reward[1].id == "1011")
                            {
                                Inventory.Instance.AddItemExp(TutorialTotalManager.Instance.slots[i].Reward[1].id, (decimal)TutorialTotalManager.Instance.slots[i].Reward[1].count, false);
                            }
                            else
                            {
                                Inventory.Instance.AddItem(TutorialTotalManager.Instance.slots[i].Reward[0].id, (int)TutorialTotalManager.Instance.slots[i].Reward[0].count, false);
                            }
                            idss.Add(TutorialTotalManager.Instance.slots[i].Reward[0].id);
                            hw.Add((decimal)TutorialTotalManager.Instance.slots[i].Reward[0].count);
                            idss.Add(TutorialTotalManager.Instance.slots[i].Reward[1].id);
                            hw.Add((decimal)TutorialTotalManager.Instance.slots[i].Reward[1].count);
                        }
                       PlayerData.Instance.RefreshInitData();
                       PlayerData.Instance. RefreshExp();
                       PlayerData.Instance. RefreshAchExp();
                       PlayerData.Instance. RefreshPlayerstat();
                        Savemanager.Instance.SaveOnlyLv();
                        Inventory.Instance.ShowEarnItem4(idss.ToArray(),hw.ToArray(),false);
                        PlayerData.Instance.RefreshExp();
                    }
                    TutorialTotalManager.Instance.RefreshNow();
                    Savemanager.Instance.SaveGuideQuest();
                    Savemanager.Instance.SaveInventory();
                    Savemanager.Instance.Save();
                    
                    break;
            }
            
            
        }
        else
        {
        }
    }

    // Failed purchase handler
    void PurchaseFailedHandler(IAPProduct product, string failureReason)
    {
        Debug.Log("The purchase of product " + product.Name + " has failed with reason: " + failureReason);
    }
}
