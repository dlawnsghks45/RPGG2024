using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class WorldBossSlot : MonoBehaviour
{
    public string bossid;
    public string RankingID;
    public Image BossImage;
    public Text BossName;
    public Text BossLv;
    public Text HpText;
    public Text MvpText;
    public Text DeathText;
    public Text PlayerCount;
    public Image HPbar;
    public GameObject RewardObj;
    public decimal curhp;
    public decimal maxhp;
    public GameObject Killobj;

    public GameObject Loading;
    
    public List<RankItem> rankItemList = new List<RankItem>();


    // Start is called before the first frame update

    public void Start()
    {
        InitData();
    }

    public void InitData()
    {
        BossImage.sprite =
            SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id(WorldBossDB.Instance.Find_id(bossid).monsterid)
                .sprite);

        RankingID = WorldBossDB.Instance.Find_id(bossid).rankinguuid;
////        Debug.Log(RankingID + "UUID");
        BossLv.text = $"Lv.{WorldBossDB.Instance.Find_id(bossid).level}";
        
        BossName.text = Inventory.GetTranslate(monsterDB.Instance
            .Find_id(WorldBossDB.Instance.Find_id(bossid).monsterid)
            .name);

        GetRankData(true);
    }
    
    public void InitData2()
    {
        BossImage.sprite =
            SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id(WorldBossDB.Instance.Find_id(bossid).monsterid)
                .sprite);

        RankingID = WorldBossDB.Instance.Find_id(bossid).rankinguuid;

        BossLv.text = $"Lv.{WorldBossDB.Instance.Find_id(bossid).level}";
        
        BossName.text = Inventory.GetTranslate(monsterDB.Instance
            .Find_id(WorldBossDB.Instance.Find_id(bossid).monsterid)
            .name);
        CalculateDamage();
        PlayerCount.text = rankItemList.Count.ToString();

    }
    private bool isrefresh;

    void falserefresh()
    {
        isrefresh = false;
    }
    public void GetRankData(bool isnorefresh = false)
    {
        if (isnorefresh)
        {
            
        }
        else if (isrefresh)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/잠시후시도"),alertmanager.alertenum.일반);
            return;
        }

        if (!isnorefresh)
        {
            isrefresh = true;
            Invoke("falserefresh", 30);
        }

        rankItemList.Clear();
        Loading.SetActive(true);
        SendQueue.Enqueue(Backend.URank.User.GetRankList, RankingID, 100, callback=> {
            // 이후 처리
            if (callback.IsSuccess())
            {
                callbackobj = callback;
                Invoke("invokeon",0.7f);
            }
        });
    }

    private BackendReturnObject callbackobj;
    void invokeon()
    {
        Loading.SetActive(false);
        LitJson.JsonData rankListJson = callbackobj.GetFlattenJSON();

        for(int i = 0; i < rankListJson["rows"].Count; i++)
        {
            RankItem rankItem = new RankItem();

            rankItem.gamerInDate = rankListJson["rows"][i]["gamerInDate"].ToString();
            rankItem.nickname = rankListJson["rows"][i]["nickname"].ToString();
            rankItem.score = decimal.Parse(rankListJson["rows"][i]["score"].ToString()) * WorldBossManager.devidetime;
            rankItem.index = rankListJson["rows"][i]["index"].ToString();
            rankItem.rank = rankListJson["rows"][i]["rank"].ToString();
            rankItem.totalCount = rankListJson["totalCount"].ToString();

            if(rankListJson["rows"][i].ContainsKey("PlayerAvatadata"))
            {
                rankItem.extraData = rankListJson["rows"][i]["PlayerAvatadata"].ToString();
            }
            rankItemList.Add(rankItem);
        }
        PlayerCount.text = rankItemList.Count.ToString();
        CalculateDamage();
    }

    public void CalculateDamage()
    {
        Killobj.SetActive(false);

        decimal dmg = 0;

        for (int i = 0; i < rankItemList.Count; i++)
        {
            dmg += rankItemList[i].score;
        }
//        Debug.Log(monsterDB.Instance.Find_id(WorldBossDB.Instance.Find_id(bossid).monsterid)
          //  .hp);
        maxhp = decimal.Parse(monsterDB.Instance.Find_id(WorldBossDB.Instance.Find_id(bossid).monsterid)
            .hp);
        curhp = maxhp;
        curhp -= dmg;

        if (curhp <= 0)
        {
            int num = (int)Enum.Parse(typeof(Timemanager.ContentEnumDaily),
                WorldBossDB.Instance.Find_id(bossid).arrynum2);

            //보상 횟수확인
            if (Timemanager.Instance.GetNowCount_daily2(num) > 0)
            {
                RewardObj.SetActive(true);
            }
            else
            {
                RewardObj.SetActive(false);
            }
            
            curhp = 0;
            Killobj.SetActive(true);
            //보상 정보 계산
            MvpText.text = string.Format(Inventory.GetTranslate("UI6/월드보스mvp"),rankItemList[0].nickname);
            DeathText.text = string.Format(Inventory.GetTranslate("UI6/월드보스토벌완료"),BossName.text);
        }
        else
        {
            Killobj.SetActive(false);
        }
        
        decimal fa = ((curhp / maxhp) );
        HpText.text = $"{fa* 100m:N1}%";
        HPbar.fillAmount = (float)fa;
    }
    
    [Button(Name = "피떨구기")]
    public void ShowBossData()
    {
        WorldBossManager.Instance.ShowBossdata(this);
    }

    public void Bt_GetReward()
    {
        if (!Settingmanager.Instance.CheckServerOn())
        {
            return;
            //다시 시도
        }
        
        if (Timemanager.Instance.ConSumeCount_DailyAscny((int)Timemanager.ContentEnumDaily.월드보스보상횟수리뉴얼, 1))
        {
            int num = (int)Enum.Parse(typeof(Timemanager.ContentEnumDaily),
                WorldBossDB.Instance.Find_id(bossid).arrynum2);
            
            if (Timemanager.Instance.ConSumeCount_DailyAscny(num, 1))
            {
                //보상지급ㅔ
//                Debug.Log("보상 지급");
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/월드보스토벌보상지급"), alertmanager.alertenum.주의);
                WorldBossManager.Instance.SetDropDataDropId(WorldBossDB.Instance.Find_id(bossid).mondropid);
                WorldBossManager.Instance.rewardtimetext.Refresh();
                RewardObj.SetActive(false);
                LogManager.Log_CrystalEarn("월드보스레이드");

            }
        }
        else
        {
            RewardObj.SetActive(false);
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/월드보스보상횟수없음"), alertmanager.alertenum.주의);
            return;
        }
    }
}
