using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equipskillmanager : MonoBehaviour
{
    //�̱��游���.
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
        //�⺻ ����
        manadrain,
        manadrainhitper,
        manadrainlv,
        manadrainrare,
        //��ų ���
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
        //��ȭ��
        legensworddmg,
        legenswordhitper,
        legenswordlv,
        legenswordrare,
        //��ȭ��
        legendaggerdmg,
        legendaggerhitper,
        legendaggerlv,
        legendaggerrare,
        //���ڵ�
        legenbowdmg,
        legenbowhitper,
        legenbowwlv,
        legenbowrare,
        //���ڿ��� ������
        legenstaffdmg, //���� ������
        legenstaffhitper, //�����Ҹ�
        legenstafflv,
        legenstaffrare,
        //����ǳ�
        legendotmaxstack, //���� ������
        legendotaddstack, //�����Ҹ�
        legendotlv,
        legendotrare,
        //�ɷ�ġ �ۼ�Ʈ
        //�ɷ�ġ 
        strup,
        strperup,
        dexup,
        dexperup,   
        intup,
        intperup,
        wisup,
        wisperup,
        //ġȮ ġ��
        critper,
        critdmgper,
        physicperup,
        magicperup,
        dotperup,
        basicatk,
        potionup,
        //�������� �г�
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
        //�����̾�,
        legendaxehp, //����� ������
        legendaxecritdmg, //ġ��Ÿ ���ط�
        //��ȭ��
        legenmacedmg,
        legenmacehitper,
        legenmacelv,
        legenmacerare,
        //��׳���ũ
        legenwanddmg,
        legenwandhitper,
        legenwandlv,
        legenwandrare,
        //����
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
