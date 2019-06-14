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
    private Sprite cachedSprite;

    public string _imageName; //the name of this specific image, defaults to {AnimationName}_{Frame}
    public string _spriteFileName; //The name of the image on disk, without any costume prefixes or file extensions
    public int _offsetX; //The X offset from the topleft of the image file that this frame starts on, in image pixels
    public int _offsetY; //The Y offset from the topleft of the image file that this frame starts on, in image pixels
    public int _width; //The width in image pixels of this frame
    public int _height; //The height in image pixels of this frame
    public float _pixelsPerUnit; //How many image pixels fit in one game unit

    public AnchorPointData _pivot; //An AnchorPoint defining where the sprites should overlap. When changing subimages, the sprites will align on their pivot points
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
    public float PixelsPerUnit
    {
        get { return _pixelsPerUnit; }
        set
        {
            _pixelsPerUnit = value;
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
    public Sprite getSprite(string directoryName="",string costumeName="")
    {
        if (cachedSprite == null || dirty)
        {
            if (directoryName.Length > 0)
            {
                cacheSprite(directoryName, costumeName);
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
    
    public void cacheSprite(string directoryName, string costumeName)
    {
        string filename = _spriteFileName;
        string path = FileLoader.PathCombine(directoryName, filename);
        //If the costume name exists and isn't an empty string, make sure it's part of the path
        if (costumeName != null && costumeName.Length > 0)
        {
            Debug.Log(directoryName);
            path = FileLoader.PathCombine(directoryName, costumeName, filename);
        }

        Texture2D cachedTextureFile = FileLoader.LoadTexture(path);
        Sprite newSprite = Sprite.Create(cachedTextureFile, new Rect(OffsetX, OffsetY, Width, Height), Pivot.getAsRelative(this), PixelsPerUnit);
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
}

/// <summary>
/// The AnimationDefinition contains a list of ImageDefinitions and the stuff necessary to define how they move
/// Keeps track of it's own frame and handles looping. The SpriteHandler need only query the animation for what
/// it's at to get the ImageDefinition.
/// </summary>
[System.Serializable]
public class AnimationDefinition
{
    public string animationName;

    public List<ImageDefinition> subimages;
    public int spriteRate;
    public bool loop;

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
        //A zero sprite rate means no animation, so we need to make sure that it's not dividing by zero
        if (spriteRate != 0)
        {
            //this integer division should throw away any decimal, which is what we want at the moment
            int resultSubimageIndex = frame / spriteRate;

            //handle looping if it's set, otherwise clamp it to between 0 and the last frame
            if (loop) resultSubimageIndex = resultSubimageIndex % subimages.Count;
            else resultSubimageIndex = Mathf.Clamp(resultSubimageIndex, 0, subimages.Count - 1);
            return resultSubimageIndex;
        } else
        {
            return 0;
        }
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

    public override string ToString()
    {
        return "Animation Definition: " + animationName;
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