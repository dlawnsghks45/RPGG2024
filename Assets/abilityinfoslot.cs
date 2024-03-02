using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class abilityinfoslot : MonoBehaviour
{
    public string abilityid;

    public Image sprite;
    public UIButton buttons;
    public void Refresh(string id)
    {
        abilityid = id;
        sprite.color = Color.white;
        sprite.sprite =
            SpriteManager.Instance.GetSprite(AbilityDBDB.Instance
                .Find_id(abilityid)
                .sprite);
        buttons.Interactable = true;
    }

    public void NoRefresh()
    {
        sprite.color = Color.black;
        buttons.Interactable = false;
    }
    public void Bt_ShowAbility()
    {
        abilitymanager.Instance.ShowAbilityOther(abilityid);
    }
}
