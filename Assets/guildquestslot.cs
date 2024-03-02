using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class guildquestslot : MonoBehaviour
{
    public Image ItemImage;
    GuildQuestDB.Row data;
    public Text GuildQuestName;
    public Text GuildQuestInfo;


    public void Refresh(string id)
    {
        data = GuildQuestDB.Instance.Find_id(id);
        ItemImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(data.itemid).sprite);
        GuildQuestName.text = Inventory.GetTranslate(data.name);
        GuildQuestInfo.text = Inventory.GetTranslate(data.info);
    }

    public void Bt_ShowGuildQuest()
    {
        MyGuildQuestManager.Instance.Bt_ShowGuildQuestPanel(data.id);
    }
}
