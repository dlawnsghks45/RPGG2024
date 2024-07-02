using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class PartyRaidBattlemanager : MonoBehaviour
{
    //�̱��游���.
    private static PartyRaidBattlemanager _instance = null;



    
    public static PartyRaidBattlemanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(PartyRaidBattlemanager)) as PartyRaidBattlemanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    public List<string> RecentUser = new List<string>();
    //��Ƽ���̵� ����
    public PartyRaidBoss battledata;
    public void StartPartyRaid()
    {
        PartyRaidRoommanager.Instance.ClearAllJoinUser();
        LeaderReward.SetActive(false);
        MemberReward.SetActive(false);
        PartyRaidRoommanager.Instance.partyroomdata.isstart = true;
        PartyRaidRoommanager.Instance.RefreshPartyData();
        PartyRaidRoommanager.Instance.PartyStartObj.SetActive(true);
        int[] playerbuff = new int[(int)PartyRaidBuffPlayer.Length];
        int[] enemybuff= new int[(int)PartyRaidBuffEnemy.Length];
        switch (PartyRaidRoommanager.Instance.partyroomdata.level)
        {
            case 1:
                enemybuff[0] = 0;
                enemybuff[1] = 0;
                enemybuff[2] = 0;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 1;
                break;
            case 2:
                enemybuff[0] = 0;
                enemybuff[1] = 0;
                enemybuff[2] = 0;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 1;
                break;
            case 3:
                enemybuff[0] = 1;
                enemybuff[1] = 0;
                enemybuff[2] = 0;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 1;
                break;
            case 4:
                enemybuff[0] = 1;
                enemybuff[1] = 1;
                enemybuff[2] = 0;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 1;
                break;
            case 5:
                enemybuff[0] = 1;
                enemybuff[1] = 1;
                enemybuff[2] = 1;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 1;
                break;
            case 6:
                enemybuff[0] = 1;
                enemybuff[1] = 1;
                enemybuff[2] = 1;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 1;
                break;
            case 7:
                enemybuff[0] = 1;
                enemybuff[1] = 1;
                enemybuff[2] = 1;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 1;
                break;
        }
        PartyRaidRoommanager.Instance.BuffInfo(enemybuff);
        battledata =
            new PartyRaidBoss(MapDB.Instance.Find_id(PartyRaidRoommanager.Instance.partyroomdata.nowmap).monsterid,PartyRaidRoommanager.Instance.GetMaxHp(),
                enemybuff, playerbuff);

        for (int i = 0; i < MainBossBloodObj.Length; i++)
        {
            MainBossBloodObj[i].SetActive(false);
        }   
        
        InitRaid();
        PartyRaidRoommanager.Instance.ExitButton.SetActive(true);
        PartyRaidRoommanager.Instance.PartyradPanel.Show(false);
        ShowChatPanels();
    }

    public GameObject[] ObjWindow;
    public GameObject ObjWindowAlert;
    public Text ObjWindowAlertText;

    public bool isbattle;
    public void ShowAdsPanels()
    {
//        Debug.Log("ä�ò�");
        ObjWindow[0].SetActive(true); //����
        ObjWindow[1].SetActive(false); //ä��
        ObjWindowAlert.SetActive(false);
        ObjWindowAlertText.text = "";
        nowalertnum = 0;
        isbattle = false;
    }
    public void ShowChatPanels()
    {
        Debug.Log("ä�ü�");
        ObjWindow[0].SetActive(false); //����
        ObjWindow[1].SetActive(true); //ä��
        ObjWindowAlert.SetActive(false);
        ObjWindowAlertText.text = "";
        nowalertnum = 0;
        isbattle = true;
    }

    public UIToggle PartyRaidLobbyToggle;
    public int nowalertnum = 0;
    public void RemoveAlert()
    {
        //��Ƽ���̵� ��
        ObjWindowAlert.SetActive(false);
        ObjWindowAlertText.text = "";
        nowalertnum = 0;
    }

    public void CountAlert()
    {
        if (PartyRaidLobbyToggle.IsOn)
        {
            RemoveAlert();
        }
        else
        {
            //��Ƽ���̵� ��
            ObjWindowAlert.SetActive(true);
            nowalertnum++;
            ObjWindowAlertText.text = nowalertnum.ToString();
        }
     
    }

    public int[] nowPenalty = new int[(int)PartyRaidBuffEnemy.Length];
    public decimal NowBossMaxHp = 0;
    public int nowmiddlenum = 0;
    
    
    public Image MonImage;
    public Image BackGround;
    public Image MonHpbar;
    public Text MonHp;
    public Text MonHpPercent;
    public penaltyslot[] PenaltySlots;
    public partyraidmiddlebossslot[] middlebosspanel;
    public float buffmax = 0;
    public int buffusernum = 0;
    void InitRaid()
    {
        MonImage.sprite = SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id(battledata.monid).sprite);
        BackGround.sprite = SpriteManager.Instance.GetSprite(MapDB.Instance.Find_id(PartyRaidRoommanager.Instance.partyroomdata.nowmap).maplayer0);
        MonHpbar.fillAmount = (float)(battledata.curhp / battledata.maxhp);
        MonHp.text = $"{dpsmanager.convertNumber(battledata.curhp)}";
        MonHpPercent.text = $"{((float)(battledata.curhp / battledata.maxhp) * 100f):N1}%";
        string[] MiddleData = PartyRaidDB.Instance.Find_id(PartyRaidRoommanager.Instance.partyroomdata.nowmap).middlemap
            .Split(';');
        foreach (var VARIABLE in middlebosspanel)
        {
            VARIABLE.gameObject.SetActive(false);
        }
        for (int i = 0; i < MiddleData.Length; i++)
        {
            int ran = Random.Range(0, 2);
            middlebosspanel[i].SetData(MiddleData[i], 0, i);
            middlebosspanel[i].gameObject.SetActive(true);
        }

        buffmax = PartyRaidRoommanager.Instance.PartyMember[0].BuffPercent;
        buffusernum = 0;
        for (var index = 0; index < PartyRaidRoommanager.Instance.PartyMember.Length; index++)
        {
            var t = PartyRaidRoommanager.Instance.PartyMember[index];
            if (t.data != null)
            {
                t.RefreshPlayerBuff();
                t.BufferObj.SetActive(false);
                if (buffmax < t.BuffPercent)
                {
                    buffmax = t.BuffPercent;
                    buffusernum = index;
                }
            }
        }
        if (buffmax > 0)
        {
            PartyRaidRoommanager.Instance.PartyMember[buffusernum].BufferObj.SetActive(true);
            PartyRaidRoommanager.Instance.PartyMember[buffusernum].BufferText.text =
                string.Format(Inventory.GetTranslate("UI7/����"),(buffmax*100f).ToString("N0"));
        }
        RefreshBossPenaly();
        ShowChatPanels();
    }

    public Text TryCountText;
    public void RefreshBossPenaly()
    {
        for (int i = 0; i < battledata.MonsterBuff.Length; i++)
        {
            if (battledata.MonsterBuff[i] != 0)
            {
                PenaltySlots[i].gameObject.SetActive(true);
                if (battledata.MonsterBuff[i] == -1)
                {
                    PenaltySlots[i].BuffOff.SetActive(true);
                }
                else
                {
                    PenaltySlots[i].BuffOff.SetActive(false);
                }
            }
            else
            {
                PenaltySlots[i].gameObject.SetActive(false);
            }
        }

        TryCountText.text = string.Format(Inventory.GetTranslate("UI7/��������Ƚ��"), battledata.BossAttackcount, 1);
        if (battledata.BossAttackcount > 0)
        {
            TryCountText.color = Color.white;
        }
        else
        {
            TryCountText.color = Color.red;
        }
    }

    public void RemovePenaly(int num)
    {
        battledata.MonsterBuff[num] = -1; //-1�� ��Ȱ��ȭ�Ȱ��̴�.
        RefreshBossPenaly();
    }

    public void Bt_StartMainBoss()
    { 
        if (PartyRaidBattlemanager.Instance.battledata.BossAttackcount <= 0)
        {
            Debug.Log("���� ���� Ƚ���� �����մϴ�.");
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/����Ƚ������"), alertmanager.alertenum.����);
            return;
        }
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/����͸�����"), alertmanager.alertenum.����);
            return;
        }
        
        if (mapmanager.Instance.islocating)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/���̵��ߺҰ�"),alertmanager.alertenum.����);
            return;
        }
        
        if (PartyRaidRoommanager.Instance.PartyMember[PartyRaidRoommanager.Instance.mypartynum].BattleObj.activeSelf)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/���̵�������"),alertmanager.alertenum.����);
            return;
        }
        PartyraidChatManager.Instance.Chat_MainBossRaidStart();
    }
    
    
    
    
    public GameObject[] MainBossBloodObj;
    public Text[] MainBossBloodText;
    public RectTransform[] MainBossBloodRect;


    public void AttackBossDmgSet(decimal dmg,string nick)
    {
        battledata.DmgPlayer.Add(dmg);
        battledata.DmgPlayerNickname.Add(nick);
        decimal curhp = battledata.maxhp;

        for (int i = 0; i < battledata.DmgPlayerNickname.Count; i++)
        {
            MainBossBloodObj[i].SetActive(true);
            decimal Percent = (battledata.DmgPlayer[i] / battledata.maxhp);
            float width = 640 * (float)Percent;
            MainBossBloodRect[i].sizeDelta = new Vector2 (width, 12);
            MainBossBloodText[i].text = $"{battledata.DmgPlayerNickname[i]}\n{Percent*100m:N1}%";
            curhp -= battledata.DmgPlayer[i];
            if (curhp <= 0)
            {
                curhp = 0;
                ShowReward();
                
                MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
                if (mapdata_Now.maptype != "0")
                {
                    //����ͷ� �����̵�
                    PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
                    mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
                    alertmanager.Instance.ShowAlert2(Inventory.GetTranslate("UI8/�ھ�����"),alertmanager.alertenum.�Ϲ�);
                    return;
                }
                
                break;
            }
        }

        MonHpbar.fillAmount = (float)(curhp / battledata.maxhp) ;
        MonHp.text = $"{dpsmanager.convertNumber(curhp)}";
        MonHpPercent.text = $"{((float)(curhp / battledata.maxhp) * 100f):N1}%";
    }

    public GameObject LeaderReward;
    public GameObject MemberReward;
    public Text RewardTextName;
    public void ShowReward()
    {
        if (PartyRaidRoommanager.Instance.PartyMember[0].data.nickname == PlayerBackendData.Instance.nickname)
        {
            //����â
            LeaderReward.SetActive(true);
            MemberReward.SetActive(false);
            RewardTextName.text = string.Format(Inventory.GetTranslate("UI7/���ĿϷ�"),
                Inventory.GetTranslate(MapDB.Instance.Find_id(PartyRaidRoommanager.Instance.partyroomdata.nowmap).name));
        }
        else
        {
            //��Ƽ��â
            LeaderReward.SetActive(false);
            MemberReward.SetActive(true);

        }
    }

    public void Bt_GiveReward()
    {
        PartyraidChatManager.Instance.Chat_GiveReward(
            PartyRaidRoommanager.Instance.DropId[PartyRaidRoommanager.Instance.partyroomdata.level - 1]);
    }

    public GameObject RewardShowPanel;


    public string rewardid;
    public void ShowRewardPanel(string dropid)
    {
        RewardShowPanel.SetActive(true);
        rewardid = dropid;
    }
    
    
    public Text RaidFinishText;
    public Image monsterimage_raidfinish;
    public GameObject raidrewardpanel;
    public itemiconslot[] reward_raidfinish;
    public GameObject effect_raidfinish;
    public List<string> dungeondropsid = new List<string>();
    public List<int> dungeondropshowmany = new List<int>();

    readonly WaitForSeconds waits = new WaitForSeconds(0.2f);

    public string nowlevel;
    public void Bt_StartGiveReward()
    {
        RewardShowPanel.SetActive(false);

        if (Timemanager.Instance.ConSumeCount_WeeklyAscny((int)Timemanager.ContentEnumWeekly.��Ƽ���̵��ְ�Ƚ��,
                1))
        {
            List<MonDropDB.Row> dropdatas = MonDropDB.Instance.FindAll_id(rewardid);
            dungeondropsid.Clear();
            dungeondropshowmany.Clear();
            foreach (var t in dropdatas)
            {
                //                Debug.Log(dropid[i]);
                Random.InitState((int)Time.deltaTime + PlayerBackendData.Instance.GetRandomSeed());
                int Ran_rate = Random.Range(0, 1000000); // 1,000,000�� 100%�̴�.
                if (Ran_rate <= int.Parse(t.rate)) //mondropmanager.Instance.getpercent(percent[i]))
                {
                    int Howmany = Random.Range(int.Parse(t.minhowmany), int.Parse(t.maxhowmany));
                    Inventory.Instance.AddItem(t.itemid, Howmany, false, true);
                    RaidManager.Instance.AddLoot(t.itemid, Howmany);
                    dungeondropsid.Add(t.itemid);
                    dungeondropshowmany.Add(Howmany);
                }
            }
            LogManager.Log_ClearRaid(PartyRaidRoommanager.Instance.partyroomdata.level.ToString());
            StartCoroutine(FinishRaidReward2());
        }
        else
        {
            if (PlayerBackendData.Instance.CheckItemAndRemove("30",1))
            {
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/�����ǻ��"), alertmanager.alertenum.�Ϲ�);
                List<MonDropDB.Row> dropdatas = MonDropDB.Instance.FindAll_id(rewardid);
                dungeondropsid.Clear();
                dungeondropshowmany.Clear();
                foreach (var t in dropdatas)
                {
                    //                Debug.Log(dropid[i]);
                    Random.InitState((int)Time.deltaTime + PlayerBackendData.Instance.GetRandomSeed());
                    int Ran_rate = Random.Range(0, 1000000); // 1,000,000�� 100%�̴�.
                    if (Ran_rate <= int.Parse(t.rate)) //mondropmanager.Instance.getpercent(percent[i]))
                    {
                        int Howmany = Random.Range(int.Parse(t.minhowmany), int.Parse(t.maxhowmany));
                        Inventory.Instance.AddItem(t.itemid, Howmany, false, true);
                        RaidManager.Instance.AddLoot(t.itemid, Howmany);
                        dungeondropsid.Add(t.itemid);
                        dungeondropshowmany.Add(Howmany);
                    }
                }
                LogManager.Log_ClearRaid(PartyRaidRoommanager.Instance.partyroomdata.level.ToString());
                StartCoroutine(FinishRaidReward2());
            }
            else
            {
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/���庸������Ƚ������"), alertmanager.alertenum.����);
            }
        }
    }

    private IEnumerator FinishRaidReward2()
    {
        RaidFinishText.text = string.Format(Inventory.GetTranslate("UI7/���ĿϷ�"),
            Inventory.GetTranslate(MapDB.Instance.Find_id(PartyRaidRoommanager.Instance.partyroomdata.nowmap).name));
        monsterimage_raidfinish.sprite =
            SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id(MapDB.Instance.Find_id(PartyRaidRoommanager.Instance.partyroomdata.nowmap).monsterid).sprite);

        foreach (var t in reward_raidfinish)
        {
            t.canvas.alpha =0;
            t.canvas.interactable = false;
        }
        effect_raidfinish.SetActive(false);
        raidrewardpanel.SetActive(true);
        yield return SpriteManager.Instance.GetWaitforSecond(0.25f);
        effect_raidfinish.SetActive(true);
        yield return new WaitForSeconds(0.95f);
        for (int i = 0; i < dungeondropsid.Count; i++)
        {
            reward_raidfinish[i].canvas.alpha = 1;
            reward_raidfinish[i].canvas.interactable = true;

            reward_raidfinish[i].Refresh(dungeondropsid[i], dungeondropshowmany[i], false, true);
            yield return waits;
        }
    }
}