using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class backgroundchange : MonoBehaviour
{
    public Sprite[] Background;
    public Image Back;
    public int nowback;

    public void Start()
    {
        InvokeRepeating("Changeback",5f,5f);
    }

    void Changeback()
    {
        int rn =   Random.Range(0, Background.Length);
        while (rn == nowback)
        {
            rn = Random.Range(0, Background.Length);
        }
        Back.sprite = Background[rn];
    }
}
