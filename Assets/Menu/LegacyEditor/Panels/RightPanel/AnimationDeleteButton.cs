using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDeleteButton : MonoBehaviour
{
    public void OnAction()
    {
        DeleteAnimation action = ScriptableObject.CreateInstance<DeleteAnimation>();
        action.init(LegacyEditorData.instance.currentAnimation);
        LegacyEditorData.instance.DoAction(action);
    }
}
