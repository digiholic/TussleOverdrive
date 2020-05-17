using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionButtonRig : MonoBehaviour
{
    public SubactionButton subactionButtonTemplate;
    private RectTransform rectTransform;

    private int count = 0;
    private List<SubactionButton> childButtons = new List<SubactionButton>();

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void BuildPanes(List<SubactionData> subactions)
    {
        childButtons.ForEach(button => Destroy(button.gameObject));
        childButtons.Clear();
        count = 0;

        foreach (SubactionData subaction in subactions)
        {
            InstantiateSubactionButton(subaction);
        }
    }

    private void InstantiateSubactionButton(SubactionData subaction)
    {
        SubactionButton newButton = Instantiate(subactionButtonTemplate, transform);
        RectTransform rect = newButton.GetComponent<RectTransform>();

        newButton.subaction = subaction;
        newButton.gameObject.SetActive(true);

        rect.anchoredPosition = new Vector2(0, -60 * count++);
        rectTransform.sizeDelta = new Vector2(0, 60 * count);

        childButtons.Add(newButton);
    }

}
