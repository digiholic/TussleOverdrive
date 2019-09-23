using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionCard : LegacyEditorWidget {
    [SerializeField] private UISprite selectedBg;
    [SerializeField] private UILabel label;

    public SubactionData subaction = null;
    public int index = 0;

    private SubactionCardRig rig;

    private void Awake()
    {
        selectedBg = GetComponent<UISprite>();
        rig = GetComponentInParent<SubactionCardRig>();
    }

    void OnSubactionChanged(SubactionData data)
    {
        if (subaction != null) //If the subaction is null it should always remain off
        {
            if (data == subaction)
            {
                selectedBg.spriteName = "selected-action-bg";
            }
            else
            {
                selectedBg.spriteName = "unselected-action-bg";
            }
        }
    }

    public void SetAnchors(Transform anchorObject,int leftAnchorOffset, int rightAnchorOffset)
    {
        selectedBg.leftAnchor.target = anchorObject;
        selectedBg.leftAnchor.relative = 0;
        selectedBg.leftAnchor.absolute = leftAnchorOffset;
        
        selectedBg.rightAnchor.target = anchorObject;
        selectedBg.rightAnchor.relative = 1;
        selectedBg.rightAnchor.absolute = rightAnchorOffset;
    }

    public void SetSubaction(SubactionData subactionToSet)
    {
        subaction = subactionToSet;
        label.text = subaction.SubactionName;
        ModifyLegacyEditorDataAction legacyAction = ScriptableObject.CreateInstance<ModifyLegacyEditorDataAction>();
        legacyAction.init("currentSubaction", subactionToSet);
        legacyAction.enableDeselect();

        GetComponent<OnClickSendAction>().action = legacyAction;
    }
    public void setIndex(int index)
    {
        int oldIndex = this.index;
        this.index = index;
    }


    public void OnDragDropStart()
    {
        Debug.Log("Dragging");
        selectedBg.depth = 303;
        selectedBg.alpha = 0.5f;
        label.depth = 304;
        label.alpha = 0.5f;
    }

    public void OnDragDropRelease()
    {
        Debug.Log("Done Dragging");
        selectedBg.depth = 301;
        selectedBg.alpha = 1f;
        label.depth = 302;
        label.alpha = 1f;

        //rig.UpdateOrder();
    }

    public override void RegisterListeners()
    {
        editor.CurrentSubactionChangedEvent += OnSubactionChanged;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentSubactionChangedEvent -= OnSubactionChanged;
    }
}
