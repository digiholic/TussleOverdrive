using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulatePopupWithVarSource : MonoBehaviour
{
    [SerializeField]
    private SubactionVarDataPanel panel;
    [SerializeField]
    private InputPickerPopup popup;

    private List<VarData> fighterList = new List<VarData>();
    private List<VarData> actionList = new List<VarData>();

    //TODO change this to use a listener so it behaves like everyting else does
    private SubactionSource lastSource;

    private void OnModelChanged()
    {
        if (LegacyEditorData.instance.loadedFighterDirty)
        {
            fighterList = LegacyEditorData.instance.loadedFighter.variables;
        }
        if (LegacyEditorData.instance.currentActionDirty)
        {
            //TODO add variables to action
            actionList = new List<VarData>();
        }
    }
    void Update()
    {
        if (panel.varData.source != lastSource)
        {
            lastSource = panel.varData.source;
            switch (panel.varData.source)
            {
                case SubactionSource.OWNER:
                    GenerateVarList(fighterList);
                    break;
                case SubactionSource.ACTION:
                    break;
                default:
                    break;
            }
        }
    }

    private void GenerateVarList(List<VarData> varDataList)
    {
        if (popup == null) return; //This can sometimes get called before Awake. Somehow.
        popup.getItems().Clear();
        foreach (VarData vardata in varDataList)
        {
            popup.getItems().Add(vardata.name);
        }
        popup.generateItems();
    }
}
