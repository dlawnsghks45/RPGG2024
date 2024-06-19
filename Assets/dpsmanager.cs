using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using Doozy.Engine.UI;
using LitJson;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;



public class dpsmanager : MonoBehaviour
{
    //????游???.
    private static dpsmanager _instance = null;

    public static dpsmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(dpsmanager)) as dpsmanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    public GameObject DPSButton;
//?????
    public Sprite[] spritedot;

    public bool isdpson;
    public bool isrec; //???????


    public float maxtime;
    public float breaktime;

    public void Settimer(float time)
    {
        maxtime = time;
    }



    //???????
    public decimal TotalDmg = 0;
    public decimal TotalDPSDmg = 0;
    public float second;

    //????? ?г?
    public Text TotalDmgText;
    public Text DPSDmgText;
    public Text TimeText;

    public void RefreshTime()
    {
        if (breaktime != 0)
        {
            TimeText.text = $"{mapmanager.Instance.Curtime:N0}\n<color=yellow><size=23>({breaktime:N0})</size></color>";
        }
        else
        {
            TimeText.text = mapmanager.Instance.Curtime.ToString("N0");
        }
    }



    //????? ?????
    public Dictionary<string, DPS> Dps = new Dictionary<string, DPS>();




    //????? ???
    public dpsslot[] dpsslots;
    public GameObject DPSPanel;


    public void StartDps()
    {
        ResetAlldps();
        DPSPanel.SetActive(true);
        isdpson = true;
        DPSButton.SetActive(true);
    }

    public GameObject savepanel;

    public void EndDps()
    {
        isdpson = false;
        DPSButton.SetActive(false);
        if (isrec)
        {
            //???? ?г? ????
            savepanel.SetActive(true);
        }
    }

    void ResetAlldps()
    {
        second = 0;
        TotalDmg = 0;
        TotalDPSDmg = 0;
        breaktime = 0;
        Dps.Clear();
        foreach (var VARIABLE in dpsslots)
        {
            VARIABLE.gameObject.SetActive(false);
        }

        TimeText.text = "0";
        DPSDmgText.text = "0";
        TotalDmgText.text = "0";
    }

    //??????
    public void AddDps(attacktype type, decimal dmg, string id,int atkcount)
    {
        if (type.Equals(attacktype.Length))
        {
            return;
        }

        if (second + breaktime == 0 && !Battlemanager.Instance.isbattle)
        {
            return;
        }

        TotalDmg += dmg;
        TotalDPSDmg = TotalDmg / (breaktime + second == 0 ? 1 : (decimal)(second + breaktime));
        TotalDmgText.text = convertNumber(TotalDmg); //  TotalDmg.ToString("N0");
        DPSDmgText.text = convertNumber(TotalDPSDmg); //.ToString("N0");
        if (Dps.ContainsKey(id))
        {
           
            //????? ????
            //??????? ??? 
            Dps[id].count++;
            Dps[id].countdivide = atkcount;
            Dps[id].totaldmg += decimal.Truncate(dmg); //???? ??? 
//            Debug.Log(id);
            if ((decimal)(second + breaktime) <= 0)
            {
                Dps[id].dps = decimal.Truncate(Dps[id].totaldmg);
            }
            else
            {
                Dps[id].dps = decimal.Truncate((Dps[id].totaldmg / (decimal)(second + breaktime)));

            }
            Dps[id].nowsecond = second;
        }
        else
        {
            DPS data = new DPS();
            data.id = id;
            data.totaldmg = decimal.Truncate(dmg); //???? ??????
            data.dps = decimal.Truncate((data.totaldmg / (breaktime + second == 0 ? 1 : (decimal)(second + breaktime))));
            data.type = type;
            data.count = 1; //1? ???????
            data.countdivide = atkcount;
            data.nowsecond = second;

            Dps.Add(id, data);
        }

        ShowDpsList(Dps, dpsslots);
    }

    public void ShowDpsList(Dictionary<string, DPS> DpsData, dpsslot[] dpsslotss)
    {

        DpsData = SortDictionary(DpsData); //??????? ??´?
        int num = 0;
        foreach (var VARIABLE in DpsData)
        {
            if (VARIABLE.Value.nowsecond.Equals(0))
                continue;
            dpsslotss[num].ShowDmg(VARIABLE.Value);
            dpsslotss[num].gameObject.SetActive(true);
            num++;
        }
    }

    public UIView Trainingobj;

    public void Bt_StartTraining()
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/사냥터만가능"), alertmanager.alertenum.주의);
            return;
        }

        if (mapmanager.Instance.islocating)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/맵이동중불가"), alertmanager.alertenum.주의);
            return;
        }

        TutorialTotalManager.Instance.CheckGuideQuest("training");

        if (maxtime.Equals(60))
        {
            isrec = true;
        }
        else
        {
            isrec = false;
        }

        Trainingobj.Hide(true);
        mapmanager.Instance.LocateMap("1999");
    }



    public static Dictionary<string, DPS> SortDictionary(Dictionary<string, DPS> dict)
    {
        // ?????????? ascending?? descending???? ????
        var sortVar = from item in dict
            orderby item.Value.totaldmg descending
            select item;

        return sortVar.ToDictionary(x => x.Key, x => x.Value);
    }
