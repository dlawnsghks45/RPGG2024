using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TranslateManager : MonoBehaviour
{
    private void OnDestroy()
    {
        _instance = null;
    }

    //ΩÃ±€≈Ê∏∏µÈ±‚.
    private static TranslateManager _instance = null;
    public static TranslateManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType(typeof(TranslateManager)) as TranslateManager;

            if (_instance == null)
            {
                //Debug.Log("Player script Error");
            }
            return _instance;
        }
    }


    public string GetTranslate(string path)
    {
        return I2.Loc.LocalizationManager.GetTermTranslation(path);
    }

}
