using BackEnd;
using Doozy.Engine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftManager : MonoBehaviour
{
    [SerializeField]
    bool istest;
    //싱글톤만들기.
    private static CraftManager _instance = null;
    public static CraftManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(CraftManager)) as CraftManager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }


    public GameObject CraftFinishedNoti;
    public TextMeshProUGUI CraftFinishedNoticounttext;

    public craftslot craftobj;
    public List<craftslot> craftslots = new List<craftslot>();
    public Transform crafttrans;
    public GameObject[] SubObj;


    //제작재료
    public craftResourceSlot[] craftresourceslots;
    //완성품
    public CraftSuccesssSlot[] craftsuccessslots;

    public bool cancraft;
    //
    public Button interactiveButton;

    public void Bt_SelectMainPanel(int num)
    {
        for (int i = 0; i < SubObj.Length; i++)
        {
            SubObj[i].SetActive(false);
        }
        SubObj[num].SetActive(true);
    }

    public void Bt_SelectSubPanel(string num)
    {
        List<string> str = new List<string>();
        for (int i = 0; i < CraftTableDB.Instance.NumRows(); i++)
        {
           // Debug.Log(CraftTableDB.Instance.GetAt(i).PanelType);
            if (CraftTableDB.Instance.GetAt(i).PanelType.Equals(num))
            {
                str.Add(CraftTableDB.Instance.GetAt(i).id);
            }
        }

        //개수 체크 안맞으면 늘림
        if (craftslots.Count < str.Count)
        {
            for (int i = 0; i < str.Count; i++)
            {
                craftslot item = Instantiate(craftobj, crafttrans);
                item.gameObject.SetActive(false);
                craftslots.Add(item);

            }
        }

        //제작슬릇보여주기
        for (int i = 0; i < craftslots.Count; i++)
        {
            if (str.Count > i)
            {
                craftslots[i].setcraft(str[i]);
                craftslots[i].gameObject.SetActive(true);
            }
            else
            {

                if (!craftslots[i].gameObject.activeSelf)
                    break;

                craftslots[i].gameObject.SetActive(false);
            }
        }
    }

    public void SearchCraftByResource(string resourceid)
    {
        if (resourceid == "")
            return;
        List<string> str = new List<string>();
        for (int i = 0; i < CraftTableDB.Instance.NumRows(); i++)
        {
            if (CraftTableDB.Instance.GetAt(i).A.Equals(resourceid))
            {
                if(!str.Contains(CraftTableDB.Instance.GetAt(i).id))
                {
                    str.Add(CraftTableDB.Instance.GetAt(i).id);
                    continue;
                }
            }
            if (CraftTableDB.Instance.GetAt(i).B.Equals(resourceid))
            {
                if (!str.Contains(CraftTableDB.Instance.GetAt(i).id))
                {
                    str.Add(CraftTableDB.Instance.GetAt(i).id);
                    continue;
                }
            }
            if (CraftTableDB.Instance.GetAt(i).C.Equals(resourceid))
            {
                if (!str.Contains(CraftTableDB.Instance.GetAt(i).id))
                {
                    str.Add(CraftTableDB.Instance.GetAt(i).id);
                    continue;
                }
            }
            if (CraftTableDB.Instance.GetAt(i).D.Equals(resourceid))
            {
                if (!str.Contains(CraftTableDB.Instance.GetAt(i).id))
                {
                    str.Add(CraftTableDB.Instance.GetAt(i).id);
                    continue;
                }
            }
            if (CraftTableDB.Instance.GetAt(i).E.Equals(resourceid))
            {
                if (!str.Contains(CraftTableDB.Instance.GetAt(i).id))
                {
                    str.Add(CraftTableDB.Instance.GetAt(i).id);
                    continue;
                }
            }
            if (CraftTableDB.Instance.GetAt(i).F.Equals(resourceid))
            {
                if (!str.Contains(CraftTableDB.Instance.GetAt(i).id))
                {
                    str.Add(CraftTableDB.Instance.GetAt(i).id);
                    continue;
                }
            }
        }


        //개수 체크 안맞으면 늘림
        if (craftslots.Count < str.Count)
        {
            for (int i = 0; i < str.Count; i++)
            {
                craftslot item = Instantiate(craftobj, crafttrans);
                item.gameObject.SetActive(false);
                craftslots.Add(item);

            }
        }

        //제작슬릇보여주기
        for (int i = 0; i < craftslots.Count; i++)
        {
            if (str.Count > i)
            {
                craftslots[i].setcraft(str[i]);
                craftslots[i].gameObject.SetActive(true);
            }
            else
            {

                if (!craftslots[i].gameObject.activeSelf)
                    break;

                craftslots[i].gameObject.SetActive(false);
            }
        }
    }
    public void SearchCraft(string itemname)
    {
        if (itemname == "")
            return;
        List<string> str = new List<string>();
        for (int i = 0; i < CraftTableDB.Instance.NumRows(); i++)
        {
            if(CraftTableDB.Instance.GetAt(i).isequip.Equals(""))
            {
                continue;
            }
            else if (CraftTableDB.Instance.GetAt(i).isequip.Equals("TRUE"))
            {
                //장비 검색
                //Debug.Log("이름" + CraftTableDB.Instance.GetAt(i).id);
                if (Inventory.GetTranslate(EquipItemDB.Instance.Find_id(CraftTableDB.Instance.GetAt(i).Successid).Name).Contains(itemname))
                {
                    str.Add(CraftTableDB.Instance.GetAt(i).id);
                    //Debug.Log("찾았다" + CraftTableDB.Instance.GetAt(i).id);
                }
            }
            else
            {
                //Debug.Log(ItemdatabasecsvDB.Instance.Find_id(CraftTableDB.Instance.GetAt(i).Successid));
                //Debug.Log(ItemdatabasecsvDB.Instance.Find_id(CraftTableDB.Instance.GetAt(i).Successid));
                //아이템 검색
                if (Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(CraftTableDB.Instance.GetAt(i).Successid).name).Contains(itemname))
                {
                    str.Add(CraftTableDB.Instance.GetAt(i).id);
                    //Debug.Log("찾았다" + CraftTableDB.Instance.GetAt(i).id);
                }
            }
        }


        //개수 체크 안맞으면 늘림
        if (craftslots.Count < str.Count)
        {
            for (int i = 0; i < str.Count; i++)
            {
                craftslot item = Instantiate(craftobj, crafttrans);
                item.gameObject.SetActive(false);
                craftslots.Add(item);

            }
        }

        //제작슬릇보여주기
        for (int i = 0; i < craftslots.Count; i++)
        {
            if (str.Count > i)
            {
                craftslots[i].setcraft(str[i]);
                craftslots[i].gameObject.SetActive(true);
            }
            else
            {

                if (!craftslots[i].gameObject.activeSelf)
                    break;

                craftslots[i].gameObject.SetActive(false);
            }
        }
    }
    public string nowselectcraftid;

    public Image SuccessImage;
    public Text SuccessItem;
    public Text craftPercent;
    public Text craftTime;

    CraftTableDB.Row craftdata;
    //제작정보창 보여주기
    string[] strid = new string[6];
    string[] strhowmany = new string[6];

    public GameObject craftpanelOn;
    public GameObject craftpanelOff;

    public InputField countinput;
    //제작개수
    public int nowselectcount;

    public void Bt_Plus()
    {
        nowselectcount++;
        countinput.text = nowselectcount.ToString();
        RefreshResource();
    }
    public void Bt_Minus()
    {
        if (nowselectcount == 1 || nowselectcount == 0)
            return;

        nowselectcount--;
        countinput.text = nowselectcount.ToString();
        RefreshResource();
    }

    public void Inputs()
    {
        if (int.Parse(countinput.text) > 100000)
        {
            countinput.text = "100000";
            nowselectcount = 100000;
        }
        else
        {
            nowselectcount = int.Parse(countinput.text);
        }
        RefreshResource();
    }

    public void Bt_MakeFull()
    {
        int max = 100000;
        int divide = 0;
        for (int i = 0; i < strid.Length; i++)
        {
            //Debug.Log(strid[i]);
            if (strid[i] != "0")
            {

                switch (strid[i])
                {
                    case "1001":
                        if (PlayerBackendData.Instance.GetCash() <= 0)
                        {
                            max = 1;
                            break;
                        }
                        divide = (int)PlayerBackendData.Instance.GetCash() / int.Parse(strhowmany[i]);
                        break;
                    default:
                        int index = PlayerBackendData.Instance.GetItemIndex(strid[i]);
                        if (index == -1)
                        {
                            max = 1;
                            break;
                        }
                        divide = PlayerBackendData.Instance.ItemInventory[index].Howmany / int.Parse(strhowmany[i]);
                        break;
                }
                //제일 낮은 수를 찾는다.
                if (divide < 0)
                    divide = 1;
                if (max > divide)
                {
                    max = divide;
                }
            }
        }

        nowselectcount = max;
        countinput.text = nowselectcount.ToString();
        RefreshResource();
    }
    

    public GameObject CraftCountingObj;
    DateTime dt;

    public void Bt_ShowCraftResourceInfo(string craftid)
    {
        craftpanelOn.SetActive(true);
        craftpanelOff.SetActive(false);
        nowselectcraftid = craftid;
        craftdata = CraftTableDB.Instance.Find_id(craftid);

        craftPercent.text = string.Format(Inventory.GetTranslate("ButtonUI/성공확률"), craftdata.SuccessPercent);
        dt = new DateTime(0);
        craftTime.text = dt.AddSeconds(double.Parse(craftdata.crafttime) * nowselectcount).ToString("dd:HH:mm:ss");
        //개수 설정
        nowselectcount = 1;
        countinput.text = "1";
        //완성품
        if (craftdata.isequip == "TRUE")
        {
            SuccessImage.sprite =
                SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(craftdata.Successid).Sprite);
            SuccessItem.text = Inventory.GetTranslate(EquipItemDB.Instance.Find_id(craftdata.Successid).Name);
            SuccessItem.color = Inventory.Instance.GetRareColor(EquipItemDB.Instance.Find_id(craftdata.Successid).Rare);
            CraftCountingObj.SetActive(false);
        }
        else
        {
            SuccessImage.sprite =
                SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(craftdata.Successid).sprite);
            SuccessItem.text = Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(craftdata.Successid).name);
            SuccessItem.color =
                Inventory.Instance.GetRareColor(ItemdatabasecsvDB.Instance.Find_id(craftdata.Successid).rare);
            CraftCountingObj.SetActive(true);
        }

        float total = float.Parse(craftdata.SuccessPercent) - float.Parse(craftdata.BigSuccessPercent);
        craftsuccessslots[0].Refresh(craftid, craftdata.isequip, craftdata.Successid, craftdata.MinSuccesshowmany,
            craftdata.MaxSuccesshowmany, total.ToString());
        craftsuccessslots[0].gameObject.SetActive(true);

        if (craftdata.BigSuccessid != "0")
        {
            craftsuccessslots[1].Refresh(craftid, craftdata.isequip, craftdata.BigSuccessid,
                craftdata.MinBigSuccesshowmany, craftdata.MaxBigSuccesshowmany, craftdata.BigSuccessPercent);
            craftsuccessslots[1].gameObject.SetActive(true);
        }
        else
        {
            craftsuccessslots[1].gameObject.SetActive(false);
        }

        if (craftdata.FailId != "0")
        {
            int failpercent = 100 - (int.Parse(craftdata.SuccessPercent) + int.Parse(craftdata.BigSuccessPercent));
            craftsuccessslots[2].Refresh(craftid, craftdata.isequip, craftdata.FailId, craftdata.MinFailHowmany,
                craftdata.MaxFailHowmany, failpercent.ToString("N0"));
            craftsuccessslots[2].gameObject.SetActive(true);
        }
        else
        {
            craftsuccessslots[2].gameObject.SetActive(false);
        }


        strid[0] = craftdata.A;
        strid[1] = craftdata.B;
        strid[2] = craftdata.C;
        strid[3] = craftdata.D;
        strid[4] = craftdata.E;
        strid[5] = craftdata.F;

        strhowmany[0] = craftdata.AH;
        strhowmany[1] = craftdata.BH;
        strhowmany[2] = craftdata.CH;
        strhowmany[3] = craftdata.DH;
        strhowmany[4] = craftdata.EH;
        strhowmany[5] = craftdata.FH;

        RefreshResource();
    }

    public Text needgoldtext;
    private decimal needgold;
    public Text Guildbuffreduce;
    public GuildEmblemSlot Guildemblem;
    private void RefreshResource()
    {
        CraftManager.Instance.cancraft = true;
        int a = int.Parse(craftdata.ResourceHowmany);
        foreach (var t in craftresourceslots)
        {
            t.gameObject.SetActive(false);
        }

        for (int i = 0; i < a; i++)
        {
            if (strid[i] == "0") continue;
            craftresourceslots[i].Refresh(strid[i], strhowmany[i]);
            craftresourceslots[i].gameObject.SetActive(true);
        }
        
        double times = double.Parse(craftdata.crafttime) * nowselectcount;
        double guildbuffminus = 0;
        
        Guildemblem.gameObject.SetActive(false);

        if (PlayerBackendData.Instance.ishaveguild)
        {
          //  guildbuffminus = times * MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.제작시간감소);
//            Guildemblem.SetEmblem(MyGuildManager.Instance.myguildclassdata.GuildFlag,MyGuildManager.Instance.myguildclassdata.GuildBanner);
         //   Guildemblem.gameObject.SetActive(true);
            craftTime.color = Color.green;
            times -= guildbuffminus;

        }
        else
        {
            Guildemblem.gameObject.SetActive(false);
            craftTime.color = Color.white;
        }
        if (guildbuffminus != 0)
        {
            Guildbuffreduce.text = $"-{MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.제작시간감소) * 100f}%";
        }
        
        times = PlayerBackendData.Instance.ispremium ? times * 0.5f : times;
        
        craftTime.text = dt.AddSeconds(times).ToString("HH:mm:ss");
        needgold = decimal.Parse(craftdata.needgold) * nowselectcount;
        needgoldtext.text = dpsmanager.convertNumber(needgold);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)needgoldtext.transform);
    }

    //제작은 4칸
    //2칸 무료 2칸 프리미엄ㅁ
    public UIView CraftAcceptPanel; //제작 확인창
    public Image CraftAcceptImage;
    public Text CraftAcceptName;
    public Text CraftAcceptCount;
    public Text CraftAcceptNeedGold;
    public void ShowCraftAcceptPanel()
    {
        CraftAcceptPanel.Show(false);
        CraftAcceptImage.sprite = SuccessImage.sprite;
        CraftAcceptName.text = SuccessItem.text;
        CraftAcceptName.color = SuccessItem.color;
        CraftAcceptCount.text = string.Format(Inventory.GetTranslate("ButtonUI/제작확인"), nowselectcount);
        CraftAcceptNeedGold.text = needgoldtext.text;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)CraftAcceptNeedGold.transform);

        interactiveButton.interactable = true;
    }

    public UIView CraftRightFinishPanel; //제작 확인창
    public Image CraftRightFinishImage;
    public Text CraftRightFinishName;
    public Text CraftRightFinishNeedFire;
    public int nowrightfinishslot;
    public int needfire;
    //즉시완료 보여줌
    public void ShowCraftRightFinishPanel(CraftTableDB.Row craftdata,double nowsecond,int slotnum)
    {
        CraftRightFinishPanel.Show(false);

        if (craftdata.isequip == "TRUE")
        {
            CraftRightFinishImage.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(craftdata.Successid).Sprite);
            CraftRightFinishName.text = Inventory.GetTranslate(EquipItemDB.Instance.Find_id(craftdata.Successid).Name);
            CraftRightFinishName.color = Inventory.Instance.GetRareColor(EquipItemDB.Instance.Find_id(craftdata.Successid).Rare);
        }
        else
        {
            CraftRightFinishImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(craftdata.Successid).sprite);
            CraftRightFinishName.text = Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(craftdata.Successid).name);
            CraftRightFinishName.color = Inventory.Instance.GetRareColor(ItemdatabasecsvDB.Instance.Find_id(craftdata.Successid).rare);
        }
        nowrightfinishslot = slotnum;
        StartCoroutine(Counting(nowsecond));
    }

    WaitForSeconds wait = new WaitForSeconds(1f);
    public IEnumerator Counting(double timesecond)
    {
        while (CraftRightFinishPanel.IsActive())
        {
            timesecond--;
            CraftRightFinishNeedFire.text = GetFireFinishCount(timesecond).ToString();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)CraftRightFinishNeedFire.transform);
            yield return wait;
        }
    }

    private int GetFireFinishCount(double timesecond)
    {
        //1분에 불꽃 2개
        if (timesecond < 60)
        {
            needfire = 1;
            return 1;
        }

        needfire = (int)(timesecond / 60) * 2;

        return needfire;
    }

    public void Bt_AcceptRightFinish()
    {
        if (craftingdoingslots[nowrightfinishslot].isfinish)
        {
            CraftRightFinishPanel.Hide(false);
            return;
        }

        if (PlayerBackendData.Instance.GetCash() < needfire)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/불꽃부족"), alertmanager.alertenum.주의);
            CraftRightFinishPanel.Hide(false);
            return;
        }

        
        if (!Settingmanager.Instance.CheckServerOn())
        {
            return;
        }
        
        BackendReturnObject servertime = Backend.Utils.GetServerTime();
       // Debug.Log(servertime);
        if (servertime.IsSuccess())
        {
            string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
            PlayerBackendData.Instance.craftdatetime[nowrightfinishslot] = time;
            craftingdoingslots[nowrightfinishslot].MakeTimeZero();
            Savemanager.Instance.SaveCraft();
            CraftRightFinishPanel.Hide(false);
            PlayerData.Instance.DownCash(needfire);
            Savemanager.Instance.SaveCash();
        }
        Savemanager.Instance.Save();
    }


    public void MakeCraftStart()
    {
        if (!Settingmanager.Instance.CheckServerOn())
        {
            return;
        }
        
        bool iscrafting = false;
        bool ishaveresource = false;
        bool ishavegold = false;
        int craftsit = 0;
        for (int i = 0; i < (PlayerBackendData.Instance.ispremium ? PlayerBackendData.Instance.craftmakingid.Length : 2); i++)
        {
            //Debug.Log(PlayerBackendData.Instance.craftmakingid[i]);
            if (PlayerBackendData.Instance.craftmakingid[i] != "") continue;
            //자리있음
            iscrafting = true;
            craftsit = i;
            break;
        }

        for (int i = 0; i < strid.Length; i++)
        {
            if (strid[i] == "0") continue;


            switch (strid[i])
            {
                case "1001":
                    if (PlayerBackendData.Instance.GetCash() <= 0)
                    {
                        ishaveresource = true;
                        break;
                    }

                    break;
                default:
                    int index = PlayerBackendData.Instance.GetItemIndex(strid[i]);

                    if (index == -1)
                    {
                        ishaveresource = true;
                        break;
                    }

                    if (PlayerBackendData.Instance.ItemInventory[index].Howmany >= int.Parse(strhowmany[i])) continue;
                    ishaveresource = true;
                    break;
            }
            break;
        }

        if (needgold <= PlayerBackendData.Instance.GetMoney())
        {
            ishavegold = true;
        }
        if (!istest)
        {
            if (ishaveresource || !cancraft)
            {
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/재료가없음"), alertmanager.alertenum.주의);
                CraftAcceptPanel.Hide(true);
                craftpanelOn.SetActive(false);
                craftpanelOff.SetActive(true);
                return;
            }

            if (!ishavegold)
            {
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/골드부족"), alertmanager.alertenum.주의);
                CraftAcceptPanel.Hide(true);
                craftpanelOn.SetActive(false);
                craftpanelOff.SetActive(true);
                return;
            }

            if (!iscrafting)
            {
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/제작자리부족"), alertmanager.alertenum.주의);
                CraftAcceptPanel.Hide(true);
                craftpanelOn.SetActive(false);
                craftpanelOff.SetActive(true);
                return;
            }
        }


        if (!istest && (!iscrafting || ishaveresource || !ishavegold)) return;
        {
            //제작 시작 
            BackendReturnObject servertime = Backend.Utils.GetServerTime();
            if (!servertime.IsSuccess()) return;
            string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
            DateTime dt = DateTime.Parse(time);

            double times = double.Parse(craftdata.crafttime) * nowselectcount;

            double guildbuffminus = times * MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.제작시간감소);
            times -= guildbuffminus;
            times = PlayerBackendData.Instance.ispremium ? times * 0.5f : times;

            dt = istest ? dt.AddSeconds(0) : dt.AddSeconds(times);
            PlayerBackendData.Instance.craftdatetime[craftsit] = dt.ToString(CultureInfo.InvariantCulture);
            PlayerBackendData.Instance.craftmakingid[craftsit] = nowselectcraftid;
            PlayerBackendData.Instance.craftdatecount[craftsit] = nowselectcount;
            Savemanager.Instance.SaveCraft();

            TimeSpan dateDiff = dt - DateTime.Parse(time);
            Invoke(nameof(forinvoke), (float)dateDiff.TotalSeconds);
            if (!istest)
            {
                for (int i = 0; i < strid.Length; i++)
                {
                    if (strid[i] != "0")
                    {
                        switch (strid[i])
                        {
                            case "1001":
                                PlayerData.Instance.DownCash(int.Parse(strhowmany[i])* nowselectcount);
                                break;
                            default:
                                PlayerBackendData.Instance.RemoveItem(strid[i], int.Parse(strhowmany[i]) * nowselectcount);
                                break;
                        }
                    }
                }

                PlayerData.Instance.DownGold(needgold);
                // Inventory.Instance.RefreshInventory();
                Savemanager.Instance.SaveInventory();
                Savemanager.Instance.SaveMoneyCashDirect();
                Savemanager.Instance.Save();
            }
            LogManager.StartCraft(craftdata.id,nowselectcount);
            craftpanelOn.SetActive(false);
            craftpanelOff.SetActive(true);
            RefreshNowCraftingCount();
            CraftAcceptPanel.Hide(false);
            
            //가이드 
            Tutorialmanager.Instance.CheckTutorial("craft");
            if (nowselectcraftid.Equals("5500") || nowselectcraftid.Equals("5501") ||
                nowselectcraftid.Equals("5502") || nowselectcraftid.Equals("5503"))
            {
                Tutorialmanager.Instance.CheckTutorial("makerageweapon");
            }
            
            
            
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/제작을시작하였다"), alertmanager.alertenum.일반);
            //재료 감소및 적용
        }
    }
    public Text NowCraftingCountText; //제작중 (2) 이렇게 표시
    public void RefreshNowCraftingCount()
    {
        int a = 0;
        foreach (var t in PlayerBackendData.Instance.craftmakingid)
        {
            if (t != "")
            {
                a++;
            }
        }

        NowCraftingCountText.text = string.Format(Inventory.GetTranslate("ButtonUI/제작중"), a);
    }



    public UIView craftingpanel;
    public craftingdoingslot[] craftingdoingslots;
    public void Bt_ShowCraftingPanel()
    {
        //제작중(개수) 버튼을 눌렀을때
        BackendReturnObject servertime = Backend.Utils.GetServerTime();

        if (servertime.IsSuccess())
        {
            craftingpanel.Show(false);
            string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
            DateTime dt = DateTime.Parse(time);

            for (int i = 0; i < PlayerBackendData.Instance.craftmakingid.Length; i++)
            {
                if (PlayerBackendData.Instance.craftmakingid[i] != "")
                {
                   // Debug.Log(PlayerBackendData.Instance.craftdatetime[i]);
                   // Debug.Log(dt.ToString());
                    craftingdoingslots[i].SetCraft(PlayerBackendData.Instance.craftmakingid[i], DateTime.Parse(PlayerBackendData.Instance.craftdatetime[i]), dt);
                }
                else
                {
                    craftingdoingslots[i].CheckPremium();
                }
            }

        }
    }
    public UIView CraftEndPanel;
    public craftresultslot[] craftresultslots; //대성공 성공 실패
    public Text[] CraftEndCountText; //대성공 성공 실패
    public int[] CraftEndCount; //대성공 성공 실패
    public ParticleSystem[] endparticle;
    public void GiveResult(CraftTableDB.Row craftdata, int index)
    {
        if (!Settingmanager.Instance.CheckServerOn())
        {
            return;
        }
        
        CraftEndPanel.Show(true);
        bool isbigsuccess = false;
        CraftEndCount[0] = 0; //대성공
        CraftEndCount[1] = 0; //성공
        CraftEndCount[2] = 0; //실패
        craftresultslots[0].gameObject.SetActive(false);
        craftresultslots[1].gameObject.SetActive(false);
        craftresultslots[2].gameObject.SetActive(false);
        
        
        //achievemanager.Instance.AddCount(Acheves.아이템제작);
        QuestManager.Instance.AddCount(1, "craft");

        if (craftdata.SuccessPercent != "0")
        {
            //확률 존재
            //횟수만큼 계산
            for (int i = 0; i < PlayerBackendData.Instance.craftdatecount[index]; i++)
            {
                //성공부터 체크
                int random = UnityEngine.Random.Range(1, 101);
                if (random <= int.Parse(craftdata.SuccessPercent))
                {
                    int random_big = UnityEngine.Random.Range(1, 101);
                    if (random_big <= int.Parse(craftdata.BigSuccessPercent))
                    {
                        //대성공
                        CraftEndCount[0]++;
                        isbigsuccess = true;
                    }
                    else
                    {
                        //성공
                        CraftEndCount[1]++;
                    }
                }
                else
                {
                    //실패

                    CraftEndCount[2]++;
                }
            }
        }
        else
        {
            //성공부터 체크
            int random_big = UnityEngine.Random.Range(1, 101);
            if (random_big <= int.Parse(craftdata.BigSuccessPercent))
            {
                //대성공
                CraftEndCount[0]++;
                isbigsuccess = true;
            }
            else
            {
                //성공
                CraftEndCount[1]++;
            }
        }

        List<string> endid = new List<string>();
        List<int> endhowmany = new List<int>();
        //보상지급
        EquipDatabase succequipdata = null;
        if (CraftEndCount[0] != 0)
        {
            //대성공이있다면
            if (craftdata.isequip == "TRUE")
            {
                succequipdata = PlayerBackendData.Instance.MakeEquipment(craftdata.BigSuccessid);
                if (succequipdata != null)
                {
                    //Debug.Log("대성공");
                    craftresultslots[0].SetEnd(SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(craftdata.BigSuccessid).Sprite),
                        $"{Inventory.GetTranslate(EquipItemDB.Instance.Find_id(craftdata.BigSuccessid).Name)} {GetEquipSmeltCountText(succequipdata.MaxStoneCount1)}", EquipItemDB.Instance.Find_id(craftdata.BigSuccessid).Rare,succequipdata); //이름 개수
                    craftresultslots[0].gameObject.SetActive(true);
                    
                    endid.Add(craftdata.BigSuccessid);
                    endhowmany.Add(1);

                }
            }
            else
            {
                craftresultslots[0].SetEnd(SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(craftdata.BigSuccessid).sprite),
                    $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(craftdata.BigSuccessid).name)}x{int.Parse(craftdata.MinBigSuccesshowmany) * CraftEndCount[0]}",
                   ItemdatabasecsvDB.Instance.Find_id(craftdata.BigSuccessid).rare); //이름 개수
                Inventory.Instance.AddItem(craftdata.BigSuccessid, int.Parse(craftdata.MinBigSuccesshowmany) * CraftEndCount[0], false);
                craftresultslots[0].gameObject.SetActive(true);
                endid.Add(craftdata.BigSuccessid);
                endhowmany.Add(int.Parse(craftdata.MinBigSuccesshowmany) * CraftEndCount[0]);
            }
        }
        if (CraftEndCount[1] != 0)
        {
            //성공이 있다면
            if (craftdata.isequip == "TRUE")
            {

                succequipdata = PlayerBackendData.Instance.MakeEquipment(craftdata.Successid);
                if (succequipdata != null)
                {
                    //Debug.Log("성공");
                    craftresultslots[1].SetEnd(SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(craftdata.Successid).Sprite),
                        $"{Inventory.GetTranslate(EquipItemDB.Instance.Find_id(craftdata.Successid).Name)} {GetEquipSmeltCountText(succequipdata.MaxStoneCount1)}"
                        , EquipItemDB.Instance.Find_id(craftdata.Successid).Rare, succequipdata); //이름 개수
                    craftresultslots[1].gameObject.SetActive(true);
                       endid.Add(craftdata.Successid);
                       endhowmany.Add(1);
                       
                       
                }
            }
            else
            {
                craftresultslots[1].SetEnd(SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(craftdata.Successid).sprite),
                    $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(craftdata.Successid).name)}x{int.Parse(craftdata.MinSuccesshowmany) * CraftEndCount[1]}",
                   ItemdatabasecsvDB.Instance.Find_id(craftdata.Successid).rare); //이름 개수
                Inventory.Instance.AddItem(craftdata.Successid, int.Parse(craftdata.MinSuccesshowmany) * CraftEndCount[1], false);
                craftresultslots[1].gameObject.SetActive(true);
                
                endid.Add(craftdata.Successid);
                endhowmany.Add(int.Parse(craftdata.MinSuccesshowmany) * CraftEndCount[1]);
            }
            
           
        }
        if (CraftEndCount[2] != 0)
        {
            //실패가 있다면
            if (craftdata.FailId != "0")
            {
                //실패 재료보상이 존재한다면.
                craftresultslots[2].SetEnd(SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(craftdata.FailId).sprite),
                    $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(craftdata.FailId).name)}x{int.Parse(craftdata.MinFailHowmany) * CraftEndCount[2]}",
                   ItemdatabasecsvDB.Instance.Find_id(craftdata.FailId).rare); //이름 개수
                Inventory.Instance.AddItem(craftdata.FailId, int.Parse(craftdata.MinFailHowmany) * CraftEndCount[2], false);
                craftresultslots[2].gameObject.SetActive(true);
            }
        }

        CraftEndCountText[0].text = CraftEndCount[0].ToString();
        CraftEndCountText[1].text = CraftEndCount[1].ToString();
        CraftEndCountText[2].text = CraftEndCount[2].ToString();


        //가이드 퀘스트
        Tutorialmanager.Instance.CheckTutorial("getcraft");
        Tutorialmanager.Instance.CheckTutorial("craftpotion");
        if (craftdata.id.Equals("10300"))
        {
            TutorialTotalManager.Instance.CheckGuideQuest("makearmor");
        }
        
        if (craftdata.id.Equals("5553") || craftdata.id.Equals("5554") ||
            craftdata.id.Equals("5555") || craftdata.id.Equals("5556") 
           )
        {
            TutorialTotalManager.Instance.CheckGuideQuest("makesubweapon");
        }
        if (craftdata.id.Equals("5557") || craftdata.id.Equals("5558") ||
            craftdata.id.Equals("5559") || craftdata.id.Equals("5560") ||
            craftdata.id.Equals("5561") || craftdata.id.Equals("5562") ||
            craftdata.id.Equals("5563") || craftdata.id.Equals("5564")
           )
        {
            TutorialTotalManager.Instance.CheckGuideQuest("makeweapon");
        }
        
        //제거
        PlayerBackendData.Instance.craftmakingid[index] = "";
        PlayerBackendData.Instance.craftdatetime[index] = "";
        PlayerBackendData.Instance.craftdatecount[index] = 0;
        
        LogManager.FinishCraft(craftdata.id,endhowmany,endid, bool.Parse(craftdata.isequip));

        if (Inventory.Instance.istrue)
        {
            Where where = new Where();
            where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
            Param param = new Param { { "PlayerCanLoadBool", "false" } };
            Inventory.Instance.istrue = false;
            SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Settingmanager.Instance.RecentServerDataCanLoadText.text = Inventory.Instance.istrue
                        ? string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"),
                            Inventory.Instance.istrue
                                ? "<Color=lime>On</color>"
                                : "<Color=red>Off</color>")
                        : string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"), "<Color=red>Off</color>");
                }
            });
        }
        CollectionRenewalManager.Instance.RefreshTotalCount();
        Settingmanager.Instance.SaveDataInventory();
        Savemanager.Instance.SaveCraft();
        Savemanager.Instance.SaveInventory();
        Savemanager.Instance.SaveEquip();
        Savemanager.Instance.Save();
        if (isbigsuccess)
        {
            //모든 파티클을 실행
            for (int i = 0; i < endparticle.Length; i++)
            {
                endparticle[i].Play();
            }
        }
        else
        {
            endparticle[0].Play();
        }
        craftingdoingslots[index].CheckPremium();
        RefreshNowCraftingCount();
        CheckNoti();
    }

    public string GetEquipSmeltCountText(int count)
    {
        if (count != 0)
            return $"[◇x{count}]";
        else
            return "";
    }


    //게임 접속 시 알람을 건다..
    public void InitNowCraftingAlert()
    {
        BackendReturnObject servertime = Backend.Utils.GetServerTime();

        if (servertime.IsSuccess())
        {
            string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
            for (int i = 0; i < PlayerBackendData.Instance.craftmakingid.Length; i++)
            {
                if (PlayerBackendData.Instance.craftmakingid[i] != "")
                {
                    //남은시간을 구해서 인보크
                    TimeSpan dateDiff = DateTime.Parse(PlayerBackendData.Instance.craftdatetime[i]) - DateTime.Parse(time);
                    //아이템이 있다면..
                    Invoke(nameof(forinvoke), (float)dateDiff.TotalSeconds);
                }
            }
        }
    }
    string crafteditems;
    public GameObject craftedalertpanel;
    void forinvoke()
    {
        craftedalertpanel.SetActive(true);
        CheckNoti();
    }


    public void CheckNoti()
    {
        int count = 0;
        for (int i = 0; i < PlayerBackendData.Instance.craftmakingid.Length; i++)
        {
            if (PlayerBackendData.Instance.craftmakingid[i] != "")
            {
                count++;
            }
        }

        if (count.Equals(0))
        {
            CraftFinishedNoti.SetActive(false);
            foreach (var VARIABLE in alertmanager.Instance.Alert_Craft)
            {
                VARIABLE.SetActive(false);
            }
        }
        else
        {
            CraftFinishedNoticounttext.text = count.ToString();
            CraftFinishedNoti.SetActive(true);

            foreach (var VARIABLE in alertmanager.Instance.Alert_Craft)
            {
                VARIABLE.SetActive(true);
            }
            alertmanager.Instance.Alert_Menu.SetActive(true);
        }

    }
}
