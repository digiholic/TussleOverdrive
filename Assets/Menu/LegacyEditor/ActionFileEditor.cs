using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ActionFileEditor : MonoBehaviour {
    public static ActionFile action_file = new ActionFile();

    public string action_file_path;
    public ActionButton buttonPrefab;
    public Transform buttonParent;

    private UIInput textInput;

	// Use this for initialization
	void Start () {
        textInput = transform.GetComponentInChildren<UIInput>();

    }
	
    void OnSubmit(string text)
    {
        action_file_path = text;
    }

    void SaveFile()
    {
        if (action_file != null)
        {
            action_file.WriteJSON(action_file_path);
        }
    }

    void LoadFile()
    {
        Debug.Log("Loading");
        string action_json_path = Path.Combine("Assets/Resources/", action_file_path);
        if (File.Exists(action_json_path))
        {
            string action_json = File.ReadAllText(action_json_path);
            action_file = JsonUtility.FromJson<ActionFile>(action_json);
            action_file.BuildDict();
        }
        int index = 0;
        foreach (DynamicAction action in action_file.actions)
        {
            ActionButton button = Instantiate<ActionButton>(buttonPrefab);
            button.transform.SetParent(buttonParent, false);
            button.LoadAction(action.name, index);
            if (index == 0) button.Activate();
            index++;
        }
    }
}
