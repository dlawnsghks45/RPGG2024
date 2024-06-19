using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DamageNumbersPro; //Import Namespace
using Sirenix.OdinInspector;

public class Player : MonoBehaviour
{
    public Castingmanager castingmanager;
    public Hpmanager hpmanager;

    public BuffManager buffmanager;

    //Assign Prefab in Inspector
    public bool isme;
    public AttackType attacktype;
    public Transform MeleeSit;
    public Transform RangeSit;
    public Transform ArrowPos;
    public string basicattackhit;
    public float arrowrotation;
    public string[] attacktrigger;

    Animator ani;
    int poolcount = 30;
    public string playeravartaid;

    #region 능력치들
    
    
    //탈리스만
    public float talisman_allstat = 0;
    public float talisman_atk = 0;
    public float talisman_matk = 0;
    public float talisman_crit = 0;
    public float talisman_critdmg = 0;
    public int talisman_MaxDotCount = 0; //�ִ� ��Ʈ ��  //�⺻ 10
    public float talisman_DotDmgUp = 0; //�ִ� ��Ʈ ��  //�⺻ 10
    public int talisman_AddStack = 0; //��Ʈ ��ų ��� �� �߰��Ǵ� ���
    public int talisman_Attakcount = 0; //��Ʈ ��ų ��� �� �߰��Ǵ� ���
    public float talisman_Bossdmg = 0; //���� �߰� ����
    public float talisman_Mondmg = 0; //���� �߰� ����
    public float talisman_dmgup = 0; //���� �߰� ����
    public float talisman_Buff = 0; //���� �߰� ����
    public float talisman_Exp = 0; //���� �߰� ����
    public float talisman_Gold = 0; //���� �߰� ����
    public float talisman_ReduceCooldown = 0; //���� �߰� ����

    
    
    
    //수집
    public float Stat_collection = 0;
    
    

    public float set_atk = 0;
    public float set_matk = 0;
    public float set_def = 0;
    public float set_mdef = 0;
    public float set_str = 0;
    public float set_dex = 0;
    public float set_int = 0;
    public float set_wis = 0;
    public float set_hit = 0;
    public float set_crit = 0;
    public float set_critdmg = 0;
    public float set_hp = 0;
    public float set_mp = 0;
    public float set_atkspeed = 0;
    public float set_castspeed = 0;
    public int set_MaxDotCount = 0; //�ִ� ��Ʈ ��  //�⺻ 10
    public float set_DotDmgUp = 0; //�ִ� ��Ʈ ��  //�⺻ 10
    public int set_AddStack = 0; //��Ʈ ��ų ��� �� �߰��Ǵ� ���
    public int set_Bossdmg = 0; //���� �߰� ����
    public float set_reducedmg = 0;
    public float set_cooldown = 0;

    public float stat_atk = 0;
    public float stat_matk = 0;
    public float stat_def = 0;
    public float stat_mdef = 0;
    public float stat_str = 0;
    public float stat_dex = 0;
    public float stat_int = 0;
    public float stat_wis = 0;
    public float stat_hit = 0;
    public float stat_crit = 0;
    public float stat_critdmg = 0;
    public float stat_hp = 0;
    public float stat_mp = 0;
    public float stat_atkspeed = 0;
    public float stat_castspeed = 0;
    public float stat_Bossdmg = 0;
    public float stat_Monsterdmg = 0;
    public float stat_reducedmg = 0;
    public float stat_basicatkup = 0;
    public float stat_potionup = 0;

    public float stat_addmatkdmg = 0;
    public float stat_addatkdmg = 0;
    public float stat_usemoremana = 0;

    //��Ʈ ����
    public int Stat_MaxDotCount = 0; //�ִ� ��Ʈ ��  //�⺻ 10
    public float Stat_DotDmgUp = 0; //�ִ� ��Ʈ ��  //�⺻ 10
    public int Stat_AddStack = 0; //��Ʈ ��ų ��� �� �߰��Ǵ� ���
    public float Stat_ReduceCoolDown = 0; //�ִ� ��Ʈ ��  //�⺻ 10
    //무력화
    public float Stat_BreakDmg = 0; //�ִ� ��Ʈ ��  //�⺻ 10
    //추가 경험치
    public float Stat_ExtraExp = 0; //�ִ� ��Ʈ ��  //�⺻ 10
    public float Stat_ExtraGold = 0; //�ִ� ��Ʈ ��  //�⺻ 10
    public float Stat_ExtraDrop = 0; //�ִ� ��Ʈ ��  //�⺻ 10
    //제련 점수
    public int Stat_SmeltPoint = 0; //�ִ� ��Ʈ ��  //�⺻ 10
    public float Stat_SmeltDmg = 0; //�ִ� ��Ʈ ��  //�⺻ 10
    public float Stat_totalbuff = 0; //�ִ� ��Ʈ ��  //�⺻ 10


    const int maxdotcount = 20;

    public float gear_atk = 0;
    public float gear_matk = 0;
    public float gear_def = 0;
    public float gear_mdef = 0;
    public float gear_str = 0;
    public float gear_dex = 0;
    public float gear_int = 0;
    public float gear_wis = 0;
    public float gear_hit = 0;
    public float gear_crit = 0;
    public float gear_critdmg = 0;
    public float gear_hp = 0;
    public float gear_mp = 0;
    public float gear_atkspeed = 0;
    public float gear_castspeed = 0;
    public int gear_maxdotcount = 0;
    public float gear_dotdmgup = 0;
    public int gear_AddStack = 0;
    public float gear_bossdmg = 0;
    public float gear_reducedmg = 0;
    public float gear_basicatkup = 0;
    public float gear_potionup = 0;
    public float gear_allstat = 0;
    public float gear_alldmg = 0;

    public float ability_atk = 0;
    public float ability_matk = 0;
    public float ability_def = 0;
    public float ability_mdef = 0;
    public float ability_str = 0;
    public float ability_dex = 0;
    public float ability_int = 0;
    public float ability_wis = 0;
    
    public float ability_strnum = 0;
    public float ability_dexnum = 0;
    public float ability_intnum = 0;
    public float ability_wisnum = 0;
    
    public float ability_hit = 0;
    public float ability_crit = 0;
    public float ability_critdmg = 0;
    public float ability_hp = 0;
    public float ability_mp = 0;
    public float ability_atkspeed = 0;
    public float ability_castspeed = 0;
    public int ability_maxdotcount = 0;
    public float ability_dotdmgup = 0;
    public int ability_AddStack = 0;
    public float ability_bossdmg = 0;
    public float ability_monsterdmg = 0;
    public float ability_breakdmg = 0;
    public float ability_reducedmg = 0;
    public float ability_basicatkup = 0;
    public float ability_potionup = 0;
    public float ability_allstat = 0;
    public float ability_allstatnum = 0;
    public float ability_exp = 0;
    public float ability_gold = 0;
    public float ability_drop = 0;
    public float ability_cooldown = 0;
    public float ability_buff = 0;
    public float AlldmgUp = 0;
    public int ability_AttackCount = 0;

    public float pet_allstatnum = 0;
    public float pet_gold = 0;
    public float pet_exp = 0;

    
    
    //5단계 특화
    public float ability_basicskillper = 0;
    public float ability_basicskilldmg = 0;

    public float ability_magicskillper = 0;
    public float ability_magicskilldmg = 0;
    
    public float ability_critskillper = 0;
    public float ability_critskilldmg = 0;
    
    public bool abilityskillallatack = false;
    
    
    public float buff_atk = 0;
    public float buff_atkPercent = 0;
    public float buff_matk = 0;
    public float buff_matkPercent = 0;
    public float buff_def = 0;
    public float buff_mdef = 0;
    public float buff_str = 0;
    public float buff_dex = 0;
    public float buff_int = 0;
    public float buff_wis = 0;
    public float buff_hit = 0;
    public float buff_crit = 0;
    public float buff_critdmg = 0;
    public float buff_Allstatper = 0;
    public float buff_hp = 0;
    public float buff_mp = 0;
    public float buff_atkspeed = 0;
    public float buff_castspeed = 0;
    public int buff_maxdotcount = 0;
    public float buff_dotdmgup = 0;
    public int buff_AddStack = 0;
    public float buff_reducecooldown = 0;
    public float buff_bossdmg = 0;
    public float buff_reducedmg = 0;
    public float buff_basicatkup = 0;
    public float buff_potionup = 0;
    public float buff_alldmgup = 0;

    
    public int AttackCount = 0; //���� Ƚ��

    //��ų ����
    public int classskillslotcount = 0;
    public int skillslotcount = 0;

    //��Ƽ ĳ���� ����
    public int classmulticasting = 0;
    public int multicasting = 0;

    #endregion



    public void ClassStat()
    {
        foreach (var t in castingmanager.skillslots)
        {
            t.islock = true;
            t.SetLockSkill();

        }

        float slotscount = skillslotcount + classskillslotcount +
                         AdventureRankSkillSlot(PlayerBackendData.Instance.GetAdLv());
        if (slotscount >= 13)
            slotscount = 12;
        for (int i = 0;
             i < slotscount;
             i++)
        {
            if (skillslotcount + classskillslotcount + AdventureRankSkillSlot(PlayerBackendData.Instance.GetAdLv()) >=
                13)
            {
                continue;
            }
            castingmanager.skillslots[i].islock = false;
            castingmanager.skillslots[i].RefreshSkill();
        }

        multicasting = classmulticasting;

        //hpmanager.CurHp = stat_hp;
        hpmanager.MaxHp = (decimal)stat_hp;
        //hpmanager.CurMp = stat_mp;
        hpmanager.MaxMp = stat_mp;
    }

