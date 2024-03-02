using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildRaidSlot : MonoBehaviour
{
   public string mapid;
   public Image MonSprite;
   public Text MonHp;
   public Text MonName;
   [SerializeField]
   itemiconslot[] reward;


   private void Start()
   {
      monsterDB.Row data = monsterDB.Instance.Find_id(MapDB.Instance.Find_id(mapid).monsterid);
      MonName.text = Inventory.GetTranslate(MapDB.Instance.Find_id(mapid).name);
      MonHp.text = $"HP: {  dpsmanager.convertNumber(decimal.Parse(data.hp))}";
      MonSprite.sprite = SpriteManager.Instance.GetSprite(data.sprite);

    
      bool isdrop= false;
      var num = 0;
      int num2 = int.Parse(MonDropDB.Instance.Find_id(data.bossdrop).num);

      for (var i = num2; i < MonDropDB.Instance.NumRows()-1; i++)
      {
         if (MonDropDB.Instance.Find_num(i.ToString()).id.Equals(data.bossdrop))
         {
            isdrop = true;
            reward[num].Refresh(MonDropDB.Instance.Find_num(i.ToString()).itemid,int.Parse(MonDropDB.Instance.Find_num(i.ToString()).minhowmany),false);
            reward[num].gameObject.SetActive(true);
            num++;
         }
         if (isdrop && !MonDropDB.Instance.Find_num(i.ToString()).id.Equals(data.bossdrop))
            break;
      }

   }

   public void Bt_SetGuildRaid()
   {
      GuildRaidManager.Instance.Bt_ShowGuildRaidInfo(mapid);
   }
}
