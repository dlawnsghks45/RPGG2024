using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Doozy.Engine.UI;
using LitJson;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class dailyrewardmanager : MonoBehaviour
{
    //�̱��游���.
    private static dailyrewardmanager _instance = null;
    public static dailyrewardmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(dailyrewardmanager)) as dailyrewardmanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    
    public dailyrewardslot[] DailyRewardslot;

    private void Start()
    {
        Initdailyreward();
    }

    private JsonData json;
    public void Initdailyreward()
    {
        
        SendQueue.Enqueue(Backend.Chart.GetChartContents, "96902", (callback) =>
        {
            //Debug.Log("��Ʈ �ҷ���");
            Debug.Log(callback);
            // ���� �۾�
            if (!callback.IsSuccess()) return;

            
           // Debug.Log("������ �Է���");
            //��Ʈ�� �ҷ��´�.
            json = callback.FlattenRows();
            //��Ʈ ������ ���� �Է�
            for (int i = 0; i < DailyRewardslot.Length; i++)
            {
//                Debug.Log("����");
                DailyRewardslot[i].InitData(json[i]["day"].ToString(), json[i]["itemid"].ToString()
                    , int.Parse(json[i]["howmany"].ToString()));

                
                //���࿡ �Ϸ� �ߴµ� ���� �� �ִ� ������� �Ϸ�ǥ�ø� ������.
                if (Timemanager.Instance.DailyRewardBool[i])
                {
                    DailyRewardslot[i].Finished();
                }
                else if (Timemanager.Instance.dailycount >= i && !Timemanager.Instance.DailyRewardBool[i])
                {
                    //���� �� �ִ�.
                    DailyRewardslot[i].CanEarn();
                }
                
                if (i + 1 == Timemanager.Instance.NowTime.Day)
                {
                    DailyRewardslot[i].IsToday();
                }

                
            }

            Refresh();
        });
    }

    public void Refresh()
    {
        for (int i = 0; i < DailyRewardslot.Length; i++)
        {
            DailyRewardslot[i].ResetData();
            //���࿡ �Ϸ� �ߴµ� ���� �� �ִ� ������� �Ϸ�ǥ�ø� ������.
            if (Timemanager.Instance.DailyRewardBool[i])
            {
                DailyRewardslot[i].Finished();
            }
            else if (Timemanager.Instance.dailycount >= i && !Timemanager.Instance.DailyRewardBool[i])
            {
                //���� �� �ִ�.
                DailyRewardslot[i].CanEarn();
            }

            if (i + 1 == Timemanager.Instance.NowTime.Day)
            {
                DailyRewardslot[i].IsToday();
            }
        }
        //ũ����Ż ǥ��
        if (Timemanager.Instance.dailycount != 27)
        {
            if (Timemanager.Instance.dailycount + 1 < Timemanager.Instance.NowTime.Day)
            {
                Debug.Log("���� ī��Ʈ" +Timemanager.Instance.dailycount );
                if (Timemanager.Instance.DailyRewardBool[Timemanager.Instance.dailycount])
                {
                    Timemanager.Instance.dailycount++;
                    Refresh();
                }
                else
                {
                    DailyRewardslot[Timemanager.Instance.dailycount + 1].CryStalOpen();
                }
            }
        }
    }

    int GetNowCount()
    {
        int num = 0;
        for (int i = 0; i < Timemanager.Instance.DailyRewardBool.Length; i++)
        {
            if (Timemanager.Instance.DailyRewardBool[i])
            {
                num++;
            }
        }
        return num-1;
    }
    

    public void bt_GetReward()
    {           
        List<string> id = new List<string>();
        List<string> howmany = new List<string>();
        bool istrue = false;
        for (int i = 0; i < DailyRewardslot.Length; i++)
        {
            if (!DailyRewardslot[i].isfinish && DailyRewardslot[i].canearn)
            {
                istrue = true;
                string itemid = json[i]["itemid"].ToString();
                string howmanys = json[i]["howmany"].ToString();
                //�������� ��´� 
                Inventory.Instance.AddItem(json[i]["itemid"].ToString(),int.Parse(howmanys));

                id.Add(itemid);
                howmany.Add(howmanys);
                
//                Debug.Log("�����");
                Timemanager.Instance.DailyRewardBool[i] = true;
                DailyRewardslot[i].Finished();

            }
        }

        if (istrue)
        {
            Param param = new Param();
            param.Add("DailyBool", Timemanager.Instance.DailyRewardBool);

         
                
            SendQueue.Enqueue(Backend.GameData.UpdateV2, "TimeData",
                PlayerBackendData.Instance.PlayerTableIndate[(int)
                    PlayerBackendData.IndatesEnum.TimeData].ToString(),
                PlayerBackendData.Instance.playerindate, param, broCallbackUpdate =>
                {
                   
                    if (broCallbackUpdate.IsSuccess())
                    {
                        Debug.Log("�����ߴ�");
                        
                    }
                    else
                    {
                        Debug.Log("�����ߴ�");
                    }
                });
            Inventory.Instance.ShowEarnItem(id.ToArray(),howmany.ToArray(),false);
        }
    }

    public UIView CrystalObj;
    
    public void Bt_FinishCrystal()
    {
        if (PlayerBackendData.Instance.GetCash() < 400)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/�Ҳɺ���"),alertmanager.alertenum.�Ϲ�);
            return;
        }
        
        PlayerData.Instance.DownCash(400);
        Timemanager.Instance.dailycount++;
        
        Param check = new Param();
        check.Add("DailyBool", Timemanager.Instance.DailyRewardBool);
        check.Add("DailyRewardCount", Timemanager.Instance.dailycount);
        
            SendQueue.Enqueue(Backend.GameData.UpdateV2, "TimeData",
            PlayerBackendData.Instance.PlayerTableIndate[(int)
                PlayerBackendData.IndatesEnum.TimeData].ToString(),
            PlayerBackendData.Instance.playerindate, check, broCallbackUpdate =>
            {
                if (broCallbackUpdate.IsSuccess())
                {
                    Refresh();
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/�⼮üũ���ſϷ�"),alertmanager.alertenum.�Ϲ�);
                    LogManager.LogDailyRewardCry();
                }
                else
                {
                    Timemanager.Instance.dailycount--;
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/�⼮üũ���Ž���"),alertmanager.alertenum.�Ϲ�);

                }
            });
        Savemanager.Instance.SaveCash();
        CrystalObj.Hide(true);
    }
}
