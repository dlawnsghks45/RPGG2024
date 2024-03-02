using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arrowpattern : MonoBehaviour
{
    public Image ArrowImage;
    public Sprite[] ArrowImagesprite;

    public int nowside;

    public void SetSide(int num)
    {
        nowside = num;
        ArrowImage.sprite = ArrowImagesprite[num];
    }
    public void Bt_Side()
    {
        
    }


}

enum arrowside
{
    left,
    up,
    right,
    down,
    length
}
