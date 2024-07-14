using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class partyraidreadyslot : MonoBehaviour
{
    public GameObject notreadyobj;
    public GameObject readyobj;
    public Text UserNickanme;
    public bool isready;
    public void InitData(string Nickname)
    {
        if (PartyRaidRoommanager.Instance.nowmyleadernickname == Nickname)
        {
            notreadyobj.SetActive(false);
            readyobj.SetActive(true);
            isready = true;
            if (Nickname == PlayerBackendData.Instance.nickname)
            {
                PartyRaidRoommanager.Instance.readyyesnopanel.SetActive(false);
            }
        }
        else
        {
            if (Nickname == PlayerBackendData.Instance.nickname)
            {
                PartyRaidRoommanager.Instance.readyyesnopanel.SetActive(true);
            }

            notreadyobj.SetActive(true);
            readyobj.SetActive(false);
            isready = false;
        }

        UserNickanme.text = Nickname;
    }

    public void SetReady()
    {
        notreadyobj.SetActive(false);
        readyobj.SetActive(true);
        isready = true;
        PartyRaidRoommanager.Instance.OnlyReadyCheck();
    }
}
