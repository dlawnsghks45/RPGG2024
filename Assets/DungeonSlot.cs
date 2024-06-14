using UnityEngine;
using UnityEngine.UI;
public class DungeonSlot : MonoBehaviour
{
    public string mapid;
    public bool islock;

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
        //   MapLevel.text = string.Format(Inventory.GetTranslate("UI/추천 레벨"), MapDB.Instance.Find_id(mapid).minlevel, MapDB.Instance.Find_id(mapid).maxlevel);
        MonsterImage.sprite = SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id(MapDB.Instance.Find_id(mapid).monsterid.Split(';')[1]).sprite);
        MapLevel.text = string.Format(Inventory.GetTranslate("UI2/모험랭크"),PlayerData.Instance.gettierstar(MapDB.Instance.Find_id(mapid).maprank));
        //Debug.Log(MapDB.Instance.Find_id(mapid).maplayer0);
        BackgroundImage0.sprite = SpriteManager.Instance.GetSprite(MapDB.Instance.Find_id(mapid).maplayer0);

        if (isgrowth)
        {
            if (!PlayerBackendData.Instance.sotang_dungeon.Contains(mapid))
            {
                islock = true;
                Debug.Log("잠김");
                mapgobutton.interactable = false;
            }
            else
            {
                islock = false;

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
                Sotang.text = Inventory.GetTranslate("UI3/소탕가능");
            }
            else
            {
                Sotang.text = Inventory.GetTranslate("UI3/소탕불가");
            } 
    }

    public void CheckLock()
    {
        if ((int.Parse(MapDB.Instance.Find_id(mapid).maprank) > PlayerBackendData.Instance.GetAdLv()))
        {
            islock = true;
            mapgobutton.interactable = false;
            Lockpanel.SetActive(true);
            LockLevel.text = string.Format(Inventory.GetTranslate("UI/입장가능조건레벨"), 
               MapDB.Instance.Find_id(mapid).maprank);
        }
        else if ((MapDB.Instance.Find_id(mapid).maparray == "")
            || PlayerBackendData.Instance.sotang_dungeon.Contains(MapDB.Instance.Find_id(mapid).maparray))
        {
            //잠금품
            Lockpanel.SetActive(false);
            mapgobutton.interactable = true;
            islock = false;
        }
        else
        {
            islock = true;
            mapgobutton.interactable = false;
            Lockpanel.SetActive(true);
            LockLevel.text = string.Format(Inventory.GetTranslate("UI/입장가능조건맵4"), 
                Inventory.GetTranslate(MapDB.Instance.Find_id(MapDB.Instance.Find_id(mapid).maparray).name));
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
