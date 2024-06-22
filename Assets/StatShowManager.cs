using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatShowManager : MonoBehaviour
{
    public GameObject StatPanel;
    public Text StatName; //선택이름
    public Text StatNumber; //선택 수치

    public StatSlot[] StatSlots;
    public StatSlot[] StatSlots_Percent;


    public Text ItemStatStatText;
    public Text ItemStatPercentText;

    //0.물리 공격력
    //

    enum statenum
    {
        힘 =0,
        민첩 = 1,
        지능 =2,
        지혜 =3,
        물공 =4,
        마공 =5,
        상태이상 = 13,
        생명력 = 10,
        정신력 = 11,
        보스피해 = 16,
        치명타 = 14,
        치명타피해 = 15,
        공속 = 8,
        시속 = 9,
        재사용시간 = 17,
    }
    
    public void Bt_ShowStat(int stat)
    {
        decimal statnumber = 0;
        decimal statnumberPercent = 0;
        decimal statnumberGearWeaponPercent = 0;
        int playerlv = PlayerBackendData.Instance.GetLv();
        Player playerdata = Battlemanager.Instance.mainplayer;

        switch (stat)
        {
            case 0: //힘
                StatName.text = Inventory.GetTranslate("Stat/힘기본");
                StatNumber.text = playerdata.stat_str.ToString("N0");
                break;
            case 1: //민
                StatName.text = Inventory.GetTranslate("Stat/민첩기본");
                StatNumber.text = playerdata.stat_dex.ToString("N0");
                break;
            case 2: //지능
                StatName.text = Inventory.GetTranslate("Stat/지능기본");
                StatNumber.text = playerdata.stat_int.ToString("N0");
                break;
            case 3: //지헤
                StatName.text = Inventory.GetTranslate("Stat/지혜기본");
                StatNumber.text = playerdata.stat_wis.ToString("N0");
                break;
            case 4: //물공
                StatName.text = Inventory.GetTranslate("Stat/물리공격력기본");
                StatNumber.text = playerdata.stat_atk.ToString("N0");
                break;
            case 5: //마공
                StatName.text = Inventory.GetTranslate("Stat/마법공격력기본");
                StatNumber.text = playerdata.stat_matk.ToString("N0");
                break;
            case 8: //공속
                StatName.text = Inventory.GetTranslate("Stat/공격 속도기본");
                StatNumber.text = $"{playerdata.stat_atkspeed * 100f:N0}%";
                break;
            case 9: //시속
                StatName.text = Inventory.GetTranslate("Stat/시전속도기본");
                StatNumber.text = $"{playerdata.stat_castspeed:N0}%";
                break;
            case 10: //생명력
                StatName.text = Inventory.GetTranslate("Stat/생명력기본");
                StatNumber.text = playerdata.stat_hp.ToString("N0");
                break;
            case 11: //정신력
                StatName.text = Inventory.GetTranslate("Stat/정신력기본");
                StatNumber.text = playerdata.stat_mp.ToString("N0");
                break;
            case 13: //상태이상
                StatName.text = Inventory.GetTranslate("Stat/상태이상피해");
                StatNumber.text = $"{(playerdata.Stat_DotDmgUp * 100f):N0}%";
                break;
            case 14: //치명타
                StatName.text = Inventory.GetTranslate("Stat/치명타확률기본");
                StatNumber.text = $"{playerdata.stat_crit:N0}%";
                break;
            case 15: //치명타피해
                StatName.text = Inventory.GetTranslate("Stat/치명타피해기본");
                StatNumber.text = $"{(playerdata.stat_critdmg * 100f):N0}%";
                break;
            case 16: //보스 피해
                StatName.text = Inventory.GetTranslate("Stat/보스피해증가기본");
                StatNumber.text = $"{playerdata.stat_Bossdmg * 100f:N0}%";
                break;
            case 17: //재사용시간감소
                StatName.text = Inventory.GetTranslate("Stat/재사용시간 감소");
                StatNumber.text = $"{playerdata.Stat_ReduceCoolDown * 100f:N0}%";
                break;
        }


        StatPanel.SetActive(true);
        for (int i = 0; i < StatSlots.Length; i++)
        {
            StatSlots[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < StatSlots_Percent.Length; i++)
        {
            StatSlots_Percent[i].gameObject.SetActive(false);
        }

        int num = 0;
        int numP = 0;





        //캐릭터
        //힘민지혜
        ClassDB.Row classdata = ClassDB.Instance.Find_id(playerdata.Nowclass.ClassId1);
        switch (stat)
        {
            case 0: //힘
                if (float.Parse(classdata.strperlv) * playerlv != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_직업")
                        , "0",
                        (decimal)(float.Parse(classdata.strperlv) * playerlv), false,
                        false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(float.Parse(classdata.strperlv) * playerlv);
                    num++;
                }

                if ((decimal)playerdata.set_str != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_장비세트")
                        , "0",
                        (decimal)playerdata.set_str, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.set_str;
                    num++;
                }

                if ((decimal)playerdata.ability_strnum != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티 고정")
                        , "0",
                        (decimal)playerdata.ability_strnum, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.ability_strnum;
                    num++;
                }
                
                
                if ((decimal)playerdata.ability_allstatnum != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티 고정")
                        , "0",
                        (decimal)playerdata.ability_allstatnum, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.ability_allstatnum;
                    num++;
                }
                
                if ((decimal)playerdata.pet_allstatnum != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_펫")
                        , "0",
                        (decimal)playerdata.pet_allstatnum, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.pet_allstatnum;
                    num++;
                }
                
                
                //특수효과 (고정)
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.strup) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .strup), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .strup);
                    num++;
                }

                //특수효과
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.strperup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .strperup), true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .strperup) * 100m;
                    numP++;
                }

                //패시브
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.strperup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_패시브")
                        , "0",
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.strperup),
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.strperup) *
                        100m;
                    numP++;
                }

                //어빌리티
                if (playerdata.ability_str != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티")
                        , "0",
                        (decimal)playerdata.ability_str,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.ability_str *
                        100m;
                    numP++;
                }
                
                if (classdata.mainstat.Equals("str"))
                {
                    //탈리스만
                    if (playerdata.talisman_allstat != 0)
                    {
                        StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI8/탈리스만세트")
                            , "0",
                            (decimal)playerdata.talisman_allstat, true,
                            true);
                        StatSlots_Percent[numP].gameObject.SetActive(true);
                        statnumberPercent +=
                            (decimal)playerdata.talisman_allstat * 100m;
                        numP++;
                    }
                    
                    //장비 주 능력치
                    if ((decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat) != 0)
                    {
                        StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특효주능력치")
                            , "0",
                            (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat), false, false);
                        StatSlots[num].gameObject.SetActive(true);
                        statnumber += (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat);
                        num++;
                    }
                    
                    
                    //수집
                    if ((decimal)playerdata.Stat_collection != 0)
                    {
                        StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_수집")
                            , "0",
                            (decimal)playerdata.Stat_collection, false, false);
                        StatSlots[num].gameObject.SetActive(true);
                        statnumber += (decimal)playerdata.Stat_collection;
                        num++;
                    }
                    
                    if ((decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup) != 0)
                    {
                        StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특효주능력치")
                            , "0",
                            (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup), true,
                            true);
                        StatSlots_Percent[numP].gameObject.SetActive(true);
                        statnumberPercent +=
                            (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup) * 100m;
                        numP++;
                    }
                    
                    if ((decimal)playerdata.buff_Allstatper != 0)
                    {
                        StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_버프")
                            , "0",
                            (decimal)playerdata.buff_Allstatper, true,
                            true);
                        StatSlots_Percent[numP].gameObject.SetActive(true);
                        statnumberPercent +=
                            (decimal)playerdata.buff_Allstatper * 100m;
                        numP++;
                    }
                }
                break;
            case 1: //민
                if (float.Parse(classdata.dexperlv) * playerlv != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_직업")
                        , "0",
                        (decimal)(float.Parse(classdata.dexperlv) * playerlv), false,
                        false);

                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(float.Parse(classdata.dexperlv) * playerlv);
                    num++;
                }

                if ((decimal)playerdata.set_dex != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_장비세트")
                        , "0",
                        (decimal)playerdata.set_dex, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.set_dex;
                    num++;
                }
                
                if ((decimal)playerdata.ability_dexnum != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티 고정")
                        , "0",
                        (decimal)playerdata.ability_dexnum, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.ability_dexnum;
                    num++;
                }
                
                
                if ((decimal)playerdata.ability_allstatnum != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티 고정")
                        , "0",
                        (decimal)playerdata.ability_allstatnum, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.ability_allstatnum;
                    num++;
                }
                
                if ((decimal)playerdata.pet_allstatnum != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_펫")
                        , "0",
                        (decimal)playerdata.pet_allstatnum, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.pet_allstatnum;
                    num++;
                }
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.dexup) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .dexup), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .dexup);
                    num++;
                }

                //특수효과
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.dexperup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.dexperup),
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.dexperup) *
                        100m;
                    numP++;
                }

                //패시브
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dexperup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_패시브")
                        , "0",
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dexperup), true,
                        true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dexperup) * 100m;
                    numP++;
                }
                
                //어빌리티
                if (playerdata.ability_dex != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티")
                        , "0",
                        (decimal)playerdata.ability_dex,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.ability_dex *
                        100m;
                    numP++;
                }

                if (classdata.mainstat.Equals("dex"))
                {
                    //탈리스만
                    if (playerdata.talisman_allstat != 0)
                    {
                        StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI8/탈리스만세트")
                            , "0",
                            (decimal)playerdata.talisman_allstat, true,
                            true);
                        StatSlots_Percent[numP].gameObject.SetActive(true);
                        statnumberPercent +=
                            (decimal)playerdata.talisman_allstat * 100m;
                        numP++;
                    }
                    
                    //장비 주 능력치
                    if ((decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat) != 0)
                    {
                        StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특효주능력치")
                            , "0",
                            (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat), false, false);
                        StatSlots[num].gameObject.SetActive(true);
                        statnumber += (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat);
                        num++;
                    }
                    
                    
                    //수집
                    if ((decimal)playerdata.Stat_collection != 0)
                    {
                        StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_수집")
                            , "0",
                            (decimal)playerdata.Stat_collection, false, false);
                        StatSlots[num].gameObject.SetActive(true);
                        statnumber += (decimal)playerdata.Stat_collection;
                        num++;
                    }
                    
                        
                    if ((decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup) != 0)
                    {
                        StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특효주능력치")
                            , "0",
                            (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup), true,
                            true);
                        StatSlots_Percent[numP].gameObject.SetActive(true);
                        statnumberPercent +=
                            (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup) * 100m;
                        numP++;
                    }
                    
                    if ((decimal)playerdata.buff_Allstatper != 0)
                    {
                        StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_버프")
                            , "0",
                            (decimal)playerdata.buff_Allstatper, true,
                            true);
                        StatSlots_Percent[numP].gameObject.SetActive(true);
                        statnumberPercent +=
                            (decimal)playerdata.buff_Allstatper * 100m;
                        numP++;
                    }
                }
                break;
            case 2: //지
                if (float.Parse(classdata.intperlv) * playerlv != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_직업")
                        , "0",
                        (decimal)(float.Parse(classdata.intperlv) * playerlv), false,
                        false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(float.Parse(classdata.intperlv) * playerlv);
                    num++;
                }

                if ((decimal)playerdata.set_int != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_장비세트")
                        , "0",
                        (decimal)playerdata.set_int, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.set_int;
                    num++;
                }
                
                    
                if ((decimal)playerdata.ability_intnum != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티 고정")
                        , "0",
                        (decimal)playerdata.ability_intnum, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.ability_intnum;
                    num++;
                }


                if ((decimal)playerdata.ability_allstatnum != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티 고정")
                        , "0",
                        (decimal)playerdata.ability_allstatnum, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.ability_allstatnum;
                    num++;
                }
                
                if ((decimal)playerdata.pet_allstatnum != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_펫")
                        , "0",
                        (decimal)playerdata.pet_allstatnum, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.pet_allstatnum;
                    num++;
                }
                
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.intup) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .intup), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .intup);
                    num++;
                }

                //특수효과
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.intperup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.intperup),
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.intperup) *
                        100m;
                    numP++;
                }

                //패시브
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.intwisperup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_패시브")
                        , "0",
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.intwisperup),
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.intwisperup) *
                        100m;
                    numP++;
                }
                //어빌리티
                if (playerdata.ability_int != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티")
                        , "0",
                        (decimal)playerdata.ability_int,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.ability_int *
                        100m;
                    numP++;
                }
                
                
                if (classdata.mainstat.Equals("int"))
                {
                    //탈리스만
                    if (playerdata.talisman_allstat != 0)
                    {
                        StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI8/탈리스만세트")
                            , "0",
                            (decimal)playerdata.talisman_allstat, true,
                            true);
                        StatSlots_Percent[numP].gameObject.SetActive(true);
                        statnumberPercent +=
                            (decimal)playerdata.talisman_allstat * 100m;
                        numP++;
                    }
                   
                    
                    //장비 주 능력치
                    if ((decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat) != 0)
                    {
                        StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특효주능력치")
                            , "0",
                            (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat), false, false);
                        StatSlots[num].gameObject.SetActive(true);
                        statnumber += (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat);
                        num++;
                    }
                    
                    
                    //수집
                    if ((decimal)playerdata.Stat_collection != 0)
                    {
                        StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_수집")
                            , "0",
                            (decimal)playerdata.Stat_collection, false, false);
                        StatSlots[num].gameObject.SetActive(true);
                        statnumber += (decimal)playerdata.Stat_collection;
                        num++;
                    }
                    
                        
                    if ((decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup) != 0)
                    {
                        StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특효주능력치")
                            , "0",
                            (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup), true,
                            true);
                        StatSlots_Percent[numP].gameObject.SetActive(true);
                        statnumberPercent +=
                            (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup) * 100m;
                        numP++;
                    }
                    
                    if ((decimal)playerdata.buff_Allstatper != 0)
                    {
                        StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_버프")
                            , "0",
                            (decimal)playerdata.buff_Allstatper, true,
                            true);
                        StatSlots_Percent[numP].gameObject.SetActive(true);
                        statnumberPercent +=
                            (decimal)playerdata.buff_Allstatper * 100m;
                        numP++;
                    }
                }
                
                
                break;
            case 3: //지혜
                if (float.Parse(classdata.wisperlv) * playerlv != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_직업")
                        , "0",
                        (decimal)(float.Parse(classdata.wisperlv) * playerlv), false,
                        false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(float.Parse(classdata.wisperlv) * playerlv);
                    num++;
                }
                

                if ((decimal)playerdata.set_wis != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_장비세트")
                        , "0",
                        (decimal)playerdata.set_wis, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.set_wis;
                    num++;
                }

                
                if ((decimal)playerdata.ability_wisnum != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티 고정")
                        , "0",
                        (decimal)playerdata.ability_wisnum, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.ability_wisnum;
                    num++;
                }
                
                
                if ((decimal)playerdata.ability_allstatnum != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티 고정")
                        , "0",
                        (decimal)playerdata.ability_allstatnum, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.ability_allstatnum;
                    num++;
                }
                
                if ((decimal)playerdata.pet_allstatnum != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_펫")
                        , "0",
                        (decimal)playerdata.pet_allstatnum, false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)playerdata.pet_allstatnum;
                    num++;
                }
                
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.wisup) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .wisup), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .wisup);
                    num++;
                }

                //특수효과
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.wisperup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.wisperup),
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.wisperup) *
                        100m;
                    numP++;
                }

                //패시브
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.intwisperup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_패시브")
                        , "0",
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.intwisperup),
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.intwisperup) *
                        100m;
                    numP++;
                }  //어빌리티
                if (playerdata.ability_wis != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티")
                        , "0",
                        (decimal)playerdata.ability_wis,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.ability_wis *
                        100m;
                    numP++;
                }
                if (classdata.mainstat.Equals("wis"))
                {
                    //탈리스만
                    if (playerdata.talisman_allstat != 0)
                    {
                        StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI8/탈리스만세트")
                            , "0",
                            (decimal)playerdata.talisman_allstat, true,
                            true);
                        StatSlots_Percent[numP].gameObject.SetActive(true);
                        statnumberPercent +=
                            (decimal)playerdata.talisman_allstat * 100m;
                        numP++;
                    }
                    //장비 주 능력치
                    if ((decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat) != 0)
                    {
                        StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특효주능력치")
                            , "0",
                            (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat), false, false);
                        StatSlots[num].gameObject.SetActive(true);
                        statnumber += (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstat);
                        num++;
                    }
                    
                    
                    //수집
                    if ((decimal)playerdata.Stat_collection != 0)
                    {
                        StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_수집")
                            , "0",
                            (decimal)playerdata.Stat_collection, false, false);
                        StatSlots[num].gameObject.SetActive(true);
                        statnumber += (decimal)playerdata.Stat_collection;
                        num++;
                    }
                    
                        
                    if ((decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup) != 0)
                    {
                        StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특효주능력치")
                            , "0",
                            (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup), true,
                            true);
                        StatSlots_Percent[numP].gameObject.SetActive(true);
                        statnumberPercent +=
                            (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.allstatperup) * 100m;
                        numP++;
                    }
                    
                    
                    if ((decimal)playerdata.buff_Allstatper != 0)
                    {
                        StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_버프")
                            , "0",
                            (decimal)playerdata.buff_Allstatper, true,
                            true);
                        StatSlots_Percent[numP].gameObject.SetActive(true);
                        statnumberPercent +=
                            (decimal)playerdata.buff_Allstatper * 100m;
                        numP++;
                    }
                }
                break;

            case 4: //물리 공격력
                
                //기본 100%
                StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_기본상승")
                    , "0",
                    (decimal)1, true,
                    true,true);
                StatSlots_Percent[numP].gameObject.SetActive(true);
                statnumberPercent +=
                    100m;
                numP++;
                
                
                if (playerdata.stat_str * 8 != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_물리힘")
                        , "0",
                        (decimal)(playerdata.stat_str * 8), false,
                        false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(playerdata.stat_str * 7);
                    num++;
                }

                if (playerdata.stat_dex * 12 != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_물리민첩")
                        , "0",
                        (decimal)(playerdata.stat_dex * 12), false,
                        false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(playerdata.stat_dex * 12);
                    num++;
                }
                
                //세트
                if ((decimal)playerdata.buff_atkPercent != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_버프")
                        , "0",
                        (decimal)playerdata.buff_atkPercent, true, true,true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.buff_atkPercent * 100m;
                    numP++;
                }

                //세트
                if ((decimal)playerdata.set_atk != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_장비세트")
                        , "0",
                        (decimal)playerdata.set_atk, true, true,true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.set_atk * 100m;
                    numP++;
                }

                //특수효과
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.physicperup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.physicperup),
                        true, true,true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .physicperup) * 100m;
                    numP++;
                }

                //패시브
                if (Passivemanager.Instance.GetPassiveStatPercent(Passivemanager.PassiveStatEnum.critmeleedmgup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_패시브")
                        , "0",
                        (decimal)Passivemanager.Instance.GetPassiveStatPercent(Passivemanager.PassiveStatEnum.critmeleedmgup),
                        true, true,true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)Passivemanager.Instance.GetPassiveStatPercent(Passivemanager.PassiveStatEnum.critmeleedmgup) *
                        100m;
                    numP++;
                }

                
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.atkdmgup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_패시브")
                        , "0",
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.atkdmgup), true,
                        true,true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.atkdmgup) * 100m;
                    numP++;
                }

                //제단
                if ((decimal)playerdata.altarstat_atkmatk != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_제단")
                        , "0",
                        (decimal)playerdata.altarstat_atkmatk, true, true,true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.altarstat_atkmatk * 100m;

                    numP++;
                }
                
                //어빌리티
                if (playerdata.ability_atk != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티")
                        , "0",
                        (decimal)playerdata.ability_atk,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.ability_atk *
                        100m;
                    numP++;
                }
                
                //탈리스만
                if (playerdata.talisman_atk != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI8/탈리스만세트")
                        , "0",
                        (decimal)playerdata.talisman_atk,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.talisman_atk *
                        100m;
                    numP++;
                }

                break;
           case 5: //마법 공격력
                
                //기본 100%
                StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_기본상승")
                    , "0",
                    (decimal)1, true,
                    true);
                StatSlots_Percent[numP].gameObject.SetActive(true);
                statnumberPercent +=
                    100m;
                numP++;
                
                
                if (playerdata.stat_int * 12 != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_마법지능")
                        , "0",
                        (decimal)(playerdata.stat_int * 12), false,
                        false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(playerdata.stat_int * 12);
                    num++;
                }

                if (playerdata.stat_wis * 7 != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_마법지혜")
                        , "0",
                        (decimal)(playerdata.stat_wis * 7), false,
                        false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(playerdata.stat_wis * 7);
                    num++;
                }
                
                //세트
                if ((decimal)playerdata.buff_matkPercent != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_버프")
                        , "0",
                        (decimal)playerdata.buff_matkPercent, true, true,true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.buff_matkPercent * 100m;
                    numP++;
                }

                //세트
                if ((decimal)playerdata.set_matk != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_장비세트")
                        , "0",
                        (decimal)playerdata.set_matk, true, true,true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.set_matk * 100m;
                    numP++;
                }

                //특수효과
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.magicperup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.magicperup),
                        true, true,true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .magicperup) * 100m;
                    numP++;
                }

                //패시브
                if (Passivemanager.Instance.GetPassiveStatPercent(Passivemanager.PassiveStatEnum.matkdmgup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_패시브")
                        , "0",
                        (decimal)Passivemanager.Instance.GetPassiveStatPercent(Passivemanager.PassiveStatEnum.matkdmgup),
                        true, true,true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)Passivemanager.Instance.GetPassiveStatPercent(Passivemanager.PassiveStatEnum.matkdmgup) *
                        100m;
                    numP++;
                }

                //제단
                if ((decimal)playerdata.altarstat_atkmatk != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_제단")
                        , "0",
                        (decimal)playerdata.altarstat_atkmatk, true, true,true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.altarstat_atkmatk * 100m;

                    numP++;
                }
                
                //어빌리티
                if (playerdata.ability_matk != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티")
                        , "0",
                        (decimal)playerdata.ability_matk,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.ability_matk*
                        100m;
                    numP++;
                }
                
                //탈리스만
                if (playerdata.talisman_matk != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI8/탈리스만세트")
                        , "0",
                        (decimal)playerdata.talisman_matk,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.talisman_matk *
                        100m;
                    numP++;
                }

                break;
                
            case 8: //공속
                if ((decimal)playerdata.set_atkspeed != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_장비세트")
                        , "0",
                        (decimal)playerdata.set_atkspeed, true, false);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.set_atkspeed;
                    numP++;
                }

                //패시브
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.atkspdup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_패시브")
                        , "0",
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.atkspdup),
                        true, true,true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.atkspdup) *
                        100m;
                    numP++;
                }
                 
                //어빌리티
                if (playerdata.ability_atkspeed != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티")
                        , "0",
                        (decimal)playerdata.ability_atkspeed,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.ability_atkspeed *
                        100m;
                    numP++;
                }
                break;
            case 9: //시속
                if ((decimal)playerdata.set_castspeed != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_장비세트")
                        , "0",
                        (decimal)playerdata.set_castspeed, true, false);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.set_castspeed;
                    numP++;
                }

                break;
            case 10: //hp
                
                //기본 100%
                StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_기본상승")
                    , "0",
                    (decimal)1, true,
                    true);
                StatSlots_Percent[numP].gameObject.SetActive(true);
                statnumberPercent +=
                    100m;
                numP++;
                
                if ((decimal)(float.Parse(classdata.Hp)) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_생명레벨")
                        , "0",
                        (decimal)(float.Parse(classdata.Hp)), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(float.Parse(classdata.Hp));
                    num++;
                }
                if ((decimal)(float.Parse(classdata.hpperlv) * playerlv) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_생명레벨")
                        , "0",
                        (decimal)(float.Parse(classdata.hpperlv) * playerlv), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(float.Parse(classdata.hpperlv) * playerlv);
                    num++;
                }
                
                if ((decimal)(playerdata.stat_str * 60) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_생명힘")
                        , "0",
                        (decimal)(playerdata.stat_str * 60), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(playerdata.stat_str * 60);
                    num++;
                }
                if ((decimal)(playerdata.stat_dex * 20) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_생명민첩")
                        , "0",
                        (decimal)(playerdata.stat_dex * 20), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(playerdata.stat_dex * 20);
                    num++;
                }
                if ((decimal)(playerdata.stat_int * 15) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_생명지능")
                        , "0",
                        (decimal)(playerdata.stat_int * 15), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(playerdata.stat_int * 15);
                    num++;
                }
                if ((decimal)(playerdata.stat_wis * 15) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_생명지혜")
                        , "0",
                        (decimal)(playerdata.stat_wis * 15), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(playerdata.stat_wis * 15);
                    num++;
                }
                
                if ((decimal)playerdata.set_hp != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_장비세트")
                        , "0",
                        (decimal)playerdata.set_hp, true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.set_hp * 100m;
                    numP++;
                }
                
                //특수효과
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.maxhpupper) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.maxhpupper),
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .maxhpupper) * 100m;
                    numP++;
                }

                //특수효과
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legendaxehp) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_고유특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legendaxehp),
                        true, true,true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberGearWeaponPercent +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .legendaxehp) * 100m;
                    numP++;
                }
                Debug.Log(Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.hpperup));
                //패시브
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.hpperup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_패시브")
                        , "0",
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.hpperup),
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.hpperup) *
                        100m;
                    numP++;
                }
                
                //길드
                if ((decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.생명력증가) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_길드버프")
                        , "0",
                        (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.생명력증가), true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.생명력증가) * 100m;
                    numP++;
                }
                
                //어빌리티
                if (playerdata.ability_hp != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티")
                        , "0",
                        (decimal)playerdata.ability_hp,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.ability_hp *
                        100m;
                    numP++;
                }
                break;
            case 11: //mp
               
                //기본 100%
                StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_기본상승")
                    , "0",
                    (decimal)1, true,
                    true);
                StatSlots_Percent[numP].gameObject.SetActive(true);
                statnumberPercent +=
                    100m;
                numP++;
                
                if ((decimal)(float.Parse(classdata.Mp)) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_정신레벨")
                        , "0",
                        (decimal)(float.Parse(classdata.Mp)), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(float.Parse(classdata.Mp));
                    num++;
                }
                if ((decimal)(float.Parse(classdata.mpperlv) * playerlv) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_정신레벨")
                        , "0",
                        (decimal)(float.Parse(classdata.mpperlv) * playerlv), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(float.Parse(classdata.mpperlv) * playerlv);
                    num++;
                }
                
                if ((decimal)(playerdata.stat_int * 10) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_정신지능")
                        , "0",
                        (decimal)(playerdata.stat_int * 10), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(playerdata.stat_int * 10);
                    num++;
                }
                if ((decimal)(playerdata.stat_wis * 30) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_정신지혜")
                        , "0",
                        (decimal)(playerdata.stat_wis * 30), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += (decimal)(playerdata.stat_wis * 30);
                    num++;
                }
                
                if ((decimal)playerdata.set_mp != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_장비세트")
                        , "0",
                        (decimal)playerdata.set_mp, true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.set_mp * 100m;
                    numP++;
                }
                
                //특수효과
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.maxmpupper) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.maxmpupper),
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .maxmpupper) * 100m;
                    numP++;
                }
                
                //길드
                if ((decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.정신력증가) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_길드버프")
                        , "0",
                        (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.정신력증가), true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.정신력증가) * 100m;
                    numP++;
                }
                //어빌리티
                if (playerdata.ability_mp != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티")
                        , "0",
                        (decimal)playerdata.ability_mp,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.ability_mp *
                        100m;
                    numP++;
                }
                break;
            case 13: //상태이상
                
                if ((decimal)(playerdata.Stat_AddStack) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_상태 이상 중첩")
                        , "0",
                        (decimal)(playerdata.Stat_AddStack), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    num++;
                }
                Debug.Log(playerdata.Stat_MaxDotCount);
                if ((decimal)(playerdata.Stat_MaxDotCount) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_상태 이상 최대 중첩")
                        , "0",
                        (decimal)(playerdata.Stat_MaxDotCount), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    num++;
                }
                
                if ((decimal)(playerdata.Stat_MaxDotCount) != 0)
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_상태 이상 최대 중첩")
                        , "0",
                        (decimal)(playerdata.Stat_MaxDotCount), false, false);
                    StatSlots[num].gameObject.SetActive(true);
                    num++;
                }
                
               
                decimal dmg = (decimal)((playerdata.stat_str + playerdata.stat_int + (playerdata.stat_wis * 1.8f)) * 12);
                dmg = dmg + (dmg * (decimal)playerdata.Stat_DotDmgUp);
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                    dmg *= 4m;
                    
                StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_상태 이상출혈")
                    , "0",
                    dmg, false, false);
                StatSlots[num].gameObject.SetActive(true);
                num++;
                
                //화상 힘 * 지능 * 지혜 * 20
                dmg = (decimal)((playerdata.stat_str + playerdata.stat_int + (playerdata.stat_wis * 1.8f)) * 12);
                dmg = dmg + (dmg * (decimal)playerdata.Stat_DotDmgUp);
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                    dmg *= 4m;
                    
                StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_상태 이상화상")
                    , "0",
                    dmg, false, false);
                StatSlots[num].gameObject.SetActive(true);
                num++;
                
                //화상 민첩 * 지능 * 지혜 * 20
                dmg = (decimal)(playerdata.stat_dex + playerdata.stat_int + (playerdata.stat_wis * 1.8f)) * 12;
                dmg = dmg + (dmg * (decimal)playerdata.Stat_DotDmgUp);
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                    dmg *= 4m;
                    
                StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_상태 이상감전")
                    , "0",
                    dmg, false, false);
                StatSlots[num].gameObject.SetActive(true);
                num++;
                
                //화상 민첩 * 지능 * 지혜 * 20
                dmg = (decimal)(playerdata.stat_dex + playerdata.stat_int + (playerdata.stat_wis * 1.8f)) * 12;
                dmg = dmg + (dmg * (decimal)playerdata.Stat_DotDmgUp);
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                    dmg *= 4m;
                    
                StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_상태 이상독")
                    , "0",
                    dmg, false, false);
                StatSlots[num].gameObject.SetActive(true);
                num++;
                
              
                dmg = (decimal)(playerdata.stat_str + playerdata.stat_dex + playerdata.stat_int +
                                (playerdata.stat_wis * 1.8f)) * 15;
                dmg = dmg + (dmg * (decimal)playerdata.Stat_DotDmgUp);
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                    dmg *= 4m;
                    
                StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_상태 이상죽음")
                    , "0",
                    dmg, false, false);
                StatSlots[num].gameObject.SetActive(true);
                num++;
                
                dmg = (decimal)(playerdata.stat_str + playerdata.stat_dex + playerdata.stat_int +
                                (playerdata.stat_wis * 1.8f)) * 17;
                dmg = dmg + (dmg * (decimal)playerdata.Stat_DotDmgUp);
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdouble) != 0)
                    dmg *= 4m;
                    
                StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_상태 이상절명")
                    , "0",
                    dmg, false, false);
                StatSlots[num].gameObject.SetActive(true);
                num++;

                
                if ((decimal)(playerdata.stat_wis * 0.00001f) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_지혜상태이상")
                        , "0",
                        (decimal)(playerdata.stat_wis * 0.00001f), true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)(playerdata.stat_wis * 0.00001f) * 100m;
                    numP++;
                }
                
                if ((decimal)playerdata.set_DotDmgUp != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_장비세트")
                        , "0",
                        (decimal)playerdata.set_DotDmgUp, true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.set_DotDmgUp * 100m;
                    numP++;
                }
                //특수효과
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.dotperup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.dotperup),
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .dotperup) * 100m;
                    numP++;
                }

                //패시브
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdmgup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_패시브")
                        , "0",
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdmgup),
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.dotdmgup) *
                        100m;
                    numP++;
                }
                
                if (playerdata.ability_dotdmgup != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티")
                        , "0",
                        (decimal)playerdata.ability_dotdmgup,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.ability_dotdmgup *
                        100m;
                    numP++;
                }
                break;
            case 14: //치명타
                StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_기본치명타확률")
                    , "0",
                    5, true, false);
                StatSlots_Percent[numP].gameObject.SetActive(true);
                statnumberPercent += 5m;
                numP++;

                if ((decimal)playerdata.set_crit != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_장비세트")
                        , "0",
                        (decimal)playerdata.set_crit, true, false);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.set_crit;
                    numP++;
                }
                
                
                
                //특수효과
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.critper) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.critper),
                        true, false);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .critper);
                    numP++;
                }
                
                //패시브
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.critup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_패시브")
                        , "0",
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.critup),
                        true, false);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.critup);
                    numP++;
                }
                //스킬버프
                if ((decimal)playerdata.buff_crit != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_버프")
                        , "0",
                        (decimal)playerdata.buff_crit, true, false);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.buff_crit;
                    numP++;
                }
                //길드
                if ((decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.치명타확률증가) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_길드버프")
                        , "0",
                        (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.치명타확률증가), true, false);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.치명타확률증가);
                    numP++;
                }
  
                if (playerdata.ability_crit != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티")
                        , "0",
                        (decimal)playerdata.ability_crit,
                        true, false);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.ability_crit;
                    numP++;
                }
                
                //탈리스만
                if ((decimal)playerdata.talisman_crit != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI8/탈리스만세트")
                        , "0",
                        (decimal)playerdata.talisman_crit, true, false);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.talisman_crit;
                    numP++;
                }
                
                
                break;
            case 15: //치명타 피해
                StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_기본치명타피해")
                    , "0",
                    1.5m, true, true);
                StatSlots_Percent[numP].gameObject.SetActive(true);
                statnumberPercent += 150m;
                numP++;

                if ((decimal)playerdata.set_critdmg != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_장비세트")
                        , "0",
                        (decimal)playerdata.set_critdmg, true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.set_critdmg * 100m;
                    numP++;
                }
                
                
                //특수효과
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.critdmgper) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.critdmgper),
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .critdmgper) * 100m;
                    numP++;
                }
                //고유특수효과
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.legendaxecritdmg) != 0)
                {
                    float critdmgslayer = (playerdata.stat_hp / 100000) *
                                          equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                                              .legendaxecritdmg);
                    
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_고유특수효과")
                        , "0",
                        (decimal)critdmgslayer,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)critdmgslayer * 100m;
                    numP++;
                }
                //패시브
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.critdmgup) + Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.critmeleedmgup) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_패시브")
                        , "0",
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.critdmgup) + (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.critmeleedmgup),
                        true, true,true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)(Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.critdmgup) + 
                                  Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.critmeleedmgup)) *
                        100m;
                    numP++;
                }
                 
                
                //스킬버프
                if ((decimal)playerdata.buff_critdmg != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_버프")
                        , "0",
                        (decimal)playerdata.buff_critdmg, true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.buff_critdmg * 100m;
                    numP++;
                }
                //길드
                if ((decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.치명타피해증가) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_길드버프")
                        , "0",
                        (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.치명타피해증가), true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.치명타피해증가) * 100m;
                    numP++;
                }
                
                if (playerdata.ability_critdmg != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티")
                        , "0",
                        (decimal)playerdata.ability_critdmg,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.ability_critdmg *
                        100m;
                    numP++;
                }
                //탈리스만
                if ((decimal)playerdata.talisman_critdmg != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI8/탈리스만세트")
                        , "0",
                        (decimal)playerdata.talisman_critdmg, true, false);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.talisman_critdmg;
                    numP++;
                }
                break;
            case 16: //보스
                if ((decimal)playerdata.set_Bossdmg != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_장비세트")
                        , "0",
                        (decimal)playerdata.set_Bossdmg, true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.set_Bossdmg * 100m;
                    numP++;
                }
                //특수효과
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.bossadddmg) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_특수효과")
                        , "0",
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.bossadddmg),
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat
                            .bossadddmg) * 100m;
                    numP++;
                }

            
                //길드
                if ((decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.보스피해증가) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_길드버프")
                        , "0",
                        (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.보스피해증가), true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.보스피해증가) * 100m;
                    numP++;
                }
                
                if (playerdata.ability_bossdmg != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티")
                        , "0",
                        (decimal)playerdata.ability_bossdmg,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.ability_bossdmg *
                        100m;
                    numP++;
                }
                  
                if (playerdata.talisman_Bossdmg != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI8/탈리스만세트")
                        , "0",
                        (decimal)playerdata.talisman_Bossdmg,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.talisman_Bossdmg *
                        100m;
                    numP++;
                }
                break;
            case 17: //재사용시간감소
                if ((decimal)playerdata.set_cooldown != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_장비세트")
                        , "0",
                        (decimal)playerdata.set_cooldown, true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.set_cooldown * 100m;
                    numP++;
                }
                //패시브
                if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.reduceskillcooldown) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_패시브")
                        , "0",
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.reduceskillcooldown),
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.reduceskillcooldown) *
                        100m;
                    numP++;
                }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        //길드
                if ((decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.재사용시간감소) != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_길드버프")
                        , "0",
                        (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.재사용시간감소), true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.재사용시간감소) * 100m;
                    numP++;
                }
                
                   
                if (playerdata.ability_reducedmg != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_어빌리티")
                        , "0",
                        (decimal)playerdata.ability_reducedmg,
                        true, true);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent +=
                        (decimal)playerdata.ability_reducedmg *
                        100m;
                    numP++;
                }
                
                //탈리스만
                if ((decimal)playerdata.talisman_ReduceCooldown != 0)
                {
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI8/탈리스만세트")
                        , "0",
                        (decimal)playerdata.talisman_ReduceCooldown, true, false);
                    StatSlots_Percent[numP].gameObject.SetActive(true);
                    statnumberPercent += (decimal)playerdata.talisman_ReduceCooldown;
                    numP++;
                }
                break;
            
            
        }



        //업적 제단 수집
        if (stat is 0 or 1 or 2 or 3)
        {
            //기본 100%
            StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_기본상승")
                , "0",
                (decimal)1, true,
                true);
            StatSlots_Percent[numP].gameObject.SetActive(true);
            statnumberPercent +=
                100m;
            numP++;

            //모험랭크
            if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.allstatperup) != 0)
            {
                StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_패시브")
                    , "0",
                    (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.allstatperup), true,
                    true);
                StatSlots_Percent[numP].gameObject.SetActive(true);
                statnumberPercent +=
                    (decimal)Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.allstatperup) * 100m;
                numP++;
            }

            //모험랭크
            if (playerdata.AdventureRank() != 0)
            {
                StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_모험랭크")
                    , "0",
                    (decimal)playerdata.AdventureRank(), true, true);
                StatSlots_Percent[numP].gameObject.SetActive(true);
                statnumberPercent += (decimal)playerdata.AdventureRank() * 100m;
                numP++;
            }

            //제단
            if ((decimal)playerdata.altarstat_strdexintwis != 0)
            {
                StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_제단")
                    , "0",
                    (decimal)playerdata.altarstat_strdexintwis, false, false);
                StatSlots[num].gameObject.SetActive(true);
                statnumber += (decimal)playerdata.altarstat_strdexintwis;

                num++;
            }

            //업적
            if ((decimal)playerdata.achstat != 0)
            {
                StatSlots[num].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_업적")
                    , "0",
                    (decimal)playerdata.achstat + 20, false, false);
                StatSlots[num].gameObject.SetActive(true);
                statnumber += (decimal)playerdata.achstat + 20;

                num++;
            }

          




            //길드
            if ((decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.모든능력치증가) != 0)
            {
                StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_길드버프")
                    , "0",
                    (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.모든능력치증가), true, true);
                StatSlots_Percent[numP].gameObject.SetActive(true);
                statnumberPercent +=
                    (decimal)MyGuildManager.Instance.GetBuffStat(MyGuildManager.GuildBuffEnum.모든능력치증가) * 100m;
                numP++;
            }

            //엘리의 축복
            if (PlayerBackendData.Instance.ispremium)
            {
                StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate("UI4/스탯보기_엘리의축복")
                    , "0",
                    15m, true, false, true);
                StatSlots_Percent[numP].gameObject.SetActive(true);
                statnumberGearWeaponPercent += 15m;
            }

           
        }

        //장비
        bool isstat = stat is 4 or 5 or 6 or 7 or 8 or 9 or 10 or 11 or 13 or 14 or 15 or 16 or 17 ? true : false;
        bool ishundred = stat is 4 or 5 or 6 or 7 or 8 or 9 or 10 or 11 or 13 or 15 or 16 or 17 ? true : false;
        for (int i = 0; i < PlayerBackendData.Instance.EquipEquiptment0.Length; i++)
        {
            if (PlayerBackendData.Instance.EquipEquiptment0[i] != null)
            {
                decimal stats = (decimal)PlayerBackendData.Instance.EquipEquiptment0[i].GetStat(stat);

                //적응형 가져오기
                if (stat is 0 or 1 or 2 or 3)
                {
                    switch (classdata.mainstat)
                    {
                        case "str":
                            if (stat != 0)
                                break;
                            stats += (decimal)PlayerBackendData.Instance.EquipEquiptment0[i].GetStat(12);
                       //     Debug.Log("힘");
                            break;
                        case "dex":
                            if (stat != 1)
                                break;
                            stats += (decimal)PlayerBackendData.Instance.EquipEquiptment0[i].GetStat(12);
//                            Debug.Log("민첩");

                            break;
                        case "int":
                            if (stat != 2)
                                break;
                            stats += (decimal)PlayerBackendData.Instance.EquipEquiptment0[i].GetStat(12);
                          //  Debug.Log("지능");

                            break;
                        case "wis":
                            if (stat != 3)
                                break;
                            stats += (decimal)PlayerBackendData.Instance.EquipEquiptment0[i].GetStat(12);
                          //  Debug.Log("지혜");

                            break;
                    }
                }

                if (stats.Equals(-135878)) continue;
                if (stats <= 0) continue;

//                Debug.Log(stats);
                if (isstat)
                {
                    //%다 
                    StatSlots_Percent[numP].RefreshShow(Inventory.GetTranslate(EquipItemDB.Instance
                            .Find_id(PlayerBackendData.Instance.EquipEquiptment0[i].Itemid).Name)
                        , PlayerBackendData.Instance.EquipEquiptment0[i].Itemrare,
                        (decimal)stats, isstat, ishundred);
                    StatSlots_Percent[numP].gameObject.SetActive(true);

                    if (ishundred)
                    {
                        if (stat is 4 or 5)
                        {
                            statnumberGearWeaponPercent += stats * 100m;
                        }
                        else
                        {
                            statnumberPercent += stats * 100m;
                        }
                    }
                    else
                    {
                        statnumberPercent += stats;
                    }

                    numP++;
                }
                else
                {
                    StatSlots[num].RefreshShow(Inventory.GetTranslate(EquipItemDB.Instance
                            .Find_id(PlayerBackendData.Instance.EquipEquiptment0[i].Itemid).Name)
                        , PlayerBackendData.Instance.EquipEquiptment0[i].Itemrare,
                        (decimal)stats, isstat, ishundred);
                    StatSlots[num].gameObject.SetActive(true);
                    statnumber += stats;
                    num++;
                }

            }
        }





        ItemStatStatText.text = statnumber.ToString("N0");

        if (stat is 0 or 1 or 2 or 3)
        {
            if (statnumberGearWeaponPercent != 0)
            {
                ItemStatPercentText.text = $"{statnumberPercent:N0}% (<color=yellow>{statnumberGearWeaponPercent:N0})%</color>";
            }
            else
            {
                ItemStatPercentText.text = $"{statnumberPercent:N0}%";

            }
        }
        else
        {
            if (statnumberGearWeaponPercent != 0)
            {
                ItemStatPercentText.text = $"{statnumberGearWeaponPercent:N0}% (<color=yellow>{statnumberPercent:N0})%</color>";
            }
            else
            {
                ItemStatPercentText.text = $"{statnumberPercent:N0}%";
            }
        }
      
    }

    
    
    public Vector2 smallrect;
    public Vector2 bigrect;
    public Vector2 smallachor;
    public Vector2 bigchor;
    public RectTransform DpsPanel;

    public void Bt_ChangeBig()
    {
        DpsPanel.sizeDelta = bigrect;
        DpsPanel.anchoredPosition = bigchor;
    }

    public void Bt_ChangeSmall()
    {
        DpsPanel.sizeDelta = smallrect;
        DpsPanel.anchoredPosition = smallachor;
    }
    
    
    
}
