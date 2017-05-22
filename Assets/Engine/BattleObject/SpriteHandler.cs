using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SpriteHandler : BattleComponent {

    public string directory;
    public string default_sprite;
    public string prefix;
    public float pixelsPerUnit = 100.0f;

    private Dictionary<string,List<Sprite>> sprites = new Dictionary<string,List<Sprite>>();
    private string current_sprite = "idle";
    private int current_frame = 0;

    private SpriteRenderer sprite_renderer;

    void LoadSpriteXML()
    {
        XMLLoader data_xml = GetComponent<XMLLoader>();

        if (data_xml != null)
        {
            string resource_path = data_xml.resource_path;
            string sprite_directory = data_xml.SelectSingleNode("//fighter/sprite_directory").GetString();

            directory = "Assets/Resources/" + resource_path + sprite_directory;
            prefix = data_xml.SelectSingleNode("//fighter/sprite_prefix").GetString();
            default_sprite = data_xml.SelectSingleNode("//fighter/default_sprite").GetString();
            pixelsPerUnit = float.Parse(data_xml.SelectSingleNode("//fighter/pixels_per_unit").GetString());

        }
    }

    // Use this for initialization
	void Start() {
        GameObject spriteComponent = new GameObject("Sprite");
        spriteComponent.transform.parent = transform;
        spriteComponent.transform.localPosition = Vector3.zero;
        sprite_renderer = spriteComponent.AddComponent<SpriteRenderer>();
    }

    public void Initialize()
    {
        LoadSpriteXML();
        DirectoryInfo info = new DirectoryInfo(directory);
        string sprite_json_path = Path.Combine(info.FullName, "sprites.json");
        
        if (File.Exists(sprite_json_path))
        {
            string sprite_json = File.ReadAllText(sprite_json_path);
            SpriteDataCollection sprite_list = JsonUtility.FromJson<SpriteDataCollection>(sprite_json);
            LoadSpritesFromData(sprite_list);
        }
        else
        {
            Debug.LogError("No sprites JSON found: "+sprite_json_path);
        }
    }

    public void LoadSpritesFromData(SpriteDataCollection sprite_list)
    {
        sprites = new Dictionary<string, List<Sprite>>();

        Dictionary<string, SpriteData> sprite_data_dict = new Dictionary<string, SpriteData>();
        
        foreach (SpriteData data in sprite_list.sprites)
        {
            sprite_data_dict[data.sprite_name] = data;
            string filename = prefix + data.sprite_name + ".png";
            Texture2D SpriteTexture = LoadTexture(Path.Combine(directory, filename));

            List<Sprite> spriteFrames = new List<Sprite>();
            foreach (Vector2 startPos in data.subimage)
            {
                Sprite newSprite = Sprite.Create(SpriteTexture, new Rect(startPos.x, startPos.y, data.sprite_size.x, data.sprite_size.y), new Vector2(0.5f, 0.5f), pixelsPerUnit);
                spriteFrames.Add(newSprite);
            }
            sprites.Add(data.sprite_name, spriteFrames);
        }
    }

    public void ChangeSprite(string _sprite_name)
    {
        if (_sprite_name == null)
        {
            Debug.LogWarning("No Sprite found");
            _sprite_name = "idle";
        }
        if (sprites.ContainsKey(_sprite_name))
        {
            current_sprite = _sprite_name;
            current_frame = 0;
            sprite_renderer.sprite = sprites[current_sprite][current_frame];
        }
        else
        {
            Debug.LogWarning("Attempted to load illegal sprite: " + _sprite_name);
        }
    }

    public void ChangeSubimage(int _frame)
    {
        if (_frame < 0)
            _frame += sprites[current_sprite].Count;
        current_frame = Mathf.Min(_frame, sprites[current_sprite].Count - 1);
        
        sprite_renderer.sprite = sprites[current_sprite][current_frame];
    }

    public void ChangeSubimage(int _frame, bool _loop)
    {
        if (_frame < 0)
            _frame += sprites[current_sprite].Count;
        if (_loop)
            current_frame = _frame % sprites[current_sprite].Count;
        else
        {
            current_frame = Mathf.Min(_frame, sprites[current_sprite].Count - 1);
        }
           

        sprite_renderer.sprite = sprites[current_sprite][current_frame];
    }

    private Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;
        
        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }

    public void RotateSprite(float degrees)
    {
        sprite_renderer.transform.Rotate(new Vector3(0, 0, degrees));
    }

    public void UnRotate()
    {
        sprite_renderer.transform.rotation = Quaternion.identity;
    }

    public void flip()
    {
        Vector3 transfVec = sprite_renderer.transform.localScale;
        transfVec.x *= -1;
        //sprite_renderer.transform.localScale = transfVec;
    }
}