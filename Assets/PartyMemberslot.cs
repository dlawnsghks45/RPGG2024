using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberslot : MonoBehaviour
{
   public int PartyNum; //0~3
   public GameObject NoUserObj;
   public GameObject HaveUserObj;
   public int nowstate = 0;
   public playerdata data;

   public Image avata;
   public Material[] playermaterial = new Material[3];
   public Image weapon;
   public Image subweapon;
   public Image pet;

   public Text PlayerName;

   public GameObject BattleObj;
   public Text BattleText;

   public GameObject[] BuffObj;
   public Text[] BuffObjText;
   
   public void Set_Battle(string mapname)
   {
      BattleText.text = mapname;
      BattleObj.SetActive(true);
   }

   public void Set_EndBattle()
   {
      BattleObj.SetActive(false);
   }

   public void RefreshPlayerBuff()
   {
      foreach (var t in BuffObj)
      {
         t.SetActive(false);
      }

      for (int i = 0; i < PartyRaidBattlemanager.Instance.battledata.playerBuff.Length; i++)
      {
         if (PartyRaidBattlemanager.Instance.battledata.playerBuff[i] != 0)
         {
            BuffObj[i].SetActive(true);
            BuffObjText[i].text = PartyRaidBattlemanager.Instance.battledata.playerBuff[i].ToString();
         }
      }
   }
   
   public void SetPartyNum()
   {
      PartyRaidRoommanager.Instance.SelectPartyUsernum = PartyNum;
   }

   public void SetPlayerData(string playerdata,int num)
   {
      data = new playerdata();
      data.GetPlayerData(playerdata);
      if (data.nickname == PlayerBackendData.Instance.nickname)
      {
         Debug.Log("내자리는" + num);
         PartyRaidRoommanager.Instance.mypartynum = num;
      }
      
      data.GetPlayerData(playerdata);
      ShowPlayer();
      if (PartyRaidRoommanager.Instance.nowmyleadernickname == PlayerBackendData.Instance.nickname)
      {
         PartyRaidRoommanager.Instance.StartButton.SetActive(true);
      }
      else
      {
   
         PartyRaidRoommanager.Instance.StartButton.SetActive(false);
      }
      NoUserObj.SetActive(false);
      HaveUserObj.SetActive(true);
      BattleObj.SetActive(false);
   }

   public void Bt_ShowPlayerData()
   {
      otherusermanager.Instance.Bt_ShowChatUserData(avata.sprite, weapon.sprite, subweapon.sprite,
         data.nickname);
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

   public void Bt_ShowInvite()
   {
      PartyRaidRoommanager.Instance.ShowInvitePanel();
   }
   
   public void ExitPlayer()
   {
      foreach (var t in playermaterial)
         t.SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 0f);
      data = null;
      NoUserObj.SetActive(true);
      HaveUserObj.SetActive(false);
      foreach(var t in BuffObj)
         t.SetActive(false);
   }
}

public class playerdata
{
   public string dataall;
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
      dataall = datastring;
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

   public string GiveData()
   {
      return dataall;
   }
}