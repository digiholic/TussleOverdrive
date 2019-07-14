using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubimageOrderPanelRig : LegacyEditorWidget
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

    void OnAnimationInfoChanged(AnimationDefinition anim)
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

        //Create all the new buttons
        for (int i = 0; i < editor.currentAnimation.subimages.Count; i++){
            string imageDef = editor.currentAnimation.subimages[i];
            instantiateButton(imageDef,i);
        }
        
        //Realign the grid
        grid.Reposition();
    }

    private void instantiateButton(string imageDef, int index)
    {
        GameObject go = NGUITools.AddChild(gameObject, spriteFileButtonPrefab);
        SubimageOrderPanel button = go.GetComponent<SubimageOrderPanel>();
        button.SetSubimage(imageDef);
        button.setIndex(index);

        button.sprite.leftAnchor.target = anchorObject;
        button.sprite.leftAnchor.relative = 0;
        button.sprite.leftAnchor.absolute = leftAnchorOffset;
        button.sprite.rightAnchor.target = anchorObject;
        button.sprite.rightAnchor.relative = 1;
        button.sprite.rightAnchor.absolute = rightAnchorOffset;

        children.Add(go);
    }

    public override void RegisterListeners()
    {
        editor.CurrentAnimationChangedEvent += OnAnimationInfoChanged;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentAnimationChangedEvent -= OnAnimationInfoChanged;
    }
}
