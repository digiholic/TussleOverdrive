using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionVarDataInput : MonoBehaviour {
    [SerializeField]
    private SubactionVarDataPanel panel;
    private UIInput input;
    private InputBoxFilter filter;

	// Use this for initialization
	void OnEnable () {
        input = GetComponent<UIInput>();
        filter = GetComponent<InputBoxFilter>();

        //The type doesn't change during runtime, so we can just set it here
        if (panel.varData.type == SubactionVarType.FLOAT) filter.filterType = InputBoxFilter.FilterType.DECIMAL;
        else if (panel.varData.type == SubactionVarType.INT) filter.filterType = InputBoxFilter.FilterType.INT;
        else filter.filterType = InputBoxFilter.FilterType.NONE;

    }

    private void Update()
    {
        input.text = panel.varData.data;
    }

    void OnAction(string inputData)
    {
        //If we have a filter object, make sure to filter the incoming text before we do anything with it.
        if (filter != null) inputData = filter.filterText(inputData);

        //Convert this to a LegacyAction
        ChangeSubactionVarDataInput legacyAction = ScriptableObject.CreateInstance<ChangeSubactionVarDataInput>();
        legacyAction.init(panel.varData, inputData);

        LegacyEditorData.instance.DoAction(legacyAction);
    }
}
