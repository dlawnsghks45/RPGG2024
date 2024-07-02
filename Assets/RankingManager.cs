using System;
using System.Globalization;
using BackEnd;
using Doozy.Engine.UI;
using LitJson;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

// ReSharper disable All

public class RankingManager : MonoBehaviour
{
    private static RankingManager _instance = null;
    public static RankingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(RankingManager)) as RankingManager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    [SerializeField]
    private bool isinit;

    public UIView RankingPanel;
    

    public string[] RankUUID =
        {
            "a7266a70-d5ab-11ed-b054-4f1c1856cbd9"
        ,"b4b6b4b0-d5ab-11ed-942d-07dba37d07d1",
        "b8ad6b50-df64-11ed-a58b-7b8196e7f0e8",
        "e244c730-0dd8-11ef-b538-adeb1f163bd9",
        "4f148330-329e-11ee-aee8-afd9bd12cdc6",//길드,`11
        "abbc0400-4bdf-11ee-9e5d-8d7bd888bb2b"
        };

    public string[] RankTableName =
        {
    "AltarWar",
    "Level",
    "BattlePoint",
    "TrainTotalDmg",
    "goods3",//길드
    "AntcaveLv",
    "Length"
    };

    public JsonData[] RankJson = new JsonData[10];
    public JsonData[] MyRankJson = new JsonData[10];

    public bool[] Rankinit = new bool[10];
    
    public enum RankEnum
    {
        성물,
        레벨,
        전투력,
        허수아비60,
        길드,
        개미굴,
        Length
    }

    [Button(Name = "InsertRank")]
    public void InsertRank()
    {
        RankInsert("2100000000000", RankEnum.성물);
    }

    private readonly decimal altarwarminval = 100000;
    // ReSharper disable Unity.PerformanceAnalysis
    public void RankInsert(string score, RankEnum rank)
    {
        #if UNITY_EDITOR
        if(RankEnum.레벨 == rank || RankEnum.허수아비60 == rank
                               || RankEnum.전투력 == rank)
        return;

       
        #endif
        if (PlayerBackendData.istest)
        {
            Test.Instance.Testobj.SetActive(true);
            return;
        }
        // [변경 필요] '복사한 UUID 값'을 '뒤끝 콘솔 > 랭킹 관리'에서 생성한 랭킹의 UUID값으로 변경해주세요.
        string rankUUID = RankUUID[(int)rank]; // 예시 : "4088f640-693e-11ed-ad29-ad8f0c3d4c70"

        string tableName = "RankData";
        string rowInDate;
        decimal endscore = 0;
      //  Debug.Log("래읔넣기시작");

        // 랭킹을 삽입하기 위해서는 게임 데이터에서 사용하는 데이터의 inDate값이 필요합니다.
        // 따라서 데이터를 불러온 후, 해당 데이터의 inDate값을 추출하는 작업을 해야합니다.
//        Debug.Log("데이터 조회를 시도합니다.");
        var bro = Backend.GameData.GetMyData(tableName, new Where());

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("데이터 조회 중 문제가 발생했습니다 : " + bro);
            return;
        }

  //      Debug.Log("데이터 조회에 성공했습니다 : " + bro);

        if (bro.FlattenRows().Count > 0)
        {
            rowInDate = bro.FlattenRows()[0]["inDate"].ToString();
        }
        else
        {
            Debug.Log("데이터가 존재하지 않습니다. 데이터 삽입을 시도합니다.");
            var bro2 = Backend.GameData.Insert(tableName);

            if (bro2.IsSuccess() == false)
            {
                Debug.LogError("데이터 삽입 중 문제가 발생했습니다 : " + bro2);
                return;
            }

            Debug.Log("데이터 삽입에 성공했습니다 : " + bro2);

            rowInDate = bro2.GetInDate();
        }

        Debug.Log("내 게임 정보의 rowInDate : " + rowInDate); // 추출된 rowIndate의 값은 다음과 같습니다.
        Param param = new Param();

        switch (rank)
        {
            case RankEnum.성물:
                Debug.Log(score);
                if (decimal.Parse(score) < altarwarminval)
                {
                    endscore = 0;
                }
                else
                {
                    decimal end = decimal.Parse(score) / altarwarminval;
                    endscore =  end;
                }
                param.Add(RankTableName[(int)rank], endscore);
                break;
            case RankEnum.레벨:
                param.Add(RankTableName[(int)rank], int.Parse(score));
                break;
            case RankEnum.전투력:
                param.Add(RankTableName[(int)rank], int.Parse(score));
                break;
            case RankEnum.허수아비60:
                param.Add(RankTableName[(int)rank], decimal.Parse(score));
                break;
            case RankEnum.개미굴:
                param.Add(RankTableName[(int)rank], decimal.Parse(score));
              
                break;
        }
        param.Add("PlayerAvatadata",PlayerBackendData.Instance.GetPlayerAvatadata()); //클래스;무기;보조무기;아바타;아바타무기;아바타보조무기
        
        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, rankUUID, tableName, rowInDate, param, rankBro => 
        {
            Debug.Log(rankBro);
            if (rankBro.IsSuccess() == false)
            {
                //    Debug.LogError("랭킹 등록 중 오류가 발생했습니다. : " + rankBro);
                switch (rank)
                {
                    case RankEnum.성물:
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/성물실패"),alertmanager.alertenum.일반);
                        break;
                    case RankEnum.레벨:
                        break;
                    case RankEnum.전투력:
                        break;
                }
                return;
            }
            switch (rank)
            {
                case RankEnum.성물:
                    altarwarmanager.Instance.FinishAltarWarLoot();
                    break;
                case RankEnum.레벨:
                    break;
                case RankEnum.전투력:
                    break;
                case RankEnum.허수아비60:
                    break;
                case RankEnum.개미굴:
                    break;
            }
        });
    }


    public rankslot[] rankslots;
    public rankslot myrankslots;

    private bool rankshow = false;

    void refreshcoolon()
    {
        rankshow = false;
    }

    public int nowselectnum;
    public void ShowRanking(int num)
    {
        nowselectnum = num;
        ShowRank(RankUUID[num]);
    }
    public void ShowGuildRanking()
    {
        nowselectnum = 4;
        ShowGuildRanking(RankUUID[4]);
    }
    public GameObject loading;
    private void ShowRank(string rankUUID)
    {
        loading.SetActive(true);

        foreach (var t in rankslots)
        {
            t.gameObject.SetActive(false);
        }
        myrankslots.gameObject.SetActive(false);
        if (rankshow && Rankinit[nowselectnum])
        {
            loading.SetActive(false);
            //alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/잠시 후 시도"), alertmanager.alertenum.일반);
            
            if (RankJson[nowselectnum] != null)
            {
                for (int i = 0; i < RankJson[nowselectnum].Count; i++)
                {
                    rankslots[i].gameObject.SetActive(true);
                    rankslots[i].SetRank(RankJson[nowselectnum][i]["rank"].ToString(), RankJson[nowselectnum][i]["nickname"].ToString(), getScore(RankJson[nowselectnum][i]["score"].ToString()), RankJson[nowselectnum][i]["gamerInDate"].ToString()
                        ,RankJson[nowselectnum][i]["PlayerAvatadata"].ToString());
                }
            }

            if (MyRankJson[nowselectnum] != null)
            {
                myrankslots.SetRank(MyRankJson[nowselectnum][0]["rank"].ToString(), MyRankJson[nowselectnum][0]["nickname"].ToString(), getScore(MyRankJson[nowselectnum][0]["score"].ToString()), MyRankJson[nowselectnum][0]["gamerInDate"].ToString()
                    ,MyRankJson[nowselectnum][0]["PlayerAvatadata"].ToString());
                myrankslots.gameObject.SetActive(true);
            }
            
        }
        else
        {
            //init 
            if (!Rankinit[nowselectnum])
                Rankinit[nowselectnum] = true;
            else
            {
                rankshow = true;
                Invoke(nameof(refreshcoolon), 10f);
            }

            SendQueue.Enqueue(Backend.URank.User.GetRankList, rankUUID,100, callback =>
            {
                // 이후 처리

                if (callback.IsSuccess() == false)
                {
                    Debug.LogError("랭킹 조회중 오류가 발생했습니다. : " + callback);
                    return;
                }

                if (!callback.IsSuccess()) return;
                
              Debug.Log(callback);
                loading.SetActive(false);
                JsonData jsondata = callback.FlattenRows();
                RankJson[nowselectnum] = jsondata;
          
                if (RankJson[nowselectnum] != null)
                {
                    for (int i = 0; i < RankJson[nowselectnum].Count; i++)
                    {
                        rankslots[i].gameObject.SetActive(true);
                        rankslots[i].SetRank(RankJson[nowselectnum][i]["rank"].ToString(), RankJson[nowselectnum][i]["nickname"].ToString(), getScore(RankJson[nowselectnum][i]["score"].ToString()), RankJson[nowselectnum][i]["gamerInDate"].ToString()
                                ,RankJson[nowselectnum][i]["PlayerAvatadata"].ToString());
                    }
                }
            });
            
            SendQueue.Enqueue(Backend.URank.User.GetMyRank, rankUUID, callback =>
            {
                // 이후 처리
                if (callback.IsSuccess())
                {
                    JsonData jsondata = callback.FlattenRows();
                    MyRankJson[nowselectnum] = jsondata;
                    if (MyRankJson[nowselectnum] != null)
                    {
                        myrankslots.SetRank(MyRankJson[nowselectnum][0]["rank"].ToString(), MyRankJson[nowselectnum][0]["nickname"].ToString(), getScore(MyRankJson[nowselectnum][0]["score"].ToString()), MyRankJson[nowselectnum][0]["gamerInDate"].ToString()
                            ,MyRankJson[nowselectnum][0]["PlayerAvatadata"].ToString());
                        myrankslots.gameObject.SetActive(true);
                    }
                }
                else
                {
                    myrankslots.NoRank();
                }
            });
        }

        
        
       
        
        
      

       
    }

        private void ShowGuildRanking(string rankUUID)
    {
        loading.SetActive(true);

        foreach (var t in rankslots)
        {
            t.gameObject.SetActive(false);
        }
        myrankslots.gameObject.SetActive(false);
        if (rankshow && Rankinit[nowselectnum])
        {
            loading.SetActive(false);
            //alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/잠시 후 시도"), alertmanager.alertenum.일반);
            
            if (RankJson[nowselectnum] != null)
            {
                for (int i = 0; i < RankJson[nowselectnum].Count; i++)
                {
                    rankslots[i].gameObject.SetActive(true);
                    rankslots[i].SetRank(RankJson[nowselectnum][i]["rank"].ToString(), RankJson[nowselectnum][i]["guildName"].ToString(), getScore(RankJson[nowselectnum][i]["score"].ToString()), RankJson[nowselectnum][i]["guildInDate"].ToString());
                }
            }

            if (MyRankJson[nowselectnum] != null)
            {
                myrankslots.SetRank(MyRankJson[nowselectnum][0]["rank"].ToString(), MyRankJson[nowselectnum][0]["guildName"].ToString(), getScore(MyRankJson[nowselectnum][0]["score"].ToString()), MyRankJson[nowselectnum][0]["guildInDate"].ToString());
                myrankslots.gameObject.SetActive(true);
            }
            
        }
        else
        {
            //init 
            if (!Rankinit[nowselectnum])
                Rankinit[nowselectnum] = true;
            else
            {
                rankshow = true;
                Invoke(nameof(refreshcoolon), 10f);
            }

            SendQueue.Enqueue(Backend.URank.Guild.GetRankList, rankUUID,100, callback =>
            {
                // 이후 처리

                if (callback.IsSuccess() == false)
                {
                    Debug.LogError("랭킹 조회중 오류가 발생했습니다. : " + callback);
                    return;
                }

                if (!callback.IsSuccess()) return;
                loading.SetActive(false);
                JsonData jsondata = callback.FlattenRows();
                RankJson[nowselectnum] = jsondata;
          
                if (RankJson[nowselectnum] != null)
                {
                    for (int i = 0; i < RankJson[nowselectnum].Count; i++)
                    {
                        rankslots[i].gameObject.SetActive(true);
                        rankslots[i].SetRank(RankJson[nowselectnum][i]["rank"].ToString(), RankJson[nowselectnum][i]["guildName"].ToString(), getScore(RankJson[nowselectnum][i]["score"].ToString()), RankJson[nowselectnum][i]["guildInDate"].ToString());
                    }
                }
            });
            
            SendQueue.Enqueue(Backend.URank.Guild.GetMyGuildRank, rankUUID, callback =>
            {
                // 이후 처리
                if (callback.IsSuccess())
                {
                    JsonData jsondata = callback.FlattenRows();
                    MyRankJson[nowselectnum] = jsondata;
                    if (MyRankJson[nowselectnum] != null)
                    {
                        myrankslots.SetRank(MyRankJson[nowselectnum][0]["rank"].ToString(), MyRankJson[nowselectnum][0]["guildName"].ToString(), getScore(MyRankJson[nowselectnum][0]["score"].ToString()), MyRankJson[nowselectnum][0]["guildInDate"].ToString());
                        myrankslots.gameObject.SetActive(true);
                    }
                }
                else
                {
                    myrankslots.NoRank();
                }
            });
        }

        
        
       
        
        
      

       
    }
    private void Start()
    {
        SendQueue.Enqueue(Backend.URank.User.GetMyRank, RankUUID[2], callback =>
        {
            // 이후 처리
            if (callback.IsSuccess())
            {
                JsonData jsondata = callback.FlattenRows();
                MyRankJson[2] = jsondata;
            }
        });
    }

    public string getmybprank()
    {
        if (MyRankJson[2] != null)
        {
            return MyRankJson[2][0]["rank"].ToString();
        }
        else
        {
            return "0";
        }
    }

    public string getScore(string score)
    {
        switch (nowselectnum)
        {
            
            case 0:
                decimal scores = decimal.Parse(score,NumberStyles.Float);
                scores = scores * (decimal)altarwarminval;
           //     Debug.Log(score);
           //     Debug.Log(scores);
                return $"{dpsmanager.convertNumber(scores)}";
            case 1:
                return $"Lv.{score}";

            case 2:
                return score;
            case 3:
//                Debug.Log(score);
             //   Debug.Log(decimal.Parse(score,System.Globalization.NumberStyles.Number
             //   | System.Globalization.NumberStyles.AllowExponent));
                
                 return $"{dpsmanager.convertNumber(decimal.Parse(score,System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent))}";
            case 4:
                return $"{score:N0} Pt";
            case 5:
                return $"{score:N0}F";
        }
        return "";
    }
}
