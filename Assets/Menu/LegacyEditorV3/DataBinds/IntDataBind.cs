using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntDataBind : MonoBehaviour
{
    public IntCallback source;
    public IntEvent target;

    private int cachedResult;

    void Update()
    {
        int data = source.Invoke();
        if (data != cachedResult)
        {
            target.Invoke(data);
            cachedResult = data;
        }
    }
}


[System.Serializable]
public class IntEvent : UnityEvent<int> { }

[System.Serializable]
public class IntCallback : SerializableCallback<int> { }