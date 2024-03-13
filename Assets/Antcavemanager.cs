using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Doozy.Engine.UI;
using LitJson;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static PlayerBackendData;

public class Antcavemanager : MonoBehaviour
{
    //싱글톤만들기.
    private static Antcavemanager _instance = null;
    public static Antcavemanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Antcavemanager)) as Antcavemanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    public UIToggle autotoggle;
    public GameObject FinishButton;
    public UIView Panel;
    public Text AntLv;
    public GameObject Loading;
    public GameObject AntCaveObj;
    public void ShowAntCave()
    {
    
        
        Panel.Show(false);
        Loading.SetActive(true);
        AntCaveObj.SetActive(false);
        string[] selects =
        {
            "AntcaveLv"
        };
        //데이터를 받아온다.
        Where where1 = new Where();
        where1.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        var broj = Backend.GameData.GetMyData("RankData", where1,selects, 1);
        JsonData jdatas = null;
        if (broj.IsSuccess())
        {
           Debug.Log("랭킹데이터 불렁모" + broj);
           Debug.Log(broj.GetReturnValuetoJSON()["rows"].Count);

            if (broj.GetReturnValuetoJSON()["rows"].Count ==0)
            {
                return;
            }
                        
            jdatas = broj.FlattenRows()[0];
            
            if (jdatas.ContainsKey("AntcaveLv"))
            {
//                Debug.Log("개미굴랭킹가져옴"+jdatas["AntcaveLv"].ToString());
                
                PlayerBackendData.Instance.AntCaveLv = int.Parse(jdatas["AntcaveLv"].ToString());
                Loading.SetActive(false);
                AntCaveObj.SetActive(true);
                AntLv.text = $"{PlayerBackendData.Instance.AntCaveLv}F";
            }
            else
            {
                PlayerBackendData.Instance.AntCaveLv = 0;
                Loading.SetActive(false);
                AntCaveObj.SetActive(true);
                AntLv.text = $"{PlayerBackendData.Instance.AntCaveLv}F";
            }
        }
    }
        /*

    private void Start()
    {
        //아이템잉벗고
        if (PlayerBackendData.Instance.CheckItemCount("1714") <= 0)
        {
            //제단레벨이 
            if (PlayerBackendData.Instance.Altar_Lvs[3] < PlayerBackendData.Instance.AntCaveLv)
            {
                PlayerBackendData.Instance.Altar_Lvs[3] = PlayerBackendData.Instance.AntCaveLv;
                Savemanager.Instance.SaveInventory();
                Savemanager.Instance.SaveExpData();
                Savemanager.Instance.Save();
                Settingmanager.Instance.SaveDataALl(false);
            }

        }
    }
        */

    private bool canstart = true;

    public void Start_AntCave()
    {
        if (!Settingmanager.Instance.CheckServerOn())
        {
            return;
        }
        
        if (mapmanager.Instance.islocating)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/맵이동중불가"), alertmanager.alertenum.주의);
            return;
        }

        DateTime nowtime = DateTime.Now;

        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/사냥터만가능"), alertmanager.alertenum.주의);
            return;
        }
        if (nowtime.Hour >= 0 && nowtime.Hour < 6)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/성물파괴이용불가"), alertmanager.alertenum.일반);
            return;
        }
        if (!canstart)
        {
            return;
        }
        if (autotoggle.IsOn)
            FinishButton.SetActive(true);


        ids_save.Clear();
        howmany_save.Clear();
        savedlv = 0;
        mapmanager.Instance.LocateMap("9999");
    }


    public void Bt_FinishAuto()
    {
        FinishButton.gameObject.SetActive(false);
        autotoggle.IsOn = false;
    }
    public List<string> ids_save = new List<string>();
    public List<int> howmany_save = new List<int>();
    private int savedlv = 0;
    public void SaveReward()
    {
        AntCaveDB.Row data = AntCaveDB.Instance.Find_id(PlayerBackendData.Instance.AntCaveLv.ToString());
        string[] idsplit = data.rewardid.Split(';');
        string[] idhowmany = data.count.Split(';');
        for (int i = 0; i < idsplit.Length; i++)
        {
            if (ids_save.Contains(idsplit[i]))
            {
                int num = ids_save.IndexOf(idsplit[i]);
                howmany_save[num] += int.Parse(idhowmany[i]);
            }
            else
            {
                ids_save.Add(idsplit[i]);
                howmany_save.Add(int.Parse(idhowmany[i]));
            }
        }

        PlayerBackendData.Instance.AntCaveLv++;
        PlayerBackendData.Instance.AntTotalClear++;
        savedlv++;
    }

    public void GiveReward()
    {
        Param param= new Param
        {
            //가방
            { "AntTotalClear", PlayerBackendData.Instance.AntTotalClear.GetDecrypted() },
        };
        
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback2) =>
        {
            if (callback2.IsSuccess())
            {
                for (int i = 0; i < ids_save.Count; i++)
                {
                    Inventory.Instance.AddItem(ids_save[i], howmany_save[i]);
                }
                Savemanager.Instance.SaveInventory();
                Savemanager.Instance.Save();
                Inventory.Instance.ShowEarnItem3(ids_save.ToArray(),howmany_save.ToArray(),false);
                ids_save.Clear();
                howmany_save.Clear();
                RankingManager.Instance.RankInsert(PlayerBackendData.Instance.AntCaveLv.ToString(),RankingManager.RankEnum.개미굴);
                FinishButton.gameObject.SetActive(false);
                Settingmanager.Instance.SaveDataALl();
                LogManager.Log_CrystalEarn("개미굴");
            }
            else
            {
                PlayerBackendData.Instance.AntCaveLv -= savedlv;
                PlayerBackendData.Instance.AntTotalClear -= savedlv;

                FinishButton.gameObject.SetActive(false);
            }
        });
        
        PlayerBackendData.Instance.spawncount = mapmanager.Instance.savespawncount;
        mapmanager.Instance.LocateMap(mapmanager.Instance.savemapid);
    }
    
    public void GiveRewardAntCave()
    {
        if (ids_save.Count != 0)
        {
            Antcavemanager.Instance.SaveReward();       
            GiveReward();
            return;
        }
        AntCaveDB.Row data = AntCaveDB.Instance.Find_id(PlayerBackendData.Instance.AntCaveLv.ToString());

        PlayerBackendData.Instance.AntCaveLv++;
        PlayerBackendData.Instance.AntTotalClear++;

        List<string> ids = new List<string>();
        List<string> howmany = new List<string>();

        string[] idsplit = data.rewardid.Split(';');
        string[] idhowmany = data.count.Split(';');
        
        Param param= new Param
        {
            //가방
            { "AntcaveLv", PlayerBackendData.Instance.AntCaveLv.GetDecrypted() },
        };
        
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        
        ids_save.Clear();
        howmany_save.Clear();
        
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback2) =>
        {
            if (callback2.IsSuccess())
            {
                for (int i = 0; i < idsplit.Length; i++)
                {
                    ids.Add(idsplit[i]);
                    howmany.Add(idhowmany[i]);
//                    Debug.Log(ids[i]);

                    Inventory.Instance.AddItem(ids[i], int.Parse(howmany[i]));
                }
                Savemanager.Instance.SaveInventory();
                Savemanager.Instance.Save();
                Inventory.Instance.ShowEarnItem2(ids.ToArray(),howmany.ToArray(),false);
                FinishButton.gameObject.SetActive(false);
                RankingManager.Instance.RankInsert(PlayerBackendData.Instance.AntCaveLv.ToString(),RankingManager.RankEnum.개미굴);
                Settingmanager.Instance.SaveDataALl();
                LogManager.Log_CrystalEarn("개미굴");
            }
            
        });
    
    }
    public decimal GetAntCaveHp()
    {
        decimal a = (decimal)(3000000000 * math.pow(1.1, PlayerBackendData.Instance.AntCaveLv));
        return a;
    }

}

