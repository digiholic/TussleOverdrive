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
