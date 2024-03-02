using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Doozy.Engine.Events;
using UnityEngine;
using UnityEngine.UI;

public class collectionrenewalslot : MonoBehaviour
{
    public string id;
    public Image image;
    public Text count;
    public Image Rare;
    public GameObject noti;
    public GameObject lockobj;
    public void Refresh(string ID)
    {
        id = ID;
        CollectionRenewalDB.Row data = CollectionRenewalDB.Instance.Find_id(id);
        bool isequip = bool.Parse(data.isequip);
        
        if (isequip)
        {
            image.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(data.itemid).Sprite);
            count.text = "";
            Rare.color = Inventory.Instance.GetRareColor(EquipItemDB.Instance.Find_id(data.itemid).Rare);

            EquipItemDB.Row ed = EquipItemDB.Instance.Find_id(data.itemid);

            string type = ed.Type;
            noti.SetActive(false);

            if (PlayerBackendData.Instance.GetTypeEquipment(type).Any(VARIABLE => VARIABLE.Value.Itemid.Equals(data.itemid) && !VARIABLE.Value.IsEquip
                    && !VARIABLE.Value.IsLock))
            {
                    noti.SetActive(true);
            }
        }
        else
        {
            image.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(data.itemid).sprite);
            count.text = data.hw;
            Rare.color = Inventory.Instance.GetRareColor(ItemdatabasecsvDB.Instance.Find_id(data.itemid).rare);

            noti.SetActive(false);
            
            if (PlayerBackendData.Instance.CheckItemCount(data.itemid) >= int.Parse(data.hw))
            {
                    noti.SetActive(true);
            }
        }
        
        //잠금확인
        if (PlayerBackendData.Instance.RenewalCollectData[int.Parse(data.num)])
        {
            noti.SetActive(false);
            lockobj.SetActive(false);
        }
        else
        {
            lockobj.SetActive(true);
        }
    }
    public void Bt_SelectCollection()
    {
        CollectionRenewalManager.Instance.Bt_Select(id);
    }
}