    //��ũ �ɷ�ġ
    public float AdventureRank()
    {
        int lv = PlayerBackendData.Instance.GetAdLv();
        if (lv == 1)
            return 0;
        else
        {
            //Debug.Log("dawd" + lv*0.1f);
            return lv * 0.1f;
        }
    }
    bool isequipskills = false;

    void CheckESKILL(EquipSkillDB.Row skilldata, EquipItemDB.Row data2 = null, EquipItemDB.Row data = null)
    {
        switch (skilldata.coreid)
        {
            case "1000": //������ ���
                if (data2 == null)
                    return;
                switch (data2.SubType)
                {
                    case "Physic":
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.smitedmg,
                            float.Parse(skilldata.A));
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.smitehitper,
                            float.Parse(skilldata.c));
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.smiterare,
                            float.Parse(skilldata.rare));
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.smitelv,
                            float.Parse(skilldata.lv));
                        break;
                    case "Magic":
                    case "Dot":
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.manadrain,
                            float.Parse(skilldata.value));
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.manadrainhitper,
                            float.Parse(skilldata.probability));
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.manadrainlv,
                            float.Parse(skilldata.lv));
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.manadrainrare,
                            float.Parse(skilldata.rare));
                        break;
                }


                break;
            case "1010": //��Ÿ
                if (data2 == null)
                    return;
                switch (data2.SubType)
                {
                    case "Physic":
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.smitedmg,
                            float.Parse(skilldata.A));
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.smitehitper,
                            float.Parse(skilldata.c));
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.smiterare,
                            float.Parse(skilldata.rare));
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.smitelv,
                            float.Parse(skilldata.lv));
                        break;
                    case "Magic":
                    case "Dot":
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.manadrain,
                            float.Parse(skilldata.value));
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.manadrainhitper,
                            float.Parse(skilldata.probability));
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.manadrainlv,
                            float.Parse(skilldata.lv));
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.manadrainrare,
                            float.Parse(skilldata.rare));
                        break;
                }

                break;
            case "1020": //õ�� ����

                if (data2 == null)
                    return;
                switch (data2.SubType)
                {
                    case "Physic":
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.thundersmash,
                            float.Parse(skilldata.c));
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.thundersmashhitper,
                            float.Parse(skilldata.A));
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.thundersmashrare,
                            float.Parse(skilldata.rare));
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.thundersmashlv,
                            float.Parse(skilldata.lv));
                        break;
                    case "Magic":
                    case "Dot":
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.explosion,
                            float.Parse(skilldata.value));
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.explosionhitper,
                            float.Parse(skilldata.probability));
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.explosionrare,
                            float.Parse(skilldata.rare));
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.explosionlv,
                            float.Parse(skilldata.lv));
                        break;
                }

                break;
            case "1030": //������
            case "1050": //��ø����
            case "1070": //��������
            case "1090": //��������
            case "1334": //��������
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.allstat,
                    float.Parse(skilldata.value));
                break;
            case "1040": //������ �ۼ�Ʈ
            case "1060": //��ø���� �ۼ�Ʈ
            case "1080": //�������� �ۼ�Ʈ
            case "1344": //�������� �ۼ�Ʈ
            case "1100": //�������� �ۼ�Ʈ

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.allstatperup,
                    float.Parse(skilldata.value));
                break;

            case "1374": //������
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.strup,
                    float.Parse(skilldata.value));
                break;
            case "1384": //������ �ۼ�Ʈ
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.strperup,
                    float.Parse(skilldata.value));
                break;
            case "1394": //��ø����
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.dexup,
                    float.Parse(skilldata.value));
                break;
            case "1404": //��ø���� �ۼ�Ʈ
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.dexperup,
                    float.Parse(skilldata.value));
                break;

            case "1414": //��������
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.intup,
                    float.Parse(skilldata.value));
                break;
            case "1424": //�������� �ۼ�Ʈ
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.intperup,
                    float.Parse(skilldata.value));
                break;

            case "1434": //��������
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.wisup,
                    float.Parse(skilldata.value));
                break;
            case "1444": //�������� �ۼ�Ʈ
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.wisperup,
                    float.Parse(skilldata.value));
                break;
            case "1354": //��������
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.physicperup,
                    float.Parse(skilldata.value));
