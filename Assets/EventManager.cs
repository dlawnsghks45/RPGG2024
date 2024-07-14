using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Doozy.Engine.UI;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    //싱글톤만들기.
    private static EventManager _instance = null;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(EventManager)) as EventManager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }
    
    // Start is called before the first frame update
    public noticeslot[] events;
    List<EventItem> eventList = new List<EventItem>();
    public GameObject Loading;
    // Update is called once per frame.
    public GameObject SeasonPassobj;
    private void Start()
    {
        EventListTest();
        RefreshEvent();
    }

    public void EventListTest()
    {
        eventList.Clear();

        Loading.SetActive(true);
        SendQueue.Enqueue(Backend.Event.EventList, 10, (bro) =>
        {
            // 이후 처리
            if (bro.IsSuccess())
            {
                Loading.SetActive(false);
                SeasonPassobj.SetActive(true);
                    JsonData jsonList = bro.FlattenRows();
                    for (int i = 0; i < jsonList.Count; i++)
                    {
                        EventItem eventItem = new EventItem();

                        eventItem.title = jsonList[i]["title"].ToString();
                        eventItem.content = jsonList[i]["content"].ToString();
                        eventItem.postingDate = DateTime.Parse(jsonList[i]["postingDate"].ToString());
                        eventItem.startDate = DateTime.Parse(jsonList[i]["startDate"].ToString());
                        eventItem.endDate = DateTime.Parse(jsonList[i]["endDate"].ToString());
                        eventItem.inDate = jsonList[i]["inDate"].ToString();
                        eventItem.uuid = jsonList[i]["uuid"].ToString();
                        eventItem.isPublic = jsonList[i]["isPublic"].ToString() == "y" ? true : false;
                        eventItem.author = jsonList[i]["author"].ToString();

                        if (jsonList[i].ContainsKey("contentImageKey"))
                        {
                            eventItem.contentImageKey = "http://upload-console.thebackend.io" +
                                                        jsonList[i]["contentImageKey"].ToString();
                        }

                        if (jsonList[i].ContainsKey("popUpImageKey"))
                        {
                            eventItem.popUpImageKey = "http://upload-console.thebackend.io" +
                                                      jsonList[i]["popUpImageKey"].ToString();
                        }

                        if (jsonList[i].ContainsKey("linkUrl"))
                        {
                            eventItem.linkUrl = jsonList[i]["linkUrl"].ToString();
                        }

                        if (jsonList[i].ContainsKey("linkButtonName"))
                        {
                            eventItem.linkButtonName = jsonList[i]["linkButtonName"].ToString();
                        }
                        events[i].Refresh(eventItem);
                        events[i].gameObject.SetActive(true);
                        eventList.Add(eventItem);
                    }
            }
        });
    }
    
    
    public UIView EventInfoPanel;
    //내용
    public Text EventTitleText;
    public Text EventInfoText;
    private string url;
    public void ShowEventInfo(EventItem data)
    {
        EventInfoPanel.Show(false);
        EventTitleText.text = data.title;
        EventInfoText.text = data.content.Split(';')[1];;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)EventInfoText.GetComponentInParent<Transform>());
        url = data.linkUrl;
    }

    public void Bt_Gourl()
    {
        Settingmanager.Instance.Bt_OpenURL(url);
    }
    
    
    
    
    
    //이벤트 페이지
    public UIView EventUI;
    public string[] EventItemID;
    public Image EventMonster;
    public itemiconslot[] eventitem;

    public void ShowEventUI()
    {
        EventUI.Show(false);
    }

    void RefreshEvent()
    {
        for (int i = 0; i < eventitem.Length; i++)
        {
            eventitem[i].Refresh(EventItemID[i], 1, false);
        }

        EventMonster.sprite = SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id("4999").sprite);
    }


}

public partial class EventItem
{
    public string uuid;
    public string content;
    public string contentImageKey;
    public string popUpImageKey;
    public DateTime postingDate;
    public DateTime startDate;
    public DateTime endDate;
    public string inDate;
    public string linkUrl;
    public string author;
    public bool isPublic;
    public string linkButtonName;
    public string title;

    public override string ToString()
    {
        return $"uuid : {uuid}\n" +
               $"content : {content}\n" +
               $"contentImageKey : {contentImageKey}\n" +
               $"popUpImageKey : {popUpImageKey}\n" +
               $"postingDate : {postingDate}\n" +
               $"startDate : {startDate}\n" +
               $"endDate : {endDate}\n" +
               $"inDate : {inDate}\n" +
               $"linkUrl : {linkUrl}\n" +
               $"author : {author}\n" +
               $"isPublic : {isPublic}\n" +
               $"linkButtonName : {linkButtonName}\n" +
               $"title : {title}\n";
    }
}
