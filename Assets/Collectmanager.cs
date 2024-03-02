using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using BackEnd;
using Doozy.Engine.UI;

public class Collectmanager : MonoBehaviour
{
     //싱글톤만들기.
     private static Collectmanager _instance = null;

     public static Collectmanager Instance
     {
          get
          {
               if (_instance == null)
               {
                    _instance = FindObjectOfType(typeof(Collectmanager)) as Collectmanager;

                    if (_instance == null)
                    {
                         //Debug.Log("Player script Error");
                    }
               }

               return _instance;
          }
     }

     public UIView CollectPanel;

     public List<collectitemslots> slots = new List<collectitemslots>();

     public UIButton AllCollectButton;

     public float[] Stat = new float[(int)Statenum.Length];

     enum Statenum
     {
          allstat,
          statupper,
          crit,
          critdmg,
          meleedmg,
          magicdmg,
          dotdmg,
          Length
     }

     public Text CollectCount;
     public int FinishCount = 0;

     void RefreshCollectCount()
     {
          FinishCount = 0;
          foreach (var VARIABLE in PlayerBackendData.Instance.CollectData)
          {
               if (VARIABLE.Value.Isfinishall)
               {
                    FinishCount++;
               }
          }

          CollectCount.text = $"{FinishCount.ToString()}/{slots.Count}";
     }

     public Text Collecttext;


     private void Start()
     {
         // Stat = new float[(int)Statenum.Length];
         // RefreshAll();
        //  RefreshStat();
     }


     public void RefreshAll()
     {
          AllCollectButton.Interactable = false;
          //.아이템
          foreach (var t in slots)
          {
               t.Refresh();
          }

          RefreshCollectCount();
     }

     //수집 완료창
     public UIView CollectEndPanel;
     public UIButton CollectEndButton;
     public Text ItemName;
     public Text ItemCount;
     public Image ItemImage;
     public Text ItemStat;
     public Text ButtonText;
     string nowid;


  
     


     //완료가능하면 맨위로 오게



     public void RefreshStat()
     {
          for (int i = 0; i < Stat.Length; i++)
               Stat[i] = 0;

          foreach (var t in slots)
          {
               CollectionDB.Row data = CollectionDB.Instance.Find_id(t.data.SetID);
               if (t.data.Isfinishall)
               {
                    Stat[(int)Enum.Parse(typeof(Statenum),
                         data.allpiecestat)] += float.Parse(data.allpiecevalue);
               }
          }

          Inventory.Instance.StringRemove();

          if (Stat[(int)Statenum.allstat] != 0)
          {
               Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Collectstat/모능"),
                    Stat[(int)Statenum.allstat]));
          }

          Collecttext.text = Inventory.Instance.StringEnd();
     }



     public void Bt_CollectAll()
     {
          foreach (var t in slots)
          {
               for (int j = 0; j < t.data.ItemID.Length; j++)
               {
                    if (!t.data.Isfinish[j])
                    {
                         if (t.isequip)
                         {
                              //장비라면
                              EquipItemDB.Row eqdata = EquipItemDB.Instance.Find_id(t.data.ItemID[j]);
                              foreach (var VARIABLE in PlayerBackendData.Instance.GetTypeEquipment(eqdata.Type)
                                            .Where(VARIABLE => VARIABLE.Value.Itemid.Equals(t.data.ItemID[j])))
                              {
                                   t.data.Curcount[j] = t.data.Maxcount[j];
                                   t.data.Isfinish[j] = true;
                                   //가이드 퀘스트
                                   Tutorialmanager.Instance.CheckTutorial("collect");
                              }
                         }
                         else
                         {
                              //아이템이라
                              if (PlayerBackendData.Instance.CheckItemCount(t.data.ItemID[j]) < t.data.Maxcount[j])
                                   continue;
                              //완
                              t.data.Curcount[j] = t.data.Maxcount[j];
                              t.data.Isfinish[j] = true;
                              PlayerBackendData.Instance.RemoveItem(t.data.ItemID[j], t.data.Maxcount[j]);
                              //가이드 퀘스트
                              Tutorialmanager.Instance.CheckTutorial("collect");
                         }
                    }
               }
          }
          RefreshAll();
          alertmanager.Instance.NotiCheck_Collection();
          Savemanager.Instance.SaveCollection();
          Savemanager.Instance.SaveInventory();
          Savemanager.Instance.Save();
          
          Param paramCollect = new Param
          {
               //가방
               { "CollectionR", PlayerBackendData.Instance.CollectData },
          };
          
          SendQueue.Enqueue(Backend.GameLog.InsertLogV2, "수접저장", paramCollect, 7,( callback ) => 
          {
               // 이후 처리
          });
     }

}

