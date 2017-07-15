using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour {
    public static ActionButton selected_action = null;

    private bool active = false;
    private string actionName;

    public void LoadAction(string action, int number)
    {
        UILabel label = transform.Find("Label").GetComponent<UILabel>();
        label.text = action;
        actionName = action;
        Vector3 pos = transform.localPosition;
        pos.y -= (40 * number);
        transform.localPosition = pos;
    }

    public void Activate()
    {
        //If there's a selected button, deactivate it
        if (ActionButton.selected_action != null)
            ActionButton.selected_action.Deactivate();
        active = true;
        ToggleBackground();
        transform.root.BroadcastMessage("ActionChanged", actionName);
        ActionButton.selected_action = this;
    }

    public void Deactivate()
    {
        active = false;
        ToggleBackground();
    }

    public void ToggleBackground()
    {
        Transform bgChild = transform.Find("Background");
        UISprite sprite = bgChild.GetComponent<UISprite>();
        sprite.enabled = !sprite.enabled;
    }
}
