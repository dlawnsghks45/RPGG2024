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
    //스킬의 FM
    /*
     * 1.스킬은 액티브와 패시브가 있다. 
     * 2.스킬 사용 시 다른 스킬 사용이 불가능 하다.
     * -.스킬에는 시전시간이 있다. 시전 중 다른 스킬 사용 불가.
     * 3.스킬은 직업마다 칸이 다르다. 최대 12개의 스킬이 사용 가능하다.
     * 4.스킬 사용 시 마나가 소비 된다. 패시브는 X
     * 5.
     * */
    public Image SkillImage;
    public Image SkillCooldown;
    public Image Skillrare;
    public Text SkillCooldownTime;

    //스킬 있다.
    public GameObject HaveSkillobj;
    public GameObject NoSkillObj; //스킬이 없다면이게 뜬다.
    public GameObject UsingSkillObj; //현재 스킬을 사용중이라면
    public GameObject OtherUsingSkillobj; //다른스킬이 사용중이라면
    public GameObject SkillLock; //다른스킬이 사용중이라면
    public GameObject AutoOn; //다른스킬이 사용중이라면
    public GameObject NoMp; //다른스킬이 사용중이라면

    public string skillsound;

    //시간및 코스트
    public float curtime; //재사용시간
    public float maxtime; //시간초
    public float castingtime; //시간초
    public float mpcost; //시간초

    //비주얼
    public string effect_player;
    public string effect_enemy;
    public string sound;

    //피해
    public string Skilltype;
    public string RangeType;

    public float Atk;
    public float Matk;
    public float Crit; //추가 치명타 확률
    public float Critdmg; //추가 치명타 피해
    public float StartAttackTerm; //공격 시작전 대기 시간
    public int AttackCount;

    //무력화 관련
    public float BreakDmg;


    //관련
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
//            Debug.Log("스킬 아이디 " + skilldata.Id);
//            Debug.Log("스킬 레어 " + skilldata.Rare);
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
            //스킬이있다
            skilldata = SkillDB.Instance.Find_Id(skillid);
//            Debug.Log(skillid);
            SkillImage.sprite = SpriteManager.Instance.GetSprite(skilldata.Sprite);
            if (isme)
            {
                //스킬의 이미지
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

                //무력화
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
            //스킬이 없다
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
        스킬있음,
        스킬없음
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
//                Debug.Log("기존 량" + mpcost);
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
                
            //    Debug.Log("변형 량 " + totalmcost);
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
                    //마나 부족
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
                //잠겨있지 않은 슬릇이라면 스킬창을 연다
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