using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPaneRig : MonoBehaviour
{
    [SerializeField] private ActionPane actionPaneTemplate;

    private RectTransform rectTransform;

    private int count = 0;
    private List<ActionPane> childPanes = new List<ActionPane>();

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void BuildPanes(List<DynamicAction> actions)
    {
        childPanes.ForEach(pane => Destroy(pane.gameObject));
        childPanes.Clear();
        count = 0;

        foreach (DynamicAction action in actions)
        {
            InstantiateActionPane(action);
        }
    }

    private void InstantiateActionPane(DynamicAction action)
    {
        ActionPane newPane = Instantiate(actionPaneTemplate, transform);
        RectTransform rect = newPane.GetComponent<RectTransform>();

        newPane.action = action;
        newPane.gameObject.SetActive(true);

        rect.anchoredPosition = new Vector2(0, -50 * count++);
        rectTransform.sizeDelta = new Vector2(0, 50 * count);

        childPanes.Add(newPane);
    }
}
