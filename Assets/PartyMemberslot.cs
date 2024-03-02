using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberslot : MonoBehaviour
{
   public bool isready;
   public GameObject NoUserObj;
   public GameObject HaveUserObj;

   public GameObject Readyobj;
   public int nowstate = 0;
   public playerdata data;

   public Image avata;
   public Material[] playermaterial = new Material[3];
   public Image weapon;
   public Image subweapon;
   public Image pet;

   public Text PlayerName;

 

   public void SetPlayerData(string playerdata)
   {
      data = new playerdata();
      data.GetPlayerData(playerdata);
      ShowPlayer();

      NoUserObj.SetActive(false);
      HaveUserObj.SetActive(true);
      Readyobj.SetActive(false);
   }

   public void Bt_ShowPlayerData()
   {
      otherusermanager.Instance.ShowPlayerData(data.nickname);
   }

   void ShowPlayer()
   {
      avata.sprite = SpriteManager.Instance.GetSprite(data.avatapath);


      if (data.petpath != "")
      {
         pet.sprite = SpriteManager.Instance.GetSprite(data.petpath);
         if (data.petrare != "0")
         {
            playermaterial[2].SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 1f);
            playermaterial[2].SetColor(Shader.PropertyToID("_OuterOutlineColor"),
               Inventory.Instance.GetRareColor(data.petrare));
         }
         else
         {
            playermaterial[2].SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 0f);
         }
        
      }

      if (data.weaponpatn != null)
      {
         weapon.sprite = SpriteManager.Instance.GetSprite(data.weaponpatn);
         if (data.subweaponrare != "0")
         {
            playermaterial[0].SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 1f);
            Debug.Log(data.weaponrare);
            playermaterial[0].SetColor(Shader.PropertyToID("_OuterOutlineColor"),
               Inventory.Instance.GetRareColor(data.weaponrare));
         }
         else
         {
            playermaterial[0].SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 0f);
         }
       
      }
      
      if (data.subweaponpath != null)
      {
         subweapon.sprite = SpriteManager.Instance.GetSprite(data.subweaponpath);
         if (data.subweaponrare != "0")
         {
            playermaterial[1].SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 1f);
            playermaterial[1].SetColor(Shader.PropertyToID("_OuterOutlineColor"),
               Inventory.Instance.GetRareColor(data.subweaponrare));
         }
         else
         {
            playermaterial[1].SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 0f);
         }
      
      }


      PlayerName.text = $"Lv.{data.lv}\n{data.nickname}";
      
   }

   public void Bt_Ready()
   {
      if (isready)
      {
         //레디중이였다면
         isready = false;
         Readyobj.SetActive(false);
      }
      else
      {
         isready = true;
         Readyobj.SetActive(true);
      }
   }
   
   public void ExitPlayer()
   {
      foreach (var t in playermaterial)
         t.SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 0f);
      Readyobj.SetActive(false);
      isready = true;
      data = null;
   }
}

public class playerdata
{
   public string nickname;
   public string indate;
   public int lv;
   public string avatapath;
   public string weaponpatn;
   public string weaponrare;
   public string subweaponpath;
   public string subweaponrare;
   public string petpath;
   public string petrare;


   public playerdata()
   {
   }

   public void GetPlayerData(string datastring)
   {
      string[] datas = datastring.Split(';');

      nickname = datas[0];
      indate = datas[1];
      lv = int.Parse(datas[2]);

      avatapath = datas[3];

      weaponpatn = datas[4];
      weaponrare = datas[5];

      subweaponpath = datas[6];
      subweaponrare = datas[7];

      petpath = datas[8];
      petrare = datas[9];

   }
}