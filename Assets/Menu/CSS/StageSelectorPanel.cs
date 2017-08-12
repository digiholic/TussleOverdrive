using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectorPanel : MonoBehaviour {
    public string selectedSprite;
    public StageInfo stage_info;

    public bool selected;

    public Vector2 GridLoc;

    private SpriteRenderer bgSprite;
    private SpriteRenderer portraitSprite;
    private List<SpriteRenderer> selectionSprites = new List<SpriteRenderer>();

    public bool active = false;
    // Use this for initialization
    void Awake()
    {
        bgSprite = GetComponent<SpriteRenderer>();
        if (selectedSprite == "") bgSprite.enabled = false;
        portraitSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();

        GridLoc.x = transform.localPosition.x;
        GridLoc.y = transform.localPosition.y;

        /*
        selectionSprites.Add(transform.Find("TLSelected").GetComponent<SpriteRenderer>());
        selectionSprites.Add(transform.Find("TRSelected").GetComponent<SpriteRenderer>());
        selectionSprites.Add(transform.Find("BLSelected").GetComponent<SpriteRenderer>());
        selectionSprites.Add(transform.Find("BRSelected").GetComponent<SpriteRenderer>());
        */
    }

    public void SetPortrait(StageInfo info)
    {
        stage_info = info;
        bgSprite.enabled = true;
        Sprite spr = info.stage_icon;
        portraitSprite.sprite = spr;
        active = true;
    }
}
