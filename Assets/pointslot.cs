using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pointslot : MonoBehaviour
{
    public GameObject Pointobj;
    public GameObject PointFinish;
    public Image StageobjImage;
    public Color normal_color;
    public Color active_color;
    public Color finish_color;
    public void ShowPoint()
    {
        Pointobj.SetActive(true);
        PointFinish.SetActive(false);
        StageobjImage.color = active_color;
    }

    public void ClosePoint()
    {
        Pointobj.SetActive(false);
        PointFinish.SetActive(false);
        StageobjImage.color = normal_color;
    }

    public void FinishPoint()
    {
        Pointobj.SetActive(false);
        PointFinish.SetActive(true);
        StageobjImage.color = finish_color;
    }
    // Start is called before the first frame update
}
