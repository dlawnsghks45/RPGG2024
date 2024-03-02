using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class jobslot : MonoBehaviour
{
    public string ClassId;

    public GameObject noti;
    
    [SerializeField]
    Image ClassSprite;

    [SerializeField]
    Text ClassName;

    [SerializeField]
    GameObject LockPanel;

    [SerializeField]
    Image ClassBackGround;

    [SerializeField]
    GameObject ClassPassive;

    [SerializeField]
    Color[] Colors; //0은 흰색 1은 사용중인색
    public bool islock;
    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        ClassSprite.sprite = SpriteManager.Instance.GetSprite(ClassDB.Instance.Find_id(ClassId).classsprite);
        ClassName.text = Inventory.GetTranslate(ClassDB.Instance.Find_id(ClassId).name);
        Inventory.Instance.ChangeItemRareColor(ClassName,ClassDB.Instance.Find_id(ClassId).tier);

        ClassBackGround.color = Colors[0];

        if(PlayerBackendData.Instance.PassiveClassId.Length != 0)
        {
//            Debug.Log(PlayerBackendData.Instance.PassiveClassId.Length);
            if (PlayerBackendData.Instance.PassiveClassId[int.Parse(ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassData[ClassId].ClassId1).tier) - 1] ==
          ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassData[ClassId].ClassId1).passive)
            {
                ClassBackGround.color = Colors[2];
                ClassPassive.SetActive(true);
            }
            else
                ClassPassive.SetActive(false);
        }
       


        if (PlayerBackendData.Instance.ClassData[ClassId].ClassId1 == PlayerBackendData.Instance.ClassId)
        {
            ClassBackGround.color = Colors[1];
        }
        

//W        Debug.Log("아이디 " + PlayerBackendData.Instance.ClassData[ClassId].ClassId1 +"OWN " + PlayerBackendData.Instance.ClassData[ClassId].Isown);
        if (PlayerBackendData.Instance.ClassData[ClassId].Isown)
        {
            LockPanel.SetActive(false);
            islock = false;
        }
        else
        {
            LockPanel.SetActive(true);
            islock = true;
        }
    }

    private void OnEnable()
    {
        noti.SetActive(false);
        //필용아이템이 있다 
        if (PlayerBackendData.Instance.CheckItemCount(ClassDB.Instance.Find_id(ClassId).RequiredItemID) > 0)
        {
            if (!PlayerBackendData.Instance.ClassData[ClassId].Isown)
                noti.SetActive(true);
        }
    }

    public bool CheckNoti() //false면 활성화 안함 true면 활성화
    {
        noti.SetActive(false);
        if (PlayerBackendData.Instance.ClassData[ClassId].Isown)
        {
            return false;
        }
        //아이템이 있고 
        if (PlayerBackendData.Instance.CheckItemCount(ClassDB.Instance.Find_id(ClassId).RequiredItemID) > 0)
        {
            //가지고있는게아니라면.
            if (PlayerBackendData.Instance.ClassData[ClassId].Isown)
            {
                noti.SetActive(false);
                return false;
            }
            else
            {
                noti.SetActive(true);
                return true;
            }
        }
        return false;
    }
    public void Bt_ShowClass()
    {
        Classmanager.Instance.OpenClassSelection(ClassId);
    }
}
