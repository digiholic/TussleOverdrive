using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LegacyEditor : MonoBehaviour {
    public static LegacyEditor editor;
    public static bool FighterLoaded = false;

    public delegate void DynamicActionMenuFunction(DynamicAction new_action);
    public delegate void StringMenuFunction(string name);
    public delegate void FighterMenuFunction(FighterInfo fighter_info);
    public delegate void ActionMenuFunction(ActionFile action_file);
    public delegate void SubActionGroupMenuFunction(SubActionGroup group);

    public static event StringMenuFunction OnWindowChanged;
    public static event StringMenuFunction OnSubwindowChanged;
    public static event FighterMenuFunction OnFighterChanged;
    public static event ActionMenuFunction OnActionFileChanged;
    public static event DynamicActionMenuFunction OnSelectedActionChanged;
    public static event StringMenuFunction OnSubactionCategoryChanged;
    public static event SubActionGroupMenuFunction OnSubactionGroupChanged;

    #region Event Firers
    public static void LoadFighterFromFile(FileInfo info_file)
    {
        editor.fighter_file = info_file;
        FighterInfo info = FighterInfo.LoadFighterInfoFile(info_file.DirectoryName, info_file.Name);
        OnFighterChanged(info);
    }
    public static void LoadActionFileFromFile(FileInfo action_file)
    {
        editor.action_file = action_file;
        ActionFile actions = ActionFile.LoadActionsFromFile(action_file.DirectoryName, action_file.Name);
        OnActionFileChanged(actions);
    }
    public static void ChangeSubactionGroup(string new_group_name)
    {
        if (editor.selected_action != null) //If we don't have an action selected, we can't pull the group from it
        {
            DynamicAction act = editor.selected_action;
            switch (new_group_name)
            {
                case "Set Up":
                    OnSubactionGroupChanged(act.set_up_subactions);
                    break;
                case "Transitions":
                    OnSubactionGroupChanged(act.state_transition_subactions);
                    break;
                case "On Frame":
                    OnSubactionGroupChanged(act.subactions_on_frame);
                    break;
                case "Tear Down":
                    OnSubactionGroupChanged(act.tear_down_subactions);
                    break;
            }
        }
    }
    public static void ChangeSelectedAction(string new_selection_name)
    {
        //It doesn't make sense to select an action if we don't have actions yet
        if (editor.current_actions != null)
        {
            editor.selected_action_name = new_selection_name;
            editor.selected_action = editor.current_actions.Get(new_selection_name);
            
            ChangeSubactionGroup(editor.sub_window);
        }
    }
    public static void FireChangeWindow(string window_name)
    {
        OnWindowChanged(window_name);
    }
    public static void FireChangeSubwindow(string subwindow_name)
    {
        OnSubwindowChanged(subwindow_name);
    }
    public static void FireChangeFighter(FighterInfo info)
    {
        OnFighterChanged(info);
    }
    public static void FireChangeActionFile(ActionFile actions)
    {
        OnActionFileChanged(actions);
    }
    public static void FireChangeSelectedAction(DynamicAction action)
    {
        OnSelectedActionChanged(action);
    }
    public static void FireChangeSubactionCategory(string subaction_category)
    {
        OnSubactionCategoryChanged(subaction_category);
    }
    public static void FireChangeSubactionGroup(SubActionGroup group)
    {
        OnSubactionGroupChanged(group);
    }
    #endregion

    #region Event hooks
    public void ChangeWindow(string new_window_name)
    {
        //Check if we are actually changing anything. No sense wasting an update if nothing changes
        if (editor.main_window != new_window_name)
        {
            editor.main_window = new_window_name;
        }
    }

    public void ChangeSubWindow(string new_subwindow_name)
    {
        //Check if we are actually changing anything. No sense wasting an update if nothing changes
        if (editor.sub_window != new_subwindow_name)
        {
            editor.sub_window = new_subwindow_name;
        }
        //If we're changing a subaction group, we need to notify that listener as well
        if (editor.main_window == "Actions" && new_subwindow_name != "Properties")
        {
            ChangeSubactionGroup(new_subwindow_name);
        }
    }
    
    public void ChangeFighter(FighterInfo info)
    {
        FighterLoaded = true;
        editor.current_fighter = info;
        FireChangeActionFile(info.action_file);
    }

    public void ChangeActionFile(ActionFile actions)
    {
        editor.current_actions = actions;
    }

    public void ChangeSelectedAction(DynamicAction new_action)
    {
        //It doesn't make sense to select an action if we don't have actions yet
        if (editor.current_actions != null)
        {
            editor.selected_action = new_action;
            editor.selected_action_name = new_action.name;
            ChangeSubactionGroup(editor.sub_window);
        }
    }

    public void ChangeSubactionCategory(string new_subaction_category)
    {
        editor.selected_subaction_category = new_subaction_category;
    }
    

    public void ChangeSubactionGroup(SubActionGroup new_group)
    {
        editor.subaction_group = new_group;
    }
    #endregion

    public static DirectoryInfo CurrentFighterDir()
    {
        if (editor.current_fighter != null && editor.current_fighter.directory_name != null)
        {
            return FileLoader.GetFighterDir(editor.current_fighter.directory_name);
        }
        else return null;
    }


    public string main_window;
    public string sub_window;
    public FileInfo fighter_file;
    public FighterInfo current_fighter;
    public FileInfo action_file;
    public ActionFile current_actions;
    public string selected_action_name;
    public DynamicAction selected_action;
    public string selected_subaction_category;
    public SubActionGroup subaction_group;

    // Use this for initialization
    void Awake()
    {
        editor = this;
        current_fighter = null;
        current_actions = null;
        selected_action = null;
        BroadcastMessage("OpenFighterBrowser");
    }

    private void OnEnable()
    {
        //Assign delegates
        OnWindowChanged += ChangeWindow;
        OnSubwindowChanged += ChangeSubWindow;
        OnFighterChanged += ChangeFighter;
        OnActionFileChanged += ChangeActionFile;
        OnSelectedActionChanged += ChangeSelectedAction;
        OnSubactionCategoryChanged += ChangeSubactionCategory;
        OnSubactionGroupChanged += ChangeSubactionGroup;
    }

    private void OnDisable()
    {
        //Free delegates
        OnWindowChanged -= ChangeWindow;
        OnSubwindowChanged -= ChangeSubWindow;
        OnFighterChanged -= ChangeFighter;
        OnActionFileChanged -= ChangeActionFile;
        OnSelectedActionChanged -= ChangeSelectedAction;
        OnSubactionCategoryChanged -= ChangeSubactionCategory;
        OnSubactionGroupChanged -= ChangeSubactionGroup;
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

    public void SaveFighter()
    {
        if (editor.fighter_file != null)
        {
            Debug.Log(editor.fighter_file.FullName);
            editor.current_fighter.WriteJSON(editor.fighter_file.FullName);
        }
        if (editor.action_file != null)
        {
            Debug.Log(editor.action_file.FullName);
            editor.current_actions.WriteJSON(editor.action_file.FullName);
        }
    }
    
}

public enum WindowType
{
    MAIN,
    SUB,
    NEW
}