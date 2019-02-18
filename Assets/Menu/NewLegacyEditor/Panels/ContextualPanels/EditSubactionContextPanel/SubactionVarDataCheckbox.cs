using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionVarDataCheckbox : MonoBehaviour
{
    [SerializeField]
    private SubactionVarDataPanel panel;
    private UICheckbox checkbox;

    // Use this for initialization
    void Awake()
    {
        checkbox = GetComponent<UICheckbox>();

        //The type doesn't change during runtime, so we can just set it here
        checkbox.isChecked = (panel.varData.data == "true");
        checkbox.eventReceiver = gameObject;
    }

    private void OnModelChanged()
    {
        if (LegacyEditorData.instance.currentSubactionDirty)
        {
            checkbox.isChecked = (panel.varData.data == "true");
        }
    }

    void OnAction()
    {
        //Convert this to a LegacyAction
        if (panel.varData.type == SubactionVarType.BOOL && panel.varData.source == SubactionSource.CONSTANT)
        {
            ChangeSubactionVarDataInput legacyAction = ScriptableObject.CreateInstance<ChangeSubactionVarDataInput>();
            legacyAction.init(panel.varData, checkbox.isChecked.ToString().ToLower());
            LegacyEditorData.instance.DoAction(legacyAction);
        }
    }
}
