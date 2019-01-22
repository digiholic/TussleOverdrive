using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewSubaction : LegacyEditorAction {
    private SubactionDataDefault subDataMaster;
    private SubactionData subDataToAdd;
    private DynamicAction actionToAddTo;
    private string groupToAddTo;

	public void init(SubactionDataDefault subData) {
        subDataMaster = subData;
        
    }

    public override void execute()
    {
        actionToAddTo = LegacyEditorData.instance.currentAction;
        groupToAddTo = LegacyEditorData.instance.subactionGroup;
        Debug.Log("CreateNewSubaction actionToAddTo: "+actionToAddTo);
        Debug.Log("CreateNewSubaction groupToAddTo: " +groupToAddTo);
        //Since we don't want to add THIS subactionData, but a copy of it, we reinstance the scriptable object
        //subDataToAdd = Instantiate(subDataMaster) as SubactionData;
        if (groupToAddTo == "Current Frame")
        {
            groupToAddTo = SubactionGroup.ONFRAME(LegacyEditorData.instance.currentFrame);
            Debug.Log("Current Group: " + groupToAddTo);
        }
        subDataToAdd = subDataMaster.CreateSubactionData();
        actionToAddTo.subactionCategories.GetIfKeyExists(groupToAddTo).Add(subDataToAdd);
        Debug.Log(actionToAddTo.subactionCategories);
        LegacyEditorData.ChangedActionData();
    }

    public override void undo()
    {
        actionToAddTo.subactionCategories.GetIfKeyExists(groupToAddTo).Remove(subDataToAdd);
        LegacyEditorData.ChangedActionData();
    }
}
