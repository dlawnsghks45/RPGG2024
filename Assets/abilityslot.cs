using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class abilityslot : MonoBehaviour
{
   [SerializeField]
   public int abnum;   
   public Image abilityimage;
   public Image abilityimageTeduri;

   public GameObject Particle;
   public int point;
   private AbilityDBDB.Row data;
   public void init(int num,AbilityDBDB.Row datas)
   {
       abnum = num;
       this.data = datas;
       abilityimage.sprite = SpriteManager.Instance.GetSprite(data.sprite);
       Refresh();
   }
   
    public  void Refresh()
   {
       if (PlayerBackendData.Instance.Abilitys[int.Parse(data.AT)-1] == data.id)
       {
           abilityimageTeduri.color = Color.cyan;
           abilityimage.color = Color.white;
           Particle.SetActive(true);
       }
       else
       {
           abilityimageTeduri.color = Color.gray;
           abilityimage.color = Color.gray;
           Particle.SetActive(false);
       }
   }

   public void Bt_ShowAbility()
   {    
       abilitymanager.Instance.ShowAbility(data);
   }
   
}

