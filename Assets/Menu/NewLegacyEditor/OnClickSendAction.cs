using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickSendAction : MonoBehaviour {
    public LegacyEditorAction action = null;

    public void SetAction(LegacyEditorAction legacyAction)
    {
        Debug.Log(legacyAction);
        action = legacyAction;
    }

    void OnAction()
    {
        if (action != null)
        {
            LegacyEditorData.instance.DoAction(action);
        } else
        {
            Debug.LogError("Calling OnClick without setting an action to perform!");
        }
    }
}
