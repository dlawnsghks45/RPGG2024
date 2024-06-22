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
        allstat,
        allstatperup,
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
        reskilllv2,
        reskillhitper2,
        reskilrare2,
        maxhpupper,
        maxmpupper,
        //슬레이어,
        legendaxehp, //생명력 증가량
        legendaxecritdmg, //치명타 피해량
        //러브
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
        gold,
        exp,
        //2차시즌무기
        E6138, //파멸의 검
        E6138_2, //파멸의 검
        E6139, //하데스의 도끼
        E6139_2, //하데스의 도끼
        E6140, //거인 분쇄기
        E6140_2, //거인 분쇄기
        E6141, //적화의 건틀릿
        E6141_2, //적화의 건틀릿
        E6141_rare, //적화의 건틀릿
        E6141_lv, //적화의 건틀릿
        E6142, //레일 건
        E6142_2, //레일 건
        E6142_rare, //레일 건
        E6142_lv, //레일 건
        E6143, //드래곤 보우
        E6143_2, //드래곤 보우
        E6143_lv, //드래곤 보우
        E6143_rare, //드래곤 보우
        E6144, //블러드 커터
        E6144_2, //블러드 커터
        E6144_3, //블러드 커터
        E6144_lv, //블러드 커터
        E6144_rare, //블러드 커터
        E6145, //단테의 석궁
        E6145_2, //단테의 석궁
        E6145_3, //단테의 석궁
        E6145_lv, //단테의 석궁
        E6145_rare, //단테의 석궁
        E6146, //제우스의 지팡이
        E6146_2, //제우스의 지팡이
        E6146_lv, //제우스의 지팡이
        E6146_rare, //제우스의 지팡이
        E6147, //포세이돈의 지팡이
        E6147_2, //포세이돈의 지팡이
        E6147_lv, //포세이돈의 지팡이
        E6147_rare, //포세이돈의 지팡이
        E6148, //아폴론의 메이스
        E6148_2, //아폴론의 메이스
        E6148_3, //아폴론의 메이스
        E6148_lv, //아폴론의 메이스
        E6148_rare, //아폴론의 메이스
        E6149, //처단자
        E6149_2, //처단자
        E6150, //고통
        E6150_2, //고통,
        E6159,//청혼의 건틀릿
        E6159_2,//청혼의 건틀릿
        E6159_3,//청혼의 건틀릿
        E6160,//드래곤 애로우
        totaldmg, //피해증가
        E61521, //크로노스
        E61521_2, //크로노스
        E61521_lv, //크로노스
        E61521_rare, //크로노스
        E61611,//크로노스 보조무기
        E61511, //절명의 채찍
        E61511_2, //절멸의 채찍
        E61621,
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
