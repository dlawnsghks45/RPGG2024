using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class talismanslot : MonoBehaviour
{
    public string keyid;
    public Image Image;
    public Image[] Eskill;
    public GameObject Lockobj;
    public Image talismancolor;
    public void Refresh(Talismandatabase data)
    {
        TalismanDB.Row datas = TalismanDB.Instance.Find_id(data.Itemid);
        keyid = data.Keyid;
        //이미지
        Image.sprite = SpriteManager.Instance.GetSprite(datas.sprite);
        //특수효과]

        int colornum = 0;
        for (int i = 0; i < Eskill.Length; i++)
        {
            Eskill[i].gameObject.SetActive(false);
        }
        if (data.Eskill != null)
        {

            for (int i = 0; i < data.Eskill.Count; i++)
            {
                Eskill[i].gameObject.SetActive(true);
                Eskill[i].color = Inventory.Instance.GetRareColor(EquipSkillDB.Instance.Find_id(data.Eskill[i]).rare);
                colornum += int.Parse(EquipSkillDB.Instance.Find_id(data.Eskill[i]).rare);
            }
        }

        talismancolor.color = TalismanManager.Instance.GetTalismanColor(colornum);
        
        
        //잠금
        if (data.Islock)
        {
            Lockobj.SetActive(true);
        }
        else
        {
            Lockobj.SetActive(false);
        }
    }

    public void Bt_ShowInfo()
    {
        if (PlayerBackendData.Instance.TalismanData.TryGetValue(keyid, out var value))
        {
            TalismanManager.Instance.Bt_ShowTalisman(value,this);
        }
    }
}
