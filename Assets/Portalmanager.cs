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
    public string PortalId; //포탈 종류는 여러가지이다.

    public void ShowPortal()
    {
        return;
        //아이디 랜덤
        Portal.SetActive(true);
        canenter = true;
        alertmanager.Instance.ShowAlert("포탈이 출현하였습니다.",alertmanager.alertenum.일반);
        Invoke(nameof(FalsePortal), 60);
    }

    void FalsePortal()
    {
        if (canenter)
        {
            //시간이 지나 사라짐
            canenter = false;
            Portal.SetActive(false);
            alertmanager.Instance.ShowAlert("포탈이 사라집니다.", alertmanager.alertenum.일반);
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
