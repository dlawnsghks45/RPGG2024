using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoulettePiece : MonoBehaviour
{
    [SerializeField] private Image iconimage;

    [SerializeField] private Text nametext;
    [SerializeField] private Image RareImage;
    private string id;
    public void SetUp(RoulettePieceData dataa)
    {
        id = dataa.id;
        iconimage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(dataa.id).sprite);
        nametext.text = dataa.hw.ToString("N0");
        switch (dataa.rare)
        {
            case "0":
                RareImage.enabled = false;
                break;
            case "1":
                RareImage.enabled = true;
                RareImage.color = Color.gray;
                break;
            case "2":
                RareImage.enabled = true;
                RareImage.color = Color.yellow;
                break;
            case "3":
                RareImage.enabled = true;
                RareImage.color = Color.magenta;
                break;
        }
    }

    public void Bt_ShowItem()
    {
        Inventory.Instance.ShowInventoryItem_NoMine(id);
    }
}

