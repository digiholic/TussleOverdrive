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
    private UICheckbox checkbox;

	// Use this for initialization
	void Start () {
        checkbox = GetComponent<UICheckbox>();
        if (value == CheckValueList.DisplayHitboxes)
            checkbox.isChecked = Settings.current_settings.display_hitboxes;
        else if (value == CheckValueList.DisplayHurtboxes)
            checkbox.isChecked = Settings.current_settings.display_hurtboxes;
        else if (value == CheckValueList.DisplayColliders)
            checkbox.isChecked = Settings.current_settings.display_colliders;
        else if (value == CheckValueList.DisplayPlatforms)
            checkbox.isChecked = Settings.current_settings.display_platforms;
    }
}
