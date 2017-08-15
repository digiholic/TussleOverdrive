using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.U2D;

[System.Serializable]
public class SpriteHandler : BattleComponent {
    public enum SpriteOrientation { LEFT, RIGHT }

    //public string directory;
    //public string default_sprite;
    //public string prefix;
    //public float pixelsPerUnit = 100.0f;
    public SpriteOrientation orientation;
    //public SpriteAtlas sprite_atlas;
    public float pixelsPerUnit;

    private FighterInfo fighter_info;
    private SpriteInfo sprite_info;
    private Dictionary<string,List<Sprite>> sprites = new Dictionary<string,List<Sprite>>();
    private string current_sprite = "idle";
    private int current_frame = 0;

    private SpriteRenderer sprite_renderer;
    private GameObject spriteComponent;

    private float rot_degrees;

    /*
    public void SaveSprites()
    {
        foreach (KeyValuePair<string,List<Sprite>> sheet in sprites)
        {
            int subimage = 0;
            foreach (Sprite sprite in sheet.Value)
            {
                Texture2D spriteTexture = sprite.texture;
                Rect rec = sprite.textureRect;
                Texture2D subtex = CropTexture(spriteTexture,(int)rec.x, (int)rec.y, (int)rec.width, (int)rec.height);
                
                byte[] data = subtex.EncodeToPNG();

                File.WriteAllBytes(sprite_info.sprite_directory + "/singles/" + sheet.Key + subimage.ToString() + ".png", data);
                subimage++;
            }
        }
    }
    */

    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();

        spriteComponent = new GameObject("Sprite");
        RectTransform componentRect = spriteComponent.AddComponent<RectTransform>();
        spriteComponent.transform.parent = transform;
        componentRect.anchorMin = new Vector2(0.5f, 0);
        componentRect.anchorMax = new Vector2(0.5f, 0);
        componentRect.anchoredPosition = Vector3.zero;

        sprite_renderer = spriteComponent.AddComponent<SpriteRenderer>();
        //sprite_renderer = gameObject.AddComponent<SpriteRenderer>();

        battleObject.spriteObject = spriteComponent;
        fighter_info = GetComponent<FighterInfoLoader>().GetFighterInfo();
        sprite_info = fighter_info.sprite_info;
        //sprite_atlas = sprite_info.sprite_atlas;

        pixelsPerUnit = sprite_info.sprite_pixelsPerUnit;
        DirectoryInfo info = new DirectoryInfo(Path.Combine("Assets/Resources/Fighters/" + fighter_info.directory_name, sprite_info.sprite_directory));
        string sprite_json_path = Path.Combine(info.FullName, "sprites.json");

        
        /*
        float pixelRatio = 100.0f / pixelsPerUnit;
        Vector3 scale = spriteComponent.transform.localScale;
        scale.x *= pixelRatio;
        scale.y *= pixelRatio;
        spriteComponent.transform.localScale = scale;
        */

