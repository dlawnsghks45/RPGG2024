using System;
using System.Collections;
using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackEnd;
using Doozy.Engine.Utils.ColorModels;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Inventory : MonoBehaviour
{
    public Color[] ItemRare;
    public Player mainplayer;

    public void Start()
    {
        //  Savemanager.Instance.settings = new ES3Settings($"{PlayerBackendData.Instance.nickname}3.es3",ES3.Location.Cache); 
        //  ES3Settings.defaultSettings.path = $"{PlayerBackendData.Instance.nickname}3.es3";
        //Savemanager.Instance.LoadEquip();
        Savemanager.Instance.LoadHpQuickSlot();
        Savemanager.Instance.LoadMpQuickSlot();
        Savemanager.Instance.LoadCraft();
        Savemanager.Instance.LoadDeathPenalty();
        CraftManager.Instance.InitNowCraftingAlert();
        //Savemanager.Instance.LoadTime();//Load Time
        Refreshadsfree();
        //가져오기
        //600초마다 시작
        InvokeRepeating(nameof(RefreshTime),1f, 600f);
        
        SetEquipDataLoad();

        Invoke(nameof(autosaveon), 1f);
    }

    void autosaveon()
    {
        Savemanager.Instance. StartAutoSave();
    }

    StringBuilder sb = new StringBuilder();
    //싱글톤만들기.
    private static Inventory _instance = null;
    public static Inventory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Inventory)) as Inventory;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }
    public void StringRemove()
    {
        sb.Remove(0, sb.Length);
    }
    //씀
    public void StringWrite(string text)
    {
        sb.Append(text);
    }
    //끝냄.
    public void StringEnd(Text text_component)
    {
        text_component.text = sb.ToString();
    }
    public string StringEnd()
    {
        return sb.ToString();
    }


    public static string GetTranslate(string _key)
    {
        return I2.Loc.LocalizationManager.GetTermTranslation(_key);
    }



    public int nowpage;
    public int maxpage;
    public Text PageText;
    public UIToggle[] EquipToggles;



    public void UpPage()
    {
        //다음페이지
        if (nowpage != maxpage - 1)
        {
            nowpage++;

            for (int i = 0; i < InventorySlots.Length; i++)
            {
                InventorySlots[i].RemoveObject();
            }
            PageText.text = $"{nowpage + 1} / {maxpage}";
            int j = 0;
            switch (nowslots)
            {
                case 0: //주무기
                    for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment0.Count; i++)
                    {
                        if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                        InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment0.ToList()[i].Value);
                        InventorySlots[j].ShowObject();
                        j++;
                    }
                    break;
                case 1: //주무기
                    for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment1.Count; i++)
                    {
                        if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                        InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment1.ToList()[i].Value);
                        InventorySlots[j].ShowObject();
                        j++;
                    }
                    break;
                case 2: //주무기
                    for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment2.Count; i++)
                    {
                        if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                        InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment2.ToList()[i].Value);
                        InventorySlots[j].ShowObject();
                        j++;
                    }
                    break;
                case 3: //주무기
                    for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment3.Count; i++)
                    {
                        if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                        InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment3.ToList()[i].Value);
                        InventorySlots[j].ShowObject();
                        j++;
                    }
                    break;
                case 4: //주무기
                    for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment4.Count; i++)
                    {
                        if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                        InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment4.ToList()[i].Value);
                        InventorySlots[j].ShowObject();
                        j++;
                    }
                    break;
                case 5: //주무기
                    for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment5.Count; i++)
                    {
                        if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                        InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment5.ToList()[i].Value);
                        InventorySlots[j].ShowObject();
                        j++;
                    }
                    break;
                case 6: //주무기
                    for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment6.Count; i++)
                    {
                        if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                        InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment6.ToList()[i].Value);
                        InventorySlots[j].ShowObject();
                        j++;
                    }
                    break;
                case 7: //주무기
                    for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment7.Count; i++)
                    {
                        if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                        InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment7.ToList()[i].Value);
                        InventorySlots[j].ShowObject();
                        j++;
                    }
                    break;
                case 8: //주무기
                    for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment8.Count; i++)
                    {
                        if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                        InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment8.ToList()[i].Value);
                        InventorySlots[j].ShowObject();
                        j++;
                    }
                    break;
                case 9: //주무기
                    for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment9.Count; i++)
                    {
                        if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                        InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment9.ToList()[i].Value);
                        InventorySlots[j].ShowObject();
                        j++;
                    }
                    break;
                case 10: //주무기
                    for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment10.Count; i++)
                    {
                        if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                        InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment10.ToList()[i].Value);
                        InventorySlots[j].ShowObject();
                        j++;
                    }
                    break;
                case 11: //주무기
                    for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment11.Count; i++)
                    {
                        if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                        InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment11.ToList()[i].Value);
                        InventorySlots[j].ShowObject();
                        j++;
                    }
                    break;
            }
        }

    }
    public void DownPage()
    {
        //다음페이지
        if (nowpage == 0) return;
        nowpage--;

        foreach (var t in InventorySlots)
        {
            t.RemoveObject();
        }
        PageText.text = $"{nowpage + 1} / {maxpage}";


        int j = 0;
        switch (nowslots)
        {
            case 0: //주무기
                for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment0.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment0.ToList()[i].Value);
                    InventorySlots[j].ShowObject();
                    j++;
                }
                break;
            case 1: //주무기
                for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment1.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment1.ToList()[i].Value);
                    InventorySlots[j].ShowObject();
                    j++;
                }
                break;
            case 2: //주무기
                for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment2.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment2.ToList()[i].Value);
                    InventorySlots[j].ShowObject();
                    j++;
                }
                break;
            case 3: //주무기
                for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment3.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment3.ToList()[i].Value);
                    InventorySlots[j].ShowObject();
                    j++;
                }
                break;
            case 4: //주무기
                for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment4.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment4.ToList()[i].Value);
                    InventorySlots[j].ShowObject();
                    j++;
                }
                break;
            case 5: //주무기
                for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment5.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment5.ToList()[i].Value);
                    InventorySlots[j].ShowObject();
                    j++;
                }
                break;
            case 6: //주무기
                for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment6.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment6.ToList()[i].Value);
                    InventorySlots[j].ShowObject();
                    j++;
                }
                break;
            case 7: //주무기
                for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment7.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment7.ToList()[i].Value);
                    InventorySlots[j].ShowObject();
                    j++;
                }
                break;
            case 8: //주무기
                for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment8.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment8.ToList()[i].Value);
                    InventorySlots[j].ShowObject();
                    j++;
                }
                break;
            case 9: //주무기
                for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment9.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment9.ToList()[i].Value);
                    InventorySlots[j].ShowObject();
                    j++;
                }
                break;
            case 10: //주무기
                for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment10.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment10.ToList()[i].Value);
                    InventorySlots[j].ShowObject();
                    j++;
                }
                break;
            case 11: //주무기
                for (int i = InventorySlots.Length * nowpage; i < PlayerBackendData.Instance.Equiptment11.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[j].Refresh(PlayerBackendData.Instance.Equiptment11.ToList()[i].Value);
                    InventorySlots[j].ShowObject();
                    j++;
                }
                break;
        }
    }

    //#장비칸
    public InventorySlot[] InventorySlots;
    public int nowslots = -1;
    public void ShowEquipInventory(int num)
    {
        nowslots = num;
        nowpage = 0;
        maxpage = (PlayerBackendData.Instance.GetTypeEquipment(nowslots.ToString()).Count / InventorySlots.Length) + 1;
        PageText.text = $"{nowpage + 1} / {maxpage}";
        Savemanager.Instance.GetEquipItemToInven();
        ShowInven();
    }

    public void Bt_RefreshNowEquipInven()
    {
        ShowEquipInventory_NoReset(nowslots);
    }
    
    
    private void ShowEquipInventory_NoReset(int num)
    {
        nowslots = num;
        int pagesave = nowpage;
        nowpage = 0;
        maxpage = (PlayerBackendData.Instance.Equiptment0.Count / InventorySlots.Length) + 1;
        PageText.text = $"{nowpage + 1} / {maxpage}";
        ShowInven();
        for(int i = 0; i < pagesave; i++)
        {
            UpPage();
        }
    }
    public void BT_ResetToggles()
    {
        foreach (var t in EquipToggles)
        {
            t.IsOn = false;
        }
    }


    public Sprite[] RareSprite; //0은 - 1은 방패
    public GameObject RareImageMineobj;
    public GameObject RareImageNotMineobj;
    public Image[] RareImage;
    public Image[] RareImageNotMine;

    public void ShowRareImage(string rare)
    {
        RareImageMineobj.SetActive(true);
        RareImageNotMineobj.SetActive(false);
        foreach (var t in RareImage)
        {
            t.sprite = RareSprite[0];
        }
        RareImage[int.Parse(rare)].sprite = RareSprite[1];
    }

    public void ShowRareImage_notmine(string[] rare)
    {
        RareImageMineobj.SetActive(false);
        RareImageNotMineobj.SetActive(true);
        for(int i = 0; i<  rare.Length;i++)
        {
            if (rare[i] == "0") continue;
            RareImageNotMine[0].color = ItemRare[i];
            break;
        }

        for(int i = rare.Length-1; i > 0; i--)
        {
            if (rare[i] == "0") continue;
            //Debug.Log("ㅇ숑1" + (rare.Length-1));
            //Debug.Log("ㅇ숑" + i);
            RareImageNotMine[1].color = ItemRare[i];
            break;
        }

    }

    void ShowInven()
    {
        foreach (var VARIABLE in InventorySlots)
        {
            VARIABLE.RemoveObject();
        }

        int num = 0;
        switch (nowslots)
        {
            case 0: //주무기
                for (int i = 0; i < PlayerBackendData.Instance.Equiptment0.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[num].Refresh(PlayerBackendData.Instance.Equiptment0.ToList()[i].Value);
                    InventorySlots[num].ShowObject();
                    num++;
                }
                break;
            case 1: //보조무기
             
                for (int i = 0; i < PlayerBackendData.Instance.Equiptment1.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[num].Refresh(PlayerBackendData.Instance.Equiptment1.ToList()[i].Value);
                    InventorySlots[num].ShowObject();
                    num++;
                }
                break;
            case 2: //투구
               
                for (int i = 0; i < PlayerBackendData.Instance.Equiptment2.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[num].Refresh(PlayerBackendData.Instance.Equiptment2.ToList()[i].Value);
                    InventorySlots[num].ShowObject();
                    num++;
                }
                break;
            case 3: //갑옷
           
                for (int i = 0; i < PlayerBackendData.Instance.Equiptment3.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[num].Refresh(PlayerBackendData.Instance.Equiptment3.ToList()[i].Value);
                    InventorySlots[num].ShowObject();
                    num++;

                }
                break;
            case 4: //장갑
               
                for (int i = 0; i < PlayerBackendData.Instance.Equiptment4.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[num].Refresh(PlayerBackendData.Instance.Equiptment4.ToList()[i].Value);
                    InventorySlots[num].ShowObject();
                    num++;

                }
                break;
            case 5: //주무기
          
                for (int i = 0; i < PlayerBackendData.Instance.Equiptment5.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[num].Refresh(PlayerBackendData.Instance.Equiptment5.ToList()[i].Value);
                    InventorySlots[num].ShowObject();
                    num++;

                }
                break;
            case 6: //주무기
            
                for (int i = 0; i < PlayerBackendData.Instance.Equiptment6.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[num].Refresh(PlayerBackendData.Instance.Equiptment6.ToList()[i].Value);
                    InventorySlots[num].ShowObject();
                    num++;
                }
                break;
            case 7: //주무기
              
                for (int i = 0; i < PlayerBackendData.Instance.Equiptment7.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[num].Refresh(PlayerBackendData.Instance.Equiptment7.ToList()[i].Value);
                    InventorySlots[num].ShowObject();
                    num++;
                }
                break;
            case 8: //등 
             
                for (int i = 0; i < PlayerBackendData.Instance.Equiptment8.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[num].Refresh(PlayerBackendData.Instance.Equiptment8.ToList()[i].Value);
                    InventorySlots[num].ShowObject();
                    num++;
                }
                break;
            case 9: //펫 
            
                for (int i = 0; i < PlayerBackendData.Instance.Equiptment9.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[num].Refresh(PlayerBackendData.Instance.Equiptment9.ToList()[i].Value);
                    InventorySlots[num].ShowObject();
                    num++;
                }
                break;
            case 10: //룬 
             
                for (int i = 0; i < PlayerBackendData.Instance.Equiptment10.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[num].Refresh(PlayerBackendData.Instance.Equiptment10.ToList()[i].Value);
                    InventorySlots[num].ShowObject();
                    num++;
                }
                break;
            case 11: //휘장
               
                for (int i = 0; i < PlayerBackendData.Instance.Equiptment11.Count; i++)
                {
                    if (InventorySlots.Length * (nowpage + 1) <= i) continue;
                    InventorySlots[num].Refresh(PlayerBackendData.Instance.Equiptment11.ToList()[i].Value);
                    InventorySlots[num].ShowObject();
                    num++;
                }
                break;
        }
    }

    public void ChangeItemRareColor(Text text, string rare)
    {
        text.color = rare switch
        {
            "0" => //커먼
                ItemRare[0],
            "1" => //레어
                ItemRare[1],
            "2" => //유니크
                ItemRare[2],
            "3" => //레전드
                ItemRare[3],
            "4" => //미스틱
                ItemRare[4],
            "5" => //에픽
                ItemRare[5],
            "6" => //
                ItemRare[6],
            "7" => //갓
                ItemRare[7],
            
            _ => text.color
        };
    }
    public void ChangeItemRareColorTMPro(TMPro.TextMeshProUGUI text, string rare)
    {
        text.color = rare switch
        {
            "0" => //커먼
                ItemRare[0],
            "1" => //레어
                ItemRare[1],
            "2" => //유니크
                ItemRare[2],
            "3" => //레전드
                ItemRare[3],
            "4" => //미스틱
                ItemRare[4],
            "5" => //브릴리언트
                ItemRare[5],
            "6" => //브릴리언트
                ItemRare[6],
            "7" => //브릴리언트
                ItemRare[7],
            _ => text.color
        };
    }
    public Color GetRareColor(string rare)
    {
        return ItemRare[int.Parse(rare)];
    }
    public string GetRareColorFF(string rare)
    {
        string str = "";
        switch (rare)
        {
            case "0":
                str = "#FFFFFF";
                break;
            case "1":
                str = "#9EFF00";
                break;
            case "2":
                str = "#00BDFF";
                break;
            case "3":
                str = "#FF7000";
                break;
            case "4":
                str = "#E500FF";
                break;
            case "5":
                str = "#FF003F";
                break;
            case "6":
                str = "#00FFC9";
                break;
            case "7":
                str = "#F8FF5F";
                break;
        }

        return str;
    }

    #region 장비관련

    public int nowsettype;
    public UIView EquipInventoryObj;
    public UIView ItemObj;
    public EquipDatabase data;
    //아이템
    public EquipmentItemData InvenslotData;
    public Text ItemNameText;
    public Text ItemInfoText;
    public TMPro.TextMeshProUGUI ItemStatText;
    public Text ItemSkillText;

    public GameObject[] InvenslotButtons; //장착 장착해제등

    public GameObject EquipskillPanel;
    public GameObject[] EquipSkills_Obj;
    public TMProHyperLink[] EquipSkills;
    public TMPro.TextMeshProUGUI EquipSkill_Count;
    public TMProHyperLink SetText;

    private void SetChangeEquipData(EquipDatabase data)
    {
        this.data = data;
        ismines = true;
        switch (EquipItemDB.Instance.Find_id(data.Itemid).Type)
        {
            case "Weapon":
                nowsettype = 0;
                break;
            case "SWeapon":
                nowsettype = 1;
                break;
            case "Helmet":
                nowsettype = 2;
                break;
            case "Chest":
                nowsettype = 3;
                break;
            case "Glove":
                nowsettype = 4;
                break;
            case "Boot":
                nowsettype = 5;
                break;
            case "Ring":
                nowsettype = 6;
                break;
            case "Necklace":
                nowsettype = 7;
                break;
            case "Wing":
                nowsettype = 8;
                break;
            case "Pet":
                nowsettype = 9;
                break;
            case "Rune":
                nowsettype = 10;
                break;
            case "Insignia":
                nowsettype = 11;
                break;
        }

    }

    public InventorySlot nowequipslot;
    public UIToggle LockUi;
    public Transform RefreshPanel;
    public Transform BattlePointTrans;
    public Transform BattlePointTrans1;
    public Text BattlePointText;
    public ParticleSystem aparticle;
    public ParticleSystem bparticle;

    public bool ismines;
    public void ShowInventoryItem(EquipDatabase  data,bool ismine = true)
    {
        if (ismine)
            this.data = data;

        ismines = ismine;
        
//        Debug.Log("이즈마인" + ismine);
        InvenslotData.Refresh(data);
        ItemNameText.text = data.GetItemName();
        ShowRareImage(data.Itemrare);
        Inventory.Instance.ChangeItemRareColor(ItemNameText, data.Itemrare);

        ItemInfoText.text = data.GetInfo();
        var aparticleMain = aparticle.main;
        aparticleMain.startColor = ItemRare[int.Parse(data.Itemrare)];
        var bparticleMain = bparticle.main;
        bparticleMain.startColor = ItemRare[int.Parse(data.Itemrare)];
        ItemStatText.text = data.GetItemStat();

        //장비점수 
        BattlePointText.text = data.GetBattlePoint().ToString("N0");
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)BattlePointTrans);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)BattlePointTrans1);
        nowsettype = EquipItemDB.Instance.Find_id(data.Itemid).Type switch
        {
            "Weapon" => 0,
            "SWeapon" => 1,
            "Helmet" => 2,
            "Chest" => 3,
            "Glove" => 4,
            "Boot" => 5,
            "Ring" => 6,
            "Necklace" => 7,
            "Wing" => 8,
            "Pet" => 9,
            "Rune" => 10,
            "Insignia" => 11,

            _ => nowsettype
        };

        foreach (var t in InvenslotButtons)
        {
            if(t != null)
                t.SetActive(false);
        }

        if (ismine && !SuccManager.Instance.IsSucc)
        {
            //장착중인 장비가 있다면.
            if (PlayerBackendData.Instance.GetEquipData()[nowsettype] != null)
            {
                //해당 장비가 있다면.
                
                //키아이디가 같으면 현재 장착중인 장비를 고른거다.
                if (PlayerBackendData.Instance.GetEquipData()[nowsettype].KeyId1 == data.KeyId1)
                {
                   // if (nowsettype != 0) //무기가 아니면 장착해제가 보이지 않음 
                        //InvenslotButtons[(int)invenslotenumbutton.장착해제].SetActive(true);
                }
                else
                {
                    //같은 장비가 아니라면 장착이 뜬다.
                    InvenslotButtons[(int)invenslotenumbutton.장착].SetActive(true);
                    InvenslotButtons[(int)invenslotenumbutton.장비버리기].SetActive(true);
                    int esnum = 0;
                    if (EquipItemDB.Instance.Find_id(data.Itemid).SpeMehodP != "0")
                    {
                        esnum = 1;
                    }

                    if (data.EquipSkill1.Count - esnum > 0)
                    {
//                        Debug.Log("응애");
                        InvenslotButtons[(int)invenslotenumbutton.전승].SetActive(true);
                    }
                    
                }
            }
            else
            {
                InvenslotButtons[(int)invenslotenumbutton.장착].SetActive(true);
                InvenslotButtons[(int)invenslotenumbutton.장비버리기].SetActive(true);
                int esnum = 0;
                if (EquipItemDB.Instance.Find_id(data.Itemid).SpeMehodP != "0")
                {
                    esnum = 1;
                }

                if (data.EquipSkill1.Count - esnum > 0)
                {
                    InvenslotButtons[(int)invenslotenumbutton.전승].SetActive(true);
                }
            }

            //재설정
            equipoptionchanger.Instance.Checkpanel();

            
            //Debug.Log(nowsettype);
            LockUi.gameObject.SetActive(true);
            LockUi.IsOn =
                PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[data.KeyId1]
                    .IsLock;


            //재련확인
            //Debug.Log(nowsettype);
            if (data.MaxStoneCount1 != 0 && data.StoneCount1 != data.MaxStoneCount1)
            {
                //제련가능횟수가 1이라도 남으면
                InvenslotButtons[(int)invenslotenumbutton.제련].SetActive(true);
            }
            else
            {
                InvenslotButtons[(int)invenslotenumbutton.제련].SetActive(false);
            }

            //제련횟수가 1회라도있으면 초기화가능
            if (data.MaxStoneCount1 != 0 && data.StoneCount1 != 0)
            {
                //제련가능횟수가 1이라도 남으면
                InvenslotButtons[(int)invenslotenumbutton.제련초기화].SetActive(true);
            }
            else
            {
                InvenslotButtons[(int)invenslotenumbutton.제련초기화].SetActive(false);
            }
        }
        else
        {
            LockUi.gameObject.SetActive(false);
        }

        EquipSkill_Count.gameObject.SetActive(false);
        foreach (var d in EquipSkills_Obj)
            d.gameObject.SetActive(false);

        //특수효과
        if (data.IshaveEquipSkill)
        {
//           Debug.Log("이쿠비 개수" + data.EquipSkill1.Count);
            for (int i = 0; i < data.EquipSkill1.Count; i++)
            {
                EquipSkills[i].SetEquipSkill(data.EquipSkill1[i]);
                EquipSkills_Obj[i].gameObject.SetActive(true);
            }

            EquipskillPanel.SetActive(true);
        }
        else
        {
            EquipskillPanel.SetActive(false);
        }

        if (EquipItemDB.Instance.Find_id(data.Itemid).SpeMehod != "0")
        {
            EsSkillShowButton.SetActive(true);
        }
        else
        {
            EsSkillShowButton.SetActive(false);
        }
        
        EquipItemDB.Row datas = EquipItemDB.Instance.Find_id(data.Itemid);

        if (datas.SetID != "0")
        {
            SetText.SetSetName(datas.id);
            SetText.gameObject.SetActive(true);
            EquipskillPanel.SetActive(true);
        }
        else
        {
            SetText.gameObject.SetActive(false);
        }



        ItemObj.Show(false);
        ItemObj.transform.SetAsLastSibling();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)RefreshPanel.transform);
    }

    public GameObject EsSkillShowButton;
    public void ShowInventoryItem_NotMine(EquipDatabase data)
    {
        
        this.data = data;
        ismines = false;
        InvenslotData.Refresh(data);
        ItemNameText.text = data.GetItemNameNotMine();
        Inventory.Instance.ChangeItemRareColor(ItemNameText, data.Itemrare);
        ShowRareImage_notmine(EquipItemDB.Instance.Find_id(data.Itemid).RarePercent.Split(';'));
        ItemInfoText.text = data.GetInfoNotMine();

        ItemStatText.text = data.GetItemStatUnReal();

        //장비점수 
        BattlePointText.text = data.GetBattlePointNotMine().ToString("N0");
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)BattlePointTrans);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)BattlePointTrans1);
        
        nowsettype = EquipItemDB.Instance.Find_id(data.Itemid).Type switch
        {
            "Weapon" => 0,
            "SWeapon" => 1,
            "Helmet" => 2,
            "Chest" => 3,
            "Glove" => 4,
            "Boot" => 5,
            "Ring" => 6,
            "Necklace" => 7,
            "Wing" => 8,
            "Pet" => 9,
            "Rune" => 10,
            "Insignia" => 10,
            _ => nowsettype
        };

        foreach (var t in InvenslotButtons)
        {
            if (t != null)
                t.SetActive(false);
        }

        EquipItemDB.Row datas = EquipItemDB.Instance.Find_id(data.Itemid);
        //특수효과

        EquipskillPanel.SetActive(false);
        EquipSkill_Count.gameObject.SetActive(false);

        foreach (var t in EquipSkills_Obj)
            t.gameObject.SetActive(false);

        //고유효과가있다면
        if (datas.SpeMehodP != "0")
        {
            EquipSkills[0].SetEquipSkill(datas.SpeMehodP);
            EquipSkills_Obj[0].gameObject.SetActive(true);
            
            EquipskillPanel.SetActive(true);
        }


        if (datas.SpeMehod != "0")
        {
            //Debug.Log(datas.SpeMehod);
            EquipSkill_Count.text = string.Format(GetTranslate("UI/특수효과개수"), "0", EquipSkillRandomGiveDB.Instance.Find_id(datas.SpeMehod).A);
            EquipSkill_Count.gameObject.SetActive(true);
            EsSkillShowButton.SetActive(true);
        }
        else
        {
            EsSkillShowButton.SetActive(false);
        }

        if(datas.SetID != "0")
        {
            SetText.SetSetName(datas.id) ;
            SetText.gameObject.SetActive(true);
            EquipskillPanel.SetActive(true);
        }
        else
        {
            SetText.gameObject.SetActive(false);
        }


        LockUi.gameObject.SetActive(false);
        ItemObj.Show(false);
        ItemObj.transform.SetAsLastSibling();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)RefreshPanel.transform);
        CheckBox();
    }

    public enum invenslotenumbutton
    {
        장착,
        장착해제,
        분해,
        제련,
        제련초기화,
        재설정,
        장비버리기,
        전승,
        Length
    }
         

    public equipitemslot[] EquipSlots;
    //장비장착
    public void Bt_EquipItem()
    {
        
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        
        
        if (PartyRaidRoommanager.Instance.partyroomdata.isstart)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/콘텐츠 중 불가능"), alertmanager.alertenum.주의);
            return;
        }
        
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/사냥터만가능"), alertmanager.alertenum.주의);
            return;
        }
       
        //현재 장비를 장착
        //아이템이있다
        if (EquipSlots[nowsettype].data != null)
        {
            if (EquipSlots[nowsettype].data.KeyId1 != "True")
            {
                EquipSlots[nowsettype].data.IsEquip = false;
                PlayerBackendData.Instance.GetEquipData()[nowsettype].IsEquip = false;
                PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[
                    EquipSlots[nowsettype].data.KeyId1].IsEquip = false;
            }
        }
        
    
        

        data.IsEquip = true;
        EquipSlots[nowsettype].SetItem(this.data);
        PlayerBackendData.Instance.GetEquipData()[nowsettype] = this.data;
        
        switch (EquipItemDB.Instance.Find_id(data.Itemid).Type)
        {
            case "Weapon":
                PlayerData.Instance.SetWeaponImage(SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(data.Itemid).EquipSprite), EquipItemDB.Instance.Find_id(data.Itemid).EquipSprite);
                PlayerData.Instance.SetMainWeaponRare(data.CraftRare1);
                mainplayer.InitAttackData(); //공격 횟수 설정
                PartyraidChatManager.Instance.Chat_ChangeVisual();

                switch (EquipItemDB.Instance.Find_id(data.Itemid).attacktype)
                {
                    case "0":
                        mainplayer.attacktype = AttackType.Melee;
                        EquipItemDB.Row data = EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid);
                        //히트사운드
                        if (data.HitSound != "")
                        {
                            mainplayer.attacktrigger = data.HitSound.Split(';');
                        }
                        if (data.HitEffect != "")
                        {
                            mainplayer.basicattackhit = data.HitEffect;
                        }
                        break;
                    case "1":
                        mainplayer.attacktype = AttackType.Range;
                        mainplayer.InitArrow();
                        mainplayer.basicattackhit = "";
                        break;

                }
                break;
            case "SWeapon":
                PlayerData.Instance.SetSubWeaponImage(SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(data.Itemid).EquipSprite), EquipItemDB.Instance.Find_id(data.Itemid).EquipSprite);
                PlayerData.Instance.SetSubWeaponRare(data.CraftRare1);
                PartyraidChatManager.Instance.Chat_ChangeVisual();

                break;
        }
        EquipSetmanager.Instance.EquipSetItem();
        Savemanager.Instance.SaveTypeEquip();
        Savemanager.Instance.Save();
        PlayerData.Instance.RefreshPlayerstat();
       // mainplayer.ClassStat();
        PlayerData.Instance.RefreshPlayerstat_Equip();

        //가이드 퀘스트
        
        if (PlayerBackendData.Instance.tutoid != Tutorialmanager.Instance.maxlv)
        {
            Tutorialmanager.Instance.CheckTutorial("equipitem");
            if (PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5500") ||
                PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5501") ||
                PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5502") ||
                PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5503") ||
                PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5504") ||
                PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5505") ||
                PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5506") ||
                PlayerBackendData.Instance.GetEquipData()[0].Itemid.Equals("E5507"))
            {
                Tutorialmanager.Instance.CheckTutorial("equipitemraze");
            }
        }

        ItemObj.Hide(false);
        EquipInventoryObj.Hide(false);
    }
    public void EquipItem_NoSave()
    {
        //현재 장비를 장착
        //아이템이있다
        if (EquipSlots[nowsettype].data != null)
        {
            EquipSlots[nowsettype].data.IsEquip = false;
            PlayerBackendData.Instance.GetEquipData()[nowsettype].IsEquip = false;
            PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[EquipSlots[nowsettype].data.KeyId1].IsEquip = false;
        }

        data.IsEquip = true;
        EquipSlots[nowsettype].SetItem(this.data);
        PlayerBackendData.Instance.GetEquipData()[nowsettype] = this.data;
        switch (EquipItemDB.Instance.Find_id(data.Itemid).Type)
        {
            case "Weapon":
                PlayerData.Instance.SetWeaponImage(SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(data.Itemid).EquipSprite), EquipItemDB.Instance.Find_id(data.Itemid).EquipSprite);
                PlayerData.Instance.SetMainWeaponRare(data.CraftRare1);
                mainplayer.InitAttackData(); //공격 횟수 설정

                switch (EquipItemDB.Instance.Find_id(data.Itemid).attacktype)
                {
                    case "0":
                        mainplayer.attacktype = AttackType.Melee;
                     EquipItemDB.Row data = EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid);
                        //히트사운드
                        if (data.HitSound != "")
                        {

                           mainplayer.attacktrigger = data.HitSound.Split(';');
                        }
                        break;
                    case "1":
                        mainplayer.attacktype = AttackType.Range;
                        mainplayer.InitArrow();
                        break;

                }
                break;
            case "SWeapon":
                PlayerData.Instance.SetSubWeaponImage(SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(data.Itemid).EquipSprite), EquipItemDB.Instance.Find_id(data.Itemid).EquipSprite);
                PlayerData.Instance.SetSubWeaponRare(data.CraftRare1);
                break;
        }
        EquipSetmanager.Instance.EquipSetItem();
    }

    //프리셋 장비장착
    public void EquipItem_Preset(string keyid, string type)
    {
        if (PlayerBackendData.Instance.GetTypeEquipment(type).ContainsKey(keyid))
        {
            data = PlayerBackendData.Instance.GetTypeEquipment(type)[keyid];
            int nowtype = int.Parse(type);
            //현재 장비를 장착
            //아이템이있다
            if (EquipSlots[nowtype].data != null)
            {
                EquipSlots[nowtype].data.IsEquip = false;
                PlayerBackendData.Instance.GetEquipData()[nowtype].IsEquip = false;
                PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[
                    EquipSlots[nowtype].data.KeyId1].IsEquip = false;
            }

            data.IsEquip = true;
            EquipSlots[nowtype].SetItem(this.data);
            PlayerBackendData.Instance.GetEquipData()[nowtype] = this.data;
            switch (EquipItemDB.Instance.Find_id(data.Itemid).Type)
            {
                case "Weapon":
                    PlayerData.Instance.SetWeaponImage(
                        SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(data.Itemid).EquipSprite),
                        EquipItemDB.Instance.Find_id(data.Itemid).EquipSprite);
                    PlayerData.Instance.SetMainWeaponRare(data.CraftRare1);
                    mainplayer.InitAttackData(); //공격 횟수 설정

                    switch (EquipItemDB.Instance.Find_id(data.Itemid).attacktype)
                    {
                        case "0":
                            mainplayer.attacktype = AttackType.Melee;
                            EquipItemDB.Row data =
                                EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid);
                            //히트사운드
                            if (data.HitSound != "")
                            {

                                mainplayer.attacktrigger = data.HitSound.Split(';');
                            }

                            break;
                        case "1":
                            mainplayer.attacktype = AttackType.Range;
                            mainplayer.InitArrow();
                            break;
                    }
                    break;
                case "SWeapon":
                    PlayerData.Instance.SetSubWeaponImage(
                        SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(data.Itemid).EquipSprite),
                        EquipItemDB.Instance.Find_id(data.Itemid).EquipSprite);
                    PlayerData.Instance.SetSubWeaponRare(data.CraftRare1);
                    break;
            }

        }
    }



    //장착슬릇 가져오기
    public equipitemslot GetEquipSlots(string type)
    {
        switch (type)
        {
            case "Weapon":
                return EquipSlots[0];
            case "SWeapon":
                return EquipSlots[1];
            case "Helmet":
                return EquipSlots[2];
            case "Chest":
                return EquipSlots[3];
            case "Glove":
                return EquipSlots[4];
            case "Boot":
                return EquipSlots[5];
            case "Ring":
                return EquipSlots[6];
            case "Necklace":
                return EquipSlots[7];
            case "Wing":
                return EquipSlots[8];
            case "Pet":
                return EquipSlots[9];
            case "Rune":
                return EquipSlots[10];
            case "Insignia":
                return EquipSlots[11];
            default:
                return EquipSlots[10];

        }
    }
    public void LockItem()
    { 
        data.IsLock = LockUi.IsOn;
        nowequipslot.Refresh(data);
        Savemanager.Instance.SaveTypeEquip();
    }
    //장착해제
    public void Bt_UnEquipItem()
    {
        //장비를 모두 제외
        PlayerBackendData.Instance.GetEquipData()[nowsettype] = null;
        PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[EquipSlots[nowsettype].data.KeyId1].IsEquip = false;
        EquipSlots[nowsettype].data = null;
        Savemanager.Instance.SaveTypeEquip();
        mainplayer.ClassStat();
        ItemObj.Hide(false);
        EquipSetmanager.Instance.EquipSetItem();
        PlayerData.Instance.RefreshPlayerstat_Equip();
        EquipInventoryObj.Hide(false);
    }

    public UIView RemoveItemPanel;
    public void Bt_RemoveGearItem()
    {
        if (data.IsLock)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/잠금장비불가"),alertmanager.alertenum.주의);
            return;
        }
        
        //장비를 모두 제외
        RemoveItemPanel.Show(true);
    }

    public void Bt_AcceptRemoveItem()
    {
        RemoveItemPanel.Hide(true);
        PlayerBackendData.Instance.GetTypeEquipment(nowsettype.ToString()).Remove(data.KeyId1);
        Savemanager.Instance.SaveTypeEquip();
        Savemanager.Instance.Save();
        ItemObj.Hide(false);
        ShowEquipInventory(nowsettype);
    }

    public void RemoveItem(string keyid)
    {
        PlayerBackendData.Instance.GetTypeEquipment(nowsettype.ToString()).Remove(keyid);
        Savemanager.Instance.SaveTypeEquip();
        Savemanager.Instance.Save();
        ItemObj.Hide(false);
    }
    public ParticleSystem[] SmeltEffect;
    string smeltneeditemid;
    int smeltneeditemhowmany;
    [SerializeField]
    UIView SmeltPanel;
    [SerializeField]
    Image SmeltItemImage;
    [SerializeField]
    Text SmeltItemhowmany;
    [SerializeField]
    Text SuccessPercentText;
    int ss = 50;
    [SerializeField]
    int ess = 0;

    public bool iscansmelt = false;
    public  bool smeltdelaybool = false; //true면 불가능 
    public void Bt_SmeltStartButton()
    {
        if (data.IsLock)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/잠금장비불가"),alertmanager.alertenum.주의);
            return;
        }
        
        if (smeltdelaybool)
        {
            return;
        }
        SmeltPanel.Show(false);
        iscansmelt = false;
        smeltdelaybool = false;
        string itemid = EquipItemDB.Instance.Find_id(data.Itemid).smeltid;
        //제련필요개수
        int count = int.Parse(EquipItemDB.Instance.Find_id(data.Itemid).smeltcount);
        AutoSmelting.Instance.SmeltingCount = count;
        int index = PlayerBackendData.Instance.GetItemIndex(itemid);



        SmeltItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(itemid).sprite);
        if (ess != 0)
        {
            //이벤트 확률 있다
            SuccessPercentText.text = string.Format(Inventory.GetTranslate("UI/제련성공확률이벤트"),ss+ess, ess);

        }
        else
        {
            SuccessPercentText.text = string.Format(Inventory.GetTranslate("UI/제련성공확률"), ss + ess);
        }

        if (index == -1)
        {
            //아이템이 없습니다.
            SmeltItemhowmany.color = Color.red;
            SmeltItemhowmany.text = $"{PlayerBackendData.Instance.CheckItemCount(itemid)} / {count}";
            iscansmelt = false;
        }
        else
        {
            if (PlayerBackendData.Instance.CheckItemCount(itemid) >= count)
            {
                //아이템 있음가능
                SmeltItemhowmany.text = $"{PlayerBackendData.Instance.CheckItemCount(itemid)} / {count}";
                SmeltItemhowmany.color = Color.cyan;
                iscansmelt = true;
            }
            else
            {
                //부족
                SmeltItemhowmany.color = Color.red;
                SmeltItemhowmany.text = $"{0} / {count}";
                iscansmelt = false;

            }
        }
        //제련아이템 개수 정하기및 필요 아이템
    }
    //제련
    private bool issuccsmelt;
    private bool isbigsuccsmelt=  false;
    public bool istrue = true;
    public void Bt_SmeltEquipItem()
    {
        if (smeltdelaybool)
        {
            return;
        }
        
        if(!Settingmanager.Instance.justcheck())
            return;
        

        SmeltEffect[0].gameObject.SetActive(false);
        SmeltEffect[1].gameObject.SetActive(false);
        //현재 장비를 장착
        //아이템이있다
        //제련아이디
        Debug.Log(EquipItemDB.Instance.Find_id(data.Itemid).smeltid);
        string itemid = EquipItemDB.Instance.Find_id(data.Itemid).smeltid;
        //제련필요개수
        int count = int.Parse(EquipItemDB.Instance.Find_id(data.Itemid).smeltcount);

        int index = PlayerBackendData.Instance.GetItemIndex(itemid);

        if (iscansmelt)
        {
            Soundmanager.Instance.PlayerSound("Sound/강화확인전");
            smeltdelaybool = true;
            //아이템 있음가능
            float temp = Time.time * 100f;
            int seed = (int)temp + PlayerBackendData.Instance.GetRandomSeed();
            Random.InitState(seed);
            int rn = Random.Range(1, 101);
            if (rn <= (ss + ess))
            {
                //성공
                int rn2 = Random.Range(1, 101);
                if (rn2 <= (10) && data.StoneCount1 < 8)
                {
                    data.BigSuccessSmelt();
                    isbigsuccsmelt = true;

                }
                else
                {
                    data.SuccessSmelt();
                    isbigsuccsmelt = false;

                }
                
                SmeltEffect[0].gameObject.SetActive(true);
                issuccsmelt = true;
                //높다면 채팅알림
            }
            else
            {
                //실패
                SmeltEffect[1].gameObject.SetActive(true);
                issuccsmelt = false;
                isbigsuccsmelt = false;
                data.FailSmelt();
            }

            //자동 제련 중이라면
            if (AutoSmelting.Instance.SmeltingingObj.IsActive())
            {
                if (isbigsuccsmelt)
                {
                    AutoSmelting.Instance.AutoSmeltinginfo.text = GetTranslate("UI3/대성공");
                }
                else if (issuccsmelt)
                {
                    AutoSmelting.Instance.AutoSmeltinginfo.text = GetTranslate("UI3/성공");

                }
                else
                {
                    AutoSmelting.Instance.AutoSmeltinginfo.text = GetTranslate("UI3/실패");
                }
                AutoSmelting.Instance.UsedItem += count;
            }

            if (istrue)
            {
                Where where = new Where();
                where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
                Param param = new Param { { "PlayerCanLoadBool", "false" } };
                istrue = false;
                SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
                {
                    if (callback.IsSuccess())
                    {
                        Settingmanager.Instance.RecentServerDataCanLoadText.text =istrue
                            ? string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"),
                                istrue
                                    ? "<Color=lime>On</color>"
                                    : "<Color=red>Off</color>")
                            : string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"), "<Color=red>Off</color>");
                        LogManager.UserSmeltCheck("체크_제련");
                    }
                });
            }
            
            PlayerBackendData.Instance.RemoveItem(itemid, count);
            Invoke(nameof(refreshterm), 0.8f);
            equipoptionchanger.Instance.RefreshEquipEquipGear();
            Savemanager.Instance.SaveTypeEquip();
            Savemanager.Instance.SaveInventory();
            ShowEquipInventory_NoReset(nowslots);
            Savemanager.Instance.Save();
            mainplayer.ClassStat();
            SmeltPanel.Hide(false);
        }
        else
        {
            //부족
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/제련불가능"), alertmanager.alertenum.주의);
        }

        //EquipInventoryObj.Hide(false);
    }
    void refreshterm()
    {
        ShowInventoryItem(data);
        if (isbigsuccsmelt)
        {
            Soundmanager.Instance.PlayerSound("Sound/긍정업");
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/제련대성공"), alertmanager.alertenum.주의);
        }
        else if (issuccsmelt)
        {
            Soundmanager.Instance.PlayerSound("Sound/강화성공");
        }
        else
        {
            Soundmanager.Instance.PlayerSound("Sound/강화실패");
        }

        Invoke(nameof(ResetSmeltAll), 1.3f);
    }
    void ResetSmeltAll()
    {
        SmeltEffect[0].gameObject.SetActive(false);
        SmeltEffect[1].gameObject.SetActive(false);
        iscansmelt = false;
        smeltdelaybool = false;
    }

    public void Bt_SmeltEquipItem2()
    {
        //현재 장비를 장착
        //아이템이있다
        data.FailSmelt();
        ShowInventoryItem(data);
        Savemanager.Instance.SaveTypeEquip();
        mainplayer.ClassStat();
        ItemObj.Hide(false);
        EquipInventoryObj.Hide(false);
    }


    //제련 초기화
    [SerializeField]
    UIView SmeltResetPanel;

    public void Button_ShowResetSmelt()
    {
        
        if (data.IsLock)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/잠금장비불가"),alertmanager.alertenum.주의);
            return;
        }
        if(!Settingmanager.Instance.justcheck())
            return;
        
        SmeltResetPanel.Show(false);
    }
    public void Bt_StartResetSmelt()
    {
        if(!Settingmanager.Instance.justcheck())
            return;
        
        if (PlayerBackendData.Instance.GetCash() >= 1000)
        {
            PlayerData.Instance.DownCash(1000);
            Savemanager.Instance.SaveCash();
            data.ResetSmelt();
            ShowInventoryItem(data);
            equipoptionchanger.Instance.RefreshEquipEquipGear();
            Savemanager.Instance.SaveTypeEquip();
            mainplayer.ClassStat();
            //ItemObj.Hide(false);
            EquipInventoryObj.Hide(false);
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/제련초기화완료"), alertmanager.alertenum.일반);
            SmeltResetPanel.Hide(false);
        }
        else
        {
            
            
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/불꽃부족"), alertmanager.alertenum.주의);
            SmeltResetPanel.Hide(false);
        }
    }

    [Button(Name = "장착장비설정")]
    public void SetEquipDataLoad()
    {
        for (int i = 0; i < PlayerBackendData.Instance.GetEquipData().Length; i++)
        {
            if (PlayerBackendData.Instance.GetEquipData()[i] == null) continue;
            //장비가 있다면
            this.data = PlayerBackendData.Instance.GetEquipData()[i];
            nowsettype = EquipItemDB.Instance.Find_id(data.Itemid).Type switch
            {
                "Weapon" => 0,
                "SWeapon" => 1,
                "Helmet" => 2,
                "Chest" => 3,
                "Glove" => 4,
                "Boot" => 5,
                "Ring" => 6,
                "Necklace" => 7,
                "Wing" => 8,
                "Pet" => 9,
                "Rune" => 10,
                "Insignia" => 11,
                _ => nowsettype
            };

            EquipItem_NoSave();
        }

        mainplayer.ClassStat();
        PlayerData.Instance.RefreshPlayerstat_Equip();
        PartyraidChatManager.Instance.Chat_ChangeVisual();

        if (ItemObj.isActiveAndEnabled)
            ItemObj.Hide(false);
        if (EquipInventoryObj.isActiveAndEnabled)
            EquipInventoryObj.Hide(false);
    }

    public void Bt_RefreshEquipslots()
    {
        foreach (var t in EquipSlots)
        {
            if (t != null)
            {
                if (t.data != null)
                {
                    t.Refresh();
                }
            }
        }
    }    
    public void Bt_Preset(int num)
    {
        if(!Settingmanager.Instance.justcheck())
            return;
        
        PlayerBackendData.Instance.nowpreset = num;

        EquipDatabase[] ed = PlayerBackendData.Instance.GetEquipData();

        for (int i = 0; i < EquipSlots.Length; i++)
        {
            if (EquipSlots[i] != null)
            {
                EquipSlots[i].data = null;
                if (ed[i] != null)
                {
                    SetChangeEquipData(ed[i]);
                    if (PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type).ContainsKey(ed[i].KeyId1))
                    {
                        //해당 아이템이 있다면
                        Bt_EquipItem();
                    }
                    else
                    {
                        //해당 아이템이 없다면
                        PlayerBackendData.Instance.GetEquipData()[i] = null; //아이템을 지운다.
                        EquipSlots[i].Refresh();

                    }
                }
            }
        }
        
        Savemanager.Instance.SaveEquip();
    }

    #endregion

    public iteminventoryslot ItemInvenslotobj;
    public List<iteminventoryslot> iteminvenslots = new List<iteminventoryslot>();
    public int nowinvencount;
    public Transform inventrans;
    public UIView InventoryPanel;
    public UIToggle[] InventoryToggles;
    public void RefreshInventory(int num = -1)
    {
        if(!InventoryPanel.IsShowing)
        {
         
        }
        
        //숫자가 있으면 그거를 연다
        if (num != -1)
            InventoryToggles[num].IsOn = true;
       
        
      //아이템인벤이없다면 새로 추가
        if (nowinvencount < PlayerBackendData.Instance.ItemInventory.Count)
        {
            for (int i = 0; i < PlayerBackendData.Instance.ItemInventory.Count - nowinvencount; i++)
            {
                iteminventoryslot item = Instantiate(ItemInvenslotobj, inventrans);
                iteminvenslots.Add(item);
                item.gameObject.SetActive(false);
            }
            nowinvencount = PlayerBackendData.Instance.ItemInventory.Count;
        }
        //아이템을 가림
        foreach (var t in iteminvenslots)
        {
            t.gameObject.SetActive(false);
        }
        SortInventory();

        //전체
        for (int i = 0; i < PlayerBackendData.Instance.ItemInventory.Count; i++)
        {
            switch (PlayerBackendData.Instance.ItemInventory[i].Id)
            {
                case "996":
                case "997": 
                    break;
                default:
                    iteminvenslots[i].Refresh(PlayerBackendData.Instance.ItemInventory[i].Id, PlayerBackendData.Instance.ItemInventory[i].Howmany);
                    break;
            }
        }

        if (InventoryToggles[0].IsOn)
        {
            //전체
            for (int i = 0; i < PlayerBackendData.Instance.ItemInventory.Count; i++)
            {
                switch (PlayerBackendData.Instance.ItemInventory[i].Id)
                {
                    case "996":
                    case "997": 
                        break;
                    default:
                        iteminvenslots[i].gameObject.SetActive(true);
                        break;
                }
            }
        }
        else
        {
              //  Debug.Log("전체아니다");
            if (InventoryToggles[1].IsOn)
            {
                //소모품
                //전체
                for (int i = 0; i < PlayerBackendData.Instance.ItemInventory.Count; i++)
                {
                    if (ItemdatabasecsvDB.Instance.Find_id(PlayerBackendData.Instance.ItemInventory[i].Id).itemtype == "2") //2는 소모품이다
                    {
                        switch (PlayerBackendData.Instance.ItemInventory[i].Id)
                        {
                            case "996":
                            case "997": 
                                break;
                            default:
                                iteminvenslots[i].gameObject.SetActive(true);
                                break;
                        }
                    }
                }
            }
            else if (InventoryToggles[2].IsOn)
            {
                //소모품
                //전체
                for (int i = 0; i < PlayerBackendData.Instance.ItemInventory.Count; i++)
                {
                    if (ItemdatabasecsvDB.Instance.Find_id(PlayerBackendData.Instance.ItemInventory[i].Id).itemtype == "1") //1은 재료이다
                    {
                        switch (PlayerBackendData.Instance.ItemInventory[i].Id)
                        {
                            case "996":
                            case "997": 
                                break;
                            default:
                                iteminvenslots[i].gameObject.SetActive(true);
                                break;
                        }
                    }
                }
            }
            else if (InventoryToggles[3].IsOn)
            {
                //소모품
                //전체
                for (int i = 0; i < PlayerBackendData.Instance.ItemInventory.Count; i++)
                {
                    if (ItemdatabasecsvDB.Instance.Find_id(PlayerBackendData.Instance.ItemInventory[i].Id).itemtype == "3") //3은 기타
                    {
                        switch (PlayerBackendData.Instance.ItemInventory[i].Id)
                        {
                            case "996":
                            case "997": 
                                break;
                            default:
                                iteminvenslots[i].gameObject.SetActive(true);
                                break;
                        }
                    }
                }
            }
            else if (InventoryToggles[4].IsOn)
            {
                //소모품
                //전체
                for (int i = 0; i < PlayerBackendData.Instance.ItemInventory.Count; i++)
                {
                    if (ItemdatabasecsvDB.Instance.Find_id(PlayerBackendData.Instance.ItemInventory[i].Id).itemtype == "4") //3은 기타
                    {
                        switch (PlayerBackendData.Instance.ItemInventory[i].Id)
                        {
                            case "996":
                            case "997": 
                                break;
                            default:
                                iteminvenslots[i].gameObject.SetActive(true);
                                break;
                        }
                    }
                }
            }
            else if (InventoryToggles[5].IsOn)
            {
                //소모품
                //전체
                for (int i = 0; i < PlayerBackendData.Instance.ItemInventory.Count; i++)
                {
                    if (ItemdatabasecsvDB.Instance.Find_id(PlayerBackendData.Instance.ItemInventory[i].Id).itemsubtype == "116") //4는 이벤트
                    {
                        switch (PlayerBackendData.Instance.ItemInventory[i].Id)
                        {
                            case "996":
                            case "997": 
                                break;
                            default:
                                iteminvenslots[i].gameObject.SetActive(true);
                                break;
                        }
                    }
                }
            }
        }
        

        //아이템 목록 출력
       
        if(!InventoryPanel.IsActive())
        InventoryPanel.Show(false);
    }

    public void Input_SearchItem(string name)
    {
        //아이템을 가림
        foreach (var t in iteminvenslots)
        {
            t.gameObject.SetActive(false);
        }

     
        //해당 아이템만 보이게끔함
        for(int i = 0; i < PlayerBackendData.Instance.ItemInventory.Count;i++)
        {
            //이름이 포함돼있다면 스페이스 포함
            if (Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(PlayerBackendData.Instance.ItemInventory[i].Id).name).Contains(name))
            {
                iteminvenslots[i].gameObject.SetActive(true);
            }
        }
    }

    public void SortInventory()
    {
        PlayerBackendData.Instance.ItemInventory.Sort(delegate (ItemInven c1, ItemInven c2) { return c1.Id.CompareTo(c2.Id); });
    }

       public void AddItemDungeon(string id, decimal count, bool issave = true, bool ismonkill = false)
    {
        ItemdatabasecsvDB.Row item = ItemdatabasecsvDB.Instance.Find_id(id);
//            Debug.Log(id);
        if (item.itemtype == "0")
        {
            switch (item.id)
            {
                case "1000": //골드
                    if (ismonkill)
                    {
                        PlayerData.Instance.UpGoldMon((decimal)count);
                    }
                    else
                        PlayerData.Instance.UpGold((decimal)count);
                    break;
                case "1001": //불꽃
                    PlayerData.Instance.UpCash((decimal)count);
                    break;
                case "1002": //경험치
                    if (ismonkill)
                    {
                        PlayerData.Instance.EarnExp((decimal)count, false);
                    }
                    else
                    {
                        PlayerData.Instance.EarnExp((decimal)count);
                    }
                    break;
                case "1011": //경험치
                        PlayerData.Instance.EarnExpNoPre((decimal)count);
                    break;
                case "999": //업적경험치
                    PlayerData.Instance.EarnAchExp((decimal)count);
                    break;
                case "998": //시즌패스경험치
                    if (PlayerBackendData.Instance.SeasonPassExp >= 100000)
                    {
                        PlayerBackendData.Instance.SeasonPassExp = 100000;
                        SeasonPass.Instance.LevelButton.SetActive(false);
                        return;
                    }
                    PlayerData.Instance.EarnSeaSonPassExp(count);
                    if (SeasonPass.Instance.Panel.IsActive())
                    {
                        SeasonPass.Instance.RefreshSeasonItem();
                        SeasonPass.Instance.Refresh();
                    }
                    break;
            }
        }
        else if (item.itemsubtype == "403")
        {
            int n = int.Parse(AvartaDB.Instance.Find_id(ItemdatabasecsvDB.Instance.Find_id(id).A).num);

            avatamanager.Instance.EarnShowAvata(ItemdatabasecsvDB.Instance.Find_id(id).A);
            PlayerBackendData.Instance.playeravata[n] = true;
            //저장
            PlayerData.Instance.RefreshPlayerstat();
            Savemanager.Instance.SaveInventory();
            Savemanager.Instance.SaveAvataData();
            Savemanager.Instance.Save();
        }
        else
        {
            PlayerBackendData.Instance.Additem(id, (int)count);
            if (InventoryPanel.isActiveAndEnabled)
            {
                RefreshInventory();
            }
        }

        if (issave)
            Savemanager.Instance.SaveInventory_SaveOn();
        // RefreshInventory();
    }
       
    public void AddItem(string id, int count, bool issave = true, bool ismonkill = false)
    {
        ItemdatabasecsvDB.Row item = ItemdatabasecsvDB.Instance.Find_id(id);
//            Debug.Log(id);
        batterysaver.Instance.AddItem(id,count);
        if (item.itemtype == "0")
        {
            switch (item.id)
            {
                case "1000": //골드
                    if (ismonkill)
                    {
                        PlayerData.Instance.UpGoldMon((decimal)count);
                    }
                    else
                        PlayerData.Instance.UpGold((decimal)count);
                    break;
                case "1001": //불꽃
                    PlayerData.Instance.UpCash((decimal)count);
                    break;
                case "1002": //경험치
                    if (ismonkill)
                    {
                        PlayerData.Instance.EarnExp((decimal)count, false);
                    }
                    else
                    {
                        PlayerData.Instance.EarnExp((decimal)count);
                    }
                    break;
                case "1011": //경험치
                        PlayerData.Instance.EarnExpNoPre((decimal)count);
                    break;
                case "999": //업적경험치
                    PlayerData.Instance.EarnAchExp((decimal)count);
                    break;
                case "998": //시즌패스경험치
                    if (PlayerBackendData.Instance.SeasonPassExp >= 100000)
                    {
                        PlayerBackendData.Instance.SeasonPassExp = 100000;
                        return;
                    }
                    PlayerData.Instance.EarnSeaSonPassExp(count);
                    if (SeasonPass.Instance.Panel.IsActive())
                    {
                        SeasonPass.Instance.RefreshSeasonItem();
                        SeasonPass.Instance.Refresh();
                    }
                    break;
            }
        }
        else if (item.itemsubtype == "403")
        {
            int n = int.Parse(AvartaDB.Instance.Find_id(ItemdatabasecsvDB.Instance.Find_id(id).A).num);

            avatamanager.Instance.EarnShowAvata(ItemdatabasecsvDB.Instance.Find_id(id).A);
            PlayerBackendData.Instance.playeravata[n] = true;
            //저장
            PlayerData.Instance.RefreshPlayerstat();
            Savemanager.Instance.SaveInventory();
            Savemanager.Instance.SaveAvataData();
            Savemanager.Instance.Save();
        }
        else
        {
            PlayerBackendData.Instance.Additem(id, count);
//            Debug.Log("아이템얻음" + id);
            if (InventoryPanel.isActiveAndEnabled)
            {
                RefreshInventory();
            }
        }

        if (issave)
            Savemanager.Instance.SaveInventory_SaveOn();
        // RefreshInventory();
    }

    public iteminventoryinfoslot iteminfopanel;
    private string nowselectid;
    public void ShowInventoryItem(string id)
    {
        iteminfopanel.transform.SetAsLastSibling();
        nowselectid = id;
        iteminfopanel.Refresh(id);
    }

    public void ShowInventoryItem_NoMine(string id)
    {
        nowselectid = id;
        iteminfopanel.transform.SetAsLastSibling();
        iteminfopanel.Refresh(id,false);
    }


    public void Bt_InventortGetCraftItem()
    {
        CraftManager.Instance.SearchCraftByResource(iteminfopanel.itemid);
    }

    private int arraynum = 0;
    public void Bt_UseItem()
    {
      
        switch (ItemdatabasecsvDB.Instance.Find_id(nowselectid).itemsubtype)
        {
            case "405":
                Levelshop.Instance.GiveTime2(int.Parse(ItemdatabasecsvDB.Instance.Find_id(nowselectid).A), 3);
                PlayerBackendData.Instance.RemoveItem(nowselectid, 1);
                Savemanager.Instance.SaveInventory();
                RefreshInventory();
                break;
               case "403":
                int n = int.Parse(AvartaDB.Instance.Find_id(ItemdatabasecsvDB.Instance.Find_id(nowselectid).A).num);

                avatamanager.Instance.EarnShowAvata(ItemdatabasecsvDB.Instance.Find_id(nowselectid).A);
                PlayerBackendData.Instance.RemoveItem(nowselectid, 1);
                PlayerBackendData.Instance.playeravata[n] = true;
                //저장
                PlayerData.Instance.RefreshPlayerstat();
                Savemanager.Instance.SaveInventory();
                Savemanager.Instance.SaveAvataData();
                RefreshInventory();
                Savemanager.Instance.Save();
                break;
            //스킬북
            case "999":
                if (!PlayerBackendData.Instance.Skills.Contains(ItemdatabasecsvDB.Instance.Find_id(nowselectid).A))
                {
                    PlayerBackendData.Instance.AddSkill(ItemdatabasecsvDB.Instance.Find_id(nowselectid).A);
                    PlayerBackendData.Instance.RemoveItem(nowselectid, 1);
                    Savemanager.Instance.SaveSkillData();
                    Savemanager.Instance.SaveInventory();
                    Savemanager.Instance.Save();
                    RefreshInventory();
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI4/스킬배우기성공"), alertmanager.alertenum.일반);

                }
                else
                {
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI4/이미있는스킬"), alertmanager.alertenum.일반);
                }
                break;
            
            
            case "402":
                //레벨업
                PlayerData.Instance.EarnExp(decimal.Parse(ItemdatabasecsvDB.Instance.Find_id(nowselectid).A));
                PlayerData.Instance.RefreshExp();
                Savemanager.Instance.SaveExpData();
                PlayerBackendData.Instance.RemoveItem(nowselectid, 1);
                Savemanager.Instance.SaveInventory();
                RefreshInventory();

                if (nowselectid == "200007" || nowselectid == "200002")
                 {
                    TutorialTotalManager.Instance.CheckGuideQuest("300scroll");
                }
                
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/경험치증가"), alertmanager.alertenum.일반);
                PlayerData.Instance.RefreshPlayerstat();
                break;
            
            case "300":
                if (!Settingmanager.Instance.CheckServerOn())
                {
                    return;
                }
                
                //엘리의축복
                AddTime(PlayerBackendData.timesenum.ellibless,
                    int.Parse(ItemdatabasecsvDB.Instance.Find_id(nowselectid).A));
                PlayerBackendData.Instance.RemoveItem(nowselectid, 1);
                Savemanager.Instance.SaveInventory();
                RefreshInventory();
                RefreshPremium();
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/엘리의축복사용됨"), alertmanager.alertenum.일반);
                break;
            case "401":
                if (!Settingmanager.Instance.CheckServerOn())
                {
                    return;
                }
                
                //충전권 A는 타임테이블 ENUM이름 B는 횟수  C는 일일 주간 daily weekly
                arraynum = ItemdatabasecsvDB.Instance.Find_id(nowselectid).C switch
                {
                    "daily" => (int)Enum.Parse(typeof(Timemanager.ContentEnumDaily),
                        ItemdatabasecsvDB.Instance.Find_id(nowselectid).A),
                    "weekly" => (int)Enum.Parse(typeof(Timemanager.ContentEnumWeekly),
                        ItemdatabasecsvDB.Instance.Find_id(nowselectid).A),
                    _ => arraynum
                };

                int chargecount = int.Parse(
                    ItemdatabasecsvDB.Instance.Find_id(nowselectid).B);
                bool ischarged = false;
                switch (ItemdatabasecsvDB.Instance.Find_id(nowselectid).C)
                {
                    case "daily":
                        if (Timemanager.Instance.DailyContentCount[arraynum] + chargecount >
                            Timemanager.Instance.DailyContentCount_Standard[arraynum])
                        {
                            
                            //충전 차서 불가능
                            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/충전꽉참"), alertmanager.alertenum.일반);
                            return;
                        }
                        
                        
                        if (Timemanager.Instance.AddDailyCount(arraynum, chargecount))
                        {
                            //충전이 된다면 
                            ischarged = true;
                        }
                        else
                        {
                            //충전 실패 
                        }
                        break;
                    case "weekly":
                        if (Timemanager.Instance.WeeklyContentCount[arraynum] + chargecount >
                            Timemanager.Instance.WeeklyContentCount_Standard[arraynum])
                        {
                            
                            //충전 차서 불가능
                            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/충전꽉참"), alertmanager.alertenum.일반);
                            return;
                        }
                        
                        
                        if (Timemanager.Instance.AddWeeklyCount(arraynum, chargecount))
                        {
                            //충전이 된다면 
                            ischarged = true;
                        }
                        else
                        {
                            //충전 실패 
                        }
                        break;
                }

                if (ischarged)
                {
                    PlayerBackendData.Instance.RemoveItem(nowselectid, 1);
                    Savemanager.Instance.SaveInventory_SaveOn();
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/충전권사용"), alertmanager.alertenum.일반);
                    RefreshInventory();
                }
                else
                {
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/충전실패"), alertmanager.alertenum.일반);

                }
            
                break;
        }
        Savemanager.Instance.Save();
    }

    public void Refreshadsfree()
    {
        if (PlayerBackendData.Instance.isadsfree)
        {
            PlayerData.Instance.adfreeobj.SetActive(true);
        }
        else
        {
            PlayerData.Instance.adfreeobj.SetActive(false);
        }
    }

    void RefreshPremium()
    {
        //엘리 
        PlayerBackendData.Instance.ispremium = Timemanager.Instance.NowTime  < PlayerBackendData.Instance.PlayerTimes[(int)PlayerBackendData.timesenum.ellibless];
        PlayerData.Instance.Elliobj.SetActive(PlayerBackendData.Instance.ispremium);
        PlayerData.Instance.ElliNoobj.SetActive(!PlayerBackendData.Instance.ispremium);

        if (PlayerBackendData.Instance.ispremium)
        {
            PlayerData.Instance.ElliFinishText.text =
                string.Format(GetTranslate("UI2/남은시간"),
                    PlayerBackendData.Instance.PlayerTimes[(int)PlayerBackendData.timesenum.ellibless].ToString( "yyyy-MM-dd hh:mm:ss"));
            PlayerData.Instance.ElliBuyButton.SetActive(false);
        }
        else
        {
            PlayerData.Instance.ElliFinishText.text = GetTranslate("UI2/엘리사용중아님");
            PlayerData.Instance.ElliBuyButton.SetActive(true);
        }
        PlayerData.Instance.RefreshPlayerstat();
      }
    
    public void RefreshTime()
    {
        
        SendQueue.Enqueue(Backend.Utils.GetServerTime, ( callback ) => 
        {
            if(!callback.IsSuccess())return;
            
            RefreshPremium();
            
            PlayerData.Instance.RefreshPlayerstat();
            
            
            
            
        });
    }
    
    
    
    
   public void AddTime(PlayerBackendData.timesenum enums,int day)
    {
        SendQueue.Enqueue(Backend.Utils.GetServerTime, ( callback ) => 
        {
         
            DateTime nowtime = DateTime.Parse(callback.GetReturnValuetoJSON()["utcTime"].ToString());
            //Debug.Log("간" +nowtime);
            switch (enums)
            {
                case PlayerBackendData.timesenum.ellibless:
                    //현재시간이 어 크면
                    if (PlayerBackendData.Instance.PlayerTimes[0] < nowtime)
                    {
                        var dateTime = nowtime.AddDays(day);
                        PlayerBackendData.Instance.PlayerTimes[0] = dateTime;
                    }
                    else
                    {
                        //여기시간이 더 크면
                        PlayerBackendData.Instance.PlayerTimes[0] =
                            PlayerBackendData.Instance.PlayerTimes[0].AddDays(day);
                    }

                    RefreshPremium();
                    break;
                case PlayerBackendData.timesenum.Length:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(enums), enums, null);
            }

            Settingmanager.Instance.OnlyInvenSave();
        });
        
        
        
    }

   public void AddTime_min(PlayerBackendData.timesenum enums,int min)
    {
        SendQueue.Enqueue(Backend.Utils.GetServerTime, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Timemanager.Instance.AddDailyCount(Timemanager.ContentEnumDaily.광고_엘리축복, 1);
                return;
            }
            DateTime nowtime = DateTime.Parse(callback.GetReturnValuetoJSON()["utcTime"].ToString());
            //Debug.Log("간" +nowtime);
            switch (enums)
            {
                case PlayerBackendData.timesenum.ellibless:
                    if (PlayerBackendData.Instance.PlayerTimes[0] < nowtime)
                    {
                        var dateTime = nowtime.AddMinutes(min);
                        PlayerBackendData.Instance.PlayerTimes[0] = dateTime;
                    }
                    else
                    {
                        PlayerBackendData.Instance.PlayerTimes[0] =
                            PlayerBackendData.Instance.PlayerTimes[0].AddMinutes(min);
                    }

                    RefreshPremium();
                    break;
                case PlayerBackendData.timesenum.Length:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(enums), enums, null);
            }
            Settingmanager.Instance.OnlyInvenSave();
        });
        
        
      
    }
    [Button(Name = "1000")]
    public void Test1000()
    {
        AddItem("3000", 5);
        AddItem("3001", 10);
        AddItem("501", 1);
        AddItem("502", 1);
        AddItem("503", 5);
        AddItem("4004", 5);
        AddItem("4003", 5);
        AddItem("4002", 5);
        AddItem("4001", 5);
        AddItem("504", 1);
        AddItem("504", 1);
        AddItem("504", 1);
    }
    [Button(Name = "1001")]
    public void Test1001()
    {
        AddItem("1001", 2);
    }
    [Button(Name = "1002")]
    public void Test1002()
    {
        AddItem("1002", 2);
    }
    [Button (Name = "1003")]
    public void Test1003()
    {
        AddItem("1003", 2);
    }
    [Button(Name = "1500")]
    public void Test1500()
    {
        AddItem("1500", 2);
    }
    [Button(Name = "1510")]
    public void Test1510()
    {
        AddItem("1510",3);
    }


    public UIView Equipskillpanel;
    public Text Equipskillnametext;
    public Text Equipskillinfotext;

    public void ShowEquipskill(string id)
    {
        Debug.Log(id);
        Equipskillpanel.Show(false);
        Equipskillnametext.text =
            $"[{Inventory.GetTranslate(EquipSkillDB.Instance.Find_id(id).name)} Lv.{EquipSkillDB.Instance.Find_id(id).lv}]";
        Inventory.Instance.ChangeItemRareColor(Equipskillnametext, EquipSkillDB.Instance.Find_id(id).rare);
        Equipskillinfotext.text = GetTranslate(EquipSkillDB.Instance.Find_id(id).info);
    }

    public UIView SellPanel;
    public Itemsell sellitemobj;
    public void Bt_SellItem()
    {
        SellPanel.Show(false);
        sellitemobj.SetSell(nowselectid);
    }
    
    //교환
    public UIView TradePanel;
    public tradebuy Tradeobj;

    public void TradeItem(string tradeid, tradeslot slot)
    {
        TradePanel.Show(false);
        Tradeobj.SetTradeItem(tradeid,slot);
    }

        public UIView BoxPanel;
        public GameObject BoxButtons;
    public itemboxslot[] boxslots;
    public Image BoxImage;
    public Text BoxNameText;
    public Text BoxChoiceText;
    public UIButton BoxGetButton;
    private int box_curselect;
    private int box_maxselect;
    private bool isbox;
    private ItemdatabasecsvDB.Row boxdata;

    public Animator earnani;
    public itemiconslot[] earnitems;
    private static readonly int Show = Animator.StringToHash("show");
    public RectTransform EarnItemRect;


    public GameObject BoxManyobj;
    public InputField BoxManyInput;
    public int curBoxcount = 1;
    public int maxBoxcount = 100;
    
    public void ShowEarnItem(string[] id, string[] count,bool eq)
    {
        StartCoroutine(showearn(id, count, eq));
    }
    public void ShowEarnItem2(string[] id, string[] count,bool eq)
    {
        StartCoroutine(showearn2(id, count, eq));
    }
    public void ShowEarnItem3(string[] id, int[] count,bool eq)
    {
        StartCoroutine(showearn3(id, count, eq));
    }
    public void ShowEarnItem4(string[] id, decimal[] count,bool eq)
    {
        StartCoroutine(showearn4(id, count, eq));
    }
    #region 다중열기버튼들

    public void Bt_Plus()
    {
        if (curBoxcount.Equals(maxBoxcount))
        {
            //최대
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/최대"), alertmanager.alertenum.일반);
            return;
        }

        curBoxcount++;
        BoxManyInput.text = curBoxcount.ToString();
    }
    public void Bt_MaxPlus()
    {
        curBoxcount = maxBoxcount;
        BoxManyInput.text = curBoxcount.ToString();
    }
    public void CheckCount(string count)
    {
        if (count.Equals("0") || count.Equals(""))
        {
            curBoxcount = 1;
            BoxManyInput.text = "1";
            return;
        }

        if (int.Parse(count) > maxBoxcount)
        {
            curBoxcount = maxBoxcount;
            BoxManyInput.text = curBoxcount.ToString();
            return;
        }

        curBoxcount = int.Parse(count);
        BoxManyInput.text = count;
    }

    public void Bt_Minus()
    {
        if (curBoxcount == 1)
        {
            //최소
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/최소"), alertmanager.alertenum.일반);
            return;
        }

        curBoxcount--;
        BoxManyInput.text = curBoxcount.ToString();

    }
    public void Bt_MinMinus()
    {
        curBoxcount=1;
        BoxManyInput.text = curBoxcount.ToString();

    }

        #endregion

        public Text showearnExp;
        public Text showearnGold;
        public GameObject shwoearnExpobj;
        public GameObject shwoearnGoldobj;
        
    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator showearn(IReadOnlyList<string> id, IReadOnlyList<string> count,bool isequip)
    {
    
        earnani.SetTrigger(Show);
        foreach (var VARIABLE in earnitems)
        {
            VARIABLE.gameObject.SetActive(false);
        }
        shwoearnExpobj.SetActive(false);
        shwoearnGoldobj.SetActive(false);
        yield return new WaitForSeconds(0.4f);
     
        decimal gold = 0;
        decimal exp = 0; 
        for (int i = 0; i < id.Count; i++)
        {
            if(i >= earnitems.Length)
                break;
            yield return SpriteManager.Instance.GetWaitforSecond(0.05f);
            switch (id[i])
            {
                case "1000":
                    gold += decimal.Parse(count[i]);
                    showearnGold.text = gold.ToString("N0");
                    shwoearnGoldobj.SetActive(true);
                    break;
                case "1002":
                    exp += decimal.Parse(count[i]);
                    showearnExp.text = exp.ToString("N0");
                    shwoearnExpobj.SetActive(true);
                    break;
                default:
                    earnitems[i].gameObject.SetActive(true);
                    earnitems[i].Refresh(id[i], decimal.Parse(count[i]), false, true, isequip);
                    break;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(EarnItemRect);
        }
        
    }

    IEnumerator showearn2(IReadOnlyList<string> id, IReadOnlyList<string> count, bool isequip)
    {

        earnani.SetTrigger(Show);
        foreach (var VARIABLE in earnitems)
        {
            VARIABLE.gameObject.SetActive(false);
        }

        shwoearnExpobj.SetActive(false);
        shwoearnGoldobj.SetActive(false);
        yield return new WaitForSeconds(0.4f);

        decimal gold = 0;
        decimal exp = 0;
        for (int i = 0; i < id.Count; i++)
        {
            yield return SpriteManager.Instance.GetWaitforSecond(0.05f);
            switch (id[i])
            {
                case "1000":
                    gold += decimal.Parse(count[i]);
                    showearnGold.text = gold.ToString("N0");
                    shwoearnGoldobj.SetActive(true);
                    break;
                case "1002":
                    exp += decimal.Parse(count[i]);
                    showearnExp.text = exp.ToString("N0");
                    shwoearnExpobj.SetActive(true);
                    break;
                default:
                    earnitems[i].gameObject.SetActive(true);
                    earnitems[i].Refresh(id[i], decimal.Parse(count[i]), false, true, isequip);
                    break;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(EarnItemRect);
        }
    }

    IEnumerator showearn3(IReadOnlyList<string> id, IReadOnlyList<int> count, bool isequip)
        {

            earnani.SetTrigger(Show);
            foreach (var VARIABLE in earnitems)
            {
                VARIABLE.gameObject.SetActive(false);
            }
            shwoearnExpobj.SetActive(false);
            shwoearnGoldobj.SetActive(false);
            yield return new WaitForSeconds(0.4f);

            decimal gold = 0;
            decimal exp = 0;
            for (int i = 0; i < id.Count; i++)
            {
                if (i == earnitems.Length)
                {
                    break;
                }
                yield return SpriteManager.Instance.GetWaitforSecond(0.05f);
                switch (id[i])
                {
                    case "1000":
                        gold += (decimal)count[i];
                        showearnGold.text = gold.ToString("N0");
                        shwoearnGoldobj.SetActive(true);
                        break;
                    case "1002":
                    case "1011":
                        exp += (decimal)count[i];
                        showearnExp.text = exp.ToString("N0");
                        shwoearnExpobj.SetActive(true);
                        break;
                    default:
                        earnitems[i].gameObject.SetActive(true);
                        earnitems[i].Refresh(id[i], (decimal)count[i], false, true, isequip);
                        break;
                }
                LayoutRebuilder.ForceRebuildLayoutImmediate(EarnItemRect);
            }
        }

    IEnumerator showearn4(IReadOnlyList<string> id, IReadOnlyList<decimal> count, bool isequip)
    {

        earnani.SetTrigger(Show);
        foreach (var VARIABLE in earnitems)
        {
            VARIABLE.gameObject.SetActive(false);
        }

        shwoearnExpobj.SetActive(false);
        shwoearnGoldobj.SetActive(false);
        yield return new WaitForSeconds(0.4f);

        decimal gold = 0;
        decimal exp = 0;
        for (int i = 0; i < id.Count; i++)
        {
            yield return SpriteManager.Instance.GetWaitforSecond(0.05f);
            switch (id[i])
            {
                case "1000":
                    gold += (decimal)count[i];
                    showearnGold.text = gold.ToString("N0");
                    shwoearnGoldobj.SetActive(true);
                    break;
                case "1002":
                case "1011":
                    exp += (decimal)count[i];
                    showearnExp.text = exp.ToString("N0");
                    shwoearnExpobj.SetActive(true);
                    break;
                default:
                    earnitems[i].gameObject.SetActive(true);
                    earnitems[i].Refresh(id[i], (decimal)count[i], false, true, isequip);
                    break;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(EarnItemRect);
        }
    }

    void CheckBox()
     {
         if (!BoxPanel.gameObject.activeSelf) return;
         isbox = true;
         BoxPanel.Hide(false);
         InventoryPanel.Hide(false);
        // iteminfopanel.gameObject.SetActive(false);
     }

    public void Bt_LeavePanel()
    {
        if (!isbox) return;
        BoxPanel.Show(true);
        InventoryPanel.Show(true);
    }
    
    public void ShowBoxMine()
    {
        ShowBox(true,nowselectid);
    }

    public void Bt_ShowBoxInfo()
    {
        ShowBox(false,nowselectid);
    }
     void ShowBox(bool ismine,string itemid)
     {
//         Debug.Log(itemid);
  //       Debug.Log(ismine);
         //초이스 박스라면 초이스 개수를 0으로만들어야함.
        BoxGetButton.Interactable = true;
        BoxPanel.transform.SetAsLastSibling();
        iteminfopanel.gameObject.SetActive(false);
        BoxPanel.Show(false);
        //박스 유무
        BoxGetButton.Interactable = ismine;
        BoxGetButton.gameObject.SetActive(ismine);
        //초이스
        BoxChoiceText.text = "";
        BoxChoiceText.gameObject.SetActive(false);
        ItemdatabasecsvDB.Row data = ItemdatabasecsvDB.Instance.Find_id(itemid);
        boxdata = data;
        if (ismine)
        {
            Debug.Log("여기다");
            maxBoxcount = PlayerBackendData.Instance.CheckItemCount(itemid);
            if (maxBoxcount > 100)
                maxBoxcount = 100;

            curBoxcount = 1;
            BoxButtons.SetActive(true);
    
            if (data.C.Equals("Choice"))
            {
                BoxChoiceText.gameObject.SetActive(true);
                BoxGetButton.Interactable = false;
                box_curselect = 0;
                box_maxselect = int.Parse(data.D);
                BoxChoiceText.text = $"0/{box_maxselect.ToString()}";
            }
        }
        else
        {
            BoxButtons.SetActive(false);
        }
        BoxManyobj.SetActive(false);

        foreach (var VARIABLE in boxslots)
        {
            VARIABLE.gameObject.SetActive(false);
        }

       
      
        
        //박스이름
        BoxNameText.text = GetTranslate(data.name);
        ChangeItemRareColor(BoxNameText,data.rare);
        //박스 이미
        BoxImage.sprite = SpriteManager.Instance.GetSprite(data.sprite);
        
        
        string[] id= data.A.Split(';');
        string[] howmany = data.B.Split(';');
        string type = data.C;
        if (data.IsEquipBox == "TRUE")
        {
         //장비상자라면.   
         for (int i = 0; i < id.Length; i++)
         {
             boxslots[i].gameObject.SetActive(true);
             boxslots[i].SetData(id[i], 1, true,type,ismine);
         }
        }
        else //아이템 전체 상자라면
        {
            for (int i = 0; i < id.Length; i++)
            {
                boxslots[i].gameObject.SetActive(true);
                boxslots[i].SetData(id[i], int.Parse (howmany[i]), false, type,ismine);
            }

            if (ismine && maxBoxcount != 1)
            {
                BoxManyobj.SetActive(true);
                BoxManyInput.text = curBoxcount.ToString();
            }
        }
        
    

       
     }

     public void Box_SelectItem()
     {
         if(boxdata.C != "Choice") return;
         Debug.Log(":ddwadwa:");
         box_curselect++;
         BoxChoiceText.text = $"{box_curselect.ToString()}/{box_maxselect.ToString()}";
         
         Debug.Log(box_curselect);
         Debug.Log(box_maxselect);

         
         if (box_curselect != box_maxselect) return;
         
         BoxGetButton.Interactable = true;
         
         foreach (var VARIABLE in boxslots)
         {
             VARIABLE.settoggleoff();                 
         }
     }

     public void Box_UnSelectItem()
     {
         if(boxdata == null) return;
         if(boxdata.C != "Choice") return;
         
         box_curselect--;
         if (box_curselect < 0)
             box_curselect = 0;
         BoxGetButton.Interactable = false;
         BoxChoiceText.text = $"{box_curselect.ToString()}/{box_maxselect.ToString()}";
         foreach (var VARIABLE in boxslots)
         {
             VARIABLE.settoggleon();                 
         }
     }

     public void Bt_GetBoxItem()
     {
         BoxPanel.Hide(true);
         PlayerBackendData.Instance.RemoveItem(boxdata.id,curBoxcount);
         string[] id = boxdata.A.Split(';');
         string[] howmany = boxdata.B.Split(';');
         isbox = false;

         List<string> getid = new List<string>();
         List<string> gethowmany = new List<string>();

         
         switch (boxdata.C)
         {
             case "All":
                 //전체받기
                 if (boxdata.IsEquipBox == "TRUE")
                 {
                     //장비지급
                     foreach (var t in id)
                     {
                         PlayerBackendData.Instance.MakeEquipment(t);
                         getid.Add(t);
                         gethowmany.Add("1");
                     }

                     if (nowselectid.Equals("527"))
                     {
                         TutorialTotalManager.Instance.CheckGuideQuest("openbox");
                     }
                     
                     Savemanager.Instance.SaveEquip();
                 }
                 else
                 {
                     for (int i = 0; i < id.Length; i++)
                     {
                         AddItem(id[i], int.Parse(howmany[i]) * curBoxcount, false);
                         getid.Add(id[i]);
                         gethowmany.Add((int.Parse(howmany[i]) * curBoxcount).ToString());
                     }
                     Savemanager.Instance.SaveInventory();
                 }

                 break;
             case "Choice":
                 if (boxdata.IsEquipBox == "TRUE")
                 {
                     //장비지급
                     foreach (var t in boxslots)
                     {
                         if (t.ChoiceToggle.IsOn && t.gameObject.activeSelf)
                         {
                             PlayerBackendData.Instance.MakeEquipment(t.id);
                             getid.Add(t.id);
                             gethowmany.Add("1");
                         }

                         if (!t.gameObject.activeSelf)
                             break;
                     }

                     Savemanager.Instance.SaveEquip();
                 }
                 else
                 {
                     foreach (var t in boxslots)
                     {
                         if (t.ChoiceToggle.IsOn && t.gameObject.activeSelf)
                         {
                             AddItem(t.id, t.itemcount* curBoxcount, false);
                             getid.Add(t.id);
                             gethowmany.Add((t.itemcount * curBoxcount).ToString());
                         }

                         if (!t.gameObject.activeSelf)
                             break;
                     }

                     Savemanager.Instance.SaveInventory();
                 }

                 break;
             case "Random":
                 if (boxdata.IsEquipBox == "TRUE")
                 {
                    Random.InitState((int)Time.deltaTime + PlayerBackendData.Instance.GetRandomSeed());
                     int ranindex = Random.Range(0, id.Length);
                     //장비지급
                     //Debug.Log("인덱스는" + ranindex);
                     PlayerBackendData.Instance.MakeEquipment(id[ranindex]);
                     getid.Add(id[ranindex]);
                     gethowmany.Add("1");
                     Savemanager.Instance.SaveEquip();
                 }
                 else
                 {
                     for (int i = 0; i < curBoxcount; i++)
                     {
                         Random.InitState((int)Time.deltaTime + PlayerBackendData.Instance.GetRandomSeed());
                         int ranindex = Random.Range(0, id.Length);
                         //Debug.Log("인덱스는" + ranindex);
                         AddItem(id[ranindex], int.Parse(howmany[ranindex]), false);
                         getid.Add(id[ranindex]);
                         gethowmany.Add(howmany[ranindex]);
                     }
                     Savemanager.Instance.SaveInventory();
                 }
                 break;
         }
         
         LogManager.BoxOpen(GetTranslate(boxdata.name),curBoxcount);
         RefreshInventory();
         Savemanager.Instance.Save();
         ShowEarnItem(getid.ToArray(), gethowmany.ToArray(), bool.Parse(boxdata.IsEquipBox));
     }


     public UIView EskillShowpanel;
     public eskillslot[] esslots;


     public void Bt_ShowNowEquipES()
     {
         ShowEskillSkills(EquipItemDB.Instance.Find_id(data.Itemid).SpeMehod);
     }

     private void ShowEskillSkills(string eskillid)
     {
        EskillShowpanel.Show(false);
         EquipSkillRandomGiveDB.Row speds = EquipSkillRandomGiveDB.Instance.Find_id(eskillid);
         List<string> skillcoreid = new List<string>();

         skillcoreid = speds.equipskills.Split(';').ToList();

         foreach (var t in esslots)
             t.gameObject.SetActive(false);

         for (int i = 0; i < skillcoreid.Count; i++)
         {
             List<string> skillall = new List<string>();
             //스킬레벨 설정
             bool isstart = false;
             for (int j = 0; j < EquipSkillDB.Instance.NumRows(); j++)
             {
                 if (skillcoreid[i] == EquipSkillDB.Instance.GetAt(j).coreid)
                 {
                     isstart = true;
                     skillall.Add(EquipSkillDB.Instance.GetAt(j).id);
                 }
                
                 //중간에 끊음
                 if (isstart && skillcoreid[i] != EquipSkillDB.Instance.GetAt(j).coreid)
                 {
                     break;
                 }
             }
             esslots[i].init(skillall.ToArray());
             esslots[i].gameObject.SetActive(true);
         }
     }

     
     //가장 높은걸로 장착
     public void bt_autoequip()
     {
         for (int i = 0; i < PlayerBackendData.Instance.EquipEquiptment0.Length; i++)
         {
             if (PlayerBackendData.Instance.GetTypeEquipment(i.ToString()).Count > 0)
             {
                 List<EquipDatabase> list = PlayerBackendData.Instance.GetTypeEquipment(i.ToString()).Values.ToList();
                 int bp = list[0].GetBattlePoint();
                 EquipDatabase keyid = list[0];
                 for (int j = 0; j < PlayerBackendData.Instance.GetTypeEquipment(i.ToString()).Count; j++)
                 {
                     if (list[j].GetBattlePoint() > bp)
                     {
                         bp = list[j].GetBattlePoint();
                         keyid = list[j];
                     }
                 }
                 //장착
                 Inventory.Instance.nowsettype = i;
                 Inventory.Instance.nowslots = i;
                 Inventory.Instance.data = keyid;
                 try
                 {
                     PlayerData.Instance.RefreshPlayerstat();
                     Inventory.Instance.EquipItem_NoSave();
                 }
                 catch (Exception e)
                 {
                     continue;
                 }
             }
         }
         Savemanager.Instance.SaveEquip();
         Savemanager.Instance.Save();
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/자동장착완료"),alertmanager.alertenum.일반);
     }



}

public class ItemInven
{
    string id;
    int howmany;
    string expiredate;

    public ItemInven()
    {
    }

    public ItemInven(string id, int howmany ,string expiredate)
    {
        this.Id = id;
        this.Howmany = howmany;
        this.Expiredate = expiredate;
    }

    public string Id { get => id; set => id = value; }
    public int Howmany { get => howmany; set => howmany = value; }
    public string Expiredate { get => expiredate; set => expiredate = value; }
}
