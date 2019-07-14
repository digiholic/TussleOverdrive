using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubimageOrderPanel : MonoBehaviour
{
    public UILabel label;
    public string subimageName;
    public UISprite sprite;
    public int index;

    [SerializeField] private OnClickSendAction upButton;
    [SerializeField] private OnClickSendAction downButton;
    [SerializeField] private OnClickSendAction deleteButton;

    // Use this for initialization
    void Awake()
    {
        label = GetComponentInChildren<UILabel>();
        sprite = GetComponent<UISprite>();
    }

    public void setIndex(int index)
    {
        this.index = index;

        ChangeSubimageOrderAction upAction = ScriptableObject.CreateInstance<ChangeSubimageOrderAction>();
        upAction.init(index, -1);
        upButton.SetAction(upAction);

        ChangeSubimageOrderAction downAction = ScriptableObject.CreateInstance<ChangeSubimageOrderAction>();
        downAction.init(index, 1);
        downButton.SetAction(downAction);

        DeleteSubimageAction deleteAction = ScriptableObject.CreateInstance<DeleteSubimageAction>();
        deleteAction.init(index);
        deleteButton.SetAction(deleteAction);
    }

    public void SetSubimage(string sub)
    {
        subimageName = sub;
        label.text = sub;
    }
    

    private void OnEnable() { }
    private void OnDisable() { }
}
