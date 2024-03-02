using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBackground : MonoBehaviour
{
    public Animator maptag;
    public SpriteRenderer rsp;
    Sequence mySequence;

    private static readonly int Show = Animator.StringToHash("Show");
    // Start is called before the first frame update
 
    [Button (Name ="¤·¤·")]
    public void Fade()
    {
        maptag.SetTrigger(Show);
        mySequence = DOTween.Sequence();
        mySequence.AppendInterval(0.7f).Append(rsp.DOColor(Color.black, 0.5f))
            .AppendInterval(1f).Append(rsp.DOFade(0f, 2f));
        mySequence.Play();
    }
    
    public void Fade2()
    {
        mySequence = DOTween.Sequence();
        mySequence.Append(rsp.DOColor(Color.black, 0.1f))
            .AppendInterval(0.3f).Append(rsp.DOFade(0f, 0.7f));
        mySequence.Play();
    }
}
