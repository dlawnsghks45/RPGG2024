using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Doozy.Engine.UI;
using EasyMobile.Scripts.Modules.InAppPurchasing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SeasonPass : MonoBehaviour
{
    //싱글톤만들기.
    private static SeasonPass _instance = null;
    public static SeasonPass Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(SeasonPass)) as SeasonPass;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    public GameObject seasonglow;
    public GameObject seasonAchieve;
    public GameObject seasonNoAchieve;
    
    public GameObject PremiumButton;
    public GameObject LevelButton;
    [SerializeField]
    int nowseasonnum = 0;
    [SerializeField] private Sprite[] PassImagesprite; 
    public UIView Panel;
    [SerializeField] private gaugebarslot gaugebar;
    public Image PassImage;
    [SerializeField] private Text PassLv;
    [SerializeField] private Text Passtype;

    private void Start()
    {
        //초기화
        if (PlayerBackendData.Instance.SeasonPassNum != nowseasonnum)
        {
            PlayerBackendData.Instance.SeasonPassNum = nowseasonnum;
            PlayerBackendData.Instance.SeasonPassPremium = false;
            PlayerBackendData.Instance.SeasonPassExp = 0;

            for (int i = 0; i < PlayerBackendData.Instance.SeasonPassBasicReward.Length; i++)
            {
                PlayerBackendData.Instance.SeasonPassBasicReward[i] = false;
                PlayerBackendData.Instance.SeasonPassPremiumReward[i] = false;
            }
            
            Param param = new Param
            {
                //가방
                { "Crystal", PlayerBackendData.Instance.GetMoney() },
                { "Gold", PlayerBackendData.Instance.GetCash() },
                { "inventory", PlayerBackendData.Instance.ItemInventory },
                { "SeasonPassNum",nowseasonnum },
                { "SeasonPassPremium", PlayerBackendData.Instance.SeasonPassPremium },
                { "SeasonPassExp", PlayerBackendData.Instance.SeasonPassExp.GetDecrypted() },
                { "SeasonPassBasicReward", PlayerBackendData.Instance.SeasonPassBasicReward },
                { "SeasonPassPremiumReward", PlayerBackendData.Instance.SeasonPassPremiumReward },
            };
            // key 컬럼의 값이 keyCode인 데이터 검색
            Where where = new Where();
            where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
            SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Savemanager.Instance.SaveInventory();
                    Savemanager.Instance.SaveMoneyCashDirect();
                    Savemanager.Instance.Save();
                    alertmanager.Instance.NotiCheck_Pass();
                }
            });
        }
        
        
        Refresh();
    }

    public int GetLv()
    {
      return (int)Math.Truncate((double)(PlayerBackendData.Instance.SeasonPassExp / 2000));
    }

    // Start is called before the first frame update
    public void Refresh()
    {
        int passlv = GetLv();
        PassLv.text = passlv.ToString();
        gaugebar.RefreshBar((float)(PlayerBackendData.Instance.SeasonPassExp - (passlv * 2000)), 2000f);
        if (PlayerBackendData.Instance.SeasonPassPremium)
        {
            Passtype.text = Inventory.GetTranslate("UI5/시즌프리미엄");
            PassImage.sprite = PassImagesprite[1];
            seasonglow.SetActive(true);
            PremiumButton.SetActive(false);
            if (PlayerBackendData.Instance.SeasonPassExp >= 100000)
            {
                SeasonPass.Instance.LevelButton.SetActive(false);
                return;
            }
        }
        else
        {
            Passtype.text = Inventory.GetTranslate("UI5/시즌일반");
            PassImage.sprite = PassImagesprite[0];
            seasonglow.SetActive(false);
            PremiumButton.SetActive(true);
            LevelButton.SetActive(false);
            
        }

        if (GetLv() >= 50)
        {
            seasonNoAchieve.SetActive(true);
            seasonAchieve.SetActive(false);
        }
        else
        {
            seasonNoAchieve.SetActive(false);
            seasonAchieve.SetActive(true);
        }
        
        RefreshSeasonItem();
        alertmanager.Instance.NotiCheck_Pass();
    }

    [SerializeField] private seasonpassslot[] seasonreward;
    public bool minlvfind = false;
    public int minlv = 0;
    public RectTransform rectreward;
      float rectstandard = 389.4077f;
      float rectperlv = 77.3429f;

      public bool ishavereward()
      {
          for (int i = 0; i < seasonreward.Length; i++)
          {
              if (seasonreward[i].RewardGBasic.activeSelf
                  || seasonreward[i].RewardGPR.activeSelf)
              {
                  return true;
              }
          }

          return false;
      }
    public void RefreshSeasonItem()
    {
        minlvfind = false;
        minlv = 0;
        for (int i = 0; i < seasonreward.Length; i++)
        {
            seasonreward[i].Refresh(SeasonPassDB.Instance.GetAt(i));
        }

        if (!minlvfind)
            minlv = GetLv();
        
//        Debug.Log("레벨은" + GetLv());
        rectreward.anchoredPosition = new Vector3(-307.3f,rectstandard + (rectperlv * minlv),0f);
    }
    public void Bt_GetAllReward()
    {
        if (!Settingmanager.Instance.CheckServerOn())
        {
            return;
        }
        
        List<string> id = new List<string>();
        List<int> hw = new List<int>();
        bool havereward = false;

        bool[] bb = PlayerBackendData.Instance.SeasonPassBasicReward;
        bool[] pb = PlayerBackendData.Instance.SeasonPassPremiumReward;
        
        for (int i = 0; i < seasonreward.Length; i++)
        {
            if (seasonreward[i].IsBasicReward())
            {
                int index = id.IndexOf(seasonreward[i].br_id);
                if (index != -1)
                {
                    hw[index] += int.Parse(seasonreward[i].br_howmany);
                }
                else
                {
                    id.Add(seasonreward[i].br_id);
                    hw.Add(int.Parse(seasonreward[i].br_howmany));
                }
                seasonreward[i].FinishBasic.SetActive(true);
                seasonreward[i].RewardGBasic.SetActive(false);
                bb[seasonreward[i].num] = true;
                havereward = true;
            }

            if (PlayerBackendData.Instance.SeasonPassPremium)
            {
                if (seasonreward[i].IsPremiumReward())
                {
                    int index = id.IndexOf(seasonreward[i].pr_id);
                    if (index != -1)
                    {
                        hw[index] += int.Parse(seasonreward[i].pr_howmany);
                    }
                    else
                    {
                        id.Add(seasonreward[i].pr_id);
                        hw.Add(int.Parse(seasonreward[i].pr_howmany));
                    }

                    seasonreward[i].FinishPR.SetActive(true);
                    seasonreward[i].RewardGPR.SetActive(false);
                    pb[seasonreward[i].num] = true;
                    havereward = true;
                }
            }
        }
        if(!havereward)
            return;
        
        Param param = new Param
        {
            //가방
            { "Crystal", PlayerBackendData.Instance.GetMoney() },
            { "Gold", PlayerBackendData.Instance.GetCash() },
            { "inventory", PlayerBackendData.Instance.ItemInventory },
            { "SeasonPassNum",nowseasonnum },
            { "SeasonPassPremium", PlayerBackendData.Instance.SeasonPassPremium },
            { "SeasonPassExp", PlayerBackendData.Instance.SeasonPassExp.GetDecrypted() },
            { "SeasonPassBasicReward", PlayerBackendData.Instance.SeasonPassBasicReward },
            { "SeasonPassPremiumReward", PlayerBackendData.Instance.SeasonPassPremiumReward },
        };
        // key 컬럼의 값이 keyCode인 데이터 검색
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
        {
            if (callback.IsSuccess())
            {
                Inventory.Instance.ShowEarnItem3(id.ToArray(),hw.ToArray(),false);

                for (int i = 0; i < id.Count; i++)
                {
                    Inventory.Instance.AddItem(id[i], hw[i], false);
                }
                PlayerBackendData.Instance.SeasonPassBasicReward = bb;
                PlayerBackendData.Instance.SeasonPassPremiumReward = pb;
                
                Savemanager.Instance.SaveInventory();
                Savemanager.Instance.SaveMoneyCashDirect();
                Savemanager.Instance.Save();
                alertmanager.Instance.NotiCheck_Pass();
                LogManager.Log_CrystalEarn("시즌패스보상");
            }
            else
            {
                Application.Quit();
            }
        });

    }
    
    
    public void SaveSeasonReward()
    {
        Param param = new Param
        {
            //가방
            { "Crystal", PlayerBackendData.Instance.GetMoney() },
            { "Gold", PlayerBackendData.Instance.GetCash() },
            { "inventory", PlayerBackendData.Instance.ItemInventory },
            { "SeasonPassNum",nowseasonnum },
            { "SeasonPassPremium", PlayerBackendData.Instance.SeasonPassPremium },
            { "SeasonPassExp", PlayerBackendData.Instance.SeasonPassExp.GetDecrypted() },
            { "SeasonPassBasicReward", PlayerBackendData.Instance.SeasonPassBasicReward },
            { "SeasonPassPremiumReward", PlayerBackendData.Instance.SeasonPassPremiumReward },
        };
        // key 컬럼의 값이 keyCode인 데이터 검색
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
        {
            if (!callback.IsSuccess()) return;
        });
    }
    
    public void SaveSeason()
    {
        Param param = new Param
        {
            //가방
            { "Achievement", PlayerBackendData.Instance.PlayerAchieveData },
            { "Crystal", PlayerBackendData.Instance.GetMoney() },
            { "Gold", PlayerBackendData.Instance.GetCash() },
            { "inventory", PlayerBackendData.Instance.ItemInventory },
        };
        // key 컬럼의 값이 keyCode인 데이터 검색
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
        {
            if (!callback.IsSuccess()) return;
        });
    }

