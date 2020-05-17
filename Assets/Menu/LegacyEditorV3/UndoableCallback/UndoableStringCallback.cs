using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoableStringCallback : UndoableCallback
{
    public StringCallback getValue;
    public StringEvent setValue;

    [SerializeField] private Stack<string> undoList = new Stack<string>();
    [SerializeField] private Stack<string> redoList = new Stack<string>();

    public void Execute(string data)
    {
        Debug.Log("Executing", this);
        redoList.Clear(); //Doing a new action blows up the redo stack
        undoList.Push(getValue.Invoke()); //Push the old value to the top of our undo stack
        setValue?.Invoke(data); //Set the value

        LegacyEditorController.ExecuteCallback(this); //Register this callback with the editor
    }

    public override void Undo()
    {
        redoList.Push(getValue.Invoke()); //Push the current value on the redo stack
        string data = undoList.Pop(); //Get the top of the undo stack
        setValue?.Invoke(data); //Set the value
    }

    public override void Redo()
    {
        string data = redoList.Pop(); //Get the top of the redo list
        undoList.Push(getValue.Invoke()); //Push the old value to the top of our undo stack
        setValue?.Invoke(data); //Set the value
    }
}