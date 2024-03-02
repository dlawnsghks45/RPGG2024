using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checklvtoggle : MonoBehaviour
{
    public Toggle toggle;
    public GameObject Lockobj;
    private void Start()
    {
        Refresh();
    }

    private void Refresh()
    {
        toggle.interactable = false;
        Lockobj.SetActive(true);
    }
}
