using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class noticeslot : MonoBehaviour
{
    [SerializeField]
    private bool isevent;
    Notice data;
    EventItem events;
    public Image BackImage;
    public Text NoticeName;
    public Text EventDate;

    public void Refresh(Notice datas)
    {
        data = datas;
        NoticeName.text = data.title;
    }

    private string[] info_event;
    public void Refresh(EventItem datas)
    {
        events = datas;
        info_event = datas.content.Split(';');
        BackImage.sprite = SpriteManager.Instance.GetSprite(info_event[0]);
        NoticeName.text = datas.title;
        EventDate.text = string.Format(Inventory.GetTranslate("UI5/이벤트 기간"),datas.endDate.ToString("yyyy-M-d")) ;
    }

    public void Bt_ClickNotice()
    {
        Noticemanager.Instance.ShowNoticeInfo(data);
    }

    public void Bt_ClickEvent()
    {
        EventManager.Instance.ShowEventInfo(events);
    }

}
