using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionVarDataInput : LegacyEditorWidget {
    [SerializeField]
    private SubactionVarDataPanel panel;
    private UIInput input;
    private InputBoxFilter filter;

    private bool ready = false;
	// Use this for initialization
	void Awake () {
        input = GetComponent<UIInput>();
        filter = GetComponent<InputBoxFilter>();

        //The type doesn't change during runtime, so we can just set it here
        if (panel.varData.type == SubactionVarType.FLOAT) filter.filterType = InputBoxFilter.FilterType.DECIMAL;
        else if (panel.varData.type == SubactionVarType.INT) filter.filterType = InputBoxFilter.FilterType.INT;
        else filter.filterType = InputBoxFilter.FilterType.NONE;

        //We need to change the text without firing another update, so unready then re-ready after setting
        ready = false;
        input.value = panel.varData.data;
        ready = true;
    }

    void OnSubactionChanged(SubactionData subaction)
    {
        //We need to change the text without firing another update, so unready then re-ready after setting
        ready = false;
        input.value = panel.varData.data;
        ready = true;
    }

    public void OnAction(string inputData)
    {
        //If we aren't accepting inputs yet, don't.
        if (!ready) return;

        //If we have a filter object, make sure to filter the incoming text before we do anything with it.
        if (filter != null) inputData = filter.filterText(inputData);

        //Convert this to a LegacyAction
        ChangeSubactionVarDataInput legacyAction = ScriptableObject.CreateInstance<ChangeSubactionVarDataInput>();
        legacyAction.init(panel.varData, inputData);

        LegacyEditorData.instance.DoAction(legacyAction);
        input.value = panel.varData.data;
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
