using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Skillinfoslot : MonoBehaviour
{
    public string skillid;
    public Image SkillImage;
    public Image SkillRare;
    public Text Skillname;
    public Text SkillInfo;
    public Text SkillMp;
    public Text SkillCasttime;
    public Text SkillCooltime;
    public Text SkillType;
    public GameObject SkillMonCountObj;
    public Text SkillMonCount; //타격횟수
    public GameObject breakobj;
    public Text breakcount;
    public SkillDB.Row skilldata;
    public GameObject[] Skillattacktype;


    [SerializeField] private GameObject[] Infoobj;
    [SerializeField] private Text[] Infotext;
    
    
    public GameObject[] Dotobj;
    public Text[] DotCount;

    public Color[] skillequipslotcolor;
    public Image[] SKillslots;


    public void ShowSlotNUM(int num)
    {
        foreach (var t in SKillslots)
        {
            t.gameObject.SetActive(true);
            t.color = Color.black;
        }

        SKillslots[num].color = Color.yellow;
    }

    enum infoenum
    {
        피해량,
        타수,
        피해타입,
        타격범위,
        재사용시간,
        시전시간,
        생명력소모,
        정신력소모,
        추가치명타확률,
        추가치명타피해,
        무력화수치,
        출혈,
        화상,
        감전,
        독,
        죽음,
      

    }
    
    public void Refresh(string id)
    {
        foreach (var t in SKillslots)
        {
            t.gameObject.SetActive(false);
        }

        if (skillid != null)
        {
            //스킬이있다
            skilldata = SkillDB.Instance.Find_Id(id);
           // Debug.Log(skillid);
            SkillImage.sprite = SpriteManager.Instance.GetSprite(skilldata.Sprite);
            SkillRare.color = Inventory.Instance.GetRareColor(skilldata.Rare);

            Skillname.text = Inventory.GetTranslate(skilldata.Name);
            Inventory.Instance.ChangeItemRareColor(Skillname,skilldata.Rare);
            SkillInfo.text = Inventory.GetTranslate(skilldata.Info);

            foreach (var t in Infoobj)
            {
                t.SetActive(false);
            }

            if (skilldata.SkillType != "Buff")
            {
                if (skilldata.SkillType != "Attack" || skilldata.SkillType != "MagicAttack")
                {
                    Infoobj[(int)infoenum.피해량].SetActive(true);
                    Infotext[(int)infoenum.피해량].text = string.Format(Inventory.GetTranslate("UI5/퍼센트"),
                        (decimal.Parse(skilldata.Matk) + decimal.Parse(skilldata.Atk)) * 100);


                    if (skilldata.AttackCount != "0")
                    {
                        Infoobj[(int)infoenum.타수].SetActive(true);
                        Infotext[(int)infoenum.타수].text =
                            string.Format(Inventory.GetTranslate("UI5/타격회"), skilldata.AttackCount);
                    }

                    if (skilldata.breakpoint != "0")
                    {
                        Infoobj[(int)infoenum.무력화수치].SetActive(true);
                        Infotext[(int)infoenum.무력화수치].text = skilldata.breakpoint;
                    }

                    if (skilldata.SkillRange != "0")
                    {
                        Infoobj[(int)infoenum.타격범위].SetActive(true);
                        Infotext[(int)infoenum.타격범위].text = skilldata.SkillRange;
                    }

                    if (skilldata.breakpoint != "0")
                    {
                        Infoobj[(int)infoenum.무력화수치].SetActive(true);
                        Infotext[(int)infoenum.무력화수치].text = skilldata.breakpoint;
                    }


                    switch (skilldata.Type)
                    {
                        case "Physic":
                            Infoobj[(int)infoenum.피해타입].SetActive(true);
                            Infotext[(int)infoenum.피해타입].text = Inventory.GetTranslate("UI5/물리");
                            Infotext[(int)infoenum.피해타입].color = Color.red;

                            break;
                        case "Magic":
                            Infoobj[(int)infoenum.피해타입].SetActive(true);
                            Infotext[(int)infoenum.피해타입].text = Inventory.GetTranslate("UI5/마법");
                            Infotext[(int)infoenum.피해타입].color = Color.cyan;
                            break;
                    }
                }
            }


            if (skilldata.CastTime != "0")
            {
                Infoobj[(int)infoenum.시전시간].SetActive(true);
                Infotext[(int)infoenum.시전시간].text = string.Format(Inventory.GetTranslate("UI5/초"),skilldata.CastTime);
            }

            if (skilldata.CoolTime != "0")
            {
                Infoobj[(int)infoenum.재사용시간].SetActive(true);
                Infotext[(int)infoenum.재사용시간].text = string.Format(Inventory.GetTranslate("UI5/초"),skilldata.CoolTime);
            }
            
            if (skilldata.UseHp != "0")
            {
                Infoobj[(int)infoenum. 생명력소모].SetActive(true);
                Infotext[(int)infoenum.생명력소모].text = skilldata.UseHp;
            }

            if (skilldata.UseMp != "0")
            {
                Infoobj[(int)infoenum.정신력소모].SetActive(true);
                Infotext[(int)infoenum.정신력소모].text = skilldata.UseMp;
            }

          
            
            if (skilldata.Crit != "0")
            {
                Infoobj[(int)infoenum.추가치명타확률].SetActive(true);
                Infotext[(int)infoenum.추가치명타확률].text = string.Format(Inventory.GetTranslate("UI5/퍼센트"),skilldata.Crit);
            }
            
            if (skilldata.CritDmg != "0")
            {
                Infoobj[(int)infoenum.추가치명타피해].SetActive(true);
                Infotext[(int)infoenum.추가치명타피해].text = string.Format(Inventory.GetTranslate("UI5/퍼센트"),(decimal.Parse(skilldata.CritDmg)*100m).ToString(CultureInfo.InvariantCulture));
            }
            
           
            
            string[] dottype = skilldata.DotType.Split(';');

            foreach (var t in dottype)
            {
                switch (t)
                {
                    case "Bleed":
                        Infoobj[(int)infoenum.출혈].SetActive(true);
                        Infotext[(int)infoenum.출혈].text = skilldata.DotCount;
                        break;
                    case "Fire":
                        Infoobj[(int)infoenum.화상].SetActive(true);
                        Infotext[(int)infoenum.화상].text = skilldata.DotCount;
                        break;
                    case "Shock":
                        Infoobj[(int)infoenum.감전].SetActive(true);
                        Infotext[(int)infoenum.감전].text = skilldata.DotCount;
                        break;
                    case "Poison":
                        Infoobj[(int)infoenum.독].SetActive(true);
                        Infotext[(int)infoenum.독].text = skilldata.DotCount;
                        break;
                    case "Death":
                        Infoobj[(int)infoenum.죽음].SetActive(true);
                        Infotext[(int)infoenum.죽음].text = skilldata.DotCount;
                        break;
                }
            }
            
            SkillMp.text = skilldata.UseMp;
            if (SkillCasttime != null)
                SkillCasttime.text = skilldata.CastTime;
            if (SkillCooltime != null)
                SkillCooltime.text = skilldata.CoolTime;
/*
            if (breakobj != null)
            {
                if(skilldata.breakpoint != "0")
                {
                    breakobj.SetActive(true);
                    breakcount.text = skilldata.breakpoint;
                }
                else
                {
                    breakobj.SetActive(false);
                }
            }

            if(skilldata.SkillRange != "0")
            {
                SkillMonCount.text = skilldata.SkillRange.ToString();
                SkillMonCountObj.SetActive(true);
            }
            else
            {
                SkillMonCountObj.SetActive(false);
            }
*/
            for (int i = 0; i < Dotobj.Length;i++)
            {
                Dotobj[i].SetActive(false);
            }

            switch(skilldata.DotType)
            {
                case "Bleed": //출혈
                    Dotobj[0].SetActive(true);
                    DotCount[0].text = skilldata.DotCount;
                    break;
                case "Fire": //화상
                    Dotobj[1].SetActive(true);
                    DotCount[1].text = skilldata.DotCount;
                    break;
                case "Shock": //감전
                    Dotobj[2].SetActive(true);
                    DotCount[2].text = skilldata.DotCount;
                    break;
                case "Poison": //
                    Dotobj[3].SetActive(true);
                    DotCount[3].text = skilldata.DotCount;
                    break;
                case "Death":
                    Dotobj[4].SetActive(true);
                    DotCount[4].text = skilldata.DotCount;
                    break;
            }

            if (Skillattacktype.Length != 0)
            {
                for (int i = 0; i < Skillattacktype.Length; i++)
                {
                    Skillattacktype[i].SetActive(false);
                }

                switch (skilldata.RangeType)
                {
                    case "M": //근거리
                        Skillattacktype[0].SetActive(true);
                        break;
                    case "R": //원거리
                        Skillattacktype[1].SetActive(true);
                        break;
                    case "MRR": //근접 원거리
                        Skillattacktype[0].SetActive(true);
                        Skillattacktype[1].SetActive(true);
                        break;
                    case "MR": //마법
                        Skillattacktype[2].SetActive(true);
                        break;
                    case "ALL": //전부
                        Skillattacktype[0].SetActive(true);
                        Skillattacktype[1].SetActive(true);
                        Skillattacktype[2].SetActive(true);
                        break;
                }
            }
            SkillType.text = Inventory.GetTranslate($"SkillStat/{skilldata.Type}");
        }
    }

    public void ShowEquipSkill()
    {
        SkillInventory.Instance.SelectSkillid = skillid;
        SkillInventory.Instance.ShowChangePanel();
    }
}
