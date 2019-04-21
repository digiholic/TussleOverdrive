using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionCard : LegacyEditorWidget {
    [SerializeField]
    private UISprite selectedBg;
    [SerializeField]
    private UISprite unselectedBg;
    [SerializeField]
    private UILabel label;

    public SubactionData subaction = null;

	// Use this for initialization
	void Start () {

	}
	
    void OnSubactionChanged(SubactionData data)
    {
        if (subaction != null) //If the subaction is null it should always remain off
        {
            if (data == subaction)
            {
                NGUITools.SetActive(selectedBg.gameObject, true);
                NGUITools.SetActive(unselectedBg.gameObject, false);
            }
            else
            {
                NGUITools.SetActive(selectedBg.gameObject, false);
                NGUITools.SetActive(unselectedBg.gameObject, true);
            }
        }
    }

    public void SetSubaction(SubactionData subactionToSet)
    {
        Debug.Log("Setting button for subaction: " + subactionToSet.SubactionName,this);
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
