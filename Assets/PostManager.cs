using BackEnd;
using Doozy.Engine.UI;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Debug;

public class PostManager : MonoBehaviour
{
    //싱글톤만들기.
    private static PostManager _instance = null;
    public static PostManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(PostManager)) as PostManager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }
    public List<Postslot> postslotlist = new List<Postslot>();
    public Transform postpaneltransform;
    public Postslot slotobj;

    private void pool(int count = 30)
    {
        for(int i = 0; i < count; i++)
        {
            Postslot post = Instantiate(slotobj, postpaneltransform);
            post.gameObject.SetActive(false);
            postslotlist.Add(post);
            totalcount++;
        }
    }

    private void Start()
    {
        pool();
    }

    public UIView PostPanel;
    public GameObject Loadingobj;
    int totalcount = 0;

    private Dictionary<string, UPostItem> postItemList = new Dictionary<string, UPostItem>();
    public void GetPostList()
    {
        postItemList.Clear();
        PostPanel.Show(false);

        if (postslotlist != null)
        {
            foreach (var t in postslotlist)
            {
                t.gameObject.SetActive(false);
            }
        }

        Loadingobj.SetActive(true);
        PostType postType = PostType.Rank;

        BackendReturnObject admincall = null;
        BackendReturnObject rankcall = null;
        
        SendQueue.Enqueue(Backend.UPost.GetPostList, PostType.Rank, 100, callback =>  {
            JsonData postListJson = callback.GetReturnValuetoJSON()["postList"];
            rankcall = callback;
            if (!callback.IsSuccess())
            {
                LogError(callback.ToString());
            }

            Log("랭크" + callback);
if (!callback.IsSuccess()) return;
            
            if (totalcount < postListJson.Count)
                pool(totalcount - postslotlist.Count);
            postType = PostType.Rank;
            for (var i = 0; i < postListJson.Count; i++)
            {
                UPostItem postItem = new UPostItem();


                postItem.inDate = postListJson[i]["inDate"].ToString();
                postItem.title = postListJson[i]["title"].ToString();
                postItem.postType = postType;

                if (postType == PostType.Rank)
                {
                    postItem.content = postListJson[i]["content"].ToString();
                    postItem.expirationDate = DateTime.Parse(postListJson[i]["expirationDate"].ToString());
                    postItem.reservationDate = DateTime.Parse(postListJson[i]["reservationDate"].ToString());
                    postItem.nickname = postListJson[i]["nickname"]?.ToString();
                    postItem.sentDate = DateTime.Parse(postListJson[i]["sentDate"].ToString());

                    if (postListJson[i].ContainsKey("author"))
                    {
                        postItem.author = postListJson[i]["author"].ToString();
                    }

                    if (postListJson[i].ContainsKey("rankType"))
                    {
                        postItem.author = postListJson[i]["rankType"].ToString();
                    }
                }

                if (postListJson[i]["items"].Count > 0)
                {

                    for (var itemNum = 0; itemNum < postListJson[i]["items"].Count; itemNum++)
                    {

                        UPostChartItem item = new UPostChartItem();
                        item.itemCount = int.Parse(postListJson[i]["items"][itemNum]["itemCount"].ToString());
                        item.chartFileName = postListJson[i]["items"][itemNum]["item"]["chartFileName"].ToString();
                        item.itemID = postListJson[i]["items"][itemNum]["item"]["idnumber"].ToString();
                        item.itemName = postListJson[i]["items"][itemNum]["item"]["name"].ToString();

                        postItem.items.Add(item);
                    }
                }

                postItemList.Add(postItem.inDate, postItem);
            }
        });
        
        SendQueue.Enqueue(Backend.UPost.GetPostList, PostType.Coupon, 30, callback =>  {
            JsonData postListJson = callback.GetReturnValuetoJSON()["postList"];

               if (!callback.IsSuccess())
            {
                LogError(callback.ToString());
            }

            if (!callback.IsSuccess()) return;
            Log("쿠폰" + callback);

            Log("Coupon" + callback);
            if (totalcount < postListJson.Count)
                pool(totalcount - postslotlist.Count);
            postType = PostType.Coupon;
            for (var i = 0; i < postListJson.Count; i++)
            {
                UPostItem postItem = new UPostItem();
                postItem.inDate = postListJson[i]["inDate"].ToString();
                postItem.title = postListJson[i]["title"].ToString();
                postItem.postType = postType;
                //Debug.Log("쿠폰");
                if (postType == PostType.Coupon)
                {
                    ///Debug.Log("쿠폰");

                    postItem.content = "";
                    postItem.nickname = "";
                }
                //Debug.Log("쿠폰");

                if (postListJson[i]["items"].Count > 0)
                {
                    //Debug.Log("쿠폰");

                    for (var itemNum = 0; itemNum < postListJson[i]["items"].Count; itemNum++)
                    {
                        //Debug.Log("쿠폰");

                        UPostChartItem item = new UPostChartItem();
                        item.itemCount = int.Parse(postListJson[i]["items"][itemNum]["itemCount"].ToString());
                        item.chartFileName = postListJson[i]["items"][itemNum]["item"]["chartFileName"].ToString();
                        item.itemID = postListJson[i]["items"][itemNum]["item"]["idnumber"].ToString();
                        item.itemName = postListJson[i]["items"][itemNum]["item"]["name"].ToString();

                        postItem.items.Add(item);
                    }
                }

                postItemList.Add(postItem.inDate,postItem);

            
            }
        });

        SendQueue.Enqueue(Backend.UPost.GetPostList, PostType.Admin, 100, callback =>
        {
            if (!callback.IsSuccess())
            {
                LogError(callback.ToString());
            }

            admincall = callback;
            if (!callback.IsSuccess()) return;
            
            Log("어드민" + callback);

            
            
            Loadingobj.SetActive(false);
            JsonData postListJson = callback.GetReturnValuetoJSON()["postList"];

            if (totalcount < postListJson.Count)
                pool(totalcount - postslotlist.Count);
            
            postType = PostType.Admin;

            for (var i = 0; i < postListJson.Count; i++)
            {
                UPostItem postItem = new UPostItem();


                postItem.inDate = postListJson[i]["inDate"].ToString();
                postItem.title = postListJson[i]["title"].ToString();
                postItem.postType = postType;

                if (postType == PostType.Admin)
                {
                    postItem.content = postListJson[i]["content"].ToString();
                    postItem.expirationDate = DateTime.Parse(postListJson[i]["expirationDate"].ToString());
                    postItem.reservationDate = DateTime.Parse(postListJson[i]["reservationDate"].ToString());
                    postItem.nickname = postListJson[i]["nickname"]?.ToString();
                    postItem.sentDate = DateTime.Parse(postListJson[i]["sentDate"].ToString());

                    if (postListJson[i].ContainsKey("author"))
                    {
                        postItem.author = postListJson[i]["author"].ToString();
                    }

                    if (postListJson[i].ContainsKey("rankType"))
                    {
                        postItem.author = postListJson[i]["rankType"].ToString();
                    }
                }

                if (postListJson[i]["items"].Count > 0)
                {

                    for (var itemNum = 0; itemNum < postListJson[i]["items"].Count; itemNum++)
                    {

                        UPostChartItem item = new UPostChartItem();
                        item.itemCount = int.Parse(postListJson[i]["items"][itemNum]["itemCount"].ToString());
                        item.chartFileName = postListJson[i]["items"][itemNum]["item"]["chartFileName"].ToString();
                        item.itemID = postListJson[i]["items"][itemNum]["item"]["idnumber"].ToString();
                        item.itemName = postListJson[i]["items"][itemNum]["item"]["name"].ToString();

                        postItem.items.Add(item);
                    }
                }

//                Log("이이템을 넣는다" + postItem.inDate);
                postItemList.Add(postItem.inDate, postItem);
             //   Log("이이템을 " + postItemList.Count);
            }
            
            ShowPostList();
        });
        
        alertmanager.Instance.NotiCheck_Post(admincall,rankcall);
        

        if (postslotlist[0].gameObject.activeSelf)
        {
            foreach (var VARIABLE in alertmanager.Instance.Alert_Post)
            {
                VARIABLE.SetActive(true);
            }
        }
        else
        {
            foreach (var VARIABLE in alertmanager.Instance.Alert_Post)
            {
                VARIABLE.SetActive(false);
            }
        }
        
        ShowPostList();
    }

    private void ReceivePostItem(PostType postType,string postIndate)
    {
        var bro = Backend.UPost.ReceivePostItem(postType, postIndate);
        JsonData postList = bro.GetReturnValuetoJSON()["postItems"];
        List<string> itemid = new List<string>();
        List<string> howmany = new List<string>();
        for (int i = 0; i < postList.Count; i++)
        {
            if (postList[i].Count <= 0)
            {
                //Debug.Log("아이템이 없는 우편");
                continue;
            }
            string itemName = postList[i]["item"]["name"].ToString();
            string itemId = postList[i]["item"]["idnumber"].ToString();
            int itemCount = (int)postList[i]["itemCount"];
          
         
            itemid.Add(itemId);
            howmany.Add(itemCount.ToString());
            
            Inventory.Instance.AddItem(itemId, itemCount);

        }
        Inventory.Instance.ShowEarnItem2(itemid.ToArray(),howmany.ToArray(),false);
        postItemList.Remove(postIndate);
        alertmanager.Instance.NotiCheck_Post();
        Savemanager.Instance.SaveInventory_SaveOn();
        LogManager.Log_CrystalEarn("우편");
    }

    void ShowPostList()
    {
        for (int i = 0; i < postslotlist.Count; i++)
            postslotlist[i].gameObject.SetActive(false);

        
//        Log("가진 우편" + postItemList.Count);
        int num = 0;
        foreach (var VARIABLE in postItemList)
        {
            postslotlist[num].Refresh(VARIABLE.Value);
            postslotlist[num].gameObject.SetActive(true);
            num++;
        }
    }
    
    
    
    //아이템 없음
    public UIView PostInfoPanel_noitem;
    public Text postinfo_name_noitem;
    public Text postinfo_content_noitem;
    public Text postinfo_deletedate_noitem;
    public void ShowPostNoItem(UPostItem postdata)
    {
        nowdata = postdata;
        PostInfoPanel_noitem.Show(false);
        postinfo_name_noitem.text = postdata.title;
        postinfo_content_noitem.text = postdata.content;
        postinfo_deletedate_noitem.text = string.Format(Inventory.GetTranslate("UI/우편 만료 날짜"), postdata.expirationDate.ToString("yyyy-MM-dd hh:mm:ss"));
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)postinfo_content_noitem.GetComponentInParent<Transform>());
    }

    UPostItem nowdata;

    //아이템 있음
    public UIView PostInfoPanel_item;
    public Text postinfo_name_item;
    public Text postinfo_content_item;
    public Text postinfo_deletedate_item;
    public postitemslot[] postsitemslot;
    public void ShowPostItem(UPostItem postdata)
    {

        foreach (var t in postsitemslot)
        {
            t.gameObject.SetActive(false);
        }
        nowdata = postdata;
        PostInfoPanel_item.Show(false);
        postinfo_name_item.text = postdata.title;
        postinfo_content_item.text = postdata.content;

        for(var i = 0; i < nowdata.items.Count;i++)
        {
            postsitemslot[i].refresh(nowdata.items[i]);
            postsitemslot[i].gameObject.SetActive(true);
        }

        if (postdata.expirationDate == DateTime.MinValue)
        {
            postinfo_deletedate_item.text = "";
        }
        else
        {
            postinfo_deletedate_item.text = string.Format(Inventory.GetTranslate("UI/우편 만료 날짜"), postdata.expirationDate.ToString("yyyy-MM-dd hh:mm:ss"));
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)postinfo_content_item.GetComponentInParent<Transform>());
    }


    public void Bt_GetPost_Item()
    {
        if (!Settingmanager.Instance.CheckServerOn())
        {
            return;
        }
        
        PostInfoPanel_noitem.Hide(true);
        PostInfoPanel_item.Hide(true);
        //Debug.Log(("타입" + nowdata.postType));
        ReceivePostItem(nowdata.postType, nowdata.inDate);
        ShowPostList();
    }
    public void Bt_GetPost_ALlItem()
    {
        if (!Settingmanager.Instance.CheckServerOn())
        {
            return;
        }
        List<string> itemid = new List<string>();
        List<string> howmany = new List<string>();
        PostInfoPanel_noitem.Hide(true);
        PostInfoPanel_item.Hide(true);
        itemssAll.Clear();
        //공지
        GetAllAdmin();
        GetAllRank();
        GetAllCoupopn();

        if (itemssAll.Count.Equals(0))
        {
            Debug.Log("가져올게 없다");
            return;
        }
        
        for (int i = 0; i < itemssAll.Count; i++)
        {
            if (itemid.Contains(itemssAll[i].itemID))
            {
                int a = itemid.IndexOf(itemssAll[i].itemID);
                decimal total = decimal.Parse(howmany[a]) + itemssAll[i].itemCount;
                howmany[a] = total.ToString("N0");
            }
            else
            {
                itemid.Add(itemssAll[i].itemID);
                howmany.Add(itemssAll[i].itemCount.ToString());
            }
            
            Inventory.Instance.AddItem(itemssAll[i].itemID,itemssAll[i].itemCount);
        }

        Inventory.Instance.ShowEarnItem2(itemid.ToArray(),howmany.ToArray(),false);
        Savemanager.Instance.SaveInventory_SaveOn();
        
        GetPostList();
    }
    List<UPostChartItem> itemssAll = new List<UPostChartItem>();
    public void GetAllAdmin()
    {
        BackendReturnObject bro = Backend.UPost.GetPostList(PostType.Admin, 100);

        if (bro.IsSuccess() == false) {
            Debug.Log("우편을 불러오는 중 에러가 발생했습니다.");
            return;
        }

        var receiveBro = Backend.UPost.ReceivePostItemAll(PostType.Admin);

        if (receiveBro.IsSuccess() == false) {
            Debug.LogError("우편 모두 수령하기 중 에러가 발생하였습니다. : " + receiveBro);
            return;
        }

        foreach (JsonData postItemJson in receiveBro.GetReturnValuetoJSON()["postItems"]) {

            for (int j = 0; j < postItemJson.Count; j++) {
                if (!postItemJson[j].ContainsKey("item")) {
                    continue;
                }

                UPostChartItem item = new UPostChartItem();
                if (postItemJson[j]["item"].ContainsKey("itemName")) {
                    item.itemName = postItemJson[j]["item"]["itemName"].ToString();
                }

                // 랭킹 보상의 경우 chartFileName이 존재하지 않습니다.  
                if (postItemJson[j]["item"].ContainsKey("chartFileName")) {
                    item.chartFileName = postItemJson[j]["item"]["chartFileName"].ToString();
                }

                if (postItemJson[j]["item"].ContainsKey("idnumber")) {
                    item.itemID = postItemJson[j]["item"]["idnumber"].ToString();
                }

                if (postItemJson[j].ContainsKey("itemCount")) {
                    item.itemCount = Int32.Parse(postItemJson[j]["itemCount"].ToString());
                }

                itemssAll.Add(item);
                Debug.Log(item.ToString());
            }
        }
    }
    public void GetAllRank()
    {
        BackendReturnObject bro = Backend.UPost.GetPostList(PostType.Rank, 100);

        if (bro.IsSuccess() == false) {
            Debug.Log("우편을 불러오는 중 에러가 발생했습니다.");
            return;
        }

        var receiveBro = Backend.UPost.ReceivePostItemAll(PostType.Rank);

        if (receiveBro.IsSuccess() == false) {
            Debug.LogError("우편 모두 수령하기 중 에러가 발생하였습니다. : " + receiveBro);
            return;
        }

        foreach (JsonData postItemJson in receiveBro.GetReturnValuetoJSON()["postItems"]) {

            for (int j = 0; j < postItemJson.Count; j++) {
                if (!postItemJson[j].ContainsKey("item")) {
                    continue;
                }

                UPostChartItem item = new UPostChartItem();
                if (postItemJson[j]["item"].ContainsKey("itemName")) {
                    item.itemName = postItemJson[j]["item"]["itemName"].ToString();
                }

                // 랭킹 보상의 경우 chartFileName이 존재하지 않습니다.  
                if (postItemJson[j]["item"].ContainsKey("chartFileName")) {
                    item.chartFileName = postItemJson[j]["item"]["chartFileName"].ToString();
                }

                if (postItemJson[j]["item"].ContainsKey("idnumber")) {
                    item.itemID = postItemJson[j]["item"]["idnumber"].ToString();
                }

                if (postItemJson[j].ContainsKey("itemCount")) {
                    item.itemCount = Int32.Parse(postItemJson[j]["itemCount"].ToString());
                }

                itemssAll.Add(item);
                Debug.Log(item.ToString());
            }
        }
    }
    public void GetAllCoupopn()
    {
        BackendReturnObject bro = Backend.UPost.GetPostList(PostType.Coupon, 100);

        if (bro.IsSuccess() == false) {
            Debug.Log("우편을 불러오는 중 에러가 발생했습니다.");
            return;
        }

        var receiveBro = Backend.UPost.ReceivePostItemAll(PostType.Coupon);

        if (receiveBro.IsSuccess() == false) {
            Debug.LogError("우편 모두 수령하기 중 에러가 발생하였습니다. : " + receiveBro);
            return;
        }

        foreach (JsonData postItemJson in receiveBro.GetReturnValuetoJSON()["postItems"]) {

            for (int j = 0; j < postItemJson.Count; j++) {
                if (!postItemJson[j].ContainsKey("item")) {
                    continue;
                }

                UPostChartItem item = new UPostChartItem();
                if (postItemJson[j]["item"].ContainsKey("itemName")) {
                    item.itemName = postItemJson[j]["item"]["itemName"].ToString();
                }

                // 랭킹 보상의 경우 chartFileName이 존재하지 않습니다.  
                if (postItemJson[j]["item"].ContainsKey("chartFileName")) {
                    item.chartFileName = postItemJson[j]["item"]["chartFileName"].ToString();
                }

                if (postItemJson[j]["item"].ContainsKey("idnumber")) {
                    item.itemID = postItemJson[j]["item"]["idnumber"].ToString();
                }

                if (postItemJson[j].ContainsKey("itemCount")) {
                    item.itemCount = Int32.Parse(postItemJson[j]["itemCount"].ToString());
                }

                Debug.Log("넣은 아이디" + item.itemID);
                itemssAll.Add(item);
                Debug.Log(item.ToString());
            }
        }
    }
}

