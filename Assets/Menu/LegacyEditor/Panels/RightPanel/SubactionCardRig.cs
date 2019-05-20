using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionCardRig : LegacyEditorWidget {
    public GameObject SubactionCardPrefab;

    private List<GameObject> children = new List<GameObject>();
    private UIGrid grid;
    [SerializeField] private UIScrollView dragPanel;
    [SerializeField] private Transform anchorObject;
    [SerializeField] private int leftAnchorOffset;
    [SerializeField] private int rightAnchorOffset;

    private DynamicAction lastKnownAction = null;
    private string lastKnownGroup = "";
    private int lastKnownFrame = -1;

    // Use this for initialization
    void Awake() {
        grid = GetComponent<UIGrid>();
    }

    void OnActionChanged(DynamicAction action)
    {
        //We only want to fire the update if the action actually changed, in case we fired an event earlier for the group or frame
        if (action != lastKnownAction)
            UpdateCard();
    }

    void OnGroupChanged(string s)
    {
        //We only want to fire the update if the action actually changed, in case we fired an event earlier for the action or frame
        if (s != lastKnownGroup)
            UpdateCard();
    }

    void OnFrameChanged(int frame)
    {
        //We only want to fire the update if the action actually changed, in case we fired an event earlier for the group or action
        if (frame != lastKnownFrame)
            UpdateCard();
    }

    void UpdateCard()
    {
        //If we're updating, set the flags so we don't update multiple times in a row
        lastKnownAction = editor.currentAction;
        lastKnownFrame = editor.currentFrame;
        lastKnownGroup = editor.subactionGroup;

        //Get rid of our old list
        foreach (GameObject child in children)
        {
            NGUITools.Destroy(child);
        }
        children.Clear(); //Empty the list for future use

        //Create all the new buttons
        DynamicAction action = LegacyEditorData.instance.currentAction;
        string subGroup = LegacyEditorData.instance.subactionGroup;
        if (subGroup == "Current Frame")
        {
            subGroup = SubactionGroup.ONFRAME(LegacyEditorData.instance.currentFrame);
        }
        if (action != null)
        {
            //Create all the new buttons
            foreach (SubactionData subData in action.subactionCategories.GetIfKeyExists(subGroup))
            {
                instantiateButton(subData);
            }

            //Realign the grid
            grid.Reposition();
            dragPanel.ResetPosition();
        }
    }

    private void instantiateButton(SubactionData subDataToSet)
    {
        GameObject go = NGUITools.AddChild(gameObject, SubactionCardPrefab);
        SubactionCard card = go.GetComponent<SubactionCard>();

        card.SetAnchors(anchorObject, leftAnchorOffset, rightAnchorOffset);

        card.SetSubaction(subDataToSet);
        card.SetEditor(editor);
        card.RegisterListeners();
        children.Add(go);
    }

    public override void RegisterListeners()
    {
        editor.CurrentActionChangedEvent += OnActionChanged;
        editor.GroupDropdownChangedEvent += OnGroupChanged;
        editor.CurrentFrameChangedEvent += OnFrameChanged;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentActionChangedEvent -= OnActionChanged;
        editor.GroupDropdownChangedEvent -= OnGroupChanged;
        editor.CurrentFrameChangedEvent -= OnFrameChanged;
    }
}
