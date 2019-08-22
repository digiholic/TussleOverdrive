using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// This is the model for the legacy editor. Each variable has properties that 
/// </summary>
public class LegacyEditorData : MonoBehaviour
{
    public static LegacyEditorData instance;
    public static ContextualPanelData contextualPanel;
    public static AnchorPositions anchors;

    public GameObject hoveredObject;
    [Header("Component Accessors")]
    [SerializeField] private PanelHider ShadowRealm;
    [SerializeField] private FileBrowser fileBrowser;
    [SerializeField] private NewFighterPopup newFighterPopup;
    public MouseToPixelHandler MouseToPixelHandler;

    public delegate void FighterInfoChangeResults(FighterInfo info);
    public delegate void SpriteInfoChangeResults(SpriteInfo info);
    public delegate void ActionFileChangeResults(ActionFile actions);
    public delegate void DynamicActionChangeResults(DynamicAction action);
    public delegate void AnimationDefinitionChangeResults(AnimationDefinition anim);
    public delegate void ImageDefinitionChangeResults(ImageDefinition def);
    public delegate void SelectedFileChangeResults(FileInfo file);
    public delegate void StringChangeResults(string s);
    public delegate void IntChangeResults(int i);
    public delegate void SubactionDataChangeResults(SubactionData data);

    public event FighterInfoChangeResults FighterInfoChangedEvent;
    public event SpriteInfoChangeResults SpriteInfoChangedEvent;
    public event ActionFileChangeResults ActionFileChangedEvent;
    public event DynamicActionChangeResults CurrentActionChangedEvent;
    public event AnimationDefinitionChangeResults CurrentAnimationChangedEvent;
    public event ImageDefinitionChangeResults CurrentImageDefinitionChangedEvent;
    public event SelectedFileChangeResults CurrentImageFileChangedEvent;
    public event StringChangeResults LeftDropdownChangedEvent;
    public event StringChangeResults RightDropdownChangedEvent;
    public event StringChangeResults GroupDropdownChangedEvent;
    public event IntChangeResults CurrentFrameChangedEvent;
    public event SubactionDataChangeResults CurrentSubactionChangedEvent;

    [Header("Fighter Data")]
    public string FighterDirName;
    #region Loaded Fighter - the currently loaded fighter info
    [SerializeField]
    private FighterInfo _loadedFighter;
    
    public FighterInfo loadedFighter
    {
        get { return _loadedFighter; }
        private set
        {
            _loadedFighter = value;
            if (FighterInfoChangedEvent != null)
                FighterInfoChangedEvent(value);
        }
    }
    #endregion
    #region Loaded Sprite Info - the currently loaded sprite info
    [SerializeField]
    private SpriteInfo _loadedSpriteInfo;

    public SpriteInfo loadedSpriteInfo
    {
        get { return _loadedSpriteInfo; }
        private set
        {
            _loadedSpriteInfo = value;
            if (SpriteInfoChangedEvent != null)
                SpriteInfoChangedEvent(value);
        }
    }
    #endregion
    #region Loaded Action File - the currently loaded action set
    [SerializeField]
    private ActionFile _loadedActionFile;
    
    public ActionFile loadedActionFile
    {
        get { return _loadedActionFile; }
        private set
        {
            _loadedActionFile = value;
            if (ActionFileChangedEvent != null)
                ActionFileChangedEvent(value);
        }
    }
    #endregion
    #region Current Action - the action that is currently selected from the left panel
    [SerializeField]
    private DynamicAction _currentAction;
    
    public DynamicAction currentAction
    {
        get { return _currentAction; }
        set
        {
            _currentAction = value;
            CurrentActionChangedEvent?.Invoke(value);
        }
    }
    #endregion
    #region Current Animation - the Animation currently selected from the left panel
    [SerializeField]
    private AnimationDefinition _currentAnimation;

    public AnimationDefinition currentAnimation
    {
        get { return _currentAnimation; }
        set
        {
            _currentAnimation = value;
            CurrentAnimationChangedEvent?.Invoke(value);
        }
    }
    #endregion
    #region Current Image File - the image file currently selected from the left panel
    [SerializeField]
    private FileInfo _currentImageFile;

