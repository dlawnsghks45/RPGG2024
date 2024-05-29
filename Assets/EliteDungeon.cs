using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EliteDungeon : MonoBehaviour
{
    public countpanel DungeonLevelCount;
    public itemiconslot[] Reward_Dungeon;
    public itemiconslot NeedDungeon;

    public int nowsetlevel;


    public Text Hp;
    
    public void Refresh()
    {
        nowsetlevel = PlayerBackendData.Instance.ContentLevel[10] + 1;
        DungeonLevelCount.SetCount(nowsetlevel);
    }

    void RefreshHp()
    {
        
    }

   
}
