using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class petboxslot : MonoBehaviour
{
    public Image MonsterSprite;
    public Image RareColor;
    public Animator ani;
    public GameObject Rare6effect1;
    public GameObject Rare6effect2;

    public void ShowPet(string id)
    {
        PetDB.Row data = PetDB.Instance.Find_id(id);
        Rare6effect1.SetActive(false);
        Rare6effect2.SetActive(false);
        MonsterSprite.sprite = SpriteManager.Instance.GetSprite(data.sprite);
        RareColor.color = Inventory.Instance.GetRareColor(data.rare);

        if (data.rare.Equals("6"))
        {
         
            ani.SetTrigger("1_show");
        }
        else
        {
            ani.SetTrigger("0_show");
        }
    }

    public void SoundA()
    {
        Soundmanager.Instance.PlayerSound2("Sound/¿œπ›ªÃ¿Ω",0.5f);
    }
    public void SoundB()
    {
        Rare6effect1.SetActive(true);
        Rare6effect2.SetActive(true);
        Soundmanager.Instance.PlayerSound2("Sound/±‡¡§æ˜",1);
    }
}
