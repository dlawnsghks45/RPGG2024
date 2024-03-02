using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Skillslot : MonoBehaviour
{
    public bool isme;
    public int index;
    [SerializeField]
    Castingmanager castingmanager;
    public string skillid;
    //��ų�� FM
    /*
     * 1.��ų�� ��Ƽ��� �нú갡 �ִ�. 
     * 2.��ų ��� �� �ٸ� ��ų ����� �Ұ��� �ϴ�.
     * -.��ų���� �����ð��� �ִ�. ���� �� �ٸ� ��ų ��� �Ұ�.
     * 3.��ų�� �������� ĭ�� �ٸ���. �ִ� 12���� ��ų�� ��� �����ϴ�.
     * 4.��ų ��� �� ������ �Һ� �ȴ�. �нú�� X
     * 5.
     * */
    public Image SkillImage;
    public Image SkillCooldown;
    public Image Skillrare;
    public Text SkillCooldownTime;

    //��ų �ִ�.
    public GameObject HaveSkillobj;
    public GameObject NoSkillObj; //��ų�� ���ٸ��̰� ���.
    public GameObject UsingSkillObj; //���� ��ų�� ������̶��
    public GameObject OtherUsingSkillobj; //�ٸ���ų�� ������̶��
    public GameObject SkillLock; //�ٸ���ų�� ������̶��
    public GameObject AutoOn; //�ٸ���ų�� ������̶��
    public GameObject NoMp; //�ٸ���ų�� ������̶��

    public string skillsound;

    //�ð��� �ڽ�Ʈ
    public float curtime; //����ð�
    public float maxtime; //�ð���
    public float castingtime; //�ð���
    public float mpcost; //�ð���

    //���־�
    public string effect_player;
    public string effect_enemy;
    public string sound;

    //����
    public string Skilltype;
    public string RangeType;

    public float Atk;
    public float Matk;
    public float Crit; //�߰� ġ��Ÿ Ȯ��
    public float Critdmg; //�߰� ġ��Ÿ ����
    public float StartAttackTerm; //���� ������ ��� �ð�
    public int AttackCount;

    //����ȭ ����
    public float BreakDmg;


    //����
    bool isusing = false;
    public bool islock = true;
    bool isauto;


    public void BtSetAuto()
    {
        if (string.IsNullOrEmpty(skillid))
            return;
        

        if (isauto)
        {
            AutoOn.SetActive(false);
            this.isauto = false;

        }
        else
        {
            AutoOn.SetActive(true);
            this.isauto = true;
        }
    }
    public void SetAuto(bool isauto)
    {
        if (string.IsNullOrEmpty(skillid))
            return;

        this.isauto = isauto;
        if (isauto)
        {
            AutoOn.SetActive(true);
        }
        else
        {
            AutoOn.SetActive(false);
        }
    }

    public SkillDB.Row skilldata;

    public void SetLockSkill()
    {
        if (islock)
        {
            SkillLock.SetActive(true);
            Skillrare.color = Color.white;
            HaveSkillobj.SetActive(false);
            NoSkillObj.SetActive(false);
            AutoOn.SetActive(false);
            isauto = false;
        }
    }

    public void RefreshSkillOtherUser()
    {
        if (skillid.Equals(""))
            islock = true;
        if (islock)
        {
            SkillLock.SetActive(true);
            Skillrare.color = Color.white;
            HaveSkillobj.SetActive(false);
            NoSkillObj.SetActive(false);
            AutoOn.SetActive(false);
            skillid = "";
            isauto = false;
        }
        else
        {
            skilldata = SkillDB.Instance.Find_Id(skillid);
//            Debug.Log(skillid);
            SkillImage.sprite = SpriteManager.Instance.GetSprite(skilldata.Sprite);
//            Debug.Log("��ų ���̵� " + skilldata.Id);
//            Debug.Log("��ų ���� " + skilldata.Rare);
            Skillrare.color = Inventory.Instance.GetRareColor(skilldata.Rare);
            HaveSkillobj.SetActive(true);
            NoSkillObj.SetActive(false);
            SkillLock.SetActive(false);
            OtherUsingSkillobj.SetActive(false);
            NoMp.SetActive(false);
            SkillCooldownTime.text = "";
            AutoOn.SetActive(false);
            SkillCooldown.fillAmount = 0;
        }
    }
    public void 
        RefreshSkill()
    {
        if (islock)
        {
            SkillLock.SetActive(true);
            Skillrare.color = Color.white;
            skillid = "";
            PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[index] = "";
            HaveSkillobj.SetActive(false);
            NoSkillObj.SetActive(false);
            AutoOn.SetActive(false);
            isauto = false;
            return;
        }
        else
        {
            isauto = true;
            AutoOn.SetActive(true);
        }
        
        if (skillid == "True")
        {
            skillid = "";
            return;
        }
        if (isme)
        {
            skillid = PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[index];
        }

        SkillLock.SetActive(false);
//        Debug.Log(skillid);


        if (!string.IsNullOrEmpty(skillid))
        {
            if (skillid == "True")
            {
                skillid = "";
                return;
            }
            if (skillid == "")
            {
                return;
            }
            //��ų���ִ�
            skilldata = SkillDB.Instance.Find_Id(skillid);
//            Debug.Log(skillid);
            SkillImage.sprite = SpriteManager.Instance.GetSprite(skilldata.Sprite);
            if (isme)
            {
                //��ų�� �̹���
                maxtime = float.Parse(skilldata.CoolTime);
                castingtime = float.Parse(skilldata.CastTime);
                mpcost = float.Parse(skilldata.UseMp);

                effect_enemy = skilldata.HitEffect;
                effect_player = skilldata.PlayerEffect;
                sound = skilldata.Sound;

                Atk = float.Parse(skilldata.Atk);
                Matk = float.Parse(skilldata.Matk);


                Crit = float.Parse(skilldata.Crit);
                Critdmg = float.Parse(skilldata.CritDmg);



                AttackCount = int.Parse(skilldata.AttackCount);

                Skilltype = skilldata.SkillType;
                RangeType = skilldata.RangeType;


                skillsound = skilldata.Sound;

                //����ȭ
                BreakDmg = float.Parse(skilldata.breakpoint);

                StartAttackTerm = float.Parse(skilldata.StartDmgTerm);

                if (Skilltype.Equals("Buff"))
                {
                    curtime = 0;
                    maxtime = 0;
                }
                else
                {
                    curtime = maxtime;
                    
                }
            }
            Skillrare.color = Inventory.Instance.GetRareColor(skilldata.Rare);
            HaveSkillobj.SetActive(true);
            NoSkillObj.SetActive(false);
            SkillLock.SetActive(false);
            OtherUsingSkillobj.SetActive(false);
            NoMp.SetActive(false);
            SkillCooldownTime.text = "";
            SetCooldown();
            islock = false;

            if (Skillmanager.Instance.AutoSkillToggle.isOn && isme)
            {
                AutoOn.SetActive(true);
            }
            else
            {
                AutoOn.SetActive(false);
            }
        }
        else
        {
            //��ų�� ����
            HaveSkillobj.SetActive(false);
            NoSkillObj.SetActive(true);
            AutoOn.SetActive(false);
            NoMp.SetActive(false);
            Skillrare.color = Color.white;
        }
    }

    private void SetCooldown()
    {
        if (!isme)
        {
            SkillCooldown.fillAmount = 0;
            return;
        }
        SkillCooldown.fillAmount = curtime / maxtime;
        if (curtime > 0)
        {
            if (curtime > 1)
                SkillCooldownTime.text = curtime.ToString("N0");
            else
                SkillCooldownTime.text = curtime.ToString("N1");
        }
        else
        {
            SkillCooldownTime.text = "";
        }
    }

    WaitForSeconds wait = new WaitForSeconds(0.1f);

    private IEnumerator Skill()
    {
        while (true)
        {
            yield return wait;
            if (!string.IsNullOrEmpty(skillid))
            {
                if (OtherUsingSkillobj.activeSelf)
                    OtherUsingSkillobj.SetActive(false);
                if (NoMp.activeSelf)
                    NoMp.SetActive(false);
                if (!Battlemanager.Instance.isbattle || !EnemySpawnManager.Instance.isspawn ||
                    Skilltype.Equals("Buff")) continue;
                
                if (curtime != 0)
                {
                    curtime -= 0.1f;
                    SetCooldown();
                }

                if (isauto)
                {
                    Bt_UseSkill();
                }
            }
        }
    }

    private void Start()
    {
        RefreshSkill();
        if (skillid == "true")
            skillid = "";
        if (Skillmanager.Instance.AutoSkillToggle.isOn)
            isauto = true;
        if (isme)
            StartCoroutine(Skill());
    }


    public enum skillstatue
    {
        ��ų����,
        ��ų����
    }

    public void ResetCooldown()
    {
        
        curtime = 0;
        SetCooldown();
    }

    public void ReduceCooldown(float sec)
    {
        curtime -= sec;
        SetCooldown();
    }
    // ReSharper disable Unity.PerformanceAnalysis
    public void Bt_UseSkill()
    {
        
        if (!string.IsNullOrEmpty(skillid))
        {
            if (curtime <= 0)
            {
//                Debug.Log("���� ��" + mpcost);
                float totalmcost = mpcost + mpcost * equipskillmanager.Instance.GetStats((int)equipskillmanager
                    .EquipStatFloat
                    .legenstaffhitper);
                
                float reducetime =  maxtime - (maxtime * castingmanager.mainplayer.Stat_ReduceCoolDown);

                if (skilldata.SkillType.Equals("Buff"))
                {
                    reducetime = maxtime;
                    if(isauto)
                    SetAuto(false);
                    return;
                }
                
            //    Debug.Log("���� �� " + totalmcost);
                if (castingtime == 0 && castingmanager.CastingSpellNoCastime(totalmcost, this))
                {
                    curtime = reducetime;
                }
                else if (castingmanager.CastingSpell(castingtime, totalmcost, this))
                {
                    curtime = reducetime;
                }
                else if (castingmanager.mainplayer.hpmanager.CurMp < totalmcost)
                {
                    //���� ����
                    NoMp.SetActive(true);
                }
                else
                {
                    OtherUsingSkillobj.SetActive(true);
                }
            }
        }
        else
        {
            if(!islock)
            {
                //������� ���� �����̶�� ��ųâ�� ����
                SkillInventory.Instance.SkillInventoryPanel.Show(true);
                SkillInventory.Instance.ShowSkillInventory();
            }
        }
    }

    public void Bt_ShowSkillInfo()
    {
        Debug.Log(skillid);
        SkillInventory.Instance.ShowChangePanel(skillid);

    }
}