using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class presetskillslot : MonoBehaviour
{
    public string skillid;
    public Image Skillimage;


    public void Refresh()
    {
        Skillimage.sprite = SpriteManager.Instance.GetSprite(SkillDB.Instance.Find_Id(skillid).Sprite);
    }
}
