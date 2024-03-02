using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equipskillmanager : MonoBehaviour
{
    //싱글톤만들기.
    private static equipskillmanager _instance = null;
    public static equipskillmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(equipskillmanager)) as equipskillmanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }
    [SerializeField]
    float[] EquipStat = new float[(int)EquipStatFloat.Length];

    private void Awake()
    {
        EquipStat = new float[(int)EquipStatFloat.Length];
    }

    public enum EquipStatFloat
    {
        //기본 공격
        manadrain,
        manadrainhitper,
        manadrainlv,
        manadrainrare,
        //스킬 사용
        smitedmg,
        smitehitper,
        smitelv,
        smiterare,
        thundersmash,
        thundersmashhitper,
        thundersmashlv,
        thundersmashrare,
        explosion,
        explosionhitper,
        explosionlv,
        explosionrare,
        //염화검
        legensworddmg,
        legenswordhitper,
        legenswordlv,
        legenswordrare,
        //멸화도
        legendaggerdmg,
        legendaggerhitper,
        legendaggerlv,
        legendaggerrare,
        //블리자드
        legenbowdmg,
        legenbowhitper,
        legenbowwlv,
        legenbowrare,
        //대자연의 지팡이
        legenstaffdmg, //피해 증가량
        legenstaffhitper, //마나소모량
        legenstafflv,
        legenstaffrare,
        //사신의낫
        legendotmaxstack, //피해 증가량
        legendotaddstack, //마나소모량
        legendotlv,
        legendotrare,
        //능력치 퍼센트
        //능력치 
        strup,
        strperup,
        dexup,
        dexperup,   
        intup,
        intperup,
        wisup,
        wisperup,
        //치확 치피
        critper,
        critdmgper,
        physicperup,
        magicperup,
        dotperup,
        basicatk,
        potionup,
        //레이즈의 분노
        razerage,
        razeragehitper,
        razeragelv,
        balrockrage,
        balrockhitper,
        balrockragelv,
        bossadddmg,
        reskilllv,
        reskillhitper,
        reskilrare,
        maxhpupper,
        maxmpupper,
        //슬레이어,
        legendaxehp, //생명력 증가량
        legendaxecritdmg, //치명타 피해량
        //멸화도
        legenmacedmg,
        legenmacehitper,
        legenmacelv,
        legenmacerare,
        //라그나로크
        legenwanddmg,
        legenwandhitper,
        legenwandlv,
        legenwandrare,
        //상저
        warriorrune,
        magerune,
        destroyervalue,
        scarvalue,
        basicmonsterdmg,
        goldexp,
        Length
    }

    public float GetStats(int num)
    {

        return EquipStat[num];
    }

    public void SetStats(int num , float value)
    {
        EquipStat[num] += value;
    }
    public void ResetStats()
    {
        for(int i = 0; i < EquipStat.Length;i++)
        {
            EquipStat[i] = 0;
        }
     
    }

    public Transform slotmother;
    public equipskillslot[] equipskillslots;
    [SerializeField]
    int esnum = 0;

    public void showequipslots(string id,string rare,string lv)
    {
        if(!SettingReNewal.Instance.EskillPanel[0].IsOn)
            return;
        
        equipskillslots[esnum].SetSkill(id,rare,lv);
        equipskillslots[esnum].transform.SetAsLastSibling();
        esnum++;
        if(esnum >= equipskillslots.Length)
        {
            esnum = 0;
        }
    }


}
