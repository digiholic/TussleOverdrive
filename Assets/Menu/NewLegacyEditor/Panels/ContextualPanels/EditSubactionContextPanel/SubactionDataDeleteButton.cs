using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionDataDeleteButton : MonoBehaviour {

    public void OnAction()
    {
        DeleteSubactionData legacyAction = ScriptableObject.CreateInstance<DeleteSubactionData>();
        legacyAction.init(LegacyEditorData.instance.currentSubaction);
        LegacyEditorData.instance.DoAction(legacyAction);
    }
}
