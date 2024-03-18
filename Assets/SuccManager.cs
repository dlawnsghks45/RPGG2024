using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class SuccManager : MonoBehaviour
{
    private static SuccManager _instance = null;

    public static SuccManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(SuccManager)) as SuccManager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    public Image itemimage;
    public Text itemtext;
    string NeedItem = "1000";
    int NeedHowmany = 500000000;
    public UIView SuccPanel;

    //���� ��� ���� �� ����.
    public GameObject SuccEquipBlockpanel;
    public bool IsSucc; //���� �������ΰ�.
    public EquipDatabase ResourceEquipKeyId; //������ ���̵�
    public EquipmentItemData ResourceSlot;
    public EquipDatabase ResultsEquipKeyId; //������ ���̵�
    public EquipmentItemData ResultSlot;
    public Eskillchanger[] EquipSkills;
    public EquipmentItemData LastSlot;

    public int lockcountEs = 0;
    public int maxcount = 2;
    public int resultcraft;
    public string resultrare;

    public UIToggle[] toggles;

    public UIView SuccNoLockPanel;
    public Text SuccNoLockText;

    private void RefreshItem()
    {
        itemimage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(NeedItem).sprite);
        itemtext.text =
            $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(NeedItem).name)} x {NeedHowmany:N0}\n{PlayerBackendData.Instance.GetMoney().ToString("N0")}";
        LayoutRebuilder.ForceRebuildLayoutImmediate(itemtext.GetComponentInParent<RectTransform>());
    }

    public void Tg_ReSetAll()
    {
        lockcountEs = 0;
        foreach (var VARIABLE in EquipSkills)
        {
            VARIABLE.UnLockEquipSkillSucc();
        }
    }

    private void Start()
    {
        NeedItem = "1000";
        NeedHowmany = 500000000;
        RefreshItem();
    }

    public void ReducelockcountEs()
    {
        lockcountEs--;
        if (lockcountEs < 0)
            lockcountEs = 0;
    }

    //�������
    public void Tg_SetGold()
    {
        NeedItem = "1000";
        NeedHowmany = 500000000;
        RefreshItem();
        LastSlot.data.CraftRare1 = resultcraft;
        LastSlot.data.Itemrare = resultrare;
        //  ResultsEquipKeyId.EquipSkill1 = resulteskill;
        //  ResultsEquipKeyId.IshaveEquipSkill = resultesbool;
        
        if (PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                .ToString())[ResultsEquipKeyId.KeyId1].SmeltSuccCount1 < ResourceSlot.data.SmeltSuccCount1)
        {
            //제련이 더 높은쪽으로
            LastSlot.data.MaxStoneCount1 = PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                .ToString())[ResultsEquipKeyId.KeyId1].SmeltSuccCount1;
            LastSlot.data.StoneCount1 = PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                .ToString())[ResultsEquipKeyId.KeyId1].StoneCount1;
            LastSlot.data.SmeltStatCount1 = PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                .ToString())[ResultsEquipKeyId.KeyId1].SmeltStatCount1;
            LastSlot.data.SmeltSuccCount1 = PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                .ToString())[ResultsEquipKeyId.KeyId1].SmeltSuccCount1;
            LastSlot.data.SmeltFailCount1 = PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                .ToString())[ResultsEquipKeyId.KeyId1].SmeltFailCount1;
        }
        
        
        //����ᰡ �� ������
        LastSlot.Refresh(LastSlot.data);
        maxcount = 2;
    }

    //���±�
    public void Tg_SetItem()
    {
        NeedItem = "1713";
        NeedHowmany = 1;
        itemimage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(NeedItem).sprite);
        itemtext.text =
            $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(NeedItem).name)} x {NeedHowmany}({PlayerBackendData.Instance.CheckItemCount("1713")})";
        LayoutRebuilder.ForceRebuildLayoutImmediate(itemtext.GetComponentInParent<RectTransform>());

        if (ResourceSlot.data.CraftRare1 > LastSlot.data.CraftRare1)
        {
            //����ᰡ �� ������
            LastSlot.data.CraftRare1 = ResourceSlot.data.CraftRare1;
        }

        if (int.Parse(ResourceSlot.data.Itemrare) > int.Parse(LastSlot.data.Itemrare))
        {
            //����ᰡ �� ������
            LastSlot.data.Itemrare = ResourceSlot.data.Itemrare;
        }

        if (PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                .ToString())[ResultsEquipKeyId.KeyId1].SmeltSuccCount1 < ResourceSlot.data.SmeltSuccCount1)
        {
            //제련이 더 높은쪽으로
            LastSlot.data.MaxStoneCount1 = ResourceSlot.data.MaxStoneCount1;
            LastSlot.data.StoneCount1 = ResourceSlot.data.StoneCount1;
            LastSlot.data.SmeltStatCount1 = ResourceSlot.data.SmeltStatCount1;
            LastSlot.data.SmeltSuccCount1 = ResourceSlot.data.SmeltSuccCount1;
            LastSlot.data.SmeltFailCount1 = ResourceSlot.data.SmeltFailCount1;
        }

        
        LastSlot.data.EquipSkill1 = ResourceSlot.data.EquipSkill1;
        LastSlot.data.IshaveEquipSkill = ResourceSlot.data.IshaveEquipSkill;

        LastSlot.data.EnchantNum1 = ResourceSlot.data.EnchantNum1;
        LastSlot.data.EnchantFail1 = ResourceSlot.data.EnchantFail1;
        
        
        LastSlot.Refresh(LastSlot.data);
        maxcount = 5;
    }

    public void CheckLock()
    {

        int onnum = 0;
        for (int i = 0; i < EquipSkills.Length; i++)
        {
            if (EquipSkills[i].gameObject.activeSelf)
                onnum++;
        }


        for (int i = 0; i < EquipSkills.Length; i++)
        {
            EquipSkills[i].LockEs.gameObject.SetActive(true);
        }

        if (lockcountEs.Equals(maxcount))
        {
            for (int i = 0; i < EquipSkills.Length; i++)
            {
                if (!EquipSkills[i].LockEs.IsOn)
                    EquipSkills[i].LockEs.Interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < EquipSkills.Length; i++)
            {
                EquipSkills[i].LockEs.Interactable = true;
            }
        }
    }

    public void Bt_StartSelectSucc()
    {
        Inventory.Instance.ItemObj.Hide(false);
        ResourceEquipKeyId = Inventory.Instance.data;
        SuccEquipBlockpanel.SetActive(true);
        IsSucc = true;
    }

    public void SelectSuccEquip(EquipDatabase data)
    {
        if (EquipItemDB.Instance.Find_id(data.Itemid).SpeMehod == "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/전승불가_특수없음"), alertmanager.alertenum.일반);
            return;
        }

        if (data.KeyId1.Equals(ResourceEquipKeyId.KeyId1))
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/전승불가_같은아이템"), alertmanager.alertenum.일반);
            return;
        }

        if (!data.CheckIsSucc(EquipItemDB.Instance.Find_id(ResourceEquipKeyId.Itemid).SpeMehod))
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/전승불가_다른타입"), alertmanager.alertenum.일반);
            return;
        }

        if (int.Parse(EquipItemDB.Instance.Find_id(data.Itemid).Tier) <
            int.Parse(EquipItemDB.Instance.Find_id(ResourceEquipKeyId.Itemid).Tier))
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI3/전승불가_티어가낮음"), alertmanager.alertenum.일반);
            return;
        }

        if (ResourceEquipKeyId == null)
            return;

        ResultsEquipKeyId = data;
        succbutton.Interactable = true;
        blockifdoing.SetActive(false);
        ResourceSlot.Refresh(ResourceEquipKeyId);
        ResultSlot.Refresh(ResultsEquipKeyId);


        EquipDatabase ep = new EquipDatabase(data);
        LastSlot.Refresh(ep);

        LastSlot.data.EquipSkill1 = ResourceEquipKeyId.EquipSkill1;
        LastSlot.data.IshaveEquipSkill = ResourceEquipKeyId.IshaveEquipSkill;
        resultcraft = ResultsEquipKeyId.CraftRare1;
        resultrare = ResultsEquipKeyId.Itemrare;
        foreach (var VARIABLE in EquipSkills)
        {
            VARIABLE.gameObject.SetActive(false);
        }

        int num = 0;
        if (EquipItemDB.Instance.Find_id(ResourceEquipKeyId.Itemid).SpeMehodP != "0")
        {
            num = 1;
        }

        for (int i = 0; i < EquipSkills.Length; i++)
        {
            EquipSkills[i].LockEs.IsOn = false;
            EquipSkills[i].UnLockEquipSkillSucc();
        }

        for (int i = 0 + num; i < ResourceEquipKeyId.EquipSkill1.Count; i++)
        {
            EquipSkills[i - num].ShowData(ResourceEquipKeyId.EquipSkill1[i]);
            EquipSkills[i - num].gameObject.SetActive(true);
        }

        SuccPanel.Show(false);

        if (toggles[0].IsOn)
        {
            Tg_SetGold();
        }
        else
        {
            Tg_SetItem();
        }
    }

    public void StopSucc()
    {
        IsSucc = false;
        SuccPanel.Hide(true);
        SuccEquipBlockpanel.SetActive(false);
        lockcountEs = 0;
        for (int i = 0; i < EquipSkills.Length; i++)
        {
            EquipSkills[i].LockEs.IsOn = false;
            EquipSkills[i].UnLockEquipSkillSucc();
            EquipSkills[i].LockEs.Interactable = true;


        }
    }

    public ParticleSystem[] effect;
    public ParticleSystem Finisheffect;

    public GameObject blockifdoing;
    public UIButton succbutton;


    public void Bt_TrySucc0()
    {
        SuccNoLockPanel.Show(false);
        SuccNoLockText.text = string.Format(Inventory.GetTranslate("UI4/잠금된전승특수효과없음"), lockcountEs);
    }


    public void Bt_TrySucc()
    {
        StartCoroutine(TrySucc());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public IEnumerator TrySucc()
    {
        int plusnum = 0;

        if (EquipItemDB.Instance.Find_id(ResourceEquipKeyId.Itemid).SpeMehodP == "0"
            && EquipItemDB.Instance.Find_id(ResultsEquipKeyId.Itemid).SpeMehodP != "0")
        {
            plusnum = 1;
        }

        switch (maxcount)
        {
            case 2:
                if (NeedItem.Equals("1000") && PlayerBackendData.Instance.GetMoney() >= NeedHowmany)
                {
                    foreach (var VARIABLE in effect)
                    {
                        VARIABLE.Play();
                    }
                    blockifdoing.SetActive(true);
                    Soundmanager.Instance.PlayerSound("Sound/강화확인전");
                    succbutton.Interactable = false;
                    //골드가 있다 소비
                    PlayerData.Instance.DownGold(NeedHowmany);
                    Debug.Log(plusnum);
                    LastSlot.data.EquipSkill1 = LastSlot.data.GetESkill(plusnum);
                    Inventory.Instance.RemoveItem(ResourceEquipKeyId.KeyId1);
                    PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                        .ToString())[ResultsEquipKeyId.KeyId1].SetEquipSkills(LastSlot.data.EquipSkill1.ToArray());
                    
                    PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                        .ToString())[ResultsEquipKeyId.KeyId1].EnchantNum1 = LastSlot.data.EnchantNum1;
                    PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                        .ToString())[ResultsEquipKeyId.KeyId1].EnchantFail1 = LastSlot.data.EnchantFail1;

                    
                    
                    
                    Savemanager.Instance.SaveMoneyCashDirect();
                    yield return SpriteManager.Instance.GetWaitforSecond(1);
                    Finisheffect.Play();
                    Savemanager.Instance.SaveTypeEquip();
                    Soundmanager.Instance.PlayerSound("Sound/Special Click 05");

                    yield return SpriteManager.Instance.GetWaitforSecond(1);
                    blockifdoing.SetActive(false);
                    Inventory.Instance.ShowEquipInventory(Inventory.Instance.nowsettype);
                    LogManager.LogSucc(ResourceEquipKeyId.KeyId1, ResultsEquipKeyId.KeyId1, "골드");
                    equipoptionchanger.Instance.Bt_SaveServerData();
                    IsSucc = false;

                }
                else
                {
                    //골드부족
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/골드부족"), alertmanager.alertenum.일반);
                }

                //골드전승
                break;
            case 5:
                if (NeedItem.Equals("1713") && PlayerBackendData.Instance.CheckItemCount("1713") >= NeedHowmany)
                {
                    foreach (var VARIABLE in effect)
                    {
                        VARIABLE.Play();
                    }

                    blockifdoing.SetActive(true);

                    Soundmanager.Instance.PlayerSound("Sound/강화확인전");
                    succbutton.Interactable = false;
                    //골드가 있다 소비
                    PlayerBackendData.Instance.RemoveItem(NeedItem, NeedHowmany);
                    LastSlot.data.EquipSkill1 = LastSlot.data.GetESkill(plusnum);
                    Inventory.Instance.RemoveItem(ResourceEquipKeyId.KeyId1);

                    int MaxStoneCount; //제련 카운트 최대 치 이며 해당 수치까지 제련이 가능
                    int StoneCount; //제련 현재 카운트
                    int[] SmeltStatCount; //제련을 할 때 해당 칸을 보여준다 0은 빈칸 1은 성공 2는 실패
                    int SmeltSuccCount; //실패횟수
                    int SmeltFailCount; //성공횟수

                    if (PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                            .ToString())[ResultsEquipKeyId.KeyId1].SmeltSuccCount1 < LastSlot.data.SmeltSuccCount1)
                    {
                        //제련이 더 높은쪽으로
                        PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                            .ToString())[ResultsEquipKeyId.KeyId1].MaxStoneCount1 = LastSlot.data.MaxStoneCount1;
                        
                        PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                            .ToString())[ResultsEquipKeyId.KeyId1].StoneCount1 = LastSlot.data.StoneCount1;
                        
                        PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                            .ToString())[ResultsEquipKeyId.KeyId1].SmeltStatCount1 = LastSlot.data.SmeltStatCount1;
                        
                        PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                            .ToString())[ResultsEquipKeyId.KeyId1].SmeltSuccCount1 = LastSlot.data.SmeltSuccCount1;
                        
                        PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                            .ToString())[ResultsEquipKeyId.KeyId1].SmeltFailCount1 = LastSlot.data.SmeltFailCount1;


                    }


                    PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                        .ToString())[ResultsEquipKeyId.KeyId1].SetEquipSkills(LastSlot.data.EquipSkill1.ToArray());

                    PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                        .ToString())[ResultsEquipKeyId.KeyId1].CraftRare1 = LastSlot.data.CraftRare1;

                    PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                        .ToString())[ResultsEquipKeyId.KeyId1].Itemrare = LastSlot.data.Itemrare;

                    PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                        .ToString())[ResultsEquipKeyId.KeyId1].EnchantNum1 = LastSlot.data.EnchantNum1;
                    PlayerBackendData.Instance.GetTypeEquipment(Inventory.Instance.nowsettype
                        .ToString())[ResultsEquipKeyId.KeyId1].EnchantFail1 = LastSlot.data.EnchantFail1;
                    
                    
                    Savemanager.Instance.SaveInventory();
                    Savemanager.Instance.SaveTypeEquip();
                    yield return SpriteManager.Instance.GetWaitforSecond(1);
                    Finisheffect.Play();
                    Soundmanager.Instance.PlayerSound("Sound/Special Click 05");
                    yield return SpriteManager.Instance.GetWaitforSecond(1);
                    blockifdoing.SetActive(false);
                    Inventory.Instance.ShowEquipInventory(Inventory.Instance.nowsettype);
                    LogManager.LogSucc(ResourceEquipKeyId.KeyId1, ResultsEquipKeyId.KeyId1, "전승석");
                    equipoptionchanger.Instance.Bt_SaveServerData();
                    Savemanager.Instance.Save();
                    IsSucc = false;
                }
                else
                {
                    //골드부족
                    alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/재료부족"), alertmanager.alertenum.일반);
                }

                //전승석전승
                break;
        }

    }

}