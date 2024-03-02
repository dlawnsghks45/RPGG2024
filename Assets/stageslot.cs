using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine.UI;

public class stageslot : MonoBehaviour
{
    public string mapid;

    public Text MapName;
    public Text MapHP;
    public Image MonsterImage;
    public Image BackgroundImage0;

    public Button mapgobutton;
    public GameObject Lockobj;
    public Text LockText; //모험가 레벨 5 이상 입장 가능
    public Text TierText; //모험가 레벨 5 이상 입장 가능

    public Color[] fieldcolor;
    public Image FieldLevelColor;
    public int nowfieldlv; //0은 보통 1은악몽 2는 지옥
    public void Refresh(string id)
    {
        mapid = id;
        MapName.text = Inventory.GetTranslate(MapDB.Instance.Find_id(mapid).name);
        decimal d = decimal.Parse(monsterDB.Instance.Find_id(MapDB.Instance.Find_id(mapid).monsterid).hp);
        MapHP.text = $"HP:{dpsmanager.convertNumber(d)}";    
        //   MapLevel.text = string.Format(Inventory.GetTranslate("UI/추천 레벨"), MapDB.Instance.Find_id(mapid).minlevel, MapDB.Instance.Find_id(mapid).maxlevel);
        MonsterImage.sprite = SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id(MapDB.Instance.Find_id(mapid).monsterid).sprite.Split(';')[0]);
        TierText.text = PlayerData.Instance.gettierstar(MapDB.Instance.Find_id(mapid).maprank);
         BackgroundImage0.sprite = SpriteManager.Instance.GetSprite(MapDB.Instance.Find_id(MapDB.Instance.Find_id(mapid).monsterid).maplayer0);

        FieldLevelColor.color = fieldcolor[nowfieldlv];

        CheckLock();
    }

    public void OnEnable()
    {
        CheckLock();
    }

    public void Bt_ShowStage()
    {
        mapmanager.Instance.ShowDropItemField(mapid);
     
    }

    public void CheckLock()
    {

        if (PlayerBackendData.Instance.GetFieldLv() < int.Parse(MapDB.Instance.Find_id(mapid).mapneednum))
        {
            //잠금
            mapgobutton.interactable = false;
            Lockobj.SetActive(true);
            LockText.text = string.Format(Inventory.GetTranslate("UI/이전스테이지필요"), Inventory.GetTranslate(MapDB.Instance.Find_id(MapDB.Instance.Find_id(mapid).mapneedid).name));
        }

        else
        {
            //잠금품
            mapgobutton.interactable = true;
            Lockobj.SetActive(false);
        }
    }

    public void Bt_ShowDrop()
    {
        mapmanager.Instance.ShowDropItemField(mapid);
    }
}
