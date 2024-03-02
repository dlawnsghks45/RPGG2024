using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using BackEnd.Tcp;
using Doozy.Engine.UI;
using LitJson;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
public class MyGuildManager : MonoBehaviour
{
//싱글톤만들기.
   private static MyGuildManager _instance = null;
   public static MyGuildManager Instance
   {
      get
      {
         if (_instance == null)
         {
            _instance = FindObjectOfType(typeof(MyGuildManager)) as MyGuildManager;
            if (_instance == null)
            {
               //Debug.Log("Player script Error");
            }
         }

         return _instance;
      }
   }

   public GameObject[] myguildpanel;
   public void ShowToggle(int num)
   {
      for(int i = 0 ; i < myguildpanel.Length;i++)
         myguildpanel[i].SetActive(false);
      switch (num)
      {
         case 0:
            GetMyGuildData();
            break;
         case 1:
            myguildpanel[1].SetActive(true);
            Bt_ShowwGuildMember();
            break;
         case 2:
            myguildpanel[2].SetActive(true);

            break;
         case 3:
            myguildpanel[3].SetActive(true);
            break;
         case 4:
            break;
      }
   }
   
   
   public GameObject Loadingbar;

   #region 내길드

   public UIView MyGuildPanel;


   //길드 엠블렘
   public GuildEmblemSlot emblem;

   public GameObject GuildLvUpBt;
   
   //길드이름
   public Text MyGuildName;

   //길드정보
   public Text[] MyGuildInfo;

   //공지사항 소개글
   public Text MyNotice;
   public Text MyWelcome;

   //길드 설정 버튼
   public GameObject GuildSettingObj;
   
   //길드버프
   public GuildBuffSlot[] GuildBuff;

   //경험치
   public Text GuildLv;
   public Text GuildExpText;
   public Image GuildExpImage;

   public UIToggle[] GuildToggle;

   //길드 데이터 클래스 
   public GuildItem myguildclassdata;
   public GameObject GuildMainPanel;
   public void OpenMyGuildPanel()
   {
      MyGuildPanel.Show(false);
      Loadingbar.SetActive(true);
      GuildMainPanel.SetActive(false);
      GuildLvUpBt.SetActive(false);

      foreach (var t in myguildpanel)
         t.SetActive(false);
      
      
      
      GetMyGuildData();
   }

   public BackendReturnObject myguilddata;

   void truegetdata()
   {
      iscangetdata = true;
   }
   
   
   
   public void RefreshGuildData()
   {
  // Debug.Log(myguildclassdata.GuildFlag);
//   Debug.Log(myguildclassdata.GuildBanner);
      emblem.SetEmblem(myguildclassdata.GuildFlag, myguildclassdata.GuildBanner);
      MyGuildName.text = myguildclassdata.guildName;
      MyNotice.text = myguildclassdata.GuildNotice;
      MyWelcome.text = myguildclassdata.GuildWelcome;

      GuildLv.text = myguildclassdata.level.ToString();
      GuildExpText.text = $"{myguildclassdata.curexp:N0} / {myguildclassdata.maxexp:N0}";
      GuildExpImage.fillAmount = myguildclassdata.curexp / myguildclassdata.maxexp;

      if (myguildclassdata.curexp >= myguildclassdata.maxexp)
      {
         GuildLvUpBt.SetActive(true);
      }
      else
      {
         GuildLvUpBt.SetActive(false);
      }
      
      
      if (myguildclassdata.masterNickname.Equals(PlayerBackendData.Instance.nickname))
      {
         GuildSettingObj.SetActive(true);
      }
      else
      {
         GuildSettingObj.SetActive(false);
      }
      
      //길드장
      MyGuildInfo[0].text = myguildclassdata.masterNickname;
      //골드
      MyGuildInfo[1].text = myguildclassdata.GuildGold.ToString("N0");
      //길드점수
      MyGuildInfo[2].text = myguildclassdata.GuildPt.ToString("N0");
      //길드원 수
      MyGuildInfo[3].text = $"{myguildclassdata.memberCount}/{myguildclassdata.GuildMaxMemer}";
      //길드 보스
       MyGuildInfo[4].text = myguildclassdata.GuildBossIDs == ""
         ? "-"
         : Inventory.GetTranslate(monsterDB.Instance.Find_id(myguildclassdata.GuildBossIDs).name);
      MyGuildInfo[5].text = myguildclassdata.GuildRaidIDs == ""
         ? "-"
         : Inventory.GetTranslate(MapDB.Instance.Find_id(myguildclassdata.GuildRaidIDs).name);
      MyGuildInfo[6].text = myguildclassdata._immediateRegistration
         ? Inventory.GetTranslate("Guild/즉시가입")
         : Inventory.GetTranslate("Guild/승인가입");
      MyGuildInfo[7].text = myguildclassdata.JoinLv.ToString();
      MyGuildInfo[8].text = myguildclassdata.JoinBp.ToString();

      //길드 버프 셋팅
      for (int i = 0; i < GuildBuff.Length; i++)
      {
         GuildBuff[i].SetGuildBuff(myguildclassdata.GuildBuffLV[i]);
      }
      
      //길드 퀘스트
      MyGuildQuestManager.Instance.RefreshQuestPanel();
      //길드 퀘스트
      GuildRaidManager.Instance.RefreshRaidRaidHp();
   }
   
   

