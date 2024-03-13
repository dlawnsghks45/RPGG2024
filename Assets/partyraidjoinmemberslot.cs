using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class partyraidjoinmemberslot : MonoBehaviour
{
  public Image Avata;
  public Image Weapon;
  public Image SubWeapon;

  public Text LvName;
  public Text Rank;


  public string namestring;

  public void SetData(string names,string[] avatapath,int lv,int rank)
  {
    namestring = names;

    LvName.text = $"Lv.{lv} {namestring}";
    Rank.text = string.Format(Inventory.GetTranslate("UI2/¸ðÇè·©Å©"), rank);

    if (avatapath[0] != "")
    {
      Avata.sprite = SpriteManager.Instance.GetSprite(avatapath[0]);
      Avata.enabled = true;

    }
    else
    {
      Avata.enabled = false;

    }
    if (avatapath[1] != "")
    {
      Weapon.sprite = SpriteManager.Instance.GetSprite(avatapath[1]);
      Weapon.enabled = true;

    }
    else
    {
      Weapon.enabled = false;

    }
    if (avatapath[2] != "")
    {
      SubWeapon.sprite = SpriteManager.Instance.GetSprite(avatapath[2]);
      SubWeapon.enabled = true;

    }
    else
    {
      SubWeapon.enabled = false;
    }
  }

  public void Bt_AcceptJoin()
  {
    PartyRaidRoommanager.Instance.joinusername.Remove(namestring);
    PartyraidChatManager.Instance.Chat_SendJoinAccept(namestring);
    PartyRaidRoommanager.Instance.RefreshJoinmemberCount();
  }

  public void Bt_ShowPlayerData()
  {
    otherusermanager.Instance.ShowPlayerData(namestring);
    PartyRaidRoommanager.Instance.PartyRaidMemberJoinPanel.SetActive(false);
  }
}
