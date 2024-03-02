using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class Sotangpanel : MonoBehaviour
{
    public string needitemid;
    public int needitempersotang;
    public UIView SotangObj;
    public InputField SotangInput;
    public int sotangcount = 1;
    public int maxsotangcount = 30;


    public void Show(string dropid)
    {
        if (PlayerBackendData.Instance.CheckItemCount(needitemid) == 0)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/아이템이부족"), alertmanager.alertenum.일반);
            return;
        }
   
        sotangcount = 1;
        maxsotangcount = PlayerBackendData.Instance.CheckItemCount(needitemid);
        if (maxsotangcount > 30)
            maxsotangcount = 30;
        SotangInput.text = sotangcount.ToString();

        SetDropData(dropid);
        
        SotangObj.Show(false);
        SotangInput.text = "1";
    }
    
    public void Bt_Plus()
    {
        if (sotangcount.Equals(maxsotangcount))
        {
            //최대
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/최대"), alertmanager.alertenum.일반);
            return;
        }

        sotangcount++;
        SotangInput.text = sotangcount.ToString();
    }
    public void Bt_MaxPlus()
    {
        sotangcount = maxsotangcount;
        SotangInput.text = sotangcount.ToString();
    }
    public void CheckCount(string count)
    {

        if (count.Equals("0") || count.Equals(""))
        {
            sotangcount = 1;
            SotangInput.text = sotangcount.ToString();
            return;
        }

        if (int.Parse(count) > maxsotangcount)
        {
            sotangcount = maxsotangcount;
            SotangInput.text = sotangcount.ToString();
            return;
        }
        sotangcount = int.Parse(count);
    }

    public void Bt_Minus()
    {
        if (sotangcount == 1)
        {
            //최소
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/최소"), alertmanager.alertenum.일반);
            return;
        }

        sotangcount--;
        SotangInput.text = sotangcount.ToString();

    }
    public void Bt_MinMinus()
    {
        sotangcount=1;
        SotangInput.text = sotangcount.ToString();

    }

    public bool StartSotang()
    {
        if (sotangcount * needitempersotang > PlayerBackendData.Instance.CheckItemCount(needitemid))
        {
            //개수가 부족하다.
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/아이템이부족"), alertmanager.alertenum.일반);
            return false;
        }
        else
        {
            GiveDropToInvenToryBoss(Mon_DropItemIDboss,
                Mon_DropItemMinHowmanyboss,
                Mon_DropItemMaxHowmanyboss, Mon_DropItemPercentboss);
            PlayerBackendData.Instance.RemoveItem(needitemid, sotangcount * needitempersotang);
            achievemanager.Instance.AddCount(Acheves.대마법사의묘,sotangcount);
            return true;
        }
    }
    
    public List<string> Mon_DropItemIDboss = new List<string>();
    public List<int> Mon_DropItemMinHowmanyboss = new List<int>();
    public List<int> Mon_DropItemMaxHowmanyboss = new List<int>();
    public List<int> Mon_DropItemPercentboss = new List<int>();

    void SetDropData(string dropid)
    {
        string nowdropbossid = dropid;
        //Debug.Log(nowdropbossid);

        Mon_DropItemIDboss.Clear();
        Mon_DropItemMinHowmanyboss.Clear();
        Mon_DropItemMaxHowmanyboss.Clear();
        Mon_DropItemPercentboss.Clear();

        
        MonDropDB.Row[] data2 = MonDropDB.Instance.FindAll_id(nowdropbossid).ToArray();

        foreach (var t in data2)
        {
            Mon_DropItemIDboss.Add(t.itemid);
            Mon_DropItemMinHowmanyboss.Add(int.Parse(t.minhowmany));
            Mon_DropItemMaxHowmanyboss.Add(int.Parse(t.maxhowmany));
            Mon_DropItemPercentboss.Add(int.Parse(t.rate));
        }
    }

    public void GiveDropToInvenToryBoss(List<string> dropid, List<int> minhowmany, List<int> maxhowmany,
        List<int> percent)
    {
        List<string> id = new List<string>();
        List<decimal> hw = new List<decimal>();

        for (int j = 0; j < sotangcount; j++)
        {
            for (int i = 0; i < dropid.Count; i++)
            {
                Random.InitState((int)Time.deltaTime + PlayerBackendData.Instance.GetRandomSeed());
                int Ran_rate = Random.Range(0, 1000000); // 1,000,000이 100%이다.
                if (Ran_rate <= mondropmanager.Instance.getpercent(percent[i]))
                {
                    int Howmany = Random.Range(minhowmany[i], maxhowmany[i]);
                    Inventory.Instance.AddItem(dropid[i], Howmany, false, true);
                    int index = id.IndexOf(dropid[i]);
                    if (index != -1)
                    {
                        hw[index] += Howmany;
                    }
                    else
                    {
                        id.Add(dropid[i]);
                        hw.Add(Howmany);
                    }
                }
            }
        }


        Inventory.Instance.ShowEarnItem4(id.ToArray(), hw.ToArray(),false);
        Savemanager.Instance.SaveInventory_SaveOn();
    }

    readonly WaitForSeconds waits = new WaitForSeconds(0.2f);
    WaitForSeconds waits2 = new WaitForSeconds(1.2f);



}