   private bool iscangetdata = true;
   private bool isinit = false;
   public void GetMyGuildData()
   {
      MyGuildQuestManager.Instance.InitGuildQuest();
      if (!isinit)
      {
         if (myguilddata != null && PlayerBackendData.Instance.ishaveguild)
         {
            isinit = true;
            Loadingbar.SetActive(false);
            GuildMainPanel.SetActive(true);
            myguildpanel[0].SetActive(true);
            RefreshGuildData();
         }
         return;   
      }
      
      if (!iscangetdata)
      {
         //데이터불러오기오류
         if (myguilddata != null)
         {
            Loadingbar.SetActive(false);
            GuildMainPanel.SetActive(true);
            myguildpanel[0].SetActive(true);
            GuildToggle[0].ToggleOn();
         }
         return;
      }

     
      bool isConnect_guild = Backend.Chat.IsChatConnect(ChannelType.Guild);

      if (!isConnect_guild)
      {
         chatmanager.Instance.JoinGuildChannel();
         
      }
      
      myguildpanel[0].SetActive(false);
      iscangetdata = false;
      Invoke("truegetdata", 15f);

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
                  SetGoods(callbackgoods);
                  myguilddata = callback;
                  SetMyGuildData();
                  Loadingbar.SetActive(false);
                  myguildpanel[0].SetActive(true);
                  GuildMainPanel.SetActive(true);
                  RefreshGuildData();
                  GetApplicant();

               }
               else
               {
                  goodsDictionary.Clear();
               }
            });
         }
      });
   }

   
   
   #region 길드가입요청승인
   public GameObject Noti_Applicant;
   public Text Noti_Count_Applicant;
   Dictionary<string,ApplicantsItem> applicantsList = new Dictionary<string, ApplicantsItem>();
   public void GetApplicant()
   {
      if (!myguildclassdata.masterNickname.Equals(PlayerBackendData.Instance.nickname)
          && myguildclassdata.viceMasterList.ContainsKey(PlayerBackendData.Instance.playerindate))
      {
         //권한
         Debug.Log("권한이 없다");
         return;
      }
      SendQueue.Enqueue(Backend.Guild.GetApplicantsV3,20, ( callback ) => 
      {
         // 이후 처리
         if (callback.IsSuccess())
         {
            applicantsList.Clear();
            JsonData applicantsListJson = callback.FlattenRows();
            Noti_Applicant.SetActive(false);

            //목록
            for (int i = 0; i < applicantsListJson.Count; i++)
            {
               ApplicantsItem applicant = new ApplicantsItem();

               if (applicantsListJson[i].ContainsKey("nickname"))
               {
                  applicant.nickname = applicantsListJson[i]["nickname"].ToString();
               }
               applicant.inDate = applicantsListJson[i]["inDate"].ToString();

               applicantsList.Add(applicant.inDate,applicant);
            }

            if (applicantsList.Count > 0)
            {
               alertmanager.Instance.NotiGuildApplyTomine();
               Noti_Applicant.SetActive(true);
               Noti_Count_Applicant.text = applicantsList.Count.ToString();
            }

         }
      });
   }

   public GuildApplicantSlot[] ApplicantSlots;
   public UIView ApplicantPanel;
   private void RefreshApplicantPanel()
   {
      foreach (var VARIABLE in ApplicantSlots)
      {
         VARIABLE.gameObject.SetActive(false);
      }
      
      if (applicantsList.Count == 0)
      {
         //알림을 모두 지움
         Noti_Applicant.SetActive(false);
         alertmanager.Instance.FinishApply();
         ApplicantPanel.Hide(true);
         return;
      }

      
      //목록 시작
      int num = 0;
      foreach (var VARIABLE in applicantsList)
      {
         ApplicantSlots[num].RefreshData(VARIABLE.Value);
         ApplicantSlots[num].gameObject.SetActive(true);
         num++;
      }
   }

   private bool applicantfalse;

   void falseapplicant()
   {
      applicantfalse = false;
   }
   public void Bt_OpenApplicantPanel()
   {
      if (!applicantfalse)
      {
         GetApplicant();
         applicantfalse = true;
         Invoke(nameof(falseapplicant), 10f);
      }
      if (applicantsList.Count == 0)
      {
         //알림을 모두 지움
         Noti_Applicant.SetActive(false);
         alertmanager.Instance.FinishApply();
         return;
      }
      RefreshApplicantPanel();
      ApplicantPanel.Show();
   }

   //가입 승인한다.
   public void ApplyApplicantPlayer(string indate)
   {
      if (myguildclassdata.memberCount >= myguildclassdata.GuildMaxMemer)
      {
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드원꽉참"),alertmanager.alertenum.일반);
         return;
      }
      
      SendQueue.Enqueue(Backend.Guild.ApproveApplicantV3, indate ,( callback ) => 
      {
         if (callback.IsSuccess())
         {
            // 이후 처리
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드가입완료"),alertmanager.alertenum.일반);
            applicantsList.Remove(indate);
            myguildclassdata.memberCount++;
            RefreshGuildData();
            RefreshApplicantPanel();
         }
         else
         {
            switch (callback.GetStatusCode())
            {
               case"412":
                  alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/이미다른소속"),alertmanager.alertenum.일반);
                  break;
               case"429": //길드 꽉차면 이 부분은 누를 때 길드 체크해야할듯
                  alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드원꽉참"),alertmanager.alertenum.일반);
                  break;
               case"403": //길드마스터 혹은 운영진이 아닐 경우
                  break;
            }
         }
      });
   }
