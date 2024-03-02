using UnityEngine;
using UnityEngine.UI;

public class Raidslot : MonoBehaviour
{
    public string mapid;
    public Text bossname;
    public Text bosshp;
    public GameObject lockpanel;
    public bool islock;
    public Text locktxt;
    public Image bossimage;
    [SerializeField]
    itemiconslot[] reward;
    private void Start()
    {
        Refresh();
    }
    private void OnEnable()
    {
        CheckLock();
    }

    void CheckLock()
    {
        if (int.Parse(MapDB.Instance.Find_id(mapid).maprank) <= PlayerBackendData.Instance.GetAdLv())
        {
            islock = false;
            lockpanel.SetActive(false);
        }
        else
        {
            islock = true;
            lockpanel.SetActive(true);
            locktxt.text = string.Format(Inventory.GetTranslate("UI/입장가능조건레벨"), 
                PlayerData.Instance.gettierstar(MapDB.Instance.Find_id(mapid).maprank));
        }
        
        if (PlayerBackendData.Instance.sotang_raid.Contains(mapid))
        {
            bosshp.text = Inventory.GetTranslate("UI3/소탕가능");
        }
        else
        {
            bosshp.text = Inventory.GetTranslate("UI3/소탕불가");
        } 
    }

    private void Refresh()
    {
        monsterDB.Row data = monsterDB.Instance.Find_id(MapDB.Instance.Find_id(mapid).monsterid);
        CheckLock();

        bossname.text = Inventory.GetTranslate(data.name);

        if (PlayerBackendData.Instance.sotang_raid.Contains(mapid))
        {
            bosshp.text = Inventory.GetTranslate("UI3/소탕가능");
        }
        else
        {
            bosshp.text = Inventory.GetTranslate("UI3/소탕불가");
        }

        bossimage.sprite = SpriteManager.Instance.GetSprite(data.sprite);


        bool isdrop = false;
        var num = 0;

        int num2 = int.Parse(MonDropDB.Instance.Find_id(data.dropid).num);

      

        for (var i = num2; i < MonDropDB.Instance.NumRows() - 1; i++)
        {
            if (MonDropDB.Instance.Find_num(i.ToString()).id.Equals(data.dropid))
            {
                isdrop = true;
                reward[num].Refresh(MonDropDB.Instance.Find_num(i.ToString()).itemid,
                    int.Parse(MonDropDB.Instance.Find_num(i.ToString()).minhowmany), false);
                reward[num].gameObject.SetActive(true);
                num++;
            }

            if (isdrop && !MonDropDB.Instance.Find_num(i.ToString()).id.Equals(data.dropid))
                break;
        }

    }

    public void Bt_ShowRaidInfo()
    {
        if(!islock)
        {
            RaidManager.Instance.Bt_SelectRaid(mapid );
            //발동
        }
    }


}
