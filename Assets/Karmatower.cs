using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class Karmatower : MonoBehaviour
{
    public UIToggle[] StageToggle;
    public GameObject[] StagePanels;
    public GameObject[] StageEffect;

    public void ShowStage(int num)
    {
        foreach (var VARIABLE in StagePanels)
        {
            VARIABLE.SetActive(false);
        }
        foreach (var VARIABLE in StageEffect)
        {
            VARIABLE.SetActive(false);
        }
        switch (num)
        {
            case 0:
                StagePanels[num].SetActive(true);
                break;

            case 1:
                StagePanels[num].SetActive(true);
                StageEffect[0].SetActive(true);
                break;

            case 2:
                StagePanels[num].SetActive(true);
                StageEffect[1].SetActive(true);

                break;

            case 3:
                StagePanels[num].SetActive(true);
                StageEffect[2].SetActive(true);

                break;
            case 4: //카르마의 탑
                if (int.Parse(Autofarmmanager.Instance.GetMapIdByFI()) < 1134)
                {
                    StagePanels[0].SetActive(true);
                    StageToggle[0].IsOn = true;
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI8/클리어제한마족"),alertmanager.alertenum.주의);
                    break;
                }

                ShowKarma();
                StageEffect[3].SetActive(true);
                StagePanels[num].SetActive(true);
                break;
            
        }
        
        
        
    }
    
    
    
    public string[] mapids;
    public string mapid;
    public Image MobImage;
    public Image BackImage;
    public Text MapName;
    public Text MobHp;
    public countpanel countpanels;


    private void Start()
    {
        countpanels.Maxcount = mapids.Length;
    }

    public void ShowKarma()
    {
        
        if (int.Parse(MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage).mapneednum) > 135)
        {
            mapid = PlayerBackendData.Instance.nowstage;
        }
        else
        {
            mapid = Autofarmmanager.Instance.GetMapIdByFI2();
        }
        MapDB.Row mapdata = MapDB.Instance.Find_id(mapid);
        monsterDB.Row mobdata = monsterDB.Instance.Find_id(mapdata.monsterid);
        string[] mobimage = mobdata.sprite.Split(';');
        MobImage.sprite = SpriteManager.Instance.GetSprite(mobimage[0]);
        BackImage.sprite = SpriteManager.Instance.GetSprite(mapdata.maplayer0);
        MapName.text = Inventory.GetTranslate(mapdata.name);
        MobHp.text = dpsmanager.convertNumber(decimal.Parse(mobdata.hp));

        countpanels.Maxcount = PlayerBackendData.Instance.GetFieldLv() - 134;
        int countnum = int.Parse(mapdata.mapneednum) - 134;
        if (countnum == 0)
            countnum = 1;
        countpanels.SetCount(countnum);
        DropCheck();
    }

    public itemslot[] dropitem;
    public void ShowKarma2()
    {
        MapDB.Row mapdata = MapDB.Instance.Find_id(mapid);
        monsterDB.Row mobdata = monsterDB.Instance.Find_id(mapdata.monsterid);
        string[] mobimage = mobdata.sprite.Split(';');
        MobImage.sprite = SpriteManager.Instance.GetSprite(mobimage[0]);
        BackImage.sprite = SpriteManager.Instance.GetSprite(mapdata.maplayer0);
        MapName.text = Inventory.GetTranslate(mapdata.name);
        MobHp.text = dpsmanager.convertNumber(decimal.Parse(mobdata.hp));
        DropCheck();

    }

    void DropCheck()
    {
        foreach (var VARIABLE in dropitem)
        {
            VARIABLE.gameObject.SetActive(false);
        }
        MapDB.Row mapdata = MapDB.Instance.Find_id(mapid);
        monsterDB.Row mondata = monsterDB.Instance.Find_id(mapdata.monsterid);

        string dropid_boss = mondata.bossdrop;

        //일반몹 드롭테이블
        List<MonDropDB.Row> dropdatas_basic = MonDropDB.Instance.FindAll_id(dropid_boss);
        // Debug.Log(dropdatas_basic.Count);
        for (int i = 0; i < dropdatas_basic.Count; i++)
        {
            dropitem[i].SetItem(dropdatas_basic[i].itemid, 0);
            dropitem[i].gameObject.SetActive(true);
        }
    }
    public void Refresh()
    {
        int countnum = countpanels.nowcount + 134;
        mapid = MapDB.Instance.Find_mapneednum(countnum.ToString()).id;
        ShowKarma2();
    }

    public void Bt_LocateMap()
    {
        mapmanager.Instance.ShowDropItemField(mapid);
    }
    
}
