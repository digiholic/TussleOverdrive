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
    public bool display_hitboxes = false;
    public bool display_hurtboxes = false;
    public bool display_colliders = false;
    public bool display_platforms = false;

    //Player Colors
    //public string[] player_colors = new string[] { "#f54e4e", "#4e54f5", "#f5f54e", "#67f54e" };
    public Color[] player_colors = new Color[4];

    public SettingsPreset preset;

    //Used for static access of settings
    public static Settings current_settings = null;

    //Private variables
    private AudioManager audio_manager;
    
    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(this);
        File.WriteAllText("Assets/Settings/settings.json",json);
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (current_settings == null) //if we don't have a settings object
        {
            //load settings from JSON
            if (File.Exists("Assets/Settings/settings.json"))
            {
                string settings_json = File.ReadAllText("Assets/Settings/settings.json");
                JsonUtility.FromJsonOverwrite(settings_json, this);
            }

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
        preset = GetComponent<SettingsPreset>();
    }

    public void ChangeBGMVolume(float vol)
    {
        music_volume = vol;
    }

    public void ChangeSFXVolume(float vol)
    {
        sfx_volume = vol;
    }

    public void ChangeHitboxDisplay(bool check)
    {
        display_hitboxes = check;
    }

    public void ChangeHurtboxDisplay(bool check)
    {
        display_hurtboxes = check;
    }

    public void ChangeColliderDisplay(bool check)
    {
        display_colliders = check;
    }

    public void ChangePlatformDisplay(bool check)
    {
        display_platforms = check;
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
                break;
            case ValidResolution.r1680x1050:
                break;
            case ValidResolution.r1600x900:
                break;
            case ValidResolution.r1440x900:
                break;
            case ValidResolution.r1366x768:
                break;
            case ValidResolution.r1360x768:
                break;
            case ValidResolution.r1280x1024:
                break;
            case ValidResolution.r1280x800:
                break;
            case ValidResolution.r1280x720:
                break;
            case ValidResolution.r1024x768:
                break;
        }
    }
}