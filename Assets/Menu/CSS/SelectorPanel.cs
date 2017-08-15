using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorPanel : MonoBehaviour {
    public string selectedSprite;
    public FighterInfo fighter_info;

    public bool[] selected = new bool[4];

    public Vector2 GridLoc = new Vector2(0, 0);

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
        
        selectionSprites.Add(transform.Find("TLSelected").GetComponent<SpriteRenderer>());
        selectionSprites.Add(transform.Find("TRSelected").GetComponent<SpriteRenderer>());
        selectionSprites.Add(transform.Find("BLSelected").GetComponent<SpriteRenderer>());
        selectionSprites.Add(transform.Find("BRSelected").GetComponent<SpriteRenderer>());
    }

    public void SetPortrait(FighterInfo info)
    {
        fighter_info = info;
        selectedSprite = "Fighters/"+info.directory_name+"/"+info.css_icon_path;
        bgSprite.enabled = true;
        Sprite spr = Resources.Load<Sprite>(selectedSprite);
        portraitSprite.sprite = spr;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        //All right, you ready for some hacky shit?
        //This is the way the corners get colored in to show all the players selecting
        //It does this by setting the colors in reverse priority order
        //The opposite corner is the least likely to stay lit (only lit when nothing else is)
        //Then we choose to assign horizontal less value than vertical
        //Then finally we set the corners themselves to match if they're lit
        //This way it always looks good no matter how many people are on one panel

        int[] colors = new int[] { -1, -1, -1, -1 };

        //Lowest color priority: opposite edges
        if (selected[0]) colors[3] = 0;
        if (selected[1]) colors[2] = 1;
        if (selected[2]) colors[1] = 2;
        if (selected[3]) colors[0] = 3;

        //Medium priority: horizontal adjacent
        if (selected[0]) colors[1] = 0;
        if (selected[1]) colors[0] = 1;
        if (selected[2]) colors[3] = 2;
        if (selected[3]) colors[2] = 3;

        //Medium priority: vertical adjacent
        if (selected[0]) colors[2] = 0;
        if (selected[1]) colors[3] = 1;
        if (selected[2]) colors[0] = 2;
        if (selected[3]) colors[1] = 3;

        //High priority: exact
        if (selected[0]) colors[0] = 0;
        if (selected[1]) colors[1] = 1;
        if (selected[2]) colors[2] = 2;
        if (selected[3]) colors[3] = 3;

        SetCornerColors(colors);
    }

    void SetCornerColor(int corner, int color)
    {
        if (color == -1) ClearColor(corner);
        else selectionSprites[corner].color = Settings.current_settings.player_colors[color];
    }

    void SetCornerColors(int[] colors)
    {
        for (int i=0; i < 4; i++)
            SetCornerColor(i, colors[i]);
    }

    void ClearColor(int corner)
    {
        Color color = new Color();
        color.a = 0;
        selectionSprites[corner].color = color;
    }
}
