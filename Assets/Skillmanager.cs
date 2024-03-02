using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skillmanager : MonoBehaviour
{
    //�̱��游���.
    private static Skillmanager _instance = null;
    public static Skillmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Skillmanager)) as Skillmanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    public Castingmanager castingmanager;
    public Player mainplayer;
    public Enemy enemy;
    public Toggle AutoSkillToggle;
    public void SetToggle()
    {
        foreach (var t in castingmanager.skillslots)
        {
            t.SetAuto(true);
        }

        Inventory.Instance.iteminfopanel.Hpslot.AutoStart();
        Inventory.Instance.iteminfopanel.Mpslot.AutoStart();

    }

    public void OffToggle()
    {
        foreach (var t in castingmanager.skillslots)
        {
            t.SetAuto(false);
        }
        Inventory.Instance.iteminfopanel.Hpslot.FalseAutoStart();
        Inventory.Instance.iteminfopanel.Mpslot.FalseAutoStart();

    }
    Enemy[] enemymany = new Enemy[3];
    // ReSharper disable Unity.PerformanceAnalysis
    public void UseSkill_Mainplayer(Skillslot skilldata)
    {
        var count = int.Parse(skilldata.skilldata.SkillRange);

        if (mainplayer.abilityskillallatack)
        {
            count = 3;
        }
        
        if (count != 1 && !EnemySpawnManager.Instance.bossstage)
        {
            enemymany = EnemySpawnManager.Instance.gettarget(count);
            foreach (var t in enemymany)
            {
                UseSkill(mainplayer, t.hpmanager, skilldata, skilldata.effect_enemy, skilldata.effect_player);
            }
            EquipSkillOneMelee(skilldata.Skilltype);
        } 
        else
        {
            UseSkill(mainplayer, Battlemanager.Instance.GetTarget().hpmanager, skilldata, skilldata.effect_enemy, skilldata.effect_player);
            EquipSkillOneMelee(skilldata.Skilltype);
        }

    }

    #region ���� ��������
    void OffAtkPercentBuff()
    {
        //���� ���� ���� ���� �����ϱ� ���� ����.
        mainplayer.buff_atkPercent = 0;
        mainplayer.buffmanager.EndBuff(0);
        PlayerData.Instance.RefreshPlayerstat();
        
    }
    void OffMAtkPercentBuff()
    {
        //���� ���� ���� ���� �����ϱ� ���� ����.
        mainplayer.buffmanager.EndBuff(1);

        mainplayer.buff_matkPercent = 0;
        PlayerData.Instance.RefreshPlayerstat();
    }
    
    void OffCritBuff()
    {
        //���� ���� ���� ���� �����ϱ� ���� ����.
        mainplayer.buff_crit = 0;
        mainplayer.buffmanager.EndBuff(2);
        PlayerData.Instance.RefreshPlayerstat();
    }
    
    void OffCritDmgBuff()
    {
        //���� ���� ���� ���� �����ϱ� ���� ����.
        mainplayer.buffmanager.EndBuff(3);

        mainplayer.buff_critdmg = 0;
        PlayerData.Instance.RefreshPlayerstat();
    }
   

    #endregion

    public void ResetAllSkills()
    {
        foreach (var t in castingmanager.skillslots)
        {
            if(t.skillid != string.Empty)
            {
                t.ResetCooldown();
            }
        }

        for (int i = 0; i < PlayerBuff.Length; i++)
        {
            PlayerBuff[i] = 0f;
            CheckBuff(i);
        }
        //OffCritBuff();
        
        castingmanager.ResetAllCasting();
    }

    public void CoolDownSkills(float sec)
    {
        foreach (var t in castingmanager.skillslots)
        {
            if (t.skillid == string.Empty) continue;
            if (t.skillid == "") continue;
            if (t.skillid == "Null") continue;
            if (t.skillid == null) continue;
//            Debug.Log(t.skillid);
            if (t.skilldata.SkillType != "Buff")
                t.ReduceCooldown(sec);
        }
    }
    // ReSharper disable Unity.PerformanceAnalysis
    private void UseSkill(Player playerdata, Hpmanager targethp, Skillslot skilldata, string effectenemy = "Attack1",
        string effectplayer = "")
    {
        if (!EnemySpawnManager.Instance.isspawn) return;
        decimal totaldmg;
        switch (skilldata.Skilltype)
        {
            //����
            case "Buff":
                break;

            //��Ʈ �ͽ��÷���
            case "DotAttack":
                Soundmanager.Instance.PlayerSound(skilldata.sound);
                
                if (skilldata.skilldata.AniArrow != "0")
                {
                    if (playerdata.isme)
                    {
                        Battlemanager.Instance.ShootArrow_MainPlayer(skilldata, targethp);
                    }
                }
                CheckDot(skilldata, targethp);
                totaldmg = 0;
                decimal admg = 0;
                decimal bdmg = 0;
                decimal cdmg = 0;
                decimal ddmg = 0;
                decimal edmg = 0;
                switch (skilldata.skilldata.BuffType)
                {
                       
                    //������
                    case "A":
                         admg = targethp.Dot_Stat[2] * targethp.Dot_Stack[2];
                        admg = admg *(decimal)skilldata.Matk;
                         bdmg = targethp.Dot_Stat[3] * targethp.Dot_Stack[3];
                        bdmg = bdmg *(decimal)skilldata.Matk;
                        totaldmg = admg+bdmg;
//                        Debug.Log("���ش�" + totaldmg);
                       //  Debug.Log(totaldmg + "��� ����");
                         if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotexplosiondmg7) >
                             0)
                         {
                             totaldmg += totaldmg * 0.5m;
                         //    Debug.Log(totaldmg + "��� ������ȭ");
                         }
                        break;
                    //ȭ������
                    case "B":
                         admg = targethp.Dot_Stat[0] * targethp.Dot_Stack[0];
                        admg = admg *(decimal)skilldata.Matk;
                         bdmg = targethp.Dot_Stat[1] * targethp.Dot_Stack[1];
                        bdmg = bdmg *(decimal)skilldata.Matk;
                        totaldmg = admg+bdmg;
                      //   Debug.Log(totaldmg + "���� ����");
                         if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotexplosiondmg7) >
                             0)
                         {
                             totaldmg += totaldmg * 0.5m;
                       //      Debug.Log(totaldmg + "���� ������ȭ");
                         }
                       // Debug.Log("���ش�" + totaldmg);

                        break;
                    //����                    
                    case "C":
                        bdmg = targethp.Dot_Stat[4] * targethp.Dot_Stack[4];
                        bdmg = bdmg *(decimal)skilldata.Matk;
                        totaldmg = admg+bdmg;
                     //   Debug.Log(totaldmg + "���� ����");
                        if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotexplosiondmg7) >
                            0)
                        {
                            totaldmg += totaldmg * 0.5m;
                        //    Debug.Log(totaldmg + "���� ������ȭ");
                        }
