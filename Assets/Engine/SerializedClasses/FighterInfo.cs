using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.U2D;

[System.Serializable]
public class FighterInfo : IJsonInfoObject{

    [SerializeField] private string display_name;
    [SerializeField] private string franchise_icon_path;
    [SerializeField] private string css_icon_path;
    [SerializeField] private string css_portrait_path;
    [SerializeField] private string action_file_path;
    [SerializeField] private string sprite_info_path;
    [SerializeField] private string sound_path;

    [SerializeField] private float weight = 100;
    [SerializeField] private float gravity = -5;
    [SerializeField] private float walkSpeed = 7;
    [SerializeField] private float runSpeed = 11;
    [SerializeField] private float friction = 0.3f;

    [SerializeField] private int airJumps = 1;
    [SerializeField] private float jumpHeight = 12.5f;
    [SerializeField] private float shortHopHeight = 8.5f;
    [SerializeField] private float maxFallSpeed = -20;
    [SerializeField] private float fastFallFactor = 2.0f;

    [SerializeField] private float maxAirSpeed = 5.5f;
    [SerializeField] private float airControl = 0.2f;
    [SerializeField] private float airResistance = 0.2f;

    public string displayName{
        get {
            return display_name;
        }
        set{
            display_name = value;
        }
    }
    public string franchiseIconPath{
        get {
            return franchise_icon_path;
        }
        set{
            franchise_icon_path = value;
            if (directory_name != null){
                franchise_icon_sprite = FileLoader.LoadSprite(FileLoader.PathCombine(FileLoader.GetFighterPath(directory_name),franchiseIconPath));
            }
        }
    }
    public string cssIconPath{
        get {
            return css_icon_path;
        }
        set {
            css_icon_path = value;
            if (directory_name != null){
                css_icon_sprite = FileLoader.LoadSprite(FileLoader.PathCombine(FileLoader.GetFighterPath(directory_name),css_icon_path));
            }
        }
    }
    public string cssPortraitPath{
        get {
            return css_portrait_path;
        }
        set{
            css_portrait_path = value;
            if (directory_name != null){
                css_portrait_sprite = FileLoader.LoadSprite(FileLoader.PathCombine(FileLoader.GetFighterPath(directory_name),css_portrait_path));
            }
        }
    }
    public string actionFilePath{
        get {
            return action_file_path;
        }
        set {
            action_file_path = value;
            if (directory_name != null){
                string action_file_json = FileLoader.LoadTextFile(FileLoader.PathCombine(FileLoader.GetFighterPath(directory_name), action_file_path));
                action_file = JsonUtility.FromJson<ActionFile>(action_file_json);
            }
        }
    }
    public string spriteInfoPath{
        get{
            return sprite_info_path;
        }
        set {
            sprite_info_path = value;
            if (directory_name != null){
                string sprite_info_json = FileLoader.LoadTextFile(FileLoader.PathCombine(FileLoader.GetFighterPath(directory_name), sprite_info_path));
                sprite_info = JsonUtility.FromJson<SpriteInfo>(sprite_info_json);
                sprite_info.LoadDirectory(directory_name,this);
            }
        }
    }
    public string soundPath{
        get {
            return sound_path;
        }
        set {
            sound_path = value;
        }
    }

    public float Weight { get => weight; set => weight = value; }
    public float Gravity { get => gravity; set => gravity = value; }
    public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
    public float RunSpeed { get => runSpeed; set => runSpeed = value; }
    public float Friction { get => friction; set => friction = value; }
    public int AirJumps { get => airJumps; set => airJumps = value; }
    public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
    public float ShortHopHeight { get => shortHopHeight; set => shortHopHeight = value; }
    public float MaxFallSpeed { get => maxFallSpeed; set => maxFallSpeed = value; }
    public float FastFallFactor { get => fastFallFactor; set => fastFallFactor = value; }
    public float MaxAirSpeed { get => maxAirSpeed; set => maxAirSpeed = value; }
    public float AirControl { get => airControl; set => airControl = value; }
    public float AirResistance { get => airResistance; set => airResistance = value; }

    public List<VarData> variables;

