using UnityEngine;
using UnityEngine.UI;

public class itemshowcountslot : MonoBehaviour
{
    //해당 아이디에 개수를 보여주는 것이다.
    [SerializeField] private RectTransform panel;
    public string id;
    public Image ItemImage;
    public Text ItemName;
    // Start is called before the first frame update
    [SerializeField]
    private bool isnameOn;
    private void Start()
    {
        if (isnameOn) return;
        InitData();
        SetData();
    }

    private void LateUpdate()
    {
        if (isnameOn) return;
        if (!gameObject.activeSelf) return;
        SetData();
    }

    public void SetData(string itemid , decimal count,bool isnameon)
    {
        isnameOn = true;
        id = itemid;

        if (isnameon)
        {
            ItemdatabasecsvDB.Row itemdb = ItemdatabasecsvDB.Instance.Find_id(itemid);
            ItemName.text = $"<color={Inventory.Instance.GetRareColorFF(itemdb.rare)}>{Inventory.GetTranslate(itemdb.name)}</color> X {count:N0} <color=cyan>({PlayerBackendData.Instance.CheckItemCount(itemid)}</color>)";
        }
        else
        {
            ItemName.text = count.ToString("N0");
        }

        InitData();
    }

    private bool isinit = false;
    public void InitData()
    {
       // if(ItemImage.sprite == null)
        ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(id).sprite);
    }
    public void SetData()
    {
        if(ItemImage.sprite == null)
            ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(id).sprite);
       // InitData();
        switch (id)
        {
            case "1000":
                ItemName.text = PlayerData.Instance.PlayerGold.text;
                break;
            case "1001":
                ItemName.text = PlayerData.Instance.PlayerCash.text;
                break;
            default:
                int curcount = PlayerBackendData.Instance.CheckItemCount(id);
                ItemName.text = curcount.ToString("N0");
                break;
        }

      //  LayoutRebuilder.ForceRebuildLayoutImmediate(panel);
    }

    
    private void OnEnable()
    {
        SetData();
    }

    public void Bt_ShowInfo()
    {
        Inventory.Instance.ShowInventoryItem_NoMine(id);
    }
}