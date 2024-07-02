using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checktogglepanel : MonoBehaviour
{
   public void OnEnable()
   {
      CheckDugneonCore();
   }

   void CheckDugneonCore()
   {
      if (
         PlayerBackendData.Instance.CheckItemCount("3000") > 0 ||
         PlayerBackendData.Instance.CheckItemCount("3001") > 0 ||
         PlayerBackendData.Instance.CheckItemCount("3003") > 0 ||
         PlayerBackendData.Instance.CheckItemCount("3004") > 0 ||
         PlayerBackendData.Instance.CheckItemCount("3005") > 0 ||
         PlayerBackendData.Instance.CheckItemCount("3006") > 0 ||
         PlayerBackendData.Instance.CheckItemCount("3007") > 0 ||
         PlayerBackendData.Instance.CheckItemCount("3008") > 0 ||
         PlayerBackendData.Instance.CheckItemCount("3009") > 0 ||
         PlayerBackendData.Instance.CheckItemCount("3010") > 0 ||
         PlayerBackendData.Instance.CheckItemCount("3011") > 0)
      {
         dungeontoggle.SetActive(true);
      }
      else
      {
         dungeontoggle.SetActive(false);
      }
   }

   public GameObject dungeontoggle;
}
