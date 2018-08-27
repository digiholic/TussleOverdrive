using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFighterInfoVar : LegacyEditorAction
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
        FighterInfo info = LegacyEditorData.instance.loadedFighter;
        previousValue = info.GetVarByName(fighterVar).value;
        info.GetVarByName(fighterVar).value = nextValue;
        LegacyEditorData.ChangedFighterData(); //Fire the model changed method since we directly modified the fighter data instead of the model itself.
    }

    public override void undo()
    {
        FighterInfo info = LegacyEditorData.instance.loadedFighter;
        info.GetVarByName(fighterVar).value = previousValue;

        LegacyEditorData.ChangedFighterData();
    }
}
