using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmeltSlot : MonoBehaviour
{
    bool issmelt;
    public GameObject[] SmeltSontes;

    public void Refresh(int num)
    {
        foreach (var t in SmeltSontes)
        {
            t.SetActive(false);
        }
    }
    
    public void SmeltShow(int num)
    {
        switch(num)
        {
            case 0:
                SmeltSontes[0].SetActive(false);
                SmeltSontes[1].SetActive(false);
                break;
            case 1:
                SmeltSontes[0].SetActive(true);
                break;
            case 2:
                SmeltSontes[1].SetActive(true);
                break;
            case 3:
                SmeltSontes[0].SetActive(false);
                SmeltSontes[1].SetActive(false);
                break;
        }
    }
}
