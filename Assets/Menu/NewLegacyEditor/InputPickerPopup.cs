using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPickerPopup : MonoBehaviour
{
    [SerializeField]
    private SubactionVarDataInput input;
    [SerializeField]
    private GameObject inputPickerDataPrefab;
    [SerializeField]
    private UIGrid grid;

    private List<string> items = new List<string>();

    public void OnInputSelected(string selectedInput)
    {
        input.OnAction(selectedInput);
    }

    public void generateItems()
    {
        foreach(string item in items)
        {
            GameObject go = NGUITools.AddChild(grid.gameObject, inputPickerDataPrefab);
            go.transform.localScale = inputPickerDataPrefab.transform.localScale; //Instantiating this through NGUI sets this to 1 for some reason
            InputPickerData data = go.GetComponent<InputPickerData>();
            data.SetLabel(item);
            data.popup = this;
        }
    }

    public void refreshGridAndLabels()
    {
        grid.Reposition();
        if (grid.transform.childCount > 0)
        {
            grid.transform.GetChild(0).GetComponent<LabelDepthUnfucker>().UnfuckLabelDepth();
        }
    }
    public List<string> getItems()
    {
        if (items == null)
        {
            items = new List<string>();
        }
        return items;
    }

    public void Dispose()
    {
        grid.transform.GetChild(0).GetComponent<LabelDepthUnfucker>().UnfuckLabelDepth();
        NGUITools.SetActive(gameObject, false);
    }
}
