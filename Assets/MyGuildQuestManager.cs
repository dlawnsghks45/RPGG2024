using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using BackEnd.Tcp;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class MyGuildQuestManager : MonoBehaviour
{
   //싱글톤만들기.
   private static MyGuildQuestManager _instance = null;

   public static MyGuildQuestManager Instance
   {
      get
      {
         if (_instance == null)
         {
            _instance = FindObjectOfType(typeof(MyGuildQuestManager)) as MyGuildQuestManager;
            if (_instance == null)
            {
               //Debug.Log("Player script Error");
            }
         }

         return _instance;
      }
   }



   public GuildQuestDB.Row nowselectdata;



   #region 길드 퀘스트 목록생성

   public guildquestslot questslotobj;
   public Transform guildtrans;

   private bool questinit = false;

   void ShowAllQuest()
   {
      if (questinit)
         return;
      questinit = true;

      for (int i = 0; i < GuildQuestDB.Instance.NumRows(); i++)
      {
         guildquestslot item = Instantiate(questslotobj, guildtrans);
         item.Refresh(GuildQuestDB.Instance.GetAt(i).id);
         item.gameObject.SetActive(true);
      }
   }

   public void InitGuildQuest()
   {
      ShowAllQuest();
   }

   #endregion


   public UIView GuildQuestAcceptpanel;
   public Text GuildQuestName;
   public Text GuildQuestInfo;

   public Text GuildmemberReward;
   public Text GuildRewardGold;
   public Text GuildmemberExp;

   public void Bt_ShowGuildQuestPanel(string questid)
   {
      nowselectdata = GuildQuestDB.Instance.Find_id(questid);
      GuildQuestAcceptpanel.Show(false);
      GuildQuestName.text = Inventory.GetTranslate(nowselectdata.name);
      GuildQuestInfo.text = Inventory.GetTranslate(nowselectdata.info);

      GuildmemberReward.text = "200";
      int gold = int.Parse(nowselectdata.guildgold);
      int exp = int.Parse(nowselectdata.guildexp);
      GuildRewardGold.text = gold.ToString("N0");
      GuildmemberExp.text = exp.ToString("N0");
   }

   public gaugebarslot gauge;
   public int maxcount = 0;


   public GameObject NoQuestPanel;
   public GameObject HaveQuestPanel;

   public GameObject[] RewardButton; //0은 납품 1은 완료 2는 보상받기

   public Text NowQuestInfo;

   //길드정보 퀘스트 패널임
   public void RefreshQuestPanel()
   {
      if (MyGuildManager.Instance.myguildclassdata.questid.Equals(""))
      {
         //퀘가 없음
         HaveQuestPanel.SetActive(false);
         NoQuestPanel.SetActive(true);
         return;
      }
      else
      {

         HaveQuestPanel.SetActive(true);
         NoQuestPanel.SetActive(false);
      }

      foreach (var VARIABLE in RewardButton)
      {
         VARIABLE.SetActive(false);
      }
      
      maxcount = int.Parse(GuildQuestDB.Instance.Find_id(MyGuildManager.Instance.myguildclassdata.questid).howmany);
      GuildQuestDB.Row data = GuildQuestDB.Instance.Find_id(MyGuildManager.Instance.myguildclassdata.questid);

      if (MyGuildManager.Instance.myguildclassdata.questcount >= maxcount && MyGuildManager.Instance.myguildclassdata.isquestfinish)
      {
         if (Timemanager.Instance.DailyContentCount[(int)Timemanager.ContentEnumDaily.길드임무개인보상] != 0)
         {
            //보상받기
            RewardButton[2].SetActive(true);
         }
      }
      else if (MyGuildManager.Instance.myguildclassdata.questcount >= maxcount && !MyGuildManager.Instance.myguildclassdata.isquestfinish)
      {
         //완료해요!!
         RewardButton[1].SetActive(true);
      }
      else
      {
         //납품해요!!
         RewardButton[0].SetActive(true);
      }

      
      NowQuestInfo.text = Inventory.GetTranslate(data.info);
//     Debug.Log("게산여기");

      gauge.RefreshBar(MyGuildManager.Instance.myguildclassdata.questcount, maxcount);
   }


   public void Bt_GetQuest()
   {
      if (!MyGuildManager.Instance.iscanaccept(1))
      {
         //길드원임
         //   Debug.Log("길드원임");
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/부마스터이상가능"), alertmanager.alertenum.일반);
         return;
      }

      //햔재 있으면
      if (MyGuildManager.Instance.myguildclassdata.questid != "")
      {
         //  Debug.Log("퀘스트가있다");
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/퀘스트진행중"), alertmanager.alertenum.일반);

         return;
      }
      
    

      SendQueue.Enqueue(Backend.Guild.GetMyGuildInfoV3, (callback) =>
      {
         // 이후 처리
         if (callback.IsSuccess())
         {
            SendQueue.Enqueue(Backend.Guild.GetMyGuildGoodsV3, (callbackgoods) =>
            {

               // 이후 처리
               // 이후 처리
               //내굿즈
               if (callbackgoods.IsSuccess())
               {
                  MyGuildManager.Instance.SetGoods(callbackgoods);
                  MyGuildManager.Instance.myguilddata = callback;
                  MyGuildManager.Instance.SetMyGuildData();
                  MyGuildManager.Instance.Loadingbar.SetActive(false);
                  MyGuildManager.Instance.RefreshGuildData();


                  MyGuildManager.Instance.myguildclassdata.questid = nowselectdata.id;
                  Debug.Log("id" + nowselectdata.id);
                  MyGuildManager.Instance.myguildclassdata.questcount = 0;
                  MyGuildManager.Instance.myguildclassdata.isquestfinish = false;
                  MyGuildManager.Instance.myguildclassdata.questgivename.Clear();
                  MyGuildManager.Instance.myguildclassdata.questgivehowmany.Clear();
                  Timemanager.Instance.RefreshNowTIme();
                  Param param = new Param
                  {
                     { "questid", nowselectdata.id },
                     { "questcount", 0 },
                     { "isquestfinish", false },
                     { "questacceptdate", Timemanager.Instance.NowTime.Date },
                     { "questgivename", MyGuildManager.Instance.myguildclassdata.questgivename },
                     { "questgivehowmany", MyGuildManager.Instance.myguildclassdata.questgivehowmany }
                  };

                  SendQueue.Enqueue(Backend.Guild.ModifyGuildV3, param, (callbackmodi) =>
                  {
                     if (callbackmodi.IsSuccess())
                     {
                        Debug.Log("퀘스트 시작");
                        GuildQuestAcceptpanel.Hide(false);
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/퀘스트시작"),
                           alertmanager.alertenum.일반);
                        chatmanager.Instance.ChattoGuildQuestStart(MyGuildManager.Instance.myguildclassdata.questid);
                        //가능하다
                        RefreshQuestPanel();
                     }
                  });
               }
            });
         }
      });
   }

   public Text QuestNameGive;
   public UIView GivePanel;
   public Text HaveItemText;
   public Text MaxCountText; //납품창에서 납품 가능한 개수
   public Text NowCountText; //현재 슬라이드가 있는 곳
   public Slider countslider;
   public Image QuestItemImage;

   public string nowitem;

   public void Bt_SetGiveQuestItem()
   {
      GivePanel.Show(false);
      GuildQuestDB.Row data = GuildQuestDB.Instance.Find_id(MyGuildManager.Instance.myguildclassdata.questid);

      nowitem = data.itemid;
      QuestNameGive.text = Inventory.GetTranslate(data.info);
      QuestItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(data.itemid).sprite);
      //현재 개수를 확인
      int count = PlayerBackendData.Instance.CheckItemCount(data.itemid);
      Debug.Log(count);
      HaveItemText.text = count == 0
         ? string.Format(Inventory.GetTranslate("UI2/보유량"), "0")
         : string.Format(Inventory.GetTranslate("UI2/보유량"), count.ToString("N0"));

      //기본 최대치를 퀘스트 최대치로
      int maxcount = int.Parse(data.howmany) - MyGuildManager.Instance.myguildclassdata.questcount;
      //퀘스트 최대치가 가진 거 보다 많으면
      if (count < int.Parse(data.howmany))
      {
         //아까 가져온걸로한다.
         maxcount = count;
      }

      MaxCountText.text = maxcount.ToString("N0");

      NowCountText.text = "0";
      countslider.value = 0;
      countslider.maxValue = maxcount;
   }


   public void Bt_GiveCountUp(int count)
   {
      if (countslider.value >= countslider.maxValue)
      {
         countslider.value = countslider.maxValue;
         return;
      }

      countslider.value += count;

   }

   public void RefreshGiveSlider()
   {
      NowCountText.text = countslider.value.ToString("N0");
   }


   private bool cangive = false;

   void falsegive()
   {
      cangive = false;
   }

   public void Bt_GiveItem()
   {

      if (cangive)
      {
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/10초뒤"), alertmanager.alertenum.일반);
         return;
      }

      bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Guild);

      if (!isConnect)
      {
         chatmanager.Instance.JoinGuildChannel();
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드서버연결중"), alertmanager.alertenum.일반);
         return;
      }
      
      cangive = true;
      Invoke(nameof(falsegive),10f);
      
      //데이터를 가져옴
      //데이터를 갱신
      //완료됐는지 확인
      //아니면 개수 확인하고 
      string savequestid = MyGuildManager.Instance.myguildclassdata.questid;
      int removecount = 0;
      SendQueue.Enqueue(Backend.Guild.GetMyGuildInfoV3, (callback) =>
      {
         // 이후 처리
         if (callback.IsSuccess())
         {
            SendQueue.Enqueue(Backend.Guild.GetMyGuildGoodsV3, (callbackgoods) =>
            {

               // 이후 처리
               // 이후 처리
               //내굿즈
               if (callbackgoods.IsSuccess())
               {
                  MyGuildManager.Instance.SetGoods(callbackgoods);
                  MyGuildManager.Instance.myguilddata = callback;
                  MyGuildManager.Instance.SetMyGuildData();
                  MyGuildManager.Instance.GuildMainPanel.SetActive(true);
                  MyGuildManager.Instance.RefreshGuildData();


                  if (MyGuildManager.Instance.myguildclassdata.isquestfinish &&
                      MyGuildManager.Instance.myguildclassdata.questid != "")
                  {
                     //이미 완료된 퀘스트입니다.
                     alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/이미임무끝"), alertmanager.alertenum.일반);
                     GivePanel.Hide(true);
                     return;
                  }
                  else if (!MyGuildManager.Instance.myguildclassdata.isquestfinish &&
                           MyGuildManager.Instance.myguildclassdata.questid != savequestid
                           && savequestid != "")
                  {
                     //진행중인 퀘스트와 다르면 안된다.
                     alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/임무가다름"), alertmanager.alertenum.일반);
                     GivePanel.Hide(true);
                     RefreshQuestPanel();
                     return;
                  }
                  else if (!MyGuildManager.Instance.myguildclassdata.isquestfinish &&
                           MyGuildManager.Instance.myguildclassdata.questid == savequestid)
                  {
                     //최대개수 가져옴
                     int maxcount = int.Parse(GuildQuestDB.Instance
                        .Find_id(MyGuildManager.Instance.myguildclassdata.questid).howmany);



                     //개수를 체크한다. 현재 개수가 많으면
                     if (MyGuildManager.Instance.myguildclassdata.questcount >= maxcount)
                     {
                        //이미 끝나서 안된다.
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/이미종료됨"),
                           alertmanager.alertenum.일반);
                        return;
                     }
                     else
                     {
                        //현재 
                        if (countslider.value > maxcount - MyGuildManager.Instance.myguildclassdata.questcount)
                        {
                           //넣을려는 개수보다 더 많이 차있다.
                           removecount = maxcount - MyGuildManager.Instance.myguildclassdata.questcount;
                        }
                        else
                        {
                           removecount = (int)countslider.value;
                        }

                        int totalcount = MyGuildManager.Instance.myguildclassdata.questcount + removecount;
                        
                        if (totalcount> maxcount)
                        {
                           totalcount= maxcount;
                        }

                        if (MyGuildManager.Instance.myguildclassdata.questgivename.Contains(PlayerBackendData.Instance
                               .nickname))
                        {
                           int index = MyGuildManager.Instance.myguildclassdata.questgivename.IndexOf(PlayerBackendData
                              .Instance.nickname);
                           MyGuildManager.Instance.myguildclassdata.questgivehowmany[index] += (int)countslider.value;
                        }
                        else
                        {
                           MyGuildManager.Instance.myguildclassdata.questgivename.Add(PlayerBackendData.Instance.nickname);
                           MyGuildManager.Instance.myguildclassdata.questgivehowmany.Add((int)countslider.value);

                        }
                        
                        Param param = new Param
                        {
                           { "questcount", totalcount},
                           { "questgivename", MyGuildManager.Instance.myguildclassdata.questgivename},
                           { "questgivehowmany", MyGuildManager.Instance.myguildclassdata.questgivehowmany}
                        };

                        SendQueue.Enqueue(Backend.Guild.ModifyGuildV3, param, (callbackmodi) =>
                        {
                           if (callbackmodi.IsSuccess())
                           {
                              
                              MyGuildManager.Instance.myguildclassdata.questcount = totalcount;
                              GivePanel.Hide(true);
                              PlayerBackendData.Instance.RemoveItem(nowitem,(int)countslider.value);
                              alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/납품완료"),
                                 alertmanager.alertenum.일반);
                              chatmanager.Instance.ChattoGiveGuildQuestItem(MyGuildManager.Instance.myguildclassdata.questcount,maxcount);
                              //가능하다
                              Savemanager.Instance.SaveInventory_SaveOn();
                              RefreshQuestPanel();
                           }
                        });
                     }
                  }

               }
            });
         }
      });
   }

   //퀘스트 완료 부길마 기상만 가능
   public void FinishGuildQuest()
   {
      if (!MyGuildManager.Instance.iscanaccept(1))
      {
         //길드원임
         //   Debug.Log("길드원임");
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/부마스터이상가능"), alertmanager.alertenum.일반);
         return;
      }

      Timemanager.Instance.RefreshNowTIme();
      
      if (MyGuildManager.Instance.myguildclassdata.questacceptdate.Equals(DateTime.MinValue) &&
          MyGuildManager.Instance.myguildclassdata.questacceptdate != Timemanager.Instance.NowTime.Date)
      {
         //다른날이면 퀘스트가 다르다.
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/이미 지난 퀘스트임"), alertmanager.alertenum.일반);   
         MyGuildManager.Instance.GetMyGuildData();
         return;
      }
      
      //햔재 있으면
      if (MyGuildManager.Instance.myguildclassdata.questid.Equals(""))
      {
         //  Debug.Log("퀘스트가있다");
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/진행중길드임무없음"), alertmanager.alertenum.일반);

         return;
      }
      
      
      bool isConnect_guild = Backend.Chat.IsChatConnect(ChannelType.Guild);

      if (!isConnect_guild)
      {
         chatmanager.Instance.JoinGuildChannel();
         
      }
      
      SendQueue.Enqueue(Backend.Guild.GetMyGuildInfoV3, (callback) =>
      {
         // 이후 처리
         if (callback.IsSuccess())
         {
            //여기서 시간 비교해서 체크
            SendQueue.Enqueue(Backend.Guild.GetMyGuildGoodsV3, ( callbackgoods ) => 
            {
               // 이후 처리
               // 이후 처리
               //내굿즈
               if (callbackgoods.IsSuccess())
               {
                  MyGuildManager.Instance.SetGoods(callbackgoods);
                  MyGuildManager.Instance.myguilddata = callback;
                  MyGuildManager.Instance.SetMyGuildData();
                  MyGuildManager.Instance.RefreshGuildData();
                  MyGuildManager.Instance.GetApplicant();

                  if (MyGuildManager.Instance.myguildclassdata.isquestfinish)
                  {
                     //이미 완료 됨
                     RefreshQuestPanel();
                  }
                  else
                  {
                     //길드 임무 완료가 가능하다.
                     Param param = new Param { { "isquestfinish", true} };
                     RewardButton[1].SetActive(false);

                     SendQueue.Enqueue(Backend.Guild.ModifyGuildV3, param, (callbackmodi) =>
                     {
                        if (callbackmodi.IsSuccess())
                        {
                           MyGuildManager.Instance.myguildclassdata.isquestfinish = true;
                           Debug.Log("완료");
                           alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드임무완료됨"),
                              alertmanager.alertenum.일반);
                           chatmanager.Instance.ChattoFinishGuildQuest(
                              MyGuildManager.Instance.myguildclassdata.questid,
                              MyGuildManager.Instance.myguildclassdata.questcount,
                           MyGuildManager.Instance.myguildclassdata.isquestfinish);


                           GuildQuestDB.Row data =
                              GuildQuestDB.Instance.Find_id(MyGuildManager.Instance.myguildclassdata.questid);
                           
                           
                           
                           Backend.Guild.ContributeGoodsV3(goodsType.goods1, int.Parse(data.guildgold));
                           MyGuildManager.Instance.myguildclassdata.GuildGold += int.Parse(data.guildgold);
                           Backend.URank.Guild.ContributeGuildGoods(RankingManager.Instance.RankUUID[4],goodsType.goods3, int.Parse(data.guildexp));
                           MyGuildManager.Instance.myguildclassdata.curexp += int.Parse(data.guildexp);
                           Backend.Guild.ContributeGoodsV3(goodsType.goods2, int.Parse(data.guildexp));
                           MyGuildManager.Instance.myguildclassdata.GuildPt += int.Parse(data.guildexp);
                           //가능하다
                           MyGuildManager.Instance.RefreshGuildData();
                        }
                     });
                  }
               }
            });
         }
      });
   }


   public void Bt_GetFinishItemUser()
   {
      if (Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.길드임무개인보상))
      {
         List<string> id = new List<string>();
         List<string> hw = new List<string>();
         id.Add("56");
         hw.Add("200");
         Inventory.Instance.AddItem("56",200);
         Inventory.Instance.ShowEarnItem(id.ToArray(),hw.ToArray(),false);
         Savemanager.Instance.SaveInventory_SaveOn();
         RewardButton[2].SetActive(false);
      }
      else
      {
         //이미 획득한 보상
      }
   }

   public UIView GuildQuestPlayerPanel;
   public Image NowQuestItem;
   public Text NowQuestText;
   public GuildQuestMemberSlot[] questgiveslots;
   public void Bt_ShowGuildQuestPlayer()
   {
      GuildQuestPlayerPanel.Show(false);
      nowselectdata = GuildQuestDB.Instance.Find_id(MyGuildManager.Instance.myguildclassdata.questid);
      NowQuestItem.sprite = SpriteManager.Instance.GetSprite(
         ItemdatabasecsvDB.Instance.Find_id(nowselectdata.itemid).sprite);
      NowQuestText.text = Inventory.GetTranslate(nowselectdata.info);
      
      foreach (var VARIABLE in questgiveslots)
      {
         VARIABLE.gameObject.SetActive(false);
      }

      for (int i = 0; i < MyGuildManager.Instance.myguildclassdata.questgivename.Count; i++)
      {
         questgiveslots[i].Refresh(MyGuildManager.Instance.myguildclassdata.questgivename[i],
            MyGuildManager.Instance.myguildclassdata.questgivehowmany[i]);
         questgiveslots[i].gameObject.SetActive(true);
      }

   }


}
