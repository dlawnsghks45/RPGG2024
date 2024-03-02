using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private void OnDestroy()
    {
        _instance = null;
    }


    //싱글톤만들기.
    private static ObjectPoolManager _instance = null;
    public static ObjectPoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(ObjectPoolManager)) as ObjectPoolManager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    public Arrow arrowobj; //투사체오브젝트
    public Arrow aniarrowobj; //투사체오브젝트

    public Transform ArrowPoolTrans;
}