    /// <summary>
    /// Directory name is not serialized, and is set when loaded so the program knows
    /// the folder name of the fighter, which might not match the fighter.
    /// </summary>
    [System.NonSerialized]
    public string directory_name;
    [System.NonSerialized]
    public Sprite franchise_icon_sprite;
    [System.NonSerialized]
    public Sprite css_icon_sprite;
    [System.NonSerialized]
    public Sprite css_portrait_sprite;
    [System.NonSerialized]
    public ActionFile action_file;
    [System.NonSerialized]
    public SpriteInfo sprite_info;
    [System.NonSerialized]
    public bool initialized = false;


    #region IJsonInfoObject Implementation
    [SerializeField]
    private TextAsset JSONFile;
    public FileInfo Save(string path)
    {
        FileInfo fileSavedTo = new FileInfo(path);
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(path, json);
        return fileSavedTo;
    }

    public FighterInfo Clone(){
        FighterInfo newInfo = new FighterInfo();
        newInfo.displayName = displayName;
        newInfo.franchiseIconPath = franchiseIconPath;
        newInfo.css_icon_path = css_icon_path;
        newInfo.css_portrait_path = css_portrait_path;
        newInfo.action_file_path = action_file_path;
        newInfo.sprite_info_path = sprite_info_path;
        newInfo.sound_path = sound_path;
        
        newInfo.variables = new List<VarData>();
        foreach (VarData var in variables){
            newInfo.variables.Add(var.Clone());
        }

        return newInfo;
    }
    /// <summary>
    /// Shortcut save method that will use the Fighter's Directory name and use the file name "fighter_info.json"
    /// </summary>
    /// <returns></returns>
    public FileInfo Save()
    {
        return Save(FileLoader.PathCombine(directory_name, "fighter_info.json"));
    }

    public void LoadFromTextAsset()
    {
        if (JSONFile != null)
        {
            TextAsset oldJSONFile = JSONFile;
            JsonUtility.FromJsonOverwrite(JSONFile.text, this);
            JSONFile = oldJSONFile;
        }
    }
    #endregion

    public void LoadDirectory(string directoryName)
    {
        directory_name = directoryName;
        string fullDirectory = FileLoader.GetFighterPath(directoryName);

        //Initialize the meta-level sprites like icons and whatnot
        franchise_icon_sprite = FileLoader.LoadSprite(FileLoader.PathCombine(fullDirectory,franchiseIconPath));
        css_icon_sprite = FileLoader.LoadSprite(FileLoader.PathCombine(fullDirectory, css_icon_path));
        css_portrait_sprite = FileLoader.LoadSprite(FileLoader.PathCombine(fullDirectory, css_portrait_path));

        //Load the external data json
        string action_file_json = FileLoader.LoadTextFile(FileLoader.PathCombine(fullDirectory, action_file_path));
        action_file = JsonUtility.FromJson<ActionFile>(action_file_json);
        string sprite_info_json = FileLoader.LoadTextFile(FileLoader.PathCombine(fullDirectory, sprite_info_path));
        sprite_info = JsonUtility.FromJson<SpriteInfo>(sprite_info_json);
        sprite_info.LoadDirectory(directoryName,this);

        //Mark the fighter info as initialized and ready
        initialized = true;
    }

    public static FighterInfo LoadFighterInfoFile(string directory, string filename="fighter_info.json")
    {
        string dir = FileLoader.GetFighterPath(directory);
        string combinedPath = Path.Combine(dir, filename);
        if (File.Exists(combinedPath))
        {
            string json = File.ReadAllText(combinedPath);
            FighterInfo info = JsonUtility.FromJson<FighterInfo>(json);
            //info.GenerateMissingAttributes(); //In case new variables need to be initialized since the fighter was created
            if (info.displayName == null) return null; //If it doesn't have a display name it's not a fighter
            info.LoadDirectory(directory);
            return info;
        }
        else
        {
            Debug.LogWarning("No fighter file found at " + directory + "/" + filename);
            return null;
        }
    }

    public VarData GetVarByName(string name)
    {
        foreach (VarData data in variables)
        {
            if (data.name == name) return data;
        }
        return null;
    }

    public void CreateOrUpdateVarData(VarData newData)
    {
        foreach (VarData data in variables)
        {
            if (data.name == newData.name)
            {
                data.type = newData.type;
                data.value = newData.value;
                return;
            }
        }
        variables.Add(newData);
    }
}
