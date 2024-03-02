using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeui : MonoBehaviour
{
    public enum AnimType
    {
        Fade,
        Scale,
    }

    public AnimType AnimationType = AnimType.Fade;
    public bool CloseDisable = true;

    CanvasGroup _CanvasGroup;
    CanvasGroup CanvasGroup
    {
        get
        {
            if (_CanvasGroup == null)
                _CanvasGroup = GetComponent<CanvasGroup>();

            return _CanvasGroup;
        }
        set
        {
            _CanvasGroup = value;
        }

    }

    void OnEnable()
    {
        if (AnimationType == AnimType.Fade)
        {
            CanvasGroup.alpha = 0f;
            CanvasGroup.DOFade(1, 0.3f)
                .OnComplete(OnOpenComplete)
                .SetUpdate(true);
        }
        else if (AnimationType == AnimType.Scale)
        {
            transform.localScale = new Vector3(0f, 0f, 0f);
            transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f)
                .SetEase(Ease.OutBack)
                .OnComplete(OnOpenComplete)
                .SetUpdate(true);
        }
    }


    private void OnDisable()
    {
        transform.DOKill();
        CanvasGroup.DOKill();
    }

    void OnOpenComplete()
    {
        Invoke(nameof(Close), closetime);
    }

    public void Close()
    {
        if (AnimationType == AnimType.Fade)
        {
            CanvasGroup.alpha = 1f;
            CanvasGroup.DOFade(0, 0.2f)
               .OnComplete(OnCloseComplete)
                .SetUpdate(true);
        }
        else if (AnimationType == AnimType.Scale)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f)
               .SetEase(Ease.InBack)
               .OnComplete(OnCloseComplete)
               .SetUpdate(true);
        }
    }

    void OnCloseComplete()
    {
        if (CloseDisable)
            close();

    }
   public float closetime = 3;
    void close()
    {
        gameObject.SetActive(false);
    }
}
