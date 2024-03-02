using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    //ΩÃ±€≈Ê∏∏µÈ±‚.
    private static ItemDropManager _instance = null;
    public static ItemDropManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(ItemDropManager)) as ItemDropManager;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    public FadeBackground fade;
    public itemspritedropslot[] dropslots;

    private int num = 0;
    // Update is called once per frame
    public void SpawnItem(string itemspritepath, bool israre, Vector3 startpos)
    {
        if(!SettingReNewal.Instance.ItemDrop[0].IsOn)
            return;
        
        if (num.Equals(dropslots.Length))
            num = 0;

        Vector3 endpos = new Vector3(startpos.x +Random.Range(-1,1f),startpos.y + Random.Range(-0.5f,0.5f), 0);
        dropslots[num].Setitem(SpriteManager.Instance.GetSprite(itemspritepath),israre, endpos, startpos);
        dropslots[num].gameObject.SetActive(true);
        num++;
    }
}
