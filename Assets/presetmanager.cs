using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class presetmanager : MonoBehaviour
{
    private static presetmanager _instance = null;

    public static presetmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(presetmanager)) as presetmanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }
    
    public UIView Panel;

    public InputField presetname;

    //스킬
    public presetskillslot[] Preset_Skillslots;

    //장비\
    [SerializeField] private InventorySlot[] Preset_EQUIPS;

    //클래스
    public Image Preset_ClassImage;
    public Text Preset_ClassName;

    //펫
    public Image Preset_PetImage;
    public GameObject[] Preset_PetStar;
    public Text Preset_PetName;
    
    //패시브
    public Text[] Preset_ClassPassive;

    public UIToggle[] Toggles;

    public GameObject[] Preset_AbilityObj;
    public Image[] Preset_AbilityImage;


    private void Start()
    {
        if (!PlayerBackendData.Instance.isloadpreset)
        {
            for (int i = 0; i < PlayerBackendData.Instance.Presets.Length; i++)
            {
                PlayerBackendData.Instance.Presets[i] = new PresetItem();
            }
        }
    }

    public int nowselectpreset;

    public void Bt_OpenPresetPanel()
    {
        Debug.Log(PlayerBackendData.Instance.nowpresetnumber +"프리셋");
        nowselectpreset = PlayerBackendData.Instance.nowpresetnumber;
        Toggles[nowselectpreset].ExecuteClick();
        ShowPreset();
    }
    
    public void ShowPreset()
    {
        Panel.Show(false);
        PresetItem items = PlayerBackendData.Instance.Presets[nowselectpreset];
        presetname.text = items.presetname;

        Preset_PetImage.enabled = false;
        presetname.text = items.presetname;
        foreach (var VARIABLE in Preset_PetStar)
        {
            VARIABLE.SetActive(false);
        }
        
        
        //스킬
        for (int i = 0; i < Preset_Skillslots.Length; i++)
        {
            Preset_Skillslots[i].Skillimage.enabled = false;
        }

        //어빌리티
        for (int i = 0; i < Preset_AbilityObj.Length; i++)
        {
            Preset_AbilityObj[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < items.Skills.Length; i++)
        {
            if (items.Skills[i] != "True")
            {
                if (items.Skills[i] != null && items.Skills[i] != "")
                {
                    Preset_Skillslots[i].skillid = items.Skills[i];
                    Preset_Skillslots[i].Skillimage.enabled = true;
                    Preset_Skillslots[i].Skillimage.sprite =
                        SpriteManager.Instance.GetSprite(SkillDB.Instance.Find_Id(items.Skills[i]).Sprite);
                }
            }
        }

        //직업
        Preset_ClassName.text = "";
        Preset_ClassImage.enabled = false;
        if (items.Classid != "")
        {
            Preset_ClassName.text = Inventory.GetTranslate(ClassDB.Instance.Find_id(items.Classid).name);
            Inventory.Instance.ChangeItemRareColor(Preset_ClassName, ClassDB.Instance.Find_id(items.Classid).tier);
            Preset_ClassImage.enabled = true;
            Preset_ClassImage.sprite = SpriteManager.Instance.GetSprite(ClassDB.Instance.Find_id(items.Classid).classsprite);
        }
        

        //장비
        for (int i = 0; i < Preset_EQUIPS.Length; i++)
        {
            Preset_EQUIPS[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < items.EquipDatas.Length; i++)
        {
            if(items.EquipDatas[i] == "True")
                continue;
            if (items.EquipDatas[i] != null && items.EquipDatas[i] != "")
            {
                //장비를 못찾으면 없앤다.
            //    Debug.Log(PlayerBackendData.Instance.GetTypeEquipment(i.ToString()));
//                Debug.Log(items.EquipDatas[i]);
                if (!PlayerBackendData.Instance.GetTypeEquipment(i.ToString()).ContainsKey(items.EquipDatas[i]))
                {
                    Debug.Log("장비를 못찾았다");
                    items.EquipDatas[i] = null;
                    continue;
                }

                Preset_EQUIPS[i]
                    .RefreshNoName(PlayerBackendData.Instance.GetTypeEquipment(i.ToString())[items.EquipDatas[i]]);
                Preset_EQUIPS[i].gameObject.SetActive(true);
            }
        }

        //펫
        if (items.PetID != "")
        {
            Preset_PetName.text = Inventory.GetTranslate(PetDB.Instance.Find_id(items.PetID).name);
            Preset_PetName.color = Inventory.Instance.GetRareColor(PetDB.Instance.Find_id(items.PetID).rare);
            Preset_PetImage.sprite = SpriteManager.Instance.GetSprite(PetDB.Instance.Find_id(items.PetID).sprite);
            Preset_PetImage.enabled = true;

            for (int i = 0; i < PlayerBackendData.Instance.PetData[items.PetID].Petstar; i++)
            {
                Preset_PetStar[i].SetActive(true);
            }
        }
        else
        {
            Preset_PetName.text = "";
            Preset_PetImage.enabled = false;
        }
        
        
        //Debug.Log("패시브");
        //패시브
        for (int i = 0; i < Preset_ClassPassive.Length; i++)
        {
//            Debug.Log(items.Passive[i]);
            Preset_ClassPassive[i].text = "";
            if(items.Passive[i] == "True")
                continue;
            if(items.Passive[i] == "")
                continue;
            if (items.Passive[i] != null)
            {
                Preset_ClassPassive[i].text =
                    Inventory.GetTranslate(PassiveDB.Instance.Find_id(items.Passive[i]).name);
                Inventory.Instance.ChangeItemRareColor(Preset_ClassPassive[i],
                    ClassDB.Instance.Find_id(items.Passive[i]).tier);
            }
        }

        //어빌리티
        for (int i = 0; i < PlayerBackendData.Instance.Abilitys.Length; i++)
        {
            if(items.Ability[i] == "True")
                continue;
            if(items.Ability[i] == "")
                continue;
            if (items.Ability[i] != null)
            {
                Preset_AbilityImage[i].sprite =
                    SpriteManager.Instance.GetSprite(AbilityDBDB.Instance.Find_id(items.Ability[i]).sprite);
                Preset_AbilityObj[i].SetActive(true);

            }
        }
    }

    
    //프리셋 저장
    public void Bt_SavePreset()
    {
        if (presetname.text == "")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI5/프리셋이름설정"), alertmanager.alertenum.주의);
            return;
        }

        for (int i = 0; i < PlayerBackendData.Instance.Presets[nowselectpreset].Skills.Length; i++)
        {
            PlayerBackendData.Instance.Presets[nowselectpreset].Skills[i] = "";
        }
        //스킬
        for (int i = 0;
             i < PlayerBackendData.Instance.Presets[nowselectpreset].Skills.Length;
             i++)
        {
            
            PlayerBackendData.Instance.Presets[nowselectpreset].Skills[i] =
                PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[i];
        }
       
           

        //장비
        for (int i = 0; i < PlayerBackendData.Instance.EquipEquiptment0.Length; i++)
        {
            if (PlayerBackendData.Instance.EquipEquiptment0[i] != null)
            {
                PlayerBackendData.Instance.Presets[nowselectpreset].EquipDatas[i] =
                    PlayerBackendData.Instance.EquipEquiptment0[i].KeyId1;
            }
        }

        string[] passivedata = PlayerBackendData.Instance.PassiveClassId;
        //패시브
        for (int i = 0; i < passivedata.Length; i++)
        {
            Preset_ClassPassive[i].text = "";
            if(passivedata[i] == "True")
                continue;
            if(passivedata[i] == "")
                continue;
            if (passivedata[i] != null)
            {
                PlayerBackendData.Instance.Presets[nowselectpreset].Passive[i] = passivedata[i];
            }
        }

        //직업
        PlayerBackendData.Instance.Presets[nowselectpreset].Classid
            = PlayerBackendData.Instance.ClassId;

        PlayerBackendData.Instance.Presets[nowselectpreset].presetname = presetname.text;
        
        //펫
        if (PlayerBackendData.Instance.nowPetid != "")
        {
            PlayerBackendData.Instance.Presets[nowselectpreset].PetID
                = PlayerBackendData.Instance.nowPetid;
        }
        
        
        for (int i = 0; i < PlayerBackendData.Instance.Abilitys.Length; i++)
        {
            if (PlayerBackendData.Instance.Abilitys[i] != "")
            {
                PlayerBackendData.Instance.Presets[nowselectpreset].Ability[i] = PlayerBackendData.Instance.Abilitys[i];
            }
        }
        ShowPreset();
        SavePreset();
    }

    public void Bt_ChangePreset(int num)
    {
        nowselectpreset = num;
        ShowPreset();
    }



    public void SavePreset()
    {
        //최근 저장시간을 기준으로 잡느다.
        Param param = new Param
        {
            {
                "nowpresetnumber", PlayerBackendData.Instance.nowpresetnumber
            },
            {
                "PresetData", PlayerBackendData.Instance.Presets
            }
        };

        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) => { });

    }
    public void SavePreset2()
    {
        //최근 저장시간을 기준으로 잡느다.
        Param param = new Param
        {
            {
                "nowpresetnumber", PlayerBackendData.Instance.nowpresetnumber
            },
        };
        
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, param, (callback) =>
        {
            
        });
    }

    public void Bt_LoadPreset()
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI4/사냥터에서만변경가능"), alertmanager.alertenum.주의);
            return;
        }
        
        
        
        PresetItem preset = PlayerBackendData.Instance.Presets[nowselectpreset];
        PlayerBackendData.Instance.nowpresetnumber = nowselectpreset;
        if (preset.presetname == "")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI5/프리셋로드불가"), alertmanager.alertenum.주의);
            return;
        }
        
        //펫
        if (preset.PetID != "")
        {
            if (PlayerBackendData.Instance.nowPetid != "")
            {
                //장착해제
                PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Isequip = false;
                petmanager.Instance.listslot[PlayerBackendData.Instance.nowPetid].Init(PlayerBackendData.Instance.nowPetid);
            }
            PlayerBackendData.Instance.nowPetid = preset.PetID;
            PlayerData.Instance.SetpetImage(SpriteManager.Instance.GetSprite(PetDB.Instance.Find_id(preset.PetID).sprite),PetDB.Instance.Find_id(preset.PetID).sprite);
            PlayerData.Instance.SetPetRare(PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar);

            PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Isequip = true;
            petmanager.Instance.listslot[PlayerBackendData.Instance.nowPetid].Init(PlayerBackendData.Instance.nowPetid);
        }
       
        //직업
        //스킬
        Classmanager.Instance.Bt_SelectClass(preset.Classid,
            preset.Skills);
        //장비
        for (int i = 0; i < preset.EquipDatas.Length; i++)
        {
            if (preset.EquipDatas[i] != null && preset.EquipDatas[i] != "")
            {
                Inventory.Instance.EquipItem_Preset(preset.EquipDatas[i], i.ToString());
            }
        }
        
        for (int i = 0; i < PlayerBackendData.Instance.Abilitys.Length; i++)
        {
            if (preset.Ability[i] != null && preset.Ability[i] != "")
            {
                PlayerBackendData.Instance.Abilitys[i] = preset.Ability[i];
            }
        }

        //어빌리티
        abilitymanager.Instance.Refresh();
        EquipSetmanager.Instance.EquipSetItem();
        //패시브
        Classmanager.Instance.Bt_SelectPassive_Preset(preset.Passive);
        Passivemanager.Instance.Refresh();
        Classmanager.Instance.ShowClassData_Preset();
        Classmanager.Instance. RefreshJobSlot();
        //저장
        Skillmanager.Instance.mainplayer.SetClass_start();
        PlayerData.Instance.RefreshClassName();
        PlayerData.Instance.RefreshPlayerstat();
        //데이터 저장
        Savemanager.Instance.SaveClassData();
        Savemanager.Instance.SaveEquip();
        Savemanager.Instance.Save();
        alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI5/프리셋불러옴"), alertmanager.alertenum.일반);
        SavePreset2();
        Panel.Hide(true);
    }
}
public class PresetItem
{
    public string presetname = "";
    public string[] Skills = new string[12];
    public string[] EquipDatas = new string[14];
    public string[] Passive = new string[10];
    public string PetID = "";
    public string[] Ability = new string[10];
    public string Classid = "";

    public PresetItem()
    {

    }
}