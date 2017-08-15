using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectorPanel : MonoBehaviour {
    public string selectedSprite;
    public StageInfo stage_info;

    public Vector2 GridLoc = new Vector2(0, 0);

    private SpriteRenderer bgSprite;
    private SpriteRenderer portraitSprite;
    private SpriteRenderer selectionSprite;

    public Sprite randomEnabledSprite;
    public Sprite randomDisabledSprite;

    public bool active = false;
    public bool selected = false;
    public bool random_enabled = true;

    // Use this for initialization
    void Awake()
    {
        bgSprite = GetComponent<SpriteRenderer>();
        if (selectedSprite == "") bgSprite.enabled = false;
        portraitSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();

        selectionSprite = transform.Find("Selector").GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (selected) selectionSprite.enabled = true;
        else selectionSprite.enabled = false;
        if (random_enabled) bgSprite.sprite = randomEnabledSprite;
        else bgSprite.sprite = randomDisabledSprite;
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
