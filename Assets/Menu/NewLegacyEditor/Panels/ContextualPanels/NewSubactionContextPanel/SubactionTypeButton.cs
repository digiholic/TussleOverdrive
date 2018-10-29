using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionTypeButton : MonoBehaviour {
    private UILabel label;

    public SubactionType subtype;
    public GameObject selectedSprite;
    public GameObject unselectedSprite;

    // Use this for initialization
    void OnEnable () {
        label = GetComponentInChildren<UILabel>();
        OnContextualPanelChanged();
	}
	
    void OnContextualPanelChanged()
    {
        //Only execute if it's the right kind of contextual panel
        if (ContextualPanelData.isOfType(typeof(NewSubactionContextPanel)))
        {
            NewSubactionContextPanel panel = (NewSubactionContextPanel)LegacyEditorData.contextualPanel;
            if (panel.selectedType == subtype)
            {
                NGUITools.SetActive(selectedSprite, true);
                NGUITools.SetActive(unselectedSprite, false);
            } else
            {
                NGUITools.SetActive(selectedSprite, false);
                NGUITools.SetActive(unselectedSprite, true);
            }
        }
    }

    public void SetSubType(SubactionType sub)
    {
        subtype = sub;
        label.text = SubactionUtilities.TypeToString(sub);
        NewSubactionChangeSubactionType legacyAction = ScriptableObject.CreateInstance<NewSubactionChangeSubactionType>();
        legacyAction.init(sub);
        GetComponent<OnClickSendAction>().action = legacyAction;
    }
}
