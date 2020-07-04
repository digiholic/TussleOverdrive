using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationListDataBind : MonoBehaviour
{
    public AnimationListCallback source;
    public AnimationListEvent target;

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
public class AnimationListCallback : SerializableCallback<List<AnimationDefinition>> { }

[System.Serializable]
public class AnimationListEvent : UnityEvent<List<AnimationDefinition>> { }