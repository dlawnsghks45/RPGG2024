using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using EasyMobile.Demo;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EquipUpgradeManager : MonoBehaviour
{
    //�̱��游���.
    private static EquipUpgradeManager _instance = null;

    public static EquipUpgradeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(EquipUpgradeManager)) as EquipUpgradeManager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    public UIToggle AutoUpgrade;
    public UIView UpgradeView;
    public Image EquipImage; //��� �̹���
    public Text EquipName; //��� �̸�
    public Text EquipUpgrade; //��ȭ ��ġ
    public Text EquipUpgradePercent; //��ȭ Ȯ��

    public itemshowcountslot NeedCountText;

    
    public UIToggle UpgradeToggle_item;
    public UIToggle UpgradeToggle_crystal;
    
    public itemshowcountslot UpgradeToggle_itemCount;
    public itemshowcountslot UpgradeToggle_crystalCount;

    private EquipDatabase data;

    private int enchantcount;
    int plusenchant;

    public GameObject itemobj;
    public GameObject crystalobj;



    public ParticleSystem StartEffect;
    public ParticleSystem SuccEffect1;
    public ParticleSystem SuccEffect2;
    public ParticleSystem FailEffect2;
    
    
    public void ShowEqupData()
    {
        data = Inventory.Instance.data;
        UpgradeView.Show(false);
        
        EquipImage.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(data.Itemid).Sprite);
        EquipName.text = data.GetItemName();
        EquipName.color =  Inventory.Instance.GetRareColor(data.Itemrare);

       enchantcount = int.Parse(EquipUpgradeDB.Instance.Find_num(data.EnchantNum1.ToString()).percent);
       plusenchant = 0;
       if (!bool.Parse(EquipUpgradeDB.Instance.Find_num(data.EnchantNum1.ToString()).itemuse))
       {
           UpgradeToggle_item.IsOn = false;
           itemobj.gameObject.SetActive(false);
       }
       else
       {
           
           if (UpgradeToggle_item.IsOn)
           {
               plusenchant += 2;
           }
           itemobj.gameObject.SetActive(true);
       }
       
       if (!bool.Parse(EquipUpgradeDB.Instance.Find_num(data.EnchantNum1.ToString()).crystaluse))
       {
           UpgradeToggle_crystal.IsOn = false;
           crystalobj.gameObject.SetActive(false);
       }
       else
       {
           if (UpgradeToggle_crystal.IsOn)
           {
               plusenchant += 4;
           }
           crystalobj.gameObject.SetActive(true);
       }
       
       
     

        
        if (UpgradeToggle_item.IsOn || UpgradeToggle_crystal.IsOn)
        {
            EquipUpgradePercent.text = $"{enchantcount+plusenchant+PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[Inventory.Instance.data.KeyId1].EnchantFail1}% (+{plusenchant}%)";
        }
        else
        {
            EquipUpgradePercent.text = $"{enchantcount+PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[Inventory.Instance.data.KeyId1].EnchantFail1}%";
        }
        
        EquipUpgrade.text =$"+{data.EnchantNum1.ToString()}";

        //��ȭ����
         needid = EquipItemDB.Instance.Find_id(data.Itemid).UpgradeNeedID;
         needcount = int.Parse(EquipUpgradeDB.Instance.Find_num(data.EnchantNum1.ToString()).needcount);
        NeedCountText.SetData(needid,needcount,true);
        
         needcount_item = int.Parse(EquipUpgradeDB.Instance.Find_num(data.EnchantNum1.ToString()).needupgradecount);
         needcount_crystal = int.Parse(EquipUpgradeDB.Instance.Find_num(data.EnchantNum1.ToString()).crystal);
        UpgradeToggle_itemCount.SetData("50005",needcount_item,true);
        UpgradeToggle_crystalCount.SetData("1001",needcount_crystal,true);
    }

    private string needid;
    private int needcount;
    private int needcount_item;
    private int needcount_crystal;


    private bool isstart = false;
    public void Bt_UpgradEquip()
    {
        
        if(isstart)
            return;
        
        
        if (PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[
                Inventory.Instance.data.KeyId1].EnchantNum1 == 100)
        {
           // Debug.Log("�ִ� ��ȭ �Դϴ�.");
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/�ִ밭ȭ"),alertmanager.alertenum.����);
            return;
        }
           
        if (PlayerBackendData.Instance.CheckItemCount(needid) < needcount)
        {
           // Debug.Log("��ȭ���� �����մϴ�.");
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/��ȭ���̺���"),alertmanager.alertenum.����);

            return;
        }
        
        //������ �ִ°�
        if (UpgradeToggle_item.IsOn)
        {
            if (PlayerBackendData.Instance.CheckItemCount("50005") < needcount_item)
            {
               // Debug.Log("�������� �����մϴ�.");
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI6/����������"),alertmanager.alertenum.����);

                return;
            }
        }

        //������ �ִ°�
        if (UpgradeToggle_crystal.IsOn)
        {
            if (PlayerBackendData.Instance.GetCash() < needcount_crystal)
            {
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/�Ҳɺ���"),alertmanager.alertenum.����);
               // Debug.Log("ũ����Ż�� �����մϴ�.");
                return;
            }
        }

        Soundmanager.Instance.PlayerSound2("Sound/��ȭȮ����",1);
        isstart = true;
        StartCoroutine(StartEnchnat());

    }

    public List<string> logdata = new List<string>();
    public IEnumerator StartEnchnat()
    {
        StartEffect.Play();        
        
        Random.InitState((int)Time.deltaTime + PlayerBackendData.Instance.GetRandomSeed());
        int ran = Random.Range(0, 100);// 1,000,000�� 100%�̴�.

        bool issucc;

        
        //������ �ִ°�
        if (UpgradeToggle_item.IsOn)
        {
            PlayerBackendData.Instance.RemoveItem("50005",needcount_item);
        }

        //������ �ִ°�
        if (UpgradeToggle_crystal.IsOn)
        {
            PlayerData.Instance.DownCash(needcount_crystal);
        }
        
        PlayerBackendData.Instance.RemoveItem(needid,needcount);

        
        
        if (enchantcount + plusenchant +enchantcount+PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[Inventory.Instance.data.KeyId1].EnchantFail1 > ran)
        {
            issucc = true;
            
            PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[Inventory.Instance.data.KeyId1].EnchantNum1++;
            PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[
                Inventory.Instance.data.KeyId1].EnchantFail1 = 0;
            Inventory.Instance.ShowInventoryItem(PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[Inventory.Instance.data.KeyId1],true);
            
            if (PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[Inventory.Instance.data.KeyId1].EnchantNum1 > 70)
                chatmanager.Instance.ChattoUpgradeUp(PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[Inventory.Instance.data.KeyId1].EnchantNum1, Inventory.Instance.data.Itemid, Inventory.Instance.data.Itemrare);
            logdata.Add($"{PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[Inventory.Instance.data.KeyId1].EnchantNum1}����");
        }
        else
        {
            issucc = false;
            PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[
                Inventory.Instance.data.KeyId1].EnchantFail1++;
            logdata.Add($"{PlayerBackendData.Instance.GetTypeEquipment(EquipItemDB.Instance.Find_id(data.Itemid).Type)[Inventory.Instance.data.KeyId1].EnchantNum1}  ����");
        }

        if (logdata.Count >= 15)
        {
            LogManager.UpgradeLog(Inventory.Instance.data,logdata.ToArray());
            logdata.Clear();
        }
        Savemanager.Instance.SaveEquip();
        Savemanager.Instance.SaveInventory();
        Savemanager.Instance.SaveCash();
        Savemanager.Instance.Save();


        yield return new WaitForSeconds(0.7f);
        
        if (issucc)
        {
            //����
            SuccEffect1.Play();
            SuccEffect2.Play();
            Soundmanager.Instance.PlayerSound2("Sound/Special Click 05",1);
        }
        else
        {
            //����
            FailEffect2.Play();
            Soundmanager.Instance.PlayerSound2("Sound/��ȭ����",1);
        }

        isstart = false;
        ShowEqupData();

        if (AutoUpgrade.IsOn)
        {
            yield return new WaitForSeconds(0.7f);
            Bt_UpgradEquip();
        }
        
    }

    public void Exit_Upgrade()
    {
        if (logdata.Count != 0)
        {
            LogManager.UpgradeLog(Inventory.Instance.data,logdata.ToArray());
            logdata.Clear();
        }
    }
}
