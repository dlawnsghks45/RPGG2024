using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passivemanager : MonoBehaviour
{
    //싱글톤만들기.
    private static Passivemanager _instance = null;
    public static Passivemanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Passivemanager)) as Passivemanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    public Passiveslot[] passiveslots;

    public void Refresh()
    {
        for(int i = 0; i <  PlayerBackendData.Instance.PassiveClassId.Length;i++)
        {

            try
            {
                if (!string.IsNullOrEmpty(PlayerBackendData.Instance.PassiveClassId[i])
                    || PlayerBackendData.Instance.PassiveClassId[i] != null)
                { 
                    passiveslots[i].gameObject.SetActive(true);
                    passiveslots[i].Refresh(PlayerBackendData.Instance.PassiveClassId[i]);
                }
                else
                {
                    //Debug.Log("i는" + i); 
                    passiveslots[i].gameObject.SetActive(false);
                }
            }
            catch (Exception e)
            {
                passiveslots[i].gameObject.SetActive(false);
            }
          
        }

        //패시브 스탯 적용.

        for(int i = 0; i < passivestats.Length;i++)
        {
            passivestats[i] = 0;
            passivestatsPercent[i] = 0;
        }

        foreach (var t in PlayerBackendData.Instance.PassiveClassId)
        {
            try
            {
                if (string.IsNullOrEmpty(t) || t == "True")
                    continue;
            }
            catch (Exception e)
            {
                continue;
            }

            
////            Debug.Log(t);
            switch (PassiveDB.Instance.Find_id( t).subtype)
            {
                case "atkspdup":
                    passivestats[0] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[0] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "reduceskillcooldown":
                    passivestats[1] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[1] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "strperup":
                    passivestats[2] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[2] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "dexperup":
                    passivestats[3] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[3] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "intwisperup":
                    passivestats[4] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[4] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "hpperup":
                    passivestats[5] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[5] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "atkperup":
                    passivestats[6] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[6] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "dodgeup":
                    passivestats[7] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[7] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "matkperup":
                    passivestats[8] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[8] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "dotdmgup":
                    passivestats[9] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[9] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "critup":
                    passivestats[10] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[10] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "attackcountup":
                    passivestats[11] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[11] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "atkcooldown":
                    passivestats[12] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[12] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "allstatperup":
                    passivestats[13] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[13] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "critdmgup":
                    passivestats[14] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[14] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "atkcountup":
                    passivestats[15] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[15] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "atkdmgup":
                    passivestats[16] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[16] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "matkdmgup":
                    passivestats[17] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[17] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "dotstackup":
                    passivestats[18] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[18] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "hpattack":
                    passivestats[19] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[19] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "critmeleedmgup":
                    passivestats[20] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[20] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "atkrangeup":
                    passivestats[21] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[21] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "makdouble":
                    passivestats[22] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[22] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "dotmaxup":
                    passivestats[23] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[23] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "attackcountupandrange":
                    passivestats[24] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[24] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "dotdouble":
                    passivestats[25] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[25] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "breakdmgup":
                    passivestats[26] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[26] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "buffup":
                    passivestats[27] += float.Parse(PassiveDB.Instance.Find_id( t).value);
                    passivestatsPercent[27] += float.Parse(PassiveDB.Instance.Find_id( t).percent);
                    break;
                case "1030":
                    passivestats[16] += float.Parse(PassiveDB.Instance.Find_id( t).value); //물리공격력
                    passivestats[27] += float.Parse(PassiveDB.Instance.Find_id( t).percent); //버프효율ㅐ
                    break;
                case "1031":
                    passivestats[16] += float.Parse(PassiveDB.Instance.Find_id( t).value); //물리공격력
                    break;
                case "1032":
                    passivestats[15] += float.Parse(PassiveDB.Instance.Find_id( t).value); //평타 횟수
                    passivestats[28] += float.Parse(PassiveDB.Instance.Find_id( t).percent); //평타공격력
                    break;
                case "1033":
                    passivestats[14] += float.Parse(PassiveDB.Instance.Find_id( t).value); //치명타
                    passivestats[13] += float.Parse(PassiveDB.Instance.Find_id( t).percent); //모든 능력치
                    passivestats[15] += 1; //평타 횟수
                    break;
                case "1034":
                    passivestats[29] += float.Parse(PassiveDB.Instance.Find_id( t).value); //시전속도
                    passivestats[30] += float.Parse(PassiveDB.Instance.Find_id( t).percent); //마법피해 증가
                    break;
                case "1035":
                    passivestats[29] += float.Parse(PassiveDB.Instance.Find_id( t).value); //치명타
                    passivestats[31] += float.Parse(PassiveDB.Instance.Find_id( t).percent); //치명타
                    break;
            }
        }
        //다시계산
        PlayerData.Instance.RefreshPlayerstat();
    }

    private void Awake()
    {
        passivestats = new float[(int)PassiveStatEnum.Length];
        passivestatsPercent = new float[(int)PassiveStatEnum.Length];
    }

    private void Start()
    {
        Refresh();
    }
    public void SetPassive(string classid)
    {
         PlayerBackendData.Instance.PassiveClassId[int.Parse(ClassDB.Instance.Find_id(classid).tier) - 1] = classid;
        
        Refresh();
    }
    [SerializeField]
    private float[] passivestats = new float[(int)PassiveStatEnum.Length];
    [SerializeField]
    private float[] passivestatsPercent = new float[(int)PassiveStatEnum.Length];


    public float GetPassiveStat(PassiveStatEnum num)
    {
        return passivestats[(int)num];
    }
    public float GetPassiveStatPercent(PassiveStatEnum num)
    {
        return passivestatsPercent[(int)num];
    }
    public enum PassiveStatEnum
    {
        atkspdup,
        reduceskillcooldown,
        strperup,
        dexperup,
        intwisperup,
        hpperup,
        atkperup,
        dodgeup,
        matkperup,
        dotdmgup,
        critup,
        attackcountup,
        atkcooldown,
        allstatperup,
        critdmgup,
        atkcountup,
        atkdmgup,
        matkdmgup,
        dotstackup,
        hpattack,
        critmeleedmgup,
        atkrangeup,
        makdouble,
        dotmaxup,
        attackcountupandrange,
        dotdouble,
        breakdmgup,
        buffup,
        basicatkdmg, //기본공격
        castspeed, //시전속도
        magicdmg7,//7차 마법사
        dotexplosiondmg7,//7차 네크
        Length
    }


}
