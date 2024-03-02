using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildBuffSlot : MonoBehaviour
{
    [SerializeField]
    private bool ismine;
    public string id;
    public int lv = 1;
    public Image GuildBuffImage;
    public Text GuildBuffName;
    public Text GuildBuffLv;

    public void Start()
    {
        GuildBuffImage.sprite = SpriteManager.Instance.GetSprite(GuildBuffDB.Instance.Find_id(id).sprite);
        GuildBuffName.text = Inventory.GetTranslate(GuildBuffDB.Instance.Find_id(id).name);
    }

    public void SetGuildBuff(int level)
    {
        this.lv = level;
        GuildBuffLv.text = $"Lv.{this.lv}";
    }

    public void ShowGuildBuff()
    {
        MyGuildManager.Instance.Bt_ShowGuildBuff(id,lv.ToString(),ismine);
    }
}
