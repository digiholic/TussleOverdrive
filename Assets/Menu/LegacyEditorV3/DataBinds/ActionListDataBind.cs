using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionListDataBind : MonoBehaviour
{
    public ActionListCallback source;
    public ActionListEvent target;

    private int cachedResultsCount;

    void Update()
    {
        List<DynamicAction> data = source.Invoke();
        if (data.Count != cachedResultsCount)
        {
            target.Invoke(data);
            cachedResultsCount = data.Count;
        }
    }
}

[System.Serializable]
public class ActionListCallback : SerializableCallback<List<DynamicAction>> { }

[System.Serializable]
public class ActionListEvent : UnityEvent<List<DynamicAction>> { }