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
            info.GenerateMissingAttributes(); //In case new variables need to be initialized since the fighter was created
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

    public void GenerateDefaultAttributes()
    {
        variables = new List<VarData>();
        variables.Add(new VarData(TussleConstants.FighterAttributes.MAX_JUMPS, "1", VarType.INT));
        variables.Add(new VarData(TussleConstants.FighterAttributes.WEIGHT, "100", VarType.FLOAT));
        variables.Add(new VarData(TussleConstants.FighterAttributes.GRAVITY, "-5", VarType.FLOAT));
        variables.Add(new VarData(TussleConstants.FighterAttributes.MAX_FALL_SPEED, "-20", VarType.FLOAT));
        variables.Add(new VarData(TussleConstants.FighterAttributes.WALK_SPEED, "7", VarType.FLOAT));
        variables.Add(new VarData(TussleConstants.FighterAttributes.RUN_SPEED, "11", VarType.FLOAT));
        variables.Add(new VarData(TussleConstants.FighterAttributes.MAX_AIR_SPEED, "5.5", VarType.FLOAT));
        variables.Add(new VarData(TussleConstants.FighterAttributes.DODGE_SPEED, "8.5", VarType.FLOAT));
        variables.Add(new VarData(TussleConstants.FighterAttributes.FRICTION, "0.3", VarType.FLOAT));
        variables.Add(new VarData(TussleConstants.FighterAttributes.AIR_RESISTANCE, "0.2", VarType.FLOAT));
        variables.Add(new VarData(TussleConstants.FighterAttributes.AIR_CONTROL, "0.2", VarType.FLOAT));
        variables.Add(new VarData(TussleConstants.FighterAttributes.JUMP_HEIGHT, "12.5", VarType.FLOAT));
        variables.Add(new VarData(TussleConstants.FighterAttributes.SHORT_HOP_HEIGHT, "8.5", VarType.FLOAT));
        variables.Add(new VarData(TussleConstants.FighterAttributes.AIR_JUMP_HEIGHT, "15", VarType.FLOAT));
        variables.Add(new VarData(TussleConstants.FighterAttributes.FASTFALL_MULTIPLIER, "2", VarType.FLOAT));
    }

    /// <summary>
    /// As attributes change, fighters might need to have defaults set for other code to work. This method is to make older fighters backwards compatible with things that
    /// are needed later on.
    /// </summary>
    public void GenerateMissingAttributes(){
        Dictionary<string,VarData> varDict = new Dictionary<string, VarData>();
        foreach (VarData var in variables){
            varDict.Add(var.name,var);
        }
        foreach (KeyValuePair<string,VarData> defaultData in AbstractFighter.DefaultVarDataStats){
            if (!varDict.ContainsKey(defaultData.Key)){
                variables.Add(defaultData.Value);
            }
        }
    }
}
