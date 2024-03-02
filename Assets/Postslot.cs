using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Postslot : MonoBehaviour
{
    UPostItem postdata;
    public Text Postname;

    public GameObject itemimage;
    public Text itemcount;
    public void Refresh(UPostItem data)
    {
        postdata = data;
        Postname.text = postdata.title;

        if(postdata.items.Count !=0)
        {
            itemimage.SetActive(true);
            itemcount.text = $"x{postdata.items.Count}";
        }
        else
        {
            itemimage.SetActive(false);
        }
    }

    public void Bt_ShowPost()
    {
        if(postdata.items.Count != 0)
        {
            PostManager.Instance.ShowPostItem(postdata);
        }
        else
        {
            PostManager.Instance.ShowPostNoItem(postdata);
        }
    }
}
