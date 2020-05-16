using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationPane : MonoBehaviour
{
    public AnimationDefinition animation;

    [SerializeField] private string _animationName;
    [SerializeField] private bool _loop;
    [SerializeField] private string _startIndex;
    [SerializeField] private string _endIndex;

    public string AnimationName { get => _animationName;
        set
        {
            _animationName = value;
            animation.animationName = value;
        }
    }
    public bool Loop { get => _loop;
        set
        {
            _loop = value;
            animation.loop = value;
        }
    }
    public string StartIndex { get => _startIndex;
        set
        {
            _startIndex = value;
            animation.startIndex = int.Parse(value);
        }
    }
    public string EndIndex { get => _endIndex;
        set
        {
            _endIndex = value;
            animation.endIndex = int.Parse(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _animationName = animation.animationName;
        _startIndex = animation.startIndex.ToString();
        _endIndex = animation.endIndex.ToString();
        _loop = animation.loop;
    }
}
