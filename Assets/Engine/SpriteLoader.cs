using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpriteLoader : MonoBehaviour {

    public string directory;
    public int width;
    public string prefix;
    public float pixelsPerUnit = 100.0f;

    private Dictionary<string,List<Sprite>> sprites = new Dictionary<string,List<Sprite>>();
    private string current_sprite = "idle";
    private int current_frame = 0;

    private SpriteRenderer sprite_renderer;

    // Use this for initialization
	void Start() {
        AbstractFighter actor = GetComponent<AbstractFighter>();
        directory = actor.sprite_directory;
        prefix = actor.sprite_prefix;
        pixelsPerUnit = float.Parse(actor.pixels_per_unit);

        sprite_renderer = GetComponent<SpriteRenderer>();

        DirectoryInfo info = new DirectoryInfo(directory);
        FileInfo[] fileInfo = info.GetFiles();
        
        
        string sprite_json_path = Path.Combine(info.FullName, "sprites.json");
        
        if (File.Exists(sprite_json_path))
        {
            string sprite_json = File.ReadAllText(sprite_json_path);
            Debug.Log(sprite_json);

            SpriteDataCollection col = new SpriteDataCollection();
            Debug.Log(JsonUtility.ToJson(col));
            
            SpriteDataCollection sprite_list = JsonUtility.FromJson<SpriteDataCollection>(sprite_json);
            Debug.Log(sprite_list.sprites);

            Dictionary<string, SpriteData> sprite_data_dict = new Dictionary<string,SpriteData>();
            foreach (SpriteData data in sprite_list.sprites)
            {
                sprite_data_dict[data.sprite_name] = data;
                Sprite NewSprite = new Sprite();
                string filename = prefix + data.sprite_name + ".png";
                Texture2D SpriteTexture = LoadTexture(Path.Combine(directory,filename));

                List<Sprite> spriteFrames = new List<Sprite>();
                foreach (Vector2 startPos in data.subimage)
                {
                    Sprite newSprite = Sprite.Create(SpriteTexture, new Rect(startPos.x, startPos.y, data.sprite_size.x, data.sprite_size.y), new Vector2(0.5f, 0.5f), pixelsPerUnit);
                    spriteFrames.Add(newSprite);
                }
                sprites.Add(data.sprite_name, spriteFrames);
            }
        }
        
        //For testing purposes at the moment
        ChangeSprite("idle");
    }

    public void ChangeSprite(string _sprite_name, int _frame = 0)
    {
        if (_sprite_name == null)
        {
            Debug.Log("No Sprite found");
            _sprite_name = "idle";
        }
        if (sprites.ContainsKey(_sprite_name))
        {
            current_sprite = _sprite_name;
            current_frame = _frame;
            sprite_renderer.sprite = sprites[current_sprite][current_frame];
        }
        else
        {
            Debug.Log("Attempted to load illegal sprite: " + _sprite_name);
        }
    }

    public void ChangeSubimage(int _frame, bool _loop=true)
    {
        if (_frame < 0)
            _frame += sprites[current_sprite].Count;
        if (_loop)
            current_frame = _frame % sprites[current_sprite].Count;
        else
            current_frame = Mathf.Min(_frame, sprites[current_sprite].Count-1);

        sprite_renderer.sprite = sprites[current_sprite][current_frame];
    }

    private Sprite LoadNewSprite(string FilePath, int frameNo)
    {
        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

        Sprite NewSprite = new Sprite();
        Texture2D SpriteTexture = LoadTexture(FilePath);

        if ((width * (frameNo+1)) > SpriteTexture.width)
            return null;
        else
            return Sprite.Create(SpriteTexture, new Rect(width * frameNo, 0, width, SpriteTexture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
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
}