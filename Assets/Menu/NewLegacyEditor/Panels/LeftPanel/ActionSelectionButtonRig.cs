using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionButtonRig : LegacyEditorWidget {
    public GameObject actionSelectionButtonPrefab;

    private List<GameObject> children = new List<GameObject>();
    private UIGrid grid;

    [SerializeField]
    private Transform anchorObject;
    [SerializeField]
    private int leftAnchorOffset, rightAnchorOffset;

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
        button.label.leftAnchor.target = anchorObject;
        button.label.leftAnchor.relative = 0;
        button.label.leftAnchor.absolute = leftAnchorOffset;
        button.label.rightAnchor.target = anchorObject;
        button.label.rightAnchor.relative = 1;
        button.label.rightAnchor.absolute = rightAnchorOffset;


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
