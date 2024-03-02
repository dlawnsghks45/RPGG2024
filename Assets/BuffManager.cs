using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public Buffslot[] Buffslots;
    public int[] BuffSlotNumber = new int[10]; 


    public void SetBuff(int buffslotsnum,string spriteimage)
    {
        for (int i = 0; i < Buffslots.Length; i++)
        {
            if (Buffslots[i].isbuff && BuffSlotNumber[i].Equals(buffslotsnum))
            {
//               Debug.Log("버프가 있다" +  i +"자리");
                //다시 이미지 안만들고 삭제
                return;
            }
        }
        for (var index = 0; index < Buffslots.Length; index++)
        {
            var t = Buffslots[index];
            if (t.isbuff) continue;
            BuffSlotNumber[buffslotsnum] = index;
            t.SetBuff(SpriteManager.Instance.GetSprite(spriteimage));
            t.gameObject.SetActive(true);
            break;
        }
    }

    public void EndBuff(int buffslotsnum)
    {
        Buffslots[BuffSlotNumber[buffslotsnum]].FinishBuff();
    }

}
