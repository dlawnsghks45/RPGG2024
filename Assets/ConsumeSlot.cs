using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumeSlot : MonoBehaviour
{
    public string HpItemId;
    public GameObject Autoobj;
    public int itemcount;
    public Image ItemImage;
    public Image CooldownImage;
    public Text ItemCountText;

    public GameObject Havepotion;
    public GameObject Nonpotion;
    float Hpfloat;
    private float Mpfloat;
    [SerializeField]
    float HpfloatP;
    [SerializeField]
    float MpfloatP;
    public const float UseTime = 3f;
    bool canuse = true;
    ItemdatabasecsvDB.Row item;
    bool ispercent;
    public void SetPotion(string itmeid)
    {
        int index = PlayerBackendData.Instance.GetItemIndex(itmeid);
        if (index == -1)
            return;


        HpItemId = itmeid;
        item = ItemdatabasecsvDB.Instance.Find_id(HpItemId);
        ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(HpItemId).sprite);
        Refresh();
        Nonpotion.SetActive(false);
        Havepotion.SetActive(true);
        Hpfloat = float.Parse(ItemdatabasecsvDB.Instance.Find_id(HpItemId).A);
        Mpfloat = float.Parse(ItemdatabasecsvDB.Instance.Find_id(HpItemId).B);
        HpfloatP = float.Parse(ItemdatabasecsvDB.Instance.Find_id(HpItemId).C);
        MpfloatP = float.Parse(ItemdatabasecsvDB.Instance.Find_id(HpItemId).D);

        if (Hpfloat.Equals(0) && Mpfloat.Equals(0))
        {
            ispercent = true;
        }
        else
        {
            ispercent = false;
        }
    }

    public void Refresh()
    {
        int index = PlayerBackendData.Instance.GetItemIndex(HpItemId);

        if (index == -1)
        {
            item = null;
            HpItemId = "";
            return;
        }
        ItemCountText.text = PlayerBackendData.Instance.ItemInventory[index].Howmany < 1000 ? PlayerBackendData.Instance.ItemInventory[index].Howmany.ToString() : "999+";
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Bt_UseItem()
    {
        if (!canuse)
            return;

        if(HpItemId == "")
            Inventory.Instance.RefreshInventory(1);
        
        int index = PlayerBackendData.Instance.GetItemIndex(HpItemId);

        if (index == -1)
        {
            RemoveItem();
            return;
        }
        else
        {
            PlayerBackendData.Instance.RemoveItem(HpItemId, 1);
            if (ispercent)
            {
                Battlemanager.Instance.mainplayer.hpmanager.HealHp(setpotionupPercentHP(HpfloatP), setpotionupPercentMP(MpfloatP));
            }
            else
            {
                Battlemanager.Instance.mainplayer.hpmanager.HealHp(setpotionup(Hpfloat), setpotionup(Mpfloat));
            }
            Refresh();
        }
        canuse = false;
        CooldownImage.fillAmount = 1;
        DOTween.To(() => CooldownImage.fillAmount, x => CooldownImage.fillAmount = x, 0, UseTime);
        Invoke(nameof(enduse), UseTime);
        Savemanager.Instance.SaveInventory_SaveOn();
        
    }

    float setpotionup(float value)
    {
        return value + (value * Battlemanager.Instance.mainplayer.stat_potionup);
    }

    float setpotionupPercentHP(float value)
    {
        float h = 0f;
        h = Battlemanager.Instance.mainplayer.stat_hp * value;
        return h;
    }
    float setpotionupPercentMP(float value)
    {
        float h = 0f;
        h = Battlemanager.Instance.mainplayer.stat_mp * value;
        return h;
    }
    void enduse()
    {
        canuse = true;
    }


    private void RemoveItem()
    {
        HpItemId = "";
        Nonpotion.SetActive(true);
        Havepotion.SetActive(false);
    }

    private void Start()
    {
        if(Skillmanager.Instance.AutoSkillToggle.isOn)
            AutoStart();
        else
        {
            FalseAutoStart();
        }
    }

    bool isauto = false;
    public void AutoStart()
    {
        isauto = true;
        Autoobj.SetActive(true);
        StartCoroutine(Auto());
    }
    public void FalseAutoStart()
    {
        Autoobj.SetActive(false);
        isauto = false;
    }
    WaitForSeconds wait = new WaitForSeconds(0.5f);

    private IEnumerator Auto()
    {
        while (true)
        {
            yield return wait;
            if (!Skillmanager.Instance.AutoSkillToggle.isOn) continue;
            
            if (item != null)
            {
                switch (item.itemsubtype)
                {
                    case "101": //체력 아이템
                        if (Battlemanager.Instance.mainplayer.hpmanager.CurHp /
                            Battlemanager.Instance.mainplayer.hpmanager.MaxHp <
                            (decimal)SettingReNewal.Instance.HpSlider.value)
                        {
                            Bt_UseItem();
                        }

                        break;
                    case "102": //정신력 아이템
                        if (Battlemanager.Instance.mainplayer.hpmanager.CurMp /
                            Battlemanager.Instance.mainplayer.hpmanager.MaxMp <
                            SettingReNewal.Instance.MpSlider.value)
                        {
                            Bt_UseItem();
                        }

                        break;
                }
            }
        }
    }
}
