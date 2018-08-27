using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeActionField : LegacyEditorAction {

    private string actionVar = null;
    private object previousValue = null;
    public object nextValue = null;

    public void init(string actionVarName, object value)
    {
        actionVar = actionVarName;
        nextValue = value;
    }

    public override void execute()
    {
        previousValue = getActionVar();
        setActionVar(nextValue);
        LegacyEditorData.ChangedActionData();
    }

    public override void undo()
    {
        setActionVar(previousValue);
        LegacyEditorData.ChangedActionData();
    }

    private object getActionVar()
    {
        DynamicAction action = LegacyEditorData.instance.currentAction;
        return action.GetType().GetField(actionVar).GetValue(action);
    }

    private void setActionVar(object valueToSet)
    {
        DynamicAction action = LegacyEditorData.instance.currentAction;
        action.GetType().GetField(actionVar).SetValue(action, valueToSet);
    }
}
