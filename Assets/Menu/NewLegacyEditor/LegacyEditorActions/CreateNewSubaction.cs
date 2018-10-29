using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewSubaction : LegacyEditorAction {
    private SubactionData subDataToAdd;
    private SubactionData subDataMaster;
    private DynamicAction actionToAddTo;
    private string groupToAddTo;

	public void init(SubactionData subData) {
        subDataMaster = subData;
        
    }

    public override void execute()
    {
        actionToAddTo = LegacyEditorData.instance.currentAction;
        groupToAddTo = LegacyEditorData.instance.subactionGroup;

        //Since we don't want to add THIS subactionData, but a copy of it, we reinstance the scriptable object
        subDataToAdd = Instantiate(subDataMaster) as SubactionData;
        actionToAddTo.subactionCategories.GetIfKeyExists(groupToAddTo).Add(subDataToAdd);

        LegacyEditorData.ChangedActionData();
    }

    public override void undo()
    {
        actionToAddTo.subactionCategories.GetIfKeyExists(groupToAddTo).Remove(subDataToAdd);
        LegacyEditorData.ChangedActionData();
    }
}
