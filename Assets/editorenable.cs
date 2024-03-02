using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class editorenable : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        #if  UNITY_EDITOR
            this.gameObject.SetActive(true);
        #else
           this.gameObject.SetActive(false);
        
        #endif
    }

}
