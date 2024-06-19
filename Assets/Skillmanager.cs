using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skillmanager : MonoBehaviour
{
    //싱글톤만들기.
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
            UseSkill(mainplayer, Battlemanager.Instance.GetTarget().hpmanager, skilldata, skilldata.effect_enemy,
                skilldata.effect_player);
            EquipSkillOneMelee(skilldata.Skilltype);
        }

    }

    #region 버프 삭제관련

    void OffAtkPercentBuff()
    {
        //제일 먼저 넣은 것이 끝나니깐 먼저 끝냄.
        mainplayer.buff_atkPercent = 0;
        mainplayer.buffmanager.EndBuff(0);
        PlayerData.Instance.RefreshPlayerstat();

    }

    void OffMAtkPercentBuff()
    {
        //제일 먼저 넣은 것이 끝나니깐 먼저 끝냄.
        mainplayer.buffmanager.EndBuff(1);

        mainplayer.buff_matkPercent = 0;
        PlayerData.Instance.RefreshPlayerstat();
    }

    void OffCritBuff()
    {
        //제일 먼저 넣은 것이 끝나니깐 먼저 끝냄.
        mainplayer.buff_crit = 0;
        mainplayer.buffmanager.EndBuff(2);
        PlayerData.Instance.RefreshPlayerstat();
    }

    void OffCritDmgBuff()
    {
        //제일 먼저 넣은 것이 끝나니깐 먼저 끝냄.
        mainplayer.buffmanager.EndBuff(3);

        mainplayer.buff_critdmg = 0;
        PlayerData.Instance.RefreshPlayerstat();
    }


    #endregion

    public void ResetAllSkills()
    {
        foreach (var t in castingmanager.skillslots)
        {
            if (t.skillid != string.Empty)
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
            //버프
            case "Buff":
                break;

            //도트 익스플로전
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

                    //독감전
                    case "A":
                        admg = targethp.Dot_Stat[2] * targethp.Dot_Stack[2];
                        admg = admg * (decimal)skilldata.Matk;
                        bdmg = targethp.Dot_Stat[3] * targethp.Dot_Stack[3];
                        bdmg = bdmg * (decimal)skilldata.Matk;
                        totaldmg = admg + bdmg;
//                        Debug.Log("피해는" + totaldmg);
                        //  Debug.Log(totaldmg + "블루 피해");
                        if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotexplosiondmg7) >
                            0)
                        {
                            totaldmg += totaldmg * 0.5m;
                            //    Debug.Log(totaldmg + "블루 피해진화");
                        }

                        break;
                    //화상출혈
                    case "B":
                        admg = targethp.Dot_Stat[0] * targethp.Dot_Stack[0];
                        admg = admg * (decimal)skilldata.Matk;
                        bdmg = targethp.Dot_Stat[1] * targethp.Dot_Stack[1];
                        bdmg = bdmg * (decimal)skilldata.Matk;
                        totaldmg = admg + bdmg;
                        //   Debug.Log(totaldmg + "레드 피해");
                        if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotexplosiondmg7) >
                            0)
                        {
                            totaldmg += totaldmg * 0.5m;
                            //      Debug.Log(totaldmg + "레드 피해진화");
                        }
                        // Debug.Log("피해는" + totaldmg);

                        break;
                    //저주                    
                    case "C":
                        bdmg = targethp.Dot_Stat[4] * targethp.Dot_Stack[4];
                        bdmg = bdmg * (decimal)skilldata.Matk;
                        totaldmg = admg + bdmg;
                        //   Debug.Log(totaldmg + "퍼플 피해");
                        if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotexplosiondmg7) >
                            0)
                        {
                            totaldmg += totaldmg * 0.5m;
                            //    Debug.Log(totaldmg + "퍼플 피해진화");
                        }

