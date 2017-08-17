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
        
        PlayerPrefs.SetString("control_attack_0", "Z");
        PlayerPrefs.SetString("control_special_0", "X");
        PlayerPrefs.SetString("control_jump_0", "C");
        PlayerPrefs.SetString("control_shield_0", "A");
        PlayerPrefs.SetString("control_left_0", "LeftArrow");
        PlayerPrefs.SetString("control_right_0", "RightArrow");
        PlayerPrefs.SetString("control_up_0", "UpArrow");
        PlayerPrefs.SetString("control_down_0", "DownArrow");

        PlayerPrefs.SetString("control_attack_1", "U");
        PlayerPrefs.SetString("control_special_1", "Y");
        PlayerPrefs.SetString("control_jump_1", "Semicolon");
        PlayerPrefs.SetString("control_shield_1", "O");
        PlayerPrefs.SetString("control_left_1", "J");
        PlayerPrefs.SetString("control_right_1", "L");
        PlayerPrefs.SetString("control_up_1", "I");
        PlayerPrefs.SetString("control_down_1", "K");

        PlayerPrefs.SetString("control_attack_2", "Alpha1");
        PlayerPrefs.SetString("control_special_2", "Alpha2");
        PlayerPrefs.SetString("control_jump_2", "Alpha3");
        PlayerPrefs.SetString("control_shield_2", "Alpha4");
        PlayerPrefs.SetString("control_left_2", "Alpha5");
        PlayerPrefs.SetString("control_right_2", "Alpha6");
        PlayerPrefs.SetString("control_up_2", "Alpha7");
        PlayerPrefs.SetString("control_down_2", "Alpha8");

        PlayerPrefs.SetString("control_attack_3", "Alpha1");
        PlayerPrefs.SetString("control_special_3", "Alpha2");
        PlayerPrefs.SetString("control_jump_3", "Alpha3");
        PlayerPrefs.SetString("control_shield_3", "Alpha4");
        PlayerPrefs.SetString("control_left_3", "Alpha5");
        PlayerPrefs.SetString("control_right_3", "Alpha6");
        PlayerPrefs.SetString("control_up_3", "Alpha7");
        PlayerPrefs.SetString("control_down_3", "Alpha8");
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
}