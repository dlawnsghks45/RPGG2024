using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Doozy.Engine.Soundy;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class SettingReNewal : MonoBehaviour
{
    private static SettingReNewal _instance = null;

    public static SettingReNewal Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(SettingReNewal)) as SettingReNewal;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    
    //사운드
    public Slider BackgroundSlider;
    public Text Backgroundtext;
    
    public Slider SkillSoundSlider;
    public Text SkillSoundtext;
    public UIToggle[] ButtonSound;

    
    
    public AudioSource BackVolume;
    public AudioSource EffectVolume;
    
    //그래픽 설정
    public UIToggle[] Dmg_Count; //피해량 표기
    public UIToggle[] Dmg_Show; //온 오프 

    //피해량
    public UIToggle[] Effect_Color;
    public UIToggle[] Effect_Show; //On Off

    //그래픽 설정
    public UIToggle[] ItemPanel;
    public UIToggle[] ItemDrop;
    public UIToggle[] EskillPanel;
    public UIToggle[] SystemChat;
    public UIToggle[] CameraShake;

    //물약
    public Slider HpSlider;
    public Text Hptext;
    
    public Slider MpSlider;
    public Text Mptext;
    
    //물약
    private void Start()
    {
      LoadSettingData();  
    }
   
    public void LoadSettingData()
    {
        SettingData data = PlayerBackendData.Instance.settingdata;
        //사운드
        BackgroundSlider.value = data.Backsound;
        Backgroundtext.text = $"{((BackgroundSlider.value / 1f) * 100f):N0}%" ;
        SkillSoundSlider.value = data.Effectsound;
        SkillSoundtext.text = $"{((SkillSoundSlider.value / 1f) * 100f):N0}%" ;

        ButtonSound[data.ButtonSound].IsOn = true;
        ButtonSound[data.ButtonSound].ExecuteClick();

        
        BackVolume.volume = data.Backsound;
        EffectVolume.volume = data.Effectsound;
        
//        Debug.Log("이펙트 컬러" + data.EffectColor);
        //피해량
        Dmg_Count[data.DmgCountNum].IsOn = true;
        Dmg_Show[data.DmgShowNum].IsOn = true;

        Effect_Color[data.EffectColor].IsOn = true;
        Effect_Show[data.EffectShow].IsOn = true;
        
        //물약
        HpSlider.value = data.Hp;
        Hptext.text = $"{((HpSlider.value / 1f) * 100f):N0}%" ;
        MpSlider.value = data.Mp;
        Mptext.text = $"{((MpSlider.value / 1f) * 100f):N0}%" ;
        
        ItemPanel[data.ItemPanel].IsOn = true;
        ItemDrop[data.ItemDrop].IsOn = true;
        EskillPanel[data.EskillPanel].IsOn = true;
        SystemChat[data.SystemChat].IsOn = true;
        CameraShake[data.CameraShake].IsOn = true;
        
        SetEffectColor();
    }
    
    public void ChangeBackVolume(float value)
    {
        BackVolume.volume = value;
        Backgroundtext.text = $"{(value * 100f):N0}%";
    }

    public void ChangeEffectVolume(float value)
    {
        EffectVolume.volume = value;
        SkillSoundtext.text = $"{(value * 100f):N0}%";
    }
    
  
    public void ChangeHPauto(float value)
    {
        Hptext.text = $"{(value * 100f):N0}%";
    }

    public void ChangeMPauto(float value)
    {
        Mptext.text = $"{(value * 100f):N0}%";
    }

    void SetButtonSound()
    {
        if (ButtonSound[0].IsOn)
        {
            SoundyManager.UnmuteAllSounds();
        }
        else
        {
            SoundyManager.MuteAllSounds();
        }
    }
    void SetEffectColor()
    {
        float a = 1f;
//        Debug.Log("이펙트 컬러는" + PlayerBackendData.Instance.settingdata.EffectColor);
        switch (PlayerBackendData.Instance.settingdata.EffectColor)
        {
            case 0:
                a = 0.25f;
                break;
            case 1:
                a = 0.5f;
                break;
            case 2:
                a = 0.75f;
                break;
            case 3:
                a = 1f;
                break;
        }
        
        foreach (var t in DamageManager.Instance.Effect)
        {
            t.SetEffectColor(a);
        }

        SetButtonSound();
    }


    public void Bt_SaveSetting()
    {
        int numeffectcolor = 0 ;
        for (int i = 0; i < Effect_Color.Length; i++)
        {
            if (Effect_Color[i].IsOn)
                numeffectcolor = i;
        }
        int numDmg_Count = 0 ;
        for (int i = 0; i < Dmg_Count.Length; i++)
        {
            if (Dmg_Count[i].IsOn)
                numDmg_Count = i;
        }
        
        //이펙트 색
//        Debug.Log("이펙트 컬러" +  numeffectcolor);
        PlayerBackendData.Instance.settingdata.SetData
            (
                BackgroundSlider.value,
                SkillSoundSlider.value,
                numDmg_Count,
                Dmg_Show[0].IsOn ? 0 : 1,
                numeffectcolor ,
                Effect_Show[0].IsOn ? 0 : 1,
                HpSlider.value,
                MpSlider.value,
                ItemPanel[0].IsOn ? 0 : 1,
                ItemDrop[0].IsOn ? 0 : 1,
                EskillPanel[0].IsOn ? 0 : 1,
                SystemChat[0].IsOn ? 0 : 1,
                ButtonSound[0].IsOn ? 0 : 1,
                CameraShake[0].IsOn ? 0 : 1
            );

//        Debug.Log("버튼 저장" +   PlayerBackendData.Instance.settingdata.ButtonSound);
        SetEffectColor();

        Param paramEquip = new Param { { "SettingData", PlayerBackendData.Instance.settingdata } };

        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramEquip, (callback) =>
        {
            // 이후 처리
            if (!callback.IsSuccess())
            {
            }

        });
        
    }
}
