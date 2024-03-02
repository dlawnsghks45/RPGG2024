using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemdropslot : MonoBehaviour
{
    public Image Rare;
    public Image ItemImage;
    public Text ItemName;
    public Animator ani;
    private static readonly int Drop = Animator.StringToHash("drop");

    public void SetDrop(string id,int count)
    {
        ItemdatabasecsvDB.Row itemdata = ItemdatabasecsvDB.Instance.Find_id(id);
        ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(id).sprite);

        switch (id)
        {
            case "1000": //°ñµå
                if (PlayerData.Instance.mainplayer.Stat_ExtraGold != 0)
                {
                    ItemName.text = $"{Inventory.GetTranslate(itemdata.name)} x {count:N0}(+<color=yellow>{(PlayerData.Instance.GetGold(count)-count):N0}</color>)";
                }
                else
                {
                    ItemName.text = $"{Inventory.GetTranslate(itemdata.name)} x {count:N0}";
                }
                break;
            case "1002": //°æÇèÄ¡
                if (PlayerBackendData.Instance.ispremium || PlayerData.Instance.mainplayer.Stat_ExtraExp != 0)
                {
//                    Debug.Log(PlayerData.Instance.mainplayer.Stat_ExtraExp);
                    ItemName.text = $"{Inventory.GetTranslate(itemdata.name)} x {count:N0}(<color=cyan>+{(PlayerData.Instance.GetExp(count)-count):N0}</color>)";
                }
                else
                {
                    ItemName.text = $"{Inventory.GetTranslate(itemdata.name)} x {count:N0}";
                }
                break;
            default:
                ItemName.text = $"{Inventory.GetTranslate(itemdata.name)} x {count:N0}";
                break;
        }        
        
        Rare.color = Inventory.Instance.GetRareColor(itemdata.rare);
        Inventory.Instance.ChangeItemRareColor(ItemName,itemdata.rare);
        ani.SetTrigger(Drop);
    }                                                   
}