//승인 거절
   public void ReJectApplicant(string indate)
   {
      SendQueue.Enqueue(Backend.Guild.RejectApplicantV3, indate, ( callback ) => 
      {
         Debug.Log(callback);
         if (callback.IsSuccess())
         {
            applicantsList.Remove(indate);
            RefreshApplicantPanel();
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드가입거절"),alertmanager.alertenum.일반);
         }
         else
         {
            
         }
         // 이후 처리
      });
   }
   

      #endregion
   
   Dictionary<string, GoodsItem> goodsDictionary = new Dictionary<string, GoodsItem>();

   public void SetGoods(BackendReturnObject bro)
   {
      goodsDictionary.Clear();
      var goodsJson = bro.GetFlattenJSON()["goods"];
      foreach (var column in goodsJson.Keys)
      {
         if (column.Contains("totalGoods"))
         {
            GoodsItem goodsItem = new GoodsItem();

            goodsItem.totalGoodsAmount = Int32.Parse(goodsJson[column].ToString());

            string goodsNum = column.Replace("totalGoods", "");
            goodsNum = goodsNum.Replace("Amount", "");

            string goodsName = "goods" + goodsNum + "UserList";

            JsonData userListJson = goodsJson[goodsName];
            for (int i = 0; i < userListJson.Count; i++)
            {
               GoodsUserItem user = new GoodsUserItem();

               user.inDate = userListJson[i]["inDate"].ToString();
               user.nickname = userListJson[i]["nickname"].ToString();
               if (userListJson[i].ContainsKey("usingTotalAmount"))
               {
                  user.usingTotalAmount = Int32.Parse(userListJson[i]["usingTotalAmount"].ToString());
               }

               user.totalAmount = Int32.Parse(userListJson[i]["totalAmount"].ToString());
               user.updatedAt = userListJson[i]["updatedAt"].ToString();

               goodsItem.userList.Add(user.inDate,user);
            }

            goodsDictionary.Add("goods" + goodsNum, goodsItem);
         }
      }
   }

   public void SetMyGuildData()
   {
      JsonData json = myguilddata.GetFlattenJSON();
      GuildItem guildItem = new GuildItem();
      
      
      //여기서 시간 체크 저장된 date와 불러온 date가 같다면 하루 올리고 초기화 1단 차 체크로 지금 시간을 가졍오고 다르면 
      //불러와서 비교 
      //저장된 시간이 없다면 바로 초기화     
      bool isreset = false;
      if (json["guild"].ContainsKey("GuildDailyTime"))
      {
         guildItem.GuildDailyTime = DateTime.Parse(json["guild"]["GuildDailyTime"].ToString());

         //   Debug.Log(guildItem.GuildDailyTime);
//         Debug.Log(Timemanager.Instance.NowTime.Date);

         Timemanager.Instance.RefreshNowTIme();
         if (Timemanager.Instance.NowTime.Date >= guildItem.GuildDailyTime.Date)
         {
            #region 초기화
            //시간이 다르면 재비교 
            isreset = true; //리셋이므로 리셋되는 건 안갖져오게 
            //시간이 같다 초기화하자
            guildItem.ReSetData();
            //하루 올린다,

            //길드 리셋 
            Param param = new Param
            {
               { "GuildDailyTime", Timemanager.Instance.NowTime.Date.AddDays(1).Date },
               { "questid", guildItem.questid },
               { "questcount", guildItem.questcount },
               { "isquestfinish", guildItem.isquestfinish },
               { "questgivename", guildItem.questgivename },
               { "questgivehowmany", guildItem.questgivehowmany },
               { "GuildRaidRankNames", guildItem.GuildRaidRankNames },
               { "GuildRaidRankDmgs", guildItem.GuildRaidRankDmgs },
               { "GuildRaidIDs", guildItem.GuildRaidIDs },
               { "isRaidfinish", guildItem.isRaidfinish }

            };

            SendQueue.Enqueue(Backend.Guild.ModifyGuildV3, param, (callback) =>
            {
               // 이후 처리
               if (callback.IsSuccess())
               {
                  //     Debug.Log("길드 일일 초기화 완료 ");
               }
            });

            #endregion
         }
      }
      //없으면 시간을 추가 
      else
      {
         //길드 리셋 
         Param param = new Param
         {
            { "GuildDailyTime", Timemanager.Instance.NowTime.Date.AddDays(1).Date },
         };
         
         guildItem.GuildDailyTime = Timemanager.Instance.NowTime.Date.AddDays(1).Date;

         SendQueue.Enqueue(Backend.Guild.ModifyGuildV3, param, (callback) =>
         {
            // 이후 처리
            if (callback.IsSuccess())
            {
               Debug.Log("서버에 시간을 넣었다");
            }
         });
      }
      
      //기본 데이터
      guildItem.memberCount = Int32.Parse(json["guild"]["memberCount"].ToString());
      guildItem.masterNickname = json["guild"]["masterNickname"].ToString();
      guildItem.inDate = json["guild"]["inDate"].ToString();
      guildItem.guildName = json["guild"]["guildName"].ToString();
      guildItem.goodsCount = Int32.Parse(json["guild"]["goodsCount"].ToString());

      //메타 데이터
      guildItem.level = Int32.Parse(json["guild"]["level"].ToString());
      try
      {
         guildItem.curexp =  goodsDictionary["goods2"].totalGoodsAmount;

      }
      catch (Exception e)
      {
         guildItem.curexp = 0;
      }

      try
      {
         guildItem.GuildGold = goodsDictionary["goods1"].totalGoodsAmount;
      }
      catch (Exception e)
      {
         guildItem.GuildGold = 0;
      }
      try
      {
         guildItem.GuildPt = goodsDictionary["goods3"].totalGoodsAmount;
      }
      catch (Exception e)
      {
         guildItem.GuildPt = 0;
      }

      //guildItem.curexp = float.Parse(json["guild"]["Exp"].ToString());
      guildItem.maxexp = (guildItem.level * 700); //레벨마다 700오름

      guildItem.GuildMaxMemer = Int32.Parse(json["guild"]["GuildMaxMemer"].ToString());
      guildItem.GuildFlag = Int32.Parse(json["guild"]["GuildFlag"].ToString());
      guildItem.GuildBanner = Int32.Parse(json["guild"]["GuildBanner"].ToString());
      //guildItem.GuildGold = decimal.Parse(json["guild"]["GuildGold"].ToString());
      guildItem.JoinLv = Int32.Parse(json["guild"]["JoinLv"].ToString());
      guildItem.JoinBp = Int32.Parse(json["guild"]["JoinBp"].ToString());

      guildItem.GuildWelcome = json["guild"]["GuildWelcome"].ToString();
      guildItem.GuildNotice = json["guild"]["GuildNotice"].ToString();


      //길드버프 레벨 가져오기
      guildItem.GuildBuffLV = new int[json["guild"]["GuildBuffLV"].Count];
      guildItem.GuildBuffLV.Initialize();
      for (int i = 0; i < json["guild"]["GuildBuffLV"].Count; i++)
      {
         guildItem.GuildBuffLV[i] = Int32.Parse(json["guild"]["GuildBuffLV"][i].ToString());
      }

      if (json["guild"].ContainsKey("_immediateRegistration"))
      {
         guildItem._immediateRegistration =
            json["guild"]["_immediateRegistration"].ToString() == "True" ? true : false;
      }

      if (json["guild"].ContainsKey("_countryCode"))
      {
         guildItem._countryCode = json["guild"]["_countryCode"].ToString();
      }
      
      
         
      //길드버프 레벨 가져오기
      guildItem.questgivename.Clear();
      //길드버프 레벨 가져오기
      guildItem.questgivehowmany.Clear();
      
      //길드 레이드
      //길드레이드 아이디
      guildItem.GuildRaidRankNames.Clear();
      guildItem.GuildRaidRankDmgs.Clear();
      
      //길드 퀘스트
      if (!isreset)
      {
         //리셋되는것들
         //퀘스트
         guildItem.questid = json["guild"].ContainsKey("questid") ? json["guild"]["questid"].ToString() : "";

         guildItem.questcount = json["guild"].ContainsKey("questcount")
            ? int.Parse(json["guild"]["questcount"].ToString())
            : 0;
         guildItem.isquestfinish = json["guild"].ContainsKey("isquestfinish")
            ? bool.Parse(json["guild"]["isquestfinish"].ToString())
            : false;
         
       //  Debug.Log("여기3");

         if (json["guild"].ContainsKey("questgivename"))
         {
            for (int i = 0; i < json["guild"]["questgivename"].Count; i++)
            {
               guildItem.questgivename.Add(json["guild"]["questgivename"][i].ToString());
               guildItem.questgivehowmany.Add(int.Parse(json["guild"]["questgivehowmany"][i].ToString()));
            }
         }
       //  Debug.Log("여기4");

         if (json["guild"].ContainsKey("GuildRaidIDs") &&
             json["guild"].ContainsKey("GuildRaidCurHPs"))
         {
            guildItem.GuildRaidIDs = json["guild"]["GuildRaidIDs"].ToString();
            guildItem.GuildRaidCurHPs = decimal.Parse(json["guild"]["GuildRaidCurHPs"].ToString(),System.Globalization.NumberStyles.Float);
         }
         else
         {
            guildItem.GuildRaidIDs = "";
            guildItem.GuildRaidCurHPs = 0;
         }
    //     Debug.Log("여기5");

         if (json["guild"].ContainsKey("GuildRaidRankNames"))
         {
            
            for (int i = 0; i < json["guild"]["GuildRaidRankNames"].Count; i++)
            {
               try
               {
//                  Debug.Log(json["guild"]["GuildRaidRankDmgs"][i].ToString());
                  decimal dmg = decimal.Parse(json["guild"]["GuildRaidRankDmgs"][i].ToString(),System.Globalization.NumberStyles.Float);
                  //    Debug.Log(dmg);
                  guildItem.GuildRaidRankNames.Add(json["guild"]["GuildRaidRankNames"][i].ToString());
                  guildItem.GuildRaidRankDmgs.Add(Decimal.Truncate(dmg));
               }
               catch
               {
                  decimal dmg = -1;
                  //    Debug.Log(dmg);
                  guildItem.GuildRaidRankNames.Add(json["guild"]["GuildRaidRankNames"][i].ToString());
                  guildItem.GuildRaidRankDmgs.Add(Decimal.Truncate(dmg));
                  throw;
               }
            
            }
         }

       //  Debug.Log("여기7");

         if (json["guild"].ContainsKey("raidacceptdate"))
         {
            guildItem.raidacceptdate = DateTime.Parse(json["guild"]["raidacceptdate"].ToString());
         }
         else
            guildItem.raidacceptdate = DateTime.MinValue;
         
//         Debug.Log("여기8");

         if (json["guild"].ContainsKey("questacceptdate"))
         {
            guildItem.questacceptdate = DateTime.Parse(json["guild"]["questacceptdate"].ToString());
         }
         else
            guildItem.questacceptdate = DateTime.MinValue;
    //     Debug.Log("여기9");


         if (json["guild"].ContainsKey("GuildBossIDs"))
         {
            guildItem.GuildBossIDs = json["guild"]["GuildBossIDs"].ToString();
         }
         else
            guildItem.GuildBossIDs = "";
      }
      
      guildItem.masterInDate = json["guild"]["masterInDate"].ToString();
      JsonData viceListJson = json["guild"]["viceMasterList"];
      for (int j = 0; j < viceListJson.Count; j++)
      {
         guildItem.viceMasterList.Add(viceListJson[j]["inDate"].ToString(),
            viceListJson[j]["nickname"].ToString());
      }

      myguildclassdata = guildItem;
   }
   
   #endregion


   public void Bt_RecoGuildOn()
   {
         SendQueue.Enqueue(Backend.RandomInfo.SetRandomData, RandomType.Guild, "", 1, callback =>
         {

            // 이후 처리
         });
   }

   public void Bt_RecoGuildOff()
   {
      
   }

   #region 길드멤버

   public Dictionary<string, GuildMemberInfo> guildMember = new Dictionary<string, GuildMemberInfo>();

   
   private bool isguildmembershow = false;
   private BackendReturnObject memberBro;
   void truegetmember()
   {
      isguildmembershow = false;
   }

   public GameObject memberloadingbar;
   public void Bt_ShowwGuildMember()
   {
      for (int i = 0; i < myguildmemberslot.Length; i++)
      {
         myguildmemberslot[i].gameObject.SetActive(false);
      }
      memberloadingbar.SetActive(true);
      SetGuildMemberInfo();
   }
   public void SetGuildMemberInfo()
   {
      if (isguildmembershow)
      {
         if(guildMember.Count != 0)
            SetGuildMemberUI();

         return;
      }

      Invoke(nameof(truegetmember), 15);
      isguildmembershow = true;
      
      
      Backend.Notification.OnIsConnectUser = (bool isConnect, string nickName, string gamerIndate) => // 핸들러 설정
      {
         //Debug.Log($"{nickName}({gamerIndate}) 님의 접속 여부 : {isConnect}");

         if (guildMember.TryGetValue(gamerIndate, out var value))
         {
            value.ChangeActive(isConnect); // 접속 여부 적용
         }
      };

      SendQueue.Enqueue(Backend.Guild.GetGuildMemberListV3, myguildclassdata.inDate, (callback) =>
      {
         // 이후 처리
         if (!callback.IsSuccess())
            return;

     
         if (callback.IsSuccess())
         {
            memberBro = callback;
            JsonData json = memberBro.FlattenRows(); // json으로 변환
            int maxMemberCount = json.Count; // 길드 멤버 수 확인
            guildMember.Clear();
            for (int i = 0; i < maxMemberCount; i++) // 
            {
               string nick = json[i]["nickname"].ToString(); // 닉네임 가져오기
               string indate = json[i]["gamerInDate"].ToString(); // inDate 가져오기
               string lastLogin = json[i]["lastLogin"].ToString(); // 최종접속일 확인하기
               string position = json[i]["position"].ToString(); // 최종접속일 확인하기
               GuildMemberInfo info = new GuildMemberInfo(indate, nick, lastLogin,position); // 길드 멤버 정보 생성
               guildMember.Add(indate, info); // inDate(고유 id)를 Key로 Dictionary에 추가
               Debug.Log(indate);
               Backend.Notification.UserIsConnectByIndate(indate); // 해당 유저가 접속중인지 확인
            }

            Invoke(nameof(SetGuildMemberUI), 0.2f);
         }
      });
   }
   void SetGuildMemberUI()
   {
    
      memberloadingbar.SetActive(false);

      int num =0;
      foreach(var obj in guildMember)
      {
      //   Debug.Log(obj.ToString());
         //이외 게임 오브젝트들을 생성하여 UI로 표시해준다.
         //실시간 알림 기능은 비동기이기에 접속 여부 표시가 조금 뒤에 순차적으로 표시될 수 있다.
         int pt = 0; 
         try
         {
            pt = goodsDictionary["goods3"].userList[obj.Value.indate].totalAmount;
         }
         catch (Exception e)
         {
         }
         myguildmemberslot[num].Refresh(true,obj.Value,pt);
         myguildmemberslot[num].gameObject.SetActive(true);
         myguildmemberslot[num].transform.SetAsFirstSibling();
         num++;
      }
   }
   public guildmemberslot[] myguildmemberslot;
   #endregion


   #region 길드설정

   public UIView GuildSettingPanel;
   public InputField NoticeInput;
   public InputField WelcomeInput;
   public InputField JoinLevel;
   public InputField JoinBattlePoint;
   public UIToggle JoinNowToggle;

   public void Bt_OpenGuildSetting()
   {
      GuildSettingPanel.Show(false);
      NoticeInput.text = myguildclassdata.GuildNotice;
      WelcomeInput.text = myguildclassdata.GuildWelcome;
      JoinLevel.text = myguildclassdata.JoinLv.ToString();
      JoinBattlePoint.text = myguildclassdata.JoinBp.ToString();
      JoinNowToggle.IsOn = myguildclassdata._immediateRegistration;
   }

   private bool iscanchangesetting = false;

   void falsesetting()
   {
      iscanchangesetting = false;
   }
   public void Bt_AcceptAllSetting()
   {
      if (iscanchangesetting)
      {
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/30초마다가능"),alertmanager.alertenum.일반);
         return;
      }

      iscanchangesetting = true;
      Invoke(nameof(falsesetting),30);
      //모든 설정을 저장
      Param param = new Param
      {
         { "GuildNotice", NoticeInput.text },
         { "GuildWelcome", WelcomeInput.text },
         { "JoinLv", JoinLevel.text },
         { "JoinBp", JoinBattlePoint.text },
      };
      SendQueue.Enqueue(Backend.Guild.SetRegistrationValueV3, JoinNowToggle.IsOn, ( callback ) =>  // 즉시 가입 설정
      {
         // 이후 처리
      });
      SendQueue.Enqueue(Backend.Guild.ModifyGuildV3, param, ( callback ) =>
      {
         // 이후 처리
         if (callback.IsSuccess())
         {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드설정완료"),alertmanager.alertenum.일반);
            GetMyGuildData();
            RefreshGuildData();
            GuildSettingPanel.Hide(false);
         }
      });
   }
   #endregion

   #region 길드탈퇴

   public void OffGuildAll()
   {
      //탈퇴성공
      PlayerBackendData.Instance.ishaveguild = false;
      OffSetBuffStat();
      MyGuildPanel.Hide(true);
      myguilddata = null;
      myguildclassdata = null;
      //길드버프삭제
      chatmanager.Instance.OffGuild();
      alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드추방됨"), alertmanager.alertenum.일반);
   }
   public void Bt_AcceptWithDraw()
   {
      SendQueue.Enqueue(Backend.Guild.WithdrawGuildV3, (callback) =>
      {
         // 이후 처리
         if (callback.IsSuccess())
         {
            //탈퇴성공
            OffGuildAll();
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드탈퇴성공"), alertmanager.alertenum.일반);
            return;
         }

         switch (callback.GetStatusCode())
         {
            case "412":
               if (myguildclassdata.masterNickname.Equals(PlayerBackendData.Instance.nickname))
               {
                  //길드마스터면 못나감  
                  alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길마는못나감_탈퇴"), alertmanager.alertenum.일반);
               }
               else
               {
                  //길드멤버가아님
                  alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드멤버아님_탈퇴"), alertmanager.alertenum.일반);

               }

               break;
         }

      });
   }



   #endregion


   #region  길드원정보

   public UIView GuildMemberSettingPanel;
   public Text GuildMemberInfoName;
   public Toggle[] GuildMemberGradeToggle;
   private GuildMemberInfo SelectMemberdata;

   public void ShowGuildMemberInfo(GuildMemberInfo info)
   {
      GuildMemberSettingPanel.Show(false);
      SelectMemberdata = info;
      GuildMemberInfoName.text = info.nickname;
      switch (info.position)
      {
         case "master":
            GuildMemberGradeToggle[2].isOn = true;     
            break;
         
         case "viceMaster":
            GuildMemberGradeToggle[1].isOn = true;     

            break;
         case "member":
            GuildMemberGradeToggle[0].isOn = true;     
            break;
      }
   }
   
   //길드원 추방
   public void Bt_GetOutPlayer()
   {
      SendQueue.Enqueue(Backend.Guild.ExpelMemberV3, SelectMemberdata.indate, ( callback ) => 
      {
         if (callback.IsSuccess())
         {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드원추방성공"),alertmanager.alertenum.일반);
            chatmanager.Instance.ChattoGuildWithDraw(SelectMemberdata.nickname);
            guildMember.Remove(SelectMemberdata.indate);
            myguildclassdata.memberCount--;
            Bt_ShowwGuildMember();
         }
         else
         {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/잠시후시도"),alertmanager.alertenum.일반);
         }
      });
      GuildMemberSettingPanel.Hide(true);
   }

   public void Bt_ChangePlayerGrade()
   {//(master/viceMaster/member)
      if (GuildMemberGradeToggle[0].isOn)
      {
         //길드원
         if (SelectMemberdata.position.Equals("member")) return;
         //하락
         SendQueue.Enqueue(Backend.Guild.ReleaseViceMasterV3, SelectMemberdata.indate, ( callback ) => 
         {
            if (callback.IsSuccess())
            {
               alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/직책변경됨"),alertmanager.alertenum.일반);
               guildMember[SelectMemberdata.indate].position = "member";
               chatmanager.Instance.ChattoGuildChangePosition(SelectMemberdata.indate,"member");
               Bt_ShowwGuildMember();
            }
            else
            {
               alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/잠시후시도"),alertmanager.alertenum.일반);
            }
         });
         
      }
      else if (GuildMemberGradeToggle[1].isOn)
      {
         //부길드마
         if (SelectMemberdata.position.Equals("viceMaster")) return;
         
         SendQueue.Enqueue(Backend.Guild.NominateViceMasterV3, SelectMemberdata.indate, ( callback ) => 
         {
            if (callback.IsSuccess())
            {
               alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/직책변경됨"),alertmanager.alertenum.일반);
               guildMember[SelectMemberdata.indate].position = "viceMaster";
               chatmanager.Instance.ChattoGuildChangePosition(SelectMemberdata.indate,"viceMaster");
               Bt_ShowwGuildMember();
            }
            else
            {
               alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/잠시후시도"),alertmanager.alertenum.일반);
            }
         });
         //부길드마스터로 승격
         

      }
      else if(GuildMemberGradeToggle[2].isOn)
      {
         //길마
         if (SelectMemberdata.position.Equals("master")) return;
         
         SendQueue.Enqueue(Backend.Guild.NominateMasterV3, SelectMemberdata.indate, ( callback ) => 
         {
            if (callback.IsSuccess())
            {
               alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/직책변경됨"),alertmanager.alertenum.일반);
               guildMember[SelectMemberdata.indate].position = "master";
               chatmanager.Instance.ChattoGuildChangePosition(SelectMemberdata.indate,"master");

               isguildmembershow = false; //길드마스터는 강제로바꿔야함
               Bt_ShowwGuildMember();
            }
            else
            {
               alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/잠시후시도"),alertmanager.alertenum.일반);
            }
            // 이후 처리
         });
      }
      GuildMemberSettingPanel.Hide(true);
   }

   #endregion



   #region 길드버프

   public UIView GuildBuffPanel;
   public Image BuffImage;
   public Text BuffName;
   public Text BuffInfo;

   //혅재 다음 관련 맥스 레벨이면 next와 arroww를 끈다
   public Text NowStat;
   public Text NextStat;
   public GameObject BuffNextObj;
   public GameObject BuffArro;

   public GameObject BuffLvUpBt;
   public GameObject NeedGoldPanel;
   public GameObject BuffLock;
   public Text BuffLvNeedGold;
   public Text BuffLvMyGold;
   public int NowPickbuffindex;
   private GuildBuffDB.Row Buffdata;
   public void Bt_ShowGuildBuff(string id, string bufflv,bool ismine)
   {
      Buffdata =
         GuildBuffDB.Instance.Find_id(id);
      float stat = float.Parse(Buffdata.upperlv);
      int lv = int.Parse(bufflv);
      NowPickbuffindex = int.Parse(Buffdata.num);
      GuildBuffPanel.Show(false);

      BuffLock.SetActive(true);
      if (myguildclassdata.masterNickname.Equals(PlayerBackendData.Instance.nickname))
         BuffLock.SetActive(false);
      
      
      BuffImage.sprite = SpriteManager.Instance.GetSprite(Buffdata.sprite);

      BuffName.text = $"Lv.{bufflv} {Inventory.GetTranslate(Buffdata.name)}";

      BuffInfo.text = string.Format(Inventory.GetTranslate(Buffdata.info), bool.Parse(Buffdata.ispercent) ? (stat*100f).ToString("N0") 
         : stat.ToString("N0"));


      NowStat.text = bool.Parse(Buffdata.ispercent) ? $"{((stat*lv) * 100f):N0}%" : (stat * lv).ToString("N0");

      if (bufflv != Buffdata.maxlv && ismine)
      {
         BuffArro.SetActive(true);
         BuffNextObj.SetActive(true);
         
         //다음레벨이 있
         BuffLvUpBt.SetActive(true);
         NeedGoldPanel.SetActive(true);
         BuffLvMyGold.text = myguildclassdata.GuildGold.ToString();
         BuffLvNeedGold.text = (Int32.Parse(Buffdata.upgradeGold) * lv).ToString();
         NextStat.text = bool.Parse(Buffdata.ispercent) ? $"{(((stat*lv)+stat) * 100f):N0}%" : ((stat*lv)+stat).ToString("N0");
      }
      else
      {
         BuffArro.SetActive(false);
         BuffNextObj.SetActive(false);
         BuffLvUpBt.SetActive(false);
         NeedGoldPanel.SetActive(false);
      }


   }

   private bool falsebufflv = false;

   private void Falsebufflv()
   {
      falsebufflv = false;
   }
   //버프 레벨업
   public void Bt_lvupbufflv()
   {
      if (falsebufflv)
      {
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/10초뒤"), alertmanager.alertenum.일반);
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
                  SetGoods(callbackgoods);
                  myguilddata = callback;
                  SetMyGuildData();
                  Loadingbar.SetActive(false);
                  myguildpanel[0].SetActive(true);

                  if (myguildclassdata.level == myguildclassdata.GuildBuffLV[NowPickbuffindex])
                  {
                     alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드버프가최대레벨"),
                        alertmanager.alertenum.일반);
                     falsebufflv = true;
                     Invoke("Falsebufflv",10);
                     return;
                  }
                  else if (myguildclassdata.level > myguildclassdata.GuildBuffLV[NowPickbuffindex] 
                           && myguildclassdata.GuildBuffLV[NowPickbuffindex] != 10 )
                  {
                     //가능
                     //골드비교
                     if (goodsDictionary["goods1"].totalGoodsAmount >=
                         int.Parse(Buffdata.upgradeGold) * myguildclassdata.GuildBuffLV[NowPickbuffindex])
                     {
                        Debug.Log("길드 버프 성공");
                        int amount = -(int.Parse(Buffdata.upgradeGold) * myguildclassdata.GuildBuffLV[NowPickbuffindex]);

                        Debug.Log(amount);
                        SendQueue.Enqueue(Backend.Guild.UseGoodsV3, goodsType.goods1, amount, (callback2) =>
                        {
                           Debug.Log(callback2);
                           // 이후 처리
                           if (callback2.IsSuccess())
                           {
                              myguildclassdata.GuildBuffLV[NowPickbuffindex]++;
                              if (NowPickbuffindex.Equals(10))
                              {
                                 myguildclassdata.GuildMaxMemer =
                                    20 + (3 * myguildclassdata.GuildBuffLV[NowPickbuffindex]);
                              }
                              Param param = new Param
                              {
                                 { "GuildBuffLV", myguildclassdata.GuildBuffLV }, //레벨 1 
                                 { "GuildMaxMemer", myguildclassdata.GuildMaxMemer } //레벨 1 
                              };
                              BackendReturnObject bro = Backend.Guild.ModifyGuildV3(param);
                              if (bro.IsSuccess())
                              {
                                 //레벨업

                                 myguildclassdata.GuildGold += amount;
                                 RefreshGuildData();
                                 Bt_ShowGuildBuff(Buffdata.id,
                                    myguildclassdata.GuildBuffLV[NowPickbuffindex].ToString(), true);
                                 alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/버프레벨업성공"),
                                    alertmanager.alertenum.일반);
                                 //버프 채팅 발사
                                 chatmanager.Instance.ChattoGuildBuffUp(NowPickbuffindex,myguildclassdata.GuildBuffLV[NowPickbuffindex]);
                                 PlayerData.Instance.RefreshPlayerstat();
                              }
                              else
                              {
                                 Backend.Guild.ContributeGoodsV3(goodsType.goods1, amount);
                              }
                           }
                           else
                           {
                              
                              myguildclassdata.GuildBuffLV[NowPickbuffindex]--;
                              alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/10초뒤"),
                                 alertmanager.alertenum.일반);
                              falsebufflv = true;
                              Invoke("Falsebufflv",10);
                           }
                        });
                     }
                     else
                     {
                        //골드가 부족
                        falsebufflv = true;
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드골드부족"),
                           alertmanager.alertenum.일반);
                        Invoke("Falsebufflv",10);
                     }
                  }

                  RefreshGuildData();
               }
            });


         }
      });
   }

   #endregion

   public void Bt_GuildLvUp()
   {
      if (myguildclassdata.level == 10)
      {
         alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/최대레벨"), alertmanager.alertenum.일반);
         return;
      }
      if (myguildclassdata.curexp >= myguildclassdata.maxexp)
      {
         Debug.Log("길드 레벨업 성공");
         Param param = new Param
         {
            { "level", myguildclassdata.level + 1 } //레벨 1 
         };
         int amount = -(int)myguildclassdata.maxexp;
         SendQueue.Enqueue(Backend.Guild.UseGoodsV3, goodsType.goods2, amount, (callback) =>
         {
            // 이후 처리
            if (callback.IsSuccess())
            {
               BackendReturnObject bro = Backend.Guild.ModifyGuildV3(param);
               if (bro.IsSuccess())
               {
                  //레벨업
                  Debug.Log("레벨업성공");
                  alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/레벨업성공"), alertmanager.alertenum.일반);
                  myguildclassdata.level++;
                  myguildclassdata.curexp -= myguildclassdata.maxexp;
                  myguildclassdata.maxexp = (myguildclassdata.level * 700); //레벨마다 700오름
                  chatmanager.Instance.ChattoGuildLvupGuild(myguildclassdata.level.ToString(), myguildclassdata.GuildGold.ToString(),
                     myguildclassdata.curexp.ToString());
                  RefreshGuildData();
               }
               else
               {
                  Backend.URank.Guild.ContributeGuildGoods(RankingManager.Instance.RankUUID[4],goodsType.goods3, amount);
               }
            }
         });
      }
   }

   public void Bt_GuildRecommand()
   {
      SendQueue.Enqueue(Backend.RandomInfo.SetRandomData, RandomType.Guild, "cb5a9f10-069d-11ee-b7ff-45e1842bef31",  1, callback => 
      {
         Debug.Log(callback);
         if (callback.IsSuccess())
         {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI5/길드홍보완료"),alertmanager.alertenum.일반);
            Debug.Log("내길드에 넣기 성공");   
         }
         // 이후 처리
      });
   }
   public void Bt_RemoveGuildRecommand()
   {
      SendQueue.Enqueue(Backend.RandomInfo.DeleteRandomData, RandomType.Guild, "cb5a9f10-069d-11ee-b7ff-45e1842bef31", callback => 
      {
         if (callback.IsSuccess())
         {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI5/길드추천삭제"),alertmanager.alertenum.일반);
            Debug.Log("내길드에 넣기 성공");   
         }
         // 이후 처리
      });
   }

   
   
   #region  길드 출석

   public UIButton AddendanceBt;
   public UIView AddendancePanel;
   //출석 체크
   public void Bt_attendance()
   {
      if (Timemanager.Instance.ConSumeCount_DailyAscny(Timemanager.ContentEnumDaily.길드출석))
      {
         //골드
         SendQueue.Enqueue(Backend.Guild.ContributeGoodsV3, goodsType.goods1, 100, (callback) =>
         {
            // 이후 처리
         });
         //경험치
//         Backend.URank.Guild.ContributeGuildGoods(RankingManager.Instance.RankUUID[4],goodsType.goods2, amount);

          SendQueue.Enqueue(Backend.URank.Guild.ContributeGuildGoods, RankingManager.Instance.RankUUID[4], goodsType.goods3, 30, callback => {
                     // 이후 처리
                  });
        
         SendQueue.Enqueue(Backend.Guild.ContributeGoodsV3, goodsType.goods2, 30, (callback) =>
         {
            // 이후 처리
            if (callback.IsSuccess())
            {
               myguildclassdata.curexp += 30;
               myguildclassdata.GuildPt += 30;
               myguildclassdata.GuildGold += 100;
               RefreshGuildData();
               //토콘 지급
               List<string> id = new List<string>();
               List<string> hw = new List<string>();
               id.Add("56");
               hw.Add("50");
               Inventory.Instance.AddItem("56", 50);
               Inventory.Instance.ShowEarnItem(id.ToArray(), hw.ToArray(), false);
               Savemanager.Instance.SaveInventory_SaveOn();
               alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/출석완료"),alertmanager.alertenum.일반);
               AddendancePanel.Hide(true);
               AddendanceBt.Interactable = false;
               
            }
         });
      }
      else
      {
         AddendancePanel.Hide(true);
         AddendanceBt.Interactable = false;
      }
   }
   #endregion
