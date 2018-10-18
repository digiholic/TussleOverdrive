using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionCard : MonoBehaviour {
    [SerializeField]
    private UISprite selectedBg;
    [SerializeField]
    private UISprite unselectedBg;
    [SerializeField]
    private UILabel label;

    public SubactionData subaction;

	// Use this for initialization
	void Start () {
        SubactionData sub = ScriptableObject.CreateInstance<SubactionData>();
        sub.SubactionName = gameObject.name;
        sub.arguments = new SubVarDict();
        sub.arguments.Add("Argument 1", new SubactionVarData("Argument 1", SubactionSource.OWNER, SubactionVarType.STRING, "Foo", true));
        SetSubaction(sub);
	}
	
    void OnModelChanged()
    {
        if (subaction != null) //If the subaction is null it should always remain off
        {
            if (LegacyEditorData.instance.currentSubaction == subaction)
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
        subaction = subactionToSet;
        label.text = subaction.SubactionName;
        ChangeCurrentSubaction legacyAction = ScriptableObject.CreateInstance<ChangeCurrentSubaction>();
        legacyAction.init(subactionToSet);
        GetComponent<OnClickSendAction>().action = legacyAction;
    }
}
