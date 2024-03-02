using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildRaidmemberslot : MonoBehaviour
{
    public Text membername;
    public Text counttext;


    public void Refresh(string playername, decimal dmg, decimal monhp)
    {
        membername.text = playername;
        //���� ���� �ۼ�Ʈ ���

        decimal percent = (dmg / monhp) * 100m;


        if (dmg == -1)
        {
            counttext.text = $"{dmg:N0}<color=cyan>(MAX DMG)</color>";
            return;
        }
        
        if (percent >= 1)
        {
            //����޴� ����
            counttext.text = $"{dmg:N0}<color=cyan>({percent:N1}%)</color>";
        }
        else
        {
            counttext.text = $"{dmg:N0}<color=red>({percent:N1}%)</color>";
        }
        
    }
}
