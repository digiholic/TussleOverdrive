using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSpriteInfo : MonoBehaviour
{
    [SerializeField] private string spriteSheetFileName;
    [SerializeField] private int numCols;
    [SerializeField] private int numRows;
    [SerializeField] private string defaultAnimation;
    [SerializeField] private List<AnimationDefinition> animations;

    public TextAsset json_file;

    public string SpriteSheetFileName { get => spriteSheetFileName; set => spriteSheetFileName = value; }
    public int NumCols { get => numCols; set => numCols = value; }
    public int NumRows { get => numRows; set => numRows = value; }
    public string DefaultAnimation { get => defaultAnimation; set => defaultAnimation = value; }
    public List<AnimationDefinition> Animations { get => animations; set => animations = value; }

    public SpriteInfo getSpriteInfo()
    {
        SpriteInfo info = new SpriteInfo();
        info.spriteSheetFileName = spriteSheetFileName;
        info.numCols = numCols;
        info.numRows = numRows;
        info.defaultAnimation = defaultAnimation;
        info.animations = animations;
        return info;
    }

    public void FromSpriteInfo(SpriteInfo info)
    {
        spriteSheetFileName = info.spriteSheetFileName;
        numCols = info.numCols;
        numRows = info.numRows;
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
}
