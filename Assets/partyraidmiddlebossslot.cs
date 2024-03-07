using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class partyraidmiddlebossslot : MonoBehaviour
{
   public Image MonImage;
   public Image Background;
   public Text Mapname;
   public GameObject fightingobj;
   public GameObject clearobj;
   [SerializeField] public partyraidbuff buffs;

   public bool isbattle;
   public bool isclear;
   
   public void SetData(string mapid,int buffnum)
   {
    //   Background.sprite = SpriteManager.Instance.GetSprite(MapDB.Instance.Find_id(partyroomdata.nowmap).maplayer0);
    
    isbattle = false;
    isclear = false;
   }

   public void SetClear()
   {
       isclear = true;
       isbattle = false;
   }
   
}
