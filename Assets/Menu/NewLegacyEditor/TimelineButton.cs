using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineButton : MonoBehaviour {
    public enum TimelineButtonType
    {
        FIRST, PREVIOUS, NEXT, LAST
    }
    public TimelineButtonType buttonType;

    void OnAction()
    {
        ChangeCurrentFrame legacyAction = ScriptableObject.CreateInstance<ChangeCurrentFrame>();

        switch (buttonType)
        {
            case TimelineButtonType.FIRST:
                legacyAction.init(0, false, false);
                break;
            case TimelineButtonType.PREVIOUS:
                legacyAction.init(-1, true, false);
                break;
            case TimelineButtonType.NEXT:
                legacyAction.init(1, true, false);
                break;
            case TimelineButtonType.LAST:
                legacyAction.init(9999, false, true);
                break;
        }
        LegacyEditorData.instance.DoAction(legacyAction);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
