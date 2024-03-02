using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Levelshop : MonoBehaviour
{
    private static Levelshop _instance = null;
    public static Levelshop Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Levelshop)) as Levelshop;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    public shopslot[] timeshopslots;
    public shopslot paenlshopslots;
    public GameObject obj;
    public void GiveTime(int num , int day)
    {
        BackendReturnObject servertime = Backend.Utils.GetServerTime();
        string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
        DateTime dts = DateTime.Parse(time);

        PlayerBackendData.Instance.PlayerShopTimes[num] = dts.AddDays(day);
        PlayerBackendData.Instance.PlayerShopTimesbuys[num] = false;
        ShowPanel();
        SaveTime();
    }
    public void GiveTime2(int num , int day)
    {
        BackendReturnObject servertime = Backend.Utils.GetServerTime();
        string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
        DateTime dts = DateTime.Parse(time);

        PlayerBackendData.Instance.PlayerShopTimes[num] = dts.AddDays(day);
        PlayerBackendData.Instance.PlayerShopTimesbuys[num] = false;
        ShowShopSlots(num, timeshopslots[num].shopid);
        ShowPanel();
        SaveTime();
    }
    public void ShowShopSlots(int num,string id)
    {
        paenlshopslots.RefreshTimeshop(num,id);
        obj.SetActive(true);

    }

    [SerializeField] private shoptimelobbyslot lobbyshopslot; 
    
    public GameObject Button;

    private void Start()
    {
        ShowPanel();
    }

    public UIButton NewProductButton;

    private List<int> savetimeint = new List<int>();
    private static readonly int Show = Animator.StringToHash("show");

    public void ShowPanel()
    {
        bool isshow = false;
        //제작중(개수) 버튼을 눌렀을때
        lobbyshopslot.OffRefresh();
        
        Button.SetActive(false);
       Timemanager.Instance.RefreshNowTIme();
       savetimeint.Clear();
        for (int i = 0; i < timeshopslots.Length; i++)
        {
            if (PlayerBackendData.Instance.PlayerShopTimes[i] > Timemanager.Instance.NowTime
                && !PlayerBackendData.Instance.PlayerShopTimesbuys[i])
            {
                timeshopslots[i].gameObject.SetActive(true);
             savetimeint.Add(i);
                isshow = true;
            }
            else
            {
                timeshopslots[i].gameObject.SetActive(false);
            }
        }

        if (isshow)
        {
            int ran = Random.Range(0, savetimeint.Count);
            
            Button.SetActive(true);
//            Debug.Log(savenum);
          //  Debug.Log(ShopDB.Instance.Find_ids(timeshopslots[savenum].shopid).sprite);
            lobbyshopslot.SetRefresh(savetimeint[ran], SpriteManager.Instance.GetSprite(ShopDB.Instance.Find_ids(timeshopslots[ran].shopid).sprite));
        }
    }
    
    public void SaveTime()
    {
        PlayerBackendData userData = PlayerBackendData.Instance;
        Param paramB = new Param
        {
            { "PlayerShopTimes", PlayerBackendData.Instance.PlayerShopTimes },
            { "PlayerShopTimesbuys", PlayerBackendData.Instance.PlayerShopTimesbuys },
        };
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramB, (callback) =>
        {
            Debug.Log(callback);
            if (!callback.IsSuccess()) return;
        });
    }

   
   
    
}
