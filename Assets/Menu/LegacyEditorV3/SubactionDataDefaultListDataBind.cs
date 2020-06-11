using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SubactionDataDefaultListDataBind : MonoBehaviour
{
    public SubactionDataDefaultListCallback source;
    public SubactionDataDefaultListEvent target;

    private int cachedResultsCount;
    private List<SubactionDataDefault> cachedList;

    void Update()
    {
        List<SubactionDataDefault> data = source.Invoke();
        if (data != cachedList || data.Count != cachedResultsCount)
        {
            target.Invoke(data);
            cachedResultsCount = data.Count;
            cachedList = data;
        }
    }
}

[System.Serializable]
public class SubactionDataDefaultListCallback : SerializableCallback<List<SubactionDataDefault>> { }

[System.Serializable]
public class SubactionDataDefaultListEvent : UnityEvent<List<SubactionDataDefault>> { }