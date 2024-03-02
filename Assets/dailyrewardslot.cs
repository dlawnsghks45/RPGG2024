using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dailyrewardslot : MonoBehaviour
{
    public Image PanelRare;
    public Text daytext;
    public string checkid;
    public itemiconslot Item;
    public Text itemname;


    public bool isfinish;
    public bool canearn;
    public GameObject finishobj;
    public GameObject crystalobj;


    public void InitData(string day,string itemid,int howmany)
    {
        isfinish = false;
        canearn = false;
        daytext.text = string.Format(Inventory.GetTranslate("UI2/회차"), day);
        Item.Refresh(itemid,howmany,false);
        itemname.text = Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(itemid).name);
        Inventory.Instance.ChangeItemRareColor(itemname, ItemdatabasecsvDB.Instance.Find_id(itemid).rare);
        PanelRare.color = Color.white;
        finishobj.SetActive(false);
        crystalobj.SetActive(false);
    }

    public void ResetData()
    {
        isfinish = false;
        canearn = false;
        finishobj.SetActive(false);
        crystalobj.SetActive(false);
        PanelRare.color = Color.white;
    }
   
    
    //얻을 수 있다.
    public void CanEarn()
    {
        PanelRare.color = Color.green;
        canearn = true;
    }
    
    //끝
    public void Finished()
    {
        isfinish = true;
        finishobj.SetActive(true);
        crystalobj.SetActive(false);
        PanelRare.color = Color.gray;
    }

    //오늘날짜 표시
    public void IsToday()
    {
        PanelRare.color = Color.yellow;
        crystalobj.SetActive(false);
    }
    //현재칸 다음으로 표시
    public void CryStalOpen()
    {
        PanelRare.color = Color.cyan;
        crystalobj.SetActive(true);
        finishobj.SetActive(false);
    }

    public void Bt_BuyCrystalDaily()
    {
        dailyrewardmanager.Instance.CrystalObj.Show(false);
    }

  
}
