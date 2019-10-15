using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHitboxVarInput : LegacyEditorWidget
{
    private bool ready;

    [SerializeField] private UIInput input;

    void Start()
    {
        if (input == null) input = GetComponent<UIInput>();
    }

    public string varDataName;

    public void changedValue(string newVal){
        //Because this window will only ever be visible when a create hitbox subaction is selected, we just need to get the current subaction.
        SubactionData createHitboxSubaction = editor.currentSubaction;
        SubactionVarData varData = createHitboxSubaction.arguments[varDataName];

        //Convert this to a LegacyAction
        ChangeSubactionVarDataInput legacyAction = ScriptableObject.CreateInstance<ChangeSubactionVarDataInput>();
        legacyAction.init(varData, newVal);

        LegacyEditorData.instance.DoAction(legacyAction);
        input.value = varData.data;
    }

    void OnSubactionChanged(SubactionData subaction)
    {
        //Since this is fired even when the panel is deactivated, we need to check if it's a createHitboxAction
        if (editor?.currentSubaction?.SubactionName == "CreateHitbox"){
            SubactionData createHitboxSubaction = editor.currentSubaction;

            SubactionVarData varData = createHitboxSubaction.arguments[varDataName];

            //We need to change the text without firing another update, so unready then re-ready after setting
            ready = false;
            input.value = varData.data;
            ready = true;
        }
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
