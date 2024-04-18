using System;
using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Doozy;
using UnityEngine;
using UnityEngine.UI;
using BackEnd.Tcp;
using Random = UnityEngine.Random;

public class altarmanager : MonoBehaviour
{
    //�̱��游���.
    private static altarmanager _instance = null;
    public static altarmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(altarmanager)) as altarmanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    
    public Text[] altarpanellv;
    public Text[] altarpanelstat;
    public decimal[] CheckAltarCount = new Decimal[4];

    public void RefreshAltarLv()
    {
        for (int i = 0; i < altarpanellv.Length; i++)
        {
            altarpanellv[i].text = $"Lv.{PlayerBackendData.Instance.Altar_Lvs[i]}";
            altarpanelstat[i].text = string.Format(Inventory.GetTranslate("Stat/���ɷ�ġ"),
                PlayerBackendData.Instance.Altar_Lvs[i] *
                int.Parse(altarrenewalDB.Instance.Find_id(i.ToString()).stat));
        }
    }
    
    public countpanel countpanels;

    public Sprite[] AltarSpriteSource;
    public Image AltarImage;
    
    public UIView AltarPanel;

    public Text altarlv;
    public Text altarstat_before;
    public Text altarstat_after;

    
    //ȭ��ǥ 
    public GameObject arrow;
    //��� 
    public GameObject altarstat_objbefroe;
    public GameObject altarstat_objafter;
    public Text percent;


    public enum AltarType
    {
        ����,
        ���,
        ���̵�,
        ���̱�
    }

    public AltarType NowSelectType;
    

    public void SelectType(int num)
    {
        NowSelectType = (AltarType)num;
        SetAltarData(num.ToString());
        Bt_OpenPanel();
    }

    [SerializeField]
    private float str;
    private float dex;
    private float ints;
    private float wis;
    private float minatk;
    private float minmanatk;
    private string needitem;
    private decimal needitemhowmany;
    public int nowpercent;
    
    
    
    private float str_next;
    private float dexnext;
    private float intsnext;
    private float wisnext;
    private float minatknext;
    private float minmanatknext;


    public int stat;
    public string needid2;
    public string[] lvscale2;
    public string[] needhowmany2;
    public string[] percent2;
    public string needtype2;
    int canlvupcount;
    public int nowindex = 0;
    public string  nowselectid;


    private void Start()
    {
        RefreshAltarLv();
    }


    public void SetAltarData(string typenum)
    {
        
        nowselectid = typenum;
        altarrenewalDB.Row data = altarrenewalDB.Instance.Find_id(typenum);

        needid2 = data.needid;
        needhowmany2 = data.needhowmany.Split(';');
        lvscale2 = data.lvscale.Split(';');
        percent2 = data.percent.Split(';');
        needtype2 = data.needtype;
        stat = int.Parse(data.stat);

        //���� ����
        int lv = PlayerBackendData.Instance.Altar_Lvs[int.Parse(typenum)];

        nowindex = 0;

        for (int i = 0; i < lvscale2.Length; i++)
        {
            if (lv >= int.Parse(lvscale2[i]))
            {
                nowindex = i;
            }
        }
     //   Debug.Log("���� �ε����� " +nowindex + "�Դϴ�");
    }
    
    
    void SetStatNext()
    {
        str_next = (PlayerBackendData.Instance.Altar_Lvs[(int)NowSelectType] + countpanels.nowcount) * stat;
        dexnext = (PlayerBackendData.Instance.Altar_Lvs[(int)NowSelectType] + countpanels.nowcount) *stat;
        intsnext =(PlayerBackendData.Instance.Altar_Lvs[(int)NowSelectType] + countpanels.nowcount) * stat;
        wisnext = (PlayerBackendData.Instance.Altar_Lvs[(int)NowSelectType] + countpanels.nowcount) * stat;
    }
    
     void  SetStat()
     {
         str = (PlayerBackendData.Instance.Altar_Lvs[(int)NowSelectType]) * stat;
         dex = (PlayerBackendData.Instance.Altar_Lvs[(int)NowSelectType]) * stat;
        ints =(PlayerBackendData.Instance.Altar_Lvs[(int)NowSelectType]) * stat;
        wis = (PlayerBackendData.Instance.Altar_Lvs[(int)NowSelectType]) * stat;
       
        needitem = needid2;
        nowpercent = int.Parse(percent2[nowindex]);
        switch (needtype2)
        {
            //�Ɵ�ް���
            case "count":
                needitemhowmany = int.Parse(needhowmany2[nowindex]);
                break;
            //���� * �����۰���
            case "levelcount":
                needitemhowmany = decimal.Parse(needhowmany2[nowindex]) * PlayerBackendData.Instance.Altar_Lvs[(int)NowSelectType];
                break;
        }
    }

     [SerializeField] private itemshowcountslot haveitemobj;
     public Image needitemsprite;
     public Text needitemsText;
     public RectTransform obj;


     public UIView AltarInfoPanel;
     
    public void Bt_OpenPanel()
    {
        AltarPanel.Show(false);
        Refresh();
    }

    public void Refresh()
    {
        AltarImage.sprite = AltarSpriteSource[(int)NowSelectType];
        SetStat();

        altarlv.text = string.Format(Inventory.GetTranslate("UI/���ܷ���"),
            PlayerBackendData.Instance.GetGoldAltarlv(NowSelectType) + 1);

//        Debug.Log(PlayerBackendData.Instance.GetGoldAltarlv(NowSelectType));

        Inventory.Instance.StringRemove();

        Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/��"), str));
        Inventory.Instance.StringWrite("\n");
        Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/��ø"), dex));
        Inventory.Instance.StringWrite("\n");
        Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/����"), ints));
        Inventory.Instance.StringWrite("\n");
        Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/����"), wis));
        Inventory.Instance.StringEnd(altarstat_before);


        LayoutRebuilder.ForceRebuildLayoutImmediate(obj);
        needitemsprite.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(needitem).sprite);
        needitemsText.text = needitemhowmany.ToString("N0");


        haveitemobj.id = needitem;
        haveitemobj.SetData();
        haveitemobj.InitData();


        percent.text = $"{nowpercent}%";


        SetStatNext();

        Inventory.Instance.StringRemove();
        Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/��"), str_next));
        Inventory.Instance.StringWrite("\n");
        Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/��ø"), dexnext));
        Inventory.Instance.StringWrite("\n");
        Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/����"), intsnext));
        Inventory.Instance.StringWrite("\n");
        Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/����"), wisnext));
        Inventory.Instance.StringEnd(altarstat_after);

        AltarInfoPanel.Show(false);
    }

    public void RefreshCounts()
    {
        for (int o = 0; o < altarpanellv.Length; o++)
        {
            altarrenewalDB.Row data = altarrenewalDB.Instance.Find_id(o.ToString());

            string needid3 = data.needid;
            string[] needhowmany3 = data.needhowmany.Split(';');
            string[] lvscale3 = data.lvscale.Split(';');
            string[] percent3 = data.percent.Split(';');
            string needtype3 = data.needtype;
            int stat3 = int.Parse(data.stat);

            //���� ����
            int lv = PlayerBackendData.Instance.Altar_Lvs[int.Parse(o.ToString())];

           int nowindex2 = 0;

            for (int i = 0; i < lvscale3.Length; i++)
            {
                if (lv >= int.Parse(lvscale3[i]))
                {
                    nowindex2 = i;
                }
            }
            
            switch (needtype3)
            {
                //�Ɵ�ް���
                case "count":
                    CheckAltarCount[o] = decimal.Parse(needhowmany3[nowindex2]);
                    break;
                //���� * �����۰���
                case "levelcount":
                    CheckAltarCount[o] = decimal.Parse(needhowmany3[nowindex2]) * PlayerBackendData.Instance.Altar_Lvs[(int)NowSelectType];
                    break;
            }
        }
    }

     public void Bt_Upgrade()
    {
        decimal counts = needitemhowmany;
        bool issucc = false;
        bool noresource = false;
        int succcount = 0;
        int failcount = 0;
        int prevlv = PlayerBackendData.Instance.GetGoldAltarlv(NowSelectType);

        for (int i = 0; i < countpanels.nowcount; i++)
        {
           
            
            
            
            nowindex = 0;

            for (int j = 0; j < lvscale2.Length; j++)
            {
                if (PlayerBackendData.Instance.GetGoldAltarlv(NowSelectType) >= int.Parse(lvscale2[j]))
                {
                    nowindex = j;
                }
            }
            
            nowpercent = int.Parse(percent2[nowindex]);
            switch (needtype2)
            {
                //�Ɵ�ް���
                case "count":
                    needitemhowmany = int.Parse(needhowmany2[nowindex]);
                    break;
                //���� * �����۰���
                case "levelcount":
                    needitemhowmany = decimal.Parse(needhowmany2[nowindex]) * PlayerBackendData.Instance.Altar_Lvs[(int)NowSelectType];
                    break;
            }
            
            
            
            switch (NowSelectType)
            {
                case AltarType.����:

                    if (!PlayerBackendData.Instance.ishaveitemcount(needitem, (int)counts))
                    {
                     //   alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/������"), alertmanager.alertenum.����);
                     noresource = true;

                        return;
                    }

                    PlayerBackendData.Instance.RemoveItem(needitem, (int)needitemhowmany);
                    if (nowpercent >= Random.Range(1, 101))
                    {
                        PlayerBackendData.Instance.SetGoldAltarLvUp(NowSelectType);
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.�Ϲ�);
                        issucc = true;
                        succcount++;

                    }
                    else
                    {
                        //����ȭ����
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.����);
                        issucc = true;
                        failcount++;
                    }

                    Refresh();
                    break;
                case AltarType.���:

                    if (PlayerBackendData.Instance.GetMoney() < needitemhowmany)
                    {
                        noresource = true;

                    //    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/������"), alertmanager.alertenum.����);
                        return;
                    }

                    PlayerData.Instance.DownGold(needitemhowmany);
                    if (nowpercent >= Random.Range(1, 101))
                    {
                        PlayerBackendData.Instance.SetGoldAltarLvUp(NowSelectType);
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.�Ϲ�);
                        issucc = true;
                        succcount++;


                    }
                    else
                    {
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.����);
                        issucc = true;
                        failcount++;


                    }

                    Refresh();
                    break;
                case AltarType.���̵�:

                    if (!PlayerBackendData.Instance.ishaveitemcount("2908", (int)counts))
                    {
                      //  alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/������"), alertmanager.alertenum.����);
                      noresource = true;

                        return;
                    }

                    PlayerBackendData.Instance.RemoveItem("2908", (int)needitemhowmany);
                    if (nowpercent >= Random.Range(1, 101))
                    {
                        PlayerBackendData.Instance.SetGoldAltarLvUp(NowSelectType);
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.�Ϲ�);
                        issucc = true;
                        succcount++;

                    }
                    else
                    {
                        //����ȭ����
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.����);
                        issucc = true;
                        failcount++;

                    }

                    Refresh();
                    break;
                case AltarType.���̱�:

                    if (!PlayerBackendData.Instance.ishaveitemcount(needitem, (int)counts))
                    {
                     //   alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/������"), alertmanager.alertenum.����);
                     noresource = true;
                        return;
                    }

                    PlayerBackendData.Instance.RemoveItem(needitem, (int)needitemhowmany);
                    if (nowpercent >= Random.Range(1, 101))
                    {
                        PlayerBackendData.Instance.SetGoldAltarLvUp(NowSelectType);
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.�Ϲ�);
                        issucc = true;
                        succcount++;
                    }
                    else
                    {
                        //����ȭ����
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.����);
                        issucc = true;
                        failcount++;
                    }

                    Refresh();
                    break;
            }
        }

        alertmanager.Instance.ShowAlert(
            string.Format(Inventory.GetTranslate("UI/���ܸ����ᰭȭ"), prevlv, prevlv + succcount, succcount, failcount),
            alertmanager.alertenum.�Ϲ�);


        if(succcount==0 && failcount ==0)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/������"), alertmanager.alertenum.����);
        }
        
        if (issucc)
        {
            //��������
            Param param = new Param
            {
                { "level", PlayerBackendData.Instance.GetLv() },
                { "levelExp", PlayerBackendData.Instance.GetExp() },
                //��������
                { "level_ach", PlayerBackendData.Instance.GetAchLv() },
                { "level_achExp", PlayerBackendData.Instance.GetAchExp() },
                //���ܷ���
                { "Altar_Lvs", PlayerBackendData.Instance.GetAllAltarlv() },
                { "inventory", PlayerBackendData.Instance.ItemInventory },
            };
            // key �÷��� ���� keyCode�� ������ �˻�
            Where where = new Where();
            where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

            SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
            {
                // ���� ó��
                if (!callback.IsSuccess()) return;

            });
            
            //���� ����
            PlayerData.Instance.RefreshPlayerstat();
            Savemanager.Instance.SaveExpData();
            Savemanager.Instance.SaveInventory();
            Savemanager.Instance.SaveMoneyDataDirect();
            alertmanager.Instance.NotiCheck_Altar();
            LogManager.SaveAltar(NowSelectType.ToString(), succcount, failcount, counts);
            Refresh();
            RefreshAltarLv();
            Savemanager.Instance.Save();
        }
        
    }
    /*
    public void Bt_Upgrade()
    {
        decimal counts = needitemhowmany;
        bool issucc = false;
        int succcount = 0;
        int failcount = 0;

        if (UseAllToggle.isOn)
        {
            SetStat();
            int prevlv = PlayerBackendData.Instance.GetGoldAltarlv(NowSelectType);
           
            switch (NowSelectType)
            {
                case AltarType.����:
                    if (!PlayerBackendData.Instance.ishaveitemcount(needitem, (int)counts))
                    {
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/������"), alertmanager.alertenum.����);
                        return;
                    }

                    while (PlayerBackendData.Instance.ishaveitemcount(needitem, (int)needitemhowmany))
                    {



                        PlayerBackendData.Instance.RemoveItem(needitem, (int)needitemhowmany);
                        if (nowpercent >= Random.Range(1, 101))
                        {
                            succcount++;
                            PlayerBackendData.Instance.SetGoldAltarLvUp(NowSelectType);
                        }
                        else
                        {
                            //����ȭ����
                            failcount++;
                        }

                        Refresh();
                    }
                    issucc = true;
                    break;
                case AltarType.���:
                    if (PlayerBackendData.Instance.GetMoney() < needitemhowmany)
                    {
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/������"), alertmanager.alertenum.����);
                        return;
                    }
                    while (PlayerBackendData.Instance.GetMoney() > needitemhowmany)
                    {
                        PlayerData.Instance.DownGold(needitemhowmany);
                        if (nowpercent >= Random.Range(1, 101))
                        {
                            succcount++;
                            PlayerBackendData.Instance.SetGoldAltarLvUp(NowSelectType);
                        }
                        else
                        {
                            //����ȭ����
                            failcount++;
                        }

                        Refresh();
                    }
                    issucc = true;
                    break;
                case AltarType.���̵�:
                    if (!PlayerBackendData.Instance.ishaveitemcount("2908", (int)counts))
                    {
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/������"), alertmanager.alertenum.����);
                        return;
                    }

                    while (PlayerBackendData.Instance.ishaveitemcount("2908", (int)needitemhowmany))
                    {



                        PlayerBackendData.Instance.RemoveItem("2908", (int)needitemhowmany);
                        if (nowpercent >= Random.Range(1, 101))
                        {
                            succcount++;
                            PlayerBackendData.Instance.SetGoldAltarLvUp(NowSelectType);
                        }
                        else
                        {
                            //����ȭ����
                            failcount++;
                        }

                        Refresh();
                    }
                    issucc = true;
                    break;
                case AltarType.���̱�:
                    if (!PlayerBackendData.Instance.ishaveitemcount(needitem, (int)counts))
                    {
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/������"), alertmanager.alertenum.����);
                        return;
                    }

                    while (PlayerBackendData.Instance.ishaveitemcount(needitem, (int)needitemhowmany))
                    {
                        PlayerBackendData.Instance.RemoveItem(needitem, (int)needitemhowmany);
                        if (nowpercent >= Random.Range(1, 101))
                        {
                            succcount++;
                            PlayerBackendData.Instance.SetGoldAltarLvUp(NowSelectType);
                        }
                        else
                        {
                            //����ȭ����
                            failcount++;
                        }
                        Refresh();
                    }

                    issucc = true;
                     break;
            }

            alertmanager.Instance.ShowAlert(
                string.Format(Inventory.GetTranslate("UI/���ܸ����ᰭȭ"), prevlv, prevlv + succcount, succcount, failcount),
                alertmanager.alertenum.�Ϲ�);
                
            //��������
            Param param = new Param
            {
                { "level", PlayerBackendData.Instance.GetLv() },
                { "levelExp", PlayerBackendData.Instance.GetExp() },
                //��������
                { "level_ach", PlayerBackendData.Instance.GetAchLv() },
                { "level_achExp", PlayerBackendData.Instance.GetAchExp() },
                //���ܷ���
                { "Altar_Lvs", PlayerBackendData.Instance.GetAllAltarlv() },
                { "inventory", PlayerBackendData.Instance.ItemInventory },
            };
            // key �÷��� ���� keyCode�� ������ �˻�
            Where where = new Where();
            where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

            SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
            {
                // ���� ó��
                if (!callback.IsSuccess()) return;

            });
        }
        else
        {
            switch (NowSelectType)
            {
                case AltarType.����:

                    if (!PlayerBackendData.Instance.ishaveitemcount(needitem, (int)counts))
                    {
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/������"), alertmanager.alertenum.����);
                        return;
                    }

                    PlayerBackendData.Instance.RemoveItem(needitem, (int)needitemhowmany);
                    if (nowpercent >= Random.Range(1, 101))
                    {
                        PlayerBackendData.Instance.SetGoldAltarLvUp(NowSelectType);
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.�Ϲ�);
                        issucc = true;
                    }
                    else
                    {
                        //����ȭ����
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.����);
                        issucc = true;
                    }

                    Refresh();
                    break;
                case AltarType.���:

                    if (PlayerBackendData.Instance.GetMoney() < needitemhowmany)
                    {

                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/������"), alertmanager.alertenum.����);
                        return;
                    }

                    PlayerData.Instance.DownGold(needitemhowmany);
                    if (nowpercent >= Random.Range(1, 101))
                    {
                        PlayerBackendData.Instance.SetGoldAltarLvUp(NowSelectType);
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.�Ϲ�);
                        issucc = true;

                    }
                    else
                    {
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.����);
                        issucc = true;

                    }

                    Refresh();
                    break;
                case AltarType.���̵�:

                    if (!PlayerBackendData.Instance.ishaveitemcount("2908", (int)counts))
                    {
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/������"), alertmanager.alertenum.����);
                        
                        return;
                    }

                    PlayerBackendData.Instance.RemoveItem("2908", (int)needitemhowmany);
                    if (nowpercent >= Random.Range(1, 101))
                    {
                        PlayerBackendData.Instance.SetGoldAltarLvUp(NowSelectType);
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.�Ϲ�);
                        issucc = true;
                    }
                    else
                    {
                        //����ȭ����
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.����);
                        issucc = true;
                    }

                    Refresh();
                    break;
                case AltarType.���̱�:

                    if (!PlayerBackendData.Instance.ishaveitemcount(needitem, (int)counts))
                    {
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/������"), alertmanager.alertenum.����);
                        return;
                    }
                    PlayerBackendData.Instance.RemoveItem(needitem, (int)needitemhowmany);
                    if (nowpercent >= Random.Range(1, 101))
                    {
                        PlayerBackendData.Instance.SetGoldAltarLvUp(NowSelectType);
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.�Ϲ�);
                        issucc = true;


                    }
                    else
                    {
                        //����ȭ����
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/���ܰ�ȭ����"), alertmanager.alertenum.����);
                        issucc = true;
                    }
                    
                    Refresh();
                    break;
            }
        }

        if (issucc)
        {
            //���� ����
            PlayerData.Instance.RefreshPlayerstat();
            Savemanager.Instance.SaveExpData();
            Savemanager.Instance.SaveInventory();
            Savemanager.Instance.SaveMoneyDataDirect();
            alertmanager.Instance.NotiCheck_Altar();
            LogManager.SaveAltar(NowSelectType.ToString(), succcount, failcount, counts);
            Refresh();
            Savemanager.Instance.Save();
        }
    }
*/
}
