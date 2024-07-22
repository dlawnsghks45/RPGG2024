using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tradebuy : MonoBehaviour
{
     tradeslot nowselectslot;
    
    public Image ItemImage;
    public Image NeedImage;
    public Image NeedTotalImage;
    public Image GiveImage;
    public string Tradeid;

    public Image ItemHaveImage;
    public Text ItemHaveCount;
    public Text ItemHaveName;

    
    private int totalcost;

    private int maxcount = 1000; 
    
    //필요 재료 개수
    private string needid= "0";
    private int needhowmany = 0;
    
    //지급 재료 개수
    private string giveid;
    private decimal givehowmany = 0;
    
    //구매 횟수    
    string count = "0"; //입력한 개수 스트링
    private decimal countint = 0;
    
    //재료 반환 개수
    public decimal needtotalcount; //
    
    //
    public Text Title;
    
    public void SetTradeItem(string tradeid, tradeslot slot)
    {
        Tradeid = tradeid;
        nowselectslot = slot;
           switch (TradeShopDB.Instance.Find_id(Tradeid).buytype)
           
        {
            case "one":
                maxcount = Timemanager.Instance.OnceTradePackage.Contains(tradeid) ? 0 : 1;
                break;
         case "daily":
             int daily = (int)Enum.Parse(typeof(Timemanager.ContentEnumDaily),
                 TradeShopDB.Instance.Find_id(Tradeid).arrynum);
             maxcount = Timemanager.Instance.DailyContentCount[daily];
             break;
         case "weekly":
             int weekly = (int)Enum.Parse(typeof(Timemanager.ContentEnumWeekly),
                 TradeShopDB.Instance.Find_id(Tradeid).arrynum);
             maxcount = Timemanager.Instance.WeeklyContentCount[weekly];

             break;
         case "UnLimit":
             maxcount = 1000000;
             break;
        }

           if (Title != null)
           {
               Title.text =  $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(TradeShopDB.Instance.Find_id(tradeid).giveid).name)}";
               Inventory.Instance.ChangeItemRareColor(Title, Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(TradeShopDB.Instance.Find_id(tradeid).giveid).rare));

           }
           Debug.Log(gameObject.name);
       //TradeShopDB.Instance.Find_id(tradeid).givehowmany;
        ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(TradeShopDB.Instance.Find_id(tradeid).giveid).sprite);
        NeedImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(TradeShopDB.Instance.Find_id(tradeid).needid).sprite);
        NeedTotalImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(TradeShopDB.Instance.Find_id(tradeid).needid).sprite);
        GiveImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(TradeShopDB.Instance.Find_id(tradeid).giveid).sprite);
        
        //주는개수
        needid = TradeShopDB.Instance.Find_id(tradeid).needid;
        needhowmany = int.Parse(TradeShopDB.Instance.Find_id(tradeid).needhowmany);
        needcountText.text = needhowmany.ToString();
        
        //구매 개수
        count = "0"; 
        countText.text = count;

        //주는 개수 X 구매 개수
        needtotalcount = 0;
        needTotalcountText.text = "0";

        //
        giveid = TradeShopDB.Instance.Find_id(tradeid).giveid;
        givehowmany = decimal.Parse(TradeShopDB.Instance.Find_id(tradeid).givehowmany);
        
        
        //보유개수
        ItemHaveImage.sprite = NeedImage.sprite;
        ItemHaveCount.text = PlayerBackendData.Instance.CheckItemCount(needid).ToString("N0");
        ItemHaveName.text = Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(needid).name);
        
        //구매 개수에 따른 지급 개수
        totalcost = 0;
        TotalgiveText.text = totalcost.ToString();

        RefreshData();
    }

    void RefreshData()
    {
        switch (TradeShopDB.Instance.Find_id(Tradeid).needid)
        {
            case "1000": //골드
                if (PlayerBackendData.Instance.GetMoney() > (needhowmany * int.Parse(count)))
                {
                    //아이템 개수가 입력한 횟수보다 많으면
                    countint = int.Parse(count);
            
                    if (countint > maxcount)
                    {
                        countint = maxcount;
                        count = countint.ToString();
                    }
            
                    countText.text = countint.ToString();
                }
                else
                {
                    countint =  (int)Math.Truncate((double)PlayerBackendData.Instance.GetMoney()/ needhowmany);
                    count = countint.ToString();

                    if (countint > maxcount)
                    {
                        countint = maxcount;
                        count = countint.ToString();
                    }
            
                    countText.text = count;
                }
                break;
            case "1001": //크리스탈
                if (PlayerBackendData.Instance.GetCash() > (needhowmany * int.Parse(count)))
                {
                    //아이템 개수가 입력한 횟수보다 많으면
                    countint = int.Parse(count);
            
                    if (countint > maxcount)
                    {
                        countint = maxcount;
                        count = countint.ToString();
                    }
            
                    countText.text = countint.ToString();
                }
                else
                {
                    countint =  (int)Math.Truncate((double)PlayerBackendData.Instance.GetCash()/ needhowmany);
                    count = countint.ToString();

                    if (countint > maxcount)
                    {
                        countint = maxcount;
                        count = countint.ToString();
                    }
            
                    countText.text = count;
                }
                break;
            default:
                if (PlayerBackendData.Instance.CheckItemCount(TradeShopDB.Instance.Find_id(Tradeid).needid) > (needhowmany * int.Parse(count)))
                {
                    //아이템 개수가 입력한 횟수보다 많으면
                    countint = int.Parse(count);
            
                    if (countint > maxcount)
                    {
                        countint = maxcount;
                        count = countint.ToString();
                    }
            
                    countText.text = countint.ToString();
                }
                else
                {
                    countint =  (int)Math.Truncate((double)PlayerBackendData.Instance.CheckItemCount(TradeShopDB.Instance.Find_id(Tradeid).needid)/ needhowmany);
                    count = countint.ToString();

                    if (countint > maxcount)
                    {
                        countint = maxcount;
                        count = countint.ToString();
                    }

                    countText.text = count;
                }
                break;
        }        
        
       

        
        
        
        needtotalcount = needhowmany * countint;
        needTotalcountText.text = needtotalcount.ToString("N0");
        TotalgiveText.text = (givehowmany * countint).ToString();
    }


    public Text TotalgiveText; //총 개수  지급개수 x 횟수
    public Text needcountText; //지급개수만 표시
    public Text needTotalcountText; //1회당 필요 개수    필요한 개수 x 횟수다
    public Text countText; //얼마나 살것인가


    public void Bt_RemoveText()
    {
        count = count[..^1];

        if (count == "")
        {
            count = "0";
            countint = 0;
        }

        RefreshData();
    }

    public void Bt_MaxCount()
    {
        switch (TradeShopDB.Instance.Find_id(Tradeid).needid)
        {
            case "1000": //골드
                countint = (int)Math.Truncate((double)PlayerBackendData.Instance.GetMoney()/ needhowmany);
                break;
            case "1001": //크리스탈
                countint = (int)Math.Truncate((double)PlayerBackendData.Instance.GetCash()/ needhowmany);
                break;
            default:
                countint =  (int)Math.Truncate((double)PlayerBackendData.Instance.CheckItemCount(TradeShopDB.Instance.Find_id(Tradeid).needid)/ needhowmany);
                break;
        }            

        if (countint > maxcount)
            countint = maxcount;
        
        count = countint.ToString();
        countText.text = count;
        RefreshData();
    }

    //0~9입력용
    public void Bt_ButtonNumber(string num)
    {
        count = count + num;

        RefreshData();
    }

    public void Bt_BuyItem() 
    {
        if (countint == 0)
        {
            Inventory.Instance.TradePanel.Hide(true);
            return;
        }
        switch (TradeShopDB.Instance.Find_id(Tradeid).needid)
        {
            case "1000": //골드
                if(PlayerBackendData.Instance.GetMoney() >= (needhowmany * countint))
                {
                    PlayerData.Instance.DownGold(needhowmany * countint);
                    PlayerBackendData.Instance.Additem(giveid, (int)givehowmany * (int)countint);
                    Inventory.Instance.RefreshInventory();
                    Inventory.Instance.TradePanel.Hide(true);
                    Savemanager.Instance.SaveInventory();
                    Savemanager.Instance.SaveMoneyDataDirect();
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Trade/교환성공"),alertmanager.alertenum.일반);
                    ConsumeTime();
                    LogManager.LogTrade(nowselectslot.tradeid,giveid,(int)givehowmany * (int)countint,"1000",needhowmany * (int)countint);
                    Savemanager.Instance.Save();

                    //여기에 포션

                    if (Tradeid.Equals("1005"))
                        Tutorialmanager.Instance.CheckTutorial("buypotion");

                }
                else
                {
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/골드부족"),alertmanager.alertenum.주의);
                }
                break;
            case "1001": //크리스탈
                if(PlayerBackendData.Instance.GetCash() >= (needhowmany * countint))
                {
                    PlayerData.Instance.DownCash(needhowmany * countint);
                    PlayerBackendData.Instance.Additem(giveid, (int)givehowmany * (int)countint);
                    Inventory.Instance.RefreshInventory();
                    Inventory.Instance.TradePanel.Hide(true);
                    Savemanager.Instance.SaveInventory();
                    Savemanager.Instance.SaveCash();
                    Savemanager.Instance.Save();

                    LogManager.LogTrade(nowselectslot.tradeid,giveid,(int)givehowmany * (int)countint,"1001",needhowmany * (int)countint);
                    
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Trade/교환성공"),alertmanager.alertenum.일반);
                    ConsumeTime();
                    
                }
                else
                {
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/불꽃부족"),alertmanager.alertenum.주의);
                }
                break;
            default:
                if(PlayerBackendData.Instance.ishaveitemcount(needid,needhowmany * (int)countint))
                {
                    PlayerBackendData.Instance.RemoveItem(needid, needhowmany * (int)countint);
                    Inventory.Instance.AddItem(giveid, (int)givehowmany * (int)countint);
                    Inventory.Instance.RefreshInventory();
                    Inventory.Instance.TradePanel.Hide(true);
                    Savemanager.Instance.SaveInventory();
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Trade/교환성공"),alertmanager.alertenum.일반);
                    LogManager.LogTrade(nowselectslot.tradeid,giveid,(int)givehowmany * (int)countint,needid,needhowmany * (int)countint);
                    Savemanager.Instance.Save();
                    ConsumeTime();
                }
                else
                {
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/재료가없음"),alertmanager.alertenum.주의);
                }
                break;
        }

        Savemanager.Instance.SaveShopData();
    }

    void ConsumeTime()
    {
        //시간 체크 횟수 
        switch (TradeShopDB.Instance.Find_id(Tradeid).buytype)
        {
            case "daily":
                int daily = (int)Enum.Parse(typeof(Timemanager.ContentEnumDaily),
                    TradeShopDB.Instance.Find_id(Tradeid).arrynum);
                Timemanager.Instance.ConSumeCount_DailyAscny(daily,(int)countint);
                nowselectslot.RefreshBuyCount();
                break;
            case "weekly":
                int weekly = (int)Enum.Parse(typeof(Timemanager.ContentEnumWeekly),
                    TradeShopDB.Instance.Find_id(Tradeid).arrynum);
                Timemanager.Instance.ConSumeCount_WeeklyAscny(weekly,(int)countint);
                nowselectslot.RefreshBuyCount();
                break;
            case "one":
                if (!Timemanager.Instance.OnceTradePackage.Contains(Tradeid))
                    Timemanager.Instance.OnceTradePackage.Add(Tradeid);
                break;
        }
    }
}
