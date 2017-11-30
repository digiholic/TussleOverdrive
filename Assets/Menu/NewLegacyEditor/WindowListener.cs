using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowListener : MonoBehaviour, LegacyDataViewer {
    public WindowType ListenForWindow;
    public List<WindowMapping> windows;

	public void WindowChanged(string window_name)
    {
        //We only do this function if the window type is main.
        if (ListenForWindow == WindowType.MAIN)
            UpdateWindows(window_name);
    }

    public void SubWindowChanged(string window_name)
    {
        if (ListenForWindow == WindowType.SUB)
            UpdateWindows(window_name);
    }

    public void CategoryChanged(string category_name)
    {
        Debug.Log(category_name);
        if (ListenForWindow == WindowType.NEW)
            UpdateWindows(category_name);
    }
    private void UpdateWindows(string window_name)
    {
        //Deactivate all main panels first
        foreach (WindowMapping mapping in windows)
        {
            NGUITools.SetActive(mapping.panel, false);
        }

        //Activate the correct one. We can't do this all at once because multiple dropdowns could lead to the same windows
        foreach (WindowMapping mapping in windows)
        {
            if (mapping.window_name == window_name)
            {
                NGUITools.SetActive(mapping.panel, true);
            }
        }
    }

    //Unused data listeners
    public void FighterChanged(FighterInfo fighter_info) {}
    public void ActionsChanged(ActionFile actions) {}
    public void SelectedActionChanged(DynamicAction action) {}
    public void SubactionGroupChanged(SubActionGroup group) {}
}

[System.Serializable]
public struct WindowMapping
{
    public string window_name;
    public GameObject panel;
}