using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoableSelectSubactionCallback : UndoableCallback
{
    [SerializeField] private SubactionButton subactionSource;

    [SerializeField] private Stack<SubactionData> undoList = new Stack<SubactionData>();
    [SerializeField] private Stack<SubactionData> redoList = new Stack<SubactionData>();

    public void Execute()
    {
        redoList.Clear(); //Doing a new action blows up the redo stack
        
        if (subactionSource.subaction == LESelectedSubaction.instance.CurrentSubaction)
        {
            undoList.Push(LESelectedSubaction.instance.CurrentSubaction);
            LESelectedSubaction.instance.SelectSubaction(null);
        } else
        {
            undoList.Push(LESelectedSubaction.instance.CurrentSubaction); //Push the old value to the top of our undo stack
            LESelectedSubaction.instance.SelectSubaction(subactionSource.subaction);
        }

        LegacyEditorController.ExecuteCallback(this); //Register this callback with the editor
    }

    public override void Undo()
    {
        redoList.Push(subactionSource.subaction); //Push the current value on the redo stack
        SubactionData data = undoList.Pop(); //Get the top of the undo stack
        LESelectedSubaction.instance.SelectSubaction(data);
    }

    public override void Redo()
    {
        SubactionData data = redoList.Pop(); //Get the top of the redo list
        undoList.Push(LESelectedSubaction.instance.CurrentSubaction); //Push the old value to the top of our undo stack
        LESelectedSubaction.instance.SelectSubaction(data);
    }
}