[SerializeField]
   float[] GuildBuffStat = new float[(int)GuildBuffEnum.Length];
   public GuildEmblemSlot GuildBuffObjEmblem; //버프창에 길드 효과를 나오게함
   public GameObject GuildBuffObj; //버프창에 길드 효과를 나오게함
   public float GetBuffStat(GuildBuffEnum enums)
   {
      return GuildBuffStat[(int)enums];
   }

   public void SetBuffStat()
   {
      if (!PlayerBackendData.Instance.ishaveguild) return;
      GuildBuffStat = new float[(int)GuildBuffEnum.Length];
      GuildBuffStat.Initialize();
//      Debug.Log("길드버프 적용중");
      for (int i = 0; i < GuildBuffDB.Instance.NumRows(); i++)
      {
         GuildBuffStat[i] = myguildclassdata.GuildBuffLV[i] * float.Parse(GuildBuffDB.Instance.GetAt(i).upperlv);
      }

      GuildBuffObjEmblem.SetEmblem(myguildclassdata.GuildFlag, myguildclassdata.GuildBanner);
      GuildBuffObj.SetActive(true);
      GuildRaidManager.Instance.RefreshRaidRaidHp();
   }

   public void OffSetBuffStat()
   {
      for (int i = 0; i < GuildBuffStat.Length; i++)
      {
         GuildBuffStat[i] = 0;
      }

      GuildBuffObj.SetActive(false);
      PlayerData.Instance.RefreshPlayerstat();
   }




   //0은 길마만 1은 길마랑 부길마 3은 길드원
   public bool iscanaccept(int num)
   {
      
      switch (num)
      {
         //영자만
         case 0:
            if (myguildclassdata.masterNickname.Equals(PlayerBackendData.Instance.nickname))
            {
               return true;
            }
            else
            {
               return false;
            }
            break;
         case 1:
            if (myguildclassdata.masterNickname.Equals(PlayerBackendData.Instance.nickname))
            {
               return true;
            }

            if (myguildclassdata.viceMasterList.ContainsKey(PlayerBackendData.Instance.playerindate))
            {
               return true;
            }

            break;
         case 3:
            break;
      }

      //안걸리면 패스
      return false;
   }
   
   
   
   
   
   
   
   
   
   
   
   
   public enum GuildBuffEnum
   {
      모든능력치증가,
      생명력증가,
      정신력증가,
      재사용시간감소,
      치명타확률증가,
      치명타피해증가,
      보스피해증가,
      무력화피해증가,
      경험치증가,
      골드획득량증가,
      길드원수증가,
      자동보상효율증가,
      드롭율증가,
      제작시간감소,
      Length
   }

}