public class UPostChartItem
{
    public string chartFileName;
    public string itemID;
    public string itemName;
    public int itemCount;
    public override string ToString()
    {
        return
        "item : \n" +
        $"| chartFileName : {chartFileName}\n" +
        $"| idnumber : {itemID}\n" +
        $"| name : {itemName}\n" +
        $"| itemCount : {itemCount}\n";
    }
}
public class UPostItem
{
    public PostType postType;
    public string title;
    public string content;
    public DateTime expirationDate;
    public DateTime reservationDate;
    public DateTime sentDate;
    public string nickname;
    public string inDate;
    public string author; // 관리자 우편만 존재합니다.
    public string rankType; // 랭킹 우편만 존재합니다.
    public List<UPostChartItem> items = new List<UPostChartItem>();
    public override string ToString()
    {
        string totalString =
        $"title : {title}\n" +
        $"inDate : {inDate}\n";
        if (postType == PostType.Admin || postType == PostType.Rank)
        {
            totalString +=
            $"content : {content}\n" +
            $"expirationDate : {expirationDate}\n" +
            $"reservationDate : {reservationDate}\n" +
            $"sentDate : {sentDate}\n" +
            $"nickname : {nickname}\n";
            if (postType == PostType.Admin)
            {
                totalString += $"author : {author}\n";
            }
            if (postType == PostType.Rank)
            {
                totalString += $"rankType : {rankType}\n";
            }
        }
        string itemList = string.Empty;
        for (int i = 0; i < items.Count; i++)
        {
            itemList += items[i].ToString();
            itemList += "\n";
        }
        totalString += itemList;
        return totalString;
    }
}