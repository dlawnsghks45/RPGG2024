using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class uimanager : MonoBehaviour
{
    //�̱��游���.
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
//        Debug.Log("�����" + PlayerBackendData.Instance.GetAdLv());
        switch (PlayerBackendData.Instance.GetAdLv())
        {
            case 2:
                //���� �ر� ����
                PlayerData.Instance.ShowNewContentReal("����");
                break;
            case 3:
                //������ ���� ����
                PlayerData.Instance.ShowNewContentReal("�����ȳ���");
                break;
            case 4: //��¢��
                PlayerData.Instance.ShowNewContentReal("������");
                break;
            case 5://���ϰ��� 
                PlayerData.Instance.ShowNewContentReal("5����");
                break;
            case 6://��ȭ�� 
                PlayerData.Instance.ShowNewContentReal("6����");
                break;
            case 7://�߷� //���� ����
                PlayerData.Instance.ShowNewContentReal("7����");
                break;
            case 10://�߷� //���� ����
                PlayerData.Instance.ShowNewContentReal("����ƺ�");
                break;
            case 11://�������� ����
                PlayerData.Instance.ShowNewContentReal("11����");
                break;
            case 12://������
                PlayerData.Instance.ShowNewContentReal("12����");
                break;
            case 13://������
                PlayerData.Instance.ShowNewContentReal("13����");
                break;
            case 14://������
                PlayerData.Instance.ShowNewContentReal("14����");
                break;
            case 15://������
                PlayerData.Instance.ShowNewContentReal("15����");
                //�� ���
                break;
            case 20://������
                PlayerData.Instance.ShowNewContentReal("20����");
                //�� ���
                break;
            case 21://������
                PlayerData.Instance.ShowNewContentReal("21����");
                //�� ���
                break;
            case 22://������
                PlayerData.Instance.ShowNewContentReal("22����");
                //�� ���
                break;
            case 23://������
                PlayerData.Instance.ShowNewContentReal("23����");
                //�� ���
                break;
            case 24://������
                PlayerData.Instance.ShowNewContentReal("24����");
                //�� ���
                break;
            case 25://������
                PlayerData.Instance.ShowNewContentReal("25����");
                //�� ���
                break;
            case 26://������
                PlayerData.Instance.ShowNewContentReal("26����");
                //�� ���
                break;
            case 27://������
                PlayerData.Instance.ShowNewContentReal("27����");
                //�� ���
                break;
            case 28://������
                PlayerData.Instance.ShowNewContentReal("28����");
                //�� ���
                break;
            case 29://������
                PlayerData.Instance.ShowNewContentReal("29����");
                //�� ���
                break;
            case 30://������
                PlayerData.Instance.ShowNewContentReal("30����");
                //�� ���
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
    
    
    //��
    public void OnDaily(int num)
    {
        PlayerBackendData.Instance.DailyOffBool[num] = true;
        Savemanager.Instance.SaveDailyPanel();
    }

    //Ŵ
    public void OffDaily(int num)
    {
        PlayerBackendData.Instance.DailyOffBool[num] = false;
        Savemanager.Instance.SaveDailyPanel();
    }

    //�̺�Ʈ �����⿡ ����
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
    //���������⿡����
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
