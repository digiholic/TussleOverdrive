using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyFighterDataInput : MonoBehaviour {
    public string varName;
    private UIInput input;

    private void Start()
    {
        input = GetComponent<UIInput>();
    }

    private void OnSelect()
    {
        //input.text = getFighterVar();
    }

    private void OnModelChanged()
    {
        if (LegacyEditorData.instance.loadedFighterDirty)
        {
            input.text = getFighterVar();
        }
    }

    public void OnAction(string inputData)
    {
        ChangeFighterInfoField action = ScriptableObject.CreateInstance<ChangeFighterInfoField>();
        action.init(varName, inputData);

        LegacyEditorData.instance.DoAction(action);
    }


    private string getFighterVar()
    {
        FighterInfo info = LegacyEditorData.instance.loadedFighter;
        return (string)info.GetType().GetField(varName).GetValue(info);
    }

}