//                        Debug.Log("���ش�" + totaldmg);
                        break;
                    case "D":
                        admg = targethp.Dot_Stat[0] * targethp.Dot_Stack[0];
                        admg = admg *(decimal)skilldata.Matk;
                        bdmg = targethp.Dot_Stat[1] * targethp.Dot_Stack[1];
                        bdmg = bdmg *(decimal)skilldata.Matk;
                        cdmg = targethp.Dot_Stat[2] * targethp.Dot_Stack[2];
                        cdmg = cdmg *(decimal)skilldata.Matk;
                        ddmg = targethp.Dot_Stat[3] * targethp.Dot_Stack[3];
                        ddmg = ddmg *(decimal)skilldata.Matk;
                        edmg = targethp.Dot_Stat[4] * targethp.Dot_Stack[4];
                        edmg = edmg *(decimal)skilldata.Matk;
                        totaldmg = admg+bdmg+cdmg+ddmg+edmg;
//                        Debug.Log(totaldmg + "������ ��Ʈ�� ����");
                        break;
                }
                
//              Debug.Log("���� ������"  + totaldmg);
                //7�� ��ũ ��ų
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotexplosiondmg7) > 0)
                {
                    totaldmg += totaldmg * 0.5m;
                }
            //    Debug.Log("���� ������"  + totaldmg);
                StartCoroutine(DamageChecker(targethp, totaldmg, playerdata.stat_crit, playerdata.stat_critdmg,
                    skilldata.AttackCount, skilldata, effectenemy));
                equipskillattack_Magic();

                break;
            //�Ϲ� ���� ����
            case "MagicAttack":
                totaldmg = (decimal)(playerdata.stat_matk * skilldata.Matk);
                Soundmanager.Instance.PlayerSound(skilldata.sound);
                //���ڿ��� ��
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenstaffdmg) != 0)
                {
                    totaldmg = totaldmg + (totaldmg *
                                           (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager
                                               .EquipStatFloat
                                               .legenstaffdmg));
                    equipskillmanager.Instance.showequipslots("1205",
                        equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenstaffrare)
                            .ToString("N0"),
                        equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenstafflv)
                            .ToString("N0"));
                    //  Debug.Log("�̱״ϽþƸ� �����ߴ�");
                }


                if (skilldata.skilldata.AniArrow != "0")
                {
                    if (playerdata.isme)
                    {
                        Battlemanager.Instance.ShootArrow_MainPlayer(skilldata, targethp);
                    }
                }

                StartCoroutine(DamageChecker(targethp, totaldmg, playerdata.stat_crit, playerdata.stat_critdmg,
                    skilldata.AttackCount, skilldata, effectenemy));
                equipskillattack_Magic();



                CheckDot(skilldata, targethp);

                break;

            //�Ϲ� ���� ����
            case "Attack":
                totaldmg = (decimal)(playerdata.stat_atk * skilldata.Atk);
                Soundmanager.Instance.PlayerSound(skilldata.sound);

                if (skilldata.skilldata.AniArrow != "0")
                {
                    if (playerdata.isme)
                    {
                        Battlemanager.Instance.ShootArrow_MainPlayer(skilldata, targethp);
                    }
                }
                //�⺻ȭ��
                else if (skilldata.skilldata.JustArrow != "0")
                {
                    Battlemanager.Instance.ShootArrow_MainPlayer_NoANi(skilldata, targethp);
                }
                StartCoroutine(DamageChecker(targethp, totaldmg, playerdata.stat_crit, playerdata.stat_critdmg,
                    skilldata.AttackCount, skilldata, effectenemy));
                
                
                equipskillattack_Physic_Melee();
                CheckDot(skilldata, targethp);
                
                
                break;
        }

    }

    public void StartBuffSystem()
    {
        StartCoroutine(PlayerBuffTimer());
    }
    public float[] PlayerBuff= new float[(int)BuffFloat.Length];
    public GameObject[] PlayerBuffImage;
    public enum 
        BuffFloat
    {
        AtkPercent, //����
        MAtkPercent, //����
        Crit, //ġ��ŸȮ��
        Critdmg, //ġ��Ÿ ����
        BasicAtk,
        Crit_P,
        AtkPercent_P,
        Critdmg_P,
        Length
    }

    public void SetBuff(BuffFloat enumnum , float time)
    {
        PlayerBuff[(int)enumnum] = time;
        PlayerBuffImage[(int)enumnum].SetActive(true);
    }
    
    private readonly WaitForSeconds waitbuff = new WaitForSeconds(0.1f);
    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator PlayerBuffTimer()
    {
        while (true)
        {
            yield return waitbuff;
            if (!Battlemanager.Instance.isbattle || !EnemySpawnManager.Instance.isspawn) continue;
            for (int i = 0; i < PlayerBuff.Length; i++)
            {
                if (PlayerBuff[i] != 0)
                {
                    PlayerBuff[i] -= 0.1f;
                    CheckBuff(i);
                }
            }
            
        }
    }

    void CheckBuff(int i)
    {
        return;
        if (PlayerBuff[i] <= 0)
        {
            //������
            PlayerBuff[i] = 0;
            PlayerBuffImage[i].SetActive(false);
            switch (i)
            {
                case 0: //����
                    OffAtkPercentBuff();
                    break;
                case 1: //����
                    OffMAtkPercentBuff();
                    break;
                case 2: //ġȮ
                    OffCritBuff();
                    break;
                case 3: //ġ��
                    OffCritDmgBuff();
                    break;
                case 4:
                    break;
            }
        }
    }
    
    //��ų���� �ߵ��Ǵ°� ���ݽ�ų�� �ش�.
    // ReSharper disable Unity.PerformanceAnalysis
    void equipskillattack_Magic()
    {
        //��׳���ũ
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenwandhitper) != 0)
        {
            if ((Random.Range(0, 100) <
                 equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenwandhitper)))
            {
                equipskillmanager.Instance.showequipslots("1250",
                    equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenwandrare)
                        .ToString("N0")
                    , equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenwandlv).ToString("N0"));


                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                enemymany = EnemySpawnManager.Instance.gettarget(3);

                foreach (var VARIABLE in enemymany)
                {
                    VARIABLE.hpmanager.TakeDamage(
                        dpsmanager.attacktype.Ư��ȿ��,"E1250",
                        (decimal)(mainplayer.stat_matk *
                                  equipskillmanager.Instance.GetStats(
                                      (int)equipskillmanager.EquipStatFloat.legenwanddmg)),
                        iscrit, mainplayer.stat_critdmg, "", 0, "Fire5");
                }
            }
        }
        
        //�ͽ��÷���
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.explosion) != 0)
        {
            if ((Random.Range(0, 100) <
                  equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.explosionhitper)))
            {
                //����
                equipskillmanager.Instance.showequipslots("1150",
                    equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.explosionrare)
                        .ToString("N0"),
                    equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.explosionlv)
                        .ToString("N0"));
                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                Battlemanager.Instance.GetTarget().hpmanager.TakeDamage(dpsmanager.attacktype.Ư��ȿ��, "E1150",
                    (decimal)(mainplayer.stat_matk *
                              equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.explosion)),
                    iscrit,
                    mainplayer.stat_critdmg, "", 0, "Fire1");
            }
        }

        //�����Ƽ ���� ��� �� 
        if (PlayerBackendData.Instance.Abilitys[4].Equals("1018"))
        {
            if (Random.Range(1, 101) <=
                (int)mainplayer.ability_magicskillper)
            {
                //����
                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                Battlemanager.Instance.GetTarget().hpmanager.TakeDamage(dpsmanager.attacktype.�����Ƽ,"A1018",
                    (decimal)(mainplayer.stat_matk *
                              mainplayer.ability_magicskilldmg),
                    iscrit, mainplayer.stat_critdmg, "", 0, "Fire5");
            }
        }

        //�ͽ��÷���
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void equipskillattack_Physic_Melee()
    {
        //õ�չ���
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.thundersmashhitper) != 0)
        {
            if ((Random.Range(0, 100) <
                  equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.thundersmashhitper)))
            {
                //����
                equipskillmanager.Instance.showequipslots("1020",
                    equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.thundersmashrare)
                        .ToString("N0")
                    , equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.thundersmashlv).ToString("N0"));
                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                Battlemanager.Instance.GetTarget().hpmanager.TakeDamage(dpsmanager.attacktype.Ư��ȿ��,"E1020",
                    (decimal)(mainplayer.stat_atk *
                              equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                                  .thundersmashlv)),
                    iscrit, mainplayer.stat_critdmg, "", 0, "Thunder4");
            }
        }
       
     
    }

    public void EquipSkillOneMelee(string type)
    {
        if (!type.Equals("Attack")) return;
        
        //�Ϸ��̴� �Ҳ� ��ȭ��
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenswordhitper) != 0)
        {
            //�����巹���̶��
            if ((Random.Range(0, 100) <
                 equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenswordhitper)))
            {
                //����
                equipskillmanager.Instance.showequipslots("1202",
                    equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenswordrare)
                        .ToString("N0")
                    , equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenswordlv).ToString("N0"));


                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                enemymany = EnemySpawnManager.Instance.gettarget(3);

                foreach (var VARIABLE in enemymany)
                {
                    VARIABLE.hpmanager.TakeDamage(
                        dpsmanager.attacktype.Ư��ȿ��,"E1202",
                        (decimal)(mainplayer.stat_atk *
                                  equipskillmanager.Instance.GetStats(
                                      (int)equipskillmanager.EquipStatFloat.legensworddmg)),
                        iscrit, mainplayer.stat_critdmg, "", 0, "Slash2");
                }
            }
        }
    }

    readonly WaitForSeconds waitattack = new WaitForSeconds(0.05f);
    readonly WaitForSeconds waitattack2 = new WaitForSeconds(0.05f);

    private IEnumerator DamageChecker(Hpmanager targethp,decimal totaldmg,float crit,float critdmg,int attackcount,Skillslot skilldata,string effectenemy)
    {
        yield return SpriteManager.Instance.GetWaitforSecond(skilldata.StartAttackTerm);
        DamageManager.Instance.ShowEffect(targethp.transform,effectenemy);

        int addcount = 0;

        if (skilldata.skilldata.BuffType.Equals("BAD"))
        {
            addcount += (mainplayer.weaponatkcount-1) * 3;
        }
        for (var i = 0; i < attackcount + addcount; i++)
        {
            if (!Battlemanager.Instance.isbattle)
                break;
            bool iscrit = Random.Range(0, 101) <= (crit + skilldata.Crit) ? true : false;
            
            if (iscrit && i.Equals(0))
            {
                yield return waitattack2;
               Battlemanager.Instance.BasicAttackAndSkillCrit();

               if (skilldata.Skilltype.Equals("Attack"))
               {
                   PhyiscSkillCrit();
               }
            }

        
            
            //7�� ������ ��ų
            if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.magicdmg7) > 0)
            {
                if (skilldata.Skilltype.Equals("MagicAttack"))
                {
//                    Debug.Log("�߰� ���� ����");
                    targethp.TakeDamage( dpsmanager.attacktype.������ų����,skilldata.skillid,totaldmg * 0.5m, iscrit , critdmg + skilldata.Critdmg, "", skilldata.BreakDmg, "",0);
                }
            }
            targethp.TakeDamage(dpsmanager.attacktype.������ų����,skilldata.skillid,totaldmg, iscrit , critdmg + skilldata.Critdmg, "", skilldata.BreakDmg, "",-1,attackcount + addcount);
            yield return waitattack;
        }
    }

    public void PhyiscSkillCrit()
    {
        //�����Ƽ ���� ��� �� 
        if (PlayerBackendData.Instance.Abilitys[4].Equals("1019"))
        {
            if (Random.Range(1, 101) <=
                (int)mainplayer.ability_critskillper)
            {
                //����
                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                Battlemanager.Instance.GetTarget().hpmanager.TakeDamage(dpsmanager.attacktype.�����Ƽ,"A1019",
                    (decimal)(mainplayer.stat_atk *
                              mainplayer.ability_critskilldmg),
                    iscrit, mainplayer.stat_critdmg, "", 0, "Slash5");
             //   Debug.Log("ũ�� �ߵ�");
                
            }
            
            
        }
    }
    void CheckDot(Skillslot skilldata, Hpmanager target)
    {
        if (skilldata.skilldata.DotType == "0") return;
        string[] dottypes;
        dottypes = skilldata.skilldata.DotType.Split(';');
        foreach (var t in dottypes)
        {
            decimal dmg = 0;
            switch (t)
            {
                case "Bleed": //����
                    //���� �� * ���� * ���� * 20
                    dmg = (decimal)((mainplayer.stat_str + mainplayer.stat_int + (mainplayer.stat_wis * 1.8f)) * 12);
                    dmg = dmg + (dmg * (decimal)mainplayer.Stat_DotDmgUp);
                    if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                        dmg *= 4m;
                    target.AddDot(Hpmanager.DotType.����,
                        int.Parse(skilldata.skilldata.DotCount) + mainplayer.Stat_AddStack, dmg, mainplayer.stat_crit,
                        mainplayer.stat_critdmg, mainplayer.Stat_MaxDotCount);
                    break;
                case "Fire": //ȭ��
                    //ȭ�� �� * ���� * ���� * 20
                    dmg = (decimal)((mainplayer.stat_str + mainplayer.stat_int + (mainplayer.stat_wis * 1.8f)) * 12);
                    dmg = dmg + (dmg * (decimal)mainplayer.Stat_DotDmgUp);
                    if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                        dmg *= 4m;

                    target.AddDot(Hpmanager.DotType.ȭ��,
                        int.Parse(skilldata.skilldata.DotCount) + mainplayer.Stat_AddStack, dmg, mainplayer.stat_crit,
                        mainplayer.stat_critdmg, mainplayer.Stat_MaxDotCount);

                    break;
                case "Shock": //����
                    //ȭ�� ��ø * ���� * ���� * 20
                    dmg = (decimal)(mainplayer.stat_dex + mainplayer.stat_int + (mainplayer.stat_wis * 1.8f)) * 12;
                    dmg = dmg + (dmg * (decimal)mainplayer.Stat_DotDmgUp);
                    if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                        dmg *= 4m;

                    target.AddDot(Hpmanager.DotType.����,
                        int.Parse(skilldata.skilldata.DotCount) + mainplayer.Stat_AddStack, dmg, mainplayer.stat_crit,
                        mainplayer.stat_critdmg, mainplayer.Stat_MaxDotCount);
                    break;
                case "Poison": //
                    //ȭ�� ��ø * ���� * ���� * 20
                    dmg = (decimal)(mainplayer.stat_dex + mainplayer.stat_int + (mainplayer.stat_wis * 1.8f)) * 12;
                    dmg = dmg + (dmg * (decimal)mainplayer.Stat_DotDmgUp);
                    if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                        dmg *= 4m;

                    target.AddDot(Hpmanager.DotType.�ߵ�,
                        int.Parse(skilldata.skilldata.DotCount) + mainplayer.Stat_AddStack, dmg, mainplayer.stat_crit,
                        mainplayer.stat_critdmg, mainplayer.Stat_MaxDotCount);
                    break;
                case "Death":
                    //���� ������ ��� ������ * 30 ��ŭ ���ظ� �ش�. 0.5�� ����
                    dmg = (decimal)(mainplayer.stat_str + mainplayer.stat_dex + mainplayer.stat_int +
                                    (mainplayer.stat_wis * 1.8f)) * 15;
                    dmg = dmg + (dmg * (decimal)mainplayer.Stat_DotDmgUp);
                    if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                        dmg *= 4m;

                    target.AddDot(Hpmanager.DotType.����,
                        int.Parse(skilldata.skilldata.DotCount) + mainplayer.Stat_AddStack, dmg, mainplayer.stat_crit,
                        mainplayer.stat_critdmg, mainplayer.Stat_MaxDotCount);
                    break;
            }
        }
        //��Ʈ�� ������ �ȴ�.

    }

}
 