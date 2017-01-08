using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpriteLoader : MonoBehaviour {

    public string directory;
    public int width;
    public string prefix = "";
    public float pixelsPerUnit = 100.0f;

    private Dictionary<string,List<Sprite>> sprites = new Dictionary<string,List<Sprite>>();
    private string current_sprite = "idle";
    private int current_frame = 0;

	// Use this for initialization
	void Awake () {

        DirectoryInfo info = new DirectoryInfo(directory);
        FileInfo[] fileInfo = info.GetFiles();
        
        //Iterate over each sprite in the directory
        for (int i=0; i < fileInfo.Length; i++)
        {
            FileInfo file = fileInfo[i];
            if (file.Extension == ".png" && file.Name.StartsWith(prefix))
            {
                //Iterate over every frame in the image
                int j = 0;
                List<Sprite> spriteFrames = new List<Sprite>();
                Sprite newSprite = LoadNewSprite(file.FullName,j);
                spriteFrames.Add(newSprite);
                while (newSprite != null) //newSprite will spit out images until it overflows, when it will return null
                {
                    j++;
                    newSprite = LoadNewSprite(file.FullName, j);
                    if (newSprite != null)
                        spriteFrames.Add(newSprite);
                }
                string spriteName = Path.GetFileNameWithoutExtension(file.Name);
                spriteName = spriteName.Remove(0,prefix.Length);
                sprites.Add(spriteName, spriteFrames);
            }
        }
        
        //For testing purposes at the moment
        ChangeSprite("idle");
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ChangeSprite(string _sprite_name, int _frame = 0)
    {
        if (_sprite_name == null)
        {
            Debug.Log("No Sprite found");
            _sprite_name = "idle";
        }
        if (sprites.ContainsKey(_sprite_name))
        {
            current_sprite = _sprite_name;
            current_frame = _frame;
            GetComponent<SpriteRenderer>().sprite = sprites[current_sprite][current_frame];
        }
        else
        {
            Debug.Log("Attempted to load illegal sprite: " + _sprite_name);
        }
    }

    public void ChangeSubimage(int _frame, bool _loop=true)
    {
        if (_frame < 0)
            _frame += sprites[current_sprite].Count;
        if (_loop)
            current_frame = _frame % sprites[current_sprite].Count;
        else
            current_frame = Mathf.Min(_frame, sprites[current_sprite].Count-1);

        GetComponent<SpriteRenderer>().sprite = sprites[current_sprite][current_frame];
    }

    private Sprite LoadNewSprite(string FilePath, int frameNo)
    {

        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

        Sprite NewSprite = new Sprite();
        Texture2D SpriteTexture = LoadTexture(FilePath);

        if ((width * (frameNo+1)) > SpriteTexture.width)
        {
            return null;
        }
        else
        {
            NewSprite = Sprite.Create(SpriteTexture, new Rect(width * frameNo, 0, width, SpriteTexture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            return NewSprite;
        }
    }

    private Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;
        
        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }

    public void printSprite()
    {
        Debug.Log(current_sprite + ", " + current_frame);
    }
}
