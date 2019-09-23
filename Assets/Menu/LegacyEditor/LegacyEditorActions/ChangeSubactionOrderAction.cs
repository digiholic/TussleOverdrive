using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSubactionOrderAction : LegacyEditorAction
{
    private DynamicAction actionToModify;
    private string subGroupToModify;

    private List<SubactionData> nextList;
    private List<SubactionData> previousList;

    public void init(List<SubactionData> nextList)
    {
        this.nextList = nextList;
    }

    public override void execute()
    {
        actionToModify = LegacyEditorData.instance.currentAction;
        subGroupToModify = LegacyEditorData.instance.subactionGroup;
        if (subGroupToModify == "Current Frame")
        {
            subGroupToModify = SubactionGroup.ONFRAME(LegacyEditorData.instance.currentFrame);
        }

        previousList = actionToModify.subactionCategories.GetIfKeyExists(subGroupToModify);
        actionToModify.subactionCategories.Set(subGroupToModify,nextList);

        LegacyEditorData.ChangedActionData();
    }

    public override void undo()
    {
        actionToModify.subactionCategories.Set(subGroupToModify,previousList);
        LegacyEditorData.ChangedActionData();
    }
}
