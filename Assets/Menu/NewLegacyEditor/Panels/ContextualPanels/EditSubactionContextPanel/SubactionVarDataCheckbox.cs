using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionVarDataCheckbox : LegacyEditorWidget
{
    [SerializeField]
    private SubactionVarDataPanel panel;
    private UIToggle checkbox;

    // Use this for initialization
    void Awake()
    {
        checkbox = GetComponent<UIToggle>();

        //The type doesn't change during runtime, so we can just set it here
        checkbox.value = (panel.varData.data == "true");
        //EventDelegate.Set(checkbox.onChange, OnAction); ^^
    }

    void OnSubactionChanged(SubactionData data)
    {
        checkbox.value = (panel.varData.data == "true");
    }

    public void OnAction()
    {
        //Convert this to a LegacyAction
        if (panel.varData.type == SubactionVarType.BOOL && panel.varData.source == SubactionSource.CONSTANT)
        {
            ChangeSubactionVarDataInput legacyAction = ScriptableObject.CreateInstance<ChangeSubactionVarDataInput>();
            legacyAction.init(panel.varData, checkbox.value.ToString().ToLower());
            LegacyEditorData.instance.DoAction(legacyAction);
        }
    }

    public override void RegisterListeners()
    {
        editor.CurrentSubactionChangedEvent += OnSubactionChanged;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentSubactionChangedEvent -= OnSubactionChanged;
    }
}
