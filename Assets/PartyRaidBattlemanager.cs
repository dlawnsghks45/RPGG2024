using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyRaidBattlemanager : MonoBehaviour
{
    //싱글톤만들기.
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


    private PartyRaidBoss battledata;
    
    //파티레이드 시작
    public void StartPartyRaid()
    {
        PartyRaidRoommanager.Instance.partyroomdata.isstart = true;
        PartyRaidRoommanager.Instance.RefreshPartyData();
        PartyRaidRoommanager.Instance.PartyStartObj.SetActive(true);
        int[] playerbuff = new int[(int)PartyRaidBuffEnemy.Length];
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
                enemybuff[6] = 0;
                break;
            case 2:
                enemybuff[0] = 0;
                enemybuff[1] = 0;
                enemybuff[2] = 0;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 0;
                break;
            case 3:
                enemybuff[0] = 1;
                enemybuff[1] = 0;
                enemybuff[2] = 0;
                enemybuff[3] = 1;
                enemybuff[4] = 1;
                enemybuff[5] = 1;
                enemybuff[6] = 0;
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
        battledata =
            new PartyRaidBoss(MapDB.Instance.Find_id(PartyRaidRoommanager.Instance.partyroomdata.nowmap).monsterid,PartyRaidRoommanager.Instance.GetMaxHp(),
                enemybuff, playerbuff);
        InitRaid();
        
    }
    
    public Image MonImage;
    public Image BackGround;
    public Image MonHpbar;
    public Text MonHp;
    public Text MonHpPercent;
    public penaltyslot[] PenaltySlots;
    [SerializeField] private partyraidmiddlebossslot[] middlebosspanel;
    void InitRaid()
    {
        MonImage.sprite = SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id(battledata.monid).sprite);
        BackGround.sprite = SpriteManager.Instance.GetSprite(MapDB.Instance.Find_id(PartyRaidRoommanager.Instance.partyroomdata.nowmap).maplayer0);
        MonHpbar.fillAmount = (float)(battledata.curhp / battledata.maxhp) * 100f;
        MonHp.text = $"{dpsmanager.convertNumber(battledata.curhp)}";
        MonHpPercent.text = $"{((float)(battledata.curhp / battledata.maxhp) * 100f):N2}%";
        RefreshBossPenaly();
    }

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
    }

    public void RemovePenaly(int num)
    {
        battledata.MonsterBuff[num] = -1; //-1는 비활성화된것이다.
        RefreshBossPenaly();
    }

    public void RefreshPlayerBuff()
    {
        
    }
    
    public void Bt_ShowPenaltyData()
    {
        
    }
    
    public void RefreshBattlePanel()
    {
        
    }
}