//                        Debug.Log("피해는" + totaldmg);
                        break;
                    case "D":
                        admg = targethp.Dot_Stat[0] * targethp.Dot_Stack[0];
                        admg = admg * (decimal)skilldata.Matk;
                        bdmg = targethp.Dot_Stat[1] * targethp.Dot_Stack[1];
                        bdmg = bdmg * (decimal)skilldata.Matk;
                        cdmg = targethp.Dot_Stat[2] * targethp.Dot_Stack[2];
                        cdmg = cdmg * (decimal)skilldata.Matk;
                        ddmg = targethp.Dot_Stat[3] * targethp.Dot_Stack[3];
                        ddmg = ddmg * (decimal)skilldata.Matk;
                        edmg = targethp.Dot_Stat[4] * targethp.Dot_Stack[4];
                        edmg = edmg * (decimal)skilldata.Matk;
                        totaldmg = admg + bdmg + cdmg + ddmg + edmg;
//                        Debug.Log(totaldmg + "포이즌 스트림 피해");
                        break;
                }

                decimal dmgup = 0m;
//              Debug.Log("폭발 피해전"  + totaldmg);
                //7차 네크 스킬
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotexplosiondmg7) > 0)
                {
                    dmgup = 0.5m;
                }

                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6149) > 0)
                {
                    dmgup = (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6149);
                }

                if (dmgup != 0)
                {
                    totaldmg += totaldmg * 0.5m;
                }

                //    Debug.Log("폭발 피해후"  + totaldmg);
                StartCoroutine(DamageChecker(targethp, totaldmg, playerdata.stat_crit, playerdata.stat_critdmg,
                    skilldata.AttackCount, skilldata, effectenemy));
                equipskillattack_Magic();

                break;
            //일반 마법 공격
            case "MagicAttack":
                totaldmg = (decimal)(playerdata.stat_matk * skilldata.Matk);
                Soundmanager.Instance.PlayerSound(skilldata.sound);
                //대자연의 힘
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenstaffdmg) != 0)
                {
                    totaldmg = totaldmg + (totaldmg *
                                           (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager
                                               .EquipStatFloat
                                               .legenstaffdmg));
                    if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6146) == 0 &&
                        equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6147) == 0 &&
                        equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E61521) == 0

                       )
                    {
                        equipskillmanager.Instance.showequipslots("1205",
                            equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenstaffrare)
                                .ToString("N0"),
                            equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenstafflv)
                                .ToString("N0"));
                    }
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

            //일반 물리 공격
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
                //기본화살
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

    public float[] PlayerBuff = new float[(int)BuffFloat.Length];
    public GameObject[] PlayerBuffImage;

    public enum
        BuffFloat
    {
        AtkPercent, //물공
        MAtkPercent, //마공
        Crit, //치명타확률
        Critdmg, //치명타 피해
        BasicAtk,
        Crit_P,
        AtkPercent_P,
        Critdmg_P,
        Length
    }

    public void SetBuff(BuffFloat enumnum, float time)
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
            //버프끝
            PlayerBuff[i] = 0;
            PlayerBuffImage[i].SetActive(false);
            switch (i)
            {
                case 0: //물공
                    OffAtkPercentBuff();
                    break;
                case 1: //마공
                    OffMAtkPercentBuff();
                    break;
                case 2: //치확
                    OffCritBuff();
                    break;
                case 3: //치피
                    OffCritDmgBuff();
                    break;
                case 4:
                    break;
            }
        }
    }

    //스킬사용시 발동되는것 공격스킬만 해당.
    // ReSharper disable Unity.PerformanceAnalysis
    void equipskillattack_Magic()
    {
        //라그나로크
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenwandhitper) != 0)
        {
            if ((Random.Range(0, 100) <
                 GetRate(equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenwandhitper))))
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
                        dpsmanager.attacktype.특수효과, "E1250",
                        (decimal)(mainplayer.stat_matk *
                                  equipskillmanager.Instance.GetStats(
                                      (int)equipskillmanager.EquipStatFloat.legenwanddmg)),
                        iscrit, mainplayer.stat_critdmg, "", 0, "Fire5");
                }
            }
        }

        //제우스의 지팡이
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6146) != 0)
        {
            equipskillmanager.Instance.showequipslots("1317",
                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6146_rare)
                    .ToString("N0")
                , equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6146_lv).ToString("N0"));


            bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
            enemymany = EnemySpawnManager.Instance.gettarget(1);

            foreach (var VARIABLE in enemymany)
            {
                VARIABLE.hpmanager.TakeDamage(
                    dpsmanager.attacktype.특수효과, "E1317",
                    (decimal)(mainplayer.stat_matk *
                              equipskillmanager.Instance.GetStats(
                                  (int)equipskillmanager.EquipStatFloat.E6146)),
                    iscrit, mainplayer.stat_critdmg, "Attack/Magic/Spell_Fire_Critical_03", 0, "Thunder7");
            }
        }

        //크로노스
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E61521) != 0)
        {
            equipskillmanager.Instance.showequipslots("1534",
                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E61521_rare)
                    .ToString("N0")
                , equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E61521_lv).ToString("N0"));


            bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
            enemymany = EnemySpawnManager.Instance.gettarget(1);

            foreach (var VARIABLE in enemymany)
            {
                VARIABLE.hpmanager.TakeDamage(
                    dpsmanager.attacktype.특수효과, "E1534",
                    (decimal)(mainplayer.stat_matk *
                              equipskillmanager.Instance.GetStats(
                                  (int)equipskillmanager.EquipStatFloat.E61521)),
                    iscrit, mainplayer.stat_critdmg, "Attack/Magic/Spell_Fire_Critical_03", 0, "Time1");
            }
        }


        //포세이돈의 지팡이
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6147) != 0)
        {
            equipskillmanager.Instance.showequipslots("1318",
                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6147_rare)
                    .ToString("N0")
                , equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6147_lv).ToString("N0"));


            bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
            enemymany = EnemySpawnManager.Instance.gettarget(3);

            foreach (var VARIABLE in enemymany)
            {
                VARIABLE.hpmanager.TakeDamage(
                    dpsmanager.attacktype.특수효과, "E1318",
                    (decimal)(mainplayer.stat_matk *
                              equipskillmanager.Instance.GetStats(
                                  (int)equipskillmanager.EquipStatFloat.E6147)),
                    iscrit, mainplayer.stat_critdmg, "Attack/Magic/Spell_Fire_Critical_03", 0, "Water6");
            }
        }

        //익스플로전
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.explosion) != 0)
        {
            if ((Random.Range(0, 100) * equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E61611) <
                 equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.explosionhitper)))
            {
                //찬다
                equipskillmanager.Instance.showequipslots("1150",
                    equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.explosionrare)
                        .ToString("N0"),
                    equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.explosionlv)
                        .ToString("N0"));
                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                Battlemanager.Instance.GetTarget().hpmanager.TakeDamage(dpsmanager.attacktype.특수효과, "E1150",
                    (decimal)(mainplayer.stat_matk *
                              equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.explosion)),
                    iscrit,
                    mainplayer.stat_critdmg, "", 0, "Fire1");
            }
        }

        //어빌리티 마법 사용 시 
        if (PlayerBackendData.Instance.Abilitys[4].Equals("1018"))
        {
            if (Random.Range(1, 101) <=
                GetRate(mainplayer.ability_magicskillper))
            {
                //찬다
                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                Battlemanager.Instance.GetTarget().hpmanager.TakeDamage(dpsmanager.attacktype.어빌리티, "A1018",
                    (decimal)(mainplayer.stat_matk *
                              mainplayer.ability_magicskilldmg),
                    iscrit, mainplayer.stat_critdmg, "", 0, "Fire5");
            }
        }

        //익스플로전
    }

    public int GetRate(float num)
    {
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E61611) != 0)
        {
            int a = (int)(num *
                          equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E61611));
