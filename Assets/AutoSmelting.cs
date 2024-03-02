using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class AutoSmelting : MonoBehaviour
{
    //싱글톤만들기.
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
    public int SmeltCount; //목표
    public int UsedItem; //사용한 제련석
    public Text UsedItemText;
    public int UsedCrystal; //사용한 크리스탈
    public Text UsedCrystalText;

    public int ResetCount; //초기화 횟수
    public Text ResetText;
    
    public Text AutoSmeltinginfo; //초기화 횟수

    
    public UIView SmeltObj;
    public UIButton SmeltStartButton;
    public UIToggle AutoSmeltToggle;
    public string needitem;
    public void Bt_StartAutoSmelting()
    {
        if (Inventory.Instance.data.StoneCount1 != 0)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/자동제련불가"),alertmanager.alertenum.일반);
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
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/최대"), alertmanager.alertenum.일반);
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
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/최소"), alertmanager.alertenum.일반);
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
 *   public int UsedItem; //사용한 제련석
    public Text UsedItemText;
    public int UsedCrystal; //사용한 크리스탈
    public Text UsedCrystalText;

    public int ResetCount; //초기화 횟수
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
                Debug.Log("개수가 부족");
                AutoSmeltinginfo.text = Inventory.GetTranslate("UI3/제련종료제련석");
                StopCoroutine(A);
                yield break;
            }
            //설정한 횟수가 5
            //최대 횟수가 7

            //7 - 3+2 = 2
            //남은 강화 가능 횟수는 2
            //필요 강화 횟수는 5-3 = 4
            
            //성공횟수와 현재가능 횟수 
//Debug.Log("durlek");
            if (Inventory.Instance.data.SmeltSuccCount1 == SmeltCount)
            {
                //성공헀다
                //료 
                AutoSmeltinginfo.text = Inventory.GetTranslate("UI3/제련종료성공");
                yield break;

            }
            
            //리셋자동
            if (AutoSmeltToggle.IsOn)
            {
                //최대 횟수 - (성공 실패) = 남은 횟수
                //목표 성공 - 성공 = 남은 성공
                //남은 횟수보다 성공이 더 많으면 리셋
                if (Inventory.Instance.data.MaxStoneCount1 -
                    (Inventory.Instance.data.SmeltSuccCount1 + Inventory.Instance.data.SmeltFailCount1) <
                    SmeltCount - Inventory.Instance.data.SmeltSuccCount1)
                {
                    //리셋돈이없다
                    if (PlayerBackendData.Instance.GetCash() < 1000)
                    {
                        AutoSmeltinginfo.text = Inventory.GetTranslate("UI3/제련종료크리스탈");
                        Debug.Log("자동리셋실패");
                        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/자동제련크리스탈부족"), alertmanager.alertenum.일반);
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
                    //다차서 종료
                  //  Debug.Log("끝났다");
                    AutoSmeltinginfo.text = Inventory.GetTranslate("UI3/제련종료");
                    StopCoroutine(A);
                    yield break;

                }

                if (Inventory.Instance.data.MaxStoneCount1 -
                    (Inventory.Instance.data.SmeltSuccCount1 + Inventory.Instance.data.SmeltFailCount1) <
                    SmeltCount - Inventory.Instance.data.SmeltSuccCount1)
                {
                    AutoSmeltinginfo.text = Inventory.GetTranslate("UI3/제련종료");
                    StopCoroutine(A);
yield break;
                }
            }

            if (PlayerBackendData.Instance.CheckItemCount(needitem) < needcount)
            {
                AutoSmeltinginfo.text = Inventory.GetTranslate("UI3/제련종료제련석");
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
        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/자동제련종료"), alertmanager.alertenum.일반);
        SmeltingingObj.Hide(true);
        StopCoroutine(A);
    }
}
