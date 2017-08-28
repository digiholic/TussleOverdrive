using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Settings : MonoBehaviour {
    public enum ScreenType
    {
        Windowed,
        Fullscreen,
        Borderless
    }
    
    public enum ValidResolution
    {
        r1920x1200,
        r1920x1080,
        r1680x1050,
        r1600x900,
        r1440x900,
        r1366x768,
        r1360x768,
        r1280x1024,
        r1280x800,
        r1280x720,
        r1024x768
    }

    //Windows
    public ValidResolution screen_resolution;
    public ScreenType window_mode;

    //Sound
    public float music_volume = .50f;
    public float sfx_volume = .50f;

    //Debug
    public bool display_hitboxes  = false;
    public bool display_hurtboxes = false;
    public bool display_colliders = false;
    public bool display_platforms = false;

    //Player Colors
    public Color[] player_colors = new Color[4];

    //Physics
    public float gravity_ratio = 1.0f;
    public float weight_ratio = 1.0f;
    public float friction_ratio = 1.0f;
    public float aircontrol_ratio = 1.0f;
    public float hitstun_ratio = 1.0f;
    public float hitlag_ratio = 1.0f;
    public float shieldstun_ratio = 1.0f;

    //Ledge Settings
    public string conflict_type = "Share";
    public string sweetspot_size = "Medium";
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

    //Used for static access of settings
    public static Settings current_settings = null;

    //Private variables
    private AudioManager audio_manager;
    
    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(this);
        File.WriteAllText("Assets/Settings/settings.json",json);
    }

    public void LoadSettings()
    {
        //load settings from JSON
        if (File.Exists("Assets/Settings/settings.json"))
        {
            string settings_json = File.ReadAllText("Assets/Settings/settings.json");
            JsonUtility.FromJsonOverwrite(settings_json, this);
        }
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (current_settings == null) //if we don't have a settings object
        {
            LoadSettings();
            current_settings = this;
        }
        else //if it's already set
        {
            Destroy(gameObject); //Destroy the new one
        }
    }

    void Start()
    {
        audio_manager = GetComponent<AudioManager>();
    }

    public void ChangeResolution()
    {
        bool fullscreen = true;
        if (window_mode == ScreenType.Windowed)
            fullscreen = false;

        switch (screen_resolution)
        {
            case ValidResolution.r1920x1200:
                Screen.SetResolution(1920, 1200, fullscreen);
                break;
            case ValidResolution.r1920x1080:
                Screen.SetResolution(1920, 1080, fullscreen);
                break;
            case ValidResolution.r1680x1050:
                Screen.SetResolution(1680, 1050, fullscreen);
                break;
            case ValidResolution.r1600x900:
                Screen.SetResolution(1600, 900, fullscreen);
                break;
            case ValidResolution.r1440x900:
                Screen.SetResolution(1440, 900, fullscreen);
                break;
            case ValidResolution.r1366x768:
                Screen.SetResolution(1366, 768, fullscreen);
                break;
            case ValidResolution.r1360x768:
                Screen.SetResolution(1360, 768, fullscreen);
                break;
            case ValidResolution.r1280x1024:
                Screen.SetResolution(1280, 1024, fullscreen);
                break;
            case ValidResolution.r1280x800:
                Screen.SetResolution(1280, 800, fullscreen);
                break;
            case ValidResolution.r1280x720:
                Screen.SetResolution(1280, 720, fullscreen);
                break;
            case ValidResolution.r1024x768:
                Screen.SetResolution(1024, 768, fullscreen);
                break;
        }
    }

    public void ChangeSetting(string setting_name,object value)
    {
        switch (setting_name)
        {
            case "gravity_ratio":
                gravity_ratio = ((int)value / 100.0f);
                break;
            case "weight_ratio":
                weight_ratio = ((int)value / 100.0f);
                break;
            case "friction_ratio":
                friction_ratio = ((int)value / 100.0f);
                break;
            case "aircontrol_ratio":
                aircontrol_ratio = ((int)value / 100.0f);
                break;
            case "hitstun_ratio":
                hitstun_ratio = ((int)value / 100.0f);
                break;
            case "hitlag_ratio":
                hitlag_ratio = ((int)value / 100.0f);
                break;
            case "shieldstun_ratio":
                shieldstun_ratio = ((int)value / 100.0f);
                break;
            case "conflict_type":
                conflict_type = value.ToString();
                break;
            case "sweetspot_size":
                sweetspot_size = value.ToString();
                break;
            case "sweetspot_facing_only":
                sweetspot_facing_only = (value.ToString() == "Yes");
                break;
            case "team_ledge_conflict":
                team_ledge_conflict = (value.ToString() == "Yes");
                break;
            case "ledge_invulnerability":
                ledge_invulnerability = (int)value;
                break;
            case "regrab_invulnerability":
                regrab_invulnerability = (value.ToString() == "Yes");
                break;
            case "slow_wakeup_threshold":
                slow_wakeup_threshold = (int)value;
                break;
            case "air_dodge_enabled":
                air_dodge_enabled = (value.ToString() == "Yes");
                break;
            case "directional_air_dodge":
                directional_air_dodge = (value.ToString() == "Directional");
                break;
            case "enable_wavedash":
                enable_wavedash = (value.ToString() == "Yes");
                break;
            case "air_dodge_lag":
                air_dodge_lag = (int)value;
                break;
            case "cancel_type":
                cancel_type = value.ToString();
                break;
            default:
                return;
        }
    }

    public object GetSetting(string setting_name)
    {
        switch (setting_name)
        {
            case "gravity_ratio":
                return gravity_ratio;
            case "weight_ratio":
                return weight_ratio;
            case "friction_ratio":
                return friction_ratio;
            case "aircontrol_ratio":
                return aircontrol_ratio;
            case "hitstun_ratio":
                return hitstun_ratio;
            case "hitlag_ratio":
                return hitlag_ratio;
            case "shieldstun_ratio":
                return shieldstun_ratio;
            case "conflict_type":
                return conflict_type;
            case "sweetspot_size":
                return sweetspot_size;
            case "sweetspot_facing_only":
                return sweetspot_facing_only;
            case "team_ledge_conflict":
                return team_ledge_conflict;
            case "ledge_invulnerability":
                return ledge_invulnerability;
            case "regrab_invulnerability":
                return regrab_invulnerability;
            case "slow_wakeup_threshold":
                return slow_wakeup_threshold;
            case "air_dodge_enabled":
                return air_dodge_enabled;
            case "directional_air_dodge":
                return directional_air_dodge;
            case "enable_wavedash":
                return enable_wavedash;
            case "air_dodge_lag":
                return air_dodge_lag;
            case "cancel_type":
                return cancel_type;
            default:
                return null;
        }
    }
}