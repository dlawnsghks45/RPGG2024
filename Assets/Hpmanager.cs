
using System;
using DG.Tweening;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Hpmanager : MonoBehaviour
{
    [SerializeField]
    int monnum;
    public bool isenemy;
    public bool isboss;

    public Image Hpbar;
    public TextMeshProUGUI HpbarText;
    public Image Hp_backbar;
    public Image Mpbar;
    public Transform Hptrans;
    public Transform Effecttrans;

    public bool isdeath;

    public decimal CurHp;
    public decimal MaxHp;
    public float CurMp;
    public float MaxMp;
    //정예몹
    public float CurBP; //BP는 Break Point //무력화 점수
    public float MaxBP;

    public SpriteRenderer mainsprite;

    [SerializeField]
    Material mat;

    private IEnumerator flashCoroutine;
    float flashDuration = 0.3f;

    [SerializeField]
    Transform dotpanel;
    public float crit;
    public float critdmg;
    public int maxdotcount;
    public dotslot[] DotSlots;
    public decimal[] Dot_Stat = new Decimal[(int)DotType.Length];
    public int[] Dot_Stack = new int[(int)DotType.Length];
    public float[] Dot_Time = new float[(int)DotType.Length];

    public void AddDot(DotType dottype,int stack,decimal stat,float crit,float critdmg,int maxdotcount)
    {
        //맨뒤로옮김 없는상태면
        if(!DotSlots[(int)dottype].gameObject.activeSelf)
        {
            DotSlots[(int)dottype].transform.SetAsLastSibling();
            DotSlots[(int)dottype].gameObject.SetActive(true);
        }
        this.maxdotcount = maxdotcount;
        this.crit = crit;
        this.critdmg = critdmg;
        Dot_Stat[(int)dottype] = stat;
        Dot_Stack[(int)dottype] += stack;
        if (Dot_Stack[(int)dottype] > maxdotcount)
            Dot_Stack[(int)dottype] = maxdotcount;

        //Debug.Log("아아" + maxdotcount);

            Dot_Time[(int)dottype] = 100;
        DotSlots[(int)dottype].Refresh(Dot_Stack[(int)dottype]);
    }

   
    public enum DotType
    {
        출혈,
        화상,
        감전,
        중독,
        죽음,
        Length
    }

    private void Start()
    {
        if (mat != null)
            mat = mainsprite.GetComponent<SpriteRenderer>().material;
        if (isenemy || isboss)
            StartCoroutine(DotEnumerator());
    }

    private IEnumerator DoFlash()
    {
        float lerpTime = 0;

        while (lerpTime < flashDuration)
        {
            lerpTime += Time.deltaTime;
            float perc = lerpTime / flashDuration;
            SetFlashAmount(1f - perc);
            yield return null;
        }
        SetFlashAmount(0);
    }

    private void Flash()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = DoFlash();
        StartCoroutine(flashCoroutine);
    }

    private void SetFlashAmount(float flashAmount)
    {
        //Debug.Log(Shader.PropertyToID("_StrongTint"));
        if (mat != null)
            mat.SetFloat(Shader.PropertyToID("_StrongTintFade"), flashAmount);
    }

    private void Update()
    {
        if (!isdeath)
        {
            if (Hp_backbar.fillAmount != Hpbar.fillAmount)
            {
                Hp_backbar.fillAmount = Mathf.Lerp(Hp_backbar.fillAmount, Hpbar.fillAmount, Time.deltaTime * 3f);
                if (!MaxHp.Equals(0))
                    RefreshHp();
            }
        }
    }

    public void ReSetDot()
    {
        for(int i = 0; i < DotSlots.Length;i++)
        {
            DotSlots[i].gameObject.SetActive(false);
            Dot_Stack[i] = 0;
            Dot_Time[i] = 0;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void RefreshHp()
    {
        if (isdeath) return;
        if (MaxHp.Equals(0))
        {
            return;
        }
        Hpbar.fillAmount = (float)(CurHp / MaxHp);
        if (Mpbar != null)
            Mpbar.fillAmount =CurMp / MaxMp;
        try
        {
            if (HpbarText != null)
                HpbarText.text = dpsmanager.convertNumber(CurHp); //CurHp.ToString("N0");
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }

        if (!isenemy) return;
        if (!EnemySpawnManager.Instance.bossstage || !isboss) return;
        switch (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype)
        {
            case "1":
                break;
            case "4"://성물전쟁
                mapmanager.Instance.RefreshHp(this, altarwarmanager.Instance.totaldmg, altarwarmanager.Instance.totaldmg, CurBP, MaxBP);
                break;
            case "8"://훈련장
                mapmanager.Instance.RefreshHp(this, dpsmanager.Instance.TotalDmg, dpsmanager.Instance.TotalDmg, CurBP, MaxBP);
                break;
        }

        //Debug.Log("피" + CurHp);
        mapmanager.Instance.RefreshHp(this, CurHp, MaxHp, CurBP, MaxBP);
    }

    public void HealAll()
    {
        CurHp = MaxHp;
        CurMp = MaxMp;
        isdeath = false;
        RefreshHp();
    }
    public void MakeZero()
    {
        CurHp = 0;
        CurMp = 0;
        Hpbar.fillAmount = 0f;
    }
   
    // ReSharper disable Unity.PerformanceAnalysis
    public void HealHp(float  hp, float mp)
    {
        CurHp += (decimal)hp;
        if (CurHp > MaxHp)
            CurHp = MaxHp;


        CurMp += mp;

        if (CurMp > MaxMp)
            CurMp = MaxMp;

        if (hp != 0)
        {
            //  DamageManager.Instance.ShowEffect(Effecttrans, "heal1");
            DamageManager.Instance.ShowDamageText(DamageManager.damagetype.회복, Hptrans, (decimal)hp);
            Soundmanager.Instance.PlayerSound("Sound/potion",0.3f);
        }

        if (mp == 0) return;
        // DamageManager.Instance.ShowEffect(Effecttrans, "heal2");
        DamageManager.Instance.ShowDamageText(DamageManager.damagetype.엠피회복, Hptrans, (decimal)mp);
        Soundmanager.Instance.PlayerSound("Sound/potion",0.3f);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Manadrain()
    {
        float mp = MaxMp * equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.manadrain);


        CurMp += mp;

        if (CurMp > MaxMp)
            CurMp = MaxMp;


        if (mp != 0)
        {
            DamageManager.Instance.ShowDamageText(DamageManager.damagetype.엠피회복, Hptrans, (decimal)mp);
            //Soundmanager.Instance.PlayerSound("Sound/potion");
        }
        
    }

    Sequence mySequence;

    // ReSharper disable Unity.PerformanceAnalysis
    public void TakeDamage(dpsmanager.attacktype attacktype, string dpsid ,decimal dmg, bool iscrit, float critdmg, string sound, float breakdmg,
        string attackeffect = "", int dotnum = -1,int atkcount = 1)
    {
        if (!EnemySpawnManager.Instance.isspawn)
            return;
        if (mySequence == null) // only create if there was none before.
        {
            mySequence = DOTween.Sequence();
            mySequence.Append(mainsprite.DOColor(Color.red, 0f))
                .AppendInterval(0.4f).Append(mainsprite.DOColor(Color.white, 0f));
            mySequence.Play();
        }

        mySequence = null;

     //   Debug.Log("치명타/보스 전 피해량" + dpsmanager.convertNumber(dmg));

        if (iscrit)
            dmg = dmg * (decimal)(critdmg);

//        Debug.Log("치명타 후 /보스 전 피해량" + dpsmanager.convertNumber(dmg));

        
        DamageManager.Instance.ShowEffect(transform, attackeffect);

        // Debug.Log(dmg);

        
        if (isboss)
        {
            dmg = dmg * (decimal)(mapmanager.Instance.finalbossdmg + Battlemanager.Instance.mainplayer.stat_Bossdmg);
        }
        else
        {
            dmg = dmg + (dmg * (decimal)Battlemanager.Instance.mainplayer.stat_Monsterdmg);
        }
        

        //Debug.Log("피해이전 체력" + CurHp);

        //피해감소 적용
        if (!isenemy)
            dmg = dmg - (dmg * (decimal)Battlemanager.Instance.mainplayer.stat_reducedmg);

     
        //피해 증가
        if(isenemy)
            dmg = dmg + (dmg * ((decimal)Battlemanager.Instance.mainplayer.AlldmgUp +(decimal)Battlemanager.Instance.mainplayer.Stat_SmeltDmg));
//        Debug.Log(Battlemanager.Instance.mainplayer.AlldmgUp + "피해증가 후" + dmg);
        
        if (mat != null)
            Flash();
        if (sound != "")
        {
            // Debug.Log(sound);
            Soundmanager.Instance.PlayerSound(sound);
        }


        switch (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype)
        {
            //파ㅣ티레이ㅐ드
            case "12":
                if (isenemy)
                {
                    //보스전
                    if (mapmanager.Instance.BossPenalty[3].activeSelf)
                    {
                        if (attacktype == dpsmanager.attacktype.물리스킬공격)
                        {
                            dmg -= dmg * 0.3m;
                        }
                    }

                    //보스전
                    if (mapmanager.Instance.BossPenalty[4].activeSelf)
                    {
                        if (attacktype == dpsmanager.attacktype.상태이상)
                        {
                            dmg -= dmg * 0.3m;
                        }
                    }

                    //보스전
                    if (mapmanager.Instance.BossPenalty[0].activeSelf)
                    {
                        //피해 30% 감고
                        dmg -= dmg * 0.3m;
                    }

                    //보스전
                    if (PartyRaidBattlemanager.Instance.battledata.playerBuff[0] != 0)
                    {
                        decimal plus = PartyRaidBattlemanager.Instance.battledata.playerBuff[0] * 0.07m;
                        dmg += dmg * plus;
                    }
                    //보스전
                    if (mapmanager.Instance.BossPenalty[5].activeSelf)
                    {
                        if (mapmanager.Instance.BreakLock.activeSelf)
                        {
                            dmg = 0;
                            iscrit = false;
                        }
                    }
                }
                break;
        }

        if (dotnum != -1)
        {
            if (iscrit)
            {
                switch (dotnum)
                {
                    case 0:
                        DamageManager.Instance.ShowDamageText(DamageManager.damagetype.출혈크리, Hptrans, dmg);
                        break;
                    case 1:
                        DamageManager.Instance.ShowDamageText(DamageManager.damagetype.화상크리, Hptrans, dmg);
                        break;
                    case 2:
                        DamageManager.Instance.ShowDamageText(DamageManager.damagetype.감전크리, Hptrans, dmg);
                        break;
                    case 3:
                        DamageManager.Instance.ShowDamageText(DamageManager.damagetype.중독크리, Hptrans, dmg);
                        break;
                    case 4:
                        DamageManager.Instance.ShowDamageText(DamageManager.damagetype.죽음, Hptrans, dmg);
                        break;
                }
            }
            else
            {
                switch (dotnum)
                {
                    case 0:
                        DamageManager.Instance.ShowDamageText(DamageManager.damagetype.출혈, Hptrans, dmg);
                        break;
                    case 1:
                        DamageManager.Instance.ShowDamageText(DamageManager.damagetype.화상, Hptrans, dmg);
                        break;
                    case 2:
                        DamageManager.Instance.ShowDamageText(DamageManager.damagetype.감전, Hptrans, dmg);
                        break;
                    case 3:
                        DamageManager.Instance.ShowDamageText(DamageManager.damagetype.중독, Hptrans, dmg);
                        break;
                    case 4:
                        DamageManager.Instance.ShowDamageText(DamageManager.damagetype.죽음, Hptrans, dmg);
                        break;
                }
            }
        }
        else
        {
            DamageManager.Instance.ShowDamageText(
                iscrit ? DamageManager.damagetype.크리피해 : DamageManager.damagetype.일반피해, Hptrans, dmg);
        }

        //무력화 계산 보스스테이지에서만 계산된다
        if (mapmanager.Instance.iscanbreak && EnemySpawnManager.Instance.bossstage)
        {
            CurBP -= breakdmg;
            if (CurBP < 0)
                CurBP = 0;
        }
        
//        Debug.Log("치명타/보스 후 피해량" + dpsmanager.convertNumber(dmg));

        //DPS추가
        if (dpsmanager.Instance.isdpson && dpsmanager.Instance.DPSButton.activeSelf)
        {
            if (!dmg.Equals(0))
                dpsmanager.Instance.AddDps(attacktype, dmg, dpsid, atkcount);
        }


        switch (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype)
        {
            //파ㅣ티레이ㅐ드
               case "12":
                   CurHp -= dmg;
//                   Debug.Log("피해를 받았다.");
                if (CurHp <= 0 && !isdeath)
                {
                    isdeath = true;
                    CurHp = 0;

                    //적이라면
                    if (isenemy)
                    {
                        // achievemanager.Instance.AddCount(Acheves.레이드격파);
                        //현재 점수가 같다면. 처음으로 간다.
                        EnemySpawnManager.Instance.NowStageindex = 0;
                        EnemySpawnManager.Instance.spawnedmonstercur = 0;
                        
                      
                        //메인보스 피해 저장
                        if (PartyRaidRoommanager.Instance.partyroomdata.nowmap.Equals(PlayerBackendData.Instance
                                .nowstage))
                        {
                            //보스방이면
                            //TotalDmg
                            PartyraidChatManager.Instance.Chat_MainBossRaidFinish(dpsmanager.Instance.TotalDmg);
//                            Debug.Log("피해넣기"+dpsmanager.Instance.TotalDmg);
                            dpsmanager.Instance.EndDps();
                        }
                        else
                        {
                            PartyraidChatManager.Instance.Chat_MiddleRaidClear();
                        }
                        
                        PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                        mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                        //클리어 했다.
                        PartyRaidRoommanager.Instance.PartyradPanel.Show(false);
                        Savemanager.Instance.SaveAchieveDirect();
                        Savemanager.Instance.Save();
                    }
                    else
                    {
                        //메인보스 피해 저장
                        if (PartyRaidRoommanager.Instance.partyroomdata.nowmap.Equals(PlayerBackendData.Instance
                                .nowstage))
                        {
                            //보스방이면
                            //TotalDmg
                            PartyraidChatManager.Instance.Chat_MainBossRaidFinish(dpsmanager.Instance.TotalDmg);
                            dpsmanager.Instance.EndDps();
                        }
                        else
                        {
                            PartyraidChatManager.Instance.Chat_MiddleRaidFail();
                        }
                        dpsmanager.Instance.DPSButton.SetActive(false);
                        dpsmanager.Instance.DpsPanel.gameObject.SetActive(false);
                        PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                        mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                        //alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/사망함"), alertmanager.alertenum.일반);
                    }
                }
                break;
            case "11":
                if (isenemy)
                {
                    WorldBossManager.Instance.AddDmg(dmg);
                }
                CurHp -= dmg;
                if (CurHp <= 0 && !isdeath)
                {
                    isdeath = true;
                    CurHp = 0;

                    //적이라면
                    if (isenemy)
                    {
                        // achievemanager.Instance.AddCount(Acheves.레이드격파);
                        //현재 점수가 같다면. 처음으로 간다.
                        EnemySpawnManager.Instance.NowStageindex = 0;
                        EnemySpawnManager.Instance.spawnedmonstercur = 0;
                        PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                        mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);

                        WorldBossManager.RankInsert(
                            WorldBossManager.Instance.TotalDmg.ToString(CultureInfo.InvariantCulture));
                        WorldBossManager.Instance.WorldbossPanel.Show(false);
                        dpsmanager.Instance.DPSButton.SetActive(false);
                        Savemanager.Instance.SaveAchieveDirect();
                        Savemanager.Instance.Save();
                    }
                    else
                    {
                        PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                        mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                        WorldBossManager.RankInsert(
                            WorldBossManager.Instance.TotalDmg.ToString(CultureInfo.InvariantCulture));
                        WorldBossManager.Instance.WorldbossPanel.Show(false);
                        dpsmanager.Instance.DPSButton.SetActive(false);
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/사망함"), alertmanager.alertenum.일반);
                        //uimanager.Instance.ShowDeathPanel.Show(false);
                    }
                }

                break;
            case "7"://길드 레이드
                GuildRaidManager.Instance.AddDmg(dmg);
                CurHp -= dmg;
                if (CurHp <= 0 && !isdeath)
                {
                    isdeath = true;
                    CurHp = 0;

                    //적이라면
                    if (isenemy)
                    {
                       // achievemanager.Instance.AddCount(Acheves.레이드격파);
                        //현재 점수가 같다면. 처음으로 간다.
                        EnemySpawnManager.Instance.NowStageindex = 0;
                        EnemySpawnManager.Instance.spawnedmonstercur = 0;
                        PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                        mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                        GuildRaidManager.Instance.DmgtoGuildMob();
                        Savemanager.Instance.SaveAchieveDirect();
                        Savemanager.Instance.Save();
                    }
                }

                break;
            case "4": //성물전쟁
                altarwarmanager.Instance.totaldmg += dmg;
                MaxHp = altarwarmanager.Instance.totaldmg;
                CurHp = MaxHp;
                TutorialTotalManager.Instance.CheckGuideQuest("tutoaltar");
                break;
            case "8": //성물전쟁
                MaxHp = dpsmanager.Instance.TotalDmg;
                CurHp = MaxHp;
                break;
            case "0":
            case "1":
            case "2":
            case "3":
            case "9":
            case "10":
                CurHp -= dmg;
          //      Debug.Log("죽었다");
                if (CurHp <= 0 && !isdeath && Battlemanager.Instance.isbattle)
                {
                    isdeath = true;
                    CurHp = 0;
                    //적이라면
                    if (isenemy)
                    {
                        //   Debug.Log("죽었다2");
                        //맵타입에따른 보상
                        switch (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype)
                        {
                            case "10":

                                if (EnemySpawnManager.Instance.NowStageindex == EnemySpawnManager.Instance.NowMaxindex)
                                {
                                    //achievemanager.Instance.AddCount(Acheves.보스처치);
                                    //QuestManager.Instance.AddCount(1, "craft");

                                    if (PlayerBackendData.Instance.ContentLevel[
                                            Contentmanager.Instance.nowPlaycontentnum] ==
                                        Contentmanager.Instance.nowplayinglevel &&
                                        PlayerBackendData.Instance.ContentLevel[
                                            Contentmanager.Instance.nowPlaycontentnum] <
                                        Contentmanager.Instance.dropid_playing.Length)
                                    {
                                        //콘텐츠 레벨업
                                        PlayerBackendData.Instance.ContentLevel[
                                            Contentmanager.Instance.nowPlaycontentnum]++;
                                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/콘텐츠레벨업성공"),
                                            alertmanager.alertenum.일반);
                                        Settingmanager.Instance.SaveContent();
                                    }

                                    TutorialTotalManager.Instance.CheckGuideQuest("contentdungeon");

                                    switch (PlayerBackendData.Instance.nowstage)
                                    {
                                        case "9998": //대마법사의 묘q
                                        case "9997": //대마법사의 묘
                                        case "9996": //대마법사의 묘
                                        case "9995": //대마법사의 묘
                                        case "9994": //대마법사의 묘
                                           // achievemanager.Instance.AddCount(Acheves.대마법사의묘);
                                            QuestManager.Instance.AddCount(1, "content2");
                                           // Savemanager.Instance.SaveAchieveDirect();
                                            //Savemanager.Instance.Save();
                                            break;
                                    }



                                    mondropmanager.Instance.GiveDropToInvenToryBoss(transform,
                                        mondropmanager.Instance.Mon_DropItemIDboss,
                                        mondropmanager.Instance.Mon_DropItemMinHowmanyboss,
                                        mondropmanager.Instance.Mon_DropItemMaxHowmanyboss,
                                        mondropmanager.Instance.Mon_DropItemPercentboss);
                                    //현재 점수가 같다면. 처음으로 간다.
                                    EnemySpawnManager.Instance.NowStageindex = 0;
                                    EnemySpawnManager.Instance.spawnedmonstercur = 0;
//                                    Debug.Log("저장 마릿수" + mapmanager.Instance.savespawncount);
                                    PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                                    mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                                }
                                else
                                {
                                    EnemySpawnManager.Instance.CheckFieldKillAllDungeon();
                                }

                                EnemySpawnManager.Instance.RefreshStage();
                                break;
                            case "0": //사냥터라면
                                if (EnemySpawnManager.Instance.NowStageindex == EnemySpawnManager.Instance.NowMaxindex)
                                {
                                    if (EnemySpawnManager.Instance.iseventspawned)
                                    {
                                        EnemySpawnManager.Instance.iseventspawned = false;
                                        QuestManager.Instance.AddCount(1, "eventmon");
                                    }

                                    //현재 점수가 같다면. 처음으로 간다.
                                    mondropmanager.Instance.GiveDropToInvenToryBoss(transform,
                                        mondropmanager.Instance.Mon_DropItemIDboss,
                                        mondropmanager.Instance.Mon_DropItemMinHowmanyboss,
                                        mondropmanager.Instance.Mon_DropItemMaxHowmanyboss,
                                        mondropmanager.Instance.Mon_DropItemPercentboss);
                                    EnemySpawnManager.Instance.CheckFieldKillBoss();
                                    EnemySpawnManager.Instance.NowStageindex = 0;
                                    EnemySpawnManager.Instance.spawnedmonstercur = 0;


                                    mondropmanager.Instance.GiveEventDropToInvenTory(transform,
                                        mondropmanager.Instance.Event_DropItemID
                                        , mondropmanager.Instance.Event_DropItemMinHowmany,
                                        mondropmanager.Instance.Event_DropItemMaxHowmany,
                                        mondropmanager.Instance.Event_DropItemPercent);
                                    mondropmanager.Instance.checkDrop = false;
                                    //업적
                                   // achievemanager.Instance.AddCount(Acheves.사냥터클리어);
                                   // achievemanager.Instance.AddCount(Acheves.보스처치);
                                   // QuestManager.Instance.AddCount(1, "content2");

                                    //시즌 패스 퀘스트 체크
                                    achievemanager.Instance.CheckFinishSeasonPass();

                                    if (Autofarmmanager.Instance.isChecking)
                                    {
                                        Autofarmmanager.Instance.FinishCheckOffline();
                                    }
                                    
                                    //스테이지면 다음 스테이지로 넘김
                                    if (int.Parse(
                                            MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).mapneednum) >=
                                        PlayerBackendData.Instance.GetFieldLv())
                                    {
                                        PlayerBackendData.Instance.SetFieldLV(int.Parse(MapDB.Instance
                                            .Find_id(PlayerBackendData.Instance.nowstage).mapneednum) + 1);
                                        Savemanager.Instance.SaveStageData();
                                    }

                                    if (!mapmanager.Instance.stageautouptoggle.isOn && MapDB.Instance
                                            .Find_id(PlayerBackendData.Instance.nowstage).mapnextstage != "0")
                                    {
                                        mapmanager.Instance.LocateMap(MapDB.Instance
                                            .Find_id(PlayerBackendData.Instance.nowstage).mapnextstage);
                                    }

                                    //특수 포탈 체크
                                    if (Portalmanager.Instance.canmakeportal())
                                    {
                                        //Portalmanager.Instance.ShowPortal();
                                    }

                                    Battlemanager.Instance.mainplayer.hpmanager.HealAll();
                                }
                                else
                                {


                                    EnemySpawnManager.Instance.CheckFieldKillAll();
                                    mondropmanager.Instance.GiveDropToInvenTory(transform,mondropmanager.Instance.Mon_DropItemID
                                        , mondropmanager.Instance.Mon_DropItemMinHowmany,
                                        mondropmanager.Instance.Mon_DropItemMaxHowmany,
                                        mondropmanager.Instance.Mon_DropItemPercent);

                                    mondropmanager.Instance.GiveEventDropToInvenTory(transform,
                                        mondropmanager.Instance.Event_DropItemID
                                        , mondropmanager.Instance.Event_DropItemMinHowmany,
                                        mondropmanager.Instance.Event_DropItemMaxHowmany,
                                        mondropmanager.Instance.Event_DropItemPercent);
                                }

                                //가이드퀘스트
                                Tutorialmanager.Instance.CheckTutorial("killmon");
                               // achievemanager.Instance.AddCount(Acheves.몬스터처치);
                                QuestManager.Instance.AddCount(1, "monkill");

                                EnemySpawnManager.Instance.RefreshStage();
                                break;
                            case "1": //던전
                                if (EnemySpawnManager.Instance.NowStageindex == EnemySpawnManager.Instance.NowMaxindex)
                                {
                                    //achievemanager.Instance.AddCount(Acheves.보스처치);
                                    //achievemanager.Instance.AddCount(Acheves.던전격파);
                                    QuestManager.Instance.AddCount(1, "dungeon");

                                    if (!PlayerBackendData.Instance.sotang_dungeon.Contains(PlayerBackendData.Instance
                                            .nowstage))
                                    {
                                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/던전최초클리어"),
                                            alertmanager.alertenum.일반);
                                        PlayerBackendData.Instance.sotang_dungeon.Add(PlayerBackendData.Instance
                                            .nowstage);
                                        Debug.Log("은 " + PlayerBackendData.Instance
                                            .nowstage);
                                        Settingmanager.Instance.SaveSotangs();
                                    }

                                    if (PlayerBackendData.Instance.nowstage.Equals("3006"))
                                    {
                                        TutorialTotalManager.Instance.CheckGuideQuest("dungeonclearborn");
                                    }
                                    if (PlayerBackendData.Instance.nowstage.Equals("3007"))
                                    {
                                        TutorialTotalManager.Instance.CheckGuideQuest("dungeon13clear");
                                    }
                                    if (PlayerBackendData.Instance.nowstage.Equals("3009"))
                                    {
                                        TutorialTotalManager.Instance.CheckGuideQuest("dungeon14clear");
                                    }
                                    if (PlayerBackendData.Instance.nowstage.Equals("3010"))
                                    {
                                        TutorialTotalManager.Instance.CheckGuideQuest("dungeon15clear");
                                    }
                                    TutorialTotalManager.Instance.CheckGuideQuest("dungeonclear");

                                    mondropmanager.Instance.GiveDropToInvenToryBoss(transform,
                                        mondropmanager.Instance.Mon_DropItemIDboss,
                                        mondropmanager.Instance.Mon_DropItemMinHowmanyboss,
                                        mondropmanager.Instance.Mon_DropItemMaxHowmanyboss,
                                        mondropmanager.Instance.Mon_DropItemPercentboss);
                                    //현재 점수가 같다면. 처음으로 간다.
                                    EnemySpawnManager.Instance.NowStageindex = 0;
                                    EnemySpawnManager.Instance.spawnedmonstercur = 0;
                                    PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;

                                    //던전 클리엉 횟수
                                    DungeonManager.Instance.FinishAutoDungeonCount++;

                                    if (DungeonManager.Instance.autocount_Doing != 1 &&
                                        DungeonManager.Instance.isautodungeon)
                                    {
                                        //반복이라면
                                        DungeonDB.Row data =
                                            DungeonDB.Instance.Find_mapid(PlayerBackendData.Instance.nowstage);
                                        if (PlayerBackendData.Instance.CheckItemAndRemove(data.needitem,
                                                int.Parse(data.needhowmany)))
                                        {
                                            EnemySpawnManager.Instance.CheckFieldKillBoss();
                                            PlayerBackendData.Instance.spawncount = 3;
                                            EnemySpawnManager.Instance.spawnedmonstercur = 0;
                                            EnemySpawnManager.Instance.NowStageindex = 0;
                                            EnemySpawnManager.Instance.RefreshStage();
                                            Battlemanager.Instance.mainplayer.hpmanager.HealAll();

                                            DungeonManager.Instance.DownAutoCount();
                                        }
                                        else
                                        {
                                            //입장권이 부족합니다.
                                            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/입장권부족"),
                                                alertmanager.alertenum.일반);
                                            PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                                            mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                                            DungeonManager.Instance.FinishDungeonLoot();
                                        }
                                    }
                                    else
                                    {
                                        //  alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/입장권부족"),alertmanager.alertenum.일반);
                                        PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;

                                        mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                                        DungeonManager.Instance.FinishDungeonLoot();
                                        DungeonManager.Instance.DungeonAutoCountShow.text = "";

                                    }
                                }
                                else
                                {
                                    mondropmanager.Instance.GiveDropToInvenTory(transform,mondropmanager.Instance.Mon_DropItemID
                                        , mondropmanager.Instance.Mon_DropItemMinHowmany,
                                        mondropmanager.Instance.Mon_DropItemMaxHowmany,
                                        mondropmanager.Instance.Mon_DropItemPercent);
                                    EnemySpawnManager.Instance.CheckFieldKillAllDungeon();
                                }

                                EnemySpawnManager.Instance.RefreshStage();
                                break;
                            case "2": //승급 시간이 끝나면
                                //플레이어의 레벨과 맵의 레벨이 같다면 레벨업
                                if (PlayerBackendData.Instance.GetAdLv() ==
                                    int.Parse(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maprank))
                                {
                                    //모험가 레벨업
                                    //achievemanager.Instance.AddCount(Acheves.승급);
                                    PlayerBackendData.Instance.AddAdLv(1);
                                    uimanager.Instance.ResetLoot();
                                    mondropmanager.Instance.GiveDropToInvenToryBoss(transform,
                                        mondropmanager.Instance.Mon_DropItemIDboss,
                                        mondropmanager.Instance.Mon_DropItemMinHowmanyboss,
                                        mondropmanager.Instance.Mon_DropItemMaxHowmanyboss,
                                        mondropmanager.Instance.Mon_DropItemPercentboss);
                                    uimanager.Instance.ShowSuccAdventureLv();
                                    LogManager.Log_CrystalEarn("승급");
                                    //가이드 퀘스트
                                    Tutorialmanager.Instance.CheckTutorial("adlvup");
                                    Tutorialmanager.Instance.CheckTutorial("adlvup3");
                                    Tutorialmanager.Instance.CheckTutorial("adlvup4");
                                    PlayerData.Instance.RefreshPlayerstat();
                                }

                                Savemanager.Instance.SaveInventory();
                                Savemanager.Instance.SaveExpData();
                                Settingmanager.Instance.SaveAllNoLog();
                                Savemanager.Instance.Save();
                                PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                                mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                                break;
                            case "3": //레이드
                                //  achievemanager.Instance.AddCount(Acheves.레이드격파);
                                if (PlayerBackendData.Instance.nowstage.Equals("5031"))
                                {
                                        alertmanager.Instance.ShowAlert(string.Format(Inventory.GetTranslate("UI8/발록레이드토벌성공"),(EliteRaid.Instance.LevelCount.nowcount).ToString()),
                                            alertmanager.alertenum.일반);
                                        
                                        mondropmanager.Instance.GiveDropToInvenToryBossPercentUp(transform,
                                            "5031",EliteRaid.Instance.GetPercent());
                                        
                                        if (EliteRaid.Instance.LevelCount.nowcount-1 ==
                                            PlayerBackendData.Instance.ContentLevel[10])
                                        {
                                            //단계 상승
                                            PlayerBackendData.Instance.ContentLevel[10]++;
                                            EliteRaid.Instance.LevelCount.Maxcount =
                                                PlayerBackendData.Instance.ContentLevel[10] + 1;
                                            EliteRaid.Instance.LevelCount.SetCount(EliteRaid.Instance.LevelCount.Maxcount);
                                           Settingmanager.Instance.SaveContent();
                                        }
                                   
                                }
                                else
                                {
                                    if (!PlayerBackendData.Instance.sotang_raid.Contains(
                                            PlayerBackendData.Instance.nowstage))
                                    {
                                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/레이드최초클리어"),
                                            alertmanager.alertenum.일반);
                                        PlayerBackendData.Instance.sotang_raid.Add(PlayerBackendData.Instance.nowstage);
                                        Settingmanager.Instance.SaveSotangs();
                                    }
                                    
                                    mondropmanager.Instance.GiveDropToInvenToryBoss(transform,
                                        mondropmanager.Instance.Mon_DropItemIDboss,
                                        mondropmanager.Instance.Mon_DropItemMinHowmanyboss,
                                        mondropmanager.Instance.Mon_DropItemMaxHowmanyboss,
                                        mondropmanager.Instance.Mon_DropItemPercentboss);
                                }


                                switch (PlayerBackendData.Instance.nowstage)
                                {
                                    case "5007":
                                        TutorialTotalManager.Instance.CheckGuideQuest("raidclearborn");
                                        break;
                                    case "5008":
                                        TutorialTotalManager.Instance.CheckGuideQuest("raidclear11");
                                        break;
                                    case "5009":
                                        TutorialTotalManager.Instance.CheckGuideQuest("raidclear12");
                                        break;
                                    case "5010":
                                        TutorialTotalManager.Instance.CheckGuideQuest("raidclear13");
                                        break;
                                    case "5011":
                                        TutorialTotalManager.Instance.CheckGuideQuest("raidclear14");
                                        break;
                                    case "5012":
                                        TutorialTotalManager.Instance.CheckGuideQuest("raidclear15");
                                        break;
                                }

                                TutorialTotalManager.Instance.CheckGuideQuest("raidclear");


                                //현재 점수가 같다면. 처음으로 간다.
                                EnemySpawnManager.Instance.NowStageindex = 0;
                                EnemySpawnManager.Instance.spawnedmonstercur = 0;
                                PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                            
                                mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                                Savemanager.Instance.SaveInventory();
                                RaidManager.Instance.FinishRaid();
                                break;

                            //개미굴
                            case "9":
                                //플레이어의 레벨과 맵의 레벨이 같다면 레벨업
                                if (Antcavemanager.Instance.autotoggle.IsOn)
                                {
                                    Antcavemanager.Instance.SaveReward();
                                    mapmanager.Instance.LocateMap("9999");
                                }
                                else
                                {
                                    Antcavemanager.Instance.GiveRewardAntCave();
                                    PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                                    Debug.Log(mapmanager.Instance.savemapid);
                                    mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                                }

                                break;
                        }

                        EnemySpawnManager.Instance.RemoveEnemy(monnum);
                    }
                    else
                    {
                        //유저라면
                        //맵타입에따른 보상
                        switch (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype)
                        {
                            case "0":
                                //Battlemanager.Instance.Set_DeathPenalty();
                                int stage = int.Parse(PlayerBackendData.Instance.nowstage)-1;
                                mapmanager.Instance.stageautouptoggle.isOn = true;
                                mapmanager.Instance.LocateMap(stage.ToString());
                                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/사망함"),alertmanager.alertenum.일반);
                                uimanager.Instance.ShowDeathPanel.Show(false);
                                Autofarmmanager.Instance.initCheckOffline();
                                break;
                            case "1":
                                PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                                mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/사망함"),alertmanager.alertenum.일반);
                                uimanager.Instance.ShowDeathPanel.Show(false);
                                break;
                            case "2": //승급 시간이 끝나면
                                PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                                mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/사망함"),alertmanager.alertenum.일반);
                                //모험가 레벨업 실패
                                uimanager.Instance.ShowDeathPanel.Show(false);

                                break;
                            case "3":
                            case "10":
                                PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                                mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/사망함"),alertmanager.alertenum.일반);
                                uimanager.Instance.ShowDeathPanel.Show(false);

                                break;
                            case "9":
                                PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                                mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/사망함"),alertmanager.alertenum.일반);
                                uimanager.Instance.ShowDeathPanel.Show(false);

                                break;
                            case "11":
                                Debug.Log("유저사망");
                                PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                                mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                                WorldBossManager.RankInsert(WorldBossManager.Instance.TotalDmg.ToString(CultureInfo.InvariantCulture));
                                WorldBossManager.Instance.WorldbossPanel.Show(false);
                                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/사망함"),alertmanager.alertenum.일반);
                                //uimanager.Instance.ShowDeathPanel.Show(false);
                                break;
                        }

                        CurHp = MaxHp;
                        CurMp = MaxMp;
                        isdeath = false;
                        RefreshHp();
                        Savemanager.Instance.SaveInventory_SaveOn();
                    }
                    EnemySpawnManager.Instance.bossstage = false;
                    mapmanager.Instance.RefreshBoss();
                }

                break;
          
        }

        RefreshHp();
    }

    public void ReSetBreakPoint()
    {
        CurBP = MaxBP;
        RefreshHp();
    }

    public bool reduceMp(float cost)
    {
        if(CurMp >= cost)
        {
            CurMp -= cost;
            return true;
        }
        else
        {
            return false;
        }
    }

  
    IEnumerator DotEnumerator()
    {
        while (true)
        {
            yield return new WaitUntil(() => Battlemanager.Instance.isbattle);

            yield return SpriteManager.Instance.GetWaitforSecond(Battlemanager.Instance.mainplayer.dottime);

            if (!Battlemanager.Instance.isbattle || !EnemySpawnManager.Instance.isspawn) continue;
            if (isdeath) continue;
            
            for (int i = 0; i < DotSlots.Length; i++)
            {
                bool iscrit = Random.Range(0, 101) <= crit ? true : false;
                if (Dot_Stack[i] == 0) continue;
                //피해를 입힘
//                Debug.Log("도ㅓ트는" + i);
                TakeDamage(dpsmanager.attacktype.상태이상,$"Dot{i}",Dot_Stat[i] * Dot_Stack[i], iscrit, critdmg, "", 0, "", i);
                Dot_Time[i] -= Battlemanager.Instance.mainplayer.dottime;
                if (Dot_Time[i] <= 0)
                    DotSlots[i].gameObject.SetActive(false);
            }

        }
    }
  
}
