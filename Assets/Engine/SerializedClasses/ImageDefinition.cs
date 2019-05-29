using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sprite Data contains all of the information needed to read a sprite from a file with all of its positioning, cropping, and sizing
/// The sprite data contains the following fields:
///     SpriteFileName - the Filename of the sprite sheet this sprite is found on
///     ImageName - the name of this specific image, defaults to {AnimationName}_{Frame}
///     offsetX - the X offset from the topleft of the image file that this frame starts on, in image pixels
///     offsetY - the Y offset from the topleft of the image file that this frame starts on, in image pixels
///     Width - the width in image pixels of this frame
///     Height - the height in image pixels of this frame
///     PixelsPerUnit - how many image pixels fit in one game unit
///     Pivot - an AnchorPoint defining where the sprites should overlap. When changing subimages, the sprites will align on their pivot points
///     Anchors - a list of AnchorPoints defining anchors to be referenced in the action file
/// </summary>
[System.Serializable]
public class ImageDefinition {
    //Whether or not any data has changed since we last cached the image. If this is true, we'll load the image from disk whenever getImage is called.
    private bool dirty = true;
    private Sprite cachedSprite;

    private string spriteFileName;
    private string imageName;
    private int offsetX;
    private int offsetY;
    private int width;
    private int height;
    private float pixelsPerUnit;
    
    private AnchorPointData pivot;
    private List<AnchorPointData> anchors;

    #region Properties
    public string SpriteFileName
    {
        get { return spriteFileName; }
        set
        {
            spriteFileName = value;
            dirty = true;
        }
    }
    public string ImageName
    {
        get { return imageName; }
        set
        {
            imageName = value;
            dirty = true;
        }
    }
    public int OffsetX
    {
        get { return offsetX; }
        set
        {
            offsetX = value;
            dirty = true;
        }
    }
    public int OffsetY
    {
        get { return offsetY; }
        set
        {
            offsetY = value;
            dirty = true;
        }
    }
    public int Width
    {
        get { return width; }
        set
        {
            width = value;
            dirty = true;
        }
    }
    public int Height
    {
        get { return height; }
        set
        {
            height = value;
            dirty = true;
        }
    }
    public float PixelsPerUnit
    {
        get { return pixelsPerUnit; }
        set
        {
            pixelsPerUnit = value;
            dirty = true;
        }
    }
    public AnchorPointData Pivot
    {
        get { return pivot; }
        set {
            pivot = value;
            dirty = true;
        }
    }

    #endregion

    /*
    private Sprite getImage()
    {
        if (dirty || cachedSprite == null)
        {
            loadImageFromFile();
            return cachedSprite;
        } else
        {
            return cachedSprite;
        }
    }
    
    private void loadImageFromFile()
    {
        if (cachedTextureFile == null)
        {
            cachedTextureFile = FileLoader.LoadTexture(spriteFileName);
        }
        cachedSprite = Sprite.Create(cachedTextureFile, new Rect(offsetX, offsetY, width, height), pivot.getAsRelative(this), pixelsPerUnit);
        dirty = false;
    }
    */

    /// <summary>
    /// Get the cached sprite of this ImageDefinition, if it exists. This will return null if the cache isn't set,
    /// or if the data has changed and this cache is invalid. Make sure to null check this!
    /// </summary>
    /// <returns></returns>
    public Sprite getCachedSprite()
    {
        //Invalidate the cachedd sprite if this data has changed
        if (dirty) cachedSprite = null;
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

    public string sprite_name;
    public Vector2 sprite_size;
    public Vector2 pivot_point = new Vector2(0.5f, 0.0f); //Defaults to bottom
    public Vector2[] subimage;
    public Dictionary<string, Vector2> anchor_points;
}

/// <summary>
/// The AnimationDefinition contains a list of ImageDefinitions and the stuff necessary to define how they move
/// Keeps track of it's own frame and handles looping. The SpriteHandler need only query the animation for what
/// it's at to get the ImageDefinition
/// </summary>
[System.Serializable]
public class AnimationDefinition
{
    private List<ImageDefinition> subimages;
    private int spriteRate;
    private bool loop;

    private int currentFrame;
    private int currentSubimageIndex;

    public ImageDefinition getCurrentSubimage(bool advance = false)
    {
        ImageDefinition currentSubimage = subimages[currentSubimageIndex];
        if (advance) //If we're advancing the current frame afterwards, do that here
        {
            //Increment the current frame, and if we've hit the spriteRate amount, move on to the next subimage index
            currentFrame++;
            if (currentFrame % spriteRate == 0) currentSubimageIndex++;

            //If we loop, we take the modulo operator to get the currentSubimageIndex, otherwise, we make sure to clamp it between 0 and the last
            if (loop) currentSubimageIndex = currentSubimageIndex % subimages.Count;
            else currentSubimageIndex = Mathf.Clamp(currentSubimageIndex, 0, subimages.Count - 1);
        }
        return currentSubimage;
    }

    public ImageDefinition getAndAdvanceSubimage()
    {
        return getCurrentSubimage(true);
    }
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

[System.Serializable]
public class SpriteDataCollection
{
    public List<ImageDefinition> sprites = new List<ImageDefinition>();
}