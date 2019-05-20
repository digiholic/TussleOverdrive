using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFighterInfoField : LegacyEditorAction
{
    private string fighterVar = null;
    private string previousValue = null;
    public string nextValue = null;
    
    public void init(string fighterVarName, string value)
    {
        fighterVar = fighterVarName;
        nextValue = value;
    }

    public override void execute()
    {
        previousValue = getFighterVar();
        setFighterVar(nextValue);
        LegacyEditorData.ChangedFighterData(); //Fire the model changed method since we directly modified the fighter data instead of the model itself.
    }

    public override void undo()
    {
        setFighterVar(previousValue);
        LegacyEditorData.ChangedFighterData();
    }

    private string getFighterVar()
    {
        FighterInfo info = LegacyEditorData.instance.loadedFighter;
        return (string)info.GetType().GetField(fighterVar).GetValue(info);
    }

    private void setFighterVar(string valueToSet)
    {
        FighterInfo info = LegacyEditorData.instance.loadedFighter;
        info.GetType().GetField(fighterVar).SetValue(info, valueToSet);
    }
}
