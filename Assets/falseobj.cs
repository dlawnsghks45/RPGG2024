using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class falseobj : MonoBehaviour
{
    public float num;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Invoke("falseobjs",num);
    }

    void falseobjs()
    {
        gameObject.SetActive(false);
    }
}
