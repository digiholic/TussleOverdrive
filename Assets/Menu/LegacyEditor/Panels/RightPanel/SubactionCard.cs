using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionCard : LegacyEditorWidget {
    [SerializeField] private UISprite selectedBg;
    [SerializeField] private UILabel label;

    public SubactionData subaction = null;

    private void Awake()
    {
        selectedBg = GetComponent<UISprite>();    
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
        /*
        selectedBg.leftAnchor.target = anchorObject;
        selectedBg.leftAnchor.relative = 0;
        selectedBg.leftAnchor.absolute = leftAnchorOffset;

        selectedBg.rightAnchor.target = anchorObject;
        selectedBg.rightAnchor.relative = 1;
        selectedBg.rightAnchor.absolute = rightAnchorOffset;
        */
    }

    public void SetSubaction(SubactionData subactionToSet)
    {
        subaction = subactionToSet;
        label.text = subaction.SubactionName;
        ChangeCurrentSubaction legacyAction = ScriptableObject.CreateInstance<ChangeCurrentSubaction>();
        legacyAction.init(subactionToSet);
        GetComponent<OnClickSendAction>().action = legacyAction;
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
