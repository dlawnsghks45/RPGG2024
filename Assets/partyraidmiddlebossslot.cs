using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class partyraidmiddlebossslot : MonoBehaviour
{
    public string mapID;
    public int middlenum;
    public Image MonImage;
   public Image Background;
   public Text Mapname;
   public GameObject fightingobj;
   public Text Fightingname;
   public GameObject clearobj;
   [SerializeField] public partyraidbuff buffs;

   public bool isbattle;
   public bool isclear;

   public int nowdebuff;
   public int nowbuff;

   public void SetData(string mapid, int buffnum, int penaltynum)
   {
       mapID = mapid;
       middlenum = penaltynum;
       MapDB.Row data = MapDB.Instance.Find_id(mapid);
       Background.sprite = SpriteManager.Instance.GetSprite(data.maplayer0);
       MonImage.sprite = SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id(data.monsterid).sprite);
       Mapname.text = Inventory.GetTranslate(data.name);
       isbattle = false;
       isclear = false;
       if (PartyRaidBattlemanager.Instance.battledata.MonsterBuff[penaltynum] != 0)
       {
           nowdebuff = penaltynum;
       }
       else
       {
           nowdebuff = -1;
       }
       nowbuff = buffnum;
       buffs.SetBuff(penaltynum, buffnum);
       fightingobj.SetActive(false);
       clearobj.SetActive(false);
   }

   public void SetClear()
   {
       isclear = true;
       isbattle = false;
       fightingobj.SetActive(false);
       clearobj.SetActive(true);
   }

   public void SetBattle(string NickName)
   {
       Fightingname.text = NickName;
       isbattle = true;
       fightingobj.SetActive(true);
   }
   public void SetNormal()
   {
       isbattle = false;
       isclear = false;

       fightingobj.SetActive(false);
       clearobj.SetActive(false);

   }
   public void Bt_StartMiddleBoss()
   {
       MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
       if (mapdata_Now.maptype != "0")
       {
           alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/사냥터만가능"), alertmanager.alertenum.주의);
           return;
       }
        
       if (mapmanager.Instance.islocating)
       {
           alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/맵이동중불가"),alertmanager.alertenum.주의);
           return;
       }
       
       if (isbattle || isclear)
       {
           alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/이미클리어한레이드"),alertmanager.alertenum.주의);
           return;
       }

       if (PartyRaidRoommanager.Instance.PartyMember[PartyRaidRoommanager.Instance.mypartynum].BattleObj.activeSelf)
       {
           alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/레이드전투중"),alertmanager.alertenum.주의);
           return;
       }
       PartyraidChatManager.Instance.Chat_MiddleRaidStart(middlenum);
   }
   
   

}
