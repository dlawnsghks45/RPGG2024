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
            case 0: //����
                break;
            case 1: //����
                break;
            case 2: //ĳ���� 
                break;
            case 3: //����
                break;
            case 4: //���
                break;
        }
    }
}
