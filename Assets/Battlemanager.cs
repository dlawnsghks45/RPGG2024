using BackEnd;
using System.Collections;
using System.Linq;
using Doozy.Engine.Utils.ColorModels;
using UnityEngine;
using UnityEngine.UI;

public class Battlemanager : MonoBehaviour
{
    public Player mainplayer;
    public Player enemyplayer;
    //싱글톤만들기.
    private static Battlemanager _instance = null;
    public static Battlemanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Battlemanager)) as Battlemanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }


    public Enemy NowTarget;





    


    private void Start()
    {
        StartCoroutine(battlestart());
        StartCoroutine(Monbattlestart());
        Skillmanager.Instance.StartBuffSystem();
    }

    public Enemy GetTarget()
    {
        NowTarget = EnemySpawnManager.Instance.GetEnemy();
        return NowTarget;
    }
   

    public ShakeCamera shake;

    public bool isbattle;
    WaitForSeconds wait = new WaitForSeconds(1.1f);
    float waitsecond = 0;
    WaitForSeconds wait_enemy = new WaitForSeconds(1.1f);
    float waitsecond_enemy = 0;
   public  WaitForSeconds waitattackterm = new WaitForSeconds(0.1f);
   public  WaitForSeconds waitattackterm2 = new WaitForSeconds(0.05f);
   // ReSharper disable Unity.PerformanceAnalysis+
  public  Enemy[] enemymany = new Enemy[3];

   // ReSharper disable Unity.PerformanceAnalysis
   private IEnumerator battlestart()
   {
       while (true)
       {
           yield return new WaitUntil(() => isbattle);

           if (!isbattle || !EnemySpawnManager.Instance.isspawn) continue;
           //속도가 다르면 맞춘다
           if (waitsecond != mainplayer.stat_atkspeed)
           {
               float atkspeed = 1.5f - (1.5f * mainplayer.stat_atkspeed);
               wait = new WaitForSeconds(atkspeed);
               waitsecond = mainplayer.stat_atkspeed;
           }

           enemymany = EnemySpawnManager.Instance.gettarget(1 +
                                                            (int)Passivemanager.Instance.GetPassiveStat(Passivemanager
                                                                .PassiveStatEnum.attackcountup) +
                                                                (int)Passivemanager.Instance.GetPassiveStat(Passivemanager
               .PassiveStatEnum.attackcountupandrange));

           if (enemymany.Length > 1)
           {
               for (int k = 0; k < mainplayer.AttackCount; k++)
               {
                   for (int i = 0; i < enemymany.Length; i++)
                   {
                       if (enemymany[i].hpmanager.isdeath) continue;
                       bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                       mainplayer.StartAni("attack");
                       if (mainplayer.attacktype == AttackType.Range)
                           ShootArrow_MainPlayer(EnemySpawnManager.Instance
                               .enemys[i].hpmanager);
                       if (iscrit)
                       {
                           // shake.ShakeCameras();
                       }


                       if (mainplayer.basicattackhit != "")
                       {
                           DamageManager.Instance.ShowEffect(enemymany[i].hpmanager.Effecttrans,
                               mainplayer.basicattackhit);
                       }


                       enemymany[i].hpmanager.TakeDamage(dpsmanager.attacktype.기본공격,"1",GetBasicAtk(), iscrit, mainplayer.stat_critdmg,
                           mainplayer.attacktrigger == null
                               ? ""
                               : mainplayer.attacktrigger[Random.Range(0, mainplayer.attacktrigger.Length)], 0,
                           "Attack1");

                       //기본공격 특수피해 계산.
                       equipskillbasicattack(enemymany[i]);
                       BasicAttackPassive();
                       //Debug.Log("역니는옴" + k);

                       if (k == 0)
                       {
                         //  Debug.Log("역니는옴");
                           BasicAttackPassiveOneOnly();
                           //마나드레인
                           if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.manadrain) !=
                               0)
                           {
                               if (Random.Range(0, 100) <
                                   equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                                       .manadrainhitper))
                               {
                                   //찬다
                                   equipskillmanager.Instance.showequipslots("1000",
                                       equipskillmanager.Instance
                                           .GetStats((int)equipskillmanager.EquipStatFloat.manadrainrare).ToString("N0")
                                       , equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.manadrainlv).ToString("N0"));
                                   mainplayer.hpmanager.Manadrain();
                               }
                           }
                       }

                   }

                   yield return waitattackterm;
               }
           }
           else
           {
               for (int i = 0; i < mainplayer.AttackCount; i++)
               {
                   if (!GetTarget().hpmanager.isdeath)
                   {
                       bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                       mainplayer.StartAni("attack");
                       if (mainplayer.attacktype == AttackType.Range)
                           ShootArrow_MainPlayer(EnemySpawnManager.Instance
                               .enemys[EnemySpawnManager.Instance.nowtarget].hpmanager);


                       if (iscrit)
                       {
                           shake.ShakeCameras();
                       }


                       if (mainplayer.basicattackhit != "")
                       {
                           DamageManager.Instance.ShowEffect(NowTarget.hpmanager.Effecttrans,
                               mainplayer.basicattackhit);
                       }


                       GetTarget().hpmanager.TakeDamage(dpsmanager.attacktype.기본공격,"1",GetBasicAtk(), iscrit, mainplayer.stat_critdmg,
                           mainplayer.attacktrigger == null
                               ? ""
                               : mainplayer.attacktrigger[Random.Range(0, mainplayer.attacktrigger.Length)], 0,
                           "Attack1");

                       
                       //기본공격 특수피해 계산.
                       equipskillbasicattack(GetTarget());
                       BasicAttackPassive();
                       if (i == 0)
                       {
                           BasicAttackPassiveOneOnly();
                           //마나드레인
                         

                           if (iscrit)
                           {
                               yield return waitattackterm2;
                               BasicAttackAndSkillCrit();
                           }
                       }
                   }

                   yield return waitattackterm;
               }
           }


           yield return wait;
       }
   }

   decimal GetBasicAtk()
   {
       decimal total = (decimal)mainplayer.stat_atk + ((decimal)mainplayer.stat_atk * (decimal)mainplayer.stat_basicatkup);
       return total;
   }
   
   
   void equipskillbasicattack(Enemy target)
    {
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.manadrain) !=
            0)
        {
            //마나드레인이라면
            if (Random.Range(0, 100) < equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.manadrainhitper))
            {
                equipskillmanager.Instance.showequipslots("1000", equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.manadrainrare).ToString("N0"),
                    equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.manadrainlv).ToString("N0"));
                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                target.hpmanager.TakeDamage(dpsmanager.attacktype.특수효과,"E1000",(decimal)(mainplayer.stat_matk * equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.manadrain)), iscrit, mainplayer.stat_critdmg, "", 0, "Slash6");
            }
        }
        
        
        //적혼의 건틀릿
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6141) != 0) 
        {
            
            equipskillmanager.Instance.showequipslots("1312",
                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6141_rare)
                    .ToString("N0"),
                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6141_lv).ToString("N0"));
            bool iscrit1 = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;

            Battlemanager.Instance.mainplayer.buffmanager.AddStack(1);
            if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6159) > 0)
            {
                target.hpmanager.TakeDamage(dpsmanager.attacktype.특수효과,"E1312",
                    (decimal)(mainplayer.stat_atk *
                              equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6141)),
                    iscrit1, mainplayer.stat_critdmg, "Attack/Magic/Fire3", 0, "Fire11");
                if (Battlemanager.Instance.mainplayer.buffmanager.IsMaxStack())
                {
                    equipskillmanager.Instance.showequipslots("1312_2", equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6141_rare).ToString(),"10");
                    bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                    decimal totaldmg = (decimal)mainplayer.stat_atk *
                                       (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6141_2);
                    Enemy enemy = GetTarget();
                
                    enemy.hpmanager.TakeDamage(dpsmanager.attacktype.특수효과,"E1312_2",totaldmg*2m, iscrit, mainplayer.stat_critdmg, "", 0, "Fire12");
                    // ShootArrow_MainPlayer("Water1", enemy.hpmanager);
                }
            }
            else
            {
                target.hpmanager.TakeDamage(dpsmanager.attacktype.특수효과,"E1312",
                    (decimal)(mainplayer.stat_atk *
                              equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6141)),
                    iscrit1, mainplayer.stat_critdmg, "Attack/Magic/Fire3", 0, "Fire9");
              
                if (Battlemanager.Instance.mainplayer.buffmanager.IsMaxStack())
                {
                    equipskillmanager.Instance.showequipslots("1312_3", equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6141_rare).ToString(),"10");
                    bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                    decimal totaldmg = (decimal)mainplayer.stat_atk *
                                       (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6141_2);
                    Enemy enemy = GetTarget();
                
                    enemy.hpmanager.TakeDamage(dpsmanager.attacktype.특수효과,"E1312_3",totaldmg, iscrit, mainplayer.stat_critdmg, "", 0, "Fire10");
                    // ShootArrow_MainPlayer("Water1", enemy.hpmanager);
                }
            }
            
       
        }
        
        
        //멸화도
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legendaggerdmg) != 0)
        {
            //마나드레인이라면
            //찬다
            equipskillmanager.Instance.showequipslots("1203",
                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legendaggerrare)
                    .ToString("N0"),
                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legendaggerlv).ToString("N0"));
            bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
            target.hpmanager.TakeDamage(dpsmanager.attacktype.특수효과,"E1203",
                (decimal)(mainplayer.stat_atk *
                          equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legendaggerdmg)),
                iscrit, mainplayer.stat_critdmg, "Attack/Magic/Fire3", 0, "Slash2");
            //   DamageManager.Instance.ShowEffect(NowTarget.hpmanager.Effecttrans,"Slash6");
        }
        
        //블러드커터
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6144) != 0)
        {
            //마나드레인이라면
            //찬다
            equipskillmanager.Instance.showequipslots("1315",
                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6144_rare)
                    .ToString("N0"),
                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6144_lv).ToString("N0"));
            bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
            target.hpmanager.TakeDamage(dpsmanager.attacktype.특수효과,"E1315",
                (decimal)(mainplayer.stat_atk *
                          equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6144)),
                iscrit, mainplayer.stat_critdmg, "Attack/Magic/Fire3", 0, "Moon1");
            //   DamageManager.Instance.ShowEffect(NowTarget.hpmanager.Effecttrans,"Slash6");
        }
        
        
        //강타
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.smitedmg) != 0)
        {
            //마나드레인이라면
            if (Random.Range(0, 100) < equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.smitehitper))
            {
                //찬다
                
                equipskillmanager.Instance.showequipslots("1010", equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.smiterare).ToString("N0"),
                    equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.smitelv).ToString("N0"));
                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                target.hpmanager.TakeDamage(dpsmanager.attacktype.특수효과,"E1010",(decimal)(mainplayer.stat_atk * equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.smitedmg)), iscrit, mainplayer.stat_critdmg, "", 0, "Slash6");
            }
        }

        //러브
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenmacedmg) != 0)
        {
            //마나드레인이라면
            //찬다
            equipskillmanager.Instance.showequipslots("1249",
                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenmacerare)
                    .ToString("N0"),
                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenmacelv).ToString("N0"));
            bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
            target.hpmanager.TakeDamage(dpsmanager.attacktype.특수효과,"E1249",
                (decimal)(mainplayer.stat_matk *
                          equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenmacedmg)),
                iscrit, mainplayer.stat_critdmg, "Attack/Magic/Fire3", 0, "Love");
            //   DamageManager.Instance.ShowEffect(NowTarget.hpmanager.Effecttrans,"Slash6");
        }
        //아폴론의 메이스
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6148) != 0)
        {
            //마나드레인이라면
            //찬다
            equipskillmanager.Instance.showequipslots("1319",
                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6148_rare)
                    .ToString("N0"),
                equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6148_lv).ToString("N0"));
            bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
            target.hpmanager.TakeDamage(dpsmanager.attacktype.특수효과,"E1319",
                (decimal)(mainplayer.stat_matk *
                          equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6148)),
                iscrit, mainplayer.stat_critdmg, "Attack/Magic/Fire3", 0, "Light3");
            //   DamageManager.Instance.ShowEffect(NowTarget.hpmanager.Effecttrans,"Slash6");
        }
        
        //레이즈의 분노
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.razerage) != 0)
        {
            //마나드레인이라면
            if (Random.Range(0, 100) < equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.razeragehitper))
            {
                //찬다

                equipskillmanager.Instance.showequipslots("1170", equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.razeragelv).ToString("N0"),
                    "10");
                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                target.hpmanager.TakeDamage(dpsmanager.attacktype.특수효과,"E1170",(decimal)(equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.razerage)), iscrit, mainplayer.stat_critdmg, "", 0, "Slash8");
                //   DamageManager.Instance.ShowEffect(NowTarget.hpmanager.Effecttrans,"Slash6");

            }
        }
        //발록의 일격
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.balrockrage) != 0)
        {
            //마나드레인이라면
            if ((Random.Range(0, 100) <
                  equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.balrockhitper))) ;
            //찬다
            {

                equipskillmanager.Instance.showequipslots("1171",
                    equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.balrockragelv)
                        .ToString("N0"), "10");
                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                target.hpmanager.TakeDamage(dpsmanager.attacktype.특수효과, "E1171",
                    (decimal)(equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.balrockrage)),
                    iscrit, mainplayer.stat_critdmg, "", 0, "Slash8");
                //   DamageManager.Instance.ShowEffect(NowTarget.hpmanager.Effecttrans,"Slash6");
            }
        }

        //어빌리티
        if (PlayerBackendData.Instance.Abilitys[4].Equals("1017"))
        {
            //마나드레인이라면
            if (Random.Range(1, 101) <=
                (int)mainplayer.ability_basicskillper)
            {
                //찬다
                bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
                target.hpmanager.TakeDamage(dpsmanager.attacktype.어빌리티, "A1017", (decimal)(mainplayer.stat_atk *
                    mainplayer.ability_basicskilldmg), iscrit, mainplayer.stat_critdmg, "", 0, "Slash8");
                //   DamageManager.Instance.ShowEffect(NowTarget.hpmanager.Effecttrans,"Slash6");
//                Debug.Log("피해" + (decimal)(mainplayer.stat_atk *
  //                                         mainplayer.ability_basicskilldmg));
            }
        }
    }


   //공격 시 딱 한번만 발동
    private void BasicAttackPassiveOneOnly()
    {
        //확률이있다면
        if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.atkcooldown) == 0) return;
        //Debug.Log("공격");
        decimal dmg = mainplayer.hpmanager.MaxHp * (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.hpattack);
        Skillmanager.Instance.CoolDownSkills(Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.atkcooldown));
    }

    //모든 타수에 발동
    private void BasicAttackPassive()
    {
      
    }

    
    //치명타 시 발동
    // ReSharper disable Unity.PerformanceAnalysis
    public void BasicAttackAndSkillCrit()
    {
        //블리자드 
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenbowdmg) != 0) 
        {
            equipskillmanager.Instance.showequipslots("1204", equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenbowrare).ToString("N0"),"10");
            bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
            decimal totaldmg = (decimal)mainplayer.stat_atk *
                               (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legenbowdmg);
            Enemy enemy = GetTarget();
            
            enemy.hpmanager.TakeDamage(dpsmanager.attacktype.특수효과,"E1204",totaldmg, iscrit, mainplayer.stat_critdmg, "", 0, "Buff2");
            ShootArrow_MainPlayer("Water1", enemy.hpmanager);
        }
        
        //드래곤 보우
        if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6143) != 0) 
        {
            Battlemanager.Instance.mainplayer.buffmanager.AddStack(1);
            equipskillmanager.Instance.showequipslots("1314", equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6143_rare).ToString("N0"),"10");
            bool iscrit = Random.Range(0, 101) <= mainplayer.stat_crit ? true : false;
            decimal totaldmg = (decimal)mainplayer.stat_atk *
                               (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6143);
            Enemy enemy = GetTarget();
            
            enemy.hpmanager.TakeDamage(dpsmanager.attacktype.특수효과,"E1314",totaldmg, iscrit, mainplayer.stat_critdmg, "", 0, "Buff2");
            ShootArrow_MainPlayer("Water1", enemy.hpmanager);
            
            if (Battlemanager.Instance.mainplayer.buffmanager.IsMaxStack())
            {
                equipskillmanager.Instance.showequipslots("1314_2", equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6143_rare).ToString("N0"),"10");
                enemy = GetTarget();
                
                totaldmg = (decimal)mainplayer.stat_atk *
                                   (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.E6143_2);
                
                Debug.Log("발사");
                enemy.hpmanager.TakeDamage(dpsmanager.attacktype.특수효과,"E1314_2",totaldmg, iscrit, mainplayer.stat_critdmg, "", 0, "Buff2");
               // ShootArrow_MainPlayer("Water1", enemy.hpmanager);
               ShootArrow_MainPlayer_NoANi2("arrow/1040", enemy.hpmanager);
            }
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator Monbattlestart()
    {
        while (true)
        {
            yield return new WaitUntil(() => isbattle);

            if (!isbattle || !EnemySpawnManager.Instance.isspawn) continue;


            if (EnemySpawnManager.Instance.bossstage)
            {
//                Debug.Log("보스의 공격");
                NowTarget = EnemySpawnManager.Instance.enemys_boss;
                if (waitsecond_enemy != NowTarget.attacktime)
                {
                    wait_enemy = SpriteManager.Instance.GetWaitforSecond(NowTarget.attacktime); 
                    waitsecond_enemy = NowTarget.attacktime;
                }

                if (mapmanager.Instance.isbreak || NowTarget.atk == 0) continue;
                for (int k = 0; k < 1; k++)
                {
                    if(NowTarget.hpmanager.isdeath)
                        continue;
                    
                    bool iscrit = Random.Range(0, 101) <= NowTarget.crit ? true : false;

                    NowTarget.ani.SetTrigger(Attackl);
                    mainplayer.hpmanager.TakeDamage(dpsmanager.attacktype.Length,"-1",(decimal)NowTarget.atk, iscrit, 1.5f, "", 0, "Attack1");
                    yield return waitattackterm;
                }
            }
            else
            {
                enemymany = EnemySpawnManager.Instance.gettarget(EnemySpawnManager.Instance.spawnedmonstercur);
                //속도가 다르면 맞춘다
                foreach (var t in enemymany)
                {
                    if (waitsecond_enemy != t.attacktime)
                    {
                        wait_enemy = SpriteManager.Instance.GetWaitforSecond(t.attacktime); 
                        waitsecond_enemy = t.attacktime;
                    }

                    if (mapmanager.Instance.isbreak || t.atk == 0) continue;
                    for (int k = 0; k < 1; k++)
                    {
                        if(t.hpmanager.isdeath)
                            continue;
                    
                        bool iscrit = Random.Range(0, 101) <= t.crit ? true : false;

                        t.ani.SetTrigger(Attackl);
                        mainplayer.hpmanager.TakeDamage(dpsmanager.attacktype.Length,"-1",(decimal)t.atk, iscrit, 1.5f, "", 0, "Attack1");
                        yield return waitattackterm;
                    }
                }
            }
         
            yield return wait_enemy;
        }
    }

    //플레이어 공격 관련

    void ShootArrow_MainPlayer(Hpmanager EnemyTransform)
    {
        foreach (var t in mainplayer.Arrow.Where(t => !t.gameObject.activeSelf))
        {
            t.SetArrow(mainplayer.ArrowPos, EnemyTransform);
            break;
        }
    }

    public void ShootArrow_MainPlayer_NoANi(Skillslot skilldata, Hpmanager EnemyTransform)
    {
        foreach (var t in mainplayer.Arrow.Where(t => !t.gameObject.activeSelf))
        {
            //  if(mainplayer.Arrow[i].arrowsprite)
            t.SetSprite(SpriteManager.Instance.GetSprite(skilldata.skilldata.JustArrow), int.Parse(skilldata.skilldata.ArrowSpeed), float.Parse(skilldata.skilldata.ArrowSize), float.Parse(skilldata.skilldata.ArrowRotation));
            t.SetArrow(mainplayer.ArrowPos, EnemyTransform,float.Parse(skilldata.skilldata.ArrowRotation));
            break;
        }
    }
    public void ShootArrow_MainPlayer_NoANi2(string arrowpath, Hpmanager EnemyTransform)
    {
        foreach (var t in mainplayer.Arrow.Where(t => !t.gameObject.activeSelf))
        {
            //  if(mainplayer.Arrow[i].arrowsprite)
            t.SetSprite(SpriteManager.Instance.GetSprite(arrowpath), 22, 2.2f, -135);
            t.SetArrow(mainplayer.ArrowPos, EnemyTransform,-135);
            break;
        }
    }
    public void ShootArrow_MainPlayer(Skillslot skilldata, Hpmanager EnemyTransform)
    {
        foreach (var t in mainplayer.AniArrow.Where(t => !t.gameObject.activeSelf))
        {
            //mainplayer.AniArrow[i].SetAni(skilldata.skilldata.AniArrow, 22);
            t.SetArrow(skilldata.skilldata.AniArrow, mainplayer.ArrowPos, EnemyTransform, float.Parse(skilldata.skilldata.ArrowRotation));
            break;
        }
    }
    public void ShootArrow_MainPlayer(string aniarrow, Hpmanager EnemyTransform)
    {
        foreach (var t in mainplayer.AniArrow.Where(t => !t.gameObject.activeSelf))
        {
            //mainplayer.AniArrow[i].SetAni(skilldata.skilldata.AniArrow, 22);
            t.SetArrow(aniarrow, mainplayer.ArrowPos, EnemyTransform,0);
            break;
        }
    }
    //죽음 패널티

    public GameObject Penalty_Obj;
    public Text PenaltyText;
    public bool ispenaltyOn; //패널티가 적용중인가.
    public void Set_DeathPenalty()
    {
        if(PlayerBackendData.Instance.ispremium)
            return;
        
        PlayerBackendData.Instance.DeathPenaltySecond = 180;
        Penalty_Obj.SetActive(true);
        StartCoroutine(StartPenaltyTimer());
    }
    public void RefreshDeath()
    {
        if (PlayerBackendData.Instance.DeathPenaltySecond <= 0) return;
        Penalty_Obj.SetActive(true);
        StartCoroutine(StartPenaltyTimer());
    }
    //패널티 타이머 시작
    WaitForSeconds waitonesecond = new WaitForSeconds(1f);
    private static readonly int Attackl = Animator.StringToHash("attackl");

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator StartPenaltyTimer()
    {
        if (Penalty_Obj.activeSelf)
            yield return null;
        
        while (PlayerBackendData.Instance.DeathPenaltySecond > 0)
        {
            PlayerBackendData.Instance.DeathPenaltySecond--;
            Savemanager.Instance.SaveDeathPenalty();

            int a = PlayerBackendData.Instance.DeathPenaltySecond;
            int m = (a % 3600) / 60;
            int s = (a % 3600) % 60;
            PenaltyText.text = string.Format(Inventory.GetTranslate("UI/사망패널티"), m, s);
            yield return waitonesecond;
        }
        Penalty_Obj.SetActive(false);
    }

}