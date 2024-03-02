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
    Color[] Colors; //0�� ��� 1�� ������λ�
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
        

//W        Debug.Log("���̵� " + PlayerBackendData.Instance.ClassData[ClassId].ClassId1 +"OWN " + PlayerBackendData.Instance.ClassData[ClassId].Isown);
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
        //�ʿ�������� �ִ� 
        if (PlayerBackendData.Instance.CheckItemCount(ClassDB.Instance.Find_id(ClassId).RequiredItemID) > 0)
        {
            if (!PlayerBackendData.Instance.ClassData[ClassId].Isown)
                noti.SetActive(true);
        }
    }

    public bool CheckNoti() //false�� Ȱ��ȭ ���� true�� Ȱ��ȭ
    {
        noti.SetActive(false);
        if (PlayerBackendData.Instance.ClassData[ClassId].Isown)
        {
            return false;
        }
        //�������� �ְ� 
        if (PlayerBackendData.Instance.CheckItemCount(ClassDB.Instance.Find_id(ClassId).RequiredItemID) > 0)
        {
            //�������ִ°Ծƴ϶��.
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
