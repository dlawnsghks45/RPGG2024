using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class dpsslot : MonoBehaviour
{
    public Image TypeImage; //�̹��� ��
    public Image DPSImage;
    public Text DPSName;
    public Text DPSCount;
    public Text DAMAGE;
    public string ids;
    public void ShowDmg(DPS dpsdata)
    {
        if(dpsdata.nowsecond.Equals(0))
            return;
        if(dpsdata.count.Equals(0))
            return;
        //���� ���̵�� �ǳʶٱ�.
            ids = dpsdata.id;
//            Debug.Log(dpsdata.id);
          //  Debug.Log(dpsdata.type.ToString());
            switch (dpsdata.type)
            {
                case dpsmanager.attacktype.�����Ƽ:
                    string a = dpsdata.id;
                    if (dpsdata.id.Contains("A"))
                    {
                       a  = dpsdata.id.Remove(0,1);

                    }
                    DPSCount.text = dpsdata.count.ToString("N0");
                    TypeImage.color = Color.white;
                    DPSImage.enabled = true;
                    DPSImage.sprite = SpriteManager.Instance.GetSprite(AbilityDBDB.Instance.Find_id(a).sprite);
                    DPSName.text = Inventory.GetTranslate("UI5/�����Ƽ");
                    break;
                case dpsmanager.attacktype.�⺻����:
                    DPSCount.text = dpsdata.count.ToString("N0");
                    TypeImage.color = Color.white;
                    DPSImage.enabled = false;
                    DPSName.text = Inventory.GetTranslate("UI3/�⺻����");
                 //   DAMAGE.text = $"{dpsmanager.convertNumber(dpsdata.totaldmg)}\n({dpsmanager.convertNumber(dpsdata.totaldmg/dpsdata.count)})";
                    break;
                case dpsmanager.attacktype.������ų����:
                    TypeImage.color = Color.red;
                    DPSImage.enabled = true;
                    DPSImage.sprite = SpriteManager.Instance.GetSprite(SkillDB.Instance.Find_Id(dpsdata.id).Sprite);
                    DPSName.text = Inventory.GetTranslate(SkillDB.Instance.Find_Id(dpsdata.id).Name);
                  //  DAMAGE.text = $"{dpsmanager.convertNumber(dpsdata.totaldmg)}\n({dpsmanager.convertNumber(dpsdata.totaldmg/dpsdata.count)})";

                    break;
                case dpsmanager.attacktype.������ų����:
//                    Debug.Log("ī��Ʈ" + dpsdata.count);
                //    Debug.Log("ī��Ʈ����̵�" + dpsdata.countdivide);
                    DPSCount.text = Math.Truncate((float)(dpsdata.count / dpsdata.countdivide)).ToString("N0");
                    TypeImage.color = Color.cyan;
                    DPSImage.enabled = true;
                    DPSImage.sprite = SpriteManager.Instance.GetSprite(SkillDB.Instance.Find_Id(dpsdata.id).Sprite);
                    DPSName.text = Inventory.GetTranslate(SkillDB.Instance.Find_Id(dpsdata.id).Name);
                 //   Debug.Log(dpsdata.id);
//                    Debug.Log(SkillDB.Instance.Find_Id(dpsdata.id).AttackCount);
                  //  DAMAGE.text = $"{dpsmanager.convertNumber(dpsdata.totaldmg)}\n({dpsmanager.convertNumber(dpsdata.totaldmg/(dpsdata.count / int.Parse(SkillDB.Instance.Find_Id(dpsdata.id).AttackCount)))})";

                    break;
                case dpsmanager.attacktype.Ư��ȿ��:
                    string ekid = dpsdata.id[1..];
                    DPSCount.text = dpsdata.count.ToString("N0");
                    TypeImage.color = Color.green;
                    DPSImage.enabled = false;
//                    Debug.Log(ekid);
                    DPSName.text = Inventory.GetTranslate(EquipSkillDB.Instance.Find_id(ekid).name);
                  //  DAMAGE.text = $"{dpsmanager.convertNumber(dpsdata.totaldmg)}\n({dpsmanager.convertNumber(dpsdata.totaldmg/dpsdata.count)})";

                    break;
                case dpsmanager.attacktype.�нú�:
                    DPSCount.text = dpsdata.count.ToString("N0");
                    TypeImage.color = Color.magenta;
                    DPSImage.enabled = false;
//                    Debug.Log(dpsdata.id);
                        DPSName.text = Inventory.GetTranslate(PassiveDB.Instance.Find_id(dpsdata.id).name);
                  //  DAMAGE.text = $"{dpsmanager.convertNumber(dpsdata.totaldmg)}\n({dpsmanager.convertNumber(dpsdata.totaldmg/dpsdata.count)})";

                    break;
                case dpsmanager.attacktype.�����̻�:
                    DPSCount.text = dpsdata.count.ToString("N0");
                    TypeImage.color = Color.yellow;
                    DPSImage.enabled = false;
                    switch (dpsdata.id)
                    {
                        case "Dot0": //���� 
                            DPSName.text = Inventory.GetTranslate("UI3/����");
                            DPSImage.sprite = dpsmanager.Instance.spritedot[0];
                            break;
                        case "Dot1": //ȭ��
                            DPSName.text = Inventory.GetTranslate("UI3/ȭ��");
                            DPSImage.sprite = dpsmanager.Instance.spritedot[1];
                            break;
                        case "Dot2": //����
                            DPSName.text = Inventory.GetTranslate("UI3/����");
                            DPSImage.sprite = dpsmanager.Instance.spritedot[2];
                            break;
                        case "Dot3": //�͵�
                            DPSName.text = Inventory.GetTranslate("UI3/�͵�");
                            DPSImage.sprite = dpsmanager.Instance.spritedot[3];
                            break;
                        case "Dot4": //����
                            DPSName.text = Inventory.GetTranslate("UI3/����");
                            DPSImage.sprite = dpsmanager.Instance.spritedot[4];
                            break;
                        case "Dot5": //����
                            DPSName.text = Inventory.GetTranslate("UI3/����");
                            DPSImage.sprite = dpsmanager.Instance.spritedot[4];
                            break;
                    }
                    //DAMAGE.text = $"{dpsmanager.convertNumber(dpsdata.totaldmg)}\n({dpsmanager.convertNumber(dpsdata.totaldmg/dpsdata.count)})";
                    break;
        }
         //   DAMAGE.text = $"{dpsmanager.convertNumber(dpsdata.totaldmg)}";
         if(dpsdata.totaldmg != 0)
            DAMAGE.text = $"{dpsmanager.convertNumber(dpsdata.totaldmg)}\n({dpsmanager.convertNumber(dpsdata.dps)})";
         else
         {
             DAMAGE.text = "";
         }
    }
}
