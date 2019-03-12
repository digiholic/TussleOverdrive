using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileMenu : MonoBehaviour
{
    private UIPopupList list;

    private void Start()
    {
        list = GetComponent<UIPopupList>();
    }

    public void OnSelectionChange()
    {
        string selected = list.value;
        Debug.Log(selected);
        switch (selected)
        {
            case "Load":
                LegacyEditorData.instance.LoadFighterClicked();
                break;
            case "Save":
                LegacyEditorData.instance.SaveFighterClicked();
                break;
            default:
                break;
        }
    }
}
