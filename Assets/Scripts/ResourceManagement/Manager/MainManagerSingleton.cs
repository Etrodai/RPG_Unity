using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManagerSingleton : MonoBehaviour
{
    private static MainManagerSingleton instance;
    public static MainManagerSingleton Instance
    {
        get => instance;
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
