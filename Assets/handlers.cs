using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class handlers : MonoBehaviour
{
    public GameObject Maintain;
    public GameObject OtherLogin;
    public GameObject BlackUser;
    
    Queue<Action> mainThreadQueue = new Queue<Action>();
    
    void Start() {
        
            Backend.ErrorHandler.InitializePoll(true);
            /*
            Backend.ErrorHandler.OnOtherDeviceLoginDetectedError = () => {
                Debug.Log("외부 로그인 감지!!!");
                mainThreadQueue.Enqueue( () => {
                    Time.timeScale = 0;
                    OtherLogin.SetActive(true);
                });
            };
            */
            Backend.ErrorHandler.OnMaintenanceError = () => {
                Debug.Log("점검 에러 발생!!!");
               
                mainThreadQueue.Enqueue( () => {
                    Maintain.SetActive(true);
                    Time.timeScale = 0;
                });
            };
            Backend.ErrorHandler.OnDeviceBlockError= () => {
                Debug.Log("디바이스 차단 발생");
               
                mainThreadQueue.Enqueue( () => {
                    Time.timeScale = 0;
                    BlackUser.SetActive(true);
                });
                ;
            };
            
            //실시간 알림
            SetNotificationHandler();
            Backend.Notification.Connect();
            
    }

    private void Update()
    {
        if (Backend.ErrorHandler.UseAsyncQueuePoll)
        {
            Backend.ErrorHandler.Poll();
        }
        
        // Queue에 행동이 저장되어 있을 경우
        if(mainThreadQueue != null && mainThreadQueue.Count > 0) {
            // Dequeue를 통해 행동을 추출 후 호출한다.(메인쓰레드이기 떄문)
            mainThreadQueue.Dequeue().Invoke();
        }
    }
    //실시간 알림
    void SetNotificationHandler() {
        Backend.Notification.OnAuthorize = (bool isSuccess, string reason) => {
            if (isSuccess) {
                Debug.Log("실시간 알림 서버 접속에 성공했습니다.");
            }

            Debug.LogWarning("실시간 알림 서버 접속에 실패했습니다.\n" + reason );
        };
        
        // 추가. 실시간 알림 우편 발송 핸들러
        Backend.Notification.OnNewPostCreated = (BackEnd.Socketio.PostRepeatType postRepeatType, string title, string content, string author) => {
            Debug.Log(
                $"우편이 발송되었습니다.\n" +
                $"| 우편 반복 타입 : {postRepeatType}\n" +
                $"| 우편 제목 : {title}\n" +
                $"| 우편 내용 : {content}\n" +
                $"| 우편 발송인 : {author}\n");
            mainThreadQueue.Enqueue( () => {
                alertmanager.Instance.PostNotiOn();
            });
        };
        
        Backend.Notification.OnReceivedGuildApplicant = () => {
            Debug.Log("새 길드 가입 신청이 도착했습니다!");
            mainThreadQueue.Enqueue( () => {
                alertmanager.Instance.NotiGuildApplyTomine();
            });
        };
        
        //길드 들어오는 용
        Backend.Notification.OnApprovedGuildJoin = () => {
            Debug.Log("가입성공");
            mainThreadQueue.Enqueue( () => {
                GuildManager.Instance.GuildJoinSucc();
                chatmanager.Instance.OnGuild();
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/길드가입됨"),alertmanager.alertenum.일반);
            });
        };
        Backend.Notification.OnRejectedGuildJoin = () => {
            Debug.Log("길드 가입 신청이 거절당했습니다...");
        };
    }

    public void Bt_QuitGame()
    {
        Savemanager.Instance.SaveInventory();
        Savemanager.Instance.SaveCollection();
        Savemanager.Instance.SaveAchieveDirect();
        Savemanager.Instance.Save();

        Application.Quit();
    }
}
