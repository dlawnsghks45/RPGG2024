using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public string monid;
    public Animator ani;
    public Color[] enemycolor;
    [SerializeField] EnemySpawnManager enemymanager;
    public Hpmanager hpmanager;
    public GameObject Hp;
    monsterDB.Row mondata;

    public float atk;
    public float crit;
    public int attackcount;
    public float attacktime = 1.5f;

    public enemyFM enemyNowType;

    public enum enemyFM
    {
        소환,
        기다림,
        죽음
    }

    public SpriteRenderer EnemySprite;

    public void SetMonster()
    {
        if (enemyNowType == enemyFM.소환)
        {
            EnemySprite.color = enemycolor[0];
            Hp.SetActive(true);
        }
        else if (enemyNowType == enemyFM.기다림)
        {
            Hp.SetActive(false);
            EnemySprite.color = enemycolor[1];
        }
        else if (enemyNowType == enemyFM.죽음)
        {
            EnemySprite.sprite = null;
        }
    }

    Sprite[] monimages;
    private static readonly int Nomove = Animator.StringToHash("nomove");

    public void RefreshMonster()
    {
        mondata = monsterDB.Instance.Find_id(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).monsterid);
        if (monid != mondata.id)
        {
            monid = mondata.id;

            string[] images;
            if (EnemySpawnManager.Instance.iseventspawned)
            {
                monsterDB.Row mondataevent = monsterDB.Instance.Find_id("4999");
                images = mondataevent.sprite.Split(';');
                monimages = new Sprite[images.Length];
            }
            else
            {
                images = mondata.sprite.Split(';');
                monimages = new Sprite[images.Length];
            }

            for (int i = 0; i < images.Length; i++)
            {
                monimages[i] = SpriteManager.Instance.GetSprite(images[i]);
            }

            atk = float.Parse(mondata.dmg);
            crit = float.Parse(mondata.crit);
        }

        if (atk == 0)
        {
            ani.SetTrigger(Nomove);
        }
