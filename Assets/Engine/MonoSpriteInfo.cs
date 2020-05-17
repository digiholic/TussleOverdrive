using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSpriteInfo : MonoBehaviour
{
    [SerializeField] private string spriteSheetFileName;
    [SerializeField] private string numCols;
    [SerializeField] private string numRows;
    [SerializeField] private string defaultAnimation;
    [SerializeField] private List<AnimationDefinition> animations;

    public TextAsset json_file;

    public string SpriteSheetFileName { get => spriteSheetFileName; set => spriteSheetFileName = value; }
    public string NumCols { get => numCols; set => numCols = value; }
    public string NumRows { get => numRows; set => numRows = value; }
    public string DefaultAnimation { get => defaultAnimation; set => defaultAnimation = value; }
    public List<AnimationDefinition> Animations { get => animations; set => animations = value; }

    public SpriteInfo getSpriteInfo()
    {
        SpriteInfo info = new SpriteInfo();
        info.spriteSheetFileName = spriteSheetFileName;
        info.numCols = int.Parse(numCols);
        info.numRows = int.Parse(numRows);
        info.defaultAnimation = defaultAnimation;
        info.animations = animations;
        return info;
    }

    public void FromSpriteInfo(SpriteInfo info)
    {
        spriteSheetFileName = info.spriteSheetFileName;
        numCols = info.numCols.ToString();
        numRows = info.numRows.ToString();
        defaultAnimation = info.defaultAnimation;
        animations = info.animations;
    }

    void Awake()
    {
        if (json_file != null)
        {
            LoadSprite();
        }
    }

    public void LoadSprite()
    {
        SpriteInfo sprite_info = JsonUtility.FromJson<SpriteInfo>(json_file.text);
        FromSpriteInfo(sprite_info);
    }

    public void AddAnimation()
    {
        animations.Add(new AnimationDefinition());
    }

    public void AddAnimation(AnimationDefinition anim)
    {
        animations.Add(anim);
    }

    public void DeleteAnimation(AnimationDefinition animation)
    {
        animations.Remove(animation);
    }

    public List<string> GetAnimationNames()
    {
        List<string> names = new List<string>();
        animations.ForEach(animation => names.Add(animation.animationName));
        return names;
    }
}
