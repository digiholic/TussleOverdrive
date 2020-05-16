using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoableBoolCallback : UndoableCallback
{
    public BoolCallback getValue;
    public BoolEvent setValue;

    [SerializeField] private Stack<bool> undoList = new Stack<bool>();
    [SerializeField] private Stack<bool> redoList = new Stack<bool>();

    public void Execute(bool data)
    {
        redoList.Clear(); //Doing a new action blows up the redo stack
        undoList.Push(getValue.Invoke()); //Push the old value to the top of our undo stack
        setValue?.Invoke(data); //Set the value

        LegacyEditorController.ExecuteCallback(this); //Register this callback with the editor
    }

    public override void Undo()
    {
        redoList.Push(getValue.Invoke()); //Push the current value on the redo stack
        bool data = undoList.Pop(); //Get the top of the undo stack
        setValue?.Invoke(data); //Set the value
    }

    public override void Redo()
    {
        bool data = redoList.Pop(); //Get the top of the redo list
        undoList.Push(getValue.Invoke()); //Push the old value to the top of our undo stack
        setValue?.Invoke(data); //Set the value
    }
}
