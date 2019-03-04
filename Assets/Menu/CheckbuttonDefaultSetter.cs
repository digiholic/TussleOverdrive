using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckbuttonDefaultSetter : MonoBehaviour {
    public enum CheckValueList
    {
        DisplayHitboxes,
        DisplayHurtboxes,
        DisplayColliders,
        DisplayPlatforms
    }

    public CheckValueList value;
    private UIToggle checkbox;

	// Use this for initialization
	void Start () {
        checkbox = GetComponent<UIToggle>();
        if (value == CheckValueList.DisplayHitboxes)
            checkbox.value = Settings.current_settings.display_hitboxes;
        else if (value == CheckValueList.DisplayHurtboxes)
            checkbox.value = Settings.current_settings.display_hurtboxes;
        else if (value == CheckValueList.DisplayColliders)
            checkbox.value = Settings.current_settings.display_colliders;
        else if (value == CheckValueList.DisplayPlatforms)
            checkbox.value = Settings.current_settings.display_platforms;
    }
}
