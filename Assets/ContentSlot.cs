using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentSlot : MonoBehaviour
{
    [SerializeField] private int rank;
    [SerializeField] private string contentname;
    public GameObject LockPanel;
    public Text LockLevelText;

    private void OnEnable()
    {
        if (rank > PlayerBackendData.Instance.GetAdLv())
        {
            LockPanel.SetActive(true);
            LockLevelText.text = string.Format(Inventory.GetTranslate("UI2/모험랭크엔터"), rank.ToString());
            return;
        }
        else
        {
            LockPanel.SetActive(false);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
        }
    }

    public void ShowContent()
    {
        if (rank > PlayerBackendData.Instance.GetAdLv())
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/랭크가낮다"),alertmanager.alertenum.일반);
            return;
        }
        
        switch (contentname)
        {
            case "파티레이드":
                PartyRaidRoommanager.Instance.PartyradPanel.Show(false);
                break;
           case "개미굴":
               Antcavemanager.Instance.ShowAntCave();
               break;
           case "대마법사":
               Contentmanager.Instance.Bt_SelectContent("0");
               break;
           case "탐욕의금고":
               Contentmanager.Instance.Bt_SelectContent("1");
               break;
           case "시간의경계":
               Contentmanager.Instance.Bt_SelectContent("2");
               break;
           case "용암굴":
               Contentmanager.Instance.Bt_SelectContent("3");
               break;
           case "유물도시":
               Contentmanager.Instance.Bt_SelectContent("4");
               break;
            case "강화던전":
                Contentmanager.Instance.Bt_SelectContent("1");
                break;
        }
    }
}