//        Debug.Log("드랍체크");
        if (EnemySpawnManager.Instance.iseventspawned)
        {
            Debug.Log("이벤트 계산");
            mondropmanager.Instance.SetDropData("4999");
        }
        else
        
        {
         //   Debug.Log("드랍체2");
            mondropmanager.Instance.SetDropData(monid);
        }

        mondropmanager.Instance.SetEventDrop(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype);

        if (EnemySprite.sprite == null)
            EnemySprite.sprite = monimages[Random.Range(0, monimages.Length)];
        hpmanager.MaxHp = decimal.Parse(mondata.hp);
        hpmanager.CurHp = hpmanager.MaxHp;
    }

    [SerializeField] private RectTransform recthp;
    // ReSharper disable Unity.PerformanceAnalysis
    public void RefreshMonster(int stage)
    {
        mondata = monsterDB.Instance.Find_id(mapmanager.Instance.dungeonMpb[stage]);
        if (monid != mondata.id)
        {
            monid = mondata.id;
            string[] images = mondata.sprite.Split(';');
            monimages = new Sprite[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                monimages[i] = SpriteManager.Instance.GetSprite(images[i]);
            }
            EnemySprite.sprite = monimages[Random.Range(0, monimages.Length)];
            crit = float.Parse(mondata.crit);
        }
       

        if (atk == 0)
        {
            ani.SetTrigger(Nomove);
        }
        switch (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype)
        {
            case  "1":
                hpmanager.MaxHp = decimal.Parse(mondata.hp);
                atk = float.Parse(mondata.dmg);
                mondropmanager.Instance.SetDropData(monid);
                mondropmanager.Instance.SetEventDrop(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype);
                break;
            
            case  "10":
                hpmanager.MaxHp = Contentmanager.Instance.GetHp(decimal.Parse(mondata.hp) *1,Contentmanager.Instance.nowplayinglevel);
                atk = Contentmanager.Instance.GetAttack(float.Parse(mondata.dmg));

                mondropmanager.Instance.SetDropDataDropId(Contentmanager.Instance.dropid_playing[Contentmanager.Instance.nowplayinglevel]);
                mondropmanager.Instance.SetEventDrop(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype);
                break;
        }
        

        if (EnemySprite.sprite == null)
            EnemySprite.sprite = monimages[Random.Range(0, monimages.Length)];
        hpmanager.CurHp = hpmanager.MaxHp;
        LayoutRebuilder.ForceRebuildLayoutImmediate(recthp);
    }

    //이벤트몬스터
    [SerializeField] private string monevent = "4999";

    // ReSharper disable Unity.PerformanceAnalysis
    public void RefreshMonsterBoss()
    {
        switch (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype)
        {
            //사냥터
            case "0":
                mondata = monsterDB.Instance.Find_id(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage)
                    .monsterid);
                monid = mondata.id;

                string[] imagess;
                if (EnemySpawnManager.Instance.iseventspawned)
                {
                    monsterDB.Row mondataevent = monsterDB.Instance.Find_id("4999");
                    imagess = mondataevent.sprite.Split(';');
                    monimages = new Sprite[imagess.Length];
                }
                else
                {
                    imagess = mondata.sprite.Split(';');
                    monimages = new Sprite[imagess.Length];
                }

                for (int i = 0; i < imagess.Length; i++)
                {
                    monimages[i] = SpriteManager.Instance.GetSprite(imagess[i]);
                }

                if (EnemySprite.sprite == null)
                    EnemySprite.sprite = monimages[Random.Range(0, monimages.Length)];
                hpmanager.MaxHp = decimal.Parse(mondata.hp) * 5m;
                hpmanager.CurHp = hpmanager.MaxHp;
                //무력화 점수
                hpmanager.MaxBP = float.Parse(mondata.breakPoint);
                hpmanager.CurBP = hpmanager.MaxBP;
                mapmanager.Instance.breakadddmg = 1.2f;
                if (EnemySpawnManager.Instance.iseventspawned)
                {
                    mondropmanager.Instance.SetDropData("4999");
                }
                else
                {
                    mondropmanager.Instance.SetDropData(monid);
                }
                mondropmanager.Instance.SetEventDrop("0");
                atk = float.Parse(mondata.dmg) * 5;

                break;
            //던전
            case "1":
                mondata = monsterDB.Instance.Find_id(
                    mapmanager.Instance.dungeonMpb[^1]);
                monid = mondata.id;
                string[] images = mondata.sprite.Split(';');
                monimages = new Sprite[images.Length];
                for (int i = 0; i < images.Length; i++)
                {
                    monimages[i] = SpriteManager.Instance.GetSprite(images[i]);
                }

                if (EnemySprite.sprite == null)
                    EnemySprite.sprite = monimages[Random.Range(0, monimages.Length)];
                hpmanager.MaxHp = decimal.Parse(mondata.hp) * 5m;
                hpmanager.CurHp = hpmanager.MaxHp;
                //무력화 점수
                hpmanager.MaxBP = float.Parse(mondata.breakPoint);
                hpmanager.CurBP = hpmanager.MaxBP;
                mondropmanager.Instance.SetEventDrop("1");
                mondropmanager.Instance.SetDropData(monid);
                mapmanager.Instance.breakadddmg = float.Parse(mondata.breakadddmg);
                atk = float.Parse(mondata.dmg) * 5;
                break;
            //승급
            case "2":
                mondata = monsterDB.Instance.Find_id(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage)
                    .monsterid);
                monid = mondata.id;
                string[] imagesss = mondata.sprite.Split(';');
                monimages = new Sprite[imagesss.Length];
                for (int i = 0; i < imagesss.Length; i++)
                {
                    monimages[i] = SpriteManager.Instance.GetSprite(imagesss[i]);
                }

                if (EnemySprite.sprite == null)
                    EnemySprite.sprite = monimages[Random.Range(0, monimages.Length)];


                hpmanager.MaxHp = decimal.Parse(mondata.hp);
                hpmanager.CurHp = hpmanager.MaxHp;
                //무력화 점수
                hpmanager.MaxBP = float.Parse(mondata.breakPoint);
                hpmanager.CurBP = hpmanager.MaxBP;
                mondropmanager.Instance.SetDropData(monid);
                mapmanager.Instance.breakadddmg = float.Parse(mondata.breakadddmg);
                atk = float.Parse(mondata.dmg);

                break;
            case "3": //레이드
                
                mondata = monsterDB.Instance.Find_id(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage)
                    .monsterid);
                monid = mondata.id;
                if (EnemySprite.sprite == null)
                    EnemySprite.sprite = SpriteManager.Instance.GetSprite(mondata.sprite);
                
                
                hpmanager.MaxBP = float.Parse(mondata.breakPoint);
                hpmanager.CurBP = hpmanager.MaxBP;
                
                mapmanager.Instance.breakadddmg = float.Parse(mondata.breakadddmg);
                mondropmanager.Instance.SetDropData(monid);
                
                if (PlayerBackendData.Instance.nowstage.Equals("5031"))
                {
                    hpmanager.MaxHp = EliteRaid.Instance.GetHp();
                    hpmanager.CurHp = hpmanager.MaxHp;
                    atk = EliteRaid.Instance.GetAtk() *5;

                }
                else
                {
                    hpmanager.MaxHp = decimal.Parse(mondata.hp);
                    hpmanager.CurHp = hpmanager.MaxHp;
                    atk = float.Parse(mondata.dmg) * 5;
                }
              
               
                //무력화 점수
                break;
            case "4": //성물전쟁
                mondata = monsterDB.Instance.Find_id(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage)
                    .monsterid);
                monid = mondata.id;
                if (EnemySprite.sprite == null)
                    EnemySprite.sprite = SpriteManager.Instance.GetSprite(mondata.sprite);
                hpmanager.MaxHp = 0;
                hpmanager.CurHp = 0;
                //무력화 점수
                hpmanager.MaxBP = float.Parse(mondata.breakPoint);
                hpmanager.CurBP = hpmanager.MaxBP;

                mapmanager.Instance.breakadddmg = float.Parse(mondata.breakadddmg);
                //mondropmanager.Instance.SetDropData(monid);
                atk = float.Parse(mondata.dmg) * 5;
                break;
            case "7": //길드레이드
                mondata = monsterDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
                monid = mondata.id;
                if (EnemySprite.sprite == null)
                    EnemySprite.sprite = SpriteManager.Instance.GetSprite(mondata.sprite);
                hpmanager.MaxHp = decimal.Parse(mondata.hp);
                hpmanager.CurHp = hpmanager.MaxHp;
                //무력화 점수
                hpmanager.MaxBP = float.Parse(mondata.breakPoint);
                hpmanager.CurBP = hpmanager.MaxBP;

                mapmanager.Instance.breakadddmg = float.Parse(mondata.breakadddmg);
                //mondropmanager.Instance.SetDropData(monid);
                atk = 0;
                break;
            case "8": //훈련장
                mondata = monsterDB.Instance.Find_id(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage)
                    .monsterid);
                monid = mondata.id;
                //Debug.Log("모ㅓㄴ스터 데티ㅓㅇ" + mondata.id);
                if (EnemySprite.sprite == null)
                    EnemySprite.sprite = SpriteManager.Instance.GetSprite(mondata.sprite);
                hpmanager.MaxHp = 0;
                hpmanager.CurHp = 0;
                //무력화 점수
                hpmanager.MaxBP = float.Parse(mondata.breakPoint);
                hpmanager.CurBP = hpmanager.MaxBP;

                mapmanager.Instance.breakadddmg = float.Parse(mondata.breakadddmg);
                //mondropmanager.Instance.SetDropData(monid);
                atk = 0;

                break;
            case "9": //개미굴
                mondata = monsterDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
                monid = mondata.id;
                if (EnemySprite.sprite == null)
                    EnemySprite.sprite = SpriteManager.Instance.GetSprite(mondata.sprite);
                hpmanager.MaxHp = Antcavemanager.Instance.GetAntCaveHp();
                hpmanager.CurHp = hpmanager.MaxHp;
                //무력화 점수
                hpmanager.MaxBP = float.Parse(mondata.breakPoint);
                hpmanager.CurBP = hpmanager.MaxBP;

                mapmanager.Instance.breakadddmg = float.Parse(mondata.breakadddmg);
                //mondropmanager.Instance.SetDropData(monid);
                atk = 0;
                break;
            
            //콘텐츠
            case "10":
