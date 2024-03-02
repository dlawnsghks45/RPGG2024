using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Color = System.Drawing.Color;

public class EffectSlots : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Animator ani;
    public bool isactive =false;

    public void FalseEffect()
    {
        isactive = false;
    }

    public void SetEffectColor(float colora)
    {
        sprite.color = new UnityEngine.Color(1, 1, 1, colora);

    }
}
