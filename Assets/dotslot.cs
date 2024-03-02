using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dotslot : MonoBehaviour
{
    public TMPro.TextMeshProUGUI StackText;

    public void Refresh(int stack)
    {
            StackText.text = stack.ToString("N0");
    }
}
