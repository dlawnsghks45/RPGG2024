using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class talismanmixslot : MonoBehaviour
{
    public int selectnum;
    public string keyid;
    public string Itemid;
    public GameObject No;
    public GameObject Yes;
    public Image Image;
    public Image talismancolor;
    public Image[] Eskill;
    public Image Line;
    public ParticleSystem startparticle;

    void ShowSlot()
    {
        for (int i = 0; i < TalismanManager.Instance.talismaninven.Count; i++)
        {
            if (TalismanManager.Instance.talismaninven[i].keyid.Equals(keyid))
            {
                TalismanManager.Instance.talismaninven[i].gameObject.SetActive(true);
                break;
            }
        }
    }
    
    public void RemoveItem()
    {
       
        keyid = "";
        No.SetActive(true);
        Yes.SetActive(false);
        Line.color = Color.black;
        talismancolor.color = Color.white;
    }


    public void SetItem(Talismandatabase data)
    {
        Line.color = Color.yellow;
        TalismanDB.Row datas = TalismanDB.Instance.Find_id(data.Itemid);
        keyid = data.Keyid;
        Itemid = data.Itemid;
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
        No.SetActive(false);
        Yes.SetActive(true);
        
        Talismanmixmanager.Instance.RefreshResult();
    }

    public void StartMix()
    {
        startparticle.Play();
    }


    public void Bt_SelectNum()
    {
        if (keyid != "")
        {
            ShowSlot();
            RemoveItem();
            Talismanmixmanager.Instance.RefreshResult();
        }
        else
        {
            Talismanmixmanager.Instance.Bt_SelectItem(selectnum);
        }
    }
}