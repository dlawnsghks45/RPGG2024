using UnityEngine;
using UnityEngine.UI;
public class DungeonSlot : MonoBehaviour
{
    public string mapid;

    public Text MapName;
    public Text MapLevel;
    public Image MonsterImage;
    public Image BackgroundImage0;

    public GameObject Lockpanel;
    public Text LockLevel;
    public Text Sotang;


    public Button mapgobutton;
    private void Start()
    {
        Refresh();
        CheckLock();
    }

    public void Refresh()
    {
        MapName.text = Inventory.GetTranslate(MapDB.Instance.Find_id(mapid).name);
        //   MapLevel.text = string.Format(Inventory.GetTranslate("UI/ÃßÃµ ·¹º§"), MapDB.Instance.Find_id(mapid).minlevel, MapDB.Instance.Find_id(mapid).maxlevel);
        MonsterImage.sprite = SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id(MapDB.Instance.Find_id(mapid).monsterid.Split(';')[1]).sprite);
        MapLevel.text = string.Format(Inventory.GetTranslate("UI2/¸ðÇè·©Å©"),PlayerData.Instance.gettierstar(MapDB.Instance.Find_id(mapid).maprank));
        //Debug.Log(MapDB.Instance.Find_id(mapid).maplayer0);
        BackgroundImage0.sprite = SpriteManager.Instance.GetSprite(MapDB.Instance.Find_id(mapid).maplayer0);

        if (isgrowth)
        {
            if (PlayerBackendData.Instance.GetAdLv() < int.Parse(MapDB.Instance.Find_id(mapid).maprank))
            {
                Debug.Log("Àá±è");
                mapgobutton.interactable = false;
            }
            else
            {
                mapgobutton.interactable = true;
            }
        }
        
    }

    [SerializeField] private bool isgrowth;

    public void OnEnable()
    {
            CheckLock();
            if (PlayerBackendData.Instance.sotang_dungeon.Contains(mapid))
            {
                Sotang.text = Inventory.GetTranslate("UI3/¼ÒÅÁ°¡´É");
            }
            else
            {
                Sotang.text = Inventory.GetTranslate("UI3/¼ÒÅÁºÒ°¡");
            } 
    }

    public void CheckLock()
    {
        if (PlayerBackendData.Instance.GetAdLv() < int.Parse(MapDB.Instance.Find_id(mapid).maprank))
        {
            mapgobutton.interactable = false;
            if (!isgrowth)
            {
                Debug.Log("Àá±è");
                //Àá±Ý
                Lockpanel.SetActive(true);

                string[] lvs = DungeonDB.Instance.Find_id(mapid).levelid.Split(';');

                switch (lvs.Length)
                {
                    case 1:
                        LockLevel.text = string.Format(Inventory.GetTranslate("UI2/¸ðÇè·©Å©"),
                            PlayerData.Instance.gettierstar(DungeonDB.Instance.Find_id(lvs[0]).maprank));
                        break;
                    case 2:
                        LockLevel.text =
                            $"{PlayerData.Instance.gettierstar(DungeonDB.Instance.Find_id(lvs[0]).maprank)}/{PlayerData.Instance.gettierstar(DungeonDB.Instance.Find_id(lvs[1]).maprank)}";
                        break;
                    case 3:
                        LockLevel.text =
                            $"{PlayerData.Instance.gettierstar(DungeonDB.Instance.Find_id(lvs[0]).maprank)}/{PlayerData.Instance.gettierstar(DungeonDB.Instance.Find_id(lvs[1]).maprank)}/{PlayerData.Instance.gettierstar(DungeonDB.Instance.Find_id(lvs[2]).maprank)}";
                        break;
                    case 4:
                        LockLevel.text =
                            $"{PlayerData.Instance.gettierstar(DungeonDB.Instance.Find_id(lvs[0]).maprank)}/{PlayerData.Instance.gettierstar(DungeonDB.Instance.Find_id(lvs[1]).maprank)}/{PlayerData.Instance.gettierstar(DungeonDB.Instance.Find_id(lvs[2]).maprank)}/{PlayerData.Instance.gettierstar(DungeonDB.Instance.Find_id(lvs[3]).maprank)}";
                        break;
                }
            }
        }
        else
        {
            //Àá±ÝÇ°
            Lockpanel.SetActive(false);
            mapgobutton.interactable = true;
        }
    }


    public void Bt_SelectDungeon()
    {
        if (isgrowth)
        {
            uimanager.Instance.AddUiview(growthmanager.Instance.panel,true);
            DungeonManager.Instance.DungeonPanel.Show(false);
            DungeonManager.Instance.RefreshCount();
        }
        DungeonManager.Instance.Bt_SelectDungeon(mapid);
    }
}
