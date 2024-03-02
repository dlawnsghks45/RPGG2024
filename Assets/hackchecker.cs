using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using BackEnd;
public class hackchecker : MonoBehaviour
{
    public GameObject speedhackobj;
    public int TargetFrameRate = 0;
    private DateTime LastUtcTime;
    public float LimitTime = 15f;
    [SerializeField]
    private float LastTimesamp;

    // �׽�Ʈ�� �ڵ�
#if UNITY_EDITOR
    public float TestDateTime;
    public float TestUnityTime;
#endif

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        TargetFrameRate = 0;
        Application.targetFrameRate = 30;
    }

//#if !UNITY_EDITOR
    private void Update()
    {
        var utcNow = DateTime.UtcNow;
        float realtimeSinceStartup = Time.realtimeSinceStartup;
        if (Application.targetFrameRate != TargetFrameRate) {
            TargetFrameRate = Application.targetFrameRate;

            LimitTime = TargetFrameRate > -1 ? TargetFrameRate * 0.5f : 15f;
            SetTempstaemp(utcNow, realtimeSinceStartup);
            return;
        }

        var elapsedSpan = utcNow - LastUtcTime;
        float elapsedTime = realtimeSinceStartup - LastTimesamp;
        if (elapsedTime > TargetFrameRate) {
            SetTempstaemp(utcNow, realtimeSinceStartup);
            float intervalTime = Mathf.Abs(elapsedTime - (float)elapsedSpan.TotalSeconds);
            if (intervalTime > LimitTime) {
                Debug.LogError($"{(Application.targetFrameRate / elapsedSpan.TotalSeconds) }�� Client Speed Hack Detected({elapsedSpan.TotalSeconds:F2} : {realtimeSinceStartup:F2})");
                NoticePopupMsg((float)(Application.targetFrameRate / elapsedSpan.TotalSeconds),(float)realtimeSinceStartup);
            }
        }
#if UNITY_EDITOR
        TestDateTime = (float)elapsedSpan.TotalSeconds;
        TestUnityTime = elapsedTime;
#endif
    }

    private void NoticePopupMsg(float a,float b)
    {
        Init();
        Debug.LogError("�ð� ������ ���� ��");
        // key �÷��� ���� keyCode�� ������ �˻�
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
     
        
        Param param = new Param
        {
            { "SpeedHackOn", true},
        };
        
        
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
        {
            // ���� ó��
            if (!callback.IsSuccess()) return;
            //Debug.Log("������ ���Լ���");
            LogManager.LogSpeedHack(a,b);
            speedhackobj.SetActive(true);
        });
    }

    private void Quit()
    {
#if UNITY_EDITOR
        // �ʱ�ȭ
#else
        Application.Quit();
#endif
    }

    private void SetTempstaemp(DateTime utcNow, float realtimeSinceStartup)
    {
        LastUtcTime = utcNow;
        LastTimesamp = realtimeSinceStartup;
    }
//#endif
}
