using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteData {
    public string sprite_name;
    public Vector2 sprite_size;
    public Vector2 pivot_point = new Vector2(0.5f, 0.0f); //Defaults to bottom
    public Vector2[] subimage;
    public Dictionary<string, Vector2> anchor_points;
}

[System.Serializable]
public class SpriteDataCollection
{
    public SpriteData[] sprites = new SpriteData[] { };
}