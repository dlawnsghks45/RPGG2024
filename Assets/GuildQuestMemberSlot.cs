using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildQuestMemberSlot : MonoBehaviour
{
    public Text membername;
    public Text counttext;
    

    public void Refresh(string playername, int count)
    {
        membername.text = playername;
        counttext.text = count.ToString("N0");
    }
}
