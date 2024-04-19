using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEditor.Build.Player;
using UnityEngine;

public class TalismanManager : MonoBehaviour
{
    public talismansetpanel[] setslots;
    public talismanequipslot[] slots;
    public GameObject NoSetobj;
    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].refersh(i);
        }
        CheckTalismanSet();
    }


    public void CheckTalismanSet()
    {
        foreach (var VARIABLE in setslots)
        {
            VARIABLE.gameObject.SetActive(false);
        }

        NoSetobj.SetActive(true);
        
        PlayerBackendData.Instance.Talisman2Set.Clear();
        PlayerBackendData.Instance.Talisman3Set.Clear();
        PlayerBackendData.Instance.Talisman5Set.Clear();


        int[] setcounts = new int[9];
        
        for (int i = 0; i < PlayerBackendData.Instance.EquipTalisman.Length; i++)
        {
            if(PlayerBackendData.Instance.EquipTalisman[i] != "")
            {
                setcounts[int.Parse(TalismanDB.Instance.Find_id(
                   PlayerBackendData.Instance.TalismanData[PlayerBackendData.Instance.EquipTalisman[i]].Itemid).num)]++;
            }
        }

        int equipsetnum = 0;

        for (int i = 0; i < setcounts.Length; i++)
        {
            Inventory.Instance.StringRemove();
            TalismanDB.Row data = TalismanDB.Instance.GetAt(i);
            bool isset = false;

            if (equipsetnum == 3)
            {
                Debug.Log("Ç®¼¼Æ®");
                break;
            }

            if (setcounts[i] >= 2)
            {
                PlayerBackendData.Instance.Talisman2Set.Add(TalismanDB.Instance.GetAt(i).id);
                Inventory.Instance.StringWrite("<color=cyan>");
                Inventory.Instance.StringWrite(Inventory.GetTranslate(data.setinfo1));
                Inventory.Instance.StringWrite("</color>");
              
                NoSetobj.SetActive(false);

                if (setcounts[i] >= 3)
                {
                    Inventory.Instance.StringWrite("\n<color=cyan>");
                    Inventory.Instance.StringWrite(Inventory.GetTranslate(data.setinfo2));
                    Inventory.Instance.StringWrite("</color>");
                    PlayerBackendData.Instance.Talisman3Set.Add(TalismanDB.Instance.GetAt(i).id);
                }
                else
                {
                    Inventory.Instance.StringWrite("\n<color=grey>");
                    Inventory.Instance.StringWrite(Inventory.GetTranslate(data.setinfo2));
                    Inventory.Instance.StringWrite("</color>");
                }
            
                if (setcounts[i] >= 5)
                {
                    Inventory.Instance.StringWrite("\n<color=cyan>");
                    Inventory.Instance.StringWrite(Inventory.GetTranslate(data.setinfo3));
                    Inventory.Instance.StringWrite("</color>");
                    PlayerBackendData.Instance.Talisman5Set.Add(TalismanDB.Instance.GetAt(i).id);
                }
                else
                {
                    Inventory.Instance.StringWrite("\n<color=grey>");
                    Inventory.Instance.StringWrite(Inventory.GetTranslate(data.setinfo3));
                    Inventory.Instance.StringWrite("</color>");
                }


                setslots[equipsetnum].Name.text = Inventory.GetTranslate(data.name);
                setslots[equipsetnum].Setinfo.text = Inventory.Instance.StringEnd();
                setslots[equipsetnum].Images.sprite = SpriteManager.Instance.GetSprite(data.sprite);
                setslots[equipsetnum].gameObject.SetActive(true);
                equipsetnum++;
            }
        }
    }
}

public class Talismandatabase
{
    private string keyid;
    private string itemid;
    private string[] eskill;


    public Talismandatabase(string keyid, string itemid, string[] eskill)
    {
        this.keyid = keyid;
        this.itemid = itemid;
        this.eskill = eskill;
    }

    public string[] Eskill
    {
        get => eskill;
        set => eskill = value;
    }

    public string Itemid
    {
        get => itemid;
        set => itemid = value;
    }

    public string Keyid
    {
        get => keyid;
        set => keyid = value;
    }
}