using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemneedslot : MonoBehaviour
{
    //필요 아이템의 개수와 이미지를 나타낸다.

    public Image sprite;
    public Text Needcount;

    public string id;
    public int count;

    [SerializeField] private RectTransform rect;
    // Start is called before the first frame update

    public void Refresh(string id, int count)
    {
        this.id = id;
        this.count = count;
        
        sprite.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(id).sprite);
        Needcount.text = $"{count:N0}";

        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }
    
    
    
}