//                Debug.Log("여기몹임");
                mondata = monsterDB.Instance.Find_id(
                    mapmanager.Instance.dungeonMpb[^1]);
                monid = mondata.id;
                string[] images2 = mondata.sprite.Split(';');
                monimages = new Sprite[images2.Length];
                for (int i = 0; i < images2.Length; i++)
                {
                    monimages[i] = SpriteManager.Instance.GetSprite(images2[i]);
                }

                if (EnemySprite.sprite == null)
                    EnemySprite.sprite = monimages[Random.Range(0, monimages.Length)];
                hpmanager.MaxHp = Contentmanager.Instance.GetHp(decimal.Parse(mondata.hp) *10m,Contentmanager.Instance.nowplayinglevel);
////                Debug.Log("보스피는" + hpmanager.MaxHp);
                hpmanager.CurHp = hpmanager.MaxHp;
                //무력화 점수
                hpmanager.MaxBP = float.Parse(mondata.breakPoint);
                hpmanager.CurBP = hpmanager.MaxBP;
                mondropmanager.Instance.SetEventDrop("10");
                mondropmanager.Instance.SetDropDataDropId(Contentmanager.Instance.dropid_playing[Contentmanager.Instance.nowplayinglevel]);
                mapmanager.Instance.breakadddmg = float.Parse(mondata.breakadddmg);
               // Debug.Log("호호");
                atk = Contentmanager.Instance.GetAttack(float.Parse(mondata.dmg)) * 5f;
                break;
            case "11": //월드보스
                mondata = monsterDB.Instance.Find_id(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage)
                    .monsterid);
                monid = mondata.id;
                if (EnemySprite.sprite == null)
                    EnemySprite.sprite = SpriteManager.Instance.GetSprite(mondata.sprite);
                hpmanager.MaxHp = WorldBossManager.nowplaydata.maxhp;
                hpmanager.CurHp =  WorldBossManager.nowplaydata.curhp;
                //무력화 점수
                hpmanager.MaxBP = float.Parse(mondata.breakPoint);
                hpmanager.CurBP = hpmanager.MaxBP;
                mapmanager.Instance.breakadddmg = float.Parse(mondata.breakadddmg);
                //mondropmanager.Instance.SetDropData(monid);
                atk = float.Parse(mondata.dmg) * 5;
                break;
            case "12": //월드보스
                mondata = monsterDB.Instance.Find_id(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage)
                    .monsterid);
                monid = mondata.id;
                if (EnemySprite.sprite == null)
                    EnemySprite.sprite = SpriteManager.Instance.GetSprite(mondata.sprite);
                 hpmanager.MaxHp = PartyRaidBattlemanager.Instance.NowBossMaxHp;
                hpmanager.CurHp =  PartyRaidBattlemanager.Instance.NowBossMaxHp;
                //무력화 점수
                hpmanager.MaxBP = float.Parse(mondata.breakPoint);
                hpmanager.CurBP = hpmanager.MaxBP;
                mapmanager.Instance.breakadddmg = float.Parse(mondata.breakadddmg);
                //mondropmanager.Instance.SetDropData(monid);
                atk = float.Parse(mondata.dmg) * 5;
                break;
        }


        mapmanager.Instance.BreakStop.SetActive(false);
        mapmanager.Instance.BreakStop2.SetActive(false);
        mapmanager.Instance.BreakLock.SetActive(false);
        mapmanager.Instance.iscanbreak = true;
        mapmanager.Instance.isbreak = false;
        mapmanager.Instance.breaknewtime = float.Parse(mondata.breaknewstart);
        mapmanager.Instance.breaktime = float.Parse(mondata.breakTime);
        crit = float.Parse(mondata.crit);

        if (atk == 0)
        {
            ani.SetTrigger(Nomove);
        }
    }

    private void Update()
    {
        SetMonster();
    }
}
