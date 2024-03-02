using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class guildmemberslot : MonoBehaviour
{
    public Sprite[] GuildGradeSpr;
    public Image GuildGrade;

    public Text Name;
    public Sprite[] IsOnlinesp; //0비접속 1 접속
    public Image GuildOnlineImage;
    public Text onlinetext;
    public GameObject Membermanagementbt;
    private GuildMemberInfo data;
    public Text MyPt;
    public void Refresh(bool ismine,GuildMemberInfo info,int pt)
    {
        data = info;
        // 길드원 1 길드 권한 (master/viceMaster/member)
        switch (info.position)
        {
            case "master":
                GuildGrade.sprite = GuildGradeSpr[0];
                break;
            case "viceMaster":
                GuildGrade.sprite = GuildGradeSpr[1];
                break;
            case "member":
                GuildGrade.sprite = GuildGradeSpr[2];
                break;
            
            
        }
        Name.text = info.nickname;

        
        if (ismine)
        {
            if (PlayerBackendData.Instance.nickname.Equals(MyGuildManager.Instance.myguildclassdata.masterNickname)
                || MyGuildManager.Instance.myguildclassdata.viceMasterList.ContainsKey(PlayerBackendData.Instance.playerindate))
            {
                Membermanagementbt.SetActive(true);
                if (info.nickname.Equals(PlayerBackendData.Instance.nickname))
                {
                    //길마는 자기꺼가 안떠야한다.
                    Membermanagementbt.SetActive(false);
                }
            }
            else
            {
                Membermanagementbt.SetActive(false);
            }


            MyPt.text = pt.ToString("N0");
            GuildOnlineImage.enabled = true;
            onlinetext.text = info.lastLoginString;
            if (info.isActive)
            {
                GuildOnlineImage.sprite = IsOnlinesp[1];
                onlinetext.color = Color.green;
            }
            else
            {
                GuildOnlineImage.sprite = IsOnlinesp[0];
                onlinetext.color = Color.gray;

            }
        }
        else
        {
            GuildOnlineImage.enabled = false;
            onlinetext.text = "";
            
        }
    }

    public void Bt_ShowManagerment()
    {
        MyGuildManager.Instance.ShowGuildMemberInfo(data);
    }

    public void Bt_ShowPlayerSpec()
    {
        uimanager.Instance.AddUiview(MyGuildManager.Instance.MyGuildPanel,true);
        otherusermanager.Instance.ShowPlayerData(data.nickname);
    }
}