        if (File.Exists(sprite_json_path))
        {
            string sprite_json = File.ReadAllText(sprite_json_path);
            SpriteDataCollection sprite_list = JsonUtility.FromJson<SpriteDataCollection>(sprite_json);
            LoadSpritesFromData(sprite_list);
        }
        else
        {
            Debug.LogError("No sprites JSON found: " + sprite_json_path);
        }

    }

    public override void ManualUpdate()
    {
        
    }

    public void LoadSpritesFromData(SpriteDataCollection sprite_list)
    {
        sprites = new Dictionary<string, List<Sprite>>();

        Dictionary<string, SpriteData> sprite_data_dict = new Dictionary<string, SpriteData>();
        
        foreach (SpriteData data in sprite_list.sprites)
        {
            sprite_data_dict[data.sprite_name] = data;
            string filename = sprite_info.sprite_prefix + data.sprite_name + ".png";
            
            Texture2D SpriteTexture = LoadTexture(Path.Combine("Assets/Resources/Fighters/"+fighter_info.directory_name+"/"+sprite_info.sprite_directory, filename));

            List<Sprite> spriteFrames = new List<Sprite>();
            foreach (Vector2 startPos in data.subimage)
            {
                Sprite newSprite = Sprite.Create(SpriteTexture, new Rect(startPos.x, startPos.y, data.sprite_size.x, data.sprite_size.y), data.pivot_point, pixelsPerUnit);
                spriteFrames.Add(newSprite);
            }
            sprites.Add(data.sprite_name, spriteFrames);
        }
        Debug.Log(sprites);
    }
    
    private void ChangeRenderer(string current_sprite, int current_frame)
    {
        sprite_renderer.sprite = sprites[current_sprite][current_frame];
        
        /*
        Sprite spr = sprite_atlas.GetSprite(current_sprite + current_frame.ToString());
        if (spr != null)
        {
            sprite_renderer.sprite = spr;
        }
        else
        {
            Debug.LogWarning("Attempted to load illegal sprite: " + current_sprite+current_frame.ToString());
        }
        */
    }

    public void ChangeSprite(string _sprite_name)
    {
        if (_sprite_name == null)
        {
            Debug.LogWarning("No Sprite found");
            _sprite_name = "idle";
        }
        current_sprite = _sprite_name;
        current_frame = 0;
        ChangeRenderer(current_sprite, current_frame);
    }

    public void ChangeSubimage(int _frame)
    {
        if (_frame < 0)
            _frame += sprites[current_sprite].Count;
        current_frame = Mathf.Min(_frame, sprites[current_sprite].Count - 1);
        

        ChangeRenderer(current_sprite, current_frame);
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

        ChangeRenderer(current_sprite, current_frame);
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
            {
                Tex2D.filterMode = FilterMode.Point;
                return Tex2D;                 // If data = readable -> return texture
            }
                
        }
        return null;                     // Return null if load failed
    }

    public void RotateSprite(float degrees)
    {
        sprite_renderer.transform.Rotate(new Vector3(0, 0, degrees));
        rot_degrees = degrees;
    }

    public void SetRotation(float degrees)
    {
        if (rot_degrees != degrees)
        {
            UnRotate();
            RotateSprite(degrees);
        }
    }

    public void UnRotate()
    {
        sprite_renderer.transform.rotation = Quaternion.identity;
        rot_degrees = 0.0f;
    }

    public void flip()
    {
        if (orientation == SpriteOrientation.LEFT)
            orientation = SpriteOrientation.RIGHT;
        else
            orientation = SpriteOrientation.LEFT;
        Vector3 transfVec = sprite_renderer.transform.localScale;
        transfVec.x *= -1;
        //sprite_renderer.transform.localScale = transfVec;
    }

    public void ReconcileDirection()
    {
        if (HasVar("facing"))
        {
            int facing = GetIntVar("facing");
            if ((facing == 1 && orientation == SpriteOrientation.LEFT) ||
                    (facing == -1 && orientation == SpriteOrientation.RIGHT))
                flip();
        }
    }

    /*
    public static Texture2D CropTexture(Texture2D pSource, int left, int top, int width, int height)
    {
        if (left < 0)
        {
            width += left;
            left = 0;
        }
        if (top < 0)
        {
            height += top;
            top = 0;
        }
        if (left + width > pSource.width)
        {
            width = pSource.width - left;
        }
        if (top + height > pSource.height)
        {
            height = pSource.height - top;
        }

        if (width <= 0 || height <= 0)
        {
            return null;
        }

        Color[] aSourceColor = pSource.GetPixels(0);

        //*** Make New
        Texture2D oNewTex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        //*** Make destination array
        int xLength = width * height;
        Color[] aColor = new Color[xLength];

        int i = 0;
        for (int y = 0; y < height; y++)
        {
            int sourceIndex = (y + top) * pSource.width + left;
            for (int x = 0; x < width; x++)
            {
                aColor[i++] = aSourceColor[sourceIndex++];
            }
        }

        //*** Set Pixels
        oNewTex.SetPixels(aColor);
        oNewTex.Apply();

        //*** Return
        return oNewTex;
    }
    */
}