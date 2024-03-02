using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gaugebarslot : MonoBehaviour
{
    public float cur;
    public float max;

    public decimal dcur;
    public decimal dmax;
    public Image bar;
    public Text Text;

    public void RefreshBar(float cur,float max)
    {
        this.cur = cur;
        this.max = max;
        bar.fillAmount = cur / max;
        RefreshText();
    }
    public void RefreshBarD(decimal cur,decimal max)
    {
    //    Debug.Log(cur);
//        Debug.Log(max);
        this.dcur = cur;
        this.dmax = max;
        bar.fillAmount = (float)(dcur / dmax);
        RefreshTextDecimal();
    }
    void RefreshText()
    {
        Text.text = $"{cur:N0}/{max:N0}";
    }

    private void RefreshTextDecimal()
    {
        decimal percent = (dcur / dmax) * 100m;
        Text.text = $"{dcur:N0}/{dmax:N0}<color=yellow>({percent:N0}%)</color>";
    }
}
