using BackEnd;
using Doozy.Engine.UI;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Noticemanager : MonoBehaviour
{
    //싱글톤만들기.
    private static Noticemanager _instance = null;
    public static Noticemanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Noticemanager)) as Noticemanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }


    public UIView NoticePanel;
    public UIView NoticeInfoPanel;
    public GameObject Loadingobj;
    public noticeslot[] notices;
    List<Notice> noticeList = new List<Notice>();

    //내용
    public Text NoticeTitleText;
    public Text NoticeInfoText;


    public void ShowNoticeInfo(Notice noticedata)
    {
        NoticeInfoPanel.Show(false);
        NoticeTitleText.text = noticedata.title;
        NoticeInfoText.text = noticedata.contents;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)NoticeInfoText.GetComponentInParent<Transform>());
    }


    public void GetNotice()
    {
        if (PlayerBackendData.Instance.DailyOffBool[0])
        {
            NoticePanel.Hide(true);
            //이벤트
            if (!PlayerBackendData.Instance.DailyOffBool[2])
            {
                uimanager.Instance.TogglePanel[2].Show(false);
            }
            else if (!PlayerBackendData.Instance.DailyOffBool[1])
            {
                uimanager.Instance.TogglePanel[1].Show(false);
            }
            
        }
        else
        {
            NoticePanel.Show(false);
        }

        Loadingobj.SetActive(true);
        SendQueue.Enqueue(Backend.Notice.NoticeList, callback =>
        {
//            Debug.Log("공지가져오기");
//            Debug.Log(callback);
            if (!callback.IsSuccess()) return;
            Loadingobj.SetActive(false);
            JsonData jsonList = callback.FlattenRows();
            for (var i = 0; i < jsonList.Count; i++)
            {
                Notice notice = new Notice
                {
                    title = jsonList[i]["title"].ToString(),
                    contents = jsonList[i]["content"].ToString(),
                    postingDate = DateTime.Parse(jsonList[i]["postingDate"].ToString()),
                    inDate = jsonList[i]["inDate"].ToString(),
                    uuid = jsonList[i]["uuid"].ToString(),
                    isPublic = jsonList[i]["isPublic"].ToString() == "y" ? true : false,
                    author = jsonList[i]["author"].ToString()
                };

                if (jsonList[i].ContainsKey("imageKey"))
                {
                    notice.imageKey = "http://upload-console.thebackend.io" + jsonList[i]["imageKey"].ToString();
                }
                if (jsonList[i].ContainsKey("linkUrl"))
                {
                    notice.linkUrl = jsonList[i]["linkUrl"].ToString();
                }
                if (jsonList[i].ContainsKey("linkButtonName"))
                {
                    notice.linkButtonName = jsonList[i]["linkButtonName"].ToString();
                }

                notices[i].Refresh(notice);
                notices[i].gameObject.SetActive(true);
                noticeList.Add(notice);
            }
        });
    }

    private void Start()
    {
        foreach (var t in notices)
            t.gameObject.SetActive(false);

        GetNotice();
    }




}

public class Notice
{
    public string title;
    public string contents;
    public DateTime postingDate;
    public string imageKey;
    public string inDate;
    public string uuid;
    public string linkUrl;
    public bool isPublic;
    public string linkButtonName;
    public string author;

    public override string ToString()
    {
        return $"title : {title}\n" +
        $"contents : {contents}\n" +
        $"postingDate : {postingDate}\n" +
        $"imageKey : {imageKey}\n" +
        $"inDate : {inDate}\n" +
        $"uuid : {uuid}\n" +
        $"linkUrl : {linkUrl}\n" +
        $"isPublic : {isPublic}\n" +
        $"linkButtonName : {linkButtonName}\n" +
        $"author : {author}\n";
    }
}