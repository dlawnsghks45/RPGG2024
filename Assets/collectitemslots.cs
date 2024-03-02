using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class collectitemslots : MonoBehaviour
{
   public Image Panel;
   //????
   public string id;
   public CollectDatabase data;
   
   public collectionslot[] items;

   public Text Name;
   public Text Info;

   public bool isequip;

   private void Awake()
   {
      Refresh();
   }

   public void Refresh()
   {
      foreach (var t in items)
         t.gameObject.SetActive(false);

      //???
      isequip = CollectionDB.Instance.Find_id(id).collecttype.Equals("equip") ? true : false;


      Name.text = Inventory.GetTranslate(CollectionDB.Instance.Find_id(id).name);
      Info.text = Inventory.GetTranslate(CollectionDB.Instance.Find_id(id).info);
      int count = 0;// t.items.Length;
      int notfinishhave = 0;
      for (int i = 0; i < data.ItemID.Length; i++)
      {
         items[i].Refresh(data.ItemID[i],data.Curcount[i], data.Maxcount[i],data.Isfinish[i] ,isequip);
         items[i].gameObject.SetActive(true);
         if (items[i].isfinish)
         {
            count++;
         }

         if (data.Curcount[i] >= data.Maxcount[i])
         {
            notfinishhave++;
         }
      }

      if (data
          .ItemID.Length.Equals(count))
      {
         data.Isfinishall = true;
      }

      if (notfinishhave != 0 && !data.Isfinishall)
      {
//         Debug.Log("수집 넣을 수 있어!!!!!!!");
         transform.SetAsFirstSibling();
      }
      

      if (data.Isfinishall)
      {
         transform.SetAsLastSibling();
         Panel.color= Color.cyan;
         Info.color = Color.cyan;
      }
      
      
      
      
   }
}
