using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ImageDefInput : LegacyEditorWidget
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
        ModifyImageDefEditorAction legacyAction = ScriptableObject.CreateInstance<ModifyImageDefEditorAction>();
        int propNum = 0;
        if (varType == VarType.INT)
        {
            if (int.TryParse(propertyVal.ToString(), out propNum))
            {
                legacyAction.init(propertyName, propNum);
            }

        } else
        {
            legacyAction.init(propertyName, propertyVal.ToString());
        }
        LegacyEditorData.instance.DoAction(legacyAction);
    }

    public void OnImageDefChanged(ImageDefinition def)
    {
        if (def != null && def.ImageName != "")
        {
            input.value = GetProperty(def);
        }
    }

    private string GetProperty(ImageDefinition def)
    {
        PropertyInfo prop = def.GetType().GetProperty(propertyName);
        return prop.GetValue(def).ToString();
    }

    public override void RegisterListeners()
    {
        editor.CurrentImageDefinitionChangedEvent += OnImageDefChanged;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentImageDefinitionChangedEvent -= OnImageDefChanged;
    }
}
