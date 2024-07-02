using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnManager : MonoBehaviour
{
    public Camera  mainCameraAni;
    //�̱��游���.
    private static EnemySpawnManager _instance = null;
    public static EnemySpawnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(EnemySpawnManager)) as EnemySpawnManager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    public Transform[] SpawnNode;
    public Transform[] BossSpawnNode;
    public bool[] ishavemon = new bool[3];

    public Enemy[] enemys;
    public Enemy enemys_boss;
    public float showspeed = 3;

    //Ÿ���������� ��� �� �Ʒ���

    //���� ī��Ʈ
    public int spawnedmonstercur =0; //���� ��ȯ�� Ƚ��
    public int spawnedmonstermax =1; //�ִ� ��ȯ Ƚ��


    //�������� ����
    public GameObject StageObj;
    public pointslot First_Point;
    public pointslot[] Middle_Point;
    public pointslot Last_Point;



    public int NowStageindex = 0; //���� �������� ��ȣ
    public int NowMaxindex = 9; //���� �������� ��ȣ

    public void CloseStageObj()
    {
        StageObj.SetActive(false);
    }
    public void RefreshStage()
    {
        StageObj.SetActive(true);
        if (NowStageindex == 0)
        {
            //ó��
            First_Point.ShowPoint();
            foreach (var t in Middle_Point)
                t.ClosePoint();

            Last_Point.ClosePoint();
        }
        else if( NowStageindex != 0 && NowStageindex != NowMaxindex)
        {
            //�߰�
            First_Point.FinishPoint();
            foreach (var t in Middle_Point)
                t.ClosePoint();

            for (int i = 0; i < NowStageindex-1; i++)
                Middle_Point[i].FinishPoint();

            Last_Point.ClosePoint();
            Middle_Point[NowStageindex-1].ShowPoint();
        }
        else if(NowStageindex == NowMaxindex)
        {
            //������
            First_Point.FinishPoint();
            foreach (var t in Middle_Point)
                t.FinishPoint();

            Last_Point.ShowPoint();
           Invoke(nameof(bt_SpawnBoss),0.1f);
        }

    }
    

    public Enemy GetEnemy()
    {
        if (bossstage)
        {
            return enemys_boss;
        }
        else
        {
            return enemys[gettarget()];
        }
    }

   

    [Button(Name = "remove")]
    public void RemoveEnemy(int num)
    {
        if (bossstage)
        {
            //Debug.Log("����");
            Enemy enemy = enemys_boss;
            enemy.transform.position = BossSpawnNode[^1].position;
            bossstage = false;
            enemy.hpmanager.isdeath = true;
            enemy.EnemySprite.sprite = null;
            mapmanager.Instance.RefreshBoss();
        }
        else
        {
            Enemy enemy = enemys[num];
            enemy.hpmanager.isdeath = true;
            enemy.EnemySprite.sprite = null;
            enemy.transform.position = SpawnNode[^1].position;
        //    Debug.Log("�̵�" + num) ;
            //�����
                if (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype == "0")
                {
                    RefreshBossGauge();
                }
        }
    }

    public void RemoveEnemySpawn()
    {
       //isspawn = false;
        if (bossstage)
        {
            for(int i = 0; i < 3; i++)
            {
                Enemy enemy = enemys[i];

                enemy.transform.position = SpawnNode[^1].position;
                enemy.EnemySprite.sprite = null;

            }
        }
        else
        {
            Enemy enemy = enemys_boss;
            enemy.transform.position = BossSpawnNode[^1].position;
            enemy.EnemySprite.sprite = null;
        }
    }
    public void RemoveEnemySpawnAll()
    {
        Enemy enemy;
        for (int i = 0; i < 3; i++)
        {
            enemy = enemys[i];

            enemy.transform.position = SpawnNode[^1].position;
            enemy.EnemySprite.sprite = null;
        }

            enemy = enemys_boss;
            enemy.transform.position = BossSpawnNode[^1].position;
            enemy.EnemySprite.sprite = null;
        }

    private void Start()
    {
        spawnedmonstermax = -1;
    }
    public bool bossstage;
    [SerializeField]
    public bool isspawn =  false;
    public bool ischangelocate = false;

    private bool stagelockindex = false;

    void stageIndexFalse()
    {
        stagelockindex = false;
    }
    
    public void CheckFieldKillAll()
    {
        spawnedmonstercur--;

        if (spawnedmonstercur != 0) return;
        mondropmanager.Instance.checkDrop = false;
        if (!stagelockindex)
        {
            stagelockindex = true;
            NowStageindex++;
            Invoke(nameof(stageIndexFalse),0.3f);
        }
        isspawn = false;
        
    }

    public void CheckFieldKillAllDungeon()
    {
        spawnedmonstercur--;
        if (spawnedmonstercur != 0) return;
        mondropmanager.Instance.checkDrop = false;
        if (!stagelockindex)
        {
            stagelockindex = true;
            NowStageindex++;
            Invoke(nameof(stageIndexFalse), 1f);
        }

        isspawn = false;
        if (EnemySpawnManager.Instance.NowStageindex == EnemySpawnManager.Instance.NowMaxindex)
        {
            if (!DungeonManager.Instance.isautodungeon)
                PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;

        }
    }


    public void CheckFieldKillBoss()
    {
        spawnedmonstercur=0;
        if (spawnedmonstercur == 0)
        {
            //Debug.Log("�ϳ�");
            isspawn = false;
        }
//        Debug.Log("�����ߴ�");
        Savemanager.Instance.SaveFieldEnd();
    }

    public bool iseventspawned; //�̺�Ʈ üũ��true�� ��ȯ
    public void Update()
    {
        if (!EnemySpawnManager.Instance.isspawn && !ischangelocate)
        {
            if (PlayerBackendData.Instance.spawncount == 0)
                PlayerBackendData.Instance.spawncount = 1;

            RemoveEnemySpawn();

            if (bossstage)
            {
                enemys_boss.transform.position = Vector3.Lerp(enemys_boss.transform.position, BossSpawnNode[0].position, showspeed * Time.deltaTime);
                if (isspawn) return;
                if (GetDistance() < 0.05f)
                    isspawn = true;
                mapmanager.Instance.StartBossShow();
                enemys_boss.enemyNowType = Enemy.enemyFM.��ȯ;

                switch (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype)
                {
                    case "4"://��������
                        enemys_boss.hpmanager.CurHp = 1;
                        enemys_boss.RefreshMonsterBoss();
                        enemys_boss.hpmanager.ReSetDot();
                        enemys_boss.hpmanager.RefreshHp();
                        enemys_boss.hpmanager.isdeath = false;
                        break;
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "10":
                        enemys_boss.RefreshMonsterBoss();
                        enemys_boss.hpmanager.CurHp = enemys_boss.hpmanager.MaxHp;
                        enemys_boss.hpmanager.ReSetDot();
                        enemys_boss.hpmanager.RefreshHp();
                        enemys_boss.hpmanager.isdeath = false;
                        break;
                    case "7":
                      //  enemys_boss.hpmanager.CurHp = enemys_boss.hpmanager.MaxHp;
                        enemys_boss.RefreshMonsterBoss();
                        enemys_boss.hpmanager.ReSetDot();
                        enemys_boss.hpmanager.RefreshHp();
                        enemys_boss.hpmanager.isdeath = false;
                        break;
                    case "8":
                        //  enemys_boss.hpmanager.CurHp = enemys_boss.hpmanager.MaxHp;
                        enemys_boss.RefreshMonsterBoss();
                        enemys_boss.hpmanager.ReSetDot();
                        enemys_boss.hpmanager.RefreshHp();
                        enemys_boss.hpmanager.isdeath = false;
                        break;
                    case "9":
                        enemys_boss.RefreshMonsterBoss();
                        enemys_boss.hpmanager.ReSetDot();
                        enemys_boss.hpmanager.RefreshHp();
                        enemys_boss.hpmanager.isdeath = false;
                        break;
                    case "11":
                        //  enemys_boss.hpmanager.CurHp = enemys_boss.hpmanager.MaxHp;
                        enemys_boss.RefreshMonsterBoss();
                        enemys_boss.hpmanager.ReSetDot();
                        enemys_boss.hpmanager.RefreshHp();
                        enemys_boss.hpmanager.isdeath = false;
                        break;
                    case "12":
                        //  enemys_boss.hpmanager.CurHp = enemys_boss.hpmanager.MaxHp;
                        enemys_boss.RefreshMonsterBoss();
                        enemys_boss.hpmanager.ReSetDot();
                        enemys_boss.hpmanager.RefreshHp();
                        enemys_boss.hpmanager.isdeath = false;
                        break;
                }
            }
            else
            {
                if (isspawn) return;
                if (GetDistance() < 0.05f)
                {
                    isspawn = true;
                    spawnedmonstercur = PlayerBackendData.Instance.spawncount;
                }
                //������ ��ŭ ����
                for (int i = 0; i < PlayerBackendData.Instance.spawncount; i++)
                {
                    enemys[i].transform.position = Vector3.Lerp(enemys[i].transform.position, SpawnNode[i].position, showspeed * Time.deltaTime);
                    //spawnedmonstercur++;
                    enemys[i].enemyNowType = Enemy.enemyFM.��ȯ;
                    //Debug.Log(mapmanager.Instance.maptype);
                    switch (mapmanager.Instance.maptype)
                    {
                        case "0":
                            // Debug.Log("�����1");
                            //�����
                            enemys[i].RefreshMonster();
                            break;
                        case "1":
                        case "10":
                            //����
                            // Debug.Log("�����2");
                            enemys[i].RefreshMonster(NowStageindex);
                            break;
                    }
                    enemys[i].hpmanager.CurHp = enemys[i].hpmanager.MaxHp;
                    enemys[i].hpmanager.ReSetDot();
                    enemys[i].hpmanager.RefreshHp();
                    enemys[i].hpmanager.isdeath = false;
                }
            }
        }
        else if (ischangelocate)
        {
            //Debug.Log("�̵���");
            ReturnEnemy();
        }
    }


    public int nowtarget;

    private int gettarget()
    {
        for(int i = 0; i < enemys.Length;i++)
        {
            if (enemys[i].hpmanager.isdeath) continue;
            nowtarget = i;
            return i;
        }
        nowtarget = 0;
        return 0;
    }

    public Enemy[] gettarget(int count)
    {
        List<Enemy> enemyslist = new List<Enemy>();

        //�ִ� ġ ����
        if (bossstage)
        {
            enemyslist.Add(enemys_boss);
            nowtarget = 0;
            return enemyslist.ToArray();
        }

        if (count > 3)
            count = 3;



        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].hpmanager.isdeath) continue;
            nowtarget = i;
            enemyslist.Add(enemys[i]);
            if (enemyslist.Count == count)
                break;
        }

        nowtarget = 0;
        return enemyslist.ToArray();
    }

    public void ReturnEnemy()
    {
        isspawn = false;
        for(int i = 0; i < 3;i++)
        {
            enemys[i].transform.position = SpawnNode[^1].position;
        }
        enemys_boss.transform.position = BossSpawnNode[^1].position;
    }

    public float GetDistance()
    {
        float distance; 
        if(bossstage)
        {
            distance = Vector3.Distance(BossSpawnNode[0].transform.position, enemys_boss.transform.position);
        }
        else
        {
            int target = gettarget();
            distance = Vector3.Distance(SpawnNode[target].transform.position, enemys[target].transform.position);
        }
        //Debug.Log(distance);
        return distance;
    }

    public int bosscount;
    const int maxbosscount = 10;
    public GameObject Bossobj;
    [SerializeField]
    private int eventspawn;
    public void bt_SpawnBoss()
    {
        if (MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).maptype.Equals("0"))
        {
                //�̺�Ʈ ���� ���
                int ran = Random.Range(1, 101);
                if (ran <= eventspawn)
                {
                    EnemySpawnManager.Instance.iseventspawned = true;
                    mondropmanager.Instance.checkDrop = false;
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/�̺�Ʈ��������"),alertmanager.alertenum.�Ϲ�);
                }
        }
            //���� ��ȯ
            bossstage = true;
            isspawn = false;
            mapmanager.Instance.StartTimer(20);
            mapmanager.Instance.RefreshBoss();
    }

    //�±޸��ͼ�ȯ
    public void bt_SpawnAdventureBoss()
    {
        bossstage = true;
            isspawn = false;
        mapmanager.Instance.StartTimer(30);
        mapmanager.Instance.RefreshBoss();
    }

    public void bt_SpawnSingleRaidBoss()
    {
        bossstage = true;
        isspawn = false;
        mapmanager.Instance.StartTimer(60);
        mapmanager.Instance.RefreshBoss();
    }
    public void bt_SpawnAntCaveBoss()
    {
        bossstage = true;
        isspawn = false;
        mapmanager.Instance.StartTimer(30);
        mapmanager.Instance.RefreshBoss();
    }
    public void bt_SpawnGuildRaidBoss()
    {
        bossstage = true;
        isspawn = false;
        mapmanager.Instance.StartTimer(60);
        mapmanager.Instance.RefreshBoss();
    }
    //�����ı�
    public void bt_SpawnAltarBoss()
    {
        bossstage = true;
        isspawn = false;
        mapmanager.Instance.StartTimer(40);
        mapmanager.Instance.RefreshBoss();
    }
    public void bt_SpawnTrainingBoss()
    {
        bossstage = true;
        isspawn = false;
        mapmanager.Instance.StartTimer(dpsmanager.Instance.maxtime);
        mapmanager.Instance.RefreshBoss();
    }
    public void bt_SpawnWorldBoss()
    {
        bossstage = true;
        isspawn = false;
        mapmanager.Instance.StartTimer(60);
        mapmanager.Instance.RefreshBoss();
    }
    public void bt_SpawnPartyRaidBoss()
    {
        bossstage = true;
        isspawn = false;
        mapmanager.Instance.StartTimer(60);
        mapmanager.Instance.RefreshBoss();
    }
    private void RefreshBossGauge()
    {

       
    }
    public int savedcount = 3;
    public void ChageFieldCount(int num)
    {
        savedcount = num;
    }
}
