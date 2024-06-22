using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class dpsslot : MonoBehaviour
{
    public Image TypeImage; //이미지 색
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
        //같은 아이디면 건너뛰기.
            ids = dpsdata.id;
//            Debug.Log(dpsdata.id);
          //  Debug.Log(dpsdata.type.ToString());
            switch (dpsdata.type)
            {
                case dpsmanager.attacktype.어빌리티:
                    string a = dpsdata.id;
                    if (dpsdata.id.Contains("A"))
                    {
                       a  = dpsdata.id.Remove(0,1);

                    }
                    DPSCount.text = dpsdata.count.ToString("N0");
                    TypeImage.color = Color.white;
                    DPSImage.enabled = true;
                    DPSImage.sprite = SpriteManager.Instance.GetSprite(AbilityDBDB.Instance.Find_id(a).sprite);
                    DPSName.text = Inventory.GetTranslate("UI5/어빌리티");
                    break;
                case dpsmanager.attacktype.기본공격:
                    DPSCount.text = dpsdata.count.ToString("N0");
                    TypeImage.color = Color.white;
                    DPSImage.enabled = false;
                    DPSName.text = Inventory.GetTranslate("UI3/기본공격");
                 //   DAMAGE.text = $"{dpsmanager.convertNumber(dpsdata.totaldmg)}\n({dpsmanager.convertNumber(dpsdata.totaldmg/dpsdata.count)})";
                    break;
                case dpsmanager.attacktype.물리스킬공격:
                    TypeImage.color = Color.red;
                    DPSImage.enabled = true;
                    DPSImage.sprite = SpriteManager.Instance.GetSprite(SkillDB.Instance.Find_Id(dpsdata.id).Sprite);
                    DPSName.text = Inventory.GetTranslate(SkillDB.Instance.Find_Id(dpsdata.id).Name);
                  //  DAMAGE.text = $"{dpsmanager.convertNumber(dpsdata.totaldmg)}\n({dpsmanager.convertNumber(dpsdata.totaldmg/dpsdata.count)})";

                    break;
                case dpsmanager.attacktype.마법스킬공격:
//                    Debug.Log("카운트" + dpsdata.count);
                //    Debug.Log("카운트디바이드" + dpsdata.countdivide);
                    DPSCount.text = Math.Truncate((float)(dpsdata.count / dpsdata.countdivide)).ToString("N0");
                    TypeImage.color = Color.cyan;
                    DPSImage.enabled = true;
                    DPSImage.sprite = SpriteManager.Instance.GetSprite(SkillDB.Instance.Find_Id(dpsdata.id).Sprite);
                    DPSName.text = Inventory.GetTranslate(SkillDB.Instance.Find_Id(dpsdata.id).Name);
                 //   Debug.Log(dpsdata.id);
//                    Debug.Log(SkillDB.Instance.Find_Id(dpsdata.id).AttackCount);
                  //  DAMAGE.text = $"{dpsmanager.convertNumber(dpsdata.totaldmg)}\n({dpsmanager.convertNumber(dpsdata.totaldmg/(dpsdata.count / int.Parse(SkillDB.Instance.Find_Id(dpsdata.id).AttackCount)))})";

                    break;
                case dpsmanager.attacktype.특수효과:
                    string ekid = dpsdata.id[1..];
                    DPSCount.text = dpsdata.count.ToString("N0");
                    TypeImage.color = Color.green;
                    DPSImage.enabled = false;
//                    Debug.Log(ekid);
                    DPSName.text = Inventory.GetTranslate(EquipSkillDB.Instance.Find_id(ekid).name);
                  //  DAMAGE.text = $"{dpsmanager.convertNumber(dpsdata.totaldmg)}\n({dpsmanager.convertNumber(dpsdata.totaldmg/dpsdata.count)})";

                    break;
                case dpsmanager.attacktype.패시브:
                    DPSCount.text = dpsdata.count.ToString("N0");
                    TypeImage.color = Color.magenta;
                    DPSImage.enabled = false;
//                    Debug.Log(dpsdata.id);
                        DPSName.text = Inventory.GetTranslate(PassiveDB.Instance.Find_id(dpsdata.id).name);
                  //  DAMAGE.text = $"{dpsmanager.convertNumber(dpsdata.totaldmg)}\n({dpsmanager.convertNumber(dpsdata.totaldmg/dpsdata.count)})";

                    break;
                case dpsmanager.attacktype.상태이상:
                    DPSCount.text = dpsdata.count.ToString("N0");
                    TypeImage.color = Color.yellow;
                    DPSImage.enabled = false;
                    switch (dpsdata.id)
                    {
                        case "Dot0": //출혈 
                            DPSName.text = Inventory.GetTranslate("UI3/출혈");
                            DPSImage.sprite = dpsmanager.Instance.spritedot[0];
                            break;
                        case "Dot1": //화상
                            DPSName.text = Inventory.GetTranslate("UI3/화상");
                            DPSImage.sprite = dpsmanager.Instance.spritedot[1];
                            break;
                        case "Dot2": //감전
                            DPSName.text = Inventory.GetTranslate("UI3/감전");
                            DPSImage.sprite = dpsmanager.Instance.spritedot[2];
                            break;
                        case "Dot3": //맹독
                            DPSName.text = Inventory.GetTranslate("UI3/맹독");
                            DPSImage.sprite = dpsmanager.Instance.spritedot[3];
                            break;
                        case "Dot4": //죽음
                            DPSName.text = Inventory.GetTranslate("UI3/죽음");
                            DPSImage.sprite = dpsmanager.Instance.spritedot[4];
                            break;
                        case "Dot5": //죽음
                            DPSName.text = Inventory.GetTranslate("UI3/절명");
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
