using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textsc : MonoBehaviour
{
    Sequence mySequence;

    void Start()
    {
        mySequence = DOTween.Sequence()
        .SetAutoKill(false) //Ãß°¡
        .OnStart(() => {
            transform.localScale = Vector3.zero;
            GetComponent<CanvasGroup>().alpha = 0;
        })
        .Append(transform.DOScale(1, 1).SetEase(Ease.Flash))
        .Join(GetComponent<CanvasGroup>().DOFade(1, 1))
        .SetDelay(0.1f);
    }

    private void OnEnable()
    {
        mySequence.Restart();
    }
}