public class CollectDatabase
{
     public string SetID
     {
          get => setid;
          set => setid = value;
     }
     public string[] ItemID
     {
          get => itemid;
          set => itemid = value;
     }

     public int[] Curcount
     {
          get => curcount;
          set => curcount = value;
     }
     public int[] Maxcount
     {
          get => maxcount;
          set => maxcount = value;
     }
     public bool[] Isfinish
     {
          get => isfinish;
          set => isfinish = value;
     }

     public bool Isfinishall
     {
          get => isfinishall;
          set => isfinishall = value;
     }

     private string setid;
     private string[] itemid;
     private int[] curcount;
     private int[] maxcount;
     private bool[] isfinish;
     private bool isfinishall;
     
     public CollectDatabase()
     {
          
     }
     
     public CollectDatabase(JsonData data)
     {
          
          this.SetID = data["SetID"].ToString();
          ItemID = CollectionDB.Instance.Find_id(this.SetID).itemid.Split(';');

          
          string[] maxhow = CollectionDB.Instance.Find_id(this.SetID).howmany.Split(';');
          Maxcount = new int[maxhow.Length];
          Isfinish = new bool[maxcount.Length];
          Curcount = new int[maxcount.Length];
          Maxcount.Initialize();
          Curcount.Initialize();

          Isfinishall = bool.Parse(data["Isfinishall"].ToString());

          
          for (int i = 0; i < data["Curcount"].Count; i++)
          {
               Curcount[i] = int.Parse(data["Curcount"][i].ToString());
                    Maxcount[i] = int.Parse(data["Maxcount"][i].ToString());
               Isfinish[i] = bool.Parse(data["Isfinish"][i].ToString());
          }
     }

     public CollectDatabase(string id)
     {
          //초기화
          this.SetID = id;
          ItemID = CollectionDB.Instance.Find_id(id).itemid.Split(';');
          string[] maxhow = CollectionDB.Instance.Find_id(id).howmany.Split(';');
          Maxcount = new int[maxhow.Length];
          Maxcount.Initialize();
          Curcount = new int[maxcount.Length];
          Curcount.Initialize();
          Isfinish = new bool[maxcount.Length];
          Isfinishall = false;
          
          for (int i = 0; i < maxhow.Length; i++)
          {
               Curcount[i] = 0;
               Maxcount[i] = int.Parse(maxhow[i]);
               Isfinish[i] = false;
          }
     }

     public CollectDatabase(string setid,string[] id, int[] curcount, bool[] isfinish,bool isfinishall)
     {
          this.SetID = setid;
          this.ItemID = id;
          this.Curcount = curcount;
          this.Isfinish = isfinish;
          this.isfinishall = isfinishall;
     }

   
     public bool CheckCountItem()
     {
          return itemid.Where((t, i) => PlayerBackendData.Instance.CheckItemCount(t) >= Maxcount[i] && !isfinish[i]).Any();
     }
     
     public bool CheckCountEquip()
     {
          foreach (var t in itemid)
          {
               EquipItemDB.Row eqdata = EquipItemDB.Instance.Find_id(t);
               if (PlayerBackendData.Instance.GetTypeEquipment(eqdata.Type).Any(VARIABLE => VARIABLE.Value.Itemid.Equals(itemid)))
               {
                    return true;
               }
          }

          return false;
     }
}