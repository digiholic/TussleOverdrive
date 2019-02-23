using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulatePopupWithVarSource : PopulatePopup
{
    [SerializeField]
    protected SubactionVarDataPanel panel;

    public List<string> fighterList = new List<string>();
    public List<string> actionList = new List<string>();

    //TODO change this to use a listener so it behaves like everyting else does
    private SubactionSource lastSource;

    private void Start()
    {
        if (LegacyEditorData.instance.loadedFighter != null)
        {
            fighterList = LegacyEditorData.instance.loadedFighter.variables.Select(varData => varData.name).ToList();
        }
    }

    private void OnModelChanged()
    {
        if (LegacyEditorData.instance.loadedFighterDirty)
        {
            fighterList = LegacyEditorData.instance.loadedFighter.variables.Select(varData => varData.name).ToList();
        }
        if (LegacyEditorData.instance.currentActionDirty)
        {
            //TODO add variables to action
            actionList = new List<string>();
        }
    }
    void Update()
    {
        if (popup == null) return; //This can sometimes get called before Awake. Somehow.

        if (panel.varData.source != lastSource)
        {
            lastSource = panel.varData.source;
            switch (panel.varData.source)
            {
                case SubactionSource.OWNER:
                    PopulateList(fighterList);
                    break;
                case SubactionSource.ACTION:
                    break;
                default:
                    break;
            }
        }
    }
}
