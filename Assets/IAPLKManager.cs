using BackEnd;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPLKManager : MonoBehaviour
{
    private void OnDestroy()
    {
        _instance = null;
    }

private static IAPLKManager _instance = null;
    public static IAPLKManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(IAPLKManager)) as IAPLKManager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }


    public void FailedBuy()
    {
        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/구매실패"),alertmanager.alertenum.주의);
    }

    public static string nowproductid;

    public void OnPurChaseCompleted(Product args)
    {
        Debug.Log(args.definition.id);
        if (args.definition.id == "") return;
    }

    


}
public enum PremiumShopPackageEnum
{
    일일패키지한정1,
    일일패키지한정2,
    Length
}
