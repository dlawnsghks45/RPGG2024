using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class LoginEventManager : MonoBehaviour
{
  private static LoginEventManager _instance = null;
  public static LoginEventManager Instance
  {
    get
    {
      if (_instance == null)
      {
        _instance = FindObjectOfType(typeof(LoginEventManager)) as LoginEventManager;

        if (_instance == null)
        {
          //Debug.Log("Player script Error");
        }
      }
      return _instance;
    }
  }

  
  public loginrewardslot[] slots;

  private void Start()
  {
    StartCoroutine(Starttime());
  }
  public GameObject LoginNoti;
  private WaitForSeconds wait = new WaitForSeconds(1f);
  public UIView panel;
  public Text TimeString;
  private int num = 0;

  public IEnumerator Starttime()
  {
    if (Timemanager.Instance.LoginTimeSecToday == 0)
    {
      for (int i = 0; i < slots.Length; i++)
      {
        slots[i].isfinish = false;
      }
    }
    
    for (int i = 0; i < slots.Length; i++)
    {
      slots[i].Refresh();
    }

    TimeString.text = "00:00";

    while (true)
    {
      yield return wait;
     
      Timemanager.Instance.LoginTimeSecEvery++;

      if (Timemanager.Instance.LoginTimeSecToday > 9000)
      {
        Savemanager.Instance.SaveLoginTime();
        yield return null;
      }
      else
      {
        Timemanager.Instance.LoginTimeSecToday++;
        Savemanager.Instance.SaveLoginTime();
      }
    
      
      num++;
      if (num.Equals(60))
      {
        num = 0;
        for (int i = 0; i < slots.Length; i++)
        {
          slots[i].Refresh();
        }
      }
      //창이열려있다면 확인
      if (panel.IsVisible)
      {
        TimeSpan time = TimeSpan.FromSeconds(Timemanager.Instance.LoginTimeSecToday);
        TimeString.text = time.ToString(@"hh\:mm\:ss");
        
        for (int i = 0; i < slots.Length; i++)
        {
          slots[i].isfinish = false;
          slots[i].Refresh();
        }
      }
    }
  }

  public void RefreshPanel()
  {
    for (int i = 0; i < slots.Length; i++)
    {
      slots[i].Refresh();
    }
  }
}
