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
        "4f148330-329e-11ee-aee8-afd9bd12cdc6",//���,`11
        "abbc0400-4bdf-11ee-9e5d-8d7bd888bb2b"
        };

    public string[] RankTableName =
        {
    "AltarWar",
    "Level",
    "BattlePoint",
    "TrainTotalDmg",
    "goods3",//���
    "AntcaveLv",
    "Length"
    };

    public JsonData[] RankJson = new JsonData[10];
    public JsonData[] MyRankJson = new JsonData[10];

    public bool[] Rankinit = new bool[10];
    
    public enum RankEnum
    {
        ����,
        ����,
        ������,
        ����ƺ�60,
        ���,
        ���̱�,
        Length
    }

    [Button(Name = "InsertRank")]
    public void InsertRank()
    {
        RankInsert("2100000000000", RankEnum.����);
    }

    private readonly decimal altarwarminval = 100000;
    // ReSharper disable Unity.PerformanceAnalysis
    public void RankInsert(string score, RankEnum rank)
    {
        #if UNITY_EDITOR
        if(RankEnum.���� == rank || RankEnum.����ƺ�60 == rank
                               || RankEnum.������ == rank)
        return;

       
        #endif
        if (PlayerBackendData.istest)
        {
            Test.Instance.Testobj.SetActive(true);
            return;
        }
        // [���� �ʿ�] '������ UUID ��'�� '�ڳ� �ܼ� > ��ŷ ����'���� ������ ��ŷ�� UUID������ �������ּ���.
        string rankUUID = RankUUID[(int)rank]; // ���� : "4088f640-693e-11ed-ad29-ad8f0c3d4c70"

        string tableName = "RankData";
        string rowInDate;
        decimal endscore = 0;
      //  Debug.Log("���óֱ����");

        // ��ŷ�� �����ϱ� ���ؼ��� ���� �����Ϳ��� ����ϴ� �������� inDate���� �ʿ��մϴ�.
        // ���� �����͸� �ҷ��� ��, �ش� �������� inDate���� �����ϴ� �۾��� �ؾ��մϴ�.
//        Debug.Log("������ ��ȸ�� �õ��մϴ�.");
        var bro = Backend.GameData.GetMyData(tableName, new Where());

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("������ ��ȸ �� ������ �߻��߽��ϴ� : " + bro);
            return;
        }

  //      Debug.Log("������ ��ȸ�� �����߽��ϴ� : " + bro);

        if (bro.FlattenRows().Count > 0)
        {
            rowInDate = bro.FlattenRows()[0]["inDate"].ToString();
        }
        else
        {
            Debug.Log("�����Ͱ� �������� �ʽ��ϴ�. ������ ������ �õ��մϴ�.");
            var bro2 = Backend.GameData.Insert(tableName);

            if (bro2.IsSuccess() == false)
            {
                Debug.LogError("������ ���� �� ������ �߻��߽��ϴ� : " + bro2);
                return;
            }

            Debug.Log("������ ���Կ� �����߽��ϴ� : " + bro2);

            rowInDate = bro2.GetInDate();
        }

        Debug.Log("�� ���� ������ rowInDate : " + rowInDate); // ����� rowIndate�� ���� ������ �����ϴ�.
        Param param = new Param();

        switch (rank)
        {
            case RankEnum.����:
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
            case RankEnum.����:
                param.Add(RankTableName[(int)rank], int.Parse(score));
                break;
            case RankEnum.������:
                param.Add(RankTableName[(int)rank], int.Parse(score));
                break;
            case RankEnum.����ƺ�60:
                param.Add(RankTableName[(int)rank], decimal.Parse(score));
                break;
            case RankEnum.���̱�:
                param.Add(RankTableName[(int)rank], decimal.Parse(score));
              
                break;
        }
        param.Add("PlayerAvatadata",PlayerBackendData.Instance.GetPlayerAvatadata()); //Ŭ����;����;��������;�ƹ�Ÿ;�ƹ�Ÿ����;�ƹ�Ÿ��������
        
        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, rankUUID, tableName, rowInDate, param, rankBro => 
        {
            Debug.Log(rankBro);
            if (rankBro.IsSuccess() == false)
            {
                //    Debug.LogError("��ŷ ��� �� ������ �߻��߽��ϴ�. : " + rankBro);
                switch (rank)
                {
                    case RankEnum.����:
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/��������"),alertmanager.alertenum.�Ϲ�);
                        break;
                    case RankEnum.����:
                        break;
                    case RankEnum.������:
                        break;
                }
                return;
            }
            switch (rank)
            {
                case RankEnum.����:
                    altarwarmanager.Instance.FinishAltarWarLoot();
                    break;
                case RankEnum.����:
                    break;
                case RankEnum.������:
                    break;
                case RankEnum.����ƺ�60:
                    break;
                case RankEnum.���̱�:
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
            //alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/��� �� �õ�"), alertmanager.alertenum.�Ϲ�);
            
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
                // ���� ó��

                if (callback.IsSuccess() == false)
                {
                    Debug.LogError("��ŷ ��ȸ�� ������ �߻��߽��ϴ�. : " + callback);
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
                // ���� ó��
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
            //alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/��� �� �õ�"), alertmanager.alertenum.�Ϲ�);
            
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
                // ���� ó��

                if (callback.IsSuccess() == false)
                {
                    Debug.LogError("��ŷ ��ȸ�� ������ �߻��߽��ϴ�. : " + callback);
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
                // ���� ó��
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
            // ���� ó��
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
