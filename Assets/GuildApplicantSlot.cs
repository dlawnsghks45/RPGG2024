using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildApplicantSlot : MonoBehaviour
{
    public Text PlayerName;

    public string indate;
    public void RefreshData(ApplicantsItem item)
    {
        PlayerName.text = item.nickname;
        indate = item.inDate;
    }
    //����
    public void Bt_ApplyPlayer()
    {
        MyGuildManager.Instance.ApplyApplicantPlayer(indate);
    }
    //����
    public void RejectPlayer()
    {
        MyGuildManager.Instance.ReJectApplicant(indate);
    }

    public void Bt_ShowPlayerInfo()
    {
        otherusermanager.Instance.ShowPlayerData(PlayerName.text);
    }
}
