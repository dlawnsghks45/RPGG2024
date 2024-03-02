using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class eskillslot : MonoBehaviour
{
   [SerializeField]
    private string[] Skillids; //특수효과 효과들  
    private int nowindex = 0;
   
   public Text esname;
   public Text esinfo;

   public Button prevbt;
   public Button nextbt;

   public Text isStackText;
   public void Bt_NextLevel()
   {
      if(nowindex.Equals(Skillids.Length-1))
         return;
      nowindex+=1;
      Debug.Log("넘겼다");
      RefreshStat();

      prevbt.interactable = true;
      
      if (nowindex.Equals(Skillids.Length - 1))
      {
         nextbt.interactable = false;
      }
   }

   public void Bt_PrevLevel()
   {
      if(nowindex.Equals(0))
         return;
      nowindex-=1;
      RefreshStat();
      nextbt.interactable = true;
      
      if (nowindex.Equals(0))
      {
         prevbt.interactable = false;
      }
   }
   
   
   

   void RefreshStat()
   {
      esname.text = $"[{Inventory.GetTranslate(EquipSkillDB.Instance.Find_id(Skillids[nowindex]).name)} Lv.{EquipSkillDB.Instance.Find_id(Skillids[nowindex]).lv}]";
      Inventory.Instance.ChangeItemRareColor(esname,EquipSkillDB.Instance.Find_id(Skillids[nowindex]).rare);
      esinfo.text = Inventory.GetTranslate(EquipSkillDB.Instance.Find_id(Skillids[nowindex]).info);
   }
   
   public void init(string[] ids)
   {
      Skillids = ids;
      nowindex = 0;

      prevbt.interactable = false;

      if (Skillids.Length == 0)     
         nextbt.interactable = false;
      else
      {
         nextbt.interactable = true;
      }

      if (EquipSkillDB.Instance.Find_id(ids[0]).isstack.Equals("TRUE"))
      {
         isStackText.text = Inventory.GetTranslate("UI2/중복가능");
      }
      else
      {
         isStackText.text = Inventory.GetTranslate("UI2/중복불가");
      }
      
      esname.text = $"[{Inventory.GetTranslate(EquipSkillDB.Instance.Find_id(Skillids[0]).name)} Lv.{EquipSkillDB.Instance.Find_id(Skillids[0]).lv}]";
      Inventory.Instance.ChangeItemRareColor(esname,EquipSkillDB.Instance.Find_id(Skillids[0]).rare);
      esinfo.text = Inventory.GetTranslate(EquipSkillDB.Instance.Find_id(Skillids[0]).info);
   }
}
