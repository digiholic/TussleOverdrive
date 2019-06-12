using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImageDefinitionButtonRig : LegacyEditorWidget
{
    public GameObject spriteFileButtonPrefab;

    private List<GameObject> children = new List<GameObject>();
    private UIGrid grid;

    [SerializeField]
    private Transform anchorObject;
    [SerializeField]
    private int leftAnchorOffset, rightAnchorOffset;

    // Use this for initialization
    void Awake()
    {
        grid = GetComponent<UIGrid>();
    }

    void OnFighterChanged(FighterInfo fighter)
    {
        UpdateList();
    }

    void OnImageFileChanged(FileInfo imageFile)
    {
        UpdateList();
    }

    void UpdateList()
    {
        //Get rid of our old list
        foreach (GameObject child in children)
        {
            NGUITools.Destroy(child);
        }
        children.Clear(); //Empty the list for future use

        //Create all the new 
        foreach (ImageDefinition imageDef in editor.loadedFighter.sprite_info.imageDefinitions)
        {
            if (editor.currentImageFile == null || editor.currentImageFile.Name == imageDef.SpriteFileName)
            {
                instantiateButton(imageDef);
            }
        }

        //Realign the grid
        grid.Reposition();
    }

    void OnLeftDropdownChanged(string s)
    {
        //If the option is "Sprites", sets all the children to enabled. Otherwise, disables them.
        //Yes I know this doesn't need to be if/else but readability > efficiency
        if (s == "Sprites")
        {
            NGUITools.SetActiveChildren(gameObject, true);
        }
        else
        {
            NGUITools.SetActiveChildren(gameObject, false);
        }
    }

    private void instantiateButton(ImageDefinition imageDef)
    {
        GameObject go = NGUITools.AddChild(gameObject, spriteFileButtonPrefab);
        ImageDefinitionButton button = go.GetComponent<ImageDefinitionButton>();
        button.SetImageDef(imageDef);
        button.label.leftAnchor.target = anchorObject;
        button.label.leftAnchor.relative = 0;
        button.label.leftAnchor.absolute = leftAnchorOffset;
        button.label.rightAnchor.target = anchorObject;
        button.label.rightAnchor.relative = 1;
        button.label.rightAnchor.absolute = rightAnchorOffset;

        children.Add(go);
    }

    public override void RegisterListeners()
    {
        editor.FighterInfoChangedEvent += OnFighterChanged;
        editor.LeftDropdownChangedEvent += OnLeftDropdownChanged;
        editor.CurrentImageFileChangedEvent += OnImageFileChanged;
    }

    public override void UnregisterListeners()
    {
        editor.FighterInfoChangedEvent -= OnFighterChanged;
        editor.LeftDropdownChangedEvent -= OnLeftDropdownChanged;
        editor.CurrentImageFileChangedEvent -= OnImageFileChanged;
    }
}
