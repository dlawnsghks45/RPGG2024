using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using BackEnd;
using LitJson;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timemanager : MonoBehaviour
{
    private void OnDestroy()
    {
        _instance = null;
    }

    private DateTime questResetTime; // 퀘스트 리셋 시간
    public TimeSpan timeToReset; // 리셋까지 남은 시간
    public JsonData timedata;
    public string timeindate;

    //출석 체
    public string tutoid = "0";

    
    public bool[] DailyRewardBool = new bool[DailyTotalCount]; 
    public const int DailyTotalCount = 28;
    public int dailycount = 0;
    
    //컨텐츠
    public int[] DailyContentCount = new int[(int)ContentEnumDaily.Length]; //랜덤 구성 
    public int[] DailyContentCount_Standard = new int[80]; //랜덤 구성 

    //총 시간
    public int LoginTimeSecEvery =0;
    //오늘 시간
    public int LoginTimeSecToday =0;
    
    private void Start()
    {
        for (int i = 0; i < TimeTableDB.Instance.NumRows(); i++)
        {
            if (TimeTableDB.Instance.GetAt(i).type.Equals("daily"))
            {
                DailyContentCount_Standard[(int)Enum.Parse(typeof(ContentEnumDaily),
                    TimeTableDB.Instance.GetAt(i).enumname)] = int.Parse(TimeTableDB.Instance.GetAt(i).maxcount);
            }
            else if (TimeTableDB.Instance.GetAt(i).type.Equals("weekly"))
            {
                WeeklyContentCount_Standard[(int)Enum.Parse(typeof(ContentEnumWeekly),
                    TimeTableDB.Instance.GetAt(i).enumname)] = int.Parse(TimeTableDB.Instance.GetAt(i).maxcount);
            }
            else if (TimeTableDB.Instance.GetAt(i).type.Equals("monthly"))
            {
                MonthlyContentCount_Standard[(int)Enum.Parse(typeof(ContentEnumMonthly),
                    TimeTableDB.Instance.GetAt(i).enumname)] = int.Parse(TimeTableDB.Instance.GetAt(i).maxcount);
            }
        }
      
    }


    public enum ContentEnumDaily
    {
        성물전쟁,
        골드러쉬,
        도적단처치,
        레이드,
        월드보스,
        길드레이드,
        교환소제련석,
        소탕,
        광고_엘리축복,
        광고_도전장,
        광고_등급재설정,
        광고_품질재설정,
        광고_특효재설정,
        광고_제련석,
        광고_사냥1시간,
        빠른전투,
        길드출석,
        이벤트던전재설정1,
        이벤트제련재설정1,
        길드임무납품,
        길드임무개인보상,
        마일리지레이드,
        길드레이드시도,
        길드레이드보상,
        길드보스전투,
        이벤트던전재설정2,
        이벤트제련재설정2,
        레이드증표교환,
        광고_낡은특효재설정,
        마법사묘교환레이드,
        일일저장가능횟수,
        블랙프라이데이승급,
        블랙프라이데이악몽,
        블랙프라이데이지옥,
        접속보상0분,
        접속보상30분,
        접속보상60분,
        접속보상90분,
        접속보상120분,
        접속보상150분,
        월드보스보상횟수,
        월드보스1공격횟수,
        월드보스2공격횟수,
        월드보스3공격횟수,
        월드보스4공격횟수,
        월드보스5공격횟수,
        월드보스6공격횟수,
        월드보스7공격횟수,
        월드보스8공격횟수,
        월드보스9공격횟수,
        월드보스10공격횟수,
        월드보스1보상횟수,
        월드보스2보상횟수,
        월드보스3보상횟수,
        월드보스4보상횟수,
        월드보스5보상횟수,
        월드보스6보상횟수,
        월드보스7보상횟수,
        월드보스8보상횟수,
        월드보스9보상횟수,
        월드보스10보상횟수,
        월드보스공격횟수,
        월드보스보상횟수리뉴얼,
        이벤트던전재설정3,
        이벤트제련재설정3,
        Length
    }

    public List<string> OncePremiumPackage =new List<string>(); //여기 들어있는건 다신 못산다.
    public List<string> OnceTradePackage =new List<string>(); //여기 들어있는건 다신 못산다.

    //컨텐츠
    public int[] WeeklyContentCount = new int[(int)ContentEnumWeekly.Length]; //랜덤 구성 
    public int[] WeeklyContentCount_Standard = new int[30]; //랜덤 구성 
    //컨텐츠
    public int[] MonthlyContentCount = new int[(int)ContentEnumMonthly.Length]; //랜덤 구성 
    public int[] MonthlyContentCount_Standard = new int[30]; //랜덤 구성 

    public enum ContentEnumWeekly
    {
        장비업그레이드패키지,
        교환소등급재설정,
        교환소품질재설정,
        교환소특효재설정,
        교환소던전입장권,
        프상장비강화패키지,
        프상던전입장권패키지,
        프상제련석패키지,
        이벤트등급재설정1,
        이벤트품질재설정1,
        이벤트특효재설정1,
        이벤트전설비법서1,
        이벤트신화비법서1,
        길드토큰_전설제작,
        길드토큰_신화제작,
        길드토큰_레이드입장권,
        프상주간패키지재설정1,
        프상주간패키지재설정2,
        프상주간패키지재설정3,
        프상주간패키지재설정1_2,
        프상주간패키지재설정2_2,
        프상주간패키지재설정3_2,
        이벤트등급재설정2,
        이벤트품질재설정2,
        이벤트특효재설정2,
        이벤트전설비법서2,
        이벤트신화비법서2,
        프상주간패키지레이드1,
        프상주간패키지레이드2,
        프상주간패키지제단석1,
        프상주간패키지제단석2,
        프상주간패키지제작1,
        프상스킬북패키지1,
        프상스킬북패키지2,
        프상스킬북패키지3,
        프상마법사입장권패키지1,
        프상마법사입장권패키지2,
        프상승급석패키지,
        프상주간펫패키지1,
        프상주간펫패키지2,
        파티레이드주간횟수,
        프상주간파티레이드충전권,
        교환소등급재설정2,
        교환소품질재설정2,
        교환소특효재설정2,
        교환소던전입장권2,
        교환소성장입장권2,
        이벤트등급재설정3,
        이벤트품질재설정3,
        이벤트특효재설정3,
        이벤트무기강화석3,
        이벤트방어구강화석3,
        이벤트장신구강화석3,
        Length
    }
    public enum ContentEnumMonthly
    {
        크리스탈5500,
        크리스탈11000,
        크리스탈33000,
        크리스탈55000,
        크리스탈110000,
        Length
    }
    
    //일일기간 현재꺼 가져오기
    public int GetNowCount_daily(ContentEnumDaily time)
    {
        return DailyContentCount[(int)time];
    }

    public int GetNowCount_daily2(int time)
    {
        return DailyContentCount[time];
    }
    public int GetNowCount_weekly2(ContentEnumWeekly time)
    {
        return WeeklyContentCount[(int)time];
    }
    //일일기간 최대치 현재꺼 가져오기
    public int GetMaxCount_daily(ContentEnumDaily time)
    {
        return DailyContentCount_Standard[(int)time];
    }
    public int GetMaxCount_weekly(ContentEnumWeekly time)
    {
        return WeeklyContentCount_Standard[(int)time];
    }
    //싱글톤만들기.
    private static Timemanager _instance = null;
    public static Timemanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Timemanager)) as Timemanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    //일일



    //주간
    //한달
    public DateTime NowTime;
    public delegate void TimeDelegate();
    public TimeDelegate timedele;
    BackendReturnObject servertime;

    private int num = 0;
    public bool isstop = false;
    public DateTime GetServerTime()
    {
        if (!internetcheck.isinternet())
        {
            uimanager.Instance.internetobj.SetActive(true);
            Time.timeScale = 0;
            isstop = true;
        }
        servertime = Backend.Utils.GetServerTime();
        string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
        DateTime parsedDate = DateTime.Parse(time);
//        Debug.Log(parsedDate);
        return parsedDate;
    }

    private bool dailyrewardinit = false;
    public void TimeInit()
    {
        //RefreshNowTIme();
        //접속시 현재 시간과 초기화 시간을 계산
        //초기화는 매일 0시 
        GetTime();
    }


    public DateTime LoginTime;
    //처음했을때 딱한번 비교하고 할때마다 로컬시간을 불러오고 현재시간과 접속했던 시간 데이터를 불러온다.
    public void StartTimeinit()
    {
        RefreshNowTIme();
        LoginTime = NowTime.Date; //햔재 시간이 처음접속한시간

        GetTime();
    }

    private string rewarddatetime;

    public void CheckdailyReward()
    {
      //  Debug.Log("브로콜백" + broCallback);
        if (broCallback.IsSuccess() == false)
        {
            // 서버에 요청을 실패한 경우 바로 리턴합니다.
            // 서버와 통신 실패 UI를 띄웁니다.
            Debug.Log("못가져왔다");
            return;
        }

        JsonData userInfo = broCallback.FlattenRows()[0];

        if (userInfo.ContainsKey("DailyRewardCount"))
        {
            if (userInfo.ContainsKey("DailyRewardCount"))
            {
                //보상 
                dailycount = int.Parse(userInfo["DailyRewardCount"].ToString());
              //  Debug.Log("보상 카운트" + dailycount);
                for (int i = 0; i < userInfo["DailyBool"].Count; i++)
                {
                    DailyRewardBool[i] = bool.Parse(userInfo["DailyBool"][i].ToString());
                }

                var lastLoginDate = DateTime.Parse(rewarddatetime);

                // 서버에 저장할 출석 정보를 미리 선언
                Param check = new Param();

                // 출석정보를 초기화 해야됨
                if (NowTime.Month != lastLoginDate.Month)
                {
                    // 빈 출석 정보 생성
                    DailyRewardBool = new bool[DailyTotalCount];
                    // 배열 내 모든 데이터를 false로 초기화
                    DailyRewardBool.Initialize();
                    dailycount = 0;
                    // 출석 정보를 갱신
                    check.Add("DailyBool", DailyRewardBool);
                    check.Add("DailyRewardCount", 0);
                }
                // 오늘 날짜의 'Month'가 같고
                // 오늘 날짜의 'Day'와 마지막 로그인 날짜의 Day'가 같으면
                // 출석을 이미 했음
                else if (NowTime.Day == lastLoginDate.Day)
                {
                    // 이미 출석했으니 그대로 리턴
                    return;
                }
                else
                {
                    if (dailycount != DailyTotalCount)
                    {
                        dailycount++; //데일리카운트 1 증가
                        check.Add("DailyRewardCount", dailycount);
                    }


                }

                SendQueue.Enqueue(Backend.GameData.UpdateV2, "TimeData",
                    PlayerBackendData.Instance.PlayerTableIndate[(int)
                        PlayerBackendData.IndatesEnum.TimeData].ToString(),
                    PlayerBackendData.Instance.playerindate, check, broCallbackUpdate =>
                    {
                        if (broCallbackUpdate.IsSuccess())
                        {
                            if (SceneManager.GetActiveScene().name == "Lobby")
                            {
                                dailyrewardmanager.Instance.Initdailyreward();
                            }

                        }
                    });
            }
        }
    }



    public void CheckTime()
    {
        RefreshNowTIme();
        if (LoginTime < NowTime.Date)
        {
            //초기화 시작
            GetTime();
        }
        else
        {
            
        }
    }
    
    private BackendReturnObject broCallback;
    public bool isachievereset = false;
    public bool isachieveresetdaily = false;
    public bool isachieveresetweekly = false;
       //12시가 지나면 체크한다.
       public void GetTime()
       {
           bool isfalse = false;
//           Debug.Log("겟타임");
           RefreshNowTIme();

           DateTime date;
           //데이터를 받아온다.
           Where where1 = new Where();
           where1.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

           broCallback = Backend.GameData.GetMyData("TimeData",where1, 1);
//           Debug.Log("유저데이터 불러옴");
    
//           Debug.Log(broCallback);

           if (broCallback.IsSuccess())
           {
               if (broCallback.GetReturnValuetoJSON()["rows"].Count <= 0)
               {

                   // 요청이 성공해도 where 조건에 부합하는 데이터가 없을 수 있기 때문에
                   // 데이터가 존재하는지 확인
                   // 위와 같은 new Where() 조건의 경우 테이블에 row가 하나도 없으면 Count가 0 이하 일 수 있다.
                   RefreshNowTIme();
                   LoginTime = NowTime;
                   param.Clear();
                   param.Add("DailyRewardCount", 0);
                   param.Add("DailyBool", DailyRewardBool);
                   param.Add("DailyCount", DailyContentCount_Standard);
                   param.Add("DailyDateTime", NowTime.ToString());
                   param.Add("WeeklyCount", WeeklyContentCount_Standard);
                   param.Add("MonthlyCount", MonthlyContentCount_Standard);
                   Debug.Log("시간을 초기화한다");

                   for (int i = 0; i < DailyContentCount.Length; i++)
                   {
                       DailyContentCount[i] = DailyContentCount_Standard[i];
                   }

                   for (int i = 0; i < WeeklyContentCount.Length; i++)
                   {
                       WeeklyContentCount[i] = WeeklyContentCount_Standard[i];
                   }

                   //다음주 월요일 계산
                   DateTime today = NowTime;
                   DayOfWeek todayDOW = today.DayOfWeek;
                   int daysUntilMonday = ((int)DayOfWeek.Sunday - (int)todayDOW + 7) % 7;
                   DateTime nextMonday = today.AddDays(daysUntilMonday);
                   param.Add("WeeklyDateTime", nextMonday.ToString());

                   int dayinmonth = DateTime.DaysInMonth(today.Year, today.Month);
                   Debug.Log("몇일인가" + dayinmonth);
                   DateTime MonthFirstDay = today.AddDays(1+dayinmonth  - today.Day);
                   param.Add("MonthlyDateTime", MonthFirstDay.Date.ToString());
                   Backend.GameData.Insert("TimeData", param);
                   GetTime();
                   return;
               }

               timedata = broCallback.GetReturnValuetoJSON()["rows"][0];
               PlayerBackendData.Instance.PlayerTableIndate[(int)PlayerBackendData.IndatesEnum.TimeData] =
                   timedata["inDate"]["S"].ToString();
               if (timedata.Keys.Contains("DailyDateTime"))
               {
                   PlayerBackendData.Instance.PlayerTableIndate[(int)PlayerBackendData.IndatesEnum.TimeData] =
                       timedata["inDate"]["S"].ToString();

                   date = System.Convert.ToDateTime(timedata["DailyDateTime"]["S"].ToString());
                   rewarddatetime = timedata["DailyDateTime"]["S"].ToString();
                   //일일
                   if (date.Date < NowTime.Date)
                   {
                       for (int i = 0; i < PlayerBackendData.Instance.DailyOffBool.Length; i++)
                       {
                           PlayerBackendData.Instance.DailyOffBool[i] = false;
                       }
                       LoginTime = NowTime;
//                       Debug.Log("초기화진행");
                       for (int i = 0; i < DailyContentCount.Length; i++)
                       {
                           DailyContentCount[i] = DailyContentCount_Standard[i];
                       }

                     
                       //일일 상점도 초기화 진행
                       //IAPLKManager.Instance.SetRandomPackage();
//                       Debug.Log("업적 초기화대기");
                       isachievereset = true;
                       isachieveresetdaily = true;
                       LoginTimeSecToday = 0;
                       //리셋
                       if (SceneManager.GetActiveScene().name == "Lobby")
                       {
                           QuestManager.Instance.ResetDaily();
                           alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI8/퀘스트가초기화"),alertmanager.alertenum.일반);
                           //출석체크
                           CheckdailyReward();
                           dailyrewardmanager.Instance.RewardPanel.Show(false);
                           //dailyrewardmanager.Instance.Initdailyreward();
                           uimanager.Instance.RefreshShopAndTrade();
                           Settingmanager.Instance.RefreshReset();
                       }

                       Param param = new Param();
                       param.Add("DailyDateTime", NowTime.Date.ToString());
                       param.Add("DailyCount", DailyContentCount);
                       SendQueue.Enqueue(Backend.GameData.UpdateV2, "TimeData",
                           PlayerBackendData.Instance.PlayerTableIndate[(int)
                               PlayerBackendData.IndatesEnum.TimeData].ToString(),
                           PlayerBackendData.Instance.playerindate, param, broCallbackUpdate =>
                           {
                               Debug.Log(broCallbackUpdate);

                               if (broCallbackUpdate.IsSuccess())
                               {
                                   Debug.Log("저자자아");
                                   Debug.Log(broCallbackUpdate);
                               }
                           });
                   }
                   else
                   {
                       //오늘자꺼
                       JsonData E1 = timedata["DailyCount"]["L"];
                       for (int j = 0; j < DailyContentCount.Length; j++)
                       {
                           try
                           {
                               if (int.Parse(E1[j]["N"].ToString()) > DailyContentCount_Standard[j])
                               {
                                   DailyContentCount[j] = DailyContentCount_Standard[j];
                               }
                               else
                               {
                                   DailyContentCount[j] = int.Parse(E1[j]["N"].ToString());
                               }
                           }
                           catch
                           {
                               DailyContentCount[j] = DailyContentCount_Standard[j];
                               isfalse = true;
                           }
                       }
                       if (isfalse)
                       {
                           Debug.Log("데이터를 넣는다");
                           Param param = new Param();
                           param.Add("DailyDateTime", GetServerTime().Date.ToString());
                           param.Add("DailyCount", DailyContentCount);
                           param.Add("DailyRewardCount", 0);
                           param.Add("DailyBool", DailyRewardBool);
                           SendQueue.Enqueue(Backend.GameData.UpdateV2, "TimeData",
                               PlayerBackendData.Instance.PlayerTableIndate[(int)
                                   PlayerBackendData.IndatesEnum.TimeData].ToString(),
                               PlayerBackendData.Instance.playerindate, param, broCallbackUpdate =>
                               {
                                   Debug.Log(broCallbackUpdate);

                                   if (broCallbackUpdate.IsSuccess())
                                   {
                                       Debug.Log("저자자아");
                                       Debug.Log(broCallbackUpdate);
                                   }
                               });
                       }
                   }

                   CheckdailyReward();
               }
               if (timedata.Keys.Contains("WeeklyDateTime"))
               {

                   PlayerBackendData.Instance.PlayerTableIndate[(int)PlayerBackendData.IndatesEnum.TimeData] =
                       timedata["inDate"]["S"].ToString();

                   date = System.Convert.ToDateTime(timedata["WeeklyDateTime"]["S"].ToString());
//                   Debug.Log("시간" + date);
               //    Debug.Log("1시간" + NowTime.Date);
                   if (date.Date < NowTime.Date)
                   {
                       Debug.Log("주간초기화진행");
                       isachieveresetweekly = true;
                       for (int i = 0; i < WeeklyContentCount.Length; i++)
                       {
                           WeeklyContentCount[i] = WeeklyContentCount_Standard[i];
                       }
                       if (SceneManager.GetActiveScene().name == "Lobby")
                       {
                           QuestManager.Instance.ResetWeekly();
                           alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI8/퀘스트가초기화"),alertmanager.alertenum.일반);
                           Settingmanager.Instance.RefreshReset();

                       }
                       Param param = new Param();

                       DateTime today = NowTime;
                       DayOfWeek todayDOW = today.DayOfWeek;
                       int daysUntilMonday = ((int)DayOfWeek.Sunday - (int)todayDOW + 7) % 7;
                       DateTime nextMonday = today.AddDays(daysUntilMonday).Date;

                       param.Add("WeeklyDateTime", nextMonday.Date.ToString());
                       param.Add("WeeklyCount", WeeklyContentCount_Standard);
                       SendQueue.Enqueue(Backend.GameData.UpdateV2, "TimeData",
                           PlayerBackendData.Instance.PlayerTableIndate[(int)
                               PlayerBackendData.IndatesEnum.TimeData].ToString(),
                           PlayerBackendData.Instance.playerindate, param, broCallbackUpdate =>
                           {
                               if (broCallbackUpdate.IsSuccess())
                               {
                                   Debug.Log(broCallbackUpdate);
                               }
                           });
                   }
                   else
                   {
                       //오늘자꺼
                       JsonData E1 = timedata["WeeklyCount"]["L"];
                       for (int j = 0; j < WeeklyContentCount.Length; j++)
                       {
                           try
                           {
                               //많으면 여기로
                               if (int.Parse(E1[j]["N"].ToString()) > WeeklyContentCount_Standard[j])
                               {
                                   WeeklyContentCount[j] = WeeklyContentCount_Standard[j];
                               }
                               else
                               {
                                   WeeklyContentCount[j] = int.Parse(E1[j]["N"].ToString());
                               }
                           }
                           catch
                           {
                               WeeklyContentCount[j] = WeeklyContentCount_Standard[j];
                               isfalse = true;
                           }
                       }

                       if (isfalse)
                       {
                           Param param = new Param();

                           DateTime today = GetServerTime();
                           DayOfWeek todayDOW = today.DayOfWeek;
                           int daysUntilMonday = ((int)DayOfWeek.Sunday - (int)todayDOW + 7) % 7;
                           DateTime nextMonday = today.AddDays(daysUntilMonday);


                           param.Add("WeeklyDateTime", nextMonday.Date.ToString());
                           param.Add("WeeklyContentCount", WeeklyContentCount);
                           SendQueue.Enqueue(Backend.GameData.UpdateV2, "TimeData",
                               PlayerBackendData.Instance.PlayerTableIndate[(int)
                                   PlayerBackendData.IndatesEnum.TimeData].ToString(),
                               PlayerBackendData.Instance.playerindate, param, broCallbackUpdate =>
                               {
                                   if (broCallbackUpdate.IsSuccess())
                                   {
                                       Debug.Log("저자자아");
                                       Debug.Log(broCallbackUpdate);
                                   }
                               });
                       }
                   }
               }
               if (timedata.Keys.Contains("MonthlyDateTime"))
               {
                   PlayerBackendData.Instance.PlayerTableIndate[(int)PlayerBackendData.IndatesEnum.TimeData] =
                       timedata["inDate"]["S"].ToString();
              //     Debug.Log(timedata["MonthlyDateTime"]["S"].ToString());
//                   Debug.Log( NowTime.Date);
                   date = System.Convert.ToDateTime(timedata["MonthlyDateTime"]["S"].ToString());
                   
                   if (date.Date < NowTime.Date)
                   {
                      Debug.Log("월간초기화진행2");
                       for (int i = 0; i < MonthlyContentCount.Length; i++)
                       {
                           MonthlyContentCount[i] = MonthlyContentCount_Standard[i];
                       }

                       isachievereset = true;
                       Param param = new Param();

                       DateTime today = NowTime;
                       int dayinmonth = DateTime.DaysInMonth(today.Year, today.Month);
                       Debug.Log("몇일인가" + dayinmonth);
                       DateTime MonthFirstDay = today.AddDays(dayinmonth - today.Day);
                       param.Add("MonthlyDateTime", MonthFirstDay.Date.ToString());
                       param.Add("MonthlyCount", MonthlyContentCount_Standard);
                       SendQueue.Enqueue(Backend.GameData.UpdateV2, "TimeData",
                           PlayerBackendData.Instance.PlayerTableIndate[(int)
                               PlayerBackendData.IndatesEnum.TimeData].ToString(),
                           PlayerBackendData.Instance.playerindate, param, broCallbackUpdate =>
                           {
                               if (broCallbackUpdate.IsSuccess())
                               {
                                    GetTime();
                               }
                           });
                   }
                   else
                   {
                       //오늘자꺼\
                       if (timedata.ContainsKey("MontlyCount") && !timedata.ContainsKey("MonthlyCount"))
                       {
                           Param param = new Param();
                           DateTime today = NowTime;
                           int dayinmonth = DateTime.DaysInMonth(today.Year, today.Month);
                           Debug.Log("몇일인가" + dayinmonth);
                           DateTime MonthFirstDay = today.AddDays(dayinmonth - today.Day);
                           param.Add("MonthlyDateTime", MonthFirstDay.Date.ToString());
                           param.Add("MonthlyCount", MonthlyContentCount_Standard);


                           SendQueue.Enqueue(Backend.GameData.UpdateV2, "TimeData",
                               PlayerBackendData.Instance.PlayerTableIndate[(int)
                                   PlayerBackendData.IndatesEnum.TimeData].ToString(),
                               PlayerBackendData.Instance.playerindate, param, broCallbackUpdate =>
                               {
                                   if (broCallbackUpdate.IsSuccess())
                                   {

                                   }
                               });
                       }
                       else
                       {
                             JsonData E1 = timedata["MonthlyCount"]["L"];
                       for (int j = 0; j < MonthlyContentCount.Length; j++)
                       {
                           try
                           {
                               //많으면 여기로
                               if (int.Parse(E1[j]["N"].ToString()) > MonthlyContentCount_Standard[j])
                               {
                                   MonthlyContentCount[j] = MonthlyContentCount_Standard[j];
                               }
                               else
                               {
                                   MonthlyContentCount[j] = int.Parse(E1[j]["N"].ToString());
                               }
                           }
                           catch
                           {
                               MonthlyContentCount[j] = MonthlyContentCount_Standard[j];
                               isfalse = true;
                           }
                       }

                       if (isfalse)
                       {
                           Param param = new Param();


                           DateTime today = NowTime;
                           int dayinmonth = DateTime.DaysInMonth(today.Year, today.Month);
                           Debug.Log("몇일인가" + dayinmonth);
                           DateTime MonthFirstDay = today.AddDays(dayinmonth - today.Day);
                           param.Add("MonthlyDateTime", MonthFirstDay.Date.ToString());
                           param.Add("MonthlyCount", MonthlyContentCount_Standard);


                           SendQueue.Enqueue(Backend.GameData.UpdateV2, "TimeData",
                               PlayerBackendData.Instance.PlayerTableIndate[(int)
                                   PlayerBackendData.IndatesEnum.TimeData].ToString(),
                               PlayerBackendData.Instance.playerindate, param, broCallbackUpdate =>
                               {
                                   if (broCallbackUpdate.IsSuccess())
                                   {

                                   }
                               });
                       }
                       }
                     
                   }
               }
               else
               {
                   param.Clear();
                   DateTime today = NowTime;
                   int dayinmonth = DateTime.DaysInMonth(today.Year, today.Month);
                   Debug.Log("몇일인가" + dayinmonth);
                   DateTime MonthFirstDay = today.AddDays(1 + dayinmonth - today.Day);
                   param.Add("MonthlyDateTime", MonthFirstDay.Date.ToString());
                   param.Add("MonthlyCount", MonthlyContentCount_Standard);


                   SendQueue.Enqueue(Backend.GameData.UpdateV2, "TimeData",
                       PlayerBackendData.Instance.PlayerTableIndate[(int)
                           PlayerBackendData.IndatesEnum.TimeData].ToString(),
                       PlayerBackendData.Instance.playerindate, param, broCallbackUpdate =>
                       {
                           if (broCallbackUpdate.IsSuccess())
                           {

                           }
                       });
               }
           }
           else
           {
               Debug.Log("서버불러오기실패");
               GetTime();
               return;
           }
       }

       Param param = new Param();
    public bool ConSumeCount_DailyAscny(ContentEnumDaily index)
    {
        param.Clear();
        GetTime();

        
        Debug.Log("시간을 확인");
        if (index == ContentEnumDaily.길드출석)
        {
            if (DailyContentCount[(int)index] > 1)
                DailyContentCount[(int)index] = 1;
        }
        Debug.Log("시간을 확인");
        if (DailyContentCount[(int)index].Equals(0))
        {
            alertmanager.Instance.ShowAlert("횟수가 부족합니다.",alertmanager.alertenum.주의);
            Debug.Log("시간을 확인");

            //timedele();
            return false;
        }
        Debug.Log("시간을 확인");
        DailyContentCount[(int)index]--;
        param.Add("DailyCount", DailyContentCount);
        Debug.Log("시간을 확인");
        bool istrue = false;

        
        BackendReturnObject broCallback = Backend.GameData.UpdateV2("TimeData",PlayerBackendData.Instance.PlayerTableIndate[(int)
            PlayerBackendData.IndatesEnum.TimeData].ToString(),PlayerBackendData.Instance.playerindate, param);
        if (!broCallback.IsSuccess())
        {
            alertmanager.Instance.ShowAlert("서버 연결이 불안정합니다.",alertmanager.alertenum.주의);
            DailyContentCount[(int)index]++;
            //timedele();
            istrue = false;
        }
        //저장완료
        else
        {
            //timedele();
            istrue = true;
        }
        return istrue;
    }
    
    public bool AddDailyCount(ContentEnumDaily enums ,int count)
    {
        param.Clear();
        if (DailyContentCount[(int)enums].Equals(DailyContentCount_Standard[(int)enums]))
        {
            
            return false;
        }
        DailyContentCount[(int)enums] +=count;
        param.Add("DailyCount", DailyContentCount);
        bool istrue = false;

        
        BackendReturnObject broCallback = Backend.GameData.UpdateV2("TimeData",PlayerBackendData.Instance.PlayerTableIndate[(int)
            PlayerBackendData.IndatesEnum.TimeData].ToString(),PlayerBackendData.Instance.playerindate, param);
        if (!broCallback.IsSuccess())
        {
            alertmanager.Instance.ShowAlert("서버 연결이 불안정합니다.",alertmanager.alertenum.주의);
            timedele?.Invoke();
            istrue = false;
        }
        //저장완료
        else
        {
            istrue = true;
        }
        return istrue;
    }
    public bool AddDailyCount(int arraynum ,int count)
    {
        param.Clear();
        DailyContentCount[arraynum] +=count;
        param.Add("DailyCount", DailyContentCount);
        bool istrue = false;

        
        BackendReturnObject broCallback = Backend.GameData.UpdateV2("TimeData",PlayerBackendData.Instance.PlayerTableIndate[(int)
            PlayerBackendData.IndatesEnum.TimeData].ToString(),PlayerBackendData.Instance.playerindate, param);
        if (!broCallback.IsSuccess())
        {
            alertmanager.Instance.ShowAlert("서버 연결이 불안정합니다.",alertmanager.alertenum.주의);
            timedele?.Invoke();
        }
        //저장완료
        else
        {
            istrue = true;
        }
        return istrue;
    }
    public bool AddWeeklyCount(int arraynum ,int count)
    {
        param.Clear();
        WeeklyContentCount[arraynum] +=count;
        param.Add("WeeklyCount", WeeklyContentCount);
        bool istrue = false;

        
        BackendReturnObject broCallback = Backend.GameData.UpdateV2("TimeData",PlayerBackendData.Instance.PlayerTableIndate[(int)
            PlayerBackendData.IndatesEnum.TimeData].ToString(),PlayerBackendData.Instance.playerindate, param);
        if (!broCallback.IsSuccess())
        {
            alertmanager.Instance.ShowAlert("서버 연결이 불안정합니다.",alertmanager.alertenum.주의);
            timedele?.Invoke();
        }
        //저장완료
        else
        {
            istrue = true;
        }
        return istrue;
    }
    public bool ConSumeCount_DailyAscny(int index,int count)
    {

        param.Clear();
        GetTime();

        if (DailyContentCount[(int)index].Equals(0))
        {
            alertmanager.Instance.ShowAlert("횟수가 부족합니다.",alertmanager.alertenum.주의);
            return false;
        }

        if (DailyContentCount[index] - count < 0)
        {
            alertmanager.Instance.ShowAlert("횟수가 부족합니다.",alertmanager.alertenum.주의);
            return false;
        }
        DailyContentCount[(int)index]-=count;
        param.Add("DailyCount", DailyContentCount);
        bool istrue = false;

        
        BackendReturnObject broCallback = Backend.GameData.UpdateV2("TimeData",PlayerBackendData.Instance.PlayerTableIndate[(int)
            PlayerBackendData.IndatesEnum.TimeData].ToString(),PlayerBackendData.Instance.playerindate, param);
        if (!broCallback.IsSuccess())
        {
            alertmanager.Instance.ShowAlert("서버 연결이 불안정합니다.",alertmanager.alertenum.주의);
            timedele?.Invoke();

            istrue = false;
        }
        //저장완료
        else
        {
            istrue = true;
        }
        return istrue;
    }
    
    public bool ConSumeCount_WeeklyAscny(int index,int count)
    {
        param.Clear();
        GetTime();

        if (WeeklyContentCount[(int)index].Equals(0))
        {
            alertmanager.Instance.ShowAlert("횟수가 부족합니다.",alertmanager.alertenum.주의);
            timedele?.Invoke();

            return false;
        }
        WeeklyContentCount[(int)index]-=count;
        param.Add("WeeklyCount", WeeklyContentCount);
        bool istrue = false;

        
        BackendReturnObject broCallback = Backend.GameData.UpdateV2("TimeData",PlayerBackendData.Instance.PlayerTableIndate[(int)
            PlayerBackendData.IndatesEnum.TimeData].ToString(),PlayerBackendData.Instance.playerindate, param);
        if (!broCallback.IsSuccess())
        {
            alertmanager.Instance.ShowAlert("서버 연결이 불안정합니다.",alertmanager.alertenum.주의);
           // timedele();
            istrue = false;
        }
        //저장완료
        else
        {
         //   timedele();
            istrue = true;
        }
        return istrue;
    }
    
    public bool AddMonthlyCount(int arraynum ,int count)
    {
        param.Clear();
        DailyContentCount[arraynum] +=count;
        param.Add("MonthlyCount", MonthlyContentCount);
        bool istrue = false;

        
        BackendReturnObject broCallback = Backend.GameData.UpdateV2("TimeData",PlayerBackendData.Instance.PlayerTableIndate[(int)
            PlayerBackendData.IndatesEnum.TimeData].ToString(),PlayerBackendData.Instance.playerindate, param);
        if (!broCallback.IsSuccess())
        {
            alertmanager.Instance.ShowAlert("서버 연결이 불안정합니다.",alertmanager.alertenum.주의);
            timedele?.Invoke();
        }
        //저장완료
        else
        {
            istrue = true;
        }
        return istrue;
    }
    public bool ConSumeCount_MonthlyAscny(int index,int count)
    {
        param.Clear();
        GetTime();

        if (MonthlyContentCount[(int)index].Equals(0))
        {
            alertmanager.Instance.ShowAlert("횟수가 부족합니다.",alertmanager.alertenum.주의);
            timedele?.Invoke();

            return false;
        }
        MonthlyContentCount[(int)index]-=count;
        param.Add("MonthlyCount", MonthlyContentCount);
        bool istrue = false;

        
        BackendReturnObject broCallback = Backend.GameData.UpdateV2("TimeData",PlayerBackendData.Instance.PlayerTableIndate[(int)
            PlayerBackendData.IndatesEnum.TimeData].ToString(),PlayerBackendData.Instance.playerindate, param);
        if (!broCallback.IsSuccess())
        {
            alertmanager.Instance.ShowAlert("서버 연결이 불안정합니다.",alertmanager.alertenum.주의);
            // timedele();
            istrue = false;
        }
        //저장완료
        else
        {
            //   timedele();
            istrue = true;
        }
        return istrue;
    }
    
    public void RefreshNowTIme()
    {
        NowTime = GetServerTime();
    }
}
