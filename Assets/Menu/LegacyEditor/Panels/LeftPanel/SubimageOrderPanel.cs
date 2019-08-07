using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubimageOrderPanel : MonoBehaviour
{
    public UILabel label;
    public string subimageName;
    public UISprite sprite;
    public int index;

    //The rig is kept to notify on drag to re-order the subimages
    public SubimageOrderPanelRig rig;

    // Use this for initialization
    void Awake()
    {
        label = GetComponentInChildren<UILabel>();
        sprite = GetComponent<UISprite>();
    }

    public void setIndex(int index)
    {
        int oldIndex = this.index;
        this.index = index;
    }

    public void SetSubimage(string sub)
    {
        subimageName = sub;
        label.text = sub;
    }
    
    public void OnDragDropStart()
    {
        Debug.Log("Dragging");
        sprite.depth = 3;
        sprite.alpha = 0.5f;
        label.depth = 4;
        label.alpha = 0.5f;
    }

    public void OnDragDropRelease()
    {
        Debug.Log("Done Dragging");
        sprite.depth = 0;
        sprite.alpha = 1f;
        label.depth = 1;
        label.alpha = 1f;

        rig.UpdateOrder();
    }

    private void OnEnable() { }
    private void OnDisable() { }
}
