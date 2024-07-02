using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Newbiemanager : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isNewbie;
    public GameObject[] NewbieMark;
    
   
    //싱글톤만들기.
    private static Newbiemanager _instance = null;
    public static Newbiemanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Newbiemanager)) as Newbiemanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }


 
   public void CheckNewbie()
    {
        DateTime indate = DateTime.Parse(PlayerBackendData.Instance.playerindate).AddHours(9);
        
        TimeSpan dateDiff =  indate - Timemanager.Instance.NowTime;
        int diffDay = dateDiff.Days;
        if (PlayerBackendData.Instance.GetLv() > 3500)
        {
            //뉴비없음
            foreach (var VARIABLE in NewbieMark)
            {
                VARIABLE.SetActive(false);
            }

            isNewbie = false;
        }
        else
        {
            //뉴비없음
            foreach (var VARIABLE in NewbieMark)
            {
                VARIABLE.SetActive(true);
            }

            isNewbie = true;
        }
    }

   public void FalseNewbie()
   {
       //뉴비없음
       foreach (var VARIABLE in NewbieMark)
       {
           VARIABLE.SetActive(false);
       }

       isNewbie = false;
   }
}
