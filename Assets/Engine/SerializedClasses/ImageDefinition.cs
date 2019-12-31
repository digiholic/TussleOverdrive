using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sprite Data contains all of the information needed to read a sprite from a file with all of its positioning, cropping, and sizing
/// The sprite data contains the following fields:
///     ImageName - The name of this specific image, defaults to {AnimationName}_{Frame}
///     SpriteFileName - The name of the image on disk, without any costume prefixes or file extensions
///     offsetX - The X offset from the topleft of the image file that this frame starts on, in image pixels
///     offsetY - The Y offset from the topleft of the image file that this frame starts on, in image pixels
///     Width - The width in image pixels of this frame
///     Height - The height in image pixels of this frame
///     PixelsPerUnit - How many image pixels fit in one game unit
///     Pivot - An AnchorPoint defining where the sprites should overlap. When changing subimages, the sprites will align on their pivot points
///     Anchors - A list of AnchorPoints defining anchors to be referenced in the action file
/// </summary>
[System.Serializable]
public class ImageDefinition {
    //Whether or not any data has changed since we last cached the image. If this is true, we'll load the image from disk whenever getImage is called.
    private bool dirty = true;
    private Sprite cachedSprite = null;

    public string _imageName = ""; //the name of this specific image, defaults to {AnimationName}_{Frame}
    public string _spriteFileName = ""; //The name of the image on disk, without any costume prefixes or file extensions
    public int _offsetX = 0; //The X offset from the topleft of the image file that this frame starts on, in image pixels
    public int _offsetY = 0; //The Y offset from the topleft of the image file that this frame starts on, in image pixels
    public int _width = 0; //The width in image pixels of this frame
    public int _height = 0; //The height in image pixels of this frame
    //public float _pixelsPerUnit = 0; //How many image pixels fit in one game unit

    public AnchorPointData _pivot = null; //An AnchorPoint defining where the sprites should overlap. When changing subimages, the sprites will align on their pivot points
    public List<AnchorPointData> _anchors; //A list of AnchorPoints defining anchors to be referenced in the action file

    #region Properties
    public string SpriteFileName
    {
        get { return _spriteFileName; }
        set
        {
            _spriteFileName = value;
            dirty = true;
        }
    }
    public string ImageName
    {
        get { return _imageName; }
        set
        {
            _imageName = value;
            dirty = true;
        }
    }
    public int OffsetX
    {
        get { return _offsetX; }
        set
        {
            _offsetX = value;
            dirty = true;
        }
    }
    public int OffsetY
    {
        get { return _offsetY; }
        set
        {
            _offsetY = value;
            dirty = true;
        }
    }
    public int Width
    {
        get { return _width; }
        set
        {
            _width = value;
            dirty = true;
        }
    }
    public int Height
    {
        get { return _height; }
        set
        {
            _height = value;
            dirty = true;
        }
    }
    public AnchorPointData Pivot
    {
        get { return _pivot; }
        set {
            _pivot = value;
            dirty = true;
        }
    }

    #endregion

    /// <summary>
    /// Get the sprite this image definition represents. If it's not cached, it'll cache it, which
    /// needs the directoryName and costumeName from the SpriteInfo. If the directoryName is empty, 
    /// it'll return the cached sprite just fine, but will give a null if it's not set.
    /// </summary>
    /// <param name="directoryName"></param>
    /// <param name="costumeName"></param>
    /// <returns></returns>
    public Sprite getSprite(string directoryName="",string costumeName="",float pixelsPerUnit=100f)
    {
        if (cachedSprite == null || dirty)
        {
            if (directoryName.Length > 0)
            {
                cacheSprite(directoryName, costumeName, pixelsPerUnit);
            } else
            {
                cachedSprite = null;
            }
        }
        return cachedSprite;
    }

    /// <summary>
    /// Set the cached sprite for this ImageDefinition. This lets us access the Sprite quickly without needing to re-slice
    /// the sheet or load the file from disk. Once cached, it will remain until any properties of this ImageDefinition are changed
    /// </summary>
    /// <param name="sprite">The Sprite to set as the cache</param>
    public void cacheSprite(Sprite sprite)
    {
        cachedSprite = sprite;
        dirty = false;
    }
    
    public void cacheSprite(string directoryName, string costumeName, float pixelsPerUnit)
    {
        string filename = _spriteFileName;
        string path = FileLoader.PathCombine(directoryName, filename);
        //If the costume name exists and isn't an empty string, make sure it's part of the path
        if (costumeName != null && costumeName.Length > 0)
        {
            path = FileLoader.PathCombine(directoryName, costumeName, filename);
        }

        Texture2D cachedTextureFile = FileLoader.LoadTexture(path);
        Sprite newSprite = Sprite.Create(cachedTextureFile, new Rect(OffsetX, OffsetY, Width, Height), Pivot.getAsRelative(this), pixelsPerUnit);
        cacheSprite(newSprite);
    }

    public override string ToString()
    {
        string ret = "Image Definition: " + _imageName;
        ret       += " - file: " + _spriteFileName;
        ret       += " - size: (" + _width + "," + _height + ")";
        ret       += " - offset: (" + _offsetX + "," + _offsetY + ")";
        return ret;
    }

    public static ImageDefinition NullDef = new ImageDefinition();
}

/// <summary>
/// A data structure for storing a "anchor point" of a sprite, used for aligning specific positions
/// </summary>
[System.Serializable]
public class AnchorPointData
{
    public static string ABSOLUTE = "absolute";
    public static string RELATIVE = "relative";

    public string anchorName;
    public float xPos;
    public float yPos;
    public string anchorType;

    public AnchorPointData(string anchorName,float xPos, float yPos,string anchorType)
    {
        this.anchorName = anchorName;
        this.xPos = xPos;
        this.yPos = yPos;
        this.anchorType = anchorType;
    }

    public Vector2 getAsRelative(ImageDefinition data)
    {
        if (anchorType == RELATIVE)
        {
            return new Vector2(xPos, yPos);
        } else
        {
            float xRel = xPos / data.Width;
            float yRel = yPos / data.Height;
            return new Vector2(xRel, yRel);
        }
    }

    public Vector2 getAsAbsolute(ImageDefinition data)
    {
        if (anchorType == ABSOLUTE)
        {
            return new Vector2(xPos, yPos);
        } else
        {
            float xAbs = xPos * data.Width;
            float yAbs = yPos * data.Height;
            return new Vector2(xAbs, yAbs);
        }
    }
}