public class GuildItem
{
   //메타데이터
   public int GuildFlag;
   public int GuildBanner;
   public int[] GuildBuffLV;
   public int level;
   public float curexp;
   public float maxexp;
   public string GuildNotice;
   public string GuildWelcome;
   public int GuildMaxMemer;
   public int GuildPt;
   public int JoinLv;
   public int JoinBp;
   public decimal GuildGold;

   //레이드보스
   public string GuildBossIDs;
   
   
   public string GuildRaidIDs;
   public decimal GuildRaidCurHPs;
   public List<string> GuildRaidRankNames  = new List<string>();
   public List<decimal> GuildRaidRankDmgs = new List<decimal>();
   public bool isRaidfinish;

   //길드레이드 수락날짜
   public DateTime raidacceptdate;
   
   
   //길드퀘스트
   public string questid;
   public int questcount;
   public bool isquestfinish;
   
   public List<string> questgivename  = new List<string>();
   public List<int> questgivehowmany  = new List<int>();

   //길드퀘스트 수락날짜
   public DateTime questacceptdate;
   
   //시간 관련
   public DateTime GuildDailyTime; //
   
   //기본 데이터
   public int memberCount;
   public Dictionary<string, string> viceMasterList = new Dictionary<string, string>();
   public string masterNickname;
   public string inDate;
   public string guildName;
   public int goodsCount;
   public bool _immediateRegistration;
   public string _countryCode;
   public string masterInDate;

