using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPaneRig : MonoBehaviour
{
    [SerializeField] AnimationPane animationPaneTemplate;
    
    private List<AnimationPane> childPanes = new List<AnimationPane>();

    public void BuildPanes(List<AnimationDefinition> animations)
    {
        childPanes.ForEach(pane => Destroy(pane.gameObject));
        childPanes.Clear();
        
        foreach(AnimationDefinition anim in animations)
        {
            InstantiateAnimationPane(anim);
        }
    }

    private void InstantiateAnimationPane(AnimationDefinition animation)
    {
        AnimationPane newPane = Instantiate(animationPaneTemplate, transform);
        newPane.animation = animation;
        newPane.gameObject.SetActive(true);
        childPanes.Add(newPane);
    }
}
