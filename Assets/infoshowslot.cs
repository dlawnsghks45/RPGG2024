using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoshowslot : MonoBehaviour
{
   public string infoname;

   public void ShowInfo()
   {
      uimanager.Instance.ShowInfoQuestion(infoname);
   }
}
