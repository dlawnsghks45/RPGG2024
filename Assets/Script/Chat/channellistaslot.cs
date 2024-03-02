using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class channellistaslot : MonoBehaviour
{
    ChatChannel channeldata;
    public Text Channelname;
    public Text ChannelPersonCount;

    public void Channel(ChatChannel data,string cn,string cpc)
    {
        channeldata = data;
        Channelname.text = cn;
        ChannelPersonCount.text = cpc;
    }
    public void Bt_JoinChannel()
    {
       // chatmanager.Instance.JoinChannel(channeldata);
    }
}
