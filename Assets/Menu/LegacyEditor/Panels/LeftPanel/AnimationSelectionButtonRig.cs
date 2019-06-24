using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSelectionButtonRig : LegacyEditorWidget
{
    public GameObject actionSelectionButtonPrefab;

    private List<GameObject> children = new List<GameObject>();
    private UIGrid grid;

    [SerializeField]
    private Transform anchorObject;
    [SerializeField]
    private int leftAnchorOffset, rightAnchorOffset;

    // Use this for initialization
    void Awake()
    {
        grid = GetComponent<UIGrid>();
    }

    void OnSpriteInfoChanged(SpriteInfo sprite_info)
    {
        //Get rid of our old list
        foreach (GameObject child in children)
        {
            NGUITools.Destroy(child);
        }
        children.Clear(); //Empty the list for future use

        //Create all the new buttons
        foreach (AnimationDefinition anim in sprite_info.animations)
        {
            instantiateButton(anim);
        }

        //Realign the grid
        grid.Reposition();
    }

    void OnLeftDropdownChanged(string s)
    {
        //If the option is "Actions", sets all the children to enabled. Otherwise, disables them.
        //Yes I know this doesn't need to be if/else but readability > efficiency
        if (s == "Animations")
        {
            NGUITools.SetActiveChildren(gameObject, true);
        }
        else
        {
            NGUITools.SetActiveChildren(gameObject, false);
        }
    }

    private void instantiateButton(AnimationDefinition anim)
    {
        GameObject go = NGUITools.AddChild(gameObject, actionSelectionButtonPrefab);
        AnimationSelectionButton button = go.GetComponent<AnimationSelectionButton>();
        button.SetAnimation(anim);
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
        editor.SpriteInfoChangedEvent += OnSpriteInfoChanged;
        editor.LeftDropdownChangedEvent += OnLeftDropdownChanged;
    }

    public override void UnregisterListeners()
    {
        editor.SpriteInfoChangedEvent -= OnSpriteInfoChanged;
        editor.LeftDropdownChangedEvent -= OnLeftDropdownChanged;
    }
}
