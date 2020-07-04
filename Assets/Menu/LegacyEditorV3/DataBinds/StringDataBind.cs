using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StringDataBind : MonoBehaviour
{
    public StringCallback source;
    public StringEvent target;

    private string cachedResult;

    void Update()
    {
        string data = source.Invoke();
        if (data != cachedResult)
        {
            target.Invoke(data);
            cachedResult = data;
        }
    }
}

[System.Serializable]
public class StringCallback : SerializableCallback<string> { }

[System.Serializable]
public class StringEvent : UnityEvent<string> { }