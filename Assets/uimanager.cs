using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class uimanager : MonoBehaviour
{
    //싱글톤만들기.
    private static uimanager _instance = null;
    public static uimanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(uimanager)) as uimanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    public List<tradeslot> TradeSlots = new List<tradeslot>();
    public List<shopslot> ShopSlots = new List<shopslot>();
    public List<adsrewardslot> adsSlots = new List<adsrewardslot>();

    public void RefreshShopAndTrade()
    {
        foreach (var VARIABLE in TradeSlots)
        {
            VARIABLE.RefreshBuyCount();
        }
        foreach (var VARIABLE in ShopSlots)
        {
            VARIABLE.RefreshBuyCount();
        }
        foreach (var VARIABLE in adsSlots)
        {
            VARIABLE.RefreshBuyCount();
        }
    }

    public GameObject internetobj;
    
    public UIView ShowDeathPanel;
    
    public Animator AdventureSuccAni;
    public Text AdventureText;
    private static readonly int Show = Animator.StringToHash("show");

    public List<string> dropsid = new List<string>();
    public List<int> dropshowmany = new List<int>();
    public itemiconslot[] ItemSlots;

    public void ResetLoot()
    {
        dropsid.Clear();
        dropshowmany.Clear();
    }
    public void AddLoot(string id, int count)
    {
        if(dropsid.Contains(id))
        {
            var i = dropsid.IndexOf(id);
            dropshowmany[i] += count;
        }
        else
        {
            dropsid.Add(id);
            dropshowmany.Add(count);
        }
    }

    void FinishAltarWarLoot()
    {
        StartCoroutine(showitemicon());
    }
    WaitForSeconds waits = new WaitForSeconds(0.2f);
    WaitForSeconds waits2 = new WaitForSeconds(0.3f);
    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator showitemicon()
    {
        mondropmanager.Instance.GiveRewardExcepDropPanel();
        foreach (var t in ItemSlots)
        {
            t.gameObject.SetActive(false);
        }
        
        yield return waits2;
        for (int i = 0; i < dropsid.Count;i++)
        {
            yield return waits;
            ItemSlots[i].gameObject.SetActive(true);
          //  Debug.Log(dropshowmany[i]);
            ItemSlots[i].Refresh(dropsid[i],dropshowmany[i],false,true);
        }
    }
    
    public void ShowSuccAdventureLv()
    {
        Invoke(nameof(show), 2.4f);
    }
    void show()
    {
        AdventureText.text = PlayerData.Instance.gettierstar((PlayerBackendData.Instance.GetAdLv()).ToString());
        AdventureSuccAni.SetTrigger(Show);
        LogManager.LogAdlv(PlayerBackendData.Instance.GetAdLv());
//        Debug.Log("현재는" + PlayerBackendData.Instance.GetAdLv());
        switch (PlayerBackendData.Instance.GetAdLv())
        {
            case 2:
                //직업 해금 가능
                PlayerData.Instance.ShowNewContentReal("직업");
                break;
            case 3:
                //감연된 농장 오픈
                PlayerData.Instance.ShowNewContentReal("감염된농장");
                break;
            case 4: //늪짖대
                PlayerData.Instance.ShowNewContentReal("늪지대");
                break;
            case 5://지하감옥 
                PlayerData.Instance.ShowNewContentReal("5던전");
                break;
            case 6://ㅎ화산 
                PlayerData.Instance.ShowNewContentReal("6던전");
                break;
            case 7://발록 //성물 전쟁
                PlayerData.Instance.ShowNewContentReal("7던전");
                break;
            case 10://발록 //성물 전쟁
                PlayerData.Instance.ShowNewContentReal("허수아비");
                break;
            case 11://죽은용의 동굴
                PlayerData.Instance.ShowNewContentReal("11던전");
                break;
            case 12://대제국
                PlayerData.Instance.ShowNewContentReal("12던전");
                break;
            case 13://대제국
                PlayerData.Instance.ShowNewContentReal("13던전");
                break;
            case 14://대제국
                PlayerData.Instance.ShowNewContentReal("14던전");
                break;
            case 15://대제국
                PlayerData.Instance.ShowNewContentReal("15던전");
                //펫 기능
                break;
            case 20://대제국
                PlayerData.Instance.ShowNewContentReal("20던전");
                //펫 기능
                break;
            case 21://대제국
                PlayerData.Instance.ShowNewContentReal("21던전");
                //펫 기능
                break;
            case 22://대제국
                PlayerData.Instance.ShowNewContentReal("22던전");
                //펫 기능
                break;
            case 23://대제국
                PlayerData.Instance.ShowNewContentReal("23던전");
                //펫 기능
                break;
            case 24://대제국
                PlayerData.Instance.ShowNewContentReal("24던전");
                //펫 기능
                break;
            case 25://대제국
                PlayerData.Instance.ShowNewContentReal("25던전");
                //펫 기능
                break;
            case 26://대제국
                PlayerData.Instance.ShowNewContentReal("26던전");
                //펫 기능
                break;
            case 27://대제국
                PlayerData.Instance.ShowNewContentReal("27던전");
                //펫 기능
                break;
            case 28://대제국
                PlayerData.Instance.ShowNewContentReal("28던전");
                //펫 기능
                break;
            case 29://대제국
                PlayerData.Instance.ShowNewContentReal("29던전");
                //펫 기능
                break;
            case 30://대제국
                PlayerData.Instance.ShowNewContentReal("30던전");
                //펫 기능
                break;
        }
        
        
        FinishAltarWarLoot();
    }

    public List<UIView> UIVIEWS = new List<UIView>();
    public List<GameObject> UIVIEWSGameObject = new List<GameObject>();

    public void AddUiview(UIView ui,bool isclose)
    {
        UIVIEWS.Add(ui);
        if (isclose)
        {
            ui.Hide(true);
        }
    }
    public void AddUIVIEWSGameObject(GameObject ui,bool isclose)
    {
        UIVIEWSGameObject.Add(ui);
        if (isclose)
        {
            ui.SetActive(false);
        }
    }
    public void Bt_ShowUIView()
    {
        if (UIVIEWS.Count != 0)
        {
            UIVIEWS[^1].Show(false);
            UIVIEWS.RemoveAt(UIVIEWS.Count - 1);
        }

        if (UIVIEWSGameObject.Count != 0)
        {
            UIVIEWSGameObject[^1].SetActive(true);
            UIVIEWSGameObject.RemoveAt(UIVIEWSGameObject.Count - 1);
        }
    }

    public GameObject infoobj;
    public Text infonametext;
    public Text infoinfotext;
    public void ShowInfoQuestion(string trigger)
    {
        infoobj.SetActive(true);
        infonametext.text = Inventory.GetTranslate($"ContentInfos/{trigger}");
        infoinfotext.text = Inventory.GetTranslate($"ContentInfos/{trigger}0");
    }

    public UIToggle[] ToggleOffs;
    public UIView[] TogglePanel;
    
    
    //끔
    public void OnDaily(int num)
    {
        PlayerBackendData.Instance.DailyOffBool[num] = true;
        Savemanager.Instance.SaveDailyPanel();
    }

    //킴
    public void OffDaily(int num)
    {
        PlayerBackendData.Instance.DailyOffBool[num] = false;
        Savemanager.Instance.SaveDailyPanel();
    }

    //이벤트 나가기에 넣음
    public void Bt_ShowPanelDailyReward()
    {
        if (isshow_reward)
        {
            return;
        }
        isshow_reward = true;
        if (!PlayerBackendData.Instance.DailyOffBool[1])
        {
            TogglePanel[1].Show(false);
        }
    }

    private bool isshow_event =false;
    private bool isshow_reward =false;
    //공지나가기에넣음
    public void Bt_ShowPanelEvent()
    {
        if (isshow_event)
        {
            return;
        }
        isshow_event = true;
        if (!PlayerBackendData.Instance.DailyOffBool[2])
        {
            TogglePanel[2].Show(false);
        }
    }

    private void Start()
    {
        for (int i = 0; i < PlayerBackendData.Instance.DailyOffBool.Length; i++)
        {
            ToggleOffs[i].IsOn = PlayerBackendData.Instance.DailyOffBool[i];
        }
    }
}
