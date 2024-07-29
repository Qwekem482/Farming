using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehavior<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null) {
            Instance = this as T;
            Debug.Log("SingletonCreate: " + Instance);
        }
        else Destroy(gameObject);
    }
}