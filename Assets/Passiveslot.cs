using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Passiveslot : MonoBehaviour
{
    public Text ClassName;
    public Text PassiveInfo;


    public void Refresh(string passiveid)
    {
        //ÆÐ½Ãºê
//        Debug.Log(passiveid);
        ClassName.text = Inventory.GetTranslate(PassiveDB.Instance.Find_id(passiveid).name);
        Inventory.Instance.ChangeItemRareColor(ClassName,ClassDB.Instance.Find_id(passiveid).tier);

        PassiveInfo.text = Inventory.GetTranslate(PassiveDB.Instance.Find_id(passiveid).info);
     
    }
}
