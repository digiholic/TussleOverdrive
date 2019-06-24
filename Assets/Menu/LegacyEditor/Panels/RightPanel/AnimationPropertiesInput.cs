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
    private UIToggle toggle;

    private void Awake()
    {
        input = GetComponent<UIInput>();
        toggle = GetComponent<UIToggle>();
    }

    public void SendAction(object propertyVal)
    {
        ModifyAnimationEditorAction legacyAction = ScriptableObject.CreateInstance<ModifyAnimationEditorAction>();
        int propNum = 0;
        float propFloat = 0.0f;
        if (varType == VarType.INT)
        {
            if (propertyVal == null) propertyVal = "0";
            if (int.TryParse(propertyVal.ToString(), out propNum))
            {
                legacyAction.init(propertyName, propNum);
            }
        }
        else if (varType == VarType.FLOAT)
        {
            if (propertyVal == null) propertyVal = "0.0";
            if (float.TryParse(propertyVal.ToString(), out propFloat))
            {
                legacyAction.init(propertyName, propFloat);
            }
        }
        else if (varType == VarType.BOOL)
        {
            if (propertyVal == null) propertyVal = "false";
            legacyAction.init(propertyName, (bool)propertyVal);
        }
        else
        {
            if (propertyVal == null) propertyVal = "";
            legacyAction.init(propertyName, propertyVal.ToString());
        }
        LegacyEditorData.instance.DoAction(legacyAction);
    }

    public void OnAnimationChanged(AnimationDefinition def)
    {
        if (def != null && def.AnimationName != "")
        {
            if (input != null)
            {
                input.value = GetStringProperty(def);
            }
            else if (toggle != null)
            {
                toggle.value = GetBoolProperty(def);
            }
        }
    }

    private string GetStringProperty(AnimationDefinition def)
    {
        PropertyInfo prop = def.GetType().GetProperty(propertyName);
        return prop.GetValue(def).ToString();
    }

    private bool GetBoolProperty(AnimationDefinition def)
    {
        PropertyInfo prop = def.GetType().GetProperty(propertyName);
        return (bool) prop.GetValue(def);
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
