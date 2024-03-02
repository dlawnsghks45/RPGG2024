using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class growthmanager : MonoBehaviour
{
    private static growthmanager _instance = null;
    public static growthmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(growthmanager)) as growthmanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }
    
    public Text nowRankText;
    public GameObject DungeonPart;
    public GameObject EquipmentPart;
    public GameObject LevelUpPart;

    public DungeonSlot dungeonslot;

    public itemiconslot[] Iconslosts;

    public decimal MonHp;
    public Text MonText;
    public itemiconslot[] MonItems;
    public Image Monsprite;

    
    int nowrank;

    public UIView panel;
    
    public void OpenPanel()
    {
        Tutorialmanager.Instance.CheckTutorial("seeequipguide");
        panel.Show(false);
        nowrank = PlayerBackendData.Instance.GetAdLv();
        Refresh();
    }
    
    void Refresh()
    {
        nowRankText.text = string.Format(Inventory.GetTranslate("UI2/모험랭크엔터"), nowrank.ToString());
        growthpathDB.Row data = growthpathDB.Instance.Find_rank(nowrank.ToString());

        //던전
        if (data.dungeonid.Equals("0"))
        {
            //
            DungeonPart.SetActive(false);
        }
        else
        {
            DungeonPart.SetActive(true);
            //던전이있다.
            dungeonslot.mapid = data.dungeonid;
            dungeonslot.Refresh();
        }
        
        
        //던전
        if (data.equipitems.Equals("0"))
        {
            //
            EquipmentPart.SetActive(false);
        }
        else
        {
            EquipmentPart.SetActive(true);
            foreach (var t in Iconslosts)
            {
                t.gameObject.SetActive(false);
            }
            string[] equipid = data.equipitems.Split(';');
            for (int i = 0; i < equipid.Length; i++)
            {
                Iconslosts[i].Refresh(equipid[i], 1, false, false, true);
                Iconslosts[i].gameObject.SetActive(true);
            }
        }
            
        /*
        //던전
        if (data.monid.Equals("0"))
        {
            LevelUpPart.SetActive(false);
        }
        else
        {
            LevelUpPart.SetActive(true);
            foreach (var t in MonItems)
            {
                t.gameObject.SetActive(false);
            }

            monsterDB.Row mondata = monsterDB.Instance.Find_id(data.monid);

            MonHp = decimal.Parse(mondata.hp);
            MonText.text = $"HP:{MonHp:N0}";
            Monsprite.sprite = SpriteManager.Instance.GetSprite(mondata.sprite);
        }
        */
    }

    public void NextRank()
    {
        if(nowrank == 18) return;
        nowrank++;
        Refresh();
    }

    public void PrevRank()
    {
        if(nowrank == 1) return;
        nowrank--;
        Refresh();
    }
}
