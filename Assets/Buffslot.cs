using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buffslot : MonoBehaviour
{
    public Image BuffImage;
    public bool isbuff;
    public void SetBuff(Sprite buffsprite)
    {
        BuffImage.sprite = buffsprite;
        isbuff = true;
    }

    public void FinishBuff()
    {
        isbuff = false;
        gameObject.SetActive(false);
    }
}
