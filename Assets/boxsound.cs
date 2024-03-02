using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxsound : MonoBehaviour
{
    public void SoundA()
    {
        if (petmanager.Instance.RareUpObj.activeSelf)
        {
            Soundmanager.Instance.PlayerSound2("Sound/Special Click 05",1f);

        }
        else
        {
            Soundmanager.Instance.PlayerSound2("Sound/UI Tight 04", 1f);
        }
    }

    public void SoundB()
    {
        Soundmanager.Instance.PlayerSound2("Sound/상자열기", 1f);
    }
}