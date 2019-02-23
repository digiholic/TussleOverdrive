using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.U2D;

[System.Serializable]
public class SpriteHandler : BattleComponent {
    public enum SpriteOrientation { LEFT, RIGHT }

    public SpriteOrientation orientation;
    //public SpriteAtlas sprite_atlas;

    private FighterInfo fighter_info;
    private SpriteInfo sprite_info;
    private Dictionary<string,List<Sprite>> sprites = new Dictionary<string,List<Sprite>>();
    
    private SpriteRenderer sprite_renderer;
    private GameObject spriteComponent;

    private float rot_degrees;

    void Awake()
    {
        if (battleObject != null)
            battleObject.spriteObject = spriteComponent;

        SetVar(TussleConstants.SpriteVariableNames.SPRITE_CURRENT, "idle");
        SetVar(TussleConstants.SpriteVariableNames.FRAME_CURRENT, 0);
        spriteComponent = new GameObject("Sprite");
        RectTransform componentRect = spriteComponent.AddComponent<RectTransform>();
        spriteComponent.transform.SetParent(transform);
        componentRect.anchorMin = new Vector2(0.5f, 0);
        componentRect.anchorMax = new Vector2(0.5f, 0);
        componentRect.anchoredPosition = Vector3.zero;

        sprite_renderer = spriteComponent.AddComponent<SpriteRenderer>();
        
    }

    public void OnFighterInfoReady(FighterInfo fInfo)
    {
        fighter_info = fInfo;
        sprite_info = fighter_info.sprite_info;
        SetVar(TussleConstants.SpriteVariableNames.PIXELS_PER_UNIT, sprite_info.sprite_pixelsPerUnit);
        LoadSpritesFromData(fInfo.getSpriteData());
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

            string path = FileLoader.PathCombine(fighter_info.directory_name, sprite_info.sprite_directory, filename);
            Texture2D SpriteTexture = FileLoader.LoadTexture(path);

            List<Sprite> spriteFrames = new List<Sprite>();
            foreach (Vector2 startPos in data.subimage)
            {
                Sprite newSprite = Sprite.Create(SpriteTexture, new Rect(startPos.x, startPos.y, data.sprite_size.x, data.sprite_size.y), data.pivot_point, GetFloatVar(TussleConstants.SpriteVariableNames.PIXELS_PER_UNIT));
                spriteFrames.Add(newSprite);
            }
            sprites.Add(data.sprite_name, spriteFrames);
        }
    }
    
    private void ChangeRenderer()
    {
        string current_sprite = GetStringVar(TussleConstants.SpriteVariableNames.SPRITE_CURRENT);
        int current_frame = GetIntVar(TussleConstants.SpriteVariableNames.FRAME_CURRENT);

        if (sprites.ContainsKey(current_sprite))
            sprite_renderer.sprite = sprites[current_sprite][current_frame];
        //else
            //Debug.LogWarning("No sprite in dictionary! Current Sprite: " + current_sprite);
        
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
            Debug.LogWarning("ChangeSprite given null name");
            _sprite_name = "idle";
        }
        SetVar(TussleConstants.SpriteVariableNames.SPRITE_CURRENT, _sprite_name);
        SetVar(TussleConstants.SpriteVariableNames.FRAME_CURRENT, 0);
        ChangeRenderer();
    }

    public void ChangeSubimage(int _frame)
    {
        ChangeSubimage(_frame, false);
    }

    public void ChangeSubimageWithLoop(int _frame)
    {
        ChangeSubimage(_frame, true);
    }

    public void ChangeSubimage(int _frame, bool _loop)
    {
        string current_sprite = GetStringVar(TussleConstants.SpriteVariableNames.SPRITE_CURRENT);
        int current_frame = GetIntVar(TussleConstants.SpriteVariableNames.FRAME_CURRENT);

        if (_frame < 0)
        {
            _frame += sprites[current_sprite].Count;
        }
        if (_loop)
        {
            current_frame = _frame % sprites[current_sprite].Count;
        }
        else
        {
            current_frame = Mathf.Clamp(_frame, 0, sprites[current_sprite].Count - 1);
        }

        SetVar(TussleConstants.SpriteVariableNames.FRAME_CURRENT, current_frame);
        ChangeRenderer();
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
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        Debug.Log("Flipping");
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
        if (HasVar(TussleConstants.FighterVariableNames.FACING_DIRECTION))
        {
            int facing = GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION);
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