using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSlot : MonoBehaviour
{
    public Text ItemName;
    public Text ItemStatText;




    public void RefreshShow(string names, string rare, decimal num, bool ispercent, bool ishundred,bool isyellow = false)
    {
        ItemName.text = names;

        if (ispercent)
        {
            if (ishundred)
            {
                ItemStatText.text = $"{num * 100m:N0}%";

            }
            else
            {
                ItemStatText.text = $"{num}%";
            }
        }
        else
        {
            ItemStatText.text = num.ToString("N0");
        }

        if (isyellow)
        {
            ItemStatText.color= Color.yellow;
        }
        else
        {
            ItemStatText.color = Color.white;
        }
        Inventory.Instance.ChangeItemRareColor(ItemName, rare);
    }
}
