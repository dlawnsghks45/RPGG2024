using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using EasyMobile;
using UnityEngine;
using UnityEngine.UI;

public class mapmanager : MonoBehaviour
{

    //싱글톤만들기.
    private static mapmanager _instance = null;
    public static mapmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(mapmanager)) as mapmanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }


    public UIToggle stagetoggle;
    public GameObject DungeonExitbutton;
    public FadeBackground fade;
    public Text MapText;
    public Slider MapTime;
    public Image HpGauge;
    public Image HpGaugeback;
    public Image BreakGauge;

    public GameObject BreakStop; //무력화가 되면 나가는 모래시계
    public GameObject BreakStop2; //무력화가 되면 나가는 모래시계
    public GameObject BreakLock; //무력화가 끝난 후 무력화가 잠기는 시간.
        //public GameObject RageObj;
   // public Text RageText;
   // public int RageCount = 0;

    public GameObject[] BossPenalty;
    public Text[] BossPenaltyText;
    public int[] BossPenaltyCount;

    public float Curtime;
    public float Maxtime;
    public bool isbreak;
    public bool iscanbreak; //무력화가 가능한 시간인가.
    public float breaktime; //무력화되는 시간
    public float breaknewtime; //무력화 새로 시작하는 시간.

    public SpriteRenderer[] background;


    public GameObject Ok;


    public Toggle stageautouptoggle;


    //맵이동 애니
    public Animator bossshowani;
    public Animator changeLocateAni;
    public Text changemaptext;
    public stageslot[] stageslots;
    public Toggle[] FieldCountLv; //보토 어려움 매우어려움

    
    //보통
    
    //지옥

    //토글로 체크 시 자기 랭크대만 보여줌
    public void MapLvShow()
    {
            for(int i = 0; i < stageslots.Length;i++)
            {
                if(int.Parse(MapDB.Instance.Find_id(stageslots[i].mapid).maprank) == PlayerBackendData.Instance.GetAdLv())
                {
                    stageslots[i].gameObject.SetActive(true);
                }
                else
                {
                    stageslots[i].gameObject.SetActive(false);
                }
            }
    }

    public void MapLvShowAll()
    {
        for (int i = 0; i < stageslots.Length; i++)
        {
              //  stageslots[i].gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (!EnemySpawnManager.Instance.bossstage) return;
        if (HpGaugeback.fillAmount != HpGauge.fillAmount)
        {
            HpGaugeback.fillAmount = Mathf.Lerp(HpGaugeback.fillAmount, HpGauge.fillAmount, Time.deltaTime * 3f);
        }
    }

    public void StartTimer(float time)
    {
        Maxtime = time;
        Curtime = time;
        MapTime.value = Curtime / Maxtime;
        StartCoroutine(Timerstart());
    }

    //타이머 종료
    // ReSharper disable Unity.PerformanceAnalysis
    private void FinishTimer()
    {
        EnemySpawnManager.Instance.bossstage = false;
        mapmanager.Instance.RefreshBoss();
        StopCoroutine(Timerstart());
    }
    
    
    //pu
    public string GetRecentMapid()
    {
        if (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype == "0")
        {
            return PlayerBackendData.Instance.nowstage;
        }
        else
        {
            return savemapid;
        }
    }

    WaitForSeconds wait = new WaitForSeconds(0.1f);
    WaitForSeconds wait2 = new WaitForSeconds(2f);
    WaitForSeconds wait3 = new WaitForSeconds(3f);
    private float healbp = 0;
    // ReSharper disable Unity.PerformanceAnalysis

    public void RageStart()
    {
        StartCoroutine(RageOn());
    }
    IEnumerator RageOn()
    {
        BossPenalty[(int)PartyRaidBuffEnemy.광폭].SetActive(true);
        BossPenaltyText[(int)PartyRaidBuffEnemy.광폭].text = "1";
        BossPenaltyCount[(int)PartyRaidBuffEnemy.광폭] = 1;
        float basicatk = EnemySpawnManager.Instance.enemys_boss.atk;
        while (EnemySpawnManager.Instance.bossstage)
        {
            yield return wait3;
            if (!mapmanager.Instance.isbreak)
            {
                BossPenaltyCount[(int)PartyRaidBuffEnemy.광폭]++;
                BossPenaltyText[(int)PartyRaidBuffEnemy.광폭].text = BossPenaltyCount[(int)PartyRaidBuffEnemy.광폭].ToString();
                EnemySpawnManager.Instance.enemys_boss.atk = (basicatk * (1.1f * BossPenaltyCount[(int)PartyRaidBuffEnemy.광폭]));
            }
        }
    }
    private IEnumerator Timerstart()
    {
        for (int i = 0; i < BossPenalty.Length; i++)
        {
            BossPenalty[i].SetActive(false);
            BossPenaltyCount[i] = 0;
            BossPenaltyText[i].text = "";
        }
        yield return wait2;
        if (bool.Parse(monsterDB.Instance.Find_id(EnemySpawnManager.Instance.enemys_boss.monid).ispenalty))
        {


            for (int i = 0; i < PartyRaidBattlemanager.Instance.nowPenalty.Length; i++)
            {
                if (PartyRaidBattlemanager.Instance.nowPenalty[i] == 1)
                {
                    BossPenalty[i].SetActive(true);
                    BossPenaltyCount[i] = 1;
                    BossPenaltyText[i].text = "";
                }
            }

            if (bool.Parse(monsterDB.Instance.Find_id(EnemySpawnManager.Instance.enemys_boss.monid).israge))
            {
                RageStart();
            }

            PlayerData.Instance.RefreshPlayerstat();
        }
        else
        {
            if (bool.Parse(monsterDB.Instance.Find_id(EnemySpawnManager.Instance.enemys_boss.monid).israge))
            {
                RageStart();
            }
        }
        
        
       
        
        while(EnemySpawnManager.Instance.bossstage)
        {
            if (!isbreak)
            {
                yield return wait;
                Curtime -= 0.1f;
                    MapTime.value = Curtime / Maxtime;
                if (dpsmanager.Instance.isdpson)
                {
                    dpsmanager.Instance.second += 0.1f;
                    dpsmanager.Instance.RefreshTime();
                }

                //시간이 종료되면
                if (!(Curtime <= 0)) continue;
                //맵타입에따른 보상
                switch (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype)
                {
                    case "0": //사냥보스 시간이 끝나면
                        EnemySpawnManager.Instance.iseventspawned = false;
                        EnemySpawnManager.Instance.CheckFieldKillBoss();
                        EnemySpawnManager.Instance.NowStageindex = 0;
                        EnemySpawnManager.Instance.spawnedmonstercur = 0;
                        EnemySpawnManager.Instance.ReturnEnemy();
                        EnemySpawnManager.Instance.RefreshStage();
                        Autofarmmanager.Instance.initCheckOffline();
                        break;
                    
                    case "1": //승급 시간이 끝나면
                    case "10": //승급 시간이 끝나면
                    case "2": //승급 시간이 끝나면
                        LocateMap(savemapid);
                        break;
                    case "3": //레이드
                        //횟수 복원
                        if (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).id.Equals("5031"))
                        {

                        }
                        else if (Timemanager.Instance.AddDailyCount(Timemanager.ContentEnumDaily.레이드, 1))
                        {
                            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/레이드실패"),
                                alertmanager.alertenum.일반);
                        }

                        LocateMap(savemapid);
                        break;
                    case "4": //성물 파괴
                        TutorialTotalManager.Instance.CheckGuideQuest("altarstart");
                        RankingManager.Instance.RankInsert(dpsmanager.Instance.TotalDmg.ToString(), RankingManager.RankEnum.성물);
                        LocateMap(savemapid);
                        dpsmanager.Instance.Bt_SaveDps();
                        dpsmanager.Instance.EndDps();
                        break;
                    case "5": //골드러쉬
                        LocateMap(savemapid);
                        break;
                    case "6": //도적단 섬멸
                        LocateMap(savemapid);
                        break;
                    case "7": //길드레이드
                        GuildRaidManager.Instance.DmgtoGuildMob();
                        LocateMap(savemapid);
                        break;
                    case "8": //훈련장
                        LocateMap(savemapid);
                        dpsmanager.Instance.EndDps();
                        break;
                    case "9": //개미굴실패
                        //횟수 복원
                        Antcavemanager.Instance.FinishButton.gameObject.SetActive(false);
                        if (Antcavemanager.Instance.ids_save.Count != 0)
                        {
                            Antcavemanager.Instance.GiveReward();
                            QuestManager.Instance.AddCount(Antcavemanager.Instance.ids_save.Count,"content3");
                        }
                        else
                        {
                            QuestManager.Instance.AddCount(1,"content3");
                        }
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI4/개미굴실패"),alertmanager.alertenum.일반);
                        LocateMap(savemapid);
                        break;
                    case "11":
                        //월드보스레이드
                        //랭킹입력
                        WorldBossManager.RankInsert(WorldBossManager.Instance.TotalDmg.ToString(CultureInfo.InvariantCulture));
                        WorldBossManager.Instance.WorldbossPanel.Show(false);
                        LocateMap(savemapid);
                        break;
                    case "12":
                        //월드보스레이드
                        //랭킹입력
                        PartyRaidRoommanager.Instance.PartyradPanel.Show(false);
                       //WorldBossManager.RankInsert(WorldBossManager.Instance.TotalDmg.ToString(CultureInfo.InvariantCulture));
                       // WorldBossManager.Instance.WorldbossPanel.Show(false);
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
                        
                        
                        LocateMap(savemapid);
                        break;
                }
                EnemySpawnManager.Instance.bossstage = false;
                RefreshBoss();
            }
            else
            {
                curbp += healbp;
                //Debug.Log(curbp);
                if (curbp > maxbp)
                    curbp = maxbp;
                
                BreakGauge.fillAmount =  curbp/ maxbp; //다시 참
                BreakGauge.color = Color.grey;
                if (dpsmanager.Instance.isdpson)
                {
                    dpsmanager.Instance.breaktime += 0.1f;
                    dpsmanager.Instance.RefreshTime();
                }
                yield return wait;
            }
        }
    }
    [Button (Name = "Test")]
    public void TestStart()
    {
        StartTimer(20f);
        isbreak = false;
    }
    Hpmanager hp;

    public float NowAddBossDmg = 1;
    public float savedaddbossdmg = 0; //저장된다 시간이 지나면 빠진다.
    public float breakadddmg =0 ; //저장된다 시간이 지나면 빠진다.
    public float finalbossdmg;

    public Text Breakadddmgtext;
    public Animator BreakResetAni;

    public void StartBossShow()
    {
        bossshowani.SetTrigger(Show);

        finalbossdmg = NowAddBossDmg;
        Breakadddmgtext.text = $"{(finalbossdmg * 100f):N0}%";
        Breakadddmgtext.color = Color.white;
    }

    private float curbp;
    private float maxbp;

    
    public void RefreshHp(Hpmanager hpmanager, decimal cur, decimal max, float curbreak, float maxbreak)
    {
        //Debug.Log("이름" + hpmanager.name);
        if (hp == null)
            hp = hpmanager;
        HpGauge.fillAmount = ((float)cur / (float)max);


        if (!isbreak)
        {
            curbp = curbreak;
            maxbp = maxbreak;
            BreakGauge.fillAmount = curbp / maxbp;
            BreakGauge.color = Color.green;
            if (maxbreak != 0 && curbreak <= 0)
            {
                BreakResetAni.SetTrigger(Reset);
                BreakResetAni.speed = 1 / breaktime;
                //브레이크 잠김
                healbp = ((maxbp / breaktime) * 0.12f);
                curbp += healbp;
                //시간 잠시 스톱
                isbreak = true;
                EnemySpawnManager.Instance.enemys_boss.ani.speed = 0;
                if (BossPenalty[(int)PartyRaidBuffEnemy.광폭].activeSelf)
                {
                    BossPenaltyCount[(int)PartyRaidBuffEnemy.광폭] /= 2;
                    BossPenaltyText[(int)PartyRaidBuffEnemy.광폭].text = BossPenaltyCount[(int)PartyRaidBuffEnemy.광폭].ToString();
                }
                
                //보스전
                if (mapmanager.Instance.BossPenalty[2].activeSelf)
                {
                    EnemySpawnManager.Instance.enemys_boss.hpmanager.CurHp +=
                        (EnemySpawnManager.Instance.enemys_boss.hpmanager.MaxHp * 0.07m);

                    if (EnemySpawnManager.Instance.enemys_boss.hpmanager.CurHp >
                        EnemySpawnManager.Instance.enemys_boss.hpmanager.MaxHp)
                    {
                        EnemySpawnManager.Instance.enemys_boss.hpmanager.CurHp =
                            EnemySpawnManager.Instance.enemys_boss.hpmanager.MaxHp;
                    }
                    
                    DamageManager.Instance.ShowDamageText(DamageManager.damagetype.회복, 
                        EnemySpawnManager.Instance.enemys_boss.transform,  (EnemySpawnManager.Instance.enemys_boss.hpmanager.MaxHp * 0.07m));
                }
                

                BreakStop.SetActive(true); //무력화
                BreakStop2.SetActive(true);
                BreakLock.SetActive(false); //무력화 잠금 없앰
                //무력화 피해
                finalbossdmg = breakadddmg + Battlemanager.Instance.mainplayer.Stat_BreakDmg;
                Breakadddmgtext.text = $"{(finalbossdmg * 100f):N0}%";
                Breakadddmgtext.color = Color.red;
                
                Invoke(nameof(FinishBreak), breaktime);
            }
            else
            {

            }
        }
    }

    //무력화 종료
    public void FinishBreak()
    {
        BreakStop.SetActive(false);
        BreakStop2.SetActive(false);
        BreakLock.SetActive(true);
        iscanbreak = false;
        isbreak = false;
        //무력화 포인트 초기화
        hp.ReSetBreakPoint();

        finalbossdmg = NowAddBossDmg;
        Breakadddmgtext.text = $"{(finalbossdmg * 100f):N0}%";
        Breakadddmgtext.color = Color.white;

        EnemySpawnManager.Instance.enemys_boss.ani.speed = 1;

        //무력화 잠금 시작
        Invoke(nameof(LockBreak), breaknewtime);
    }
    public void LockBreak() //해당 시간 후 잠금이 풀림
    {
        BreakLock.SetActive(false);
        iscanbreak = true;
    }

    public void RefreshBoss()
    {
        Ok.SetActive(EnemySpawnManager.Instance.bossstage);
    }
    [Button (Name = "MapTest")]
    public void TestLocate()
    {
        LocateMap("1002");
    }
    public string savemapid;
    public int savespawncount;
    public string maptype;
    public string[] dungeonMpb;
    //맵이동
    public void ExitDungeon()
    {
        LocateMap(savemapid);
        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/던전탈출성공"),alertmanager.alertenum.일반);
    }

    public bool islocating = false;

    public void falseislocating()
    {
        islocating = false;
    }    
    public void LocateMap(string id)
    {
        Autofarmmanager.Instance.initCheckOffline();
        mondropmanager.Instance.checkDrop = false;
        EnemySpawnManager.Instance.iseventspawned = false;
        if (id.Equals("999"))
        {
            id = "1000";
        }
        if (!Battlemanager.Instance.isbattle)
            return;
        Portalmanager.Instance.RemovePortal();
        
        //ekerl
        stagetoggle.gameObject.SetActive(false);
        DungeonExitbutton.gameObject.SetActive(false);
        DungeonManager.Instance.DungeonAutoCountShow.text = "";
       
        
//        Debug.Log("맵이동");
        if (id.Equals(""))
        {
            id = PlayerBackendData.Instance.nowstage;
        }
        
        CancelInvoke();
        FinishTimer();
        //화면 페이드
        fade.Fade();
        PlayerBackendData.Instance.nowstage = id;
        Battlemanager.Instance.isbattle = false;
        EnemySpawnManager.Instance.ischangelocate = true;
        //2초뒤 맵이름 변경

        //가이드퀘스트
        TutorialTotalManager.Instance.CheckFinish();
        
        //맵타입
        maptype = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype;
        mondropmanager.Instance.SetEventDrop(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype);

        
        
        
        //맵타입에따른 추가
        switch (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype)
        {
            case "0": //사냥터


                if (EnemySpawnManager.Instance.savedcount.Equals(3))
                {
                }


                if (EnemySpawnManager.Instance.savedcount != 0)
                {
                    savespawncount = 3;
                }
                savemapid = PlayerBackendData.Instance.nowstage;
                PlayerBackendData.Instance.spawncount = savespawncount;
                EnemySpawnManager.Instance.spawnedmonstercur = 0;
                EnemySpawnManager.Instance.spawnedmonstermax =
                    int.Parse(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).mapcount);
                EnemySpawnManager.Instance.NowStageindex = 0;
                EnemySpawnManager.Instance.NowMaxindex = 9;
                Savemanager.Instance.Saveonlystage();
                EnemySpawnManager.Instance.RemoveEnemySpawnAll();

               // Debug.Log("여기다");

                Savemanager.Instance.SaveStageData();
                EnemySpawnManager.Instance.RefreshStage();
                mondropmanager.Instance.SetEventDrop(
                    MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype);
                if (EnemySpawnManager.Instance.iseventspawned)
                {
                    mondropmanager.Instance.checkDrop = false;
                    mondropmanager.Instance.SetDropData("4999");
                }
                else
                {
                    mondropmanager.Instance.checkDrop = false;
                    mondropmanager.Instance.SetDropData(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).monsterid);
                }
                
                stagetoggle.gameObject.SetActive(true);
                islocating = true;
                Invoke(nameof(falseislocating), 4f);
                break;
            case "1": //던전
                dungeonMpb = DungeonDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).monid.Split(';');
                EnemySpawnManager.Instance.spawnedmonstercur = 0;
                EnemySpawnManager.Instance.spawnedmonstermax =
                    int.Parse(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).mapcount);
                EnemySpawnManager.Instance.NowStageindex = 0;
                EnemySpawnManager.Instance.NowMaxindex = 9;
                EnemySpawnManager.Instance.RefreshStage();
                Battlemanager.Instance.mainplayer.hpmanager.HealAll();
                DungeonExitbutton.gameObject.SetActive(true);
                DungeonManager.Instance.DungeonAutoCountShow.text =
                    string.Format(Inventory.GetTranslate("UI2/남은던전횟수"),
                        DungeonManager.Instance.autocount_Doing.ToString());
                break;
            case "2": //승급
                EnemySpawnManager.Instance.spawnedmonstercur = 0;
                EnemySpawnManager.Instance.spawnedmonstermax =
                    int.Parse(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).mapcount);
                Invoke(nameof(StartAdventureLv), 2.1f);
                EnemySpawnManager.Instance.CloseStageObj();
                break;
            case "3": //싱글 레이드
                EnemySpawnManager.Instance.spawnedmonstercur = 0;
                EnemySpawnManager.Instance.spawnedmonstermax =
                    int.Parse(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).mapcount);
                Invoke(nameof(StartSingleBoss), 2.1f);
                Battlemanager.Instance.mainplayer.hpmanager.HealAll();
                EnemySpawnManager.Instance.CloseStageObj();
                break;
            case "4": //성물파괴
                EnemySpawnManager.Instance.spawnedmonstercur = 0;
                EnemySpawnManager.Instance.spawnedmonstermax =
                    int.Parse(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).mapcount);
                Invoke(nameof(StartAltarBoss), 2.1f);
                EnemySpawnManager.Instance.CloseStageObj();
                Battlemanager.Instance.mainplayer.hpmanager.MakeZero();
                dpsmanager.Instance.StartDps();

                break;
            case "5": //골드러쉬
                break;
            case "6": //도적단 섬멸
                break;
            case "7": //길드 레이드
                EnemySpawnManager.Instance.spawnedmonstercur = 0;
                EnemySpawnManager.Instance.spawnedmonstermax =
                    int.Parse(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).mapcount);
                Invoke(nameof(StartSingleBoss), 2.1f);
                //Battlemanager.Instance.mainplayer.hpmanager.HealAll();
                EnemySpawnManager.Instance.CloseStageObj();
                break;
            case "8": //훈련장
                EnemySpawnManager.Instance.spawnedmonstercur = 0;
                EnemySpawnManager.Instance.spawnedmonstermax =
                    int.Parse(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).mapcount);
                Invoke(nameof(StartTrainingBoss), 2.1f);
                EnemySpawnManager.Instance.CloseStageObj();
                Battlemanager.Instance.mainplayer.hpmanager.MakeZero();
                dpsmanager.Instance.StartDps();
                break;
            case "9": //개미굴
                EnemySpawnManager.Instance.spawnedmonstercur = 0;
                EnemySpawnManager.Instance.spawnedmonstermax =
                    int.Parse(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).mapcount);
                Invoke(nameof(StartAntCave), 2.1f);
                //Battlemanager.Instance.mainplayer.hpmanager.HealAll();
                EnemySpawnManager.Instance.CloseStageObj();
                break;
            case "10": //콘텐츠
                dungeonMpb = Contentmanager.Instance.monid_playing;
                EnemySpawnManager.Instance.spawnedmonstercur = 0;
                EnemySpawnManager.Instance.spawnedmonstermax =
                    int.Parse(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).mapcount);
                EnemySpawnManager.Instance.NowStageindex = 0;
                EnemySpawnManager.Instance.NowMaxindex = 9;
                EnemySpawnManager.Instance.RefreshStage();
                Battlemanager.Instance.mainplayer.hpmanager.HealAll();
                break;
            case "11": //월드보스레이드
                EnemySpawnManager.Instance.spawnedmonstercur = 0;
                EnemySpawnManager.Instance.spawnedmonstermax =
                    int.Parse(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).mapcount);
                Invoke(nameof(StartWorldBoss), 2.1f);
                EnemySpawnManager.Instance.CloseStageObj();
                dpsmanager.Instance.StartDps();
                break;
            case "12": //월드보스레이드
                if (PartyRaidRoommanager.Instance.partyroomdata.nowmap.Equals(PlayerBackendData.Instance
                        .nowstage))
                {
                    //보스방이면
                    //TotalDmg
                    dpsmanager.Instance.StartDps();
                }
                EnemySpawnManager.Instance.spawnedmonstercur = 0;
                EnemySpawnManager.Instance.spawnedmonstermax =
                    int.Parse(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).mapcount);
                Invoke(nameof(StartPartyRaidBoss), 2.1f);
                EnemySpawnManager.Instance.CloseStageObj();
                break;
        }
        changeLocateAni.SetTrigger(Mapchange);
        if (maptype.Equals("9"))
        {
            changemaptext.text =
                $"{Inventory.GetTranslate(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).name)} {PlayerBackendData.Instance.AntCaveLv}F";
        }
        else
        {
            changemaptext.text =
                $"{Inventory.GetTranslate(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).name)}";
        }
     
        alertmanager.Instance.NotiCheck_Collection();
        Invoke(nameof(RefreshMapName),2);
        Invoke(nameof(RefreshBattle), 4);
        //Debug.Log("배틀시작");
    }
    //승급 시작
    public void StartAdventureLv()
    {
        EnemySpawnManager.Instance.bt_SpawnAdventureBoss();
        Battlemanager.Instance.mainplayer.hpmanager.HealAll();
        Skillmanager.Instance.ResetAllSkills();
    }
    public void StartGuildRaidBoss()
    {
        EnemySpawnManager.Instance.bt_SpawnGuildRaidBoss();
        Battlemanager.Instance.mainplayer.hpmanager.HealAll();
        Skillmanager.Instance.ResetAllSkills();
    }
    public void StartSingleBoss()
    {
        EnemySpawnManager.Instance.bt_SpawnSingleRaidBoss();
        Battlemanager.Instance.mainplayer.hpmanager.HealAll();
        Skillmanager.Instance.ResetAllSkills();
    }
    public void StartAntCave()
    {
        EnemySpawnManager.Instance.bt_SpawnAntCaveBoss();
        Battlemanager.Instance.mainplayer.hpmanager.HealAll();
        Skillmanager.Instance.ResetAllSkills();
    }
    public void StartAltarBoss()
    {
        EnemySpawnManager.Instance.bt_SpawnAltarBoss();
        Battlemanager.Instance.mainplayer.hpmanager.HealAll();
        Skillmanager.Instance.ResetAllSkills();
    }
    public void StartTrainingBoss()
    {
        EnemySpawnManager.Instance.bt_SpawnTrainingBoss();
        Battlemanager.Instance.mainplayer.hpmanager.HealAll();
        Skillmanager.Instance.ResetAllSkills();
    }
    public void StartWorldBoss()
    {
        EnemySpawnManager.Instance.bt_SpawnWorldBoss();
        Battlemanager.Instance.mainplayer.hpmanager.HealAll();
        Skillmanager.Instance.ResetAllSkills();
    }
    public void StartPartyRaidBoss()
    {
        EnemySpawnManager.Instance.bt_SpawnWorldBoss();
        Battlemanager.Instance.mainplayer.hpmanager.HealAll();
        Skillmanager.Instance.ResetAllSkills();
    }
    public void RefreshMapName()
    {
        maptype = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype;
        background[0].sprite = SpriteManager.Instance.GetSprite(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maplayer0);
        EnemySpawnManager.Instance.RemoveEnemySpawn();
        if (PlayerBackendData.Instance.nowstage.Equals("5031"))
        {
            changemaptext.text =
                $"{Inventory.GetTranslate(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).name)} {string.Format(Inventory.GetTranslate("UI8/단계"),EliteRaid.Instance.LevelCount.nowcount)}";
        }
        else if (maptype.Equals("9"))
        {
            changemaptext.text =
                $"{Inventory.GetTranslate(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).name)} {PlayerBackendData.Instance.AntCaveLv}F";
        }
        else
        {
            changemaptext.text =
                $"{Inventory.GetTranslate(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).name)}";
        }
        EnemySpawnManager.Instance.ischangelocate = false;
        MapText.text = changemaptext.text;
    }

    public void RefreshBattle()
    {
        Battlemanager.Instance.isbattle = true;
        CancelInvoke();
    }

    public string GetMapLevel(string level)
    {
        return level switch
        {
            "1" => "Ⅰ",
            "2" => "Ⅱ",
            "3" => "Ⅲ",
            "4" => "Ⅳ",
            _ => ""
        };
    }
    private void Start()
    {
        maptype = "0";
        RefreshMapName();
        Initmapdata();
        // ReSharper disable once SuspiciousTypeConversion.Global
        mapmanager.Instance.savemapid = PlayerBackendData.Instance.nowstage;
        mapmanager.Instance.savespawncount = PlayerBackendData.Instance.spawncount;
        EnemySpawnManager.Instance.savedcount = 3;

        if (PlayerBackendData.Instance.nowstage.Equals(""))
        {
            PlayerBackendData.Instance.nowstage = "1000"; 
            LocateMap("1000");
        }
        EnemySpawnManager.Instance.RefreshStage();
        mondropmanager.Instance.SetEventDrop(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype);

    }

    [SerializeField] private Transform field_basic;
    [SerializeField] private Transform field_nightmare;
    [SerializeField] private Transform field_hell;
    [SerializeField] private Transform field_hell2;
    [SerializeField] private stageslot stageobj;
    void Initmapdata()
    {
        for (int i = 0; i < MapDB.Instance.NumRows(); i++)
        {
            MapDB.Row data = MapDB.Instance.GetAt(i);

            if (data.maparray.Equals("보통사냥터"))
            {
                stageslot slot = Instantiate(stageobj, field_basic);
                slot.Refresh(data.id);
                slot.gameObject.SetActive(true);
            }
            
            if (data.maparray.Equals("악몽사냥터"))
            {
                stageslot slot = Instantiate(stageobj, field_nightmare);
                slot.Refresh(data.id);
                slot.gameObject.SetActive(true);
            }
            
            if (data.maparray.Equals("지옥사냥터"))
            {
                stageslot slot = Instantiate(stageobj, field_hell);
                slot.Refresh(data.id);
                slot.gameObject.SetActive(true);
            }
            if (data.maparray.Equals("지옥사냥터2"))
            {
                stageslot slot = Instantiate(stageobj, field_hell2);
                slot.Refresh(data.id);
                slot.gameObject.SetActive(true);
            }
        }
    }
    
    string nowselectmapid;
    public UIView mapobj;
    public UIView dropItemPanel_Field;
    public Text dropItemPanelMapnameText;
    public Text dropItemPanelMapRankText;
    public itemslot[] Basic_FieldMonDropItem;
    public itemslot[] Boss_FieldMonDropItem;

    private static readonly int Show = Animator.StringToHash("Show");
    private static readonly int Mapchange = Animator.StringToHash("mapchange");
    private static readonly int Reset = Animator.StringToHash("reset");

    //해당 맵의 드롭아이템을 보여줌
    public void ShowDropItemField(string mapid)
    {
        Debug.Log(mapid);
        nowselectmapid = mapid;
        dropItemPanelMapRankText.text = PlayerData.Instance.gettierstar(MapDB.Instance.Find_id(mapid).maprank);
        dropItemPanelMapnameText.text = Inventory.GetTranslate(MapDB.Instance.Find_id(mapid).name);
        for (int i =0; i < Basic_FieldMonDropItem.Length;i++)
        {
            Basic_FieldMonDropItem[i].gameObject.SetActive(false);
            Boss_FieldMonDropItem[i].gameObject.SetActive(false);
        }

        MapDB.Row mapdata = MapDB.Instance.Find_id(mapid);
        monsterDB.Row mondata = monsterDB.Instance.Find_id(mapdata.monsterid);

        string dropid = mondata.dropid;
        string dropid_boss = mondata.bossdrop;

        //일반몹 드롭테이블
        List<MonDropDB.Row> dropdatas_basic = MonDropDB.Instance.FindAll_id(dropid);
       // Debug.Log(dropdatas_basic.Count);
        for (int i = 0; i < dropdatas_basic.Count; i++)
        {
            Basic_FieldMonDropItem[i].SetItem(dropdatas_basic[i].itemid, 0);
            Basic_FieldMonDropItem[i].gameObject.SetActive(true);
        }
        //보스 드롭테이블
        List<MonDropDB.Row> dropdatas_boss = MonDropDB.Instance.FindAll_id(dropid_boss);
        for (int i = 0; i < dropdatas_boss.Count; i++)
        {
            Boss_FieldMonDropItem[i].SetItem(dropdatas_boss[i].itemid, 0);
            Boss_FieldMonDropItem[i].gameObject.SetActive(true);
        }
        mapmanager.Instance.FieldCountLv[PlayerBackendData.Instance.spawncount - 1].isOn = true;

        dropItemPanel_Field.Show(true);
        
        
        Autofarmmanager.Instance.Bt_RefreshOfflineData();
    }

    public void Bt_LocateMap()
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/콘텐츠 중 불가"), alertmanager.alertenum.주의);
            return;
        }
        
        LocateMap(nowselectmapid);
        mapobj.Hide(false);
    }

    public void Bt_OfflineCheck()
    {
        LocateMap(nowselectmapid);
        mapobj.Hide(false);
        Autofarmmanager.Instance.StartCheckOffline();
    }
    public List<MonDropDB.Row> dropitems(string dropid)
    {
        return MonDropDB.Instance.FindAll_id(dropid);
    }
}
