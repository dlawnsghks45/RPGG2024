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
    public Text LockText; //���谡 ���� 5 �̻� ���� ����
    public Text TierText; //���谡 ���� 5 �̻� ���� ����

    public Color[] fieldcolor;
    public Image FieldLevelColor;
    public int nowfieldlv; //0�� ���� 1���Ǹ� 2�� ����
    public void Refresh(string id)
    {
        mapid = id;
        MapName.text = Inventory.GetTranslate(MapDB.Instance.Find_id(mapid).name);
        decimal d = decimal.Parse(monsterDB.Instance.Find_id(MapDB.Instance.Find_id(mapid).monsterid).hp);
        MapHP.text = $"HP:{dpsmanager.convertNumber(d)}";    
        //   MapLevel.text = string.Format(Inventory.GetTranslate("UI/��õ ����"), MapDB.Instance.Find_id(mapid).minlevel, MapDB.Instance.Find_id(mapid).maxlevel);
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
            //���
            mapgobutton.interactable = false;
            Lockobj.SetActive(true);
            LockText.text = string.Format(Inventory.GetTranslate("UI/�������������ʿ�"), Inventory.GetTranslate(MapDB.Instance.Find_id(MapDB.Instance.Find_id(mapid).mapneedid).name));
        }

        else
        {
            //���ǰ
            mapgobutton.interactable = true;
            Lockobj.SetActive(false);
        }
    }

    public void Bt_ShowDrop()
    {
        mapmanager.Instance.ShowDropItemField(mapid);
    }
}