//                            Debug.Log("물공증가횟수");
                break;
            case "1364": //마법공격력
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.magicperup,
                    float.Parse(skilldata.value));
                break;
            case "1130": //
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.critper,
                    float.Parse(skilldata.value));
                break;
            case "1140": //�������� �ۼ�Ʈ
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.critdmgper,
                    float.Parse(skilldata.value));
                break;
            case "1150": //�ͽ��÷���
                if (data2 == null)
                    return;
                switch (data2.SubType)
                {
                    case "Physic":
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.thundersmash,
                            float.Parse(skilldata.c));
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.thundersmashhitper,
                            float.Parse(skilldata.A));
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.thundersmashrare,
                            float.Parse(skilldata.rare));
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.thundersmashlv,
                            float.Parse(skilldata.lv));
                        break;
                    case "Magic":
                    case "Dot":
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.explosion,
                            float.Parse(skilldata.value));
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.explosionhitper,
                            float.Parse(skilldata.probability));
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.explosionrare,
                            float.Parse(skilldata.rare));
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.explosionlv,
                            float.Parse(skilldata.lv));
                        break;
                }

                break;
            case "1160": //���� ��������
            case "1291": //���� ��������
            case "1292": //���� ��������
            case "1293": //���� ��������
            case "1294": //���� ��������
            case "1295": //���� ��������
            case "1296": //���� ��������
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.bossadddmg,
                    float.Parse(skilldata.value));
                break;
            case "1170": //�������� �г�
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.razerage,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.razeragehitper,
                    float.Parse(skilldata.probability));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.razeragelv,
                    float.Parse(skilldata.rare));
                break;
            case "1171": //
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.balrockrage,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.balrockhitper,
                    float.Parse(skilldata.probability));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.balrockragelv,
                    float.Parse(skilldata.rare));
                break;
            case "1464": //�����̻�����
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.dotperup,
                    float.Parse(skilldata.value));
                break;
            case "1474": //�����̻�����
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.basicatk,
                    float.Parse(skilldata.value));
                break;
            case "1192": //�����̻�����
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.potionup,
                    float.Parse(skilldata.value));
                break;
            case "1202": //ㅁ염화검
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legensworddmg,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenswordhitper,
                    100f);
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenswordrare,
                    float.Parse(skilldata.rare));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenswordlv,
                    float.Parse(skilldata.lv));
                break;
            case "1203": //멸화도
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legendaggerdmg,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legendaggerhitper,
                    100f);
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legendaggerlv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legendaggerrare,
                    float.Parse(skilldata.rare));
                break;
            case "1204": //블리잦드 
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenbowdmg,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenbowhitper,
                    100f);
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenbowwlv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenbowrare,
                    float.Parse(skilldata.rare));
                break;
            case "1205": //대자연의 
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenstaffdmg,
                    float.Parse(skilldata.c));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenstaffhitper,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenstafflv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenstaffrare,
                    float.Parse(skilldata.rare));
                break;
            case "1206": //사신의 낫
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legendotaddstack,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legendotmaxstack,
                    float.Parse(skilldata.c));

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legendotlv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legendotrare,
                    float.Parse(skilldata.rare));
                break;
            case "1207": //제왕의 반지
            case "1328": //제왕의 반지
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.strperup,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.dexperup,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.intperup,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.wisperup,
                    float.Parse(skilldata.value));
                break;
            case "1208": //절대 마력의 반지
            case "1329": //절대 마력의 반지
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.physicperup,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.magicperup,
                    float.Parse(skilldata.value));
                break;
            case "1209": //핏빛 칼날 반지
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.critdmgper,
                    float.Parse(skilldata.value));
                break;
            case "1210": //투신의 목걸이
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.critdmgper,
                    float.Parse(skilldata.value));
                break;
            case "1211": //봉인된 고대 드래곤의 목걸이
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.reskilllv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.reskillhitper,
                    float.Parse(skilldata.probability));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.reskilrare,
                    float.Parse(skilldata.rare));
                break;
            case "1333": //봉인된 고대 드래곤의 목걸이
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.reskilllv2,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.reskillhitper2,
                    float.Parse(skilldata.probability));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.reskilrare2,
                    float.Parse(skilldata.rare));
                break;
            case "1332": //골드 획득량
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.gold,
                    float.Parse(skilldata.value));

                break;
            case "1212": //핏빛 칼날 목걸이
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.dotperup,
                    float.Parse(skilldata.value));
                break;
            case "1330": //독사의 반지
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legendotaddstack,
                    float.Parse(skilldata.value));
                break;
            case "1331": //성기사의 목걸이
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.totaldmg,
                    float.Parse(skilldata.value));
                break;
            case "1494": //마나번
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenstaffhitper,
                    float.Parse(skilldata.c));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.magicperup,
                    float.Parse(skilldata.value));
                break;
            case "1504": //부패
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legendotmaxstack,
                    float.Parse(skilldata.value));
                break;
            case "1514": //광적인 분노
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.maxhpupper,
                    -float.Parse(skilldata.c));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.physicperup,
                    float.Parse(skilldata.value));
                break;
            case "1243": //ㅁ염화검
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legensworddmg,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenswordhitper,
                    100f);
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenswordrare,
                    float.Parse(skilldata.rare));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenswordlv,
                    float.Parse(skilldata.lv));
                break;
            case "1244": //멸화도
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legendaggerdmg,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legendaggerhitper,
                    100f);
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legendaggerlv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legendaggerrare,
                    float.Parse(skilldata.rare));
                break;
            case "1245": //블리잦드 
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenbowdmg,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenbowhitper,
                    100f);
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenbowwlv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenbowrare,
                    float.Parse(skilldata.rare));
                break;
            case "1246": //대자연의 
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenstaffdmg,
                    float.Parse(skilldata.c));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenstaffhitper,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenstafflv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenstaffrare,
                    float.Parse(skilldata.rare));
                break;
            case "1247": //사신의 낫
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legendotaddstack,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legendotmaxstack,
                    float.Parse(skilldata.c));

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legendotlv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legendotrare,
                    float.Parse(skilldata.rare));
                break;
            case "1248": //슬레이어 
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.physicperup,
                    float.Parse(skilldata.value));
                break;
            case "1249": //러브(마법) 사냥용 
                /*
                 * 기본 공격 시 마법 공격력의 600% 만큼 피해를 입힌다.
                 */
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenmacedmg,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenmacehitper,
                    100f);
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenmacelv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenmacerare,
                    float.Parse(skilldata.rare));
                break;
            case "1250": //라그나로크
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenwanddmg,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenwandhitper,
                    100f);
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenwandrare,
                    float.Parse(skilldata.rare));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenwandlv,
                    float.Parse(skilldata.lv));
                break;

            case "1251": //거인의 기백
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.warriorrune,
                    float.Parse(skilldata.value));
                break;
            case "1261": //초월자의정신
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.magerune,
                    float.Parse(skilldata.value));
                break;
            case "1271": //분쇄자
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.destroyervalue,
                    float.Parse(skilldata.value));
                break;
            case "1281": //상처
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.scarvalue,
                    float.Parse(skilldata.value));
                break;

            case "1297": //일반몬스터 피해
            case "1298": //일반몬스터 피해
            case "1299": //일반몬스터 피해
            case "1300": //일반몬스터 피해
            case "1301": //일반몬스터 피해
            case "1302": //일반몬스터 피해

                break;
            case "1303": //경험치 골드 룬
            case "1304":
            case "1305":
            case "1306":
            case "1307":
            case "1308":
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.basicmonsterdmg,
                    float.Parse(skilldata.c));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.goldexp,
                    float.Parse(skilldata.value));
                break;

            case "1309": //파멸의 검
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legensworddmg,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenswordhitper,
                    100f);
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenswordrare,
                    float.Parse(skilldata.rare));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenswordlv,
                    float.Parse(skilldata.lv));
                break;

            case "1310": //하데스의 도끼
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.physicperup,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.E6139,
                    1);
                break;
            case "1311": //거인 분쇄기
                ability_buff = float.Parse(skilldata.value);
                break;
            case "1312": //적화의 건틀릿
                Battlemanager.Instance.mainplayer.buffmanager.SetEquipSkills(data.id, 50);
                isequipskills = true;

                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6141,
                    float.Parse(skilldata.value));

                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6141_2,
                    float.Parse(skilldata.c));

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.E6141_lv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6141_rare,
                    float.Parse(skilldata.rare));
                break;

            case "1313": //골드건
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6142,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6142_2,
                    float.Parse(skilldata.c));

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.E6142_lv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6142_rare,
                    float.Parse(skilldata.rare));
                break;
            case "1314": //드래곤 보우
                Battlemanager.Instance.mainplayer.buffmanager.SetEquipSkills(data.id, 50);
                isequipskills = true;
                //치명타 공격 시 적에게 결정 중첩 +1
                //결정 스택이 +100 이라면 폭발하고 중첩을 0으로 감소
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6143, //얼음
                    float.Parse(skilldata.c));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6143_2, //피해
                    float.Parse(skilldata.c));

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.E6143_lv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6143_rare,
                    float.Parse(skilldata.rare));
                break;
            case "1315": //블러드 커터
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6144,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6144_2,
                    100f);
                //공격횟수
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6144_3,
                    float.Parse(skilldata.c));

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.E6144_lv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6144_rare,
                    float.Parse(skilldata.rare));
                break;
            case "1316": //단테의 석궁
                Battlemanager.Instance.mainplayer.buffmanager.SetEquipSkills(data.id, 5);
                isequipskills = true;
                //무력화 피해 증가
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6145,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6145_2,
                    float.Parse(skilldata.type));
                //민첩 증가
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.dexperup,
                    float.Parse(skilldata.c));
                //치명타 피해 증가
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.critdmgper,
                    float.Parse(skilldata.probability));

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.E6145_lv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6145_rare,
                    float.Parse(skilldata.rare));
                break;
            case "1317": //제우스의 지팡이

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenstaffdmg,
                    float.Parse(skilldata.c));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenstaffhitper,
                    float.Parse(skilldata.value));

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.E6146,
                    float.Parse(skilldata.probability));

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.E6146_lv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6146_rare,
                    float.Parse(skilldata.rare));

                break;
            case "1318": //포세이돈의 지팡이
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenstaffdmg,
                    float.Parse(skilldata.c));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenstaffhitper,
                    float.Parse(skilldata.value));

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.E6147,
                    float.Parse(skilldata.probability));

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.E6147_lv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6147_rare,
                    float.Parse(skilldata.rare));
                break;



            case "1319": //아폴론의 메이스
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.E6148,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6148_2,
                    100f);
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6148_3,
                    2);
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.E6148_lv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.E6148_rare,
                    float.Parse(skilldata.rare));


                break;
            case "1320": //처단자
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legendotaddstack,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legendotmaxstack,
                    float.Parse(skilldata.c));

                //피해증가
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6149,
                    float.Parse(skilldata.probability));
                break;
            case "1321": //고통
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legendotaddstack,
                    float.Parse(skilldata.value));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legendotmaxstack,
                    float.Parse(skilldata.c));

                //피해증가
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6150,
                    float.Parse(skilldata.probability));
                break;



            case "1326": //청화의
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6141) >
                    0)
                {
                    equipskillmanager.Instance.SetStats(
                        (int)equipskillmanager.EquipStatFloat.E6159_2,
                        2);
                }

                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6159,
                    float.Parse(skilldata.value));
                break;
            case "1327": //드래곤 애로우
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E6160,
                    float.Parse(skilldata.value));
                break;





            //피해 증가
            case "1172":
            case "1110":
            case "1120":
            case "1454":
            case "1182":
                if (data2 == null)
                    return;
                switch (data2.SubType)
                {
                    case "Physic":
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.physicperup,
                            float.Parse(skilldata.value));
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.basicatk,
                            float.Parse(skilldata.value));
                        break;
                    case "Magic":
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.magicperup,
                            float.Parse(skilldata.value));
                        // Debug.Log("성불2");
                        break;
                    case "Dot":
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.dotperup,
                            float.Parse(skilldata.c));
