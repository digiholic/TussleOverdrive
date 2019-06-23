using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The AnimationDefinition contains a list of ImageDefinitions and the stuff necessary to define how they move
/// Keeps track of it's own frame and handles looping. The SpriteHandler need only query the animation for what
/// it's at to get the ImageDefinition.
/// </summary>
[System.Serializable]
public class AnimationDefinition
{
    public string _animationName = "";
    public int _spriteRate = 1;
    public bool _loop = false;

    public List<ImageDefinition> subimages;
    private int currentFrame;
    private int currentSubimageIndex;

    #region Properties
    public string AnimationName
    {
        get { return _animationName;  }
        set { _animationName = value; }
    }
    public int SpriteRate
    {
        get { return _spriteRate;  }
        set { _spriteRate = value; }
    }
    public bool Loop
    {
        get { return _loop;  }
        set { _loop = value; }
    }
    #endregion

    public AnimationDefinition(string name,int spriteRate, bool loop)
    {
        _animationName = name;
        _spriteRate = spriteRate;
        _loop = loop;
    }

    public AnimationDefinition(AnimationDefinition old)
    {
        _animationName = old.AnimationName;
        _spriteRate = old.SpriteRate;
        _loop = old.Loop;
    }

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
        if (_spriteRate != 0)
        {
            //this integer division should throw away any decimal, which is what we want at the moment
            int resultSubimageIndex = frame / _spriteRate;

            //handle looping if it's set, otherwise clamp it to between 0 and the last frame
            if (_loop) resultSubimageIndex = resultSubimageIndex % subimages.Count;
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
        return "Animation Definition: " + _animationName;
    }

    public static AnimationDefinition NullAnimation = new AnimationDefinition("", 1, false);
}
