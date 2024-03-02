using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject[] Buttonpanels;
    public void OpenPanel(int num)
    {
        foreach (var t in Buttonpanels)
        {
            t.SetActive(false);
        }
            Buttonpanels[num].SetActive(true);
        switch(num)
        {
            case 0: //상점
                break;
            case 1: //전투
                break;
            case 2: //캐릭터 
                break;
            case 3: //영지
                break;
            case 4: //길드
                break;
        }
    }
}
