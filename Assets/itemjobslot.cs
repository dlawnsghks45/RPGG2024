using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemjobslot : MonoBehaviour
{
    public string id;
    [SerializeField]
    Text text;

    public void Refresh()
    {
        text.text = $"x{PlayerBackendData.Instance.CheckItemCount(id)}";
    }


    private void OnEnable()
    {
        Refresh();
    }
}