   public void ReSetData()
   {
      //레이드
      GuildBossIDs = "";
      GuildRaidIDs = "";
      GuildRaidCurHPs = 0;
      GuildRaidRankNames.Clear();
      GuildRaidRankDmgs.Clear();
      isRaidfinish = false;
      //퀘스트
      Timemanager.Instance.RefreshNowTIme();
      GuildDailyTime = Timemanager.Instance.NowTime.AddDays(1).Date;
      questid = "";
      questcount = 0;
      isquestfinish = false;
      questgivename.Clear();
      questgivehowmany.Clear();
   }

   public void SetGuildRaid(string mapid)
   {
      MapDB.Row mapdata = MapDB.Instance.Find_id(mapid);
      monsterDB.Row data = monsterDB.Instance.Find_id(mapdata.monsterid);
      GuildRaidIDs = mapid;
      GuildRaidCurHPs = decimal.Parse(data.hp,System.Globalization.NumberStyles.Float);
      GuildRaidRankNames.Clear();
      GuildRaidRankDmgs.Clear();
      raidacceptdate = Timemanager.Instance.NowTime.Date;
   }
   
   public override string ToString()
   {
      string viceMasterString = string.Empty;
      foreach (var li in viceMasterList)
      {
         viceMasterString += $"부길드마스터 : {li.Value}({li.Key})\n";
      }
      return $"memberCount : {memberCount}\n" +
             $"masterNickname : {masterNickname}\n" +
             $"inDate : {inDate}\n" +
             $"guildName : {guildName}\n" +
             $"goodsCount : {goodsCount}\n" +
             $"_immediateRegistration : {_immediateRegistration}\n" +
             $"_countryCode : {_countryCode}\n" +
             $"masterInDate : {masterInDate}\n" +
             $"memberCount : {memberCount}\n" +
             viceMasterString;
   }
};

