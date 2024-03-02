using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class AutoSmelting : MonoBehaviour
{
    //�̱��游���.
    private static AutoSmelting _instance = null;

    public static AutoSmelting Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(AutoSmelting)) as AutoSmelting;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    public itemshowcountslot ItemCountSlots;
    public itemshowcountslot ItemCountSlots_Doing;

    public int needcount;
    
    public SmeltSlot[] SmeltSlot;
    public int SmeltCount; //��ǥ
    public int UsedItem; //����� ���ü�
    public Text UsedItemText;
    public int UsedCrystal; //����� ũ����Ż
    public Text UsedCrystalText;

    public int ResetCount; //�ʱ�ȭ Ƚ��
    public Text ResetText;
    
    public Text AutoSmeltinginfo; //�ʱ�ȭ Ƚ��

    
    public UIView SmeltObj;
    public UIButton SmeltStartButton;
    public UIToggle AutoSmeltToggle;
    public string needitem;
    public void Bt_StartAutoSmelting()
    {
        if (Inventory.Instance.data.StoneCount1 != 0)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/�ڵ����úҰ�"),alertmanager.alertenum.�Ϲ�);
            return;
        }

        needitem = EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid).smeltid;
        needcount= int.Parse(EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid).smeltcount);
        ItemCountSlots.id = needitem;
        ItemCountSlots.ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid).smeltid).sprite);
        
        ItemCountSlots_Doing.id = needitem;
        ItemCountSlots_Doing.ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid).smeltid).sprite);

        
        SmeltObj.Show(false);
        SmeltCount = 0;
        UsedItem = 0;
        UsedCrystal = 0;
        ResetCount = 0;
        RefreshSmeltDoing();
        foreach (var t in SmeltSlot)
        {
            t.gameObject.SetActive(false);
        }
        for (var index = 0; index < Inventory.Instance.data.MaxStoneCount1; index++)
        {
            var t = SmeltSlot[index];
            t.gameObject.SetActive(true);
            t.SmeltShow(0);
        }

        RefreshSmelt();
    }

    public void Bt_Plus()
    {
        if (SmeltCount.Equals(Inventory.Instance.data.MaxStoneCount1))
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/�ִ�"), alertmanager.alertenum.�Ϲ�);
            return;
        }
        SmeltSlot[SmeltCount].SmeltShow(1);
        SmeltCount++;
        RefreshSmelt();
    }

    public void Bt_Minus()
    {
        if (SmeltCount.Equals(0))
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/�ּ�"), alertmanager.alertenum.�Ϲ�);
            return;
        }
        
        SmeltSlot[SmeltCount-1].SmeltShow(0);
        SmeltCount--;

        RefreshSmelt();
    }

    public void RefreshSmelt()
    {
        if (SmeltCount > 0)
            SmeltStartButton.Interactable = true;
        else
        {
            SmeltStartButton.Interactable = false;
        }
    }


    public UIView Smeltoriginobj;
    public UIView SmeltingingObj;
    public void Bt_StartAutoSmeltingStart()
    {
        SmeltObj.Hide(true);
        Smeltoriginobj.Hide(true);
        SmeltingingObj.Show(false);
        A = StartSmelt();
        StartCoroutine(A);
    }
/*
 *
 *   public int UsedItem; //����� ���ü�
    public Text UsedItemText;
    public int UsedCrystal; //����� ũ����Ż
    public Text UsedCrystalText;

    public int ResetCount; //�ʱ�ȭ Ƚ��
    public Text ResetText;
 */
    public void RefreshSmeltDoing()
    {
        UsedItemText.text = UsedItem.ToString();
        UsedCrystalText.text = UsedCrystal.ToString();
        ResetText.text = ResetCount.ToString();
    }

    private void Start()
    {
        A = StartSmelt();
    }

    public IEnumerator StartSmelt()
    {
        while (true)
        {
            if (PlayerBackendData.Instance.CheckItemCount(needitem) < needcount)
            {
                Debug.Log("������ ����");
                AutoSmeltinginfo.text = Inventory.GetTranslate("UI3/�����������ü�");
                StopCoroutine(A);
                yield break;
            }
            //������ Ƚ���� 5
            //�ִ� Ƚ���� 7

            //7 - 3+2 = 2
            //���� ��ȭ ���� Ƚ���� 2
            //�ʿ� ��ȭ Ƚ���� 5-3 = 4
            
            //����Ƚ���� ���簡�� Ƚ�� 
//Debug.Log("durlek");
            if (Inventory.Instance.data.SmeltSuccCount1 == SmeltCount)
            {
                //��������
                //�� 
                AutoSmeltinginfo.text = Inventory.GetTranslate("UI3/�������Ἲ��");
                yield break;

            }
            
            //�����ڵ�
            if (AutoSmeltToggle.IsOn)
            {
                //�ִ� Ƚ�� - (���� ����) = ���� Ƚ��
                //��ǥ ���� - ���� = ���� ����
                //���� Ƚ������ ������ �� ������ ����
                if (Inventory.Instance.data.MaxStoneCount1 -
                    (Inventory.Instance.data.SmeltSuccCount1 + Inventory.Instance.data.SmeltFailCount1) <
                    SmeltCount - Inventory.Instance.data.SmeltSuccCount1)
                {
                    //���µ��̾���
                    if (PlayerBackendData.Instance.GetCash() < 1000)
                    {
                        AutoSmeltinginfo.text = Inventory.GetTranslate("UI3/��������ũ����Ż");
                        Debug.Log("�ڵ����½���");
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/�ڵ�����ũ����Ż����"), alertmanager.alertenum.�Ϲ�);
                        Savemanager.Instance.SaveTypeEquip();
                        StopCoroutine(A);
                        yield break;
                    }

                    UsedCrystal += 1000;
                    ResetCount++;
                    Inventory.Instance.Bt_StartResetSmelt();
                }
            }
            else
            {
                if (Inventory.Instance.data.MaxStoneCount1.Equals(Inventory.Instance.data.StoneCount1))
                {
                    //������ ����
                  //  Debug.Log("������");
                    AutoSmeltinginfo.text = Inventory.GetTranslate("UI3/��������");
                    StopCoroutine(A);
                    yield break;

                }

                if (Inventory.Instance.data.MaxStoneCount1 -
                    (Inventory.Instance.data.SmeltSuccCount1 + Inventory.Instance.data.SmeltFailCount1) <
                    SmeltCount - Inventory.Instance.data.SmeltSuccCount1)
                {
                    AutoSmeltinginfo.text = Inventory.GetTranslate("UI3/��������");
                    StopCoroutine(A);
yield break;
                }
            }

            if (PlayerBackendData.Instance.CheckItemCount(needitem) < needcount)
            {
                AutoSmeltinginfo.text = Inventory.GetTranslate("UI3/�����������ü�");
                StopCoroutine(A);
            }

          
            Inventory.Instance.iscansmelt = true;
            Inventory.Instance.smeltdelaybool = false;
            Inventory.Instance.Bt_SmeltEquipItem();
            RefreshSmeltDoing();
                yield return new WaitForSeconds(1.5f);

        }
    }

    private IEnumerator  A;
    public int SmeltingCount;
    public void Bt_CancelSmelt()
    {
        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/�ڵ���������"), alertmanager.alertenum.�Ϲ�);
        SmeltingingObj.Hide(true);
        StopCoroutine(A);
    }
}
