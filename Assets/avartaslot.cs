using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class avartaslot : MonoBehaviour
{
    public string id;
    public Image AvartaImage;
    public Image AvartaRare;
    public Text AvartaName;

    public GameObject blind;

    public GameObject isuing;
    public GameObject weaponbody;
    // Start is called before the first frame update
    public void Init(string id)
    {
        weaponbody.SetActive(false);
        this.id = id;
        AvartaImage.sprite = SpriteManager.Instance.GetSprite(AvartaDB.Instance.Find_id(id).sprite);
        AvartaRare.color = Inventory.Instance.GetRareColor(AvartaDB.Instance.Find_id(id).rare);
        AvartaName.text = Inventory.GetTranslate(AvartaDB.Instance.Find_id(id).name);
        AvartaName.color = Inventory.Instance.GetRareColor(AvartaDB.Instance.Find_id(id).rare);
        Refresh();
        
        switch (AvartaDB.Instance.Find_id(id).type)
        {
            case "W":
                weaponbody.SetActive(true);
                break;
        }
    }

    public void Refresh()
    {
        isuing.SetActive(false);
        switch (AvartaDB.Instance.Find_id(id).type)
        {
            case "A":
                if (PlayerBackendData.Instance.avata_avata.Equals(id))
                {
                    isuing.SetActive(true);
                }
                break;
            case "W":
                if (PlayerBackendData.Instance.avata_weapon.Equals(id))
                {
                    isuing.SetActive(true);
                }
                break;
            case "S":
                if (PlayerBackendData.Instance.avata_subweapon.Equals(id))
                {
                    isuing.SetActive(true);
                }
                break;
        }
        
        if (!PlayerBackendData.Instance.playeravata[int.Parse(AvartaDB.Instance.Find_id(id).num)])
        {
            blind.SetActive(true);
        }
        else
        {
            blind.SetActive(false);
        }
    }

    public void Bt_Select()
    {
        avatamanager.Instance.Bt_ShowAvata(id);
    }
}
