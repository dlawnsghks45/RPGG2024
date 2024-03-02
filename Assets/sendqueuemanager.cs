using BackEnd;
using BackEnd.Tcp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sendqueuemanager : MonoBehaviour
{
    void FixedUpdate()
    {
        if (sendqueuestart)
        {
            SendQueue.Poll();
            Backend.AsyncPoll();
        }
    }

    private bool sendqueuestart;

    private void Start()
    {
        if (SendQueue.IsInitialize == false)
        {
            // SendQueue 초기화
            SendQueue.StartSendQueue(true, ExceptionHandler);
            sendqueuestart = true;
        }
    }

    void ExceptionHandler(Exception e)
    {
        // 예외 처리
    }

    void OnApplicationQuit()
    {
        // 큐에 처리되지 않는 요청이 남아있는 경우 대기하고 싶은 경우
        // 큐에 몇 개의 함수가 남아있는지 체크
        while (SendQueue.UnprocessedFuncCount > 0)
        {
            // 처리
        }
        Backend.Chat.LeaveChannel(ChannelType.Public); //채팅방 나가기
        Backend.Notification.DisConnect(); //실시간알림
        SendQueue.StopSendQueue();
    }
}
