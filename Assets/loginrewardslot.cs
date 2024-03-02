using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class loginrewardslot : MonoBehaviour
{
  [SerializeField] private string giveid;
  [SerializeField] private int givehw;
  [SerializeField] private itemiconslot itemslots;
  [SerializeField] private int timesec;
  public UIButton RewardButton;
  public GameObject FinishButton;
  public bool isfinish;
  private void Start()
  {
    itemslots.Refresh(giveid,givehw,false);
    Refresh();
  }

  public void Refresh()
  {
    if (isfinish)
    {
      RewardButton.gameObject.SetActive(false);
      FinishButton.gameObject.SetActive(true);
      return;
    }
    bool iscanearn = false;
    if (timesec <= Timemanager.Instance.LoginTimeSecToday)
    {
      switch (timesec)
      {
        case 0:
          //������ �ִٸ�
          if (Timemanager.Instance.GetNowCount_daily(Timemanager.ContentEnumDaily.���Ӻ���0��) > 0)
          {
            iscanearn = true;
            RewardButton.gameObject.SetActive(true);
            RewardButton.Interactable = true;
            FinishButton.gameObject.SetActive(false);
          }
          else
          {
            isfinish = true;
            RewardButton.gameObject.SetActive(false);
            FinishButton.gameObject.SetActive(true);
          }
          break;
        case 1800:
          //������ �ִٸ�
          if (Timemanager.Instance.GetNowCount_daily(Timemanager.ContentEnumDaily.���Ӻ���30��) > 0)
          {
            iscanearn = true;
            RewardButton.gameObject.SetActive(true);
            RewardButton.Interactable = true;
            FinishButton.gameObject.SetActive(false);
          }
          else
          {
            isfinish = true;

            RewardButton.gameObject.SetActive(false);
            FinishButton.gameObject.SetActive(true);
          }
          break;
        case 3600:
          //������ �ִٸ�
          if (Timemanager.Instance.GetNowCount_daily(Timemanager.ContentEnumDaily.���Ӻ���60��) > 0)
          {
            iscanearn = true;

            RewardButton.gameObject.SetActive(true);
            RewardButton.Interactable = true;
            FinishButton.gameObject.SetActive(false);
          }
          else
          {
            isfinish = true;

            RewardButton.gameObject.SetActive(false);
            FinishButton.gameObject.SetActive(true);
          }
          break;
        case 5400:
          //������ �ִٸ�
          if (Timemanager.Instance.GetNowCount_daily(Timemanager.ContentEnumDaily.���Ӻ���90��) > 0)
          {
            iscanearn = true;

            RewardButton.gameObject.SetActive(true);
            RewardButton.Interactable = true;
            FinishButton.gameObject.SetActive(false);
          }
          else
          {
            isfinish = true;

            RewardButton.gameObject.SetActive(false);
            FinishButton.gameObject.SetActive(true);
          }
          break;
        case 7200:
          //������ �ִٸ�
          if (Timemanager.Instance.GetNowCount_daily(Timemanager.ContentEnumDaily.���Ӻ���120��) > 0)
          {
            iscanearn = true;

            RewardButton.gameObject.SetActive(true);
            RewardButton.Interactable = true;
            FinishButton.gameObject.SetActive(false);
          }
          else
          {
            isfinish = true;

            RewardButton.gameObject.SetActive(false);
            FinishButton.gameObject.SetActive(true);
          }
          break;
        case 9000:
          //������ �ִٸ�
          if (Timemanager.Instance.GetNowCount_daily(Timemanager.ContentEnumDaily.���Ӻ���150��) > 0)
          {
            iscanearn = true;

            RewardButton.gameObject.SetActive(true);
            RewardButton.Interactable = true;
            FinishButton.gameObject.SetActive(false);
          }
          else
          {
            isfinish = true;
            RewardButton.gameObject.SetActive(false);
            FinishButton.gameObject.SetActive(true);
          }
          break;
      }


      if (iscanearn)
      {
        //��Ƽ ����
        LoginEventManager.Instance.LoginNoti.SetActive(true);
        alertmanager.Instance.Alert_SeasonPass[1].SetActive(true);

      }
      
      RewardButton.gameObject.SetActive(true);
    }
    else
    {
      RewardButton.Interactable = false;
      RewardButton.gameObject.SetActive(true);
      FinishButton.gameObject.SetActive(false);
    }
  }

  public void Bt_GetItem()
  {
    switch (timesec)
    {
      case 0:
        if (Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.���Ӻ���0��))
        {
          Inventory.Instance.AddItem(giveid, givehw);
          alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/������ɿϷ�"),alertmanager.alertenum.�Ϲ�);
          Savemanager.Instance.SaveInventory();
          Savemanager.Instance.SaveLoginTime();
          Savemanager.Instance.Save();
          RewardButton.gameObject.SetActive(false);
          FinishButton.gameObject.SetActive(true);
          alertmanager.Instance.Alert_SeasonPass[1].SetActive(false);
          LoginEventManager.Instance.LoginNoti.SetActive(false);
        }

        break;
      case 1800:
        if (Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.���Ӻ���30��))
        {
          Inventory.Instance.AddItem(giveid, givehw);
          alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/������ɿϷ�"),alertmanager.alertenum.�Ϲ�);
          Savemanager.Instance.SaveInventory();
          Savemanager.Instance.SaveLoginTime();
          Savemanager.Instance.Save();
          RewardButton.gameObject.SetActive(false);
          FinishButton.gameObject.SetActive(true);
          alertmanager.Instance.Alert_SeasonPass[1].SetActive(false);
          LoginEventManager.Instance.LoginNoti.SetActive(false);
        }

        break;
      case 3600:
        //������ �ִٸ�
        if (Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.���Ӻ���60��))
        {
          Inventory.Instance.AddItem(giveid, givehw);
          alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/������ɿϷ�"),alertmanager.alertenum.�Ϲ�);
          Savemanager.Instance.SaveInventory();
          Savemanager.Instance.SaveLoginTime();
          Savemanager.Instance.Save();
          RewardButton.gameObject.SetActive(false);
          FinishButton.gameObject.SetActive(true);
          alertmanager.Instance.Alert_SeasonPass[1].SetActive(false);
          LoginEventManager.Instance.LoginNoti.SetActive(false);
        }

        break;
      case 5400:
        //������ �ִٸ�
        if (Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.���Ӻ���90��))
        {
          Inventory.Instance.AddItem(giveid, givehw);
          alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/������ɿϷ�"),alertmanager.alertenum.�Ϲ�);
          Savemanager.Instance.SaveInventory();
          Savemanager.Instance.SaveLoginTime();
          Savemanager.Instance.Save();
          RewardButton.gameObject.SetActive(false);
          FinishButton.gameObject.SetActive(true);
          alertmanager.Instance.Alert_SeasonPass[1].SetActive(false);
          LoginEventManager.Instance.LoginNoti.SetActive(false);
        }

        break;
      case 7200:
        //������ �ִٸ�
        if (Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.���Ӻ���120��))
        {
          Inventory.Instance.AddItem(giveid, givehw);
          alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/������ɿϷ�"),alertmanager.alertenum.�Ϲ�);
          Savemanager.Instance.SaveInventory();
          Savemanager.Instance.SaveLoginTime();
          Savemanager.Instance.Save();
          RewardButton.gameObject.SetActive(false);
          FinishButton.gameObject.SetActive(true);
          alertmanager.Instance.Alert_SeasonPass[1].SetActive(false);
          LoginEventManager.Instance.LoginNoti.SetActive(false);
        }

        break;
      case 9000:
        //������ �ִٸ�
        if (Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.���Ӻ���150��))
        {
          Inventory.Instance.AddItem(giveid, givehw);
          alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/������ɿϷ�"),alertmanager.alertenum.�Ϲ�);
          Savemanager.Instance.SaveInventory();
          Savemanager.Instance.SaveLoginTime();
          Savemanager.Instance.Save();
          RewardButton.gameObject.SetActive(false);
          FinishButton.gameObject.SetActive(true);
          alertmanager.Instance.Alert_SeasonPass[1].SetActive(false);
          LoginEventManager.Instance.LoginNoti.SetActive(false);
        }
        break;
    }
  
  }
}
