using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoableSelectActionCallback : UndoableCallback
{
    [SerializeField] private ActionPane actionSource;

    [SerializeField] private Stack<DynamicAction> undoList = new Stack<DynamicAction>();
    [SerializeField] private Stack<DynamicAction> redoList = new Stack<DynamicAction>();

    public void Execute()
    {
        redoList.Clear(); //Doing a new action blows up the redo stack
        
        if (actionSource.action == LESelectedAction.instance.CurrentAction)
        {
            undoList.Push(LESelectedAction.instance.CurrentAction);
            LESelectedAction.instance.SelectAction(null);
        } else
        {
            undoList.Push(LESelectedAction.instance.CurrentAction); //Push the old value to the top of our undo stack
            LESelectedAction.instance.SelectAction(actionSource.action);
        }

        LegacyEditorController.ExecuteCallback(this); //Register this callback with the editor
    }

    public override void Undo()
    {
        redoList.Push(actionSource.action); //Push the current value on the redo stack
        DynamicAction data = undoList.Pop(); //Get the top of the undo stack
        LESelectedAction.instance.SelectAction(data);
    }

    public override void Redo()
    {
        DynamicAction data = redoList.Pop(); //Get the top of the redo list
        undoList.Push(LESelectedAction.instance.CurrentAction); //Push the old value to the top of our undo stack
        LESelectedAction.instance.SelectAction(data);
    }
}
