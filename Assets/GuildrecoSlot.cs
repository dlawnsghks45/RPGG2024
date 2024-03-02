using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildrecoSlot : MonoBehaviour
{
    public GuildEmblemSlot emblem;
    public Text GuildName;
    public Text GuildLv;
    public Text GuildMasterName;
    public Text GuildMember;
    public Text GuildWelcome;
    public string guildindate;

    private GuildItem data;
    public void Refresh(GuildItem data)
    {
        this.data = data;
        emblem.SetEmblem(data.GuildFlag,data.GuildBanner);
        GuildName.text = data.guildName;
        GuildLv.text = $"Lv.{data.level.ToString()}";
        GuildMasterName.text = data.masterNickname;
        GuildMember.text = $"{data.memberCount}/{data.GuildMaxMemer}";
        GuildWelcome.text = data.GuildWelcome;
        guildindate = data.inDate;
    }

    public void Bt_ShowGuildInfo()
    {
        GuildManager.Instance.Bt_ShowSelectGuild(data);
    }
}