/*
 *
 *
 *public enum attacktype
    {
        ??????,
        ???????????,
        ???????????,
        ?н??,
        ???????,
        ??????,
        ??????,
        Length
    }
 * 
 */
    
    public enum attacktype
    {
        기본공격,
        물리스킬공격,
        마법스킬공격,
        특수효과,
        어빌리티,
        상태이상,
        패시브,
        Length
    }

    public static string convertNumber(decimal price)
    {
        
        
//        Debug.Log(price);
        if (price.Equals(0))
            return "";
        
        if (PlayerBackendData.Instance.settingdata.DmgCountNum == 0)
        {
            string num = $"{price:# #### #### #### #### #### #### ####}".TrimStart().Replace(" ", ",");

//        Debug.Log("?????" + price);
            //  Debug.Log("?????num" + num);
        
            string[] unit = new string[] {  " ", "A", "B", "C", "D", "E" ,"F" ,"G"};
            string[] str = num.Split(',');
            string result = "";
            int cnt = str.Length;
     
            for (int i = 0; i < str.Length; i++)
            {
                if (Convert.ToInt32(str[i]) != 0)
                {
                    result =  result + Convert.ToInt32(str[i]) + unit[cnt-1];
                }

                cnt--;
                if (cnt == str.Length-1)
                {
                    break;
                }
            }
            return result;
        }
        else  if (PlayerBackendData.Instance.settingdata.DmgCountNum == 1)
        {
            string num = $"{price:# #### #### #### #### #### #### ####}".TrimStart().Replace(" ", ",");

//        Debug.Log("?????" + price);
            //  Debug.Log("?????num" + num);
        
            string[] unit = new string[] {  " ", "만", "억", "조", "경", "해" ,"자" ,"양"};
            string[] str = num.Split(',');
            string result = "";
            int cnt = str.Length;
     
            for (int i = 0; i < str.Length; i++)
            {
                if (Convert.ToInt32(str[i]) != 0)
                {
                    result =  result + Convert.ToInt32(str[i]) + unit[cnt-1];
                }

                cnt--;
                if (cnt == str.Length-2)
                {
                    break;
                }
            }
            return result;
        }
        else
        {
            return price.ToString("N0");
        }
        return price.ToString("N0");
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

//???? ???? 
//???,????,????,???
//?? ?????,??? ??????? , ?ð???,????? ?ð???

    public void Bt_SaveDps()
    {
        Timemanager.Instance.RefreshNowTIme();
        PlayerBackendData userData = PlayerBackendData.Instance;
        //??????? 3?ð??? ??? ??????.
        //??? ????ð??? ???????? ?????.
        Param param = new Param
        {
            { "level_train", userData.GetLv() },
            { "NowClass_train", userData.ClassData[userData.ClassId] },
            { "level_adven_train", userData.GetAdLv() },
            { "Passive_train", userData.PassiveClassId },
            { "Playerstat_train", PlayerData.Instance.GetPlayerStatForSave() },
            { "EquipmentNow_train", userData.EquipEquiptment0 },
            { "nowPetData_train", userData.PetData[userData.nowPetid] },
            { "TrainDmg", Dps },
            { "TrainTotalDmg", Decimal.Truncate(TotalDmg) },
            { "TrainTime", maxtime },
            { "BreakTime", breaktime },
            { "DPSDmgText", TotalDPSDmg },
            { "TrainAbility", userData.Abilitys }
        };

        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
        {
            if (!callback.IsSuccess())
            {
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/?허수아비랭킹등록실패"), alertmanager.alertenum.일반);
                return;
            }

            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/?허수아비랭킹등록성공"), alertmanager.alertenum.일반);
            savepanel.gameObject.SetActive(false);
            RankingManager.Instance.RankInsert(TotalDmg.ToString(), RankingManager.RankEnum.허수아비60);
        });
    }
}


public class DPS
{
    public string id;
    public dpsmanager.attacktype type;
    public decimal dps;
    public decimal totaldmg;
    public int count;
    public int countdivide;
    public float nowsecond;

    public DPS(JsonData data)
    {
        id = data["id"].ToString();
        type = (dpsmanager.attacktype)int.Parse(data["type"].ToString());
        string dmg = data["dps"].ToString();
//        Debug.Log(total);
        // ReSharper disable once InterpolatedStringExpressionIsNotIFormattable
        totaldmg = decimal.Parse(data["totaldmg"].ToString(), System.Globalization.NumberStyles.Float);
        dps = decimal.Parse(dmg,System.Globalization.NumberStyles.Float);
        count = int.Parse(data["count"].ToString());
        if (data.ContainsKey("countdivide"))
        {
            countdivide =  int.Parse(data["countdivide"].ToString());
        }
        else
        {
            countdivide = count;
        }
        nowsecond = float.Parse(data["nowsecond"].ToString());
    }

    public DPS()
    {
    }
}

