using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castingmanager : MonoBehaviour
{


    public Player mainplayer;
    public castingbarslot[] Castingbar;
    public Skillslot[] skillslots;

    public void ResetAllCasting()
    {
        for(int i = 0; i < Castingbar.Length;i++)
        {
            Castingbar[i].FinishCasting();
        }
    }


    public bool CastingSpell(float time, float cost,Skillslot skilldata)
    {
        for (int i = 0; i < mainplayer.multicasting; i++)
        {
            //캐스팅 자리가있다면
            if (!Castingbar[i].isCasting)
            {
                if (mainplayer.hpmanager.reduceMp(cost))
                {
//                    Debug.Log("캐팅전 스피드" + time);
                    time -= time * mainplayer.stat_castspeed;
                //    Debug.Log("캐스팅후 스피드" + time);
                    Castingbar[i].SetCasting(time,skilldata);
                    return true;
                }
            }
        }
        return false;
    }

    public bool CastingSpellNoCastime(float cost, Skillslot skilldata)
    {
        //캐스팅 자리가있다면
        if (!Castingbar[4].isCasting)
        {
            if (mainplayer.hpmanager.reduceMp(cost))
            {
                Castingbar[4].SetCasting(0, skilldata);
                return true;
            }
        }
        return false;
    }
}
