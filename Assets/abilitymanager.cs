using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class abilitymanager : MonoBehaviour
{
    
    private static abilitymanager _instance = null;

    public static abilitymanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(abilitymanager)) as abilitymanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    
    public void ShowAbility()
    {

    }

    [SerializeField] private abilityslot[] abilityslots;

    public void Start()
    {
        for (int i = 0; i < abilityslots.Length; i++)
        {
            AbilityDBDB.Row data = AbilityDBDB.Instance.GetAt(i);
            abilityslots[i].init(i, data);
        }
        abilitymanager.Instance.Bt_RefreshReco();
    }

    public UIView AbilityInfoPanel;
    public Image AbilityImage;
    public Text AbilityName;
    public Text AbilityInfo;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
    public UIButton SelectButton;
    public AbilityDBDB.Row nowdata;
    public void ShowAbility(AbilityDBDB.Row data)
    {
        nowdata = data;
        AbilityInfoPanel.Show(false);
        AbilityImage.sprite = SpriteManager.Instance.GetSprite(data.sprite);
        AbilityName.text = Inventory.GetTranslate(data.name);
        AbilityInfo.text = Inventory.GetTranslate(data.info);
        SelectButton.gameObject.SetActive(true);
        if (int.Parse(nowdata.Maxlv) <= PlayerBackendData.Instance.GetLv())
        {
            SelectButton.Interactable = true;
        }
        else
        {
            SelectButton.Interactable = false;
        }
    }

    
    public void ShowAbilityOther(string id)
    {
        AbilityDBDB.Row data = AbilityDBDB.Instance.Find_id(id);
        AbilityInfoPanel.Show(false);
        AbilityImage.sprite = SpriteManager.Instance.GetSprite(data.sprite);
        AbilityName.text = Inventory.GetTranslate(data.name);
        AbilityInfo.text = Inventory.GetTranslate(data.info);
        SelectButton.gameObject.SetActive(false);
    }

    public void Bt_RefreshReco()
    {
        string type = EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.EquipEquiptment0[0].Itemid).SubType;
        for (var index = 0; index < abilityslots.Length; index++)
        {
//            Debug.Log("타입"+type);
            var t = abilityslots[index];
            t.RefreshReco(type);
        }
    }
    
    public void Refresh()
    {
        foreach (var t in abilityslots)
        {
            t.Refresh();
        }
        
    }

    public void Bt_select()
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        
        if (PartyRaidRoommanager.Instance.partyroomdata.isstart)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/콘텐츠 중 불가능"), alertmanager.alertenum.주의);
            return;
        }
        
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/사냥터만가능"), alertmanager.alertenum.주의);
            return;
        }

        
        
        if (int.Parse(nowdata.Maxlv) <= PlayerBackendData.Instance.GetLv())
        {
            PlayerBackendData.Instance.Abilitys[int.Parse(nowdata.AT) - 1] = nowdata.id;
            AbilityInfoPanel.Hide(false);  
            PlayerData.Instance.RefreshPlayerstat();
            Refresh();
            Savemanager.Instance.SaveAbility();
            Savemanager.Instance.Save();
            alertmanager.Instance.NotiCheck_Ability();
            TutorialTotalManager.Instance.CheckFinish();
        }
    }
}