//                                        Debug.Log("성불3");
                        break;
                }

                break;
            //피해 증가
            case "1213":
            case "1223":
            case "1233":
            case "1524":
                if (data2 == null)
                    return;
                switch (data2.SubType)
                {
                    case "Physic":
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.maxhpupper,
                            -float.Parse(skilldata.subtype));
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.physicperup,
                            float.Parse(skilldata.type));
                        break;
                    case "Magic":
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.legenstaffhitper,
                            float.Parse(skilldata.value));
                        equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.magicperup,
                            float.Parse(skilldata.probability));
                        break;
                    case "Dot":
                        equipskillmanager.Instance.SetStats(
                            (int)equipskillmanager.EquipStatFloat.legendotmaxstack,
                            float.Parse(skilldata.c));
                        break;
                }

                break;

            case "1534": //<고유>브레이크 타임 //크로노스

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.legenstaffdmg,
                    float.Parse(skilldata.c));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.legenstaffhitper,
                    float.Parse(skilldata.value));

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.E61521,
                    float.Parse(skilldata.probability));

                equipskillmanager.Instance.SetStats((int)equipskillmanager.EquipStatFloat.E61521_lv,
                    float.Parse(skilldata.lv));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E61521_rare,
                    float.Parse(skilldata.rare));
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E61611,
                    1.5f);
                break;
            //피해 증가
            case "1536": //크로노스
                equipskillmanager.Instance.SetStats(
                    (int)equipskillmanager.EquipStatFloat.E61611,
                    float.Parse(skilldata.value));
                break;
        }
    }


    public float AdventureRankSkillSlot(int lv)
    {
        switch (lv)
        {
            case 1:
                return 0;
            case 2:
            case 3:
            case 4:
                return 1;
            case 5:
            case 6:
            case 7:
                return 2;
            case 8:
            case 9:
            case 10:
                return 3;
            case 11:
            case 12:
            case 13:
            case 14:
            case 15:
            case 16:
            case 17:
                return 4;
            case 18:
            case 19:
            case 20:
            case 21:
            case 22:
            case 23:
            case 24:
                return 5;
            case 25:
            case 26:
            case 27:
            case 28:
            case 29:
            case 30:
            case 31:
            case 32:
            case 33:
            case 34:
                return 6;

        }

        return 0;
    }

    public float altarstat_strdexintwis = 0;
    public float achstat = 0;
    public float altarstat_atkmatk = 0;
    // ReSharper disable Unity.PerformanceAnalysis
    public void RefreshStat()
    {
        ClassDB.Row classdata = ClassDB.Instance.Find_id(Nowclass.ClassId1);

        equipskillmanager.Instance.ResetStats();

        int playerlv = PlayerBackendData.Instance.GetLv();

        achstat = 0; //업적
        altarstat_strdexintwis = 0; //제단
        altarstat_atkmatk = 0; //제단 

        
        //탈리스만스탯
        talisman_allstat = 0;
        talisman_atk = 0;
        talisman_matk = 0;
        talisman_crit = 0;
         talisman_critdmg = 0;
        talisman_MaxDotCount = 0; //�ִ� ��Ʈ ��  //�⺻ 10
        talisman_DotDmgUp = 0; //�ִ� ��Ʈ ��  //�⺻ 10
        talisman_AddStack = 0; //��Ʈ ��ų ��� �� �߰��Ǵ� ���
        talisman_Attakcount = 0; //��Ʈ ��ų ��� �� �߰��Ǵ� ���
        talisman_Bossdmg = 0; //���� �߰� ����
        talisman_Mondmg = 0; //���� �߰� ����
        talisman_dmgup = 0; //���� �߰� ����
        talisman_Buff = 0;
        talisman_Exp = 0;
        talisman_Gold = 0;
        talisman_ReduceCooldown = 0;
        
        
        set_atk = 0;
        set_matk = 0;
        set_def = 0;
        set_mdef = 0;
        set_str = 0;
        set_dex = 0;
        set_int = 0;
        set_wis = 0;
        set_hit = 0;
        set_crit = 0;
        set_critdmg = 0;
        set_hp = 0;
        set_mp = 0;
        set_atkspeed = 0;
        set_castspeed = 0;
        set_MaxDotCount = 0; //�ִ� ��Ʈ ��  //�⺻ 10
        set_DotDmgUp = 0; //�ִ� ��Ʈ ��  //�⺻ 10
        set_AddStack = 0; //��Ʈ ��ų ��� �� �߰��Ǵ� ���
        set_reducedmg = 0;
        set_cooldown = 0;

        stat_atk = 0;
        stat_matk = 0;
        stat_def = 0;
        stat_mdef = 0;
        stat_str = 0;
        stat_dex = 0;
        stat_int = 0;
        stat_wis = 0;
        stat_hit = 0;
        stat_crit = 0;
        stat_critdmg = 0;
        stat_hp = 0;
        stat_mp = 0;
        stat_atkspeed = 0;
        stat_castspeed = 0;
        Stat_MaxDotCount = maxdotcount;
        Stat_DotDmgUp = 0;
        Stat_ReduceCoolDown = 0;
        stat_reducedmg = 0;
        stat_basicatkup = 0;
        stat_potionup = 0;

        stat_addmatkdmg = 0;
        stat_addatkdmg = 0;
        stat_usemoremana = 0;
        Stat_BreakDmg = 0;
        Stat_ExtraExp = 0;
        Stat_ExtraGold = 0;
        stat_Monsterdmg = 0;
        Stat_ExtraDrop = 0;
        
        //제련 점수
        Stat_SmeltPoint = 0;
        Stat_SmeltDmg = 0;

        
        //버프효율증가
        Stat_totalbuff = 0;
        
        //수집
        Stat_collection = 0;



        AlldmgUp = 0;
        
        
        //펫
        pet_allstatnum = 0;
        pet_exp = 0;
        pet_gold = 0;
        
        //어빌리티
        ability_atk = 0;
        ability_matk = 0;
        ability_def = 0;
        ability_mdef = 0;
        ability_str = 0;
        ability_dex = 0;
        ability_int = 0;
        ability_wis = 0;
        
        ability_strnum = 0;
        ability_dexnum = 0;
        ability_intnum = 0;
        ability_wisnum = 0;
        
        
        ability_hit = 0;
        ability_crit = 0;
        ability_critdmg = 0;
        ability_hp = 0;
        ability_mp = 0;
        ability_atkspeed = 0;
        ability_castspeed = 0;
        ability_maxdotcount = 0;
        ability_dotdmgup = 0;
        ability_AddStack = 0;
        ability_bossdmg = 0;
        ability_monsterdmg = 0;
        ability_breakdmg = 0;
        ability_reducedmg = 0;
        ability_basicatkup = 0;
        ability_potionup = 0;
        ability_allstat = 0;
        ability_allstatnum= 0;
        ability_exp = 0;
        ability_gold = 0;
        ability_drop = 0;
        ability_cooldown = 0;
        ability_buff = 0;
        ability_AttackCount = 0;

        //5단계 특화
        ability_basicskillper = 0;
        ability_basicskilldmg = 0;

        ability_magicskillper = 0;
        ability_magicskilldmg = 0;

        ability_critskillper = 0;
        ability_critskilldmg = 0;

        abilityskillallatack = false;



        gear_atk = 0;
        gear_matk = 0;
        gear_def = 0;
        gear_mdef = 0;
        gear_str = 0;
        gear_dex = 0;
        gear_int = 0;
        gear_wis = 0;
        gear_hit = 0;
        gear_crit = 0;
        gear_critdmg = 0;
        gear_hp = 0;
        gear_mp = 0;
        gear_atkspeed = 0;
        gear_castspeed = 0;
        gear_maxdotcount = 0;
        gear_dotdmgup = 0;
        gear_reducedmg = 0;
        gear_basicatkup = 0;
        gear_potionup = 0;
        gear_allstat = 0;
        gear_alldmg = 0;
        buff_atk = 0;
        buff_atkPercent = 0;
        buff_matk = 0;
        buff_matkPercent = 0;
        buff_def = 0;
        buff_mdef = 0;
        buff_str = 0;
        buff_dex = 0;
        buff_int = 0;
        buff_wis = 0;
        buff_hit = 0;
        buff_crit = 0;
        buff_critdmg = 0;
        buff_Allstatper = 0;
        buff_hp = 0;
        buff_mp = 0;
        buff_atkspeed = 0;
        buff_castspeed = 0;
        buff_basicatkup = 0;
        buff_alldmgup = 0;


        isequipskills = false;
        

        for (int i = 0; i < AvartaDB.Instance.NumRows(); i++)
        {
//            Debug.Log(i);
            if (PlayerBackendData.Instance.playeravata[i])
            {
                switch (AvartaDB.Instance.GetAt(i).stattype)
                {
                    case "allstat":
                        ability_allstat += float.Parse(AvartaDB.Instance.GetAt(i).stat);
                        break;
                    case "gold":
                        Stat_ExtraGold += float.Parse(AvartaDB.Instance.GetAt(i).stat);
                        break;
                    case "exp":
                        Stat_ExtraExp += float.Parse(AvartaDB.Instance.GetAt(i).stat);
                        break;
                    case "expdmg":
                        Stat_ExtraExp += float.Parse(AvartaDB.Instance.GetAt(i).stat);
                         AlldmgUp += float.Parse(AvartaDB.Instance.GetAt(i).stat2);
                        break;
                    case "golddmg":
                        Stat_ExtraGold += float.Parse(AvartaDB.Instance.GetAt(i).stat);
                        AlldmgUp += float.Parse(AvartaDB.Instance.GetAt(i).stat2);
                        break;
                }   
            }
        }
       // Debug.Log("펫 계산전" + ability_allstatnum);

        foreach (var VARIABLE in PlayerBackendData.Instance.PetData)
        {
            if (VARIABLE.Value.Ishave)
            {
                PetDB.Row petdata = PetDB.Instance.Find_id(VARIABLE.Value.Petid);
                switch (petdata.stathave)
                {
                    case "allstat":
                        pet_allstatnum += float.Parse(petdata.stathavenum.Split(';')[VARIABLE.Value.Petstar]);
//                        Debug.Log("로그" + float.Parse(petdata.stathavenum.Split(';')[VARIABLE.Value.Petstar])) ;
                        break;
                    case "gold":
                        pet_gold += float.Parse(petdata.stathavenum.Split(';')[VARIABLE.Value.Petstar]);
                        break;
                    case "exp":
                        pet_exp += float.Parse(petdata.stathavenum.Split(';')[VARIABLE.Value.Petstar]);
                        
                        break;
                }   
            }
         
        }

 
        
        
//        Debug.Log("펫 계산후" + ability_allstatnum);
        //펫
        if (PlayerBackendData.Instance.nowPetid != "")
        {
            PetDB.Row petdata = PetDB.Instance.Find_id(PlayerBackendData.Instance.nowPetid);

            string[] stattypes = petdata.Stattype.Split(';');
            string[] stat0 = petdata.Stat0.Split(';');
            string[] stat1 = petdata.Stat1.Split(';');
            string[] stat2 = petdata.Stat2.Split(';');
            string[] stat3 = petdata.Stat3.Split(';');
            string[] stat4 = petdata.Stat4.Split(';');
            string[] nowstat = new string[5];
            for (int a = 0; a< stattypes.Length; a++)
            {
                switch (a)
                {
                    case 0:
                        nowstat = stat0;
                        break;
                    case 1:
                        nowstat = stat1;
                        break;
                    case 2:
                        nowstat = stat2;
                        break;
                    case 3:
                        nowstat = stat3;
                        break;
                    case 4:
                        nowstat = stat4;
                        break;
                }
                switch (stattypes[a])
                {
                    case "crit":
                        ability_crit +=
                            float.Parse(nowstat[
                                PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar]);
//                        Debug.Log("크리");
                        break;
                    case "meleemagic":
                        ability_atk +=
                            float.Parse(nowstat[
                                PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar]);
                        ability_matk +=
                            float.Parse(nowstat[
                                PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar]);
                       // Debug.Log("물마");
                        break;
                    case "dotdmg":
                        ability_dotdmgup +=
                            float.Parse(nowstat[
                                PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar]);
                  //      Debug.Log("상태이상");
                        break;
                    case "critdmg":
                        ability_critdmg +=
                            float.Parse(nowstat[
                                PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar]);
               //         Debug.Log("치명피해");

                        break;
                    case "alldmg":
                        AlldmgUp +=   float.Parse(nowstat[
                            PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar]);
//                        Debug.Log("피증" + float.Parse(nowstat[
                  //          PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar]));
                        break;
                    case "melee":
                        ability_atk +=
                            float.Parse(nowstat[
                                PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar]);
                 //       Debug.Log("물리");

                        break;
                    case "magic":
                        ability_matk +=
                            float.Parse(nowstat[
                                PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar]);
                 //       Debug.Log("마법");

                        break;
                    case "exp":
                        ability_exp +=
                            float.Parse(nowstat[
                                PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar]);
                     //   Debug.Log("경험치");

                        break;
                    case "gold":
                        ability_gold +=
                            float.Parse(nowstat[
                                PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar]);
                    //    Debug.Log("골드");

                        break;
                    case "monatk":
                        ability_monsterdmg +=
                            float.Parse(nowstat[
                                PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar]);
                    //    Debug.Log("일반 몬스터");

                        break;
                    case "potionup":
                        ability_potionup +=
                            float.Parse(nowstat[
                                PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar]);
                  //      Debug.Log("포션업");

                        break;
                    case "bossdmg":
                        ability_bossdmg +=
                            float.Parse(nowstat[
                                PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar]);
                     //   Debug.Log("보스");

                        break;
                }
            }
        }
      
        
        
        
        // Debug.Log("1");
        //��� ����
        for (int i = 0; i < PlayerBackendData.Instance.GetEquipData().Length; i++)
        {
            if (PlayerBackendData.Instance.GetEquipData()[i] != null)
            {
                EquipItemDB.Row data =
                    EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[i].Itemid);
                EquipItemDB.Row data2 =
                    EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid);
                gear_atk += PlayerBackendData.Instance.GetEquipData()[i].GetStat(4);
                gear_matk += PlayerBackendData.Instance.GetEquipData()[i].GetStat(5);
                gear_def += PlayerBackendData.Instance.GetEquipData()[i].GetStat(6);
                gear_mdef += PlayerBackendData.Instance.GetEquipData()[i].GetStat(7);

                gear_str += PlayerBackendData.Instance.GetEquipData()[i].GetStat(0);
                gear_dex += PlayerBackendData.Instance.GetEquipData()[i].GetStat(1);
                gear_int += PlayerBackendData.Instance.GetEquipData()[i].GetStat(2);
                gear_wis += PlayerBackendData.Instance.GetEquipData()[i].GetStat(3);

                gear_hit += float.Parse(data.Hit);
//                Debug.Log(PlayerBackendData.Instance.GetEquipData()[i].Itemid+" " + data.Hp);
                gear_hp += float.Parse(data.Hp);
                gear_mp += float.Parse(data.Mp);

                gear_atkspeed += float.Parse(data.atkspeed);
                gear_castspeed += float.Parse(data.castspeed);

                gear_maxdotcount += int.Parse(data.maxdotcount);
                gear_dotdmgup += float.Parse(data.dotdmgup);
                gear_reducedmg += float.Parse(data.def);

                gear_crit += float.Parse(data.Crit);
                gear_critdmg += float.Parse(data.CritDmg);


                gear_allstat += PlayerBackendData.Instance.GetEquipData()[i].GetStat(12);
                gear_alldmg += PlayerBackendData.Instance.GetEquipData()[i].Alldmg1 * 0.01f;
                if (!PlayerBackendData.Instance.GetEquipData()[i].IshaveEquipSkill) continue;
                foreach (var skilldata in PlayerBackendData.Instance.GetEquipData()[i].EquipSkill1
                             .Select(t => EquipSkillDB.Instance.Find_id(t)))
                {
                    try
                    {
                        CheckESKILL(skilldata,data2,data);
                    }
                    catch (Exception e)
                    {
//                        Debug.Log("버그");
                        continue;
                    }
                }
            }
        }

        if (!isequipskills)
        {
            //스킬 다운
            Battlemanager.Instance.mainplayer.buffmanager.OffEquipSkills();
        }

        foreach (var v in PlayerBackendData.Instance.GiveEquipTalismanData())
        {
            if (v != null)
            {
                if (v.Eskill != null)
                {
                    for (int i = 0; i < v.Eskill.Count; i++)
                    {
                        EquipSkillDB.Row skilldata = EquipSkillDB.Instance.Find_id(v.Eskill[i]);
                        CheckESKILL(skilldata);
//                        Debug.Log("스킬체크했다" + v.Eskill[i]);
                    }
                }
            }
        }
        

        //��Ʈ���
        foreach (var SetData in EquipSetmanager.Instance.SetIDs.Select(t => SetDBDB.Instance.Find_id(t)))
        {
            set_atk += float.Parse(SetData.atk);
            set_matk += float.Parse(SetData.matk);
            set_def += 0;
            set_mdef += 0;
            set_str += float.Parse(SetData.str);
            set_dex += float.Parse(SetData.dex);
            set_int += float.Parse(SetData.Int);
            set_wis += float.Parse(SetData.wis);
            set_hit += 0;
            set_crit += float.Parse(SetData.crit);
            set_critdmg += float.Parse(SetData.critdmg);
            set_hp += float.Parse(SetData.hp);
            set_mp += float.Parse(SetData.mp);
            set_atkspeed += float.Parse(SetData.atkspeed);
            set_castspeed += float.Parse(SetData.castspd);
            set_MaxDotCount += int.Parse(SetData.maxdotcount); //�ִ� ��Ʈ ��  //�⺻ 10
            set_DotDmgUp += float.Parse(SetData.dotdmgup); //�ִ� ��Ʈ ��  //�⺻ 10
            set_AddStack += int.Parse(SetData.dotstackup); //��Ʈ ��ų ��� �� �߰��Ǵ� ���
            set_reducedmg += float.Parse(SetData.reduceddmg);
            set_cooldown += float.Parse(SetData.cooldown);
        }

        //특성
        for (int i = 0; i < PlayerBackendData.Instance.Abilitys.Length; i++)
        {
            if (PlayerBackendData.Instance.Abilitys[i] != "")
            {
                switch (PlayerBackendData.Instance.Abilitys[i])
                {
                    case "1000": //주능력치

                        switch (classdata.mainstat)
                        {
                            case "str":
                                ability_str += float.Parse(AbilityDBDB.Instance
                                    .Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                                break;
                            case "dex":
                                ability_dex += float.Parse(AbilityDBDB.Instance
                                    .Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                                break;
                            case "int":
                                ability_int = float.Parse(AbilityDBDB.Instance
                                    .Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                                break;
                            case "wis":
                                ability_wis += float.Parse(AbilityDBDB.Instance
                                    .Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                                break;
                        }

                        break;
                    case "1001": //치명타 확률
                        ability_crit +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1002": //치명타 피해
                        ability_critdmg +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1003": //생명력
                        ability_hp +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1004": //정신력
                        ability_mp +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1005": //물공
                    case "1023": //물공
                        ability_atk +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1006": //마공
                    case "1025": //물공
                        ability_matk +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1007":
                    case "1026":
                        ability_dotdmgup +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1008":
                        ability_exp +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1009":
                    case "1022":
                        ability_buff +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1010":
                        ability_bossdmg +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1011":
                        ability_monsterdmg +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1012":
                        ability_breakdmg +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1013":
                        ability_gold +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1014":
                        ability_atkspeed +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1015":
                        ability_cooldown +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1016":
                        ability_drop +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        break;
                    case "1017": //기본 추가
                        ability_basicskillper +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        ability_basicskilldmg +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).B);
                        break;
                    case "1018": //마법 추가
                        ability_magicskillper +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        ability_magicskilldmg +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).B);
                        break;
                    case "1019": //크리 추가
                        ability_critskillper +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        ability_critskilldmg +=
                            float.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).B);
                        break;
                    case "1020":
                        ability_AddStack +=
                            int.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).A);
                        ability_maxdotcount +=
                            int.Parse(AbilityDBDB.Instance.Find_id(PlayerBackendData.Instance.Abilitys[i]).B);
                        break;
                    case "1021":
                        abilityskillallatack = true;
                        break;
                    case "1024":
                        ability_AttackCount += 1;
                        break;
                    case "1027":
                        ability_strnum += 6 * PlayerBackendData.Instance.GetLv();
                        break;
                    case "1028":
                        ability_dexnum += 6 * PlayerBackendData.Instance.GetLv();
                        break;
                    case "1029":
                        ability_intnum += 6 * PlayerBackendData.Instance.GetLv();
                        break;
                    case "1030":
                        ability_wisnum += 6 * PlayerBackendData.Instance.GetLv();
                        break;
                    
                    case "1031":
                        ability_allstatnum += 3 * PlayerBackendData.Instance.GetLv();
                        break;
                }
            }
        }



        for (var index = 0; index < Skillmanager.Instance.PlayerBuffImage.Length; index++)
        {
            var t = Skillmanager.Instance.PlayerBuffImage[index];
            t.SetActive(false);
        }

        Stat_totalbuff = (Passivemanager.Instance.GetPassiveStat(Passivemanager
            .PassiveStatEnum.buffup) + ability_buff) + talisman_Buff;
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype.Equals("12"))
        {
            alertmanager.Instance.ShowAlert2(string.Format(Inventory.GetTranslate("UI7/버퍼효과발동"),PartyRaidBattlemanager.Instance.buffmax * 100f), alertmanager.alertenum.일반);
            Stat_totalbuff = PartyRaidBattlemanager.Instance.buffmax;
           // Debug.Log("파티레이드임");
        }
        string[] skills = PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1;
        for (int s = 0; s < skills.Length; s++)
        {
            try
            {
                if (skills[s] != null)
                {
                    SkillDB.Row skilldata = SkillDB.Instance.Find_Id(skills[s]);
//                    Debug.Log(skills[s]);
                    switch (skilldata.BuffType)
                    {
                        //파티버프
                        //기본 공격력 증가
                        case "Crit_P":
                            // Debug.Log("스킬을 사용");
                            buff_crit = float.Parse(skilldata.Atk);
                            buff_crit += buff_crit * Stat_totalbuff;
                            Skillmanager.Instance.PlayerBuffImage[2].SetActive(false);
                            Skillmanager.Instance.PlayerBuffImage[5].SetActive(true);
                            break;
                        //기본 공격력 증가
                        case "AtkPercent_P":
                            buff_atkPercent = float.Parse(skilldata.Atk);
                            buff_matkPercent = float.Parse(skilldata.Atk);
                            if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.buffup) != 0)
                            {
                                buff_atkPercent += buff_atkPercent * Stat_totalbuff;
                                buff_matkPercent += buff_matkPercent * Stat_totalbuff;
                            }

                            // Debug.Log("버프증가량");
                            Skillmanager.Instance.PlayerBuffImage[6].SetActive(true);
                            Skillmanager.Instance.PlayerBuffImage[0].SetActive(false);
                            Skillmanager.Instance.PlayerBuffImage[1].SetActive(false);
                            break;
                        //기본 공격력 증가
                        case "Critdmg_P":
                            buff_Allstatper = float.Parse(skilldata.CritDmg);
                            buff_Allstatper += buff_Allstatper * Stat_totalbuff;
                            //   Invoke(nameof(OffCritDmgBuff), float.Parse(skilldata.skilldata.AttackCount));
                            Skillmanager.Instance.PlayerBuffImage[7].SetActive(true);
                            Skillmanager.Instance.PlayerBuffImage[3].SetActive(false);
                            break;

                        //기본 공격력 증가
                        case "BasicAtk":
                            buff_basicatkup = float.Parse(skilldata.Atk);
                            if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.buffup) != 0)
                            {
                                buff_basicatkup += buff_basicatkup * Stat_totalbuff;
                            }

                            // Debug.Log("버프증가량");
                            Skillmanager.Instance.PlayerBuffImage[4].SetActive(true);
                            break;

                        case "AtkPercent": //물리공격력증가
                            buff_atkPercent = float.Parse(skilldata.Atk);
                            if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.buffup) != 0)
                            {
                                buff_atkPercent += buff_atkPercent * Stat_totalbuff;
                            }

                            Skillmanager.Instance.PlayerBuffImage[0].SetActive(true);
                            Skillmanager.Instance.PlayerBuffImage[6].SetActive(false);
                            break;
                        case "MAtkPercent": //마법공격력증가
                            buff_matkPercent = float.Parse(skilldata.Matk);
                            buff_matkPercent += buff_matkPercent * Stat_totalbuff;
                            Skillmanager.Instance.PlayerBuffImage[1].SetActive(true);
                            Skillmanager.Instance.PlayerBuffImage[6].SetActive(false);
                            break;
                        case "Crit": //크리티컬
                            // Debug.Log("스킬을 사용");
                            buff_crit = float.Parse(skilldata.Atk);
                            buff_crit += buff_crit * Stat_totalbuff;
                            Skillmanager.Instance.PlayerBuffImage[2].SetActive(true);
                            Skillmanager.Instance.PlayerBuffImage[5].SetActive(false);
                            break;
                        case "Critdmg": //크리티컬
                            buff_Allstatper = float.Parse(skilldata.CritDmg);
                            buff_Allstatper += buff_Allstatper * Stat_totalbuff;
                            //   Invoke(nameof(OffCritDmgBuff), float.Parse(skilldata.skilldata.AttackCount));
                            Skillmanager.Instance.PlayerBuffImage[3].SetActive(true);
                            Skillmanager.Instance.PlayerBuffImage[7].SetActive(false);
                            break;
                        case "Alldmgup": //크리티컬
                            buff_alldmgup = float.Parse(skilldata.Atk);
                            buff_alldmgup += buff_alldmgup * Stat_totalbuff;
                            //   Invoke(nameof(OffCritDmgBuff), float.Parse(skilldata.skilldata.AttackCount));
                            Skillmanager.Instance.PlayerBuffImage[8].SetActive(true);
                            break;

                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                continue;
            }
        }


        #region  제련점수

        //제련 점수
        for (int i = 0; i < PlayerBackendData.Instance.GetEquipData().Length; i++)
        {
            if (PlayerBackendData.Instance.GetEquipData()[i] != null)
            {
                Stat_SmeltPoint += PlayerBackendData.Instance.GetEquipData()[i].SmeltSuccCount1;
            }
        }

        Stat_SmeltDmg = (float)(Math.Truncate((double)Stat_SmeltPoint / 4) * 0.03f);
