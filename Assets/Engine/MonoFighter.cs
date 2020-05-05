using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is essentially a fighter info, but as a monobehavior. This lets us set stuff in the editor for events, while keeping the serialized class seperate.
/// think of it kinda like a factory object but using the Unity editor.
/// </summary>
public class MonoFighter : MonoBehaviour
{
    [SerializeField] private string display_name;
    [SerializeField] private string franchise_icon_path;
    [SerializeField] private string css_icon_path;
    [SerializeField] private string css_portrait_path;
    [SerializeField] private string action_file_path;
    [SerializeField] private string sprite_info_path;
    [SerializeField] private string sound_path;

    [SerializeField] private string weight;
    [SerializeField] private string gravity;
    [SerializeField] private string walkSpeed;
    [SerializeField] private string runSpeed;
    [SerializeField] private string friction;

    [SerializeField] private string airJumps;
    [SerializeField] private string jumpHeight;
    [SerializeField] private string shortHopHeight;
    [SerializeField] private string maxFallSpeed;
    [SerializeField] private string fastFallFactor;

    [SerializeField] private string maxAirSpeed;
    [SerializeField] private string airControl;
    [SerializeField] private string airResistance;

    public List<VarData> variables;

    public TextAsset json_file;

    public string Display_name { get => display_name; set => display_name = value; }
    public string Franchise_icon_path { get => franchise_icon_path; set => franchise_icon_path = value; }
    public string Css_icon_path { get => css_icon_path; set => css_icon_path = value; }
    public string Css_portrait_path { get => css_portrait_path; set => css_portrait_path = value; }
    public string Action_file_path { get => action_file_path; set => action_file_path = value; }
    public string Sprite_info_path { get => sprite_info_path; set => sprite_info_path = value; }
    public string Sound_path { get => sound_path; set => sound_path = value; }

    public string Weight { get => weight; set => weight = value; }
    public string Gravity { get => gravity; set => gravity = value; }
    public string WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
    public string RunSpeed { get => runSpeed; set => runSpeed = value; }
    public string Friction { get => friction; set => friction = value; }
    public string AirJumps { get => airJumps; set => airJumps = value; }
    public string JumpHeight { get => jumpHeight; set => jumpHeight = value; }
    public string ShortHopHeight { get => shortHopHeight; set => shortHopHeight = value; }
    public string MaxFallSpeed { get => maxFallSpeed; set => maxFallSpeed = value; }
    public string FastFallFactor { get => fastFallFactor; set => fastFallFactor = value; }
    public string MaxAirSpeed { get => maxAirSpeed; set => maxAirSpeed = value; }
    public string AirControl { get => airControl; set => airControl = value; }
    public string AirResistance { get => airResistance; set => airResistance = value; }

    public FighterInfo getFighterInfo()
    {
        FighterInfo info = new FighterInfo();
        info.displayName = display_name;
        info.franchiseIconPath = franchise_icon_path;
        info.cssIconPath = css_icon_path;
        info.cssPortraitPath = css_portrait_path;
        info.actionFilePath = action_file_path;
        info.spriteInfoPath = sprite_info_path;
        info.soundPath = sound_path;

        info.Weight = float.Parse(Weight);
        info.Gravity = float.Parse(Gravity);
        info.WalkSpeed = float.Parse(WalkSpeed);
        info.RunSpeed = float.Parse(RunSpeed);
        info.Friction = float.Parse(Friction);
        info.AirJumps = int.Parse(AirJumps);
        info.JumpHeight = float.Parse(JumpHeight);
        info.ShortHopHeight = float.Parse(ShortHopHeight);
        info.MaxFallSpeed = float.Parse(MaxFallSpeed);
        info.FastFallFactor = float.Parse(FastFallFactor);
        info.MaxAirSpeed = float.Parse(MaxAirSpeed);
        info.AirControl = float.Parse(AirControl);
        info.AirResistance = float.Parse(AirResistance);

        info.variables = variables;

        return info;
    }

    public void fromFighterInfo(FighterInfo info)
    {
        display_name = info.displayName;
        franchise_icon_path = info.franchiseIconPath;
        css_icon_path = info.cssIconPath;
        css_portrait_path = info.cssPortraitPath;
        action_file_path = info.actionFilePath;
        sprite_info_path = info.spriteInfoPath;
        sound_path = info.soundPath;

        Weight = info.Weight.ToString();
        Gravity = info.Gravity.ToString();
        WalkSpeed = info.WalkSpeed.ToString();
        RunSpeed = info.RunSpeed.ToString();
        Friction = info.Friction.ToString();
        AirJumps = info.AirJumps.ToString();
        JumpHeight = info.JumpHeight.ToString();
        ShortHopHeight = info.ShortHopHeight.ToString();
        MaxFallSpeed = info.MaxFallSpeed.ToString();
        MaxAirSpeed = info.MaxAirSpeed.ToString();
        AirControl = info.AirControl.ToString();
        AirResistance = info.AirResistance.ToString();

        variables = info.variables;
    }

    void Awake()
    {
        if (json_file != null)
        {
            LoadFighter();
        }
    }

    public void LoadFighter()
    {
        FighterInfo fighter_info = JsonUtility.FromJson<FighterInfo>(json_file.text);
        fromFighterInfo(fighter_info);
    }
}
