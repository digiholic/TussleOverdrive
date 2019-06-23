using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AnimationPropertiesInput : LegacyEditorWidget
{
    [SerializeField]
    private string propertyName;
    [SerializeField]
    private VarType varType;

    private UIInput input;

    private void Awake()
    {
        input = GetComponent<UIInput>();
    }

    public void SendAction(object propertyVal)
    {
        ModifyAnimationEditorAction legacyAction = ScriptableObject.CreateInstance<ModifyAnimationEditorAction>();
        int propNum = 0;
        if (varType == VarType.INT)
        {
            if (int.TryParse(propertyVal.ToString(), out propNum))
            {
                legacyAction.init(propertyName, propNum);
            }
        }
        else
        {
            legacyAction.init(propertyName, propertyVal.ToString());
        }
        LegacyEditorData.instance.DoAction(legacyAction);
    }

    public void OnAnimationChanged(AnimationDefinition def)
    {
        if (def != null && def.AnimationName != "")
        {
            input.value = GetProperty(def);
        }
    }

    private string GetProperty(AnimationDefinition def)
    {
        PropertyInfo prop = def.GetType().GetProperty(propertyName);
        return prop.GetValue(def).ToString();
    }

    public override void RegisterListeners()
    {
        editor.CurrentAnimationChangedEvent += OnAnimationChanged;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentAnimationChangedEvent -= OnAnimationChanged;
    }
}
