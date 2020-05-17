using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoableSelectActionCallback : UndoableCallback
{
    [SerializeField] private ActionPane actionSource;
    [SerializeField] private MonoActionFile actionDestination;

    [SerializeField] private Stack<DynamicAction> undoList = new Stack<DynamicAction>();
    [SerializeField] private Stack<DynamicAction> redoList = new Stack<DynamicAction>();

    public void Execute()
    {
        redoList.Clear(); //Doing a new action blows up the redo stack
        if (actionSource.action == actionDestination.currentAction)
        {
            undoList.Push(actionDestination.currentAction);
            actionDestination.currentAction = null;
        } else
        {
            undoList.Push(actionDestination.currentAction); //Push the old value to the top of our undo stack
            actionDestination.currentAction = actionSource.action;
        }

        LegacyEditorController.ExecuteCallback(this); //Register this callback with the editor
    }

    public override void Undo()
    {
        redoList.Push(actionSource.action); //Push the current value on the redo stack
        DynamicAction data = undoList.Pop(); //Get the top of the undo stack
        actionDestination.currentAction = data;
    }

    public override void Redo()
    {
        DynamicAction data = redoList.Pop(); //Get the top of the redo list
        undoList.Push(actionDestination.currentAction); //Push the old value to the top of our undo stack
        actionDestination.currentAction = data;
    }
}
