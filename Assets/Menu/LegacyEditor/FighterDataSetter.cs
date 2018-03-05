using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterDataSetter : MonoBehaviour, LegacyDataViewer {
    public FighterDataPropertyType variable;
    private UIInput display_area;

    public void FighterChanged(FighterInfo info)
    {
        switch (variable)
        {
            case FighterDataPropertyType.NAME:
                display_area.text = info.display_name;
                break;
            case FighterDataPropertyType.FRANCHISE_ICON:
                display_area.text = info.franchise_icon_path;
                break;
            case FighterDataPropertyType.CSS_ICON:
                display_area.text = info.css_icon_path;
                break;
            case FighterDataPropertyType.CSS_PORTRAIT:
                display_area.text = info.css_portrait_path;
                break;
            case FighterDataPropertyType.ACTION_FILE:
                Debug.Log(gameObject.name);
                display_area.text = info.action_file_path;
                break;
        }
    }

    void DataChanged(string text)
    {
        FighterInfo info = LegacyEditor.editor.current_fighter;
        switch (variable)
        {
            case FighterDataPropertyType.NAME:
                info.display_name = display_area.text;
                break;
            case FighterDataPropertyType.FRANCHISE_ICON:
                info.franchise_icon_path = display_area.text;
                break;
            case FighterDataPropertyType.CSS_ICON:
                info.css_icon_path = display_area.text;
                break;
            case FighterDataPropertyType.CSS_PORTRAIT:
                info.css_portrait_path = display_area.text;
                break;
            case FighterDataPropertyType.ACTION_FILE:
                info.action_file_path = display_area.text;
                break;
        }
        LegacyEditor.FireChangeFighter(info);
    }

    public void ActionFileChanged(ActionFile actions) { }
    public void CategoryChanged(string category_name) { }
    public void SelectedActionChanged(DynamicAction action) { }
    public void SubactionGroupChanged(SubActionGroup group) { }
    public void SubWindowChanged(string sub_window_name) { }
    public void WindowChanged(string window_name) { }

    public void OnEnable()
    {
        display_area = GetComponent<UIInput>();
        Debug.Log("Setting display area: "+display_area);
        LegacyEditor.OnFighterChanged += FighterChanged;
    }

    public void OnDisable()
    {
        LegacyEditor.OnFighterChanged -= FighterChanged;
    }

    public enum FighterDataPropertyType
    {
        NAME, FRANCHISE_ICON, CSS_ICON, CSS_PORTRAIT, ACTION_FILE
    }
}
