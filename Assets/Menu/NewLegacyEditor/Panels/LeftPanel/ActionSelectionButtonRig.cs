using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionButtonRig : LegacyEditorWidget {
    public GameObject actionSelectionButtonPrefab;

    private List<GameObject> children = new List<GameObject>();
    private UIGrid grid;

	// Use this for initialization
	void Awake () {
        grid = GetComponent<UIGrid>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnActionFileChanged(ActionFile actionFile)
    {
        //Get rid of our old list
        foreach (GameObject child in children)
        {
            NGUITools.Destroy(child);
        }
        children.Clear(); //Empty the list for future use

        //Create all the new buttons
        foreach (DynamicAction action in actionFile.actions)
        {
            instantiateButton(action);
        }

        //Realign the grid
        grid.Reposition();
    }

    void OnLeftDropdownChanged(string s)
    {
        //If the option is "Actions", sets all the children to enabled. Otherwise, disables them.
        //Yes I know this doesn't need to be if/else but readability > efficiency
        if (s == "Actions")
        {
            NGUITools.SetActiveChildren(gameObject, true);
        }
        else
        {
            NGUITools.SetActiveChildren(gameObject, false);
        }
    }

    private void instantiateButton(DynamicAction action)
    {
        GameObject go = NGUITools.AddChild(gameObject, actionSelectionButtonPrefab);
        ActionSelectionButton button = go.GetComponent<ActionSelectionButton>();
        button.SetAction(action);
        children.Add(go);
    }

    public override void RegisterListeners()
    {
        editor.ActionFileChangedEvent += OnActionFileChanged;
        editor.LeftDropdownChangedEvent += OnLeftDropdownChanged;
    }

    public override void UnregisterListeners()
    {
        editor.ActionFileChangedEvent -= OnActionFileChanged;
        editor.LeftDropdownChangedEvent -= OnLeftDropdownChanged;
    }
}
