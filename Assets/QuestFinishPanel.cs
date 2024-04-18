using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestFinishPanel : MonoBehaviour
{
    public Text NameText;
    public void Refresh(string name)
    {
        NameText.text = string.Format(Inventory.GetTranslate("UI8/Äù½ºÆ®¿Ï·á"),name) ;
    }
}
