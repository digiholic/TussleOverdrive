using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDeleteButton : MonoBehaviour {

    void OnAction()
    {
        DeleteAction action = ScriptableObject.CreateInstance<DeleteAction>();
        action.init(LegacyEditorData.instance.currentAction);
        LegacyEditorData.instance.DoAction(action);
    }
}