[SerializeField]
    private itemiconslot[] buyitems;
    [SerializeField]
    private itemiconslot[] buyitemsLvup;


    
    public UIView BuyPremiumPanel;
    public UIView BuyLevelPanel;
    public void ShowSeasonPassBuy()
    {
        List<string> id = new List<string>();
        List<int> hw = new List<int>();
        BuyPremiumPanel.Show(false);
        foreach (var t in buyitems)
        {
            t.canvas.alpha =0;
            t.canvas.interactable =false;
        }
        for (int i = 0; i < 50; i++)
        {
            int index = id.IndexOf(seasonreward[i].pr_id);
            if (index != -1)
            {
                hw[index] += int.Parse(seasonreward[i].pr_howmany);
            }
            else
            {
                id.Add(seasonreward[i].pr_id);
                hw.Add(int.Parse(seasonreward[i].pr_howmany));
            }
        }
        for (int i = 0; i < id.Count; i++)
        {
            buyitems[i].Refresh(id[i],hw[i],false);
            buyitems[i].canvas.alpha = 1;
            buyitems[i].canvas.interactable = true;
        }
    }

    public const int crystallvup = 400;
    public Text crystallvtext;
    public Text LvupText;
    [SerializeField] private countpanel countpanel;
    public void ShowSeasonLevelBuy()
    {
        countpanel.SetCount(1);
        countpanel.Maxcount = 50 - GetLv();
        RefreshdSeasonPassLv();
        BuyLevelPanel.Show(false);
    }

    public void RefreshdSeasonPassLv()
    {
        
        if (countpanel.nowcount == 0)
        {
            crystallvtext.text = "0";
            return;
        }

        List<string> id = new List<string>();
        List<int> hw = new List<int>();

        LvupText.text = $"Lv.{GetLv()} ▶  Lv.{GetLv()+countpanel.nowcount}"; 
        crystallvtext.text = (crystallvup * countpanel.nowcount).ToString("N0");
        for (int i = GetLv(); i < GetLv() + countpanel.nowcount; i++)
        {

            int index = id.IndexOf(seasonreward[i].br_id);
            if (index != -1)
            {
                hw[index] += int.Parse(seasonreward[i].br_howmany);
            }
            else
            {
                id.Add(seasonreward[i].br_id);
                hw.Add(int.Parse(seasonreward[i].br_howmany));
            }

            index = id.IndexOf(seasonreward[i].pr_id);
            if (index != -1)
            {
                hw[index] += int.Parse(seasonreward[i].pr_howmany);
            }
            else
            {
                id.Add(seasonreward[i].pr_id);
                hw.Add(int.Parse(seasonreward[i].pr_howmany));
            }
        }

        for (int i = 0; i < buyitemsLvup.Length; i++)
        {
            buyitemsLvup[i].canvas.alpha = 0;
            buyitemsLvup[i].canvas.interactable = false;
        }
        for (int i = 0; i < id.Count; i++)
        {
            buyitemsLvup[i].Refresh(id[i],hw[i],false);
            buyitemsLvup[i].canvas.alpha = 1;
            buyitemsLvup[i].canvas.interactable = true;
        }
    }
    public void BuySeasonPass()
    {
        InAppPurchasing.Purchase("rpgg2.package.seasonpass1");
    }

    public void Bt_LevelBuy()
    {
        if (PlayerBackendData.Instance.GetCash() < countpanel.nowcount * crystallvup)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/불꽃부족"),alertmanager.alertenum.주의);
            return;
        }
        else
        {
            List<string> id = new List<string>();
            List<string> howmany = new List<string>();
            alertmanager.Instance.ShowAlert( string.Format(Inventory.GetTranslate("UI6/시즌 패스 레벨 구매 완료"),GetLv(),GetLv()+countpanel.nowcount),alertmanager.alertenum.일반);
            Inventory.Instance.AddItem("998", countpanel.nowcount * 2000);
            id.Add("998");
            howmany.Add((countpanel.nowcount * 2000).ToString());
            PlayerData.Instance.DownCash(countpanel.nowcount * crystallvup);
            Savemanager.Instance.SaveCash();
            Savemanager.Instance.Save();
            SeasonPass.Instance.BuyLevelPanel.Hide(true);
            SeasonPass.Instance.Refresh();
            SeasonPass.Instance.SaveSeasonReward();
            Inventory.Instance.ShowEarnItem(id.ToArray(), howmany.ToArray(), false);
        }
    }
}
