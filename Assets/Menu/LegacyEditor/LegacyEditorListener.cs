using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// This class is just to listen for messages and call static methods in Legacy Editor.
/// It exists purely so that we can cut out the middleman and use UIButtonMessages instead of needing scripts to convert them.
/// </summary>
public class LegacyEditorListener : MonoBehaviour {

    public static void ChangeWindow(string new_window_name)
    {
        LegacyEditor.FireChangeWindow(new_window_name);
    }
    public static void ChangeSubWindow(string new_subwindow_name)
    {
        LegacyEditor.FireChangeSubwindow(new_subwindow_name);
    }
    public static void ChangeFighter(FileInfo info_file)
    {
        LegacyEditor.LoadFighterFromFile(info_file);
    }
    public static void ChangeActions(FileInfo action_file)
    {
        LegacyEditor.LoadActionFileFromFile(action_file);
    }
    public static void ChangeSelectedAction(string new_selection_name)
    {
        LegacyEditor.ChangeSelectedAction(new_selection_name);
    }
    public void ChangeSubactionCategory(string new_subaction_category)
    {
        LegacyEditor.FireChangeSubactionCategory(new_subaction_category);
    }
}
