using System;
using BackEnd;
using Doozy.Engine.UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Contentmanager : MonoBehaviour
{
    //싱글톤만들기.
    private static Contentmanager _instance = null;
    public static Contentmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Contentmanager)) as Contentmanager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }
    
    public UIView ContentPanel;
    public Image Background;
    public Image BossImage;
    public Text ContentMapName;
    public Text ContentLevel;
    public itemiconslot[] DropItems;

    public infoshowslot infoname;

    private string[] monid;
    [SerializeField]
    private string[] dropid;
    private string[] hplevel;
    private string[] atklevel;
    
    public string[] monid_playing;
    public string[] dropid_playing;
    public string[] hplevel_playing;
    private string[] atklevel_playing;

    public Text ContentLevelHp;
    
    public int nowlevel;
    private ContentEnterDB.Row data;
    private ContentEnterDB.Row dataplaying;
    
    
    public int nowplayinglevel;
    public int nowcontentnum;
    public int nowPlaycontentnum;

    
    public void Bt_SelectContent(string num)
    {
        data = ContentEnterDB.Instance.Find_num(num);
        monid = data.monid.Split(';');
        dropid = data.reward.Split(';');
        hplevel = data.uphp.Split(';');
        atklevel = data.upattack.Split(';');
        
        if (PlayerBackendData.Instance.ContentLevel[int.Parse(num)] >= hplevel.Length)
        {
            nowlevel = hplevel.Length-1;
        }
        else
        {
            nowlevel = PlayerBackendData.Instance.ContentLevel[int.Parse(num)];
        }
        ContentPanel.Show(false);
       
        nowcontentnum = int.Parse(num);
        //Debug.Log(data.monid);
     
        Background.sprite = SpriteManager.Instance.GetSprite(data.background);
        BossImage.sprite = SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id(MapDB.Instance.Find_id(data.mapid).monsterid).sprite);
        ContentMapName.text =
            Inventory.GetTranslate(MapDB.Instance.Find_id(data.mapid).name);
        infoname.infoname = data.infoname;
        ShowReward();
    }


    public Decimal GetHp(decimal hp,int num)
    {
        decimal a = (decimal)(hp* (decimal)math.pow(float.Parse(hplevel_playing[num]),num));
        return a;
    }
    public Decimal GetHpReward(decimal hp,int num)
    {
        decimal a = (decimal)(hp* (decimal)math.pow(float.Parse(hplevel[num]),num));
        return a;
    }
    public float GetAttack(float attack)
    {
        float a = attack * float.Parse(atklevel_playing[nowplayinglevel]);
//        Debug.Log("공격력" + a);
        return a;
    }
    public void ShowReward()
    {
        ContentLevel.text = $"{nowlevel + 1}단계";
      //  Debug.Log(monsterDB.Instance.Find_id(data.monid.Split(';')[^1]).hp);
        ContentLevelHp.text = dpsmanager.convertNumber(GetHpReward(decimal.Parse(monsterDB.Instance.Find_id(data.monid.Split(';')[^1]).hp) *10m ,nowlevel));
        
        for (int i = 0; i < DropItems.Length; i++)
        {
            DropItems[i].canvas.alpha = 0;
            DropItems[i].canvas.interactable = false;
            DropItems[i].canvas.blocksRaycasts = false;
        }

        int num = 0;
        int num2 = int.Parse(MonDropDB.Instance.Find_id(dropid[nowlevel]).num);
        bool isfind = false;
        for (int i = num2; i < MonDropDB.Instance.NumRows() - 1; i++)
        {
            if (dropid[nowlevel] != "0")
            {
                if (MonDropDB.Instance.Find_num(i.ToString()).id.Equals(dropid[nowlevel]))
                {
                    if(MonDropDB.Instance.Find_num(i.ToString()).isshow.Equals("FALSE"))
                        continue;
                    //     Debug.Log("들어갔다" + MonDropDB.Instance.Find_num(i.ToString()).itemid);
                    isfind = true;
                    DropItems[num].Refresh(MonDropDB.Instance.Find_num(i.ToString()).itemid, int.Parse(MonDropDB.Instance.Find_num(i.ToString()).minhowmany), false);
                    DropItems[num].canvas.alpha = 1;
                    DropItems[num].canvas.interactable = true;
                    DropItems[num].canvas.blocksRaycasts = true;
                    num++;
                }
            }

            if (isfind && !MonDropDB.Instance.Find_num(i.ToString()).id.Equals(dropid[nowlevel]))
                break;
        }
    }

    public void Bt_LocateContent()
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/사냥터만가능"), alertmanager.alertenum.주의);
            return;
        }
        
        if (mapmanager.Instance.islocating)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/맵이동중불가"),alertmanager.alertenum.주의);
            return;
        }

        if (PlayerBackendData.Instance.CheckItemCount(data.needitem) < int.Parse(data.needhowmany))
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/입장권부족"), alertmanager.alertenum.주의);
            return;
        }
        
        if (!Settingmanager.Instance.CheckServerOn())
        {
            return;
        }
        monid_playing = monid;
        dropid_playing = dropid;
        nowplayinglevel = nowlevel;
        nowPlaycontentnum = nowcontentnum;
        hplevel_playing = hplevel;
        atklevel_playing = atklevel;
        dataplaying = data;
        LogManager.ContentLog(nowcontentnum.ToString(), data.needitem);
        PlayerBackendData.Instance.spawncount = 3;
        PlayerBackendData.Instance.RemoveItem(data.needitem,int.Parse(data.needhowmany));
        mapmanager.Instance.LocateMap(dataplaying.mapid);
        Savemanager.Instance.SaveInventory_SaveOn();
    }
    
    public void UpLevel()
    {
        if (PlayerBackendData.Instance.ContentLevel[int.Parse(data.num)] == nowlevel)
        {
            return;
        }

        if (nowlevel == hplevel.Length-1)
        {
            return;
        }
        
        nowlevel++;
        ShowReward();

    }
    
    

    public void DownLevel()
    {
        if (0 == nowlevel)
        {
            return;
        }

        nowlevel--;
        ShowReward();
    }


    public Sotangpanel sotangpanels;
    public void Bt_ShowSotang()
    {
        if (PlayerBackendData.Instance.ContentLevel[nowcontentnum] <= nowlevel)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/1회클리어소탕가능"), alertmanager.alertenum.일반);
            return;
        }

        sotangpanels.Show(dropid[nowlevel]);
    }
    public void Bt_StartSotang()
    {
        if (!Settingmanager.Instance.CheckServerOn())
        {
            return;
            //다시 시도
        }

        
        if (sotangpanels.StartSotang())
        {
            LogManager.ContentSotangLog(data.num, "1750", sotangpanels.sotangcount);
            sotangpanels.SotangObj.Hide(false);
           
        }
    }
}
