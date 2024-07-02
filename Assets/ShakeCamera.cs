using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public float A = 1.0f;
    public float B = 1.0f;


    public void ShakeCameras()
    {
        if (SettingReNewal.Instance.CameraShake[0].IsOn)
        {
            Camera.main.DOShakeRotation(A, B, fadeOut: true);
            Camera.main.DOShakePosition(A, B, fadeOut: true);
        }
     
    }
}
