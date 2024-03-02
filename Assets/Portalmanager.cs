using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portalmanager : MonoBehaviour
{
    private static Portalmanager _instance = null;
    public static Portalmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Portalmanager)) as Portalmanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    bool canenter;
    public GameObject Portal;
    public string PortalId; //��Ż ������ ���������̴�.

    public void ShowPortal()
    {
        return;
        //���̵� ����
        Portal.SetActive(true);
        canenter = true;
        alertmanager.Instance.ShowAlert("��Ż�� �����Ͽ����ϴ�.",alertmanager.alertenum.�Ϲ�);
        Invoke(nameof(FalsePortal), 60);
    }

    void FalsePortal()
    {
        if (canenter)
        {
            //�ð��� ���� �����
            canenter = false;
            Portal.SetActive(false);
            alertmanager.Instance.ShowAlert("��Ż�� ������ϴ�.", alertmanager.alertenum.�Ϲ�);
        }
    }

    public void EnterPortal()
    {
        if(canenter)
        {
            Portal.SetActive(false);
            canenter = false;
        }
    }

    public void RemovePortal()
    {
        CancelInvoke(nameof(FalsePortal));
        FalsePortal();
    }
    public bool canmakeportal()
    {
        if (canenter)
        {
            return false;
        }
        else
            return true;
    }
}
