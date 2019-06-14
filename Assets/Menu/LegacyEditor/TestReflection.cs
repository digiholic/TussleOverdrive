using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestReflection : LegacyEditorWidget
{
    public bool modifyData = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (modifyData)
        {
            ModifyLegacyEditorDataAction legacyAction = ScriptableObject.CreateInstance<ModifyLegacyEditorDataAction>();
            legacyAction.init("leftDropdown", "Fighter");
            legacyAction.addAdjacentProperty("rightDropdown", LegacyEditorConstants.RightDropdownOptionsDict["Fighter"][0]);
            LegacyEditorData.instance.DoAction(legacyAction);

            //editor.GetType().GetProperty("leftDropdown").SetValue(editor, "Fighter");
            modifyData = false;
        }
    }

    public void testDataChanged(string s)
    {
        
    }

    public override void RegisterListeners()
    {
        editor.LeftDropdownChangedEvent += testDataChanged;
    }

    public override void UnregisterListeners()
    {
        editor.LeftDropdownChangedEvent -= testDataChanged;
    }
}
