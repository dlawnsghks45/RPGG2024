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
            LockLevelText.text = string.Format(Inventory.GetTranslate("UI2/���跩ũ����"), rank.ToString());
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
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/��ũ������"),alertmanager.alertenum.�Ϲ�);
            return;
        }
        
        switch (contentname)
        {
            case "��Ƽ���̵�":
                PartyRaidRoommanager.Instance.PartyradPanel.Show(false);
                break;
           case "���̱�":
               Antcavemanager.Instance.ShowAntCave();
               break;
           case "�븶����":
               Contentmanager.Instance.Bt_SelectContent("0");
               break;
           case "Ž���Ǳݰ�":
               Contentmanager.Instance.Bt_SelectContent("1");
               break;
           case "�ð��ǰ��":
               Contentmanager.Instance.Bt_SelectContent("2");
               break;
           case "��ϱ�":
               Contentmanager.Instance.Bt_SelectContent("3");
               break;
           case "��������":
               Contentmanager.Instance.Bt_SelectContent("4");
               break;
            case "��ȭ����":
                Contentmanager.Instance.Bt_SelectContent("1");
                break;
        }
    }
}
