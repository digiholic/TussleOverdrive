using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionCreateButton : MonoBehaviour {
    private UILabel label;

    public SubactionDataDefault subData;

    private void OnEnable()
    {
        label = GetComponentInChildren<UILabel>();
    }

    public void SetAction(SubactionDataDefault subDataToSet)
    {
        subData = subDataToSet;
        label.text = subData.SubactionName;
        CreateNewSubaction legacyAction = ScriptableObject.CreateInstance<CreateNewSubaction>();
        legacyAction.init(subDataToSet);
        GetComponent<OnClickSendAction>().action = legacyAction;
    }
}