//        Debug.Log("점수 는" + Stat_SmeltDmg);
        #endregion


        Passivemanager pm = Passivemanager.Instance;


        achstat = float.Parse(AchievementStatDB.Instance
            .Find_level(PlayerBackendData.Instance.GetAchLv().ToString()).str);
        altarstat_strdexintwis =
            (PlayerBackendData.Instance.GetGoldAltarlv(altarmanager.AltarType.제단) *
             float.Parse(altarrenewalDB.Instance.Find_id("0").stat)) +
            (PlayerBackendData.Instance.GetGoldAltarlv(altarmanager.AltarType.골드) *
             float.Parse(altarrenewalDB.Instance.Find_id("1").stat)) +
            (PlayerBackendData.Instance.GetGoldAltarlv(altarmanager.AltarType.레이드) *
             float.Parse(altarrenewalDB.Instance.Find_id("2").stat)) +
            (PlayerBackendData.Instance.GetGoldAltarlv(altarmanager.AltarType.개미굴) *
             float.Parse(altarrenewalDB.Instance.Find_id("3").stat));
   
        
//          Debug.Log("총 능력치" + altarstat_strdexintwis);
        float premiumper = PlayerBackendData.Instance.ispremium ? 0.15f : 0f;
        float advenper = AdventureRank();
        //Collectmanager.Instance.RefreshStat();
        CollectionRenewalManager.Instance.RefreshStat();
        float str_allstat = 0;
        float dex_allstat = 0;
        float int_allstat = 0;
        float wis_allstat = 0;
        float str_allstatper = 0;
        float dex_allstatper = 0;
        float int_allstatper = 0;
        float wis_allstatper = 0;

        
                 
        //탈리스만 2세트
        foreach (var t in PlayerBackendData.Instance.Talisman2Set)
        {
            switch (t)
            {
                case "T1000":
                    talisman_allstat += 0.2f;
                    break;
                case "T1001":
                    talisman_crit += 0.2f;
                    break;
                case "T1002":
                    talisman_atk += 5f;
                    break;
                case "T1003":
                    talisman_Bossdmg += 1f;
                    break;
                case "T1004":
                    talisman_DotDmgUp += 10f;
                    break;
                case "T1005":
                    talisman_matk += 5f;
                    break;
                case "T1006":
                    talisman_Buff += 1f;
                    break;
                case "T1007":
                    talisman_Exp += 1f;
                    talisman_Gold += 0.5f;
                    break;
                case "T1008":
                    talisman_Mondmg += 1f;
                    talisman_Exp += 0.5f;
                    break;
            }
        }

        //탈리스만 3세트
        foreach (var t in PlayerBackendData.Instance.Talisman3Set)
        {
            switch (t)
            {
                case "T1000":
                    talisman_allstat += 0.2f;
                    break;
                case "T1001":
                    talisman_dmgup += 0.12f;
                    break;
                case "T1002":
                    talisman_atk += 10f;
                    break;
                case "T1003":
                    talisman_Bossdmg += 1.5f;
                    break;
                case "T1004":
                    talisman_DotDmgUp += 10f;
                    talisman_MaxDotCount += 30;
                    talisman_AddStack += 30;
                    break;
                case "T1005":
                    talisman_ReduceCooldown += 0.03f;
                    talisman_matk += 10f;
                    break;
                case "T1006":
                    talisman_Buff += 1.5f;
                    break;
                case "T1007":
                    talisman_Exp += 1.5f;
                    talisman_Gold += 0.5f;
                    break;
                case "T1008":
                    talisman_Mondmg += 1.5f;
                    talisman_Exp += 0.5f;

                    break;
            }
        }

         //탈리스만 5세트
                foreach (var t in PlayerBackendData.Instance.Talisman5Set)
                {
                    switch (t)
                    {
                        case "T1000":
                            talisman_allstat += 0.6f;
                            break;
                        case "T1001":
                        case "T1002":
                        case "T1004":
                        case "T1005":
                            talisman_dmgup += 0.2f;
                            break;
                        
                        case "T1003":
                            talisman_Bossdmg += 2f;
                            break;
                       
                        case "T1006":
                            talisman_Buff += 2.5f;
                            break;
                        case "T1007":
                            talisman_Exp += 2.0f;
                            talisman_Gold += 0.5f;
                            break;
                        case "T1008":
                            talisman_Mondmg += 2f;
                            talisman_Exp += 0.5f;

                            break;
                    }
                }

        
        
        
        switch (classdata.mainstat)
        {
            case "str":
                str_allstat += gear_allstat+ Stat_collection + equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat);
                str_allstatper += equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup) + buff_Allstatper + talisman_allstat;
                break;
            case "dex":
                dex_allstat += gear_allstat+ Stat_collection + equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat);
                dex_allstatper += equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup)+ buff_Allstatper+ talisman_allstat;
                break;
            case "int":
                int_allstat = gear_allstat + Stat_collection + equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat);
                int_allstatper += equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup)+ buff_Allstatper+ talisman_allstat;
                break;
            case "wis":
                wis_allstat += gear_allstat + Stat_collection + equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat);
                wis_allstatper  += equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup)+ buff_Allstatper+ talisman_allstat;
                break;
        }
        
        stat_str = (float.Parse(classdata.strperlv) * playerlv) + gear_str + buff_str +
                   equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.strup) + set_str + ability_strnum +
                   achstat + altarstat_strdexintwis  + str_allstat + ability_allstatnum + pet_allstatnum;
        stat_dex = (float.Parse(classdata.dexperlv) * playerlv) + gear_dex + buff_dex +
                   equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.dexup) + set_dex + ability_dexnum +
                   achstat + altarstat_strdexintwis  + dex_allstat + ability_allstatnum+ pet_allstatnum;

        stat_int = (float.Parse(classdata.intperlv) * playerlv) + gear_int + buff_int +
                   equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.intup) + set_int + ability_intnum +
                   achstat + altarstat_strdexintwis  + int_allstat + ability_allstatnum+ pet_allstatnum;
        stat_wis = (float.Parse(classdata.wisperlv) * playerlv) + gear_wis + buff_int +
                   equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.wisup) + set_wis + ability_wisnum +
                   achstat + altarstat_strdexintwis  + wis_allstat + ability_allstatnum+ pet_allstatnum;
        
        
        float allstatper = pm.GetPassiveStat(Passivemanager.PassiveStatEnum.allstatperup) ;


        stat_str += stat_str * (advenper + str_allstatper +
                                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.strperup) +
                                pm.GetPassiveStat(Passivemanager.PassiveStatEnum.strperup) + allstatper + ability_str
                                + MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.모든능력치증가) + ability_allstat);
        stat_dex += stat_dex * (advenper + dex_allstatper +
                                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.dexperup) +
                                pm.GetPassiveStat(Passivemanager.PassiveStatEnum.dexperup) + allstatper + ability_dex
                                + MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.모든능력치증가) + ability_allstat
                               );
        stat_int += stat_int * (advenper + int_allstatper +
                                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.intperup) +
                                pm.GetPassiveStat(Passivemanager.PassiveStatEnum.intwisperup) + allstatper + ability_int
                                + MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.모든능력치증가) + ability_allstat);
        stat_wis += stat_wis * (advenper + wis_allstatper +
                                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.wisperup) +
                                pm.GetPassiveStat(Passivemanager.PassiveStatEnum.intwisperup) + allstatper + ability_wis
                                + MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.모든능력치증가) + ability_allstat);


     

        stat_str += stat_str * (premiumper);

        stat_dex += stat_dex * (premiumper);
        stat_int += stat_int * (premiumper);
        stat_wis += stat_wis * (premiumper);

      
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6139) >= 1)
        {
            int mix = (int)((stat_int + stat_wis));
            stat_str += mix;
            stat_int = 0;
            stat_wis = 0;
        }
        
        
        //추가 경험치
        Stat_ExtraExp += MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.경험치증가) + ability_exp + pet_exp + talisman_Exp;
        Stat_ExtraGold += MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.골드획득량증가) + ability_gold + pet_gold + talisman_Gold;
        Stat_ExtraDrop += MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.드롭율증가) + ability_drop;

        stat_hp = (float.Parse(classdata.hpperlv) * playerlv) + (stat_str * 60) + (stat_dex * 20) +
                  (stat_int * 15) + (stat_wis * 15) + float.Parse(classdata.Hp) + set_hp;

        stat_mp = (float.Parse(classdata.mpperlv) * playerlv) + (stat_int * 40) + (stat_wis * 40) +
                  (stat_str * 15) + (stat_dex * 15) + float.Parse(classdata.Mp) + set_mp;
        stat_hp += stat_hp * (pm.GetPassiveStat(Passivemanager.PassiveStatEnum.hpperup) + gear_hp + ability_hp
            + MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.생명력증가)
            + equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.maxhpupper));

        //슬레이어 검사
        stat_hp += stat_hp * equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legendaxehp)
                   + ability_hp;


        stat_mp += stat_mp * (MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.정신력증가) + gear_mp
            + ability_mp
            + equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.maxmpupper)) ;

        stat_hit = gear_hit + buff_hit;
        stat_crit = 5f + gear_crit + buff_crit + ability_crit +
                    equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.critper) +
                    pm.GetPassiveStat(Passivemanager.PassiveStatEnum.critup) + set_crit
                    + MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.치명타확률증가)  + talisman_crit;

        if (mapmanager.Instance.BossPenalty[1].activeSelf)
        {
            Debug.Log("치명타 감소됨");
            stat_crit -= 30f;
        }


        stat_critdmg = 1.5f + gear_critdmg + buff_critdmg + ability_critdmg +
                       equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.critdmgper) +
                       pm.GetPassiveStat(Passivemanager.PassiveStatEnum.critdmgup) +
                       pm.GetPassiveStat(Passivemanager.PassiveStatEnum.critmeleedmgup) + set_critdmg
                       + MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.치명타피해증가) + talisman_critdmg;

        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6142) > 0)
        {
////            Debug.Log("레일건적용");
            stat_critdmg +=
                stat_crit * equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6142);
        }
        
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.warriorrune) != 0)
        {
            float critdmgslayer = (stat_str+stat_dex) / equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                .warriorrune);