    public FileInfo currentImageFile
    {
        get { return _currentImageFile; }
        set
        {
            _currentImageFile = value;
            CurrentImageFileChangedEvent?.Invoke(value);
        }
    }
    #endregion
    #region Current Image Definition - the Image Definition currently selected from the right panel
    [SerializeField]
    private ImageDefinition _currentImageDef;

    public ImageDefinition currentImageDef
    {
        get { return _currentImageDef; }
        set
        {
            _currentImageDef = value;
            CurrentImageDefinitionChangedEvent?.Invoke(value);
        }
    }
    #endregion

    [Header("Navigation")]
    #region Left Dropdown - what is selected on the left dropdown menu
    [SerializeField]
    private string _leftDropdown;
    
    public string leftDropdown
    {
        get { return _leftDropdown; }
        set
        {
            _leftDropdown = value;
            if (LeftDropdownChangedEvent != null)
                LeftDropdownChangedEvent(value);
        }
    }
    #endregion
    #region Right Dropdown - what is selected on the right dropdown menu
    [SerializeField]
    private string _rightDropdown;
    
    public string rightDropdown
    {
        get { return _rightDropdown; }
        set
        {
            _rightDropdown = value;
            if (RightDropdownChangedEvent != null)
                RightDropdownChangedEvent(value);
        }
    }
    #endregion
    #region Subaction Group - The current selected Subaction Group
    [SerializeField]
    private string _subactionGroup;
    
    public string subactionGroup
    {
        get {
            if (_subactionGroup == "Current Frame")
            {
                return SubactionGroup.ONFRAME(currentFrame);
            }
            return _subactionGroup;
        }
        set
        {
            _subactionGroup = value;
            if (GroupDropdownChangedEvent != null)
                GroupDropdownChangedEvent(value);
        }
    }
    #endregion
    #region Current Frame - the frame that is currently being shown in the viewer and right pane
    [SerializeField]
    private int _currentFrame;
    
    public int currentFrame
    {
        get { return _currentFrame; }
        set
        {
            _currentFrame = value;
            if (CurrentFrameChangedEvent != null)
                CurrentFrameChangedEvent(value);
        }
    }
    #endregion
    #region Current Subaction - the subaction that is currently selected for editing
    private SubactionData _currentSubaction = null;
    
    public SubactionData currentSubaction
    {
        get { return _currentSubaction; }
        set
        {
            _currentSubaction = value;
            if (CurrentSubactionChangedEvent != null)
                CurrentSubactionChangedEvent(value);
        }
    }
    #endregion
    #region Contextual Panel Controller - the ContextualPanelData of the currently visible Contextual Panel
    [SerializeField]
    private ContextualPanelData _contextualPanelController;
    
