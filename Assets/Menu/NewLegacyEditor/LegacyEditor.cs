using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LegacyEditor : MonoBehaviour {
    public static LegacyEditor editor;
    public static bool FighterLoaded = false;

    /************************************************
     *                WINDOW SELECTOR               *
     ***********************************************/
    public string main_window;
    
    /// <summary>
    /// Change the main window. Can be Fighter or Action
    /// </summary>
    /// <param name="new_window_name">Either "Fighter" or "Action"</param>
    public static void ChangeWindow(string new_window_name)
    {
        //Check if we are actually changing anything. No sense wasting an update if nothing changes
        if (editor.main_window != new_window_name)
        {
            editor.main_window = new_window_name;
            BroadcastWindowChanged();
        }
    }
    public static void BroadcastWindowChanged()
    {
        editor.BroadcastMessage("WindowChanged", editor.main_window, SendMessageOptions.DontRequireReceiver);
    }

    /************************************************
     *              SUBWINDOW SELECTOR              *
     ***********************************************/
    public string sub_window;

    public static void ChangeSubWindow(string new_subwindow_name)
    {
        //Check if we are actually changing anything. No sense wasting an update if nothing changes
        if (editor.sub_window != new_subwindow_name)
        {
            editor.sub_window = new_subwindow_name;
            BroadcastSubWindowChanged();
        }
        //If we're changing a subaction group, we need to notify that listener as well
        if (editor.main_window == "Action" && new_subwindow_name != "Properties")
        {
            ChangeSubactionGroup(new_subwindow_name);
        }
    }
    public static void BroadcastSubWindowChanged()
    {
        editor.BroadcastMessage("SubWindowChanged", editor.sub_window, SendMessageOptions.DontRequireReceiver);
    }

    /************************************************
     *                 FIGHTER INFO                 *
     ***********************************************/
    public FileInfo fighter_file;
    public FighterInfo current_fighter;

    public static void ChangeFighter(FileInfo info_file)
    {
        editor.fighter_file = info_file;
        FighterInfo info = FighterInfo.LoadFighterInfoFile(info_file.DirectoryName, info_file.Name);
        ChangeFighter(info);
    }
    public static void ChangeFighter(FighterInfo info)
    {
        FighterLoaded = true;
        editor.current_fighter = info;
        editor.current_actions = info.action_file;
        BroadcastFighterChanged();
    }
    public static void BroadcastFighterChanged()
    {
        editor.BroadcastMessage("FighterChanged", editor.current_fighter, SendMessageOptions.DontRequireReceiver);
    }
    public static void SaveFighter()
    {
        if (editor.fighter_file != null)
        {
            Debug.Log(editor.fighter_file.FullName);
            editor.current_fighter.WriteJSON(editor.fighter_file.FullName);
        }
    }

    /************************************************
     *                  ACTION FILE                 *
     ***********************************************/
    public FileInfo action_file;
    public ActionFile current_actions;

    public static void ChangeActions(FileInfo action_file)
    {
        editor.action_file = action_file;
        ActionFile actions = ActionFile.LoadActionsFromFile(action_file.DirectoryName, action_file.Name);
        ChangeActions(actions);
    }
    public static void ChangeActions(ActionFile actions)
    {
        editor.current_actions = actions;
        BroadcastActionsChanged();
    }
    public static void BroadcastActionsChanged()
    {
        editor.BroadcastMessage("ActionsChanged", editor.current_actions, SendMessageOptions.DontRequireReceiver);
    }

    /************************************************
     *                ACTION SELECTOR               *
     ***********************************************/
    public string selected_action_name;
    public DynamicAction selected_action;

    public static void ChangeSelectedAction(string new_selection_name)
    {
        //It doesn't make sense to select an action if we don't have actions yet
        if (editor.current_actions != null)
        {
            editor.selected_action_name = new_selection_name;
            editor.selected_action = editor.current_actions.Get(new_selection_name);
            BroadcastSelectedActionChanged();
        }
    }
    public static void ChangeSelectedAction(DynamicAction new_action)
    {
        //It doesn't make sense to select an action if we don't have actions yet
        if (editor.current_actions != null)
        {
            editor.selected_action = new_action;
            editor.selected_action_name = new_action.name;
            BroadcastSelectedActionChanged();
        }
    }
    public static void BroadcastSelectedActionChanged()
    {
        editor.BroadcastMessage("SelectedActionChanged", editor.selected_action, SendMessageOptions.DontRequireReceiver);
    }

    /************************************************
     *               SUBACTION CATEGORY             *
     ***********************************************/
    public string selected_subaction_category;
    
    public static void ChangeSubactionCategory(string new_subaction_category)
    {
        Debug.Log(new_subaction_category);
        editor.selected_subaction_category = new_subaction_category;
        BroadcastCategoryChanged();
    }
    public static void BroadcastCategoryChanged()
    {
        editor.BroadcastMessage("CategoryChanged", editor.selected_subaction_category, SendMessageOptions.DontRequireReceiver);
    }

    /************************************************
     *               SUBACTION GROUP                *
     ***********************************************/
    public SubActionGroup subaction_group;

    public static void ChangeSubactionGroup(string new_group_name)
    {
        if (editor.selected_action != null) //If we don't have an action selected, we can't pull the group from it
        {
            DynamicAction act = editor.selected_action;
            switch (new_group_name)
            {
                case "Set Up":
                    ChangeSubactionGroup(act.set_up_subactions);
                    break;
                case "Transitions":
                    ChangeSubactionGroup(act.state_transition_subactions);
                    break;
                case "On Frame":
                    ChangeSubactionGroup(act.subactions_on_frame);
                    break;
                case "Tear Down":
                    ChangeSubactionGroup(act.tear_down_subactions);
                    break;
            }
        }
    }
    public static void ChangeSubactionGroup(SubActionGroup new_group)
    {
        editor.subaction_group = new_group;
        BroadcastSubactionGroupChanged();
    }
    public static void BroadcastSubactionGroupChanged()
    {
        editor.BroadcastMessage("SubactionGroupChanged", editor.subaction_group, SendMessageOptions.DontRequireReceiver);
    }

    // Use this for initialization
    void Awake()
    {
        editor = this;
        current_fighter = null;
        current_actions = null;
        selected_action = null;
    }

    void Update()
    {
        //Key commands
        //Control can be either button. We have to check a lot so it's been put here for simplicity's sake
        //bool ctrlDown = (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl));
        bool ctrlDown = true;
        if (ctrlDown && (Input.GetKeyDown(KeyCode.Z)))
        {
            BuilderActionHistory.Undo();
        }
        if (ctrlDown && (Input.GetKeyDown(KeyCode.Y)))
        {
            BuilderActionHistory.Redo();
        }
    }
    
    public static DirectoryInfo CurrentFighterDir()
    {
        if (editor.current_fighter != null && editor.current_fighter.directory_name != null)
        {
            return FileLoader.GetFighterDir(editor.current_fighter.directory_name);
        }
        else return null;
    }
}

public enum WindowType
{
    MAIN,
    SUB,
    NEW
}