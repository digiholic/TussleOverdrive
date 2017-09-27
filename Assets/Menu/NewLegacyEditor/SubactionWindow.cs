using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionWindow : MonoBehaviour {
    public GameObject data_row_prefab;

    private UIGrid grid;
    private List<SubactionDataRow> subaction_rows = new List<SubactionDataRow>();

    void Start()
    {
        grid = GetComponent<UIGrid>();
    }

    //Gets called by message from ActionSelectorDataRow
    public void ChangeSelectedAction(DynamicAction action)
    {
        RemoveData();
        if (action.set_up_actions.subactions.Count > 0)
        {
            foreach (string action_text in action.set_up_actions.subactions)
            {
                Debug.Log(action_text);
                InstantiateRow(action_text);
            }
            subaction_rows[0].Select();
        }
        grid.Reposition();
    }

    void RemoveData()
    {
        foreach (SubactionDataRow action_row in subaction_rows)
        {
            NGUITools.Destroy(action_row.gameObject);
        }
        subaction_rows = new List<SubactionDataRow>();
    }

    private SubactionDataRow InstantiateRow(string action_text)
    {
        GameObject go = NGUITools.AddChild(gameObject, data_row_prefab);
        SubactionDataRow data = go.GetComponent<SubactionDataRow>();
        //data.subaction = 
        data.SetText(action_text);
        subaction_rows.Add(data);
        return data;
    }
}
