﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the model for the legacy editor. Each variable has properties that 
/// </summary>
public class LegacyEditorData : MonoBehaviour
{
    public static LegacyEditorData instance;

    #region Loaded Fighter - the currently loaded fighter info
    [SerializeField]
    private FighterInfo _loadedFighter;
    public bool loadedFighterDirty { get; private set; }

    public FighterInfo loadedFighter
    {
        get { return _loadedFighter; }
        private set
        {
            _loadedFighter = value;
            loadedFighterDirty = true;
        }
    }
    #endregion
    #region Loaded Action File - the currently loaded action set
    [SerializeField]
    private ActionFile _loadedActionFile;
    public bool loadedActionFileDirty { get; private set; }

    public ActionFile loadedActionFile
    {
        get { return _loadedActionFile; }
        private set
        {
            _loadedActionFile = value;
            loadedActionFileDirty = true;
        }
    }
    #endregion
    #region Current Action - the action that is currently selected from the left panel
    [SerializeField]
    private DynamicAction _currentAction;
    public bool currentActionDirty { get; private set; }

    public DynamicAction currentAction
    {
        get { return _currentAction; }
        set
        {
            _currentAction = value;
            currentActionDirty = true;
        }
    }
    #endregion
    #region Left Dropdown - what is selected on the left dropdown menu
    [SerializeField]
    private string _leftDropdown;
    public bool leftDropdownDirty { get; private set; }

    public string leftDropdown
    {
        get { return _leftDropdown; }
        set
        {
            _leftDropdown = value;
            leftDropdownDirty = true;
        }
    }
    #endregion
    #region Right Dropdown - what is selected on the right dropdown menu
    [SerializeField]
    private string _rightDropdown;
    public bool rightDropdownDirty { get; private set; }

    public string rightDropdown
    {
        get { return _rightDropdown; }
        private set
        {
            _rightDropdown = value;
            rightDropdownDirty = true;
        }
    }
    #endregion
    #region Current Frame - the frame that is currently being shown in the viewer and right pane
    [SerializeField]
    private int _currentFrame;
    public bool currentFrameDirty { get; private set; }

    public int currentFrame
    {
        get { return _currentFrame; }
        set
        {
            _currentFrame = value;
            currentFrameDirty = true;
        }
    }
    #endregion
    #region Current Selected - the builder object that is currently selected for editing
    [SerializeField]
    private BuilderSelectable _currentSelected;
    public bool currentSelectedDirty { get; private set; }

    public BuilderSelectable currentSelected
    {
        get { return _currentSelected; }
        private set
        {
            _currentSelected = value;
            currentSelectedDirty = true;
        }
    }
    #endregion

    //TODO
    public string contextFighterCategory;
    public string contextSubactionCategory;
    public string actionSearchText;
    public string spriteSearchText;
    public string sortOrder;

    /// <summary>
    /// Set the singleton instance at OnEnable time, the earliest we can
    /// </summary>
    private void OnEnable()
    {
        instance = this;
    }

    /// <summary>
    /// Fire all the changes to things that were modified in the editor
    /// </summary>
    private void Start()
    {
        loadedFighterDirty = true;
        loadedActionFileDirty = true;
        FireModelChange();
    }

    private void Update()
    {
        CheckKeyboardShortcuts();
    }
    /// <summary>
    /// Calls everything's OnModelChanged methods, then unsets the dirty bits for everything
    /// </summary>
    public void FireModelChange()
    {
        BroadcastMessage("OnModelChanged");

        //After the broadcast, clear all the "dirty" bits
        loadedFighterDirty = false;
        loadedActionFileDirty = false;
        currentActionDirty = false;
        leftDropdownDirty = false;
        rightDropdownDirty = false;
        currentFrameDirty = false;
        currentSelectedDirty = false;
    }

    private Stack<LegacyEditorAction> undoList = new Stack<LegacyEditorAction>();
    private Stack<LegacyEditorAction> redoList = new Stack<LegacyEditorAction>();

    public void Undo()
    {
        //If we have no history we don't have anything to undo and just quietly don't do anything
        if (undoList.Count > 0)
        {
            LegacyEditorAction act = undoList.Pop();
            act.undo();
            Debug.Log("Undoing Action: " + act);
            redoList.Push(act);
            BroadcastMessage("OnModelChanged");
        }
    }

    public void Redo()
    {
        //If we have nothing to redo then we just quietly don't do anything
        if (redoList.Count > 0)
        {
            LegacyEditorAction act = redoList.Pop();
            act.execute();
            undoList.Push(act);
            BroadcastMessage("OnModelChanged");
        }
    }

    public void DoAction(LegacyEditorAction act)
    {
        //Once we do a new thing, our redo list blows the hell up
        redoList.Clear();
        //Then actually do the thing
        act.execute();
        //This is a special tool that will help us later
        undoList.Push(act);
        BroadcastMessage("OnModelChanged");
    }

    private void CheckKeyboardShortcuts()
    {
        //Check for CTRL shortcuts. Since the editor keyboard shortcuts can't be disabled, if you're in editor, it'll activate without ctrl
        //TODO remove this for final build so testing is easier.
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Undo();
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                Redo();
            }
        }

    }
}