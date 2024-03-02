using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class petmixslot : MonoBehaviour
{
   public Image PetImage;
   public Text Petname;
   public Image Petrare;

   public GameObject PetSelectPanel;

   public ParticleSystem startparticle;
   public string petid;

   public void SetData(string petid)
   {
      this.petid = petid;
      PetImage.sprite = SpriteManager.Instance.GetSprite(PetDB.Instance.Find_id(petid).sprite);
      Petname.text = Inventory.GetTranslate(PetDB.Instance.Find_id(petid).name);
      Petname.color = Inventory.Instance.GetRareColor(PetDB.Instance.Find_id(petid).rare);
      Petrare.color = Petname.color;
      PetSelectPanel.SetActive(true);
   }
   public void RemoveData()
   {
      if (petmanager.Instance.PetMixDic.ContainsKey(petid))
      {
         petmanager.Instance.PetMixDic[petid]--;
         if (petmanager.Instance.PetMixDic[petid] <= 0)
         {
            petmanager.Instance.PetMixDic.Remove(petid);
         }
      }
      petid = "";
      PetSelectPanel.SetActive(false);
   }

   public void RemoveDataFinish()
   {
      if (petmanager.Instance.PetMixDic.ContainsKey(petid))
      {
         petmanager.Instance.PetMixDic[petid]--;
         if (petmanager.Instance.PetMixDic[petid] <= 0)
         {
            petmanager.Instance.PetMixDic.Remove(petid);
         }
      }
    
      petid = "";
      PetSelectPanel.SetActive(false);
   }

   
}