    public ContextualPanelData contextualPanelController
    {
        get { return _contextualPanelController; }
        private set
        {
            _contextualPanelController = value;
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
        anchors = GetComponent<AnchorPositions>();
        BroadcastMessage("InitializeWidget",null,SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// Fire all the changes to things that were modified in the editor
    /// </summary>
    private void Start()
    {
        //Load the fighter, then initialize it. We need to call the changed event here once the directory loads before we do the rest
        loadedFighter.LoadDirectory(FighterDirName);
        FighterInfoChangedEvent(loadedFighter);

        loadedActionFile = ActionFile.LoadActionsFromFile(FighterDirName, loadedFighter.action_file_path);
        currentAction = loadedActionFile.GetFirst();
        loadedSpriteInfo = SpriteInfo.LoadSpritesFromFile(FighterDirName, loadedFighter.sprite_info_path);
        loadedSpriteInfo.LoadDirectory(FighterDirName);
        FireModelChange();
    }

    private void Update()
    {
        CheckKeyboardShortcuts();
        //TODO screen with hook here maybe? Or maybe it should be in the individual components...
        //Debug.Log(Screen.width);
    }

    public void LoadNewFighter(FighterInfo fInfo)
    {
        FighterDirName = fInfo.directory_name;

        loadedFighter = fInfo;
        loadedActionFile = ActionFile.LoadActionsFromFile(FighterDirName, fInfo.action_file_path);
        currentAction = loadedActionFile.GetFirst();
        loadedSpriteInfo = SpriteInfo.LoadSpritesFromFile(FighterDirName, fInfo.sprite_info_path);
        loadedSpriteInfo.LoadDirectory(FighterDirName);

        FireModelChange();
    }

    /// <summary>
    /// Calls every event listener
    /// </summary>
    public void FireModelChange()
    {
        FighterInfoChangedEvent?.Invoke(loadedFighter);
        ActionFileChangedEvent?.Invoke(loadedActionFile);
        CurrentActionChangedEvent?.Invoke(currentAction);
        LeftDropdownChangedEvent?.Invoke(leftDropdown);
        RightDropdownChangedEvent?.Invoke(rightDropdown);
        CurrentFrameChangedEvent?.Invoke(currentFrame);
        CurrentSubactionChangedEvent?.Invoke(currentSubaction);
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
            redoList.Push(act);
            FireModelChange();
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
            FireModelChange();
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

    public void LoadFighterClicked()
    {
        fileBrowser.BrowseForJSON(FileLoader.FighterDir, LoadFighterFromFile);
        fileBrowser.SetErrorText("File does not contain a fighter");
    }

    public void SaveFighterClicked()
    {
        loadedFighter.Save();
        string path = FileLoader.PathCombine(FileLoader.GetFighterPath(FighterDirName), loadedFighter.action_file_path);
        loadedActionFile.Save(path);
        path = FileLoader.PathCombine(FileLoader.GetFighterPath(FighterDirName), loadedFighter.sprite_info_path);
        loadedSpriteInfo.Save(path);
        Debug.Log("Saved Fighter: " + path);
    }

    public void NewFighterClicked()
    {
        newFighterPopup.Initialize();
    }

    private bool LoadFighterFromFile(FileInfo info)
    {
        FighterInfo fInfo = FighterInfo.LoadFighterInfoFile(info.DirectoryName, info.Name);
        if (fInfo != null)
        {
            LoadNewFighter(fInfo);
            return true;
        }
        return false;
    }

    #region static helper methods
    /// <summary>
    /// Banish a panel to the shadow realm, a place where panels no longer in use go.
    /// </summary>
    /// <param name="panelToBanish">The panel to banish. Can technically be any gameObject, but it's mostly used for panels.</param>
    public static void Banish(GameObject panelToBanish)
    {
        if (instance == null || instance.ShadowRealm == null)
        {
            FindObjectOfType<PanelHider>().Banish(panelToBanish);
        } else
        {
            instance.ShadowRealm.Banish(panelToBanish);
        }
    }

    /// <summary>
    /// Returns a panel to the place it was banished from intact.
    /// </summary>
    /// <param name="panelToBanish">An object in the shadow realm to be returned from whence it came</param>
    public static void Unbanish(GameObject panelToBanish)
    {
        if (instance == null || instance.ShadowRealm == null)
        {
            FindObjectOfType<PanelHider>().Unbanish(panelToBanish);
        }
        else
        {
            instance.ShadowRealm.Unbanish(panelToBanish);
        }
    }

    /// <summary>
    /// Returns a panel to the the given parent
    /// </summary>
    /// <param name="panelToBanish">An object in the shadow realm to be returned from whence it came</param>
    /// <param name="destinationPanel">The object to return this object to</param>
    public static void Unbanish(GameObject panelToBanish, GameObject destinationPanel)
    {
        if (instance == null || instance.ShadowRealm == null)
        {
            FindObjectOfType<PanelHider>().Unbanish(panelToBanish,destinationPanel);
        }
        else
        {
            instance.ShadowRealm.Unbanish(panelToBanish);
        }
    }

    /// <summary>
    /// This is a quick helper method that notifies the model that the FighterInfo has changed. It fires the reload method that will cause the view to update.
    /// Since the actions that modify the data directly access the FighterInfo field, it bypasses the setter that would normally do this. MAKE SURE TO CALL THIS WHENEVER THE FIGHTERINFO CHANGES
    /// </summary>
    public static void ChangedFighterData()
    {
        instance.FighterInfoChangedEvent?.Invoke(instance.loadedFighter);
    }

    /// <summary>
    /// This is a quick helper method that notifies the model that the SpriteInfo has changed. It fires the reload method that will cause the view to update.
    /// Since the actions that modify the data directly access the SpriteInfo field, it bypasses the setter that would normally do this. MAKE SURE TO CALL THIS WHENEVER THE SPRITEINFO CHANGES
    /// </summary>
    public static void ChangedSpriteInfo()
    {
        instance.SpriteInfoChangedEvent?.Invoke(instance.loadedSpriteInfo);
    }

    /// <summary>
    /// This is a quick helper method that notifies the model that the Current Action has changed. It fires the reload method that will cause the view to update.
    /// Since the actions that modify the data directly access the DynamicAction field, it bypasses the setter that would normally do this. MAKE SURE TO CALL THIS WHENEVER THE CURRENTACTION CHANGES
    /// </summary>
    public static void ChangedActionData()
    {
        instance.CurrentActionChangedEvent?.Invoke(instance.currentAction);
        instance.ActionFileChangedEvent?.Invoke(instance.loadedActionFile);
    }

    /// <summary>
    /// This is a quick helper method that notifies the model that the Action File has changed. It fires the reload method that will cause the view to update.
    /// Since the actions that modify the data directly access the ActionFile's fields, it bypasses the setter that would normally do this. MAKE SURE TO CALL THIS WHENEVER THE ACTIONFILE CHANGES
    /// </summary>
    public static void ChangedActionFile()
    {
        instance.ActionFileChangedEvent?.Invoke(instance.loadedActionFile);
    }

    /// <summary>
    /// This is a quick helper method that notifies the model that the Subaction has changed. It fires the reload method that will cause the view to update.
    /// Since the actions that modify the data directly access the Subaction's fields, it bypasses the setter that would normally do this. MAKE SURE TO CALL THIS WHENEVER THE SUBACTION CHANGES
    /// </summary>
    public static void ChangedSubaction()
    {
        instance.CurrentSubactionChangedEvent?.Invoke(instance.currentSubaction);
        instance.GroupDropdownChangedEvent?.Invoke(instance.subactionGroup);
    }

    /// <summary>
    /// This is a quick helper method that notifies the model that the ImageDef has changed. It fires the reload method that will cause the view to update.
    /// Since the actions that modify the data directly access the ImageDef's fields, it bypasses the setter that would normally do this. MAKE SURE TO CALL THIS WHENEVER THE IMAGEDEF CHANGES
    /// </summary>
    public static void ChangedImageDef()
    {
        instance.CurrentImageDefinitionChangedEvent?.Invoke(instance.currentImageDef);
    }

    /// <summary>
    /// This is a quick helper method that notifies the model that the Animation has changed. It fires the reload method that will cause the view to update.
    /// Since the actions that modify the data directly access the AnimationDefinition's fields, it bypasses the setter that would normally do this. MAKE SURE TO CALL THIS WHENEVER THE ANIMATION CHANGES
    /// </summary>
    public static void ChangedAnimation()
    {
        instance.CurrentAnimationChangedEvent?.Invoke(instance.currentAnimation);
    }

    /// <summary>
    /// Returns true if the current action is a new action that is not currently in the ActionFile.
    /// If the action is in the ActionFile, it will return false.
    /// </summary>
    /// <returns></returns>
    public static bool CurrentActionIsNew()
    {
        DynamicAction action = instance.currentAction;
        ActionFile actionFile = instance.loadedActionFile;
        return !actionFile.actions.Contains(action);
    }

    public static bool CurrentAnimationIsNew()
    {
        AnimationDefinition def = instance.currentAnimation;
        SpriteInfo info = instance.loadedSpriteInfo;
        return !info.animations.Contains(def);
    }
    #endregion
}