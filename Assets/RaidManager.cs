using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public class RaidManager : MonoBehaviour
{
    private static RaidManager _instance = null;

    public static RaidManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(RaidManager)) as RaidManager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    public GameObject[] Minigame;
    public arrowpattern[] Arrows;
    public string raidmonsterid;
    public UIView RaidInfoPanel;
    public Image BossImage;
    public Text BossHp;
    public Text BossName;
    public itemiconslot[] reward;


    #region 화살표맞추기

    public List<int> arrow_randoms = new List<int>();

    public void StartArrowGame()
    {
        arrow_randoms.Clear();

        int rn = Random.Range(4, 9);


    }

    string nowselectmap;

    public void SelectMap(string mapid)
    {
        nowselectmap = mapid;
    }

    #endregion

    public void Bt_GoAdMap()
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

        if (!Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.레이드))
        {
            if(PlayerBackendData.Instance.CheckItemCount("200000") > 0) 
            {
                PlayerBackendData.Instance.RemoveItem("200000",1);
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/레이드입장권사용"), alertmanager.alertenum.일반);
                Savemanager.Instance.SaveInventory();
                Savemanager.Instance.Save();
            }
            else
            {
                return;
            }
        }

        dungeondropsid.Clear();
        dungeondropshowmany.Clear();
        mapmanager.Instance.LocateMap(nowselectmap);
    }


    public void Bt_SelectRaid(string mapid)
    {
        nowselectmap = mapid;
        monsterDB.Row data = monsterDB.Instance.Find_id(MapDB.Instance.Find_id(mapid).monsterid);
        raidmonsterid = data.id;
        BossName.text = Inventory.GetTranslate(data.name);
        BossHp.text = $"HP: {dpsmanager.convertNumber(decimal.Parse(data.hp))}";
        BossImage.sprite = SpriteManager.Instance.GetSprite(data.sprite);

        foreach (var t in reward)
        {
            t.gameObject.SetActive(false);
        }

        bool isdrop = false;
        int num = 0;
        
        SetDropData(data.dropid);

        for (int i = 0; i < Mon_DropItemIDboss.Count; i++)
        {
            reward[i].Refresh(Mon_DropItemIDboss[i],
                Mon_DropItemMinHowmanyboss[i], false);
            reward[i].gameObject.SetActive(true);
        }

        RaidInfoPanel.Show(false);
    }

    public UIView SotangObj;
    public InputField SotangInput;
    public int sotangcount = 1;
    public int maxsotangcount = 30;

    public void Bt_Plus()
    {
        if (sotangcount.Equals(maxsotangcount))
        {
            //최대
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/최대"), alertmanager.alertenum.일반);
            return;
        }

        sotangcount++;
        SotangInput.text = sotangcount.ToString();
    }
    public void Bt_MaxPlus()
    {
        sotangcount = maxsotangcount;
        SotangInput.text = sotangcount.ToString();
    }
    public void CheckCount(string count)
    {
        if (count.Equals("0") || count.Equals(""))
        {
            sotangcount = 1;
            SotangInput.text = "1";
            return;
        }

        if (int.Parse(count) > maxsotangcount)
        {
            sotangcount = maxsotangcount;
            SotangInput.text = sotangcount.ToString();
            return;
        }

        sotangcount = int.Parse(count);
        SotangInput.text = count;


    }

    public void Bt_Minus()
    {
        if (sotangcount == 1)
        {
            //최소
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/최소"), alertmanager.alertenum.일반);
            return;
        }

        sotangcount--;
        SotangInput.text = sotangcount.ToString();

    }
    public void Bt_MinMinus()
    {
        sotangcount=1;
        SotangInput.text = sotangcount.ToString();

    }
    public void Bt_Sotang()
    {
        if (!PlayerBackendData.Instance.sotang_raid.Contains(nowselectmap))
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/1회클리어소탕가능"), alertmanager.alertenum.일반);
            return;
        }
        if (PlayerBackendData.Instance.CheckItemCount("200000") == 0)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/아이템이부족"), alertmanager.alertenum.일반);
            return;
        }
        
        sotangcount = 1;
        maxsotangcount = PlayerBackendData.Instance.CheckItemCount("200000");
        if (maxsotangcount > 30)
            maxsotangcount = 30;
        SotangInput.text = sotangcount.ToString();

        SotangObj.Show(false);
    }

    public Text RaidFinishText;
    public Image monsterimage_raidfinish;
    public GameObject raidrewardpanel;
    public itemiconslot[] reward_raidfinish;
    public GameObject effect_raidfinish;
    public List<string> dungeondropsid = new List<string>();
    public List<int> dungeondropshowmany = new List<int>();

    public void Bt_StartFinishSotang()
    {
        dungeondropsid.Clear();
        dungeondropshowmany.Clear();
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

        if (!Settingmanager.Instance.CheckServerOn())
        {
            return;
            //다시 시도
        }
        
        
        if (PlayerBackendData.Instance.CheckItemAndRemove("200000", sotangcount))
        {
           // achievemanager.Instance.AddCount(Acheves.레이드격파, sotangcount);
            QuestManager.Instance.AddCount(sotangcount, "singleraid");

            LogManager.Stang_Raid(sotangcount);
            GiveDropToInvenToryBoss(Mon_DropItemIDboss,
                Mon_DropItemMinHowmanyboss,
                Mon_DropItemMaxHowmanyboss, Mon_DropItemPercentboss);
            
            StartCoroutine(FinishRaidReward2());
        }
    }
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator FinishRaidReward()
    {
        Debug.Log(raidmonsterid);
        RaidFinishText.text = string.Format(Inventory.GetTranslate("UI/레이드토벌완료"),
            Inventory.GetTranslate(monsterDB.Instance.Find_id(raidmonsterid).name));
        monsterimage_raidfinish.sprite =
            SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id(raidmonsterid).sprite);
        yield return SpriteManager.Instance.GetWaitforSecond(3);

        foreach (var t in reward_raidfinish)
        {
            t.canvas.alpha =0;
            t.canvas.interactable =false;
        }
       // achievemanager.Instance.AddCount(Acheves.레이드격파, 1);
        QuestManager.Instance.AddCount(1, "singleraid");

        
        effect_raidfinish.SetActive(false);
        raidrewardpanel.SetActive(true);
        yield return SpriteManager.Instance.GetWaitforSecond(0.25f);

        effect_raidfinish.SetActive(true);
        yield return SpriteManager.Instance.GetWaitforSecond(0.95f);
        for (int i = 0; i < dungeondropsid.Count; i++)
        {
            reward_raidfinish[i].canvas.alpha = 1;
            reward_raidfinish[i].canvas.interactable = true;
            reward_raidfinish[i].Refresh(dungeondropsid[i], dungeondropshowmany[i], false, true);
            yield return waits;
        }
        
        Savemanager.Instance.Save();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public IEnumerator FinishRaidReward2()
    {
        Debug.Log(raidmonsterid);
        RaidFinishText.text = string.Format(Inventory.GetTranslate("UI/레이드토벌완료"),
            Inventory.GetTranslate(monsterDB.Instance.Find_id(raidmonsterid).name));
        monsterimage_raidfinish.sprite =
            SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id(raidmonsterid).sprite);

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
    
    public void FinishRaid()
    {
        StartCoroutine(FinishRaidReward());
    }

    public void  AddLoot(string id, int count)
    {
        if (dungeondropsid.Contains(id))
        {
            int i = dungeondropsid.IndexOf(id);
            dungeondropshowmany[i] += count;
        }
        else
        {
            dungeondropsid.Add(id);
            dungeondropshowmany.Add(count);
        }
    }


    readonly WaitForSeconds waits = new WaitForSeconds(0.2f);
    WaitForSeconds waits2 = new WaitForSeconds(1.2f);




    public void GiveDropToInvenToryBoss(List<string> dropid, List<int> minhowmany, List<int> maxhowmany,
        List<int> percent)
    {
        for (int j = 0; j < sotangcount; j++)
        {
            for (int i = 0; i < dropid.Count; i++)
            {
//                Debug.Log(dropid[i]);
                Random.InitState((int)Time.deltaTime + PlayerBackendData.Instance.GetRandomSeed());
                int Ran_rate = Random.Range(0, 1000000); // 1,000,000이 100%이다.
                if (Ran_rate <= mondropmanager.Instance.getpercent(percent[i]))
                {
                    int Howmany = Random.Range(minhowmany[i], maxhowmany[i]);
                    Inventory.Instance.AddItem(dropid[i], Howmany, false, true);
                    RaidManager.Instance.AddLoot(dropid[i], Howmany);
                }
            }
        }

        //       Debug.Log(sotangcount);
        Savemanager.Instance.SaveInventory_SaveOn();
    }



    public List<string> Mon_DropItemIDboss = new List<string>();
    public List<int> Mon_DropItemMinHowmanyboss = new List<int>();
    public List<int> Mon_DropItemMaxHowmanyboss = new List<int>();
    public List<int> Mon_DropItemPercentboss = new List<int>();

    public void SetDropData(string nowdropbossid)
    {
        //Debug.Log(nowdropbossid);

        Mon_DropItemIDboss.Clear();
        Mon_DropItemMinHowmanyboss.Clear();
        Mon_DropItemMaxHowmanyboss.Clear();
        Mon_DropItemPercentboss.Clear();

        
        MonDropDB.Row[] data2 = MonDropDB.Instance.FindAll_id(nowdropbossid).ToArray();

        foreach (var t in data2)
        {
            Mon_DropItemIDboss.Add(t.itemid);
            Mon_DropItemMinHowmanyboss.Add(int.Parse(t.minhowmany));
            Mon_DropItemMaxHowmanyboss.Add(int.Parse(t.maxhowmany));
            Mon_DropItemPercentboss.Add(int.Parse(t.rate));
        }
    }
}






public enum minigames
{
    화살표맞추기,
    Length
}