public class GuildMemberInfo
{
   public string nickname = ""; // 유저의 닉네임
   public string indate = ""; // 유저의 inDate
   public DateTime lastLogin; // 마지막 접속 시간
   public string lastLoginString = ""; //마지막 접속 시간 표시
   public bool isActive = false; // 현재 게임에 접속중인지(실시간알림 서버에 접속했는지)
   public string position;
   //생성자
   public GuildMemberInfo(string _indate, string _nickname, string _lastLogin,string position)
   {
      this.indate = _indate;
      this.nickname = _nickname;
      this.lastLogin = DateTime.Parse(_lastLogin);
      this.position = position;
      SetLastLoginString(this.lastLogin);
   }

   // 접속시간을 표시해주는 string을 설정해준다.(현재시간 - 마지막 접속 시간)
   void SetLastLoginString(DateTime time)
   {
      TimeSpan ts = DateTime.Now - time;
      if (ts.Days > 0)
      {
         lastLoginString = string.Format(Inventory.GetTranslate("Guild/일전"), ts.Days);//  $"{ts.Days}일 전";
      }
      else if (ts.Hours > 0)
      {
         lastLoginString = string.Format(Inventory.GetTranslate("Guild/시간전"), ts.Hours);//  $"{ts.Days}일 전";
      }
      else if (ts.Minutes > 0)
      {
         lastLoginString = string.Format(Inventory.GetTranslate("Guild/분전"), ts.Minutes);//  $"{ts.Days}일 전";
      }
      else
      {
         lastLoginString = $"방금 전";
      }
   }

