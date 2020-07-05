using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPaneRig : MonoBehaviour
{
    [SerializeField] private ActionPane actionPaneTemplate;
    private List<ActionPane> childPanes = new List<ActionPane>();

    public void BuildPanes(List<DynamicAction> actions)
    {
        childPanes.ForEach(pane => Destroy(pane.gameObject));
        childPanes.Clear();

        foreach (DynamicAction action in actions)
        {
            InstantiateActionPane(action);
        }
    }

    private void InstantiateActionPane(DynamicAction action)
    {
        ActionPane newPane = Instantiate(actionPaneTemplate, transform);
        newPane.action = action;
        newPane.gameObject.SetActive(true);
        childPanes.Add(newPane);
    }
}
