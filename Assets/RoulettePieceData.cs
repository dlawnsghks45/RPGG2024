using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoulettePieceData : MonoBehaviour
{
    public string id;
    public int hw;
    public string rare;

    [Range(1, 100)] public int chance;

    [HideInInspector] public int index;
    [HideInInspector] public int weight;
}
