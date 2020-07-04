using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoolDataBind : MonoBehaviour
{
    public BoolCallback source;
    public BoolEvent target;

    private bool cachedResult;

    void Update()
    {
        bool data = source.Invoke();
        if (data != cachedResult)
        {
            target.Invoke(data);
            cachedResult = data;
        }
    }
}

[System.Serializable]
public class BoolCallback : SerializableCallback<bool> { }

[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }