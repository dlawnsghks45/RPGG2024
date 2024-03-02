using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shoptimelobbyslot : MonoBehaviour
{
    public GameObject Button_shop;
    public Image ButtonImage;
    public Text ButtonTime;
    private int slotnum;

    public void SetRefresh(int num,Sprite sprite)
    {
        slotnum = num;
        Button_shop.SetActive(true);
        ButtonImage.sprite = sprite;
        RefreshTime();
    }

    public void OffRefresh()
    {
        gameObject.SetActive(false);
    }

    public void RefreshTime()
    {
        TimeSpan dateDiff = PlayerBackendData.Instance.PlayerShopTimes[slotnum] - Timemanager.Instance.NowTime;
        nowsecond = dateDiff.TotalSeconds;
        dt = new DateTime(dateDiff.Ticks);
        dt.AddSeconds(nowsecond);
        isfinish = false;
        StartCoroutine(TimeStart());
        //남은 시간 보이기
        Refresh();
    }

    private void OnDisable()
    {
        isfinish = true;
        gameObject.SetActive(false);
    }

    public double nowsecond;
    DateTime dt;
    private bool isfinish;
    WaitForSeconds wait = new WaitForSeconds(1f);
    //시간제용
    IEnumerator TimeStart()
    {
        while (!isfinish)
        {
            yield return wait;
            nowsecond--;
            if (nowsecond <= 0)
            {
                isfinish = true;
                Refresh();
            }
            else
            {
                dt = dt.AddSeconds(-1);
                Refresh();
            }
        }
    }
    public void Refresh()
    {
//        Debug.Log("버튼콜");
        ButtonTime.text = dt.ToString("dd:HH:mm:ss");
    }
}
