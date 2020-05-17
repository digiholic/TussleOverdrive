using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationListDataBind : MonoBehaviour
{
    public ObjectListCallback source;
    public ObjectListEvent target;

    private int cachedResultsCount;

    void Update()
    {
        List<AnimationDefinition> data = source.Invoke();
        if (data.Count != cachedResultsCount)
        {
            target.Invoke(data);
            cachedResultsCount = data.Count;
        }
    }
}

[System.Serializable]
public class ObjectListCallback : SerializableCallback<List<AnimationDefinition>> { }

[System.Serializable]
public class ObjectListEvent : UnityEvent<List<AnimationDefinition>> { }