   public void ChangeActive(bool _isActive) // 접속중인지 설정
   {
      this.isActive = _isActive;
      if(this.isActive)
      {
         lastLoginString = Inventory.GetTranslate("Guild/접속중");
      }
      //UI적으로 온라인이면 초록불 또는 오프라인이면 회색불등의 값이 바뀌도록 설정해준다.

   }

   public override string ToString()
   {
      //string.Format("inDate : {0} / 닉네임 : {1} / 최종접속일자 : {2} / 접속중 : {3}", indate, nickname, lastLogin, isActive);

      if (isActive)
      {
         return string.Format("닉네임 : {0}  온라인", nickname);
      }
      else
      {
         return string.Format("닉네임 : {0} 오프라인 {1}", nickname, lastLoginString);

      }
   }
}

public class GoodsItem
{
    public int totalGoodsAmount;
    public Dictionary<string,GoodsUserItem> userList = new Dictionary<string,GoodsUserItem>();
}

public class GoodsUserItem
{
    public int usingTotalAmount;
    public int totalAmount;
    public string inDate;
    public string nickname;
    public string updatedAt;
    public override string ToString()
    {
        return $"\tnickname : {nickname}\n" +
        $"\tinDate : {inDate}\n" +
        $"\ttotalAmount : {totalAmount}\n" +
        $"\tusingTotalAmount : {usingTotalAmount}\n" +
        $"\tupdatedAt : {updatedAt}\n";
    }
}

public class ApplicantsItem
{
   public string inDate;
   public string nickname;
   public override string ToString()
   {
      return $"nickname : {nickname}\ninDate : {inDate}\n";
   }
}