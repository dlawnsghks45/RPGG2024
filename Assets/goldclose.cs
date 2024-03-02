using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldclose : MonoBehaviour
{
    public Animator ani;
    bool isclose;
    private static readonly int Show = Animator.StringToHash("Show");

    public void Bt_SetGoldClose()
    {
        if(isclose)
        {
            isclose = false;
            ani.SetBool(Show,isclose);
        }
        else
        {
            isclose = true;
            ani.SetBool(Show,isclose);
        }
    }
}