///            Debug.Log(a);

            return a;
        }
        else
        {
            return (int)num;
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    void equipskillattack_Physic_Melee()
    {
        //천둥번개
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.thundersmashhitper) != 0)
        {
            if ((Random.Range(0, 100) <
                 GetRate(equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.thundersmashhitper))))
            {
                //찬다
                equipskillmanager.Instance.showequipslots("1020",
                    equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.thundersmashrare)
                        .ToString("N0")
                    , equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.thundersmashlv).ToString("N0"));
                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                Battlemanager.Instance.GetTarget().hpmanager.TakeDamage(dpsmanager.attacktype.특수효과, "E1020",
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

        //일렁이는 불꽃 염화검
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenswordhitper) != 0)
        {
            //마나드레인이라면
            if ((Random.Range(0, 100) <
                 GetRate(equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenswordhitper))))
            {
                //찬다
                equipskillmanager.Instance.showequipslots("1202",
                    equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenswordrare)
                        .ToString("N0")
                    , equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenswordlv).ToString("N0"));


                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                enemymany = EnemySpawnManager.Instance.gettarget(3);

                foreach (var VARIABLE in enemymany)
                {
                    VARIABLE.hpmanager.TakeDamage(
                        dpsmanager.attacktype.특수효과, "E1202",
                        (decimal)(mainplayer.stat_atk *
                                  equipskillmanager.Instance.GetStats(
                                      (int)equipskillmanager.EquipStatFloat.legensworddmg)),
                        iscrit, mainplayer.stat_critdmg, "", 0, "Slash2");
                }
            }
        }

        //일1313
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6142) != 0)
        {

            //찬다
            equipskillmanager.Instance.showequipslots("1313",
                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6142_rare)
                    .ToString("N0")
                , equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6142_lv).ToString("N0"));


            bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
            enemymany = EnemySpawnManager.Instance.gettarget(3);

            foreach (var VARIABLE in enemymany)
            {
                VARIABLE.hpmanager.TakeDamage(
                    dpsmanager.attacktype.특수효과, "E1313",
                    (decimal)(mainplayer.stat_atk *
                              equipskillmanager.Instance.GetStats(
                                  (int)equipskillmanager.EquipStatFloat.E6142)),
                    iscrit, mainplayer.stat_critdmg, "", 0, "Hit1");
            }
        }

        //일1313
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6145) != 0)
        {
            Battlemanager.Instance.mainplayer.buffmanager.AddStack(1);

            if (Battlemanager.Instance.mainplayer.buffmanager.IsMaxStack())
            {

                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                enemymany = EnemySpawnManager.Instance.gettarget(3);

                //찬다
                equipskillmanager.Instance.showequipslots("1316",
                    equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6145_rare)
                        .ToString("N0")
                    , equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6145_lv).ToString("N0"));

                foreach (var VARIABLE in enemymany)
                {
                    VARIABLE.hpmanager.TakeDamage(
                        dpsmanager.attacktype.특수효과, "E1316",
                        (decimal)(mainplayer.stat_atk *
                                  equipskillmanager.Instance.GetStats(
                                      (int)equipskillmanager.EquipStatFloat.E6145_2)),
                        iscrit, mainplayer.stat_critdmg, "", 0, "Hit1");
                }
            }
        }
    }

    readonly WaitForSeconds waitattack = new WaitForSeconds(0.05f);
    readonly WaitForSeconds waitattack2 = new WaitForSeconds(0.05f);

    private IEnumerator DamageChecker(Hpmanager targethp, decimal totaldmg, float crit, float critdmg, int attackcount,
        Skillslot skilldata, string effectenemy)
    {
        yield return SpriteManager.Instance.GetWaitforSecond(skilldata.StartAttackTerm);
        DamageManager.Instance.ShowEffect(targethp.transform, effectenemy);

        int addcount = 0;

        if (skilldata.skilldata.BuffType.Equals("BAD"))
        {
            addcount += (mainplayer.weaponatkcount - 1) * 3;
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
            //7차 마법사 스킬
            if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.magicdmg7) > 0)
            {
                if (skilldata.Skilltype.Equals("MagicAttack"))
                {
//                    Debug.Log("추가 피해 마법");
                    targethp.TakeDamage(dpsmanager.attacktype.마법스킬공격, skilldata.skillid, totaldmg * 0.5m, iscrit,
                        critdmg + skilldata.Critdmg, "", skilldata.BreakDmg, "", 0);
                }
            }

            targethp.TakeDamage(dpsmanager.attacktype.마법스킬공격, skilldata.skillid, totaldmg, iscrit,
                critdmg + skilldata.Critdmg, "", skilldata.BreakDmg, "", -1, attackcount + addcount);
            yield return waitattack;
        }
    }
    public void PhyiscSkillCrit()
    {
        //어빌리티 마법 사용 시 
        if (PlayerBackendData.Instance.Abilitys[4].Equals("1019"))
        {
            if (Random.Range(1, 101) <=
                GetRate(mainplayer.ability_critskillper))
            {
                //찬다
                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                Battlemanager.Instance.GetTarget().hpmanager.TakeDamage(dpsmanager.attacktype.어빌리티, "A1019",
                    (decimal)(mainplayer.stat_atk *
                              mainplayer.ability_critskilldmg),
                    iscrit, mainplayer.stat_critdmg, "", 0, "Slash5");
                //   Debug.Log("크리 발동");

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
                case "Bleed": //출혈
                    //출혈 힘 * 지능 * 지혜 * 20
                    dmg = (decimal)((mainplayer.stat_str + mainplayer.stat_int + (mainplayer.stat_wis * 1.8f)) * 12);
                    dmg = dmg + (dmg * (decimal)mainplayer.Stat_DotDmgUp);
                    if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                        dmg *= 4m;
                    target.AddDot(Hpmanager.DotType.출혈,
                        int.Parse(skilldata.skilldata.DotCount) + mainplayer.Stat_AddStack, dmg, mainplayer.stat_crit,
                        mainplayer.stat_critdmg, mainplayer.Stat_MaxDotCount);
                    break;
                case "Fire": //화상
                    //화상 힘 * 지능 * 지혜 * 20
                    dmg = (decimal)((mainplayer.stat_str + mainplayer.stat_int + (mainplayer.stat_wis * 1.8f)) * 12);
                    dmg = dmg + (dmg * (decimal)mainplayer.Stat_DotDmgUp);
                    if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                        dmg *= 4m;

                    target.AddDot(Hpmanager.DotType.화상,
                        int.Parse(skilldata.skilldata.DotCount) + mainplayer.Stat_AddStack, dmg, mainplayer.stat_crit,
                        mainplayer.stat_critdmg, mainplayer.Stat_MaxDotCount);

                    break;
                case "Shock": //감전
                    //화상 민첩 * 지능 * 지혜 * 20
                    dmg = (decimal)(mainplayer.stat_dex + mainplayer.stat_int + (mainplayer.stat_wis * 1.8f)) * 12;
                    dmg = dmg + (dmg * (decimal)mainplayer.Stat_DotDmgUp);
                    if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                        dmg *= 4m;

                    target.AddDot(Hpmanager.DotType.감전,
                        int.Parse(skilldata.skilldata.DotCount) + mainplayer.Stat_AddStack, dmg, mainplayer.stat_crit,
                        mainplayer.stat_critdmg, mainplayer.Stat_MaxDotCount);
                    break;
                case "Poison": //
                    //화상 민첩 * 지능 * 지혜 * 20
                    dmg = (decimal)(mainplayer.stat_dex + mainplayer.stat_int + (mainplayer.stat_wis * 1.8f)) * 12;
                    dmg = dmg + (dmg * (decimal)mainplayer.Stat_DotDmgUp);
                    if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                        dmg *= 4m;

                    target.AddDot(Hpmanager.DotType.중독,
                        int.Parse(skilldata.skilldata.DotCount) + mainplayer.Stat_AddStack, dmg, mainplayer.stat_crit,
                        mainplayer.stat_critdmg, mainplayer.Stat_MaxDotCount);
                    break;
                case "Death":
                    //죽음 지헤의 모든 스탯의 * 30 만큼 피해를 준다. 0.5초 마다
                    dmg = (decimal)(mainplayer.stat_str + mainplayer.stat_dex + mainplayer.stat_int +
                                    (mainplayer.stat_wis * 1.8f)) * 15;
                    dmg = dmg + (dmg * (decimal)mainplayer.Stat_DotDmgUp);
                    if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                        dmg *= 4m;

                    target.AddDot(Hpmanager.DotType.죽음,
                        int.Parse(skilldata.skilldata.DotCount) + mainplayer.Stat_AddStack, dmg, mainplayer.stat_crit,
                        mainplayer.stat_critdmg, mainplayer.Stat_MaxDotCount);
                    break;
            }
        }
        //도트는 갱신이 된다.

    }

}