using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPickerPopup : MonoBehaviour
{
    [SerializeField]
    private UIInput input;
    [SerializeField]
    private GameObject inputPickerDataPrefab;
    [SerializeField]
    private UIGrid grid;

    private List<string> items = new List<string>();

    public void OnInputSelected(string selectedInput)
    {
        //Doing a bit of dark voodoo magic to call this private method in a library without modifying the library
        //This will let us do everything the UIInput does as if we typed it in
        input.value = "";
        input.SendMessage("Append", selectedInput + '\n');
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
            grid.BroadcastMessage("UnfuckLabelDepth",SendMessageOptions.DontRequireReceiver);
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
        grid.BroadcastMessage("RefuckLabelDepth",SendMessageOptions.DontRequireReceiver);
        NGUITools.SetActive(gameObject, false);
    }
}
