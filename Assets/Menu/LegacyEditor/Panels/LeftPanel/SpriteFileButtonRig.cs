﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpriteFileButtonRig : LegacyEditorWidget
{
    public GameObject spriteFileButtonPrefab;

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

    void OnFighterChanged(FighterInfo fighter)
    {
        //Get rid of our old list
        foreach (GameObject child in children)
        {
            NGUITools.Destroy(child);
        }
        children.Clear(); //Empty the list for future use

        //Create all the new 
        foreach (FileInfo imageFile in fighter.sprite_info.spriteFiles)
        {
            instantiateButton(imageFile);
        }

        //Realign the grid
        grid.Reposition();
    }

    void OnLeftDropdownChanged(string s)
    {
        //If the option is "Sprites", sets all the children to enabled. Otherwise, disables them.
        //Yes I know this doesn't need to be if/else but readability > efficiency
        if (s == "Sprites")
        {
            NGUITools.SetActiveChildren(gameObject, true);
        }
        else
        {
            NGUITools.SetActiveChildren(gameObject, false);
        }
    }

    private void instantiateButton(FileInfo file)
    {
        GameObject go = NGUITools.AddChild(gameObject, spriteFileButtonPrefab);
        SpriteFileButton button = go.GetComponent<SpriteFileButton>();
        button.SetImageFile(file);
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
        editor.FighterInfoChangedEvent += OnFighterChanged;
        editor.LeftDropdownChangedEvent += OnLeftDropdownChanged;
    }

    public override void UnregisterListeners()
    {
        editor.FighterInfoChangedEvent -= OnFighterChanged;
        editor.LeftDropdownChangedEvent -= OnLeftDropdownChanged;
    }
}
