using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LegacyEditor : MonoBehaviour {
    public static LegacyEditor editor;
    public static bool FighterLoaded = false;

    public FighterInfo current_fighter;
    public ActionFile current_actions;
    public DynamicAction selected_action;
    public string current_group_name;

    public FileInfo fighter_file;
    public FileInfo action_file;

	// Use this for initialization
	void Awake () {
        editor = this;
        current_fighter = null;
        current_actions = null;
        selected_action = null;
	}
	
    public void LoadFighter(FighterInfo info)
    {
        FighterLoaded = true;
        current_fighter = info;
        current_actions = info.action_file;
        RefreshFighter();
        RefreshActions();
    }

    void SaveFighter()
    {
        if (fighter_file != null)
        {
            Debug.Log(fighter_file.FullName);
            current_fighter.WriteJSON(fighter_file.FullName);
        }
    }

    public static void RefreshFighter()
    {
        editor.BroadcastMessage("RefreshFighter", editor.current_fighter,SendMessageOptions.DontRequireReceiver);
    }

    public static void RefreshActions()
    {
        editor.BroadcastMessage("RefreshActions", editor.current_actions, SendMessageOptions.DontRequireReceiver);
    }

    public static void ActionChanged()
    {
        editor.BroadcastMessage("ActionChanged", editor.selected_action, SendMessageOptions.DontRequireReceiver);
    }

    public static void SubActionGroupChanged()
    {
        editor.BroadcastMessage("ActionChanged", editor.selected_action, SendMessageOptions.DontRequireReceiver);
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
