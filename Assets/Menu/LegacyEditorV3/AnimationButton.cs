using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;

    public AnimationDefinition animation;

    [SerializeField] private string animationName;
    [SerializeField] private bool loop;
    [SerializeField] private string startIndex;
    [SerializeField] private string endIndex;

    public string AnimationName { get => animationName;
        set
        {
            animationName = value;
            animation.animationName = value;
        }
    }
    public bool Loop { get => loop;
        set
        {
            loop = value;
            animation.loop = value;
        }
    }
    public string StartIndex { get => startIndex;
        set
        {
            startIndex = value;
            animation.startIndex = int.Parse(value);
        }
    }
    public string EndIndex { get => endIndex;
        set
        {
            endIndex = value;
            animation.endIndex = int.Parse(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        text.text = animation.animationName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
