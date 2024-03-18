using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject EquipSkillObj;
    public Image EquipskillImage_Weapon;
    public Image EquipskillBar;
    public float equipskillcur;
    public float equipskillmax;


    public void OffEquipSkills()
    {
        EquipSkillObj.SetActive(false);
    }

    public string nowequipid;
    public void SetEquipSkills(string equipid,float max)
    {
        if(nowequipid.Equals(equipid) && EquipSkillObj.activeSelf)
            return;

        nowequipid = equipid;
        EquipskillImage_Weapon.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(equipid).Sprite);
        EquipSkillObj.SetActive(true);
        equipskillmax = max;
        equipskillcur = 0;
        RefreshBar();
    }

    void RefreshBar()
    {
        EquipskillBar.fillAmount = equipskillcur / equipskillmax;
    }

    public void AddStack(float stack)
    {
        equipskillcur += stack;
        if (equipskillcur >= equipskillmax)
        {
            equipskillcur = equipskillmax;
        }
    }

    public bool IsMaxStack()
    {
        if (equipskillcur >= equipskillmax)
        {
            equipskillcur = 0;
            RefreshBar();
            return true;
        }
        else
        {
            RefreshBar();
            return false;
        }
        
    }
}
