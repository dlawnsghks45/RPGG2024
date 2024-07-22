using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Utils.ColorModels;
using UnityEngine;
using UnityEngine.UI;

public class castingbarslot : MonoBehaviour
{
    [SerializeField] CanvasGroup canvas;
    public Player mainplayer;
    public Image Castingbar;
    public Image SkillImage;
    public float maxcount;
    public float curcount;
    public bool isCasting;
    Skillslot nowskill;



    public void SetCasting(float max, Skillslot skilldata)
    {
        nowskill = skilldata;
        maxcount = max;
        if (maxcount < 0)
            maxcount = 0;
        curcount = 0;
        SkillImage.sprite = skilldata.SkillImage.sprite;
        Castingbar.fillAmount = curcount / maxcount;
        isCasting = true;
        if (max <= 0)
        {
            canvas.alpha = 0;
        }
        else
        {
            canvas.alpha = 1;
        }
    }

    public void Start()
    {
        StartCoroutine(Casting());
    }

    public void FinishCasting()
    {
        isCasting = false;
        curcount = 0f;
        Castingbar.fillAmount = 0f;
    }

    //�����ӵ�
    WaitForSeconds castingsecond = new WaitForSeconds(0.1f);

    //������ ������
    WaitForSeconds castingsecond2 = new WaitForSeconds(0.15f);

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator Casting()
    {
        while (true)
        {
            yield return castingsecond;
            if (!isCasting || !EnemySpawnManager.Instance.isspawn) continue;
            if (!Battlemanager.Instance.isbattle) continue;

            if (maxcount != 0)
            {

                curcount += 0.1f;
                Castingbar.fillAmount = curcount / maxcount;

                if (!(curcount > maxcount)) continue;

                //�ణ ���� ���� �߻�
                yield return castingsecond2;
            }

            yield return new WaitUntil(() => Battlemanager.Instance.isbattle);

            isCasting = false;
            curcount = 0f;
            Castingbar.fillAmount = 0f;
            //��ų �߻�
            if (mainplayer.isme)
            {
                Skillmanager.Instance.UseSkill_Mainplayer(nowskill);
                if (nowskill.skilldata.SkillType == "MagicAttack")
                {
                    //��ĳ���
                    if (Passivemanager.Instance.GetPassiveStat(Passivemanager.PassiveStatEnum.makdouble) != 0)
                    {
                        if (Random.Range(0, 100) <
                            Skillmanager.Instance.GetRate(Passivemanager.Instance.GetPassiveStatPercent(Passivemanager.PassiveStatEnum.makdouble)))
                        {
                            yield return castingsecond2;
                            // Debug.Log("��ų�ߵ�");
                            Skillmanager.Instance.UseSkill_Mainplayer(nowskill);
                            DamageManager.Instance.ShowEffect(Battlemanager.Instance.mainplayer.transform, "Buff8");
                            if (dpsmanager.Instance.isdpson)
                                dpsmanager.Instance.AddDps(dpsmanager.attacktype.�нú�, 0, "C1028",1);
                        }
                    }
                }

                //���� �巡��
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.reskillhitper) != 0)
                {
                    if (Random.Range(0, 100) <
                        Skillmanager.Instance.GetRate( equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.reskillhitper)))
                    {
                        yield return castingsecond2;
                        // Debug.Log("��ų�ߵ�");
                        Skillmanager.Instance.UseSkill_Mainplayer(nowskill);
                        equipskillmanager.Instance.showequipslots("1211",
                            equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.reskilrare)
                                .ToString("N0"),
                            equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.reskilllv)
                                .ToString("N0"));
                        DamageManager.Instance.ShowEffect(Battlemanager.Instance.mainplayer.transform, "Buff7");
                        if (dpsmanager.Instance.isdpson)
                            dpsmanager.Instance.AddDps(dpsmanager.attacktype.Ư��ȿ��, 0, "E1211",1);
                    }
                }
                //�¾��� �����
                if (equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.reskillhitper2) != 0)
                {
                    if (Random.Range(0, 100) <
                        Skillmanager.Instance.GetRate(equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.reskillhitper2)))
                    {
                        yield return castingsecond2;
                        // Debug.Log("��ų�ߵ�");
                        Skillmanager.Instance.UseSkill_Mainplayer(nowskill);
                        equipskillmanager.Instance.showequipslots("1333",
                            equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.reskilrare2)
                                .ToString("N0"),
                            equipskillmanager.Instance.GetStats((int)equipskillmanager.EquipStatFloat.reskilllv2)
                                .ToString("N0"));
                        DamageManager.Instance.ShowEffect(Battlemanager.Instance.mainplayer.transform, "Buff7");
                        if (dpsmanager.Instance.isdpson)
                            dpsmanager.Instance.AddDps(dpsmanager.attacktype.Ư��ȿ��, 0, "E1333",1);
                    }
                }
            }

            canvas.alpha = 0;
        }
    }
}