//            Debug.Log("기백 증가 치피는" + critdmgslayer) ;
            stat_critdmg += critdmgslayer*0.01f;
        }
        
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.magerune) != 0)
        {
            float critdmgslayer = (stat_int+stat_wis) / equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                .magerune);
//            Debug.Log("정신 증가 치피는" + critdmgslayer) ;
            stat_critdmg += critdmgslayer*0.01f;
        }

        float addAtk = 0;
        if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.hpattack) != 0)
        {
            addAtk = stat_hp * Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.hpattack);
        }

        stat_atk = (stat_str * 16) + (stat_dex * 24) + buff_atk;
        //���� ���ݷ��� �����
        stat_atk = (stat_atk * gear_atk);


        stat_atk = (stat_atk *
                    (1 + buff_atkPercent + equipskillmanager.Instance.GetStats(
                         (int)equipskillmanager.EquipStatFloat.physicperup)
                     + pm.GetPassiveStat(Passivemanager.PassiveStatEnum.atkdmgup) +
                     pm.GetPassiveStatPercent(Passivemanager.PassiveStatEnum
                         .critmeleedmgup) + set_atk + altarstat_atkmatk + ability_atk+ talisman_atk) );
        stat_atk += addAtk;
        stat_matk = (stat_int * 28) + (stat_wis * 10) + buff_matk;

        stat_matk = (stat_matk * gear_matk);

        stat_matk = (stat_matk * (1 + buff_matkPercent +
                                  equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                                      .magicperup) +
                                  pm.GetPassiveStat(Passivemanager.PassiveStatEnum.matkdmgup) + set_matk +
                                  altarstat_atkmatk + ability_matk + talisman_matk));

        stat_def = gear_def + buff_def;
        stat_mdef = gear_mdef + buff_mdef;
        stat_reducedmg = gear_reducedmg + set_reducedmg;

        stat_atkspeed = gear_atkspeed + 0f + buff_atkspeed +
                        pm.GetPassiveStat(Passivemanager.PassiveStatEnum.atkspdup) + set_atkspeed + ability_atkspeed;



        stat_castspeed = gear_castspeed + 0f + buff_castspeed + set_castspeed + Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.castspeed);
