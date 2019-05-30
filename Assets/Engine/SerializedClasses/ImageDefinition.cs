using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sprite Data contains all of the information needed to read a sprite from a file with all of its positioning, cropping, and sizing
/// The sprite data contains the following fields:
///     SpriteFileName - The name of the image on disk, without any costume prefixes or file extensions
///     ImageName - The name of this specific image, defaults to {AnimationName}_{Frame}
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
    private Sprite cachedSprite;

    private string spriteFileName; //The name of the image on disk, without any costume prefixes or file extensions
    private string imageName; //the name of this specific image, defaults to {AnimationName}_{Frame}
    private int offsetX; //The X offset from the topleft of the image file that this frame starts on, in image pixels
    private int offsetY; //The Y offset from the topleft of the image file that this frame starts on, in image pixels
    private int width; //The width in image pixels of this frame
    private int height; //The height in image pixels of this frame
    private float pixelsPerUnit; //How many image pixels fit in one game unit

    private AnchorPointData pivot; //An AnchorPoint defining where the sprites should overlap. When changing subimages, the sprites will align on their pivot points
    private List<AnchorPointData> anchors; //A list of AnchorPoints defining anchors to be referenced in the action file

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
/// it's at to get the ImageDefinition.
/// </summary>
[System.Serializable]
public class AnimationDefinition
{
    private List<ImageDefinition> subimages;
    private int spriteRate;
    private bool loop;

    private int currentFrame;
    private int currentSubimageIndex;

    /// <summary>
    /// Gets the subimage of the current frame. Optionally also advances to the next frame
    /// </summary>
    /// <param name="advance">If true, advance to the next frame after getting the image so it's ready for the next call. Defaults to false</param>
    /// <returns>The ImageDefinition at the current index</returns>
    public ImageDefinition getCurrentSubimage(bool advance = false)
    {
        ImageDefinition currentSubimage = subimages[currentSubimageIndex];
        if (advance) //If we're advancing the current frame afterwards, do that here
        {
            //Increment the current frame, and if we've hit the spriteRate amount, move on to the next subimage index
            currentFrame++;
            currentSubimageIndex = getIndexForFrame(currentFrame);
        }
        return currentSubimage;
    }

    /// <summary>
    /// A shortcut method for calling getCurrentSubimage and advancing to the next frame.
    /// </summary>
    /// <returns>The ImageDefinition at the current index</returns>
    public ImageDefinition getAndAdvanceSubimage()
    {
        return getCurrentSubimage(true);
    }

    /// <summary>
    /// Sets the current frame and updates the subimage index to match
    /// </summary>
    /// <param name="frame"></param>
    public void setFrame(int frame)
    {
        currentFrame = frame;
        currentSubimageIndex = getIndexForFrame(currentFrame);
    }

    /// <summary>
    /// Gets the subimage index for the current frame. For example, if there are 8 subimages and a sprite rate of 2,
    /// you'd get back the index of 4 on frames 8 and 9. This does not set any internal indexes or frames, so you can safely
    /// call this to peek ahead at an animation without screwing up the auto-animator
    /// </summary>
    /// <param name="frame">The frame to check for the index at</param>
    /// <returns>The subimage index that would be shown on that frame</returns>
    public int getIndexForFrame(int frame)
    {
        //this integer division should throw away any decimal, which is what we want at the moment
        int resultSubimageIndex = frame / spriteRate;

        //handle looping if it's set, otherwise clamp it to between 0 and the last frame
        if (loop) resultSubimageIndex = resultSubimageIndex % subimages.Count;
        else resultSubimageIndex = Mathf.Clamp(resultSubimageIndex, 0, subimages.Count - 1);
        return resultSubimageIndex;
    }

    /// <summary>
    /// Gets the image associated with the given frame. This does not set any internal indexes or frames, so you can safely 
    /// call this to peek ahead at an animation without screwing up the auto-animator
    /// </summary>
    /// <param name="frame">The frame to check for the image at</param>
    /// <returns>The ImageDefinition that would be loaded on that frame</returns>
    public ImageDefinition getImageForFrame(int frame)
    {
        return subimages[getIndexForFrame(frame)];
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