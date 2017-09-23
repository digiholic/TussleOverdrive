using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelector : MonoBehaviour {
    public GameObject data_row_prefab;

    private UIGrid grid;
    private List<ActionSelectorDataRow> action_rows = new List<ActionSelectorDataRow>();

    void OnEnable()
    {
        grid = GetComponent<UIGrid>();
        if (LegacyEditor.editor.current_actions != null)
        {
            RefreshActions(LegacyEditor.editor.current_actions);
        }
    }

    void RemoveData()
    {
        foreach(ActionSelectorDataRow action_row in action_rows)
        {
            NGUITools.Destroy(action_row);
        }
    }

    void RefreshActions(ActionFile actions)
    {
        foreach(DynamicAction action in actions.actions)
        {
            InstantiateRow(action);
        }
        action_rows[0].Select();
        grid.Reposition();
    }

    private ActionSelectorDataRow InstantiateRow(DynamicAction action)
    {
        GameObject go = NGUITools.AddChild(gameObject, data_row_prefab);
        ActionSelectorDataRow data = go.GetComponent<ActionSelectorDataRow>();
        data.action = action;
        data.SetText(action.name);
        action_rows.Add(data);
        return data;
    }
}