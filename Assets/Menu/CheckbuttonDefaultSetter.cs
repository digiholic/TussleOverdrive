using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckbuttonDefaultSetter : MonoBehaviour {
    public enum CheckValueList
    {
        DisplayHitboxes,
        DisplayHurtboxes,
        DisplayColliders,
        DisplayPlatforms
    }

    public CheckValueList value;
    private Toggle checkbox;
    
	// Use this for initialization
	void Start () {
        checkbox = GetComponent<Toggle>();
        if (value == CheckValueList.DisplayHitboxes)
            checkbox.isOn = Settings.current_settings.display_hitboxes;
        else if (value == CheckValueList.DisplayHurtboxes)
            checkbox.isOn = Settings.current_settings.display_hurtboxes;
        else if (value == CheckValueList.DisplayColliders)
            checkbox.isOn = Settings.current_settings.display_colliders;
        else if (value == CheckValueList.DisplayPlatforms)
            checkbox.isOn = Settings.current_settings.display_platforms;
    }
}
