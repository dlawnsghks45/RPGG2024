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
    
    //�ʿ� ��� ����
    private string needid= "0";
    private int needhowmany = 0;
    
    //���� ��� ����
    private string giveid;
    private decimal givehowmany = 0;
    
    //���� Ƚ��    
    string count = "0"; //�Է��� ���� ��Ʈ��
    private decimal countint = 0;
    
    //��� ��ȯ ����
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
        
        //�ִ°���
        needid = TradeShopDB.Instance.Find_id(tradeid).needid;
        needhowmany = int.Parse(TradeShopDB.Instance.Find_id(tradeid).needhowmany);
        needcountText.text = needhowmany.ToString();
        
        //���� ����
        count = "0"; 
        countText.text = count;

        //�ִ� ���� X ���� ����
        needtotalcount = 0;
        needTotalcountText.text = "0";

        //
        giveid = TradeShopDB.Instance.Find_id(tradeid).giveid;
        givehowmany = decimal.Parse(TradeShopDB.Instance.Find_id(tradeid).givehowmany);
        
        
        //��������
        ItemHaveImage.sprite = NeedImage.sprite;
        ItemHaveCount.text = PlayerBackendData.Instance.CheckItemCount(needid).ToString("N0");
        ItemHaveName.text = Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(needid).name);
        
        //���� ������ ���� ���� ����
        totalcost = 0;
        TotalgiveText.text = totalcost.ToString();

        RefreshData();
    }

    void RefreshData()
    {
        switch (TradeShopDB.Instance.Find_id(Tradeid).needid)
        {
            case "1000": //���
                if (PlayerBackendData.Instance.GetMoney() > (needhowmany * int.Parse(count)))
                {
                    //������ ������ �Է��� Ƚ������ ������
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
            case "1001": //ũ����Ż
                if (PlayerBackendData.Instance.GetCash() > (needhowmany * int.Parse(count)))
                {
                    //������ ������ �Է��� Ƚ������ ������
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
                    //������ ������ �Է��� Ƚ������ ������
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


    public Text TotalgiveText; //�� ����  ���ް��� x Ƚ��
    public Text needcountText; //���ް����� ǥ��
    public Text needTotalcountText; //1ȸ�� �ʿ� ����    �ʿ��� ���� x Ƚ����
    public Text countText; //�󸶳� ����ΰ�


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
            case "1000": //���
                countint = (int)Math.Truncate((double)PlayerBackendData.Instance.GetMoney()/ needhowmany);
                break;
            case "1001": //ũ����Ż
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

    //0~9�Է¿�
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
            case "1000": //���
                if(PlayerBackendData.Instance.GetMoney() >= (needhowmany * countint))
                {
                    PlayerData.Instance.DownGold(needhowmany * countint);
                    PlayerBackendData.Instance.Additem(giveid, (int)givehowmany * (int)countint);
                    Inventory.Instance.RefreshInventory();
                    Inventory.Instance.TradePanel.Hide(true);
                    Savemanager.Instance.SaveInventory();
                    Savemanager.Instance.SaveMoneyDataDirect();
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Trade/��ȯ����"),alertmanager.alertenum.�Ϲ�);
                    ConsumeTime();
                    LogManager.LogTrade(nowselectslot.tradeid,giveid,(int)givehowmany * (int)countint,"1000",needhowmany * (int)countint);
                    Savemanager.Instance.Save();

                    //���⿡ ����

                    if (Tradeid.Equals("1005"))
                        Tutorialmanager.Instance.CheckTutorial("buypotion");

                }
                else
                {
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/������"),alertmanager.alertenum.����);
                }
                break;
            case "1001": //ũ����Ż
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
                    
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Trade/��ȯ����"),alertmanager.alertenum.�Ϲ�);
                    ConsumeTime();
                    
                }
                else
                {
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/�Ҳɺ���"),alertmanager.alertenum.����);
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
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Trade/��ȯ����"),alertmanager.alertenum.�Ϲ�);
                    LogManager.LogTrade(nowselectslot.tradeid,giveid,(int)givehowmany * (int)countint,needid,needhowmany * (int)countint);
                    Savemanager.Instance.Save();
                    ConsumeTime();
                }
                else
                {
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/��ᰡ����"),alertmanager.alertenum.����);
                }
                break;
        }

        Savemanager.Instance.SaveShopData();
    }

    void ConsumeTime()
    {
        //�ð� üũ Ƚ�� 
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
