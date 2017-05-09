using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SettingsPreset : MonoBehaviour {
    public enum ConflictType { SHARE,HOG,TRUMP }

    public string preset_name = "default";

    //Physics
    public float gravity_ratio = 1.0f;
    public float weight_ratio = 1.0f;
    public float friction_ratio = 1.0f;
    public float aircontrol_ratio = 1.0f;
    public float hitstun_ratio = 1.0f;
    public float hitlag_ratio = 1.0f;
    public float shieldstun_ratio = 1.0f;

    //Ledge Settings
    public ConflictType conflict_type = ConflictType.SHARE;
    public string sweetspot_size = "MEDIUM";
    public bool sweetspot_facing_only = false;
    public bool team_ledge_conflict = false;
    public int ledge_invulnerability = 60;
    public bool regrab_invulnerability = false;
    public int slow_wakeup_threshold = 100;

    //Air Dodge Settings
    public bool air_dodge_enabled = true;
    public bool directional_air_dodge = true;
    public bool enable_wavedash = true;
    public int air_dodge_lag = 16;

    //Respawn Settings
    public int respawn_downtime = 0;
    public int respawn_lifetime = 300;
    public int respawn_invuln = 120;

    //L-Cancel Settings
    public string cancel_type = "NONE";

    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(this);
        File.WriteAllText("Assets/Settings/Presets/" + preset_name+".json", json);
    }

    void LoadSettingsPreset(string new_preset)
    {
        //load settings from JSON
        if (File.Exists("Assets/Settings/Presets/"+new_preset+".json"))
        {
            string settings_json = File.ReadAllText("Assets/Settings/Presets/" + new_preset+".json");
            JsonUtility.FromJsonOverwrite(settings_json, this);
        }
    }
}
