using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileMenu : MonoBehaviour
{
    void OnSelectionChange(string selected)
    {
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