////        Debug.Log("시전 속도 " + Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.castspeed));
    
        //���� ����
        stat_Bossdmg = gear_bossdmg + 0f + buff_bossdmg + set_Bossdmg + ability_bossdmg +
                       equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.bossadddmg)
                       + MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.보스피해증가) + talisman_Bossdmg;
        stat_Monsterdmg = ability_monsterdmg + talisman_Mondmg;

        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.basicmonsterdmg) != 0)
        {
            stat_Monsterdmg += equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                .basicmonsterdmg);
//            Debug.Log("일반몬스터피해" + stat_Monsterdmg) ;
        }
        
        Stat_BreakDmg = MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.무력화피해증가) +
                        Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.breakdmgup) +
                        equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6145) +
                        ability_breakdmg;


        //��Ʈ
        Stat_MaxDotCount = gear_maxdotcount + buff_maxdotcount + maxdotcount +
                           (int)pm.GetPassiveStat(Passivemanager.PassiveStatEnum.dotstackup) + set_MaxDotCount
                           + (int)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                               .legendotmaxstack)
                           + ability_maxdotcount  + talisman_MaxDotCount;

        //        Debug.Log("최대중첩"+Stat_MaxDotCount);


        float dotstatup = stat_wis * 0.00001f;

        Stat_DotDmgUp = gear_dotdmgup + buff_dotdmgup + pm.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdmgup) +
                        set_DotDmgUp + dotstatup + ability_dotdmgup +
                        equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.dotperup) + talisman_DotDmgUp;
        Stat_AddStack = gear_AddStack + buff_AddStack +
                        (int)pm.GetPassiveStat(Passivemanager.PassiveStatEnum.dotstackup) + set_AddStack
                        + (int)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .legendotaddstack)
                        + ability_AddStack + talisman_AddStack;
        Stat_ReduceCoolDown = pm.GetPassiveStat(Passivemanager.PassiveStatEnum.reduceskillcooldown) + set_cooldown +
                              MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.재사용시간감소) +
                              ability_cooldown + talisman_ReduceCooldown;

        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.scarvalue) != 0)
        {
            float adddots = (Stat_MaxDotCount / equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                .scarvalue) *  0.05f);
