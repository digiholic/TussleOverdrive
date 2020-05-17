using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPaneRig : MonoBehaviour
{
    [SerializeField] AnimationPane animationPaneTemplate;
    private RectTransform rectTransform;

    private int count = 0;
    private List<AnimationPane> childPanes = new List<AnimationPane>();

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void BuildPanes(List<AnimationDefinition> animations)
    {
        childPanes.ForEach(pane => Destroy(pane.gameObject));
        childPanes.Clear();
        count = 0;

        foreach(AnimationDefinition anim in animations)
        {
            InstantiateAnimationPane(anim);
        }
    }

    private void InstantiateAnimationPane(AnimationDefinition animation)
    {
        AnimationPane newPane = Instantiate(animationPaneTemplate, transform);
        RectTransform rect = newPane.GetComponent<RectTransform>();

        newPane.animation = animation;
        newPane.gameObject.SetActive(true);

        rect.anchoredPosition = new Vector2(0, -130 * count++);
        rectTransform.sizeDelta = new Vector2(0, 130 * count);

        childPanes.Add(newPane);
    }
}
