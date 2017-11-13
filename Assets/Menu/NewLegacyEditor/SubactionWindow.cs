using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionWindow : MonoBehaviour {
    public GameObject data_row_prefab;

    private DynamicGridLayout grid;
    private List<SubactionDataRowExpanded> subaction_rows = new List<SubactionDataRowExpanded>();

    void Start()
    {
        grid = GetComponent<DynamicGridLayout>();
    }

    public void ActionChanged(DynamicAction action)
    {
        RemoveData();
        if (action.set_up_subactions.subactions.Count > 0)
        {
            foreach (Subaction action_text in action.set_up_subactions.subactions)
            {
                Debug.Log(action_text);
                InstantiateRow(action_text);
            }
            //subaction_rows[0].Select();
        }
        grid.Reposition(); //FIXME: For some reason, this doesn't actually reposition it in time. It's still stacked on top of each other.
    }

    void RemoveData()
    {
        foreach (SubactionDataRowExpanded action_row in subaction_rows)
        {
            NGUITools.Destroy(action_row.gameObject);
        }
        grid.ClearData();
        subaction_rows.Clear();
    }

    private SubactionDataRowExpanded InstantiateRow(Subaction action_text)
    {
        GameObject go = NGUITools.AddChild(gameObject, data_row_prefab);
        SubactionDataRowExpanded data = go.GetComponent<SubactionDataRowExpanded>();
        data.subaction = action_text;
        subaction_rows.Add(data);
        grid.AddData(data.GetComponent<DynamicGridCell>());
        return data;
    }
}
