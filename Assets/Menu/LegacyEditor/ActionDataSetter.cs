using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDataSetter : MonoBehaviour, LegacyDataViewer {
    public ActionDataPropertyType variable;
    private UIInput display_area;
    private UICheckbox bool_display;

    void Start()
    {
    }

    //This can be either a string or a bool depending on what changed. We'll know once we process the switch
    public void DataChanged(object value)
    { 
        DynamicAction action = LegacyEditor.editor.selected_action;
        if (action != null)
        {
            switch (variable)
            {
                case ActionDataPropertyType.NAME:
                    action.name = (string)value;
                    break;
                case ActionDataPropertyType.LENGTH:
                    action.length = string.IsNullOrEmpty((string)value) ? 0 : int.Parse((string)value);
                    break;
                case ActionDataPropertyType.SPRITE:
                    action.sprite = (string)value;
                    break;
                case ActionDataPropertyType.SPRITE_RATE:
                    action.sprite_rate = string.IsNullOrEmpty((string)value) ? 0 : int.Parse((string)value);
                    break;
                case ActionDataPropertyType.LOOP:
                    action.loop = (bool)value;
                    break;
                case ActionDataPropertyType.EXIT_ACTION:
                    action.exit_action = (string)value;
                    break;
            }
            LegacyEditor.FireChangeSelectedAction(action);
        }
    }

    public void OnEnable()
    {
        display_area = GetComponent<UIInput>();
        bool_display = GetComponent<UICheckbox>();
        LegacyEditor.OnSelectedActionChanged += SelectedActionChanged;
    }

    public void OnDisable()
    {
        LegacyEditor.OnSelectedActionChanged -= SelectedActionChanged;
    }

    public void SelectedActionChanged(DynamicAction action)
    {
        switch (variable) {
            case ActionDataPropertyType.NAME:
                display_area.text = action.name;
                break;
            case ActionDataPropertyType.LENGTH:
                display_area.text = action.length.ToString();
                break;
            case ActionDataPropertyType.SPRITE:
                display_area.text = action.sprite;
                break;
            case ActionDataPropertyType.SPRITE_RATE:
                display_area.text = action.sprite_rate.ToString();
                break;
            case ActionDataPropertyType.LOOP:
                bool_display.isChecked = action.loop;
                break;
            case ActionDataPropertyType.EXIT_ACTION:
                display_area.text = action.exit_action;
                break;
        }
    }

    #region unused setters
    public void ActionFileChanged(ActionFile actions) {}
    public void CategoryChanged(string category_name) {}
    public void FighterChanged(FighterInfo fighter_info) {}
    public void SubactionGroupChanged(SubActionGroup group) {}
    public void SubWindowChanged(string sub_window_name) {}
    public void WindowChanged(string window_name) {}
    #endregion

    public enum ActionDataPropertyType
    {
        NAME,LENGTH,SPRITE,SPRITE_RATE,LOOP,EXIT_ACTION
    }
}
