using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class internetcheck : MonoBehaviour
{
    public static bool isinternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
