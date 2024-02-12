using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get => instance;
        set
        {
            T test = value;
            if (instance != null)
            {
                Destroy(test);
                return;
            }

            instance = value;
           
        }
    }
}
