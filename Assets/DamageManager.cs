using System;
using DamageNumbersPro;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    //�̱��游���.
    private static DamageManager _instance = null;

    public static DamageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(DamageManager)) as DamageManager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }


    //������ ����    
    public DamageNumber DamagePrefab;
    public DamageNumber CriticalPrefab;
    public DamageNumber CombineDamagePrefab;
    public DamageNumber CombineCritDamagePrefab;


    //��� ����
    public DamageNumber GoldPrefab;
    public DamageNumber CombineGoldPrefab;

    //ȸ�� ����
    public DamageNumber HealPrefab;
    public DamageNumber CombineHealPrefab;

    public DamageNumber MpPrefab;

    public DamageNumber[] DotPrefab;
    public DamageNumber[] DotPrefab_Crit;



    public enum damagetype
    {
        �Ϲ�����,
        ũ������,
        ��ü�Ϲ�����,
        ��üũ������,
        �Ϲݰ��,
        ��ü���,
        ȸ��,
        ��üȸ��,
        ����ȸ��,
        ����,
        ȭ��,
        ����,
        �ߵ�,
        ����,
        ����ũ��,
        ȭ��ũ��,
        ����ũ��,
        �ߵ�ũ��,
        ����ũ��,
        Length
    }


    public void ShowDamageText(damagetype texttype, Transform trans, decimal dmg, float ysizeup = 0)
    {
        if (!SettingReNewal.Instance.Dmg_Show[0].IsOn) return;

        
        string dmgg = dpsmanager.convertNumber(dmg);
        switch (texttype)
        {
            case damagetype.�Ϲ�����:
                var position18 = trans.position;
                DamageNumber damagebasic =
                    DamagePrefab.Spawn(new Vector3(position18.x, position18.y + ysizeup, position18.z),
                        dmgg);
                break;
            case damagetype.ũ������:
                var position17 = trans.position;
                DamageNumber damagecrit =
                    CriticalPrefab.Spawn(new Vector3(position17.x, position17.y + ysizeup, position17.z),
                        dmgg);
                break;
            case damagetype.��ü�Ϲ�����:
                var position16 = trans.position;
                DamageNumber damagecombine =
                    CombineDamagePrefab.Spawn(
                        new Vector3(position16.x, position16.y + ysizeup, position16.z), dmgg);
                break;
            case damagetype.��üũ������:
                var position15 = trans.position;
                DamageNumber Critdamagecombine =
                    CombineCritDamagePrefab.Spawn(
                        new Vector3(position15.x, position15.y + ysizeup, position15.z), dmgg);
                break;
            case damagetype.�Ϲݰ��:
                var position14 = trans.position;
                DamageNumber gold =
                    GoldPrefab.Spawn(new Vector3(position14.x, position14.y + ysizeup, position14.z), dmgg);
                break;
            case damagetype.��ü���:
                var position13 = trans.position;
                DamageNumber goldcombine =
                    CombineGoldPrefab.Spawn(new Vector3(position13.x, position13.y + ysizeup, position13.z),
                        dmgg);
                break;
            case damagetype.ȸ��:
                var position12 = trans.position;
                DamageNumber heal =
                    HealPrefab.Spawn(new Vector3(position12.x, position12.y + ysizeup, position12.z), dmgg);
                break;
            case damagetype.��üȸ��:
                var position11 = trans.position;
                DamageNumber healcombine =
                    CombineHealPrefab.Spawn(new Vector3(position11.x, position11.y + ysizeup, position11.z),
                        dmgg);
                break;
            case damagetype.����ȸ��:
                var position10 = trans.position;
                DamageNumber Mpcombine =
                    MpPrefab.Spawn(new Vector3(position10.x, position10.y + ysizeup, position10.z), dmgg);
                break;

            case damagetype.����:
                var position9 = trans.position;
                DamageNumber Bleed = DotPrefab[0]
                    .Spawn(new Vector3(position9.x, position9.y + ysizeup, position9.z), dmgg);
                break;

            case damagetype.ȭ��:
                var position8 = trans.position;
                DamageNumber Fire = DotPrefab[1]
                    .Spawn(new Vector3(position8.x, position8.y + ysizeup, position8.z), dmgg);
                break;

            case damagetype.����:
                var position7 = trans.position;
                DamageNumber Shock = DotPrefab[2]
                    .Spawn(new Vector3(position7.x, position7.y + ysizeup, position7.z), dmgg);
                break;

            case damagetype.�ߵ�:
                var position6 = trans.position;
                DamageNumber Poison = DotPrefab[3]
                    .Spawn(new Vector3(position6.x, position6.y + ysizeup, position6.z), dmgg);
                break;

            case damagetype.����:
                var position5 = trans.position;
                DamageNumber Death = DotPrefab[4]
                    .Spawn(new Vector3(position5.x, position5.y + ysizeup, position5.z), dmgg);
                break;
            case damagetype.����ũ��:
                var position4 = trans.position;
                DamageNumber BleedCrit = DotPrefab_Crit[0]
                    .Spawn(new Vector3(position4.x, position4.y + ysizeup, position4.z), dmgg);
                break;

            case damagetype.ȭ��ũ��:
                var position3 = trans.position;
                DamageNumber FireCrit = DotPrefab_Crit[1]
                    .Spawn(new Vector3(position3.x, position3.y + ysizeup, position3.z), dmgg);
                break;

            case damagetype.����ũ��:
                var position2 = trans.position;
                DamageNumber ShockCrit = DotPrefab_Crit[2]
                    .Spawn(new Vector3(position2.x, position2.y + ysizeup, position2.z), dmgg);
                break;

            case damagetype.�ߵ�ũ��:
                var position = trans.position;
                DamageNumber PoisonCrit = DotPrefab_Crit[3]
                    .Spawn(new Vector3(position.x, position.y + ysizeup, position.z), dmgg);
                break;

            case damagetype.����ũ��:
                var position1 = trans.position;
                DamageNumber DeathCrit = DotPrefab_Crit[4]
                    .Spawn(new Vector3(position1.x, position1.y + ysizeup, position1.z), dmgg);
                break;
            case damagetype.Length:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(texttype), texttype, null);
        }

    }

    public EffectSlots[] Effect;
    public EffectSlots LevelUpEffect;
    private int indexeffect = 0;
    private static readonly int Lvup = Animator.StringToHash("lvup");

    // ReSharper disable Unity.PerformanceAnalysis
    public void ShowEffect(Transform trans, string EffectName)
    {
        if (EffectName == "" || !SettingReNewal.Instance.Effect_Show[0].IsOn) return;

        if (indexeffect == Effect.Length)
        {
            indexeffect = 0;
            foreach (var t in Effect)
            {
                t.isactive = false;
            }
        }

        foreach (var t in Effect)
        {
            if (t.isactive) continue;
            // Debug.Log("I�� " + i);
            t.transform.position = trans.position;
            t.isactive = true;
            t.ani.SetTrigger(EffectName);
            indexeffect++;
            break;
        }
    }

    public void ShowEffect_LvUp(Transform trans)
    {
        LevelUpEffect.transform.position = trans.position;
        LevelUpEffect.ani.SetTrigger(Lvup);
    }

    public void CreateNumber(float number, Vector3 position)
    {

    }
}
