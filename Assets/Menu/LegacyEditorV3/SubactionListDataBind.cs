using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SubactionListDataBind : MonoBehaviour
{
    public SubactionListCallback source;
    public SubactionListEvent target;

    private int cachedResultsCount;
    private List<SubactionData> cachedList;

    void Update()
    {
        List<SubactionData> data = source.Invoke();
        if (data != cachedList || data.Count != cachedResultsCount)
        {
            target.Invoke(data);
            cachedResultsCount = data.Count;
            cachedList = data;
        }
    }
}

[System.Serializable]
public class SubactionListCallback : SerializableCallback<List<SubactionData>> { }

[System.Serializable]
public class SubactionListEvent : UnityEvent<List<SubactionData>> { }