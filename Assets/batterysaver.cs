using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using Doozy.Engine.Utils.ColorModels;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class batterysaver : MonoBehaviour
{
    //�̱��游���.
    private static batterysaver _instance = null;
    public static batterysaver Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(batterysaver)) as batterysaver;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }
    public List<string> itemid= new List<string>();
    public List<int> itemhowmany= new List<int>();

    public itemiconslot[] items;

    bool ison = false;
    private int itemcount = 0;

    public Text StageText;
    public Text TimeText;
    public Text ExpText;
    public Text GoldText;

    private int nowsecend = 0;

    private decimal exp;
    private decimal gold;
    public Canvas[] FalseCanvas;
    public GameObject[] FalseCanvas2;
    
    
    
    
    public UIView savepanel;
    public void Bt_BatterySaver()
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/����͸�����"), alertmanager.alertenum.����);
            return;
        }
        
        itemid.Clear();
        itemhowmany.Clear();

        gold = 0;
        exp = 0;
        GoldText.text = "0";
        ExpText.text = "0";
        
        foreach (var VARIABLE in items)
        {
            VARIABLE.gameObject.SetActive(false);
        }

        StageText.text = mapmanager.Instance.MapText.text;

        for (int i = 0; i < FalseCanvas.Length; i++)
        {
            FalseCanvas[i].enabled = false;
        }
        for (int i = 0; i < FalseCanvas2.Length; i++)
        {
            FalseCanvas2[i].SetActive(false);
        }
        nowsecend = 0;
        ison = true;
        savepanel.Show(false);
        QualitySettings.vSyncCount = 0;
        OnDemandRendering.renderFrameInterval = 3;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        StartCoroutine(timer());
    }
    

    IEnumerator timer()
    {
        TimeText.text = "00:00:00";
        while (savepanel.gameObject.activeSelf)
        {
        yield return SpriteManager.Instance.GetWaitforSecond(1f);
        nowsecend++;
        
        int hours, minute, second;

        //�ð�����

        hours = nowsecend / 3600;//�� ����

        minute = nowsecend % 3600 / 60;//���� ���ϱ����ؼ� �Էµǰ� ���������� �� 60�� ������.

        second = nowsecend % 3600 % 60;//������ ���� �ð����� ���� �� ������ �ð��� �ʷ� �����

        TimeText.text = $"{hours:D2}:{minute:D2}:{second:D2}";
        }
    }

    public void AdditemGoldExp(string id,decimal count)
    {
        if(!ison) return;

        switch (id)
        {
            case "1000": //���
                gold += count;
                GoldText.text = gold.ToString("N0");
                break;
            case "1002": //����ġ
                exp += count;
                ExpText.text = exp.ToString("N0");
                break;
        }
    }
    public void AddItem(string id,int count)
    {
        if(!ison) return;

        switch (id)
        {
            case "1000": //���
            case "1002": //����ġ
                break;
            default:
                if (itemid.Contains(id))
                {
                    int index = itemid.IndexOf(id);
                    itemhowmany[index] += count;
                    items[index].gameObject.SetActive(true);
                    items[index].Refresh(itemid[index],itemhowmany[index],true,true);
                }
                else
                {
                    itemid.Add(id);
                    itemhowmany.Add(count);
                    items[itemcount].gameObject.SetActive(true);
                    items[itemcount].Refresh(itemid[itemcount],itemhowmany[itemcount],true,true);
                    itemcount++;
                }
                break;
        }        
        
      
    }


    public void Bt_ExitBatterySaver()
    {
        ison = false;
        itemcount = 0;
        OnDemandRendering.renderFrameInterval = 1;
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        for (int i = 0; i < FalseCanvas.Length; i++)
        {
            FalseCanvas[i].enabled = true;
        }
        for (int i = 0; i < FalseCanvas2.Length; i++)
        {
            FalseCanvas2[i].SetActive(true);
        }
        savepanel.Hide(false);
    }
  
    public  Slider slider;

    public void bt_upgaguge()
    {
        slider.value += 0.01f;
        if(slider.value >= 1f)
            Bt_ExitBatterySaver();
    }

    public void bt_pointup()
    {
        slider.value = 0;
    }

    private void Update()
    {
        if (Input.touchCount <= 0) return;
        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Ended) return;
        if (curcount != 0)
            curcount = 0;
    }
/*
    private void Start()
    {
        StartCoroutine(SleepMode());
    }
*/
    [SerializeField]
    private float curcount;
    private float totalcount = 1200;
    private WaitForSeconds wait = new WaitForSeconds(60);
    IEnumerator SleepMode()
    {
        while (true)
        {
            yield return wait;
            if (!batterysaver.Instance.ison)
            {
                curcount += 
                    60;

                if (curcount >= totalcount)
                {
                    Debug.Log("������� ����");
                    batterysaver.Instance.Bt_BatterySaver();
                    curcount = 0;
                }
            }
        }
    }
}