//            Debug.Log("증가 상태이상피해" + adddots) ;
            Stat_DotDmgUp += adddots;
        }
        
        
        
        //기본 공격 
        stat_basicatkup = gear_basicatkup + equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
            .basicatk) + buff_basicatkup +  pm.GetPassiveStat(Passivemanager.PassiveStatEnum.basicatkdmg);
////        Debug.Log(pm.GetPassiveStat(Passivemanager.PassiveStatEnum.basicatkdmg) + "기본공격피해");
//        Debug.Log(Stat_ExtraExp);
//        Debug.Log(Stat_ExtraGold);
       
            Stat_ExtraExp += equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.goldexp)+
                             equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.gold);
            Stat_ExtraGold += equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.goldexp)+
                              equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.exp);
        
        //물약효율
        stat_potionup = gear_potionup + equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
            .potionup) + buff_potionup + ability_potionup;
        //도트를 체크한다
        DotCheck();
        InitAttackData(); //���� Ƚ�� ����;
        
        //분쇄자
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.scarvalue) != 0)
        {
            float basicdmg = AttackCount * equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                .destroyervalue);
//            Debug.Log("증가 기본공격피해" + basicdmg) ;
            stat_basicatkup += basicdmg;
        }

        AlldmgUp += buff_alldmgup + gear_alldmg + equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.totaldmg) + talisman_dmgup;

    }

    public void DotCheck()
    {
       
        if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0 && dottime != 0.25f)
        {
            dottime = 0.25f;
        }
        
        if(Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) ==0f)
        {
            dottime = 0.5f;
        }
        
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                .E6150) > 0)
        {
            dottime -= equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                .E6150);
            Debug.Log("도트시간감소" + equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                .E6150));
        }
    }
    public float dottime = 0.5f;
    
    public void EquipStat()
    {

    }

    public ClassDatabase Nowclass;

    public void SetClass_start()
    {
        Nowclass = PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId];
        classskillslotcount = int.Parse(ClassDB.Instance.Find_id(Nowclass.ClassId1).skillslotcount);
        classmulticasting = int.Parse(ClassDB.Instance.Find_id(Nowclass.ClassId1).skillslotcount);
        PlayerData.Instance.SetAvartaImage(
            SpriteManager.Instance.GetSprite(ClassDB.Instance.Find_id(Nowclass.ClassId1).classsprite),
            ClassDB.Instance.Find_id(Nowclass.ClassId1).classsprite);

        if (PlayerBackendData.Instance.GetEquipData()[0] != null)
            PlayerData.Instance.SetWeaponImage(
                SpriteManager.Instance.GetSprite(EquipItemDB.Instance
                    .Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid).EquipSprite),
                EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid).EquipSprite);
        else
            PlayerData.Instance.SetWeaponImage_remove();

        if (PlayerBackendData.Instance.GetEquipData()[1] != null)
            PlayerData.Instance.SetSubWeaponImage(
                SpriteManager.Instance.GetSprite(EquipItemDB.Instance
                    .Find_id(PlayerBackendData.Instance.GetEquipData()[1].Itemid).EquipSprite),
                EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[1].Itemid).EquipSprite);
        else
            PlayerData.Instance.SetSubWeaponImage_remove();

        if (PlayerBackendData.Instance.nowPetid != "")
        {
            PlayerData.Instance.SetpetImage(SpriteManager.Instance.GetSprite(PetDB.Instance.Find_id(PlayerBackendData.Instance.nowPetid).sprite),PetDB.Instance.Find_id(PlayerBackendData.Instance.nowPetid).sprite);
            PlayerData.Instance.SetPetRare(PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar);
        }
        else
        {
            PlayerData.Instance.SetPetImage_Remove();
        }
        
        RefreshStat();
        ClassStat();
    }






    public List<Arrow> Arrow = new List<Arrow>();
    public List<Arrow> AniArrow = new List<Arrow>();

    public void InitArrow()
    {
        EquipItemDB.Row data = null;
        try
        {
            data = EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid);
        }
        catch
        {
            data = null;
        }

        //�ַο찡 ����
        if (data == null)
            return;
        //��Ʈ����
        if (data.HitSound != "")
        {
            attacktrigger = data.HitSound.Split(';');
        }

        if (Arrow.Count == 0)
        {
            for (int i = 0; i < poolcount; i++)
            {
                //����
                Arrow arrow = Instantiate(ObjectPoolManager.Instance.arrowobj,
                    ObjectPoolManager.Instance.ArrowPoolTrans);
                arrow.SetSprite(SpriteManager.Instance.GetSprite(data.ArrowSprite), int.Parse(data.ArrowSpeed),
                    float.Parse(data.ArrowSize), -float.Parse(data.ArrowRotation));
                arrow.gameObject.SetActive(false);
                Arrow.Add(arrow);
            }
        }
        else
        {
            foreach (var t in Arrow)
            {
                //����
                t.SetSprite(SpriteManager.Instance.GetSprite(data.ArrowSprite), int.Parse(data.ArrowSpeed),
                    float.Parse(data.ArrowSize), -135f);
            }
        }

        if (AniArrow.Count == 0)
        {
            for (int i = 0; i < poolcount; i++)
            {
                //����
                Arrow arrow = Instantiate(ObjectPoolManager.Instance.aniarrowobj,
                    ObjectPoolManager.Instance.ArrowPoolTrans);
                //arrow.SetSprite(SpriteManager.Instance.GetSprite(data.ArrowSprite), int.Parse(data.ArrowSpeed), float.Parse(data.ArrowSize));
                arrow.gameObject.SetActive(false);
                AniArrow.Add(arrow);
            }
        }
        else
        {
            for (int i = 0; i < Arrow.Count; i++)
            {
                //����
                AniArrow[i].SetSprite(SpriteManager.Instance.GetSprite(data.ArrowSprite), int.Parse(data.ArrowSpeed),
                    float.Parse(data.ArrowSize), -135);
            }
        }
    }

    public int weaponatkcount;
    public void InitAttackData()
    {
        EquipItemDB.Row data = EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[0].Itemid);
        weaponatkcount = int.Parse(data.HitCount);
        AttackCount =
            weaponatkcount + (int)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum
                               .atkcountup)
                           + (int)Passivemanager.Instance.GetPassiveStatPercent(Passivemanager.PassiveStatEnum
                               .attackcountupandrange) + ability_AttackCount + 
                           (int)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6144_3) //블러드 커터
                           +(int)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6148_3) //아폴론의 메이스
                           +(int)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6159_2) + talisman_Attakcount; //청화적화온
        // Debug.Log("ī��Ʈ"+  (int)Passivemanager.Instance.GetPassiveStat( Passivemanager.PassiveStatEnum.atkcountup));
        RefreshAttackCount();
    }

    [Button(Name = "RefreshAttackCount")]
    public void RefreshAttackCount()
    {
        Battlemanager.Instance.waitattackterm = new WaitForSeconds(0.2f / AttackCount);
    }

    private void Awake()
    {
        ani = GetComponent<Animator>();
        castingmanager = GetComponent<Castingmanager>();
        buffmanager = GetComponent<BuffManager>();
        StartCoroutine(battlestart());
        InitArrow();
        SetClass_start();

    }

    private void Start()
    {
        PlayerBackendData.Instance.StartCheck();
    }


    public void StartAni(string trigger)
    {
        this.enabled = true;
        ani.SetTrigger(trigger);
    }


    [SerializeField] readonly WaitForSeconds wait = new WaitForSeconds(0.1f);

    private IEnumerator battlestart()
    {
        while (true)
        {
            yield return new WaitUntil(() => Battlemanager.Instance.isbattle);

            if (Battlemanager.Instance.isbattle && EnemySpawnManager.Instance.GetDistance() < 0.1f)
            {

                yield return wait;
            }
        }
    }

    public void Update()
    {
        if (Battlemanager.Instance.isbattle)
        {
            if (attacktype == AttackType.Melee)
            {
                //�ٰŸ�
                transform.position = Vector3.Lerp(transform.position, MeleeSit.position, 10 * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, RangeSit.position, 10 * Time.deltaTime);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, RangeSit.position, 10 * Time.deltaTime);
        }
    }
}

public enum AttackType
{
    Melee,
    Range
}
