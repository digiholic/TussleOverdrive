using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings : MonoBehaviour {
    //Windows

    //Sound
    public float music_volume = 50.0f;
    public float sfx_volume = 50.0f;

    //Debug
    public bool display_hitboxes = false;
    public bool display_hurtboxes = false;
    public bool display_colliders = false;
    public bool display_platforms = false;

    //Player Colors
    public string[] player_colors = new string[] { "#f54e4e", "#4e54f5", "#f5f54e", "#67f54e" };

    //Physics
    public float gravity_ratio = 1.0f;
    public float weight_ratio = 1.0f;
    public float friction_ratio = 1.0f;
    public float aircontrol_ratio = 1.0f;
    public float hitstun_ratio = 1.0f;
    public float hitlag_ratio = 1.0f;
    public float shieldstun_ratio = 1.0f;

    //Ledge Settings
    public string conflict_type = "SHARE";
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

    public void saveSettings()
    {
        string json = JsonUtility.ToJson(this);
        System.IO.File.WriteAllText("Assets/Settings/settings.json",json);
    }
}