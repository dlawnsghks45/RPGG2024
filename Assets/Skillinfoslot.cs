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
    public Text SkillMonCount; //Ÿ��Ƚ��
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
        ���ط�,
        Ÿ��,
        ����Ÿ��,
        Ÿ�ݹ���,
        ����ð�,
        �����ð�,
        ����¼Ҹ�,
        ���ŷ¼Ҹ�,
        �߰�ġ��ŸȮ��,
        �߰�ġ��Ÿ����,
        ����ȭ��ġ,
        ����,
        ȭ��,
        ����,
        ��,
        ����,
      

    }
    
    public void Refresh(string id)
    {
        foreach (var t in SKillslots)
        {
            t.gameObject.SetActive(false);
        }

        if (skillid != null)
        {
            //��ų���ִ�
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
                    Infoobj[(int)infoenum.���ط�].SetActive(true);
                    Infotext[(int)infoenum.���ط�].text = string.Format(Inventory.GetTranslate("UI5/�ۼ�Ʈ"),
                        (decimal.Parse(skilldata.Matk) + decimal.Parse(skilldata.Atk)) * 100);


                    if (skilldata.AttackCount != "0")
                    {
                        Infoobj[(int)infoenum.Ÿ��].SetActive(true);
                        Infotext[(int)infoenum.Ÿ��].text =
                            string.Format(Inventory.GetTranslate("UI5/Ÿ��ȸ"), skilldata.AttackCount);
                    }

                    if (skilldata.breakpoint != "0")
                    {
                        Infoobj[(int)infoenum.����ȭ��ġ].SetActive(true);
                        Infotext[(int)infoenum.����ȭ��ġ].text = skilldata.breakpoint;
                    }

                    if (skilldata.SkillRange != "0")
                    {
                        Infoobj[(int)infoenum.Ÿ�ݹ���].SetActive(true);
                        Infotext[(int)infoenum.Ÿ�ݹ���].text = skilldata.SkillRange;
                    }

                    if (skilldata.breakpoint != "0")
                    {
                        Infoobj[(int)infoenum.����ȭ��ġ].SetActive(true);
                        Infotext[(int)infoenum.����ȭ��ġ].text = skilldata.breakpoint;
                    }


                    switch (skilldata.Type)
                    {
                        case "Physic":
                            Infoobj[(int)infoenum.����Ÿ��].SetActive(true);
                            Infotext[(int)infoenum.����Ÿ��].text = Inventory.GetTranslate("UI5/����");
                            Infotext[(int)infoenum.����Ÿ��].color = Color.red;

                            break;
                        case "Magic":
                            Infoobj[(int)infoenum.����Ÿ��].SetActive(true);
                            Infotext[(int)infoenum.����Ÿ��].text = Inventory.GetTranslate("UI5/����");
                            Infotext[(int)infoenum.����Ÿ��].color = Color.cyan;
                            break;
                    }
                }
            }


            if (skilldata.CastTime != "0")
            {
                Infoobj[(int)infoenum.�����ð�].SetActive(true);
                Infotext[(int)infoenum.�����ð�].text = string.Format(Inventory.GetTranslate("UI5/��"),skilldata.CastTime);
            }

            if (skilldata.CoolTime != "0")
            {
                Infoobj[(int)infoenum.����ð�].SetActive(true);
                Infotext[(int)infoenum.����ð�].text = string.Format(Inventory.GetTranslate("UI5/��"),skilldata.CoolTime);
            }
            
            if (skilldata.UseHp != "0")
            {
                Infoobj[(int)infoenum. ����¼Ҹ�].SetActive(true);
                Infotext[(int)infoenum.����¼Ҹ�].text = skilldata.UseHp;
            }

            if (skilldata.UseMp != "0")
            {
                Infoobj[(int)infoenum.���ŷ¼Ҹ�].SetActive(true);
                Infotext[(int)infoenum.���ŷ¼Ҹ�].text = skilldata.UseMp;
            }

          
            
            if (skilldata.Crit != "0")
            {
                Infoobj[(int)infoenum.�߰�ġ��ŸȮ��].SetActive(true);
                Infotext[(int)infoenum.�߰�ġ��ŸȮ��].text = string.Format(Inventory.GetTranslate("UI5/�ۼ�Ʈ"),skilldata.Crit);
            }
            
            if (skilldata.CritDmg != "0")
            {
                Infoobj[(int)infoenum.�߰�ġ��Ÿ����].SetActive(true);
                Infotext[(int)infoenum.�߰�ġ��Ÿ����].text = string.Format(Inventory.GetTranslate("UI5/�ۼ�Ʈ"),(decimal.Parse(skilldata.CritDmg)*100m).ToString(CultureInfo.InvariantCulture));
            }
            
           
            
            string[] dottype = skilldata.DotType.Split(';');

            foreach (var t in dottype)
            {
                switch (t)
                {
                    case "Bleed":
                        Infoobj[(int)infoenum.����].SetActive(true);
                        Infotext[(int)infoenum.����].text = skilldata.DotCount;
                        break;
                    case "Fire":
                        Infoobj[(int)infoenum.ȭ��].SetActive(true);
                        Infotext[(int)infoenum.ȭ��].text = skilldata.DotCount;
                        break;
                    case "Shock":
                        Infoobj[(int)infoenum.����].SetActive(true);
                        Infotext[(int)infoenum.����].text = skilldata.DotCount;
                        break;
                    case "Poison":
                        Infoobj[(int)infoenum.��].SetActive(true);
                        Infotext[(int)infoenum.��].text = skilldata.DotCount;
                        break;
                    case "Death":
                        Infoobj[(int)infoenum.����].SetActive(true);
                        Infotext[(int)infoenum.����].text = skilldata.DotCount;
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
                case "Bleed": //����
                    Dotobj[0].SetActive(true);
                    DotCount[0].text = skilldata.DotCount;
                    break;
                case "Fire": //ȭ��
                    Dotobj[1].SetActive(true);
                    DotCount[1].text = skilldata.DotCount;
                    break;
                case "Shock": //����
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
                    case "M": //�ٰŸ�
                        Skillattacktype[0].SetActive(true);
                        break;
                    case "R": //���Ÿ�
                        Skillattacktype[1].SetActive(true);
                        break;
                    case "MRR": //���� ���Ÿ�
                        Skillattacktype[0].SetActive(true);
                        Skillattacktype[1].SetActive(true);
                        break;
                    case "MR": //����
                        Skillattacktype[2].SetActive(true);
                        break;
                    case "ALL": //����
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
