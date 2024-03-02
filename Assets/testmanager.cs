using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class testmanager : MonoBehaviour
{
    public TMP_InputField itemid;
    public TMP_InputField itemhowmany;
    public void Bt_CreateItem()
    {
        try
        {
            PlayerBackendData.Instance.Additem(itemid.text, int.Parse(itemhowmany.text));
        }
        catch
        {
           // Debug.Log("아이템이없음");
        }

    }
    
}
